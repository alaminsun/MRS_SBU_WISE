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
    public class ProductCollectionRptController : Controller
    {

        //MarketWiseInstRptDAO reportObj = new MarketWiseInstRptDAO();
        ProductCollectionStsDAO reportObj = new ProductCollectionStsDAO();
        ReportModel model = new ReportModel();
        DataTable dataTableObj = new DataTable();
        //
        // GET: /ProductCollectionRpt/
        public ActionResult frmRptProductCollectionStatus()
        {
            ViewBag.formTitle = "Product Collection Status Report";
            return View();
        }


        //Get Report Data
        [HttpPost]
        public ActionResult frmProductCollectionReport(ReportModel model)
        {
            //DataTable dt;
            ReportDocument rd = new ReportDocument();
            string ReportPath = Server.MapPath("~/Reports");
            switch (model.ReportName)
            {
                case "ProductCollection":
                    dataTableObj = reportObj.GetProductCollectionReport(model);
                    ReportPath = ReportPath + "/RptProductCollectionStatus.rpt";
                    break;
                default:
                    dataTableObj = reportObj.GetPreOrgSlpOtcColectionReport(model);
                    ReportPath = ReportPath + "/RptPreOrgSlpOtcCllSts.rpt";
                    break;
            }
            rd.Load(ReportPath);
            rd.SetDataSource(dataTableObj);
            rd.Refresh();
            //reportDocument.Load(rptPath);
            //reportDocument.SetDataSource(dataTableObj);
            //reportDocument.Refresh();
            rd.SetParameterValue("FromDate", model.FromDate);
            rd.SetParameterValue("ToDate", model.ToDate);
            rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            rd.SetParameterValue("DEVBY", Session["DEVBY"]);
            rd.SetParameterValue("ProjectName", Session["ProjectName"]);

            rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
	}
}