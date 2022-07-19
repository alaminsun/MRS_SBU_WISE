using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class UserWorkingSessionDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        public void Save(UserWorkingSessionModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "INSERT INTO DEPOT (SESSION_SLNO,ENTRY_DATE,PURCHASE_DATE)" +
                "VALUES('" + model.SessionSlno + "', (TO_DATE('" + model.EntryDate + "', (TO_DATE('" + model.PurchaseDate + "','dd/MM/yyyy')),')";
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
                    _vmMsg.Msg = "Depo Code already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            //return _vmMsg;
        }
        //public ValidationMsg Update(UserWorkingSessionModel model, string userid)
        //{
        //    _vmMsg = new ValidationMsg();
        //    try
        //    {
        //        string qry = "UPDATE DEPOT SET ENTRY_DATE = '" + model.DepoName + "',PURCHASE_DATE = '" + model.SAPDepoCode + "',DEPOT_ODR_SLNO = '" + model.SlNo + "'," +
        //                     "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + DateTime.Now.ToShortDateString() + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
        //                     "WHERE SESSION_SLNO = '" + model.SessionSlno + "'";
        //        if (dbHelper.CmdExecute(qry) > 0)
        //        {
        //            _vmMsg.Type = Enums.MessageType.Update;
        //            _vmMsg.Msg = "Updated Successfully.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _vmMsg.Type = Enums.MessageType.Error;
        //        _vmMsg.Msg = "Failed to Update.";
        //        if (ex.Message.Contains("ORA-01438"))
        //        {
        //            _vmMsg.Type = Enums.MessageType.Error;
        //            _vmMsg.Msg = "Value larger than specified precision allowed.";
        //        }
        //    }
        //    return _vmMsg;
        //}
        //public ValidationMsg Delete(string SessionSlno)
        //{
        //    var vmMsg = new ValidationMsg();
        //    try
        //    {
        //        string qry = "DELETE FROM DEPOT WHERE SESSION_SLNO = '" + SessionSlno + "' ";
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
        //    }
        //    return vmMsg;
        //}
        //public List<UserWorkingSessionModel> GetAllDepoList()
        //{
        //    string Qry = "";
        //    Qry = "SELECT SESSION_SLNO ,ENTRY_DATE,PURCHASE_DATE,DEPOT_ODR_SLNO FROM DEPOT ";
        //    Qry = Qry + "ORDER BY SESSION_SLNO";

        //    DataTable dt = dbHelper.GetDataTable(Qry);
        //    List<UserWorkingSessionModel> items;
        //    items = (from DataRow row in dt.Rows
        //             select new UserWorkingSessionModel
        //             {
        //                 SessionSlno = row["SESSION_SLNO"].ToString(),
        //                 DepoName = row["ENTRY_DATE"].ToString(),
        //                 SAPDepoCode = row["PURCHASE_DATE"].ToString(),
        //                 SlNo = row["DEPOT_ODR_SLNO"].ToString()
        //             }).ToList();

        //    return items;
        //}
    }
}