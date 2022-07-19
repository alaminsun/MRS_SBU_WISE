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
    public class ManagementDesignationDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;
        public ValidationMsg Save(ManagementDesignationModel model, string userid)
        {
            string qry;
            _vmMsg = new ValidationMsg();
            //DataTable dt = dbHelper.GetDataTable("SELECT MGMT_CODE FROM MANAGEMENT_TEAM ORDER BY MGMT_CODE DESC");
            //code = (Convert.ToInt16(dt.Rows[0]["MGMT_CODE"].ToString()) + 1).ToString();
            try
            {
                if (string.IsNullOrEmpty(model.DIVISION_CODE))
                {
                    qry = "INSERT INTO MANAGEMENT_TEAM (MGMT_CODE,MGMT_NAME,GRP_DSIG_CODE, LOCATION_CODE,ENTERED_BY, ENTERED_DATE, ENTERED_TERMINAL)" +
                "VALUES('" + model.MGMT_CODE + "', '" + model.MGMT_NAME + "', '" + model.GRP_DSIG_CODE + "', '" + model.REGION_CODE + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                }
                else
                {
                    qry = "INSERT INTO MANAGEMENT_TEAM (MGMT_CODE,MGMT_NAME,GRP_DSIG_CODE, LOCATION_CODE, ENTERED_BY, ENTERED_DATE, ENTERED_TERMINAL)" +
                "VALUES('" + model.MGMT_CODE + "', '" + model.MGMT_NAME + "', '" + model.GRP_DSIG_CODE + "', '" + model.DIVISION_CODE + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                }
                
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
                    _vmMsg.Msg = "Personnel Code or Name already Exist.";
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
        public ValidationMsg Update(ManagementDesignationModel model, string userid)
        {
            string qry;
            _vmMsg = new ValidationMsg();
            try
            {
                if (model.GRP_DSIG_CODE == "6")
                {
                    qry = "UPDATE MANAGEMENT_TEAM SET MGMT_NAME = '" + model.MGMT_NAME + "'," +
             " GRP_DSIG_CODE = '" + model.GRP_DSIG_CODE + "'," +
             " LOCATION_CODE = '" + model.REGION_CODE + "'," +
             " UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
             " WHERE MGMT_CODE = '" + model.MGMT_CODE + "'";
                }
                else
                {
                    qry = "UPDATE MANAGEMENT_TEAM SET MGMT_NAME = '" + model.MGMT_NAME + "'," +
             " GRP_DSIG_CODE = '" + model.GRP_DSIG_CODE + "'," +
             " LOCATION_CODE = '" + model.DIVISION_CODE + "'," +
             " UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
             " WHERE MGMT_CODE = '" + model.MGMT_CODE + "'";
                }

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
        public ValidationMsg Delete(string MGMT_CODE)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM MANAGEMENT_TEAM WHERE MGMT_CODE = '" + MGMT_CODE + "' ";
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
            // string Qry = "SELECT MT.MGMT_CODE,MT.MGMT_NAME,MT.GRP_DSIG_CODE,GD.GRP_DSIG_NAME,MT.REGION_CODE,MT.REGION_NAME FROM MANAGEMENT_TEAM MT,GRP_DSIG GD WHERE MT.GRP_DSIG_CODE = GD.GRP_DSIG_CODE(+) ORDER BY MGMT_CODE";

            string Qry = "SELECT MT.MGMT_CODE,MT.MGMT_NAME,MT.GRP_DSIG_CODE,GD.GRP_DSIG_NAME,MT.LOCATION_CODE, (RTRIM (D.DIVISION_NAME) || ' ' || R.REGION_NAME) Location" +
                        " FROM MANAGEMENT_TEAM MT " +
                        " LEFT JOIN GRP_DSIG GD on  MT.GRP_DSIG_CODE = GD.GRP_DSIG_CODE " +
                        " LEFT JOIN REGION R on  MT.LOCATION_CODE = R.REGION_CODE " +
                        " LEFT JOIN DIVISION D on  MT.LOCATION_CODE = D.DIVISION_CODE " +
                        " ORDER BY MGMT_CODE ";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ManagementDesignationModel> items;
            items = (from DataRow row in dt.Rows
                     select new ManagementDesignationModel
                     {
                         MGMT_CODE = row["MGMT_CODE"].ToString(),
                         MGMT_NAME = row["MGMT_NAME"].ToString(),
                         GRP_DSIG_NAME = row["GRP_DSIG_NAME"].ToString(),
                         GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString(),
                         REGION_CODE = row["LOCATION_CODE"].ToString(),
                         REGION_NAME = row["Location"].ToString(),
                         DIVISION_CODE = row["LOCATION_CODE"].ToString(),
                         DIVISION_NAME = row["Location"].ToString(),
                         //DIVISION_NAME = row["Location"].ToString()
                     }).ToList();

            return items;
        }
        public List<string> GetSuggestManagementDesignationList()
        {
            string Qry = "SELECT MGMT_NAME FROM MANAGEMENT_TEAM ORDER BY MGMT_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ManagementDesignationModel> items;
            items = (from DataRow row in dt.Rows
                     select new ManagementDesignationModel
                     {
                         MGMT_NAME = row["MGMT_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.MGMT_NAME).ToList();
        }

        public List<ManagementDesignationModel> GetGroupDesignationList()
        {
            string Qry = "";
            Qry = "SELECT GRP_DSIG_CODE,GRP_DSIG_NAME FROM GRP_DSIG ";
            Qry = Qry + " ORDER BY GRP_DSIG_CODE";

            List<ManagementDesignationModel> items;

            DataTable dt = dbHelper.GetDataTable(Qry);

            items = (from DataRow row in dt.Rows
                     select new ManagementDesignationModel
                                        {
                                            GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString(),
                                            GRP_DSIG_NAME = row["GRP_DSIG_NAME"].ToString()
                                        }).ToList();

            return items;
        }

        public List<ManagementDesignationModel> GetRegionList()
        {
            string Qry = "";
            Qry = "SELECT distinct REGION_CODE,REGION_NAME FROM LOCATION_VUE ";
            Qry = Qry + " ORDER BY REGION_CODE";

            List<ManagementDesignationModel> items;

            DataTable dt = dbHelper.GetDataTable(Qry);

            items = (from DataRow row in dt.Rows
                     select new ManagementDesignationModel
                     {
                         //DIVISION_CODE = row["DIVISION_CODE"].ToString(),
                         //DIVISION_NAME = row["DIVISION_NAME"].ToString(),
                         REGION_CODE = row["REGION_CODE"].ToString(),
                         REGION_NAME = row["REGION_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<ManagementDesignationModel> GetDivisionList()
        {
            string Qry = "";
            Qry = "SELECT distinct DIVISION_CODE,DIVISION_NAME FROM DIVISION ";
            Qry = Qry + " ORDER BY DIVISION_CODE";

            List<ManagementDesignationModel> items;

            DataTable dt = dbHelper.GetDataTable(Qry);

            items = (from DataRow row in dt.Rows
                     select new ManagementDesignationModel
                     {
                         DIVISION_CODE = row["DIVISION_CODE"].ToString(),
                         DIVISION_NAME = row["DIVISION_NAME"].ToString(),
                         //REGION_CODE = row["REGION_CODE"].ToString(),
                         //REGION_NAME = row["REGION_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<ManagementDesignationModel> GetManagementInfo()
        {
            string Qry = "";
            Qry = "Select * from MANAGEMENT_TEAM";
            //Qry = Qry + " ORDER BY DIVISION_CODE";

            List<ManagementDesignationModel> items;

            DataTable dt = dbHelper.GetDataTable(Qry);

            items = (from DataRow row in dt.Rows
                     select new ManagementDesignationModel
                     {
                         GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString(),
                         //DIVISION_CODE = row["DIVISION_CODE"].ToString(),
                         //DIVISION_NAME = row["DIVISION_NAME"].ToString(),
                         //REGION_CODE = row["REGION_CODE"].ToString(),
                         //REGION_NAME = row["REGION_NAME"].ToString()
                     }).ToList();

            return items;
        }
    }
}