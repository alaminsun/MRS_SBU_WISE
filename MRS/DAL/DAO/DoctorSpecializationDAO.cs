using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRS.DAL.Gateway;
using MRS.Models;
using System.Data;
using MRS.DAL.Common;
using System.Text.RegularExpressions;

namespace MRS.DAL.DAO
{
    public class DoctorSpecializationDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<DoctorSpecializationModel> GetDoctorSpecializationData()
        {
            string Qry = "SELECT SPECIALIZATION_CODE,SPECIALIZATION FROM DOCTOR_SPECIALIZATION ORDER BY SPECIALIZATION_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DoctorSpecializationModel> itemList = new List<DoctorSpecializationModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DoctorSpecializationModel
                        {
                            SPECIALIZATION_CODE = row["SPECIALIZATION_CODE"].ToString(),
                            SPECIALIZATION = row["SPECIALIZATION"].ToString()
                        }).ToList();
            return itemList;
        }

        public ValidationMsg Save(DoctorSpecializationModel model, string userid)
        {
            DataTable dt = _dbHelper.GetDataTable("SELECT SPECIALIZATION_CODE FROM DOCTOR_SPECIALIZATION ORDER BY SPECIALIZATION_CODE DESC");
            code = (Convert.ToInt16(dt.Rows[0]["SPECIALIZATION_CODE"].ToString()) + 1).ToString();

            try
            {
                //string saveQuery = "INSERT INTO DOCTOR_SPECIALIZATION (SPECIALIZATION_CODE,SPECIALIZATION,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + model.SPECIALIZATION_CODE + "','" + model.SPECIALIZATION + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                string saveQuery = "INSERT INTO DOCTOR_SPECIALIZATION (SPECIALIZATION_CODE,SPECIALIZATION,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.SPECIALIZATION + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                if (_dbHelper.CmdExecute(saveQuery) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Saved Successfully";
                }
            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed To Save Data";
                if (ex.Message.Contains("ORA-00001"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Specialization Code Already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _validationMsg;
        }
        public string GetCode()
        {
            return code;
        }
        public ValidationMsg Update(DoctorSpecializationModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE DOCTOR_SPECIALIZATION SET SPECIALIZATION_CODE = '" + model.SPECIALIZATION_CODE + "',SPECIALIZATION = '" + model.SPECIALIZATION + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE SPECIALIZATION_CODE = '" + model.SPECIALIZATION_CODE + "'";
                if (_dbHelper.CmdExecute(updateQuery) > 0)
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

        public ValidationMsg Delete(string specializationCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM DOCTOR_SPECIALIZATION WHERE SPECIALIZATION_CODE = '" + specializationCode + "' ";
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

        public List<string> GetSuggestDocSpecializationList()
        {
            string Qry = "SELECT SPECIALIZATION FROM DOCTOR_SPECIALIZATION ORDER BY SPECIALIZATION";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DoctorSpecializationModel> items;
            items = (from DataRow row in dt.Rows
                     select new DoctorSpecializationModel
                     {
                         SPECIALIZATION = row["SPECIALIZATION"].ToString()
                     }).ToList();
            return items.Select(m => m.SPECIALIZATION).ToList();
        }
    }
}