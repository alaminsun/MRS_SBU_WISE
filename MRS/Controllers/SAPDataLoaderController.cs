using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SAPDataLoaderController : Controller
    {
        SAPDataLoaderDAO Dalobject = new SAPDataLoaderDAO();
        private DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        public SAPDataLoaderController()
        {
            _vmMsg = new ValidationMsg();
        }
        public ActionResult frmSAPDataLoaderform()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "SAP Data Loader Form";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmSAPDataLoaderform(ReportModel model)
        {
            DataTable dt = new DataTable();
            string session_No = "POUploaded" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second;
            dt = Dalobject.SAPDataUploadedList(model, session_No);
            ReportDocument reportDocument = new ReportDocument();
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptSAPDataLoaderPOUser.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("POUser", model.USER_ID);
            reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, true, session_No);

            reportDocument.Close();
            reportDocument.Dispose();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetPOUserList()
        {
            var POUserList = Dalobject.GetPOUserList();
            return Json(POUserList, JsonRequestBehavior.AllowGet);
        }

        //Amir Hamza
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save()
        {
            _vmMsg = Dalobject.Save();
            return Json(new { msg = _vmMsg });
        }
    }
}
