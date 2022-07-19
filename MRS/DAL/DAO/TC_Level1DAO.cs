using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System.Text.RegularExpressions;

namespace MRS.DAL.DAO
{
    public class TC_Level1DAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<TC_LEVEL1Model> GetTherapeuticLevel1List()
        {
            string Qry = "SELECT TC_L1_CODE,TC_L1_DESC,TC_L1_SLNO FROM TC_LEVEL1 ORDER BY TC_L1_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<TC_LEVEL1Model> itemList = new List<TC_LEVEL1Model>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new TC_LEVEL1Model
                        {
                            TC_L1_CODE = row["TC_L1_CODE"].ToString(),
                            TC_L1_DESC = row["TC_L1_DESC"].ToString(),
                            TC_L1_SLNO = row["TC_L1_SLNO"].ToString()

                        }).ToList();
            return itemList;

            //OracleConnection con = new OracleConnection(dbCon.StringRead());
            //con.Open();
            //OracleCommand cmd = new OracleCommand(Qry, con);
            //OracleDataReader reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    TC_LEVEL1Model modelData = new TC_LEVEL1Model();
            //    modelData.TC_L1_CODE = reader["TC_L1_CODE"].ToString();
            //    modelData.TC_L1_DESC = reader["TC_L1_DESC"].ToString();

            //    getId.Add(modelData);
            //}
            //con.Close();
            //return getId;
        }

        public ValidationMsg Save(TC_LEVEL1Model model, string userid)
        {
            string firstCharacter = model.TC_L1_DESC[0].ToString();
            string Qry = "SELECT TC_L1_CODE FROM TC_LEVEL1 WHERE TC_L1_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<TC_LEVEL1Model> items = new List<TC_LEVEL1Model>();
            TC_LEVEL1Model objModel = new TC_LEVEL1Model();
            foreach (DataRow row in dt.Rows)
            {
                if (row["TC_L1_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["TC_L1_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.TC_L1_CODE = row["TC_L1_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.TC_L1_CODE).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.TC_L1_CODE.Substring(1, 3);
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
                //string saveQuery = "INSERT INTO TC_LEVEL1 (TC_L1_CODE,TC_L1_DESC,TC_L1_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + model.TC_L1_CODE + "','" + model.TC_L1_DESC + "','" + model.TC_L1_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                string saveQuery = "INSERT INTO TC_LEVEL1 (TC_L1_CODE,TC_L1_DESC,TC_L1_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.TC_L1_DESC + "','" + model.TC_L1_SLNO + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
        public ValidationMsg Update(TC_LEVEL1Model model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE TC_LEVEL1 SET TC_L1_CODE = '" + model.TC_L1_CODE + "',TC_L1_DESC = '" + model.TC_L1_DESC + "',TC_L1_SLNO = '" + model.TC_L1_SLNO + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE TC_L1_CODE = '" + model.TC_L1_CODE + "'";
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

        public ValidationMsg Delete(string tcL1Code)
        {
            try
            {
                string deleteQuery = "DELETE FROM TC_LEVEL1 WHERE TC_L1_CODE = '" + tcL1Code + "' ";
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
        public List<string> GetSuggestTCLevel1List()
        {
            string Qry = "SELECT TC_L1_DESC FROM TC_LEVEL1 ORDER BY TC_L1_DESC";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<TC_LEVEL1Model> items;
            items = (from DataRow row in dt.Rows
                     select new TC_LEVEL1Model
                     {
                         TC_L1_DESC = row["TC_L1_DESC"].ToString()
                     }).ToList();
            return items.Select(m => m.TC_L1_DESC).ToList();
        }
    }
}