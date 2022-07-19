using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MRS.DAL.DAO
{
    public class DashboardSalesDAO
    {
        private readonly DBConnection dbCon = new DBConnection();
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        DateFormate dateType = new DateFormate();
        public List<DashBoardSalesModel> GetDesignationWiseSummery(DashBoardSalesModel model)
        {
            string Qry = "";

            //if (String.IsNullOrEmpty(model.FromDate) || String.IsNullOrEmpty(model.ToDate))
            {
                model.FromDate = "01-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Year.ToString();
                model.ToDate = Convert.ToDateTime(DateTime.Today).ToString("dd-MM-yyyy");
            }

            Qry = "SELECT M.GRP_DSIG_CODE, M.GRP_DSIG_SHORT_NAME, SUM(M.TOT_CEILING_AMOUNT) TOT_CEILING_AMOUNT, SUM(M.TOT_GRP_EXP) TOT_GRP_EXP " +
                "FROM  " +
                "(SELECT A.GRP_DSIG_CODE,  C.GRP_DSIG_SHORT_NAME, B.HONORARIUM_CODE, HONORARIUM_NAME, " +
                "(NVL(D.CEILING_AMT,0)*CEIL(MONTHS_BETWEEN (TO_DATE('" + model.ToDate + "', 'DD-MM-YYYY'), TO_DATE('" + model.FromDate + "', 'DD-MM-YYYY')))*NVL(A.NO_OF_PERS_IN_GRP,0)) TOT_CEILING_AMOUNT,  " +
                "NVL(E.TOT_GRP_EXP,0) TOT_GRP_EXP   " +
                "FROM  " +
                "(SELECT GRP_DSIG_CODE, COUNT(GRP_DSIG_CODE) NO_OF_PERS_IN_GRP " +
                "FROM MANAGEMENT_TEAM  " +
                "GROUP BY GRP_DSIG_CODE) A, " +
                "HONORARIUM_TYPE B, " +
                "GRP_DSIG C, " +
                "HON_WISE_CEILING D, " +
                "(SELECT GRP_DSIG_CODE,  HONORARIUM_CODE, SUM(EXPENSE_AMT) TOT_GRP_EXP  " +
                "FROM DOC_HONOR_PAID_INFO  " +
                "WHERE HONOR_APROV_DATE>= TO_DATE('" + model.FromDate + "', 'DD-MM-YYYY') " +
                "AND HONOR_APROV_DATE<=TO_DATE('" + model.ToDate + "', 'DD-MM-YYYY') " +
                "GROUP BY GRP_DSIG_CODE, HONORARIUM_CODE) E " +
                "WHERE A.GRP_DSIG_CODE=C.GRP_DSIG_CODE (+) " +
                "AND A.GRP_DSIG_CODE=D.GRP_DSIG_CODE(+) " +
                "AND B.HONORARIUM_CODE(+)=D.HONORARIUM_CODE " +
                "AND A.GRP_DSIG_CODE=E.GRP_DSIG_CODE (+) " +
                "AND B.HONORARIUM_CODE=E.HONORARIUM_CODE " +
                "ORDER BY A.GRP_DSIG_CODE, B.HONORARIUM_CODE) M " +
                "GROUP BY M.GRP_DSIG_CODE, M.GRP_DSIG_SHORT_NAME " +
                "ORDER BY M.GRP_DSIG_CODE";
            

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DashBoardSalesModel> itemList = new List<DashBoardSalesModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DashBoardSalesModel
                        {
                            GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString(),
                            GRP_DSIG_NAME = row["GRP_DSIG_SHORT_NAME"].ToString(),
                            TOT_CEILING_AMOUNT = row["TOT_CEILING_AMOUNT"].ToString(),
                            TOT_GRP_EXP = row["TOT_GRP_EXP"].ToString(),
                        }).ToList();
            return itemList;
        }

        public List<DashBoardSalesModel> GetMonthlyInvestmentComparison(DashBoardSalesModel model)
        {
                string Qry ="SELECT X.MONTH_NO, X.MONTH_NAME, "+
                            "NVL(Y.PREV_YEAR_MON_WS_EXP,0) PREV_YEAR_MON_WS_EXP, "+
                            "NVL(Z.CUR_YEAR_MON_WS_EXP,0) CUR_YEAR_MON_WS_EXP "+

                            "FROM  "+
                            "MONTH_NAME X, "+
                            "(SELECT TO_CHAR(HONOR_APROV_DATE, 'MM') MONTH_NO , SUM(EXPENSE_AMT) PREV_YEAR_MON_WS_EXP  "+
                            "FROM DOC_HONOR_PAID_INFO  "+
                            "WHERE  TO_CHAR(HONOR_APROV_DATE, 'RRRR')=(TO_CHAR(SYSDATE,'RRRR')-1) "+
                            "GROUP BY TO_CHAR(HONOR_APROV_DATE, 'MM')) Y, "+
        
                            "(SELECT TO_CHAR(HONOR_APROV_DATE, 'MM') MONTH_NO , SUM(EXPENSE_AMT) CUR_YEAR_MON_WS_EXP  "+
                            "FROM DOC_HONOR_PAID_INFO  " +
                            "WHERE  TO_CHAR(HONOR_APROV_DATE, 'RRRR')=TO_CHAR(SYSDATE,'RRRR') "+
                            "GROUP BY TO_CHAR(HONOR_APROV_DATE, 'MM'))  Z  "+
     
                            "WHERE X.MONTH_NO=Y.MONTH_NO (+) "+
                            "AND X.MONTH_NO=Z.MONTH_NO (+) "+
                            "ORDER BY X.MONTH_NO";

           
            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DashBoardSalesModel> itemList = new List<DashBoardSalesModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DashBoardSalesModel
                        {
                            MONTH_NO = row["MONTH_NO"].ToString(),
                            MONTH_NAME = row["MONTH_NAME"].ToString().Substring(0,3),
                            PREV_YEAR_MON_WS_EXP = row["PREV_YEAR_MON_WS_EXP"].ToString(),
                            CUR_YEAR_MON_WS_EXP = row["CUR_YEAR_MON_WS_EXP"].ToString(),
                        }).ToList();
            return itemList;
        }

        public List<DashBoardSalesModel> GetTypeWiseExpense(DashBoardSalesModel model)
        {
            if (String.IsNullOrEmpty(model.FromDate) || String.IsNullOrEmpty(model.ToDate))
            {
                model.FromDate = "01-01-" + DateTime.Today.Year.ToString();
                model.ToDate = Convert.ToDateTime(DateTime.Today).ToString("dd-MM-yyyy");
            }

            string Qry = "SELECT M.HONORARIUM_NAME, NVL(N.TOT_GRP_EXP,0) TOT_GRP_EXP "+
                        "FROM "+
                        "HONORARIUM_TYPE M, "+
                        "(SELECT HONORARIUM_CODE, SUM(EXPENSE_AMT) TOT_GRP_EXP  "+
                        "FROM DOC_HONOR_PAID_INFO  "+
                        "WHERE  HONOR_APROV_DATE>= TO_DATE('" + model.FromDate + "', 'DD-MM-YYYY') "+
                        "AND HONOR_APROV_DATE<=TO_DATE('" + model.ToDate + "', 'DD-MM-YYYY') " +
                        "GROUP BY HONORARIUM_CODE) N "+
                        "WHERE M.HONORARIUM_CODE=N.HONORARIUM_CODE(+) " +
                        "ORDER BY M.HONORARIUM_CODE"; 


            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DashBoardSalesModel> itemList = new List<DashBoardSalesModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DashBoardSalesModel
                        {
                            HONORARIUM_NAME = row["HONORARIUM_NAME"].ToString(),
                            TOT_GRP_EXP = row["TOT_GRP_EXP"].ToString(),
                        }).ToList();
            return itemList;
        }

        public List<DashBoardSalesModel> GetDoctorLeaderDashBoard(DashBoardSalesModel model)
        {
            if (String.IsNullOrEmpty(model.FromDate) || String.IsNullOrEmpty(model.ToDate))
            {
                model.FromDate = "01-01-" + DateTime.Today.Year.ToString();
                model.ToDate = Convert.ToDateTime(DateTime.Today).ToString("dd-MM-yyyy");
            }

            DBConnection dbConnection = new DBConnection();
            DataTable dt = new DataTable();
            OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead());
            OracleCommand oracleCommand = new OracleCommand();
            oracleCommand.Connection = oracleConnection;
            oracleCommand.CommandText = "PRC_LEADER_NON_LEADER";
            oracleCommand.CommandType = CommandType.StoredProcedure;
            oracleCommand.Parameters.Add("pFROM_DATE", OracleType.VarChar).Value = dateType.StringDateDdMonYYYY(model.FromDate);// model.FromDate;
            oracleCommand.Parameters.Add("pTO_DATE", OracleType.VarChar).Value = dateType.StringDateDdMonYYYY(model.ToDate);
            oracleCommand.Parameters.Add("pUID", OracleType.VarChar).Value = GetLAN();
            oracleConnection.Open();

            if (oracleCommand.ExecuteNonQuery() > 0)
            {
                string Qry = "SELECT LEADER_NON_LEADER, COUNT(DOCTOR_ID) NO_OF_DOC " +
                            "FROM Tmp_Market_Share " +
                            "WHERE User_ID='" + GetLAN() + "'" +
                            "GROUP BY LEADER_NON_LEADER";

                _dataTable = _dbHelper.GetDataTable(Qry);
            }
            List<DashBoardSalesModel> itemList = new List<DashBoardSalesModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DashBoardSalesModel
                        {
                            LEADER_NON_LEADER = row["LEADER_NON_LEADER"].ToString(),
                            NO_OF_DOCTOR = row["NO_OF_DOC"].ToString(),
                        }).ToList();
            return itemList;
        }

        public List<DashBoardSalesModel> GetCommitmentInfoDashBoard(DashBoardSalesModel model)
        {
            if (String.IsNullOrEmpty(model.FromDate) || String.IsNullOrEmpty(model.ToDate))
            {
                model.FromDate = "01-01-" + DateTime.Today.Year.ToString();
                model.ToDate = Convert.ToDateTime(DateTime.Today).ToString("dd-MM-yyyy");
            }

            DBConnection dbConnection = new DBConnection();
            DataTable dt = new DataTable();
            OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead());
            OracleCommand oracleCommand = new OracleCommand();
            oracleCommand.Connection = oracleConnection;
            oracleCommand.CommandText = "PRC_LEADER_NON_LEADER";
            oracleCommand.CommandType = CommandType.StoredProcedure;
            oracleCommand.Parameters.Add("pFROM_DATE", OracleType.VarChar).Value = dateType.StringDateDdMonYYYY(model.FromDate);// model.FromDate;
            oracleCommand.Parameters.Add("pTO_DATE", OracleType.VarChar).Value = dateType.StringDateDdMonYYYY(model.ToDate);
            oracleCommand.Parameters.Add("pUID", OracleType.VarChar).Value = GetLAN();
            oracleConnection.Open();

            if (oracleCommand.ExecuteNonQuery() > 0)
            {

                string Qry = "SELECT COMMITMENT_MEETUP, COUNT(DOCTOR_ID) NO_OF_DOC  " +
                            "FROM  " +
                            "(SELECT DOCTOR_ID, COMMITMENT_MEETUP(COMITMENT_SHARE_PCT, FIRST_MFG_PCT) COMMITMENT_MEETUP  " +
                            "FROM Tmp_Market_Share " +
                            "WHERE User_ID='" + GetLAN() + "')" +
                            "GROUP BY COMMITMENT_MEETUP";

                _dataTable = _dbHelper.GetDataTable(Qry);
            }
            List<DashBoardSalesModel> itemList = new List<DashBoardSalesModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DashBoardSalesModel
                        {
                            COMMITMENT_MEETUP = row["COMMITMENT_MEETUP"].ToString(),
                            NO_OF_TRANSACTION = row["NO_OF_DOC"].ToString(),
                        }).ToList();
            return itemList;
        }

        public List<DashBoardSalesModel> GetMonthWiseMarketShare(DashBoardSalesModel model)
        {
            string Qry = "";

            Qry =   "SELECT M.MONTH_NO, M.MONTH_NAME, M.CUR_YEAR_SQA_SHARE_PCT, M.PRE_YEAR_SQA_SHARE_PCT " +
                    "FROM MRS.MON_WS_SQA_SHARE_PCT_VUE M";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DashBoardSalesModel> itemList = new List<DashBoardSalesModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DashBoardSalesModel
                        {
                            MONTH_NO = row["MONTH_NO"].ToString(),
                            MONTH_NAME = row["MONTH_NAME"].ToString().Substring(0, 3),
                            CUR_YEAR_MON_WS_EXP = row["CUR_YEAR_SQA_SHARE_PCT"].ToString(),
                            PREV_YEAR_MON_WS_EXP = row["PRE_YEAR_SQA_SHARE_PCT"].ToString(),                          
                        }).ToList();        

            return itemList;
        }


        public List<DashBoardSalesModel> GetMonthWiseLeaderSts(DashBoardSalesModel model)
        {
            string Qry = "";

            Qry = "SELECT M.MONTH_NO, M.MONTH_NAME, M.CUR_YEAR_NON_LEADER_MKT_PCT,   M.PRE_YEAR_NON_LEADER_MKT_PCT "+
                    "FROM MRS.MON_WS_NON_LEADER_MKT_PCT_VUE M";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DashBoardSalesModel> itemList = new List<DashBoardSalesModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DashBoardSalesModel
                        {
                            MONTH_NO = row["MONTH_NO"].ToString(),
                            MONTH_NAME = row["MONTH_NAME"].ToString().Substring(0, 3),
                            CUR_YEAR_MON_WS_EXP = row["CUR_YEAR_NON_LEADER_MKT_PCT"].ToString(),
                            PREV_YEAR_MON_WS_EXP = row["PRE_YEAR_NON_LEADER_MKT_PCT"].ToString(),
                        }).ToList();

            return itemList;
        }

        private string GetLAN()
        {
            String ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ip))
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (ip == "::1")
            {
                ip = "127.0.0.1";
            }
            return ip;
        }

    }
}