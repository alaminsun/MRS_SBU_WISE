using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class SelectedTCLevel2InfoDAO
    {
        DBHelper dbHelper = new DBHelper();

        private ValidationMsg _vmMsg;
        public ValidationMsg Save(TC_LEVEL2Model model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                DataTable dt = dbHelper.GetDataTable("select * from TC_SELE_LEVEL2");
                if (dt.Rows.Count < 17)
                {
                    string qry = "INSERT INTO TC_SELE_LEVEL2 (TC_L2_CODE)" +
                    "VALUES('" + model.TC_L2_CODE + "')";

                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        _vmMsg.Type = Enums.MessageType.Success;
                        _vmMsg.Msg = "Saved Successfully.";
                    }
                }
                else
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Sorry,You cant not insert more than 17 Manufacturer.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("MRS.PK_SEL_MANUFACTURER_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Manufacturer Code already Exist.";
                }

                if (ex.Message.Contains("ORA-12899"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value too large for Sl No(Maximum: 2 Digits).";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string TC_L2_CODE)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM TC_SELE_LEVEL2 WHERE TC_L2_CODE = '" + TC_L2_CODE + "' ";
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
        public List<TC_LEVEL2Model> GetAllSelectedTCLevel2List()
        {
            var items = new List<TC_LEVEL2Model>();
            DataTable dt = dbHelper.GetDataTable("select SM.TC_L2_CODE,M.TC_L2_DESC from TC_LEVEL2 M,TC_SELE_LEVEL2 SM where M.TC_L2_CODE = SM.TC_L2_CODE");
            items = (from DataRow row1 in dt.Rows
                     select new TC_LEVEL2Model
                     {
                         TC_L2_CODE = row1["TC_L2_CODE"].ToString(),
                         TC_L2_DESC = row1["TC_L2_DESC"].ToString()
                     }).OrderBy(m => m.TC_L2_CODE).ToList();

            return items;
        }
    }
}