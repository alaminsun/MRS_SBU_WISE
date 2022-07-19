using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class SessionDAO
    {
        DBConnection dbCon = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        ValidationMsg _vmMsg;
        public ValidationMsg Save(SessionModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                int sessionNo = GetSessionId();
                string query = "Insert Into USER_WORKING_SESSION(SESSION_SLNO,ENTRY_DATE,PURCHASE_DATE,USER_CODE,DATA_SOURCE,TAG) Values(" + sessionNo + ",(TO_DATE('" + model.PurchaseDate + "','dd/MM/yyyy'))," +
                              "(TO_DATE('" + model.EntryDate + "','dd/MM/yyyy')),'" + model.UserId + "','" + model.DataSource + "','" + model.Tag + "')";
                if (dbHelper.CmdExecute(query) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                }
                _vmMsg.Msg = "Saved Successfully.";
                _vmMsg.ReturnId = sessionNo;
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "This Data already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Update(SessionModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                string query = "Update USER_WORKING_SESSION set ENTRY_DATE=(TO_DATE('" + model.EntryDate + "','dd/MM/yyyy')), PURCHASE_DATE=(TO_DATE('" + model.PurchaseDate + "','dd/MM/yyyy')) where SESSION_SLNO=" + model.SessionSl + "";
                if (dbHelper.CmdExecute(query) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                }
                _vmMsg.Msg = "Update Successfully.";
                _vmMsg.ReturnId = Convert.ToInt64( model.SessionSl);
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "This Data already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }


        public List<SessionModel> GetSessionData()
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                List<SessionModel> sessions = new List<SessionModel>();
                string query = "Select SESSION_SLNO,TO_CHAR(ENTRY_DATE,'dd/MM/yyyy')ENTRY_DATE,TO_CHAR(PURCHASE_DATE,'dd/MM/yyyy')PURCHASE_DATE, " +
                               "USER_CODE,USERNAME,DATA_SOURCE,TAG from USER_WORKING_SESSION,USER_LOGIN_INFO " +
                               "where USER_WORKING_SESSION.USER_CODE = USER_LOGIN_INFO.USERID";
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SessionModel model = new SessionModel();
                        model.SessionSl = reader["SESSION_SLNO"].ToString();// Convert.ToInt64(reader["SESSION_SLNO"]);
                        model.EntryDate = reader["ENTRY_DATE"].ToString();
                        model.PurchaseDate = reader["PURCHASE_DATE"].ToString();
                        model.UserId = reader["USER_CODE"].ToString();
                        model.UserName = reader["USERNAME"].ToString();
                        model.DataSource = reader["DATA_SOURCE"].ToString();
                        model.Tag = reader["TAG"].ToString();

                        sessions.Add(model);
                    }
                }
                return sessions;
            }
        }

        public ValidationMsg Delete(int slNo)
        {
            try
            {
                _vmMsg = new ValidationMsg();
                string query = "Delete from USER_WORKING_SESSION where SESSION_SLNO =" + slNo + "";
                if (dbHelper.CmdExecute(query) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Delete. Data Is In Used.";
            }
            return _vmMsg;
        }

        private int GetSessionId()
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select NVL(Max(SESSION_SLNO),0)SESSION_SLNO from USER_WORKING_SESSION";
                // sessionSl = 0;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();

                int sessionSl = Convert.ToInt32(cmd.ExecuteScalar());
                switch (DateTime.Now.Year)
                {
                    case 2016:
                        if (sessionSl == 0)
                        {
                            sessionSl = Convert.ToInt32(DateTime.Now.Year + "" + (1).ToString().PadLeft(6, '0'));
                        }
                        else
                        {
                            sessionSl++;
                        }
                        break;

                    case 2017:
                        if (sessionSl == 0)
                        {
                            sessionSl = Convert.ToInt32(DateTime.Now.Year + "" + (1).ToString().PadLeft(6, '0'));
                        }
                        else
                        {
                            sessionSl++;
                        }
                        //sessionSl = Convert.ToInt32(DateTime.Now.Year + "" + (1).ToString().PadLeft(6, '0'));
                        //if (sessionSl != 201700000)
                        //{
                        //    sessionSl++;
                        //}
                        break;

                    case 2018:
                        if (sessionSl == 0)
                        {
                            sessionSl = Convert.ToInt32(DateTime.Now.Year + "" + (1).ToString().PadLeft(6, '0'));
                        }
                        else
                        {
                            sessionSl++;
                        }
                        //sessionSl = Convert.ToInt32(DateTime.Now.Year + "" + (1).ToString().PadLeft(6, '0'));
                        //if (sessionSl != 201800000)
                        //{
                        //    sessionSl++;
                        //}
                        break;

                    case 2019:
                        if (sessionSl == 0)
                        {
                            sessionSl = Convert.ToInt32(DateTime.Now.Year + "" + (1).ToString().PadLeft(6, '0'));
                        }
                        else
                        {
                            sessionSl++;
                        }
                        //sessionSl = Convert.ToInt32(DateTime.Now.Year + "" + (1).ToString().PadLeft(6, '0'));
                        //if (sessionSl != 201900000)
                        //{
                        //    sessionSl++;
                        //}
                        break;
                }
                return sessionSl;
            }
        }

    }
}