using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using MRS.DAL.Gateway;
using MRS.Models;
using System.Data;
using MRS.DAL.Common;
using System.Text.RegularExpressions;

namespace MRS.DAL.DAO
{
    public class DoctorGroupDAO
    {

        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;


        public List<DoctorGroupModel> GetDoctorGroupInformationData()
        {
            string Qry = "SELECT GROUP_ID,GROUP_NAME FROM DOCTOR_GROUP ORDER BY GROUP_ID ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DoctorGroupModel> itemList = new List<DoctorGroupModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DoctorGroupModel
                        {
                            GROUP_ID = row["GROUP_ID"].ToString(),
                            GROUP_NAME = row["GROUP_NAME"].ToString()

                        }).ToList();
            return itemList;

        }

        public ValidationMsg Save(DoctorGroupModel model, string userid)
        {
            string firstCharacter = model.GROUP_NAME[0].ToString();
            string Qry = "SELECT GROUP_ID FROM DOCTOR_GROUP WHERE GROUP_ID LIKE '" + firstCharacter + "%'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DoctorGroupModel> items = new List<DoctorGroupModel>();
            DoctorGroupModel objModel = new DoctorGroupModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["GROUP_ID"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["GROUP_ID"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.GROUP_ID = row["GROUP_ID"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.GROUP_ID).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.GROUP_ID.Substring(1, 3);
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
                //string saveQuery = "INSERT INTO DOCTOR_GROUP (GROUP_ID,GROUP_NAME) VALUES('" + model.GROUP_ID + "','" + model.GROUP_NAME + "') ";
                string saveQuery = "INSERT INTO DOCTOR_GROUP (GROUP_ID,GROUP_NAME) VALUES('" + code+ "','" + model.GROUP_NAME + "') ";
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
                    _validationMsg.Msg = "Doctor Group Code Already Exist.";
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
        public ValidationMsg Update(DoctorGroupModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE DOCTOR_GROUP SET GROUP_ID = '" + model.GROUP_ID + "',GROUP_NAME = '" + model.GROUP_NAME + "'" +
                             "WHERE GROUP_ID = '" + model.GROUP_ID + "'";
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

        public ValidationMsg Delete(string doctorGroupCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM DOCTOR_GROUP WHERE GROUP_ID = '" + doctorGroupCode + "' ";
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
        public List<string> GetSuggestDocGroupList()
        {
            string Qry = "SELECT GROUP_NAME FROM DOCTOR_GROUP ORDER BY GROUP_NAME";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DoctorGroupModel> items;
            items = (from DataRow row in dt.Rows
                     select new DoctorGroupModel
                     {
                         GROUP_NAME = row["GROUP_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.GROUP_NAME).ToList();
        }
    }
}