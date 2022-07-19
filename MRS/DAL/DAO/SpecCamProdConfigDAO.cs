using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;

namespace MRS.DAL.DAO
{
    public class SpecCamProdConfigDAO
    {
        private DBHelper _dbHelper = new DBHelper();
        DBConnection dbConnection = new DBConnection();
        private ValidationMsg _validationMsg = new ValidationMsg();
        DataTable _dt = new DataTable();
        public ValidationMsg Save(SpecCamProdConfigModel model, string userid)
        {
            try
            {
                // using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                //{
                #region  Check SCP_CODE for new entry

                _dt = _dbHelper.GetDataTable("SELECT 1 FROM SPECIAL_CAMPAIGN WHERE SCP_CODE='" + model.SCP_CODE + "'");
                if (_dt.Rows.Count > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Already Entry This Special Campaign.";
                    return _validationMsg;
                }
                #endregion
                #region Insert Special Campaign  Data

                string insertQuery = "INSERT INTO SPECIAL_CAMPAIGN (SC_ID,SCP_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES(SPECIAL_CAMPAIGN_SEQ.NEXTVAL,'" + model.SCP_CODE +
                    "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";

                _dbHelper.CmdExecute(insertQuery);

                #endregion

                _dt = _dbHelper.GetDataTable("SELECT MAX(SC_ID) FROM SPECIAL_CAMPAIGN");
                if (_dt.Rows.Count > 0)
                    model.SC_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                #region Insert Generic List Info Data

                if (model.GenericInfoList != null)
                {
                    string qryDocInsert = string.Empty;
                    foreach (SCPGenericModel objGenericInfo in model.GenericInfoList)
                    {
                        if (objGenericInfo.SCG_ID == 0)
                        {  //insert
                            qryDocInsert = "INSERT INTO SCP_GENERIC(SCG_ID,SC_ID,GENERIC_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                           "VALUES(SCP_GENERIC_SEQ.NEXTVAL,'" + model.SC_ID + "', '" + objGenericInfo.GENERIC_CODE +
                            "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                            _dbHelper.CmdExecute(qryDocInsert);


                            #region Insert Dosage Form Info

                            _dt = _dbHelper.GetDataTable("SELECT MAX(SCG_ID) FROM SCP_GENERIC");
                            if (_dt.Rows.Count > 0)
                                objGenericInfo.SCG_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                            if (model.DosFormInfoList != null)
                            {

                                foreach (SCPGDosageFormModel objDosInfo in model.DosFormInfoList)
                                {
                                    //insert
                                    qryDocInsert = "INSERT INTO SCP_GENERIC_DF(DF_ID,SCG_ID,DOSAGE_FORM_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                   "VALUES(SCP_GENERIC_DF_SEQ.NEXTVAL,'" + objGenericInfo.SCG_ID + "', '" + objDosInfo.DOSAGE_FORM_CODE +
                                    "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                    _dbHelper.CmdExecute(qryDocInsert);


                                    #region Insert Dosage Strength Form Info

                                    _dt = _dbHelper.GetDataTable("SELECT MAX(DF_ID) FROM SCP_GENERIC_DF");
                                    if (_dt.Rows.Count > 0)
                                        objDosInfo.DF_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                                    if (model.DfStrengthInfoList != null)
                                    {

                                        foreach (SCPGDFStrengthModel objStrengthInfo in model.DfStrengthInfoList)
                                        {
                                            //insert
                                            qryDocInsert = "INSERT INTO SCP_GEN_DF_STRENGTH(DFS_ID,DF_ID,DSG_STRENGTH_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                           "VALUES(SCP_GEN_DF_STRENGTH_SEQ.NEXTVAL,'" + objDosInfo.DF_ID + "', '" + objStrengthInfo.DSG_STRENGTH_CODE +
                                            "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                            _dbHelper.CmdExecute(qryDocInsert);
                                        }
                                    }

                                    #endregion
                                }
                            }

                            #endregion
                        }

                    }
                }

                #endregion


                _validationMsg.ReturnId = model.SC_ID;
                _validationMsg.ReturnCode = model.SCP_CODE;
                // scope.Complete();

                _validationMsg.Type = Enums.MessageType.Success;
                _validationMsg.Msg = "Data Saved Successfully";
                //}
            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed To Save Data";
            }
            return _validationMsg;
        }
        public ValidationMsg Update(SpecCamProdConfigModel model, string userid)
        {
            try
            {
                string qryDocInsert = string.Empty;
                string updateQuery = string.Empty;
                if (model.SC_ID > 0)
                {
                    updateQuery = "UPDATE SPECIAL_CAMPAIGN SET SCP_CODE = '" + model.SCP_CODE +
                       "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "'" +
                       "  WHERE SC_ID = '" + model.SC_ID + "'";
                    _dbHelper.CmdExecute(updateQuery);

                    #region Generic Info

                    if (model.GenericInfoList != null)
                    {

                        foreach (SCPGenericModel objGenericInfo in model.GenericInfoList)
                        {
                            if (objGenericInfo.SCG_ID == 0) //insert
                            {
                                #region  Check Generic_CODE for new entry

                                _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_GENERIC SGR  LEFT OUTER JOIN SPECIAL_CAMPAIGN SC ON SC.SC_ID=SGR.SC_ID WHERE SGR.GENERIC_CODE='" + objGenericInfo.GENERIC_CODE + "' AND SGR.SC_ID='" + model.SC_ID + "'");
                                if (_dt.Rows.Count > 0)
                                {
                                    _validationMsg.Type = Enums.MessageType.Error;
                                    _validationMsg.Msg = "Already Entry This Generic Info.";
                                    return _validationMsg;
                                }
                                #endregion
                                qryDocInsert = "INSERT INTO SCP_GENERIC(SCG_ID,SC_ID,GENERIC_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                               "VALUES(SCP_GENERIC_SEQ.NEXTVAL,'" + model.SC_ID + "', '" + objGenericInfo.GENERIC_CODE +
                                "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                _dbHelper.CmdExecute(qryDocInsert);


                                #region Insert Dosage Form Info

                                _dt = _dbHelper.GetDataTable("SELECT MAX(SCG_ID) FROM SCP_GENERIC");
                                if (_dt.Rows.Count > 0)
                                    objGenericInfo.SCG_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                                if (model.DosFormInfoList != null)
                                {

                                    foreach (SCPGDosageFormModel objDosInfo in model.DosFormInfoList)
                                    {
                                        if (objDosInfo.DF_ID == 0) //insert
                                        {
                                            #region  Check Dosage_CODE for new entry

                                            _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_GENERIC_DF GDF LEFT OUTER JOIN SCP_GENERIC SGR  ON SGR.SCG_ID=GDF.SCG_ID WHERE GDF.DOSAGE_FORM_CODE='" + objDosInfo.DOSAGE_FORM_CODE + "' AND GDF.SCG_ID='" + objGenericInfo.SCG_ID + "'");
                                            if (_dt.Rows.Count > 0)
                                            {
                                                _validationMsg.Type = Enums.MessageType.Error;
                                                _validationMsg.Msg = "Already Entry This Dosage Info.";
                                                return _validationMsg;
                                            }
                                            #endregion
                                            qryDocInsert = "INSERT INTO SCP_GENERIC_DF(DF_ID,SCG_ID,DOSAGE_FORM_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                           "VALUES(SCP_GENERIC_DF_SEQ.NEXTVAL,'" + objGenericInfo.SCG_ID + "', '" + objDosInfo.DOSAGE_FORM_CODE +
                                            "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                            _dbHelper.CmdExecute(qryDocInsert);


                                            #region Insert Dosage Strength Form Info

                                            _dt = _dbHelper.GetDataTable("SELECT MAX(DF_ID) FROM SCP_GENERIC_DF");
                                            if (_dt.Rows.Count > 0)
                                                objDosInfo.DF_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                                            if (model.DfStrengthInfoList != null)
                                            {

                                                foreach (SCPGDFStrengthModel objStrengthInfo in model.DfStrengthInfoList)
                                                {
                                                    if (objStrengthInfo.DFS_ID == 0) //insert
                                                    {
                                                        #region  Check Strength_CODE for new entry

                                                        _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_GEN_DF_STRENGTH SDS LEFT OUTER JOIN SCP_GENERIC_DF SDF ON SDF.DF_ID=SDS.DF_ID WHERE SDS.DSG_STRENGTH_CODE='" + objStrengthInfo.DSG_STRENGTH_CODE + "' AND SDS.DF_ID='" + objDosInfo.DF_ID + "'");
                                                        if (_dt.Rows.Count > 0)
                                                        {
                                                            _validationMsg.Type = Enums.MessageType.Error;
                                                            _validationMsg.Msg = "Already Entry This Dosage Strength Info.";
                                                            return _validationMsg;
                                                        }
                                                        #endregion
                                                        
                                                        qryDocInsert = "INSERT INTO SCP_GEN_DF_STRENGTH(DFS_ID,DF_ID,DSG_STRENGTH_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                                       "VALUES(SCP_GEN_DF_STRENGTH_SEQ.NEXTVAL,'" + objDosInfo.DF_ID + "', '" + objStrengthInfo.DSG_STRENGTH_CODE +
                                                        "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                                        _dbHelper.CmdExecute(qryDocInsert);
                                                    }
                                                    else //update Strength
                                                    {
                                                        updateQuery = "UPDATE SCP_GEN_DF_STRENGTH SET DSG_STRENGTH_CODE = '" + objStrengthInfo.DSG_STRENGTH_CODE +
                     "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "'" +
                     "  WHERE DFS_ID = '" + objStrengthInfo.DFS_ID + "'";
                                                        _dbHelper.CmdExecute(updateQuery);

                                                    }
                                                }
                                            }
                                        }
                                        else //update Dosage
                                        {
                                            updateQuery = "UPDATE SCP_GENERIC_DF SET DOSAGE_FORM_CODE = '" + objDosInfo.DOSAGE_FORM_CODE +
                   "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "'" +
                   "  WHERE DF_ID = '" + objDosInfo.DF_ID + "'";
                                            _dbHelper.CmdExecute(updateQuery);

                                            //_dt = _dbHelper.GetDataTable("SELECT MAX(DF_ID) FROM SCP_GENERIC_DF");
                                            //if (_dt.Rows.Count > 0)
                                            //    objDosInfo.DF_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                                            if (model.DfStrengthInfoList != null)
                                            {

                                                foreach (SCPGDFStrengthModel objStrengthInfo in model.DfStrengthInfoList)
                                                {
                                                    if (objStrengthInfo.DFS_ID == 0) //insert
                                                    {
                                                        #region  Check Strength_CODE for new entry

                                                        _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_GEN_DF_STRENGTH SDS LEFT OUTER JOIN SCP_GENERIC_DF SDF ON SDF.DF_ID=SDS.DF_ID WHERE SDS.DSG_STRENGTH_CODE='" + objStrengthInfo.DSG_STRENGTH_CODE + "' AND SDS.DF_ID='" + objDosInfo.DF_ID + "'");
                                                        if (_dt.Rows.Count > 0)
                                                        {
                                                            _validationMsg.Type = Enums.MessageType.Error;
                                                            _validationMsg.Msg = "Already Entry This Dosage Strength Info.";
                                                            return _validationMsg;
                                                        }
                                                        #endregion
                                                        qryDocInsert = "INSERT INTO SCP_GEN_DF_STRENGTH(DFS_ID,DF_ID,DSG_STRENGTH_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                                       "VALUES(SCP_GEN_DF_STRENGTH_SEQ.NEXTVAL,'" + objDosInfo.DF_ID + "', '" + objStrengthInfo.DSG_STRENGTH_CODE +
                                                        "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                                        _dbHelper.CmdExecute(qryDocInsert);
                                                    }
                                                    else //update Strength
                                                    {
                                                        updateQuery = "UPDATE SCP_GEN_DF_STRENGTH SET DSG_STRENGTH_CODE = '" + objStrengthInfo.DSG_STRENGTH_CODE +
                     "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "'" +
                     "  WHERE DFS_ID = '" + objStrengthInfo.DFS_ID + "'";
                                                        _dbHelper.CmdExecute(updateQuery);

                                                    }
                                                }
                                            }
                                        }
                                            #endregion
                                    }
                                }

                                #endregion
                            }
                            #region Update & Insert Generic Master Detail  data list

                            else //Update Generic
                            {
                                updateQuery = "UPDATE SCP_GENERIC SET GENERIC_CODE = '" + objGenericInfo.GENERIC_CODE +
                      "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "'" +
                      "  WHERE SCG_ID = '" + objGenericInfo.SCG_ID + "'";
                                _dbHelper.CmdExecute(updateQuery);
                                #region Insert Dosage Form Info

                                //_dt = _dbHelper.GetDataTable("SELECT MAX(SCG_ID) FROM SCP_GENERIC");
                                //if (_dt.Rows.Count > 0)
                                //    objGenericInfo.SCG_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                                if (model.DosFormInfoList != null)
                                {

                                    foreach (SCPGDosageFormModel objDosInfo in model.DosFormInfoList)
                                    {
                                        if (objDosInfo.DF_ID == 0) //insert
                                        {
                                            #region  Check Dosage_CODE for new entry

                                            _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_GENERIC_DF GDF LEFT OUTER JOIN SCP_GENERIC SGR  ON SGR.SCG_ID=GDF.SCG_ID WHERE GDF.DOSAGE_FORM_CODE='" + objDosInfo.DOSAGE_FORM_CODE + "' AND GDF.SCG_ID='" + objGenericInfo.SCG_ID + "'");
                                            if (_dt.Rows.Count > 0)
                                            {
                                                _validationMsg.Type = Enums.MessageType.Error;
                                                _validationMsg.Msg = "Already Entry This Dosage Info.";
                                                return _validationMsg;
                                            }
                                            #endregion
                                            qryDocInsert = "INSERT INTO SCP_GENERIC_DF(DF_ID,SCG_ID,DOSAGE_FORM_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                           "VALUES(SCP_GENERIC_DF_SEQ.NEXTVAL,'" + objGenericInfo.SCG_ID + "', '" + objDosInfo.DOSAGE_FORM_CODE +
                                            "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                            _dbHelper.CmdExecute(qryDocInsert);


                                            #region Insert & Update Dosage Strength Form Info

                                            _dt = _dbHelper.GetDataTable("SELECT MAX(DF_ID) FROM SCP_GENERIC_DF");
                                            if (_dt.Rows.Count > 0)
                                                objDosInfo.DF_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                                            if (model.DfStrengthInfoList != null)
                                            {

                                                foreach (SCPGDFStrengthModel objStrengthInfo in model.DfStrengthInfoList)
                                                {
                                                    if (objStrengthInfo.DFS_ID == 0) //insert
                                                    {
                                                        #region  Check Strength_CODE for new entry

                                                        _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_GEN_DF_STRENGTH SDS LEFT OUTER JOIN SCP_GENERIC_DF SDF ON SDF.DF_ID=SDS.DF_ID WHERE SDS.DSG_STRENGTH_CODE='" + objStrengthInfo.DSG_STRENGTH_CODE + "' AND SDS.DF_ID='" + objDosInfo.DF_ID + "'");
                                                        if (_dt.Rows.Count > 0)
                                                        {
                                                            _validationMsg.Type = Enums.MessageType.Error;
                                                            _validationMsg.Msg = "Already Entry This Dosage Strength Info.";
                                                            return _validationMsg;
                                                        }
                                                        #endregion

                                                        qryDocInsert = "INSERT INTO SCP_GEN_DF_STRENGTH(DFS_ID,DF_ID,DSG_STRENGTH_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                                       "VALUES(SCP_GEN_DF_STRENGTH_SEQ.NEXTVAL,'" + objDosInfo.DF_ID + "', '" + objStrengthInfo.DSG_STRENGTH_CODE +
                                                        "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                                        _dbHelper.CmdExecute(qryDocInsert);
                                                    }
                                                    else //update Strength
                                                    {
                                                        updateQuery = "UPDATE SCP_GEN_DF_STRENGTH SET DSG_STRENGTH_CODE = '" + objStrengthInfo.DSG_STRENGTH_CODE +
                     "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "'" +
                     "  WHERE DFS_ID = '" + objStrengthInfo.DFS_ID + "'";
                                                        _dbHelper.CmdExecute(updateQuery);

                                                    }
                                                }
                                            }
                                        }
                                        else //update Dosage
                                        {
                                            updateQuery = "UPDATE SCP_GENERIC_DF SET DOSAGE_FORM_CODE = '" + objDosInfo.DOSAGE_FORM_CODE +
                   "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "'" +
                   "  WHERE DF_ID = '" + objDosInfo.DF_ID + "'";
                                            _dbHelper.CmdExecute(updateQuery);

                                            if (model.DfStrengthInfoList != null)
                                            {

                                                foreach (SCPGDFStrengthModel objStrengthInfo in model.DfStrengthInfoList)
                                                {
                                                    if (objStrengthInfo.DFS_ID == 0) //insert
                                                    {
                                                        #region  Check Strength_CODE for new entry

                                                        _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_GEN_DF_STRENGTH SDS LEFT OUTER JOIN SCP_GENERIC_DF SDF ON SDF.DF_ID=SDS.DF_ID WHERE SDS.DSG_STRENGTH_CODE='" + objStrengthInfo.DSG_STRENGTH_CODE + "' AND SDS.DF_ID='" + objDosInfo.DF_ID + "'");
                                                        if (_dt.Rows.Count > 0)
                                                        {
                                                            _validationMsg.Type = Enums.MessageType.Error;
                                                            _validationMsg.Msg = "Already Entry This Dosage Strength Info.";
                                                            return _validationMsg;
                                                        }
                                                        #endregion

                                                        qryDocInsert = "INSERT INTO SCP_GEN_DF_STRENGTH(DFS_ID,DF_ID,DSG_STRENGTH_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                                       "VALUES(SCP_GEN_DF_STRENGTH_SEQ.NEXTVAL,'" + objDosInfo.DF_ID + "', '" + objStrengthInfo.DSG_STRENGTH_CODE +
                                                        "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                                        _dbHelper.CmdExecute(qryDocInsert);
                                                    }
                                                    else //update Strength
                                                    {
                                                        updateQuery = "UPDATE SCP_GEN_DF_STRENGTH SET DSG_STRENGTH_CODE = '" + objStrengthInfo.DSG_STRENGTH_CODE +
                     "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "'" +
                     "  WHERE DFS_ID = '" + objStrengthInfo.DFS_ID + "'";
                                                        _dbHelper.CmdExecute(updateQuery);

                                                    }
                                                }
                                            }
                                        }
                                            #endregion
                                    }
                                }

                                #endregion
                            }
                            #endregion
                        }
                    }

                    #endregion


                }

                _validationMsg.Type = Enums.MessageType.Update;
                _validationMsg.Msg = "Updated Successfully.";

            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed To Update Data";

            }
            return _validationMsg;
        }



        public List<SpecCamProdConfigModel> GetAll(int p)
        {
            string Qry = "SELECT SC.SC_ID,SC.SCP_CODE,SI.SCP_NAME FROM SPECIAL_CAMPAIGN SC  LEFT OUTER JOIN SCP_INFO SI ON SI.SCP_CODE=SC.SCP_CODE ORDER BY SC.SC_ID DESC";

            _dt = _dbHelper.GetDataTable(Qry);
            List<SpecCamProdConfigModel> itemList = new List<SpecCamProdConfigModel>();
            itemList = (from DataRow row in _dt.Rows
                        select new SpecCamProdConfigModel
                        {
                            SC_ID = Convert.ToInt32(row["SC_ID"].ToString()),
                            SCP_CODE = row["SCP_CODE"].ToString(),
                            SCP_NAME = row["SCP_NAME"].ToString(),
                        }).ToList();
            return itemList;
        }

        public IEnumerable<SCPGenericModel> GetGenericItemList(int p)
        {
            var query = new StringBuilder();
            query.Append("  SELECT SGR.SCG_ID,SGR.SC_ID,GR.GENERIC_CODE,GR.GENERIC_NAME FROM SCP_GENERIC SGR ");
            query.Append("  LEFT OUTER JOIN SPECIAL_CAMPAIGN SC ON SC.SC_ID=SGR.SC_ID ");
            query.Append("  LEFT OUTER JOIN GENERIC GR ON GR.GENERIC_CODE=SGR.GENERIC_CODE ");
            query.Append("  WHERE 1=1");

            if (p != 0)
            {
                query.Append(" AND SGR.SC_ID ='" + p + "'");
            }
            query.Append(" ORDER BY SGR.SCG_ID DESC");

            //_dt = _dbHelper.GetDataTable(Qry);
            List<SCPGenericModel> listData = new List<SCPGenericModel>();

            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query.ToString(), con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SCPGenericModel model = new SCPGenericModel();
                        model.SCG_ID = Convert.ToInt32(reader["SCG_ID"]);
                        model.SC_ID = Convert.ToInt32(reader["SC_ID"]);
                        model.GENERIC_CODE = reader["GENERIC_CODE"].ToString();
                        model.GENERIC_NAME = reader["GENERIC_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public IEnumerable<SCPGDosageFormModel> GetDosageItemList(int p)
        {
            var query = new StringBuilder();
            query.Append("  SELECT GDF.DF_ID,GDF.SCG_ID,GDF.DOSAGE_FORM_CODE,DF.DOSAGE_FORM_NAME FROM SCP_GENERIC_DF GDF  ");
            query.Append("  LEFT OUTER JOIN SCP_GENERIC SGR  ON SGR.SCG_ID=GDF.SCG_ID ");
            query.Append("  LEFT OUTER JOIN DOSAGE_FORM DF ON DF.DOSAGE_FORM_CODE=GDF.DOSAGE_FORM_CODE ");
            query.Append("  WHERE 1=1");

            if (p != 0)
            {
                query.Append(" AND GDF.SCG_ID ='" + p + "'");
            }
            query.Append(" ORDER BY GDF.DF_ID DESC");

            //_dt = _dbHelper.GetDataTable(Qry);
            List<SCPGDosageFormModel> listData = new List<SCPGDosageFormModel>();

            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query.ToString(), con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SCPGDosageFormModel model = new SCPGDosageFormModel();
                        model.DF_ID = Convert.ToInt32(reader["DF_ID"]);
                        model.SCG_ID = Convert.ToInt32(reader["SCG_ID"]);
                        model.DOSAGE_FORM_CODE = reader["DOSAGE_FORM_CODE"].ToString();
                        model.DOSAGE_FORM_NAME = reader["DOSAGE_FORM_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public IEnumerable<SCPGDFStrengthModel> GetStrengthItemList(int p)
        {
            var query = new StringBuilder();
            query.Append("  SELECT SDS.DFS_ID,SDS.DF_ID,SDS.DSG_STRENGTH_CODE,DS.DSG_STRENGTH_NAME FROM SCP_GEN_DF_STRENGTH  SDS");
            query.Append("  LEFT OUTER JOIN SCP_GENERIC_DF SDF ON SDF.DF_ID=SDS.DF_ID  ");
            query.Append("  LEFT OUTER JOIN DOSAGE_STRENGTH DS ON DS.DSG_STRENGTH_CODE=SDS.DSG_STRENGTH_CODE ");
            query.Append("  WHERE 1=1");

            if (p != 0)
            {
                query.Append(" AND SDS.DF_ID ='" + p + "'");
            }
            query.Append(" ORDER BY SDS.DFS_ID DESC");

            //_dt = _dbHelper.GetDataTable(Qry);
            List<SCPGDFStrengthModel> listData = new List<SCPGDFStrengthModel>();

            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query.ToString(), con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SCPGDFStrengthModel model = new SCPGDFStrengthModel();
                        model.DFS_ID = Convert.ToInt32(reader["DFS_ID"]);
                        model.DF_ID = Convert.ToInt32(reader["DF_ID"]);
                        model.DSG_STRENGTH_CODE = reader["DSG_STRENGTH_CODE"].ToString();
                        model.DSG_STRENGTH_NAME = reader["DSG_STRENGTH_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public ValidationMsg Delete(int SC_ID)
        {
            try
            {
                string deleteQuery = "DELETE FROM SPECIAL_CAMPAIGN WHERE SC_ID = '" + SC_ID + "' ";
                if (_dbHelper.CmdExecute(deleteQuery) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-02292"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Child Record Found.";
                    return _validationMsg;
                }
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to Delete Data.";
            }
            return _validationMsg;
        }

        public ValidationMsg DeleteGenericGrid(int SCG_ID)
        {
            try
            {
                string deleteQuery = "DELETE FROM SCP_GENERIC WHERE SCG_ID = '" + SCG_ID + "' ";
                if (_dbHelper.CmdExecute(deleteQuery) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-02292"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Child Record Found.";
                    return _validationMsg;
                }
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to Delete Data.";
            }
            return _validationMsg;
        }
        public ValidationMsg DeleteDosageGrid(int DF_ID)
        {
            try
            {
                string deleteQuery = "DELETE FROM SCP_GENERIC_DF WHERE DF_ID = '" + DF_ID + "' ";
                if (_dbHelper.CmdExecute(deleteQuery) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-02292"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Child Record Found.";
                    return _validationMsg;
                }
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to Delete Data.";
            }
            return _validationMsg;
        }
        public ValidationMsg DeleteStrengthGrid(int DFS_ID)
        {
            try
            {
                string deleteQuery = "DELETE FROM SCP_GEN_DF_STRENGTH WHERE DFS_ID = '" + DFS_ID + "' ";
                if (_dbHelper.CmdExecute(deleteQuery) > 0)
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