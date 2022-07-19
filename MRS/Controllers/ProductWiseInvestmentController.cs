using MRS.DAL.Common;
using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRS.Controllers
{
    public class ProductWiseInvestmentController : Controller
    {
        ProductWiseInvestmentDAO productWiseInvestmentDao;
        private ValidationMsg _vmMsg = new ValidationMsg();
        DBHelper dbHelper = new DBHelper();

        //
        // GET: /ProductWiseInvestment/

        #region Excel Loader
        public ActionResult frmProductWiseInvestmentDBSysExcel()
        {
            {
                if (Session["UserId"] != null)
                {
                    ViewBag.formTitle = "Product wise Investment : Excel Loader";
                    return View();
                }
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult frmProductWiseInvestmentDBSysExcel(ProductWiseInvestmentModel pdr)
        {
            // Get Temp User Session Table Data
            //System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_DOC_HONOR_PAID_INFO Where HONOR_APROV_NO = '2016060001'");
            DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_PROD_WISE_INVESTMENT ORDER BY MARKET_CODE");
            var results = from t in dt.AsEnumerable()
                          select new
                          {
                              MARKET_CODE = t["MARKET_CODE"].ToString(),
                              DOCTOR_ID = t["DOCTOR_ID"].ToString(),
                              PRODUCT_CODE = t["PRODUCT_CODE"].ToString(),
                              INVESTMENT_AMOUNT = t["INVESTMENT_AMOUNT"].ToString(),
                              COMMITMENT = t["COMMITMENT"].ToString(),
                              DURATION_FROM = t["DURATION_FROM"].ToString(),
                              DURATION_TO = t["DURATION_TO"].ToString(),
                              DATA_REMARKS = t["DATA_REMARKS"].ToString(),
                              REMARKS = t["REMARKS"].ToString(),
                              //HONORARIUM_CODE = t["HONORARIUM_CODE"].ToString(),
                              //EXPENSE_DETAILS = t["EXPENSE_DETAILS"].ToString(),
                              //EXPENSE_AMT = t["EXPENSE_AMT"].ToString(),
                              //EXPENSE_FROM = t["EXPENSE_FROM"].ToString(),
                              //EXPENSE_TO = t["EXPENSE_TO"].ToString(),
                              //PRESC_SHARE_PCT = t["PRESC_SHARE_PCT"].ToString(),
                              //PRESC_SHARE_FROM = t["PRESC_SHARE_FROM"].ToString(),
                              //PRESC_SHARE_TO = t["PRESC_SHARE_TO"].ToString(),
                              //COMITMENT_SHARE_PCT = t["COMITMENT_SHARE_PCT"].ToString(),
                              //ENTERED_BY = t["ENTERED_BY"].ToString(),
                              //DATA_REMARKS = t["DATA_REMARKS"].ToString(),
                          };

            foreach (var u in results)
            {
                string rmks = "";

                //#region Checking Expense From Date

                //if (string.IsNullOrEmpty(u.EXPENSE_FROM))
                //{
                //    rmks = rmks + "Expense From Date is Empty, ";
                //}

                //#endregion

                //#region Checking Expense To Date

                //if (string.IsNullOrEmpty(u.EXPENSE_TO))
                //{
                //    rmks = rmks + "Expense To Date is Empty, ";
                //}

                //#endregion

                //#region Checking Expense From Date

                //if (string.IsNullOrEmpty(u.PRESC_SHARE_FROM))
                //{
                //    rmks = rmks + "Present Share From Date is Empty, ";
                //}

                //#endregion

                //#region Checking Expense To Date

                //if (string.IsNullOrEmpty(u.PRESC_SHARE_TO))
                //{
                //    rmks = rmks + "Present Share To Date is Empty, ";
                //}

                //#endregion

                //#region Checking Honorarium Approve Date

                //if (string.IsNullOrEmpty(u.HONOR_APROV_DATE))
                //{
                //    rmks = rmks + "Honor Approved Date is Empty, ";
                //}

                //#endregion

                #region Checking Doctor ID / Market Code / Product Code

                if (string.IsNullOrEmpty(u.DOCTOR_ID) && string.IsNullOrEmpty(u.MARKET_CODE) && string.IsNullOrEmpty(u.PRODUCT_CODE))
                {
                    rmks = rmks + "Doctor ID or Market Code or Product Code is Required, ";
                }
                else
                {
                    if (!string.IsNullOrEmpty(u.MARKET_CODE))
                    {
                        DataTable dtInst = dbHelper.GetDataTable("Select * From LOCATION_VUE Where MARKET_CODE = '" + u.MARKET_CODE + "'");
                        if (dtInst.Rows.Count > 0)
                        {
                        }
                        else
                        {
                            rmks = rmks + "Invalid Market Code, ";
                        }
                    }

                    else if (!string.IsNullOrEmpty(u.DOCTOR_ID))
                    {
                        DataTable dtDoc = dbHelper.GetDataTable("Select * From Doctor Where DOCTOR_ID = '" + u.DOCTOR_ID + "'");

                        if (dtDoc.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            rmks = rmks + "Invalid Doctor ID, ";
                        }
                    }
                    else
                    {
                        DataTable dtDoc = dbHelper.GetDataTable("Select * From PRODUCT Where PROD_ID = '" + u.PRODUCT_CODE + "'");

                        if (dtDoc.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            rmks = rmks + "Invalid Product Code, ";
                        }
                    }
                }

                #endregion

                //#region Checking Honorarium Code

                //if (string.IsNullOrEmpty(u.HONORARIUM_CODE))
                //{
                //    rmks = rmks + "Honorarium Code is Empty, ";
                //}
                //else
                //{
                //    DataTable dtHonor = dbHelper.GetDataTable("Select * From HONORARIUM_TYPE Where HONORARIUM_CODE = " + u.HONORARIUM_CODE);

                //    if (dtHonor.Rows.Count > 0)
                //    {
                //    }
                //    else
                //    {
                //        rmks = rmks + "Invalid Honorarium Code, ";
                //    }
                //}

                //#endregion

                #region Checking Market Code

                if (string.IsNullOrEmpty(u.MARKET_CODE))
                {
                    rmks = rmks + "Market Code is Empty, ";
                }
                else
                {
                    if (u.MARKET_CODE.Length >= 3)
                    {
                        DataTable dtDoc = dbHelper.GetDataTable("Select * From LOCATION_VUE Where MARKET_CODE = '" + u.MARKET_CODE + "'");

                        if (dtDoc.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            rmks = rmks + "No Location Data Found, ";
                        }
                    }
                    else
                    {
                        DataTable dtDoc = dbHelper.GetDataTable("Select * From LOCATION_VUE Where MARKET_CODE = '" + Convert.ToInt32(u.MARKET_CODE).ToString("D3") + "'");

                        if (dtDoc.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            rmks = rmks + "No Location Data Found, ";
                        }

                    }
                }

                #endregion

                //#region COMMITMENT



                //if (string.IsNullOrEmpty(u.COMMITMENT))
                //{
                //    rmks = rmks + "Commitment is Empty, ";
                //}
                //else
                //{
                //    DataTable dtDoc = dbHelper.GetDataTable("Select * From PRODUCT Where PROD_ID = '" + u.PRODUCT_CODE + "'");

                //    if (dtDoc.Rows.Count > 0)
                //    {

                //    }
                //    else
                //    {
                //        rmks = rmks + "Invalid Product Code, ";
                //    }
                //}

                //#endregion

                //#region Checking Management CODE

                //if (string.IsNullOrEmpty(u.MGMT_CODE))
                //{
                //    rmks = rmks + "Management Code is Empty, ";
                //}
                //else
                //{
                //    DataTable dtDoc = dbHelper.GetDataTable("Select * From MANAGEMENT_TEAM Where MGMT_CODE = " + Convert.ToInt32(u.MGMT_CODE).ToString("D8"));

                //    if (dtDoc.Rows.Count > 0)
                //    {
                //    }
                //    else
                //    {
                //        rmks = rmks + "Invalid Management Code, ";
                //    }
                //}

                //#endregion

                #region Updating Data Remarks on Data Table

                var rowsToUpdate = dt.AsEnumerable().Where(r => r.Field<string>("DOCTOR_ID") == u.DOCTOR_ID);

                foreach (var row in rowsToUpdate)
                {
                    if (rmks.Length > 0)
                    {
                        row.SetField("DATA_REMARKS", rmks);
                    }
                    else
                    {
                        row.SetField("DATA_REMARKS", "OK");
                    }
                }

                #endregion

            }

            var cnt = dt.AsEnumerable().Where(r => r.Field<string>("DATA_REMARKS").ToString().ToUpper() != "OK");

            #region Creating Excel file for Error Data

            if (cnt != null && cnt.Count() > 0)
            {

                System.Data.DataTable dtExcel = ConvertToDataTable(results.ToList());
                //objExcel.WriteDataTableToExcel(dtExcel, "Session", fileName, "UserWorkinngSession");

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=ERROR_Report(Product_wise_Investment)_" + DateTime.Today.ToString("dd-MM-yyyy") + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                GridView GridView1 = new GridView();
                GridView1.DataSource = dtExcel;
                GridView1.DataBind();
                GridView1.RenderControl(hw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                ViewBag.Msg = "Error Found in Data, Please Check the generated Excel File";
                return View();
            }



            #endregion

            #region Else Insert Data to the Main Database

            else
            {
                string tag = string.Empty;
                string tagNo = DateTime.Now.ToString("ddMMyyyyHHmm");
                int chkCnt = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    DataTable marketDt;

                    if (dr["MARKET_CODE"].ToString().Length >= 3)
                    {
                        marketDt = dbHelper.GetDataTable("Select * From LOCATION_VUE Where MARKET_CODE = '" + dr["MARKET_CODE"].ToString() + "'");
                    }
                    else
                    {
                        marketDt = dbHelper.GetDataTable("Select * From LOCATION_VUE Where MARKET_CODE = '" + Convert.ToInt32(dr["MARKET_CODE"].ToString()).ToString("D3") + "'");
                    }

                    var mrkt = from t in marketDt.AsEnumerable()
                               select new
                               {
                                   DIVISION_CODE = t["DIVISION_CODE"].ToString(),
                                   ZONE_CODE = t["ZONE_CODE"].ToString(),
                                   REGION_CODE = t["REGION_CODE"].ToString(),
                                   TERRITORY_CODE = t["TERRITORY_CODE"].ToString()
                               };

                    //System.Data.DataTable honorDT = dbHelper.GetDataTable("Select B.CEILING_AMT, A.GRP_DSIG_CODE from Management_team a left join hon_wise_ceiling b on A.GRP_DSIG_CODE = B.GRP_DSIG_CODE where A.MGMT_CODE = '" + Convert.ToInt32(dr["MGMT_CODE"].ToString()).ToString("D8") + "' AND B.HONORARIUM_CODE = '" + Convert.ToInt32(dr["HONORARIUM_CODE"]).ToString("D2") + "'");

                    //var honor = from t in honorDT.AsEnumerable()
                    //            select new
                    //            {
                    //                CEILING_AMT = t["CEILING_AMT"].ToString(),
                    //                GRP_DSIG_CODE = t["GRP_DSIG_CODE"].ToString()
                    //            };

                    //tagNo = DateTime.Now.ToString("ddMMyyyyHHmm");


                    DataTable dtChck = dbHelper.GetDataTable("Select * From PROD_WISE_INVESTMENT_EXCEL Where " +
                    " DOCTOR_ID = '" + dr["DOCTOR_ID"].ToString() + "'  " +
                    " AND MARKET_CODE = '" + dr["MARKET_CODE"].ToString() + "'" +
                    " AND PRODUCT_CODE = '" + dr["PRODUCT_CODE"].ToString() + "'");

                    if (dtChck.Rows.Count > 0)
                    {
                        chkCnt++;
                    }
                    else
                    {
                        //DateTime today = Convert.ToDateTime(dr["HONOR_APROV_DATE"].ToString());
                        //DataTable dataTableSlNo = dbHelper.GetDataTable("SELECT MAX (HONOR_APROV_NO) FROM DOC_HONOR_PAID_INFO WHERE SUBSTR(HONOR_APROV_NO, 1,4) = '" + today.ToString("yyyy") + "' AND SUBSTR(HONOR_APROV_NO, 5,2) = '" + today.ToString("MM") + "'");
                        //string PreAprovNo = string.Empty;

                        //string HONOR_APROV_NO = "";
                        string marketCode = "";

                        //PreAprovNo = dataTableSlNo.Rows[0][0].ToString();

                        //if (PreAprovNo.Length > 0)
                        //{
                        //    long lastNumber = Convert.ToInt64(PreAprovNo) + 1;
                        //    HONOR_APROV_NO = lastNumber.ToString();
                        //}
                        //else
                        //{
                        //    HONOR_APROV_NO = today.ToString("yyyy") + today.ToString("MM") + "0001";
                        //}


                        if (dr["MARKET_CODE"].ToString().Length == 3)
                        {
                            marketCode = dr["MARKET_CODE"].ToString();
                        }
                        else if (dr["MARKET_CODE"].ToString().Length < 3)
                        {
                            marketCode = Convert.ToInt32(dr["MARKET_CODE"].ToString()).ToString("D3");
                        }

                        string qry = "";
                        //string qry = "INSERT INTO DOC_HONOR_PAID_INFO (MARKET_CODE,DOCTOR_ID,PRODUCT_CODE, COMMITMENT, DURATION_FROM, DURATION_TO)" +
                        //            "VALUES(" + dr["MARKET_CODE"].ToString() + ",'" + dr["DOCTOR_ID"].ToString() + "','" + dr["PRODUCT_CODE"].ToString() + "'," +
                        //            "'" + dr["COMMITMENT"].ToString() + "',(TO_DATE('" + Convert.ToDateTime(dr["DURATION_FROM"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')), (TO_DATE('" + Convert.ToDateTime(dr["DURATION_FROM"].ToString()).ToString("dd/MM/yyyy") + "')";

                        var duration_Form = dr["DURATION_FROM"].ToString();
                        var duration_To = dr["DURATION_TO"].ToString();


                        if (dr["DURATION_FROM"].ToString() != "" && dr["DURATION_TO"].ToString() != "")
                        {
                            qry = "INSERT INTO PROD_WISE_INVESTMENT_EXCEL (MARKET_CODE,DOCTOR_ID,PRODUCT_CODE, INVESTMENT_AMOUNT, TAG_NO, ENTERED_BY, ENTERED_DATE, ENTERED_TERMINAL, COMMITMENT, COMMITMENT_TYPE, DURATION_FROM, DURATION_TO, REMARKS)" +
                                   " VALUES('" + dr["MARKET_CODE"].ToString() + "','" + dr["DOCTOR_ID"].ToString() + "','" + dr["PRODUCT_CODE"].ToString() + "','" + dr["INVESTMENT_AMOUNT"].ToString() + "','" + tagNo + "','" + Session["USER_ID"].ToString() + "',(TO_DATE('" + DateTime.Today.ToString("dd'/'MM'/'yyyy") + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "', " +
                                   "'" + dr["COMMITMENT"].ToString() + "', '" + dr["COMMITMENT_TYPE"].ToString() + "',(TO_DATE('" + Convert.ToDateTime(dr["DURATION_FROM"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),(TO_DATE('" + Convert.ToDateTime(dr["DURATION_TO"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),'" + dr["REMARKS"].ToString() + "')";
                        }
                        else
                        {
                            qry = "INSERT INTO PROD_WISE_INVESTMENT_EXCEL (MARKET_CODE,DOCTOR_ID,PRODUCT_CODE, INVESTMENT_AMOUNT, TAG_NO, ENTERED_BY, ENTERED_DATE, ENTERED_TERMINAL, COMMITMENT, COMMITMENT_TYPE,  DURATION_FROM, DURATION_TO, REMARKS)" +
                                   " VALUES('" + dr["MARKET_CODE"].ToString() + "','" + dr["DOCTOR_ID"].ToString() + "','" + dr["PRODUCT_CODE"].ToString() + "','" + dr["INVESTMENT_AMOUNT"].ToString() + "','" + tagNo + "','" + Session["USER_ID"].ToString() + "',(TO_DATE('" + DateTime.Today.ToString("dd'/'MM'/'yyyy") + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "', " +
                                   "'" + dr["COMMITMENT"].ToString() + "','" + dr["COMMITMENT_TYPE"].ToString() + "','" + null + "', '" + null + "','" + dr["REMARKS"].ToString() + "')";
                        }


                        dbHelper.CmdExecute(qry);
                    }
                }

                dbHelper.CmdExecute("DELETE FROM TEMP_PROD_WISE_INVESTMENT");

                int ttlRow = dt.Rows.Count;

                if (ttlRow == chkCnt)
                {
                    ViewBag.Tag = tag;
                    ViewBag.Msg = "All Repeated Data. No Data Saved !!!";
                }
                else
                {
                    ViewBag.Tag = tag;
                    ViewBag.Msg = "Data Successfully Saved. Tracking No: " + tagNo + ". Repeated Data: " + chkCnt.ToString();
                }
            }

            #endregion

            return View();
        }

        #endregion

        #region Upload Excel File And Insert Into Temp Table

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
            int cnt = 0;
            List<DoctorHonorariumPaidModel> doctorHonorPaidList = new List<DoctorHonorariumPaidModel>();
            try
            {
                if (string.IsNullOrEmpty(TempData["file"].ToString()))
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

                //string HONOR_APROV_DATE_str = string.Empty, NATIONAL_str = string.Empty, MGMT_CODE_str = string.Empty;
                string INSTITUTE_ID_str = string.Empty;
                //EXP_AMOUNT_str = string.Empty, 
                string MARKET_CODE_str = string.Empty;
                string PRODUCT_CODE_str = string.Empty;

                string DURATION_FROM_DATE_str = string.Empty;
                string DURATION_TO_DATE_str = string.Empty;


                string INVESTMENT_AMOUNT_str = string.Empty, EXP_AMT_TO_DATE_str = string.Empty, DOCTOR_ID_str = string.Empty.Trim();
                string REMARKS_str = string.Empty, HONORARIUM_CODE_str = string.Empty, EXPENSE_DETAILS_str = string.Empty;

                //string PRESENT_SHARE_str = string.Empty, PRESENT_FROM_DATE_str = string.Empty, 

                string COMMITMENT_str = string.Empty, COMMITMENT_TYPE_str = string.Empty;

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

                string query = String.Format("SELECT * from [{0}$]", "Sheet1");
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connectionString);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                System.Data.DataTable myDataTable = dataSet.Tables[0];

                //Check No 1
                if (dataSet.Tables[0].Columns.Count == 9)
                {
                    cnt = 1;
                    dbHelper.CmdExecute("DELETE FROM TEMP_PROD_WISE_INVESTMENT");
                    //Check No 2
                    foreach (DataRow dr in myDataTable.Rows)
                    {
                        DataTable dt = dbHelper.GetDataTable("SELECT MAX(HONOR_PAID_SLNO) as SL_No, MAX(HONOR_APROV_NO) as Approve_No from DOC_HONOR_PAID_INFO");
                        int paidSl = int.Parse(dt.Rows[0][0].ToString());

                        #region Value Checking

                        //#region MGMT_CODE

                        ////if (string.IsNullOrEmpty(dr["APPROVED_BY_CODE"].ToString()))
                        //if (string.IsNullOrEmpty(dr[1].ToString()))
                        //{
                        //    MGMT_CODE_str = string.Empty;
                        //}
                        //else
                        //{
                        //    //MGMT_CODE_str = dr["APPROVED_BY_CODE"].ToString();
                        //    MGMT_CODE_str = dr[1].ToString();
                        //}

                        //#endregion

                        //#region HONOR_APROV_DATE

                        ////if (string.IsNullOrEmpty(dr["HONOR_APROV_DATE"].ToString()))
                        //if (string.IsNullOrEmpty(dr[0].ToString()))
                        //{
                        //    HONOR_APROV_DATE_str = string.Empty;
                        //}
                        //else
                        //{
                        //    //HONOR_APROV_DATE_str = Convert.ToDateTime(dr["HONOR_APROV_DATE"]).ToString("dd/MM/yyyy");
                        //    HONOR_APROV_DATE_str = Convert.ToDateTime(dr[0]).ToString("dd/MM/yyyy");
                        //}

                        //#endregion

                        //#region NATIONAL

                        ////if (string.IsNullOrEmpty(dr["NATIONAL"].ToString()))
                        //if (string.IsNullOrEmpty(dr[3].ToString()))
                        //{
                        //    NATIONAL_str = "N";
                        //}
                        //else
                        //{
                        //    //NATIONAL_str = dr["NATIONAL"].ToString();
                        //    NATIONAL_str = dr[3].ToString();
                        //}

                        //#endregion

                        #region MARKET_CODE

                        //if (string.IsNullOrEmpty(dr["MARKET_CODE"].ToString()))
                        if (string.IsNullOrEmpty(dr[0].ToString()))
                        {
                            MARKET_CODE_str = string.Empty;
                        }
                        else
                        {
                            //MARKET_CODE_str = dr["MARKET_CODE"].ToString();
                            MARKET_CODE_str = dr[0].ToString();
                        }

                        #endregion




                        //#region SOCIETY_ID

                        ////if (string.IsNullOrEmpty(dr["SOCIETY_ID"].ToString()))
                        //if (string.IsNullOrEmpty(dr[5].ToString()))
                        //{
                        //    SOCIETY_ID_str = string.Empty;
                        //}
                        //else
                        //{
                        //    //SOCIETY_ID_str = dr["SOCIETY_ID"].ToString();
                        //    SOCIETY_ID_str = dr[5].ToString();
                        //}

                        //#endregion

                        #region DOCTOR_ID

                        //if (string.IsNullOrEmpty(dr["DOCTOR_ID"].ToString()))
                        if (string.IsNullOrEmpty(dr[1].ToString()) && string.IsNullOrWhiteSpace(dr[1].ToString()))
                        {
                            DOCTOR_ID_str = string.Empty.Trim();
                        }
                        else
                        {
                            //DOCTOR_ID_str = dr["DOCTOR_ID"].ToString();
                            DOCTOR_ID_str = dr[1].ToString();
                        }

                        #endregion

                        #region PRODUCT_CODE

                        //if (string.IsNullOrEmpty(dr["HONORARIUM_TYPE_CODE"].ToString()))
                        if (string.IsNullOrEmpty(dr[2].ToString()))
                        {
                            PRODUCT_CODE_str = string.Empty;
                        }
                        else
                        {
                            //HONORARIUM_CODE_str = dr["HONORARIUM_TYPE_CODE"].ToString();
                            PRODUCT_CODE_str = dr[2].ToString();
                        }

                        #endregion

                        #region INVESTMENT_AMOUNT

                        //if (string.IsNullOrEmpty(dr["MARKET_CODE"].ToString()))
                        if (string.IsNullOrEmpty(dr[3].ToString()))
                        {
                            INVESTMENT_AMOUNT_str = string.Empty;
                        }
                        else
                        {
                            //MARKET_CODE_str = dr["MARKET_CODE"].ToString();
                            INVESTMENT_AMOUNT_str = dr[3].ToString();
                        }

                        #endregion

                        #region COMMITMENT

                        //if (string.IsNullOrEmpty(dr["DETAIL_EXPENSE"].ToString()))
                        if (string.IsNullOrEmpty(dr[4].ToString()))
                        {
                            COMMITMENT_str = string.Empty;
                        }
                        else
                        {
                            //EXPENSE_DETAILS_str = dr["DETAIL_EXPENSE"].ToString();
                            COMMITMENT_str = dr[4].ToString();
                        }

                        #endregion

                        #region COMMITMENT_TYPE

                        //if (string.IsNullOrEmpty(dr["INSTITUTE_ID"].ToString()))
                        if (string.IsNullOrEmpty(dr[5].ToString()))
                        {
                            COMMITMENT_TYPE_str = string.Empty;
                        }
                        else
                        {
                            //INSTITUTE_ID_str = dr["INSTITUTE_ID"].ToString();
                            COMMITMENT_TYPE_str = dr[5].ToString();
                        }

                        #endregion

                        //#region EXP_AMOUNT

                        ////if (string.IsNullOrEmpty(dr["EXP_AMOUNT"].ToString()))
                        //if (string.IsNullOrEmpty(dr[10].ToString()))
                        //{
                        //    EXP_AMOUNT_str = string.Empty;
                        //}
                        //else
                        //{
                        //    //EXP_AMOUNT_str = dr["EXP_AMOUNT"].ToString();
                        //    EXP_AMOUNT_str = dr[10].ToString();
                        //}

                        //#endregion

                        //#region EXP_AMT_FROM_DATE

                        ////if (string.IsNullOrEmpty(dr["EXP_AMT_FROM_DATE"].ToString()))
                        //if (string.IsNullOrEmpty(dr[11].ToString()))
                        //{
                        //    EXP_AMT_FROM_DATE_str = string.Empty;
                        //}
                        //else
                        //{
                        //    //EXP_AMT_FROM_DATE_str = Convert.ToDateTime(dr["EXP_AMT_FROM_DATE"]).ToString("dd/MM/yyyy");
                        //    EXP_AMT_FROM_DATE_str = Convert.ToDateTime(dr[11]).ToString("dd/MM/yyyy");
                        //}

                        //#endregion

                        //#region EXP_AMT_TO_DATE

                        ////if (string.IsNullOrEmpty(dr["EXP_AMT_TO_DATE"].ToString()))
                        //if (string.IsNullOrEmpty(dr[12].ToString()))
                        //{
                        //    EXP_AMT_TO_DATE_str = string.Empty;
                        //}
                        //else
                        //{
                        //    //EXP_AMT_TO_DATE_str = Convert.ToDateTime(dr["EXP_AMT_TO_DATE"]).ToString("dd/MM/yyyy");
                        //    EXP_AMT_TO_DATE_str = Convert.ToDateTime(dr[12]).ToString("dd/MM/yyyy");
                        //}

                        //#endregion

                        //#region PRESENT_SHARE

                        ////if (string.IsNullOrEmpty(dr["PRESENT_SHARE"].ToString()))
                        //if (string.IsNullOrEmpty(dr[13].ToString()))
                        //{
                        //    PRESENT_SHARE_str = string.Empty;
                        //}
                        //else
                        //{
                        //    //PRESENT_SHARE_str = dr["PRESENT_SHARE"].ToString();
                        //    PRESENT_SHARE_str = dr[13].ToString();
                        //}

                        //#endregion

                        #region DURATION_FROM_DATE
                        string DURATION_FROM_DATE = dr[6].ToString();


                        //if (string.IsNullOrEmpty(dr["PRESENT_FROM_DATE"].ToString()))
                        if (dr[6].ToString() == "")
                        {
                            DURATION_FROM_DATE_str = string.Empty;
                        }
                        else
                        {
                            //PRESENT_FROM_DATE_str = Convert.ToDateTime(dr["PRESENT_FROM_DATE"]).ToString("dd/MM/yyyy");
                            DURATION_FROM_DATE_str = Convert.ToDateTime(dr[6]).ToString("dd/MM/yyyy");
                        }

                        #endregion

                        #region DURATION_TO_DATE

                        //if (string.IsNullOrEmpty(dr["PRESENT_TO_DATE"].ToString()))
                        if (string.IsNullOrEmpty(dr[7].ToString()))
                        {
                            DURATION_TO_DATE_str = string.Empty;
                        }
                        else
                        {
                            //PRESENT_TO_DATE_str = Convert.ToDateTime(dr["PRESENT_TO_DATE"]).ToString("dd/MM/yyyy");
                            DURATION_TO_DATE_str = Convert.ToDateTime(dr[7]).ToString("dd/MM/yyyy");
                        }

                        #endregion

                        #region Remarks

                        //if (string.IsNullOrEmpty(dr["COMMITMENT_SHARE"].ToString()))
                        if (string.IsNullOrEmpty(dr[8].ToString()))
                        {
                            REMARKS_str = string.Empty;
                        }
                        else
                        {
                            //COMMITMENT_SHARE_str = dr["COMMITMENT_SHARE"].ToString();
                            REMARKS_str = dr[8].ToString();
                        }

                        #endregion

                        #endregion

                        //string qry = "INSERT INTO TEMP_PROD_WISE_INVESTMENT (MARKET_CODE,DOCTOR_ID,PRODUCT_CODE, COMMITMENT, DURATION_FROM, DURATION_TO)" +
                        //            " VALUES('" + dr["MARKET_CODE"].ToString() + "','" + dr["DOCTOR_ID"].ToString() + "','" + dr["PRODUCT_CODE"].ToString() + "', " +
                        //            "'" + dr["COMMITMENT"].ToString() + "',(TO_DATE('" + Convert.ToDateTime(dr["DURATION_FROM"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),(TO_DATE('" + Convert.ToDateTime(dr["DURATION_TO"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')))";

                        string qry = "INSERT INTO TEMP_PROD_WISE_INVESTMENT (MARKET_CODE,DOCTOR_ID,PRODUCT_CODE, INVESTMENT_AMOUNT, COMMITMENT, COMMITMENT_TYPE, DURATION_FROM, DURATION_TO, REMARKS)" +
                                    " VALUES('" + dr["MARKET_CODE"].ToString() + "','" + dr["DOCTOR_ID"].ToString() + "','" + dr["PRODUCT_CODE"].ToString() + "','" + dr["INVESTMENT_AMOUNT"].ToString() + "', " +
                                    "'" + dr["COMMITMENT"].ToString() + "','" + dr["COMMITMENT_TYPE"].ToString() + "', (TO_DATE('" + DURATION_FROM_DATE_str + "','dd/MM/yyyy')), (TO_DATE('" + DURATION_TO_DATE_str + "','dd/MM/yyyy')),'" + dr["REMARKS"].ToString() + "')";



                        dbHelper.CmdExecute(qry);
                        cnt++;
                    }
                    ViewBag.Msg = "Data Successfully Loaded to Database, Press Save to Continue...";
                }
                else
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Invalid Excel File Format";
                    return Json(new { msg = _vmMsg }, JsonRequestBehavior.AllowGet);
                }
                Remove(TempData["file"].ToString());

                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Excel File Uploaded Successfully";

            }
            catch (Exception ex)
            {
                if (fileName == "")
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Invalid Data Format";
                }
                else
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Failed To Upload Excel File.";
                }
            }
            return Json(new { msg = _vmMsg }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public System.Data.DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            System.Data.DataTable table = new System.Data.DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        public ActionResult Remove(string fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

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

            // Return an empty string to signify success
            return Content("");
        }
    }
}