using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MRS.DAL.Gateway;
using System.ComponentModel;


namespace MRS.Controllers
{
    public class DoctorGroupMstDtlController : Controller
    {
        private DoctorGroupMstDtlDAO _doctorGroupMstDtlDao;
        private ValidationMsg _vmMsg;
        DBHelper dbHelper = new DBHelper();

        public DoctorGroupMstDtlController()
        {
            _vmMsg = new ValidationMsg();
            _doctorGroupMstDtlDao = new DoctorGroupMstDtlDAO();
        }
        public ActionResult FrmDoctorGroupMstDtl()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Doctor Group Detail Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetGroupData()
        {
            var tcL1L2List = _doctorGroupMstDtlDao.GetDoctorGroupList();
            return Json(tcL1L2List, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDoctorData()
        {
            var data = _doctorGroupMstDtlDao.GetDoctorGroupDtlList();
            var doctorDetailList = Json(data, JsonRequestBehavior.AllowGet);
            doctorDetailList.MaxJsonLength = int.MaxValue;
            return doctorDetailList;
        }

        [HttpPost]
        public ActionResult Save(DoctorGroupMstModel model)
        {
            //_userId = Convert.ToInt32(Session["UserID"]);

            _vmMsg = _doctorGroupMstDtlDao.Save(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult Update(DoctorGroupMstModel model)
        {
            //_userId = Convert.ToInt32(Session["UserID"]);
            _vmMsg = _doctorGroupMstDtlDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult SaveAndUpdate(DoctorGroupMstModel model)
        //{
        //    _vmMsg = model.SUStatus == 0 ? _doctorGroupMstDtlDao.Save(model, Session["mappingUserID"].ToString()) : _doctorGroupMstDtlDao.Update(model, Session["mappingUserID"].ToString());
        //    return Json(new { msg = _vmMsg });
        //}


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSearchMasterList()
        {
            var tcL1L2SearchList = _doctorGroupMstDtlDao.GetSearchMasterList();
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSearchDetailDoctorList(string doctorGroupMstSlNo)
        {
            var data = _doctorGroupMstDtlDao.GetSearchDtliList(doctorGroupMstSlNo);
            var doctorDetailSearchList = Json(data, JsonRequestBehavior.AllowGet);
            doctorDetailSearchList.MaxJsonLength = int.MaxValue;
            return doctorDetailSearchList;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAllList(string doctorGroupMstSlNo)
        {
            _vmMsg = _doctorGroupMstDtlDao.Delete(doctorGroupMstSlNo);
            return Json(new { msg = _vmMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDetailList(string doctorGroupDtlSlNo)
        {
            _vmMsg = _doctorGroupMstDtlDao.DeleteDetailData(doctorGroupDtlSlNo);
            return Json(new { msg = _vmMsg });
        }


        #region Upload Excel File And Insert Into Database

        public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.FileName = "";
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content"), fileName);
                    file.SaveAs(physicalPath);
                    TempData["file"] = fileName;
                    break;
                }
            }
            // Return an empty string to signify success
            return Content("");
        }


        public ActionResult LoadExcelFile(string fileName)
        {
            //Doctor Group Detail Grid List
            List<DoctorGroupDtlModel> DoctorGroupDtlLoadList = new List<DoctorGroupDtlModel>();

            //Excel Doctor Id List
            List<DoctorGroupDtlModel> ExcelDoctorgroupDetailList = new List<DoctorGroupDtlModel>();
            try
            {
                if (string.IsNullOrEmpty(TempData["file"].ToString()))
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

                string connectionString = "";
                string filename = Path.Combine(Server.MapPath("~/Content"), TempData["file"].ToString());
                string[] d = filename.Split('.');
                string fileExtension = "." + d[d.Length - 1].ToString();
                if (d.Length > 0)
                {
                    if (fileExtension == ".xls")
                    {
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    }
                }

                string query = String.Format("SELECT * from [{0}$]", "Sheet 1");
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connectionString);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                System.Data.DataTable myDataTable = dataSet.Tables[0];



                // Load Database DOCTOR ID into List
                System.Data.DataTable dtDoc = dbHelper.GetDataTable("SELECT DOCTOR_ID FROM DOCTOR");
                List<DoctorGroupDtlModel> DocList;
                DocList = (from DataRow row in dtDoc.Rows
                           select new DoctorGroupDtlModel
                           {
                               DOCTOR_ID = row["DOCTOR_ID"].ToString()
                           }).ToList();

                // Load Excel DOCTOR ID into List
                ExcelDoctorgroupDetailList = (from DataRow row in myDataTable.Rows
                                              select new DoctorGroupDtlModel()
                                              {
                                                  DOCTOR_ID = row["DOCTOR_ID"].ToString()
                                              }).ToList();

                // Take All Common Doctor From Excel and Database
                var unUsedDoctorList = ExcelDoctorgroupDetailList.Select(a => a.DOCTOR_ID).Intersect(DocList.Select(b => b.DOCTOR_ID));

                foreach (var objExcelDoctorList in unUsedDoctorList)
                {

                    // RETRIVE DOCTOR NAME,DESIGNATION,SPECIALIZATION,DEGREE AGAINST DOCTOR ID.
                    //string str = "SELECT DC.DOCTOR_ID,DC.DOCTOR_NAME,DC.DEGREE_CODE,DC.DESIGNATION_CODE,DC.SPECIA_1ST_CODE,DD.DEGREE,DCD.DESIGNATION,DS.SPECIALIZATION  " +
                    //           "FROM Doctor DC LEFT JOIN DOCTOR_DEGREE DD ON DC.DEGREE_CODE = DD.DEGREE_CODE  " +
                    //           "LEFT JOIN DOCTOR_DESIGNATION DCD ON DC.DESIGNATION_CODE = DCD.DESIGNATION_CODE  " +
                    //           "LEFT JOIN DOCTOR_SPECIALIZATION DS ON DC.SPECIA_1ST_CODE = DS.SPECIALIZATION_CODE " +
                    //           "where DC.DOCTOR_ID ='" + objExcelDoctorList + "'";
                    string str = "SELECT DC.DOCTOR_ID,DC.DOCTOR_NAME,DC.DEGREE,DC.DESIGNATION,DS.SPECIALIZATION " +
                                "FROM Doctor DC,DOCTOR_SPECIALIZATION DS where DC.SPECIA_1ST_CODE = DS.SPECIALIZATION_CODE(+)" +
                                " and DC.DOCTOR_ID ='" + objExcelDoctorList + "'";

                    System.Data.DataTable UserWorkinngSessionDt = dbHelper.GetDataTable(str);
                    var gridDisplayList = (from DataRow row in UserWorkinngSessionDt.Rows
                                           select new DoctorGroupDtlModel
                                           {
                                               DOCTOR_ID = row["DOCTOR_ID"].ToString(),
                                               DEGREE = row["DEGREE"].ToString(),
                                               DOCTOR_NAME = row["DOCTOR_NAME"].ToString(),
                                               DESIGNATION = row["DESIGNATION"].ToString(),
                                               SPECIALIZATION = row["SPECIALIZATION"].ToString()
                                           }).FirstOrDefault();

                    DoctorGroupDtlLoadList.Add(gridDisplayList);
                }

                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Excel File Uploaded Successfully";

            }
            catch (Exception ex)
            {
                if (fileName == "")
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Excel file not found.";
                }
                else
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Fialed To Upload Excel File.";
                }

                //_vmMsg.Type = Enums.MessageType.Error;
                //_vmMsg.Msg = "Excel File Data";
            }
            //return Json(new { msg = _vmMsg },DoctorGroupDtlLoadList, JsonRequestBehavior.AllowGet);
            var result = new { msg = _vmMsg, DoctorGroupDtlLoadList };
            return Json(DoctorGroupDtlLoadList, JsonRequestBehavior.AllowGet);
        }


        //public ActionResult Remove(string fileNames)
        //{
        //    // The parameter of the Remove action must be called "fileNames"

        //    if (!string.IsNullOrEmpty(fileNames))
        //    {
        //        var fileName = Path.GetFileName(fileNames);
        //        var physicalPath = Path.Combine(Server.MapPath("~/Content"), fileName);
        //        // TODO: Verify user permissions
        //        if (System.IO.File.Exists(physicalPath))
        //        {
        //            // The files are not actually removed in this demo
        //            System.IO.File.Delete(physicalPath);
        //        }
        //    }

        //    // Return an empty string to signify success
        //    return Content("");
        //}

        #endregion
    }
}