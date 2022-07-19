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
    public class SpecCamDocInsConfigDAO
    {
        private DBHelper _dbHelper = new DBHelper();
        DBConnection dbConnection = new DBConnection();
        private ValidationMsg _validationMsg = new ValidationMsg();
        DataTable _dt = new DataTable();
        IDGenerated idGenerated = new IDGenerated();
        public ValidationMsg Save(SpecCamDocInsConfigModel model, string userid)
        {
            try
            {
                // using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                //{

                string insertQuery = string.Empty;
                #region  Check SCP_CODE for new entry

                _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_DOC_INST_CONFIG WHERE SCP_CODE='" + model.SCP_CODE + "'");
                if (_dt.Rows.Count > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Already Entry This Special Campaign.";
                    return _validationMsg;
                }
                #endregion

                #region Insert SCDIC Master Data

                insertQuery = "INSERT INTO SCP_DOC_INST_CONFIG (SDIC_ID,SCP_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES(SCP_DOC_INST_CONFIG_SEQ.NEXTVAL,'" + model.SCP_CODE +
                   "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";

                _dbHelper.CmdExecute(insertQuery);

                #endregion

                _dt = _dbHelper.GetDataTable("SELECT MAX(SDIC_ID) FROM SCP_DOC_INST_CONFIG");
                if (_dt.Rows.Count > 0)
                    model.SDIC_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                #region  Insert Scp Group Info
                if (model.ScpGroupInfoList != null)
                {
                    string qryDocInsert = string.Empty;
                    foreach (SCPGroupInfoModel objScpGroupInfo in model.ScpGroupInfoList)
                    {
                        qryDocInsert = "INSERT INTO SCP_DOC_INST_CONFIG_GROUP(SDICG_ID,SDIC_ID,GROUP_NAME,HBTERRITORY_CODE,HBTERRITORY_NAME,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                         "VALUES(SCP_DOC_INST_CONFIG_GROUP_SEQ.NEXTVAL,'" + model.SDIC_ID + "', '" + objScpGroupInfo.GROUP_NAME +
                          "','" + objScpGroupInfo.HBTERRITORY_CODE + "','" + objScpGroupInfo.HBTERRITORY_NAME + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                        _dbHelper.CmdExecute(qryDocInsert);

                        _dt = _dbHelper.GetDataTable("SELECT MAX(SDICG_ID) FROM SCP_DOC_INST_CONFIG_GROUP");
                        if (_dt.Rows.Count > 0)
                            objScpGroupInfo.SDICG_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                        #region Insert Doctor List Info Data

                        if (model.DoctorInfoList != null)
                        {
                            //string qryDocInsert = string.Empty;
                            foreach (SCPCDoctorInfoModel objDoctorInfo in model.DoctorInfoList)
                            {
                                if (objDoctorInfo.SDI_ID == 0)
                                {  //insert
                                    #region  Check Duplicate Entry With DoctorID & Market Code

                                    _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_DOC_INFO  SDI  WHERE SDI.SDICG_ID='" + objScpGroupInfo.SDICG_ID + "' AND  SDI.DOCTOR_ID='" + objDoctorInfo.DoctorId + "' AND SDI.MARKET_CODE='" + objDoctorInfo.MarketCode + "'");
                                    if (_dt.Rows.Count > 0)
                                    {
                                        _validationMsg.Type = Enums.MessageType.Error;
                                        _validationMsg.Msg = "Duplicate Entry Found In Doctor Grid.";
                                        return _validationMsg;
                                    }
                                    #endregion
                                    qryDocInsert = "INSERT INTO SCP_DOC_INFO(SDI_ID,SDICG_ID,DOCTOR_ID,MARKET_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                   "VALUES(SCP_DOC_INFO_SEQ.NEXTVAL,'" + objScpGroupInfo.SDICG_ID + "', '" + objDoctorInfo.DoctorId + "', '" + objDoctorInfo.MarketCode +
                                    "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                    _dbHelper.CmdExecute(qryDocInsert);

                                }
                                else
                                {
                                    // update
                                }
                            }
                        }

                        #endregion

                        #region Insert Institute List Info Data

                        if (model.InstitutionInfoList != null)
                        {
                            string qryInsInsert = string.Empty;
                            foreach (SCPInstitutionInfoModel objInstInfo in model.InstitutionInfoList)
                            {
                                if (objInstInfo.SDII_ID == 0)
                                {
                                    #region  Check Duplicate Entry With Institute Code & Market Code

                                    _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_DOC_INST_INFO  SII WHERE  SII.SDICG_ID='" + objScpGroupInfo.SDICG_ID + "' AND SII.INSTI_CODE='" + objInstInfo.INSTI_CODE + "' AND SII.MARKET_CODE='" + objInstInfo.MarketCode + "'");
                                    if (_dt.Rows.Count > 0)
                                    {
                                        _validationMsg.Type = Enums.MessageType.Error;
                                        _validationMsg.Msg = "Duplicate Entry Found In Institute Grid.";
                                        return _validationMsg;
                                    }
                                    #endregion
                                    //insert
                                    qryInsInsert = "INSERT INTO SCP_DOC_INST_INFO(SDII_ID,SDICG_ID,INSTI_CODE,MARKET_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                  "VALUES(SCP_DOC_INST_INFO_SEQ.NEXTVAL,'" + objScpGroupInfo.SDICG_ID + "', '" + objInstInfo.INSTI_CODE + "', '" + objInstInfo.MarketCode +
                                   "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                    _dbHelper.CmdExecute(qryInsInsert);

                                }
                                else
                                {
                                    // update
                                }
                            }
                        }

                        #endregion
                    }
                }



                #endregion



                _validationMsg.ReturnId = model.SDIC_ID;
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
        public ValidationMsg Update(SpecCamDocInsConfigModel model, string userid)
        {
            try
            {
                if (model.SDIC_ID > 0)
                {
                    string updateQuery = "UPDATE SCP_DOC_INST_CONFIG SET SCP_CODE = '" + model.SCP_CODE +
                        "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "'" +
                        "  WHERE SDIC_ID = '" + model.SDIC_ID + "'";
                    _dbHelper.CmdExecute(updateQuery);

                    if (model.ScpGroupInfoList != null)
                    {

                        foreach (SCPGroupInfoModel objGroupInfo in model.ScpGroupInfoList)
                        {

                            if (objGroupInfo.SDICG_ID == 0)
                            {
                                string qryInsert = "INSERT INTO SCP_DOC_INST_CONFIG_GROUP(SDICG_ID,SDIC_ID,GROUP_NAME,HBTERRITORY_CODE,HBTERRITORY_NAME,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                         "VALUES(SCP_DOC_INST_CONFIG_GROUP_SEQ.NEXTVAL,'" + model.SDIC_ID + "', '" + objGroupInfo.GROUP_NAME +
                          "','" + objGroupInfo.HBTERRITORY_CODE + "','" + objGroupInfo.HBTERRITORY_NAME + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                _dbHelper.CmdExecute(qryInsert);

                                _dt = _dbHelper.GetDataTable("SELECT MAX(SDICG_ID) FROM SCP_DOC_INST_CONFIG_GROUP");
                                if (_dt.Rows.Count > 0)
                                    objGroupInfo.SDICG_ID = Convert.ToInt32(_dt.Rows[0][0].ToString());

                                #region Doctor Info

                                if (model.DoctorInfoList != null)
                                {
                                    foreach (SCPCDoctorInfoModel objDoctorInfo in model.DoctorInfoList)
                                    {
                                        #region  Check Duplicate Entry With DoctorID & Market Code

                                        _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_DOC_INFO  SDI  WHERE SDI.SDICG_ID='" + objGroupInfo.SDICG_ID + "' AND  SDI.DOCTOR_ID='" + objDoctorInfo.DoctorId + "' AND SDI.MARKET_CODE='" + objDoctorInfo.MarketCode + "'");
                                        if (_dt.Rows.Count > 0)
                                        {
                                            _validationMsg.Type = Enums.MessageType.Error;
                                            _validationMsg.Msg = "Duplicate Entry Found In Doctor Grid.";
                                            return _validationMsg;
                                        }
                                        #endregion
                                        if (objDoctorInfo.SDI_ID == 0)
                                        {
                                            string qryDocInsert = "INSERT INTO SCP_DOC_INFO(SDI_ID,SDICG_ID,DOCTOR_ID,MARKET_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                          "VALUES(SCP_DOC_INFO_SEQ.NEXTVAL,'" + objGroupInfo.SDICG_ID + "', '" + objDoctorInfo.DoctorId + "', '" + objDoctorInfo.MarketCode +
                                           "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                            _dbHelper.CmdExecute(qryDocInsert);
                                        }
                                        else
                                        {
                                            string updateqry = "UPDATE SCP_DOC_INFO SET DOCTOR_ID = '" + objDoctorInfo.DoctorId + "',MARKET_CODE = '" + objDoctorInfo.MarketCode + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "' WHERE SDI_ID = '" + objDoctorInfo.SDI_ID + "'";
                                            _dbHelper.CmdExecute(updateqry);
                                        }
                                    }
                                }
                                #endregion

                                #region Institute Info

                                if (model.InstitutionInfoList != null)
                                {
                                    foreach (SCPInstitutionInfoModel objInstInfo in model.InstitutionInfoList)
                                    {
                                        #region  Check Duplicate Entry With Institute Code & Market Code

                                        _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_DOC_INST_INFO  SII WHERE  SII.SDICG_ID='" + objGroupInfo.SDICG_ID + "' AND SII.INSTI_CODE='" + objInstInfo.INSTI_CODE + "' AND SII.MARKET_CODE='" + objInstInfo.MarketCode + "'");
                                        if (_dt.Rows.Count > 0)
                                        {
                                            _validationMsg.Type = Enums.MessageType.Error;
                                            _validationMsg.Msg = "Duplicate Entry Found In Institute Grid.";
                                            return _validationMsg;
                                        }
                                        #endregion
                                        if (objInstInfo.SDII_ID == 0)
                                        {
                                            //insert
                                            string qryInsInsert = "INSERT INTO SCP_DOC_INST_INFO(SDII_ID,SDICG_ID,INSTI_CODE,MARKET_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                           "VALUES(SCP_DOC_INST_INFO_SEQ.NEXTVAL,'" + objGroupInfo.SDICG_ID + "', '" + objInstInfo.INSTI_CODE + "', '" + objInstInfo.MarketCode +
                                            "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                            _dbHelper.CmdExecute(qryInsInsert);
                                        }
                                        else
                                        {
                                            string updateqry = "UPDATE SCP_DOC_INST_INFO SET INSTI_CODE = '" + objInstInfo.INSTI_CODE + "',MARKET_CODE = '" + objInstInfo.MarketCode + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "' WHERE SDII_ID = '" + objInstInfo.SDII_ID + "'";
                                            _dbHelper.CmdExecute(updateqry);
                                        }
                                    }
                                }
                                #endregion

                            }
                            else  //update group
                            {
                                updateQuery = "UPDATE SCP_DOC_INST_CONFIG_GROUP SET GROUP_NAME = '" + objGroupInfo.GROUP_NAME +
                        "', HBTERRITORY_CODE= '" + objGroupInfo.HBTERRITORY_CODE + "',HBTERRITORY_NAME = '" + objGroupInfo.HBTERRITORY_NAME + "',  UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "'" +
                        "  WHERE SDICG_ID = '" + objGroupInfo.SDICG_ID + "'";
                                _dbHelper.CmdExecute(updateQuery);


                                #region Doctor Info

                                if (model.DoctorInfoList != null)
                                {
                                    foreach (SCPCDoctorInfoModel objDoctorInfo in model.DoctorInfoList)
                                    {
                                        #region  Check Duplicate Entry With DoctorID & Market Code

                                        _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_DOC_INFO  SDI  WHERE SDI.SDICG_ID='" + objGroupInfo.SDICG_ID + "' AND  SDI.DOCTOR_ID='" + objDoctorInfo.DoctorId + "' AND SDI.MARKET_CODE='" + objDoctorInfo.MarketCode + "'");
                                        if (_dt.Rows.Count > 0)
                                        {
                                            _validationMsg.Type = Enums.MessageType.Error;
                                            _validationMsg.Msg = "Duplicate Entry Found In Doctor Grid.";
                                            return _validationMsg;
                                        }
                                        #endregion
                                        if (objDoctorInfo.SDI_ID == 0)
                                        {
                                            Int64 SDI_ID = idGenerated.getMAXSL("SDI_ID", "SCP_DOC_INFO");
                                            string qryDocInsert = "INSERT INTO SCP_DOC_INFO(SDI_ID,SDICG_ID,DOCTOR_ID,MARKET_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                          "VALUES(" + SDI_ID + ",'" + objGroupInfo.SDICG_ID + "', '" + objDoctorInfo.DoctorId + "', '" + objDoctorInfo.MarketCode +
                                           "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                            _dbHelper.CmdExecute(qryDocInsert);
                                        }
                                        else
                                        {
                                            string updateqry = "UPDATE SCP_DOC_INFO SET DOCTOR_ID = '" + objDoctorInfo.DoctorId + "',MARKET_CODE = '" + objDoctorInfo.MarketCode + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "' WHERE SDI_ID = '" + objDoctorInfo.SDI_ID + "'";
                                            _dbHelper.CmdExecute(updateqry);
                                        }
                                    }
                                }
                                #endregion

                                #region Institute Info

                                if (model.InstitutionInfoList != null)
                                {
                                    foreach (SCPInstitutionInfoModel objInstInfo in model.InstitutionInfoList)
                                    {
                                        #region  Check Duplicate Entry With Institute Code & Market Code

                                        _dt = _dbHelper.GetDataTable("SELECT 1 FROM SCP_DOC_INST_INFO  SII WHERE  SII.SDICG_ID='" + objGroupInfo.SDICG_ID + "' AND SII.INSTI_CODE='" + objInstInfo.INSTI_CODE + "' AND SII.MARKET_CODE='" + objInstInfo.MarketCode + "'");
                                        if (_dt.Rows.Count > 0)
                                        {
                                            _validationMsg.Type = Enums.MessageType.Error;
                                            _validationMsg.Msg = "Duplicate Entry Found In Institute Grid.";
                                            return _validationMsg;
                                        }
                                        #endregion
                                        if (objInstInfo.SDII_ID == 0)
                                        {
                                            //insert
                                            string qryInsInsert = "INSERT INTO SCP_DOC_INST_INFO(SDII_ID,SDICG_ID,INSTI_CODE,MARKET_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                           "VALUES(SCP_DOC_INST_INFO_SEQ.NEXTVAL,'" + objGroupInfo.SDICG_ID + "', '" + objInstInfo.INSTI_CODE + "', '" + objInstInfo.MarketCode +
                                            "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                                            _dbHelper.CmdExecute(qryInsInsert);
                                        }
                                        else
                                        {
                                            string updateqry = "UPDATE SCP_DOC_INST_INFO SET INSTI_CODE = '" + objInstInfo.INSTI_CODE + "',MARKET_CODE = '" + objInstInfo.MarketCode + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_BY = '" + userid + "' WHERE SDII_ID = '" + objInstInfo.SDII_ID + "'";
                                            _dbHelper.CmdExecute(updateqry);
                                        }
                                    }
                                }
                                #endregion


                            }

                        }
                    }


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

        public List<DoctorInformationModel> GetAllDoctorInfo()
        {
            var query = new StringBuilder();
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();

            query.Append(" SELECT  D.DOCTOR_ID,D.DOCTOR_NAME,D.DEGREE,D.ADDRESS1,D.ADDRESS2,D.ADDRESS3,D.ADDRESS4,");
            query.Append("  DS.SPECIALIZATION  FROM DOCTOR  D");
            query.Append("  LEFT OUTER JOIN DOCTOR_SPECIALIZATION   DS ON DS.SPECIALIZATION_CODE=D.SPECIA_1ST_CODE ");
            query.Append("  WHERE ROWNUM <=500");


            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query.ToString(), con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.DoctorName = reader["DOCTOR_NAME"].ToString();
                        model.Degree = reader["DEGREE"].ToString();
                        model.SpeciFirstName = reader["SPECIALIZATION"].ToString();
                        //model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        model.Address = reader["ADDRESS1"].ToString() + "," + reader["ADDRESS2"].ToString() + "," + reader["ADDRESS3"].ToString() + "" + reader["ADDRESS4"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }
        //public IEnumerable<string> GetDoctorListForSearch()
        //{
        //    //return _context.SalesInvoices.Select(m => m.InvoiceNo).Distinct().ToList();
        //    //return "";
        //}
        //public IEnumerable<DoctorInformationModel> GetDoctorBySearch(string doctorName)
        //{

        //    var query = new StringBuilder();
        //    List<DoctorInformationModel> listData = new List<DoctorInformationModel>();

        //    query.Append(" SELECT  D.DOCTOR_ID,D.DOCTOR_NAME,D.DEGREE,D.ADDRESS1,D.ADDRESS2,D.ADDRESS3,D.ADDRESS4,");
        //    query.Append("  DS.SPECIALIZATION  FROM DOCTOR  D");
        //    query.Append("  LEFT OUTER JOIN DOCTOR_SPECIALIZATION   DS ON DS.SPECIALIZATION_CODE=D.SPECIA_1ST_CODE ");
        //    //query.Append("  WHERE ROWNUM <=500");
        //    query.Append("  WHERE 1=1");
        //    if (!string.IsNullOrEmpty(doctorName))
        //    {
        //        //query.Append(" AND D.DOCTOR_NAME LIKE '%" + doctorName + "%'"); contain

        //        query.Append(" AND D.DOCTOR_NAME LIKE '" + doctorName + "%'"); //startwith
        //    }
        //    else
        //    {
        //        query.Append(" AND ROWNUM <=500");
        //    }
        //    query.Append(" ORDER BY D.DOCTOR_NAME");

        //    using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
        //    {
        //        OracleCommand cmd = new OracleCommand(query.ToString(), con);
        //        con.Open();
        //        using (OracleDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                DoctorInformationModel model = new DoctorInformationModel();
        //                model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
        //                model.DoctorName = reader["DOCTOR_NAME"].ToString();
        //                model.Degree = reader["DEGREE"].ToString();
        //                model.SpeciFirstName = reader["SPECIALIZATION"].ToString();
        //                //model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
        //                model.Address = reader["ADDRESS1"].ToString() + "," + reader["ADDRESS2"].ToString() + "," + reader["ADDRESS3"].ToString() + "" + reader["ADDRESS4"].ToString();
        //                listData.Add(model);
        //            }
        //        }
        //    }
        //    return listData;
        //}

        public IEnumerable<DoctorInformationModel> GetDoctorBySearch(string doctorCode)
        {

            var query = new StringBuilder();
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();

            query.Append(" SELECT  D.DOCTOR_ID,D.DOCTOR_NAME,D.DEGREE,D.ADDRESS1,D.ADDRESS2,D.ADDRESS3,D.ADDRESS4,");
            query.Append("  DS.SPECIALIZATION  FROM DOCTOR  D");
            query.Append("  LEFT OUTER JOIN DOCTOR_SPECIALIZATION   DS ON DS.SPECIALIZATION_CODE=D.SPECIA_1ST_CODE ");
            //query.Append("  WHERE ROWNUM <=500");
            query.Append("  WHERE 1=1");
            if (!string.IsNullOrEmpty(doctorCode))
            {
                //query.Append(" AND D.DOCTOR_NAME LIKE '%" + doctorName + "%'"); contain

                //query.Append(" AND D.DOCTOR_ID LIKE '" + doctorCode + "%'"); //startwith
                query.Append(" AND D.DOCTOR_ID = '" + doctorCode + "'"); //startwith
            }
            else
            {
                query.Append(" AND ROWNUM <=500");
            }
            query.Append(" ORDER BY D.DOCTOR_NAME");

            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query.ToString(), con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.DoctorName = reader["DOCTOR_NAME"].ToString();
                        model.Degree = reader["DEGREE"].ToString();
                        model.SpeciFirstName = reader["SPECIALIZATION"].ToString();
                        //model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        model.Address = reader["ADDRESS1"].ToString() + "," + reader["ADDRESS2"].ToString() + "," + reader["ADDRESS3"].ToString() + "" + reader["ADDRESS4"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }
        public List<InstitutionInformationModel> GetAllInstituteInfo()
        {
            var query = new StringBuilder();
            List<InstitutionInformationModel> listData = new List<InstitutionInformationModel>();

            query.Append(" SELECT IT.INSTI_CODE,IT.INSTI_NAME,TP.INSTI_TYPE_NAME,IT.ADDRESS1,IT.ADDRESS2,IT.ADDRESS3,IT.ADDRESS4,");
            query.Append("  TP.INSTI_TYPE_NAME FROM INSTITUTION IT");
            query.Append("  LEFT OUTER JOIN INSTITUTION_TYPE TP ON TP.INSTI_TYPE_CODE=IT.INSTI_CODE ");
            query.Append("  WHERE ROWNUM <=500");



            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query.ToString(), con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        InstitutionInformationModel model = new InstitutionInformationModel();
                        model.INSTI_CODE = reader["INSTI_CODE"].ToString();
                        model.InstitutionName = reader["INSTI_NAME"].ToString();
                        model.Type = reader["INSTI_TYPE_NAME"].ToString();
                        model.Address = reader["ADDRESS1"].ToString() + "," + reader["ADDRESS2"].ToString() + "," + reader["ADDRESS3"].ToString() + "" + reader["ADDRESS4"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        //public IEnumerable<InstitutionInformationModel> GetInstituteBySearch(string instituteName)
        //{

        //    var query = new StringBuilder();
        //    List<InstitutionInformationModel> listData = new List<InstitutionInformationModel>();

        //    query.Append(" SELECT IT.INSTI_CODE,IT.INSTI_NAME,TP.INSTI_TYPE_NAME,IT.ADDRESS1,IT.ADDRESS2,IT.ADDRESS3,IT.ADDRESS4,");
        //    query.Append("  TP.INSTI_TYPE_NAME FROM INSTITUTION IT");
        //    query.Append("  LEFT OUTER JOIN INSTITUTION_TYPE TP ON TP.INSTI_TYPE_CODE=IT.INSTI_CODE ");
        //    //query.Append("  WHERE ROWNUM <=500");
        //    query.Append("  WHERE 1=1");
        //    if (!string.IsNullOrEmpty(instituteName))
        //    {
        //        //query.Append(" AND IT.INSTI_NAME LIKE '%" + instituteName + "%'"); contain

        //        query.Append(" AND IT.INSTI_NAME LIKE '" + instituteName + "%'"); //startwith
        //    }
        //    else
        //    {
        //        query.Append(" AND ROWNUM <=500");
        //    }
        //    query.Append(" ORDER BY IT.INSTI_NAME");

        //    using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
        //    {
        //        OracleCommand cmd = new OracleCommand(query.ToString(), con);
        //        con.Open();
        //        using (OracleDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                InstitutionInformationModel model = new InstitutionInformationModel();
        //                model.INSTI_CODE = reader["INSTI_CODE"].ToString();
        //                model.InstitutionName = reader["INSTI_NAME"].ToString();
        //                model.Type = reader["INSTI_TYPE_NAME"].ToString();
        //                model.Address = reader["ADDRESS1"].ToString() + "," + reader["ADDRESS2"].ToString() + "," + reader["ADDRESS3"].ToString() + "" + reader["ADDRESS4"].ToString();
        //                listData.Add(model);
        //            }
        //        }
        //    }
        //    return listData;
        //}
        public IEnumerable<InstitutionInformationModel> GetInstituteBySearch(string instituteCode)
        {

            var query = new StringBuilder();
            List<InstitutionInformationModel> listData = new List<InstitutionInformationModel>();

            query.Append(" SELECT IT.INSTI_CODE,IT.INSTI_NAME,TP.INSTI_TYPE_NAME,IT.ADDRESS1,IT.ADDRESS2,IT.ADDRESS3,IT.ADDRESS4,");
            query.Append("  TP.INSTI_TYPE_NAME FROM INSTITUTION IT");
            query.Append("  LEFT OUTER JOIN INSTITUTION_TYPE TP ON TP.INSTI_TYPE_CODE=IT.INSTI_CODE ");
            //query.Append("  WHERE ROWNUM <=500");
            query.Append("  WHERE 1=1");
            if (!string.IsNullOrEmpty(instituteCode))
            {
                //query.Append(" AND IT.INSTI_NAME LIKE '%" + instituteName + "%'"); contain

                // query.Append(" AND IT.INSTI_CODE LIKE '" + instituteCode + "%'"); //startwith
                query.Append(" AND IT.INSTI_CODE ='" + instituteCode + "'"); //startwith
            }
            else
            {
                query.Append(" AND ROWNUM <=500");
            }
            query.Append(" ORDER BY IT.INSTI_NAME");

            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query.ToString(), con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        InstitutionInformationModel model = new InstitutionInformationModel();
                        model.INSTI_CODE = reader["INSTI_CODE"].ToString();
                        model.InstitutionName = reader["INSTI_NAME"].ToString();
                        model.Type = reader["INSTI_TYPE_NAME"].ToString();
                        model.Address = reader["ADDRESS1"].ToString() + "," + reader["ADDRESS2"].ToString() + "," + reader["ADDRESS3"].ToString() + "" + reader["ADDRESS4"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }
        public List<SpecCamDocInsConfigModel> GetAll(int p)
        {
            string Qry = "SELECT SDIC.SDIC_ID,SDIC.SCP_CODE,SI.SCP_NAME FROM SCP_DOC_INST_CONFIG SDIC  LEFT OUTER JOIN SCP_INFO SI ON SI.SCP_CODE=SDIC.SCP_CODE ORDER BY SDIC.SDIC_ID DESC";

            _dt = _dbHelper.GetDataTable(Qry);
            List<SpecCamDocInsConfigModel> itemList = new List<SpecCamDocInsConfigModel>();
            itemList = (from DataRow row in _dt.Rows
                        select new SpecCamDocInsConfigModel
                        {
                            SDIC_ID = Convert.ToInt32(row["SDIC_ID"].ToString()),
                            SCP_CODE = row["SCP_CODE"].ToString(),
                            SCP_NAME = row["SCP_NAME"].ToString(),
                        }).ToList();
            return itemList;
        }

        public IEnumerable<SCPGroupInfoModel> GetScpGroupItemList(int p)
        {
            string Qry = "SELECT SDICG_ID, SDIC_ID, GROUP_NAME,HBTERRITORY_CODE,HBTERRITORY_NAME FROM SCP_DOC_INST_CONFIG_GROUP WHERE SDIC_ID='" + p + "' ORDER BY SDICG_ID DESC";

            _dt = _dbHelper.GetDataTable(Qry);
            List<SCPGroupInfoModel> itemList = new List<SCPGroupInfoModel>();
            itemList = (from DataRow row in _dt.Rows
                        select new SCPGroupInfoModel
                        {
                            SDICG_ID = Convert.ToInt32(row["SDICG_ID"].ToString()),
                            SDIC_ID = Convert.ToInt32(row["SDIC_ID"].ToString()),
                            GROUP_NAME = row["GROUP_NAME"].ToString(),
                            HBTERRITORY_NAME = row["HBTERRITORY_NAME"].ToString(),
                            HBTERRITORY_CODE = row["HBTERRITORY_CODE"].ToString(),
                        }).ToList();
            return itemList;
        }
        public List<SCPGroupInfoModel> GetAllTerritoryList()
        {
            string Qry = "";
            //Qry = "SELECT HBTERRITORY_CODE ,HBTERRITORY_NAME FROM HBTERRITOTYINFO ";
            Qry = "SELECT '0' HBTERRITORY_CODE, '' HBTERRITORY_NAME  from DUAL " +
            " union" +
            " SELECT HBTERRITORY_CODE ,HBTERRITORY_NAME FROM HBTERRITOTYINFO ";
            Qry = Qry + "ORDER BY HBTERRITORY_CODE";

            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<SCPGroupInfoModel> items;
            items = (from DataRow row in dt.Rows
                     select new SCPGroupInfoModel
                     {
                         HBTERRITORY_CODE = row["HBTERRITORY_CODE"].ToString(),
                         HBTERRITORY_NAME = row["HBTERRITORY_NAME"].ToString(),

                     }).ToList();
            return items;
        }
        public List<SCPCDoctorInfoModel> GetDocItemList(int p)
        {
            var query = new StringBuilder();
            query.Append("  SELECT SDI.SDI_ID,SDI.DOCTOR_ID , SDI.MARKET_CODE,D.DOCTOR_NAME,D.DEGREE,D.ADDRESS1,D.ADDRESS2,D.ADDRESS3,D.ADDRESS4, ");
            query.Append("  DS.SPECIALIZATION,NVL(M.MARKET_NAME,'Press F9')MARKET_NAME FROM SCP_DOC_INFO  SDI ");
            query.Append("  LEFT OUTER JOIN DOCTOR D ON D.DOCTOR_ID=SDI.DOCTOR_ID ");
            query.Append("  LEFT OUTER JOIN MARKET M ON M.MARKET_CODE=SDI.MARKET_CODE ");
            query.Append("  LEFT OUTER JOIN DOCTOR_SPECIALIZATION DS ON DS.SPECIALIZATION_CODE=D.SPECIA_1ST_CODE ");
            query.Append("  WHERE 1=1");

            if (p != 0)
            {
                query.Append(" AND SDI.SDICG_ID ='" + p + "'"); //SDICG_ID
            }
            query.Append("  ORDER BY SDI.SDI_ID DESC");

            //_dt = _dbHelper.GetDataTable(Qry);
            List<SCPCDoctorInfoModel> listData = new List<SCPCDoctorInfoModel>();

            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query.ToString(), con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SCPCDoctorInfoModel model = new SCPCDoctorInfoModel();
                        model.SDI_ID = Convert.ToInt32(reader["SDI_ID"]);
                        model.DoctorId = reader["DOCTOR_ID"].ToString(); //Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.DoctorName = reader["DOCTOR_NAME"].ToString();
                        model.Degree = reader["DEGREE"].ToString();
                        model.SpeciFirstName = reader["SPECIALIZATION"].ToString();
                        //model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        model.Address = reader["ADDRESS1"].ToString() + "," + reader["ADDRESS2"].ToString() + "," + reader["ADDRESS3"].ToString() + "" + reader["ADDRESS4"].ToString();
                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        model.MarketName = reader["MARKET_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<SCPInstitutionInfoModel> GetInstiItemList(int p)
        {
            var query = new StringBuilder();
            query.Append("  SELECT SII.SDII_ID,SII.INSTI_CODE,INST.INSTI_NAME,TP.INSTI_TYPE_NAME,INST.ADDRESS1,INST.ADDRESS2,INST.ADDRESS3,INST.ADDRESS4, ");
            query.Append("  SII.MARKET_CODE, NVL(M.MARKET_NAME,'Press F9')MARKET_NAME FROM SCP_DOC_INST_INFO  SII ");
            query.Append("  LEFT OUTER JOIN INSTITUTION INST ON INST.INSTI_CODE=SII.INSTI_CODE ");
            query.Append("  LEFT OUTER JOIN MARKET M ON M.MARKET_CODE=SII.MARKET_CODE ");
            query.Append("  LEFT OUTER JOIN INSTITUTION_TYPE TP ON TP.INSTI_TYPE_CODE=INST.INSTI_CODE ");
            query.Append("  WHERE 1=1");

            if (p != 0)
            {
                query.Append(" AND SII.SDICG_ID ='" + p + "'");
            }
            query.Append("  ORDER BY SII.SDII_ID DESC");

            //_dt = _dbHelper.GetDataTable(Qry);
            List<SCPInstitutionInfoModel> listData = new List<SCPInstitutionInfoModel>();

            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query.ToString(), con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SCPInstitutionInfoModel model = new SCPInstitutionInfoModel();
                        model.SDII_ID = Convert.ToInt32(reader["SDII_ID"]);
                        model.INSTI_CODE = reader["INSTI_CODE"].ToString();//Convert.ToInt32(reader["INSTI_CODE"]);
                        model.InstitutionName = reader["INSTI_NAME"].ToString();
                        model.Type = reader["INSTI_TYPE_NAME"].ToString();
                        model.Address = reader["ADDRESS1"].ToString() + "," + reader["ADDRESS2"].ToString() + "," + reader["ADDRESS3"].ToString() + "" + reader["ADDRESS4"].ToString();
                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        model.MarketName = reader["MARKET_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public ValidationMsg Delete(int SDIC_ID)
        {
            try
            {
                string deleteQuery = "DELETE FROM SCP_DOC_INST_CONFIG WHERE SDIC_ID = '" + SDIC_ID + "' ";
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

        public ValidationMsg DeletedDoctorGrid(int SDI_ID)
        {
            try
            {
                string deleteQuery = "DELETE FROM SCP_DOC_INFO WHERE SDI_ID = '" + SDI_ID + "' ";
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
        public ValidationMsg DeletedInstiGrid(int SDII_ID)
        {
            try
            {
                string deleteQuery = "DELETE FROM SCP_DOC_INST_INFO WHERE SDII_ID = '" + SDII_ID + "' ";
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



        public ValidationMsg DeleteScpGroupGrid(int p)
        {
            try
            {
                string deleteQuery = "DELETE FROM SCP_DOC_INST_CONFIG_GROUP WHERE SDICG_ID = '" + p + "' ";
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
    }
}