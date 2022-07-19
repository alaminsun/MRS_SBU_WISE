using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MRS.DAL.DAO
{
    public class DepoDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;
        public ValidationMsg Save(DepoModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string firstCharacter = model.DepoName[0].ToString();
            string Qry = "SELECT DEPOT_CODE FROM DEPOT WHERE DEPOT_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DepoModel> items = new List<DepoModel>();
            DepoModel objModel = new DepoModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["DEPOT_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["DEPOT_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.DepoCode = row["DEPOT_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.DepoCode).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.DepoCode.Substring(1, 3);
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
                //string qry = "INSERT INTO DEPOT (DEPOT_CODE,DEPOT_NAME,DEPOT_SAP_CODE,DEPOT_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //"VALUES('" + model.DepoCode + "', '" + model.DepoName + "', '" + model.SAPDepoCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                string qry = "INSERT INTO DEPOT (DEPOT_CODE,DEPOT_NAME,DEPOT_SAP_CODE,DEPOT_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                "VALUES('" + code + "', '" + model.DepoName + "', '" + model.SAPDepoCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                if (dbHelper.CmdExecute(qry) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Saved Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Depo Code already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public string GetCode()
        {
            return code;
        }
        public ValidationMsg Update(DepoModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE DEPOT SET DEPOT_NAME = '" + model.DepoName + "',DEPOT_SAP_CODE = '" + model.SAPDepoCode + "',DEPOT_ODR_SLNO = '" + model.SlNo + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE DEPOT_CODE = '" + model.DepoCode + "'";
                if (dbHelper.CmdExecute(qry) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Update;
                    _vmMsg.Msg = "Updated Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Update.";
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string DepoCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM DEPOT WHERE DEPOT_CODE = '" + DepoCode + "' ";
                if (dbHelper.CmdExecute(qry) > 0)
                {
                    vmMsg.Type = Enums.MessageType.Success;
                    vmMsg.Msg = "Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                vmMsg.Type = Enums.MessageType.Error;
                vmMsg.Msg = "Failed to Delete.";
                if (ex.Message.Contains("ORA-02292"))
                {
                    vmMsg.Type = Enums.MessageType.Error;
                    vmMsg.Msg = "Child Record Found.";
                }
            }
            return vmMsg;
        }
        public List<DepoModel> GetAllDepoList()
        {
            string Qry = "";
            Qry = "SELECT DEPOT_CODE ,DEPOT_NAME,DEPOT_SAP_CODE,DEPOT_ODR_SLNO FROM DEPOT ";
            Qry = Qry + "ORDER BY DEPOT_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DepoModel> items;
            items = (from DataRow row in dt.Rows
                     select new DepoModel
                     {
                         DepoCode = row["DEPOT_CODE"].ToString(),
                         DepoName = row["DEPOT_NAME"].ToString(),
                         SAPDepoCode = row["DEPOT_SAP_CODE"].ToString(),
                         SlNo = row["DEPOT_ODR_SLNO"].ToString()
                     }).ToList();

            return items;
        }
        public List<string> GetSuggestDepoList()
        {
            string Qry = "SELECT DEPOT_NAME FROM DEPOT ORDER BY DEPOT_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DepoModel> items;
            items = (from DataRow row in dt.Rows
                     select new DepoModel
                     {
                         DepoName = row["DEPOT_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.DepoName).ToList();
        }
    }
}