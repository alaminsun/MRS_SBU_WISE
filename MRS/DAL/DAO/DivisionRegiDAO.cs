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
    public class DivisionRegiDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;

        public int DivisionSl = 0;

        public ValidationMsg Save(DivisionRegiMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region DivisionRegiMas

                    string query = "select NVL(MAX(DIVI_REGI_MAS_SLNO),0)+1 from DIVI_REGI_MAS";
                    DataTable dt = dbHelper.GetDataTable(query);
                    if (dt.Rows.Count > 0)
                        model.DIVI_REGI_MAS_SLNO = dt.Rows[0][0].ToString();

                    string qry = "INSERT INTO DIVI_REGI_MAS(DIVI_REGI_MAS_SLNO,DIVISION_CODE)" +
                    "VALUES(" + model.DIVI_REGI_MAS_SLNO + ", '" + model.DIVISION_CODE + "')";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        DivisionSl = Convert.ToInt16(model.DIVI_REGI_MAS_SLNO);
                    }

                    #endregion

                    #region DivisionRegiDtl

                    if (model.RegionList != null)
                    {
                        foreach (DivisionRegiDtlModel objDivisonModel in model.RegionList)
                        {
                            DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(DIVI_REGI_DTL_SLNO),0)+1 from DIVI_REGI_DTL");
                            if (dtDtl.Rows.Count > 0)
                                objDivisonModel.DIVI_REGI_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                            objDivisonModel.DIVI_REGI_MAS_SLNO = DivisionSl.ToString();
                            objDivisonModel.DIVI_REGI_STATUS = string.IsNullOrEmpty(objDivisonModel.DIVI_REGI_STATUS) ? "Active" : objDivisonModel.DIVI_REGI_STATUS;
                            objDivisonModel.DIVI_REGI_STATUS = objDivisonModel.DIVI_REGI_STATUS == "Active" ? "A" : "I";

                            qry = "INSERT INTO DIVI_REGI_DTL(DIVI_REGI_DTL_SLNO,DIVI_REGI_MAS_SLNO,REGION_CODE,DIVI_REGI_STATUS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                            "VALUES('" + objDivisonModel.DIVI_REGI_DTL_SLNO + "', '" + model.DIVI_REGI_MAS_SLNO + "', '" + objDivisonModel.REGION_CODE + "', '" + objDivisonModel.DIVI_REGI_STATUS + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
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

                if (ex.Message.Contains("MRS.UK_DIVI_REGI_MAS_DIVI_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Division already Exist.";
                }
                else if (ex.Message.Contains("MRS.UK_REGION_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Region already Exist.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Update(DivisionRegiMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                #region Update

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region DivisionRegiMas

                    string qry = "UPDATE DIVI_REGI_MAS SET DIVISION_CODE = '" + model.DIVISION_CODE + "' WHERE DIVI_REGI_MAS_SLNO = '" + model.DIVI_REGI_MAS_SLNO + "'";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        _vmMsg.Type = Enums.MessageType.Update;
                        _vmMsg.Msg = "Updated Successfully.";
                    }

                    #endregion

                    #region DivisionRegiDtl

                    if (model.RegionList != null)
                    {
                        foreach (DivisionRegiDtlModel objDivisonModel in model.RegionList)
                        {
                            if (string.IsNullOrEmpty(objDivisonModel.DIVI_REGI_DTL_SLNO))
                            {
                                DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(DIVI_REGI_DTL_SLNO),0)+1 from DIVI_REGI_DTL");
                                if (dtDtl.Rows.Count > 0)
                                    objDivisonModel.DIVI_REGI_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                                objDivisonModel.DIVI_REGI_MAS_SLNO = DivisionSl.ToString();
                                objDivisonModel.DIVI_REGI_STATUS = string.IsNullOrEmpty(objDivisonModel.DIVI_REGI_STATUS) ? "Active" : objDivisonModel.DIVI_REGI_STATUS;
                                objDivisonModel.DIVI_REGI_STATUS = objDivisonModel.DIVI_REGI_STATUS == "Active" ? "A" : "I";

                                qry = "INSERT INTO DIVI_REGI_DTL(DIVI_REGI_DTL_SLNO,DIVI_REGI_MAS_SLNO,REGION_CODE,DIVI_REGI_STATUS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objDivisonModel.DIVI_REGI_DTL_SLNO + "', '" + model.DIVI_REGI_MAS_SLNO + "', '" + objDivisonModel.REGION_CODE + "', '" + objDivisonModel.DIVI_REGI_STATUS + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                                if (dbHelper.CmdExecute(qry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Success;
                                }
                            }
                            else
                            {
                                objDivisonModel.DIVI_REGI_STATUS = string.IsNullOrEmpty(objDivisonModel.DIVI_REGI_STATUS) ? "Active" : objDivisonModel.DIVI_REGI_STATUS;
                                objDivisonModel.DIVI_REGI_STATUS = objDivisonModel.DIVI_REGI_STATUS == "Active" ? "A" : "I";
                                string updateqry = "UPDATE DIVI_REGI_DTL SET REGION_CODE = '" + objDivisonModel.REGION_CODE + "',DIVI_REGI_STATUS = '" + objDivisonModel.DIVI_REGI_STATUS + "'," +
                                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                             "WHERE DIVI_REGI_DTL_SLNO = '" + objDivisonModel.DIVI_REGI_DTL_SLNO + "'";
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

                if (ex.Message.Contains("MRS.UK_DIVI_REGI_MAS_DIVI_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Division already Exist.";
                }
                else if (ex.Message.Contains("MRS.UK_REGION_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Region already Exist.";
                }
            }
            return _vmMsg;
        }

        public List<DivisionRegiMasModel> GetDivisionList()
        {
            string Qry = "";
            Qry = "SELECT DIVISION_CODE ,DIVISION_NAME FROM DIVISION ";
            Qry = Qry + "ORDER BY DIVISION_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DivisionRegiMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new DivisionRegiMasModel
                     {
                         DIVISION_CODE = row["DIVISION_CODE"].ToString(),
                         DIVISION_NAME = row["DIVISION_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<DivisionRegiMasModel> GetDivisionSearchList()
        {
            DataTable dt = dbHelper.GetDataTable("select ZM.DIVI_REGI_MAS_SLNO,Z.DIVISION_CODE,Z.DIVISION_NAME from DIVISION Z inner join DIVI_REGI_MAS ZM on Z.DIVISION_CODE = ZM.DIVISION_CODE ORDER BY Z.DIVISION_CODE");
            List<DivisionRegiMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new DivisionRegiMasModel
                     {
                         DIVI_REGI_MAS_SLNO = row["DIVI_REGI_MAS_SLNO"].ToString(),
                         DIVISION_CODE = row["DIVISION_CODE"].ToString(),
                         DIVISION_NAME = row["DIVISION_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<DivisionRegiDtlModel> GetRegiInfoList()
        {
            string Qry = "";
            Qry = "SELECT REGION_CODE ,REGION_NAME FROM REGION ";
            Qry = Qry + "ORDER BY REGION_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DivisionRegiDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new DivisionRegiDtlModel
                     {
                         REGION_CODE = row["REGION_CODE"].ToString(),
                         REGION_NAME = row["REGION_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<DivisionRegiDtlModel> GetSaveRegiList(string DIVI_REGI_MAS_SLNO)
        {
            var items = new List<DivisionRegiDtlModel>();
            DataTable dt = dbHelper.GetDataTable("select ZM.DIVI_REGI_DTL_SLNO,ZM.DIVI_REGI_STATUS, Z.REGION_CODE,Z.REGION_NAME from REGION Z inner join DIVI_REGI_DTL ZM on Z.REGION_CODE = ZM.REGION_CODE  WHERE DIVI_REGI_MAS_SLNO = " + DIVI_REGI_MAS_SLNO + " ORDER BY ZM.DIVI_REGI_DTL_SLNO");
            items = (from DataRow row in dt.Rows
                     select new DivisionRegiDtlModel
                     {
                         REGION_CODE = row["REGION_CODE"].ToString(),
                         REGION_NAME = row["REGION_NAME"].ToString(),
                         DIVI_REGI_DTL_SLNO = row["DIVI_REGI_DTL_SLNO"].ToString(),
                         DIVI_REGI_STATUS = row["DIVI_REGI_STATUS"].ToString() == "A" ? "Active" : "Inactive"
                     }).ToList();

            return items;
        }

        public ValidationMsg Delete(string DIVI_REGI_MAS_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM DIVI_REGI_MAS WHERE DIVI_REGI_MAS_SLNO = " + DIVI_REGI_MAS_SLNO + "";
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

        public ValidationMsg DeletedRegiInfo(string DIVI_REGI_DTL_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM DIVI_REGI_DTL WHERE DIVI_REGI_DTL_SLNO = " + DIVI_REGI_DTL_SLNO + "";
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
