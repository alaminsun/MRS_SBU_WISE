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
    public class PrescriptionDAO
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbCon = new DBConnection();
        IDGenerated idGenerated = new IDGenerated();
        ValidationMsg _vmMsg;

        public Market LoadMarket(string prescNo)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                Market markets = new Market();
                //string query = "Select a.Market_Code, a.Market_Name " +
                //                "From LOCATION_VUE a, Prescription_Master b " +
                //                "Where A.MARKET_CODE = B.MARKET_CODE " +
                //                "and B.PRESC_MAS_SLNO ='" + prescNo + "'";

                string query = "Select a.Market_Code, a.Market_Name,A.TERRITORY_CODE,A.TERRITORY_NAME,A.REGION_CODE,A.REGION_NAME,A.DIVISION_CODE,A.DIVISION_NAME From LOCATION_VUE a, Prescription_Master b Where A.MARKET_CODE = B.MARKET_CODE and B.PRESC_MAS_SLNO ='" + prescNo + "'";

                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        markets.MarketCode = reader["MARKET_CODE"].ToString();
                        markets.MarketName = reader["MARKET_NAME"].ToString();
                    }
                }
                return markets;
            }
        }

        public PrescriptionModel LoadMarketO(string prescNo)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                PrescriptionModel markets = new PrescriptionModel();
                //string query = "Select a.Market_Code, a.Market_Name " +
                //                "From LOCATION_VUE a, Prescription_Master b " +
                //                "Where A.MARKET_CODE = B.MARKET_CODE " +
                //                "and B.PRESC_MAS_SLNO ='" + prescNo + "'";

                string query = "Select a.Market_Code, a.Market_Name,A.TERRITORY_CODE,A.TERRITORY_NAME,A.REGION_CODE,A.REGION_NAME,A.DIVISION_CODE,A.DIVISION_NAME From LOCATION_VUE a, Prescription_Master b Where A.MARKET_CODE = B.MARKET_CODE and B.PRESC_MAS_SLNO ='" + prescNo + "'";

                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        markets.MARKET_CODE_O = reader["MARKET_CODE"].ToString();
                        markets.MARKET_NAME_O = reader["MARKET_NAME"].ToString();
                    }
                }
                return markets;
            }
        }

        public List<Product> ProductList()
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select PROD_ID,PRODUCT_NAME,PRODUCT.DOSAGE_FORM_CODE, DOSAGE_FORM_NAME,PRODUCT.DSG_STRENGTH_CODE,DSG_STRENGTH_NAME,UNIT_PRICE " +
                                "from PRODUCT Join DOSAGE_FORM on PRODUCT.DOSAGE_FORM_CODE = DOSAGE_FORM.DOSAGE_FORM_CODE " +
                                "JOIN DOSAGE_STRENGTH ON PRODUCT.DSG_STRENGTH_CODE = DOSAGE_STRENGTH.DSG_STRENGTH_CODE " +
                                "ORDER BY PRODUCT_NAME";
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                List<Product> products = new List<Product>();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product model = new Product();
                        model.ProductId = reader["PROD_ID"].ToString();
                        model.ProductName = reader["PRODUCT_NAME"].ToString();
                        model.DosageFormCode = reader["DOSAGE_FORM_CODE"].ToString();
                        model.DosageForm = reader["DOSAGE_FORM_NAME"].ToString();
                        model.DosageStrengthCode = reader["DSG_STRENGTH_CODE"].ToString();
                        model.DosageStrength = reader["DSG_STRENGTH_NAME"].ToString();
                        if (!reader.IsDBNull(6))
                        {
                            model.UnitPrice = Convert.ToDecimal(reader["UNIT_PRICE"]);
                        }
                        else
                        {
                            model.UnitPrice = 0;
                        }


                        products.Add(model);
                    }
                    return products;
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
                    string query = "Insert into PRESCRIPTION_MASTER(PRESC_MAS_SLNO,SESSION_SLNO,PRESC_CATE_CODE,ZONE_CODE,DIVISION_CODE,REGION_CODE,TERRITORY_CODE,MARKET_CODE, DOCTOR_ID)values(" + model.PrescriptionMstSl + "," + HttpContext.Current.Session["SessionSl"] + ", " +
                                   "'" + model.CategoryCode + "','" + model.ZoneCode + "','" + model.DivisionCode + "','" + model.RegionCode + "','" + model.TerritoryCode + "','" + model.MarketCode + "'," + model.DoctorId + ")";
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
                if (model.MarketCode == null)
                {
                    masterUpdate = "Update Prescription_Master SET DOCTOR_ID='" + model.DoctorId + "' Where PRESC_MAS_SLNO = '" + model.PrescriptionMstSl + "'";
                }
                else
                {

                    masterUpdate = "Update Prescription_Master SET DOCTOR_ID='" + model.DoctorId + "', ZONE_CODE= '"+model.ZoneCode+"',  DIVISION_CODE='" + model.DivisionCode + "',REGION_CODE='" + model.RegionCode + "', TERRITORY_CODE= '" + model.TerritoryCode + "',  MARKET_CODE='" + model.MarketCode + "' Where PRESC_MAS_SLNO = '" + model.PrescriptionMstSl + "'";
                }

                dbHelper.CmdExecute(masterUpdate);

                long prescriptionMstSl = 0;
                foreach (var detailModel in model.Details)
                {
                    if (detailModel.PrescriptionDtlSl == 0)
                    {
                        prescriptionMstSl = GetMstSl(model.DoctorId, model.SessionSl);
                        //if (prescriptionMstSl == 0)
                        //{
                        //    model.PrescriptionMstSl = idGenerated.getMAXSL("PRESC_MAS_SLNO", "PRESCRIPTION_MASTER");
                        //    string query = "Insert into PRESCRIPTION_MASTER(PRESC_MAS_SLNO,SESSION_SLNO,PRESC_CATE_CODE,MARKET_CODE,DOCTOR_ID)values(" + model.PrescriptionMstSl + "," + HttpContext.Current.Session["SessionSl"] + ", " +
                        //            "'" + model.CategoryCode + "','" + model.MarketCode + "'," + model.DoctorId + ")";
                        //    dbHelper.CmdExecute(query);
                        //}
                        //model.PrescriptionMstSl = idGenerated.getMAXSL("PRESC_MAS_SLNO", "PRESCRIPTION_MASTER");

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
                _vmMsg.ReturnId = prescriptionMstSl;
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

        private long GetMstSl(int doctorId, long sessionSl)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select PRESC_MAS_SLNO from PRESCRIPTION_MASTER Where DOCTOR_ID=" + doctorId + " AND SESSION_SLNO = " + sessionSl + "";
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

        public List<SessionModel> GetSessionSL()
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select SESSION_SLNO,TO_CHAR(ENTRY_DATE,'dd/MM/yyyy')ENTRY_DATE,TO_CHAR(PURCHASE_DATE,'dd/MM/yyyy')PURCHASE_DATE from USER_WORKING_SESSION Where USER_CODE ='" + HttpContext.Current.Session["mappingUserID"] + "' Order By SESSION_SLNO Desc";
                OracleCommand cmd = new OracleCommand(query, con);
                List<SessionModel> sessionList = new List<SessionModel>();
                con.Open();

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SessionModel model = new SessionModel();
                        model.SessionSl = reader["SESSION_SLNO"].ToString();// Convert.ToInt64(reader["SESSION_SLNO"]);
                        model.EntryDate = reader["ENTRY_DATE"].ToString();
                        model.PurchaseDate = reader["PURCHASE_DATE"].ToString();
                        sessionList.Add(model);
                    }
                    return sessionList;
                }
            }
        }

        public List<PrescriptionModel> GetPrescriptionList(string prscNo)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select * FROM PRESCRIPTION_MASTER WHERE  LENGTH(DOCTOR_ID) > 0 AND PRESC_MAS_SLNO = '" + prscNo + "'";
                OracleCommand cmd = new OracleCommand(query, con);
                List<PrescriptionModel> prescList = new List<PrescriptionModel>();
                con.Open();

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PrescriptionModel model = new PrescriptionModel();
                        model.SessionSl = Convert.ToInt64(reader["SESSION_SLNO"].ToString());
                        model.PrescriptionMstSl = Convert.ToInt64(reader["PRESC_MAS_SLNO"].ToString());
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"].ToString());
                        prescList.Add(model);
                    }
                    return prescList;
                }
            }
        }

        public List<Product> GetSInstituteProduct(int InstituteCode, long sessionSl)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {

                string query = "Select PD.PRESC_DET_SLNO,PD.PRESC_MAS_SLNO,PD.PROD_ID,P.PRODUCT_NAME,DSG_STRENGTH_NAME,DOSAGE_FORM_NAME,NVL(PD.PURCHASE_QTY,0)PURCHASE_QTY,NVL(PD.UNIT_PRICE,0)UNIT_PRICE " +
                              "from   PRESCRIPTION_DETAIL PD Inner Join PRESCRIPTION_MASTER PM ON PD.PRESC_MAS_SLNO = PM.PRESC_MAS_SLNO " +
                              "INNER JOIN PRODUCT P ON PD.PROD_ID = P.PROD_ID " +
                              "INNER JOIN DOSAGE_FORM D ON P.DOSAGE_FORM_CODE =  D.DOSAGE_FORM_CODE " +
                              "INNER JOIN DOSAGE_STRENGTH DS ON P.DSG_STRENGTH_CODE = DS.DSG_STRENGTH_CODE  " +
                              "WHERE PM.INSTI_CODE = " + InstituteCode + " AND PM.PRESC_MAS_SLNO = " + sessionSl + "";
                //string query = "Select PD.PRESC_MAS_SLNO, PD.PRESC_DET_SLNO, PR.PROD_ID, PR.PRODUCT_NAME, DF.DOSAGE_FORM_NAME, DS.DSG_STRENGTH_NAME,NVL(PD.PURCHASE_QTY,0)PURCHASE_QTY,NVL(PD.UNIT_PRICE,0)UNIT_PRICE " +
                //                "From Prescription_Detail pd, Product pr, Dosage_Form df, Dosage_Strength ds " +
                //                "Where PD.PRESC_MAS_SLNO = '" + PrescNo + "'" +
                //                "AND PR.PROD_ID = PD.PROD_ID " +
                //                "AND DF.DOSAGE_FORM_CODE = PR.DOSAGE_FORM_CODE " +
                //                "AND DS.DSG_STRENGTH_CODE = PR.DSG_STRENGTH_CODE";

                OracleCommand cmd = new OracleCommand(query, con);
                List<Product> products = new List<Product>();
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product model = new Product();
                        model.PrescriptionDtlSl = Convert.ToInt64(reader["PRESC_DET_SLNO"]);
                        model.PrescriptionMstSl = Convert.ToInt64(reader["PRESC_MAS_SLNO"]);
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

        public List<PrescriptionModel> GetInstPrescriptionList(string prscNo)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                //string query = "Select * FROM PRESCRIPTION_MASTER WHERE UPPER(PRESC_CATE_CODE)=UPPER('o')";
                string query = "Select * FROM PRESCRIPTION_MASTER WHERE  LENGTH(INSTI_CODE) > 0 AND PRESC_MAS_SLNO = '" + prscNo + "'";
                OracleCommand cmd = new OracleCommand(query, con);
                List<PrescriptionModel> prescList = new List<PrescriptionModel>();
                con.Open();

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PrescriptionModel model = new PrescriptionModel();
                        model.SessionSl = Convert.ToInt64(reader["SESSION_SLNO"].ToString());
                        model.PrescriptionMstSl = Convert.ToInt64(reader["PRESC_MAS_SLNO"].ToString());
                        model.InstituteCode = Convert.ToInt32(reader["INSTI_CODE"].ToString());
                        prescList.Add(model);
                    }
                    return prescList;
                }
            }
        }

        public List<PrescriptionModel> GetOTCPrescriptionList()
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select * FROM PRESCRIPTION_MASTER WHERE UPPER(PRESC_CATE_CODE)=UPPER('T')";
                OracleCommand cmd = new OracleCommand(query, con);
                List<PrescriptionModel> prescList = new List<PrescriptionModel>();
                con.Open();

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PrescriptionModel model = new PrescriptionModel();
                        model.SessionSl = Convert.ToInt64(reader["SESSION_SLNO"].ToString());
                        model.PrescriptionMstSl = Convert.ToInt64(reader["PRESC_MAS_SLNO"].ToString());
                        prescList.Add(model);
                    }
                    return prescList;
                }
            }
        }

        public List<DoctorMarket> GetSessionDoctorsData(int sessionSl)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select d.DOCTOR_ID,DOCTOR_NAME,DEGREE,DESIGNATION,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,M.MARKET_CODE,MD.MARKET_NAME " +
                               "from   PRESCRIPTION_MASTER M Inner Join DOCTOR d on M.DOCTOR_ID = D.DOCTOR_ID " +
                               "Inner Join MARKET MD on M.MARKET_CODE = MD.MARKET_CODE " +
                               "Where  M.SESSION_SLNO=" + sessionSl + "";
                OracleCommand cmd = new OracleCommand(query, con);
                List<DoctorMarket> doctors = new List<DoctorMarket>();
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorMarket model = new DoctorMarket();
                        model.DoctorId = Convert.ToInt64(reader["DOCTOR_ID"]);
                        model.DoctorName = reader["DOCTOR_NAME"].ToString();
                        model.Designation = reader["DESIGNATION"].ToString();
                        model.Degree = reader["DEGREE"].ToString();
                        model.Address1 = reader["ADDRESS1"].ToString();
                        model.Address2 = reader["ADDRESS2"].ToString();
                        model.Address3 = reader["ADDRESS3"].ToString();
                        model.Address4 = reader["ADDRESS4"].ToString();
                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        model.MarketName = reader["MARKET_NAME"].ToString();

                        doctors.Add(model);
                    }
                    return doctors;
                }
            }
        }

        public DoctorMarket GetDoctorInfo(int DocID)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select d.DOCTOR_ID,DOCTOR_NAME,DEGREE,DESIGNATION,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,M.MARKET_CODE,MD.MARKET_NAME from   PRESCRIPTION_MASTER M Inner Join DOCTOR d on M.DOCTOR_ID = D.DOCTOR_ID Inner Join MARKET MD on M.MARKET_CODE = MD.MARKET_CODE " +
                                "where M.DOCTOR_ID = '" + DocID + "' ";

                OracleCommand cmd = new OracleCommand(query, con);
                DoctorMarket doctors = new DoctorMarket();
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        doctors.DoctorId = Convert.ToInt64(reader["DOCTOR_ID"]);
                        doctors.DoctorName = reader["DOCTOR_NAME"].ToString();
                        doctors.Designation = reader["DESIGNATION"].ToString();
                        doctors.Degree = reader["DEGREE"].ToString();
                        doctors.Address1 = reader["ADDRESS1"].ToString();
                        doctors.Address2 = reader["ADDRESS2"].ToString();
                        doctors.Address3 = reader["ADDRESS3"].ToString();
                        doctors.Address4 = reader["ADDRESS4"].ToString();
                        doctors.MarketCode = reader["MARKET_CODE"].ToString();
                        doctors.MarketName = reader["MARKET_NAME"].ToString();
                    }
                }

                return doctors;
            }
        }

        public InstitutionInformationModel GetInstInfo(int InstituteCode)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                //string query = "Select * from INSTITUTION " +
                //                "where INSTI_CODE in (Select INSTI_CODE From PRESCRIPTION_MASTER  where PRESC_MAS_SLNO = '" + PrescNo + "')";

                //string query = "Select d.INSTI_CODE,DOCTOR_NAME,DEGREE,DESIGNATION,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,M.MARKET_CODE,MD.MARKET_NAME from   PRESCRIPTION_MASTER M Inner Join INSTITUTION d on M.DOCTOR_ID = D.DOCTOR_ID Inner Join MARKET MD on M.MARKET_CODE = MD.MARKET_CODE " +
                //                "where M.DOCTOR_ID = '" + DocID + "' ";

                string query = "Select d.INSTI_CODE,d.INSTI_NAME,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,M.MARKET_CODE,MD.MARKET_NAME from   PRESCRIPTION_MASTER M Inner Join INSTITUTION d on M.INSTI_CODE = D.INSTI_CODE Inner Join MARKET MD on M.MARKET_CODE = MD.MARKET_CODE where M.INSTI_CODE = " + InstituteCode + "";

                OracleCommand cmd = new OracleCommand(query, con);
                InstitutionInformationModel inst = new InstitutionInformationModel();
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inst.INSTI_CODE = reader["INSTI_CODE"].ToString();
                        inst.INSTI_NAME = reader["INSTI_NAME"].ToString();
                        inst.ADDRESS2 = reader["ADDRESS2"].ToString();
                        inst.ADDRESS1 = reader["ADDRESS1"].ToString();
                        inst.ADDRESS2 = reader["ADDRESS2"].ToString();
                        inst.ADDRESS3 = reader["ADDRESS3"].ToString();
                        inst.ADDRESS4 = reader["ADDRESS4"].ToString();
                    }
                }

                return inst;
            }
        }

        public List<Product> GetSearchProduct(int doctorId, long sessionSl)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select PD.PRESC_DET_SLNO,PD.PRESC_MAS_SLNO,PD.PROD_ID,P.PRODUCT_NAME,DSG_STRENGTH_NAME,DOSAGE_FORM_NAME,NVL(PD.PURCHASE_QTY,0)PURCHASE_QTY,NVL(PD.UNIT_PRICE,0)UNIT_PRICE " +
                              "from   PRESCRIPTION_DETAIL PD Inner Join PRESCRIPTION_MASTER PM ON PD.PRESC_MAS_SLNO = PM.PRESC_MAS_SLNO " +
                              "INNER JOIN PRODUCT P ON PD.PROD_ID = P.PROD_ID " +
                              "INNER JOIN DOSAGE_FORM D ON P.DOSAGE_FORM_CODE =  D.DOSAGE_FORM_CODE " +
                              "INNER JOIN DOSAGE_STRENGTH DS ON P.DSG_STRENGTH_CODE = DS.DSG_STRENGTH_CODE  " +
                              "WHERE PM.DOCTOR_ID = " + doctorId + " AND PM.PRESC_MAS_SLNO = " + sessionSl + "";

                OracleCommand cmd = new OracleCommand(query, con);
                List<Product> products = new List<Product>();
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product model = new Product();
                        model.PrescriptionDtlSl = Convert.ToInt64(reader["PRESC_DET_SLNO"]);
                        model.PrescriptionMstSl = Convert.ToInt64(reader["PRESC_MAS_SLNO"]);
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

        public ValidationMsg DeleteData(long? sl)
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

        public ValidationMsg Delete(int doctorId, long sessionSl)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string query = "Delete from PRESCRIPTION_MASTER Where DOCTOR_ID=" + doctorId + " AND PRESC_MAS_SLNO=" + sessionSl + "";
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

        public List<ReportModel> MarketList()
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



        public List<PrescriptionModel> OMarketList()
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select * From LOCATION_VUE";
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                List<PrescriptionModel> markets = new List<PrescriptionModel>();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PrescriptionModel model = new PrescriptionModel();
                        //model.ProductId = reader["PROD_ID"].ToString();
                        model.MARKET_CODE_O = reader["MARKET_CODE"].ToString();
                        model.MARKET_NAME_O = reader["MARKET_NAME"].ToString();
                        model.TERRITORY_CODE_O = reader["TERRITORY_CODE"].ToString();
                        model.TERRITORY_NAME_O = reader["TERRITORY_NAME"].ToString();
                        model.REGION_CODE_O = reader["REGION_CODE"].ToString();
                        model.REGION_NAME_O = reader["REGION_NAME"].ToString();
                        model.DIVISION_CODE_O = reader["DIVISION_CODE"].ToString();
                        model.DIVISION_NAME_O = reader["DIVISION_NAME"].ToString();
                        model.ZONE_CODE_O = reader["ZONE_CODE"].ToString();
                        model.ZONE_NAME_O = reader["ZONE_NAME"].ToString();
                        markets.Add(model);
                    }
                    return markets;
                }
            }
        }


    }

    public class Market
    {
        public string MarketCode { get; set; }
        public string MarketName { get; set; }

        public string SBU_GROUP { get; set; }
    }

    public class Product
    {
        public long PrescriptionDtlSl { get; set; }
        public long PrescriptionMstSl { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string DosageFormCode { get; set; }
        public string DosageForm { get; set; }
        public string DosageStrengthCode { get; set; }
        public string DosageStrength { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? Quantity { get; set; }
    }

    public class DoctorMarket
    {
        public long DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Degree { get; set; }
        public string Designation { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
    }

}