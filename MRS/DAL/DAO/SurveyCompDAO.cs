using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MRS.DAL.DAO
{
    public class SurveyCompDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        string code = string.Empty;

        public ValidationMsg Save(SurveyCompModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            DataTable dt = dbHelper.GetDataTable("SELECT SURVEY_COMP_CODE FROM SURVEY_COMP ORDER BY SURVEY_COMP_CODE DESC");
            code = (Convert.ToInt16(dt.Rows[0]["SURVEY_COMP_CODE"].ToString()) + 1).ToString();
            try
            {
                //string qry = "INSERT INTO SURVEY_COMP (SURVEY_COMP_CODE,SURVEY_COMP_NAME) " +
                //"VALUES('" + model.SurveyCompCode + "', '" + model.SurveyCompName + "')";
                string qry = "INSERT INTO SURVEY_COMP (SURVEY_COMP_CODE,SURVEY_COMP_NAME) " +
                "VALUES('" + code + "', '" + model.SurveyCompName + "')";
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
                    _vmMsg.Msg = "Survey Company Code already Exist.";
                }
                if (ex.Message.Contains("ORA-12899"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value too Large for Survey Company Code(Maximum: 2 Digit)";
                }
            }
            return _vmMsg;
        }
        public string GetCode()
        {
            return code;
        }
        public ValidationMsg Update(SurveyCompModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string qry = "UPDATE SURVEY_COMP SET SURVEY_COMP_NAME = '" + model.SurveyCompName + "'" +
                             "WHERE SURVEY_COMP_CODE = '" + model.SurveyCompCode + "'";
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
                if (ex.Message.Contains("ORA-12899"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "value too large for column for Survey Company Code(Maximum: 2 Digit)";
                }
            }
            return _vmMsg;
        }
        public ValidationMsg Delete(string SurveyCompCode)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM SURVEY_COMP WHERE SURVEY_COMP_CODE = '" + SurveyCompCode + "' ";
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
        public List<SurveyCompModel> GetAllSurveyCompList()
        {
            string Qry = "";
            Qry = "SELECT SURVEY_COMP_CODE ,SURVEY_COMP_NAME FROM SURVEY_COMP ";
            Qry = Qry + "ORDER BY SURVEY_COMP_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<SurveyCompModel> items;
            items = (from DataRow row in dt.Rows
                     select new SurveyCompModel
                     {
                         SurveyCompCode = row["SURVEY_COMP_CODE"].ToString(),
                         SurveyCompName = row["SURVEY_COMP_NAME"].ToString()
                     }).ToList();

            return items;
        }
        public List<string> GetSuggestSurveyCompList()
        {
            string Qry = "SELECT SURVEY_COMP_NAME FROM SURVEY_COMP ORDER BY SURVEY_COMP_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<SurveyCompModel> items;
            items = (from DataRow row in dt.Rows
                     select new SurveyCompModel
                     {
                         SurveyCompName = row["SURVEY_COMP_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.SurveyCompName).ToList();
        }
    }
}