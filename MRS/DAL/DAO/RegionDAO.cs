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
    public class RegionDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;

        public ValidationMsg Save(RegionModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            //string firstCharacter = model.RegionName[0].ToString();
            //string Qry = "SELECT REGION_CODE FROM REGION WHERE REGION_CODE LIKE '" + firstCharacter + "%'";
            //DataTable dt = dbHelper.GetDataTable(Qry);
            //List<RegionModel> items = new List<RegionModel>();
            //RegionModel objModel = new RegionModel();
            //foreach (DataRow row in dt.Rows)
            //{
            //    if (row["REGION_CODE"].ToString().Length == 4)
            //    {
            //        if (Regex.Matches(row["REGION_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
            //        {
            //            objModel.RegionCode = row["REGION_CODE"].ToString();
            //            items.Add(objModel);
            //        }
            //    }
            //}
            //if (items.Count > 0)
            //{
            //    items = items.OrderByDescending(m => m.RegionCode).ToList();
            //    var lastStringValObj = items.FirstOrDefault();
            //    string lastNum = lastStringValObj.RegionCode.Substring(1, 3);
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
                //string qry = "INSERT INTO REGION (REGION_CODE,REGION_NAME,REGI_SAP_CODE,REGI_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //"VALUES('" + model.RegionCode + "', '" + model.RegionName + "', '" + model.SAPRegionCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                string qry = "INSERT INTO REGION (REGION_CODE,REGION_NAME,REGI_SAP_CODE,REGI_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                "VALUES('" + model.RegionCode + "', '" + model.RegionName.Replace("'", "''") + "', '" + model.SAPRegionCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
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
                    _vmMsg.Msg = "Region Code already Exist.";
                }
                if (ex.Message.Contains("UK_REGION_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Region Name already Exist.";
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
        public ValidationMsg Update(RegionModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE REGION SET REGION_NAME = '" + model.RegionName.Replace("'", "''") + "',REGI_SAP_CODE = '" + model.SAPRegionCode + "',REGI_ODR_SLNO = '" + model.SlNo + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE REGION_CODE = '" + model.RegionCode + "'";
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
                    _vmMsg.Msg = "Region Code already Exist.";
                }
                if (ex.Message.Contains("UK_REGION_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Region Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string RegionCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM REGION WHERE REGION_CODE = '" + RegionCode + "' ";
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

        public List<RegionModel> GetAllRegionList()
        {
            DataTable dt = dbHelper.GetDataTable("SELECT * FROM REGION ORDER BY REGION_CODE");
            List<RegionModel> items;
            items = (from DataRow row in dt.Rows
                     select new RegionModel
                     {
                         RegionCode = row["REGION_CODE"].ToString(),
                         RegionName = row["REGION_NAME"].ToString(),
                         SAPRegionCode = row["REGI_SAP_CODE"].ToString(),
                         SlNo = row["REGI_ODR_SLNO"].ToString()
                     }).ToList();

            return items;
        }
        public List<string> GetSuggestRegionList()
        {
            string Qry = "SELECT REGION_NAME FROM REGION ORDER BY REGION_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<RegionModel> items;
            items = (from DataRow row in dt.Rows
                     select new RegionModel
                     {
                         RegionName = row["REGION_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.RegionName).ToList();
        }
    }
}