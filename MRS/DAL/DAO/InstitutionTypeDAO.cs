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
    public class InstitutionTypeDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        List<InstitutionTypeInformationModel> items;
        string code = string.Empty;

        public List<InstitutionTypeInformationModel> GetInstitutionTypeInformationData()
        {
            string Qry = "SELECT INSTI_TYPE_CODE,INSTI_TYPE_NAME,INSTI_TYPE_SHORT_NAME FROM INSTITUTION_TYPE ORDER BY INSTI_TYPE_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<InstitutionTypeInformationModel> itemList = new List<InstitutionTypeInformationModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new InstitutionTypeInformationModel
                        {
                            INSTI_TYPE_CODE = row["INSTI_TYPE_CODE"].ToString(),
                            INSTI_TYPE_NAME = row["INSTI_TYPE_NAME"].ToString(),
                            INSTI_TYPE_SHORT_NAME = row["INSTI_TYPE_SHORT_NAME"].ToString()

                        }).ToList();
            return itemList;

        }

        public ValidationMsg Save(InstitutionTypeInformationModel model, string userid)
        {
            DataTable dt = _dbHelper.GetDataTable("SELECT INSTI_TYPE_CODE FROM INSTITUTION_TYPE ORDER BY INSTI_TYPE_CODE DESC");
            code = (Convert.ToInt16(dt.Rows[0]["INSTI_TYPE_CODE"].ToString()) + 1).ToString();
            try
            {
                //string saveQuery = "INSERT INTO INSTITUTION_TYPE (INSTI_TYPE_CODE,INSTI_TYPE_NAME,INSTI_TYPE_SHORT_NAME,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + model.INSTI_TYPE_CODE + "','" + model.INSTI_TYPE_NAME + "','" + model.INSTI_TYPE_SHORT_NAME + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
                string saveQuery = "INSERT INTO INSTITUTION_TYPE (INSTI_TYPE_CODE,INSTI_TYPE_NAME,INSTI_TYPE_SHORT_NAME,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.INSTI_TYPE_NAME + "','" + model.INSTI_TYPE_SHORT_NAME + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
                    _validationMsg.Msg = "Instutution Type Code Already Exist.";
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
        public ValidationMsg Update(InstitutionTypeInformationModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE INSTITUTION_TYPE SET INSTI_TYPE_CODE = '" + model.INSTI_TYPE_CODE + "',INSTI_TYPE_NAME = '" + model.INSTI_TYPE_NAME + "',INSTI_TYPE_SHORT_NAME = '" + model.INSTI_TYPE_SHORT_NAME + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE INSTI_TYPE_CODE = '" + model.INSTI_TYPE_CODE + "'";
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


        public ValidationMsg Delete(string institutionTypeCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM INSTITUTION_TYPE WHERE INSTI_TYPE_CODE = '" + institutionTypeCode + "'";
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
        public List<string> GetSuggestInsTypeList()
        {
            string Qry = "SELECT INSTI_TYPE_NAME FROM INSTITUTION_TYPE ORDER BY INSTI_TYPE_NAME";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<InstitutionTypeInformationModel> items;
            items = (from DataRow row in dt.Rows
                     select new InstitutionTypeInformationModel
                     {
                         INSTI_TYPE_NAME = row["INSTI_TYPE_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.INSTI_TYPE_NAME).ToList();
        } 
    }
}