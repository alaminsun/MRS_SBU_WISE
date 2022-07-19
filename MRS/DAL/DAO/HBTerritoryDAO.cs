using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class HBTerritoryDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;

        public ValidationMsg Save(HBTerritoryModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            //string firstCharacter = model.TerritoryName[0].ToString();
            //string Qry = "SELECT TERRITORY_CODE FROM TERRITORY WHERE TERRITORY_CODE LIKE '" + firstCharacter + "%'";
            //DataTable dt = dbHelper.GetDataTable(Qry);
            //List<TerritoryModel> items = new List<TerritoryModel>();
            //TerritoryModel objModel = new TerritoryModel();
            //foreach (DataRow row in dt.Rows)
            //{
            //    if (row["TERRITORY_CODE"].ToString().Length == 4)
            //    {
            //        if (Regex.Matches(row["TERRITORY_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
            //        {
            //            objModel.TerritoryCode = row["TERRITORY_CODE"].ToString();
            //            items.Add(objModel);
            //        }
            //    }
            //}
            //if (items.Count > 0)
            //{
            //    items = items.OrderByDescending(m => m.TerritoryCode).ToList();
            //    var lastStringValObj = items.FirstOrDefault();
            //    string lastNum = lastStringValObj.TerritoryCode.Substring(1, 3);
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
                //string qry = "INSERT INTO TERRITORY (TERRITORY_CODE,TERRITORY_NAME,TERRI_SAP_CODE,TERRI_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //"VALUES('" + model.TerritoryCode + "', '" + model.TerritoryName + "', '" + model.SAPTerritoryCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                //string qry = "INSERT INTO TERRITORY (TERRITORY_CODE,TERRITORY_NAME,TERRI_SAP_CODE,TERRI_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //"VALUES('" + code + "', '" + model.TerritoryName + "', '" + model.SAPTerritoryCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                string qry = "INSERT INTO HBTERRITOTYINFO (HBTERRITORY_CODE,HBTERRITORY_NAME,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
               "VALUES('" + model.HBTerritoryCode + "', '" + model.HBTerritoryName + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
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
                    _vmMsg.Msg = "Territory Code already Exist.";
                }
                if (ex.Message.Contains("UK_TERRITORY_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Territory Code already Exist.";
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
        public ValidationMsg Update(HBTerritoryModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE HBTERRITOTYINFO SET HBTERRITORY_NAME = '" + model.HBTerritoryName + "',UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy'))" +
                             "WHERE HBTERRITORY_CODE = '" + model.HBTerritoryCode + "'";
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
        public ValidationMsg Delete(string HBTerritoryCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM HBTERRITOTYINFO WHERE HBTERRITORY_CODE = '" + HBTerritoryCode + "' ";
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
        public List<HBTerritoryModel> GetAllHBTerritoryList()
        {
            string Qry = "";
            Qry = "SELECT HBTERRITORY_CODE ,HBTERRITORY_NAME FROM HBTERRITOTYINFO ";
            Qry = Qry + "ORDER BY HBTERRITORY_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<HBTerritoryModel> items;
            items = (from DataRow row in dt.Rows
                     select new HBTerritoryModel
                     {
                         HBTerritoryCode = row["HBTERRITORY_CODE"].ToString(),
                         HBTerritoryName = row["HBTERRITORY_NAME"].ToString()
                     }).ToList();
            return items;
        }
        public List<string> GetSuggestHBTerritoryList()
        {
            string Qry = "SELECT HBTERRITORY_NAME FROM HBTERRITOTYINFO ORDER BY HBTERRITORY_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<HBTerritoryModel> items;
            items = (from DataRow row in dt.Rows
                     select new HBTerritoryModel
                     {
                         HBTerritoryName = row["HBTERRITORY_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.HBTerritoryName).ToList();
        }
    }
}