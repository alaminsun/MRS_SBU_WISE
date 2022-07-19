using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;


namespace MRS.DAL.DAO
{
    public class MarketDocPracLocDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg = new ValidationMsg();
        public int MarketDocPracLocSl = 0;
        private List<MarketDocpraclocMstModel> itemMasterList;
        private List<MarketDocpraclocDtlModel> itemDetailList;



        public List<MarketDocpraclocMstModel> GetMarketList()
        {
            string Qry = "";
            Qry = "SELECT MARKET_CODE ,MARKET_NAME FROM MARKET ";
            Qry = Qry + "ORDER BY MARKET_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);

            itemMasterList = (from DataRow row in dt.Rows
                              select new MarketDocpraclocMstModel
                              {
                                  MARKET_CODE = row["MARKET_CODE"].ToString(),
                                  MARKET_NAME = row["MARKET_NAME"].ToString()
                              }).ToList();

            return itemMasterList;
        }

        public List<MarketDocpraclocDtlModel> GetDoctorPracticeLocationList()
        {
            string Qry = "";
            Qry = "SELECT DP_LOC_CODE ,DP_LOC_NAME FROM DOC_PRACTICE_LOC ";
            Qry = Qry + "ORDER BY DP_LOC_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            itemDetailList = (from DataRow row in dt.Rows
                              select new MarketDocpraclocDtlModel
                              {
                                  DP_LOC_CODE = row["DP_LOC_CODE"].ToString(),
                                  DP_LOC_NAME = row["DP_LOC_NAME"].ToString()
                              }).ToList();

            return itemDetailList;
        }

        public ValidationMsg Save(MarketDocpraclocMstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    DataTable dataTable =
                        dbHelper.GetDataTable(
                            "SELECT (NVL(MAX(MKT_DP_LOC_MAS_SLNO),0)+1)MKT_DP_LOC_MAS_SLNO FROM MKT_DP_LOC_MAS");
                    if (dataTable.Rows.Count > 0)
                    {
                        model.MKT_DP_LOC_MAS_SLNO = dataTable.Rows[0][0].ToString();
                    }
                    string query = "INSERT INTO MKT_DP_LOC_MAS(MKT_DP_LOC_MAS_SLNO,MARKET_CODE)" +
                                   "VALUES(" + model.MKT_DP_LOC_MAS_SLNO + ", '" + model.MARKET_CODE + "')";
                    if (dbHelper.CmdExecute(query) > 0)
                    {
                        MarketDocPracLocSl = Convert.ToInt16(model.MKT_DP_LOC_MAS_SLNO);
                    }
                    if (model.MarketDocpraclocDtlList != null)
                    {
                        foreach (MarketDocpraclocDtlModel objMarketDplocaDtlModel in model.MarketDocpraclocDtlList)
                        {
                            DataTable dataTableDtl =
                                dbHelper.GetDataTable(
                                    "SELECT (NVL(MAX(MKT_DP_LOC_DTL_SLNO),0)+1)MKT_DP_LOC_DTL_SLNO FROM MKT_DP_LOC_DTL");
                            if (dataTableDtl.Rows.Count > 0)
                            {
                                objMarketDplocaDtlModel.MKT_DP_LOC_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
                                objMarketDplocaDtlModel.MKT_DP_LOC_MAS_SLNO = MarketDocPracLocSl.ToString();//New added Line which is in Query Also
                            }
                            string querydtl =
                                "INSERT INTO MKT_DP_LOC_DTL(MKT_DP_LOC_DTL_SLNO,MKT_DP_LOC_MAS_SLNO,DP_LOC_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objMarketDplocaDtlModel.MKT_DP_LOC_DTL_SLNO + "', '" + model.MKT_DP_LOC_MAS_SLNO +
                                "', '" + objMarketDplocaDtlModel.DP_LOC_CODE + "', '" + userid + "',(TO_DATE('" +
                                 model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() +
                                "')";
                            if (dbHelper.CmdExecute(querydtl) > 0)
                            {
                                _vmMsg.Type = Enums.MessageType.Success;

                            }

                        }

                    }
                    scope.Complete();
                    //_vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Saved Successfully.";
                }
            }
            catch (Exception ex)
            {

                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Doctor Practice Location Code Already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }

            return _vmMsg;
        }

        public ValidationMsg Update(MarketDocpraclocMstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string query = "";
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    if (model.MKT_DP_LOC_MAS_SLNO != "")
                    {
                        string qry = "SELECT TC1.MKT_DP_LOC_MAS_SLNO,TC1.MARKET_CODE,TL1.MARKET_NAME FROM MKT_DP_LOC_MAS TC1 INNER JOIN MARKET TL1 ON TC1.MARKET_CODE=TL1.MARKET_CODE";
                        qry = qry + " ORDER BY TC1.MKT_DP_LOC_MAS_SLNO";

                        DataTable dt = dbHelper.GetDataTable(qry);
                        itemMasterList = (from DataRow row in dt.Rows
                                          select new MarketDocpraclocMstModel
                                          {
                                              MARKET_CODE = row["MARKET_CODE"].ToString(),
                                              MARKET_NAME = row["MARKET_NAME"].ToString(),
                                              MKT_DP_LOC_MAS_SLNO = row["MKT_DP_LOC_MAS_SLNO"].ToString()
                                          }).ToList();
                        if (itemMasterList.Count > 0)
                        {
                            query = "UPDATE MKT_DP_LOC_MAS SET MARKET_CODE = '" + model.MARKET_CODE + "' WHERE MKT_DP_LOC_MAS_SLNO = '" + model.MKT_DP_LOC_MAS_SLNO + "'";
                        }

                        dbHelper.CmdExecute(query);
                    }
                    if (model.MarketDocpraclocDtlList != null)
                    {
                        foreach (MarketDocpraclocDtlModel objDistrictUpazilaUpdateDtlModel in model.MarketDocpraclocDtlList)
                        {
                            if (objDistrictUpazilaUpdateDtlModel.MKT_DP_LOC_DTL_SLNO != null)
                            {

                                string querydtl =
                                                "UPDATE MKT_DP_LOC_DTL SET DP_LOC_CODE = '" + objDistrictUpazilaUpdateDtlModel.DP_LOC_CODE + "'," +
                                                "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                                "WHERE MKT_DP_LOC_MAS_SLNO = '" + model.MKT_DP_LOC_MAS_SLNO + "' AND MKT_DP_LOC_DTL_SLNO = '" + objDistrictUpazilaUpdateDtlModel.MKT_DP_LOC_DTL_SLNO + "'";

                                dbHelper.CmdExecute(querydtl);
                            }
                            else
                            {
                                foreach (MarketDocpraclocDtlModel objMarketDplocaDtlModel in model.MarketDocpraclocDtlList)
                                {
                                    DataTable dataTableDtl =
                                        dbHelper.GetDataTable(
                                            "SELECT (NVL(MAX(MKT_DP_LOC_DTL_SLNO),0)+1)MKT_DP_LOC_DTL_SLNO FROM MKT_DP_LOC_DTL");
                                    if (dataTableDtl.Rows.Count > 0)
                                    {
                                        objMarketDplocaDtlModel.MKT_DP_LOC_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
                                        objMarketDplocaDtlModel.MKT_DP_LOC_MAS_SLNO = MarketDocPracLocSl.ToString();//New added Line which is in Query Also
                                    }
                                    string querydtl =
                                        "INSERT INTO MKT_DP_LOC_DTL(MKT_DP_LOC_DTL_SLNO,MKT_DP_LOC_MAS_SLNO,DP_LOC_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                        "VALUES('" + objMarketDplocaDtlModel.MKT_DP_LOC_DTL_SLNO + "', '" + model.MKT_DP_LOC_MAS_SLNO +
                                        "', '" + objMarketDplocaDtlModel.DP_LOC_CODE + "', '" + userid + "',(TO_DATE('" +
                                         model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() +
                                        "')";
                                    dbHelper.CmdExecute(querydtl);
                                }
                            }
                        }
                    }
                    scope.Complete();
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Updated Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Update.";

                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }

        public List<MarketDocpraclocMstModel> GetSearchMasterList()
        {
            string qry = "SELECT TC1.MKT_DP_LOC_MAS_SLNO,TC1.MARKET_CODE,TL1.MARKET_NAME FROM MKT_DP_LOC_MAS TC1 INNER JOIN MARKET TL1 ON TC1.MARKET_CODE=TL1.MARKET_CODE";
            qry = qry + " ORDER BY TC1.MKT_DP_LOC_MAS_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemMasterList = (from DataRow row in dt.Rows
                              select new MarketDocpraclocMstModel
                              {
                                  MARKET_CODE = row["MARKET_CODE"].ToString(),
                                  MARKET_NAME = row["MARKET_NAME"].ToString(),
                                  MKT_DP_LOC_MAS_SLNO = row["MKT_DP_LOC_MAS_SLNO"].ToString()
                              }).ToList();
            return itemMasterList;
        }

        public List<MarketDocpraclocDtlModel> GetSearchDtliList(string marketDoctorPracLocMstSlNo)
        {
            string qry = "SELECT TC2.MKT_DP_LOC_DTL_SLNO,TC2.MKT_DP_LOC_MAS_SLNO,TC2.DP_LOC_CODE,TL2.DP_LOC_NAME FROM MKT_DP_LOC_DTL TC2 INNER JOIN DOC_PRACTICE_LOC TL2 ON TC2.DP_LOC_CODE=TL2.DP_LOC_CODE WHERE TC2.MKT_DP_LOC_MAS_SLNO='" + marketDoctorPracLocMstSlNo + "'";
            qry = qry + " ORDER BY TC2.MKT_DP_LOC_DTL_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemDetailList = (from DataRow row in dt.Rows
                              select new MarketDocpraclocDtlModel
                              {
                                  DP_LOC_CODE = row["DP_LOC_CODE"].ToString(),
                                  DP_LOC_NAME = row["DP_LOC_NAME"].ToString(),
                                  MKT_DP_LOC_DTL_SLNO = row["MKT_DP_LOC_DTL_SLNO"].ToString()
                              }).ToList();
            return itemDetailList;
        }

        public ValidationMsg DeleteDetailData(string marketDocPracLocDtlSlNo)
        {
            try
            {
                string deleteDetailQuery = "DELETE FROM MKT_DP_LOC_DTL WHERE MKT_DP_LOC_DTL_SLNO = '" + marketDocPracLocDtlSlNo + "'";
                if (dbHelper.CmdExecute(deleteDetailQuery) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Delete Grid Data.";
            }
            return _vmMsg;
        }

        public ValidationMsg Delete(string marketDocPracLocMstSlNo)
        {

            try
            {
                string querydtl = "DELETE FROM MKT_DP_LOC_MAS WHERE MKT_DP_LOC_MAS_SLNO = '" +
                                    marketDocPracLocMstSlNo + "'";
                if (dbHelper.CmdExecute(querydtl) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }
            }

            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Delete All Data.";
            }
            return _vmMsg;
        }

    }
}