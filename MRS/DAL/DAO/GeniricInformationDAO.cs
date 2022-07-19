using System;
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
    public class GeniricInformationDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<GenericInformationModel> GetGenericInformationData()
        {
            string Qry = "SELECT GENERIC_CODE,GENERIC_NAME,GEN_ORD_SLNO FROM GENERIC ORDER BY GENERIC_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<GenericInformationModel> itemList = new List<GenericInformationModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new GenericInformationModel
                        {
                            GENERIC_CODE = row["GENERIC_CODE"].ToString(),
                            GENERIC_NAME = row["GENERIC_NAME"].ToString(),
                            GEN_ORD_SLNO = row["GEN_ORD_SLNO"].ToString()
                        }).ToList();
            return itemList;
        }

        public ValidationMsg Save(GenericInformationModel model, string userid)
        {
            string firstCharacter = model.GENERIC_NAME[0].ToString();
            string Qry = "SELECT GENERIC_CODE FROM GENERIC WHERE GENERIC_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<GenericInformationModel> items = new List<GenericInformationModel>();
            GenericInformationModel objModel = new GenericInformationModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["GENERIC_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["GENERIC_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.GENERIC_CODE = row["GENERIC_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.GENERIC_CODE).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.GENERIC_CODE.Substring(1, 3);
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
                //string saveQuery = "INSERT INTO GENERIC (GENERIC_CODE,GENERIC_NAME,GEN_ORD_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + model.GENERIC_CODE + "','" + model.GENERIC_NAME + "','" + model.GEN_ORD_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                string saveQuery = "INSERT INTO GENERIC (GENERIC_CODE,GENERIC_NAME,GEN_ORD_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.GENERIC_NAME + "','" + model.GEN_ORD_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
                    _validationMsg.Msg = "Generic Code Already Exist.";
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
        public ValidationMsg Update(GenericInformationModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE GENERIC SET GENERIC_CODE = '" + model.GENERIC_CODE + "',GENERIC_NAME = '" + model.GENERIC_NAME + "',GEN_ORD_SLNO = '" + model.GEN_ORD_SLNO + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE GENERIC_CODE = '" + model.GENERIC_CODE + "'";
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

        public ValidationMsg Delete(string genericCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM GENERIC WHERE GENERIC_CODE = '" + genericCode + "' ";
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
        public List<string> GetSuggestGenericList()
        {
            string Qry = "SELECT GENERIC_NAME FROM GENERIC ORDER BY GENERIC_NAME";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<GenericInformationModel> items;
            items = (from DataRow row in dt.Rows
                     select new GenericInformationModel
                     {
                         GENERIC_NAME = row["GENERIC_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.GENERIC_NAME).ToList();
        }
    }
}