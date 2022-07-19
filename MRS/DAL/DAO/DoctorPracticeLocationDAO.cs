using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Web;

namespace MRS.DAL.DAO
{
    public class DoctorPracticeLocationDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        List<DoctorPracticeLocationModel> items;
        string code = string.Empty;


        public List<DoctorPracticeLocationModel> GetDoctorPracticeLocationData()
        {
            string Qry = "SELECT DP_LOC_CODE,DP_LOC_NAME FROM DOC_PRACTICE_LOC ORDER BY DP_LOC_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DoctorPracticeLocationModel> itemList = new List<DoctorPracticeLocationModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DoctorPracticeLocationModel
                        {
                            DP_LOC_CODE = row["DP_LOC_CODE"].ToString(),
                            DP_LOC_NAME = row["DP_LOC_NAME"].ToString()
                        }).ToList();
            return itemList;

        }

        public ValidationMsg Save(DoctorPracticeLocationModel model, string userid)
        {
            string firstCharacter = model.DP_LOC_NAME[0].ToString();
            string Qry = "SELECT DP_LOC_CODE FROM DOC_PRACTICE_LOC WHERE DP_LOC_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DoctorPracticeLocationModel> items = new List<DoctorPracticeLocationModel>();
            DoctorPracticeLocationModel objModel = new DoctorPracticeLocationModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["DP_LOC_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["DP_LOC_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.DP_LOC_CODE = row["DP_LOC_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.DP_LOC_CODE).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.DP_LOC_CODE.Substring(1, 3);
                int lastNumber = Convert.ToInt32(lastNum) + 1;
                if (lastNumber < 10)
                    code = firstCharacter + "00" + lastNumber.ToString();
                else if (lastNumber < 100)
                    code = firstCharacter + "0" + lastNumber.ToString();
            }
            else
                code = firstCharacter + "001";
            try
            {
                string saveQuery = "INSERT INTO DOC_PRACTICE_LOC (DP_LOC_CODE,DP_LOC_NAME,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.DP_LOC_NAME + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
                    _validationMsg.Msg = "Doctor Practice Location Code Already Exist.";
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
        public ValidationMsg Update(DoctorPracticeLocationModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE DOC_PRACTICE_LOC SET DP_LOC_CODE = '" + model.DP_LOC_CODE + "',DP_LOC_NAME = '" + model.DP_LOC_NAME + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE DP_LOC_CODE = '" + model.DP_LOC_CODE + "'";
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

        public ValidationMsg Delete(string doctorPracticeLocCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM DOC_PRACTICE_LOC WHERE DP_LOC_CODE = '" + doctorPracticeLocCode + "' ";
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

        public List<string> GetSuggestDocPracLocationList()
        {
            string Qry = "SELECT DP_LOC_NAME FROM DOC_PRACTICE_LOC ORDER BY DP_LOC_NAME";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DoctorPracticeLocationModel> items;
            items = (from DataRow row in dt.Rows
                     select new DoctorPracticeLocationModel
                     {
                         DP_LOC_NAME = row["DP_LOC_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.DP_LOC_NAME).ToList();
        }


    }
}