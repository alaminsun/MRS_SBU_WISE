using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MRS.DAL.Gateway;
using System.IO;
using System.Data.OleDb;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using MRS.DAL.Common;
using System.Globalization;

namespace MRS.Controllers
{
    public class DoctorHonorariumPaidController : Controller
    {
        DoctorHonorariumPaidDAO doctorHonorariumPaidDao;
        private ValidationMsg _vmMsg;
        DBHelper dbHelper = new DBHelper();

        string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt", 
                         "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss", 
                         "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt", 
                         "M/d/yyyy h:mm", "M/d/yyyy h:mm", 
                         "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm",
                         "MM/d/yyyy HH:mm:ss.ffffff", "dd/MM/yyyy hh:mm",
                         "d/M/yyyy", "dd-MM-yyyy", "dd-MM-yyyy hh:mm tt",
                         "dd/MM/yyyy hh:mm tt", "dd-MMM-yy hh:mm:ss tt"};

        public DoctorHonorariumPaidController()
        {
            _vmMsg = new ValidationMsg();
            doctorHonorariumPaidDao = new DoctorHonorariumPaidDAO();
        }

        #region FrmDoctorHonorariumPaid

        public ActionResult FrmDoctorHonorariumPaid()
        {
            {
                if (Session["UserId"] != null)
                {
                    ViewBag.formTitle = "Investment Database System";
                    return View();
                }
                return RedirectToAction("Login", "Home");
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(DoctorHonorariumPaidModel model)
        {
            //_vmMsg = model.SUStatus == 0 ? doctorHonorariumPaidDao.Save(model, Session["mappingUserID"].ToString()) : doctorHonorariumPaidDao.Update(model, Session["mappingUserID"].ToString());
            _vmMsg = model.SUStatus == 0 ? doctorHonorariumPaidDao.Save(model, Session["USER_ID"].ToString(), Session["roleID"].ToString()) : doctorHonorariumPaidDao.Update(model, Session["USER_ID"].ToString(), Session["roleID"].ToString());
            return Json(new { Code = doctorHonorariumPaidDao.GetHONORPAIDSLNO(), AprovNo = doctorHonorariumPaidDao.GetAprovNo(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetHonarariumPaidLocationList( string DivisionCode, string regionCode, int userID, int roleID)
        {
            int groupDsigCode = Convert.ToInt32(Session["GRP_DSIG_CODE"]);
            var locationList = doctorHonorariumPaidDao.GetHonarariumPaidLocationList(DivisionCode, regionCode, userID, roleID, groupDsigCode);
            var doctorLocationList = Json(locationList, JsonRequestBehavior.AllowGet);
            doctorLocationList.MaxJsonLength = int.MaxValue;
            return doctorLocationList;
        }

        [HttpPost]
        public ActionResult GetManagement(int managementCode)
        {
            var tcL1L2List = doctorHonorariumPaidDao.GetManagement(managementCode);
            var doctorLocationList = Json(tcL1L2List, JsonRequestBehavior.AllowGet);
            doctorLocationList.MaxJsonLength = int.MaxValue;
            return doctorLocationList;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetManagementList()
        {
            var tcL1L2List = doctorHonorariumPaidDao.GetManagementList();
            var doctorLocationList = Json(tcL1L2List, JsonRequestBehavior.AllowGet);
            doctorLocationList.MaxJsonLength = int.MaxValue;
            return doctorLocationList;
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult GetDoctorList()
        //{
        //    var data = doctorHonorariumPaidDao.GetDoctorList();
        //    var doctorDetailList = Json(data, JsonRequestBehavior.AllowGet);
        //    doctorDetailList.MaxJsonLength = int.MaxValue;
        //    return doctorDetailList;
        //}




        [HttpPost]
        public JsonResult GetDoctorsDataRSMandDSM(int doctorId, string doctorName, string degree, string regionCode, string DivisionCode, int userRole, int userID)
        {
            int groupDsigCode = Convert.ToInt32(Session["GRP_DSIG_CODE"]);
            var listData = doctorHonorariumPaidDao.GetDoctorListRSMandDSM(doctorId, doctorName, degree, regionCode, DivisionCode, userRole, userID, groupDsigCode);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }

        [HttpPost]
        public JsonResult GetDoctorsData(int doctorId, string doctorName, string degree)
        {
            var listData = doctorHonorariumPaidDao.GetDoctorList(doctorId, doctorName, degree);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }

        //[HttpPost]
        //public JsonResult GetDoctorsDataRSM(int doctorId, string doctorName, string degree, string regionCode)
        //{
        //    var listData = doctorHonorariumPaidDao.GetDoctorListRSM(doctorId, doctorName, degree, regionCode);
        //    var data = Json(listData, JsonRequestBehavior.AllowGet);
        //    data.MaxJsonLength = int.MaxValue;
        //    return data;
        //}

        //[HttpPost]
        //public JsonResult GetDoctorsDataDSM(int doctorId, string doctorName, string degree, string DivisionCode)
        //{
        //    var listData = doctorHonorariumPaidDao.GetDoctorListDSM(doctorId, doctorName, degree, DivisionCode);
        //    var data = Json(listData, JsonRequestBehavior.AllowGet);
        //    data.MaxJsonLength = int.MaxValue;
        //    return data;
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetInstitutionList(string regionCode, string DivisionCode, int userID, int roleID)
        {
            int groupDsigCode = Convert.ToInt32(Session["GRP_DSIG_CODE"]);
            var tcL1L2List = doctorHonorariumPaidDao.GetInstitutionList(regionCode, DivisionCode, userID, roleID, groupDsigCode);
            var doctorLocationList = Json(tcL1L2List, JsonRequestBehavior.AllowGet);
            doctorLocationList.MaxJsonLength = int.MaxValue;
            return doctorLocationList;
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult GetHonarariumTypeList()
        //{
        //    var tcL1L2List = doctorHonorariumPaidDao.GetHonarariumTypeList();
        //    var doctorLocationList = Json(tcL1L2List, JsonRequestBehavior.AllowGet);
        //    doctorLocationList.MaxJsonLength = int.MaxValue;
        //    return doctorLocationList;
        //}
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetHonarariumTypeList()
        {
            var tcL1L2List = doctorHonorariumPaidDao.GetHonarariumTypeList();
            var doctorLocationList = Json(tcL1L2List, JsonRequestBehavior.AllowGet);
            doctorLocationList.MaxJsonLength = int.MaxValue;
            return doctorLocationList;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSocietyAssociationList()
        {
            var tcL1L2List = doctorHonorariumPaidDao.GetSocietyAssociationList();
            var doctorLocationList = Json(tcL1L2List, JsonRequestBehavior.AllowGet);
            doctorLocationList.MaxJsonLength = int.MaxValue;
            return doctorLocationList;
        }

        [HttpPost]
        public JsonResult GetDoctorInfo(int DoctorId, string regionCode, string DivisionCode, int roleID, int userID)
        {
            int groupDsigCode = Convert.ToInt32(Session["GRP_DSIG_CODE"]);
            string name = doctorHonorariumPaidDao.GetDoctorInfo(DoctorId, regionCode, DivisionCode, roleID, userID, groupDsigCode);
            return Json(new { Name = name });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProductList()
        {
            var tcL1L2List = doctorHonorariumPaidDao.GetProductList();
            var doctorLocationList = Json(tcL1L2List, JsonRequestBehavior.AllowGet);
            doctorLocationList.MaxJsonLength = int.MaxValue;
            return doctorLocationList;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetSearchList(string approvNo, int doctorId, string doctorName, int donAmount, int roleID, int userID)
        {
            int groupDsigCode = Convert.ToInt32(Session["GRP_DSIG_CODE"]);
            var tcL1L2SearchList = doctorHonorariumPaidDao.GetSearchList(approvNo, doctorId, doctorName, donAmount, roleID, userID, groupDsigCode);
            var jsonResult = Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string HONOR_PAID_SLNO)
        {
            _vmMsg = doctorHonorariumPaidDao.Delete(HONOR_PAID_SLNO);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedDocCommitProduct(string DOC_COMMI_PROD_SLNO)
        {
            _vmMsg = doctorHonorariumPaidDao.DeletedDocCommitProduct(DOC_COMMI_PROD_SLNO);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetCeilingAmount(string MGMT_CODE, string HONORARIUM_NAME, string GRP_DSIG_CODE, string month, string year)
        {
            var amount = doctorHonorariumPaidDao.GetCeilingAmount(MGMT_CODE, HONORARIUM_NAME, GRP_DSIG_CODE, month, year);
            return Json(amount, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSaveDocCommitProductList(string HONOR_PAID_SLNO)
        {
            var DocCommitProductList = doctorHonorariumPaidDao.GetSaveDocCommitProductList(HONOR_PAID_SLNO);
            return Json(DocCommitProductList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestAprovNoList()
        {
            var allData = doctorHonorariumPaidDao.GetSuggestAprovNoList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInstiID(int instCode, string regionCode, string DivisionCode, int roleID, int userID)
        {
            int groupDsigCode = Convert.ToInt32(Session["GRP_DSIG_CODE"]);
            string name = doctorHonorariumPaidDao.GetInstituteInfo(instCode, regionCode, DivisionCode, roleID, userID, groupDsigCode);
            return Json(new { Name = name });
        }

        [HttpPost]
       public JsonResult GetMarketID(string marketCode, string regionCode, string DivisionCode, int roleID, int userID)
       {
           int groupDsigCode = Convert.ToInt32(Session["GRP_DSIG_CODE"]);
            DoctorHonorariumPaidModel dmc = new DoctorHonorariumPaidModel();
            dmc = doctorHonorariumPaidDao.GetMarketInfo(marketCode, regionCode, DivisionCode, roleID, userID, groupDsigCode);
            return Json(dmc);
        }

        [HttpPost]
        public JsonResult GetShareData(string Presc_Share_From, string Presc_Share_To, string Market_Code, string Doctor_ID, string Insti_Code)
        {
            DoctorHonorariumPaidModel dmc = new DoctorHonorariumPaidModel();
            dmc = doctorHonorariumPaidDao.GetShareData(Presc_Share_From, Presc_Share_To, Market_Code, Doctor_ID, Insti_Code);
            return Json(dmc);
        }

        [HttpPost]
        public JsonResult GetDonationInfoByDoctorID(string DoctorId)
        {
            DoctorInvestmentModel dmc = new DoctorInvestmentModel();
            dmc = doctorHonorariumPaidDao.GetDonationInfoByDoctorID(Convert.ToInt32(DoctorId));
            return Json(dmc);
        }


        #endregion

        #region Excel Loader
        public ActionResult FrmInvestmentDBSysExcel()
        {
            {
                if (Session["UserId"] != null)
                {
                    ViewBag.formTitle = "Investment Database System : Excel Loader";
                    return View();
                }
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        public ActionResult FrmInvestmentDBSysExcel(DoctorHonorariumPaidModel pdr)
        {
            // Get Temp User Session Table Data
            //System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_DOC_HONOR_PAID_INFO Where HONOR_APROV_NO = '2016060001'");
            System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_DOC_HONOR_PAID_INFO ORDER BY HONOR_PAID_SLNO");
            var results = from t in dt.AsEnumerable()
                          select new
                          {
                              HONOR_PAID_SLNO = t["HONOR_PAID_SLNO"].ToString(),
                              HONOR_APROV_NO = t["HONOR_APROV_NO"].ToString(),
                              HONOR_APROV_DATE = t["HONOR_APROV_DATE"].ToString(),
                              MGMT_CODE = t["MGMT_CODE"].ToString(),
                              MARKET_CODE = t["MARKET_CODE"].ToString(),                         
                              NATIONAL = t["NATIONAL"].ToString(),
                              SOCIETY_ID = t["SOCIETY_ID"].ToString(),
                              INSTI_CODE = t["INSTI_CODE"].ToString(),
                              DOCTOR_ID = t["DOCTOR_ID"].ToString(),
                              HONORARIUM_CODE = t["HONORARIUM_CODE"].ToString(),
                              EXPENSE_DETAILS = t["EXPENSE_DETAILS"].ToString(),
                              EXPENSE_AMT = t["EXPENSE_AMT"].ToString(),
                              EXPENSE_FROM = t["EXPENSE_FROM"].ToString(),
                              EXPENSE_TO = t["EXPENSE_TO"].ToString(),
                              PRESC_SHARE_PCT = t["PRESC_SHARE_PCT"].ToString(),
                              PRESC_SHARE_FROM = t["PRESC_SHARE_FROM"].ToString(),
                              PRESC_SHARE_TO = t["PRESC_SHARE_TO"].ToString(),
                              COMITMENT_SHARE_PCT = t["COMITMENT_SHARE_PCT"].ToString(),
                              ENTERED_BY = t["ENTERED_BY"].ToString(),
                              DATA_REMARKS = t["DATA_REMARKS"].ToString(),
                          };

            foreach (var u in results)
            {
                string rmks = "";

                #region Checking Expense From Date

                if (string.IsNullOrEmpty(u.EXPENSE_FROM))
                {
                    rmks = rmks + "Expense From Date is Empty, ";
                }

                #endregion

                #region Checking Expense To Date

                if (string.IsNullOrEmpty(u.EXPENSE_TO))
                {
                    rmks = rmks + "Expense To Date is Empty, ";
                }

                #endregion

                #region Checking Expense From Date

                if (string.IsNullOrEmpty(u.PRESC_SHARE_FROM))
                {
                    rmks = rmks + "Present Share From Date is Empty, ";
                }

                #endregion

                #region Checking Expense To Date

                if (string.IsNullOrEmpty(u.PRESC_SHARE_TO))
                {
                    rmks = rmks + "Present Share To Date is Empty, ";
                }

                #endregion

                #region Checking Honorarium Approve Date

                if (string.IsNullOrEmpty(u.HONOR_APROV_DATE))
                {
                    rmks = rmks + "Honor Approved Date is Empty, ";
                }

                #endregion
                
                #region Checking Doctor ID / Institute ID / Society ID

                if (string.IsNullOrEmpty(u.DOCTOR_ID) && string.IsNullOrEmpty(u.INSTI_CODE) && string.IsNullOrEmpty(u.SOCIETY_ID))
                {
                    rmks = rmks + "Doctor ID or Institute ID or Society ID is Required, ";
                }
                else
                {
                    if (! string.IsNullOrEmpty(u.INSTI_CODE))
                    {
                        DataTable dtInst = dbHelper.GetDataTable("Select * From INSTITUTION Where INSTI_CODE = " + u.INSTI_CODE);
                        if (dtInst.Rows.Count > 0)
                        {
                        }
                        else
                        {
                            rmks = rmks + "Invalid Institution Code, ";
                        }
                    }

                    else if (!string.IsNullOrEmpty(u.DOCTOR_ID))
                    {
                        DataTable dtDoc = dbHelper.GetDataTable("Select * From Doctor Where DOCTOR_ID = " + u.DOCTOR_ID);

                        if (dtDoc.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            rmks = rmks + "Invalid Doctor Code, ";
                        }                        
                    }
                    else
                    {
                        DataTable dtDoc = dbHelper.GetDataTable("Select * From DOCTOR_SOCITY Where SOCIETY_ID = " + u.SOCIETY_ID);

                        if (dtDoc.Rows.Count > 0)
                        {

                        }
                        else
                        {
                            rmks = rmks + "Invalid Society ID, ";
                        }
                    }
                }

                #endregion

                #region Checking Honorarium Code

                if (string.IsNullOrEmpty(u.HONORARIUM_CODE))
                {
                    rmks = rmks + "Honorarium Code is Empty, ";
                }
                else
                {
                    DataTable dtHonor = dbHelper.GetDataTable("Select * From HONORARIUM_TYPE Where HONORARIUM_CODE = " + u.HONORARIUM_CODE);
                    
                    if (dtHonor.Rows.Count > 0)
                    {
                    }
                    else
                    {
                        rmks = rmks + "Invalid Honorarium Code, ";
                    }
                }

                #endregion

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

                #region Expense Details

                if (string.IsNullOrEmpty(u.EXPENSE_DETAILS))
                {
                    rmks = rmks + "Expense Details is Empty, ";
                }

                #endregion

                #region Checking Management CODE

                if (string.IsNullOrEmpty(u.MGMT_CODE))
                {
                    rmks = rmks + "Management Code is Empty, ";
                }
                else
                {
                    DataTable dtDoc = dbHelper.GetDataTable("Select * From MANAGEMENT_TEAM Where MGMT_CODE = " + Convert.ToInt32(u.MGMT_CODE).ToString("D8"));

                    if (dtDoc.Rows.Count > 0)
                    {
                    }
                    else
                    {
                        rmks = rmks + "Invalid Management Code, ";
                    }
                }

                #endregion

                #region Updating Data Remarks on Data Table

                var rowsToUpdate = dt.AsEnumerable().Where(r => r.Field<string>("HONOR_APROV_NO") == u.HONOR_APROV_NO);

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
                Response.AddHeader("content-disposition", "attachment;filename=ERROR_Report(Investment)_"+ DateTime.Today.ToString("dd-MM-yyyy")+".xls");
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

                    System.Data.DataTable honorDT = dbHelper.GetDataTable("Select B.CEILING_AMT, A.GRP_DSIG_CODE from Management_team a left join hon_wise_ceiling b on A.GRP_DSIG_CODE = B.GRP_DSIG_CODE where A.MGMT_CODE = '" + Convert.ToInt32(dr["MGMT_CODE"].ToString()).ToString("D8") + "' AND B.HONORARIUM_CODE = '" +  Convert.ToInt32(dr["HONORARIUM_CODE"]).ToString("D2") +"'");               

                    var honor = from t in honorDT.AsEnumerable()
                                select new
                                {
                                    CEILING_AMT = t["CEILING_AMT"].ToString(),
                                    GRP_DSIG_CODE = t["GRP_DSIG_CODE"].ToString()
                                };

                    tagNo = DateTime.Now.ToString("ddMMyyyyHHmm");


                    DataTable dtChck = dbHelper.GetDataTable("Select * From DOC_HONOR_PAID_INFO Where MGMT_CODE = '" + Convert.ToInt32(dr["MGMT_CODE"].ToString()).ToString("D8") + "' AND DOCTOR_ID = '" + dr["DOCTOR_ID"].ToString() + "' AND HONOR_APROV_DATE = (TO_DATE('" + Convert.ToDateTime(dr["HONOR_APROV_DATE"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')) AND HONORARIUM_CODE = '" + dr["HONORARIUM_CODE"].ToString() + "' AND EXPENSE_AMT = '" + dr["EXPENSE_AMT"].ToString() + "'");

                    if (dtChck.Rows.Count > 0)
                    {
                        chkCnt ++;
                    }
                    else
                    {
                        DateTime today = Convert.ToDateTime(dr["HONOR_APROV_DATE"].ToString());
                        DataTable dataTableSlNo = dbHelper.GetDataTable("SELECT MAX (HONOR_APROV_NO) FROM DOC_HONOR_PAID_INFO WHERE SUBSTR(HONOR_APROV_NO, 1,4) = '" + today.ToString("yyyy") + "' AND SUBSTR(HONOR_APROV_NO, 5,2) = '" + today.ToString("MM") + "'");
                        string PreAprovNo = string.Empty;

                        string HONOR_APROV_NO = "";
                        string marketCode = "";

                        PreAprovNo = dataTableSlNo.Rows[0][0].ToString();

                        if (PreAprovNo.Length > 0)
                        {
                            long lastNumber = Convert.ToInt64(PreAprovNo) + 1;
                            HONOR_APROV_NO = lastNumber.ToString();
                        }
                        else
                        {
                            HONOR_APROV_NO = today.ToString("yyyy") + today.ToString("MM") + "0001";
                        }


                        if (dr["MARKET_CODE"].ToString().Length == 3)
                        {
                            marketCode = dr["MARKET_CODE"].ToString();
                        }
                        else if (dr["MARKET_CODE"].ToString().Length < 3)
                        {
                            marketCode = Convert.ToInt32(dr["MARKET_CODE"].ToString()).ToString("D3");
                        }

                        string qry = "INSERT INTO DOC_HONOR_PAID_INFO (HONOR_PAID_SLNO,HONOR_APROV_NO,MGMT_CODE, HONOR_APROV_DATE, NATIONAL, MARKET_CODE, SOCIETY_ID, " +
                                    "DOCTOR_ID, HONORARIUM_CODE, EXPENSE_DETAILS, DIVISION_CODE, ZONE_CODE, REGION_CODE, TERRITORY_CODE," +
                                    "INSTI_CODE, EXPENSE_AMT,EXPENSE_FROM,EXPENSE_TO,PRESC_SHARE_PCT, PRESC_SHARE_FROM, PRESC_SHARE_TO, COMITMENT_SHARE_PCT, " +
                                    "ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL,TAG_NO, MON_CELI_AMT, GRP_DSIG_CODE)" +
                                    "VALUES(" + dr["HONOR_PAID_SLNO"].ToString() + ",'" + HONOR_APROV_NO + "','" + Convert.ToInt32(dr["MGMT_CODE"].ToString()).ToString("D8") + "',(TO_DATE('" + Convert.ToDateTime(dr["HONOR_APROV_DATE"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),'" + dr["NATIONAL"].ToString() + "','" +  marketCode /*dr["MARKET_CODE"].ToString()*/ + "','" + dr["SOCIETY_ID"].ToString() + "'," +
                                    "'" + dr["DOCTOR_ID"].ToString() + "','" + dr["HONORARIUM_CODE"].ToString() + "','" + dr["EXPENSE_DETAILS"].ToString() + "','" + mrkt.FirstOrDefault().DIVISION_CODE.ToString() + "','" + mrkt.FirstOrDefault().ZONE_CODE.ToString() + "','" + mrkt.FirstOrDefault().REGION_CODE.ToString() + "','" + mrkt.FirstOrDefault().TERRITORY_CODE.ToString() + "'," +
                                    "'" + dr["INSTI_CODE"].ToString() + "','" + dr["EXPENSE_AMT"].ToString() + "',(TO_DATE('" + Convert.ToDateTime(dr["EXPENSE_FROM"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),(TO_DATE('" + Convert.ToDateTime(dr["EXPENSE_TO"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),'" + dr["PRESC_SHARE_PCT"].ToString() + "', (TO_DATE('" + Convert.ToDateTime(dr["PRESC_SHARE_FROM"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),(TO_DATE('" + Convert.ToDateTime(dr["PRESC_SHARE_TO"].ToString()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),'" + dr["COMITMENT_SHARE_PCT"].ToString() + "'," +
                                    "'" + Session["USER_ID"].ToString() + "',(TO_DATE('" + DateTime.Today.ToString("dd'/'MM'/'yyyy") + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "','" + tagNo + "','" + honor.FirstOrDefault().CEILING_AMT.ToString() + "','" + honor.FirstOrDefault().GRP_DSIG_CODE.ToString() + "')";

                        dbHelper.CmdExecute(qry);
                    }  
                }

                dbHelper.CmdExecute("DELETE FROM TEMP_DOC_HONOR_PAID_INFO");

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

                string HONOR_APROV_DATE_str = string.Empty, NATIONAL_str = string.Empty, MGMT_CODE_str = string.Empty;
                string INSTITUTE_ID_str = string.Empty, EXP_AMOUNT_str = string.Empty, MARKET_CODE_str = string.Empty; 
                 //MARKET_NAME_str = string.Empty;


                string EXP_AMT_FROM_DATE_str = string.Empty, EXP_AMT_TO_DATE_str = string.Empty, DOCTOR_ID_str = string.Empty.Trim();
                string SOCIETY_ID_str = string.Empty, HONORARIUM_CODE_str = string.Empty, EXPENSE_DETAILS_str = string.Empty;

                string PRESENT_SHARE_str = string.Empty, PRESENT_FROM_DATE_str = string.Empty, PRESENT_TO_DATE_str = string.Empty;
                string COMMITMENT_SHARE_str = string.Empty;

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
                if (dataSet.Tables[0].Columns.Count == 17)
                {
                    cnt = 1;
                    dbHelper.CmdExecute("DELETE FROM TEMP_DOC_HONOR_PAID_INFO");
                    //Check No 2
                    foreach (DataRow dr in myDataTable.Rows)
                    {                       
                        DataTable dt = dbHelper.GetDataTable("SELECT MAX(HONOR_PAID_SLNO) as SL_No, MAX(HONOR_APROV_NO) as Approve_No from DOC_HONOR_PAID_INFO");
                        int paidSl = int.Parse(dt.Rows[0][0].ToString());

                        #region Value Checking

                        #region MGMT_CODE

                        //if (string.IsNullOrEmpty(dr["APPROVED_BY_CODE"].ToString()))
                        if (string.IsNullOrEmpty(dr[1].ToString()))
                        {
                            MGMT_CODE_str = string.Empty;
                        }
                        else
                        {
                            //MGMT_CODE_str = dr["APPROVED_BY_CODE"].ToString();
                            MGMT_CODE_str = dr[1].ToString();
                        }

                        #endregion

                        #region HONOR_APROV_DATE

                        //if (string.IsNullOrEmpty(dr["HONOR_APROV_DATE"].ToString()))
                        if (string.IsNullOrEmpty(dr[0].ToString()))
                        {
                            HONOR_APROV_DATE_str = string.Empty;
                        }
                        else
                        {
                            //HONOR_APROV_DATE_str = Convert.ToDateTime(dr["HONOR_APROV_DATE"]).ToString("dd/MM/yyyy");
                            HONOR_APROV_DATE_str = Convert.ToDateTime(dr[0]).ToString("dd/MM/yyyy");
                        }

                        #endregion

                        #region NATIONAL

                        //if (string.IsNullOrEmpty(dr["NATIONAL"].ToString()))
                        if (string.IsNullOrEmpty(dr[3].ToString()))
                        {
                            NATIONAL_str = "N";
                        }
                        else
                        {
                            //NATIONAL_str = dr["NATIONAL"].ToString();
                            NATIONAL_str = dr[3].ToString();
                        }

                        #endregion

                        #region MARKET_CODE

                        //if (string.IsNullOrEmpty(dr["MARKET_CODE"].ToString()))
                        if (string.IsNullOrEmpty(dr[4].ToString()))
                        {
                            MARKET_CODE_str = string.Empty;
                        }
                        else
                        {
                            //MARKET_CODE_str = dr["MARKET_CODE"].ToString();
                            MARKET_CODE_str = dr[4].ToString();
                        }

                        #endregion


                        //#region MARKET_NAME

                        ////if (string.IsNullOrEmpty(dr["MARKET_CODE"].ToString()))
                        //if (string.IsNullOrEmpty(dr[5].ToString()))
                        //{
                        //    MARKET_NAME_str = string.Empty;
                        //}
                        //else
                        //{
                        //    //MARKET_CODE_str = dr["MARKET_CODE"].ToString();
                        //    MARKET_NAME_str = dr[5].ToString();
                        //}

                        //#endregion

                        #region SOCIETY_ID

                        //if (string.IsNullOrEmpty(dr["SOCIETY_ID"].ToString()))
                        if (string.IsNullOrEmpty(dr[5].ToString()))
                        {
                            SOCIETY_ID_str = string.Empty;
                        }
                        else
                        {
                            //SOCIETY_ID_str = dr["SOCIETY_ID"].ToString();
                            SOCIETY_ID_str = dr[5].ToString();
                        }

                        #endregion

                        #region DOCTOR_ID

                        //if (string.IsNullOrEmpty(dr["DOCTOR_ID"].ToString()))
                        if (string.IsNullOrEmpty(dr[6].ToString()) && string.IsNullOrWhiteSpace(dr[6].ToString()))
                        {
                            DOCTOR_ID_str = string.Empty.Trim();
                        }
                        else
                        {
                            //DOCTOR_ID_str = dr["DOCTOR_ID"].ToString();
                            DOCTOR_ID_str = dr[6].ToString();
                        }

                        #endregion

                        #region HONORARIUM_CODE

                        //if (string.IsNullOrEmpty(dr["HONORARIUM_TYPE_CODE"].ToString()))
                        if (string.IsNullOrEmpty(dr[7].ToString()))
                        {
                            HONORARIUM_CODE_str = string.Empty;
                        }
                        else
                        {
                            //HONORARIUM_CODE_str = dr["HONORARIUM_TYPE_CODE"].ToString();
                            HONORARIUM_CODE_str = dr[7].ToString();
                        }

                        #endregion

                        #region EXPENSE_DETAILS

                        //if (string.IsNullOrEmpty(dr["DETAIL_EXPENSE"].ToString()))
                        if (string.IsNullOrEmpty(dr[8].ToString()))
                        {
                            EXPENSE_DETAILS_str = string.Empty;
                        }
                        else
                        {
                            //EXPENSE_DETAILS_str = dr["DETAIL_EXPENSE"].ToString();
                            EXPENSE_DETAILS_str = dr[8].ToString();
                        }

                        #endregion

                        #region INSTITUTE_ID

                        //if (string.IsNullOrEmpty(dr["INSTITUTE_ID"].ToString()))
                        if (string.IsNullOrEmpty(dr[9].ToString()))
                        {
                            INSTITUTE_ID_str = string.Empty;
                        }
                        else
                        {
                            //INSTITUTE_ID_str = dr["INSTITUTE_ID"].ToString();
                            INSTITUTE_ID_str = dr[9].ToString();
                        }

                        #endregion

                        #region EXP_AMOUNT

                        //if (string.IsNullOrEmpty(dr["EXP_AMOUNT"].ToString()))
                        if (string.IsNullOrEmpty(dr[10].ToString()))
                        {
                            EXP_AMOUNT_str = string.Empty;
                        }
                        else
                        {
                            //EXP_AMOUNT_str = dr["EXP_AMOUNT"].ToString();
                            EXP_AMOUNT_str = dr[10].ToString();
                        }

                        #endregion

                        #region EXP_AMT_FROM_DATE

                        //if (string.IsNullOrEmpty(dr["EXP_AMT_FROM_DATE"].ToString()))
                        if (string.IsNullOrEmpty(dr[11].ToString()))
                        {
                            EXP_AMT_FROM_DATE_str = string.Empty;
                        }
                        else
                        {
                            //EXP_AMT_FROM_DATE_str = Convert.ToDateTime(dr["EXP_AMT_FROM_DATE"]).ToString("dd/MM/yyyy");
                            EXP_AMT_FROM_DATE_str = Convert.ToDateTime(dr[11]).ToString("dd/MM/yyyy");
                        }

                        #endregion

                        #region EXP_AMT_TO_DATE

                        //if (string.IsNullOrEmpty(dr["EXP_AMT_TO_DATE"].ToString()))
                        if (string.IsNullOrEmpty(dr[12].ToString()))
                        {
                            EXP_AMT_TO_DATE_str = string.Empty;
                        }
                        else
                        {
                            //EXP_AMT_TO_DATE_str = Convert.ToDateTime(dr["EXP_AMT_TO_DATE"]).ToString("dd/MM/yyyy");
                            EXP_AMT_TO_DATE_str = Convert.ToDateTime(dr[12]).ToString("dd/MM/yyyy");
                        }

                        #endregion

                        #region PRESENT_SHARE

                        //if (string.IsNullOrEmpty(dr["PRESENT_SHARE"].ToString()))
                        if (string.IsNullOrEmpty(dr[13].ToString()))
                        {
                            PRESENT_SHARE_str = string.Empty;
                        }
                        else
                        {
                            //PRESENT_SHARE_str = dr["PRESENT_SHARE"].ToString();
                            PRESENT_SHARE_str = dr[13].ToString();
                        }

                        #endregion

                        #region PRESENT_FROM_DATE

                        //if (string.IsNullOrEmpty(dr["PRESENT_FROM_DATE"].ToString()))
                        if (string.IsNullOrEmpty(dr[14].ToString()))
                        {
                            PRESENT_FROM_DATE_str = string.Empty;
                        }
                        else
                        {
                            //PRESENT_FROM_DATE_str = Convert.ToDateTime(dr["PRESENT_FROM_DATE"]).ToString("dd/MM/yyyy");
                            PRESENT_FROM_DATE_str = Convert.ToDateTime(dr[14]).ToString("dd/MM/yyyy");
                        }

                        #endregion

                        #region PRESENT_TO_DATE

                        //if (string.IsNullOrEmpty(dr["PRESENT_TO_DATE"].ToString()))
                        if (string.IsNullOrEmpty(dr[15].ToString()))
                        {
                            PRESENT_TO_DATE_str = string.Empty;
                        }
                        else
                        {
                            //PRESENT_TO_DATE_str = Convert.ToDateTime(dr["PRESENT_TO_DATE"]).ToString("dd/MM/yyyy");
                            PRESENT_TO_DATE_str = Convert.ToDateTime(dr[15]).ToString("dd/MM/yyyy");
                        }

                        #endregion

                        #region COMMITMENT_SHARE

                        //if (string.IsNullOrEmpty(dr["COMMITMENT_SHARE"].ToString()))
                        if (string.IsNullOrEmpty(dr[16].ToString()))
                        {
                            COMMITMENT_SHARE_str = string.Empty;
                        }
                        else
                        {
                            //COMMITMENT_SHARE_str = dr["COMMITMENT_SHARE"].ToString();
                            COMMITMENT_SHARE_str = dr[16].ToString();
                        }

                        #endregion

                        #endregion

                        string qry = "INSERT INTO TEMP_DOC_HONOR_PAID_INFO (HONOR_PAID_SLNO,HONOR_APROV_NO,MGMT_CODE, HONOR_APROV_DATE, NATIONAL, MARKET_CODE, SOCIETY_ID, " +
                            "DOCTOR_ID, HONORARIUM_CODE, EXPENSE_DETAILS," +
                            "INSTI_CODE, EXPENSE_AMT,EXPENSE_FROM,EXPENSE_TO,PRESC_SHARE_PCT, PRESC_SHARE_FROM, PRESC_SHARE_TO, COMITMENT_SHARE_PCT)" +
                        "VALUES('" + (paidSl + cnt) + "','" + (paidSl + cnt) + "','" + MGMT_CODE_str + "',(TO_DATE('" + HONOR_APROV_DATE_str + "','dd/MM/yyyy')),'" + NATIONAL_str + "','" + MARKET_CODE_str + "'," +
                        "'" + SOCIETY_ID_str.Trim() + "','" + DOCTOR_ID_str.Trim() + "','" + HONORARIUM_CODE_str + "','" + EXPENSE_DETAILS_str + "'," +
                        "'" + INSTITUTE_ID_str.Trim() + "','" + EXP_AMOUNT_str + "',(TO_DATE('" + EXP_AMT_FROM_DATE_str + "','dd/MM/yyyy')),(TO_DATE('" + EXP_AMT_TO_DATE_str + "','dd/MM/yyyy')), '" + PRESENT_SHARE_str + "',(TO_DATE('" + PRESENT_FROM_DATE_str + "','dd/MM/yyyy')), (TO_DATE('" + PRESENT_TO_DATE_str + "','dd/MM/yyyy')), '" + COMMITMENT_SHARE_str + "' )";

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