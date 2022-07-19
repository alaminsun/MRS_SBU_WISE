using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class GroupDesignationDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;
        public ValidationMsg Save(GroupDesignationModel model)
        {
            _vmMsg = new ValidationMsg();
            DataTable dt = dbHelper.GetDataTable("select NVL(MAX(GRP_DSIG_CODE),0)+1 GRP_DSIG_CODE from GRP_DSIG");
            code = dt.Rows[0]["GRP_DSIG_CODE"].ToString();
            try
            {
                string qry = "INSERT INTO GRP_DSIG (GRP_DSIG_CODE,GRP_DSIG_NAME)" +
                "VALUES('" + code + "', '" + model.GRP_DSIG_NAME + "')";

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
                    _vmMsg.Msg = "GroupDesignation Code already Exist.";
                }
                if (ex.Message.Contains("UK_INDI_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "GroupDesignation Name already Exist.";
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
        public ValidationMsg Update(GroupDesignationModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE GRP_DSIG SET GRP_DSIG_NAME = '" + model.GRP_DSIG_NAME + "'" +
                             "WHERE GRP_DSIG_CODE = '" + model.GRP_DSIG_CODE + "'";
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
                    _vmMsg.Msg = "GroupDesignation Code already Exist.";
                }
                if (ex.Message.Contains("UK_INDI_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "GroupDesignation Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string GRP_DSIG_CODE)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM GRP_DSIG WHERE GRP_DSIG_CODE = '" + GRP_DSIG_CODE + "' ";
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
        public List<GroupDesignationModel> GetAllGroupDesignationList()
        {
            string Qry = "";
            Qry = "SELECT GRP_DSIG_CODE ,GRP_DSIG_NAME FROM GRP_DSIG ";
            Qry = Qry + "ORDER BY GRP_DSIG_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<GroupDesignationModel> items;
            items = (from DataRow row in dt.Rows
                     select new GroupDesignationModel
                     {
                         GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString(),
                         GRP_DSIG_NAME = row["GRP_DSIG_NAME"].ToString()
                     }).ToList();
            return items;
        }
        public List<string> GetSuggestGroupDesignationList()
        {
            string Qry = "SELECT GRP_DSIG_NAME FROM GRP_DSIG ORDER BY GRP_DSIG_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<GroupDesignationModel> items;
            items = (from DataRow row in dt.Rows
                     select new GroupDesignationModel
                     {
                         GRP_DSIG_NAME = row["GRP_DSIG_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.GRP_DSIG_NAME).ToList();
        }
    }
}