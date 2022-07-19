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
    public class SelectedGenericDAO
    {
        DBHelper dbHelper = new DBHelper();
        List<SelectedGenericModel> items;
        private ValidationMsg _vmMsg;

        public List<SelectedGenericModel> GetAllSelectedGenericList()
        {
            string Qry = "";
            Qry = "SELECT GENERIC_CODE FROM GENERIC_SELECTED ";
            Qry = Qry + "ORDER BY GENERIC_CODE";
            DataTable dt = dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string Qry1 = "";
                    Qry1 = "SELECT SG.GENERIC_CODE ,G.GENERIC_NAME FROM GENERIC_SELECTED SG INNER JOIN GENERIC G ON G.GENERIC_CODE = SG.GENERIC_CODE";
                    Qry1 = Qry1 + " ORDER BY SG.GENERIC_CODE";

                    DataTable dt1 = dbHelper.GetDataTable(Qry1);
                    items = (from DataRow row1 in dt1.Rows
                             select new SelectedGenericModel
                             {
                                 GENERIC_CODE = row1["GENERIC_CODE"].ToString(),
                                 GENERIC_NAME = row1["GENERIC_NAME"].ToString()
                             }).ToList();
                }
            }
            return items;
        }


        public ValidationMsg Save(SelectedGenericModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM GENERIC_SELECTED";
                dbHelper.CmdExecute(qry);
                foreach (var detailModel in model.SelectedGenericList)
                {
                    string qry1 = "INSERT INTO GENERIC_SELECTED (GENERIC_CODE)" +
                                  "VALUES('" + detailModel.GENERIC_CODE + "')";
                    dbHelper.CmdExecute(qry1);
                }
                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Saved Successfully.";
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to save.";
            }
            return _vmMsg;
        }


        public ValidationMsg Update(SelectedGenericModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE GENERIC_SELECTED SET GENERIC_CODE = '" + model.GENERIC_CODE + "'" +
                             " WHERE GENERIC_CODE = '" + model.GENERIC_CODE + "'";
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


        public ValidationMsg Delete(string genericCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM GENERIC_SELECTED WHERE GENERIC_CODE = '" + genericCode + "' ";
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
            }
            return vmMsg;
        }

        public List<SelectedGenericModel> GetSelectedGenericList(string selectGenericList)
        {
            string Qry = "SELECT * FROM GENERIC WHERE GENERIC_CODE in (" + selectGenericList + ")";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<SelectedGenericModel> items;
            items = (from DataRow row in dt.Rows
                     select new SelectedGenericModel
                     {
                         GENERIC_CODE = row["GENERIC_CODE"].ToString(),
                         GENERIC_NAME = row["GENERIC_NAME"].ToString()
                     }).ToList();

            return items;
        }
    }
}