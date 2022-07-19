using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Data.OracleClient;

namespace MRS.DAL.DAO
{
    public class ZoneDiviDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;

        public int ZoneSl = 0;

        public ValidationMsg Save(ZoneDiviMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region ZoneDiviMas

                    string query = "select NVL(MAX(ZONE_DIVI_MAS_SLNO),0)+1 from ZONE_DIVI_MAS";
                    DataTable dt = dbHelper.GetDataTable(query);
                    if (dt.Rows.Count > 0)
                        model.ZONE_DIVI_MAS_SLNO = dt.Rows[0][0].ToString();

                    string qry = "INSERT INTO ZONE_DIVI_MAS(ZONE_DIVI_MAS_SLNO,ZONE_CODE)" +
                    "VALUES(" + model.ZONE_DIVI_MAS_SLNO + ", '" + model.ZONE_CODE + "')";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        ZoneSl = Convert.ToInt16(model.ZONE_DIVI_MAS_SLNO);
                    }

                    #endregion

                    #region ZoneDiviDtl

                    if (model.DivisonList != null)
                    {
                        foreach (ZoneDiviDtlModel objDivisonModel in model.DivisonList)
                        {
                            DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(ZONE_DIVI_DTL_SLNO),0)+1 from ZONE_DIVI_DTL");
                            if (dtDtl.Rows.Count > 0)
                                objDivisonModel.ZONE_DIVI_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                            objDivisonModel.ZONE_DIVI_MAS_SLNO = ZoneSl.ToString();
                            objDivisonModel.ZONE_DIVI_STATUS = string.IsNullOrEmpty(objDivisonModel.ZONE_DIVI_STATUS) ? "Active" : objDivisonModel.ZONE_DIVI_STATUS;
                            objDivisonModel.ZONE_DIVI_STATUS = objDivisonModel.ZONE_DIVI_STATUS == "Active" ? "A" : "I";

                            qry = "INSERT INTO ZONE_DIVI_DTL(ZONE_DIVI_DTL_SLNO,ZONE_DIVI_MAS_SLNO,DIVISION_CODE,ZONE_DIVI_STATUS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                            "VALUES('" + objDivisonModel.ZONE_DIVI_DTL_SLNO + "', '" + model.ZONE_DIVI_MAS_SLNO + "', '" + objDivisonModel.DIVISION_CODE + "', '" + objDivisonModel.ZONE_DIVI_STATUS + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
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

                if (ex.Message.Contains("UK_ZONE_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Zone already Exist.";
                }
                else if (ex.Message.Contains("UK_DIVISION_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Division already Exist.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Update(ZoneDiviMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                #region Update

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region ZoneDiviMas

                    string qry = "UPDATE ZONE_DIVI_MAS SET ZONE_CODE = '" + model.ZONE_CODE + "' WHERE ZONE_DIVI_MAS_SLNO = '" + model.ZONE_DIVI_MAS_SLNO + "'";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        _vmMsg.Type = Enums.MessageType.Update;
                        _vmMsg.Msg = "Updated Successfully.";
                    }

                    #endregion

                    #region ZoneDiviDtl

                    if (model.DivisonList != null)
                    {
                        foreach (ZoneDiviDtlModel objDivisonModel in model.DivisonList)
                        {
                            if (string.IsNullOrEmpty(objDivisonModel.ZONE_DIVI_DTL_SLNO))
                            {
                                DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(ZONE_DIVI_DTL_SLNO),0)+1 from ZONE_DIVI_DTL");
                                if (dtDtl.Rows.Count > 0)
                                    objDivisonModel.ZONE_DIVI_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                                objDivisonModel.ZONE_DIVI_MAS_SLNO = ZoneSl.ToString();
                                objDivisonModel.ZONE_DIVI_STATUS = string.IsNullOrEmpty(objDivisonModel.ZONE_DIVI_STATUS) ? "Active" : objDivisonModel.ZONE_DIVI_STATUS;
                                objDivisonModel.ZONE_DIVI_STATUS = objDivisonModel.ZONE_DIVI_STATUS == "Active" ? "A" : "I";

                                qry = "INSERT INTO ZONE_DIVI_DTL(ZONE_DIVI_DTL_SLNO,ZONE_DIVI_MAS_SLNO,DIVISION_CODE,ZONE_DIVI_STATUS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objDivisonModel.ZONE_DIVI_DTL_SLNO + "', '" + model.ZONE_DIVI_MAS_SLNO + "', '" + objDivisonModel.DIVISION_CODE + "', '" + objDivisonModel.ZONE_DIVI_STATUS + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                                if (dbHelper.CmdExecute(qry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Success;
                                }
                            }
                            else
                            {
                                objDivisonModel.ZONE_DIVI_STATUS = string.IsNullOrEmpty(objDivisonModel.ZONE_DIVI_STATUS) ? "Active" : objDivisonModel.ZONE_DIVI_STATUS;
                                objDivisonModel.ZONE_DIVI_STATUS = objDivisonModel.ZONE_DIVI_STATUS == "Active" ? "A" : "I";
                                string updateqry = "UPDATE ZONE_DIVI_DTL SET DIVISION_CODE = '" + objDivisonModel.DIVISION_CODE + "',ZONE_DIVI_STATUS = '" + objDivisonModel.ZONE_DIVI_STATUS + "'," +
                                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                             "WHERE ZONE_DIVI_DTL_SLNO = '" + objDivisonModel.ZONE_DIVI_DTL_SLNO + "'";
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

                if (ex.Message.Contains("UK_ZONE_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Zone already Exist.";
                }
                else if (ex.Message.Contains("UK_DIVISION_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Division already Exist.";
                }
            }
            return _vmMsg;
        }

        public List<ZoneDiviMasModel> GetZoneList()
        {
            string Qry = "";
            Qry = "SELECT ZONE_CODE ,ZONE_NAME FROM ZONE ";
            Qry = Qry + "ORDER BY ZONE_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ZoneDiviMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new ZoneDiviMasModel
                     {
                         ZONE_CODE = row["ZONE_CODE"].ToString(),
                         ZONE_NAME = row["ZONE_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<ZoneDiviMasModel> GetZoneSearchList()
        {
            DataTable dt = dbHelper.GetDataTable("select ZM.ZONE_DIVI_MAS_SLNO,Z.ZONE_CODE,Z.ZONE_NAME from ZONE Z inner join ZONE_DIVI_MAS ZM on Z.ZONE_CODE = ZM.ZONE_CODE ORDER BY Z.ZONE_CODE");
            List<ZoneDiviMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new ZoneDiviMasModel
                     {
                         ZONE_DIVI_MAS_SLNO = row["ZONE_DIVI_MAS_SLNO"].ToString(),
                         ZONE_CODE = row["ZONE_CODE"].ToString(),
                         ZONE_NAME = row["ZONE_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<ZoneDiviDtlModel> GetDiviInfoList()
        {
            string Qry = "";
            Qry = "SELECT DIVISION_CODE ,DIVISION_NAME FROM DIVISION ";
            Qry = Qry + "ORDER BY DIVISION_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ZoneDiviDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new ZoneDiviDtlModel
                     {
                         DIVISION_CODE = row["DIVISION_CODE"].ToString(),
                         DIVISION_NAME = row["DIVISION_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<ZoneDiviDtlModel> GetSaveDiviList(string ZONE_DIVI_MAS_SLNO)
        {
            var items = new List<ZoneDiviDtlModel>();
            DataTable dt = dbHelper.GetDataTable("select ZM.ZONE_DIVI_DTL_SLNO,ZM.ZONE_DIVI_STATUS, Z.DIVISION_CODE,Z.DIVISION_NAME from DIVISION Z inner join ZONE_DIVI_DTL ZM on Z.DIVISION_CODE = ZM.DIVISION_CODE  WHERE ZONE_DIVI_MAS_SLNO = " + ZONE_DIVI_MAS_SLNO + " ORDER BY ZM.ZONE_DIVI_DTL_SLNO");
            items = (from DataRow row in dt.Rows
                     select new ZoneDiviDtlModel
                     {
                         DIVISION_CODE = row["DIVISION_CODE"].ToString(),
                         DIVISION_NAME = row["DIVISION_NAME"].ToString(),
                         ZONE_DIVI_DTL_SLNO = row["ZONE_DIVI_DTL_SLNO"].ToString(),
                         ZONE_DIVI_STATUS = row["ZONE_DIVI_STATUS"].ToString() == "A" ? "Active" : "Inactive"
                     }).ToList();

            return items;
        }

        public ValidationMsg Delete(string ZONE_DIVI_MAS_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM ZONE_DIVI_MAS WHERE ZONE_DIVI_MAS_SLNO = " + ZONE_DIVI_MAS_SLNO + "";
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

        public ValidationMsg DeletedDiviInfo(string ZONE_DIVI_DTL_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM ZONE_DIVI_DTL WHERE ZONE_DIVI_DTL_SLNO = " + ZONE_DIVI_DTL_SLNO + "";
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
