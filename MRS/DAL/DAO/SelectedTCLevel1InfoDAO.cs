using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class SelectedTCLevel1InfoDAO
    {
        DBHelper dbHelper = new DBHelper();
        List<TC_LEVEL1Model> items;

        private ValidationMsg _vmMsg;
        public ValidationMsg Save(TC_LEVEL1Model model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                DataTable dt = dbHelper.GetDataTable("select * from TC_SELE_LEVEL1");
                if (dt.Rows.Count < 17)
                {
                    string qry = "INSERT INTO TC_SELE_LEVEL1 (TC_L1_CODE)" +
                    "VALUES('" + model.TC_L1_CODE + "')";

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
      
        public ValidationMsg Delete(string TC_L1_CODE)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM TC_SELE_LEVEL1 WHERE TC_L1_CODE = '" + TC_L1_CODE + "' ";
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
        public List<TC_LEVEL1Model> GetAllSelectedTCLevel1List()
        {
            var items = new List<TC_LEVEL1Model>();
            DataTable dt = dbHelper.GetDataTable("select SM.TC_L1_CODE,M.TC_L1_DESC from TC_LEVEL1 M,TC_SELE_LEVEL1 SM where M.TC_L1_CODE = SM.TC_L1_CODE");
            items = (from DataRow row1 in dt.Rows
                     select new TC_LEVEL1Model
                     {
                         TC_L1_CODE = row1["TC_L1_CODE"].ToString(),
                         TC_L1_DESC = row1["TC_L1_DESC"].ToString()
                     }).OrderBy(m => m.TC_L1_CODE).ToList();

            return items;
        }
    }
}