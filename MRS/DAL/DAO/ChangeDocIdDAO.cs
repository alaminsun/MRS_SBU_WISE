using Microsoft.Office.Interop.Excel;
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
    public class ChangeDocIdDAO
    {

        private readonly DBHelper _dbHelper = new DBHelper();
        ValidationMsg _validationMsg = new ValidationMsg(); 


        //DBHelper dbHelper = new DBHelper();
        DBHelper dbHelper = new DBHelper();
        System.Data.DataTable _dataTable = new System.Data.DataTable();
        DBConnection dbConnection = new DBConnection();
        IDGenerated idGenerated = new IDGenerated();
        ValidationMsg _vmMsg;
        long mxSl = 0;
        string code = string.Empty;
        public List<ChangeDocIdModel> GetOldDoctorList(int doctorOldId, string doctorOldName, string Olddegree)
        {
            List<ChangeDocIdModel> listData = new List<ChangeDocIdModel>();
            string query = "SELECT DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,HONORARIUM,DOCTOR_NAME,DEGREE,NVL(DEGREE_CODE,0)DEGREE_CODE,DESIGNATION," +
                           " NVL(DESIGNATION_CODE,0)DESIGNATION_CODE,NVL(a.SPECIA_1ST_CODE,0)SPECIA_1ST_CODE,NVL(a.SPECIA_2ND_CODE,0)SPECIA_2ND_CODE,GENDER,RELIGION," +
                           " TO_CHAR(DATE_OF_BIRTH,'dd/MM/yyyy')DATE_OF_BIRTH, DOC_PERS_PHONE,DOCTOR_EMAIL,NVL(PATIENT_PER_DAY,0)PATIENT_PER_DAY, " +
                           " NVL(AVG_PRESC_VALUE,0)AVG_PRESC_VALUE,NVL(PRESC_SHARE,0)PRESC_SHARE,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,REMARKS," +
                           " B.SPECIALIZATION  First_SPECIALIZATION ,c.SPECIALIZATION Secound_SPECIALIZATION " +
                          "  FROM Doctor a, DOCTOR_SPECIALIZATION b,DOCTOR_SPECIALIZATION c " +
                          "  where  a.SPECIA_1ST_CODE = b.SPECIALIZATION_CODE (+)" +
                           " and a.SPECIA_2ND_CODE = c.SPECIALIZATION_CODE (+)";
            if (doctorOldName != "" && Olddegree != "")
            {
                query += " AND UPPER(DOCTOR_NAME) LIKE '%" + doctorOldName.ToUpper() + "%' AND UPPER(DEGREE) LIKE '%" + Olddegree.ToUpper() + "%'";
            }
            else if (doctorOldName != "")
            {
                query += " AND UPPER(DOCTOR_NAME) LIKE '%" + doctorOldName.ToUpper() + "%'";
            }
            else if (doctorOldId != 0)
            {
                query += " AND DOCTOR_ID=" + doctorOldId + "";
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
                        ChangeDocIdModel model = new ChangeDocIdModel();
                        //model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        //model.RegistrationNo = reader["REGISTRATION_NO"].ToString();
                        //model.PotentialCategory = reader["POTENTIAL_CATEGORY"].ToString();
                        //model.Honorium = reader["HONORARIUM"].ToString();
                        //model.DoctorName = reader["DOCTOR_NAME"].ToString();
                        //model.Degree = reader["DEGREE"].ToString();
                        //model.DegreeCategory = Convert.ToInt32(reader["DEGREE_CODE"]);
                        //model.Designation = reader["DESIGNATION"].ToString();
                        //model.DesignationCategory = Convert.ToInt32(reader["DESIGNATION_CODE"]);
                        //model.SpeciFirstCode = Convert.ToInt32(reader["SPECIA_1ST_CODE"]);
                        //model.SpeciFirstName = reader["FSPECIALIZATION"].ToString();
                        ////model.SpeciSecondCode = Convert.ToInt32(reader["SPECIA_2ND_CODE"]);
                        //model.SpeciSecondCode = reader["SPECIA_2ND_CODE"].ToString();
                        //model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        //model.Gender = reader["GENDER"].ToString();
                        //model.Religion = reader["RELIGION"].ToString();
                        //model.DateOfBirth = reader["DATE_OF_BIRTH"].ToString();
                        //model.Phone = reader["DOC_PERS_PHONE"].ToString();
                        //model.Email = reader["DOCTOR_EMAIL"].ToString();
                        //model.PatientNo = Convert.ToInt32(reader["PATIENT_PER_DAY"]);
                        //model.PrescriptionValue = Convert.ToInt32(reader["AVG_PRESC_VALUE"]);
                        //model.PrescriptionShare = Convert.ToInt32(reader["PRESC_SHARE"]);
                        //model.Address1 = reader["ADDRESS1"].ToString();
                        //model.Address2 = reader["ADDRESS2"].ToString();
                        //model.Address3 = reader["ADDRESS3"].ToString();
                        //model.Address4 = reader["ADDRESS4"].ToString();
                        //model.Remarks = reader["REMARKS"].ToString();
                        model.Old_Doc_Id = reader["DOCTOR_ID"].ToString();
                         model.Old_Honorariam = reader["HONORARIUM"].ToString();
                         model.Old_Doc_Name = reader["DOCTOR_NAME"].ToString();
                         model.Old_Degree = reader["DEGREE"].ToString();
                         //Old_Specialization = row["HONORARIUM"].ToString(),
                         model.Old_Designation = reader["DESIGNATION"].ToString();
                          model.Old_Address1 = reader["ADDRESS1"].ToString();
                         model.Old_Address2 = reader["ADDRESS2"].ToString();
                         model.Old_Address3 = reader["ADDRESS3"].ToString();
                         model.Old_Address4 = reader["ADDRESS4"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }


        public List<ChangeDocIdModel> GetNewDoctorList(int doctorNewId, string doctorNewName, string Newdegree)
        {
            List<ChangeDocIdModel> listData = new List<ChangeDocIdModel>();
            string query = "select * from doctor_information_vue ";

            if (doctorNewName != "" && Newdegree != "")
            {
                query += " Where UPPER(DOCTOR_NAME) LIKE '%" + doctorNewName.ToUpper() + "%' AND UPPER(DEGREE) LIKE '%" + Newdegree.ToUpper() + "%'";
            }
            else if (doctorNewName != "")
            {
                query += " Where UPPER(DOCTOR_NAME) LIKE '%" + doctorNewName.ToUpper() + "%'";
            }
            else if (doctorNewId != 0)
            {
                query += " Where DOCTOR_ID=" + doctorNewId + "";
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
                        ChangeDocIdModel model = new ChangeDocIdModel();
                        //model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        //model.RegistrationNo = reader["REGISTRATION_NO"].ToString();
                        //model.PotentialCategory = reader["POTENTIAL_CATEGORY"].ToString();
                        //model.Honorium = reader["HONORARIUM"].ToString();
                        //model.DoctorName = reader["DOCTOR_NAME"].ToString();
                        //model.Degree = reader["DEGREE"].ToString();
                        //model.DegreeCategory = Convert.ToInt32(reader["DEGREE_CODE"]);
                        //model.Designation = reader["DESIGNATION"].ToString();
                        //model.DesignationCategory = Convert.ToInt32(reader["DESIGNATION_CODE"]);
                        //model.SpeciFirstCode = Convert.ToInt32(reader["SPECIA_1ST_CODE"]);
                        //model.SpeciFirstName = reader["FSPECIALIZATION"].ToString();
                        ////model.SpeciSecondCode = Convert.ToInt32(reader["SPECIA_2ND_CODE"]);
                        //model.SpeciSecondCode = reader["SPECIA_2ND_CODE"].ToString();
                        //model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        //model.Gender = reader["GENDER"].ToString();
                        //model.Religion = reader["RELIGION"].ToString();
                        //model.DateOfBirth = reader["DATE_OF_BIRTH"].ToString();
                        //model.Phone = reader["DOC_PERS_PHONE"].ToString();
                        //model.Email = reader["DOCTOR_EMAIL"].ToString();
                        //model.PatientNo = Convert.ToInt32(reader["PATIENT_PER_DAY"]);
                        //model.PrescriptionValue = Convert.ToInt32(reader["AVG_PRESC_VALUE"]);
                        //model.PrescriptionShare = Convert.ToInt32(reader["PRESC_SHARE"]);
                        //model.Address1 = reader["ADDRESS1"].ToString();
                        //model.Address2 = reader["ADDRESS2"].ToString();
                        //model.Address3 = reader["ADDRESS3"].ToString();
                        //model.Address4 = reader["ADDRESS4"].ToString();
                        //model.Remarks = reader["REMARKS"].ToString();
                        model.New_Doc_Id = reader["DOCTOR_ID"].ToString();
                        //model.Old_Honorariam = reader["name"].ToString();
                        model.New_Doc_Name = reader["name"].ToString();
                        model.New_Degree = reader["DEGREE"].ToString();
                        //Old_Specialization = row["HONORARIUM"].ToString(),
                        model.New_Designation = reader["DESIGNATION"].ToString();
                        model.New_Address1 = reader["ADDRESS1"].ToString();
                        model.New_Address2 = reader["ADDRESS2"].ToString();
                        model.New_Address3 = reader["ADDRESS3"].ToString();
                        model.New_Address4 = reader["ADDRESS4"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public string GetOldPrescriptioncont(string OldId)
        {
            
            OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead());
            string Qry = "   select count(PRESC_MAS_SLNO) from prescription_master where doctor_id = '" + OldId + "'and doctor_id is not null";
            oracleConnection.Open();
            OracleCommand oracleCommand = new OracleCommand(Qry, oracleConnection);
            string i = Convert.ToString(oracleCommand.ExecuteOracleScalar());
            return i;
           
        }

        public string GetNewPrescriptioncont(string NewId)
        {
            OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead());
            string Qry = "   select count(PRESC_MAS_SLNO) from prescription_master where doctor_id = '" + NewId + "'and doctor_id is not null";
            oracleConnection.Open();
            OracleCommand oracleCommand = new OracleCommand(Qry, oracleConnection);
            string i = Convert.ToString(oracleCommand.ExecuteOracleScalar());
            return i;
        }


        public ValidationMsg Update(ChangeDocIdModel model)
        {
            try
            {
                //if (model.PROD_STATUS == "A")
                //    model.PROD_STATUS = "A";
                //else
                //    model.PROD_STATUS = "I";


                string updateQuery = "update prescription_master set doctor_id= " + model.New_Doc_Id + " where doctor_id=" + model.Old_Doc_Id + " ";
                if (_dbHelper.CmdExecute(updateQuery) >= 0)
                {
                    _validationMsg.Type = Enums.MessageType.Update;
                    _validationMsg.Msg = "Data Updated Successfully";

                }
            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed To Update Data";

                if (ex.Message.Contains("ORA-01438"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _validationMsg;
        }


        public ValidationMsg Delete(string OldId)
        {
            try
            {
                string deleteQuery = "DELETE FROM DOCTOR WHERE DOCTOR_ID = '" + OldId + "' ";
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

                if (ex.Message.Contains("ORA-02292"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Child Record Found";
                }

            }
            return _validationMsg;
        }

        public string GetCode()
        {
            return code;
        }

    }
}