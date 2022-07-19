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
    public class MarketTypeDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;

        public ValidationMsg Save(MarketTypeModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string firstCharacter = model.MarketTypeName[0].ToString();
            string Qry = "SELECT MKT_TYPE_CODE FROM MKT_TYPE WHERE MKT_TYPE_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketTypeModel> items = new List<MarketTypeModel>();
            MarketTypeModel objModel = new MarketTypeModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["MKT_TYPE_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["MKT_TYPE_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.MarketTypeCode = row["MKT_TYPE_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.MarketTypeCode).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.MarketTypeCode.Substring(1, 3);
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
                //string qry = "INSERT INTO MKT_TYPE (MKT_TYPE_CODE,MKT_TYPE_NAME,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //"VALUES('" + model.MarketTypeCode + "', '" + model.MarketTypeName + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                string qry = "INSERT INTO MKT_TYPE (MKT_TYPE_CODE,MKT_TYPE_NAME,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                "VALUES('" + code + "', '" + model.MarketTypeName + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
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
                    _vmMsg.Msg = "MarketType Code already Exist.";
                }
                if (ex.Message.Contains("UK_MKT_TYPE_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "MarketType Name already Exist.";
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
        public ValidationMsg Update(MarketTypeModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE MKT_TYPE SET MKT_TYPE_NAME = '" + model.MarketTypeName + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE MKT_TYPE_CODE = '" + model.MarketTypeCode + "'";
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
                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "MarketType Code already Exist.";
                }
                if (ex.Message.Contains("UK_MKT_TYPE_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "MarketType Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Delete(string MarketTypeCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM MKT_TYPE WHERE MKT_TYPE_CODE = '" + MarketTypeCode + "' ";
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

        public List<MarketTypeModel> GetAllMarketTypeList()
        {
            string Qry = "";
            Qry = "SELECT MKT_TYPE_CODE ,MKT_TYPE_NAME FROM MKT_TYPE ";
            Qry = Qry + "ORDER BY MKT_TYPE_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketTypeModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketTypeModel
                     {
                         MarketTypeCode = row["MKT_TYPE_CODE"].ToString(),
                         MarketTypeName = row["MKT_TYPE_NAME"].ToString()
                     }).ToList();

            return items;
        }
        public List<string> GetSuggestMarketTypeList()
        {
            string Qry = "SELECT MKT_TYPE_NAME FROM MKT_TYPE ORDER BY MKT_TYPE_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketTypeModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketTypeModel
                     {
                         MarketTypeName = row["MKT_TYPE_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.MarketTypeName).ToList();
        }
    }
}