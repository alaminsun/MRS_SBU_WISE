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
    public class RptInstitutionMonthlyValueShareController : Controller
    {
        private InstitutionGroupMstDtlDAO _institutionGroupMstDtlDao;
        private ValidationMsg _vmMsg;
        DBHelper dbHelper = new DBHelper();
        ReportDAO Dalobject = new ReportDAO();
        List<InstitutionGroupDtlModel> ExcelInstitutiongroupDetailList = new List<InstitutionGroupDtlModel>();
        public RptInstitutionMonthlyValueShareController()
        {
            _vmMsg = new ValidationMsg();
            _institutionGroupMstDtlDao = new InstitutionGroupMstDtlDAO();
        }

        public ActionResult frmRptInstitutionMonthlyValueShare()
        {
            //if (Session["UserId"] != null)
            //{
            ViewBag.formTitle = "Institution Monthly Value Share";
            return View();
            //}
            //return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult frmRptInstitutionMonthlyValueShare(ReportModel model)
        {
            model.instiList = (List<InstitutionGroupDtlModel>)Session["instiList"]; ;
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.InstitutionMonthlyValueShare(model);
            string ReportPath = Server.MapPath("~/Reports");

            switch (model.ReportName)
            {
                case "SELECTED INSTITUTIONS MONTHLY VALUE SHARE(By %)":
                    ReportPath = ReportPath + "/SELECTED INSTITUTIONS MONTHLY VALUE SHARE (by perc).rpt";
                    break;
                default:
                    ReportPath = ReportPath + "/SELECTED INSTITUTIONS MONTHLY VALUE SHARE (by Val).rpt";
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
        public ActionResult GetInstitutionData()
        {
            var data = _institutionGroupMstDtlDao.GetInstitutionList();
            var doctorDetailList = Json(data, JsonRequestBehavior.AllowGet);
            doctorDetailList.MaxJsonLength = int.MaxValue;
            return doctorDetailList;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDetailList(string institutionGroupDtlSlNo)
        {
            _vmMsg = _institutionGroupMstDtlDao.DeleteDetailData(institutionGroupDtlSlNo);
            return Json(new { msg = _vmMsg });
        }

        #region Excel Upload in Institution Grid Detail

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
            List<InstitutionGroupDtlModel> InstitutionGroupDtlLoadList = new List<InstitutionGroupDtlModel>();

            //Excel Doctor Id List
            //List<InstitutionGroupDtlModel> ExcelInstitutiongroupDetailList = new List<InstitutionGroupDtlModel>();
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
                System.Data.DataTable dtDoc = dbHelper.GetDataTable("SELECT INSTI_CODE FROM INSTITUTION");
                List<InstitutionGroupDtlModel> DocList;
                DocList = (from DataRow row in dtDoc.Rows
                           select new InstitutionGroupDtlModel
                           {
                               INSTI_CODE = row["INSTI_CODE"].ToString()
                           }).ToList();

                // Load Excel DOCTOR ID into List
                ExcelInstitutiongroupDetailList = (from DataRow row in myDataTable.Rows
                                                   select new InstitutionGroupDtlModel()
                                                   {
                                                       INSTI_CODE = row["INSTITUTION_CODE"].ToString()
                                                   }).ToList();
                Session["instiList"] = ExcelInstitutiongroupDetailList;
                // Take All Common Doctor From Excel and Database
                var unUsedDoctorList = ExcelInstitutiongroupDetailList.Select(a => a.INSTI_CODE).Intersect(DocList.Select(b => b.INSTI_CODE));

                foreach (var objExcelDoctorList in unUsedDoctorList)
                {

                    // RETRIVE DOCTOR NAME,DESIGNATION,SPECIALIZATION,DEGREE AGAINST DOCTOR ID.
                    string str = "SELECT " +
                                 " I.INSTI_CODE," +
                                 " I.INSTI_NAME," +
                                 " I.INSTI_TYPE_CODE," +
                                 " IT.INSTI_TYPE_NAME," +
                                 " (  I.ADDRESS1||' '||I.ADDRESS2||' '||I.ADDRESS3||' '||I.ADDRESS4 ) AS ADDRESS  " +
                                 " FROM    INSTITUTION I " +
                                 " LEFT JOIN INSTITUTION_TYPE IT  ON I.INSTI_TYPE_CODE = IT.INSTI_TYPE_CODE" +
                                 " WHERE I.INSTI_CODE = '" + objExcelDoctorList + "'";

                    System.Data.DataTable UserWorkinngSessionDt = dbHelper.GetDataTable(str);
                    var gridDisplayList = (from DataRow row in UserWorkinngSessionDt.Rows
                                           select new InstitutionGroupDtlModel
                                           {
                                               INSTI_CODE = row["INSTI_CODE"].ToString(),
                                               INSTI_NAME = row["INSTI_NAME"].ToString(),
                                               INSTI_TYPE_CODE = row["INSTI_TYPE_CODE"].ToString(),
                                               INSTI_TYPE_NAME = row["INSTI_TYPE_NAME"].ToString(),
                                               ADDRESS = row["ADDRESS"].ToString()
                                           }).FirstOrDefault();

                    InstitutionGroupDtlLoadList.Add(gridDisplayList);
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
            return Json(InstitutionGroupDtlLoadList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(string fileNames)
        {
            if (!string.IsNullOrEmpty(fileNames))
            {
                var fileName = Path.GetFileName(fileNames);
                var physicalPath = Path.Combine(Server.MapPath("~/Content"), fileName);
                // TODO: Verify user permissions
                if (System.IO.File.Exists(physicalPath))
                {
                    // The files are not actually removed in this demo
                    System.IO.File.Delete(physicalPath);
                }
            }
            return Content("");
        }

        #endregion
    }
}