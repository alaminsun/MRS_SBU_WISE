using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class ChangePasswordDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        //string code = string.Empty;
        //public ValidationMsg Save(ChangePasswordModel model, string userid)
        //{
        //    _vmMsg = new ValidationMsg();
        //    DataTable dt = dbHelper.GetDataTable("SELECT MGMT_CODE FROM MANAGEMENT_TEAM ORDER BY MGMT_CODE DESC");
        //    code = (Convert.ToInt16(dt.Rows[0]["MGMT_CODE"].ToString()) + 1).ToString();
        //    try
        //    {
        //        string qry = "INSERT INTO MANAGEMENT_TEAM (MGMT_CODE,MGMT_NAME)" +
        //        "VALUES('" + code + "', '" + model.MGMT_NAME + "')";
        //        if (dbHelper.CmdExecute(qry) > 0)
        //        {
        //            _vmMsg.Type = Enums.MessageType.Success;
        //            _vmMsg.Msg = "Saved Successfully.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _vmMsg.Type = Enums.MessageType.Error;
        //        _vmMsg.Msg = "Failed to save.";

        //        if (ex.Message.Contains("ORA-00001"))
        //        {
        //            _vmMsg.Type = Enums.MessageType.Error;
        //            _vmMsg.Msg = "ChangePassword Code already Exist.";
        //        }
        //        if (ex.Message.Contains("ORA-01438"))
        //        {
        //            _vmMsg.Type = Enums.MessageType.Error;
        //            _vmMsg.Msg = "Value larger than specified precision allowed.";
        //        }
        //    }
        //    return _vmMsg;
        //}
        //public string GetCode()
        //{
        //    return code;
        //}
        public ValidationMsg Update(ChangePasswordModel model, string userid, string Password)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                if (Password == model.OldPassword)
                {
                    string qry = "UPDATE USER_LOGIN_INFO SET PASSWORD = '" + model.Password + "',OLD_PASSWORD = '" + model.OldPassword + "'" +
                                 "WHERE USER_ID = '" + userid + "'";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        _vmMsg.Type = Enums.MessageType.Update;
                        _vmMsg.Msg = "Updated Successfully.";
                    }
                }
                else
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Wrong Old Password.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Update.";                
            }
            return _vmMsg;
        }
        //public ValidationMsg Delete(string MGMT_CODE)
        //{
        //    var vmMsg = new ValidationMsg();
        //    try
        //    {
        //        string qry = "DELETE FROM MANAGEMENT_TEAM WHERE MGMT_CODE = '" + MGMT_CODE + "' ";
        //        if (dbHelper.CmdExecute(qry) > 0)
        //        {
        //            vmMsg.Type = Enums.MessageType.Success;
        //            vmMsg.Msg = "Deleted Successfully.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vmMsg.Type = Enums.MessageType.Error;
        //        vmMsg.Msg = "Failed to Delete.";
        //        if (ex.Message.Contains("ORA-02292"))
        //        {
        //            vmMsg.Type = Enums.MessageType.Error;
        //            vmMsg.Msg = "Child Record Found.";
        //        }
        //    }
        //    return vmMsg;
        //}
        //public List<ChangePasswordModel> GetAllChangePasswordList()
        //{
        //    string Qry = "";
        //    Qry = "SELECT MGMT_CODE ,MGMT_NAME FROM MANAGEMENT_TEAM ";
        //    Qry = Qry + "ORDER BY MGMT_CODE";

        //    DataTable dt = dbHelper.GetDataTable(Qry);
        //    List<ChangePasswordModel> items;
        //    items = (from DataRow row in dt.Rows
        //             select new ChangePasswordModel
        //             {
        //                 MGMT_CODE = row["MGMT_CODE"].ToString(),
        //                 MGMT_NAME = row["MGMT_NAME"].ToString()
        //             }).ToList();

        //    return items;
        //}
        //public List<string> GetSuggestChangePasswordList()
        //{
        //    string Qry = "SELECT MGMT_NAME FROM MANAGEMENT_TEAM ORDER BY MGMT_NAME";
        //    DataTable dt = dbHelper.GetDataTable(Qry);
        //    List<ChangePasswordModel> items;
        //    items = (from DataRow row in dt.Rows
        //             select new ChangePasswordModel
        //             {
        //                 MGMT_NAME = row["MGMT_NAME"].ToString()
        //             }).ToList();
        //    return items.Select(m => m.MGMT_NAME).ToList();
        //}
    }
}