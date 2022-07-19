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
    public class IndicationInfoDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;
        public ValidationMsg Save(IndicationInfoModel model)
        {
            _vmMsg = new ValidationMsg();
            string firstCharacter = model.IndicationName[0].ToString();
            string Qry = "SELECT INDI_CODE FROM INDICATION WHERE INDI_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<IndicationInfoModel> items = new List<IndicationInfoModel>();
            IndicationInfoModel objModel = new IndicationInfoModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["INDI_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["INDI_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.IndicationCode = row["INDI_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.IndicationCode).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.IndicationCode.Substring(1, 3);
                int lastNumber = Convert.ToInt32(lastNum) + 1;
                if (lastNumber < 10)
                    code = firstCharacter + "00" + lastNumber.ToString();
                else if (lastNumber < 100)
                    code = firstCharacter + "0" + lastNumber.ToString();
                else if (lastNumber >= 100)
                    code = firstCharacter + lastNumber.ToString();

            }
            else
                code = firstCharacter + "001";
            try
            {
                //string qry = "INSERT INTO INDICATION (INDI_CODE,INDICATION_NAME)" +
                //"VALUES('" + model.IndicationCode + "', '" + model.IndicationName + "')";

                string qry = "INSERT INTO INDICATION (INDI_CODE,INDICATION_NAME)" +
                "VALUES('" + code + "', '" + model.IndicationName + "')";

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
                    _vmMsg.Msg = "Indication Code already Exist.";
                }
                if (ex.Message.Contains("UK_INDI_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Indication Name already Exist.";
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
        public ValidationMsg Update(IndicationInfoModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE INDICATION SET INDICATION_NAME = '" + model.IndicationName + "'" +
                             "WHERE INDI_CODE = '" + model.IndicationCode + "'";
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
                    _vmMsg.Msg = "Indication Code already Exist.";
                }
                if (ex.Message.Contains("UK_INDI_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Indication Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string IndicationCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM INDICATION WHERE INDI_CODE = '" + IndicationCode + "' ";
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
        public List<IndicationInfoModel> GetAllIndicationList()
        {
            string Qry = "";
            Qry = "SELECT INDI_CODE ,INDICATION_NAME FROM INDICATION ";
            Qry = Qry + "ORDER BY INDI_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<IndicationInfoModel> items;
            items = (from DataRow row in dt.Rows
                     select new IndicationInfoModel
                     {
                         IndicationCode = row["INDI_CODE"].ToString(),
                         IndicationName = row["INDICATION_NAME"].ToString()
                     }).ToList();
            return items;
        }
        public List<string> GetSuggestIndicationList()
        {
            string Qry = "SELECT INDICATION_NAME FROM INDICATION ORDER BY INDICATION_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<IndicationInfoModel> items;
            items = (from DataRow row in dt.Rows
                     select new IndicationInfoModel
                     {
                         IndicationName = row["INDICATION_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.IndicationName).ToList();
        }
    }
}