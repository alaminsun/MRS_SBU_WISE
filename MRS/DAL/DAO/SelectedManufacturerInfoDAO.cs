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
    public class SelectedManufacturerInfoDAO
    {
        DBHelper dbHelper = new DBHelper();
        List<ManufacturerInfoModel> items;

        private ValidationMsg _vmMsg;
        public ValidationMsg Save(ManufacturerInfoModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                DataTable dt = dbHelper.GetDataTable("select * from SELECTED_MANUFACTURER");
                if (dt.Rows.Count < 17)
                {
                    foreach (var detailModel in model.SelectedManufacturerList)
                    {
                        string qry = "INSERT INTO SELECTED_MANUFACTURER (MANUFACTURER_CODE,MFC_NIC_NAME,SLNO)" +
                                    "VALUES('" + detailModel.ManufacturerCode + "', '" + detailModel.MfcNicName + "', '" + detailModel.SlNo + "')";
                        dbHelper.CmdExecute(qry);
                    }
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Saved Successfully.";
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
            }
            return _vmMsg;
        }
        public ValidationMsg Update(ManufacturerInfoModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE SELECTED_MANUFACTURER SET SLNO = '" + model.SlNo + "'" +
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

                if (ex.Message.Contains("MRS.PK_SEL_MANUFACTURER_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Manufacturer Code already Exist.";
                }
                if (ex.Message.Contains("MRS.SELE_MANUFAC_SLNO_UIDX"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Sl No already Exist.";
                }
                if (ex.Message.Contains("ORA-12899"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value too large for Sl No(Maximum: 2 Digits).";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string ManufacturerCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM SELECTED_MANUFACTURER WHERE MANUFACTURER_CODE = '" + ManufacturerCode + "' ";
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

        public List<ManufacturerInfoModel> GetAllManufacturerList()
        {
            string Qry = "";
            //Qry = "SELECT MANUFACTURER_CODE ,MANUFACTURER_NAME,MFC_NIC_NAME,MFC_ORDER_SLNO FROM MANUFACTURER ";
            Qry = "SELECT * FROM MANUFACTURER WHERE STATUS ='A'";
            //Qry = "SELECT * FROM MANUFACTURER ";
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

        public List<SelectedManufacturerInfoModel> GetAllSelectedManufacturerList()
        {
            var items = new List<SelectedManufacturerInfoModel>();
            DataTable dt = dbHelper.GetDataTable("select SM.MANUFACTURER_CODE,M.MANUFACTURER_NAME,SM.MFC_NIC_NAME,SM.SLNO MFC_ORDER_SLNO from MANUFACTURER M,SELECTED_MANUFACTURER SM where M.MANUFACTURER_CODE = SM.MANUFACTURER_CODE");
            items = (from DataRow row1 in dt.Rows
                     select new SelectedManufacturerInfoModel
                     {
                         ManufacturerCode = row1["MANUFACTURER_CODE"].ToString(),
                         ManufacturerName = row1["MANUFACTURER_NAME"].ToString(),
                         MfcNicName = row1["MFC_NIC_NAME"].ToString(),
                         SlNo = Convert.ToInt16(row1["MFC_ORDER_SLNO"].ToString())
                     }).OrderBy(m => m.SlNo).ToList();

            return items;
        }
        public List<ManufacturerInfoModel> GetSelectedManufacturerList(string selectManufacturerList)
        {
            string Qry = "SELECT * FROM MANUFACTURER WHERE STATUS ='A' and MANUFACTURER_CODE in (" + selectManufacturerList + ") ORDER BY MANUFACTURER_CODE";

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
    }
}