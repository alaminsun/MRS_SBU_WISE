using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace MRS.DAL.DAO
{
    public class UpazilaDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<UpazilaModel> GetUpazilaList()
        {
            string Qry = "SELECT UPAZILA_CODE,UPAZILA_NAME,UPAZILA_ORD_SLNO FROM UPAZILA ORDER BY UPAZILA_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<UpazilaModel> itemList = new List<UpazilaModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new UpazilaModel
                        {
                            UPAZILA_CODE = row["UPAZILA_CODE"].ToString(),
                            UPAZILA_NAME = row["UPAZILA_NAME"].ToString(),
                            UPAZILA_ORD_SLNO = row["UPAZILA_ORD_SLNO"].ToString()

                        }).ToList();
            return itemList;
        }

        public ValidationMsg Save(UpazilaModel model, string userid)
        {
            string firstCharacter = model.UPAZILA_NAME[0].ToString();
            string Qry = "SELECT UPAZILA_CODE FROM UPAZILA WHERE UPAZILA_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<UpazilaModel> items = new List<UpazilaModel>();
            UpazilaModel objModel = new UpazilaModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["UPAZILA_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["UPAZILA_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.UPAZILA_CODE = row["UPAZILA_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.UPAZILA_CODE).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.UPAZILA_CODE.Substring(1, 3);
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
                string saveQuery = "INSERT INTO UPAZILA (UPAZILA_CODE,UPAZILA_NAME,UPAZILA_ORD_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.UPAZILA_NAME + "','" + model.UPAZILA_ORD_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
                    _validationMsg.Msg = "Upazila Code Already Exist.";
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
        public ValidationMsg Update(UpazilaModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE UPAZILA SET UPAZILA_CODE = '" + model.UPAZILA_CODE + "',UPAZILA_NAME = '" + model.UPAZILA_NAME + "',UPAZILA_ORD_SLNO = '" + model.UPAZILA_ORD_SLNO + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE UPAZILA_CODE = '" + model.UPAZILA_CODE + "'";
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

        public ValidationMsg Delete(string upazilaCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM UPAZILA WHERE UPAZILA_CODE = '" + upazilaCode + "' ";
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
        public List<string> GetSuggestUpazilaList()
        {
            string Qry = "SELECT UPAZILA_NAME FROM UPAZILA ORDER BY UPAZILA_NAME";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<UpazilaModel> items;
            items = (from DataRow row in dt.Rows
                     select new UpazilaModel
                     {
                         UPAZILA_NAME = row["UPAZILA_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.UPAZILA_NAME).ToList();
        }
    }
}