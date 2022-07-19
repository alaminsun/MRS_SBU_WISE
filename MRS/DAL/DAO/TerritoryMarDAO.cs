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
    public class TerritoryMarDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;

        public int TerritorySl = 0;

        public ValidationMsg Save(TerritoryMarMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region TerritoryMarMas

                    string query = "select NVL(MAX(TERRI_MKT_MAS_SLNO),0)+1 from TERRI_MKT_MAS";
                    DataTable dt = dbHelper.GetDataTable(query);
                    if (dt.Rows.Count > 0)
                        model.TERRI_MKT_MAS_SLNO = dt.Rows[0][0].ToString();

                    string qry = "INSERT INTO TERRI_MKT_MAS(TERRI_MKT_MAS_SLNO,TERRITORY_CODE)" +
                    "VALUES(" + model.TERRI_MKT_MAS_SLNO + ", '" + model.TERRITORY_CODE + "')";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        TerritorySl = Convert.ToInt16(model.TERRI_MKT_MAS_SLNO);
                    }

                    #endregion

                    #region TerritoryMarDtl

                    if (model.MarketList != null)
                    {
                        foreach (TerritoryMarDtlModel objMarsonModel in model.MarketList)
                        {
                            DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(TERRI_MKT_DTL_SLNO),0)+1 from TERRI_MKT_DTL");
                            if (dtDtl.Rows.Count > 0)
                                objMarsonModel.TERRI_MKT_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                            objMarsonModel.TERRI_MKT_MAS_SLNO = TerritorySl.ToString();

                            qry = "INSERT INTO TERRI_MKT_DTL(TERRI_MKT_DTL_SLNO,TERRI_MKT_MAS_SLNO,MARKET_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                            "VALUES('" + objMarsonModel.TERRI_MKT_DTL_SLNO + "', '" + model.TERRI_MKT_MAS_SLNO + "', '" + objMarsonModel.MARKET_CODE + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
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

                if (ex.Message.Contains("MRS.UK_TERRI_MKT_MAS_TERRI_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "MArket already Exist.";
                }
                else if (ex.Message.Contains("MRS.UK_TERR_MKT_DTL_MKT_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Territory already Exist.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Update(TerritoryMarMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                #region Update

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region TerritoryMarMas

                    string qry = "UPDATE TERRI_MKT_MAS SET TERRITORY_CODE = '" + model.TERRITORY_CODE + "' WHERE TERRI_MKT_MAS_SLNO = '" + model.TERRI_MKT_MAS_SLNO + "'";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        _vmMsg.Type = Enums.MessageType.Update;
                        _vmMsg.Msg = "Updated Successfully.";
                    }

                    #endregion

                    #region TerritoryMarDtl

                    if (model.MarketList != null)
                    {
                        foreach (TerritoryMarDtlModel objMarsonModel in model.MarketList)
                        {
                            if (string.IsNullOrEmpty(objMarsonModel.TERRI_MKT_DTL_SLNO))
                            {
                                DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(TERRI_MKT_DTL_SLNO),0)+1 from TERRI_MKT_DTL");
                                if (dtDtl.Rows.Count > 0)
                                    objMarsonModel.TERRI_MKT_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                                objMarsonModel.TERRI_MKT_MAS_SLNO = TerritorySl.ToString();

                                qry = "INSERT INTO TERRI_MKT_DTL(TERRI_MKT_DTL_SLNO,TERRI_MKT_MAS_SLNO,MARKET_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objMarsonModel.TERRI_MKT_DTL_SLNO + "', '" + model.TERRI_MKT_MAS_SLNO + "', '" + objMarsonModel.MARKET_CODE + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                                if (dbHelper.CmdExecute(qry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Success;
                                }
                            }
                            else
                            {
                                string updateqry = "UPDATE TERRI_MKT_DTL SET MARKET_CODE = '" + objMarsonModel.MARKET_CODE + "'," +
                                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                             "WHERE TERRI_MKT_DTL_SLNO = '" + objMarsonModel.TERRI_MKT_DTL_SLNO + "'";
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

                if (ex.Message.Contains("UK_MARKET_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Market already Exist.";
                }
                else if (ex.Message.Contains("UK_TERRITORY_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Territory already Exist.";
                }
            }
            return _vmMsg;
        }

        public List<TerritoryMarMasModel> GetTerritoryList()
        {
            string Qry = "";
            Qry = "SELECT TERRITORY_CODE ,TERRITORY_NAME FROM TERRITORY ";
            Qry = Qry + "ORDER BY TERRITORY_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<TerritoryMarMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new TerritoryMarMasModel
                     {
                         TERRITORY_CODE = row["TERRITORY_CODE"].ToString(),
                         TERRITORY_NAME = row["TERRITORY_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<TerritoryMarMasModel> GetTerritorySearchList()
        {
            DataTable dt = dbHelper.GetDataTable("select ZM.TERRI_MKT_MAS_SLNO,Z.TERRITORY_CODE,Z.TERRITORY_NAME from TERRITORY Z inner join TERRI_MKT_MAS ZM on Z.TERRITORY_CODE = ZM.TERRITORY_CODE ORDER BY Z.TERRITORY_CODE");
            List<TerritoryMarMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new TerritoryMarMasModel
                     {
                         TERRI_MKT_MAS_SLNO = row["TERRI_MKT_MAS_SLNO"].ToString(),
                         TERRITORY_CODE = row["TERRITORY_CODE"].ToString(),
                         TERRITORY_NAME = row["TERRITORY_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<TerritoryMarDtlModel> GetMarInfoList()
        {
            string Qry = "";
            //Qry = "SELECT MARKET_CODE ,MARKET_NAME FROM MARKET ";
            Qry = "SELECT a.*, B.MKT_TYPE_NAME FROM MARKET A Left Join MKT_TYPE B ON A.MKT_TYPE_CODE= B.MKT_TYPE_CODE ";
            Qry = Qry + " ORDER BY MARKET_CODE ";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<TerritoryMarDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new TerritoryMarDtlModel
                     {
                         MARKET_CODE = row["MARKET_CODE"].ToString(),
                         MARKET_NAME = row["MARKET_NAME"].ToString(),
                         MarketTypeCode = row["MKT_TYPE_CODE"].ToString(),
                         MarketTypeName = row["MKT_TYPE_NAME"].ToString(),
                         SBU_GROUP = row["SBU_UNIT"].ToString()
                     }).ToList();

            return items;
        }

        public List<TerritoryMarDtlModel> GetSaveMarList(string TERRI_MKT_MAS_SLNO)
        {
            var items = new List<TerritoryMarDtlModel>();
            DataTable dt = dbHelper.GetDataTable("select ZM.TERRI_MKT_DTL_SLNO, Z.MARKET_CODE,Z.MARKET_NAME,Z.SBU_UNIT,Z.MKT_TYPE_CODE,A.MKT_TYPE_NAME from MARKET Z  left join TERRI_MKT_DTL ZM on Z.MARKET_CODE = ZM.MARKET_CODE left join MKT_TYPE A on A.MKT_TYPE_CODE= Z.MKT_TYPE_CODE WHERE TERRI_MKT_MAS_SLNO = '" + TERRI_MKT_MAS_SLNO + "' ORDER BY ZM.TERRI_MKT_DTL_SLNO");
            items = (from DataRow row in dt.Rows
                     select new TerritoryMarDtlModel
                     {
                         MARKET_CODE = row["MARKET_CODE"].ToString(),
                         MARKET_NAME = row["MARKET_NAME"].ToString(),
                         SBU_GROUP = row["SBU_UNIT"].ToString(),
                         TERRI_MKT_DTL_SLNO = row["TERRI_MKT_DTL_SLNO"].ToString(),
                         MarketTypeCode = row["MKT_TYPE_CODE"].ToString(),
                         MarketTypeName = row["MKT_TYPE_NAME"].ToString(),
                     }).ToList();

            return items;
        }

        public ValidationMsg Delete(string TERRI_MKT_MAS_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM TERRI_MKT_MAS WHERE TERRI_MKT_MAS_SLNO = " + TERRI_MKT_MAS_SLNO + "";
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

        public ValidationMsg DeletedMarInfo(string TERRI_MKT_DTL_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM TERRI_MKT_DTL WHERE TERRI_MKT_DTL_SLNO = " + TERRI_MKT_DTL_SLNO + "";
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