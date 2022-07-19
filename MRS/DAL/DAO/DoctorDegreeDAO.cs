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
    public class DoctorDegreeDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<DoctorDegreeModel> GetDoctorDegreeData()
        {
            string Qry = "SELECT DEGREE_CODE,DEGREE FROM DOCTOR_DEGREE ORDER BY DEGREE_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DoctorDegreeModel> itemList = new List<DoctorDegreeModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DoctorDegreeModel
                        {
                            DEGREE_CODE = row["DEGREE_CODE"].ToString(),
                            DEGREE = row["DEGREE"].ToString(),

                        }).ToList();
            return itemList;
        }

        public ValidationMsg Save(DoctorDegreeModel model, string userid)
        {
            DataTable dt = _dbHelper.GetDataTable("SELECT DEGREE_CODE FROM DOCTOR_DEGREE ORDER BY DEGREE_CODE DESC");
            code = (Convert.ToInt16(dt.Rows[0]["DEGREE_CODE"].ToString()) + 1).ToString();
            try
            {
                //string saveQuery = "INSERT INTO DOCTOR_DEGREE (DEGREE_CODE,DEGREE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + model.DEGREE_CODE + "','" + model.DEGREE + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                string saveQuery = "INSERT INTO DOCTOR_DEGREE (DEGREE_CODE,DEGREE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.DEGREE + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
                    _validationMsg.Msg = "Degree Code Already Exist.";
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
        public ValidationMsg Update(DoctorDegreeModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE DOCTOR_DEGREE SET DEGREE_CODE = '" + model.DEGREE_CODE + "',DEGREE = '" + model.DEGREE + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE DEGREE_CODE = '" + model.DEGREE_CODE + "'";
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

        public ValidationMsg Delete(string degreeCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM DOCTOR_DEGREE WHERE DEGREE_CODE = '" + degreeCode + "' ";
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

        public List<string> GetSuggestDocDegreeList()
        {
            string Qry = "SELECT DEGREE FROM DOCTOR_DEGREE ORDER BY DEGREE";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DoctorDegreeModel> items;
            items = (from DataRow row in dt.Rows
                     select new DoctorDegreeModel
                     {
                         DEGREE = row["DEGREE"].ToString()
                     }).ToList();
            return items.Select(m => m.DEGREE).ToList();
        }
    }
}