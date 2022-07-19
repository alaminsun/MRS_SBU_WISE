﻿using MRS.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace MRS.DAL.Common
{
    public class IDGenerated
    {
        DBConnection dbConnection = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        public long getMAXSL(string columnName, string tableName)
        {
            long MAXID = 0;
            string QueryString = "select NVL(MAX(" + columnName + "),0)+1 id from " + tableName + "";
            OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead());
            oracleConnection.Open();
            OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
            OracleDataReader rdr = oracleCommand.ExecuteReader();
            if (rdr.Read())
            {
                MAXID = Convert.ToInt64(rdr["id"].ToString());
            }
            rdr.Close();
            oracleConnection.Close();
            return MAXID;
        }
        public string getMAXID(string columnName, string tableName, string fm9)
        {
            string MAXID = "";
            string QueryString = "select to_char((select NVL(MAX(" + columnName + "),0)+1 from " + tableName + " ), '" + fm9 + "') id from dual";
            OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead());
            oracleConnection.Open();
            OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
            OracleDataReader rdr = oracleCommand.ExecuteReader();
            if (rdr.Read())
            {
                MAXID = rdr[0].ToString();
            }
            rdr.Close();
            oracleConnection.Close();
            return MAXID;
        }
        public string getRequisitionMAXID(string columnName, string tableName, string fm9)
        {
            string MAXID = "";
            string QueryString = "select to_char((select NVL(MAX(" + columnName + "),0)+1 from " + tableName + " ), '" + fm9 + "') id from dual";
            OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead());
            oracleConnection.Open();
            OracleCommand oracleCommand = new OracleCommand(QueryString, oracleConnection);
            OracleDataReader rdr = oracleCommand.ExecuteReader();
            if (rdr.Read())
            {
                MAXID = rdr[0].ToString();
            }
            rdr.Close();
            oracleConnection.Close();
            return MAXID;
        }
    }
}