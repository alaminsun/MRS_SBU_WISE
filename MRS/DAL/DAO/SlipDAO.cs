using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{

    public class SlipDAO
    {

        private readonly DBHelper _dbHelper = new DBHelper();
        //DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        //List<InstituteCoverMarketInformationDtlModel> items = new List<InstituteCoverMarketInformationDtlModel>();
        DBConnection dbCon = new DBConnection();
        IDGenerated idGenerated = new IDGenerated();

        public List<SlipMasterModel> GetMarketInfoList()
        {
            string Qry = "";
            Qry = "SELECT * FROM MARKET ORDER BY MARKET_CODE";

            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<SlipMasterModel> items;
            items = (from DataRow row in dt.Rows
                     select new SlipMasterModel
                     {
                         SP_MARKET_CODE = row["MARKET_CODE"].ToString(),
                         SP_MARKET_NAME = row["MARKET_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<SlipDetailModel> GetProductInfoList()
        {

            string Qry = "";
            //Qry = "SELECT PRODUCT_NAME, DOSAGE_FORM_CODE, DSG_STRENGTH_CODE, NVL(UNIT_PRICE,0) UNIT_PRICE, PROD_ID FROM PRODUCT";
            Qry = "SELECT PRODUCT.PRODUCT_NAME, PRODUCT.DOSAGE_FORM_CODE,DOSAGE_FORM.DOSAGE_FORM_NAME, DOSAGE_STRENGTH.DSG_STRENGTH_NAME ,PRODUCT.DSG_STRENGTH_CODE, " +
                   "NVL(PRODUCT.UNIT_PRICE,0)UNIT_PRICE, PROD_ID FROM PRODUCT, DOSAGE_FORM, DOSAGE_STRENGTH " +
                    "Where   PRODUCT.DOSAGE_FORM_CODE = DOSAGE_FORM.DOSAGE_FORM_CODE AND PRODUCT.DSG_STRENGTH_CODE = DOSAGE_STRENGTH.DSG_STRENGTH_CODE";

            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<SlipDetailModel> items;
            items = (from DataRow row in dt.Rows
                     select new SlipDetailModel
                     {
                         PRODUCT_NAME = row["PRODUCT_NAME"].ToString(),
                         DOSAGE_FORM_CODE = row["DOSAGE_FORM_CODE"].ToString(),
                         DOSAGE_FORM_NAME = row["DOSAGE_FORM_NAME"].ToString(),
                         DSG_STRENGTH_CODE = row["DSG_STRENGTH_CODE"].ToString(),
                         DSG_STRENGTH_NAME = row["DSG_STRENGTH_NAME"].ToString(),
                         PROD_ID = row["PROD_ID"].ToString(),
                         UNIT_PRICE = Convert.ToDecimal(row["UNIT_PRICE"].ToString())
                     }).ToList();

            return items;
        }

        public ValidationMsg Save(SlipMasterModel master, string userid)
        {

            try
            {
                //string SlipCategory = "S";
                //master.PRESC_CATE_CODE = SlipCategory;

                ////_vmMsg = new ValidationMsg();
                //master.PRESC_MAS_SLNO = idGenerated.getMAXSL("PRESC_MAS_SLNO", "PRESCRIPTION_MASTER");

                //if (master.SlipDetailModels.Count() > 0)
                //{
                //    //MstSL = GetMstSLNO();
                //    //string mstSL = MstSL.ToString();
                //    string qry = "INSERT INTO PRESCRIPTION_MASTER(PRESC_MAS_SLNO,MARKET_CODE,SESSION_SLNO,PRESC_CATE_CODE) VALUES(" + master.PRESC_MAS_SLNO + ", '" + master.MARKET_CODE + "'," + HttpContext.Current.Session["SessionSl"] + ",'" + master.PRESC_CATE_CODE + "')";
                //    _dbHelper.CmdExecute(qry);
                //    foreach (var detailModel in master.SlipDetailModels)
                //    {
                //        detailModel.PRESC_DET_SLNO = idGenerated.getMAXSL("PRESC_DET_SLNO", "PRESCRIPTION_DETAIL");
                //        qry = "INSERT INTO PRESCRIPTION_DETAIL(PRESC_MAS_SLNO, PRESC_DET_SLNO, PROD_ID, PURCHASE_QTY, UNIT_PRICE) VALUES('" + master.PRESC_MAS_SLNO + "', '" + detailModel.PRESC_DET_SLNO + "', '" + detailModel.PROD_ID + "', '" + detailModel.PURCHASE_QTY + "','" + detailModel.UNIT_PRICE + "' )";
                //        _dbHelper.CmdExecute(qry);
                //    }
                //    _validationMsg.Type = Enums.MessageType.Success;
                //    _validationMsg.Msg = "Save Successful.";
                //}
                if (IsMSTExitsByInstituteCode(master.SP_MARKET_CODE))
                {
                    if (master.SlipDetailModels.Count() > 0)
                    {
                        string mstSL = GetMstSLNO(master.SP_MARKET_CODE);
                        master.SP_PRESC_MAS_SLNO = Convert.ToInt64(mstSL);
                        //string qry = "INSERT INTO INSTI_MKT_MAS(INSTI_MKT_MAS_SLNO,INSTI_CODE) VALUES(" + master.INSTI_MKT_MAS_SLNO + ", '" + master.INSTI_CODE + "')";
                        //_dbHelper.CmdExecute(qry);
                        foreach (var detailModel in master.SlipDetailModels)
                            if ((IsMSTExits(mstSL)) && (IsDTLExits(detailModel.PROD_ID, mstSL)))
                            {
                                string query = "Update PRESCRIPTION_DETAIL Set PROD_ID='" + detailModel.PROD_ID + "', PURCHASE_QTY='" + detailModel.PURCHASE_QTY + "', UNIT_PRICE='" + detailModel.UNIT_PRICE + "'  WHERE PRESC_DET_SLNO = '" + detailModel.SP_PRESC_DET_SLNO + "'";
                                _dbHelper.CmdExecute(query);


                            }
                            else
                            {
                                detailModel.SP_PRESC_DET_SLNO = idGenerated.getMAXSL("PRESC_DET_SLNO", "PRESCRIPTION_DETAIL");
                                string qry1 = "INSERT INTO PRESCRIPTION_DETAIL(PRESC_MAS_SLNO, PRESC_DET_SLNO, PROD_ID, PURCHASE_QTY, UNIT_PRICE) VALUES('" + master.SP_PRESC_MAS_SLNO + "', '" + detailModel.SP_PRESC_DET_SLNO + "', '" + detailModel.PROD_ID + "', '" + detailModel.PURCHASE_QTY + "','" + detailModel.UNIT_PRICE + "' )";
                                _dbHelper.CmdExecute(qry1);
                            }
                        _validationMsg.Type = Enums.MessageType.Success;
                        _validationMsg.Msg = "Save Successful.";
                    }
                }
                else
                {
                    string SlipCategory = "S";
                    master.SP_PRESC_CAT_CODE = SlipCategory;
                    master.SP_PRESC_MAS_SLNO = idGenerated.getMAXSL("PRESC_MAS_SLNO", "PRESCRIPTION_MASTER");
                    if (master.SlipDetailModels.Count() > 0)
                    {
                        string qry = "INSERT INTO PRESCRIPTION_MASTER(PRESC_MAS_SLNO,MARKET_CODE,SESSION_SLNO,PRESC_CATE_CODE) VALUES(" + master.SP_PRESC_MAS_SLNO + ", '" + master.SP_MARKET_CODE + "'," + HttpContext.Current.Session["SessionSl"] + ",'" + master.SP_PRESC_CAT_CODE + "')";
                        _dbHelper.CmdExecute(qry);
                        foreach (var detailModel in master.SlipDetailModels)
                        {
                            detailModel.SP_PRESC_DET_SLNO = idGenerated.getMAXSL("PRESC_DET_SLNO", "PRESCRIPTION_DETAIL");
                            qry = "INSERT INTO PRESCRIPTION_DETAIL(PRESC_MAS_SLNO, PRESC_DET_SLNO, PROD_ID, PURCHASE_QTY, UNIT_PRICE) VALUES('" + master.SP_PRESC_MAS_SLNO + "', '" + detailModel.SP_PRESC_DET_SLNO + "', '" + detailModel.PROD_ID + "', '" + detailModel.PURCHASE_QTY + "','" + detailModel.UNIT_PRICE + "' )";
                            _dbHelper.CmdExecute(qry);
                        }
                        _validationMsg.Type = Enums.MessageType.Success;
                        _validationMsg.Msg = "Save Successful.";
                    }
                }
            }

            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("ORA-00001"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "This Data already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Value larger than specified precision allowed.";
                }
                if (ex.Message.Contains("ORA-01400"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Fill The Required Field.";
                }
            }
            return _validationMsg;
        }

        private bool IsDTLExits(string prodId, string mstSL)
        {
            bool isTrue = false;
            string Qry = "SELECT * FROM PRESCRIPTION_DETAIL  WHERE PROD_ID = '" + prodId + "' and PRESC_MAS_SLNO =" + mstSL + "";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }

        private bool IsMSTExits(string mstSL)
        {
            bool isTrue = false;
            string Qry = "SELECT * FROM PRESCRIPTION_MASTER WHERE PRESC_MAS_SLNO = " + mstSL + "";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }

        private bool IsMSTExitsByInstituteCode(string marketCode)
        {
            bool isTrue = false;
            string Qry = "SELECT * FROM PRESCRIPTION_MASTER WHERE MARKET_CODE = '" + marketCode + "'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }



        public ValidationMsg Update(SlipMasterModel master, string userid)
        {
            try
            {
                //int? MstSL;
                OracleConnection oracleConnection = new OracleConnection(dbCon.StringRead());
                //_vmMsg = new ValidationMsg();
                //MstSL = GetMstSLNO();
                //maxID = MstSL.ToString();
                foreach (var detailModel in master.SlipDetailModels)
                {
                    if (detailModel.SP_PRESC_DET_SLNO == 0)
                    {
                        string prescriptionMstSl = GetMstSLNO(master.SP_MARKET_CODE);
                        detailModel.SP_PRESC_DET_SLNO = idGenerated.getMAXSL("PRESC_DET_SLNO", "PRESCRIPTION_DETAIL");
                        string qry = "INSERT INTO PRESCRIPTION_DETAIL(PRESC_MAS_SLNO, PRESC_DET_SLNO, PROD_ID, PURCHASE_QTY, UNIT_PRICE) VALUES('" + prescriptionMstSl + "', '" + detailModel.SP_PRESC_DET_SLNO + "', '" + detailModel.PROD_ID + "', '" + detailModel.PURCHASE_QTY + "','" + detailModel.UNIT_PRICE + "' )";
                        _dbHelper.CmdExecute(qry);
                    }
                    else
                    {
                        string query = "Update PRESCRIPTION_DETAIL Set PROD_ID='" + detailModel.PROD_ID + "', PURCHASE_QTY='" + detailModel.PURCHASE_QTY + "', UNIT_PRICE='" + detailModel.UNIT_PRICE + "'  WHERE PRESC_DET_SLNO = '" + detailModel.SP_PRESC_DET_SLNO + "'";
                        _dbHelper.CmdExecute(query);
                    }
                }
                _validationMsg.Type = Enums.MessageType.Success;
                _validationMsg.Msg = "Update Successful.";

            }

            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("ORA-00001"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "This Data already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Value larger than specified precision allowed.";
                }
                if (ex.Message.Contains("ORA-01400"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Fill The Required Field.";
                }
            }

            return _validationMsg;
        }

        private string GetMstSLNO(string marketCode)
        {
            string SL = "0";
            string Qry = "Select PRESC_MAS_SLNO from PRESCRIPTION_MASTER Where MARKET_CODE='" + marketCode + "'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                SL = dt.Rows[0][0].ToString();
            }

            return SL;
            //using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            //{
            //    string query = "Select PRESC_MAS_SLNO from PRESCRIPTION_MASTER Where MARKET_CODE=" + marketCode + "";
            //    int slNo = 0;
            //    OracleCommand cmd = new OracleCommand(query, con);
            //    con.Open();
            //    using (OracleDataReader reader = cmd.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            slNo = Convert.ToInt64(reader["PRESC_MAS_SLNO"]);
            //        }
            //        return slNo;
            //    }
            //}
        }

        public List<SlipMasterModel> GetMarketList()
        {
            string Qry = "";
            Qry = "Select m.PRESC_MAS_SLNO, m.SESSION_SLNO, m.PRESC_CATE_CODE, m.MARKET_CODE, s.MARKET_NAME FROM PRESCRIPTION_MASTER m,MARKET s WHERE M.MARKET_CODE = S.MARKET_CODE ORDER BY M.MARKET_CODE ";

            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<SlipMasterModel> items;
            items = (from DataRow row in dt.Rows
                     select new SlipMasterModel
                     {
                         SP_PRESC_MAS_SLNO = Convert.ToInt64(row["PRESC_MAS_SLNO"].ToString()),
                         SessionSl = Convert.ToInt64(row["SESSION_SLNO"].ToString()),
                         SP_PRESC_CAT_CODE = row["PRESC_CATE_CODE"].ToString(),
                         SP_MARKET_CODE = row["MARKET_CODE"].ToString(),
                         SP_MARKET_NAME = row["MARKET_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<SlipDetailModel> GetProductList()
        {
            string Qry = "";
            Qry = "Select d.PRESC_MAS_SLNO, d.PRESC_DET_SLNO, d.PROD_ID, d.PURCHASE_QTY, d.UNIT_PRICE, S.DOSAGE_FORM_CODE, A.DOSAGE_FORM_NAME, B.DSG_STRENGTH_NAME, S.DSG_STRENGTH_CODE, s.PRODUCT_NAME FROM PRESCRIPTION_DETAIL d,PRODUCT s, DOSAGE_FORM a, DOSAGE_STRENGTH b WHERE D.PROD_ID = S.PROD_ID AND S.DOSAGE_FORM_CODE = A.DOSAGE_FORM_CODE AND S.DSG_STRENGTH_CODE = B.DSG_STRENGTH_CODE ";

            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<SlipDetailModel> items;
            items = (from DataRow row in dt.Rows
                     select new SlipDetailModel
                     {
                         SP_PRESC_MAS_SLNO = Convert.ToInt16(row["PRESC_MAS_SLNO"].ToString()),
                         SP_PRESC_DET_SLNO = Convert.ToInt16(row["PRESC_DET_SLNO"].ToString()),
                         PROD_ID = row["PROD_ID"].ToString(),
                         DOSAGE_FORM_CODE = row["DOSAGE_FORM_CODE"].ToString(),
                         DOSAGE_FORM_NAME = row["DOSAGE_FORM_NAME"].ToString(),
                         DSG_STRENGTH_CODE = row["DSG_STRENGTH_CODE"].ToString(),
                         DSG_STRENGTH_NAME = row["DSG_STRENGTH_NAME"].ToString(),
                         PURCHASE_QTY = Convert.ToInt16(row["PURCHASE_QTY"].ToString()),
                         UNIT_PRICE = Convert.ToDecimal(row["UNIT_PRICE"].ToString()),
                         PRODUCT_NAME = row["PRODUCT_NAME"].ToString()
                     }).ToList();

            return items;
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
                        model.SessionSl = reader["SESSION_SLNO"].ToString(); // Convert.ToInt64(reader["SESSION_SLNO"]);
                        model.EntryDate = reader["ENTRY_DATE"].ToString();
                        model.PurchaseDate = reader["PURCHASE_DATE"].ToString();
                        sessionList.Add(model);
                    }
                    return sessionList;
                }
            }
        }

        public List<SlipDetailModel> GetSearchProduct(string marketCode, string sessionSl)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select PD.PRESC_DET_SLNO,PD.PRESC_MAS_SLNO,PD.PROD_ID,P.PRODUCT_NAME,DSG_STRENGTH_NAME,DOSAGE_FORM_NAME,NVL(PD.PURCHASE_QTY,0)PURCHASE_QTY,NVL(PD.UNIT_PRICE,0)UNIT_PRICE " +
                              "from   PRESCRIPTION_DETAIL PD Inner Join PRESCRIPTION_MASTER PM ON PD.PRESC_MAS_SLNO = PM.PRESC_MAS_SLNO " +
                              "INNER JOIN PRODUCT P ON PD.PROD_ID = P.PROD_ID " +
                              "INNER JOIN DOSAGE_FORM D ON P.DOSAGE_FORM_CODE =  D.DOSAGE_FORM_CODE " +
                              "INNER JOIN DOSAGE_STRENGTH DS ON P.DSG_STRENGTH_CODE = DS.DSG_STRENGTH_CODE  " +
                              "WHERE PM.MARKET_CODE = " + marketCode + " AND SESSION_SLNO = " + sessionSl + "";

                OracleCommand cmd = new OracleCommand(query, con);
                List<SlipDetailModel> products = new List<SlipDetailModel>();
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SlipDetailModel model = new SlipDetailModel();
                        model.SP_PRESC_DET_SLNO = Convert.ToInt64(reader["PRESC_DET_SLNO"]);
                        model.SP_PRESC_MAS_SLNO = Convert.ToInt64(reader["PRESC_MAS_SLNO"]);
                        model.PROD_ID = reader["PROD_ID"].ToString();
                        model.PRODUCT_NAME = reader["PRODUCT_NAME"].ToString();
                        model.DSG_STRENGTH_NAME = reader["DSG_STRENGTH_NAME"].ToString();
                        model.DOSAGE_FORM_NAME = reader["DOSAGE_FORM_NAME"].ToString();
                        model.PURCHASE_QTY = Convert.ToInt32(reader["PURCHASE_QTY"]);
                        model.UNIT_PRICE = Convert.ToDecimal(reader["UNIT_PRICE"]);

                        products.Add(model);
                    }
                    return products;
                }
            }
        }



        public List<SlipMasterModel> GetSessionMarketData(int sessionSl)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select distinct M.PRESC_MAS_SLNO, M.MARKET_CODE,M.PRESC_CATE_CODE,MD.MARKET_NAME " +
                               "from   PRESCRIPTION_MASTER M " +
                               "Inner Join MARKET MD on M.MARKET_CODE = MD.MARKET_CODE " +
                               "Where  M.SESSION_SLNO=" + sessionSl + " AND PRESC_CATE_CODE = 'S' ";
                OracleCommand cmd = new OracleCommand(query, con);
                List<SlipMasterModel> markets = new List<SlipMasterModel>();
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SlipMasterModel model = new SlipMasterModel();
                        model.SP_MARKET_CODE = reader["MARKET_CODE"].ToString();
                        model.SP_MARKET_NAME = reader["MARKET_NAME"].ToString();
                        model.SP_PRESC_MAS_SLNO = Convert.ToInt64(reader["PRESC_MAS_SLNO"].ToString());
                        model.SP_PRESC_CAT_CODE = reader["PRESC_CATE_CODE"].ToString();
                        //model.Degree = reader["DEGREE"].ToString();
                        //model.Address1 = reader["ADDRESS1"].ToString();
                        //model.Address2 = reader["ADDRESS2"].ToString();
                        //model.Address3 = reader["ADDRESS3"].ToString();
                        //model.Address4 = reader["ADDRESS4"].ToString();
                        //model.MarketCode = reader["MARKET_CODE"].ToString();
                        //model.MarketName = reader["MARKET_NAME"].ToString();

                        markets.Add(model);
                    }
                    return markets;
                }
            }
        }

        public ValidationMsg DeleteData(long? sl)
        {
            try
            {
                string query = "Delete from PRESCRIPTION_DETAIL Where PRESC_DET_SLNO=" + sl + "";
                if (_dbHelper.CmdExecute(query) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to Delete Data.";
            }
            return _validationMsg;

        }

        public ValidationMsg Delete(string marketCode)
        {
            try
            {
                string query = "Delete from PRESCRIPTION_MASTER Where MARKET_CODE=" + marketCode + "";
                if (_dbHelper.CmdExecute(query) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to Delete Data.";
            }
            return _validationMsg;
        }
    }
}