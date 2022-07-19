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
    public class DosageStrengthDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<DosageStrengthModel> GetDosageStrengthInformation()
        {
            string Qry = "SELECT DSG_STRENGTH_CODE,DSG_STRENGTH_NAME,DF_STRENGTH_SLNO FROM DOSAGE_STRENGTH ORDER BY DSG_STRENGTH_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DosageStrengthModel> itemList = new List<DosageStrengthModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DosageStrengthModel
                        {
                            DSG_STRENGTH_CODE = row["DSG_STRENGTH_CODE"].ToString(),
                            DSG_STRENGTH_NAME = row["DSG_STRENGTH_NAME"].ToString(),
                            DF_STRENGTH_SLNO = row["DF_STRENGTH_SLNO"].ToString()
                        }).ToList();

            return itemList;

        }

        public ValidationMsg Save(DosageStrengthModel model, string userid)
        {
            string firstCharacter = model.DSG_STRENGTH_NAME[0].ToString();
            string Qry = "SELECT DSG_STRENGTH_CODE FROM DOSAGE_STRENGTH WHERE DSG_STRENGTH_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DosageStrengthModel> items = new List<DosageStrengthModel>();
            DosageStrengthModel objModel = new DosageStrengthModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["DSG_STRENGTH_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["DSG_STRENGTH_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.DSG_STRENGTH_CODE = row["DSG_STRENGTH_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.DSG_STRENGTH_CODE).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.DSG_STRENGTH_CODE.Substring(1, 3);
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
                //string saveQuery = "INSERT INTO DOSAGE_STRENGTH (DSG_STRENGTH_CODE,DSG_STRENGTH_NAME,DF_STRENGTH_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + model.DSG_STRENGTH_CODE + "','" + model.DSG_STRENGTH_NAME + "','" + model.DF_STRENGTH_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                string saveQuery = "INSERT INTO DOSAGE_STRENGTH (DSG_STRENGTH_CODE,DSG_STRENGTH_NAME,DF_STRENGTH_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.DSG_STRENGTH_NAME + "','" + model.DF_STRENGTH_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
                    _validationMsg.Msg = "Therapeutic Class Level-1 Code Already Exist.";
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
        public ValidationMsg Update(DosageStrengthModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE DOSAGE_STRENGTH SET DSG_STRENGTH_CODE = '" + model.DSG_STRENGTH_CODE + "',DSG_STRENGTH_NAME = '" + model.DSG_STRENGTH_NAME + "',DF_STRENGTH_SLNO = '" + model.DF_STRENGTH_SLNO + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE DSG_STRENGTH_CODE = '" + model.DSG_STRENGTH_CODE + "'";
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

        public ValidationMsg Delete(string dosageStrengthCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM DOSAGE_STRENGTH WHERE DSG_STRENGTH_CODE = '" + dosageStrengthCode + "' ";
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
        public List<string> GetSuggestDosageStrengthList()
        {
            string Qry = "SELECT DSG_STRENGTH_NAME FROM DOSAGE_STRENGTH ORDER BY DSG_STRENGTH_NAME";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DosageStrengthModel> items;
            items = (from DataRow row in dt.Rows
                     select new DosageStrengthModel
                     {
                         DSG_STRENGTH_NAME = row["DSG_STRENGTH_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.DSG_STRENGTH_NAME).ToList();
        }
    }
}