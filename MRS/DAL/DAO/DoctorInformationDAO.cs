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
    public class DoctorInformationDAO
    {

        DBHelper dbHelper = new DBHelper();
        DBConnection dbConnection = new DBConnection();
        IDGenerated idGenerated = new IDGenerated();
        ValidationMsg _vmMsg;
        long mxSl = 0;
        string code = string.Empty;
        public List<SpecializationModel> GetSpecializationData()
        {
            List<SpecializationModel> listData = new List<SpecializationModel>();
            string query = "SELECT SPECIALIZATION_CODE,SPECIALIZATION FROM DOCTOR_SPECIALIZATION ORDER BY SPECIALIZATION";
            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                { 
                    while (reader.Read())
                    {
                        SpecializationModel model = new SpecializationModel();
                        model.SpecializationCode = reader.GetInt32(0);
                        model.Specialization = reader.GetString(1);
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }
        public List<DoctorInformationModel> GetDesignationCategory()
        {
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();
            string query = "SELECT DESIGNATION_CODE,DESIGNATION FROM DOCTOR_DESIGNATION";
            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.DesignationCategory = reader.GetInt32(0);
                        model.DesignationName = reader.GetString(1);
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<DoctorInformationModel> GetDegreeInfo()
        {
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();
            string query = "SELECT DEGREE_CODE,DEGREE FROM DOCTOR_DEGREE";
            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.DegreeCategory = reader.GetInt32(0);
                        model.DegreeName = reader.GetString(1);
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public ValidationMsg Save(DoctorInformationModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                if (model.DoctorInfoModel != null)
                {
                    foreach (DoctorInformationModel docModel in model.DoctorInfoModel)
                    {
                        mxSl = idGenerated.getMAXSL("DOCTOR_ID", "DOCTOR Where DOCTOR_ID not in (900000)");

                        string qry1 = "INSERT INTO DOCTOR (DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,HONORARIUM,DOCTOR_NAME,DEGREE,DEGREE_CODE,DESIGNATION,DESIGNATION_CODE,SPECIA_1ST_CODE,SPECIA_2ND_CODE,GENDER,RELIGION,DATE_OF_BIRTH,DOC_PERS_PHONE, " +
                                     "DOCTOR_EMAIL,PATIENT_PER_DAY,AVG_PRESC_VALUE,PRESC_SHARE,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,REMARKS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                        "VALUES(" + mxSl + ", '" + docModel.RegistrationNo + "', '" + docModel.PotentialCategory + "', '" + docModel.Honorium + "', '" + docModel.DoctorName + "','" + docModel.Degree + "', " +
                        "'" + docModel.DegreeCategory + "','" + docModel.Designation + "','" + docModel.DesignationCategory + "','" + docModel.SpeciFirstCode + "','" + docModel.SpeciSecondCode + "'," +
                        "'" + docModel.Gender + "','" + docModel.Religion + "',(TO_DATE('" + docModel.DateOfBirth + "','dd-MM-yyyy')),'" + docModel.Phone + "','" + docModel.Email + "','" + docModel.PatientNo + "','" + docModel.PrescriptionValue + "'," +
                        "'" + docModel.PrescriptionShare + "','" + docModel.Address1 + "','" + docModel.Address2 + "','" + docModel.Address3 + "','" + docModel.Address4 + "','" + docModel.Remarks + "','" + userid + "'," +
                        "(TO_DATE('" + docModel.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";

                        dbHelper.CmdExecute(qry1);

                        if (model.DoctorMarketDetailsModels != null)
                        {
                            model.DoctorMstSl = idGenerated.getMAXSL("DOC_MKT_MAS_SLNO", "DOC_MKT_MAS");
                            string query = "Insert into DOC_MKT_MAS(DOC_MKT_MAS_SLNO,DOCTOR_ID)values(" + model.DoctorMstSl + "," + mxSl + ")";
                            dbHelper.CmdExecute(query);

                            foreach (var detailModel in model.DoctorMarketDetailsModels)
                            {
                                detailModel.DoctorDetailSl = idGenerated.getMAXSL("DOC_MKT_DTL_SLNO", "DOC_MKT_DTL");
                                string query1 = "Insert Into DOC_MKT_DTL(DOC_MKT_DTL_SLNO,DOC_MKT_MAS_SLNO,PRAC_MKT_CODE,SBU_UNIT,CHAMB_ADDRESS1,CHAMB_ADDRESS2,CHAMB_ADDRESS3,CHAMB_ADDRESS4,CHAMB_PHONE, " +
                                        "UPAZILA_CODE,MDP_LOC_CODE,EDP_LOC_CODE,INSTI_CODE,ENTRY_DATE,MDP_LOC_NAME,EDP_LOC_NAME) Values(" + detailModel.DoctorDetailSl + "," + model.DoctorMstSl + ",'" + detailModel.MarketCode + "','" + detailModel.SBU_GROUP + "', " +
                                        "'" + detailModel.ChamberAddress1 + "','" + detailModel.ChamberAddress2 + "','" + detailModel.ChamberAddress3 + "','" + detailModel.ChamberAddress4 + "','" + detailModel.Phone + "', " +
                                        "'" + detailModel.UpazilaCode + "','" + detailModel.MorningLocCode + "','" + detailModel.EveningLocCode + "'," + detailModel.InstituteCode + ",(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + detailModel.MorningLocTextName + "','" + detailModel.EveningTextLocName + "')";
                                dbHelper.CmdExecute(query1);
                            }
                        }
                    }
                }

                mxSl = idGenerated.getMAXSL("DOCTOR_ID", "DOCTOR Where DOCTOR_ID not in (900000)");

                string qry = "INSERT INTO DOCTOR (DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,HONORARIUM,DOCTOR_NAME,DEGREE,DEGREE_CODE,DESIGNATION,DESIGNATION_CODE,SPECIA_1ST_CODE,SPECIA_2ND_CODE,GENDER,RELIGION,DATE_OF_BIRTH,DOC_PERS_PHONE, " +
                             "DOCTOR_EMAIL,PATIENT_PER_DAY,AVG_PRESC_VALUE,PRESC_SHARE,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,REMARKS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                "VALUES(" + mxSl + ", '" + model.RegistrationNo + "', '" + model.PotentialCategory + "', '" + model.Honorium + "', '" + model.DoctorName + "','" + model.Degree + "', " +
                "'" + model.DegreeCategory + "','" + model.Designation + "','" + model.DesignationCategory + "','" + model.SpeciFirstCode + "','" + model.SpeciSecondCode + "'," +
                "'" + model.Gender + "','" + model.Religion + "',(TO_DATE('" + model.DateOfBirth + "','dd-MM-yyyy')),'" + model.Phone + "','" + model.Email + "','" + model.PatientNo + "','" + model.PrescriptionValue + "'," +
                "'" + model.PrescriptionShare + "','" + model.Address1 + "','" + model.Address2 + "','" + model.Address3 + "','" + model.Address4 + "','" + model.Remarks + "','" + userid + "'," +
                "(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";

                dbHelper.CmdExecute(qry);

                if (model.DoctorMarketDetailsModels != null)
                {
                    model.DoctorMstSl = idGenerated.getMAXSL("DOC_MKT_MAS_SLNO", "DOC_MKT_MAS");
                    string query = "Insert into DOC_MKT_MAS(DOC_MKT_MAS_SLNO,DOCTOR_ID)values(" + model.DoctorMstSl + "," + mxSl + ")";
                    dbHelper.CmdExecute(query);

                    foreach (var detailModel in model.DoctorMarketDetailsModels)
                    {
                        detailModel.DoctorDetailSl = idGenerated.getMAXSL("DOC_MKT_DTL_SLNO", "DOC_MKT_DTL");
                        string query1 = "Insert Into DOC_MKT_DTL(DOC_MKT_DTL_SLNO,DOC_MKT_MAS_SLNO,PRAC_MKT_CODE,SBU_UNIT,CHAMB_ADDRESS1,CHAMB_ADDRESS2,CHAMB_ADDRESS3,CHAMB_ADDRESS4,CHAMB_PHONE, " +
                                "UPAZILA_CODE,MDP_LOC_CODE,EDP_LOC_CODE,INSTI_CODE,ENTRY_DATE,MDP_LOC_NAME,EDP_LOC_NAME) Values(" + detailModel.DoctorDetailSl + "," + model.DoctorMstSl + ",'" + detailModel.MarketCode + "','" + detailModel.SBU_GROUP + "', " +
                                "'" + detailModel.ChamberAddress1 + "','" + detailModel.ChamberAddress2 + "','" + detailModel.ChamberAddress3 + "','" + detailModel.ChamberAddress4 + "','" + detailModel.Phone + "', " +
                                "'" + detailModel.UpazilaCode + "','" + detailModel.MorningLocCode + "','" + detailModel.EveningLocCode + "'," + detailModel.InstituteCode + ",(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + detailModel.MorningLocTextName + "','" + detailModel.EveningTextLocName + "')";
                        dbHelper.CmdExecute(query1);
                    }
                }
                if (model.Relatives != null)
                {
                    foreach (DoctorRelativeInfo detail in model.Relatives)
                    {
                        string query = "Insert into DOCTOR_RELATIVE_INFO(RELA_DOC_ID,DOCTOR_ID,RELATION) Values('" + detail.RelativeId + "','" + model.DoctorId + "','" + detail.Relation + "')";
                        dbHelper.CmdExecute(query);
                    }
                }
                /*else
                {
                    mxSl = idGenerated.getMAXSL("DOCTOR_ID", "DOCTOR Where DOCTOR_ID not in (900000)");

                    string qry = "INSERT INTO DOCTOR (DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,HONORARIUM,DOCTOR_NAME,DEGREE,DEGREE_CODE,DESIGNATION,DESIGNATION_CODE,SPECIA_1ST_CODE,SPECIA_2ND_CODE,GENDER,RELIGION,DATE_OF_BIRTH,DOC_PERS_PHONE, " +
                                 "DOCTOR_EMAIL,PATIENT_PER_DAY,AVG_PRESC_VALUE,PRESC_SHARE,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,REMARKS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                    "VALUES(" + mxSl + ", '" + model.RegistrationNo + "', '" + model.PotentialCategory + "', '" + model.Honorium + "', '" + model.DoctorName + "','" + model.Degree + "', " +
                    "'" + model.DegreeCategory + "','" + model.Designation + "','" + model.DesignationCategory + "','" + model.SpeciFirstCode + "','" + model.SpeciSecondCode + "'," +
                    "'" + model.Gender + "','" + model.Religion + "',(TO_DATE('" + model.DateOfBirth + "','dd-MM-yyyy')),'" + model.Phone + "','" + model.Email + "','" + model.PatientNo + "','" + model.PrescriptionValue + "'," +
                    "'" + model.PrescriptionShare + "','" + model.Address1 + "','" + model.Address2 + "','" + model.Address3 + "','" + model.Address4 + "','" + model.Remarks + "','" + userid + "'," +
                    "(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";

                    dbHelper.CmdExecute(qry);

                    if (model.DoctorMarketDetailsModels != null)
                    {
                        model.DoctorMstSl = idGenerated.getMAXSL("DOC_MKT_MAS_SLNO", "DOC_MKT_MAS");
                        string query = "Insert into DOC_MKT_MAS(DOC_MKT_MAS_SLNO,DOCTOR_ID)values(" + model.DoctorMstSl + "," + mxSl + ")";
                        dbHelper.CmdExecute(query);

                        foreach (var detailModel in model.DoctorMarketDetailsModels)
                        {
                            detailModel.DoctorDetailSl = idGenerated.getMAXSL("DOC_MKT_DTL_SLNO", "DOC_MKT_DTL");
                            string query1 = "Insert Into DOC_MKT_DTL(DOC_MKT_DTL_SLNO,DOC_MKT_MAS_SLNO,PRAC_MKT_CODE,CHAMB_ADDRESS1,CHAMB_ADDRESS2,CHAMB_ADDRESS3,CHAMB_ADDRESS4,CHAMB_PHONE, " +
                                    "UPAZILA_CODE,MDP_LOC_CODE,EDP_LOC_CODE,INSTI_CODE,ENTRY_DATE) Values(" + detailModel.DoctorDetailSl + "," + model.DoctorMstSl + ",'" + detailModel.MarketCode + "', " +
                                    "'" + detailModel.ChamberAddress1 + "','" + detailModel.ChamberAddress2 + "','" + detailModel.ChamberAddress3 + "','" + detailModel.ChamberAddress4 + "','" + detailModel.Phone + "', " +
                                    "'" + detailModel.UpazilaCode + "','" + detailModel.MorningLocCode + "','" + detailModel.EveningLocCode + "'," + detailModel.InstituteCode + ",(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')))";
                            dbHelper.CmdExecute(query1);
                        }
                    }
                    if (model.Relatives != null)
                    {
                        foreach (DoctorRelativeInfo detail in model.Relatives)
                        {
                            string query = "Insert into DOCTOR_RELATIVE_INFO(RELA_DOC_ID,DOCTOR_ID,RELATION) Values('" + detail.RelativeId + "','" + model.DoctorId + "','" + detail.Relation + "')";
                            dbHelper.CmdExecute(query);
                        }
                    }
                }*/

                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Saved Successfully.";
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
                if (ex.Message.Contains("ORA-02291"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Please Select Designation & Degree Category";
                }
            }
            return _vmMsg;
        }
        public string GetCode()
        {
            code = mxSl.ToString();
            return code;
        }
        public ValidationMsg Update(DoctorInformationModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                if ((string.IsNullOrEmpty(model.SpeciSecondCode)) || (model.SpeciSecondCode == "0"))
                    model.SpeciSecondCode = string.Empty;
                string qry = "UPDATE DOCTOR SET REGISTRATION_NO='" + model.RegistrationNo + "',POTENTIAL_CATEGORY='" + model.PotentialCategory + "',HONORARIUM='" + model.Honorium + "'," +
                             "DOCTOR_NAME='" + model.DoctorName + "',DEGREE='" + model.Degree + "',DEGREE_CODE='" + model.DegreeCategory + "',DESIGNATION='" + model.Designation + "'," +
                             "DESIGNATION_CODE=" + model.DesignationCategory + ",SPECIA_1ST_CODE=" + model.SpeciFirstCode + ",SPECIA_2ND_CODE='" + model.SpeciSecondCode + "'," +
                             "GENDER='" + model.Gender + "',RELIGION='" + model.Religion + "',DATE_OF_BIRTH=(TO_DATE('" + model.DateOfBirth + "','dd/MM/yyyy')),DOC_PERS_PHONE='" + model.Phone + "'," +
                             "DOCTOR_EMAIL='" + model.Email + "',PATIENT_PER_DAY=" + model.PatientNo + ",AVG_PRESC_VALUE=" + model.PrescriptionValue + ",PRESC_SHARE=" + model.PrescriptionShare + "," +
                             "ADDRESS1='" + model.Address1 + "',ADDRESS2='" + model.Address2 + "',ADDRESS3='" + model.Address3 + "',ADDRESS4='" + model.Address4 + "',REMARKS='" + model.Remarks + "'," +
                             "UPDATED_BY='" + userid + "',UPDATED_DATE=(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL='" + GetIPAddress.LocalIPAddress() + "' Where DOCTOR_ID =" + model.DoctorId + "";
                dbHelper.CmdExecute(qry);

                long doctorMstSl = GetMstSl(model.DoctorId);
                if (doctorMstSl == 0)
                {
                    doctorMstSl = idGenerated.getMAXSL("DOC_MKT_MAS_SLNO", "DOC_MKT_MAS");
                    string query = "Insert into DOC_MKT_MAS(DOC_MKT_MAS_SLNO,DOCTOR_ID)values(" + doctorMstSl + "," + model.DoctorId + ")";
                    dbHelper.CmdExecute(query);
                }
                if (model.DoctorMarketDetailsModels != null)
                {
                    foreach (var detailModel in model.DoctorMarketDetailsModels)
                    {
                        if (detailModel.DoctorDetailSl == 0)
                        {
                            detailModel.DoctorDetailSl = idGenerated.getMAXSL("DOC_MKT_DTL_SLNO", "DOC_MKT_DTL");
                            string query1 = "Insert Into DOC_MKT_DTL(DOC_MKT_DTL_SLNO,DOC_MKT_MAS_SLNO,PRAC_MKT_CODE,SBU_UNIT,CHAMB_ADDRESS1,CHAMB_ADDRESS2,CHAMB_ADDRESS3,CHAMB_ADDRESS4,CHAMB_PHONE, " +
                                    "UPAZILA_CODE,MDP_LOC_CODE,EDP_LOC_CODE,INSTI_CODE,ENTRY_DATE,MDP_LOC_NAME,EDP_LOC_NAME) Values(" + detailModel.DoctorDetailSl + "," + doctorMstSl + ",'" + detailModel.MarketCode + "','" + detailModel.SBU_GROUP + "', " +
                                    "'" + detailModel.ChamberAddress1 + "','" + detailModel.ChamberAddress2 + "','" + detailModel.ChamberAddress3 + "','" + detailModel.ChamberAddress4 + "','" + detailModel.Phone + "', " +
                                    "'" + detailModel.UpazilaCode + "','" + detailModel.MorningLocCode + "','" + detailModel.EveningLocCode + "'," + detailModel.InstituteCode + ",(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + detailModel.MorningLocTextName + "','" + detailModel.EveningTextLocName + "')";
                            dbHelper.CmdExecute(query1);
                        }
                        else
                        {
                            if (detailModel.ChamberAddress1 == null || detailModel.ChamberAddress2 == null || detailModel.ChamberAddress3 == null || detailModel.ChamberAddress4 == null)
                            {
                                string queryChamb = "";
                                if (detailModel.ChamberAddress1 != null && detailModel.ChamberAddress2 == null && detailModel.ChamberAddress3 == null && detailModel.ChamberAddress4 == null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                else if (detailModel.ChamberAddress2 != null && detailModel.ChamberAddress1 == null && detailModel.ChamberAddress3 == null && detailModel.ChamberAddress4 == null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                           "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                           "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                else if (detailModel.ChamberAddress3 != null && detailModel.ChamberAddress2 == null && detailModel.ChamberAddress1 == null && detailModel.ChamberAddress4 == null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                else if (detailModel.ChamberAddress4 != null && detailModel.ChamberAddress2 == null && detailModel.ChamberAddress3 == null && detailModel.ChamberAddress1 == null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }

                                else if (detailModel.ChamberAddress1 == null && detailModel.ChamberAddress2 == null && detailModel.ChamberAddress3 != null && detailModel.ChamberAddress4 != null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                           "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                           "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                else if (detailModel.ChamberAddress1 == null && detailModel.ChamberAddress2 != null && detailModel.ChamberAddress3 != null && detailModel.ChamberAddress4 == null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                else if (detailModel.ChamberAddress1 != null && detailModel.ChamberAddress2 != null && detailModel.ChamberAddress3 == null && detailModel.ChamberAddress4 == null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                else if (detailModel.ChamberAddress1 != null && detailModel.ChamberAddress2 == null && detailModel.ChamberAddress3 == null && detailModel.ChamberAddress4 != null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                else if (detailModel.ChamberAddress1 != null && detailModel.ChamberAddress2 != null && detailModel.ChamberAddress3 != null && detailModel.ChamberAddress4 == null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                else if (detailModel.ChamberAddress1 != null && detailModel.ChamberAddress2 != null && detailModel.ChamberAddress3 == null && detailModel.ChamberAddress4 != null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                else if (detailModel.ChamberAddress1 != null && detailModel.ChamberAddress2 == null && detailModel.ChamberAddress3 != null && detailModel.ChamberAddress4 != null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                else if (detailModel.ChamberAddress1 == null && detailModel.ChamberAddress2 != null && detailModel.ChamberAddress3 != null && detailModel.ChamberAddress4 != null)
                                {
                                    if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else if (detailModel.MorningLocTextName != null && detailModel.EveningTextLocName == null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }

                                    else if (detailModel.MorningLocTextName == null && detailModel.EveningTextLocName != null)
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    else
                                    {
                                        queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName.Replace("'", "''") + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName.Replace("'", "''") + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                    }
                                    
                                }
                                //else if (detailModel.ChamberAddress4 != null && detailModel.ChamberAddress2 == null && detailModel.ChamberAddress3 == null && detailModel.ChamberAddress1 == null)
                                //{
                                //    queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                //          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                //          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                //}
                                //else if (detailModel.ChamberAddress4 != null && detailModel.ChamberAddress2 != null && detailModel.ChamberAddress3 != null && detailModel.ChamberAddress1 != null && detailModel.MorningLocName != null && detailModel.EveningLocName == null)
                                //{
                                //    queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                //          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                //          "MDP_LOC_CODE='" + detailModel.MorningLocCode.Replace("'", "''") + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                //}
                                //else if (detailModel.ChamberAddress4 != null && detailModel.ChamberAddress2 != null && detailModel.ChamberAddress3 != null && detailModel.ChamberAddress1 != null && detailModel.MorningLocName == null && detailModel.EveningLocName != null)
                                //{
                                //    queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                //          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                //          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode.Replace("'", "''") + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                //}
                                else
                                {
                                    queryChamb = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                          "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                          "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                }
                                
                                dbHelper.CmdExecute(queryChamb);

                            }
                            else
                            {

                                string query = "Update DOC_MKT_DTL SET SBU_UNIT= '" + detailModel.SBU_GROUP + "', PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1= '" + detailModel.ChamberAddress1.Replace("'", "''") + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2.Replace("'", "''") + "', " +
              "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3.Replace("'", "''") + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4.Replace("'", "''") + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
              "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + ",MDP_LOC_NAME='" + detailModel.MorningLocTextName + "',EDP_LOC_NAME='" + detailModel.EveningTextLocName + "' Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                                dbHelper.CmdExecute(query);
                            }

                        }
                    }
                }
                if (model.Relatives != null)
                {
                    foreach (DoctorRelativeInfo detail in model.Relatives)
                    {
                        if (CheckRelative(model.DoctorId, detail.RelativeId) == 0)
                        {
                            string query = "";
                            query = "Insert into DOCTOR_RELATIVE_INFO(RELA_DOC_ID,DOCTOR_ID,RELATION) Values('" + detail.RelativeId + "','" + model.DoctorId + "','" + detail.Relation + "')";
                            dbHelper.CmdExecute(query);
                        }
                    }
                }
                mxSl = model.DoctorId;

                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Update Successful.";
                return _vmMsg;
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
            }
            return _vmMsg;
        }
        private long GetMstSl(long doctorId)
        {
            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                string query = "Select DOC_MKT_MAS_SLNO from DOC_MKT_MAS Where DOCTOR_ID=" + doctorId + "";
                long slNo = 0;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        slNo = Convert.ToInt64(reader["DOC_MKT_MAS_SLNO"]);
                    }
                    return slNo;
                }
            }
        }
        private int CheckRelative(int doctorId, int relativeid)
        {
            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                string query = "Select Count(*) from DOCTOR_RELATIVE_INFO Where DOCTOR_ID=" + doctorId + " AND RELA_DOC_ID = " + relativeid + "";
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();

                object o = cmd.ExecuteScalar();
                int total = 0;
                if (o != null)
                {
                    total = Convert.ToInt32(o.ToString());
                }
                return total;
            }
        }

        public List<DoctorInformationModel> GetDoctorList(int doctorId, string doctorName, string degree)
        {
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();
            string query = "SELECT DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,HONORARIUM,DOCTOR_NAME,DEGREE,NVL(DEGREE_CODE,0)DEGREE_CODE," +
                          "DESIGNATION,NVL(DESIGNATION_CODE,0)DESIGNATION_CODE,NVL(SPECIA_1ST_CODE,0)SPECIA_1ST_CODE,NVL(SPECIA_2ND_CODE,0)SPECIA_2ND_CODE,GENDER,RELIGION," +
                          "TO_CHAR(DATE_OF_BIRTH,'dd/MM/yyyy')DATE_OF_BIRTH, DOC_PERS_PHONE,DOCTOR_EMAIL," +
                          "NVL(PATIENT_PER_DAY,0)PATIENT_PER_DAY,NVL(AVG_PRESC_VALUE,0)AVG_PRESC_VALUE,NVL(PRESC_SHARE,0)PRESC_SHARE,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4," +
                          "REMARKS,FSPECIALIZATION,SPECIALIZATION SSPECIALIZATION FROM " +
                //"(SELECT DOCTOR.*,SPECIALIZATION FSPECIALIZATION FROM DOCTOR, DOCTOR_SPECIALIZATION WHERE DOCTOR.SPECIA_1ST_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE) DOCTORS " +
                          "(SELECT DOCTOR.*,SPECIALIZATION FSPECIALIZATION FROM DOCTOR Left Join DOCTOR_SPECIALIZATION ON  DOCTOR.SPECIA_1ST_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE) DOCTORS " +
                          "Left Join DOCTOR_SPECIALIZATION ON DOCTORS.SPECIA_2ND_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE Where 1=1 ";
            if (doctorName != "" && degree != "")
            {
                query += " AND UPPER(DOCTOR_NAME) LIKE '%" + doctorName.ToUpper() + "%' AND UPPER(DEGREE) LIKE '%" + degree.ToUpper() + "%'";
            }
            else if (doctorName != "")
            {
                query += " AND UPPER(DOCTOR_NAME) LIKE '%" + doctorName.ToUpper() + "%'";
            }
            else if (doctorId != 0)
            {
                query += " AND DOCTOR_ID=" + doctorId + "";
            }
            else
            {
                query = query;
            }
            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.RegistrationNo = reader["REGISTRATION_NO"].ToString();
                        model.PotentialCategory = reader["POTENTIAL_CATEGORY"].ToString();
                        model.Honorium = reader["HONORARIUM"].ToString();
                        model.DoctorName = reader["DOCTOR_NAME"].ToString();
                        model.Degree = reader["DEGREE"].ToString();
                        model.DegreeCategory = Convert.ToInt32(reader["DEGREE_CODE"]);
                        model.Designation = reader["DESIGNATION"].ToString();
                        model.DesignationCategory = Convert.ToInt32(reader["DESIGNATION_CODE"]);
                        model.SpeciFirstCode = Convert.ToInt32(reader["SPECIA_1ST_CODE"]);
                        model.SpeciFirstName = reader["FSPECIALIZATION"].ToString();
                        //model.SpeciSecondCode = Convert.ToInt32(reader["SPECIA_2ND_CODE"]);
                        model.SpeciSecondCode = reader["SPECIA_2ND_CODE"].ToString();
                        model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        model.Gender = reader["GENDER"].ToString();
                        model.Religion = reader["RELIGION"].ToString();
                        model.DateOfBirth = reader["DATE_OF_BIRTH"].ToString();
                        model.Phone = reader["DOC_PERS_PHONE"].ToString();
                        model.Email = reader["DOCTOR_EMAIL"].ToString();
                        model.PatientNo = Convert.ToInt32(reader["PATIENT_PER_DAY"]);
                        model.PrescriptionValue = Convert.ToInt32(reader["AVG_PRESC_VALUE"]);
                        model.PrescriptionShare = Convert.ToInt32(reader["PRESC_SHARE"]);
                        model.Address1 = reader["ADDRESS1"].ToString();
                        model.Address2 = reader["ADDRESS2"].ToString();
                        model.Address3 = reader["ADDRESS3"].ToString();
                        model.Address4 = reader["ADDRESS4"].ToString();
                        model.Remarks = reader["REMARKS"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<DoctorRelativeInfo> GetDoctorRelativeList(int doctorId)
        {
            List<DoctorRelativeInfo> listData = new List<DoctorRelativeInfo>();
            string query = "SELECT DOCTOR_RELATIVE_INFO.*,DOCTOR_NAME FROM DOCTOR_RELATIVE_INFO,DOCTOR " +
                           "WHERE DOCTOR_RELATIVE_INFO.RELA_DOC_ID = DOCTOR.DOCTOR_ID AND DOCTOR_RELATIVE_INFO.DOCTOR_ID ='" + doctorId + "'"; ;
            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorRelativeInfo model = new DoctorRelativeInfo();
                        model.RelativeId = reader.GetInt32(0);
                        model.DocId = reader.GetInt32(1);
                        model.Relation = reader["RELATION"].ToString();
                        model.DoctorName = reader["DOCTOR_NAME"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public ValidationMsg DeleteRelative(int relativeId)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string query = "Delete from DOCTOR_RELATIVE_INFO where RELA_DOC_ID =" + relativeId + "";
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
        public List<string> GetSuggestDocList()
        {
            string Qry = "SELECT DOCTOR_NAME FROM DOCTOR ORDER BY DOCTOR_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DoctorInformationModel> items;
            items = (from DataRow row in dt.Rows
                     select new DoctorInformationModel
                     {
                         DoctorName = row["DOCTOR_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.DoctorName).ToList();
        }

        public ValidationMsg Delete(int doctorId)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string queryRelInfo = "Delete from DOCTOR_RELATIVE_INFO where DOCTOR_ID =" + doctorId + "";
                dbHelper.CmdExecute(queryRelInfo);
                string queryMktDel = "Delete from DOC_MKT_DTL where DOC_MKT_MAS_SLNO in (select DOC_MKT_MAS_SLNO from DOC_MKT_MAS where DOCTOR_ID =" + doctorId + ")";
                dbHelper.CmdExecute(queryMktDel);
                string queryMktMst = "Delete from DOC_MKT_MAS where DOCTOR_ID =" + doctorId + "";
                dbHelper.CmdExecute(queryMktMst);
                string queryDocInfo = "Delete from DOCTOR where DOCTOR_ID =" + doctorId + "";
                dbHelper.CmdExecute(queryDocInfo);

                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Data Deleted Successfully.";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-02292"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Used By Another Child.";
                }
                else
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Failed to Delete Data.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg DeleteDetail(int slNo)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string query = "Delete from DOC_MKT_DTL where DOC_MKT_DTL_SLNO=" + slNo + "";
                dbHelper.CmdExecute(query);
                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Data Deleted Successfully.";
            }
            catch (Exception ex)
            {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Failed to Delete Data.";
            }
            return _vmMsg;
        }
    }

    public class SpecializationModel
    {
        public int SpecializationCode { get; set; }
        public string Specialization { get; set; }
    }

}
