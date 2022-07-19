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
    public class RegionTerriDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;

        public int RegionSl = 0;

        public ValidationMsg Save(RegionTerriMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region RegionTerriMas

                    string query = "select NVL(MAX(REGI_TERRI_MAS_SLNO),0)+1 from REGI_TERRI_MAS";
                    DataTable dt = dbHelper.GetDataTable(query);
                    if (dt.Rows.Count > 0)
                        model.REGI_TERRI_MAS_SLNO = dt.Rows[0][0].ToString();

                    string qry = "INSERT INTO REGI_TERRI_MAS(REGI_TERRI_MAS_SLNO,REGION_CODE)" +
                    "VALUES(" + model.REGI_TERRI_MAS_SLNO + ", '" + model.REGION_CODE + "')";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        RegionSl = Convert.ToInt16(model.REGI_TERRI_MAS_SLNO);
                    }

                    #endregion

                    #region RegionTerriDtl

                    if (model.TerritoryList != null)
                    {
                        foreach (RegionTerriDtlModel objTerrisonModel in model.TerritoryList)
                        {
                            DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(REGI_TERRI_DTL_SLNO),0)+1 from REGI_TERRI_DTL");
                            if (dtDtl.Rows.Count > 0)
                                objTerrisonModel.REGI_TERRI_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                            objTerrisonModel.REGI_TERRI_MAS_SLNO = RegionSl.ToString();
                            objTerrisonModel.REGI_TERRI_STATUS = string.IsNullOrEmpty(objTerrisonModel.REGI_TERRI_STATUS) ? "Active" : objTerrisonModel.REGI_TERRI_STATUS;
                            objTerrisonModel.REGI_TERRI_STATUS = objTerrisonModel.REGI_TERRI_STATUS == "Active" ? "A" : "I";

                            qry = "INSERT INTO REGI_TERRI_DTL(REGI_TERRI_DTL_SLNO,REGI_TERRI_MAS_SLNO,TERRITORY_CODE,REGI_TERRI_STATUS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                            "VALUES('" + objTerrisonModel.REGI_TERRI_DTL_SLNO + "', '" + model.REGI_TERRI_MAS_SLNO + "', '" + objTerrisonModel.TERRITORY_CODE + "', '" + objTerrisonModel.REGI_TERRI_STATUS + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
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

                if (ex.Message.Contains("MRS.UK_REG_TERRI_MAS_REGI_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Region already Exist.";
                }
                else if (ex.Message.Contains("MRS.UK_TERRITORY_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Territory already Exist.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Update(RegionTerriMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                #region Update

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region RegionTerriMas

                    string qry = "UPDATE REGI_TERRI_MAS SET REGION_CODE = '" + model.REGION_CODE + "' WHERE REGI_TERRI_MAS_SLNO = '" + model.REGI_TERRI_MAS_SLNO + "'";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        _vmMsg.Type = Enums.MessageType.Update;
                        _vmMsg.Msg = "Updated Successfully.";
                    }

                    #endregion

                    #region RegionTerriDtl

                    if (model.TerritoryList != null)
                    {
                        foreach (RegionTerriDtlModel objTerrisonModel in model.TerritoryList)
                        {
                            if (string.IsNullOrEmpty(objTerrisonModel.REGI_TERRI_DTL_SLNO))
                            {
                                DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(REGI_TERRI_DTL_SLNO),0)+1 from REGI_TERRI_DTL");
                                if (dtDtl.Rows.Count > 0)
                                    objTerrisonModel.REGI_TERRI_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                                objTerrisonModel.REGI_TERRI_MAS_SLNO = RegionSl.ToString();
                                objTerrisonModel.REGI_TERRI_STATUS = string.IsNullOrEmpty(objTerrisonModel.REGI_TERRI_STATUS) ? "Active" : objTerrisonModel.REGI_TERRI_STATUS;
                                objTerrisonModel.REGI_TERRI_STATUS = objTerrisonModel.REGI_TERRI_STATUS == "Active" ? "A" : "I";

                                qry = "INSERT INTO REGI_TERRI_DTL(REGI_TERRI_DTL_SLNO,REGI_TERRI_MAS_SLNO,TERRITORY_CODE,REGI_TERRI_STATUS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objTerrisonModel.REGI_TERRI_DTL_SLNO + "', '" + model.REGI_TERRI_MAS_SLNO + "', '" + objTerrisonModel.TERRITORY_CODE + "', '" + objTerrisonModel.REGI_TERRI_STATUS + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                                if (dbHelper.CmdExecute(qry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Success;
                                }
                            }
                            else
                            {
                                objTerrisonModel.REGI_TERRI_STATUS = string.IsNullOrEmpty(objTerrisonModel.REGI_TERRI_STATUS) ? "Active" : objTerrisonModel.REGI_TERRI_STATUS;
                                objTerrisonModel.REGI_TERRI_STATUS = objTerrisonModel.REGI_TERRI_STATUS == "Active" ? "A" : "I";
                                string updateqry = "UPDATE REGI_TERRI_DTL SET TERRITORY_CODE = '" + objTerrisonModel.TERRITORY_CODE + "',REGI_TERRI_STATUS = '" + objTerrisonModel.REGI_TERRI_STATUS + "'," +
                                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                             "WHERE REGI_TERRI_DTL_SLNO = '" + objTerrisonModel.REGI_TERRI_DTL_SLNO + "'";
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

                if (ex.Message.Contains("MRS.UK_REG_TERRI_MAS_REGI_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Region already Exist.";
                }
                else if (ex.Message.Contains("MRS.UK_TERRITORY_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Territory already Exist.";
                }
            }
            return _vmMsg;
        }

        public List<RegionTerriMasModel> GetRegionList()
        {
            string Qry = "";
            Qry = "SELECT REGION_CODE ,REGION_NAME FROM REGION ";
            Qry = Qry + "ORDER BY REGION_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<RegionTerriMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new RegionTerriMasModel
                     {
                         REGION_CODE = row["REGION_CODE"].ToString(),
                         REGION_NAME = row["REGION_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<RegionTerriMasModel> GetRegionSearchList()
        {
            DataTable dt = dbHelper.GetDataTable("select ZM.REGI_TERRI_MAS_SLNO,Z.REGION_CODE,Z.REGION_NAME from REGION Z inner join REGI_TERRI_MAS ZM on Z.REGION_CODE = ZM.REGION_CODE ORDER BY Z.REGION_CODE");
            List<RegionTerriMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new RegionTerriMasModel
                     {
                         REGI_TERRI_MAS_SLNO = row["REGI_TERRI_MAS_SLNO"].ToString(),
                         REGION_CODE = row["REGION_CODE"].ToString(),
                         REGION_NAME = row["REGION_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<RegionTerriDtlModel> GetTerriInfoList()
        {
            string Qry = "";
            Qry = "SELECT TERRITORY_CODE ,TERRITORY_NAME FROM TERRITORY ";
            Qry = Qry + "ORDER BY TERRITORY_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<RegionTerriDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new RegionTerriDtlModel
                     {
                         TERRITORY_CODE = row["TERRITORY_CODE"].ToString(),
                         TERRITORY_NAME = row["TERRITORY_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<RegionTerriDtlModel> GetSaveTerriList(string REGI_TERRI_MAS_SLNO)
        {
            var items = new List<RegionTerriDtlModel>();
            DataTable dt = dbHelper.GetDataTable("select ZM.REGI_TERRI_DTL_SLNO,ZM.REGI_TERRI_STATUS, Z.TERRITORY_CODE,Z.TERRITORY_NAME from TERRITORY Z inner join REGI_TERRI_DTL ZM on Z.TERRITORY_CODE = ZM.TERRITORY_CODE  WHERE REGI_TERRI_MAS_SLNO = " + REGI_TERRI_MAS_SLNO + " ORDER BY ZM.REGI_TERRI_DTL_SLNO");
            items = (from DataRow row in dt.Rows
                     select new RegionTerriDtlModel
                     {
                         TERRITORY_CODE = row["TERRITORY_CODE"].ToString(),
                         TERRITORY_NAME = row["TERRITORY_NAME"].ToString(),
                         REGI_TERRI_DTL_SLNO = row["REGI_TERRI_DTL_SLNO"].ToString(),
                         REGI_TERRI_STATUS = row["REGI_TERRI_STATUS"].ToString() == "A" ? "Active" : "Inactive"
                     }).ToList();

            return items;
        }

        public ValidationMsg Delete(string REGI_TERRI_MAS_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM REGI_TERRI_MAS WHERE REGI_TERRI_MAS_SLNO = " + REGI_TERRI_MAS_SLNO + "";
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

        public ValidationMsg DeletedTerriInfo(string REGI_TERRI_DTL_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM REGI_TERRI_DTL WHERE REGI_TERRI_DTL_SLNO = " + REGI_TERRI_DTL_SLNO + "";
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