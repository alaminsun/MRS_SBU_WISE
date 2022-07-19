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
    public class DivisonDAO
    {
        DBHelper dbHelper = new DBHelper();
        string code = string.Empty;
        private ValidationMsg _vmMsg;
        public ValidationMsg Save(DivisonModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            //string firstCharacter = model.DivisonName[0].ToString();
            //string Qry = "SELECT DIVISION_CODE FROM DIVISION WHERE DIVISION_CODE LIKE '" + firstCharacter + "%'";
            //DataTable dt = dbHelper.GetDataTable(Qry);
            //List<DivisonModel> items = new List<DivisonModel>();
            //DivisonModel objModel = new DivisonModel();
            //foreach (DataRow row in dt.Rows)
            //{
            //    if (row["DIVISION_CODE"].ToString().Length == 4)
            //    {
            //        if (Regex.Matches(row["DIVISION_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
            //        {
            //            objModel.DivisonCode = row["DIVISION_CODE"].ToString();
            //            items.Add(objModel);
            //        }
            //    }
            //}
            //if (items.Count > 0)
            //{
            //    items = items.OrderByDescending(m => m.DivisonCode).ToList();
            //    var lastStringValObj = items.FirstOrDefault();
            //    string lastNum = lastStringValObj.DivisonCode.Substring(1, 3);
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
                //string qry = "INSERT INTO DIVISION (DIVISION_CODE,DIVISION_NAME,DIVI_SAP_CODE,DIVI_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //"VALUES('" + model.DivisonCode + "', '" + model.DivisonName + "', '" + model.SAPDivisonCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                string qry = "INSERT INTO DIVISION (DIVISION_CODE,DIVISION_NAME,DIVI_SAP_CODE,DIVI_ODR_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                "VALUES('" + model.DivisonCode + "', '" + model.DivisonName + "', '" + model.SAPDivisonCode + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
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
                    _vmMsg.Msg = "Divison Code already Exist.";
                }
                if (ex.Message.Contains("UK_DIVISION_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Divison Name already Exist.";
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
        public ValidationMsg Update(DivisonModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE DIVISION SET DIVISION_NAME = '" + model.DivisonName + "',DIVI_SAP_CODE = '" + model.SAPDivisonCode + "',DIVI_ODR_SLNO = '" + model.SlNo + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                             "WHERE DIVISION_CODE = '" + model.DivisonCode + "'";
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
                    _vmMsg.Msg = "Divison Code already Exist.";
                }
                if (ex.Message.Contains("UK_DIVISION_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Divison Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string DivisonCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM DIVISION WHERE DIVISION_CODE = '" + DivisonCode + "' ";
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
        public List<DivisonModel> GetAllDivisonList()
        {
            string Qry = "";
            Qry = "SELECT DIVISION_CODE ,DIVISION_NAME,DIVI_SAP_CODE,DIVI_ODR_SLNO FROM DIVISION ";
            Qry = Qry + "ORDER BY DIVISION_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DivisonModel> items;
            items = (from DataRow row in dt.Rows
                     select new DivisonModel
                     {
                         DivisonCode = row["DIVISION_CODE"].ToString(),
                         DivisonName = row["DIVISION_NAME"].ToString(),
                         SAPDivisonCode = row["DIVI_SAP_CODE"].ToString(),
                         SlNo = row["DIVI_ODR_SLNO"].ToString()
                     }).ToList();

            return items;
        }
        public List<string> GetSuggestDivisonList() 
        {
            string Qry = "SELECT DIVISION_NAME FROM DIVISION ORDER BY DIVISION_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DivisonModel> items;
            items = (from DataRow row in dt.Rows
                     select new DivisonModel
                     {
                         DivisonName = row["DIVISION_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.DivisonName).ToList();
        }
    }
}