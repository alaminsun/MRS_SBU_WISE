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
    public class ManufacturerInfoDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;

        public ValidationMsg Save(ManufacturerInfoModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string firstCharacter = model.ManufacturerName[0].ToString();
            string Qry = "SELECT MANUFACTURER_CODE FROM MANUFACTURER WHERE MANUFACTURER_CODE LIKE '" + firstCharacter + "%'";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ManufacturerInfoModel> items = new List<ManufacturerInfoModel>();
            ManufacturerInfoModel objModel = new ManufacturerInfoModel();
            foreach (DataRow row in dt.Rows)
            {
                if (row["MANUFACTURER_CODE"].ToString().Length == 4)
                {
                    if (Regex.Matches(row["MANUFACTURER_CODE"].ToString().Substring(1, 3), @"[a-zA-Z]").Count == 0)
                    {
                        objModel.ManufacturerCode = row["MANUFACTURER_CODE"].ToString();
                        items.Add(objModel);
                    }
                }
            }
            if (items.Count > 0)
            {
                items = items.OrderByDescending(m => m.ManufacturerCode).ToList();
                var lastStringValObj = items.FirstOrDefault();
                string lastNum = lastStringValObj.ManufacturerCode.Substring(1, 3);
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
                if (model.Status == "Active")
                    model.Status = "A";
                else
                    model.Status = "I";
                //string qry = "INSERT INTO MANUFACTURER (MANUFACTURER_CODE,MANUFACTURER_NAME,MFC_NIC_NAME,MFC_ORDER_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL,STATUS)" +
                //"VALUES('" + model.ManufacturerCode + "', '" + model.ManufacturerName + "', '" + model.MfcNicName + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "','" + model.Status + "')";
                string qry = "INSERT INTO MANUFACTURER (MANUFACTURER_CODE,MANUFACTURER_NAME,MFC_NIC_NAME,MFC_ORDER_SLNO,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL,STATUS)" +
                "VALUES('" + code + "', '" + model.ManufacturerName + "', '" + model.MfcNicName + "', '" + model.SlNo + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "','" + model.Status + "')";
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
                    _vmMsg.Msg = "Manufacturer Code already Exist.";
                }
                if (ex.Message.Contains("UK_MANUFACTURER_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Manufacturer Name already Exist.";
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
        public ValidationMsg Update(ManufacturerInfoModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                if (model.Status == "Active")
                    model.Status = "A";
                else
                    model.Status = "I";
                string qry = "UPDATE MANUFACTURER SET MANUFACTURER_NAME = '" + model.ManufacturerName + "',MFC_NIC_NAME = '" + model.MfcNicName + "',MFC_ORDER_SLNO = '" + model.SlNo + "'," +
                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "',STATUS = '" + model.Status + "'" +
                             "WHERE MANUFACTURER_CODE = '" + model.ManufacturerCode + "'";
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
                    _vmMsg.Msg = "Manufacturer Code already Exist.";
                }
                if (ex.Message.Contains("UK_MANUFACTURER_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Manufacturer Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string ManufacturerCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM MANUFACTURER WHERE MANUFACTURER_CODE = '" + ManufacturerCode + "' ";
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
        public List<ManufacturerInfoModel> GetAllManufacturerList()
        {
            string Qry = "";
            //Qry = "SELECT MANUFACTURER_CODE ,MANUFACTURER_NAME,MFC_NIC_NAME,MFC_ORDER_SLNO FROM MANUFACTURER ";
            //Qry = "SELECT * FROM MANUFACTURER WHERE STATUS ='A'";
            Qry = "SELECT * FROM MANUFACTURER ";
            Qry = Qry + "ORDER BY MANUFACTURER_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ManufacturerInfoModel> items;
            items = (from DataRow row in dt.Rows
                     select new ManufacturerInfoModel
                     {
                         ManufacturerCode = row["MANUFACTURER_CODE"].ToString(),
                         ManufacturerName = row["MANUFACTURER_NAME"].ToString(),
                         MfcNicName = row["MFC_NIC_NAME"].ToString(),
                         SlNo = row["MFC_ORDER_SLNO"].ToString(),
                         Status = row["STATUS"].ToString() == "A" ? "Active" : "Inactive"
                     }).ToList();

            return items;
        }

        public List<string> GetSuggestManufacturerList()
        {
            string Qry = "SELECT MANUFACTURER_NAME FROM MANUFACTURER ORDER BY MANUFACTURER_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ManufacturerInfoModel> items;
            items = (from DataRow row in dt.Rows
                     select new ManufacturerInfoModel
                     {
                         ManufacturerName = row["MANUFACTURER_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.ManufacturerName).ToList();
        }
    }
}