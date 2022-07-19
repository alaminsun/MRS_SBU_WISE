using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace MRS.DAL.Gateway
{
    public class DBHelper
    {
        DBConnection dbConnection = new DBConnection();
        public DataTable GetDataTable(string Qry)
        {
            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(Qry, dbConnection.StringRead());
            DataTable dt = new DataTable();
            oracleDataAdapter.Fill(dt);
            return dt;
        }

        public int CmdExecute(string Qry)
        {
            int noOfRows = 0;
            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(Qry, con);
                con.Open();
                noOfRows = cmd.ExecuteNonQuery();
            }
            return noOfRows;

            //OracleConnection oracleConnection = new OracleConnection(dbConnection.StringRead());
            //oracleConnection.Open();
            //OracleCommand oracleCommand = new OracleCommand(Qry, oracleConnection);
            //int noOfRows = oracleCommand.ExecuteNonQuery();
            //oracleConnection.Close();
            //return noOfRows;
        }
    }
}