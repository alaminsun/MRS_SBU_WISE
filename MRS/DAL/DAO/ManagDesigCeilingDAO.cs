using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class ManagDesigCeilingDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        public ValidationMsg Save(ManagDesigCeilingModel model, string userid)
        {
            _vmMsg = new ValidationMsg();

            try
            {
                DataTable dtGRP_DSIG_CODE = dbHelper.GetDataTable("SELECT GRP_DSIG_CODE FROM HON_WISE_CEILING WHERE GRP_DSIG_CODE = " + model.GRP_DSIG_CODE + "");
                if (dtGRP_DSIG_CODE.Rows.Count > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Designation Group Name already Exist.";
                }
                else
                {
                    if (model.HonCeilingAmountList != null)
                    {
                        foreach (var detailModel in model.HonCeilingAmountList)
                        {
                            DataTable dt = dbHelper.GetDataTable("select NVL(MAX(HON_WISE_CEILING_CODE),0)+1 HON_WISE_CEILING_CODE from HON_WISE_CEILING");
                            string code = code = dt.Rows[0]["HON_WISE_CEILING_CODE"].ToString();
                            if (string.IsNullOrEmpty(detailModel.CEILING_AMT))
                                detailModel.CEILING_AMT = "0";
                            string qry = "INSERT INTO HON_WISE_CEILING (HON_WISE_CEILING_CODE,GRP_DSIG_CODE,HONORARIUM_CODE,CEILING_AMT)" +
                            "VALUES('" + code + "', '" + model.GRP_DSIG_CODE + "','" + detailModel.HONORARIUM_CODE + "', " + detailModel.CEILING_AMT + ")";
                            dbHelper.CmdExecute(qry);
                        }
                    }
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
                    _vmMsg.Msg = "ManagDesigCeiling Code already Exist.";
                }
                if (ex.Message.Contains("UK_INDI_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "ManagDesigCeiling Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Update(ManagDesigCeilingModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                if (model.HonCeilingAmountList != null)
                {
                    foreach (var detailModel in model.HonCeilingAmountList)
                    {
                        if (string.IsNullOrEmpty(detailModel.HON_WISE_CEILING_CODE))
                        {
                            DataTable dt = dbHelper.GetDataTable("select NVL(MAX(HON_WISE_CEILING_CODE),0)+1 HON_WISE_CEILING_CODE from HON_WISE_CEILING");
                            string code = code = dt.Rows[0]["HON_WISE_CEILING_CODE"].ToString();
                            if (string.IsNullOrEmpty(detailModel.CEILING_AMT))
                                detailModel.CEILING_AMT = "0";
                            string qry = "INSERT INTO HON_WISE_CEILING (HON_WISE_CEILING_CODE,GRP_DSIG_CODE,HONORARIUM_CODE,CEILING_AMT)" +
                            "VALUES('" + code + "', '" + model.GRP_DSIG_CODE + "','" + detailModel.HONORARIUM_CODE + "', " + detailModel.CEILING_AMT + ")";
                            dbHelper.CmdExecute(qry);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(detailModel.CEILING_AMT))
                                detailModel.CEILING_AMT = "0";
                            string qry = "UPDATE HON_WISE_CEILING SET GRP_DSIG_CODE = '" + model.GRP_DSIG_CODE + "',HONORARIUM_CODE = '" + detailModel.HONORARIUM_CODE + "',CEILING_AMT = " + detailModel.CEILING_AMT + "" +
                            "WHERE HON_WISE_CEILING_CODE = '" + detailModel.HON_WISE_CEILING_CODE + "'";
                            dbHelper.CmdExecute(qry);
                        }
                    }
                }
                _vmMsg.Type = Enums.MessageType.Update;
                _vmMsg.Msg = "Updated Successfully.";
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Update.";
                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "ManagDesigCeiling Code already Exist.";
                }
                if (ex.Message.Contains("UK_INDI_NAME"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "ManagDesigCeiling Name already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string HON_WISE_CEILING_CODE)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM HON_WISE_CEILING WHERE HON_WISE_CEILING_CODE = '" + HON_WISE_CEILING_CODE + "' ";
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
        public List<ManagementDesignationModel> GetAllManagementDesignationList()
        {
            string Qry = "SELECT GRP_DSIG_CODE,GRP_DSIG_NAME FROM GRP_DSIG ORDER BY GRP_DSIG_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ManagementDesignationModel> items;
            items = (from DataRow row in dt.Rows
                     select new ManagementDesignationModel
                     {
                         GRP_DSIG_NAME = row["GRP_DSIG_NAME"].ToString(),
                         GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString()
                     }).ToList();

            return items;
            //string Qry = "SELECT MT.GRP_DSIG_CODE,MT.MGMT_NAME,MT.GRP_DSIG_CODE,GD.GRP_DSIG_NAME FROM MANAGEMENT_TEAM MT,GRP_DSIG GD WHERE MT.GRP_DSIG_CODE = GD.GRP_DSIG_CODE(+)";

            //DataTable dt = dbHelper.GetDataTable(Qry);
            //List<ManagementDesignationModel> items;
            //items = (from DataRow row in dt.Rows
            //         select new ManagementDesignationModel
            //         {
            //             GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString(),
            //             MGMT_NAME = row["MGMT_NAME"].ToString(),
            //             GRP_DSIG_NAME = row["GRP_DSIG_NAME"].ToString(),
            //             GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString()
            //         }).ToList();

            //return items;
        }
        public List<ManagementDesignationModel> GetHonDesigCeilingSearchList()
        {
            //string Qry = "SELECT DISTINCT MT.GRP_DSIG_CODE,MT.MGMT_NAME,MT.GRP_DSIG_CODE,GD.GRP_DSIG_NAME FROM HON_WISE_CEILING HWC, MANAGEMENT_TEAM MT,GRP_DSIG GD WHERE MT.GRP_DSIG_CODE = GD.GRP_DSIG_CODE AND HWC.GRP_DSIG_CODE = MT.GRP_DSIG_CODE";
            //string Qry = "SELECT DISTINCT MT.GRP_DSIG_CODE,MT.MGMT_NAME,MT.GRP_DSIG_CODE,GD.GRP_DSIG_NAME FROM HON_WISE_CEILING HWC, MANAGEMENT_TEAM MT,GRP_DSIG GD WHERE MT.GRP_DSIG_CODE = GD.GRP_DSIG_CODE(+) AND HWC.GRP_DSIG_CODE = MT.GRP_DSIG_CODE";
            string Qry = "SELECT DISTINCT HWC.GRP_DSIG_CODE,GD.GRP_DSIG_NAME FROM HON_WISE_CEILING HWC,GRP_DSIG GD WHERE HWC.GRP_DSIG_CODE = GD.GRP_DSIG_CODE(+)";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ManagementDesignationModel> items;
            items = (from DataRow row in dt.Rows
                     select new ManagementDesignationModel
                     {
                         //GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString(),
                         //MGMT_NAME = row["MGMT_NAME"].ToString(),
                         GRP_DSIG_NAME = row["GRP_DSIG_NAME"].ToString(),
                         GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString()
                     }).ToList();

            return items;
        }

        public List<ManagDesigCeilingModel> GetSaveCeilingHonList(string GRP_DSIG_CODE)
        {
            string Qry = "SELECT DISTINCT HWC.HON_WISE_CEILING_CODE,HWC.HONORARIUM_CODE,HT.HONORARIUM_NAME,HWC.CEILING_AMT FROM HON_WISE_CEILING HWC, HONORARIUM_TYPE HT WHERE HWC.HONORARIUM_CODE = HT.HONORARIUM_CODE AND HWC.GRP_DSIG_CODE ='" + GRP_DSIG_CODE + "'";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ManagDesigCeilingModel> items;
            items = (from DataRow row in dt.Rows
                     select new ManagDesigCeilingModel
                     {
                         HON_WISE_CEILING_CODE = row["HON_WISE_CEILING_CODE"].ToString(),
                         HONORARIUM_CODE = row["HONORARIUM_CODE"].ToString(),
                         HONORARIUM_NAME = row["HONORARIUM_NAME"].ToString(),
                         CEILING_AMT = row["CEILING_AMT"].ToString() == "0" ? "" : row["CEILING_AMT"].ToString()
                     }).ToList();

            return items;
        }
    }
}