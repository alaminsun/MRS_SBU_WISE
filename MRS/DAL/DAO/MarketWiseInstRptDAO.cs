using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class MarketWiseInstRptDAO
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbCon = new DBConnection();
        public List<ReportModel> GetAllRegionInfo()
        {
            //string Qry = "SELECT distinct GENERIC_CODE,GENERIC_NAME||' | '||GENERIC_CODE GENERIC_NAME FROM GENERIC ORDER BY GENERIC_NAME ";
            string Qry = "SELECT * FROM REGION ORDER BY REGION_NAME ";

            OracleConnection con = new OracleConnection(dbCon.StringRead());
            con.Open();
            OracleCommand cmd = new OracleCommand(Qry, con);
            OracleDataReader reader = cmd.ExecuteReader();
            List<ReportModel> getId = new List<ReportModel>();
            while (reader.Read())
            {
                ReportModel model = new ReportModel();
                model.REGION_CODE = reader["REGION_CODE"].ToString();
                model.REGION_NAME = reader["REGION_NAME"].ToString();
                getId.Add(model);
            }
            return getId;
        }

        public List<ReportModel> GetAllInstitutionType()
        {
            //string Qry = "SELECT distinct GENERIC_CODE,GENERIC_NAME||' | '||GENERIC_CODE GENERIC_NAME FROM GENERIC ORDER BY GENERIC_NAME ";
            string Qry = "SELECT * FROM INSTITUTION_TYPE ORDER BY INSTI_TYPE_NAME ";

            OracleConnection con = new OracleConnection(dbCon.StringRead());
            con.Open();
            OracleCommand cmd = new OracleCommand(Qry, con);
            OracleDataReader reader = cmd.ExecuteReader();
            List<ReportModel> getId = new List<ReportModel>();
            while (reader.Read())
            {
                ReportModel model = new ReportModel();
                model.INSTI_TYPE_CODE = reader["INSTI_TYPE_CODE"].ToString();
                model.INSTI_TYPE_NAME = reader["INSTI_TYPE_NAME"].ToString();
                getId.Add(model);
            }
            return getId;
        }


        public DataTable GetMarketWiseInstitute(ReportModel model)
        {
            string Qry = "Select * from VW_MARKET_WISE_INSTITUTION";
            if (model.REGION_CODE == null && model.INSTI_TYPE_CODE == null && model.FromDate == null && model.ToDate == null)
            {
                Qry += " Order by " + model.Orderby + "";
            }

            else if (model.REGION_CODE != null && model.INSTI_TYPE_CODE == null && model.FromDate == null && model.ToDate == null)
            {
                Qry += " WHERE REGION_CODE = '" + model.REGION_CODE + "' Order by " + model.Orderby + "";
            }

            else if (model.INSTI_TYPE_CODE != null && model.REGION_CODE == null && model.FromDate == null && model.ToDate == null)
            {
                Qry += " WHERE INSTI_TYPE_CODE = '" + model.INSTI_TYPE_CODE + "' order by " + model.Orderby + "";
            }

            else if (model.FromDate != null && model.ToDate != null && model.INSTI_TYPE_CODE == null && model.REGION_CODE == null)
            {
                Qry += " WHERE ENTERED_DATE BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') AND To_Date ('" + model.ToDate + "','dd/MM/yyyy') Order by " + model.Orderby + "";
            }

            else if (model.INSTI_TYPE_CODE != null && model.REGION_CODE != null && model.FromDate == null && model.ToDate == null)
            {
                Qry += " WHERE REGION_CODE = '" + model.REGION_CODE + "' AND  INSTI_TYPE_CODE = '" + model.INSTI_TYPE_CODE + "' Order by '" + model.Orderby + "'";
            }

            else if (model.INSTI_TYPE_CODE != null && model.REGION_CODE != null && model.FromDate != null && model.ToDate != null)
            {
                Qry += " WHERE REGION_CODE = '" + model.REGION_CODE + "' AND  INSTI_TYPE_CODE = '" + model.INSTI_TYPE_CODE + "' AND ENTERED_DATE BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') AND To_Date ('" + model.ToDate + "','dd/MM/yyyy') Order by " + model.Orderby + "";
            }
            else if (model.REGION_CODE != null && model.FromDate != null && model.ToDate != null && model.INSTI_TYPE_CODE == null)
            {
                Qry += " WHERE REGION_CODE = '" + model.REGION_CODE + "' AND  ENTERED_DATE BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') AND To_Date ('" + model.ToDate + "','dd/MM/yyyy') Order by " + model.Orderby + "";
            }
            else if (model.INSTI_TYPE_CODE != null && model.REGION_CODE == null && model.FromDate != null && model.ToDate != null)
            {
                Qry += " WHERE INSTI_TYPE_CODE = '" + model.INSTI_TYPE_CODE + "' AND  ENTERED_DATE BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') AND To_Date ('" + model.ToDate + "','dd/MM/yyyy') Order by " + model.Orderby + "";
            }
            else
            {
                Qry = Qry;
            }
            DataTable dt = dbHelper.GetDataTable(Qry);
            return dt;
        }
    }
}