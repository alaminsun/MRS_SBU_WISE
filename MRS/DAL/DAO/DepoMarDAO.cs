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
    public class DepoMarDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;

        public int DepoSl = 0;

        public ValidationMsg Save(DepoMarMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region DepoMarMas

                    string query = "select NVL(MAX(DEPOT_MKT_MAS_SLNO),0)+1 from DEPOT_MKT_MAS";
                    DataTable dt = dbHelper.GetDataTable(query);
                    if (dt.Rows.Count > 0)
                        model.DEPOT_MKT_MAS_SLNO = dt.Rows[0][0].ToString();

                    string qry = "INSERT INTO DEPOT_MKT_MAS(DEPOT_MKT_MAS_SLNO,DEPOT_CODE)" +
                    "VALUES(" + model.DEPOT_MKT_MAS_SLNO + ", '" + model.DEPOT_CODE + "')";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        DepoSl = Convert.ToInt16(model.DEPOT_MKT_MAS_SLNO);
                    }

                    #endregion

                    #region DepoMarDtl

                    if (model.MarketList != null)
                    {
                        foreach (DepoMarDtlModel objMarsonModel in model.MarketList)
                        {
                            DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(DEPOT_MKT_DTL_SLNO),0)+1 from DEPOT_MKT_DTL");
                            if (dtDtl.Rows.Count > 0)
                                objMarsonModel.DEPOT_MKT_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                            objMarsonModel.DEPOT_MKT_MAS_SLNO = DepoSl.ToString();

                            qry = "INSERT INTO DEPOT_MKT_DTL(DEPOT_MKT_DTL_SLNO,DEPOT_MKT_MAS_SLNO,MARKET_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                            "VALUES('" + objMarsonModel.DEPOT_MKT_DTL_SLNO + "', '" + model.DEPOT_MKT_MAS_SLNO + "', '" + objMarsonModel.MARKET_CODE + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                            if (dbHelper.CmdExecute(qry) > 0)
                            {
                                _vmMsg.Type = Enums.MessageType.Success;

                            }
                        }
                    }

                    #endregion

                    scope.Complete();

                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Saved Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Save.";

                if (ex.Message.Contains("MRS.UK_DEPOT_MKT_MAS_DEPOT_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Depo already Exist.";
                }
                else if (ex.Message.Contains("MRS.UK_DEPOT_MARKET_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Market already Exist.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Update(DepoMarMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                #region Update

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region DepoMarMas

                    string qry = "UPDATE DEPOT_MKT_MAS SET DEPOT_CODE = '" + model.DEPOT_CODE + "' WHERE DEPOT_MKT_MAS_SLNO = '" + model.DEPOT_MKT_MAS_SLNO + "'";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        _vmMsg.Type = Enums.MessageType.Update;
                        _vmMsg.Msg = "Updated Successfully.";
                    }

                    #endregion

                    #region DepoMarDtl

                    if (model.MarketList != null)
                    {
                        foreach (DepoMarDtlModel objMarsonModel in model.MarketList)
                        {
                            if (string.IsNullOrEmpty(objMarsonModel.DEPOT_MKT_DTL_SLNO))
                            {
                                DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(DEPOT_MKT_DTL_SLNO),0)+1 from DEPOT_MKT_DTL");
                                if (dtDtl.Rows.Count > 0)
                                    objMarsonModel.DEPOT_MKT_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                                objMarsonModel.DEPOT_MKT_MAS_SLNO = DepoSl.ToString();

                                qry = "INSERT INTO DEPOT_MKT_DTL(DEPOT_MKT_DTL_SLNO,DEPOT_MKT_MAS_SLNO,MARKET_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objMarsonModel.DEPOT_MKT_DTL_SLNO + "', '" + model.DEPOT_MKT_MAS_SLNO + "', '" + objMarsonModel.MARKET_CODE + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                                if (dbHelper.CmdExecute(qry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Success;
                                }
                            }
                            else
                            {
                                string updateqry = "UPDATE DEPOT_MKT_DTL SET MARKET_CODE = '" + objMarsonModel.MARKET_CODE + "'," +
                                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                             "WHERE DEPOT_MKT_DTL_SLNO = '" + objMarsonModel.DEPOT_MKT_DTL_SLNO + "'";
                                if (dbHelper.CmdExecute(updateqry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Update;
                                    _vmMsg.Msg = "Updated Successfully.";
                                }
                            }
                        }
                    }

                    #endregion

                    scope.Complete();

                    _vmMsg.Type = Enums.MessageType.Update;
                    _vmMsg.Msg = "Updated Successfully.";
                }

                #endregion
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Update.";

                if (ex.Message.Contains("MRS.UK_DEPOT_MKT_MAS_DEPOT_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Depo already Exist.";
                }
                else if (ex.Message.Contains("MRS.UK_DEPOT_MARKET_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Market already Exist.";
                }
            }
            return _vmMsg;
        }

        public List<DepoMarMasModel> GetDepoList()
        {
            string Qry = "";
            Qry = "SELECT DEPOT_CODE ,DEPOT_NAME FROM DEPOT ";
            Qry = Qry + "ORDER BY DEPOT_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DepoMarMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new DepoMarMasModel
                     {
                         DEPOT_CODE = row["DEPOT_CODE"].ToString(),
                         DEPOT_NAME = row["DEPOT_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<DepoMarMasModel> GetDepoSearchList()
        {
            DataTable dt = dbHelper.GetDataTable("select ZM.DEPOT_MKT_MAS_SLNO,Z.DEPOT_CODE,Z.DEPOT_NAME from DEPOT Z inner join DEPOT_MKT_MAS ZM on Z.DEPOT_CODE = ZM.DEPOT_CODE ORDER BY Z.DEPOT_CODE");
            List<DepoMarMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new DepoMarMasModel
                     {
                         DEPOT_MKT_MAS_SLNO = row["DEPOT_MKT_MAS_SLNO"].ToString(),
                         DEPOT_CODE = row["DEPOT_CODE"].ToString(),
                         DEPOT_NAME = row["DEPOT_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<DepoMarDtlModel> GetMarInfoList()
        {
            string Qry = "";
            Qry = "SELECT MARKET_CODE ,MARKET_NAME FROM MARKET ";
            Qry = Qry + "ORDER BY MARKET_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DepoMarDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new DepoMarDtlModel
                     {
                         MARKET_CODE = row["MARKET_CODE"].ToString(),
                         MARKET_NAME = row["MARKET_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<DepoMarDtlModel> GetSaveMarList(string DEPOT_MKT_MAS_SLNO)
        {
            var items = new List<DepoMarDtlModel>();
            DataTable dt = dbHelper.GetDataTable("select ZM.DEPOT_MKT_DTL_SLNO, Z.MARKET_CODE,Z.MARKET_NAME from MARKET Z inner join DEPOT_MKT_DTL ZM on Z.MARKET_CODE = ZM.MARKET_CODE  WHERE DEPOT_MKT_MAS_SLNO = " + DEPOT_MKT_MAS_SLNO + " ORDER BY ZM.DEPOT_MKT_DTL_SLNO");
            items = (from DataRow row in dt.Rows
                     select new DepoMarDtlModel
                     {
                         MARKET_CODE = row["MARKET_CODE"].ToString(),
                         MARKET_NAME = row["MARKET_NAME"].ToString(),
                         DEPOT_MKT_DTL_SLNO = row["DEPOT_MKT_DTL_SLNO"].ToString()
                     }).ToList();

            return items;
        }

        public ValidationMsg Delete(string DEPOT_MKT_MAS_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM DEPOT_MKT_MAS WHERE DEPOT_MKT_MAS_SLNO = " + DEPOT_MKT_MAS_SLNO + "";
                if (dbHelper.CmdExecute(qry) > 0)
                {
                    vmMsg.Type = Enums.MessageType.Success;
                    vmMsg.Msg = "Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                vmMsg.Type = Enums.MessageType.Error;
                vmMsg.Msg = "Failed to Delete.";
            }
            return vmMsg;
        }

        public ValidationMsg DeletedMarInfo(string DEPOT_MKT_DTL_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM DEPOT_MKT_DTL WHERE DEPOT_MKT_DTL_SLNO = " + DEPOT_MKT_DTL_SLNO + "";
                if (dbHelper.CmdExecute(qry) > 0)
                {
                    vmMsg.Type = Enums.MessageType.Success;
                    vmMsg.Msg = "Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                vmMsg.Type = Enums.MessageType.Error;
                vmMsg.Msg = "Failed to Delete.";
            }
            return vmMsg;
        }
    }
}