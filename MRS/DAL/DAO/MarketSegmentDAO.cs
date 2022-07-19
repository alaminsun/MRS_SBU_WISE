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
    public class MarketSegmentDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;

        public ValidationMsg Save(MarketSegmentModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string firstCharacter = model.MarketSegmentName[0].ToString();
            string Qry = "SELECT MKT_SEGMENT_CODE FROM MKT_SEGMENT WHERE MKT_SEGMENT_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketSegmentModel> items = new List<MarketSegmentModel>();
            MarketSegmentModel objModel = new MarketSegmentModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["MKT_SEGMENT_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["MKT_SEGMENT_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.MarketSegmentCode = row["MKT_SEGMENT_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.MarketSegmentCode).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.MarketSegmentCode.Substring(1, 3);
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
                //string qry = "INSERT INTO MKT_SEGMENT (MKT_SEGMENT_CODE,MKT_SEGMENT_NAME,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //"VALUES('" + model.MarketSegmentCode + "', '" + model.MarketSegmentName + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                string qry = "INSERT INTO MKT_SEGMENT (MKT_SEGMENT_CODE,MKT_SEGMENT_NAME,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                "VALUES('" + code + "', '" + model.MarketSegmentName + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
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
                    _vmMsg.Msg = "MarketSegment Code already Exist.";
                }
                if (ex.Message.Contains("UK_MKT_SEGMENT_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "MarketSegment Name already Exist.";
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
        public ValidationMsg Update(MarketSegmentModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE MKT_SEGMENT SET MKT_SEGMENT_NAME = '" + model.MarketSegmentName + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE MKT_SEGMENT_CODE = '" + model.MarketSegmentCode + "'";
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
                    _vmMsg.Msg = "MarketSegment Code already Exist.";
                }
                if (ex.Message.Contains("UK_MKT_SEGMENT_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "MarketSegment Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Delete(string MarketSegmentCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM MKT_SEGMENT WHERE MKT_SEGMENT_CODE = '" + MarketSegmentCode + "' ";
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

        public List<MarketSegmentModel> GetAllMarketSegmentList()
        {
            string Qry = "";
            Qry = "SELECT MKT_SEGMENT_CODE ,MKT_SEGMENT_NAME FROM MKT_SEGMENT ";
            Qry = Qry + "ORDER BY MKT_SEGMENT_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketSegmentModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketSegmentModel
                     {
                         MarketSegmentCode = row["MKT_SEGMENT_CODE"].ToString(),
                         MarketSegmentName = row["MKT_SEGMENT_NAME"].ToString()
                     }).ToList();

            return items;
        }
        public List<string> GetSuggestMarketSegmentList()
        {
            string Qry = "SELECT MKT_SEGMENT_NAME FROM MKT_SEGMENT ORDER BY MKT_SEGMENT_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketSegmentModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketSegmentModel
                     {
                         MarketSegmentName = row["MKT_SEGMENT_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.MarketSegmentName).ToList();
        }
    }
}