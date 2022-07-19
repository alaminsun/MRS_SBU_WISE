using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRS.Controllers
{
    public class PrescriptionMasterDataController : Controller
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        public PrescriptionMasterDataController()
        {
            _vmMsg = new ValidationMsg();
        }
        public ActionResult frmPrescriptionMasterData()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Prescription Master Data";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PrescriptionMasterData()
        {
            try
            {
                System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_PRESCRIPTION_MASTER");
                foreach (DataRow dr in dt.Rows)
                {
                    System.Data.DataTable dtunitPrice = dbHelper.GetDataTable("select ZONE_CODE,DIVISION_CODE,REGION_CODE,TERRITORY_CODE from LOCATION_VUE where MARKET_CODE ='" + dr["MARKET_CODE"].ToString() + "'");
                    var zoneCode = dtunitPrice.Rows[0]["ZONE_CODE"].ToString();
                    var divisionCode = dtunitPrice.Rows[0]["DIVISION_CODE"].ToString();
                    var regionCode = dtunitPrice.Rows[0]["REGION_CODE"].ToString();
                    var territoryCode = dtunitPrice.Rows[0]["TERRITORY_CODE"].ToString();

                    string qry = "INSERT INTO PRESCRIPTION_MASTER(PRESC_MAS_SLNO,SESSION_SLNO,PRESC_CATE_CODE,ZONE_CODE,DIVISION_CODE,REGION_CODE,TERRITORY_CODE,MARKET_CODE,DOCTOR_ID,INSTI_CODE)" +
                    "VALUES('" + dr["PRESC_MAS_SLNO"].ToString() + "', '" + dr["SESSION_SLNO"].ToString() + "', '" + dr["PRESC_CATE_CODE"].ToString() + "','" + zoneCode + "','" + divisionCode + "','" + regionCode + "','" + territoryCode + "','" + dr["MARKET_CODE"].ToString() + "','" + dr["DOCTOR_ID"].ToString() + "','" + dr["INSTI_CODE"].ToString() + "')";
                    dbHelper.CmdExecute(qry);
                }
                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Data Successfully Loaded to Prescription Master.";
            }
            catch
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Fail to Save Prescription Master Data.";
            }
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult frmPrescriptionMasterData(PrescriptionMasterDataModel model)
        {
            // Get Temp Prescription Master Data Table Data
            System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_PRESCRIPTION_MASTER");
            List<PrescriptionMasterDataModel> PrescriptionMasterDataList;
            PrescriptionMasterDataList = (from DataRow row in dt.Rows
                                          select new PrescriptionMasterDataModel
                                          {
                                              PrescriptionSlno = row["PRESC_MAS_SLNO"].ToString(),
                                              SessionSlno = row["SESSION_SLNO"].ToString(),
                                              PrescriptionType = row["PRESC_CATE_CODE"].ToString(),
                                              Market = row["MARKET_CODE"].ToString(),
                                              DoctorId = row["DOCTOR_ID"].ToString(),
                                              InstitutionCode = row["INSTI_CODE"].ToString()
                                          }).ToList();

            // Get Prescription Master Data  Table Data
            System.Data.DataTable dtExit = dbHelper.GetDataTable("SELECT * FROM PRESCRIPTION_MASTER");
            List<PrescriptionMasterDataModel> ExitPrescriptionMasterDataList;
            ExitPrescriptionMasterDataList = (from DataRow row in dtExit.Rows
                                              select new PrescriptionMasterDataModel
                                              {
                                                  PrescriptionSlno = row["PRESC_MAS_SLNO"].ToString()
                                              }).ToList();

            // Get User Session Data
            System.Data.DataTable UserWorkinngSessionDt = dbHelper.GetDataTable("SELECT * from USER_WORKING_SESSION");
            List<PrescriptionMasterDataModel> UserWorkinngSessionList = new List<PrescriptionMasterDataModel>();
            UserWorkinngSessionList = (from DataRow row in UserWorkinngSessionDt.Rows
                                       select new PrescriptionMasterDataModel
                                       {
                                           SessionSlno = row["SESSION_SLNO"].ToString()
                                       }).ToList();

            // Get INSTITUTION Data
            System.Data.DataTable dtInstCode = dbHelper.GetDataTable("SELECT * FROM INSTITUTION");
            List<PrescriptionMasterDataModel> InstCodeList;
            InstCodeList = (from DataRow row in dtInstCode.Rows
                            select new PrescriptionMasterDataModel
                            {
                                InstitutionCode = row["INSTI_CODE"].ToString()
                            }).ToList();

            // Get Market Data
            System.Data.DataTable dtMarket = dbHelper.GetDataTable("SELECT * FROM MARKET");
            List<PrescriptionMasterDataModel> MarketList;
            MarketList = (from DataRow row in dtMarket.Rows
                          select new PrescriptionMasterDataModel
                          {
                              Market = row["MARKET_CODE"].ToString()
                          }).ToList();

            // Get DOCTOR Data
            System.Data.DataTable dtDoc = dbHelper.GetDataTable("SELECT * FROM DOCTOR");
            List<PrescriptionMasterDataModel> DocList;
            DocList = (from DataRow row in dtDoc.Rows
                       select new PrescriptionMasterDataModel
                       {
                           DoctorId = row["DOCTOR_ID"].ToString()
                       }).ToList();

            List<PrescriptionMasterDataModel> ExcelPrescriptionMasterDataList = new List<PrescriptionMasterDataModel>();

            // Check NULL/EMPTY 
            var PrescriptionMasterDataListNuLL = PrescriptionMasterDataList.Where(m => string.IsNullOrEmpty(m.PrescriptionSlno));
            foreach (var objNuLL in PrescriptionMasterDataListNuLL)
            {
                if (string.IsNullOrEmpty(objNuLL.Remarks))
                {
                    objNuLL.Remarks = "PSE";//Prescription Serial Number Empty
                    ExcelPrescriptionMasterDataList.Add(objNuLL);
                }
                else
                    objNuLL.Remarks = objNuLL.Remarks + "," + "PSE";//Prescription Serial Number Empty
            }

            // Check No 3 Existing in PrescMasSlno
            var notInSessionSlnoList = PrescriptionMasterDataList.Where(p => !UserWorkinngSessionList.Any(p2 => p2.SessionSlno == p.SessionSlno));
            foreach (var objExit in notInSessionSlnoList)
            {
                if (string.IsNullOrEmpty(objExit.Remarks))
                {
                    objExit.Remarks = "SSNE";// Session Serial Not Exist
                    ExcelPrescriptionMasterDataList.Add(objExit);
                }
                else
                    objExit.Remarks = objExit.Remarks + "," + "SSNE";// Session Serial Not Exist
            }

            //Check No 4 Duplicate PrescriptionSlno
            if (PrescriptionMasterDataList.GroupBy(n => n.PrescriptionSlno).Any(c => c.Count() > 1))
            {
                var list = PrescriptionMasterDataList.GroupBy(m => m.PrescriptionSlno).ToList();
                foreach (var obj in list)
                {
                    if (obj.Count() > 1)
                    {
                        foreach (var objDuplicate in obj)
                        {
                            if (string.IsNullOrEmpty(objDuplicate.Remarks))
                            {
                                objDuplicate.Remarks = "DPS";//Duplicate Prescription SL. No. 
                                ExcelPrescriptionMasterDataList.Add(objDuplicate);
                            }
                            else
                                objDuplicate.Remarks = objDuplicate.Remarks + "," + "DPS";//Duplicate Prescription SL. No. 
                        }
                    }
                }
            }

            // Check No 5 Existing in SessionSlno
            var notInPrescriptionSlnoList = ExitPrescriptionMasterDataList.Select(a => a.PrescriptionSlno).Intersect(PrescriptionMasterDataList.Select(b => b.PrescriptionSlno));
            foreach (var objExit in notInPrescriptionSlnoList)
            {
                var avd = PrescriptionMasterDataList.Where(m => m.PrescriptionSlno == objExit).FirstOrDefault();
                if (string.IsNullOrEmpty(avd.Remarks))
                {
                    avd.Remarks = "EPSN-PM";// Exist Prescription SL. No. in Prescription Master
                    ExcelPrescriptionMasterDataList.Add(avd);
                }
                else
                    avd.Remarks = avd.Remarks + "," + "EPSN-PM";// Exist Prescription SL. No. in Prescription Master
            }

            // Check 6 NULL/EMPTY 
            var PrescriptionType = PrescriptionMasterDataList.Where(m => (m.PrescriptionType != "P") && (m.PrescriptionType != "O") && (m.PrescriptionType != "S") && (m.PrescriptionType != "T"));
            foreach (var objNuLL in PrescriptionType)
            {
                if (string.IsNullOrEmpty(objNuLL.Remarks))
                {
                    objNuLL.Remarks = "IPT";//Invalid Prescription Type
                    ExcelPrescriptionMasterDataList.Add(objNuLL);
                }
                else
                    objNuLL.Remarks = objNuLL.Remarks + "," + "IPT";//Invalid Prescription Type
            }

            // Check 7 PrescriptionType 
            var PrescriptionTypeList = PrescriptionMasterDataList.Where(m => m.PrescriptionType == "P" && string.IsNullOrEmpty(m.DoctorId));
            foreach (var objPrescriptionType in PrescriptionTypeList)
            {
                if (string.IsNullOrEmpty(objPrescriptionType.Remarks))
                {
                    objPrescriptionType.Remarks = "IPT-EDI";//Invalid Prescription Type-Empty Doctor ID
                    ExcelPrescriptionMasterDataList.Add(objPrescriptionType);
                }
                else
                    objPrescriptionType.Remarks = objPrescriptionType.Remarks + "," + "IPT-EDI";//Invalid Prescription Type-Empty Doctor ID
            }

            // Check 7 PrescriptionType 
            var PrescriptionTypeInstitutionCodeList = PrescriptionMasterDataList.Where(m => m.PrescriptionType == "P" && (!string.IsNullOrEmpty(m.InstitutionCode)));
            foreach (var objPrescriptionType in PrescriptionTypeInstitutionCodeList)
            {
                if (string.IsNullOrEmpty(objPrescriptionType.Remarks))
                {
                    objPrescriptionType.Remarks = "IPT-FIC";//Invalid Prescription Type-Found Institution Code
                    ExcelPrescriptionMasterDataList.Add(objPrescriptionType);
                }
                else
                    objPrescriptionType.Remarks = objPrescriptionType.Remarks + "," + "IPT-FIC";//Invalid Prescription Type-Found Institution Code
            }

            // Check 8 PrescriptionType 

            var DocIDList = PrescriptionMasterDataList.Where(m => m.PrescriptionType == "P" && m.DoctorId != "900000");
            var notInDocIDList = DocIDList.Where(p => !DocList.Any(p2 => p2.DoctorId == p.DoctorId));
            foreach (var objDocID in notInDocIDList)
            {
                if (string.IsNullOrEmpty(objDocID.Remarks))
                {
                    objDocID.Remarks = "DII";// Doctor ID Invalid
                    ExcelPrescriptionMasterDataList.Add(objDocID);
                }
                else
                    objDocID.Remarks = objDocID.Remarks + "," + "DII";// Doctor ID Invalid
            }

            // Check No 9,14,18 Existing in Market
            var notInMarketList = PrescriptionMasterDataList.Where(p => !MarketList.Any(p2 => p2.Market == p.Market));
            //var notInMarketList = PrescriptionMasterDataList.Where(p => !MarketList.Any(p2 => p2.Market == p.Market));
            foreach (var objMarket in notInMarketList)
            {
                if (string.IsNullOrEmpty(objMarket.Remarks))
                {
                    objMarket.Remarks = "IMC";// Invalid Market Code
                    ExcelPrescriptionMasterDataList.Add(objMarket);
                }
                else
                    objMarket.Remarks = objMarket.Remarks + "," + "IMC";// Invalid Market Code
            }

            //// Check 10 Location View 
            //if (notInMarketList.Count() == 0)
            //{
            //    // Get DocCoverMKT View Data
            //    System.Data.DataTable dtDocCoverMKT = dbHelper.GetDataTable("select DMS.DOCTOR_ID,DTL.PRAC_MKT_CODE from DOC_MKT_MAS DMS,DOC_MKT_DTL DTL where DMS.DOC_MKT_MAS_SLNO = DTL.DOC_MKT_MAS_SLNO");
            //    List<PrescriptionMasterDataModel> DocCoverMKTList;
            //    DocCoverMKTList = (from DataRow row in dtDocCoverMKT.Rows
            //                       select new PrescriptionMasterDataModel
            //                       {
            //                           DoctorId = row["DOCTOR_ID"].ToString(),
            //                           Market = row["PRAC_MKT_CODE"].ToString()
            //                       }).ToList();

            //    //var notInDocCoverMKTList = PrescriptionMasterDataList.Where(p => !DocCoverMKTList.Any(p2 => p2.Market == p.Market));
            //    var notInDocCoverMKTList = DocIDList.Where(p => !DocCoverMKTList.Any(p2 => p2.Market == p.Market));
            //    foreach (var objLocVW in notInDocCoverMKTList)
            //    {
            //        if (string.IsNullOrEmpty(objLocVW.Remarks))
            //        {
            //            objLocVW.Remarks = "NMAAD";// No Market is Assigned against Doctor
            //            ExcelPrescriptionMasterDataList.Add(objLocVW);
            //        }
            //        else
            //            objLocVW.Remarks = objLocVW.Remarks + "," + "NMAAD";// No Market is Assigned against Doctor
            //    }
            //}

            // Check 11,16 Location View 
            if (notInMarketList.Count() == 0)
            {
                // Get LOCATION View Data
                System.Data.DataTable dtLocationView = dbHelper.GetDataTable("SELECT * FROM LOCATION_VUE");
                List<PrescriptionMasterDataModel> LocationViewList;
                LocationViewList = (from DataRow row in dtLocationView.Rows
                                    select new PrescriptionMasterDataModel
                                    {
                                        Market = row["MARKET_CODE"].ToString()
                                    }).ToList();

                var notInLocVWList = PrescriptionMasterDataList.Where(p => !LocationViewList.Any(p2 => p2.Market == p.Market));
                foreach (var objLocVW in notInLocVWList)
                {
                    if (string.IsNullOrEmpty(objLocVW.Remarks))
                    {
                        objLocVW.Remarks = "AMNE-MLS";// Assigned Market Have no Existance in Market Locaton Structure
                        ExcelPrescriptionMasterDataList.Add(objLocVW);
                    }
                    else
                        objLocVW.Remarks = objLocVW.Remarks + "," + "AMNE-MLS";// Assigned Market Have no Existance in Market Locaton Structure
                }
            }

            // Check 12 PrescriptionType 
            var InstitutionList = PrescriptionMasterDataList.Where(m => (m.PrescriptionType) == "O" && ((string.IsNullOrEmpty(m.InstitutionCode)) || (!string.IsNullOrEmpty(m.DoctorId))));
            foreach (var objInstitution in InstitutionList)
            {
                if (string.IsNullOrEmpty(objInstitution.Remarks))
                {
                    objInstitution.Remarks = "IDN";// Institution ID Null
                    ExcelPrescriptionMasterDataList.Add(objInstitution);
                }
                else
                    objInstitution.Remarks = objInstitution.Remarks + "," + "IDN";// Institution ID Null
            }

            // New Check 22 Doctor and Market Relation 
            var DocMktRelList = PrescriptionMasterDataList.Where(m => m.PrescriptionType == "P" && m.DoctorId != "900000");
            if (DocMktRelList.Count() > 0)
            {
                foreach (var objDocMktRel in DocMktRelList)
                {
                    System.Data.DataTable dtFounDocMktRel = dbHelper.GetDataTable("select DMS.DOCTOR_ID,DTL.PRAC_MKT_CODE from DOC_MKT_MAS DMS,DOC_MKT_DTL DTL where DMS.DOC_MKT_MAS_SLNO = DTL.DOC_MKT_MAS_SLNO and DMS.DOCTOR_ID ='" + objDocMktRel.DoctorId + "' and DTL.PRAC_MKT_CODE ='" + objDocMktRel.Market + "'");
                    if (dtFounDocMktRel.Rows.Count == 0)
                    {
                        if (string.IsNullOrEmpty(objDocMktRel.Remarks))
                        {
                            objDocMktRel.Remarks = "IDMR";// Invalid Doctor Market Relation
                            ExcelPrescriptionMasterDataList.Add(objDocMktRel);
                        }
                        else
                            objDocMktRel.Remarks = objDocMktRel.Remarks + "," + "IDMR";// Invalid Doctor Market Relation                  
                    }
                }
            }

            // Check 13 PrescriptionType 
            //var InstitutionCodeList = PrescriptionMasterDataList.Where(m => m.PrescriptionType == "O" && m.InstitutionCode != "99999");
            var InstitutionCodeList = PrescriptionMasterDataList.Where(m => m.PrescriptionType == "O");// && m.InstitutionCode != "99999");
            if (InstitutionList.Count() == 0)
            {
                //var InstitutionCodeList = PrescriptionMasterDataList.Where(m => m.PrescriptionType == "O" && m.InstitutionCode != "99999");
                var notInInstitutionCodeList = InstitutionCodeList.Where(p => !InstCodeList.Any(p2 => p2.InstitutionCode == p.InstitutionCode));
                foreach (var objInstitutionCode in notInInstitutionCodeList)
                {
                    if (string.IsNullOrEmpty(objInstitutionCode.Remarks))
                    {
                        objInstitutionCode.Remarks = "IIC";// Invalid Institution Code
                        ExcelPrescriptionMasterDataList.Add(objInstitutionCode);
                    }
                    else
                        objInstitutionCode.Remarks = objInstitutionCode.Remarks + "," + "IIC";// Invalid Institution Code
                }
            }

            //// Check 15 Location View 
            //if (notInMarketList.Count() == 0)
            //{
            //    // Get Inst Mkt Data
            //    System.Data.DataTable dtInstMkt = dbHelper.GetDataTable("select DMS.INSTI_CODE,DTL.MARKET_CODE from INSTI_MKT_MAS DMS,INSTI_MKT_DTL DTL where DMS.INSTI_MKT_MAS_SLNO = DTL.INSTI_MKT_MAS_SLNO");
            //    List<PrescriptionMasterDataModel> InstMktList;
            //    InstMktList = (from DataRow row in dtInstMkt.Rows
            //                   select new PrescriptionMasterDataModel
            //                   {
            //                       InstitutionCode = row["INSTI_CODE"].ToString(),
            //                       Market = row["MARKET_CODE"].ToString()
            //                   }).ToList();

            //    //var notInInstMktList = PrescriptionMasterDataList.Where(p => !InstMktList.Any(p2 => p2.Market == p.Market));
            //    var notInInstMktList = InstitutionCodeList.Where(p => !InstMktList.Any(p2 => p2.Market == p.Market));
            //    foreach (var objLocVW in notInInstMktList)
            //    {
            //        if (string.IsNullOrEmpty(objLocVW.Remarks))
            //        {
            //            objLocVW.Remarks = "NMAAI";// No Market is Assigned against Institusion Code
            //            ExcelPrescriptionMasterDataList.Add(objLocVW);
            //        }
            //        else
            //            objLocVW.Remarks = objLocVW.Remarks + "," + "NMAAI";// No Market is Assigned against Institusion Code
            //    }
            //}

            // New Check 23 Doctor and Market Relation 
            var InsMktRelList = PrescriptionMasterDataList.Where(m => m.PrescriptionType == "O" && m.InstitutionCode != "99999");
            if (InsMktRelList.Count() > 0)
            {
                foreach (var objInsMktRel in InsMktRelList)
                {
                    System.Data.DataTable dtFounInsMktRel = dbHelper.GetDataTable("select DMS.INSTI_CODE,DTL.MARKET_CODE from INSTI_MKT_MAS DMS,INSTI_MKT_DTL DTL where DMS.INSTI_MKT_MAS_SLNO = DTL.INSTI_MKT_MAS_SLNO and DMS.INSTI_CODE ='" + objInsMktRel.InstitutionCode + "' and DTL.MARKET_CODE ='" + objInsMktRel.Market + "'");
                    if (dtFounInsMktRel.Rows.Count == 0)
                    {
                        if (string.IsNullOrEmpty(objInsMktRel.Remarks))
                        {
                            objInsMktRel.Remarks = "IIMR";// Invalid Institution Market Relation
                            ExcelPrescriptionMasterDataList.Add(objInsMktRel);
                        }
                        else
                            objInsMktRel.Remarks = objInsMktRel.Remarks + "," + "IIMR";// Invalid Institution Market Relation                  
                    }
                }
            }


            // Check 17 PrescriptionType 
            var PrescriptionTypeSliporTickList = PrescriptionMasterDataList.Where(m => ((m.PrescriptionType == "S") || (m.PrescriptionType == "T")) && ((!string.IsNullOrEmpty(m.DoctorId)) || (!string.IsNullOrEmpty(m.InstitutionCode))));
            foreach (var objPrescriptionType in PrescriptionTypeSliporTickList)
            {
                if (string.IsNullOrEmpty(objPrescriptionType.Remarks))
                {
                    objPrescriptionType.Remarks = "IPT-ST";//Invalid Prescription Type-Slip OR OTC
                    ExcelPrescriptionMasterDataList.Add(objPrescriptionType);
                }
                else
                    objPrescriptionType.Remarks = objPrescriptionType.Remarks + "," + "IPT-ST";//Invalid Prescription Type-Slip OR OTC
            }

            // Create Excel File
            //if ((PrescriptionMasterDataList.GroupBy(n => n.SessionSlno).Any(c => c.Count() > 0)) || (exitDuplicateUserWorkinngSessionList.Count() > 0) || (UserWorkinngSessionListNuLL.Count() > 0))
            if (ExcelPrescriptionMasterDataList.Count > 0)
            {
                System.Data.DataTable dtExcel = ConvertToDataTable(ExcelPrescriptionMasterDataList);

                Response.Clear();

                Response.Buffer = true;

                Response.AddHeader("content-disposition", "attachment;filename=PrescriptionMasterData.xls");

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

                //string fileName = "D:\\PrescriptionMasterData-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls";

                //Excel.ExcelUtlity objExcel = new Excel.ExcelUtlity();
                //System.Data.DataTable dtExcel = ConvertToDataTable(ExcelPrescriptionMasterDataList);
                //objExcel.WriteDataTableToExcel(dtExcel, "Pres_mast", fileName, "UserWorkinngSession");

                //_vmMsg.Type = Enums.MessageType.Error;
                //_vmMsg.Msg = "Sorry,Some Error Found, Please See the Excel File " + fileName;
            }
            else
            {
                //System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_PRESCRIPTION_MASTER");
                foreach (DataRow dr in dt.Rows)
                {
                    System.Data.DataTable dtunitPrice = dbHelper.GetDataTable("select ZONE_CODE,DIVISION_CODE,REGION_CODE,TERRITORY_CODE from LOCATION_VUE where MARKET_CODE ='" + dr["MARKET_CODE"].ToString() + "'");
                    var zoneCode = dtunitPrice.Rows[0]["ZONE_CODE"].ToString();
                    var divisionCode = dtunitPrice.Rows[0]["DIVISION_CODE"].ToString();
                    var regionCode = dtunitPrice.Rows[0]["REGION_CODE"].ToString();
                    var territoryCode = dtunitPrice.Rows[0]["TERRITORY_CODE"].ToString();

                    string qry = "INSERT INTO PRESCRIPTION_MASTER(PRESC_MAS_SLNO,SESSION_SLNO,PRESC_CATE_CODE,ZONE_CODE,DIVISION_CODE,REGION_CODE,TERRITORY_CODE,MARKET_CODE,DOCTOR_ID,INSTI_CODE)" +
                    "VALUES('" + dr["PRESC_MAS_SLNO"].ToString() + "', '" + dr["SESSION_SLNO"].ToString() + "', '" + dr["PRESC_CATE_CODE"].ToString() + "','" + zoneCode + "','" + divisionCode + "','" + regionCode + "','" + territoryCode + "','" + dr["MARKET_CODE"].ToString() + "','" + dr["DOCTOR_ID"].ToString() + "','" + dr["INSTI_CODE"].ToString() + "')";
                    dbHelper.CmdExecute(qry);
                }
                ViewBag.Msg = "Data Successfully Loaded to Prescription Master.";
                //_vmMsg.Type = Enums.MessageType.Success;
                //_vmMsg.Msg = "Data Successfully Loaded to Prescription Master.";
            }
            return View();
            //return Json(new { msg = _vmMsg });
        }

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

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string PrescriptionMasterDataCode)
        {
            //_vmMsg = Dalobject.Delete(PrescriptionMasterDataCode);
            return Json(new { msg = _vmMsg });
        }

        public ActionResult LoadExcelFile(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(TempData["file"].ToString()))
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

                var results = new List<PrescriptionMasterDataModel>();

                string connectionString = "";
                string filename = Path.Combine(Server.MapPath("~/Content"), TempData["file"].ToString());
                //string filename = @"C:\Test\" + TempData["file"].ToString();
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

                string query = String.Format("SELECT * from [{0}$]", "Pres_mast");
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connectionString);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                System.Data.DataTable myDataTable = dataSet.Tables[0];
                //Check No 1
                if (dataSet.Tables[0].Columns.Count == 6)
                {
                    Int64 i = 0;
                    dbHelper.CmdExecute("DELETE FROM TEMP_PRESCRIPTION_MASTER");
                    //Check No 2
                    foreach (DataRow dr in myDataTable.Rows)
                    {
                        string qry = "INSERT INTO TEMP_PRESCRIPTION_MASTER(PRESC_MAS_SLNO,SESSION_SLNO,PRESC_CATE_CODE,MARKET_CODE,DOCTOR_ID,INSTI_CODE)" +
                            //"VALUES('" + dr["Prescription Slno"].ToString() + "', '" + dr["Session Slno"].ToString() + "', '" + dr["Prescription Type"].ToString() + "',lpad('" + dr["Market"].ToString() + "', 4, 0),'" + dr["Doctor Id"].ToString() + "','" + dr["Institution Code"].ToString() + "')";
                            "VALUES('" + dr["Prescription Slno"].ToString() + "', '" + dr["Session Slno"].ToString() + "', '" + dr["Prescription Type"].ToString() + "','" + dr["Market"].ToString() + "','" + dr["Doctor Id"].ToString() + "','" + dr["Institution Code"].ToString() + "')";
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
                Remove(TempData["file"].ToString());
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Fail to Save Excel File Data";
            }
            return Json(new { msg = _vmMsg }, JsonRequestBehavior.AllowGet);
        }

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



