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
    public class SCP_InfoDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        IDGenerated idGenerated = new IDGenerated();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<SCP_INFOModel> GetSCPInfoList()
        {
            string Qry = "SELECT SCP_CODE,SCP_NAME FROM SCP_INFO ORDER BY SCP_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<SCP_INFOModel> itemList = new List<SCP_INFOModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new SCP_INFOModel
                        {
                            SCP_CODE = row["SCP_CODE"].ToString(),
                            SCP_NAME = row["SCP_NAME"].ToString(),
                        }).ToList();
            return itemList;
        }

        public ValidationMsg Save(SCP_INFOModel model, string userid)
        {
            List<SCP_INFOModel> items = new List<SCP_INFOModel>();
            SCP_INFOModel objModel = new SCP_INFOModel();
            string cd = "";
    
            cd = idGenerated.getMAXSL("SCP_CODE", "SCP_INFO").ToString();

            if (cd.Length == 1)
            {
                code = "0000" + cd;
            }
            else if (cd.Length == 2)
            {
                code = "000" + cd;
            }
            else if (cd.Length == 3)
            {
                code = "00" + cd;
            }
            else if (cd.Length == 4)
            {
                code = "0" + cd;
            }
            else
            {
                code = cd;
            }

            try
            {
                string saveQuery = "INSERT INTO SCP_INFO (SCP_CODE,SCP_NAME,ENTERED_BY,ENTRY_DATE,WORK_STATION) VALUES('" + code + "','" + model.SCP_NAME + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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

        public ValidationMsg Update(SCP_INFOModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE SCP_INFO SET SCP_CODE = '" + model.SCP_CODE + "',SCP_NAME = '" + model.SCP_NAME + "',SCP_PURPOSE = '" + model.SCP_PURPOSE + "'," +
                             "MODIFIED_BY = '" + userid + "',MODIFIED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),WORK_STATION = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE SCP_CODE = '" + model.SCP_CODE + "'";
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

        public ValidationMsg Delete(string scpCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM SCP_INFO WHERE SCP_CODE = '" + scpCode + "' ";
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
    }
}