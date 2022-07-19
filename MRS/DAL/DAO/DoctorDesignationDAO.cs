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
    public class DoctorDesignationDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<DoctorDesignationModel> GetDoctorDesignationData()
        {
            string Qry = "SELECT DESIGNATION_CODE,DESIGNATION FROM DOCTOR_DESIGNATION ORDER BY DESIGNATION_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DoctorDesignationModel> itemList = new List<DoctorDesignationModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DoctorDesignationModel
                        {
                            DESIGNATION_CODE = row["DESIGNATION_CODE"].ToString(),
                            DESIGNATION = row["DESIGNATION"].ToString()

                        }).ToList();
            return itemList;
        }

        public ValidationMsg Save(DoctorDesignationModel model, string userid)
        {
            DataTable dt = _dbHelper.GetDataTable("SELECT DESIGNATION_CODE FROM DOCTOR_DESIGNATION ORDER BY DESIGNATION_CODE DESC");
            code = (Convert.ToInt16(dt.Rows[0]["DESIGNATION_CODE"].ToString()) + 1).ToString();
            try
            {
                //string saveQuery = "INSERT INTO DOCTOR_DESIGNATION (DESIGNATION_CODE,DESIGNATION,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + model.DESIGNATION_CODE + "','" + model.DESIGNATION + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                string saveQuery = "INSERT INTO DOCTOR_DESIGNATION (DESIGNATION_CODE,DESIGNATION,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.DESIGNATION + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
                    _validationMsg.Msg = "Designation Code Already Exist.";
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
        public ValidationMsg Update(DoctorDesignationModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE DOCTOR_DESIGNATION SET DESIGNATION_CODE = '" + model.DESIGNATION_CODE + "',DESIGNATION = '" + model.DESIGNATION + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE DESIGNATION_CODE = '" + model.DESIGNATION_CODE + "'";
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

        public ValidationMsg Delete(string designationCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM DOCTOR_DESIGNATION WHERE DESIGNATION_CODE = '" + designationCode + "' ";
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
        public List<string> GetSuggestDocDesignationList()
        {
            string Qry = "SELECT DESIGNATION FROM DOCTOR_DESIGNATION ORDER BY DESIGNATION";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DoctorDesignationModel> items;
            items = (from DataRow row in dt.Rows
                     select new DoctorDesignationModel
                     {
                         DESIGNATION = row["DESIGNATION"].ToString()
                     }).ToList();
            return items.Select(m => m.DESIGNATION).ToList();
        }
    }
}