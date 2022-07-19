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
    public class UserCreationDAO
    {
        DBConnection dbCon = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        public List<LoginModel> CheckAuthirizedUser()
        {
            string query = "Select * from user_login_info Where STATUS='A'";
            OracleConnection con = new OracleConnection(dbCon.StringRead());
            con.Open();
            OracleCommand cmd = new OracleCommand(query, con);
            OracleDataReader reader = cmd.ExecuteReader();
            List<LoginModel> getId = new List<LoginModel>();
            while (reader.Read())
            {
                LoginModel modelData = new LoginModel();
                modelData.UserID = reader["USERID"].ToString();
                modelData.User_ID = reader["USER_ID"].ToString();
                modelData.Password = reader["PASSWORD"].ToString();
                modelData.UserName = reader["USERNAME"].ToString();
                getId.Add(modelData);
            }
            con.Close();
            return getId;
        }
    }
}