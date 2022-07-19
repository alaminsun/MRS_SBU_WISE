using MRS.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OracleClient;
using MRS.Models;
using MRS.DAL.Common;

namespace MRS.DAL.DAO
{
    public class OrganizationDAO
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbCon = new DBConnection();
        IDGenerated idGenerated = new IDGenerated();
        ValidationMsg _vmMsg;
        public List<Institute> GetInstitutes()
        {
            using(OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "SELECT INSTI_CODE,INSTI_NAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4 FROM INSTITUTION ORDER BY INSTI_CODE";
                OracleCommand cmd = new OracleCommand(query,con);

                con.Open();
                List<Institute> institutes = new List<Institute>();
                using(OracleDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Institute model = new Institute();
                        model.InstituteCode = Convert.ToInt32(reader["INSTI_CODE"]);
                        model.InstituteName = reader["INSTI_NAME"].ToString();
                        model.Address1 = reader["ADDRESS1"].ToString();
                        model.Address2 = reader["ADDRESS2"].ToString();
                        model.Address3 = reader["ADDRESS3"].ToString();
                        model.Address4 = reader["ADDRESS4"].ToString();

                        institutes.Add(model);
                    }
                    return institutes;
                }
            }
        }

        public List<Institute> GetSessionInstitutes(int sessionSl)
        {
            using(OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select P.INSTI_CODE,INSTI_NAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,P.MARKET_CODE,MARKET_NAME " +
                                "from   PRESCRIPTION_MASTER P Inner Join INSTITUTION I  " +
                                "ON     P.INSTI_CODE = I.INSTI_CODE " +
                                "INNER JOIN     MARKET M ON P.MARKET_CODE = M.MARKET_CODE " +
                                "WHERE SESSION_SLNO = " + sessionSl + "";
                OracleCommand cmd = new OracleCommand(query, con);

                List<Institute> institutes = new List<Institute>();
                con.Open();
                using(OracleDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Institute model = new Institute();
                        model.InstituteCode = Convert.ToInt32(reader["INSTI_CODE"]);
                        model.InstituteName = reader["INSTI_NAME"].ToString();
                        model.Address1 = reader["ADDRESS1"].ToString();
                        model.Address2 = reader["ADDRESS2"].ToString();
                        model.Address3 = reader["ADDRESS3"].ToString();
                        model.Address4 = reader["ADDRESS4"].ToString();
                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        model.MarketName = reader["MARKET_NAME"].ToString();

                        institutes.Add(model);
                    }
                }
                return institutes;
            }
        }

        public List<Institute> GetMarkets(string prescNo)
        {
            using(OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                //string query = "Select DISTINCT M.MARKET_CODE,MARKET_NAME  " +
                //                "FROM   MARKET M INNER JOIN INSTI_MKT_DTL IMD ON M.MARKET_CODE = IMD.MARKET_CODE  " +
                //                "INNER JOIN INSTI_MKT_MAS IM ON IMD.INSTI_MKT_MAS_SLNO = IM.INSTI_MKT_MAS_SLNO  " +
                //                "INNER JOIN  INSTITUTION I ON IM.INSTI_CODE = I.INSTI_CODE " +
                //                "WHERE I.INSTI_CODE = " + prescNo + "";
                string query = "Select a.Market_Code, a.Market_Name,A.TERRITORY_CODE,A.TERRITORY_NAME,A.REGION_CODE,A.REGION_NAME,A.DIVISION_CODE,A.DIVISION_NAME From LOCATION_VUE a, Prescription_Master b Where A.MARKET_CODE = B.MARKET_CODE and B.PRESC_MAS_SLNO ='" + prescNo + "'";

                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                List<Institute> markets = new List<Institute>();
                using(OracleDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Institute model = new Institute();
                        model.MarketCodeO = reader["MARKET_CODE"].ToString();
                        model.MarketNameO = reader["MARKET_NAME"].ToString();
                        markets.Add(model);
                    }
                }
                return markets;
            }
        }

        public List<ReportModel> GetOrgMarketList()
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select * From LOCATION_VUE";
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                List<ReportModel> markets = new List<ReportModel>();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ReportModel model = new ReportModel();
                        //model.ProductId = reader["PROD_ID"].ToString();
                        model.MARKET_CODE = reader["MARKET_CODE"].ToString();
                        model.MARKET_NAME = reader["MARKET_NAME"].ToString();
                        model.TERRITORY_CODE = reader["TERRITORY_CODE"].ToString();
                        model.TERRITORY_NAME = reader["TERRITORY_NAME"].ToString();
                        model.REGION_CODE = reader["REGION_CODE"].ToString();
                        model.REGION_NAME = reader["REGION_NAME"].ToString();
                        model.DIVISION_CODE = reader["DIVISION_CODE"].ToString();
                        model.DIVISION_NAME = reader["DIVISION_NAME"].ToString();
                        model.ZONE_CODE = reader["ZONE_CODE"].ToString();
                        model.ZONE_NAME = reader["ZONE_NAME"].ToString();
                        markets.Add(model);
                    }
                    return markets;
                }
            }
        }
        public ValidationMsg Save(PrescriptionModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                model.PrescriptionMstSl = idGenerated.getMAXSL("PRESC_MAS_SLNO", "PRESCRIPTION_MASTER");
                if (model.Details != null)
                {
                    string query = "Insert into PRESCRIPTION_MASTER(PRESC_MAS_SLNO,SESSION_SLNO,PRESC_CATE_CODE,MARKET_CODE,INSTI_CODE)values(" + model.PrescriptionMstSl + "," + HttpContext.Current.Session["SessionSl"] + ", " +
                                   "'" + model.CategoryCode + "','" + model.MARKET_CODE_O + "'," + model.InstituteCode + ")";
                    dbHelper.CmdExecute(query);
                    foreach (var detailModel in model.Details)
                    {
                        detailModel.PrescriptionDtlSl = idGenerated.getMAXSL("PRESC_DET_SLNO", "PRESCRIPTION_DETAIL");
                        string query1 = "Insert Into PRESCRIPTION_DETAIL(PRESC_DET_SLNO,PRESC_MAS_SLNO,PROD_ID,PURCHASE_QTY,UNIT_PRICE " +
                                        ") Values(" + detailModel.PrescriptionDtlSl + "," + model.PrescriptionMstSl + ",'" + detailModel.ProductId + "', " +
                                        "'" + detailModel.Quantity + "','" + detailModel.UnitPrice + "')";
                        dbHelper.CmdExecute(query1);
                    }
                    _vmMsg.ReturnId = model.PrescriptionMstSl;
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Save Successful.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "This Data already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
                if (ex.Message.Contains("ORA-01400"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Fill The Required Field.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Update(PrescriptionModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string masterUpdate = "";
                if (model.MARKET_CODE_O == null)
                {
                    masterUpdate = "Update Prescription_Master SET INSTI_CODE='" + model.InstituteCode + "' Where PRESC_MAS_SLNO = '" + model.PrescriptionMstSl + "'";
                }
                else
                {

                    //masterUpdate = "Update Prescription_Master SET INSTI_CODE='" + model.InstituteCode + "', MARKET_CODE='" + model.MarketCode + "' Where PRESC_MAS_SLNO = '" + model.PrescriptionMstSl + "'";
                    masterUpdate = "Update Prescription_Master SET INSTI_CODE='" + model.InstituteCode + "', ZONE_CODE= '" + model.ZONE_CODE_O + "',  DIVISION_CODE='" + model.DIVISION_CODE_O + "',REGION_CODE='" + model.REGION_CODE_O + "', TERRITORY_CODE= '" + model.TERRITORY_CODE_O + "',  MARKET_CODE='" + model.MARKET_CODE_O + "' Where PRESC_MAS_SLNO = '" + model.PrescriptionMstSl + "'";
                }
                dbHelper.CmdExecute(masterUpdate);

                long prescriptionMstSl = 0;
                foreach (var detailModel in model.Details)
                {
                    if (detailModel.PrescriptionDtlSl == 0)
                    {
                        prescriptionMstSl = GetMstSl(model.InstituteCode , model.SessionSl);

                        if (prescriptionMstSl == 0)
                        {
                            detailModel.PrescriptionDtlSl = idGenerated.getMAXSL("PRESC_DET_SLNO", "PRESCRIPTION_DETAIL");
                            string query1 = "Insert Into PRESCRIPTION_DETAIL(PRESC_DET_SLNO,PRESC_MAS_SLNO,PROD_ID,PURCHASE_QTY,UNIT_PRICE " +
                                            ") Values(" + detailModel.PrescriptionDtlSl + "," + model.PrescriptionMstSl + ",'" + detailModel.ProductId + "', " +
                                            "'" + detailModel.Quantity + "','" + detailModel.UnitPrice + "')";
                            dbHelper.CmdExecute(query1);
                        }
                        else
                        {
                            detailModel.PrescriptionDtlSl = idGenerated.getMAXSL("PRESC_DET_SLNO", "PRESCRIPTION_DETAIL");
                            string query1 = "Insert Into PRESCRIPTION_DETAIL(PRESC_DET_SLNO,PRESC_MAS_SLNO,PROD_ID,PURCHASE_QTY,UNIT_PRICE " +
                                            ") Values(" + detailModel.PrescriptionDtlSl + "," + prescriptionMstSl + ",'" + detailModel.ProductId + "', " +
                                            "'" + detailModel.Quantity + "','" + detailModel.UnitPrice + "')";
                            dbHelper.CmdExecute(query1);
                        }
                        
                    }
                    else
                    {
                        string query = "Update PRESCRIPTION_DETAIL SET PURCHASE_QTY='" + detailModel.Quantity + "'  Where PRESC_DET_SLNO=" + detailModel.PrescriptionDtlSl + "";
                        dbHelper.CmdExecute(query);
                    }
                }
                _vmMsg.ReturnId = (long)prescriptionMstSl;
                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Update Successful.";
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "This Data already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
                if (ex.Message.Contains("ORA-01400"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Fill The Required Field.";
                }
            }
            return _vmMsg;
        }

        private long GetMstSl(int instituteCode, long sessionSl)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                //string query = "Select PRESC_MAS_SLNO from PRESCRIPTION_MASTER Where INSTI_CODE=" + instituteCode + "";
                string query = "Select PRESC_MAS_SLNO from PRESCRIPTION_MASTER Where INSTI_CODE =" + instituteCode + " AND SESSION_SLNO = " + sessionSl + "";
                long slNo = 0;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        slNo = Convert.ToInt64(reader["PRESC_MAS_SLNO"]);
                    }
                    return slNo;
                }
            }
        }
        public ValidationMsg DeleteData(long sl)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string query = "Delete from PRESCRIPTION_DETAIL Where PRESC_DET_SLNO=" + sl + "";
                if (dbHelper.CmdExecute(query) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Delete Data.";
            }
            return _vmMsg;
        }

        public List<Product> GetSearchProduct(int instituteId)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select PD.PRESC_DET_SLNO,PD.PRESC_MAS_SLNO,PD.PROD_ID,P.PRODUCT_NAME,DSG_STRENGTH_NAME,DOSAGE_FORM_NAME,NVL(PD.PURCHASE_QTY,0)PURCHASE_QTY,NVL(PD.UNIT_PRICE,0)UNIT_PRICE " +
                              "from   PRESCRIPTION_DETAIL PD Inner Join PRESCRIPTION_MASTER PM ON PD.PRESC_MAS_SLNO = PM.PRESC_MAS_SLNO " +
                              "INNER JOIN PRODUCT P ON PD.PROD_ID = P.PROD_ID " +
                              "INNER JOIN DOSAGE_FORM D ON P.DOSAGE_FORM_CODE =  D.DOSAGE_FORM_CODE " +
                              "INNER JOIN DOSAGE_STRENGTH DS ON P.DSG_STRENGTH_CODE = DS.DSG_STRENGTH_CODE  " +
                              "WHERE PM.INSTI_CODE = " + instituteId + "";

                OracleCommand cmd = new OracleCommand(query, con);
                List<Product> products = new List<Product>();
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product model = new Product();
                        model.PrescriptionDtlSl = Convert.ToInt32(reader["PRESC_DET_SLNO"]);
                        model.PrescriptionMstSl = Convert.ToInt32(reader["PRESC_MAS_SLNO"]);
                        model.ProductId = reader["PROD_ID"].ToString();
                        model.ProductName = reader["PRODUCT_NAME"].ToString();
                        model.DosageStrength = reader["DSG_STRENGTH_NAME"].ToString();
                        model.DosageForm = reader["DOSAGE_FORM_NAME"].ToString();
                        model.Quantity = Convert.ToInt32(reader["PURCHASE_QTY"]);
                        model.UnitPrice = Convert.ToDecimal(reader["UNIT_PRICE"]);

                        products.Add(model);
                    }
                    return products;
                }
            }
        }

        public ValidationMsg Delete(long sessionSl, int instituteCode)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string query = "Delete from PRESCRIPTION_MASTER Where PRESC_MAS_SLNO=" + sessionSl + " AND INSTI_CODE=" + instituteCode + "";
                if (dbHelper.CmdExecute(query) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Delete Data.";
            }
            return _vmMsg;
        }
    }

    public class Institute : Market
    {
        public int InstituteCode { get; set; }
        public string InstituteName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string MarketCodeO { get; set; }
        public string MarketNameO { get; set; }
    }
}