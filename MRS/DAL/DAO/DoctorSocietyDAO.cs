using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class DoctorSocietyDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;
        public ValidationMsg Save(DoctorSocietyModel model)
        {
            _vmMsg = new ValidationMsg();
            DataTable dt = dbHelper.GetDataTable("select NVL(MAX(SOCIETY_ID),0)+1 SOCIETY_ID from DOCTOR_SOCITY");
            code = dt.Rows[0]["SOCIETY_ID"].ToString();
            try
            {
                string qry = "INSERT INTO DOCTOR_SOCITY (SOCIETY_ID,SOCITY_NAME)" +
                "VALUES('" + code + "', '" + model.SOCITY_NAME + "')";

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
                    _vmMsg.Msg = "DoctorSociety Code already Exist.";
                }
                if (ex.Message.Contains("UK_INDI_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "DoctorSociety Name already Exist.";
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
        public ValidationMsg Update(DoctorSocietyModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE DOCTOR_SOCITY SET SOCITY_NAME = '" + model.SOCITY_NAME + "'" +
                             "WHERE SOCIETY_ID = '" + model.SOCIETY_ID + "'";
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
                    _vmMsg.Msg = "DoctorSociety Code already Exist.";
                }
                if (ex.Message.Contains("UK_INDI_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "DoctorSociety Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string SOCIETY_ID)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM DOCTOR_SOCITY WHERE SOCIETY_ID = '" + SOCIETY_ID + "' ";
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
        public List<DoctorSocietyModel> GetAllDoctorSocietyList()
        {
            string Qry = "";
            Qry = "SELECT SOCIETY_ID ,SOCITY_NAME FROM DOCTOR_SOCITY ";
            Qry = Qry + "ORDER BY SOCIETY_ID";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DoctorSocietyModel> items;
            items = (from DataRow row in dt.Rows
                     select new DoctorSocietyModel
                     {
                         SOCIETY_ID = row["SOCIETY_ID"].ToString(),
                         SOCITY_NAME = row["SOCITY_NAME"].ToString()
                     }).ToList();
            return items;
        }
        public List<string> GetSuggestDoctorSocietyList()
        {
            string Qry = "SELECT SOCITY_NAME FROM DOCTOR_SOCITY ORDER BY SOCITY_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<DoctorSocietyModel> items;
            items = (from DataRow row in dt.Rows
                     select new DoctorSocietyModel
                     {
                         SOCITY_NAME = row["SOCITY_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.SOCITY_NAME).ToList();
        }
    }
}