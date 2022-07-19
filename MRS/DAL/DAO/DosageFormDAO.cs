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
    public class DosageFormDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<DosageFormModel> GetDosageFormInformationData()
        {
            string Qry = "SELECT DOSAGE_FORM_CODE,DOSAGE_FORM_NAME,DF_ODR_SLNO FROM DOSAGE_FORM ORDER BY DOSAGE_FORM_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DosageFormModel> itemList = new List<DosageFormModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DosageFormModel
                        {
                            DOSAGE_FORM_CODE = row["DOSAGE_FORM_CODE"].ToString(),
                            DOSAGE_FORM_NAME = row["DOSAGE_FORM_NAME"].ToString(),
                            DF_ODR_SLNO = row["DF_ODR_SLNO"].ToString()

                        }).ToList();
            return itemList;
        }

        public ValidationMsg Save(DosageFormModel model, string userid)
        {
            string firstCharacter = model.DOSAGE_FORM_NAME[0].ToString();
            string Qry = "SELECT DOSAGE_FORM_CODE FROM DOSAGE_FORM WHERE DOSAGE_FORM_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DosageFormModel> items = new List<DosageFormModel>();
            DosageFormModel objModel = new DosageFormModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["DOSAGE_FORM_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["DOSAGE_FORM_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.DOSAGE_FORM_CODE = row["DOSAGE_FORM_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.DOSAGE_FORM_CODE).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.DOSAGE_FORM_CODE.Substring(1, 3);
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
                //string saveQuery = "INSERT INTO DOSAGE_FORM (DOSAGE_FORM_CODE,DOSAGE_FORM_NAME,DF_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + model.DOSAGE_FORM_CODE + "','" + model.DOSAGE_FORM_NAME + "','" + model.DF_ODR_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                string saveQuery = "INSERT INTO DOSAGE_FORM (DOSAGE_FORM_CODE,DOSAGE_FORM_NAME,DF_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.DOSAGE_FORM_NAME + "','" + model.DF_ODR_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
                    _validationMsg.Msg = "Dosage Form Code Already Exist.";
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
        public ValidationMsg Update(DosageFormModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE DOSAGE_FORM SET DOSAGE_FORM_CODE = '" + model.DOSAGE_FORM_CODE + "',DOSAGE_FORM_NAME = '" + model.DOSAGE_FORM_NAME + "',DF_ODR_SLNO = '" + model.DF_ODR_SLNO + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE DOSAGE_FORM_CODE = '" + model.DOSAGE_FORM_CODE + "'";
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


        public ValidationMsg Delete(string dosageFormCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM DOSAGE_FORM WHERE DOSAGE_FORM_CODE = '" + dosageFormCode + "' ";
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
        public List<string> GetSuggestDosageFormList()
        {
            string Qry = "SELECT DOSAGE_FORM_NAME FROM DOSAGE_FORM ORDER BY DOSAGE_FORM_NAME";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DosageFormModel> items;
            items = (from DataRow row in dt.Rows
                     select new DosageFormModel
                     {
                         DOSAGE_FORM_NAME = row["DOSAGE_FORM_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.DOSAGE_FORM_NAME).ToList();
        }
    }
}