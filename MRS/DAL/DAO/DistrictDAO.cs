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
    public class DistrictDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<DistrictModel> GetDistrictList()
        {
            string Qry = "SELECT DISTC_CODE,DISTC_NAME,DISTC_ODR_SLNO FROM DISTRICT ORDER BY DISTC_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DistrictModel> itemList = new List<DistrictModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DistrictModel
                        {
                            DISTC_CODE = row["DISTC_CODE"].ToString(),
                            DISTC_NAME = row["DISTC_NAME"].ToString(),
                            DISTC_ODR_SLNO = row["DISTC_ODR_SLNO"].ToString()

                        }).ToList();
            return itemList;
        }

        public ValidationMsg Save(DistrictModel model, string userid)
        {            
            string firstCharacter = model.DISTC_NAME[0].ToString();
            string Qry = "SELECT DISTC_CODE FROM DISTRICT WHERE DISTC_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DistrictModel> items = new List<DistrictModel>();
            DistrictModel objModel = new DistrictModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["DISTC_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["DISTC_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.DISTC_CODE = row["DISTC_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.DISTC_CODE).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.DISTC_CODE.Substring(1, 3);
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
                //string saveQuery = "INSERT INTO DISTRICT (DISTC_CODE,DISTC_NAME,DISTC_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + model.DISTC_CODE + "','" + model.DISTC_NAME + "','" + model.DISTC_ODR_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                string saveQuery = "INSERT INTO DISTRICT (DISTC_CODE,DISTC_NAME,DISTC_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.DISTC_NAME + "','" + model.DISTC_ODR_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
                    _validationMsg.Msg = "District Code Already Exist.";
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
        public ValidationMsg Update(DistrictModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE DISTRICT SET DISTC_CODE = '" + model.DISTC_CODE + "',DISTC_NAME = '" + model.DISTC_NAME + "',DISTC_ODR_SLNO = '" + model.DISTC_ODR_SLNO + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE DISTC_CODE = '" + model.DISTC_CODE + "'";
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

        public ValidationMsg Delete(string districtCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM DISTRICT WHERE DISTC_CODE = '" + districtCode + "' ";
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
        public List<string> GetSuggestDistrictList()
        {
            string Qry = "SELECT DISTC_NAME FROM DISTRICT ORDER BY DISTC_NAME";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DistrictModel> items;
            items = (from DataRow row in dt.Rows
                     select new DistrictModel
                     {
                         DISTC_NAME = row["DISTC_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.DISTC_NAME).ToList();
        }
    }
}