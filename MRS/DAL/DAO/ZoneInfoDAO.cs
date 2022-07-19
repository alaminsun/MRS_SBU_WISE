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
    public class ZoneInfoDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;
        public ValidationMsg Save(ZoneInfoModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string firstCharacter = model.ZoneName[0].ToString();
            string Qry = "SELECT ZONE_CODE FROM ZONE WHERE ZONE_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ZoneInfoModel> items = new List<ZoneInfoModel>();
            ZoneInfoModel objModel = new ZoneInfoModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["ZONE_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["ZONE_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.ZoneCode = row["ZONE_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.ZoneCode).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.ZoneCode.Substring(1, 3);
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
                //model.CurrentDate;
                string cure = model.CurrentDate;
                string qry = "INSERT INTO ZONE (ZONE_CODE,ZONE_NAME,ZONE_SAP_CODE,ZONE_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                "VALUES('" + code + "', '" + model.ZoneName + "', '" + model.SAPZoneCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";

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
                    _vmMsg.Msg = "Zone Code already Exist.";
                }
                if (ex.Message.Contains("UK_ZONE_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Zone Name already Exist.";
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
        public ValidationMsg Update(ZoneInfoModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE ZONE SET ZONE_NAME = '" + model.ZoneName + "',ZONE_SAP_CODE = '" + model.SAPZoneCode + "',ZONE_ODR_SLNO = '" + model.SlNo + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE ZONE_CODE = '" + model.ZoneCode + "'";
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
                    _vmMsg.Msg = "Zone Code already Exist.";
                }
                if (ex.Message.Contains("UK_ZONE_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Zone Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string ZoneCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM ZONE WHERE ZONE_CODE = '" + ZoneCode + "' ";
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
        public List<ZoneInfoModel> GetAllZoneList()
        {
            string Qry = "";
            Qry = "SELECT ZONE_CODE ,ZONE_NAME,ZONE_SAP_CODE,ZONE_ODR_SLNO FROM ZONE ";
            Qry = Qry + "ORDER BY ZONE_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ZoneInfoModel> items;
            items = (from DataRow row in dt.Rows
                     select new ZoneInfoModel
                     {
                         ZoneCode = row["ZONE_CODE"].ToString(),
                         ZoneName = row["ZONE_NAME"].ToString(),
                         SAPZoneCode = row["ZONE_SAP_CODE"].ToString(),
                         SlNo = row["ZONE_ODR_SLNO"].ToString()
                     }).ToList();

            return items;
        }
        public List<string> GetSuggestZoneList()
        {
            string Qry = "SELECT ZONE_NAME FROM ZONE ORDER BY ZONE_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ZoneInfoModel> items;
            items = (from DataRow row in dt.Rows
                     select new ZoneInfoModel
                     {
                         ZoneName = row["ZONE_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.ZoneName).ToList();
        }
    }
}