using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ionic.Zip;
using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;
using MRS.Reports;
using MRS.DAL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Configuration;
using System.Data.OracleClient;


using System.Web.DataAccess;

namespace MRS.Controllers
{
    public class ReportsController : Controller
    {
        ReportDAO Dalobject = new ReportDAO();
        private DBHelper dbHelper = new DBHelper();
        ReportDAO reportObj = new ReportDAO();
        Export export = new Export();
        ReportDataHeader reportDataHeader = new ReportDataHeader();

        public ActionResult frmReports()
        {
            ViewBag.Region = Dalobject.GetRegionList();
            ViewBag.formTitle = "Reports";
            return View();
        }
        [HttpPost]
        public ActionResult frmReports(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            //var subreportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);
            //dataTableObj = reportObj.GetReportDataMakert(model);

            //var rptPath1 = Server.MapPath("~/Reports");
            //rptPath1 = rptPath1 + "/RptHeader.rpt";
            //subreportDocument.Load(rptPath1);
            ////reportDocument.SetDataSource(dataTableObj);
            //subreportDocument.Refresh();
            //subreportDocument.SetParameterValue("@CompName", "SQUARE PHARMACEUTICALS LIMITED");
            //subreportDocument.SetParameterValue("@DeptName", "MARKET REASEARCH & PLANNING CELL");
            //reportDocument.SetParameterValue("@CompName", "SQUARE PHARMACEUTICALS LIMITED", "RptHeader");
            //reportDocument.SetParameterValue("@DeptName", "MARKET REASEARCH & PLANNING CELL", "RptHeader");

            switch (model.ReportName)
            {
                case "ManufacturerInfo":
                    rptPath = rptPath + "/CrystalReport2.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    break;
                case "SelectedManufacturerInfo":
                    rptPath = rptPath + "/RptSelectedManufacturerInfo.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "DesignationInfo":
                    rptPath = rptPath + "/RptDesignationInfo.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "Divison":
                    rptPath = rptPath + "/RptDivison.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "Region":
                    rptPath = rptPath + "/RptRegion.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "Territory":
                    rptPath = rptPath + "/RptTerritory.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "Market":
                    rptPath = rptPath + "/RptMarket.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "Depo":
                    rptPath = rptPath + "/RptDepo.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "TherapeuticClassLevel1":
                    rptPath = rptPath + "/RptTherapeuticClassLevel1.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                //case "Designation Division Region Territory and Market Location":
                //    rptPath = rptPath + "/Zone Division Region Territory and Market Location.rpt";
                //    reportDocument.Load(rptPath);
                //    reportDocument.SetDataSource(dataTableObj);
                //    reportDocument.Refresh();
                //    break;
                ////case "Manufacturer Wise Product List":
                ////    rptPath = rptPath + "/Manufacturer Wise Product List.rpt";
                ////    reportDocument.Load(rptPath);
                ////    reportDocument.SetDataSource(dataTableObj);
                ////    reportDocument.Refresh();
                ////    break;
                ////case "Institutional Information":
                ////    rptPath = rptPath + "/Institutional Information.rpt";
                ////    reportDocument.Load(rptPath);
                ////    reportDocument.SetDataSource(dataTableObj);
                ////    reportDocument.Refresh();
                ////    break;
                ////case "Market Wise Doctor List":
                ////    rptPath = rptPath + "/Market Wise Doctor List.rpt";
                //    reportDocument.Load(rptPath);
                //    reportDocument.SetDataSource(dataTableObj);
                //    reportDocument.Refresh();
                //    break;
                //case "Specialization wise Doctor List":
                //    rptPath = rptPath + "/Specialization wise Doctor List.rpt";
                //    reportDocument.Load(rptPath);
                //    reportDocument.SetDataSource(dataTableObj);
                //    reportDocument.Refresh();
                //    break;
                case "PotentialCategory":
                    rptPath = rptPath + "/RptPotentialCategory.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "DegreeWiseNoOfDoctor":
                    rptPath = rptPath + "/RptDegreeWiseDoctor.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "CategoryWiseInstitute":
                    rptPath = rptPath + "/RptCategorywiseInstitute.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "InstituteInformation":
                    rptPath = rptPath + "/RptInstitution.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "DosageFormInfo":
                    rptPath = rptPath + "/RptDosageForm.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "DosageStrengthInfo":
                    rptPath = rptPath + "/RptDosageStrength.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "allGeneric":
                    rptPath = rptPath + "/RptGenericInfo.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "selectedGeneric":
                    rptPath = rptPath + "/RptGenericInfo.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "allMnufacturer":
                    rptPath = rptPath + "/RptManufacturerInfo.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
                case "selectedMnufacturer":
                    rptPath = rptPath + "/RptManufacturerInfo.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();
                    break;
            }
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, "crReport");
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptHRMarketShareValue()
        {
            ViewBag.formTitle = "HR Market Share Value";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptHRMarketShareValue(ReportModel model)
        {
                    //string day = DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
                    //string month = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
                    //string rptName = day + month + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();


                   DataTable dt = new DataTable();
                   ReportDocument reportDocument = new ReportDocument();
                   string ReportPath = Server.MapPath("~/Reports");
                   string rptName = "Market_Value_Share_" + model.FromDate + "_" + model.ToDate + "_PMS";
                   dt = Dalobject.GetHRMarketShareValueExcelData(model);
                   ReportPath = ReportPath + "/RptMarketValueShareValuePMSExcel.rpt";
                   reportDocument.Load(ReportPath);
                   reportDocument.SetDataSource(dt);
                   reportDocument.Refresh();

                   if (dt.Rows.Count > 0)
                   {
                       if (model.ReportType == "PDF")
                           reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, rptName);
                       else if (model.ReportType == "EXCEL")
                           reportDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, rptName);
                       else if (model.ReportType == "Text")
                           reportDocument.ExportToHttpResponse(ExportFormatType.Text, System.Web.HttpContext.Current.Response, false, rptName);
                       else
                           export.ExportToExcel(dt);



                       reportDocument.Close();
                       reportDocument.Dispose();
                       //return View();
                   }
            //int i = 0;
            //int j = 0;
            ////string sql = null;
            //string data = null;
            //Microsoft.Office.Interop.Excel.Application xlApp;
            //Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            //Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            //object misValue = System.Reflection.Missing.Value;
            //xlApp = new Microsoft.Office.Interop.Excel.Application();
            //xlApp.Visible = false;
            //xlWorkBook = (Microsoft.Office.Interop.Excel.Workbook)(xlApp.Workbooks.Add(Missing.Value));
            //xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.ActiveSheet;

            //string conn = ConfigurationManager.ConnectionStrings["Conn"].ConnectionString;
            //OracleConnection con = new OracleConnection(conn);
            //con.Open();
            ////string Qry = "";
            ////var cmd = new OracleCommand("SELECT * FROM REGION", con);
            //dt = Dalobject.GetHRMarketShareValueExcelData(model);
            ////var reader = dt.ExecuteReader();
            //int k = 0;
            //for (i = 0; i < dt.Columns.Count; i++)
            //{
            //    data = (dt.Columns[i].ColumnName);
            //    xlWorkSheet.Cells[1, k + 1] = data;
            //    k++;
            //}
            //char lastColumn = (char)(65 + dt.Columns.Count - 1);
            //xlWorkSheet.get_Range("A1", lastColumn + "1").Font.Bold = true;
            //xlWorkSheet.get_Range("A1", lastColumn + "1").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
            ////dt.Close();

            ////sql = "SELECT * FROM REGION";
            ////OracleDataAdapter dscmd = new OracleDataAdapter(sql, con);
            ////DataSet ds = new DataSet();
            ////dscmd.Fill(ds);
            //for (i = 0; i <= dt.Rows.Count - 1; i++)
            //{
            //    var newj = 0;
            //    for (j = 0; j <= dt.Columns.Count - 1; j++)
            //    {
            //        data = dt.Rows[i].ItemArray[j].ToString();

            //        xlWorkSheet.Cells[i + 2, newj + 1] = data;
            //        newj++;
            //    }
            //}
            //string fileName = "Market_Value_Share_PMS_" + rptName + "";

            //// Close Excel
            //xlWorkBook.Close(true, fileName, Type.Missing);
            //xlApp.Quit();

            //// Ensure you release resources
            //releaseObject(xlApp);
            //releaseObject(xlWorkBook);
            //releaseObject(xlWorkSheet);
            //        return View();

            //rptName = "HR_Market_Share_Value" + rptName;
            //export.CompanyName = reportDataHeader.ComName;
            //export.ReportName = "HR Market Share Value";
            ////export.FromDate = model.FromDate;
            ////export.ToDate = model.ToDate;
            //export.ExportToExcel(dt);
            //rptName = "MarketShareValuePMS" + rptName;




            return View();

        }

        public ActionResult frmIndicationWiseCompanyShareofaGeneric()
        {
            ViewBag.formTitle = "Indication Wise Company Share of a Generic";
            return View();
        }

        [HttpPost]
        public ActionResult frmIndicationWiseCompanyShareofaGeneric(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.IndicationWiseCompanyShareofaGeneric(model);
            string ReportPath = Server.MapPath("~/Reports");
            if(model.ReportType == "PDF")
            {
                ReportPath = ReportPath + "/RptReligionWiseNo.OfDoctor.rpt";
            }
            else
            {
                ReportPath = ReportPath + "/RptIndicationWiseCompanyShareofaGeneric.rpt";

            }
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            if (!string.IsNullOrEmpty(model.ddlPresCategory))
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                else if (model.ddlPresCategory == "0")
                    reportDocument.SetParameterValue("PrescriptionCateName", "All");
            }
            if (!string.IsNullOrEmpty(model.ddlDataSource))
            {

                if (model.ddlDataSource == "1")
                    reportDocument.SetParameterValue("DataSource", "Square");
                else if (model.ddlDataSource == "2")
                    reportDocument.SetParameterValue("DataSource", "4P");
                else if (model.ddlDataSource == "3")
                    reportDocument.SetParameterValue("DataSource", "MSP");
                else if (model.ddlDataSource == "0")
                    reportDocument.SetParameterValue("DataSource", "All Surveyor");
            }
            if (!string.IsNullOrEmpty(model.GENERIC_CODE))
            {
                if (model.GENERIC_CODE == "000")
                    reportDocument.SetParameterValue("Generic", "All Generic");
                else
                    reportDocument.SetParameterValue("Generic", model.GENERIC_NAME);
            }
            if (!string.IsNullOrEmpty(model.DOSAGE_FORM_CODE))
            {
                if (model.DOSAGE_FORM_CODE == "00")
                    reportDocument.SetParameterValue("DosageForm", "All Dosage Form");
                else
                    reportDocument.SetParameterValue("DosageForm", model.DOSAGE_FORM_NAME);
            }
            if (!string.IsNullOrEmpty(model.DSG_STRENGTH_CODE))
            {
                if (model.DSG_STRENGTH_CODE == "0")
                    reportDocument.SetParameterValue("DosageStrength", "All Dosage Strength");
                else
                    reportDocument.SetParameterValue("DosageStrength", model.DSG_STRENGTH_NAME);
            }

            string downFileName = "";
            downFileName = "Indication_Wise_Company_Share_of_a_Generic" + "_" + "From:" + model.FromDate + "_" + "To:" +
                           model.ToDate + "_" + model.GENERIC_NAME + "_" + model.DOSAGE_FORM_NAME + "";
            reportDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptLocationWiseFrequencyShare()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.DataSourceDropDownList = Dalobject.DataSourceListDropDown();
                ViewBag.formTitle = "LOCATION WISE FREQUENCY SHARE REPORT";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [HttpPost]
        public ActionResult frmRptLocationWiseFrequencyShare(ReportModel model)
        {
            ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetLocationWiseFrequencyShare(model);
             switch (model.ReportName){
                       case "MarketWiseFrequencyShare":
                        reportPath = reportPath + "/RptMarketWiseFrequencyShare.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                       case "MarketWiseFrequencyShareInNo":
                        reportPath = reportPath + "/RptMarketWiseFrequencyShareinNo.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                       case "NationalFrequencyShareinPCT":
                        reportPath = reportPath + "/RptNationalFrequencyShareinPCT.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                       case "NationalFrequencyShareinNo":
                        reportPath = reportPath + "/RptNationalFrequencyShareinNo.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                       case "ZoneWiseFrequencyShareInPct":
                        reportPath = reportPath + "/RptZoneWiseFrequencyShareInPct.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                       case "ZoneWiseFrequencyShareInNo":
                        reportPath = reportPath + "/RptZoneWiseFrequencyShareinNo.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                       case "DivisionWiseFrequencyShareInPerc":
                        reportPath = reportPath + "/RptDivisionWiseFrequencyShareInPerc.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                       case "DivisionWiseFrequencyShareInNo":
                        reportPath = reportPath + "/RptDivisionWiseFrequencyShareInNo.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                       case "RegionWiseFrequencyShareInPerc":
                        reportPath = reportPath + "/RptRegionWiseFrequencyShareInPerc.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                       case "RegionWiseFrequencyShareInNo":
                        reportPath = reportPath + "/RptRegionWiseFrequencyShareInNo.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                       case "TerritoryWiseFrequencyShareInPerc":
                        reportPath = reportPath + "/RptTerritoryWiseFrequencyShareInPerc.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                       case "TerritoryWiseFrequencyShareInNo":
                        reportPath = reportPath + "/RptTerritoryWiseFrequencyShareInNo.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                       case "DivisionRegionTerritoryMarketWiseFrequencyShareInPerc":
                        reportPath = reportPath + "/RptDivisionRegionTerritoryMarketWiseFrequencyShareInPerc.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                       case "DivisionRegionTerritoryMarketWiseFrequencyShareInNo":
                        reportPath = reportPath + "/RptDivisionRegionTerritoryMarketWiseFrequencyShareInNo.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
             }

             if (model.ReportName == "MarketWiseFrequencyShare")
            {
                 string downfilename = "Market_Wise_Frequency_Share" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else if (model.ReportName == "MarketWiseFrequencyShareInNo")
            {
                string downfilename = "Market_Wise_Frequency_Share_In_No" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
             else if (model.ReportName == "NationalFrequencyShareinPCT")
             {
                 string downfilename = "National_Frequency_Share_in_PCT" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }
             else if (model.ReportName == "NationalFrequencyShareinNo")
             {
                 string downfilename = "National_Frequency_Share_in_No" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }
             else if (model.ReportName == "ZoneWiseFrequencyShareInPct")
             {
                 string downfilename = "Zone_Wise_Frequency_Share_In_Pct" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }
             else if (model.ReportName == "ZoneWiseFrequencyShareInNo")
             {
                 string downfilename = "Zone_Wise_Frequency_Share_In_No" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }

             else if (model.ReportName == "DivisionWiseFrequencyShareInPerc")
             {
                 string downfilename = "Division_Wise_Frequency_Share_In_Perc" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }
             else if (model.ReportName == "DivisionWiseFrequencyShareInNo")
             {
                 string downfilename = "Division_Wise_Frequency_Share_In_No" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }
             else if (model.ReportName == "RegionWiseFrequencyShareInPerc")
             {
                 string downfilename = "Region_Wise_Frequency_Share_In_Perc" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }
             else if (model.ReportName == "RegionWiseFrequencyShareInNo")
             {
                 string downfilename = "Region_Wise_Frequency_Share_In_No" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }

             else if (model.ReportName == "TerritoryWiseFrequencyShareInPerc")
             {
                 string downfilename = "Territory_Wise_Frequency_Share_In_Perc" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }
             else if (model.ReportName == "TerritoryWiseFrequencyShareInNo")
             {
                 string downfilename = "Territory_Wise_Frequency_Share_In_No" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }

             else if (model.ReportName == "DivisionRegionTerritoryMarketWiseFrequencyShareInPerc")
             {
                 string downfilename = "Division_Region_Territory_Market_Wise_Frequency_Share_In_Perc" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }
             else if (model.ReportName == "DivisionRegionTerritoryMarketWiseFrequencyShareInNo")
             {
                 string downfilename = "Division_Region_Territory_Market_Wise_Frequency_Share_In_No" + "From:" + model.FromDate + "To:" + model.ToDate;
                 reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
             }
            

            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }


        public ActionResult frmRptReligionWiseNoOfDoctor()
        {
            ViewBag.formTitle = "Religion Wise No. Of Doctor";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptReligionWiseNoOfDoctor(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.ReligionWiseNoOfDoctor(model);
            string ReportPath = Server.MapPath("~/Reports");
            if(model.ReportType == "PDF")
            {
                ReportPath = ReportPath + "/RptReligionWiseNo.OfDoctor.rpt";
            }
            else
            {
                ReportPath = ReportPath + "/RptReligionWiseNo.OfDoctorExcel.rpt";

            }
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            string downFileName = "Religion_Wise_No._Of_Doctor";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptGenderWiseNoOfDoctor()
        {
            ViewBag.formTitle = "Gender Wise No. Of Doctor";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptGenderWiseNoOfDoctor(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.GenderWiseNoOfDoctor(model);
            string ReportPath = Server.MapPath("~/Reports");
            if(model.ReportType == "PDF")
            {
                ReportPath = ReportPath + "/RptGenderWiseNoOfDoctor.rpt";
            }
            else
            {
                ReportPath = ReportPath + "/RptGenderWiseNoOfDoctorExcel.rpt";
            }
            
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            string downFileName = "Gender_Wise_No._Of_Doctor";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptFrequencyRankingGenericVsSpecialization()
        {
            ViewBag.formTitle = "Frequency Ranking Generic Vs Specialization";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptFrequencyRankingGenericVsSpecialization(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.FrequencyRankingGenericVsSpecialization(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptFrequencyRankingGenericVsSpecialization.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            //reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            //reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            //reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            //reportDocument.SetParameterValue("FromDate", model.FromDate);
            //reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("Specialization ", model.SPECIALIZATION);
            string downFileName = "Frequency_Ranking_Generic_Vs_Specialization";
            reportDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptPrescriptionFrequencyDistributionGenericVsSpecialization()
        {
            ViewBag.formTitle = "Prescription Frequency Distribution Generic Vs Specialization";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptPrescriptionFrequencyDistributionGenericVsSpecialization(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.PrescriptionFrequencyDistributionGenericVsSpecialization(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptPrescriptionFrequencyDistributionGenericVsSpecialization.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            string downFileName = "Prescription_Frequency_Distribution_Generic_Vs_Specialization_" + model.FromDate + "_to_" + model.ToDate + "";
            reportDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptIndividualDoctorGenericWiseProductShareRankingofTheRegion()
        {
            ViewBag.formTitle = "Individual Doctor Generic wise Product Share Ranking of a Region ";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptIndividualDoctorGenericWiseProductShareRankingofTheRegion(ReportModel model)
        {
            if (model.REGION_CODE == "00")
            {

                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();
                        //
                        DataTable dt;
                        ReportDocument rd = new ReportDocument();
                        dt = Dalobject.IndividualDoctorGenericWiseProductShareRankingofTheRegion(model);
                        string ReportPath = Server.MapPath("~/Reports");
                        //ReportPath = ReportPath + "/Individual Doctor Generic Wise Product Share Ranking of The Region.rpt";
                        if(model.ReportType == "PDF")
                        {
                            ReportPath = ReportPath + "/Individual Doctor Generic Wise Product Share Ranking of The Region.rpt";
                        }
                        else
                        {
                            ReportPath = ReportPath + "/Individual Doctor Generic Wise Product Share Ranking of The Region Excel.rpt";
                        }
                        
                        rd.Load(ReportPath);
                        rd.SetDataSource(dt);
                        rd.Refresh();

                        rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        rd.SetParameterValue("DEVBY", Session["DEVBY"]);
                        rd.SetParameterValue("ProjectName", Session["ProjectName"]);
                        rd.SetParameterValue("FromDate", model.FromDate);
                        rd.SetParameterValue("ToDate", model.ToDate);
                        rd.SetParameterValue("Region", model.REGION_NAME);
                       
                        //
                        string path = Server.MapPath("~/Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //else
                        //{
                        //    Directory.Delete(path);
                        //}
                        //string downfilename = "Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region_" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        //var physicalPath = Path.Combine(Server.MapPath("~/Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region"), downfilename);
                        if (model.ReportType == "PDF")
                        {
                            string downfilename = "Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                            var physicalPath = Path.Combine(Server.MapPath("~/Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region"), downfilename);
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        }

                        else
                        {
                            string downfilename = "Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xlsx";
                            var physicalPath = Path.Combine(Server.MapPath("~/Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region"), downfilename);
                            rd.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                        }

                        rd.Close();
                        rd.Dispose();
                        GC.Collect();
                    }
                }
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }
            else if (model.REGION_CODE == "000")
            {
                DataTable dt;
                ReportDocument rd = new ReportDocument();
                dt = Dalobject.IndividualDoctorGenericWiseProductShareRankingofTheRegion(model);
                string ReportPath = Server.MapPath("~/Reports");
                //ReportPath = ReportPath + "/Individual Doctor Generic Wise Product Share Ranking of The Region.rpt";
                if (model.ReportType == "PDF")
                {
                    ReportPath = ReportPath + "/Individual Doctor Generic Wise Product Share Ranking of The Region.rpt";
                }
                else
                {
                    ReportPath = ReportPath + "/Individual Doctor Generic Wise Product Share Ranking of The Region Excel.rpt";
                }
                rd.Load(ReportPath);
                rd.SetDataSource(dt);
                rd.Refresh();

                rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                rd.SetParameterValue("DEVBY", Session["DEVBY"]);
                rd.SetParameterValue("ProjectName", Session["ProjectName"]);
                rd.SetParameterValue("FromDate", model.FromDate);
                rd.SetParameterValue("ToDate", model.ToDate);
                rd.SetParameterValue("Region", model.REGION_NAME);

                string downfilename = "Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                rd.Close();
                rd.Dispose();
                GC.Collect();
            }
            else
            {
                DataTable dt;
                ReportDocument rd = new ReportDocument();
                dt = Dalobject.IndividualDoctorGenericWiseProductShareRankingofTheRegion(model);
                string ReportPath = Server.MapPath("~/Reports");
                if (model.ReportType == "PDF")
                {
                    ReportPath = ReportPath + "/Individual Doctor Generic Wise Product Share Ranking of The Region.rpt";
                }
                else
                {
                    ReportPath = ReportPath + "/Individual Doctor Generic Wise Product Share Ranking of The Region Excel.rpt";
                }
                rd.Load(ReportPath);
                rd.SetDataSource(dt);
                rd.Refresh();

                rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                rd.SetParameterValue("DEVBY", Session["DEVBY"]);
                rd.SetParameterValue("ProjectName", Session["ProjectName"]);
                rd.SetParameterValue("FromDate", model.FromDate);
                rd.SetParameterValue("ToDate", model.ToDate);
                rd.SetParameterValue("Region", model.REGION_NAME);

                string downfilename = "Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Region_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                rd.Close();
                rd.Dispose();
                GC.Collect();
            }
            //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptDoctorsCoveredBy4PSurvey()
        {
            ViewBag.formTitle = "Doctor's Covered By Survey Company";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptDoctorsCoveredBy4PSurvey(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.DoctorsCoveredBy4PSurvey(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptDoctorsCoveredBy4PSurvey.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            if (model.ddlDataSource == "0")
            {
                reportDocument.SetParameterValue("ddlDataSource", "All Surveyor");
            }  
            if (model.ddlDataSource == "1")
            {
                reportDocument.SetParameterValue("ddlDataSource", "Square");
            }
            if (model.ddlDataSource == "2")
            {
                reportDocument.SetParameterValue("ddlDataSource", "4P");
            }
            if (model.ddlDataSource == "3")
            {
                reportDocument.SetParameterValue("ddlDataSource", "MSP");
            }
            string downFileName = "Doctor's_Covered_By_4P_Survey_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }
        public ActionResult frmRptFrequencyBasedProductShareRanking()
        {
            ViewBag.formTitle = "Frequency Based Prescriber List";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptFrequencyBasedProductShareRanking(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.FrequencyBasedProductShareRanking(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptFrequencyBasedProductShareRanking.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            //reportDocument.SetParameterValue("Region", model.REGION_NAME);
            string downFileName = "Frequency_Based_Product_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate + "_" + model.GENERIC_NAME;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptInstitutionWiseFrequencyBasedProductShareRanking()
        {
            ViewBag.formTitle = "Institution Wise Frequency Based Product Share Ranking";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptInstitutionWiseFrequencyBasedProductShareRanking(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.InstitutionWiseFrequencyBasedProductShareRanking(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptInstitutionWiseFrequencyBasedProductShareRanking.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("Region", model.REGION_NAME);
            string downFileName = "Institution_WIse_Frequency_Based_Product_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }


        public ActionResult frmRptSpecificGenericSelectionwithotherGenericofDoctor()
        {
            ViewBag.formTitle = "Specific Generic Selection with other Generic of Doctor";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptSpecificGenericSelectionwithotherGenericofDoctor(ReportModel model)
        {



            if (model.REGION_CODE == "ALL")
            {

                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();
                        //
                        DataTable dt;
                        ReportDocument reportDocument = new ReportDocument();
                        dt = Dalobject.SpecificGenericSelectionwithotherGenericofDoctor(model);
                        string ReportPath = Server.MapPath("~/Reports");
                        ReportPath = ReportPath + "/RptSpecificGenericSelectionwithotherGenericofDoctor.rpt";
                        reportDocument.Load(ReportPath);
                        reportDocument.SetDataSource(dt);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        reportDocument.SetParameterValue("Region", model.REGION_NAME);

                        //
                        string path = Server.MapPath("~/Specific_Generic_Selection_with_other_Generic_of_Doctor/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string downfilename = "Specific_Generic_Selection_with_other_Generic_of_Doctor_" + model.REGION_NAME + ".xlsx";
                        var physicalPath = Path.Combine(Server.MapPath("~/Specific_Generic_Selection_with_other_Generic_of_Doctor"), downfilename);
                        reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);

                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();
                    }
                }
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Specific_Generic_Selection_with_other_Generic_of_Doctor/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Specific_Generic_Selection_with_other_Generic_of_Doctor");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Specific_Generic_Selection_with_other_Generic_of_Doctor");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Specific_Generic_Selection_with_other_Generic_of_Doctor"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }
            else if (model.REGION_CODE == "NAT")
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.SpecificGenericSelectionwithotherGenericofDoctor(model);
                string ReportPath = Server.MapPath("~/Reports");
                ReportPath = ReportPath + "/RptSpecificGenericSelectionwithotherGenericofDoctor.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                reportDocument.SetParameterValue("Region", model.REGION_NAME);
                string downFileName = "Specific_Generic_Selection_with_other_Generic_of_Doctor_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }
            else
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.SpecificGenericSelectionwithotherGenericofDoctor(model);
                string ReportPath = Server.MapPath("~/Reports");
                ReportPath = ReportPath + "/RptSpecificGenericSelectionwithotherGenericofDoctor.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                reportDocument.SetParameterValue("Region", model.REGION_NAME);
                string downFileName = "Specific_Generic_Selection_with_other_Generic_of_Doctor_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }
            //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();




            return View();
        }


        public ActionResult frmRptSpecificGenericSelectionwithotherGenericofInstitution()
        {
            ViewBag.formTitle = "Specific Generic Selection with other Generic of Institution";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptSpecificGenericSelectionwithotherGenericofInstitution(ReportModel model)
        {

            if (model.REGION_CODE == "ALL")
            {

                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();
                        //
                        DataTable dt;
                        ReportDocument reportDocument = new ReportDocument();
                        dt = Dalobject.SpecificGenericSelectionwithotherGenericofInstitution(model);
                        string ReportPath = Server.MapPath("~/Reports");
                        ReportPath = ReportPath + "/RptSpecificGenericSelectionwithotherGenericofInstitution.rpt";
                        reportDocument.Load(ReportPath);
                        reportDocument.SetDataSource(dt);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        reportDocument.SetParameterValue("Region", model.REGION_NAME);

                        //
                        string path = Server.MapPath("~/Specific_Generic_Selection_with_other_Generic_of_Institution/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string downfilename = "Specific_Generic_Selection_with_other_Generic_of_Institution_" + model.REGION_NAME + ".xlsx";
                        var physicalPath = Path.Combine(Server.MapPath("~/Specific_Generic_Selection_with_other_Generic_of_Institution"), downfilename);
                        reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);

                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();
                    }
                }
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Specific_Generic_Selection_with_other_Generic_of_Institution/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Specific_Generic_Selection_with_other_Generic_of_Institution");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Specific_Generic_Selection_with_other_Generic_of_Institution");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Specific_Generic_Selection_with_other_Generic_of_Institution"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }
            else if (model.REGION_CODE == "NAT")
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.SpecificGenericSelectionwithotherGenericofInstitution(model);
                string ReportPath = Server.MapPath("~/Reports");
                ReportPath = ReportPath + "/RptSpecificGenericSelectionwithotherGenericofInstitution.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                reportDocument.SetParameterValue("Region", model.REGION_NAME);
                string downFileName = "Specific_Generic_Selection_with_other_Generic_of_Institution_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }
            else
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.SpecificGenericSelectionwithotherGenericofInstitution(model);
                string ReportPath = Server.MapPath("~/Reports");
                ReportPath = ReportPath + "/RptSpecificGenericSelectionwithotherGenericofInstitution.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                reportDocument.SetParameterValue("Region", model.REGION_NAME);
                string downFileName = "Specific_Generic_Selection_with_other_Generic_of_Institution_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }
            //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();




            return View();
        }

        public ActionResult frmRptInvestmentDatabaseSystem()
        {
            ViewBag.formTitle = "Investment Database System";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptInvestmentDatabaseSystem(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.InvestmentDatabaseSystem(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/InvestmentDatabaseSystem.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            string downfilename = "InvestmentDatabaseSystem_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptNationalIdenticalProductValueShare()
        {
            ViewBag.formTitle = "National Identical Product Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptNationalIdenticalProductValueShare(ReportModel model)
        {
            //List<string> ComName = new List<string>();
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.NationalIdenticalProductValueShare(model.FromDate, model.ToDate); ;
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/NATIONAL IDENTICAL VALUE SHARE.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            //ComName = Dalobject.GetColumnName();
            for (int i = 0; i < Dalobject.GetColumnName().Count; i++)
            {
                string sdf = Dalobject.GetColumnName()[i].ToString();
                string dfg = "Col" + (i + 1) + "";
                reportDocument.SetParameterValue("Col" + (i + 1) + "", Dalobject.GetColumnName()[i].ToString());
            }
            string downfilename = "National_Identical_Product_Value_Share_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptRegionIdenticalProductValueShare()
        {
            ViewBag.formTitle = "Region Wise Identical Product Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptRegionIdenticalProductValueShare(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.RegionIdenticalProductValueShare(model.FromDate, model.ToDate);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/REGION WISE IDENTICAL VALUE SHARE.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("region", "Area");
            for (int i = 0; i < Dalobject.GetColumnName().Count; i++)
            {
                string sdf = Dalobject.GetColumnName()[i].ToString();
                string dfg = "Col" + (i + 1) + "";
                reportDocument.SetParameterValue("Col" + (i + 1) + "", Dalobject.GetColumnName()[i].ToString());
            }
            string downfilename = "Region_Wise_Identical_Product_Value_Share_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptTerritoryIdenticalProductValueShare()
        {
            ViewBag.formTitle = "Territory Wise Identical Product Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptTerritoryIdenticalProductValueShare(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.TerritoryIdenticalProductValueShare(model.FromDate, model.ToDate);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/TERRITORY WISE IDENTICAL VALUE SHARE.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("territory", "Territory");
            for (int i = 0; i < Dalobject.GetColumnName().Count; i++)
            {
                string sdf = Dalobject.GetColumnName()[i].ToString();
                string dfg = "Col" + (i + 1) + "";
                reportDocument.SetParameterValue("Col" + (i + 1) + "", Dalobject.GetColumnName()[i].ToString());
            }
            string downfilename = "Territory_Wise_Identical_Product_Value_Share_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptMarketIdenticalProductValueShare()
        {
            ViewBag.formTitle = "Market Wise Identical Product Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptMarketIdenticalProductValueShare(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.MarketIdenticalProductValueShare(model.FromDate, model.ToDate);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/MARKET WISE IDENTICAL VALUE SHARE.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("market", "Market");
            for (int i = 0; i < Dalobject.GetColumnName().Count; i++)
            {
                string sdf = Dalobject.GetColumnName()[i].ToString();
                string dfg = "Col" + (i + 1) + "";
                reportDocument.SetParameterValue("Col" + (i + 1) + "", Dalobject.GetColumnName()[i].ToString());
            }
            string downfilename = "Market_Wise_Identical_Product_Value_Share_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptPrescriptionInformation()
        {
            ViewBag.formTitle = "Prescription Information";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptPrescriptionInformation(ReportModel model)
        {
            if (string.IsNullOrEmpty(model.REGION_NAME))
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                 if (dtRegion.Rows.Count > 0)
                 {
                     for (int i = 0; i < dtRegion.Rows.Count; i++)
                     {
                         model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                         model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                         DataTable dt;
                         ReportDocument reportDocument = new ReportDocument();
                         dt = Dalobject.PrescriptionInformation(model);
                         string ReportPath = Server.MapPath("~/Reports");
                         //ReportPath = ReportPath + "/Prescription Information.rpt";
                         //ReportPath = ReportPath + "/Prescriptionsurvey.rpt";
                         ReportPath = ReportPath + "/SurveySheet.rpt";
                         reportDocument.Load(ReportPath);
                         reportDocument.SetDataSource(dt);
                         reportDocument.Refresh();
                         reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                         reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                         reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                         reportDocument.SetParameterValue("FromDate", model.FromDate);
                         reportDocument.SetParameterValue("ToDate", model.ToDate);


                         string path = Server.MapPath("~/Prescription_Information/");
                         if (!Directory.Exists(path))
                         {
                             Directory.CreateDirectory(path);
                         }

                         
                         if (model.ReportType == "PDF")
                         {
                             string downfilename = "Prescription_Information" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                             var physicalPath = Path.Combine(Server.MapPath("~/Prescription_Information"), downfilename);
                             reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                         }

                         else
                         {
                             string downfilename = "Prescription_Information" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xls";
                             var physicalPath = Path.Combine(Server.MapPath("~/Prescription_Information"), downfilename);
                             reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                         }
                         reportDocument.Close();
                         reportDocument.Dispose();
                         GC.Collect();

                     }
                 }

                 string[] filePaths = Directory.GetFiles(Server.MapPath("~/Prescription_Information/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Prescription_Information");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Prescription_Information");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Prescription_Information"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }

            else
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.PrescriptionInformation(model);
                string ReportPath = Server.MapPath("~/Reports");
                //ReportPath = ReportPath + "/Prescription Information.rpt";
                //ReportPath = ReportPath + "/Prescriptionsurvey.rpt";
                ReportPath = ReportPath + "/SurveySheet.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();
                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                string downfilename = "Prescription_Information_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
                
            }

            return View();
        }

        public ActionResult frmRptTherapeuticClassWiseNationalValueShareDivision()
        {
            ViewBag.formTitle = "Therapeutic Class  Wise Value Share of a Division";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptTherapeuticClassWiseNationalValueShareDivision(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.TherapeuticClassWiseNationalValueShareDivision(model);
            string ReportPath = Server.MapPath("~/Reports");

            switch (model.ReportName)
            {
                case "Therapeutic Class Wise Value Share of a Division (By %)":
                    ReportPath = ReportPath + "/Therapeutic Class  wise Value Share (By Perc) of a Division.rpt";
                    break;
                default:
                    ReportPath = ReportPath + "/Therapeutic Class  wise Value Share (By Val) of a Division.rpt";
                    break;
            }
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            if (string.IsNullOrEmpty(model.DIVISION_NAME))
                reportDocument.SetParameterValue("division", "All Division");
            else
                reportDocument.SetParameterValue("division", model.DIVISION_NAME);
            reportDocument.SetParameterValue("ddlTheraClassLevel", model.ddlTheraClassLevel);
            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlGeneric", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlGeneric", "OTC");
            }

            if (model.ReportName == "Therapeutic Class Wise Value Share of a Division (By %)")
            {
                string downfilename = "Therapeutic_Class_Wise_Value_Share_of_a_Division_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "Therapeutic_Class_Wise_Value_Share_of_a_Division_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            }

            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptTherapeuticClassWiseNationalValueShareTerritory()
        {
            ViewBag.formTitle = "Therapeutic Class  Wise Value Share of a Territory";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptTherapeuticClassWiseNationalValueShareTerritory(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.TherapeuticClassWiseNationalValueShareTerritory(model);
            string ReportPath = Server.MapPath("~/Reports");

            switch (model.ReportName)
            {
                case "Therapeutic Class Wise Value Share of a Territory (By %)":
                    ReportPath = ReportPath + "/Therapeutic Class  wise Value Share (By Perc) of a Territory.rpt";
                    break;
                default:
                    ReportPath = ReportPath + "/THERAPEUTIC CLASS WISE VALUE SHARE BY VALUE FOR TERRITORY.rpt";
                    break;
            }
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            if (string.IsNullOrEmpty(model.TERRITORY_NAME))
                reportDocument.SetParameterValue("territory", "All Territory");
            else
                reportDocument.SetParameterValue("territory", model.TERRITORY_NAME);
            reportDocument.SetParameterValue("ddlTheraClassLevel", model.ddlTheraClassLevel);
            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlGeneric", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlGeneric", "OTC");
            }

            if (model.ReportName == "Therapeutic Class Wise Value Share of a Territory (By %)")
            {
                string downfilename = "Therapeutic_Class_Wise_Value_Share_of_a_Territory_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "Therapeutic_Class_Wise_Value_Share_of_a_Territory_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptTherapeuticClassWiseNationalValueShare()
        {
            ViewBag.formTitle = "Therapeutic Class Wise National Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptTherapeuticClassWiseNationalValueShare(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.TherapeuticClassWiseNationalValueShare(model);
            string ReportPath = Server.MapPath("~/Reports");

            switch (model.ReportName)
            {
                case "Therapeutic Class Wise National Value Share (By %)":
                    ReportPath = ReportPath + "/RptTherapeuticClassWiseNationalValueShare(By Perc).rpt";
                    break;
                default:
                    ReportPath = ReportPath + "/Therapeutic Class wise National Value Share (By Value).rpt";
                    break;
            }
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            //if (string.IsNullOrEmpty(model.MARKET_NAME))
            //    reportDocument.SetParameterValue("region", "All Market");
            //else
            //    reportDocument.SetParameterValue("region", model.MARKET_NAME);
            reportDocument.SetParameterValue("ddlTheraClassLevel", model.ddlTheraClassLevel);
            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlGeneric", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlGeneric", "OTC");
            }

            if (model.ReportName == "Therapeutic Class Wise National Value Share (By %)")
            {
                string downFileName = "Therapeutic_Class_Wise_National_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else
            {
                string downFileName = "Therapeutic_Class_Wise_National_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        //
        public ActionResult frmRptTherapeuticClassWiseValueShareofaRegion()
        {
            ViewBag.formTitle = "Therapeutic Class Wise Value Share of a Region";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptTherapeuticClassWiseValueShareofaRegion(ReportModel model)
        {
            if (string.IsNullOrEmpty(model.REGION_NAME))
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();
                        //
                        ReportDocument rd = new ReportDocument();
                        string ReportPath = Server.MapPath("~/Reports");
                        var dataTableObj = new DataTable();
                        switch (model.ReportName)
                        {
                            case "Therapeutic Class  wise Value Share (By %) of a Region":
                                dataTableObj = Dalobject.TherapeuticClasswiseValueShareofaRegionPerc(model);
                                ReportPath = ReportPath + "/RptTherapeuticClasswiseValueShareofaRegionPerc.rpt";
                                break;
                            default:
                                dataTableObj = Dalobject.TherapeuticClasswiseValueShareofaRegionValue(model);
                                ReportPath = ReportPath + "/RptTherapeuticClasswiseValueShareofaRegionValue.rpt";
                                break;
                        }

                        rd.Load(ReportPath);
                        rd.SetDataSource(dataTableObj);
                        rd.Refresh();
                        rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        rd.SetParameterValue("DEVBY", Session["DEVBY"]);
                        rd.SetParameterValue("ProjectName", Session["ProjectName"]);
                        rd.SetParameterValue("FromDate", model.FromDate);
                        rd.SetParameterValue("ToDate", model.ToDate);
                        rd.SetParameterValue("ddlTherapeutic", model.ddlTherapeutic);
                        if (model.ddlPresCategory == "0")
                            rd.SetParameterValue("ddlPresCategory", "All");
                        else
                        {
                            if (model.ddlPresCategory == "P")
                                rd.SetParameterValue("ddlPresCategory", "Prescription");
                            else if (model.ddlPresCategory == "O")
                                rd.SetParameterValue("ddlPresCategory", "Orgranization");
                            else if (model.ddlPresCategory == "S")
                                rd.SetParameterValue("ddlPresCategory", "Slip");
                            else if (model.ddlPresCategory == "T")
                                rd.SetParameterValue("ddlPresCategory", "OTC");
                        }

                        rd.SetParameterValue("REGION_NAME", model.REGION_NAME);
                        rd.Close();
                        rd.Dispose();
                        GC.Collect();
                        //
                        string path = Server.MapPath("~/Therapeutic_Class_Wise_Value_Share_of/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //else
                        //{
                        //    Directory.Delete(path);
                        //}
                        string downfilename = "Therapeutic_Class_Wise_Value_Share_of_" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        var physicalPath = Path.Combine(Server.MapPath("~/Therapeutic_Class_Wise_Value_Share_of"), downfilename);
                        if (model.ReportType == "PDF")
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        else
                            rd.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                    }
                }
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Therapeutic_Class_Wise_Value_Share_of/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Therapeutic_Class_Wise_Value_Share_of");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Therapeutic_Class_Wise_Value_Share_of");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Therapeutic_Class_Wise_Value_Share_of"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }
            else
            {
                ReportDocument rd = new ReportDocument();
                string ReportPath = Server.MapPath("~/Reports");
                var dataTableObj = new DataTable();
                switch (model.ReportName)
                {
                    case "Therapeutic Class  wise Value Share (By %) of a Region":
                        dataTableObj = Dalobject.TherapeuticClasswiseValueShareofaRegionPerc(model);
                        ReportPath = ReportPath + "/RptTherapeuticClasswiseValueShareofaRegionPerc.rpt";
                        break;
                    default:
                        dataTableObj = Dalobject.TherapeuticClasswiseValueShareofaRegionValue(model);
                        ReportPath = ReportPath + "/RptTherapeuticClasswiseValueShareofaRegionValue.rpt";
                        break;
                }

                rd.Load(ReportPath);
                rd.SetDataSource(dataTableObj);
                rd.Refresh();

                rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                rd.SetParameterValue("DEVBY", Session["DEVBY"]);
                rd.SetParameterValue("ProjectName", Session["ProjectName"]);
                rd.SetParameterValue("FromDate", model.FromDate);
                rd.SetParameterValue("ToDate", model.ToDate);
                rd.SetParameterValue("ddlTherapeutic", model.ddlTherapeutic);
                if (model.ddlPresCategory == "0")
                    rd.SetParameterValue("ddlPresCategory", "All");
                else
                {
                    if (model.ddlPresCategory == "P")
                        rd.SetParameterValue("ddlPresCategory", "Prescription");
                    else if (model.ddlPresCategory == "O")
                        rd.SetParameterValue("ddlPresCategory", "Orgranization");
                    else if (model.ddlPresCategory == "S")
                        rd.SetParameterValue("ddlPresCategory", "Slip");
                    else if (model.ddlPresCategory == "T")
                        rd.SetParameterValue("ddlPresCategory", "OTC");
                }

                //rd.SetParameterValue("ddlPresCategory", model.ddlPresCategory);
                rd.SetParameterValue("REGION_NAME", model.REGION_NAME);

                if (model.ReportName == "Therapeutic Class  wise Value Share (By %) of a Region")
                {
                    string downFileName = "Therapeutic_Class_Wise_Value_Share_of_a_Region_By_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                }
                else
                {
                    string downFileName = "Therapeutic_Class_Wise_Value_Share_of_a_Region_By_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                    //rd.ExportToDisk(ExportFormatType.PortableDocFormat,)
                    //rd.SaveAs()
                    //rd.Export()
                }

                rd.Close();
                rd.Dispose();
                GC.Collect();
            }
            //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptGenericWiseValueShareNationalVsRegion()
        {
            ViewBag.formTitle = "Generic wise Value Share National Vs Region";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericWiseValueShareNationalVsRegion(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.GenericWiseValueShareNationalVsRegion(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptGenericWisevalueShareNationalVsRegion.rpt";

            //System.Data.DataTable dtExcel = ConvertToDataTable(ExcelUserWorkinngSessionList);

            //Response.Clear();

            //Response.Buffer = true;

            //Response.AddHeader("content-disposition", "attachment;filename=Generic_Wise_Value_Share_National_Vs_Region_.xls");

            //Response.Charset = "";

            //Response.ContentType = "application/vnd.ms-excel";

            //StringWriter sw = new StringWriter();

            //HtmlTextWriter hw = new HtmlTextWriter(sw);

            //GridView GridView1 = new GridView();

            //GridView1.DataSource = dt; // dt 
            //GridView1.DataBind();

            //GridView1.RenderControl(hw);

            //Response.Output.Write(sw.ToString());

            //Response.Flush();

            //Response.End();

            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("MANUFACTURER_NAME", model.MANUFACTURER_NAME);
            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlGeneric", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlGeneric", "OTC");
            }

            string downFileName = "Generic_Wise_Value_Share_National_Vs_Region_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "Generic wise Value Share National Vs Region");
            return View();
        }
        public ActionResult frmRptGenericWiseNationalValueShare()
        {
            ViewBag.formTitle = "Generic Wise National Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericWiseNationalValueShare(ReportModel model)
        {
            ReportDocument rd = new ReportDocument();
            string ReportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            switch (model.ReportName)
            {
                case "Generic Wise National Value Share (by %)":
                    dataTableObj = Dalobject.GenericWiseNationalValueShareByPerc(model);
                    ReportPath = ReportPath + "/RptGenericWiseNationalValueShare(by Perc).rpt";
                    break;
                default:
                    dataTableObj = Dalobject.GenericWiseNationalValueShareByValue(model);
                    ReportPath = ReportPath + "/RptGenericWiseNationalValueShare(By Value).rpt";
                    break;
            }

            rd.Load(ReportPath);
            rd.SetDataSource(dataTableObj);
            rd.Refresh();

            rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            rd.SetParameterValue("DEVBY", Session["DEVBY"]);
            rd.SetParameterValue("ProjectName", Session["ProjectName"]);
            rd.SetParameterValue("FromDate", model.FromDate);
            rd.SetParameterValue("ToDate", model.ToDate);
            rd.SetParameterValue("ddlGeneric", model.ddlGeneric);
            //rd.SetParameterValue("Address", model.Address);

            if (model.ReportName == "Generic Wise National Value Share (by %)")
            {
                string downFileName = "Generic_Wise_National_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);

            }
            else
            {
                string downFileName = "Generic_Wise_National_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            rd.Close();
            rd.Dispose();
            GC.Collect();
            //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();

        }

        public ActionResult frmRptMarketWiseValueShareOfAGeneric()
        {
            ViewBag.formTitle = "Market Wise Value Share of a Generic";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptMarketWiseValueShareOfAGeneric(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.MarketWiseValueShareOfAGeneric(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptMarketWiseValueShareOfAGeneric.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("GENERIC_NAME", model.GENERIC_NAME);
            reportDocument.SetParameterValue("DOSAGE_FORM", model.DOSAGE_FORM_NAME);
            reportDocument.SetParameterValue("DSG_STRENGTH", model.DSG_STRENGTH_NAME);
            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlGeneric", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlGeneric", "OTC");
            }

            string downFileName = "Market_Wise_Value_Share_of_a_Generic_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }
        public ActionResult frmRptTerritoryWiseValueShareOfAGeneric()
        {
            ViewBag.formTitle = "Territory Wise Value Share of a Generic";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptTerritoryWiseValueShareOfAGeneric(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.TerritoryWiseValueShareOfAGeneric(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptTerritoryWiseValueShareOfAGeneric.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("GENERIC_NAME", model.GENERIC_NAME);
            reportDocument.SetParameterValue("DOSAGE_FORM", model.DOSAGE_FORM_NAME);
            reportDocument.SetParameterValue("DSG_STRENGTH", model.DSG_STRENGTH_NAME);
            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlGeneric", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlGeneric", "OTC");
            }

            string downFileName = "Territory_Wise_Value_Share_of_a_Generic_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }
       
        public ActionResult frmRptGenericWiseNationalValueShareoftheMarket()
        {
            ViewBag.formTitle = "Generic Wise Value Share of the Market";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericWiseNationalValueShareoftheMarket(ReportModel model)
        {
            //if (string.IsNullOrEmpty(model.MARKET_NAME))
            //{
            //    DataTable dtRegion = dbHelper.GetDataTable("SELECT MARKET_CODE,MARKET_NAME FROM MARKET ORDER BY MARKET_CODE");
            //    if (dtRegion.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dtRegion.Rows.Count; i++)
            //        {

            //            model.MARKET_CODE = dtRegion.Rows[i]["MARKET_CODE"].ToString();
            //            model.MARKET_NAME = dtRegion.Rows[i]["MARKET_NAME"].ToString();
            //            DataTable dt;
            //            ReportDocument reportDocument = new ReportDocument();
            //            dt = Dalobject.GenericWiseNationalValueShareoftheMarket(model);
            //            string ReportPath = Server.MapPath("~/Reports");

            //            switch (model.ReportName)
            //            {
            //                case "Generic Wise National Value Share of the Market (By %)":
            //                    ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE MARKET by Perc.rpt";
            //                    break;
            //                default:
            //                    ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUSE SHARE OF A MARKET BY VALUE.rpt";
            //                    break;
            //            }
            //            //ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE REGION By Perc.rpt";
            //            reportDocument.Load(ReportPath);
            //            reportDocument.SetDataSource(dt);
            //            reportDocument.Refresh();
            //            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            //            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            //            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            //            reportDocument.SetParameterValue("FromDate", model.FromDate);
            //            reportDocument.SetParameterValue("ToDate", model.ToDate);
            //            if (string.IsNullOrEmpty(model.MARKET_NAME))
            //            {

            //                reportDocument.SetParameterValue("market", "All Market");
            //            }
            //            else
            //            {
            //                reportDocument.SetParameterValue("market", model.MARKET_NAME);
            //            }

            //            if (model.ddlPresCategory == "0")
            //                reportDocument.SetParameterValue("ddlGeneric", "All");
            //            else
            //            {
            //                if (model.ddlPresCategory == "P")
            //                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
            //                else if (model.ddlPresCategory == "O")
            //                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
            //                else if (model.ddlPresCategory == "S")
            //                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
            //                else if (model.ddlPresCategory == "T")
            //                    reportDocument.SetParameterValue("ddlGeneric", "OTC");
            //            }

            //            //if (model.ReportName == "Generic Wise National Value Share of the Market (By %)")
            //            //{
            //            //    string downfilename = "Generic_Wise_National_Value_Share_of_the_Market_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
            //            //    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            //            //}
            //            //else
            //            //{
            //            //    string downfilename = "Generic_Wise_National_Value_Share_of_the_Market_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
            //            //    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            //            //}
            //        }


            //    }


            //}

            //else
            //{
            //    DataTable dt;
            //    ReportDocument reportDocument = new ReportDocument();
            //    dt = Dalobject.GenericWiseNationalValueShareoftheMarket(model);
            //    string ReportPath = Server.MapPath("~/Reports");

            //    switch (model.ReportName)
            //    {
            //        case "Generic Wise National Value Share of the Market (By %)":
            //            ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE MARKET by Perc.rpt";
            //            break;
            //        default:
            //            ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUSE SHARE OF A MARKET BY VALUE.rpt";
            //            break;
            //    }
            //    //ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE REGION By Perc.rpt";
            //    reportDocument.Load(ReportPath);
            //    reportDocument.SetDataSource(dt);
            //    reportDocument.Refresh();
            //    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            //    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            //    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            //    reportDocument.SetParameterValue("FromDate", model.FromDate);
            //    reportDocument.SetParameterValue("ToDate", model.ToDate);
            //    if (string.IsNullOrEmpty(model.MARKET_NAME))
            //    {

            //        reportDocument.SetParameterValue("market", "All Market");
            //    }
            //    else
            //    {
            //        reportDocument.SetParameterValue("market", model.MARKET_NAME);
            //    }

            //    if (model.ddlPresCategory == "0")
            //        reportDocument.SetParameterValue("ddlGeneric", "All");
            //    else
            //    {
            //        if (model.ddlPresCategory == "P")
            //            reportDocument.SetParameterValue("ddlGeneric", "Prescription");
            //        else if (model.ddlPresCategory == "O")
            //            reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
            //        else if (model.ddlPresCategory == "S")
            //            reportDocument.SetParameterValue("ddlGeneric", "Slip");
            //        else if (model.ddlPresCategory == "T")
            //            reportDocument.SetParameterValue("ddlGeneric", "OTC");
            //    }
            //}


            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.GenericWiseNationalValueShareoftheMarket(model);
            string ReportPath = Server.MapPath("~/Reports");

            switch (model.ReportName)
            {
                case "Generic Wise National Value Share of the Market (By %)":
                    ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE MARKET by Perc.rpt";
                    break;
                default:
                    ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUSE SHARE OF A MARKET BY VALUE.rpt";
                    break;
            }
            //ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE REGION By Perc.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);


            if (string.IsNullOrEmpty(model.MARKET_NAME))
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT MARKET_CODE,MARKET_NAME FROM MARKET ORDER BY MARKET_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {

                        model.MARKET_CODE = dtRegion.Rows[i]["MARKET_CODE"].ToString();
                        model.MARKET_NAME = dtRegion.Rows[i]["MARKET_NAME"].ToString();
                    }
                }
                reportDocument.SetParameterValue("market", model.MARKET_NAME);
            }
            else
            {
                reportDocument.SetParameterValue("market", model.MARKET_NAME);
            }

            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlGeneric", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlGeneric", "OTC");

            }

            if (model.ReportName == "Generic Wise National Value Share of the Market (By %)")
            {
                string downfilename = "Generic_Wise_National_Value_Share_of_the_Market_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "Generic_Wise_National_Value_Share_of_the_Market_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            }

            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");

        }

        public ActionResult frmRptGenericWiseNationalValueShareoftheTerritory()
        {
            ViewBag.formTitle = "Generic Wise Value Share of the Territory";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericWiseNationalValueShareoftheTerritory(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.GenericWiseNationalValueShareoftheTerritory(model);
            string ReportPath = Server.MapPath("~/Reports");

            switch (model.ReportName)
            {
                case "Generic Wise National Value Share of the Territory (By %)":
                    ReportPath = ReportPath + "/RptGenericWiseValueShareofaTerritory(By Perc).rpt";
                    break;
                default:
                    ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE TERRITORY By Val.rpt";
                    break;
            }
            //ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE REGION By Perc.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            if (string.IsNullOrEmpty(model.TERRITORY_NAME))
                reportDocument.SetParameterValue("territory", "All Territory");
            else
                reportDocument.SetParameterValue("territory", model.TERRITORY_NAME);
            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlGeneric", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlGeneric", "OTC");
            }
            if (model.ReportName == "Generic Wise National Value Share of the Territory (By %)")
            {
                string downFileName = "Generic_Wise_National_Value_Share_of_the_Territory_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else
            {
                string downFileName = "Generic_Wise_National_Value_Share_of_the_Territory_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, "crReport");
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptGenericWiseValueShare()
        {
            ViewBag.formTitle = "Generic Wise Value Share of a Region";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericWiseValueShare(ReportModel model)
        {
            if (string.IsNullOrEmpty(model.REGION_NAME))
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                        //DataTable dt;
                        ReportDocument reportDocument = new ReportDocument();
                        string ReportPath = Server.MapPath("~/Reports");
                        var dt = new DataTable();

                        switch (model.ReportName)
                        {
                            case "Generic Wise National Value Share of the Region (By %)":
                                dt = Dalobject.GenericWiseValueShareByPerc(model);
                                ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE REGION By Perc.rpt";
                                break;
                            default:
                                dt = Dalobject.GenericWiseValueShareByPerc(model);
                                ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE REGION by Val.rpt";
                                break;
                        }
                        //ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE REGION By Perc.rpt";
                        reportDocument.Load(ReportPath);
                        reportDocument.SetDataSource(dt);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (string.IsNullOrEmpty(model.REGION_NAME))
                            reportDocument.SetParameterValue("region", "All Region");
                        else
                            reportDocument.SetParameterValue("region", model.REGION_NAME);
                        if (model.ddlPresCategory == "0")
                            reportDocument.SetParameterValue("ddlGeneric", "All");
                        else
                        {
                            if (model.ddlPresCategory == "P")
                                reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                            else if (model.ddlPresCategory == "O")
                                reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                            else if (model.ddlPresCategory == "S")
                                reportDocument.SetParameterValue("ddlGeneric", "Slip");
                            else if (model.ddlPresCategory == "T")
                                reportDocument.SetParameterValue("ddlGeneric", "OTC");
                        }



                        string path = Server.MapPath("~/Generic_Wise_National_Value_Share_of_the_Region/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        if (model.ReportName == "Generic Wise National Value Share of the Region (By %)")
                        {
                            if (model.ReportType == "PDF")
                            {
                            string downfilename = "Generic_Wise_National_Value_Share_of_the_Region_" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                            var physicalPath = Path.Combine(Server.MapPath("~/Generic_Wise_National_Value_Share_of_the_Region"), downfilename);
                            reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                            }
                            else
                            {
                            string downfilename = "Generic_Wise_National_Value_Share_of_the_Region_" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xlsx";
                            var physicalPath = Path.Combine(Server.MapPath("~/Generic_Wise_National_Value_Share_of_the_Region"), downfilename);
                            reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                            }
                        }

                        else
                        {
                            if (model.ReportType == "PDF")
                            {
                            string downfilename = "Generic_Wise_National_Value_Share_of_the_Region" + model.REGION_NAME + "_By_Value_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                            var physicalPath = Path.Combine(Server.MapPath("~/Generic_Wise_National_Value_Share_of_the_Region"), downfilename);
                            reportDocument.ExportToDisk(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, physicalPath);  
                            }
                            else
                            {
                            string downfilename = "Generic_Wise_National_Value_Share_of_the_Region" + model.REGION_NAME + "_By_Value_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xlsx";
                            var physicalPath = Path.Combine(Server.MapPath("~/Generic_Wise_National_Value_Share_of_the_Region"), downfilename);
                            reportDocument.ExportToDisk(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, physicalPath);
                            }
                            
                        }

                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();
                    }
                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Generic_Wise_National_Value_Share_of_the_Region/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Generic_Wise_National_Value_Share_of_the_Region");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Generic_Wise_National_Value_Share_of_the_Region");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Generic_Wise_National_Value_Share_of_the_Region"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }

            else
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.GenericWiseValueShareByPerc(model);
                string ReportPath = Server.MapPath("~/Reports");

                switch (model.ReportName)
                {
                    case "Generic Wise National Value Share of the Region (By %)":
                        ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE REGION By Perc.rpt";
                        break;
                    default:
                        ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE REGION by Val.rpt";
                        break;
                }
                //ReportPath = ReportPath + "/GENERIC WISE NATIONAL VALUE SHARE (ALL) OF THE REGION By Perc.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                if (string.IsNullOrEmpty(model.REGION_NAME))
                    reportDocument.SetParameterValue("region", "All Region");
                else
                    reportDocument.SetParameterValue("region", model.REGION_NAME);
                if (model.ddlPresCategory == "0")
                    reportDocument.SetParameterValue("ddlGeneric", "All");
                else
                {
                    if (model.ddlPresCategory == "P")
                        reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                    else if (model.ddlPresCategory == "O")
                        reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                    else if (model.ddlPresCategory == "S")
                        reportDocument.SetParameterValue("ddlGeneric", "Slip");
                    else if (model.ddlPresCategory == "T")
                        reportDocument.SetParameterValue("ddlGeneric", "OTC");
                }

                if (model.ReportName == "Generic Wise National Value Share of the Region (By %)")
                {
                    string downfilename = "Generic_Wise_National_Value_Share_of_the_Region_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                else
                {
                    string downfilename = "Generic_Wise_National_Value_Share_of_the_Region_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }


            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptGenericContributionToNationalShare()
        {
            ViewBag.formTitle = "Generic Contribution To National Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericContributionToNationalShare(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.GenericContributionToNationalShare(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/GENERIC CONTRIBUTION TO NATIONAL SHARE (ALL).rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlGeneric", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlGeneric", "OTC");
            }

            if (model.ReportName == "Therapeutic Class Wise Value Share of a Territory (By %)")
            {
                string downfilename = "Therapeutic_Class_Wise_Value_Share_of_a_Territory_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "Therapeutic_Class_Wise_Value_Share_of_a_Territory_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            }

            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptGenericWiseValueShareOfaManufacturer()
        {
            ViewBag.formTitle = "Generic Wise Product Share Value Of a Manufacturer";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericWiseValueShareOfaManufacturer(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.GenericWiseValueShareOfaManufacturer(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/GENERIC WISE PRODUCT SHARE VALUE OF SQUARE PHARMACEUTICALS LTD.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            if (string.IsNullOrEmpty(model.MANUFACTURER_NAME))
                reportDocument.SetParameterValue("manufacturer", "All Manufacturer");
            else
                reportDocument.SetParameterValue("manufacturer", model.MANUFACTURER_NAME);

            string downfilename = "Generic_Wise_Product_Share_Value_Of_a_Manufacturer_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptGenericWisePrescriberListofaMarket()
        {
            ViewBag.formTitle = "Generic Wise Prescriber List of a Market";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericWisePrescriberListofaMarket(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.GenericWisePrescriberListofaMarket(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/Generic wise Prescriber List of a Market.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            if (string.IsNullOrEmpty(model.MARKET_NAME))
                reportDocument.SetParameterValue("market", "All Territory");
            else
                reportDocument.SetParameterValue("market", model.MARKET_NAME);
            if (string.IsNullOrEmpty(model.GENERIC_NAME))
                reportDocument.SetParameterValue("generic", "All Generic");
            else
                reportDocument.SetParameterValue("generic", model.GENERIC_NAME);

            string downfilename = "Generic_Wise_Prescriber_List_of_a_Market_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        public ActionResult frmRptGenericWisePrescriberListofaTerritory()
        {
            ViewBag.formTitle = "Generic Wise Prescriber List of a Territory";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericWisePrescriberListofaTerritory(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.GenericWisePrescriberListofaTerritory(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/Generic wise Prescriber List of a Territory.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            if (string.IsNullOrEmpty(model.TERRITORY_NAME))
                reportDocument.SetParameterValue("territory", "All Territory");
            else
                reportDocument.SetParameterValue("territory", model.TERRITORY_NAME);
            if (string.IsNullOrEmpty(model.GENERIC_NAME))
                reportDocument.SetParameterValue("generic", "All Generic");
            else
                reportDocument.SetParameterValue("generic", model.GENERIC_NAME);

            string downfilename = "Generic_Wise_Prescriber_List_of_a_Territory_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        public ActionResult frmRptGenericWisePrescriberListofaRegion()
        {
            ViewBag.formTitle = "Generic Wise Prescriber List of a Region";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericWisePrescriberListofaRegion(ReportModel model)
        {
            if (string.IsNullOrEmpty(model.REGION_NAME))
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                        DataTable dt;
                        ReportDocument reportDocument = new ReportDocument();
                        dt = Dalobject.GenericWisePrescriberListofaRegion(model);
                        string ReportPath = Server.MapPath("~/Reports");
                        ReportPath = ReportPath + "/Generic wise Prescriber List of a Region.rpt";
                        reportDocument.Load(ReportPath);
                        reportDocument.SetDataSource(dt);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (string.IsNullOrEmpty(model.REGION_NAME))
                            reportDocument.SetParameterValue("region", "All Region");
                        else
                            reportDocument.SetParameterValue("region", model.REGION_NAME);
                        if (string.IsNullOrEmpty(model.GENERIC_NAME))
                            reportDocument.SetParameterValue("generic", "All Generic");
                        else
                            reportDocument.SetParameterValue("generic", model.GENERIC_NAME);

                        string path = Server.MapPath("~/Generic_Wise_Prescriber_List_of_a_Region/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string downfilename = "Generic_Wise_Prescriber_List_of_a_Region_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        var physicalPath = Path.Combine(Server.MapPath("~/Generic_Wise_Prescriber_List_of_a_Region"), downfilename);
                        if (model.ReportType == "PDF")
                            reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        else
                            reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();
                    }
                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Generic_Wise_Prescriber_List_of_a_Region/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Generic_Wise_Prescriber_List_of_a_Region");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Generic_Wise_Prescriber_List_of_a_Region");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Generic_Wise_Prescriber_List_of_a_Region"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }

            }

            else
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.GenericWisePrescriberListofaRegion(model);
                string ReportPath = Server.MapPath("~/Reports");
                ReportPath = ReportPath + "/Generic wise Prescriber List of a Region.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                if (string.IsNullOrEmpty(model.REGION_NAME))
                    reportDocument.SetParameterValue("region", "All Region");
                else
                    reportDocument.SetParameterValue("region", model.REGION_NAME);
                if (string.IsNullOrEmpty(model.GENERIC_NAME))
                    reportDocument.SetParameterValue("generic", "All Generic");
                else
                    reportDocument.SetParameterValue("generic", model.GENERIC_NAME);

                string downFileName = "Generic_Wise_Prescriber_List_of_a_Region_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }


            return View();
        }
        public ActionResult frmMarketWiseProductShareRankingRpt()
        {
            ViewBag.formTitle = "Market Wise Product Share Ranking";
            return View();
        }
        [HttpPost]
        public ActionResult frmMarketWiseProductShareRankingRpt(ReportModel model)
        {

            ReportDocument rd = new ReportDocument();
            string ReportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            switch (model.ReportName)
            {
                case "MARKET Wise Product Share Ranking (By %)":
                    dataTableObj = Dalobject.MarketWiseProductShareRankingPerc(model);
                    ReportPath = ReportPath + "/RptMarketWiseProductShareRankingPerc.rpt";
                    break;
                default:
                    dataTableObj = Dalobject.MarketWiseProductShareRankingValue(model);
                    ReportPath = ReportPath + "/RptMarketWiseProductShareRankingValue.rpt";
                    break;
            }

            rd.Load(ReportPath);
            rd.SetDataSource(dataTableObj);
            rd.Refresh();

            rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            rd.SetParameterValue("DEVBY", Session["DEVBY"]);
            rd.SetParameterValue("ProjectName", Session["ProjectName"]);
            rd.SetParameterValue("FromDate", model.FromDate);
            rd.SetParameterValue("ToDate", model.ToDate);
            //rd.SetParameterValue("NumberOfTran", model.NumberOfTran);
            //rd.SetParameterValue("Address", model.Address);
            if (model.ReportName == "MARKET Wise Product Share Ranking (By %)")
            {
                string downFileName = "Market_Wise_Product_Share_Ranking_(By_%)_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else
            {

                string downFileName = "Market_Wise_Product_Share_Ranking_(By_Value)_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            rd.Close();
            rd.Dispose();
            GC.Collect();
            //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        public ActionResult frmProductShareRankingOfARegionRpt()
        {
            ViewBag.formTitle = "Territory Wise Product Share Ranking";
            return View();
        }

        [HttpPost]
        public ActionResult frmProductShareRankingOfARegionRpt(ReportModel model)
        {

            ReportDocument rd = new ReportDocument();
            string ReportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            switch (model.ReportName)
            {
                case "Territory WISE Product Share Ranking (By %)":
                    dataTableObj = Dalobject.ProductShareRankingOfARegionPerc(model);
                    ReportPath = ReportPath + "/RptTerritoryWiseProductShareRankingByPerc.rpt";
                    break;
                default:
                    dataTableObj = Dalobject.ProductShareRankingOfARegionValue(model);
                    ReportPath = ReportPath + "/RptTerritoryWiseProductShareRankingByValue.rpt";
                    break;
            }

            rd.Load(ReportPath);
            rd.SetDataSource(dataTableObj);
            rd.Refresh();
            rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            rd.SetParameterValue("DEVBY", Session["DEVBY"]);
            rd.SetParameterValue("ProjectName", Session["ProjectName"]);
            rd.SetParameterValue("FromDate", model.FromDate);
            rd.SetParameterValue("ToDate", model.ToDate);
            //rd.SetParameterValue("NumberOfTran", model.NumberOfTran);
            //rd.SetParameterValue("Address", model.Address);

            if (model.ReportName == "Territory WISE Product Share Ranking (By %)")
            {
                string downFileName = "Territory_Wise_Product_Share_Ranking_(By_%)_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else
            {
                string downFileName = "Territory_Wise_Product_Share_Ranking_(By_Value)_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            rd.Close();
            rd.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmFrequencyBasedNationalProductShareRankingRpt()
        {
            ViewBag.formTitle = "Frequency Base National Product Share Ranking";
            return View();
        }

        [HttpPost]
        public ActionResult frmFrequencyBasedNationalProductShareRankingRpt(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.FrequencyBasedNationalProductShareRankingPerc(model);
            string ReportPath = Server.MapPath("~/Reports");
            if(model.ReportType == "PDF")
            {
                ReportPath = ReportPath + "/RptFrequencyBasedNationalProductShareRanking.rpt";
            }
            else
            {
                ReportPath = ReportPath + "/RptFrequencyBasedNationalProductShareRankingExcel.rpt";
            }
            
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlPresCategory", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlPresCategory", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlPresCategory", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlPresCategory", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlPresCategory", "OTC");
            }
            //rd.SetParameterValue("NumberOfTran", model.NumberOfTran);
            //rd.SetParameterValue("Address", model.Address);

            string downFileName = "Frequency_Base_National_Product_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");

        }

        public ActionResult frmFrequencyBaseRegionProductShareRankingRpt()
        {
            ViewBag.formTitle = "Frequency Base Region Product Share Ranking";
            return View();
        }

        [HttpPost]
        public ActionResult frmFrequencyBaseRegionProductShareRankingRpt(ReportModel model)
        {
            if (model.REGION_CODE == "00")
            {

                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                        DataTable dt;
                        ReportDocument reportDocument = new ReportDocument();
                        dt = Dalobject.FrequencyBaseRegionProductShareRankingValue(model);
                        string ReportPath = Server.MapPath("~/Reports");
                        if(model.ReportType == "PDF")
                        {
                            ReportPath = ReportPath + "/RptFrequencyBaseRegionProductShareRanking.rpt";
                        }
                        else
                        {
                            ReportPath = ReportPath + "/RptFrequencyBaseRegionProductShareRankingExcel.rpt";
                        }
                        
                        reportDocument.Load(ReportPath);
                        reportDocument.SetDataSource(dt);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        //reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);

                        if (model.REGION_CODE == "00")
                        {
                            reportDocument.SetParameterValue("REGION_NAME", "All Region");
                        }

                        else
                        {
                            reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);
                        }

                        if (model.ddlPresCategory == "0")
                            reportDocument.SetParameterValue("ddlPresCategory", "All");
                        else
                        {
                            if (model.ddlPresCategory == "P")
                                reportDocument.SetParameterValue("ddlPresCategory", "Prescription");
                            else if (model.ddlPresCategory == "O")
                                reportDocument.SetParameterValue("ddlPresCategory", "Orgranization");
                            else if (model.ddlPresCategory == "S")
                                reportDocument.SetParameterValue("ddlPresCategory", "Slip");
                            else if (model.ddlPresCategory == "T")
                                reportDocument.SetParameterValue("ddlPresCategory", "OTC");
                        }
                       

                        string path = Server.MapPath("~/Frequency_Base_Region_Product_Share_Ranking/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        
                        if (model.ReportType == "PDF")
                        {
                            string downfilename = "Frequency_Base_Region_Product_Share_Ranking_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                            //string downfilename = "Frequency_Base_Region_Product_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate + ".pdf";
                           var physicalPath = Path.Combine(Server.MapPath("~/Frequency_Base_Region_Product_Share_Ranking"), downfilename);
                           reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        }

                        else
                        {
                            string downfilename = "Frequency_Base_Region_Product_Share_Ranking_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xls";
                            //string downfilename = "Frequency_Base_Region_Product_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate + ".xls";
                            var physicalPath = Path.Combine(Server.MapPath("~/Frequency_Base_Region_Product_Share_Ranking"), downfilename);
                            reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                        }

                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();
                            
                    }
                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Frequency_Base_Region_Product_Share_Ranking/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Frequency_Base_Region_Product_Share_Ranking");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Frequency_Base_Region_Product_Share_Ranking");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    //string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    string zipName = String.Format("Region_{0}.zip", "Frequency_Base_Region_Product_From: " + model.FromDate + "_To: " + model.ToDate + "");

                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Frequency_Base_Region_Product_Share_Ranking"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }

            else
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.FrequencyBaseRegionProductShareRankingValue(model);
                string ReportPath = Server.MapPath("~/Reports");
                if (model.ReportType == "PDF")
                {
                    ReportPath = ReportPath + "/RptFrequencyBaseRegionProductShareRanking.rpt";
                }
                else
                {
                    ReportPath = ReportPath + "/RptFrequencyBaseRegionProductShareRankingExcel.rpt";
                }
                //ReportPath = ReportPath + "/RptFrequencyBaseRegionProductShareRanking.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                //reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);

                if (model.REGION_CODE == "00")
                {
                    reportDocument.SetParameterValue("REGION_NAME", "All Region");
                }
                else
                {
                    reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);
                }

                if (model.ddlPresCategory == "0")
                    reportDocument.SetParameterValue("ddlPresCategory", "All");
                else
                {
                    if (model.ddlPresCategory == "P")
                        reportDocument.SetParameterValue("ddlPresCategory", "Prescription");
                    else if (model.ddlPresCategory == "O")
                        reportDocument.SetParameterValue("ddlPresCategory", "Orgranization");
                    else if (model.ddlPresCategory == "S")
                        reportDocument.SetParameterValue("ddlPresCategory", "Slip");
                    else if (model.ddlPresCategory == "T")
                        reportDocument.SetParameterValue("ddlPresCategory", "OTC");
                }
                string downFileName = "Frequency_Base_Region_Product_Share_Ranking_" + model.REGION_NAME +"_"+"From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }

            return View();
        }

        public ActionResult frmRptNationalvsRegionProductShareRanking()
        {
            ViewBag.formTitle = "National vs Region Product Share Ranking";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptNationalvsRegionProductShareRanking(ReportModel model)
        {
            if (string.IsNullOrEmpty(model.REGION_NAME))
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                        DataTable dt;
                        ReportDocument reportDocument = new ReportDocument();
                        dt = Dalobject.NationalvsRegionProductShareRanking(model);
                        string ReportPath = Server.MapPath("~/Reports");
                        ReportPath = ReportPath + "/NATIONAL vs REGION Product Share Ranking.rpt";
                        reportDocument.Load(ReportPath);
                        reportDocument.SetDataSource(dt);
                        reportDocument.Refresh();
                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();
                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (string.IsNullOrEmpty(model.REGION_NAME))
                            reportDocument.SetParameterValue("region", "All Region");
                        else
                            reportDocument.SetParameterValue("region", model.REGION_NAME);
                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();
                        string path = Server.MapPath("~/National_vs_Region_Product_Share_Ranking/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string downfilename = "National_vs_Region_Product_Share_Ranking_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        var physicalPath = Path.Combine(Server.MapPath("~/National_vs_Region_Product_Share_Ranking"), downfilename);
                        if (model.ReportType == "PDF")
                            reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        else
                            reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                    }

                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/National_vs_Region_Product_Share_Ranking/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("National_vs_Region_Product_Share_Ranking");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "National_vs_Region_Product_Share_Ranking");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/National_vs_Region_Product_Share_Ranking"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }

            else
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.NationalvsRegionProductShareRanking(model);
                string ReportPath = Server.MapPath("~/Reports");
                ReportPath = ReportPath + "/NATIONAL vs REGION Product Share Ranking.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                if (string.IsNullOrEmpty(model.REGION_NAME))
                    reportDocument.SetParameterValue("region", "All Region");
                else
                    reportDocument.SetParameterValue("region", model.REGION_NAME);

                string downfilename = "National_vs_Region_Product_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }
            return View();
        }

        public ActionResult frmRptIndividualInstitutesGenericWiseProductShareRanking()
        {
            ViewBag.formTitle = "Individual Institution's Generic wise Product Share Ranking";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptIndividualInstitutesGenericWiseProductShareRanking(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.IndividualInstitutesGenericWiseProductShareRanking(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/Individual Institution's Generic wise Product Share Ranking.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("insticode", model.INSTI_CODE);
            reportDocument.SetParameterValue("instiname", model.INSTI_NAME);
            reportDocument.SetParameterValue("Address", model.Address);

            string downfilename = "Individual_Institution_Generic_wise_Product_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptIndividualDoctorsGenericWiseProductShareRanking()
        {
            ViewBag.formTitle = "Individual Doctor's Generic Wise Product Share Ranking";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptIndividualDoctorsGenericWiseProductShareRanking(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.IndividualDoctorsGenericWiseProductShareRanking(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/Individual Doctor's Generic wise Product Share Ranking.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            //reportDocument.Dispose();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("DoctorId", model.DOCTOR_ID);

            if (string.IsNullOrEmpty(model.DOCTOR_NAME))
            {
                string Qry = "select DOCTOR_ID,DOCTOR_NAME,DEGREE,DESIGNATION,full_address(address1,address2, address3, address4) address from doctor where DOCTOR_ID ='" + model.DOCTOR_ID + "' ";
                DataTable dtbl = dbHelper.GetDataTable(Qry);
                string sd = dtbl.Rows[0]["DOCTOR_NAME"].ToString();
                reportDocument.SetParameterValue("DoctorName", dtbl.Rows[0]["DOCTOR_NAME"].ToString());
                reportDocument.SetParameterValue("Degree", dtbl.Rows[0]["DEGREE"].ToString());
                reportDocument.SetParameterValue("Designation", dtbl.Rows[0]["DESIGNATION"].ToString());
                reportDocument.SetParameterValue("Address", dtbl.Rows[0]["address"].ToString());
            }
            else
            {
                reportDocument.SetParameterValue("DoctorName", model.DOCTOR_NAME);
                reportDocument.SetParameterValue("Degree", model.DEGREE);
                reportDocument.SetParameterValue("Designation", model.DESIGNATION);
                reportDocument.SetParameterValue("Address", model.DocAddress);
            }
            //reportDocument.SetParameterValue("DoctorName", model.DOCTOR_NAME);
            //reportDocument.SetParameterValue("Degree", model.DEGREE);
            //reportDocument.SetParameterValue("Designation", model.DESIGNATION);
            //reportDocument.SetParameterValue("Address", model.DocAddress);

            string downfilename = "Individual_Doctor_Generic_Wise_Product_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptIndividualInstituteGenericWiseProductShareRankingofTheMarket()
        {
            ViewBag.formTitle = "Individual Institute Generic wise Product Share Ranking of a Market";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptIndividualInstituteGenericWiseProductShareRankingofTheMarket(ReportModel model)
        {
            DataTable dt;
            ReportDocument rd = new ReportDocument();
            dt = Dalobject.IndividualInstituteGenericWiseProductShareRankingofTheMarket(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/Individual Institute Generic Wise Product Share Ranking of The Market.rpt";
            rd.Load(ReportPath);
            rd.SetDataSource(dt);
            rd.Refresh();

            rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            rd.SetParameterValue("DEVBY", Session["DEVBY"]);
            rd.SetParameterValue("ProjectName", Session["ProjectName"]);
            rd.SetParameterValue("FromDate", model.FromDate);
            rd.SetParameterValue("ToDate", model.ToDate);
            rd.SetParameterValue("Market", model.MARKET_NAME);

            string downfilename = "Individual_Institute_Generic_wise_Product_Share_Ranking_of_a_Market_" + "From:" + model.FromDate + "To:" + model.ToDate;
            rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            rd.Close();
            rd.Dispose();
            GC.Collect();
            //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptIndividualDoctorGenericWiseProductShareRankingofTheMarket()
        {
            ViewBag.formTitle = "Individual Doctor Generic wise Product Share Ranking of a Market";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptIndividualDoctorGenericWiseProductShareRankingofTheMarket(ReportModel model)
        {
            DataTable dt;
            ReportDocument rd = new ReportDocument();
            dt = Dalobject.IndividualDoctorGenericWiseProductShareRankingofTheMarket(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/Individual Doctor Generic Wise Product Share Ranking of The Market.rpt";
            rd.Load(ReportPath);
            rd.SetDataSource(dt);
            rd.Refresh();

            rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            rd.SetParameterValue("DEVBY", Session["DEVBY"]);
            rd.SetParameterValue("ProjectName", Session["ProjectName"]);
            rd.SetParameterValue("FromDate", model.FromDate);
            rd.SetParameterValue("ToDate", model.ToDate);
            rd.SetParameterValue("Market", model.MARKET_NAME);

            string downfilename = "Individual_Doctor_Generic_wise_Product_Share_Ranking_of_a_Market_" + "From:" + model.FromDate + "To:" + model.ToDate;
            rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            rd.Close();
            rd.Dispose();
            GC.Collect();
            //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmMarketWiseDoctorValueShare()
        {
            ViewBag.formTitle = "Market Wise Doctor Value Share";
            return View();
        }

        [HttpPost]
        public ActionResult frmMarketWiseDoctorValueShare(ReportModel model)
         {
            if (model.REGION_CODE == "00")
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                        DataTable dt;
                        ReportDocument rd = new ReportDocument();
                        string ReportPath = Server.MapPath("~/Reports");
                        if (model.ReportType == "PDF")
                        {
                            dt = Dalobject.MarketWiseDoctorValueShare(model);
                            ReportPath = ReportPath + "/RptMarketWiseDoctorValueShare.rpt";
                        }
                        else
                        {
                            
                            //dt = Dalobject.MarketWiseDoctorValueShareExcel(model);
                            dt = Dalobject.MarketWiseDoctorValueShare(model);
                            ReportPath = ReportPath + "/RptMarketWiseDoctorValueShareExcel.rpt";
                           
                            
                        }
                        rd.Load(ReportPath);
                        rd.SetDataSource(dt);
                        rd.Refresh();
                        rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        rd.SetParameterValue("DEVBY", Session["DEVBY"]);
                        rd.SetParameterValue("ProjectName", Session["ProjectName"]);
                        rd.SetParameterValue("FromDate", model.FromDate);
                        rd.SetParameterValue("ToDate", model.ToDate);
                        rd.SetParameterValue("Condition", model.ddlCondition);
                        rd.SetParameterValue("SharePerc", model.SharePerc);
                        //string downFileName = "Market_Wise_Doctor_Value_Share" + "From:" + model.FromDate + "To:" + model.ToDate;
                        //if (model.ReportType == "PDF")
                        //{
                        //    rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                        //}

                        //else
                        //{
                        //    rd.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                        //}
                        //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
                      

                        string path = Server.MapPath("~/Market_Wise_Doctor_Value_Share/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        //string downfilename = "Market_Wise_Doctor_Value_Share" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        //var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Doctor_Value_Share"), downfilename);
                        if (model.ReportType == "PDF") {
                            string downfilename = "Market_Wise_Doctor_Value_Share" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                            var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Doctor_Value_Share"), downfilename);
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        }

                        else
                        {
                            string downfilename = "Market_Wise_Doctor_Value_Share" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xls";
                            var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Doctor_Value_Share"), downfilename);
                            rd.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                        }
                            
                        rd.Close();
                        rd.Dispose();
                        GC.Collect();
                    }
                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Market_Wise_Doctor_Value_Share/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Market_Wise_Doctor_Value_Share");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Market_Wise_Doctor_Value_Share");
                    }
                     
                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Market_Wise_Doctor_Value_Share"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }

            else if (model.REGION_CODE == "000")
            {
                DataTable dt;
                ReportDocument rd = new ReportDocument();

                string ReportPath = Server.MapPath("~/Reports");
                if (model.ReportType == "PDF")
                {
                    dt = Dalobject.MarketWiseDoctorValueShare(model);
                    ReportPath = ReportPath + "/RptMarketWiseDoctorValueShare.rpt";
                }
                else
                {
                    
                    //dt = Dalobject.MarketWiseDoctorValueShareExcel(model);
                    dt = Dalobject.MarketWiseDoctorValueShare(model);
                    ReportPath = ReportPath + "/RptMarketWiseDoctorValueShareExcel.rpt";

                }
                rd.Load(ReportPath);
                rd.SetDataSource(dt);
                rd.Refresh();
                rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                rd.SetParameterValue("DEVBY", Session["DEVBY"]);
                rd.SetParameterValue("ProjectName", Session["ProjectName"]);
                rd.SetParameterValue("FromDate", model.FromDate);
                rd.SetParameterValue("ToDate", model.ToDate);
                rd.SetParameterValue("Condition", model.ddlCondition);
                rd.SetParameterValue("SharePerc", model.SharePerc);
                string downFileName = "Market_Wise_Doctor_Value_Share" + "From:" + model.FromDate + "To:" + model.ToDate;
                if (model.ReportType == "PDF")
                {
                    rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                }

                else
                {
                    rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                }
                //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
                rd.Close();
                rd.Dispose();
                GC.Collect();
            }

            else
            {
                DataTable dt;
                ReportDocument rd = new ReportDocument();
                
                string ReportPath = Server.MapPath("~/Reports");
                if (model.ReportType == "PDF")
                {
                    dt = Dalobject.MarketWiseDoctorValueShare(model);
                    ReportPath = ReportPath + "/RptMarketWiseDoctorValueShare.rpt";
                }
                else
                {
                    
                    //dt = Dalobject.MarketWiseDoctorValueShareExcel(model);
                    dt = Dalobject.MarketWiseDoctorValueShare(model);
                    ReportPath = ReportPath + "/RptMarketWiseDoctorValueShareExcel.rpt";
                    
                }
                rd.Load(ReportPath);
                rd.SetDataSource(dt);
                rd.Refresh();
                rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                rd.SetParameterValue("DEVBY", Session["DEVBY"]);
                rd.SetParameterValue("ProjectName", Session["ProjectName"]);
                rd.SetParameterValue("FromDate", model.FromDate);
                rd.SetParameterValue("ToDate", model.ToDate);
                rd.SetParameterValue("Condition", model.ddlCondition);
                rd.SetParameterValue("SharePerc", model.SharePerc);
                string downFileName = "Market_Wise_Doctor_Value_Share" + "From:" + model.FromDate + "To:" + model.ToDate;
                if (model.ReportType == "PDF")
                {
                    rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                }

                else
                {
                    rd.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                }
                //rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
                rd.Close();
                rd.Dispose();
                GC.Collect();
            }
            
            return View();
        }

        public ActionResult frmMarketWiseIndividualDoctorValueShare()
        {
            return View();
        }
        [HttpPost]
        public ActionResult frmMarketWiseIndividualDoctorValueShare(ReportModel model)
        {

            if (model.REGION_CODE == "00")
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT Distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                        ReportDocument rd = new ReportDocument();
                        string ReportPath = Server.MapPath("~/Reports");
                        var dataTableObj = new DataTable();
                        if (model.ReportType == "PDF")
                        {
                            switch (model.ReportName)
                            {
                                case "Market Wise Individua Doctor Value Share":
                                    dataTableObj = Dalobject.MarketWiseIndividualDoctorValueShare(model);
                                    ReportPath = ReportPath + "/RptMarketWiseIndividualDoctorValueShare.rpt";
                                    break;
                                default:
                                    dataTableObj = Dalobject.MarketWiseIndividualInstitutionValueShare(model);
                                    ReportPath = ReportPath + "/RptMarketWiseIndividualInstitutionValueShare.rpt";
                                    break;
                            }
                        }
                        else
                        {
                            switch (model.ReportName)
                            {
                                case "Market Wise Individua Doctor Value Share":
                                    dataTableObj = Dalobject.MarketWiseIndividualDoctorValueShare(model);
                                    ReportPath = ReportPath + "/RptMarketWiseIndividualDoctorValueShare.rpt";
                                    break;
                                default:
                                    dataTableObj = Dalobject.MarketWiseIndividualInstitutionValueShare(model);
                                    ReportPath = ReportPath + "/RptMarketWiseIndividualInstitutionValueShareExcel.rpt";
                                    break;
                            }
                        }

                        rd.Load(ReportPath);
                        rd.SetDataSource(dataTableObj);
                        rd.Refresh();

                        rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        rd.SetParameterValue("DEVBY", Session["DEVBY"]);
                        rd.SetParameterValue("ProjectName", Session["ProjectName"]);
                        rd.SetParameterValue("FromDate", model.FromDate);
                        rd.SetParameterValue("ToDate", model.ToDate);
                        rd.SetParameterValue("Condition", model.ddlCondition);
                        rd.SetParameterValue("NumberOfTran", model.NumberOfTran);

                        string path = Server.MapPath("~/Market_Wise_Institution_Value_Share/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        //string downfilename = "Market_Wise_Doctor_Value_Share" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        //var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Doctor_Value_Share"), downfilename);
                        if (model.ReportType == "PDF")
                        {
                            string downfilename = "Market_Wise_Institution_Value_Share" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                            var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Institution_Value_Share"), downfilename);
                            rd.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        }

                        else
                        {
                            string downfilename = "Market_Wise_Institution_Value_Share" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xls";
                            var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Institution_Value_Share"), downfilename);
                            rd.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                        }

                        rd.Close();
                        rd.Dispose();
                        GC.Collect();
                    }
                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Market_Wise_Institution_Value_Share/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Market_Wise_Institution_Value_Share");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Market_Wise_Institution_Value_Share");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Market_Wise_Institution_Value_Share"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }
            else
            {
                ReportDocument rd = new ReportDocument();
                string ReportPath = Server.MapPath("~/Reports");
                var dataTableObj = new DataTable();
                if (model.ReportType == "PDF")
                {
                    switch (model.ReportName)
                    {
                        case "Market Wise Individua Doctor Value Share":
                            dataTableObj = Dalobject.MarketWiseIndividualDoctorValueShare(model);
                            ReportPath = ReportPath + "/RptMarketWiseIndividualDoctorValueShare.rpt";
                            break;
                        default:
                            dataTableObj = Dalobject.MarketWiseIndividualInstitutionValueShare(model);
                            ReportPath = ReportPath + "/RptMarketWiseIndividualInstitutionValueShare.rpt";
                            break;
                    }
                }
                else
                {
                    switch (model.ReportName)
                    {
                        case "Market Wise Individua Doctor Value Share":
                            dataTableObj = Dalobject.MarketWiseIndividualDoctorValueShare(model);
                            ReportPath = ReportPath + "/RptMarketWiseIndividualDoctorValueShare.rpt";
                            break;
                        default:
                            dataTableObj = Dalobject.MarketWiseIndividualInstitutionValueShare(model);
                            ReportPath = ReportPath + "/RptMarketWiseIndividualInstitutionValueShareExcel.rpt";
                            break;
                    }
                }

                rd.Load(ReportPath);
                rd.SetDataSource(dataTableObj);
                rd.Refresh();

                rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                rd.SetParameterValue("DEVBY", Session["DEVBY"]);
                rd.SetParameterValue("ProjectName", Session["ProjectName"]);
                rd.SetParameterValue("FromDate", model.FromDate);
                rd.SetParameterValue("ToDate", model.ToDate);
                rd.SetParameterValue("Condition", model.ddlCondition);
                rd.SetParameterValue("NumberOfTran", model.NumberOfTran);

                if (model.ReportName == "Market Wise Individua Doctor Value Share")
                {
                    string downFileName = "Market_Wise_Individua_Doctor_Value_Share_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                }
                else
                {
                    string downFileName = "Market_Wise_Individual_lnstitution_Value_Share_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                }
                rd.Close();
                rd.Dispose();
                GC.Collect();
                
            }

            return View();


        }

        public ActionResult frmGroupWiseSelectedInstitutionValueShare()
        {
            ViewBag.formTitle = "Group Wise Selected Institution Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmGroupWiseSelectedInstitutionValueShare(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetGroupWiseSelectedInstitutionValueShareData(model);
            switch (model.ReportName)
            {
                case "Group Wise Selected Institution Value Share By Percentage":
                    rptPath = rptPath + "/Group Wise Selected Institution Value Share By Percentage.rpt";
                    break;
                default:
                    rptPath = rptPath + "/Group Wise Selected Institution Value Share By Value.rpt";
                    break;
            }
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("InsGroup", model.GROUP_NAME);
            if (model.ReportName == "Group Wise Selected Institution Value Share By Percentage")
            {
                string downfilename = "Group_Wise_Selected_Institution_Value_Share_By_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "Group_Wise_Selected_Institution_Value_Share_By_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        public ActionResult frmMarketWiseIndividualInstitutionValueReport()
        {
            ViewBag.formTitle = "Institution Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmMarketWiseIndividualInstitutionValueReport(ReportModel model)
        {
            DataTable dt;
            ReportDocument rd = new ReportDocument();
            string ReportPath = Server.MapPath("~/Reports");
            switch (model.ReportName)
            {
                case "Market wise Individual Institution Value Share by Value":
                    dt = Dalobject.MarketWiseIndividualInstitutionValue(model);
                    ReportPath = ReportPath + "/RptMarketWiseIndividualInstitutionValue(ByValue).rpt";
                    break;
                case "Individual Institution Value Share By Perc":
                    dt = Dalobject.IndividualInstitutionPercShare(model);
                    ReportPath = ReportPath + "/RptIndividualInstitutionValueShare(byPerc).rpt";
                    break;
                case "Individual Institution Value Share By Value":
                    dt = Dalobject.IndividualInstitutionValueShare(model);  
                    ReportPath = ReportPath + "/RptIndividualInstitutionValueShare(byValue).rpt";
                    break;
                default:
                    dt = Dalobject.MarketWiseIndividualInstitutionValuebyPerc(model);
                    ReportPath = ReportPath + "/RptMarketWiseIndividualInstitutionValue(ByPerc).rpt";
                    break;
            }

            rd.Load(ReportPath);
            rd.SetDataSource(dt);
            rd.Refresh();

            rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            rd.SetParameterValue("DEVBY", Session["DEVBY"]);
            rd.SetParameterValue("ProjectName", Session["ProjectName"]);
            rd.SetParameterValue("FromDate", model.FromDate);
            rd.SetParameterValue("ToDate", model.ToDate);
            rd.SetParameterValue("INSTI_CODE", model.INSTI_CODE);
            //rd.SetParameterValue("INSTI_NAME", model.INSTI_NAME);
            //rd.SetParameterValue("Address", model.Address);

            if (string.IsNullOrEmpty(model.INSTI_NAME))
            {
                string Qry = "select INSTI_CODE,INSTI_NAME,full_address(address1,address2, address3, address4) address from INSTITUTION where INSTI_CODE ='" + model.INSTI_CODE + "' ";
                dt = dbHelper.GetDataTable(Qry);
                string sd = dt.Rows[0]["INSTI_NAME"].ToString();
                rd.SetParameterValue("INSTI_NAME", dt.Rows[0]["INSTI_NAME"].ToString());
                //rd.SetParameterValue("Degree", dt.Rows[0]["DEGREE"].ToString());
                //rd.SetParameterValue("Designation", dt.Rows[0]["DESIGNATION"].ToString());
                rd.SetParameterValue("Address", dt.Rows[0]["address"].ToString());
            }

            else
            {
                rd.SetParameterValue("INSTI_NAME", model.INSTI_NAME);
                //rd.SetParameterValue("Degree", model.DEGREE);
                //if (model.DESIGNATION == null)
                //{
                //    string Qry = "select DOCTOR_ID,DOCTOR_NAME,DEGREE,DESIGNATION,full_address(address1,address2, address3, address4) address from doctor where DOCTOR_ID ='" + model.DOCTOR_ID + "' ";
                //    DataTable dt = dbHelper.GetDataTable(Qry);
                //    rd.SetParameterValue("Designation", dt.Rows[0]["DESIGNATION"].ToString());

                //}
                //else
                //{
                //    rd.SetParameterValue("Designation", model.DESIGNATION);
                //}
                rd.SetParameterValue("Address", model.Address);
            }
            if (model.ReportName == "Market wise Individual Institution Value Share by Value")
            {
                string downFileName = "Market_wise_Individual_Institution_Value_Share_by_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else if (model.ReportName == "Individual Institution Value Share By Perc")
            {
                string downFileName = "Individual_Institution_Value_Share_By_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else if (model.ReportName == "Individual Institution Value Share By Value")
            {
                string downFileName = "Individual_Institution_Value_Share_By_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else
            {
                string downFileName = "Market_wise_Individual_Institution_Value_Share_by_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }

            rd.Close();
            rd.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmGroupWiseSelectedDoctorValueShareValue()
        {
            ViewBag.formTitle = "Group Wise Selected Doctor Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmGroupWiseSelectedDoctorValueShareValue(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            //dataTableObj = reportObj.GetReportData(model);
            dataTableObj = reportObj.GetGroupWiseSelectedDoctorValueShareData(model);
            switch (model.ReportName)
            {
                case "Group Wise Selected Doctor Value Share Val":
                    rptPath = rptPath + "/RptGroupWiseSelectedDoctorValueShare(byValue).rpt";
                    break;
                default:
                    rptPath = rptPath + "/RptGroupWiseSelectedDoctorValueShare(byPerc).rpt";
                    break;
            }
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            string GroupName = reportObj.GetGroupName(model.ddlGroup);
            reportDocument.SetParameterValue("GROUP_NAME", GroupName);
            //if (model.ddlGroup == model.GROUP_ID)
            //{
            //    reportDocument.SetParameterValue("GROUP_NAME", model.GROUP_NAME);
            //}
            if (model.ReportName == "Group Wise Selected Doctor Value Share Val")
            {
                string downFileName = "Group_Wise_Selected_Doctor_Value_Share_Val_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else
            {
                string downFileName = "Group_Wise_Selected_Doctor_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptMarketWiseIndividualDoctorValueShare()
        {
            ViewBag.formTitle = "Doctor Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptMarketWiseIndividualDoctorValueShare(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            //ViewBag.USER_ID = Session["USER_ID"].ToString();
            dataTableObj = reportObj.GetReportData(model);

            switch (model.ReportName)
            {

                case "Market Wise Individual Doctor Value Share":
                    rptPath = rptPath + "/Market Wise Individual Doctor Value Share.rpt";
                    break;
                case "Individual Doctor Value Share Percentage":
                    rptPath = rptPath + "/Indivisual Doctor Value Share Percentage.rpt";
                    break;
                case "Individual Doctor Value Share":
                    rptPath = rptPath + "/Individual Doctor Value Share.rpt";
                    break;
                default:
                    rptPath = rptPath + "/Market Wise Individual Doctor Value Share Percentage.rpt";
                    break;
            }                                                                                                                                                                                              
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("DoctorId", model.DOCTOR_ID);

            if (string.IsNullOrEmpty(model.DOCTOR_NAME))
            {
                string Qry = "select DOCTOR_ID,DOCTOR_NAME,DEGREE,DESIGNATION,full_address(address1,address2, address3, address4) address from doctor where DOCTOR_ID ='" + model.DOCTOR_ID + "' ";
                DataTable dt = dbHelper.GetDataTable(Qry);
                string sd = dt.Rows[0]["DOCTOR_NAME"].ToString();
                reportDocument.SetParameterValue("DoctorName", dt.Rows[0]["DOCTOR_NAME"].ToString());
                reportDocument.SetParameterValue("Degree", dt.Rows[0]["DEGREE"].ToString());
                reportDocument.SetParameterValue("Designation", dt.Rows[0]["DESIGNATION"].ToString());
                reportDocument.SetParameterValue("Address", dt.Rows[0]["address"].ToString());
            }
            else
            {
                reportDocument.SetParameterValue("DoctorName", model.DOCTOR_NAME);
                reportDocument.SetParameterValue("Degree", model.DEGREE);
                if(model.DESIGNATION == null)
                {
                    string Qry = "select DOCTOR_ID,DOCTOR_NAME,DEGREE,DESIGNATION,full_address(address1,address2, address3, address4) address from doctor where DOCTOR_ID ='" + model.DOCTOR_ID + "' ";
                    DataTable dt = dbHelper.GetDataTable(Qry);
                    reportDocument.SetParameterValue("Designation", dt.Rows[0]["DESIGNATION"].ToString());

                }
                else
                {
                    reportDocument.SetParameterValue("Designation", model.DESIGNATION);
                }
                reportDocument.SetParameterValue("Address", model.DocAddress);
            }

            if (model.ReportName == "Market Wise Individual Doctor Value Share")
            {
                string downFileName = "Market_Wise_Individual_Doctor_Value_Share_By_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else if (model.ReportName == "Individual Doctor Value Share Percentage")
            {
                string downFileName = "Individual_Doctor_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else if (model.ReportName == "Individual Doctor Value Share")
            {
                string downFileName = "Individual_Doctor_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            else
            {
                string downFileName = "Market_Wise_Individual_Doctor_Value_Share_by_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptProductCollectionStatus()
        {
            ViewBag.formTitle = "Product Collection Status";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptProductCollectionStatus(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);

            rptPath = rptPath + "/RptProductCollectionStatus.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);

            string downFileName = "Product_Collection_Status_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptOofPrescriptionOrgSlipOtcCollectionStatus()
        {
            ViewBag.formTitle = "NO.of Prescription,ORG.Slip,Slip,OTC Collection Status";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptOofPrescriptionOrgSlipOtcCollectionStatus(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);

            rptPath = rptPath + "/RptPreOrgSlpOtcCllSts.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);

            string downFileName = "NO.of_Prescription,_ORG.Slip,_Slip,_OTC_Collection_Status_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptGenericInformation()
        {
            ViewBag.formTitle = "Generic Information";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericInformation(ReportModel model)
        {
            model.ReportName = model.ddlGeneric;
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);

            rptPath = rptPath + "/RptGenericInfo.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("ddlGeneric", model.ddlGeneric);
            string downFileName = "Generic_Information";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptManufacturerInformation()
        {
            ViewBag.formTitle = "Manufacturer Information";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptManufacturerInformation(ReportModel model)
        {
            model.ReportName = model.ddlManufacturer;
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);

            rptPath = rptPath + "/RptManufacturerInfo.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("ddlManufacturer", model.ddlManufacturer);

            string downFileName = "Manufacturer_Information";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptDosageStrengthInformation()
        {
            ViewBag.formTitle = "Dosage Strength Information";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptDosageStrengthInformation(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);

            rptPath = rptPath + "/RptDosageStrength.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);

            string downFileName = "Dosage_Strength_Information";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptDosageFromInformation()
        {
            ViewBag.formTitle = "Dosage From Information";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptDosageFromInformation(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);
            rptPath = rptPath + "/RptDosageForm.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);

            string downFileName = "Dosage_From_Information";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }
        public ActionResult frmRptCategoryWiseNoOfInstitution()
        {
            ViewBag.formTitle = "Category Wise No. Of Institution";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptCategoryWiseNoOfInstitution(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);

            rptPath = rptPath + "/RptCategorywiseInstitute.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);

            string downfilename = "Category_Wise_No._Of_Institution";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptPotentialCategoryWiseNoOfDoctor()
        {
            ViewBag.formTitle = "Potential Category Wise No. Of Doctor";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptPotentialCategoryWiseNoOfDoctor(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);

            rptPath = rptPath + "/RptPotentialCategory.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            string downfilename = "Potential_Category_Wise_No._Of_Doctor";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptDegreeWiseNoOfDoctor()
        {
            ViewBag.formTitle = "Degree Wise No. Of Doctor";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptDegreeWiseNoOfDoctor(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);

            rptPath = rptPath + "/RptDegreeWiseDoctor.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            string downfilename = "Degree_Wise_No._Of_Doctor";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptZoneDivisionRegionTerritoryandMarketLocation()
        {
            ViewBag.formTitle = "Location structure";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptZoneDivisionRegionTerritoryandMarketLocation(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);

            rptPath = rptPath + "/Zone Division Region Territory and Market Location.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);

            string downfilename = "Location_Structure";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptSpecializationwiseDoctorList()
        {
            ViewBag.formTitle = "Specialization Wise Doctor List";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptSpecializationwiseDoctorList(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportData(model);
            if (model.ReportType == "PDF")
            {
                rptPath = rptPath + "/Specialization Wise Doctor List.rpt";
            }
            else
            {
                rptPath = rptPath + "/Specialization Wise Doctor List Excel.rpt";
            }
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            string downfilename = "Specialization_Wise_Doctor_List";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, downfilename);
            return View();
        }

        public ActionResult frmMarketWiseDoctorList()
        {
            ViewBag.formTitle = "Market Wise Doctor List";
            return View();
        }
        [HttpPost]
        public ActionResult frmMarketWiseDoctorList(ReportModel model)
        {

            if (string.IsNullOrEmpty(model.REGION_NAME))
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT Distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                    if (dtRegion.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtRegion.Rows.Count; i++)
                        {
                            model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                            model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                            var reportDocument = new ReportDocument();
                            var rptPath = Server.MapPath("~/Reports");
                            var dataTableObj = new DataTable();
                            var reportObj = new ReportDAO();
                            if (model.ReportType == "PDF")
                            {
                                dataTableObj = reportObj.GetReportDataMarketWiseDoctorList(model);
                                rptPath = rptPath + "/MarketWiseDoctorList.rpt";
                            }
                            else
                            {
                                dataTableObj = reportObj.GetReportDataMarketWiseDoctorList(model);
                                rptPath = rptPath + "/MarketWiseDoctorListExcel.rpt";

                            }
                            reportDocument.Load(rptPath);
                            reportDocument.SetDataSource(dataTableObj);
                            reportDocument.Refresh();

                            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                            
                            string path = Server.MapPath("~/Market_Wise_Doctor_List/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            if (model.ReportType == "PDF")
                            {
                                string downfilename = "Market_Wise_Doctor_List" + model.REGION_NAME + "" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                                var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Doctor_List"), downfilename);
                                reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                            }

                            else
                            {
                                string downfilename = "Market_Wise_Doctor_List" + model.REGION_NAME + "" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xls";
                                var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Doctor_List"), downfilename);
                                reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                            }
                            reportDocument.Close();
                            reportDocument.Dispose();
                            GC.Collect();
                        }
                    }

                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/Market_Wise_Doctor_List/"));
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                        zip.AddDirectoryByName("Market_Wise_Doctor_List");
                        foreach (string filePath in filePaths)
                        {
                            zip.AddFile(filePath, "Market_Wise_Doctor_List");
                        }

                        Response.Clear();
                        Response.BufferOutput = false;
                        string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                        zip.Save(Response.OutputStream);
                        Response.End();

                        foreach (string filePath in filePaths)
                        {
                            // Delete those files
                            var fileName = Path.GetFileName(filePath);
                            var physicalPathDel = Path.Combine(Server.MapPath("~/Market_Wise_Doctor_List"), fileName);
                            if (System.IO.File.Exists(physicalPathDel))
                            {
                                System.IO.File.Delete(physicalPathDel);
                            }
                        }
                    }
            }

            else
            {
                if (model.ReportType == "PDF")
                {
                    var reportDocument = new ReportDocument();
                    var rptPath = Server.MapPath("~/Reports");
                    var dataTableObj = new DataTable();
                    var reportObj = new ReportDAO();
                    dataTableObj = reportObj.GetReportDataMarketWiseDoctorList(model);
                    rptPath = rptPath + "/MarketWiseDoctorList.rpt";
                    //rptPath = rptPath + "/CrystalReport1.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    //reportDocument.SetParameterValue("Region_Name", model.REGION_NAME);

                    string downfilename = "Market_Wise_Doctor_List";
                    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, downfilename);
                    reportDocument.Close();
                    reportDocument.Dispose();
                    GC.Collect();
                }
                else
                {
                    var reportDocument = new ReportDocument();
                    var rptPath = Server.MapPath("~/Reports");
                    var dataTableObj = new DataTable();
                    var reportObj = new ReportDAO();
                    dataTableObj = reportObj.GetReportDataMarketWiseDoctorList(model);
                    rptPath = rptPath + "/MarketWiseDoctorListExcel.rpt";
                    //rptPath = rptPath + "/CrystalReport1.rpt";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    //reportDocument.SetParameterValue("Region_Name", model.REGION_NAME);

                    string downfilename = "Market_Wise_Doctor_List";
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                    reportDocument.Close();
                    reportDocument.Dispose();
                    GC.Collect();
                }

            }

            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmManufacturerWiseProductList()
        {
            ViewBag.formTitle = "Manufacturer Wise Product List";
            return View();
        }
        [HttpPost]
        public ActionResult frmManufacturerWiseProductList(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportDataManufacturerWiseProductList(model);

            rptPath = rptPath + "/Manufacturer Wise Product List.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            if (!string.IsNullOrEmpty(model.MANUFACTURER_NAME))
                reportDocument.SetParameterValue("Manufacturer", model.MANUFACTURER_NAME);
            else
                reportDocument.SetParameterValue("Manufacturer", "ALL Manufacturer");
            string downfilename = "Manufacturer_Wise_Product_List";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        public ActionResult frmInstitutionInformation()
        {
            ViewBag.formTitle = "Institution Information";
            return View();
        }

        [HttpPost]
        public ActionResult frmInstitutionInformation(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetReportDataInstitutionInformation(model);
            if (model.ReportType == "PDF")
            {
                rptPath = rptPath + "/Institutional Information.rpt";
            }
            else
            {
                rptPath = rptPath + "/Institutional Information Excel.rpt";
            }
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);

            string downfilename = "Institution_Information";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptMarketWiseInstitutionList()
        {
            ViewBag.formTitle = "Market Wise Institution List";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptMarketWiseInstitutionList(ReportModel model)
        {

            if (string.IsNullOrEmpty(model.REGION_NAME))
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                        var reportDocument = new ReportDocument();
                        var rptPath = Server.MapPath("~/Reports");
                        var dataTableObj = new DataTable();
                        var reportObj = new ReportDAO();
                        dataTableObj = reportObj.GetReportDataInstitutionList(model);

                        //rptPath = rptPath + "/RptMarketWiseInstitutionList.rpt";
                        if (model.ReportType == "PDF")
                        {
                            rptPath = rptPath + "/RptMarketWiseInstitutionList.rpt";
                        }
                        else
                        {
                            rptPath = rptPath + "/RptMarketWiseInstitutionListExcel.rpt";
                        }
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                       
                        string path = Server.MapPath("~/Market_Wise_Institution_List/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        //string downfilename = "Market_Wise_Institution_List_" + model.REGION_NAME + "" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        //var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Institution_List"), downfilename);
                        //if (model.ReportType == "PDF")
                        //    reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        //else
                        //    reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);


                        if (model.ReportType == "PDF")
                        {
                            string downfilename = "Market_Wise_Institution_List_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                            var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Institution_List"), downfilename);
                            reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        }

                        else
                        {
                            string downfilename = "Market_Wise_Institution_List_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xlsx";
                            var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Institution_List"), downfilename);
                            reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                        }



                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();
                        //string downFileName = "Market_Wise_Institution_List_" + "From:" + model.FromDate + "To:" + model.ToDate;
                        //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                        //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);

                    }

                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Market_Wise_Institution_List/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Market_Wise_Institution_List");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Market_Wise_Institution_List");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Market_Wise_Institution_List"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }

            }

            else
            {
                var reportDocument = new ReportDocument();
                var rptPath = Server.MapPath("~/Reports");
                var dataTableObj = new DataTable();
                var reportObj = new ReportDAO();
                dataTableObj = reportObj.GetReportDataInstitutionList(model);
                if(model.ReportType == "PDF")
                {
                    rptPath = rptPath + "/RptMarketWiseInstitutionList.rpt";
                }
                else
                {
                    rptPath = rptPath + "/RptMarketWiseInstitutionListExcel.rpt";
                }
                reportDocument.Load(rptPath);
                reportDocument.SetDataSource(dataTableObj);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                string downFileName = "Market_Wise_Institution_List_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }

            return View();
        }

        [HttpGet]
        public ActionResult frmIndicationInformation()
        {
            ViewBag.formTitle = "Indication Information";
            return View();
        }

        [HttpPost]
        public ActionResult frmIndicationInformation(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.GetIndicationInfo(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptIndication.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            string downFileName = "Indication_Information";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        [HttpGet]
        public ActionResult frmRptSpecializationAndTherapeuticClassLevel4WiseValueShareRanking13()
        {
            ViewBag.formTitle = "Specialization And Therapeutic (Level-4) Wise Manufacturer Share Ranking";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptSpecializationAndTherapeuticClassLevel4WiseValueShareRanking13(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.SpecializationAndTherapeuticClassLevel4WiseValueShareRanking(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptSpecializationAndTherapeuticClassLevel4WiseValueShareRanking.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("SPECIALIZATION", model.SPECIALIZATION);
            reportDocument.SetParameterValue("TC_L4_DESC", model.TC_L4_DESC);
            reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
            reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);
            string downFileName = "Specialization_And_Therapeutic_(Level-4)_Wise_Manufacturer_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptSpecializationAndGenericWiseValueShareRanking()
        {
            ViewBag.formTitle = "Specialization And Generic Wise Value Share Ranking";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptSpecializationAndGenericWiseValueShareRanking(ReportModel model)
        {
            if (model.REGION_CODE == "00")
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                        DataTable dt;
                        ReportDocument reportDocument = new ReportDocument();
                        dt = Dalobject.SpecializationAndGenericWiseValueShareRanking(model);
                        string ReportPath = Server.MapPath("~/Reports");
                        ReportPath = ReportPath + "/RptSpecializationAndGenericWiseValueShareRanking.rpt";
                        reportDocument.Load(ReportPath);
                        reportDocument.SetDataSource(dt);
                        reportDocument.Refresh();
                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        reportDocument.SetParameterValue("SPECIALIZATION", model.SPECIALIZATION);
                        reportDocument.SetParameterValue("GENERIC_NAME", model.GENERIC_NAME);
                        reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
                        reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);                       
                        string path = Server.MapPath("~/Specialization_And_Generic_Wise_Value_Share_Ranking/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string downfilename = "Specialization_And_Generic_Wise_Value_Share_Ranking_" + model.REGION_NAME + "" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        var physicalPath = Path.Combine(Server.MapPath("~/Specialization_And_Generic_Wise_Value_Share_Ranking"), downfilename);
                        if (model.ReportType == "PDF")
                            reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        else
                            reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);

                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();
                    }
                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Specialization_And_Generic_Wise_Value_Share_Ranking/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Specialization_And_Generic_Wise_Value_Share_Ranking");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Specialization_And_Generic_Wise_Value_Share_Ranking");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Specialization_And_Generic_Wise_Value_Share_Ranking"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }

            else if (model.REGION_CODE == "000")
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.SpecializationAndGenericWiseValueShareRanking(model);
                string ReportPath = Server.MapPath("~/Reports");
                ReportPath = ReportPath + "/RptSpecializationAndGenericWiseValueShareRanking.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();
                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                reportDocument.SetParameterValue("SPECIALIZATION", model.SPECIALIZATION);
                reportDocument.SetParameterValue("GENERIC_NAME", model.GENERIC_NAME);
                reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
                reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);
                string downFileName = "Specialization_And_Generic_Wise_Value_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }
            else
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.SpecializationAndGenericWiseValueShareRanking(model);
                string ReportPath = Server.MapPath("~/Reports");
                ReportPath = ReportPath + "/RptSpecializationAndGenericWiseValueShareRanking.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();
                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                reportDocument.SetParameterValue("SPECIALIZATION", model.SPECIALIZATION);
                reportDocument.SetParameterValue("GENERIC_NAME", model.GENERIC_NAME);
                reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
                reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);
                string downFileName = "Specialization_And_Generic_Wise_Value_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }

            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        public ActionResult frmRptPrescriptionWiseNumberProduct()
        {
            ViewBag.formTitle = "Prescription Wise Number Product";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptPrescriptionWiseNumberProduct(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.PrescriptionWiseNumberProduct(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptPrescriptionWiseNumberProduct.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            if (model.MANUFACTURER_CODE == "SQA")
            {
                reportDocument.SetParameterValue("SQA", "SQUARE PHARMACEUTICAL LTD");
            }
            else
            {
                reportDocument.SetParameterValue("SQA", model.MANUFACTURER_NAME);
            }

            string downFileName = "Prescription_Wise_Number_Product_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptTherapeuticClassLevelwiseNationalValueShareContribution()
        {
            ViewBag.formTitle = "Therapeutic Class Level wise National Value Share Contribution";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptTherapeuticClassLevelwiseNationalValueShareContribution(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.TherapeuticClassLevelwiseNationalValueShareContribution(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptTherapeuticClassLevelwiseNationalValueShareContribution.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("ddlTherapeutic", model.ddlTherapeutic);
            //reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
            //reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);
            if (model.ddlPresCategory == "0")
                reportDocument.SetParameterValue("ddlPresCategory", "All");
            else
            {
                if (model.ddlPresCategory == "P")
                    reportDocument.SetParameterValue("ddlPresCategory", "Prescription");
                else if (model.ddlPresCategory == "O")
                    reportDocument.SetParameterValue("ddlPresCategory", "Orgranization");
                else if (model.ddlPresCategory == "S")
                    reportDocument.SetParameterValue("ddlPresCategory", "Slip");
                else if (model.ddlPresCategory == "T")
                    reportDocument.SetParameterValue("ddlPresCategory", "OTC");
            }

            string downFileName = "Therapeutic_Class_Level_wise_National_Value_Share_Contribution_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        public ActionResult frmRptManufacturerContributiontoNationalShare()
        {
            ViewBag.formTitle = "Manufacturer Contribution to National Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptManufacturerContributiontoNationalShare(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.ManufacturerContributiontoNationalShare(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptManufacturerContributiontoNationalShare.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            // reportDocument.SetParameterValue("TC_L3_DESC", model.TC_L3_DESC);
            // reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
            reportDocument.SetParameterValue("ddlManufacturer", model.ddlManufacturer);


            string downFileName = "Manufacturer_Contribution_to_National_Share_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        public ActionResult frmRptGenericWiseIndividualInstitutionValueSharePerc()
        {
            ViewBag.formTitle = "Generic Wise Individual Institution Value Share";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptGenericWiseIndividualInstitutionValueSharePerc(ReportModel model)
        {
            ReportDocument rd = new ReportDocument();
            string ReportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            switch (model.ReportName)
            {
                case "Generic Wise Individual Institution Value Share (By %)":
                    dataTableObj = Dalobject.GenericWiseIndividualInstitutionValueSharePerc(model);
                    ReportPath = ReportPath + "/RptGenericWiseIndividualInstitutionValueSharePerc.rpt";
                    break;
                default:
                    dataTableObj = Dalobject.GenericWiseIndividualInstitutionValueShareValue(model);
                    ReportPath = ReportPath + "/RptGenericWiseIndividualInstitutionValueShareValue.rpt";
                    break;
            }

            rd.Load(ReportPath);
            rd.SetDataSource(dataTableObj);
            rd.Refresh();

            rd.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            rd.SetParameterValue("DEVBY", Session["DEVBY"]);
            rd.SetParameterValue("ProjectName", Session["ProjectName"]);
            rd.SetParameterValue("FromDate", model.FromDate);
            rd.SetParameterValue("ToDate", model.ToDate);
            rd.SetParameterValue("INSTI_CODE", model.INSTI_CODE);


            rd.SetParameterValue("INSTI_NAME", model.INSTI_NAME);
            rd.SetParameterValue("Address", model.Address);
            if (model.ReportName == "Generic Wise Individual Institution Value Share (By %)")
            {
                string downFileNamePer = "Generic_Wise_Individual_Institution_Value_Share_(By %)_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileNamePer);
            }
            else
            {
                string downFileNameValue = "Generic_Wise_Individual_Institution_Value_Share_(By Value)_" + "From:" + model.FromDate + "To:" + model.ToDate;
                rd.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileNameValue);
            }

            rd.Close();
            rd.Dispose();
            GC.Collect();
            return View();
        }
        public ActionResult frmRptProductFrequencyInPrescription()
        {
            ViewBag.formTitle = "Product Frequency in Prescription";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptProductFrequencyInPrescription(ReportModel model)
        {
            if (model.REGION_CODE == "00")
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                        DataTable dt;
                        ReportDocument reportDocument = new ReportDocument();
                        dt = Dalobject.ProductFrequencyInPrescription(model);
                        string ReportPath = Server.MapPath("~/Reports");
                        ReportPath = ReportPath + "/RptProductFrequencyInPrescription.rpt";
                        reportDocument.Load(ReportPath);
                        reportDocument.SetDataSource(dt);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        // reportDocument.SetParameterValue("TC_L3_DESC", model.TC_L3_DESC);
                        // reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
                        reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);
                       
                        string path = Server.MapPath("~/Product_Frequency_in_Prescription/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string downfilename = "Product_Frequency_in_Prescription_" + model.REGION_NAME + "" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        var physicalPath = Path.Combine(Server.MapPath("~/Product_Frequency_in_Prescription"), downfilename);
                        if (model.ReportType == "PDF")
                            reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        else
                            reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();

                    }

                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Product_Frequency_in_Prescription/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Product_Frequency_in_Prescription");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Product_Frequency_in_Prescription");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Product_Frequency_in_Prescription"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }

            else if (model.REGION_CODE == "000")
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.ProductFrequencyInPrescription(model);
                string ReportPath = Server.MapPath("~/Reports");
                ReportPath = ReportPath + "/RptProductFrequencyInPrescription.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                // reportDocument.SetParameterValue("TC_L3_DESC", model.TC_L3_DESC);
                // reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
                reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);

                string downFileName = "Product_Frequency_in_Prescription_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }

            else
            {
                DataTable dt;
                ReportDocument reportDocument = new ReportDocument();
                dt = Dalobject.ProductFrequencyInPrescription(model);
                string ReportPath = Server.MapPath("~/Reports");
                ReportPath = ReportPath + "/RptProductFrequencyInPrescription.rpt";
                reportDocument.Load(ReportPath);
                reportDocument.SetDataSource(dt);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                // reportDocument.SetParameterValue("TC_L3_DESC", model.TC_L3_DESC);
                // reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
                reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);

                string downFileName = "Product_Frequency_in_Prescription_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFileName);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }

            return View();
        }

        public ActionResult frmRptSpecializationAndTherapeuticLevelThreeWiseManufacturerShareRanking()
        {
            ViewBag.formTitle = "Specialization And Therapeutic (Level-3) Wise Manufacturer Share Ranking";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptSpecializationAndTherapeuticLevelThreeWiseManufacturerShareRanking(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.SpecializationAndTherapeuticLevelThreeWiseManufacturerShareRanking(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptSpecializationAndTherapeuticLevel-3WiseManufacturerShareRanking.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("TC_L3_DESC", model.TC_L3_DESC);
            reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
            reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);

            string downFileName = "Specialization_And_Therapeutic_(Level-3)_Wise_Manufacturer_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        public ActionResult frmRptSpecializationAndTherapeuticLevelTwoWiseManufacturerShareRanking()
        {
            ViewBag.formTitle = "Specialization And Therapeutic (Level-2) Wise Manufacturer Share Ranking";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptSpecializationAndTherapeuticLevelTwoWiseManufacturerShareRanking(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.SpecializationAndTherapeuticLevelTwoWiseManufacturerShareRanking(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptSpecializationAndTherapeuticLevel-2WiseManufacturerShareRanking.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("TC_L2_DESC", model.TC_L2_DESC);
            reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
            reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);
            //if (string.IsNullOrEmpty(model.MARKET_NAME))
            //    reportDocument.SetParameterValue("market", "All Territory");
            //else
            //    reportDocument.SetParameterValue("market", model.MARKET_NAME);
            //if (string.IsNullOrEmpty(model.GENERIC_NAME))
            //    reportDocument.SetParameterValue("generic", "All Generic");
            //else
            //    reportDocument.SetParameterValue("generic", model.GENERIC_NAME);
            string downFileName = "Specialization_And_Therapeutic_(Level-2)_Wise_Manufacturer_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        public ActionResult frmRptSpecializationAndTherapeuticLevelOneWiseManufacturerShareRanking()
        {
            ViewBag.formTitle = "Specialization And Therapeutic (Level-1) Wise Manufacturer Share Ranking";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptSpecializationAndTherapeuticLevelOneWiseManufacturerShareRanking(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.SpecializationAndTherapeuticLevelOneWiseManufacturerShareRanking(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptSpecializationAndTherapeuticLevel-1WiseManufacturerShareRanking.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            reportDocument.SetParameterValue("TC_L1_DESC", model.TC_L1_DESC);
            reportDocument.SetParameterValue("DOSAGE_FORM_NAME", model.DOSAGE_FORM_NAME);
            reportDocument.SetParameterValue("REGION_NAME", model.REGION_NAME);
            //if (string.IsNullOrEmpty(model.MARKET_NAME))
            //    reportDocument.SetParameterValue("market", "All Territory");
            //else
            //    reportDocument.SetParameterValue("market", model.MARKET_NAME);
            //if (string.IsNullOrEmpty(model.GENERIC_NAME))
            //    reportDocument.SetParameterValue("generic", "All Generic");
            //else
            //    reportDocument.SetParameterValue("generic", model.GENERIC_NAME);
            string downFileName = "Specialization_And_Therapeutic_(Level-1)_Wise_Manufacturer_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        // Method///
        public ActionResult GetRegionRankin()
        {
            var data = Dalobject.GetRegionRankinList();
            var RegionList = Json(data, JsonRequestBehavior.AllowGet);
            RegionList.MaxJsonLength = int.MaxValue;
            return RegionList;
        }

        [HttpGet]
        public JsonResult GetGeneric()
        {
            var data = Dalobject.GetAllGenericInfo();
            var allGeneric = Json(data, JsonRequestBehavior.AllowGet);
            allGeneric.MaxJsonLength = int.MaxValue;
            return allGeneric;
            //List<ReportModel> allGeneric = Dalobject.GetAllGenericInfo();
            //return Json(allGeneric, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPrescriptionCategory()
        {
            List<ReportModel> allPrescriptionCategory = Dalobject.GetAllPrescriptionCategory();
            return Json(allPrescriptionCategory, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataSource()
        {
            List<ReportModel> allGeneric = Dalobject.GetAllDataSource();
            return Json(allGeneric, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDesignationList()
        {
            var zoneList = Dalobject.GetDesignationList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSpecializationList()
        {
            var SpecializationList = Dalobject.GetSpecializationList();
            return Json(SpecializationList, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDegreeList()
        {
            var zoneList = Dalobject.GetDegreeList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDivisionList()
        {
            var zoneList = Dalobject.GetDivisionList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetRegionList()
        {
            var zoneList = Dalobject.GetRegionList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetReportRegionList()
        {
            var zoneList = Dalobject.GetReportRegionList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetGenericList()
        {
            var genericList = Dalobject.GetGenericList();
            var data = Json(genericList, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
            
            //return Json(genericList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetManufacturerList()
        {
            var zoneList = Dalobject.GetManufacturerList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetManufacturerListForReport()
        {
            var zoneList = Dalobject.GetManufacturerListForReport();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetInstitutionTypeList()
        {
            var zoneList = Dalobject.GetInstitutionTypeList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetDoctorList()
        {
            var data = Dalobject.GetDoctorList();
            var doctorList = Json(data, JsonRequestBehavior.AllowGet);
            doctorList.MaxJsonLength = int.MaxValue;
            return doctorList;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMarketList()
        {
            var data = Dalobject.GetAllMarketInfo();
            var marketList = Json(data, JsonRequestBehavior.AllowGet);
            marketList.MaxJsonLength = int.MaxValue;
            return marketList;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMarketAgainstRegionList(string region)
        {
            var data = Dalobject.GetMarketAgainstRegionList(region);
            var marketList = Json(data, JsonRequestBehavior.AllowGet);
            marketList.MaxJsonLength = int.MaxValue;
            return marketList;
        }
        public ActionResult GetTherapeuticOneList()
        {
            var TherapeuticList = Dalobject.GetTherapeuticOneList();
            return Json(TherapeuticList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTherapeuticTwoList()
        {
            var TherapeuticList = Dalobject.GetTherapeuticTwoList();
            return Json(TherapeuticList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTherapeuticThreeList()
        {
            var TherapeuticList = Dalobject.GetTherapeuticThreeList();
            return Json(TherapeuticList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTherapeuticFourList()
        {
            var TherapeuticList = Dalobject.GetTherapeuticFourList();
            return Json(TherapeuticList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllGeneric()
        {
            var data = Dalobject.GetAllGeneric();
            var genericList = Json(data, JsonRequestBehavior.AllowGet);
            genericList.MaxJsonLength = int.MaxValue;
            return genericList;
        }
        public ActionResult GetDosageFormList()
        {
            var DosageFormList = Dalobject.GetDosageFormList();
            return Json(DosageFormList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDosageStrengthList()
        {
            var DosageStrengthList = Dalobject.GetDosageStrengthList();
            return Json(DosageStrengthList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllSpecializationList()
        {
            var SpecializationList = Dalobject.GetAllSpecializationList();
            return Json(SpecializationList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ManufacturerList()
        {
            var zoneList = Dalobject.ManufacturerList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        ////////////////////////

        [HttpGet]
        public JsonResult GetDoctorGroup()
        {
            var data = Dalobject.GetAllGroup().OrderBy(m => m.GROUP_ID);
            var doctorGroupList = Json(data, JsonRequestBehavior.AllowGet);
            doctorGroupList.MaxJsonLength = int.MaxValue;
            return doctorGroupList;

        }

        [HttpGet]
        public JsonResult GetInstituteGroup()
        {
            var data = Dalobject.GetInstituteGroup().OrderBy(m => m.GROUP_ID);
            var doctorGroupList = Json(data, JsonRequestBehavior.AllowGet);
            doctorGroupList.MaxJsonLength = int.MaxValue;
            return doctorGroupList;

        }

        [HttpPost]
        public JsonResult GetInstitute(string InstiCode)
        {
            if (string.IsNullOrEmpty(InstiCode))
            {
                var data = Dalobject.GetAllInstitute(InstiCode).OrderBy(m => m.INSTI_CODE);
                var instituteDetailList = Json(data, JsonRequestBehavior.AllowGet);
                instituteDetailList.MaxJsonLength = int.MaxValue;
                return instituteDetailList;
            }
            else
            {
                var data = Dalobject.GetAllInstitute(InstiCode).OrderBy(m => m.INSTI_CODE);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
           
        }

        //[HttpPost]
        //public JsonResult GetInstituteInfo(string InstiCode)
        //{
        //    var data = Dalobject.GetInstitute(InstiCode);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}


        [HttpGet]
        public JsonResult GetRegion()
        {
            List<ReportModel> allRegion = Dalobject.GetAllRegionInfo();
            return Json(allRegion, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRegionNAR()
        {
            List<ReportModel> allRegion = Dalobject.GetRegionNAR();
            return Json(allRegion, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetTerritory()
        {
            List<ReportModel> allTerritory = Dalobject.GetAllTerritoryInfo();
            return Json(allTerritory, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetTerritoryList()
        {
            List<ReportModel> allTerritory = Dalobject.GetTerritoryList();
            return Json(allTerritory, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMarket()
        {
            var data = Dalobject.GetAllMarketInfo();
            var allMarket = Json(data, JsonRequestBehavior.AllowGet);
            allMarket.MaxJsonLength = int.MaxValue;
            return allMarket;
            //List<ReportModel> allMarket = Dalobject.GetAllMarketInfo();
            //return Json(allMarket, JsonRequestBehavior.AllowGet);
        }

        ////// END /////////////////




        // *************************************** ALL JIBON'S REPORT *************************************************//

        #region Therapeutic Class Level Report
        public ActionResult frmRptTherapeuticClassLevel1Information()
        {
            ViewBag.formTitle = "Therapeutic Class Level Reports";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptTherapeuticClassLevel1Information(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.TherapeuticClassLevelReport(model);


            switch (model.ReportName)
            {
                case "TherapeuticClassLevel1":
                    reportPath = reportPath + "/RptTherapeuticClassLevel1.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    break;
                case "TherapeuticClassLevel2":
                    reportPath = reportPath + "/RptTherapeuticClassLevel2.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    break;
                case "TherapeuticClassLevel3":
                    reportPath = reportPath + "/RptTherapeuticClassLevel3.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    break;
                case "TherapeuticClassLevel4":
                    reportPath = reportPath + "/RptTherapeuticClassLevel4.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    break;
            }

            if (model.ReportName == "TherapeuticClassLevel1")
            {
                string downfilename = "TherapeuticClassLevel1";
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else if (model.ReportName == "TherapeuticClassLevel2")
            {
                string downfilename = "TherapeuticClassLevel2" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else if (model.ReportName == "TherapeuticClassLevel3")
            {
                string downfilename = "TherapeuticClassLevel3" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "TherapeuticClassLevel14" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        #endregion

        #region Generic Wise Product Report
        public ActionResult frmRptGenericWiseProductList()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Generic Wise Product Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptGenericWiseProductList(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GenericWiseProductList(model);

            rptPath = rptPath + "/frmRptGenericWiseProductList.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            //string df = Session["COMPANY_NAME"].ToString();
            //string df1 = Session["DEVBY"].ToString();
            //string df2 = Session["ProjectName"].ToString();
            //string df3 = model.GENERIC_NAME;
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            if (!string.IsNullOrEmpty(model.GENERIC_NAME))
                reportDocument.SetParameterValue("Generic", model.GENERIC_NAME);
            else
                reportDocument.SetParameterValue("Generic", "All Generic");

            string downfilename = "Generic_Wise_Product_Report";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }
        #endregion

        #region Generic Wise Product Detail Report
        public ActionResult frmRptGenericWiseDetailProductRPT()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Generic Wise Product Detail Report";
                return View();
            }
            return RedirectToAction("Login", "Home");

        }
        [HttpPost]
        public ActionResult frmRptGenericWiseDetailProductRPT(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GenericWiseDetailProductList(model);
            rptPath = rptPath + "/RptGenericWiseDetailProduct.rpt";//Change report name here
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            if (!string.IsNullOrEmpty(model.GENERIC_NAME))
                reportDocument.SetParameterValue("Generic", model.GENERIC_NAME);
            else
                reportDocument.SetParameterValue("Generic", "All Generic");

            string downfilename = "Generic_Wise_Product_Detail_Report";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");

            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();//Change report name here
        }
        #endregion

        #region Manufecturer Wise Product Detail Report
        public ActionResult frmRptManufacturerWiseDetailProductList()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Manufacturer Wise Product Detail Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptManufacturerWiseDetailProductList(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.ManufacturerWiseDetailProductList(model);
            rptPath = rptPath + "/RptManufecturerWiseDetailProduct.rpt";//Change report name here
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            string df = Session["COMPANY_NAME"].ToString();
            string df1 = Session["DEVBY"].ToString();
            string df2 = Session["ProjectName"].ToString();
            string df3 = model.MANUFACTURER_NAME;
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            if (!string.IsNullOrEmpty(model.MANUFACTURER_NAME))
                reportDocument.SetParameterValue("Manufacturer", model.MANUFACTURER_NAME);
            else
                reportDocument.SetParameterValue("Manufacturer", "All Manufacturer");

            string downfilename = "Manufacturer_Wise_Product_Detail_Report";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        #endregion

        #region Value Share Report for National/Division/Zone/Region/Market/Territory
        public ActionResult frmRptLocationWiseValueShare()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.DataSourceDropDownList = Dalobject.DataSourceListDropDown();
                ViewBag.formTitle = "LOCATION WISE VALUE SHARE REPORT";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]

        public ActionResult frmRptLocationWiseValueShare(ReportModel model)
        {
            ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            string day = DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            string rptName = day + month + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            if (model.ReportType == "PDF")
            {
                dataTableObj = reportObj.GetLocationWiseValueShareData(model);
            }
            else
            {
                dataTableObj = reportObj.GetLocationWiseValueShareExcelData(model); 
            }
            //foreach (DataRow row in dataTableObj.Rows)
            //{
            //    string file = row.Field<string>("National_pct");
            //}
            if (model.ReportType == "PDF")
            {
                switch (model.ReportName)
                {
                    case "NationalValueShare":
                        reportPath = reportPath + "/RptNationalValueShare.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                    case "NationalValueShareValue":
                        reportPath = reportPath + "/RptNationalValueShareValue.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                    case "ZoneValueSharePersent":
                        reportPath = reportPath + "/RptZoneValueSharePersent.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                    case "ZoneValueShareValue":
                        reportPath = reportPath + "/RptZoneValueShareValue.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                    case "DivisionValueSharePersent":
                        reportPath = reportPath + "/RptDivisionValueSharePersent.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                    case "DivisionValueShareValue":
                        reportPath = reportPath + "/RptDivisionValueShareValue.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                    case "RegionValueSharePersent":
                        reportPath = reportPath + "/RptRegionValueSharePersent.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                    case "RegionValueShareValue":
                        reportPath = reportPath + "/RptRegionValueShareValue.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;


                    case "TerritoryValueSharePersent":
                        reportPath = reportPath + "/RptTerritoryValueSharePersent.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                    case "TerritoryValueShareValue":
                        reportPath = reportPath + "/RptTerritoryValueShareValue.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                    case "MarketValueSharePersent":
                        reportPath = reportPath + "/RptMarketValueSharePersent.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                    case "MarketValueShareValue":
                        reportPath = reportPath + "/RptMarketValueShareValue.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                    case "DivisionRegionTerritoryMarketValueShare":
                        reportPath = reportPath + "/RptDivisionRegionTerritoryMarketValueShare.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                    case "DivisionRegionTerritoryMarketValueSharebyValue":
                        reportPath = reportPath + "/RptDivisionRegionTerritoryMarketValueSharebyValue.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                }
            }

            else
            {
                switch (model.ReportName)
                {
                    case "NationalValueShare":
                        reportPath = reportPath + "/RptNationalValueShareExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();    
                        break;

                    case "NationalValueShareValue":
                        reportPath = reportPath + "/RptNationalValueShareValueExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        //reportDocument.SetParameterValue("FromDate", model.FromDate);
                        //reportDocument.SetParameterValue("ToDate", model.ToDate);
                        break;

                    case "DivisionValueSharePersent":
                        reportPath = reportPath + "/RptDivisionValueSharePersentExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        //reportDocument.SetParameterValue("FromDate", model.FromDate);
                        //reportDocument.SetParameterValue("ToDate", model.ToDate);
                        break;

                    case "DivisionValueShareValue":
                        reportPath = reportPath + "/RptDivisionValueShareValueExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        //reportDocument.SetParameterValue("FromDate", model.FromDate);
                        //reportDocument.SetParameterValue("ToDate", model.ToDate); 
                        break;

                    case "RegionValueSharePersent":
                        reportPath = reportPath + "/RptRegionValueSharePersentExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        break;

                    case "RegionValueShareValue":
                        reportPath = reportPath + "/RptRegionValueShareValueExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        //reportDocument.SetParameterValue("FromDate", model.FromDate);
                        //reportDocument.SetParameterValue("ToDate", model.ToDate);
                        break;

                    case "TerritoryValueSharePersent":
                        reportPath = reportPath + "/RptTerritoryValueSharePersentExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        break;

                    case "TerritoryValueShareValue":
                        reportPath = reportPath + "/RptTerritoryValueShareValueExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        //reportDocument.SetParameterValue("FromDate", model.FromDate);
                        //reportDocument.SetParameterValue("ToDate", model.ToDate);
                        break;

                    case "MarketValueSharePersent":
                        reportPath = reportPath + "/RptMarketValueSharePersentExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        //reportDocument.SetParameterValue("FromDate", model.FromDate);
                        //reportDocument.SetParameterValue("ToDate", model.ToDate);
                        break;

                    case "MarketValueShareValue":
                        reportPath = reportPath + "/RptMarketValueShareValueExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();
                        //reportDocument.SetParameterValue("FromDate", model.FromDate);
                        //reportDocument.SetParameterValue("ToDate", model.ToDate);
                        break;

                    case "MarketValueShareValuePMS":
                        rptName = "Market_Value_Share_" + model.FromDate + "_To_" + model.ToDate + "_PMS";
                        export.ExportToExcel2(dataTableObj, rptName);
                   //DataTable dt = new DataTable();
                   //reportDocument = new ReportDocument();
                   //string ReportPath = Server.MapPath("~/Reports");
                   //rptName = "Market_Value_Share_" + model.FromDate + "_" + model.ToDate + "_PMS";
                   //dt = Dalobject.GetHRMarketShareValueExcelData(model);
                   //ReportPath = ReportPath + "/RptMarketValueShareValuePMSExcel.rpt";
                   //reportDocument.Load(ReportPath);
                   //reportDocument.SetDataSource(dt);
                   //reportDocument.Refresh();
                    break;

                    case "DivisionRegionTerritoryMarketValueShare":
                        reportPath = reportPath + "/RptDivisionRegionTerritoryMarketValueShareExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                    case "DivisionRegionTerritoryMarketValueSharebyValue":
                        reportPath = reportPath + "/RptDivisionRegionTerritoryMarketValueSharebyValueExcel.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                        {
                            if (model.PRESC_CATE_CODE == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.PRESC_CATE_CODE == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.PRESC_CATE_CODE == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.PRESC_CATE_CODE == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.PRESC_CATE_CODE == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                }
                
            }
            

            if (model.ReportName == "NationalValueShare")
            {   
                if( model.ReportType == "PDF")
                {
                    string downfilename = "National_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                
                else
                {
                    string downFilename = "National_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFilename);
                }
            }
            else if (model.ReportName == "NationalValueShareValue")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "National_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }

                else
                {
                    string downFilename = "National_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFilename);
                }

                
            }
            else if (model.ReportName == "ZoneValueSharePersent")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "Zone_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }

                else
                {
                    string downFilename = "Zone_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFilename);
                }
                
            }
            else if (model.ReportName == "ZoneValueShareValue")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "Zone_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }

                else
                {
                    string downfilename = "Zone_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
                }
               
            }
            else if (model.ReportName == "DivisionValueSharePersent")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "Division_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }

                else
                {
                    string downfilename = "Division_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
                    
                }
                
            }
            else if (model.ReportName == "DivisionValueShareValue")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "Division_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }

                else
                {
                    string downfilename = "Division_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                

            }
            else if (model.ReportName == "RegionValueSharePersent")
            {
                if(model.ReportType == "PDF")
                {
                    string downfilename = "Region_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                else
                {
                    string downFilename = "Region_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFilename);
                }
                
            }
            else if (model.ReportName == "RegionValueShareValue")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "Region_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }

                else
                {
                    string downfilename = "Region_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                
            }
            else if (model.ReportName == "TerritoryValueSharePersent")
            {
                if(model.ReportType == "PDF")
                {
                    string downfilename = "Territory_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                else
                {
                    string downFilename = "Territory_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFilename);
                }
            }
            else if (model.ReportName == "TerritoryValueShareValue")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "Territory_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }

                else
                {
                    string downFilename = "Territory_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFilename);
                }
               
            }
            else if (model.ReportName == "MarketValueSharePersent")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "Market_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                else
                {
                    string downFilename = "Market_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFilename);
                }

                //string downfilename = "Market_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else if (model.ReportName == "MarketValueShareValue")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "Market_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }

                else
                {
                    string downfilename = "Market_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
                }
               
            }
            //else if (model.ReportName == "MarketValueShareValuePMS")
            //{
            //    if (model.ReportType == "PDF")
            //    {
            //        string downfilename = "Market_Value_Share_Value_PMS" + "From:" + model.FromDate + "To:" + model.ToDate;
            //        reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            //    }

            //    else
            //    {
            //        string downfilename = "Market_Value_Share_Value_PMS" + "From:" + model.FromDate + "To:" + model.ToDate;
            //        reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            //    }

            //}
            else if (model.ReportName == "DivisionRegionTerritoryMarketValueShare")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "Division_Region_Territory_Market_Value_Share_perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }

                else
                {

                    string downFilename = "Division_Region_Territory_Market_Value_Share_perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downFilename);
                    //string downfilename = "Division_Region_Territory_Market_Value_Share_perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    //reportDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                
            }
            else if (model.ReportName == "DivisionRegionTerritoryMarketValueSharebyValue")
            {
                if (model.ReportType == "PDF")
                {
                    string downfilename = "Division_Region_Territory_Market_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }

                else
                {
                    string downfilename = "Division_Region_Territory_Market_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
                //MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }


        //    public ActionResult frmRptLocationWiseValueShare(ReportModel model)
        //    {
        //        ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();

        //      if (model.REGION_CODE != null)
        //      {
        //        if (model.REGION_CODE == "00")
        //        {

        //             DataTable dtRegion = dbHelper.GetDataTable("SELECT REGION_CODE,REGION_NAME FROM REGION ORDER BY REGION_CODE");
        //             if (dtRegion.Rows.Count > 0)
        //             {
        //                 for (int i = 0; i < dtRegion.Rows.Count; i++)
        //                 {
        //                     model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
        //                     model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

        //                     var reportDocument = new ReportDocument();
        //                     var reportPath = Server.MapPath("~/Reports");
        //                     var dataTableObj = new DataTable();
        //                     var reportObj = new ReportDAO();
        //                     dataTableObj = reportObj.GetLocationWiseValueShareRegionTerritoryMarketValueShare(model);

        //                     switch (model.ReportName)
        //                     {
        //                         case "DivisionRegionTerritoryMarketValueShare":
        //                             reportPath = reportPath + "/RptDivisionRegionTerritoryMarketValueShare.rpt";
        //                             reportDocument.Load(reportPath);
        //                             reportDocument.SetDataSource(dataTableObj);
        //                             reportDocument.Refresh();
        //                             reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                             reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                             reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                             reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                             reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                             if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                             {
        //                                 if (model.PRESC_CATE_CODE == "P")
        //                                     reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                                 else if (model.PRESC_CATE_CODE == "T")
        //                                     reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                                 else if (model.PRESC_CATE_CODE == "O")
        //                                     reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                                 else if (model.PRESC_CATE_CODE == "S")
        //                                     reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                                 else if (model.PRESC_CATE_CODE == "0")
        //                                     reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                             }
        //                             break;
        //                         case "DivisionRegionTerritoryMarketValueSharebyValue":
        //                             reportPath = reportPath + "/RptDivisionRegionTerritoryMarketValueSharebyValue.rpt";
        //                             reportDocument.Load(reportPath);
        //                             reportDocument.SetDataSource(dataTableObj);
        //                             reportDocument.Refresh();
        //                             reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                             reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                             reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                             reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                             reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                             if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                             {
        //                                 if (model.PRESC_CATE_CODE == "P")
        //                                     reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                                 else if (model.PRESC_CATE_CODE == "T")
        //                                     reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                                 else if (model.PRESC_CATE_CODE == "O")
        //                                     reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                                 else if (model.PRESC_CATE_CODE == "S")
        //                                     reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                                 else if (model.PRESC_CATE_CODE == "0")
        //                                     reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                             }
        //                             break;
        //                     }

        //                     string path = Server.MapPath("~/Sheets/");
        //                     if (!Directory.Exists(path))
        //                     {
        //                         Directory.CreateDirectory(path);
        //                     }

        //                     if (model.ReportName == "DivisionRegionTerritoryMarketValueShare")
        //                     {

        //                         string downfilename = "Division_Region_Territory_Market_Value_Share_perc_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
        //                         var physicalPath = Path.Combine(Server.MapPath("~/Sheets"), downfilename);
        //                         if (model.ReportType == "PDF")
        //                             reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
        //                         else
        //                             reportDocument.ExportToDisk(ExportFormatType.ExcelRecord, physicalPath);

        //                     }
        //                     else
        //                     {
        //                         string downfilename = "Division_Region_Territory_Market_Value_Share_Value_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
        //                         var physicalPath = Path.Combine(Server.MapPath("~/Sheets"), downfilename);
        //                         if (model.ReportType == "PDF")
        //                             reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
        //                         else
        //                             reportDocument.ExportToDisk(ExportFormatType.ExcelRecord, physicalPath);

        //                     }


        //                 }
        //             }

        //             string[] filePaths = Directory.GetFiles(Server.MapPath("~/Sheets/"));
        //             using (ZipFile zip = new ZipFile())
        //             {
        //                 zip.AlternateEncodingUsage = ZipOption.AsNecessary;
        //                 zip.AddDirectoryByName("Sheets");
        //                 foreach (string filePath in filePaths)
        //                 {
        //                     zip.AddFile(filePath, "Sheets");
        //                 }

        //                 Response.Clear();
        //                 Response.BufferOutput = false;
        //                 string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
        //                 Response.ContentType = "application/zip";
        //                 Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
        //                 zip.Save(Response.OutputStream);
        //                 Response.End();

        //                 foreach (string filePath in filePaths)
        //                 {
        //                     // Delete those files
        //                     var fileName = Path.GetFileName(filePath);
        //                     var physicalPathDel = Path.Combine(Server.MapPath("~/Sheets"), fileName);
        //                     if (System.IO.File.Exists(physicalPathDel))
        //                     {
        //                         System.IO.File.Delete(physicalPathDel);
        //                     }
        //                 }
        //             }
        //        }

        //        else if (model.REGION_CODE != "00")
        //        {
        //            var reportDocument = new ReportDocument();
        //            var reportPath = Server.MapPath("~/Reports");
        //            var dataTableObj = new DataTable();
        //            var reportObj = new ReportDAO();
        //            dataTableObj = reportObj.GetLocationWiseValueShareRegionTerritoryMarketValueShare(model);

        //            switch (model.ReportName)
        //            {
        //                case "DivisionRegionTerritoryMarketValueShare":
        //                    reportPath = reportPath + "/RptDivisionRegionTerritoryMarketValueShare.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;
        //                case "DivisionRegionTerritoryMarketValueSharebyValue":
        //                    reportPath = reportPath + "/RptDivisionRegionTerritoryMarketValueSharebyValue.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;
        //            }

        //           if (model.ReportName == "DivisionRegionTerritoryMarketValueShare")
        //            {
        //                string downfilename = "Division_Region_Territory_Market_Value_Share_perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else
        //            {
        //                string downfilename = "Division_Region_Territory_Market_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }

        //        }

        //}

        //        else
        //        {
        //            var reportDocument = new ReportDocument();
        //            var reportPath = Server.MapPath("~/Reports");
        //            var dataTableObj = new DataTable();
        //            var reportObj = new ReportDAO();
        //            dataTableObj = reportObj.GetLocationWiseValueShareData(model);

        //            switch (model.ReportName)
        //            {

        //                case "DivisionRegionTerritoryMarketValueShare":
        //                    reportPath = reportPath + "/RptDivisionRegionTerritoryMarketValueShare.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;
        //                case "DivisionRegionTerritoryMarketValueSharebyValue":
        //                    reportPath = reportPath + "/RptDivisionRegionTerritoryMarketValueSharebyValue.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;

        //                case "NationalValueShare":
        //                    reportPath = reportPath + "/RptNationalValueShare.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;

        //                case "NationalValueShareValue":
        //                    reportPath = reportPath + "/RptNationalValueShareValue.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;

        //                case "ZoneValueSharePersent":
        //                    reportPath = reportPath + "/RptZoneValueSharePersent.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;

        //                case "ZoneValueShareValue":
        //                    reportPath = reportPath + "/RptZoneValueShareValue.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;

        //                case "DivisionValueSharePersent":
        //                    reportPath = reportPath + "/RptDivisionValueSharePersent.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;

        //                case "DivisionValueShareValue":
        //                    reportPath = reportPath + "/RptDivisionValueShareValue.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;
        //                case "RegionValueSharePersent":
        //                    reportPath = reportPath + "/RptRegionValueSharePersent.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;

        //                case "RegionValueShareValue":
        //                    reportPath = reportPath + "/RptRegionValueShareValue.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;


        //                case "TerritoryValueSharePersent":
        //                    reportPath = reportPath + "/RptTerritoryValueSharePersent.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;

        //                case "TerritoryValueShareValue":
        //                    reportPath = reportPath + "/RptTerritoryValueShareValue.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;
        //                case "MarketValueSharePersent":
        //                    reportPath = reportPath + "/RptMarketValueSharePersent.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;
        //                case "MarketValueShareValue":
        //                    reportPath = reportPath + "/RptMarketValueShareValue.rpt";
        //                    reportDocument.Load(reportPath);
        //                    reportDocument.SetDataSource(dataTableObj);
        //                    reportDocument.Refresh();
        //                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
        //                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
        //                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
        //                    reportDocument.SetParameterValue("FromDate", model.FromDate);
        //                    reportDocument.SetParameterValue("ToDate", model.ToDate);
        //                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
        //                    {
        //                        if (model.PRESC_CATE_CODE == "P")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
        //                        else if (model.PRESC_CATE_CODE == "T")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
        //                        else if (model.PRESC_CATE_CODE == "O")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
        //                        else if (model.PRESC_CATE_CODE == "S")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
        //                        else if (model.PRESC_CATE_CODE == "0")
        //                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
        //                    }
        //                    break;
        //            }

        //            if (model.ReportName == "NationalValueShare")
        //            {
        //                string downfilename = "National_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else if (model.ReportName == "NationalValueShareValue")
        //            {
        //                string downfilename = "National_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else if (model.ReportName == "ZoneValueSharePersent")
        //            {
        //                string downfilename = "Zone_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else if (model.ReportName == "ZoneValueShareValue")
        //            {
        //                string downfilename = "Zone_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else if (model.ReportName == "DivisionValueSharePersent")
        //            {
        //                string downfilename = "Division_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else if (model.ReportName == "DivisionValueShareValue")
        //            {
        //                string downfilename = "Division_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);

        //            }
        //            else if (model.ReportName == "RegionValueSharePersent")
        //            {
        //                string downfilename = "Region_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else if (model.ReportName == "RegionValueShareValue")
        //            {
        //                string downfilename = "Region_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else if (model.ReportName == "TerritoryValueSharePersent")
        //            {
        //                string downfilename = "Territory_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else if (model.ReportName == "TerritoryValueShareValue")
        //            {
        //                string downfilename = "Territory_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else if (model.ReportName == "MarketValueSharePersent")
        //            {
        //                string downfilename = "Market_Value_Share_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else if (model.ReportName == "MarketValueShareValue")
        //            {
        //                string downfilename = "Market_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }

        //           else if (model.ReportName == "DivisionRegionTerritoryMarketValueShare")
        //            {
        //                string downfilename = "Division_Region_Territory_Market_Value_Share_perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }
        //            else
        //            {
        //                string downfilename = "Division_Region_Territory_Market_Value_Share_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
        //                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
        //            }

        //            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
        //        }

        //        return View();
        //    }
        #endregion

        #region National Wise Product Share Ranking Report Report
        public ActionResult GenericListForProductShareRanking()
        {
            var reportObj = new ReportDAO();
            var data = reportObj.GenericListForProductShareRanking();
            var genericList = Json(data, JsonRequestBehavior.AllowGet);
            genericList.MaxJsonLength = int.MaxValue;
            return genericList;

        }

        public ActionResult frmRptProductShareRanking()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "National Product Share Ranking Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptProductShareRanking(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetProductWiseValueShareData(model);


            switch (model.ReportName)
            {
                case "NationalProductShareRankingValue":
                    reportPath = reportPath + "/RptNationalProductShareRankingByValue.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    if (!string.IsNullOrEmpty(model.ddlPresCategory))
                    {
                        if (model.ddlPresCategory == "P")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                        else if (model.ddlPresCategory == "T")
                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                        else if (model.ddlPresCategory == "O")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                        else if (model.ddlPresCategory == "S")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                        else if (model.ddlPresCategory == "0")
                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
                    }
                    break;

                case "NationalProductShareRankingPersent":
                    reportPath = reportPath + "/RptNationalProductShareRankingByPersent.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    if (!string.IsNullOrEmpty(model.ddlPresCategory))
                    {
                        if (model.ddlPresCategory == "P")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                        else if (model.ddlPresCategory == "T")
                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                        else if (model.ddlPresCategory == "O")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                        else if (model.ddlPresCategory == "S")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                        else if (model.ddlPresCategory == "0")
                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
                    }
                    break;
            }

            if (model.ReportName == "NationalProductShareRankingValue")
            {
                string downfilename = "National_Product_Share_Ranking_Report_Value" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "National_Product_Share_Ranking_Report_Perc" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }

            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        #endregion

        #region Division wise value Share Ranking Report
        public ActionResult DivisionWiseProductShareRanking()
        {
            var reportObj = new ReportDAO();
            var divisionList = reportObj.DivisionListForProductShareRanking();
            return Json(divisionList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult frmRptDivisionWiseProductRankingReport()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Division Wise Product Share Ranking Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptDivisionWiseProductRankingReport(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.DivisionWiseProductRankingReport(model);


            switch (model.ReportName)
            {
                case "DivisionProductShareRankingPersent":
                    reportPath = reportPath + "/RptDivisionProductShareRankingPersent.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    if (!string.IsNullOrEmpty(model.ddlPresCategory))
                    {
                        if (model.ddlPresCategory == "P")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                        else if (model.ddlPresCategory == "T")
                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                        else if (model.ddlPresCategory == "O")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                        else if (model.ddlPresCategory == "S")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                        else if (model.ddlPresCategory == "0")
                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
                    }
                    break;

                case "DivisionProductShareRankingValue":
                    reportPath = reportPath + "/RptDivisionProductShareRankingValue.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    if (!string.IsNullOrEmpty(model.ddlPresCategory))
                    {
                        if (model.ddlPresCategory == "P")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                        else if (model.ddlPresCategory == "T")
                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                        else if (model.ddlPresCategory == "O")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                        else if (model.ddlPresCategory == "S")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                        else if (model.ddlPresCategory == "0")
                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
                    }
                    break;
            }

            if (model.ReportName == "DivisionProductShareRankingPersent")
            {
                string downfilename = "Division_Product_Share_Ranking_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "Division_Product_Share_Ranking_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        #endregion

        #region Region Share Ranking Report Report

        public ActionResult RegionListForProductShareRanking()
        {
            var reportObj = new ReportDAO();
            var regionList = reportObj.RegionListForProductShareRanking();
            return Json(regionList, JsonRequestBehavior.AllowGet);

        }
        public ActionResult frmRptRegionWiseProductRankingReport()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Region Wise Product Share Ranking Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptRegionWiseProductRankingReport(ReportModel model)
        {

            if (model.REGION_CODE == "00")
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();

                        var reportDocument = new ReportDocument();
                        var reportPath = Server.MapPath("~/Reports");
                        var dataTableObj = new DataTable();
                        var reportObj = new ReportDAO();
                        dataTableObj = reportObj.RegionWiseProductRankingReport(model);


                        switch (model.ReportName)
                        {
                            case "RegionProductShareRankingPersent":
                                if (model.ReportType == "PDF")
                                {
                                    reportPath = reportPath + "/RptRegionWiseProductShareRankingByPersent.rpt";
                                }
                                else
                                {
                                    reportPath = reportPath + "/RptRegionWiseProductShareRankingByPersentExcel.rpt";
                                }
                                
                                reportDocument.Load(reportPath);
                                reportDocument.SetDataSource(dataTableObj);
                                reportDocument.Refresh();
                                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                                reportDocument.SetParameterValue("FromDate", model.FromDate);
                                reportDocument.SetParameterValue("ToDate", model.ToDate);
                                if (!string.IsNullOrEmpty(model.ddlPresCategory))
                                {
                                    if (model.ddlPresCategory == "P")
                                        reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                                    else if (model.ddlPresCategory == "T")
                                        reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                                    else if (model.ddlPresCategory == "O")
                                        reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                                    else if (model.ddlPresCategory == "S")
                                        reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                                    else if (model.ddlPresCategory == "0")
                                        reportDocument.SetParameterValue("PrescriptionCateName", "All");
                                }

                                break;

                            case "RegionProductShareRankingValue":
                                if(model.ReportType == "PDF")
                                {
                                    reportPath = reportPath + "/RptRegionWiseProductShareRankingByValue.rpt";
                                }
                                else
                                {
                                    reportPath = reportPath + "/RptRegionWiseProductShareRankingByValueExcel.rpt";
                                }
                                reportDocument.Load(reportPath);
                                reportDocument.SetDataSource(dataTableObj);
                                reportDocument.Refresh();
                                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                                reportDocument.SetParameterValue("FromDate", model.FromDate);
                                reportDocument.SetParameterValue("ToDate", model.ToDate);
                                if (!string.IsNullOrEmpty(model.ddlPresCategory))
                                {
                                    if (model.ddlPresCategory == "P")
                                        reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                                    else if (model.ddlPresCategory == "T")
                                        reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                                    else if (model.ddlPresCategory == "O")
                                        reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                                    else if (model.ddlPresCategory == "S")
                                        reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                                    else if (model.ddlPresCategory == "0")
                                        reportDocument.SetParameterValue("PrescriptionCateName", "All");

                                }

                                break;
                        }

                        string path = Server.MapPath("~/Region_Product_Share_Ranking/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        if (model.ReportName == "RegionProductShareRankingPersent")
                        {
                            if (model.ReportType == "PDF")
                            {
                                string downfilename = "Region_Product_Share_Ranking_" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                                //string downfilename = "Region_Product_Share_Ranking_" + model.REGION_NAME + "_From_" + model.FromDate + "";
                                var physicalPath = Path.Combine(Server.MapPath("~/Region_Product_Share_Ranking"), downfilename);
                                reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                            }
                            else
                            {
                                string downfilename = "Region_Product_Share_Ranking_" + model.REGION_NAME + "_By_Perc_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xlsx";
                                var physicalPath = Path.Combine(Server.MapPath("~/Region_Product_Share_Ranking"), downfilename);
                                reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                            }
                            reportDocument.Close();
                            reportDocument.Dispose();
                            GC.Collect();
                        }

                        else
                        {
                            if (model.ReportType == "PDF")
                            {
                                string downfilename = "Region_Product_Share_Ranking_" + model.REGION_NAME + "_By_Value_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                                var physicalPath = Path.Combine(Server.MapPath("~/Region_Product_Share_Ranking"), downfilename);
                                reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                            }
                            else
                            {
                                string downfilename = "Region_Product_Share_Ranking_" + model.REGION_NAME + "_By_Value_" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xlsx";
                                var physicalPath = Path.Combine(Server.MapPath("~/Region_Product_Share_Ranking"), downfilename);
                                reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                            }
                            reportDocument.Close();
                            reportDocument.Dispose();
                            GC.Collect();
                        }
                    }

                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/Region_Product_Share_Ranking/"));
                    using (ZipFile zip = new ZipFile())
                    {
                        zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                        zip.AddDirectoryByName("Region_Product_Share_Ranking");
                        foreach (string filePath in filePaths)
                        {
                            zip.AddFile(filePath, "Region_Product_Share_Ranking");
                        }

                        Response.Clear();
                        Response.BufferOutput = false;
                        //string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                        string zipName = String.Format("Region_{0}.zip", "Product_Share_Ranking_From: " + model.FromDate + "_To: " + model.ToDate + "");
                        Response.ContentType = "application/zip";
                        Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                        zip.Save(Response.OutputStream);
                        Response.End();

                        foreach (string filePath in filePaths)
                        {
                            // Delete those files
                            var fileName = Path.GetFileName(filePath);
                            var physicalPathDel = Path.Combine(Server.MapPath("~/Region_Product_Share_Ranking"), fileName);
                            if (System.IO.File.Exists(physicalPathDel))
                            {
                                System.IO.File.Delete(physicalPathDel);
                            }
                        }
                    }

                }
            }

            else
            {
                var reportDocument = new ReportDocument();
                var reportPath = Server.MapPath("~/Reports");
                var dataTableObj = new DataTable();
                var reportObj = new ReportDAO();
                dataTableObj = reportObj.RegionWiseProductRankingReport(model);

                switch (model.ReportName)
                {
                    case "RegionProductShareRankingPersent":
                        if (model.ReportType == "PDF")
                        {
                            reportPath = reportPath + "/RptRegionWiseProductShareRankingByPersent.rpt";
                        }
                        else
                        {
                            reportPath = reportPath + "/RptRegionWiseProductShareRankingByPersentExcel.rpt";
                        }
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.ddlPresCategory))
                        {
                            if (model.ddlPresCategory == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.ddlPresCategory == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.ddlPresCategory == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.ddlPresCategory == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.ddlPresCategory == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;

                    case "RegionProductShareRankingValue":
                        //reportPath = reportPath + "/RptRegionWiseProductShareRankingByValue.rpt";
                        if (model.ReportType == "PDF")
                        {
                            reportPath = reportPath + "/RptRegionWiseProductShareRankingByValue.rpt";
                        }
                        else
                        {
                            reportPath = reportPath + "/RptRegionWiseProductShareRankingByValueExcel.rpt";
                        }
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.ddlPresCategory))
                        {
                            if (model.ddlPresCategory == "P")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                            else if (model.ddlPresCategory == "T")
                                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                            else if (model.ddlPresCategory == "O")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                            else if (model.ddlPresCategory == "S")
                                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                            else if (model.ddlPresCategory == "0")
                                reportDocument.SetParameterValue("PrescriptionCateName", "All");
                        }
                        break;
                }

                if (model.ReportName == "RegionProductShareRankingPersent")
                {
                    string downfilename = "Region_Product_Share_Ranking_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                else
                {
                    string downfilename = "Region_Product_Share_Ranking_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
                }
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();

            }
            return View();
        }

        #endregion

        #region Zone wise value Share Ranking Report
        public ActionResult ZoneWiseProductShareRanking()
        {
            var reportObj = new ReportDAO();
            var zoneList = reportObj.ZoneListForProductShareRanking();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult frmRptZoneWiseProductRankingReport()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Zone Wise Product Share Ranking Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptZoneWiseProductRankingReport(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.ZoneWiseProductRankingReport(model);


            switch (model.ReportName)
            {
                case "ZoneProductShareRankingPersent":
                    reportPath = reportPath + "/RptZoneProductShareRankingPersent.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                    {
                        if (model.PRESC_CATE_CODE == "P")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                        else if (model.PRESC_CATE_CODE == "T")
                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                        else if (model.PRESC_CATE_CODE == "O")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                        else if (model.PRESC_CATE_CODE == "S")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                        else if (model.PRESC_CATE_CODE == "0")
                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
                    }
                    break;

                case "ZoneProductShareRankingValue":
                    reportPath = reportPath + "/RptZoneProductShareRankingValue.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                    {
                        if (model.PRESC_CATE_CODE == "P")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                        else if (model.PRESC_CATE_CODE == "T")
                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                        else if (model.PRESC_CATE_CODE == "O")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                        else if (model.PRESC_CATE_CODE == "S")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                        else if (model.PRESC_CATE_CODE == "0")
                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
                    }
                    break;
            }

            if (model.ReportName == "ZoneProductShareRankingPersent")
            {
                string downfilename = "Zone_Product_Share_Ranking_Perc_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "Zone_Product_Share_Ranking_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }
        #endregion

        #region GENERIC WISE NATIONAL VALUE SHARE FOR DIVISION Report

        public ActionResult GenericListForAllandSelectedData()
        {
            var reportObj = new ReportDAO();
            var genericList = reportObj.GenericListForAllandSelectedData();
            return Json(genericList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetPrescriptionListForDropDown()
        {
            var PrescriptionList = Dalobject.PrescriptionListDropDown();
            return Json(PrescriptionList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult FrmRptGenericWiseValueShareDivision()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Generic Wise National Value Share for Division";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult FrmRptGenericWiseValueShareDivision(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GenericWiseNationalValueShareforDivisionReport(model);


            switch (model.ReportName)
            {
                case "GenericWiseNationalValueShareForDivisionPersentReport":
                    reportPath = reportPath + "/GENERIC WISE NATIONAL VALUSE SHARE OF A DIVISION PERSENT.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    reportDocument.SetParameterValue("Division_Name", model.DIVISION_NAME);
                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                    {
                        if (model.PRESC_CATE_CODE == "P")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                        else if (model.PRESC_CATE_CODE == "T")
                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                        else if (model.PRESC_CATE_CODE == "O")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                        else if (model.PRESC_CATE_CODE == "S")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                        else if (model.PRESC_CATE_CODE == "0")
                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
                    }
                    break;

                case "GenericWiseNationalValueShareForDivisionValueReport":
                    reportPath = reportPath + "/GENERIC WISE NATIONAL VALUSE SHARE OF A DIVISION VALUE.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    reportDocument.SetParameterValue("Division_Name", model.DIVISION_NAME);
                    if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                    {
                        if (model.PRESC_CATE_CODE == "P")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                        else if (model.PRESC_CATE_CODE == "T")
                            reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                        else if (model.PRESC_CATE_CODE == "O")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                        else if (model.PRESC_CATE_CODE == "S")
                            reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                        else if (model.PRESC_CATE_CODE == "0")
                            reportDocument.SetParameterValue("PrescriptionCateName", "All");
                    }
                    break;
            }

            if (model.ReportName == "GenericWiseNationalValueShareForDivisionPersentReport")
            {
                string downfilename = "Generic_Wise_National_Value_Share_For_Division_Percent_Report_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "Generic_Wise_National_Value_Share_For_Division_Value_Report_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        #endregion

        // 3rd Phase Report

        #region Therapeutic Class Level Value Share By % & Value of the Market

        public ActionResult frmRptTherapeuticClassLevelValueSharebyMarket()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Therapeutic Class Wise Product Share of the Market Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptTherapeuticClassLevelValueSharebyMarket(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.TherapeuticWiseNationalValueShareforMarketReport(model);

            switch (model.ReportName)
            {
                case "TherapeuticClassWiseValueShareofaMarketByPersent":
                    reportPath = reportPath + "/THERAPEUTIC CLASS WISE VALUE SHARE BY PERSENT FOR MARKET.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    if (string.IsNullOrEmpty(model.MARKET_NAME))
                        reportDocument.SetParameterValue("market", "All Market");
                    else
                        reportDocument.SetParameterValue("market", model.MARKET_NAME);
                    reportDocument.SetParameterValue("ddlTheraClassLevel", model.ddlTheraClassLevel);
                    if (model.ddlPresCategory == "0")
                        reportDocument.SetParameterValue("ddlGeneric", "All");
                    else
                    {
                        if (model.ddlPresCategory == "P")
                            reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                        else if (model.ddlPresCategory == "O")
                            reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                        else if (model.ddlPresCategory == "S")
                            reportDocument.SetParameterValue("ddlGeneric", "Slip");
                        else if (model.ddlPresCategory == "T")
                            reportDocument.SetParameterValue("ddlGeneric", "OTC");
                    }
                    break;

                case "TherapeuticClassWiseValueShareofaMarketByValue":
                    reportPath = reportPath + "/THERAPEUTIC CLASS WISE VALUE SHARE BY VALUE FOR MARKET.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    if (string.IsNullOrEmpty(model.MARKET_NAME))
                        reportDocument.SetParameterValue("market", "All Market");
                    else
                        reportDocument.SetParameterValue("market", model.MARKET_NAME);
                    reportDocument.SetParameterValue("ddlTheraClassLevel", model.ddlTheraClassLevel);
                    if (model.ddlPresCategory == "0")
                        reportDocument.SetParameterValue("ddlGeneric", "All");
                    else
                    {
                        if (model.ddlPresCategory == "P")
                            reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                        else if (model.ddlPresCategory == "O")
                            reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                        else if (model.ddlPresCategory == "S")
                            reportDocument.SetParameterValue("ddlGeneric", "Slip");
                        else if (model.ddlPresCategory == "T")
                            reportDocument.SetParameterValue("ddlGeneric", "OTC");
                    }
                    break;
                //reportDocument.SetParameterValue("Division_Name", model.DIVISION_NAME);
                //if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
                //{
                //    if (model.PRESC_CATE_CODE == "P")
                //        reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
                //    else if (model.PRESC_CATE_CODE == "T")
                //        reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
                //    else if (model.PRESC_CATE_CODE == "O")
                //        reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
                //    else if (model.PRESC_CATE_CODE == "S")
                //        reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
                //    else if (model.PRESC_CATE_CODE == "0")
                //        reportDocument.SetParameterValue("PrescriptionCateName", "All");
                //}
                //break;
            }

            if (model.ReportName == "TherapeuticClassWiseValueShareofaMarketByPersent")
            {
                string downfilename = "Therapeutic_Class_Wise_Value_Share_of_a_Market_By_Percent_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "Therapeutic_Class_Wise_Value_Share_of_a_Market_By_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();

            //DataTable dt;
            //ReportDocument reportDocument = new ReportDocument();
            //dt = Dalobject.TherapeuticWiseNationalValueShareforMarketReport(model);
            //string ReportPath = Server.MapPath("~/Reports");

            //switch (model.ReportName)
            //{
            //    case "TherapeuticClassWiseValueShareofaMarketByPersent":
            //        ReportPath = ReportPath + "/THERAPEUTIC CLASS WISE VALUE SHARE BY PERSENT FOR MARKET.rpt";
            //        break;
            //    default:
            //        ReportPath = ReportPath + "/THERAPEUTIC CLASS WISE VALUE SHARE BY VALUE FOR MARKET.rpt";
            //        break;
            //}
            //reportDocument.Load(ReportPath);
            //reportDocument.SetDataSource(dt);
            //reportDocument.Refresh();
            //reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            //reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            //reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            //reportDocument.SetParameterValue("FromDate", model.FromDate);
            //reportDocument.SetParameterValue("ToDate", model.ToDate);
            //if (string.IsNullOrEmpty(model.MARKET_NAME))
            //    reportDocument.SetParameterValue("market", "All Market");
            //else
            //    reportDocument.SetParameterValue("market", model.MARKET_NAME);
            //reportDocument.SetParameterValue("ddlTheraClassLevel", model.ddlTheraClassLevel);
            //if (model.ddlPresCategory == "0")
            //    reportDocument.SetParameterValue("ddlGeneric", "All");
            //else
            //{
            //    if (model.ddlPresCategory == "P")
            //        reportDocument.SetParameterValue("ddlGeneric", "Prescription");
            //    else if (model.ddlPresCategory == "O")
            //        reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
            //    else if (model.ddlPresCategory == "S")
            //        reportDocument.SetParameterValue("ddlGeneric", "Slip");
            //    else if (model.ddlPresCategory == "T")
            //        reportDocument.SetParameterValue("ddlGeneric", "OTC");
            //}
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            //return View();


            ////var reportDocument = new ReportDocument();
            ////var reportPath = Server.MapPath("~/Reports");
            ////var dataTableObj = new DataTable();
            ////var reportObj = new ReportDAO();
            ////dataTableObj = reportObj.ZoneWiseProductRankingReport(model);


            ////switch (model.ReportName)
            ////{
            ////    case "TherapeuticClassWiseValueShareofaMarketByPersent":
            ////        reportPath = reportPath + "/RptZoneProductShareRankingPersent.rpt";
            ////        reportDocument.Load(reportPath);
            ////        reportDocument.SetDataSource(dataTableObj);
            ////        reportDocument.Refresh();
            ////        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            ////        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            ////        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            ////        reportDocument.SetParameterValue("FromDate", model.FromDate);
            ////        reportDocument.SetParameterValue("ToDate", model.ToDate);
            ////        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
            ////        {
            ////            if (model.PRESC_CATE_CODE == "P")
            ////                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
            ////            else if (model.PRESC_CATE_CODE == "T")
            ////                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
            ////            else if (model.PRESC_CATE_CODE == "O")
            ////                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
            ////            else if (model.PRESC_CATE_CODE == "S")
            ////                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
            ////            else if (model.PRESC_CATE_CODE == "0")
            ////                reportDocument.SetParameterValue("PrescriptionCateName", "All");
            ////        }
            ////        break;

            ////    case "TherapeuticClassWiseValueShareofaMarketByValue":
            ////        reportPath = reportPath + "/RptZoneProductShareRankingValue.rpt";
            ////        reportDocument.Load(reportPath);
            ////        reportDocument.SetDataSource(dataTableObj);
            ////        reportDocument.Refresh();
            ////        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            ////        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            ////        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            ////        reportDocument.SetParameterValue("FromDate", model.FromDate);
            ////        reportDocument.SetParameterValue("ToDate", model.ToDate);
            ////        if (!string.IsNullOrEmpty(model.PRESC_CATE_CODE))
            ////        {
            ////            if (model.PRESC_CATE_CODE == "P")
            ////                reportDocument.SetParameterValue("PrescriptionCateName", "Prescription");
            ////            else if (model.PRESC_CATE_CODE == "T")
            ////                reportDocument.SetParameterValue("PrescriptionCateName", "OTC");
            ////            else if (model.PRESC_CATE_CODE == "O")
            ////                reportDocument.SetParameterValue("PrescriptionCateName", "Organization");
            ////            else if (model.PRESC_CATE_CODE == "S")
            ////                reportDocument.SetParameterValue("PrescriptionCateName", "Slip");
            ////            else if (model.PRESC_CATE_CODE == "0")
            ////                reportDocument.SetParameterValue("PrescriptionCateName", "All");
            ////        }
            ////        break;
            ////}
            ////reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            ////return View();
        }

        #endregion

        #region Specialization and Generic wise Manufecturer Share Ranking

        public ActionResult DosageFormList()
        {
            var reportObj = new ReportDAO();
            var dosageList = reportObj.DosageFormList();
            return Json(dosageList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LocationList()
        {
            var reportObj = new ReportDAO();
            var locationList = reportObj.LocationList();
            return Json(locationList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult frmRptDegreeCategoryandGenericwiseManufacturerShareRanking()
        {
            if (Session["UserId"] != null)
            {
                //ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Degree Category and Generic wise Manufacturer Share Ranking Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptDegreeCategoryandGenericwiseManufacturerShareRanking(ReportModel model)
        {
            if (model.REGION_CODE == "00")
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();
                        //
                        var reportDocument = new ReportDocument();
                        var reportPath = Server.MapPath("~/Reports");
                        var dataTableObj = new DataTable();
                        var reportObj = new ReportDAO();
                        dataTableObj = reportObj.DegreeCategoryandGenericwiseManufacturerShareRanking(model);

                        if (model.ReportType == "PDF")
                        {
                            reportPath = reportPath + "/RptSpecializationandGenericwiseManufacturerShareRanking.rpt";
                        }
                        else
                        {
                            reportPath = reportPath + "/RptDegreeCategoryandGenericwiseManufacturerShareRankingExcel.rpt";
                        }

                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (string.IsNullOrEmpty(model.DOSAGE_FORM_NAME))
                            reportDocument.SetParameterValue("dosage_form", "All Dosage Form");
                        else
                            reportDocument.SetParameterValue("dosage_form", model.DOSAGE_FORM_NAME);

                        if (string.IsNullOrEmpty(model.DSG_STRENGTH_NAME))
                            reportDocument.SetParameterValue("dosage_strength", "All Dosage Form");
                        else
                            reportDocument.SetParameterValue("dosage_strength", model.DSG_STRENGTH_NAME);

                        if (string.IsNullOrEmpty(model.GENERIC_NAME))
                            reportDocument.SetParameterValue("generic", "All Generic");
                        else
                            reportDocument.SetParameterValue("generic", model.GENERIC_NAME);

                        if (!string.IsNullOrEmpty(model.REGION_NAME))
                            reportDocument.SetParameterValue("Region", model.REGION_NAME);
                        else
                            reportDocument.SetParameterValue("Region", "ALL Region");

                        string path = Server.MapPath("~/Degree_Category_and_Generic_wise_Manufacturer_Share_Ranking_Report/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //string downfilename = "Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        //var physicalPath = Path.Combine(Server.MapPath("~/Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report"), downfilename);
                        if (model.ReportType == "PDF")
                        {
                            string downfilename = "Degree_Category_and_Generic_wise_Manufacturer_Share_Ranking_Report" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                            var physicalPath = Path.Combine(Server.MapPath("~/Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report"), downfilename);
                            reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        }
                        else
                        {
                            string downfilename = "Degree_Category_and_Generic_wise_Manufacturer_Share_Ranking_Report" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xlsx";
                            var physicalPath = Path.Combine(Server.MapPath("~/Degree_Category_and_Generic_wise_Manufacturer_Share_Ranking_Report"), downfilename);
                            reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                        }

                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();

                    }

                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Degree_Category_and_Generic_wise_Manufacturer_Share_Ranking_Report/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Degree_Category_and_Generic_wise_Manufacturer_Share_Ranking_Report");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Degree_Category_and_Generic_wise_Manufacturer_Share_Ranking_Report");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Degree_Category_and_Generic_wise_Manufacturer_Share_Ranking_Report"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }

            }

            else if (model.REGION_CODE == "000")
            {
                var reportDocument = new ReportDocument();
                var reportPath = Server.MapPath("~/Reports");
                var dataTableObj = new DataTable();
                var reportObj = new ReportDAO();
                dataTableObj = reportObj.DegreeCategoryandGenericwiseManufacturerShareRanking(model);
                reportPath = reportPath + "/RptDegreeCategoryandGenericwiseManufacturerShareRankingExcel.rpt";
                reportDocument.Load(reportPath);
                reportDocument.SetDataSource(dataTableObj);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                if (string.IsNullOrEmpty(model.DOSAGE_FORM_NAME))
                    reportDocument.SetParameterValue("dosage_form", "All Dosage Form");
                else
                    reportDocument.SetParameterValue("dosage_form", model.DOSAGE_FORM_NAME);

                if (string.IsNullOrEmpty(model.DSG_STRENGTH_NAME))
                    reportDocument.SetParameterValue("dosage_strength", "All Dosage Form");
                else
                    reportDocument.SetParameterValue("dosage_strength", model.DSG_STRENGTH_NAME);

                if (string.IsNullOrEmpty(model.GENERIC_NAME))
                    reportDocument.SetParameterValue("generic", "All Generic");
                else
                    reportDocument.SetParameterValue("generic", model.GENERIC_NAME);

                if (!string.IsNullOrEmpty(model.REGION_NAME))
                    reportDocument.SetParameterValue("Region", model.REGION_NAME);
                else
                    reportDocument.SetParameterValue("Region", "ALL Region");

                string downfilename = "Degree_Category_and_Generic_wise_Manufacturer_Share_Ranking_Report" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }

            else
            {
                var reportDocument = new ReportDocument();
                var reportPath = Server.MapPath("~/Reports");
                var dataTableObj = new DataTable();
                var reportObj = new ReportDAO();
                dataTableObj = reportObj.DegreeCategoryandGenericwiseManufacturerShareRanking(model);

                if (model.ReportType == "PDF")
                {
                    reportPath = reportPath + "/RptSpecializationandGenericwiseManufacturerShareRanking.rpt";
                }
                else
                {
                    reportPath = reportPath + "/RptDegreeCategoryandGenericwiseManufacturerShareRankingExcel.rpt";
                }
                reportDocument.Load(reportPath);
                reportDocument.SetDataSource(dataTableObj);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                if (string.IsNullOrEmpty(model.DOSAGE_FORM_NAME))
                    reportDocument.SetParameterValue("dosage_form", "All Dosage Form");
                else
                    reportDocument.SetParameterValue("dosage_form", model.DOSAGE_FORM_NAME);


                if (string.IsNullOrEmpty(model.DSG_STRENGTH_NAME))
                    reportDocument.SetParameterValue("dosage_strength", "All Dosage Form");
                else
                    reportDocument.SetParameterValue("dosage_strength", model.DSG_STRENGTH_NAME);


                if (string.IsNullOrEmpty(model.GENERIC_NAME))
                    reportDocument.SetParameterValue("generic", "All Generic");
                else
                    reportDocument.SetParameterValue("generic", model.GENERIC_NAME);

                if (!string.IsNullOrEmpty(model.REGION_NAME))
                    reportDocument.SetParameterValue("Region", model.REGION_NAME);
                else
                    reportDocument.SetParameterValue("Region", "ALL Region");

                string downfilename = "Degree_Category_and_Generic_wise_Manufacturer_Share_Ranking_Report" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }

            //reportDocument.SetParameterValue("ddlTheraClassLevel", model.ddlTheraClassLevel);
            //if (model.ddlPresCategory == "0")
            //    reportDocument.SetParameterValue("ddlGeneric", "All");
            //else
            //{
            //    if (model.ddlPresCategory == "P")
            //        reportDocument.SetParameterValue("ddlGeneric", "Prescription");
            //    else if (model.ddlPresCategory == "O")
            //        reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
            //    else if (model.ddlPresCategory == "S")
            //        reportDocument.SetParameterValue("ddlGeneric", "Slip");
            //    else if (model.ddlPresCategory == "T")
            //        reportDocument.SetParameterValue("ddlGeneric", "OTC");
            //}

            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();


        }

        public ActionResult frmRptSpecializationandGenericwiseManufacturerShareRanking()
        {
            if (Session["UserId"] != null)
            {
                //ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Specialization and Generic wise Manufacturer Share Ranking Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptSpecializationandGenericwiseManufacturerShareRanking(ReportModel model)
        {
            if (model.REGION_CODE == "00")
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        model.REGION_CODE = dtRegion.Rows[i]["REGION_CODE"].ToString();
                        model.REGION_NAME = dtRegion.Rows[i]["REGION_NAME"].ToString();
                        //
                        var reportDocument = new ReportDocument();
                        var reportPath = Server.MapPath("~/Reports");
                        var dataTableObj = new DataTable();
                        var reportObj = new ReportDAO();
                        dataTableObj = reportObj.SpecializationandGenericWiseManufacurerShareReport(model);

                        if (model.ReportType == "PDF")
                        {
                            reportPath = reportPath + "/RptSpecializationandGenericwiseManufacturerShareRanking.rpt";
                        }
                        else
                        {
                            reportPath = reportPath + "/RptSpecializationandGenericwiseManufacturerShareRankingExcel.rpt";
                        }
                      
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (string.IsNullOrEmpty(model.DOSAGE_FORM_NAME))
                            reportDocument.SetParameterValue("dosage_form", "All Dosage Form");
                        else
                            reportDocument.SetParameterValue("dosage_form", model.DOSAGE_FORM_NAME);

                        if (string.IsNullOrEmpty(model.GENERIC_NAME))
                            reportDocument.SetParameterValue("generic", "All Generic");
                        else
                            reportDocument.SetParameterValue("generic", model.GENERIC_NAME);

                        if (!string.IsNullOrEmpty(model.REGION_NAME))
                            reportDocument.SetParameterValue("Region", model.REGION_NAME);
                        else
                            reportDocument.SetParameterValue("Region", "ALL Region");
                       
                        string path = Server.MapPath("~/Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        //string downfilename = "Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        //var physicalPath = Path.Combine(Server.MapPath("~/Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report"), downfilename);
                        if (model.ReportType == "PDF")
                        {
                                string downfilename = "Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                                var physicalPath = Path.Combine(Server.MapPath("~/Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report"), downfilename);
                                reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        }
                     else
                        {
                               string downfilename = "Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report_" + model.REGION_NAME + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".xlsx";
                               var physicalPath = Path.Combine(Server.MapPath("~/Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report"), downfilename);
                                reportDocument.ExportToDisk(ExportFormatType.ExcelWorkbook, physicalPath);
                        }

                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();

                    }

                }

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }

            }

            else if (model.REGION_CODE == "000")
            {
                var reportDocument = new ReportDocument();
                var reportPath = Server.MapPath("~/Reports");
                var dataTableObj = new DataTable();
                var reportObj = new ReportDAO();
                dataTableObj = reportObj.SpecializationandGenericWiseManufacurerShareReport(model);

                if (model.ReportType == "PDF")
                {
                    reportPath = reportPath + "/RptSpecializationandGenericwiseManufacturerShareRanking.rpt";
                }
                else
                {
                    reportPath = reportPath + "/RptSpecializationandGenericwiseManufacturerShareRankingExcel.rpt";
                }
                reportDocument.Load(reportPath);
                reportDocument.SetDataSource(dataTableObj);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                if (string.IsNullOrEmpty(model.DOSAGE_FORM_NAME))
                    reportDocument.SetParameterValue("dosage_form", "All Dosage Form");
                else
                    reportDocument.SetParameterValue("dosage_form", model.DOSAGE_FORM_NAME);

                if (string.IsNullOrEmpty(model.GENERIC_NAME))
                    reportDocument.SetParameterValue("generic", "All Generic");
                else
                    reportDocument.SetParameterValue("generic", model.GENERIC_NAME);

                if (!string.IsNullOrEmpty(model.REGION_NAME))
                    reportDocument.SetParameterValue("Region", model.REGION_NAME);
                else
                    reportDocument.SetParameterValue("Region", "ALL Region");

                string downfilename = "Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }

            else
            {
                var reportDocument = new ReportDocument();
                var reportPath = Server.MapPath("~/Reports");
                var dataTableObj = new DataTable();
                var reportObj = new ReportDAO();
                dataTableObj = reportObj.SpecializationandGenericWiseManufacurerShareReport(model);

                if (model.ReportType == "PDF")
                {
                    reportPath = reportPath + "/RptSpecializationandGenericwiseManufacturerShareRanking.rpt";
                }
                else
                {
                    reportPath = reportPath + "/RptSpecializationandGenericwiseManufacturerShareRankingExcel.rpt";
                }
                reportDocument.Load(reportPath);
                reportDocument.SetDataSource(dataTableObj);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                if (string.IsNullOrEmpty(model.DOSAGE_FORM_NAME))
                    reportDocument.SetParameterValue("dosage_form", "All Dosage Form");
                else
                    reportDocument.SetParameterValue("dosage_form", model.DOSAGE_FORM_NAME);

                if (string.IsNullOrEmpty(model.GENERIC_NAME))
                    reportDocument.SetParameterValue("generic", "All Generic");
                else
                    reportDocument.SetParameterValue("generic", model.GENERIC_NAME);

                if (!string.IsNullOrEmpty(model.REGION_NAME))
                    reportDocument.SetParameterValue("Region", model.REGION_NAME);
                else
                    reportDocument.SetParameterValue("Region", "ALL Region");

                string downfilename = "Specialization_and_Generic_wise_Manufacturer_Share_Ranking_Report_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }

            //reportDocument.SetParameterValue("ddlTheraClassLevel", model.ddlTheraClassLevel);
            //if (model.ddlPresCategory == "0")
            //    reportDocument.SetParameterValue("ddlGeneric", "All");
            //else
            //{
            //    if (model.ddlPresCategory == "P")
            //        reportDocument.SetParameterValue("ddlGeneric", "Prescription");
            //    else if (model.ddlPresCategory == "O")
            //        reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
            //    else if (model.ddlPresCategory == "S")
            //        reportDocument.SetParameterValue("ddlGeneric", "Slip");
            //    else if (model.ddlPresCategory == "T")
            //        reportDocument.SetParameterValue("ddlGeneric", "OTC");
            //}

            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();


        }

        #endregion

        #region Specialization and Therapeutic Class Level-4 wise Share Ranking

        public ActionResult TherapeuticClassLevel4List()
        {
            var reportObj = new ReportDAO();
            var dosageList = reportObj.TherapeuticClassLevel4List();
            return Json(dosageList, JsonRequestBehavior.AllowGet);

        }
        public ActionResult frmRptSpecializationandTherapeuticClassLevel4wiseValueShareRanking()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Specialization and Therapeutic (Level-4) Wise Value Share Ranking Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptSpecializationandTherapeuticClassLevel4wiseValueShareRanking(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.SpecializationandTherapeuticWiseManufacurerShareReport(model);

            reportPath = reportPath + "/RptSpecializationJibonTherapeuticClassLevel4wiseValueShareRanking.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);


            if (!string.IsNullOrEmpty(model.REGION_NAME))
                reportDocument.SetParameterValue("Region", model.REGION_NAME);
            else
                reportDocument.SetParameterValue("Region", "ALL Region");

            if (!string.IsNullOrEmpty(model.TC_L4_DESC))
                reportDocument.SetParameterValue("Therapeutic", model.TC_L4_DESC);
            else
                reportDocument.SetParameterValue("Therapeutic", "ALL Therapeutic");

            if (!string.IsNullOrEmpty(model.DOSAGE_FORM_NAME))
                reportDocument.SetParameterValue("DosageForm", model.DOSAGE_FORM_NAME);
            else
                reportDocument.SetParameterValue("DosageForm", "ALL Dosage Form");

            //if (string.IsNullOrEmpty(model.MARKET_NAME))
            //    reportDocument.SetParameterValue("generic", "All Market");
            //else
            //    reportDocument.SetParameterValue("generic", model.MARKET_NAME);
            //reportDocument.SetParameterValue("ddlTheraClassLevel", model.ddlTheraClassLevel);
            //if (model.ddlPresCategory == "0")
            //    reportDocument.SetParameterValue("ddlGeneric", "All");
            //else
            //{
            //    if (model.ddlPresCategory == "P")
            //        reportDocument.SetParameterValue("ddlGeneric", "Prescription");
            //    else if (model.ddlPresCategory == "O")
            //        reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
            //    else if (model.ddlPresCategory == "S")
            //        reportDocument.SetParameterValue("ddlGeneric", "Slip");
            //    else if (model.ddlPresCategory == "T")
            //        reportDocument.SetParameterValue("ddlGeneric", "OTC");
            //}

            string downfilename = "Specialization_and_Therapeutic_Level-4_Wise_Value_Share_Ranking_Report_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();


        }

        #endregion

        #region  Therapeutic Class wise Value Share Ranking for National Vs Region

        public ActionResult frmRptTherapeuticClasswiseValueShareRankingforNationalVsRegion()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Therapeutic Class Wise Value Share National Vs Region Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptTherapeuticClasswiseValueShareRankingforNationalVsRegion(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.TherapeuticClassWiseValueShareforNationalVsRegionReport(model);

            reportPath = reportPath + "/RptTherapeuticClasswiseValueShareRankingforNationalVsRegion.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);
            //if (string.IsNullOrEmpty(model.MARKET_NAME))
            //    reportDocument.SetParameterValue("generic", "All Market");
            //else
            //    reportDocument.SetParameterValue("generic", model.MARKET_NAME);

            if (!string.IsNullOrEmpty(model.MANUFACTURER_NAME))
                reportDocument.SetParameterValue("Manufacturer", model.MANUFACTURER_NAME);
            else
                reportDocument.SetParameterValue("Manufacturer", "ALL Manufacturer");

            reportDocument.SetParameterValue("ddlTheraClassLevel", model.ddlTheraClassLevel);
            if (model.PRESC_CATE_CODE == "0")
                reportDocument.SetParameterValue("ddlGeneric", "All");
            else
            {
                if (model.PRESC_CATE_CODE == "P")
                    reportDocument.SetParameterValue("ddlGeneric", "Prescription");
                else if (model.PRESC_CATE_CODE == "O")
                    reportDocument.SetParameterValue("ddlGeneric", "Orgranization");
                else if (model.PRESC_CATE_CODE == "S")
                    reportDocument.SetParameterValue("ddlGeneric", "Slip");
                else if (model.PRESC_CATE_CODE == "T")
                    reportDocument.SetParameterValue("ddlGeneric", "OTC");
            }

            string downfilename = "Therapeutic_Class_Wise_Value_Share_National_Vs_Region_Report_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();


        }

        #endregion

        #region Therapeutic Class Level 4 Wise Individual Institution Value Share (By %) AND By Value

        public ActionResult frmRptTherapeuticClassWiseIndividualInstitutionValueShareByPersent()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Therapeutic (L-4) wise Individual Institution Value Share";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptTherapeuticClassWiseIndividualInstitutionValueShareByPersent(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.TherapeuticClassWiseIndividualInstitutionValueShareReport(model);


            switch (model.ReportName)
            {
                case "TherapeuticClassWiseIndividualInstitutionValueShareByPersent":
                    reportPath = reportPath + "/RptTherapeuticLevel4wiseIndividualInstitutionValueSharePersent.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    reportDocument.SetParameterValue("INSTI_CODE", model.INSTI_CODE);
                    reportDocument.SetParameterValue("INSTI_NAME", model.INSTI_NAME);
                    reportDocument.SetParameterValue("Address", model.Address);
                    break;

                case "TherapeuticClassWiseIndividualInstitutionValueShareByValue":
                    reportPath = reportPath + "/RptTherapeuticLevel4wiseIndividualInstitutionValueShareValue.rpt";
                    reportDocument.Load(reportPath);
                    reportDocument.SetDataSource(dataTableObj);
                    reportDocument.Refresh();

                    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                    reportDocument.SetParameterValue("FromDate", model.FromDate);
                    reportDocument.SetParameterValue("ToDate", model.ToDate);
                    reportDocument.SetParameterValue("INSTI_CODE", model.INSTI_CODE);
                    reportDocument.SetParameterValue("INSTI_NAME", model.INSTI_NAME);
                    reportDocument.SetParameterValue("Address", model.Address);
                    break;
            }

            if (model.ReportName == "TherapeuticClassWiseIndividualInstitutionValueShareByPersent")
            {
                string downfilename = "Therapeutic_Class_Wise_Individual_Institution_Value_Share_By_Percent_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            else
            {
                string downfilename = "Therapeutic_Class_Wise_Individual_Institution_Value_Share_By_Value_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            }
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        #endregion

        #region Market Wise Individual Doctor Value Share of a Therapeutic Class (L-4)

        public ActionResult frmMarketWiseIndividualDoctorValueShareofaTherapeuticClass()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Market Wise Individual Doctor Value Share of a Therapeutic Class ";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmMarketWiseIndividualDoctorValueShareofaTherapeuticClass(ReportModel model)
        {
            if (model.REGION_CODE == "00")
            {
                DataTable dtRegion = dbHelper.GetDataTable("SELECT Distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ORDER BY REGION_CODE");
                if (dtRegion.Rows.Count > 0)
                {
                    for (int i = 0; i < dtRegion.Rows.Count; i++)
                    {
                        var reportDocument = new ReportDocument();
                        var reportPath = Server.MapPath("~/Reports");
                        var dataTableObj = new DataTable();
                        var reportObj = new ReportDAO();
                        dataTableObj = reportObj.MarketWiseIndividualDoctorValueShareReport(model);

                        reportPath = reportPath + "/RptMarketWiseIndividualDoctorValueShareReport.rpt";
                        reportDocument.Load(reportPath);
                        reportDocument.SetDataSource(dataTableObj);
                        reportDocument.Refresh();

                        reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                        reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                        reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                        reportDocument.SetParameterValue("FromDate", model.FromDate);
                        reportDocument.SetParameterValue("ToDate", model.ToDate);
                        if (!string.IsNullOrEmpty(model.TC_L4_DESC))
                            reportDocument.SetParameterValue("Therapeutic", model.TC_L4_DESC);
                        else
                            reportDocument.SetParameterValue("Therapeutic", "ALL Therapeutic");
                        reportDocument.SetParameterValue("Condition", model.ddlCondition);
                        reportDocument.SetParameterValue("NumberOfTran", model.NumberOfTran);
                        reportDocument.Close();
                        reportDocument.Dispose();
                        GC.Collect();
                        string path = Server.MapPath("~/Market_Wise_Individual_Doctor_Value_Share_of_a_Therapeutic_Class/");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        string downfilename = "Market_Wise_Individual_Doctor_Value_Share_of_a_Therapeutic_Class_" + model.REGION_NAME + "" + DateTime.Now.ToString("yyyy-MM-dd hh_mm_ss") + ".pdf";
                        var physicalPath = Path.Combine(Server.MapPath("~/Market_Wise_Individual_Doctor_Value_Share_of_a_Therapeutic_Class"), downfilename);
                        if (model.ReportType == "PDF")
                            reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, physicalPath);
                        else
                            reportDocument.ExportToDisk(ExportFormatType.ExcelRecord, physicalPath);
                    }
                }
                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Market_Wise_Individual_Doctor_Value_Share_of_a_Therapeutic_Class/"));
                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName("Market_Wise_Individual_Doctor_Value_Share_of_a_Therapeutic_Class");
                    foreach (string filePath in filePaths)
                    {
                        zip.AddFile(filePath, "Market_Wise_Individual_Doctor_Value_Share_of_a_Therapeutic_Class");
                    }

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Rigion_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();

                    foreach (string filePath in filePaths)
                    {
                        // Delete those files
                        var fileName = Path.GetFileName(filePath);
                        var physicalPathDel = Path.Combine(Server.MapPath("~/Market_Wise_Individual_Doctor_Value_Share_of_a_Therapeutic_Class"), fileName);
                        if (System.IO.File.Exists(physicalPathDel))
                        {
                            System.IO.File.Delete(physicalPathDel);
                        }
                    }
                }
            }

            else
            {
                var reportDocument = new ReportDocument();
                var reportPath = Server.MapPath("~/Reports");
                var dataTableObj = new DataTable();
                var reportObj = new ReportDAO();
                dataTableObj = reportObj.MarketWiseIndividualDoctorValueShareReport(model);

                reportPath = reportPath + "/RptMarketWiseIndividualDoctorValueShareReport.rpt";
                reportDocument.Load(reportPath);
                reportDocument.SetDataSource(dataTableObj);
                reportDocument.Refresh();

                reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
                reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
                reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
                reportDocument.SetParameterValue("FromDate", model.FromDate);
                reportDocument.SetParameterValue("ToDate", model.ToDate);
                if (!string.IsNullOrEmpty(model.TC_L4_DESC))
                    reportDocument.SetParameterValue("Therapeutic", model.TC_L4_DESC);
                else
                    reportDocument.SetParameterValue("Therapeutic", "ALL Therapeutic");
                reportDocument.SetParameterValue("Condition", model.ddlCondition);
                reportDocument.SetParameterValue("NumberOfTran", model.NumberOfTran);

                string downfilename = "Market_Wise_Individual_Doctor_Value_Share_of_a_Therapeutic_Class_" + "From:" + model.FromDate + "To:" + model.ToDate;
                reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
                reportDocument.Close();
                reportDocument.Dispose();
                GC.Collect();
            }


            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        #endregion


        #region Designation Wise Donation Report

        public ActionResult frmRptDesignationWiseDonation()
        {
            if (Session["UserId"] != null)
            {
                //ViewBag.PrescriptionDropDownList = Dalobject.PrescriptionListDropDown();
                ViewBag.formTitle = "Designation Wise Donation Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult frmRptDesignationWiseDonation(ReportModel model)
        {
            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.DesignationWiseDonation(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/RptDesignationWiseDonation.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);


            string downFileName = "Group_Designation_Wise_Investment_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }



        #endregion

        #region Donation Report
        public ActionResult frmRptDonationReport()
        {
            ViewBag.formTitle = "Donation Report";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptDonationReport(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.DonationDetails(model);

            reportPath = reportPath + "/RptDonationDetails.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);

            string downfilename = "Designation_Wise_Investment" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }
        #endregion

        #region User Wise Entry Report

        public ActionResult frmRptUserEntry()
        {
            ViewBag.formTitle = "User Wise Entry Report";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptUserEntry(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.UserWiseInput(model);

            reportPath = reportPath + "/RptUserInput.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate);

            string downfilename = "User_Wise_Entry_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        #endregion

        #region Special Campaign Program Information
        public ActionResult frmRptSpecialCampngInfo()
        {
            ViewBag.formTitle = "Special Campaign Information Reports";
            return View();
        }
        [HttpPost]
        public ActionResult frmRptSpecialCampngInfo(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.SpecialCampaignInfo(model);

            reportPath = reportPath + "/rptSpecialCampProgram.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);

            string downfilename = "SpecialCampaignInformation";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }
        #endregion

        #region Market wise Monthly Invesment Status

        public ActionResult frmMarketWiseMonthlyInvestment()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Market Wise Monthly Investment Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetHonorariumList()
        {
            var zoneList = Dalobject.GetHonorariumList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult frmMarketWiseMonthlyInvestment(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();


            dataTableObj = reportObj.MarketWiseMonthlyInvestment(model);
            reportPath = reportPath + "/RptMonthlyMarketWiseInvestment.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("YEAR", model.YEAR);

            if (model.HONORARIUM_CODE == "00")
            {
                reportDocument.SetParameterValue("Honor_Type", "Honorarium Type : All");
            }
            else
            {
                DataTable dtHonor = dbHelper.GetDataTable("Select * from Honorarium_Type where Honorarium_Code = '" + model.HONORARIUM_CODE + "'");

                foreach (DataRow dr in dtHonor.Rows)
                {
                    reportDocument.SetParameterValue("Honor_Type", "Honorarium Type : " + dr["HONORARIUM_NAME"].ToString());
                }
            }

            string downfilename = "Market_Wise_Monthly_Investment_" + DateTime.Today.Date;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }


        #endregion
        #region Special Camp Wise Product Information

        public ActionResult frmSpecialCampWiseProdInfo()
        {
            //if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Special Campaign Wise Product Information ";
                return View();
            }
            //return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSCPList()
        {
            var zoneList = Dalobject.GetSCPList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult frmSpecialCampWiseProdInfo(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();


            dataTableObj = reportObj.SpecialCampaignWiseProdInfo(model);
            //reportPath = reportPath + "/RptSpecialCampaignWiseProd.rpt";
            reportPath = reportPath + "/RptSpecialCampaignWiseProdDrill.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);

            if (model.SCP_CODE == "ALL")
            {
                reportDocument.SetParameterValue("SCP_Details", "Special Campaign Program Type : All");
            }
            else
            {
                DataTable dtHonor = dbHelper.GetDataTable("Select * from SCP_INFO where SCP_Code = '" + model.SCP_CODE + "'");

                foreach (DataRow dr in dtHonor.Rows)
                {
                    reportDocument.SetParameterValue("SCP_Details", "Special Campaign Program Type : " + dr["SCP_NAME"].ToString());
                }
            }

            string downfilename = "Special_Campaign_Product_Info_" + DateTime.Today.Date.ToShortDateString();
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        #endregion

        #region Doctor Wise Leader / Non Leader Report

        public ActionResult frmRptDocWiseLeaderNonLeader()
        {
            ViewBag.formTitle = "Doctor Wise Leader / Non Leader Status Report";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptDocWiseLeaderNonLeader(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.DoctorWiseLeaderNonLeader(model);

            reportPath = reportPath + "/RptDocLeader.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", "From: " + model.FromDate + "       To: " + model.ToDate);
            for (int i = 0; i < Dalobject.GetColumnsForDocLeader().Count; i++)
            {
                string sdf = Dalobject.GetColumnsForDocLeader()[i].ToString();
                string dfg = "Col" + (i + 1) + "";
                reportDocument.SetParameterValue("Col" + (i + 1) + "", Dalobject.GetColumnsForDocLeader()[i].ToString());
            }

            string downfilename = "Doctor_Wise_Leader_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        #endregion

        #region Doctor Commitment Meetup Report

        public ActionResult frmRptDocCommitmentMeetUp()
        {
            ViewBag.formTitle = "Doctor Commitment Meetup Report";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptDocCommitmentMeetUp(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.DoctorWiseCommitment(model);

            reportPath = reportPath + "/RptDocCommitmentMeetUp.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", "From: " + model.FromDate + "       To: " + model.ToDate);

            string downfilename = "Doctor_Commitment_Status_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        #endregion


        #region Doctor Wise Leader / Non Leader Only Excel Report

        public ActionResult frmRptDocWiseLeaderExcelOnly()
        {
            ViewBag.formTitle = "Doctor Wise Leader / Non Leader Excel Report";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptDocWiseLeaderExcelOnly(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();

            #region Code for Raw excel if needed

            //System.Data.DataTable dtExcel = dataTableObj;
            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=Doctor_Leader_Excel_Report_" + DateTime.Today.ToString("dd-MM-yyyy") + ".xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-excel";
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //GridView GridView1 = new GridView();
            //GridView1.DataSource = dtExcel;
            //GridView1.DataBind();
            //GridView1.RenderControl(hw);
            //Response.Output.Write(sw.ToString());
            //Response.Flush();
            //Response.End();

            #endregion

            dataTableObj = reportObj.DoctorLeaderExcelOnly(model);
            reportPath = reportPath + "/RptDocLeadeExcelOnly.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            for (int i = 0; i < Dalobject.GetColumnsForDocLeader().Count; i++)
            {
                string sdf = Dalobject.GetColumnsForDocLeader()[i].ToString();
                string dfg = "Col" + (i + 1) + "";
                reportDocument.SetParameterValue("Col" + (i + 1) + "", Dalobject.GetColumnsForDocLeader()[i].ToString());
            }

            string downfilename = "Doctor_Wise_Leader_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        #endregion

        #region Doctor Commitment Meetup Only Excel Report

        public ActionResult frmRptDocCommitmenExcelOnly()
        {
            ViewBag.formTitle = "Doctor Commitment Meetup Report";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptDocCommitmenExcelOnly(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.DoctorWiseCommitmentExcelOnly(model);

            reportPath = reportPath + "/RptDocCommitmentExcel.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            string downfilename = "Doctor_Commitment_Status_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        #endregion


        #region Product Info Report

        public ActionResult frmProductInfo()
        {
            ViewBag.formTitle = "Product Information Reports";
            return View();
        }

        [HttpPost]
        public ActionResult frmProductInfo(ReportModel model)
        {

            DataTable dt;
            ReportDocument reportDocument = new ReportDocument();
            dt = Dalobject.ProductInfo(model);
            string ReportPath = Server.MapPath("~/Reports");
            ReportPath = ReportPath + "/rptProductInfo.rpt";
            reportDocument.Load(ReportPath);
            reportDocument.SetDataSource(dt);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            //reportDocument.SetParameterValue("FromDate", model.FromDate);
            //reportDocument.SetParameterValue("ToDate", model.ToDate);
            //reportDocument.SetParameterValue("Region", model.REGION_NAME);
            //string downFileName = "Frequency_Based_Product_Share_Ranking_" + "From:" + model.FromDate + "To:" + model.ToDate;
            string downfilename = "Product_Information";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();


        }

        #endregion

        #region Doctor Info Report

        public ActionResult frmDoctorInfo()
        {
            ViewBag.formTitle = "Doctor Information Reports";
            return View();
        }

        [HttpPost]
        public ActionResult frmDoctorInfo(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.DoctorInfo(model);

            reportPath = reportPath + "/rptDoctorInfo.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();
            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);

            string downfilename = "Doctor_Information" ;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            return View();
        }

        #endregion


        #region Doctor-Cum-Chemist Doctor List Report

        public ActionResult frmDoctorCumChemistInfo()
        {
            ViewBag.formTitle = "Doctor Cum Chemist Info Reports";
            return View();
        }

        [HttpPost]
        public ActionResult frmDoctorCumChemistInfo(ReportModel model)
        {
            ReportDataHeader reportDataHeader = new ReportDataHeader();
            Export export = new Export();
            DataTable dt = new DataTable();

            string day = DateTime.Now.Day.ToString().Length == 1 ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString().Length == 1 ? "0" + DateTime.Now.Month.ToString() : DateTime.Now.Month.ToString();
            string rptName = day + month + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            model.FromDate = DateTime.Now.ToString("dd/MM/yyyy");
            model.ToDate = DateTime.Now.ToString("dd/MM/yyyy");
            rptName = "Doctor Cum Chemist Info Reports" + rptName;
            dt = reportObj.GetDoctorCumChemistInfo(model);
            export.CompanyName = "SQUARE PHARMACEUTICALS LTD";
            export.ReportName = "Doctor Cum Chemist Info Reports";
            export.FromDate = model.FromDate;
            export.ToDate = model.ToDate;
            export.ExportToExcel(dt, rptName);
            return View();
            //var reportDocument = new ReportDocument();
            //var reportPath = Server.MapPath("~/Reports");
            //var dataTableObj = new DataTable();
            //var reportObj = new ReportDAO();
            //dataTableObj = reportObj.GetDoctorCumChemistInfo(model);
            //reportPath = reportPath + "/rptDoctorCumChemistInfo.rpt";
            //reportDocument.Load(reportPath);
            //reportDocument.SetDataSource(dataTableObj);
            //reportDocument.Refresh();

            //string downfilename = "Doctor_Cum_Chemist_Info" + rptName;
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            //reportDocument.Close();
            //reportDocument.Dispose();
            //GC.Collect();
            //return View();
        }


        public ActionResult frmRptRegionTerritoryMarketWiseDoctorCumChemistsNo()
        {
            ViewBag.formTitle = "Region Territory Market wise Doctor Cum Chemists No.";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptRegionTerritoryMarketWiseDoctorCumChemistsNo(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.GetRegionTerritoryMarketWiseDoctorCumChemistsNo(model);

            rptPath = rptPath + "/RptMarketTerritoryRegionWiseDoctorCumChemistsNoReport.rpt";
            reportDocument.Load(rptPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            string downfilename = "Region_Territory_Market_Wise_Doctor_Cum_Chemists_No";
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelWorkbook, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            //reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, "crReport");
            return View();
        }

        #endregion



        #region Investment Database Only Excel Report

        public ActionResult frmRptInvestmentExcelOnly()
        {
            ViewBag.formTitle = "Investment System Report";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptInvestmentExcelOnly(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.InvestmentReport(model);
            reportPath = reportPath + "/rptInvestmentSystem.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            string downfilename = "Investment_Report_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();
            
            //var downFileName = "Investment_Report_" + "From:" + model.FromDate + "To:" + model.ToDate;
            ////It represent name of column for which you want to select records
            //string[] selectedColumns = new[] { "HONOR_APROV_NO", "HONOR_APROV_DATE", "MGMT_CODE", "MGMT_NAME", "GRP_DSIG_NAME", "NATIONAL", "REGION_NAME", "TERRITORY_NAME", "MARKET_CODE", "MARKET_NAME", "SOCIETY_ID", "SOCITY_NAME", "DOCTOR_ID", "DOCTOR_NAME", "INSTI_CODE", "INSTI_NAME", "HONORARIUM_NAME", "EXPENSE_DETAILS", "EXPENSE_AMT", "EXPENSE_FROM", "EXPENSE_TO", "PRESC_SHARE_PCT", "COMITMENT_SHARE_PCT" };
            //MadeDataForExcel(dataTableObj, downFileName, selectedColumns);
            return View();
        }

        #endregion

        #region Product wise Investment Excel Report

        public ActionResult frmRptProductWiseInvestmentExcelOnly()
        {
            ViewBag.formTitle = "Product wise Investment System Report";
            return View();
        }

        [HttpPost]
        public ActionResult frmRptProductWiseInvestmentExcelOnly(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var reportPath = Server.MapPath("~/Reports");
            var dataTableObj = new DataTable();
            var reportObj = new ReportDAO();
            dataTableObj = reportObj.ProductWiseInvestmentReport(model);
            reportPath = reportPath + "/rptProductWiseInvestmentSystem.rpt";
            reportDocument.Load(reportPath);
            reportDocument.SetDataSource(dataTableObj);
            reportDocument.Refresh();

            string downfilename = "Product_Wise_Investment_Report_" + "From:" + model.FromDate + "To:" + model.ToDate;
            reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downfilename);
            reportDocument.Close();
            reportDocument.Dispose();
            GC.Collect();

            //var downFileName = "Investment_Report_" + "From:" + model.FromDate + "To:" + model.ToDate;
            ////It represent name of column for which you want to select records
            //string[] selectedColumns = new[] { "HONOR_APROV_NO", "HONOR_APROV_DATE", "MGMT_CODE", "MGMT_NAME", "GRP_DSIG_NAME", "NATIONAL", "REGION_NAME", "TERRITORY_NAME", "MARKET_CODE", "MARKET_NAME", "SOCIETY_ID", "SOCITY_NAME", "DOCTOR_ID", "DOCTOR_NAME", "INSTI_CODE", "INSTI_NAME", "HONORARIUM_NAME", "EXPENSE_DETAILS", "EXPENSE_AMT", "EXPENSE_FROM", "EXPENSE_TO", "PRESC_SHARE_PCT", "COMITMENT_SHARE_PCT" };
            //MadeDataForExcel(dataTableObj, downFileName, selectedColumns);
            return View();
        }

        #endregion



        #region SCP Module Report Developed By M. Jiaur Rahman

        #region Spacial Campaign Market wise Value Share

        public ActionResult frmRptSpacialCampaignMarketWiseShare()
        {

            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Special Campaign Market Wise Share Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptSpacialCampaignMarketWiseShare(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(SystemsAuth.Authorize.VersionConflictMsg()))
            {
                ViewBag.msg = SystemsAuth.Authorize.VersionConflictMsg();
                return View();
            }
            bool isExcel = false;
            string downFileName = string.Empty;
            string fromTodate = (model.SCP_NAME).Replace(" ", "_") + "_From_" + model.FromDate + "_To_" + model.ToDate + "_";
            dt = Dalobject.GetScpGroupWiseShare(model);
            switch (model.ReportName)
            {
                case "ScpGroupValueShare":
                    if (model.ReportType == "PDF")
                    {
                        // rptPath = rptPath + "/ScpGroupValueShare.rpt";
                        rptPath = rptPath + "/ScpGroupValueShare.rpt";
                        model.ReportName = "Special_Campaign_Group_Value_Share_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);
                        reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    else
                    {
                        // rptPath = rptPath + "/ScpGroupValueShare.rpt";
                        rptPath = rptPath + "/ScpGroupValueShareExcel.rpt";
                        model.ReportName = "Special_Campaign_Group_Value_Share_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);
                        reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }

                    break;
                case "ScpGroupValueSharePersent":
                    if (model.ReportType == "PDF")
                    {
                        //rptPath = rptPath + "/ScpGroupValueSharePersent.rpt";
                        rptPath = rptPath + "/ScpGroupValueSharePersent.rpt";
                        model.ReportName = "Special_Campaign_Group_Value_Share_Percentage_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);
                        reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    else
                    {
                        //rptPath = rptPath + "/ScpGroupValueSharePersent.rpt";
                        rptPath = rptPath + "/ScpGroupValueSharePersentExcel.rpt";
                        model.ReportName = "Special_Campaign_Group_Value_Share_Percentage_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);
                        reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
             
                    break;
                case "ScpGroupWiseDoctorValueShare":
                    if (model.ReportType == "PDF")
                    {
                        rptPath = rptPath + "/ScpGroupWiseDoctorValueShare.rpt";
                        model.ReportName = "Special_Campaign_GroupWise_Doctor_Value_Share_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                        reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    else
                    {
                        // DataTable dtGroupData=dt.DefaultView.ToTable(false,"DOCTOR_ID,SDICG_ID")

                        rptPath = rptPath + "/ScpGroupWiseDoctorValueShareExcel.rpt";
                        model.ReportName = "Special_Campaign_GroupWise_Doctor_Value_Share_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                        reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    break;
                case "ScpGroupWiseDoctorValueSharePercent":
                    if (model.ReportType == "PDF")
                    {
                        rptPath = rptPath + "/ScpGroupWiseDoctorValueSharePercent.rpt";
                        model.ReportName = "Special_Campaign_GroupWise_Doctor_Value_Share_Percentage_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                        reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    else
                    {
                        rptPath = rptPath + "/ScpGroupWiseDoctorValueSharePercentExcel.rpt";
                        model.ReportName = "Special_Campaign_GroupWise_Doctor_Value_Share_Percentage_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                        reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    break;
                case "ScpGroupWiseInstValueShare":
                    if (model.ReportType == "PDF")
                    {
                        rptPath = rptPath + "/ScpGroupWiseInstValueShare.rpt";
                        model.ReportName = "Special_Campaign_GroupWise_Institute_Value_Share_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                        reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    else
                    {
                        rptPath = rptPath + "/ScpGroupWiseInstValueShareExcel.rpt";
                        model.ReportName = "Special_Campaign_GroupWise_Institute_Value_Share_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                        reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    break;
                case "ScpGroupWiseInstValueSharePercent":
                    if (model.ReportType == "PDF")
                    {
                        rptPath = rptPath + "/ScpGroupWiseInstValueSharePercent.rpt";
                        model.ReportName = "Special_Campaign_GroupWise_Institute_Value_Share_Percentage_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                        reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    else
                    {
                        rptPath = rptPath + "/ScpGroupWiseInstValueSharePercentExcel.rpt";
                        model.ReportName = "Special_Campaign_GroupWise_Institute_Value_Share_Percentage_" + fromTodate;
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdc(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                        reportDocument.ExportToHttpResponse(ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    break;
                case "ScpDoctorPrescriptionInfo":
                    rptPath = rptPath + "/ScpDoctorPrescriptionInfo.rpt";
                    model.ReportName = "Special_Campaign_DoctorPrescriptionInfo_" + fromTodate;
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dt);
                    downFileName = BindParameterScdc(model, reportDocument, downFileName);
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                    reportDocument.Close();
                    reportDocument.Dispose();
                    break;
                
                case "ScpNationalValueShare":
                    rptPath = rptPath + "/ScpNationalValueSharebyValue.rpt";
                    model.ReportName = "Special_Campaign_NationalValueShare" + fromTodate;
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dt);
                    downFileName = BindParameterScdc(model, reportDocument, downFileName);
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                    reportDocument.Close();
                    reportDocument.Dispose();
                    break;
                case "ScpNationalValueSharePercent":
                    rptPath = rptPath + "/ScpNationalValueSharebyPercent.rpt";
                    model.ReportName = "Special_Campaign_NationalValueSharePercent" + fromTodate;
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dt);
                    downFileName = BindParameterScdc(model, reportDocument, downFileName);
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                    reportDocument.Close();
                    reportDocument.Dispose();
                    break;

            }

            #region  for excel

            //if (!isExcel)
            //{
            //    reportDocument.Load(rptPath);
            //    reportDocument.SetDataSource(dt);
            //    reportDocument.Refresh();
            //    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            //    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            //    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            //    reportDocument.SetParameterValue("FromDate", model.FromDate == null ? "" : model.FromDate);
            //    reportDocument.SetParameterValue("ToDate", model.ToDate == null ? "" : model.ToDate);
            //    reportDocument.SetParameterValue("PrescriptionCateName", model.SCP_NAME ?? "ALL CAMPAIGN");

            //    string downFileName = model.ReportName + " From:" + model.FromDate + "To:" + model.ToDate;
            //    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
            //    reportDocument.Close();
            //}
            //else if (isExcel && model.ReportType == "PDF")
            //{
            //    reportDocument.Load(rptPath);
            //    reportDocument.SetDataSource(dt);
            //    reportDocument.Refresh();
            //    reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            //    reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            //    reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            //    reportDocument.SetParameterValue("FromDate", model.FromDate == null ? "" : model.FromDate);
            //    reportDocument.SetParameterValue("ToDate", model.ToDate == null ? "" : model.ToDate);
            //    reportDocument.SetParameterValue("PrescriptionCateName", model.SCP_NAME ?? "ALL CAMPAIGN");

            //    string downFileName = model.ReportName + " From:" + model.FromDate + "To:" + model.ToDate;
            //    reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, downFileName);
            //    reportDocument.Close();
            //}
            //else
            //{
            //    //create excel file manually

            //    string downFileName = model.ReportName + " From:" + model.FromDate + "To:" + model.ToDate;
            //    //var tempArticles = objRepository.MyArticles_DisplayRecords();
            //    //Create new Excel Workbook
            //    var workbook = new HSSFWorkbook();

            //    //Create new Excel Sheet
            //    var sheet = workbook.CreateSheet();

            //    ////(Optional) set the width of the columns
            //    //sheet.SetColumnWidth(0, 20 * 256);//Title
            //    //sheet.SetColumnWidth(1, 20 * 256);//Section
            //    //sheet.SetColumnWidth(2, 20 * 256);//Views
            //    //sheet.SetColumnWidth(3, 20 * 256);//Downloads
            //    //sheet.SetColumnWidth(4, 20 * 256);//Status


            //    ////Create a header row
            //    //var headerRow = sheet.CreateRow(0);
            //    //headerRow.CreateCell(0).SetCellValue("Title");
            //    //headerRow.CreateCell(1).SetCellValue("Section");
            //    //headerRow.CreateCell(2).SetCellValue("Views");
            //    //headerRow.CreateCell(3).SetCellValue("Downloads");
            //    //headerRow.CreateCell(4).SetCellValue("Status");

            //    ////(Optional) freeze the header row so it is not scrolled
            //    //sheet.CreateFreezePane(0, 1, 0, 1);

            //    //int rowNumber = 1;

            //    ////Populate the sheet with values from the grid data

            //    //foreach (IndexTelerikGridViewModel objArticles in tempArticles)
            //    //{
            //    //    //Create a new Row
            //    //    var row = sheet.CreateRow(rowNumber++);

            //    //    //Set the Values for Cells
            //    //    row.CreateCell(0).SetCellValue(objArticles.sTitle);
            //    //    row.CreateCell(1).SetCellValue(objArticles.sSection);
            //    //    row.CreateCell(2).SetCellValue(objArticles.iViews);
            //    //    row.CreateCell(3).SetCellValue(objArticles.iDownloads);
            //    //    row.CreateCell(4).SetCellValue(objArticles.sStatus);

            //    //}


            //    ////Write the Workbook to a memory stream
            //    //MemoryStream output = new MemoryStream();
            //    //workbook.Write(output);

            //    ////Return the result to the end user
            //    //return File(output.ToArray(),   //The binary data of the XLS file
            //    // "application/vnd.ms-excel", //MIME type of Excel files
            //    // downFileName);     //Suggested file name in the "Save as" dialog which will be displayed to the end user "ArticleList.xls"


            //    //Write the workbook to a memory stream
            //    MemoryStream output = new MemoryStream();
            //    workbook.Write(output);


            //    Response.ContentType = "application/vnd.ms-excel";
            //    Response.AddHeader("content-disposition", "attachment;filename=" + downFileName + "_" + DateTime.Today.ToShortDateString() + ".xls");
            //    Response.BinaryWrite(output.ToArray());
            //    Response.End();
            //}
            #endregion

            return View();
        }
        #endregion

        #region Spacial Campaign Doctor & Institution Info

        public ActionResult frmRptSpacialCampaignDoctorAndInstituteInfo()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Special Campaign Doctor & Institution Information Report";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult frmRptSpacialCampaignDoctorAndInstituteInfo(ReportModel model)
        {
            var reportDocument = new ReportDocument();
            var rptPath = Server.MapPath("~/Reports");
            DataTable dt = new DataTable();
            string downFileName = string.Empty;
            if (!string.IsNullOrEmpty(SystemsAuth.Authorize.VersionConflictMsg()))
            {
                ViewBag.msg = SystemsAuth.Authorize.VersionConflictMsg();
                return View();
            }

            model.SCP_NAME = (model.SCP_NAME).Replace(" ", "_");
            dt = Dalobject.GetScpDoctorAndInstituteInfo(model);
            switch (model.ReportName)
            {
                case "ScpGroupWiseDoctor":
                    if (model.ReportType == "PDF")
                    {
                        rptPath = rptPath + "/ScpGroupWiseDoctor.rpt";
                        model.ReportName = "Special_Campaign_Group_Wise_Doctor_" + model.SCP_NAME + "_";
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdi(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                        reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    else
                    {

                        downFileName = "Special_Campaign_Group_Wise_Doctor_" + model.SCP_NAME + "_";
                        //It represent name of column for which you want to select records
                        string[] selectedColumns = new[] { "SINO", "GROUP_NAME", "DOCTOR_ID", "DOCTOR_NAME", "ADDRESS", "POTENTIAL_CATEGORY", "DEGREE", "SPECIALIZATION", "DESIGNATION" };
                        MadeDataForExcel(dt, downFileName, selectedColumns);
                    }

                    break;
                case "ScpGroupWiseInstitute":
                    if (model.ReportType == "PDF")
                    {
                        rptPath = rptPath + "/ScpGroupWiseInstitute.rpt";
                        model.ReportName = "Special_Campaign_Group_Wise_Institute_" + model.SCP_NAME + "_";
                        reportDocument.Load(rptPath);
                        reportDocument.SetDataSource(dt);
                        downFileName = BindParameterScdi(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                        reportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, false, downFileName);
                        reportDocument.Close();
                        reportDocument.Dispose();
                    }
                    else
                    {
                        downFileName = "Special_Campaign_Group_Wise_Institute_" + model.SCP_NAME + "_";
                        //It represent name of column for which you want to select records
                        string[] selectedColumns = new[] { "SINO", "GROUP_NAME", "INSTI_CODE", "INSTI_NAME", "ADDRESS"};
                        MadeDataForExcel(dt, downFileName, selectedColumns);
                    }
                    break;
                case "ScpWiseGroupList":
                    rptPath = rptPath + "/ScpWiseGroupList.rpt";
                    model.ReportName = "Special_Campaign_Group_List_" + model.SCP_NAME + "_";
                    reportDocument.Load(rptPath);
                    reportDocument.SetDataSource(dt);
                    downFileName = BindParameterScdi(model, reportDocument, downFileName);// +" From:" + model.FromDate + "To:" + model.ToDate;
                    reportDocument.ExportToHttpResponse(model.ReportType == "PDF" ? ExportFormatType.PortableDocFormat : ExportFormatType.ExcelRecord, System.Web.HttpContext.Current.Response, false, downFileName);
                    reportDocument.Close();
                    reportDocument.Dispose();
                    break;
            }
            return View();
        }

        private void MadeDataForExcel(DataTable dt, string downFileName, string[] selectedColumns)
        {
            //dt.DefaultView.Sort = "GROUP_NAME";
            DataTable dtSelectedColumns = dt.DefaultView.ToTable(false, selectedColumns);
            string attachment = "attachment; filename=" + downFileName + DateTime.Today.ToShortDateString() + ".xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dtSelectedColumns.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            int r = 0;
            foreach (DataRow dr in dtSelectedColumns.Rows)
            {
                r = r + 1;
                tab = "";
                Response.Write(tab + r.ToString());
                tab = "\t";
                for (i = 1; i < dtSelectedColumns.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }
        private string BindParameterScdi(ReportModel model, ReportDocument reportDocument, string downFileName)
        {
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            //reportDocument.SetParameterValue("FromDate", model.FromDate == null ? "" : model.FromDate);
            //reportDocument.SetParameterValue("ToDate", model.ToDate == null ? "" : model.ToDate);
            reportDocument.SetParameterValue("ScpName", model.SCP_NAME ?? "ALL CAMPAIGN");

            downFileName = model.ReportName + DateTime.Today.ToShortDateString();
            return downFileName;

        }

        private string BindParameterScdc(ReportModel model, ReportDocument reportDocument, string downFileName)
        {
            reportDocument.Refresh();

            reportDocument.SetParameterValue("COMPANY_NAME", Session["COMPANY_NAME"]);
            reportDocument.SetParameterValue("DEVBY", Session["DEVBY"]);
            reportDocument.SetParameterValue("ProjectName", Session["ProjectName"]);
            reportDocument.SetParameterValue("FromDate", model.FromDate == null ? "" : model.FromDate);
            reportDocument.SetParameterValue("ToDate", model.ToDate == null ? "" : model.ToDate);
            reportDocument.SetParameterValue("PrescriptionCateName", model.SCP_NAME ?? "ALL CAMPAIGN");

            downFileName = model.ReportName + DateTime.Today.ToShortDateString();
            return downFileName;
        }

        #endregion
        #endregion



    }
}