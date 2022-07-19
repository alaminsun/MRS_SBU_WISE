using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class MarketWiseInstRptController : Controller
    {

        MarketWiseInstRptDAO reportObj = new MarketWiseInstRptDAO();
        ReportModel model = new ReportModel();
        DataTable dataTableObj = new DataTable();
        //
        // GET: /MarketWiseInstRpt/
        public ActionResult frmMarketWiseInstiListRpt()
        {
            ViewBag.formTitle = "Market Wise Institution Report";
            return View();
        }

        [HttpGet]
        public JsonResult GetRegion()
        {
            List<ReportModel> allRegion = reportObj.GetAllRegionInfo();
            return Json(allRegion, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllInstitutionType()
        {
            List<ReportModel> allType = reportObj.GetAllInstitutionType();
            return Json(allType, JsonRequestBehavior.AllowGet);
        }


        //Get Report Data
        [HttpPost]
        public ActionResult frmMarketWiseReport(ReportModel model)
        {
            //DataTable dt;
            ReportDocument rd = new ReportDocument();
            string ReportPath = Server.MapPath("~/Reports");
            dataTableObj = reportObj.GetMarketWiseInstitute(model);
            ReportPath = ReportPath + "/RptMarketWiseInstitutionList.rpt";

            rd.Load(ReportPath);
            rd.SetDataSource(dataTableObj);
            rd.Refresh();
            rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            rd.SetParameterValue("DEVBY", Session["DEVBY"]);
            rd.SetParameterValue("ProjectName", Session["ProjectName"]);



            //TextObject vCompanyName;
            //vCompanyName = (TextObject)rd.ReportDefinition.ReportObjects["txtComName"];
            //vCompanyName.Text = reportDataHeader.ComName;

            //TextObject vAddress;
            //vAddress = (TextObject)rd.ReportDefinition.ReportObjects["txtComAddress"];
            //vAddress.Text = reportDataHeader.ComAddress;

            //TextObject vDevBy;
            //vDevBy = (TextObject)rd.ReportDefinition.ReportObjects["txtDevBy"];
            //vDevBy.Text = reportDataHeader.DevCompany;

            //TextObject vFromDate;
            //vFromDate = (TextObject)rd.ReportDefinition.ReportObjects["EntryDate"];
            //if (model.DateFrom != null)
            //{
            //    vFromDate.Text = model.DateFrom;
            //}

            //TextObject vToDate;
            //vToDate = (TextObject)rd.ReportDefinition.ReportObjects["EntryDate"];
            //if (model.DateTo != null)
            //{
            //    vToDate.Text = model.DateTo;
            //}

            //if (dataTableObj.Rows.Count > 0)
            //{
            //    if (model.ReportName == "MarketWiseInstiListRpt")
            //    {
            //        if (model.ReportType == "PDF")
            //            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "crReport");
            //        else
            //            rd.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            //    }
            //    else
            //    {
            //        if (model.ReportType == "PDF")
            //            rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, "Collection_Register_Report");
            //        else
            //            rd.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "Collection_Register_Report");
            //    }

            //    rd.Close();
            //    rd.Dispose();

            //    return View();
            //}
            //else
            //    rd.Close();
            //rd.Dispose();
            //return RedirectToAction("MRV", "MRV");

            rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

	}
}