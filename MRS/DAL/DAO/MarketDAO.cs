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
    public class MarketDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;

        public ValidationMsg Save(MarketModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            //string firstCharacter = model.MarketName[0].ToString();
            //string Qry = "SELECT MARKET_CODE FROM MARKET WHERE MARKET_CODE LIKE '" + firstCharacter + "%'";
            //DataTable dt = dbHelper.GetDataTable(Qry);
            //List<MarketModel> items = new List<MarketModel>();
            //MarketModel objModel = new MarketModel();
            //foreach (DataRow row in dt.Rows)
            //{
            //    if (row["MarketCode"].ToString().Length == 4)
            //    {
            //        if (Regex.Matches(row["MarketCode"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
            //        {
            //            objModel.MarketCode = row["MarketCode"].ToString();
            //            items.Add(objModel);
            //        }
            //    }
            //}
            //if (items.Count > 0)
            //{
            //    items = items.OrderByDescending(m => m.MarketCode).ToList();
            //    var lastStringValObj = items.FirstOrDefault();
            //    string lastNum = lastStringValObj.MarketCode.Substring(1, 3);
            //    int lastNumber = Convert.ToInt32(lastNum) + 1;
            //    if (lastNumber < 10)
            //        code = firstCharacter + "00" + lastNumber.ToString();
            //    else if (lastNumber < 100)
            //        code = firstCharacter + "0" + lastNumber.ToString();
            //}
            //else
            //    code = firstCharacter + "001";
            try
            {
                //string qry = "INSERT INTO MARKET (MARKET_CODE,MARKET_NAME,MKT_SAP_CODE,MKT_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //"VALUES('" + model.MarketCode + "', '" + model.MarketName + "', '" + model.SAPMarketCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                //string qry = "INSERT INTO MARKET (MARKET_CODE,MARKET_NAME,MKT_SAP_CODE,MKT_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //"VALUES('" + code + "', '" + model.MarketName + "', '" + model.SAPMarketCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                string qry = "INSERT INTO MARKET (MARKET_CODE,MARKET_NAME,SBU_UNIT,MKT_SAP_CODE,MKT_ODR_SLNO,MKT_TYPE_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                "VALUES('" + model.MarketCode + "', '" + model.MarketName.Replace("'", "''") + "','" + model.SBU_GROUP + "', '" + model.SAPMarketCode + "', '" + model.SlNo + "','" + model.MarketTypeCode + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
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
                    _vmMsg.Msg = "Market Code already Exist.";
                }
                if (ex.Message.Contains("UK_MARKET_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Market Name already Exist.";
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
        public ValidationMsg Update(MarketModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE MARKET SET MARKET_NAME = '" + model.MarketName.Replace("'", "''") + "',SBU_UNIT = '" + model.SBU_GROUP + "',MKT_SAP_CODE = '" + model.SAPMarketCode + "',MKT_TYPE_CODE = '" + model.MarketTypeCode + "', MKT_ODR_SLNO = '" + model.SlNo + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE MARKET_CODE = '" + model.MarketCode + "'";
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
                    _vmMsg.Msg = "Market Code already Exist.";
                }
                if (ex.Message.Contains("UK_MARKET_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Market Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string MarketCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM MARKET WHERE MARKET_CODE = '" + MarketCode + "' ";
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
        public List<MarketModel> GetAllMarketList()
        {
            string Qry = "";
           // Qry = "SELECT MARKET_CODE ,MARKET_NAME,MKT_SAP_CODE,MKT_ODR_SLNO FROM MARKET ";
            Qry = "SELECT a.*,B.MKT_TYPE_NAME FROM MARKET A Left Join MKT_TYPE B ON A.MKT_TYPE_CODE= B.MKT_TYPE_CODE";
            //Qry = " SELECT a.*,(A.MKT_TYPE_CODE ||  '||' || B.MKT_TYPE_NAME) as MarketType FROM MARKET A Left Join MKT_TYPE B ON A.MKT_TYPE_CODE= B.MKT_TYPE_CODE ";
            Qry = Qry + " ORDER BY MARKET_CODE ";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketModel
                     {
                         MarketCode = row["MARKET_CODE"].ToString(),
                         MarketName = row["MARKET_NAME"].ToString(),
                         SBU_GROUP = row["SBU_UNIT"].ToString(),
                         SAPMarketCode = row["MKT_SAP_CODE"].ToString(),
                         MarketTypeCode = row["MKT_TYPE_CODE"].ToString(),
                         MarketTypeName = row["MKT_TYPE_NAME"].ToString(),
                         SlNo = row["MKT_ODR_SLNO"].ToString()
                     }).ToList();

            return items;
        }
        public List<string> GetSuggestMarketList()
        {
            string Qry = "SELECT MARKET_NAME FROM MARKET ORDER BY MARKET_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketModel
                     {
                         MarketName = row["MARKET_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.MarketName).ToList();
        }

        public List<MarketModel> GetMarketType()
        {
            string Qry = "SELECT * From MKT_TYPE ORDER BY MKT_TYPE_CODE";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketModel
                     {
                         MarketTypeCode = row["MKT_TYPE_CODE"].ToString(),
                         MarketTypeName = row["MKT_TYPE_NAME"].ToString()
                     }).ToList();

            return items;
        }
    }
}