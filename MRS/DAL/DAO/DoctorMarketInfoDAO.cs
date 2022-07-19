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
    public class DoctorMarketInfoDAO
    {
        DBConnection dbCon = new DBConnection();
        DBHelper dbHelper = new DBHelper();
        IDGenerated idGenerated = new IDGenerated();

        private ValidationMsg _vmMsg;

        public List<Institution> GetInstitionList()
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select INSTI_CODE,INSTI_NAME from INSTITUTION ORDER BY INSTI_NAME";
                List<Institution> institutions = new List<Institution>();
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Institution model = new Institution();
                        model.InstituteCode = Convert.ToInt32(reader["INSTI_CODE"]);
                        model.InstituteName = reader["INSTI_NAME"].ToString();
                        institutions.Add(model);
                    }
                }
                return institutions;
            }
        }

        public List<DistrictUpazila> GetDistrictUpazila()
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select * from vw_District_Upazila";
                List<DistrictUpazila> DistrictUpazilas = new List<DistrictUpazila>();
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DistrictUpazila model = new DistrictUpazila();
                        model.DistrictCode = reader["DISTC_CODE"].ToString();
                        model.DistrictName = reader["DISTC_NAME"].ToString();
                        model.UpazilaCode = reader["UPAZILA_CODE"].ToString();
                        model.UpazilaName = reader["UPAZILA_NAME"].ToString();
                        DistrictUpazilas.Add(model);
                    }
                }
                return DistrictUpazilas;
            }
        }

        public List<Loacation> GetLocation(string marketCode)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select d.DP_LOC_CODE,p.DP_LOC_NAME " +
                                "from  MKT_DP_LOC_DTL d,MKT_DP_LOC_MAS m,DOC_PRACTICE_LOC p " +
                                "where  d.MKT_DP_LOC_MAS_SLNO = m.MKT_DP_LOC_MAS_SLNO " +
                                "and    d.DP_LOC_CODE = p.DP_LOC_CODE " +
                                " and    M.MARKET_CODE = '" + marketCode + "'";
                List<Loacation> Locations = new List<Loacation>();
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Loacation model = new Loacation();
                        model.LocCode = reader["DP_LOC_CODE"].ToString();
                        model.LocName = reader["DP_LOC_NAME"].ToString();
                        Locations.Add(model);
                    }
                }
                return Locations;
            }
        }

        public ValidationMsg Save(DoctorMarketInfoModel model)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                if (model.DoctorMarketDetailsModels.Count() > 0)
                {
                    if (CheckExistData(model.DoctorId) == 0)
                    {
                        model.DoctorMstSl = idGenerated.getMAXSL("DOC_MKT_MAS_SLNO", "DOC_MKT_MAS");
                        string query = "Insert into DOC_MKT_MAS(DOC_MKT_MAS_SLNO,DOCTOR_ID)values(" + model.DoctorMstSl + "," + model.DoctorId + ")";
                        dbHelper.CmdExecute(query);

                        foreach (var detailModel in model.DoctorMarketDetailsModels)
                        {
                            detailModel.DoctorDetailSl = idGenerated.getMAXSL("DOC_MKT_DTL_SLNO", "DOC_MKT_DTL");
                            string query1 = "Insert Into DOC_MKT_DTL(DOC_MKT_DTL_SLNO,DOC_MKT_MAS_SLNO,PRAC_MKT_CODE,CHAMB_ADDRESS1,CHAMB_ADDRESS2,CHAMB_ADDRESS3,CHAMB_ADDRESS4,CHAMB_PHONE, " +
                                    "UPAZILA_CODE,MDP_LOC_CODE,EDP_LOC_CODE,INSTI_CODE,ENTRY_DATE) Values(" + detailModel.DoctorDetailSl + "," + model.DoctorMstSl + ",'" + detailModel.MarketCode + "', " +
                                    "'" + detailModel.ChamberAddress1 + "','" + detailModel.ChamberAddress2 + "','" + detailModel.ChamberAddress3 + "','" + detailModel.ChamberAddress4 + "','" + detailModel.Phone + "', " +
                                    "'" + detailModel.UpazilaCode + "','" + detailModel.MorningLocCode + "','" + detailModel.EveningLocCode + "'," + detailModel.InstituteCode + ",(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')))";
                            dbHelper.CmdExecute(query1);
                        }
                        _vmMsg.Type = Enums.MessageType.Success;
                        _vmMsg.Msg = "Save Successful.";
                    }
                    else
                    {
                        _vmMsg.Type = Enums.MessageType.Error;
                        _vmMsg.Msg = "Data Already Exist.";
                    }
                }
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

        private long CheckExistData(long doctorId)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select COUNT(DOCTOR_ID) FROM DOC_MKT_MAS WHERE DOCTOR_ID=" + doctorId + "";
                long total = 0;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();

                object o = cmd.ExecuteScalar();
                if (o != null)
                {
                    total = Int64.Parse(o.ToString());
                }
                return total;
            }
        }

        public ValidationMsg Update(DoctorMarketInfoModel model)
        {
            _vmMsg = new ValidationMsg();
            long doctorMstSl = GetMstSl(model.DoctorId);

            foreach (var detailModel in model.DoctorMarketDetailsModels)
            {
                if (detailModel.DoctorDetailSl == 0)
                {
                    detailModel.DoctorDetailSl = idGenerated.getMAXSL("DOC_MKT_DTL_SLNO", "DOC_MKT_DTL");
                    string query1 = "Insert Into DOC_MKT_DTL(DOC_MKT_DTL_SLNO,DOC_MKT_MAS_SLNO,PRAC_MKT_CODE,CHAMB_ADDRESS1,CHAMB_ADDRESS2,CHAMB_ADDRESS3,CHAMB_ADDRESS4,CHAMB_PHONE, " +
                            "UPAZILA_CODE,MDP_LOC_CODE,EDP_LOC_CODE,INSTI_CODE,ENTRY_DATE) Values(" + detailModel.DoctorDetailSl + "," + doctorMstSl + ",'" + detailModel.MarketCode + "', " +
                            "'" + detailModel.ChamberAddress1 + "','" + detailModel.ChamberAddress2 + "','" + detailModel.ChamberAddress3 + "','" + detailModel.ChamberAddress4 + "','" + detailModel.Phone + "', " +
                            "'" + detailModel.UpazilaCode + "','" + detailModel.MorningLocCode + "','" + detailModel.EveningLocCode + "'," + detailModel.InstituteCode + ",(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')))";
                    dbHelper.CmdExecute(query1);
                }
                else
                {
                    string query = "Update DOC_MKT_DTL SET PRAC_MKT_CODE='" + detailModel.MarketCode + "',CHAMB_ADDRESS1='" + detailModel.ChamberAddress1 + "',CHAMB_ADDRESS2='" + detailModel.ChamberAddress2 + "', " +
                                  "CHAMB_ADDRESS3='" + detailModel.ChamberAddress3 + "',CHAMB_ADDRESS4='" + detailModel.ChamberAddress4 + "',CHAMB_PHONE='" + detailModel.Phone + "',UPAZILA_CODE='" + detailModel.UpazilaCode + "'," +
                                  "MDP_LOC_CODE='" + detailModel.MorningLocCode + "',EDP_LOC_CODE='" + detailModel.EveningLocCode + "',INSTI_CODE=" + detailModel.InstituteCode + " Where DOC_MKT_DTL_SLNO=" + detailModel.DoctorDetailSl + "";
                    dbHelper.CmdExecute(query);
                }
            }
            _vmMsg.Type = Enums.MessageType.Success;
            _vmMsg.Msg = "Update Successful.";
            return _vmMsg;
        }

        private long GetMstSl(long doctorId)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select DOC_MKT_MAS_SLNO from DOC_MKT_MAS Where DOCTOR_ID=" + doctorId + "";
                long slNo = 0;
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        slNo = Convert.ToInt64(reader["DOC_MKT_MAS_SLNO"]);
                    }
                    return slNo;
                }
            }
        }
        public List<DoctorMarketDetailsModel> GetDoctorPracticeList(int doctorId)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                List<DoctorMarketDetailsModel> doctorMarketList = new List<DoctorMarketDetailsModel>();
                string query = "Select distinct DOC_MKT_DTL.*,MARKET_NAME,INSTI_NAME,UPAZILA_NAME,DISTC_NAME,m.DP_LOC_NAME MDP_LOC_NAME, " +
                               "e.DP_LOC_NAME EDP_LOC_NAME from DOC_MKT_DTL " +
                               "Inner Join DOC_MKT_MAS on DOC_MKT_DTL.DOC_MKT_MAS_SLNO = DOC_MKT_MAS.DOC_MKT_MAS_SLNO " +
                               "Left Join MARKET on DOC_MKT_DTL.PRAC_MKT_CODE = MARKET.MARKET_CODE " +
                               "Left Join INSTITUTION on DOC_MKT_DTL.INSTI_CODE = INSTITUTION.INSTI_CODE " +
                               "Left Join vw_District_Upazila on DOC_MKT_DTL.UPAZILA_CODE = vw_District_Upazila.UPAZILA_CODE " +
                               "Left Join vw_Market_Location m on DOC_MKT_DTL.MDP_LOC_CODE = m.DP_LOC_CODE " +
                               "Left Join vw_Market_Location e on DOC_MKT_DTL.EDP_LOC_CODE = e.DP_LOC_CODE " +
                               "Where DOC_MKT_MAS.DOCTOR_ID=" + doctorId + "";
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorMarketDetailsModel model = new DoctorMarketDetailsModel();
                        model.DoctorDetailSl = Convert.ToInt32(reader["DOC_MKT_DTL_SLNO"]);
                        model.DoctorMstSl = Convert.ToInt32(reader["DOC_MKT_MAS_SLNO"]);
                        model.MarketCode = reader["PRAC_MKT_CODE"].ToString();
                        model.MarketName = reader["MARKET_NAME"].ToString();
                        model.ChamberAddress1 = reader["CHAMB_ADDRESS1"].ToString();
                        model.ChamberAddress2 = reader["CHAMB_ADDRESS2"].ToString();
                        model.ChamberAddress3 = reader["CHAMB_ADDRESS3"].ToString();
                        model.ChamberAddress4 = reader["CHAMB_ADDRESS4"].ToString();
                        model.Phone = reader["CHAMB_PHONE"].ToString();
                        model.UpazilaCode = reader["UPAZILA_CODE"].ToString();
                        model.UpazilaName = reader["UPAZILA_NAME"].ToString();
                        model.DistrictName = reader["DISTC_NAME"].ToString();
                        model.MorningLocCode = reader["MDP_LOC_CODE"].ToString();
                        model.SBU_GROUP = reader["SBU_UNIT"].ToString();
                        model.EveningLocCode = reader["EDP_LOC_CODE"].ToString();
                        //model.EveningLocName = reader["EDP_LOC_NAME"].ToString();

                        model.MorningLocTextName = reader["MDP_LOC_NAME"].ToString();
                        model.EveningTextLocName = reader["EDP_LOC_NAME"].ToString();

                        if (!string.IsNullOrEmpty(reader["INSTI_CODE"].ToString()))
                        {
                            model.InstituteCode = Convert.ToInt32(reader["INSTI_CODE"]);
                        }
                        else
                        {
                            model.InstituteCode = 0;
                        }

                        //if (!reader.IsDBNull(16))
                        //{
                        //    model.InstituteCode = Convert.ToInt32(reader["INSTI_CODE"]);
                        //}
                        //else
                        //{
                        //    model.InstituteCode = 0;
                        //}
                        model.InstituteName = reader["INSTI_NAME"].ToString();
                        doctorMarketList.Add(model);
                    }
                    return doctorMarketList;
                }
            }
        }

        public List<DoctorMarketDetailsModel> GetSearchPopUpMktData(int doctorId)
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                List<DoctorMarketDetailsModel> doctorMarketList = new List<DoctorMarketDetailsModel>();
                string query = "select MARKET_CODE, MARKET_NAME, md.SBU_UNIT from MARKET m,DOC_MKT_DTL md,DOC_MKT_MAS mm " +
                                "where M.MARKET_CODE = MD.PRAC_MKT_CODE and MD.DOC_MKT_MAS_SLNO = MM.DOC_MKT_MAS_SLNO and MM.DOCTOR_ID = " + doctorId + "";

                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorMarketDetailsModel model = new DoctorMarketDetailsModel();

                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        model.MarketName = reader["MARKET_NAME"].ToString();
                        model.SBU_GROUP = reader["SBU_UNIT"].ToString();

                        doctorMarketList.Add(model);
                    }
                    return doctorMarketList;
                }
            }
        }
        public ValidationMsg Delete(int slNo)
        {
            _vmMsg = new ValidationMsg();
            string query = "Delete from DOC_MKT_DTL where DOC_MKT_DTL_SLNO=" + slNo + "";
            dbHelper.CmdExecute(query);
            return _vmMsg;
        }

        public List<Market> MarketList()
        {
            using (OracleConnection con = new OracleConnection(dbCon.StringRead()))
            {
                string query = "Select MARKET_CODE,MARKET_NAME, SBU_UNIT from MARKET ORDER BY MARKET_NAME";
                List<Market> markets = new List<Market>();
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Market model = new Market();
                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        model.MarketName = reader["MARKET_NAME"].ToString();
                        model.SBU_GROUP = reader["SBU_UNIT"].ToString();
                        markets.Add(model);
                    }
                }
                return markets;
            }
        }
    }

    public class Institution
    {
        public int InstituteCode { get; set; }
        public string InstituteName { get; set; }
    }

    public class DistrictUpazila
    {
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string UpazilaCode { get; set; }
        public string UpazilaName { get; set; }
    }

    public class Loacation
    {
        public string LocCode { get; set; }
        public string LocName { get; set; }
    }
}
