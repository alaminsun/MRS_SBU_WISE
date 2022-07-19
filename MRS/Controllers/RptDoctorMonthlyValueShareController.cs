using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class RptDoctorMonthlyValueShareController : Controller
    {
        private ValidationMsg _vmMsg;
        DBHelper dbHelper = new DBHelper();
        private DoctorGroupMstDtlDAO _doctorGroupMstDtlDao;
        ReportDAO Dalobject = new ReportDAO();
        List<DoctorGroupDtlModel> ExcelDoctorgroupDetailList = new List<DoctorGroupDtlModel>();
        public RptDoctorMonthlyValueShareController()
        {
            _vmMsg = new ValidationMsg();
            _doctorGroupMstDtlDao = new DoctorGroupMstDtlDAO();
        }
        public ActionResult frmRptDoctorMonthlyValueShare()
        {
            //if (Session["UserId"] != null)
            //{
            ViewBag.formTitle = "Doctor Monthly Value Share";
            return View();
            //}
            //return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult frmRptDoctorMonthlyValueShare(ReportModel model)
        {
            model.doctorList = (List<DoctorGroupDtlModel>)Session["doctorList"];
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.DoctorMonthlyValueShare(model);
            string ReportPath = Server.MapPath("~/Reports");

            switch (model.ReportName)
            {
                case "SELECTED DOCTOR MONTHLY VALUE SHARE(By %)":
                    ReportPath = ReportPath + "/SELECTED DOCTOR MONTHLY VALUE SHARE (by perc).rpt";
                    break;
                default:
                    ReportPath = ReportPath + "/SELECTED DOCTOR MONTHLY VALUE SHARE (by Val).rpt";
                    break;
            }
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("Year", model.ddlYear);
            reportDocument.SetParameterValue("Manufacturer", model.MANUFACTURER_NAME);

            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDoctorData()
        {
            var data = _doctorGroupMstDtlDao.GetDoctorGroupDtlList();
            var doctorDetailList = Json(data, JsonRequestBehavior.AllowGet);
            doctorDetailList.MaxJsonLength = int.MaxValue;
            return doctorDetailList;
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
            //List<DoctorGroupDtlModel> ExcelDoctorgroupDetailList = new List<DoctorGroupDtlModel>();
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

                // Save//

                //Check No 1
                if (dataSet.Tables[0].Columns.Count == 1)
                {
                    Int64 i = 0;
                    dbHelper.CmdExecute("DELETE FROM GROUP_DOC_ID_TEMP");
                    //Check No 2
                    foreach (DataRow dr in myDataTable.Rows)
                    {
                        string qry = "INSERT INTO GROUP_DOC_ID_TEMP(DOCTOR_ID)" +
                            "VALUES('" + dr["DOCTOR_ID"].ToString() + "')";
                        dbHelper.CmdExecute(qry);
                        i++;
                    }
                    _vmMsg.Type = Enums.MessageType.Success;//dataSet.Tables[0].Rows.Count.ToString()--Count Number of Rows
                    _vmMsg.Msg = i.ToString() + " Data Successfully Exported from Excel to Temp Database File, Please Click the Check Button.";
                }
                else
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Excel File Columns are Mismatch";
                }

                //

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
                Session["doctorList"] = ExcelDoctorgroupDetailList;
                // Take All Common Doctor From Excel and Database
                var unUsedDoctorList = ExcelDoctorgroupDetailList.Select(a => a.DOCTOR_ID).Intersect(DocList.Select(b => b.DOCTOR_ID));

                foreach (var objExcelDoctorList in unUsedDoctorList)
                {

                    // RETRIVE DOCTOR NAME,DESIGNATION,SPECIALIZATION,DEGREE AGAINST DOCTOR ID.
                    string str = "SELECT DC.DOCTOR_ID,DC.DOCTOR_NAME,DC.DEGREE_CODE,DC.DESIGNATION_CODE,DC.SPECIA_1ST_CODE,DD.DEGREE,DCD.DESIGNATION,DS.SPECIALIZATION  " +
                               "FROM Doctor DC LEFT JOIN DOCTOR_DEGREE DD ON DC.DEGREE_CODE = DD.DEGREE_CODE  " +
                               "LEFT JOIN DOCTOR_DESIGNATION DCD ON DC.DESIGNATION_CODE = DCD.DESIGNATION_CODE  " +
                               "LEFT JOIN DOCTOR_SPECIALIZATION DS ON DC.SPECIA_1ST_CODE = DS.SPECIALIZATION_CODE " +
                               "where DC.DOCTOR_ID ='" + objExcelDoctorList + "'";

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
            }
            var result = new { msg = _vmMsg, DoctorGroupDtlLoadList };
            return Json(DoctorGroupDtlLoadList, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}