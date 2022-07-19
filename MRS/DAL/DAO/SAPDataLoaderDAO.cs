using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class SAPDataLoaderDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        public List<SAPDataLoaderModel> GetPOUserList()
        {
            string Qry = "";
            Qry = "select USER_ID,USERNAME,EMPLOYEE_NAME from USER_LOGIN_INFO where USERID='5'";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<SAPDataLoaderModel> items;
            items = (from DataRow row in dt.Rows
                     select new SAPDataLoaderModel
                     {
                         POUserCode = Convert.ToInt32(row["USER_ID"].ToString()),
                         POUser = row["USERNAME"].ToString(),
                         EmpolyeeName = row["EMPLOYEE_NAME"].ToString()
                     }).ToList();

            return items;
        }
        public DataTable SAPDataUploadedList(ReportModel model, string session_No)
        {
            string Qry = "";
            string delqry = "delete from TEMP_SAP_DOC_HONOR_PAID_INFO";
            dbHelper.CmdExecute(delqry);

            string insqry = "insert into TEMP_SAP_DOC_HONOR_PAID_INFO(HONOR_PAID_SLNO,VENDORCODE,EXPENSE_AMT,EXPENSE_DETAILS)" +
                            "select HONOR_PAID_SLNO,TO_CHAR(DOCTOR_ID) Vendorcode,EXPENSE_AMT,EXPENSE_DETAILS from DOC_HONOR_PAID_INFO where HONORARIUM_CODE ='01' and DOCTOR_ID is not null and INSTI_CODE is null and LOAD_STATUS='N' " +
                            " union all " +
                            "select HONOR_PAID_SLNO,'I'||TO_CHAR(INSTI_CODE) Vendorcode,EXPENSE_AMT,EXPENSE_DETAILS from DOC_HONOR_PAID_INFO where HONORARIUM_CODE ='01' and INSTI_CODE is not null and DOCTOR_ID is null and LOAD_STATUS='N' " +
                            " union all " +
                            "select HONOR_PAID_SLNO,'S'||SOCIETY_ID Vendorcode ,EXPENSE_AMT,EXPENSE_DETAILS from DOC_HONOR_PAID_INFO where HONORARIUM_CODE ='01' and SOCIETY_ID is not null and DOCTOR_ID is null and INSTI_CODE is null and LOAD_STATUS='N' ";
            dbHelper.CmdExecute(insqry);

            string qry = "update TEMP_SAP_DOC_HONOR_PAID_INFO set SESSION_NO='" + session_No + "'";
            dbHelper.CmdExecute(qry);

            Qry = "select HONOR_PAID_SLNO,TO_CHAR(DOCTOR_ID) Vendorcode,EXPENSE_AMT,EXPENSE_DETAILS from DOC_HONOR_PAID_INFO where HONORARIUM_CODE ='01' and DOCTOR_ID is not null and INSTI_CODE is null and LOAD_STATUS='N' " +
                  " union all " +
                  "select HONOR_PAID_SLNO,'I'||TO_CHAR(INSTI_CODE) Vendorcode,EXPENSE_AMT,EXPENSE_DETAILS from DOC_HONOR_PAID_INFO where HONORARIUM_CODE ='01' and INSTI_CODE is not null and DOCTOR_ID is null and LOAD_STATUS='N' " +
                  " union all " +
                  "select HONOR_PAID_SLNO,'S'||SOCIETY_ID Vendorcode ,EXPENSE_AMT,EXPENSE_DETAILS from DOC_HONOR_PAID_INFO where HONORARIUM_CODE ='01' and SOCIETY_ID is not null and DOCTOR_ID is null and INSTI_CODE is null and LOAD_STATUS='N'";

            DataTable dt = dbHelper.GetDataTable(Qry);
            dt.Columns.Add("POUser", typeof(System.Int32), model.USER_ID);
            return dt;

            //string Qry = "";
            //if (model.ReportType == "Doctor")
            //{
            //    string delqry = "delete from TEMP_SAP_DOC_HONOR_PAID_INFO";
            //    dbHelper.CmdExecute(delqry);

            //    string insqry = "insert into TEMP_SAP_DOC_HONOR_PAID_INFO(HONOR_PAID_SLNO,DOCTOR_ID,EXPENSE_AMT,EXPENSE_DETAILS)" +
            //                    "select HONOR_PAID_SLNO,DOCTOR_ID,EXPENSE_AMT,EXPENSE_DETAILS from DOC_HONOR_PAID_INFO where DOCTOR_ID is not null and LOAD_STATUS is null";
            //    dbHelper.CmdExecute(insqry);

            //    Qry = "select HONOR_PAID_SLNO,DOCTOR_ID,EXPENSE_AMT,EXPENSE_DETAILS from DOC_HONOR_PAID_INFO where DOCTOR_ID is not null and LOAD_STATUS is null";
            //}
            //else
            //{
            //    string delqry = "delete from TEMP_SAP_DOC_HONOR_PAID_INFO";
            //    dbHelper.CmdExecute(delqry);

            //    string insqry = "insert into TEMP_SAP_DOC_HONOR_PAID_INFO(HONOR_PAID_SLNO,INSTI_CODE,EXPENSE_AMT,EXPENSE_DETAILS)" +
            //                    "select HONOR_PAID_SLNO,INSTI_CODE,EXPENSE_AMT,EXPENSE_DETAILS from DOC_HONOR_PAID_INFO where INSTI_CODE is not null and LOAD_STATUS is null";
            //    dbHelper.CmdExecute(insqry);

            //    Qry = "select HONOR_PAID_SLNO,INSTI_CODE,EXPENSE_AMT,EXPENSE_DETAILS from DOC_HONOR_PAID_INFO where INSTI_CODE is not null and LOAD_STATUS is null";
            //}
            //DataTable dt = dbHelper.GetDataTable(Qry);
            //dt.Columns.Add("POUser", typeof(System.Int32), model.USER_ID);
            //return dt;
        }

        public ValidationMsg Save()
        {
            _vmMsg = new ValidationMsg();
            string qry = "update DOC_HONOR_PAID_INFO set LOAD_STATUS='Y',SESSION_NO=(SELECT SESSION_NO FROM TEMP_SAP_DOC_HONOR_PAID_INFO WHERE ROWNUM <= 1) where HONOR_PAID_SLNO in (select HONOR_PAID_SLNO from TEMP_SAP_DOC_HONOR_PAID_INFO)";
            dbHelper.CmdExecute(qry);
            _vmMsg.Type = Enums.MessageType.Success;
            return _vmMsg;
        }
    }
}
