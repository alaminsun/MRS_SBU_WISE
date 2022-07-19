using MRS.Controllers;
using MRS.DAL.Common;
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
    public class InstitutionCoverMarketInformationDAO
    {

        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        List<InstituteCoverMarketInformationDtlModel> items = new List<InstituteCoverMarketInformationDtlModel>();
        DBConnection dbCon = new DBConnection();
        IDGenerated idGenerated = new IDGenerated();
        public int InstituteSl = 0;
        public string maxID { get; set; }
        public string MarketCodeStatus { get; set; }
        public List<InstituteCoverMarketInformationMasModel> GetInstitutionInfoList(int instCode)
        {
            string Qry = "";
            //Qry = "select  a.INSTI_CODE,a.INSTI_NAME,a.INSTI_TYPE_CODE,b.INSTI_TYPE_NAME,a.ADDRESS1, a.ADDRESS2,a.ADDRESS3,a.ADDRESS4 from INSTITUTION a,INSTITUTION_TYPE b Where a.INSTI_TYPE_CODE=b.INSTI_TYPE_CODE";
            Qry = "select  a.INSTI_CODE,a.INSTI_NAME,a.INSTI_TYPE_CODE,b.INSTI_TYPE_NAME,a.ADDRESS1, a.ADDRESS2,a.ADDRESS3,a.ADDRESS4 from INSTITUTION a,INSTITUTION_TYPE b Where a.INSTI_TYPE_CODE=b.INSTI_TYPE_CODE and a.INSTI_CODE =" + instCode + " ";
            Qry = Qry + " ORDER BY INSTI_CODE";

            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<InstituteCoverMarketInformationMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new InstituteCoverMarketInformationMasModel
                     {
                         INSTI_CODE = row["INSTI_CODE"].ToString(),
                         INSTI_NAME = row["INSTI_NAME"].ToString(),
                         INSTI_TYPE_CODE = row["INSTI_TYPE_CODE"].ToString(),
                         INSTI_TYPE_NAME = row["INSTI_TYPE_NAME"].ToString(),
                         ADDRESS1 = row["ADDRESS1"].ToString(),
                         ADDRESS2 = row["ADDRESS2"].ToString(),
                         ADDRESS3 = row["ADDRESS3"].ToString(),
                         ADDRESS4 = row["ADDRESS4"].ToString()
                     }).ToList();

            return items;
        }

        public List<InstituteCoverMarketInformationDtlModel> GetMarketInfoList()
        {
            string Qry = "";
            Qry = "SELECT MARKET_CODE, MARKET_NAME, SBU_UNIT FROM MARKET ORDER BY MARKET_CODE ";


            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<InstituteCoverMarketInformationDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new InstituteCoverMarketInformationDtlModel
                     {
                         MARKET_CODE = row["MARKET_CODE"].ToString(),
                         MARKET_NAME = row["MARKET_NAME"].ToString(),
                         SBU_GROUP = row["SBU_UNIT"].ToString()

                     }).ToList();

            return items;
        }

        public ValidationMsg Save(InstituteCoverMarketInformationMasModel master, string userid)
        {
            OracleConnection oracleConnection = new OracleConnection(dbCon.StringRead());
            try
            {
                if (IsMSTExitsByInstituteCode(master.INSTI_CODE))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Data Already Exist.";
                }
                else
                {
                    master.INSTI_MKT_MAS_SLNO = idGenerated.getMAXSL("INSTI_MKT_MAS_SLNO", "INSTI_MKT_MAS");
                    if (master.MarketList.Count() > 0)
                    {
                        string qry = "INSERT INTO INSTI_MKT_MAS(INSTI_MKT_MAS_SLNO,INSTI_CODE) VALUES(" + master.INSTI_MKT_MAS_SLNO + ", '" + master.INSTI_CODE + "')";
                        _dbHelper.CmdExecute(qry);
                        foreach (var detailModel in master.MarketList)
                        {
                            detailModel.INSTI_MKT_DET_SLNO = idGenerated.getMAXSL("INSTI_MKT_DTL_SLNO", "INSTI_MKT_DTL");
                            qry = "INSERT INTO INSTI_MKT_DTL(INSTI_MKT_MAS_SLNO,INSTI_MKT_DTL_SLNO, MARKET_CODE,SBU_UNIT,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + master.INSTI_MKT_MAS_SLNO + "', '" + detailModel.INSTI_MKT_DET_SLNO + "', '" + detailModel.MARKET_CODE + "', '" + detailModel.SBU_GROUP + "', '" + userid + "',(TO_DATE('" + detailModel.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                            _dbHelper.CmdExecute(qry);
                        }
                        _validationMsg.Type = Enums.MessageType.Success;
                        _validationMsg.Msg = "Save Successful.";
                    }
                }


            }

            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed To Save Data";
                if (ex.Message.Contains("ORA-00001"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "This Data Already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _validationMsg;
        }

        private bool IsDTLExits(string MarketCode, string mstSL)
        {
            bool isTrue = false;
            string Qry = "SELECT * FROM INSTI_MKT_DTL WHERE MARKET_CODE = '" + MarketCode + "' and INSTI_MKT_MAS_SLNO =" + mstSL + "";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }

        private bool IsMSTExits(string mstSL)
        {
            bool isTrue = false;
            string Qry = "SELECT * FROM INSTI_MKT_MAS WHERE INSTI_MKT_MAS_SLNO = " + mstSL + "";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }



        public List<InstituteCoverMarketInformationDtlModel> GetSaveMarketList(string instituteCode)
        {
            //string query = "SELECT distinct D.INSTI_MKT_DTL_SLNO, D.INSTI_MKT_MAS_SLNO, D.MARKET_CODE,M.MARKET_NAME FROM INSTI_MKT_DTL D, MARKET M WHERE D.MARKET_CODE = M.MARKET_CODE and D.INSTI_MKT_MAS_SLNO='" + instituteCode + "'  ORDER BY D.MARKET_CODE ";

            string query = "";
            query = "SELECT distinct INSTI_MKT_DTL.*,MARKET.MARKET_NAME FROM INSTI_MKT_DTL " +
             " LEFT JOIN INSTI_MKT_MAS on INSTI_MKT_DTL.INSTI_MKT_MAS_SLNO = INSTI_MKT_MAS.INSTI_MKT_MAS_SLNO " +
              " LEFT JOIN MARKET ON INSTI_MKT_DTL.MARKET_CODE = MARKET.MARKET_CODE " +
             " WHERE INSTI_MKT_MAS.INSTI_CODE = '" + instituteCode + "' ORDER BY INSTI_MKT_DTL.MARKET_CODE ";

            OracleConnection con = new OracleConnection(dbCon.StringRead());
            List<InstituteCoverMarketInformationDtlModel> instituteCoverMarketInformationDtlModel = new List<InstituteCoverMarketInformationDtlModel>();
            OracleCommand cmd = new OracleCommand(query, con);
            con.Open();
            List<InstituteCoverMarketInformationDtlModel> items = new List<InstituteCoverMarketInformationDtlModel>();
            OracleDataReader reader = cmd.ExecuteReader();
            {
                while (reader.Read())
                {
                    InstituteCoverMarketInformationDtlModel model = new InstituteCoverMarketInformationDtlModel();
                    model.INSTI_MKT_DET_SLNO = Convert.ToInt32(reader["INSTI_MKT_DTL_SLNO"].ToString());
                    model.INSTI_MKT_MAS_SLNO = Convert.ToInt32(reader["INSTI_MKT_MAS_SLNO"].ToString());
                    model.MARKET_CODE = reader["MARKET_CODE"].ToString();
                    model.MARKET_NAME = reader["MARKET_NAME"].ToString();
                    model.SBU_GROUP = reader["SBU_UNIT"].ToString();
                    items.Add(model);
                }
            }
            return items;
        }

        //public List<InstituteCoverMarketInformationMasModel> GetInstituteSearchList(string instituteCode)
        public List<InstituteCoverMarketInformationMasModel> GetInstituteSearchList(int searchCode)
        {
            string Qry = "";
            //Qry = " SELECT distinct M.INSTI_MKT_MAS_SLNO, M.INSTI_CODE, A.INSTI_NAME, A.INSTI_TYPE_CODE, B.INSTI_TYPE_NAME, A.ADDRESS1,A.ADDRESS2, A.ADDRESS3, A.ADDRESS4 FROM INSTI_MKT_MAS M, INSTITUTION A, INSTITUTION_TYPE B WHERE M.INSTI_CODE = A.INSTI_CODE AND A.INSTI_TYPE_CODE = B.INSTI_TYPE_CODE and M.INSTI_CODE =" + searchCode + "";
            Qry = " SELECT distinct M.INSTI_MKT_MAS_SLNO, M.INSTI_CODE, A.INSTI_NAME, A.INSTI_TYPE_CODE, " +
                  " B.INSTI_TYPE_NAME, A.ADDRESS1,A.ADDRESS2, A.ADDRESS3, A.ADDRESS4 FROM INSTI_MKT_MAS M " +
                  " left join INSTITUTION A on A.INSTI_CODE = M.INSTI_CODE " +
                  " left join INSTITUTION_TYPE B on B.INSTI_TYPE_CODE = A.INSTI_TYPE_CODE" +
                  " Where 1=1";
            if (searchCode > 0)
            {
                //Qry = Qry + " and UPPER(A.INSTI_NAME) = UPPER('" + searchCode + "')";
                Qry = Qry + " And M.INSTI_CODE = " + searchCode + "";
            }
            Qry = Qry + " ORDER BY INSTI_CODE";

            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<InstituteCoverMarketInformationMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new InstituteCoverMarketInformationMasModel
                     {
                         INSTI_MKT_MAS_SLNO = Convert.ToInt32(row["INSTI_MKT_MAS_SLNO"].ToString()),
                         INSTI_CODE = row["INSTI_CODE"].ToString(),
                         INSTI_NAME = row["INSTI_NAME"].ToString(),
                         INSTI_TYPE_CODE = row["INSTI_TYPE_CODE"].ToString(),
                         INSTI_TYPE_NAME = row["INSTI_TYPE_NAME"].ToString(),
                         ADDRESS1 = row["ADDRESS1"].ToString(),
                         ADDRESS2 = row["ADDRESS2"].ToString(),
                         ADDRESS3 = row["ADDRESS3"].ToString(),
                         ADDRESS4 = row["ADDRESS4"].ToString()


                         //,
                         //ZONE_NAME = row["ZONE_NAME"].ToString()
                     }).ToList();

            return items;
        }




        public List<InstituteCoverMarketInformationDtlModel> GetAllDistributorDtlDataById(string model)
        {
            string queryMst = " SELECT D.INSTI_MKT_DTL_SLNO, D.INSTI_MKT_MAS_SLNO, D.MARKET_CODE,M.MARKET_NAME FROM INSTI_MKT_DTL D, MARKET M WHERE D.MARKET_CODE = M.MARKET_CODE AND INSTI_MKT_MAS_SLNO = '" + model + "' ORDER BY MARKET_CODE ";
            //string queryMst = " SELECT D.INSTI_MKT_DTL_SLNO, D.INSTI_MKT_MAS_SLNO, D.MARKET_CODE,A.MARKET_NAME FROM INSTI_MKT_MAS M,INSTI_MKT_DTL D, MARKET A WHERE D.MARKET_CODE = A.MARKET_CODE AND M.INSTI_MKT_MAS_SLNO = D.INSTI_MKT_MAS_SLNO AND M.INSTI_CODE = '" + model + "' ORDER BY MARKET_CODE ";
            OracleConnection con = new OracleConnection(dbCon.StringRead());
            con.Open();
            OracleCommand cmd = new OracleCommand(queryMst, con);
            OracleDataReader reader = cmd.ExecuteReader();
            List<InstituteCoverMarketInformationDtlModel> getId = new List<InstituteCoverMarketInformationDtlModel>();
            while (reader.Read())
            {
                InstituteCoverMarketInformationDtlModel modelData = new InstituteCoverMarketInformationDtlModel();
                modelData.INSTI_MKT_DET_SLNO = reader.GetInt32(0);
                modelData.INSTI_MKT_MAS_SLNO = reader.GetInt32(1);
                modelData.MARKET_CODE = reader["MARKET_CODE"].ToString();
                modelData.MARKET_NAME = reader["MARKET_NAME"].ToString();

                getId.Add(modelData);
            }
            con.Close();
            return getId;
        }


        public ValidationMsg Delete(string model)
        {

            try
            {
                var detailData = GetAllDistributorDtlDataById(model);
                if (detailData.Count > 0)
                {
                    foreach (var data in detailData)
                    {
                        string queryDtl = "DELETE FROM INSTI_MKT_DTL WHERE INSTI_MKT_MAS_SLNO='" + model + "'";
                        _dbHelper.CmdExecute(queryDtl);
                    }
                }

                string queryMst = "DELETE FROM INSTI_MKT_MAS WHERE INSTI_MKT_MAS_SLNO='" + model + "'";
                if (_dbHelper.CmdExecute(queryMst) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to Delete Data.";
            }
            return _validationMsg;

        }



        public ValidationMsg Update(InstituteCoverMarketInformationMasModel master, string userid)
        {
            string MstSL = "";
            OracleConnection oracleConnection = new OracleConnection(dbCon.StringRead());
            //_vmMsg = new ValidationMsg();
            MstSL = GetMstSLNO(master.INSTI_CODE);
            maxID = MstSL.ToString();
            foreach (var detailModel in master.MarketList)
            {
                //if (detailModel.INSTI_MKT_DET_SLNO == 0)
                //{
                //    detailModel.INSTI_MKT_DET_SLNO = idGenerated.getMAXSL("INSTI_MKT_DTL_SLNO", "INSTI_MKT_DTL");
                //    string queryDtl = "INSERT INTO INSTI_MKT_DTL(INSTI_MKT_MAS_SLNO,INSTI_MKT_DTL_SLNO, MARKET_CODE,SBU_UNIT,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                //                      "VALUES('" + MstSL + "', '" + detailModel.INSTI_MKT_DET_SLNO + "', '" + detailModel.MARKET_CODE + "','" + detailModel.SBU_GROUP + "', '" + userid + "',(TO_DATE('" + detailModel.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                //    _dbHelper.CmdExecute(queryDtl);
                //}
                //else
                //{
                //    string query = "Update INSTI_MKT_DTL Set MARKET_CODE='" + detailModel.MARKET_CODE + "',SBU_UNIT='" + detailModel.SBU_GROUP + "' WHERE INSTI_MKT_DTL_SLNO = '" + detailModel.INSTI_MKT_DET_SLNO + "'";
                //    _dbHelper.CmdExecute(query);
                //}

                if(IsDTLExits(detailModel.MARKET_CODE,maxID)) {
                    string query = "Update INSTI_MKT_DTL Set MARKET_CODE='" + detailModel.MARKET_CODE + "',SBU_UNIT='" + detailModel.SBU_GROUP + "' WHERE INSTI_MKT_DTL_SLNO = '" + detailModel.INSTI_MKT_DET_SLNO + "'";
                    _dbHelper.CmdExecute(query);
                }
                else{

                    if (detailModel.INSTI_MKT_DET_SLNO == 0)
                    {
                        detailModel.INSTI_MKT_DET_SLNO = idGenerated.getMAXSL("INSTI_MKT_DTL_SLNO", "INSTI_MKT_DTL");
                        string queryDtl = "INSERT INTO INSTI_MKT_DTL(INSTI_MKT_MAS_SLNO,INSTI_MKT_DTL_SLNO, MARKET_CODE,SBU_UNIT,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                          "VALUES('" + MstSL + "', '" + detailModel.INSTI_MKT_DET_SLNO + "', '" + detailModel.MARKET_CODE + "','" + detailModel.SBU_GROUP + "', '" + userid + "',(TO_DATE('" + detailModel.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                        _dbHelper.CmdExecute(queryDtl);
                    }
                    else
                    {
                        string query = "Update INSTI_MKT_DTL Set MARKET_CODE='" + detailModel.MARKET_CODE + "',SBU_UNIT='" + detailModel.SBU_GROUP + "' WHERE INSTI_MKT_DTL_SLNO = '" + detailModel.INSTI_MKT_DET_SLNO + "'";
                        _dbHelper.CmdExecute(query);
                    }
                }
            }

            _validationMsg.Type = Enums.MessageType.Success;
            _validationMsg.Msg = "Update Successful.";
            return _validationMsg;
        }



        private string GetMstSLNO(string instituteCode)
        {
            string SL = "0";
            string Qry = "Select INSTI_MKT_MAS_SLNO from INSTI_MKT_MAS Where INSTI_CODE='" + instituteCode + "'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                SL = dt.Rows[0][0].ToString();
            }

            return SL;
        }

        private bool IsMSTExitsByInstituteCode(string instituteCode)
        {
            bool isTrue = false;
            string Qry = "SELECT * FROM INSTI_MKT_MAS WHERE INSTI_CODE = '" + instituteCode + "'";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }


        public bool IsMSTExits(int? SLNo)
        {
            bool isTrue = false;
            string Qry = "SELECT * FROM INSTI_MKT_MAS WHERE INSTI_MKT_MAS_SLNO = " + SLNo + "";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }



        public ValidationMsg DeleteByDtlId(string dtlSlNo)
        {

            try
            {
                string query = "DELETE FROM INSTI_MKT_DTL WHERE INSTI_MKT_DTL_SLNO='" + dtlSlNo + "'";
                if (_dbHelper.CmdExecute(query) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to Delete Data.";
            }
            return _validationMsg;
        }


    }

}


