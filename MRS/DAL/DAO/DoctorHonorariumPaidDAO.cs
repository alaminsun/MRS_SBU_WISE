using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;


namespace MRS.DAL.DAO
{
    public class DoctorHonorariumPaidDAO
    {
        private DBHelper _dbHelper = new DBHelper();
        DBConnection dbConnection = new DBConnection();
        private ValidationMsg _validationMsg = new ValidationMsg();
        private List<DoctorHonorariumPaidModel> doctorHonorariumPaidList;
        string code = string.Empty;
        string AprovNo = string.Empty;

        public ValidationMsg Save(DoctorHonorariumPaidModel model, string userid, string roleId)
        {
            DataTable chk = new DataTable();

            try
            {
                if (String.IsNullOrEmpty(model.DOCTOR_ID))
                {
                    //chk = _dbHelper.GetDataTable("Select * from Doc_Honor_Paid_Info where MGMT_CODE = '" + model.MGMT_CODE + "' AND HONOR_APROV_DATE = (TO_DATE('" + model.HONOR_APROV_DATE + "','dd/MM/yyyy')) AND MARKET_CODE = '" + model.MARKET_CODE + "' AND INSTI_CODE = '" + model.INSTI_CODE + "' AND DOCTOR_ID is null AND EXPENSE_AMT = '" + model.EXPENSE_AMT + "'");
                    chk = _dbHelper.GetDataTable("Select * from Doc_Honor_Paid_Info where MGMT_CODE = '" + model.MGMT_CODE + "' AND HONOR_APROV_DATE = (TO_DATE('" + model.HONOR_APROV_DATE + "','dd/MM/yyyy')) AND MARKET_CODE = '" + model.MARKET_CODE + "' AND INSTI_CODE = '" + model.INSTI_CODE + "' AND DOCTOR_ID is null AND EXPENSE_AMT = '" + model.EXPENSE_AMT + "' AND EXPENSE_DETAILS = '"+ model.EXPENSE_DETAILS +"'");
                }
                else if (String.IsNullOrEmpty(model.INSTI_CODE))
                {
                    chk = _dbHelper.GetDataTable("Select * from Doc_Honor_Paid_Info where MGMT_CODE = '" + model.MGMT_CODE + "' AND HONOR_APROV_DATE = (TO_DATE('" + model.HONOR_APROV_DATE + "','dd/MM/yyyy')) AND MARKET_CODE = '" + model.MARKET_CODE + "' AND DOCTOR_ID = '" + model.DOCTOR_ID + "' AND INSTI_CODE is null AND EXPENSE_AMT = '" + model.EXPENSE_AMT + "' AND EXPENSE_DETAILS = '" + model.EXPENSE_DETAILS + "'");
                }
                else
                {
                    chk = _dbHelper.GetDataTable("Select * from Doc_Honor_Paid_Info where MGMT_CODE = '" + model.MGMT_CODE + "' AND HONOR_APROV_DATE = (TO_DATE('" + model.HONOR_APROV_DATE + "','dd/MM/yyyy')) AND MARKET_CODE = '" + model.MARKET_CODE + "' AND DOCTOR_ID = '" + model.DOCTOR_ID + "' AND INSTI_CODE = '" + model.INSTI_CODE + "' AND EXPENSE_AMT = '" + model.EXPENSE_AMT + "' AND EXPENSE_DETAILS = '" + model.EXPENSE_DETAILS + "'");
                }

                if (chk.Rows.Count == 0)
                {
                    #region Honarium

                    DataTable dataTable = _dbHelper.GetDataTable("SELECT (NVL(MAX(HONOR_PAID_SLNO),0)+1)HONOR_PAID_SLNO FROM DOC_HONOR_PAID_INFO");
                    if (dataTable.Rows.Count > 0)
                    {
                        model.HONOR_PAID_SLNO = dataTable.Rows[0][0].ToString();
                    }

                    DateTime today = DateTime.ParseExact(model.HONOR_APROV_DATE.ToString(), "dd'/'MM'/'yyyy", null);
                    DataTable dataTableSlNo = _dbHelper.GetDataTable("SELECT MAX (HONOR_APROV_NO) FROM DOC_HONOR_PAID_INFO WHERE SUBSTR(HONOR_APROV_NO, 1,4) = '" + today.ToString("yyyy") + "' AND SUBSTR(HONOR_APROV_NO, 5,2) = '" + today.ToString("MM") + "'");
                    string PreAprovNo = string.Empty;

                    PreAprovNo = dataTableSlNo.Rows[0][0].ToString();

                    if (PreAprovNo.Length > 0)
                    {
                        long lastNumber = Convert.ToInt64(PreAprovNo) + 1;
                        model.HONOR_APROV_NO = lastNumber.ToString();
                    }
                    else
                    {
                        model.HONOR_APROV_NO = today.ToString("yyyy") + today.ToString("MM") + "0001";
                    }

                    string saveQuery = "";

                    if (string.IsNullOrEmpty(model.MARKET_CODE))
                    {
                        saveQuery =
                            "INSERT INTO DOC_HONOR_PAID_INFO (HONOR_PAID_SLNO,HONOR_APROV_NO,HONOR_APROV_DATE,MGMT_CODE,LOCATION_CODE,MARKET_CODE,TERRITORY_CODE,REGION_CODE,ZONE_CODE,DIVISION_CODE,MARKET_NAME,TERRITORY_NAME,REGION_NAME,ZONE_NAME,DIVISION_NAME,NATIONAL,SOCIETY_ID,INSTI_CODE,DOCTOR_ID,HONORARIUM_CODE,EXPENSE_DETAILS,EXPENSE_AMT,EXPENSE_FROM,EXPENSE_TO,PRESC_SHARE_PCT,PRESC_SHARE_FROM,PRESC_SHARE_TO,COMITMENT_SHARE_PCT,HONORARIUM_DURATION,DURATION_FROM,DURATION_TO,MON_CELI_AMT,TOT_APPROVED_AMT,TOT_DUE_AMT,EXPENSE_DURATION,PRESENT_SHARE_DURATION,GRP_DSIG_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL, OTHER_REMARKS, ADDITIONAL_COMMITMENT) VALUES('" +
                            model.HONOR_PAID_SLNO + "','" + model.HONOR_APROV_NO + "',(TO_DATE('" +
                            model.HONOR_APROV_DATE +
                            "','dd/MM/yyyy')),'" + model.MGMT_CODE + "','" + model.LOCATION_CODE + "','" + model.MARKET_CODE + "','" +
                            model.TERRITORY_CODE + "','" + model.REGION_CODE + "','" + model.ZONE_CODE + "','" +
                            model.DIVISION_CODE + "','" + model.MARKET_NAME + "','" +
                            model.TERRITORY_NAME + "','" + model.REGION_NAME + "','" +
                            model.ZONE_NAME + "','" + model.DIVISION_NAME + "','" + model.NATIONAL + "','" +
                            model.SOCIETY_ID + "','" + model.INSTI_CODE + "','" + model.DOCTOR_ID + "','" +
                            model.HONORARIUM_CODE + "','" + model.EXPENSE_DETAILS+ "','" + model.EXPENSE_AMT +
                            "',(TO_DATE('" + model.EXPENSE_FROM + "','dd/MM/yyyy')),(TO_DATE('" + model.EXPENSE_TO +
                            "','dd/MM/yyyy')),'" + model.PRESC_SHARE_PCT +
                            "',(TO_DATE('" + model.PRESC_SHARE_FROM + "','dd/MM/yyyy')),(TO_DATE('" +
                            model.PRESC_SHARE_TO + "','dd/MM/yyyy')),'" + model.COMITMENT_SHARE_PCT + "','" +
                            model.HONORARIUM_DURATION + "',(TO_DATE('" + model.DURATION_FROM +
                            "','dd/MM/yyyy')),(TO_DATE('" + model.DURATION_TO + "','dd/MM/yyyy')),'" +
                            model.MON_CELI_AMT + "','" + model.TOT_APPROVED_AMT + "','" + model.TOT_DUE_AMT + "','" +
                            model.EXPENSE_DURATION + "','" + model.PRESENT_SHARE_DURATION + "','" + model.GRP_DSIG_CODE +
                            "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" +
                            GetIPAddress.LocalIPAddress() + "','" + model.OTHER_REMARKS + "','" +
                            model.ADDITIONAL_COMITMENT + "' ) ";
                    }
                    else
                    {
                        saveQuery =
                           "INSERT INTO DOC_HONOR_PAID_INFO (HONOR_PAID_SLNO,HONOR_APROV_NO,HONOR_APROV_DATE,MGMT_CODE,LOCATION_CODE,MARKET_CODE,TERRITORY_CODE,REGION_CODE,ZONE_CODE,DIVISION_CODE,MARKET_NAME,TERRITORY_NAME,REGION_NAME,ZONE_NAME,DIVISION_NAME,NATIONAL,SOCIETY_ID,INSTI_CODE,DOCTOR_ID,HONORARIUM_CODE,EXPENSE_DETAILS,EXPENSE_AMT,EXPENSE_FROM,EXPENSE_TO,PRESC_SHARE_PCT,PRESC_SHARE_FROM,PRESC_SHARE_TO,COMITMENT_SHARE_PCT,HONORARIUM_DURATION,DURATION_FROM,DURATION_TO,MON_CELI_AMT,TOT_APPROVED_AMT,TOT_DUE_AMT,EXPENSE_DURATION,PRESENT_SHARE_DURATION,GRP_DSIG_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL, OTHER_REMARKS, ADDITIONAL_COMMITMENT) VALUES('" +
                           model.HONOR_PAID_SLNO + "','" + model.HONOR_APROV_NO + "',(TO_DATE('" +
                           model.HONOR_APROV_DATE +
                           "','dd/MM/yyyy')),'" + model.MGMT_CODE + "','" + model.LOCATION_CODE + "','" + model.MARKET_CODE + "','" +
                           model.TERRITORY_CODE + "','" + model.REGION_CODE + "','" + model.ZONE_CODE + "','" +
                           model.DIVISION_CODE + "','" + model.MARKET_NAME.Replace("'", "''") + "','" +
                           model.TERRITORY_NAME.Replace("'", "''") + "','" + model.REGION_NAME.Replace("'", "''") + "','" +
                           model.ZONE_NAME + "','" + model.DIVISION_NAME.Replace("'", "''") + "','" + model.NATIONAL + "','" +
                           model.SOCIETY_ID + "','" + model.INSTI_CODE + "','" + model.DOCTOR_ID + "','" +
                           model.HONORARIUM_CODE + "','" + model.EXPENSE_DETAILS.Replace("'", "''") + "','" + model.EXPENSE_AMT +
                           "',(TO_DATE('" + model.EXPENSE_FROM + "','dd/MM/yyyy')),(TO_DATE('" + model.EXPENSE_TO +
                           "','dd/MM/yyyy')),'" + model.PRESC_SHARE_PCT +
                           "',(TO_DATE('" + model.PRESC_SHARE_FROM + "','dd/MM/yyyy')),(TO_DATE('" +
                           model.PRESC_SHARE_TO + "','dd/MM/yyyy')),'" + model.COMITMENT_SHARE_PCT + "','" +
                           model.HONORARIUM_DURATION + "',(TO_DATE('" + model.DURATION_FROM +
                           "','dd/MM/yyyy')),(TO_DATE('" + model.DURATION_TO + "','dd/MM/yyyy')),'" +
                           model.MON_CELI_AMT + "','" + model.TOT_APPROVED_AMT + "','" + model.TOT_DUE_AMT + "','" +
                           model.EXPENSE_DURATION + "','" + model.PRESENT_SHARE_DURATION + "','" + model.GRP_DSIG_CODE +
                           "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" +
                           GetIPAddress.LocalIPAddress() + "','" + model.OTHER_REMARKS + "','" +
                           model.ADDITIONAL_COMITMENT + "' ) ";
                    }


                    _dbHelper.CmdExecute(saveQuery);

                    #endregion

                    #region DoctorCommitProduct

                    if (model.DoctorCommitProductList != null)
                    {
                        foreach (DoctorCommitProductModel objDoctorCommitProduct in model.DoctorCommitProductList)
                        {
                            string query = "select DOC_COMMI_PROD_SLNO from DOC_COMMITMENT_PROD order by DOC_COMMI_PROD_SLNO desc";
                            DataTable dt = _dbHelper.GetDataTable(query);
                            if (dt.Rows.Count > 0)
                                objDoctorCommitProduct.DOC_COMMI_PROD_SLNO = (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1);
                            else
                                objDoctorCommitProduct.DOC_COMMI_PROD_SLNO = 1;

                            string qry = "INSERT INTO DOC_COMMITMENT_PROD(DOC_COMMI_PROD_SLNO,HONOR_PAID_SLNO,PROD_ID, PRODUCT_DETAILS)" +
                            "VALUES('" + objDoctorCommitProduct.DOC_COMMI_PROD_SLNO + "','" + model.HONOR_PAID_SLNO + "', '" + objDoctorCommitProduct.PROD_ID + "','" + objDoctorCommitProduct.PRODUCT_DETAILS + "')";
                            _dbHelper.CmdExecute(qry);
                        }
                    }

                    #endregion

                    code = model.HONOR_PAID_SLNO;
                    AprovNo = model.HONOR_APROV_NO;

                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Saved Successfully : " + AprovNo;
                }
                else
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Data Already Exists";
                }
            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed To Save Data";
                if (ex.Message.Contains("ORA-00001"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Honararium App.No. Already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _validationMsg;
        }
        public string GetHONORPAIDSLNO()
        {
            return code;
        }
        public string GetAprovNo()
        {
            return AprovNo;
        }
        public ValidationMsg Update(DoctorHonorariumPaidModel model, string userid, string roleId)
        {
            try
            {

           string updateQuery;
               if (string.IsNullOrEmpty(model.MARKET_CODE))
                {
                    updateQuery = "   UPDATE DOC_HONOR_PAID_INFO SET HONOR_APROV_NO = '" + model.HONOR_APROV_NO + "'," +
                                    "                                  HONOR_APROV_DATE = (TO_DATE('" + model.HONOR_APROV_DATE + "','dd/MM/yyyy'))," +
                                    "                                  LOCATION_CODE = '" + model.LOCATION_CODE + "'," +
                                    "                                  MGMT_CODE = '" + model.MGMT_CODE + "'," +
                                    "                                  MARKET_CODE = '" + model.MARKET_CODE + "'," +
                                    "                                  MARKET_NAME = '" + model.MARKET_NAME + "'," +
                                    "                                  TERRITORY_CODE = '" + model.TERRITORY_CODE + "'," +
                                    "                                  TERRITORY_NAME = '" + model.TERRITORY_NAME + "'," +
                                    "                                  REGION_CODE = '" + model.REGION_CODE + "'," +
                                    "                                  REGION_NAME = '" + model.REGION_NAME + "'," +
                                    "                                  ZONE_CODE = '" + model.ZONE_CODE + "'," +
                                    "                                  ZONE_NAME = '" + model.ZONE_NAME + "'," +
                                    "                                  DIVISION_CODE = '" + model.DIVISION_CODE + "'," +
                                    "                                  DIVISION_NAME = '" + model.DIVISION_NAME + "'," +
                                    "                                  NATIONAL = '" + model.NATIONAL + "'," +
                                    "                                  SOCIETY_ID = '" + model.SOCIETY_ID + "'," +
                                    "                                  INSTI_CODE = '" + model.INSTI_CODE + "'," +
                                    "                                  DOCTOR_ID = '" + model.DOCTOR_ID + "'," +
                                    "                                  HONORARIUM_CODE = '" + model.HONORARIUM_CODE + "'," +
                                    "                                  EXPENSE_DETAILS = '" + model.EXPENSE_DETAILS + "'," +
                                    "                                  EXPENSE_AMT = '" + model.EXPENSE_AMT + "'," +
                                    "                                  EXPENSE_FROM = (TO_DATE('" + model.EXPENSE_FROM + "','dd/MM/yyyy'))," +
                                    "                                  EXPENSE_TO = (TO_DATE('" + model.EXPENSE_TO + "','dd/MM/yyyy'))," +
                                    "                                  PRESC_SHARE_PCT = '" + model.PRESC_SHARE_PCT + "'," +
                                    "                                  PRESC_SHARE_FROM = (TO_DATE('" + model.PRESC_SHARE_FROM + "','dd/MM/yyyy'))," +
                                    "                                  PRESC_SHARE_TO = (TO_DATE('" + model.PRESC_SHARE_TO + "','dd/MM/yyyy'))," +
                                    "                                  COMITMENT_SHARE_PCT = '" + model.COMITMENT_SHARE_PCT + "'," +
                                    "                                  HONORARIUM_DURATION = '" + model.HONORARIUM_DURATION + "'," +
                                    "                                  DURATION_FROM = (TO_DATE('" + model.DURATION_FROM + "','dd/MM/yyyy'))," +
                                    "                                  DURATION_TO = (TO_DATE('" + model.DURATION_TO + "','dd/MM/yyyy'))," +
                                    "                                  MON_CELI_AMT = '" + model.MON_CELI_AMT + "'," +
                                    "                                  TOT_APPROVED_AMT = '" + model.TOT_APPROVED_AMT + "'," +
                                    "                                  TOT_DUE_AMT = '" + model.TOT_DUE_AMT + "'," +
                                    "                                  EXPENSE_DURATION = '" + model.EXPENSE_DURATION + "'," +
                                    "                                  PRESENT_SHARE_DURATION = '" + model.PRESENT_SHARE_DURATION + "'," +
                                    "                                  GRP_DSIG_CODE = '" + model.GRP_DSIG_CODE + "'," +
                                    "                                  ADDITIONAL_COMMITMENT = '" + model.ADDITIONAL_COMITMENT + "'," +
                                    "                                  OTHER_REMARKS = '" + model.OTHER_REMARKS + "'," +
                                    "                                  UPDATED_BY = '" + userid + "'" +
                                    " ,UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                    "                            WHERE HONOR_PAID_SLNO = '" + model.HONOR_PAID_SLNO + "'";
                }

                else
                {
                    updateQuery = "   UPDATE DOC_HONOR_PAID_INFO SET HONOR_APROV_NO = '" + model.HONOR_APROV_NO + "'," +
                                    "                                  HONOR_APROV_DATE = (TO_DATE('" + model.HONOR_APROV_DATE + "','dd/MM/yyyy'))," +
                                    "                                  MGMT_CODE = '" + model.MGMT_CODE + "'," +
                                    "                                  LOCATION_CODE = '" + model.LOCATION_CODE + "'," +
                                    "                                  MARKET_CODE = '" + model.MARKET_CODE + "'," +
                                    "                                  MARKET_NAME = '" + model.MARKET_NAME.Replace("'", "''") + "'," +
                                    "                                  TERRITORY_CODE = '" + model.TERRITORY_CODE + "'," +
                                    "                                  TERRITORY_NAME = '" + model.TERRITORY_NAME.Replace("'", "''") + "'," +
                                    "                                  REGION_CODE = '" + model.REGION_CODE + "'," +
                                    "                                  REGION_NAME = '" + model.REGION_NAME.Replace("'", "''") + "'," +
                                    "                                  ZONE_CODE = '" + model.ZONE_CODE + "'," +
                                    "                                  ZONE_NAME = '" + model.ZONE_NAME.Replace("'", "''") + "'," +
                                    "                                  DIVISION_CODE = '" + model.DIVISION_CODE + "'," +
                                    "                                  DIVISION_NAME = '" + model.DIVISION_NAME.Replace("'", "''") + "'," +
                                    "                                  NATIONAL = '" + model.NATIONAL + "'," +
                                    "                                  SOCIETY_ID = '" + model.SOCIETY_ID + "'," +
                                    "                                  INSTI_CODE = '" + model.INSTI_CODE + "'," +
                                    "                                  DOCTOR_ID = '" + model.DOCTOR_ID + "'," +
                                    "                                  HONORARIUM_CODE = '" + model.HONORARIUM_CODE + "'," +
                                    "                                  EXPENSE_DETAILS = '" + model.EXPENSE_DETAILS.Replace("'", "''") + "'," +
                                    "                                  EXPENSE_AMT = '" + model.EXPENSE_AMT + "'," +
                                    "                                  EXPENSE_FROM = (TO_DATE('" + model.EXPENSE_FROM + "','dd/MM/yyyy'))," +
                                    "                                  EXPENSE_TO = (TO_DATE('" + model.EXPENSE_TO + "','dd/MM/yyyy'))," +
                                    "                                  PRESC_SHARE_PCT = '" + model.PRESC_SHARE_PCT + "'," +
                                    "                                  PRESC_SHARE_FROM = (TO_DATE('" + model.PRESC_SHARE_FROM + "','dd/MM/yyyy'))," +
                                    "                                  PRESC_SHARE_TO = (TO_DATE('" + model.PRESC_SHARE_TO + "','dd/MM/yyyy'))," +
                                    "                                  COMITMENT_SHARE_PCT = '" + model.COMITMENT_SHARE_PCT + "'," +
                                    "                                  HONORARIUM_DURATION = '" + model.HONORARIUM_DURATION + "'," +
                                    "                                  DURATION_FROM = (TO_DATE('" + model.DURATION_FROM + "','dd/MM/yyyy'))," +
                                    "                                  DURATION_TO = (TO_DATE('" + model.DURATION_TO + "','dd/MM/yyyy'))," +
                                    "                                  MON_CELI_AMT = '" + model.MON_CELI_AMT + "'," +
                                    "                                  TOT_APPROVED_AMT = '" + model.TOT_APPROVED_AMT + "'," +
                                    "                                  TOT_DUE_AMT = '" + model.TOT_DUE_AMT + "'," +
                                    "                                  EXPENSE_DURATION = '" + model.EXPENSE_DURATION + "'," +
                                    "                                  PRESENT_SHARE_DURATION = '" + model.PRESENT_SHARE_DURATION + "'," +
                                    "                                  GRP_DSIG_CODE = '" + model.GRP_DSIG_CODE + "'," +
                                    "                                  ADDITIONAL_COMMITMENT = '" + model.ADDITIONAL_COMITMENT + "'," +
                                    "                                  OTHER_REMARKS = '" + model.OTHER_REMARKS + "'," +
                                    "                                  UPDATED_BY = '" + userid + "'" +
                                    " ,UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                    "                            WHERE HONOR_PAID_SLNO = '" + model.HONOR_PAID_SLNO + "'";
                }
               

                _dbHelper.CmdExecute(updateQuery);

                #region DoctorCommitProduct

                if (model.DoctorCommitProductList != null)
                {
                    foreach (DoctorCommitProductModel objDoctorCommitProduct in model.DoctorCommitProductList)
                    {
                        if (string.IsNullOrEmpty(objDoctorCommitProduct.DOC_COMMI_PROD_SLNO.ToString()) || objDoctorCommitProduct.DOC_COMMI_PROD_SLNO == 0)
                        {
                            string query = "select DOC_COMMI_PROD_SLNO from DOC_COMMITMENT_PROD order by DOC_COMMI_PROD_SLNO desc";
                            DataTable dt = _dbHelper.GetDataTable(query);
                            if (dt.Rows.Count > 0)
                                objDoctorCommitProduct.DOC_COMMI_PROD_SLNO = (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1);
                            else
                                objDoctorCommitProduct.DOC_COMMI_PROD_SLNO = 1;
                            string qry = "INSERT INTO DOC_COMMITMENT_PROD(DOC_COMMI_PROD_SLNO,HONOR_PAID_SLNO,PROD_ID, PRODUCT_DETAILS)" +
                            "VALUES('" + objDoctorCommitProduct.DOC_COMMI_PROD_SLNO + "','" + model.HONOR_PAID_SLNO + "', '" + objDoctorCommitProduct.PROD_ID + "', '" + objDoctorCommitProduct.PRODUCT_DETAILS + "')";
                            _dbHelper.CmdExecute(qry);
                        }
                        else
                        {
                            string updateqry = "UPDATE DOC_COMMITMENT_PROD SET PROD_ID = '" + objDoctorCommitProduct.PROD_ID + "', PRODUCT_DETAILS = '" + objDoctorCommitProduct.PRODUCT_DETAILS + "' WHERE DOC_COMMI_PROD_SLNO = '" + objDoctorCommitProduct.DOC_COMMI_PROD_SLNO + "'";
                            _dbHelper.CmdExecute(updateqry);
                        }
                    }
                }

                _validationMsg.Type = Enums.MessageType.Update;
                _validationMsg.Msg = "Updated Successfully.";

                #endregion
         }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed To Update Data";

                if (ex.Message.Contains("ORA-01438"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _validationMsg;
        }

        //****************************** LIST OF VALOE/POP UP **************************//
        public string GetDoctorInfo(int doctorId, string regionCode, string DivisionCode, int roleID, int userID, int groupDsigCode)
        {
            string name = "";
            string query;

            if (roleID == 15)
            {

                query = "SELECT M.REGION_CODE,C.REGISTRATION_NO,C.POTENTIAL_CATEGORY, NVL(C.DEGREE_CODE,0)DEGREE_CODE, NVL(D.SPECIALIZATION_CODE,0)SPECIALIZATION_CODE,NVL(C.DESIGNATION_CODE,0)DESIGNATION_CODE,b.prac_mkt_code,M.MARKET_NAME,a.doctor_id,(RTRIM (c.honorarium) || ' ' || c.doctor_name) name, " +
                    " c.degree,c.designation,d.specialization,c.address1,c.address2,c.address3,c.address4,c.potential_category,B.ENTRY_DATE " +
                    " FROM doc_mkt_mas a,doc_mkt_dtl b,doctor c,doctor_specialization d,LOCATION_VUE m " +
                    " WHERE a.doc_mkt_mas_slno = b.doc_mkt_mas_slno AND a.doctor_id = c.doctor_id " +
                    " AND b.prac_mkt_code = M.MARKET_CODE AND c.specia_1st_code = d.specialization_code(+) ";


                if (doctorId != 0)
                {
                    query += " AND C.DOCTOR_ID=" + doctorId + "";
                }

                if (groupDsigCode == 5)
                {
                    query += " AND M.DIVISION_CODE = '" + DivisionCode + "'";
                }
                else
                {
                    query += " AND M.REGION_CODE = '" + regionCode + "'";
                }
                //if ((userID == 1053) || (userID == 1013) || (userID == 1813) || (userID == 513) || (userID == 1488) || (userID == 743) || (userID == 663) || (userID == 1090) || (userID == 664) || (userID == 2164))
                //{
                //    query += " AND M.DIVISION_CODE = '" + DivisionCode + "'";
                //}
                //else
                //{
                //    query += " AND M.REGION_CODE = '" + regionCode + "'";
                //}

                query += " ORDER BY M.ZONE_CODE, M.DIVISION_CODE,M.REGION_CODE,M.TERRITORY_CODE,M.MARKET_CODE";
            }
            else
            {
                 query = "SELECT DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,HONORARIUM,(RTRIM (honorarium) || ' ' || doctor_name) name,DEGREE,NVL(DEGREE_CODE,0)DEGREE_CODE," +
                                          "DESIGNATION,NVL(DESIGNATION_CODE,0)DESIGNATION_CODE,NVL(SPECIA_1ST_CODE,0)SPECIA_1ST_CODE,NVL(SPECIA_2ND_CODE,0)SPECIA_2ND_CODE,GENDER,RELIGION," +
                                          "TO_CHAR(DATE_OF_BIRTH,'dd/MM/yyyy')DATE_OF_BIRTH, DOC_PERS_PHONE,DOCTOR_EMAIL," +
                                          "NVL(PATIENT_PER_DAY,0)PATIENT_PER_DAY,NVL(AVG_PRESC_VALUE,0)AVG_PRESC_VALUE,NVL(PRESC_SHARE,0)PRESC_SHARE,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4," +
                                          "REMARKS,FSPECIALIZATION,SPECIALIZATION SSPECIALIZATION FROM " +
                    //"(SELECT DOCTOR.*,SPECIALIZATION FSPECIALIZATION FROM DOCTOR, DOCTOR_SPECIALIZATION WHERE DOCTOR.SPECIA_1ST_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE) DOCTORS " +
                                          "(SELECT DOCTOR.*,SPECIALIZATION FSPECIALIZATION FROM DOCTOR Left Join DOCTOR_SPECIALIZATION ON  DOCTOR.SPECIA_1ST_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE) DOCTORS " +
                                          "Left Join DOCTOR_SPECIALIZATION ON DOCTORS.SPECIA_2ND_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE Where 1=1 ";

                         if (doctorId != 0)
                            {
                                query += " AND DOCTOR_ID=" + doctorId + "";
                            }
            }





            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = reader["name"].ToString();

                    }
                }
            }
            //return listData;
            return name;


        }


        public DoctorInvestmentModel GetDonationInfoByDoctorID(int doctorId)
        {
            string query = "SELECT A.DOCTOR_ID, A.DONATION, A.MEDICINE, A.HONORARIUM, A.GIFT, A.DONATION_SCIETY, A.OTHERS, B.COMMITMENT_SHARE_PCT FROM MRS.DOCTOR_INVEST_STATUS_VUE A LEFT JOIN MRS.DOCTOR_COMMITMEN_SHARE_VUE B ON B.DOCTOR_ID = A.DOCTOR_ID " +
                           "WHERE A.DOCTOR_ID = " + doctorId;

            DoctorInvestmentModel dmc = new DoctorInvestmentModel();

            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();

                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dmc.DoctorId = reader["DOCTOR_ID"].ToString();
                        dmc.Donation = reader["DONATION"].ToString();
                        dmc.Medicine = reader["MEDICINE"].ToString();
                        dmc.Honorarium = reader["HONORARIUM"].ToString();
                        dmc.Gift = reader["GIFT"].ToString();
                        dmc.DonationSociety = reader["DONATION_SCIETY"].ToString();
                        dmc.Others = reader["OTHERS"].ToString();
                        dmc.CommitSharePct = reader["COMMITMENT_SHARE_PCT"].ToString();
                    }
                }
            }
            return dmc;
        }

        public string GetInstituteInfo(int instID, string regionCode, string DivisionCode, int roleID, int userID, int groupDsigCode)
        {
            string name = "";
            string qry = "";
            if (roleID == 15)
            {
                if (groupDsigCode == 5)
                {
                    qry = "SELECT A.INSTI_CODE,A.INSTI_NAME, C.MARKET_CODE, E.MARKET_NAME, E.REGION_CODE,E.REGION_NAME FROM INSTITUTION A, INSTI_MKT_MAS B, INSTI_MKT_DTL C, LOCATION_VUE E " +
                  "  where B.INSTI_MKT_MAS_SLNO = C.INSTI_MKT_MAS_SLNO " +
                   " AND B.INSTI_CODE = A.INSTI_CODE " +
                   " AND C.MARKET_CODE = E.MARKET_CODE " +
                   " AND E.DIVISION_CODE = '" + DivisionCode + "'" +
                   "AND A.INSTI_CODE = " + instID + "";
                }
                else
                {
                    qry = "SELECT A.INSTI_CODE,A.INSTI_NAME, C.MARKET_CODE, E.MARKET_NAME, E.REGION_CODE,E.REGION_NAME FROM INSTITUTION A, INSTI_MKT_MAS B, INSTI_MKT_DTL C, LOCATION_VUE E " +
                  "  where B.INSTI_MKT_MAS_SLNO = C.INSTI_MKT_MAS_SLNO " +
                   " AND B.INSTI_CODE =  A.INSTI_CODE " +
                   " AND C.MARKET_CODE = E.MARKET_CODE " +
                   " AND E.REGION_CODE = '" + regionCode + "'" +
                    "AND A.INSTI_CODE = " + instID + "";
                }
                //if ((userID == 1053) || (userID == 1013) || (userID == 1813) || (userID == 513) || (userID == 1488) || (userID == 743) || (userID == 663) || (userID == 1090) || (userID == 664) || (userID == 2164))
                //{
                //    qry = "SELECT A.INSTI_CODE,A.INSTI_NAME, C.MARKET_CODE, E.MARKET_NAME, E.REGION_CODE,E.REGION_NAME FROM INSTITUTION A, INSTI_MKT_MAS B, INSTI_MKT_DTL C, LOCATION_VUE E " +
                //  "  where B.INSTI_MKT_MAS_SLNO = C.INSTI_MKT_MAS_SLNO " +
                //   " AND B.INSTI_CODE = A.INSTI_CODE " +
                //   " AND C.MARKET_CODE = E.MARKET_CODE " +  
                //   " AND E.DIVISION_CODE = '" + DivisionCode + "'" +
                //   "AND A.INSTI_CODE = " + instID + "";
                //}
                //else
                //{
                //    qry = "SELECT A.INSTI_CODE,A.INSTI_NAME, C.MARKET_CODE, E.MARKET_NAME, E.REGION_CODE,E.REGION_NAME FROM INSTITUTION A, INSTI_MKT_MAS B, INSTI_MKT_DTL C, LOCATION_VUE E " +
                //  "  where B.INSTI_MKT_MAS_SLNO = C.INSTI_MKT_MAS_SLNO " +
                //   " AND B.INSTI_CODE =  A.INSTI_CODE " +
                //   " AND C.MARKET_CODE = E.MARKET_CODE " +
                //   " AND E.REGION_CODE = '" + regionCode + "'"+
                //    "AND A.INSTI_CODE = " + instID + "";
                //}

            }
            else
            {
                //qry = "SELECT A.INSTI_CODE,A.INSTI_NAME, C.MARKET_CODE, E.MARKET_NAME, E.REGION_CODE,E.REGION_NAME FROM INSTITUTION A, INSTI_MKT_MAS B, INSTI_MKT_DTL C, LOCATION_VUE E " +
                //  "  where B.INSTI_MKT_MAS_SLNO = C.INSTI_MKT_MAS_SLNO " +
                //   " AND B.INSTI_CODE = A.INSTI_CODE " +
                //   " AND C.MARKET_CODE = E.MARKET_CODE ";
                qry = "SELECT INSTI_CODE,INSTI_NAME FROM INSTITUTION Where INSTI_CODE = " + instID + "";
            }
            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(qry, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        name = reader["INSTI_NAME"].ToString();
                    }
                }
            }
            return name;
        }
        public DoctorHonorariumPaidModel GetMarketInfo(string marketCode, string regionCode, string DivisionCode, int roleID, int userID, int groupDsigCode)
        {

            string qry = "";
            if (roleID == 15)
            {
                if (groupDsigCode == 5)
                {
                    qry = "Select * from location_vue where market_code = '" + marketCode + "' And DIVISION_CODE = '" + DivisionCode + "'";
                }
                else
                {
                    qry = "Select * from location_vue where market_code = '" + marketCode + "' And REGION_CODE = '" + regionCode + "'";
                }
                //if ((userID == 1053) || (userID == 1013) || (userID == 1813) || (userID == 513) || (userID == 1488) || (userID == 743) || (userID == 663) || (userID == 1090) || (userID == 664) || (userID == 2164))
                //{
                //    qry = "Select * from location_vue where market_code = '" + marketCode + "' And DIVISION_CODE = '" + DivisionCode + "'";
                //}
                //else
                //{
                //    qry = "Select * from location_vue where market_code = '" + marketCode + "' And REGION_CODE = '" + regionCode + "'";
                //}

            }
            else
            {
                qry = "Select * from location_vue where market_code = '" + marketCode + "'";
            }
            //string query = "Select * from location_vue where market_code = '" + marketCode + "'";

            DoctorHonorariumPaidModel dmc = new DoctorHonorariumPaidModel();

            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(qry, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dmc.MARKET_CODE = reader["MARKET_CODE"].ToString();
                        dmc.MARKET_NAME = reader["MARKET_NAME"].ToString();
                        dmc.TERRITORY_CODE = reader["TERRITORY_CODE"].ToString();
                        dmc.TERRITORY_NAME = reader["TERRITORY_NAME"].ToString();
                        dmc.ZONE_CODE = reader["ZONE_CODE"].ToString();
                        dmc.ZONE_NAME = reader["ZONE_NAME"].ToString();
                        dmc.DIVISION_CODE = reader["DIVISION_CODE"].ToString();
                        dmc.DIVISION_NAME = reader["DIVISION_NAME"].ToString();
                        dmc.REGION_CODE = reader["REGION_CODE"].ToString();
                        dmc.REGION_NAME = reader["REGION_NAME"].ToString();
                    }
                }
            }
            return dmc;
        }

        public DoctorHonorariumPaidModel GetShareData(string Presc_Share_From, string Presc_Share_To, string Market_Code, string Doctor_ID, string Insti_Code)
        {
            string query = "SELECT ROUND(DECODE(NVL(Z.ALL_MFG_TOT_VALUE,0),0,0, (NVL(Y.SQUARE_VALUE,0)/NVL(Z.ALL_MFG_TOT_VALUE,0))*100 ),2) SQA_SHARE_PCT , NVL(Z.NO_OF_TRANSACTION,0) NO_OF_TRANSACTION " +
                            "FROM  " +
                                "(SELECT  SUM(NVL(A.PURCHASE_QTY,0)*NVL(A.UNIT_PRICE,0)) SQUARE_VALUE " +
                                "FROM USER_WORKING_SESSION E, PRESCRIPTION_DETAIL A, " +
                                "PRESCRIPTION_MASTER B, PRODUCT C " +
                                "WHERE E.SESSION_SLNO=B.SESSION_SLNO " +
                                "AND B.PRESC_MAS_SLNO=A.PRESC_MAS_SLNO " +
                                "AND TO_DATE(E.ENTRY_DATE)>=(TO_DATE('" + Presc_Share_From + "','dd/MM/yyyy')) " +
                                "AND TO_DATE(E.ENTRY_DATE)<=(TO_DATE('" + Presc_Share_To + "','dd/MM/yyyy')) " +
                                "AND B.MARKET_CODE='" + Market_Code + "' " +
                                "AND (B.DOCTOR_ID='" + Doctor_ID + "' OR B.INSTI_CODE= '" + Insti_Code + "') " +
                                "AND A.PROD_ID=C.PROD_ID " +
                                "AND C.MANUFACTURER_CODE='SQA') Y, " +

                                "(SELECT SUM(NVL(A.PURCHASE_QTY,0)*NVL(A.UNIT_PRICE,0)) ALL_MFG_TOT_VALUE, COUNT(A.PROD_ID) NO_OF_TRANSACTION " +
                                "FROM USER_WORKING_SESSION E, PRESCRIPTION_DETAIL A, " +
                                "PRESCRIPTION_MASTER B, PRODUCT C " +
                                "WHERE E.SESSION_SLNO=B.SESSION_SLNO " +
                                "AND B.PRESC_MAS_SLNO=A.PRESC_MAS_SLNO " +
                                "AND TO_DATE(E.ENTRY_DATE)>=(TO_DATE('" + Presc_Share_From + "','dd/MM/yyyy')) " +
                                "AND TO_DATE(E.ENTRY_DATE)<=(TO_DATE('" + Presc_Share_To + "','dd/MM/yyyy')) " +
                                "AND B.MARKET_CODE='" + Market_Code + "' " +
                                "AND (B.DOCTOR_ID='" + Doctor_ID + "' OR B.INSTI_CODE='" + Insti_Code + "') " +
                                "AND A.PROD_ID=C.PROD_ID) Z";

            DoctorHonorariumPaidModel dmc = new DoctorHonorariumPaidModel();

            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dmc.PRESC_SHARE_PCT = reader["SQA_SHARE_PCT"].ToString();
                        dmc.RowCount = reader["NO_OF_TRANSACTION"].ToString();
                    }
                }
            }
            return dmc;
        }

        public List<DoctorHonorariumPaidModel> GetHonarariumPaidLocationList( string DivisionCode, string regionCode, int userID, int roleID, int groupDsigCode)
        {
            string qry = "";
            if(roleID == 15)
            {
                if (groupDsigCode == 5)
                {
                    qry = "SELECT * FROM LOCATION_VUE Where DIVISION_CODE = '" + DivisionCode + "'";
                }
                else
                {
                    qry = "SELECT * FROM LOCATION_VUE Where REGION_CODE = '" + regionCode + "'";
                }

                //if ((userID == 1053) || (userID == 1013) || (userID == 1813) || (userID == 513) || (userID == 1488) || (userID == 743) || (userID == 663) || (userID == 1090) || (userID == 664) || (userID == 2164))
                //{
                //    qry = "SELECT * FROM LOCATION_VUE Where DIVISION_CODE = '" + DivisionCode + "'";
                //}
                //else
                //{
                //    qry = "SELECT * FROM LOCATION_VUE Where REGION_CODE = '" + regionCode + "'";
                //}
                
            }
            else
            {
                qry = "SELECT * FROM LOCATION_VUE";
            }
            
            qry = qry + " ORDER BY MARKET_CODE";

            DataTable dt = _dbHelper.GetDataTable(qry);

            doctorHonorariumPaidList = (from DataRow row in dt.Rows
                                        select new DoctorHonorariumPaidModel
                                        {
                                            MARKET_CODE = row["MARKET_CODE"].ToString(),
                                            MARKET_NAME = row["MARKET_NAME"].ToString(),
                                            TERRITORY_CODE = row["TERRITORY_CODE"].ToString(),
                                            TERRITORY_NAME = row["TERRITORY_NAME"].ToString(),
                                            ZONE_CODE = row["ZONE_CODE"].ToString(),
                                            ZONE_NAME = row["ZONE_NAME"].ToString(),
                                            DIVISION_CODE = row["DIVISION_CODE"].ToString(),
                                            DIVISION_NAME = row["DIVISION_NAME"].ToString(),
                                            REGION_CODE = row["REGION_CODE"].ToString(),
                                            REGION_NAME = row["REGION_NAME"].ToString()
                                        }).ToList();

            return doctorHonorariumPaidList;
        }

        public List<DoctorHonorariumPaidModel> GetManagementList()
        {
            string qry;
                // qry = "SELECT MT.MGMT_CODE,MT.MGMT_NAME,MT.GRP_DSIG_CODE,GD.GRP_DSIG_NAME FROM MANAGEMENT_TEAM MT,GRP_DSIG GD WHERE MT.GRP_DSIG_CODE = GD.GRP_DSIG_CODE ORDER BY MGMT_CODE";
            qry = "SELECT MT.MGMT_CODE,MT.MGMT_NAME,MT.GRP_DSIG_CODE,MT.LOCATION_CODE,(RTRIM (D.DIVISION_NAME) || ' ' || R.REGION_NAME) Location,GD.GRP_DSIG_NAME FROM MANAGEMENT_TEAM MT " +
                   " LEFT JOIN GRP_DSIG GD on MT.GRP_DSIG_CODE = GD.GRP_DSIG_CODE " +
                   "  LEFT JOIN DIVISION D on MT.LOCATION_CODE = D.DIVISION_CODE " +
                   " LEFT JOIN REGION R on MT.LOCATION_CODE = R.REGION_CODE ORDER BY MGMT_CODE";
            // qry = qry + " ";

            DataTable dt = _dbHelper.GetDataTable(qry);

            doctorHonorariumPaidList = (from DataRow row in dt.Rows
                                        select new DoctorHonorariumPaidModel
                                        {
                                            MGMT_CODE = row["MGMT_CODE"].ToString(),
                                            MGMT_NAME = row["MGMT_NAME"].ToString(),
                                            GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString(),
                                            GRP_DSIG_NAME = row["GRP_DSIG_NAME"].ToString(),
                                            REGION_CODE = row["LOCATION_CODE"].ToString(),
                                            REGION_NAME = row["Location"].ToString(),
                                            DIVISION_CODE = row["LOCATION_CODE"].ToString(),
                                            DIVISION_NAME = row["Location"].ToString(),

                                        }).ToList();

            return doctorHonorariumPaidList;
        }

        public List<DoctorHonorariumPaidModel> GetManagement(int managementCode)
        {
            string qry = "";
               //qry = "SELECT MT.MGMT_CODE,MT.MGMT_NAME,MT.GRP_DSIG_CODE,GD.GRP_DSIG_NAME FROM MANAGEMENT_TEAM MT,GRP_DSIG GD WHERE MT.GRP_DSIG_CODE = GD.GRP_DSIG_CODE AND MT.MGMT_CODE =" + managementCode + " ORDER BY MGMT_CODE ";
            //qry = "SELECT MT.MGMT_CODE,MT.MGMT_NAME,MT.GRP_DSIG_CODE,GD.GRP_DSIG_NAME, MT.LOCATION_CODE,R.REGION_NAME  "+
            //      " FROM MANAGEMENT_TEAM MT,GRP_DSIG GD, REGION R WHERE MT.GRP_DSIG_CODE = GD.GRP_DSIG_CODE AND MT.LOCATION_CODE= R.REGION_CODE AND MT.MGMT_CODE =" + managementCode + "  ORDER BY MGMT_CODE";

            qry = "SELECT MT.MGMT_CODE,MT.MGMT_NAME,MT.GRP_DSIG_CODE,GD.GRP_DSIG_NAME, MT.LOCATION_CODE,D.DIVISION_NAME,R.REGION_NAME FROM MANAGEMENT_TEAM MT " +
                  "  LEFT JOIN GRP_DSIG GD on MT.GRP_DSIG_CODE = GD.GRP_DSIG_CODE " +
                  "  LEFT JOIN REGION R on MT.LOCATION_CODE = R.REGION_CODE " +
                  "  LEFT JOIN DIVISION D on MT.LOCATION_CODE = D.DIVISION_CODE " +
                  "  Where MT.MGMT_CODE = " + managementCode + " ORDER BY MGMT_CODE";

            //qry = qry + " ";

            DataTable dt = _dbHelper.GetDataTable(qry);

            doctorHonorariumPaidList = (from DataRow row in dt.Rows
                                        select new DoctorHonorariumPaidModel
                                        {
                                            MGMT_CODE = row["MGMT_CODE"].ToString(),
                                            MGMT_NAME = row["MGMT_NAME"].ToString(),
                                            GRP_DSIG_CODE = row["GRP_DSIG_CODE"].ToString(),
                                            GRP_DSIG_NAME = row["GRP_DSIG_NAME"].ToString(),
                                            REGION_CODE1 = row["LOCATION_CODE"].ToString(),
                                            REGION_NAME1 = row["REGION_NAME"].ToString(),
                                            DIVISION_CODE1 = row["LOCATION_CODE"].ToString(),
                                            DIVISION_NAME1 = row["DIVISION_NAME"].ToString(),
                                            

                                        }).ToList();

            return doctorHonorariumPaidList;
        }

        //public List<DoctorHonorariumPaidModel> GetDoctorList()
        //{
        //    //string Qry = "";
        //    //Qry = "SELECT DOCTOR_ID,DOCTOR_NAME FROM Doctor ";
        //    //Qry = Qry + " ORDER BY DOCTOR_ID";

        //    string Qry = "SELECT * FROM DOCTOR_DEGREE";
        //    DataTable dt = _dbHelper.GetDataTable(Qry);

        //    doctorHonorariumPaidList = (from DataRow row in dt.Rows
        //                                select new DoctorHonorariumPaidModel
        //                                {
        //                                    DOCTOR_ID = row["DEGREE_CODE"].ToString(),
        //                                    DOCTOR_NAME = row["DEGREE"].ToString()
        //                                    //,
        //                                    //DEGREE = row["DEGREE"].ToString(),
        //                                    //DESIGNATION = row["DESIGNATION"].ToString()
        //                                    //,
        //                                    //DOCTOR_ID = row["DOCTOR_ID"].ToString(),
        //                                    //DOCTOR_NAME = row["DOCTOR_NAME"].ToString()
        //                                }).ToList();

        //    return doctorHonorariumPaidList;
        //}


        public List<DoctorInformationModel> GetDoctorListRSMandDSM(int doctorId, string doctorName, string degree, string regionCode, string DivisionCode, int userRole, int userID, int groupDsigCode)
        {
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();

            string query;

            
                 query = "SELECT M.REGION_CODE,C.REGISTRATION_NO,C.POTENTIAL_CATEGORY, NVL(C.DEGREE_CODE,0)DEGREE_CODE, NVL(D.SPECIALIZATION_CODE,0)SPECIALIZATION_CODE,NVL(C.DESIGNATION_CODE,0)DESIGNATION_CODE,b.prac_mkt_code,M.MARKET_NAME,a.doctor_id,(RTRIM (c.honorarium) || ' ' || c.doctor_name) name, " +
                               " c.degree,c.designation,d.specialization,c.address1,c.address2,c.address3,c.address4, M.MARKET_CODE, M.MARKET_NAME,c.potential_category,B.ENTRY_DATE " +
                               " FROM doc_mkt_mas a,doc_mkt_dtl b,doctor c,doctor_specialization d,LOCATION_VUE m " +
                               " WHERE a.doc_mkt_mas_slno = b.doc_mkt_mas_slno AND a.doctor_id = c.doctor_id " +
                               " AND b.prac_mkt_code = M.MARKET_CODE AND c.specia_1st_code = d.specialization_code(+) ";

                 if ((doctorName != "") && (degree != ""))
                 {
                     query += " AND UPPER(name) LIKE '%" + doctorName.ToUpper() + "%' AND UPPER(DEGREE) LIKE '%" + degree.ToUpper() + "%'";
                 }
                 if (doctorName != "")
                 {
                     query += " AND UPPER(c.doctor_name) LIKE '%" + doctorName.ToUpper() + "%'";
                 }
                 if (doctorId != 0)
                 {
                     query += " AND C.DOCTOR_ID=" + doctorId + "";
                 }
                 if (userRole == 15)
                 {
                     if (groupDsigCode == 5)
                     {
                         query += " AND M.DIVISION_CODE = '" + DivisionCode + "'";
                     }
                     else
                     {
                         query += " AND M.REGION_CODE = '" + regionCode + "'";
                     }

                     //if ((userID == 1053) || (userID == 1013) || (userID == 1813) || (userID == 513) || (userID == 1488) || (userID == 743) || (userID == 663) || (userID == 1090) || (userID == 664) || (userID == 2164))
                     //{
                     //    query += " AND M.DIVISION_CODE = '" + DivisionCode + "'";
                     //}

                     //else
                     //{
                     //    query += " AND M.REGION_CODE = '" + regionCode + "'";
                     //}
                 }

                 query += " ORDER BY M.ZONE_CODE, M.DIVISION_CODE,M.REGION_CODE,M.TERRITORY_CODE,M.MARKET_CODE";
            



            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.RegistrationNo = reader["REGISTRATION_NO"].ToString();
                        model.PotentialCategory = reader["POTENTIAL_CATEGORY"].ToString();
                        //model.Honorium = reader["HONORARIUM"].ToString();
                        model.DoctorName = reader["name"].ToString();
                        model.Degree = reader["DEGREE"].ToString();
                        model.DegreeCategory = Convert.ToInt32(reader["DEGREE_CODE"]);
                        model.Designation = reader["DESIGNATION"].ToString();
                        model.DesignationCategory = Convert.ToInt32(reader["DESIGNATION_CODE"]);
                        model.SpeciFirstCode = Convert.ToInt32(reader["SPECIALIZATION_CODE"]);
                        model.SpeciFirstName = reader["specialization"].ToString();
                        model.MarketCode = reader["MARKET_CODE"].ToString();
                        model.MarketName = reader["MARKET_NAME"].ToString();
                        ////model.SpeciSecondCode = Convert.ToInt32(reader["SPECIA_2ND_CODE"]);
                        //model.SpeciSecondCode = reader["SPECIA_2ND_CODE"].ToString();
                        //model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        //model.Gender = reader["GENDER"].ToString();
                        //model.Religion = reader["RELIGION"].ToString();
                        //model.DateOfBirth = reader["DATE_OF_BIRTH"].ToString();
                        //model.Phone = reader["DOC_PERS_PHONE"].ToString();
                        //model.Email = reader["DOCTOR_EMAIL"].ToString();
                        //model.PatientNo = Convert.ToInt32(reader["PATIENT_PER_DAY"]);
                        //model.PrescriptionValue = Convert.ToInt32(reader["AVG_PRESC_VALUE"]);
                        //model.PrescriptionShare = Convert.ToInt32(reader["PRESC_SHARE"]);
                        model.Address1 = reader["ADDRESS1"].ToString();
                        model.Address2 = reader["ADDRESS2"].ToString();
                        //model.Address3 = reader["ADDRESS3"].ToString();
                        //model.Address4 = reader["ADDRESS4"].ToString();
                        //model.Remarks = reader["REMARKS"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<DoctorInformationModel> GetDoctorList(int doctorId, string doctorName, string degree)
        {
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();
            string query = "SELECT DOCTOR_ID,REGISTRATION_NO,POTENTIAL_CATEGORY,HONORARIUM,(RTRIM (honorarium) || ' ' || doctor_name) name,DEGREE,NVL(DEGREE_CODE,0)DEGREE_CODE," +
                          "DESIGNATION,NVL(DESIGNATION_CODE,0)DESIGNATION_CODE,NVL(SPECIA_1ST_CODE,0)SPECIA_1ST_CODE,NVL(SPECIA_2ND_CODE,0)SPECIA_2ND_CODE,GENDER,RELIGION," +
                          "TO_CHAR(DATE_OF_BIRTH,'dd/MM/yyyy')DATE_OF_BIRTH, DOC_PERS_PHONE,DOCTOR_EMAIL," +
                          "NVL(PATIENT_PER_DAY,0)PATIENT_PER_DAY,NVL(AVG_PRESC_VALUE,0)AVG_PRESC_VALUE,NVL(PRESC_SHARE,0)PRESC_SHARE,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4," +
                          "REMARKS,FSPECIALIZATION,SPECIALIZATION SSPECIALIZATION FROM " +
                          //"(SELECT DOCTOR.*,SPECIALIZATION FSPECIALIZATION FROM DOCTOR, DOCTOR_SPECIALIZATION WHERE DOCTOR.SPECIA_1ST_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE) DOCTORS " +
                          "(SELECT DOCTOR.*,SPECIALIZATION FSPECIALIZATION FROM DOCTOR Left Join DOCTOR_SPECIALIZATION ON  DOCTOR.SPECIA_1ST_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE) DOCTORS " +
                          "Left Join DOCTOR_SPECIALIZATION ON DOCTORS.SPECIA_2ND_CODE = DOCTOR_SPECIALIZATION.SPECIALIZATION_CODE Where 1=1 ";

            //string query = "SELECT M.REGION_CODE,C.REGISTRATION_NO,C.POTENTIAL_CATEGORY, NVL(C.DEGREE_CODE,0)DEGREE_CODE, NVL(D.SPECIALIZATION_CODE,0)SPECIALIZATION_CODE,NVL(C.DESIGNATION_CODE,0)DESIGNATION_CODE,b.prac_mkt_code,M.MARKET_NAME,a.doctor_id,(RTRIM (c.honorarium) || ' ' || c.doctor_name) name, " +
            //               " c.degree,c.designation,d.specialization,c.address1,c.address2,c.address3,c.address4,c.potential_category,B.ENTRY_DATE " +
            //               " FROM doc_mkt_mas a,doc_mkt_dtl b,doctor c,doctor_specialization d,LOCATION_VUE m " +
            //               " WHERE a.doc_mkt_mas_slno = b.doc_mkt_mas_slno AND a.doctor_id = c.doctor_id " +
            //               " AND b.prac_mkt_code = M.MARKET_CODE AND c.specia_1st_code = d.specialization_code(+) ";

            if ((doctorName != "") && (degree != "" ))
            {
                query += " AND UPPER(name) LIKE '%" + doctorName.ToUpper() + "%' AND UPPER(DEGREE) LIKE '%" + degree.ToUpper() + "%'";
            }
            else if (doctorName != "")
            {
                query += " AND UPPER(name) LIKE '%" + doctorName.ToUpper() + "%'";
            }
            else if (doctorId != 0)
            {
                query += " AND DOCTOR_ID=" + doctorId + "";
            }
            else
            {
                query = query;
            }
            //query += " ORDER BY M.ZONE_CODE, M.DIVISION_CODE,M.REGION_CODE,M.TERRITORY_CODE,M.MARKET_CODE";
            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.RegistrationNo = reader["REGISTRATION_NO"].ToString();
                        model.PotentialCategory = reader["POTENTIAL_CATEGORY"].ToString();
                        model.Honorium = reader["HONORARIUM"].ToString();
                        model.DoctorName = reader["name"].ToString();
                        model.Degree = reader["DEGREE"].ToString();
                        model.DegreeCategory = Convert.ToInt32(reader["DEGREE_CODE"]);
                        model.Designation = reader["DESIGNATION"].ToString();
                        model.DesignationCategory = Convert.ToInt32(reader["DESIGNATION_CODE"]);
                        model.SpeciFirstCode = Convert.ToInt32(reader["SPECIA_1ST_CODE"]);
                        model.SpeciFirstName = reader["FSPECIALIZATION"].ToString();
                        ////model.SpeciSecondCode = Convert.ToInt32(reader["SPECIA_2ND_CODE"]);
                        //model.SpeciSecondCode = reader["SPECIA_2ND_CODE"].ToString();
                        //model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        //model.Gender = reader["GENDER"].ToString();
                        //model.Religion = reader["RELIGION"].ToString();
                        //model.DateOfBirth = reader["DATE_OF_BIRTH"].ToString();
                        //model.Phone = reader["DOC_PERS_PHONE"].ToString();
                        //model.Email = reader["DOCTOR_EMAIL"].ToString();
                        //model.PatientNo = Convert.ToInt32(reader["PATIENT_PER_DAY"]);
                        //model.PrescriptionValue = Convert.ToInt32(reader["AVG_PRESC_VALUE"]);
                        //model.PrescriptionShare = Convert.ToInt32(reader["PRESC_SHARE"]);
                        model.Address1 = reader["ADDRESS1"].ToString();
                        model.Address2 = reader["ADDRESS2"].ToString();
                        //model.Address3 = reader["ADDRESS3"].ToString();
                        //model.Address4 = reader["ADDRESS4"].ToString();
                        //model.Remarks = reader["REMARKS"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }
        public List<DoctorInformationModel> GetDoctorListRSM(int doctorId, string doctorName, string degree, string regionCode)
        {
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();


            //string query = "SELECT M.REGION_CODE,C.REGISTRATION_NO,C.POTENTIAL_CATEGORY, C.DEGREE_CODE, D.SPECIALIZATION_CODE,C.DESIGNATION_CODE,b.prac_mkt_code,M.MARKET_NAME,a.doctor_id,(RTRIM (c.honorarium) || ' ' || c.doctor_name) name, " +
            //               " c.degree,c.designation,d.specialization,c.address1,c.address2,c.address3,c.address4,c.potential_category,B.ENTRY_DATE " +
            //               " FROM doc_mkt_mas a,doc_mkt_dtl b,doctor c,doctor_specialization d,LOCATION_VUE m " +
            //               " WHERE a.doc_mkt_mas_slno = b.doc_mkt_mas_slno AND a.doctor_id = c.doctor_id " +
            //               " AND b.prac_mkt_code = M.MARKET_CODE AND c.specia_1st_code = d.specialization_code(+) AND M.REGION_CODE = '" + regionCode + "'" +
            //               " ORDER BY M.ZONE_CODE, M.DIVISION_CODE,M.REGION_CODE,M.TERRITORY_CODE,M.MARKET_CODE";
            string query = "SELECT M.REGION_CODE,C.REGISTRATION_NO,C.POTENTIAL_CATEGORY, NVL(C.DEGREE_CODE,0)DEGREE_CODE, NVL(D.SPECIALIZATION_CODE,0)SPECIALIZATION_CODE,NVL(C.DESIGNATION_CODE,0)DESIGNATION_CODE,b.prac_mkt_code,M.MARKET_NAME,a.doctor_id,(RTRIM (c.honorarium) || ' ' || c.doctor_name) name, " +
               " c.degree,c.designation,d.specialization,c.address1,c.address2,c.address3,c.address4,c.potential_category,B.ENTRY_DATE " +
               " FROM doc_mkt_mas a,doc_mkt_dtl b,doctor c,doctor_specialization d,LOCATION_VUE m " +
               " WHERE a.doc_mkt_mas_slno = b.doc_mkt_mas_slno AND a.doctor_id = c.doctor_id " +
               " AND b.prac_mkt_code = M.MARKET_CODE AND c.specia_1st_code = d.specialization_code(+) ";
               //" AND M.REGION_CODE = '" + regionCode + "'" +
               //"Where 1=1";
            if ((doctorName != "" && doctorName != null) && (degree != "" && degree != null))
            {
                query += " AND UPPER(name) LIKE '%" + doctorName.ToUpper() + "%' AND UPPER(DEGREE) LIKE '%" + degree.ToUpper() + "%'";
            }
            if (doctorName != "" && doctorName != null)
            {
                query += " AND UPPER(name) LIKE '%" + doctorName.ToUpper() + "%'";
            }
            if (doctorId != 0)
            {
                query += " AND C.DOCTOR_ID=" + doctorId + "";
            }
            if (regionCode != "")
            {
                query += " AND M.REGION_CODE = '" + regionCode + "'";
            }

                query +=  " ORDER BY M.ZONE_CODE, M.DIVISION_CODE,M.REGION_CODE,M.TERRITORY_CODE,M.MARKET_CODE";



            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.RegistrationNo = reader["REGISTRATION_NO"].ToString();
                        model.PotentialCategory = reader["POTENTIAL_CATEGORY"].ToString();
                        //model.Honorium = reader["HONORARIUM"].ToString();
                        model.DoctorName = reader["name"].ToString();
                        model.Degree = reader["DEGREE"].ToString();
                        model.DegreeCategory = Convert.ToInt32(reader["DEGREE_CODE"]);
                        model.Designation = reader["DESIGNATION"].ToString();
                        model.DesignationCategory = Convert.ToInt32(reader["DESIGNATION_CODE"]);
                        model.SpeciFirstCode = Convert.ToInt32(reader["SPECIALIZATION_CODE"]);
                        model.SpeciFirstName = reader["specialization"].ToString();
                        ////model.SpeciSecondCode = Convert.ToInt32(reader["SPECIA_2ND_CODE"]);
                        //model.SpeciSecondCode = reader["SPECIA_2ND_CODE"].ToString();
                        //model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        //model.Gender = reader["GENDER"].ToString();
                        //model.Religion = reader["RELIGION"].ToString();
                        //model.DateOfBirth = reader["DATE_OF_BIRTH"].ToString();
                        //model.Phone = reader["DOC_PERS_PHONE"].ToString();
                        //model.Email = reader["DOCTOR_EMAIL"].ToString();
                        //model.PatientNo = Convert.ToInt32(reader["PATIENT_PER_DAY"]);
                        //model.PrescriptionValue = Convert.ToInt32(reader["AVG_PRESC_VALUE"]);
                        //model.PrescriptionShare = Convert.ToInt32(reader["PRESC_SHARE"]);
                        model.Address1 = reader["ADDRESS1"].ToString();
                        model.Address2 = reader["ADDRESS2"].ToString();
                        //model.Address3 = reader["ADDRESS3"].ToString();
                        //model.Address4 = reader["ADDRESS4"].ToString();
                        //model.Remarks = reader["REMARKS"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }

        public List<DoctorInformationModel> GetDoctorListDSM(int doctorId, string doctorName, string degree, string DivisionCode)
        {
            List<DoctorInformationModel> listData = new List<DoctorInformationModel>();


            //string query = "SELECT M.REGION_CODE,C.REGISTRATION_NO,C.POTENTIAL_CATEGORY, C.DEGREE_CODE, D.SPECIALIZATION_CODE,C.DESIGNATION_CODE,b.prac_mkt_code,M.MARKET_NAME,a.doctor_id,(RTRIM (c.honorarium) || ' ' || c.doctor_name) name, " +
            //               " c.degree,c.designation,d.specialization,c.address1,c.address2,c.address3,c.address4,c.potential_category,B.ENTRY_DATE " +
            //               " FROM doc_mkt_mas a,doc_mkt_dtl b,doctor c,doctor_specialization d,LOCATION_VUE m " +
            //               " WHERE a.doc_mkt_mas_slno = b.doc_mkt_mas_slno AND a.doctor_id = c.doctor_id " +
            //               " AND b.prac_mkt_code = M.MARKET_CODE AND c.specia_1st_code = d.specialization_code(+) AND M.REGION_CODE = '" + regionCode + "'" +
            //               " ORDER BY M.ZONE_CODE, M.DIVISION_CODE,M.REGION_CODE,M.TERRITORY_CODE,M.MARKET_CODE";
            string query = "SELECT M.REGION_CODE,C.REGISTRATION_NO,C.POTENTIAL_CATEGORY, NVL(C.DEGREE_CODE,0)DEGREE_CODE, NVL(D.SPECIALIZATION_CODE,0)SPECIALIZATION_CODE,NVL(C.DESIGNATION_CODE,0)DESIGNATION_CODE,b.prac_mkt_code,M.MARKET_NAME,a.doctor_id,(RTRIM (c.honorarium) || ' ' || c.doctor_name) name, " +
               " c.degree,c.designation,d.specialization,c.address1,c.address2,c.address3,c.address4,c.potential_category,B.ENTRY_DATE " +
               " FROM doc_mkt_mas a,doc_mkt_dtl b,doctor c,doctor_specialization d,LOCATION_VUE m " +
               " WHERE a.doc_mkt_mas_slno = b.doc_mkt_mas_slno AND a.doctor_id = c.doctor_id " +
               " AND b.prac_mkt_code = M.MARKET_CODE AND c.specia_1st_code = d.specialization_code(+) ";
               //" AND M.REGION_CODE = '" + regionCode + "'" +
               //"Where 1=1";
            if ((doctorName != "" && doctorName != null) && (degree != "" && degree != null))
            {
                query += " AND UPPER(name) LIKE '%" + doctorName.ToUpper() + "%' AND UPPER(DEGREE) LIKE '%" + degree.ToUpper() + "%'";
            }
            if (doctorName != "" && doctorName != null)
            {
                query += " AND UPPER(name) LIKE '%" + doctorName.ToUpper() + "%'";
            }
            if (doctorId != 0)
            {
                query += " AND C.DOCTOR_ID=" + doctorId + "";
            }
            if (DivisionCode != "")
            {
                query += " AND M.DIVISION_CODE = '" + DivisionCode + "'";
            }

                query +=  " ORDER BY M.ZONE_CODE, M.DIVISION_CODE,M.REGION_CODE,M.TERRITORY_CODE,M.MARKET_CODE";



            using (OracleConnection con = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand cmd = new OracleCommand(query, con);
                con.Open();
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DoctorInformationModel model = new DoctorInformationModel();
                        model.DoctorId = Convert.ToInt32(reader["DOCTOR_ID"]);
                        model.RegistrationNo = reader["REGISTRATION_NO"].ToString();
                        model.PotentialCategory = reader["POTENTIAL_CATEGORY"].ToString();
                        //model.Honorium = reader["HONORARIUM"].ToString();
                        model.DoctorName = reader["name"].ToString();
                        model.Degree = reader["DEGREE"].ToString();
                        model.DegreeCategory = Convert.ToInt32(reader["DEGREE_CODE"]);
                        model.Designation = reader["DESIGNATION"].ToString();
                        model.DesignationCategory = Convert.ToInt32(reader["DESIGNATION_CODE"]);
                        model.SpeciFirstCode = Convert.ToInt32(reader["SPECIALIZATION_CODE"]);
                        model.SpeciFirstName = reader["specialization"].ToString();
                        ////model.SpeciSecondCode = Convert.ToInt32(reader["SPECIA_2ND_CODE"]);
                        //model.SpeciSecondCode = reader["SPECIA_2ND_CODE"].ToString();
                        //model.SpeciSecondName = reader["SSPECIALIZATION"].ToString();
                        //model.Gender = reader["GENDER"].ToString();
                        //model.Religion = reader["RELIGION"].ToString();
                        //model.DateOfBirth = reader["DATE_OF_BIRTH"].ToString();
                        //model.Phone = reader["DOC_PERS_PHONE"].ToString();
                        //model.Email = reader["DOCTOR_EMAIL"].ToString();
                        //model.PatientNo = Convert.ToInt32(reader["PATIENT_PER_DAY"]);
                        //model.PrescriptionValue = Convert.ToInt32(reader["AVG_PRESC_VALUE"]);
                        //model.PrescriptionShare = Convert.ToInt32(reader["PRESC_SHARE"]);
                        model.Address1 = reader["ADDRESS1"].ToString();
                        model.Address2 = reader["ADDRESS2"].ToString();
                        //model.Address3 = reader["ADDRESS3"].ToString();
                        //model.Address4 = reader["ADDRESS4"].ToString();
                        //model.Remarks = reader["REMARKS"].ToString();
                        listData.Add(model);
                    }
                }
            }
            return listData;
        }
        


        public List<DoctorHonorariumPaidModel> GetHonarariumTypeList()
        {
            string qry = "";
            qry = "SELECT HONORARIUM_CODE,HONORARIUM_NAME FROM HONORARIUM_TYPE ";
            qry = qry + " ORDER BY HONORARIUM_CODE";

            DataTable dt = _dbHelper.GetDataTable(qry);

            doctorHonorariumPaidList = (from DataRow row in dt.Rows
                                        select new DoctorHonorariumPaidModel
                                        {
                                            HONORARIUM_CODE = row["HONORARIUM_CODE"].ToString(),
                                            HONORARIUM_NAME = row["HONORARIUM_NAME"].ToString()
                                        }).ToList();

            return doctorHonorariumPaidList;
        }
        public List<DoctorHonorariumPaidModel> GetHonarariumTypeList(string GRP_DSIG_CODE)
        {
            string Qry = "SELECT HWC.HONORARIUM_CODE,HT.HONORARIUM_NAME FROM HON_WISE_CEILING HWC,HONORARIUM_TYPE HT WHERE HWC.HONORARIUM_CODE = HT.HONORARIUM_CODE";   // AND HWC.GRP_DSIG_CODE = '" + GRP_DSIG_CODE + "'";

            DataTable dt = _dbHelper.GetDataTable(Qry);

            doctorHonorariumPaidList = (from DataRow row in dt.Rows
                                        select new DoctorHonorariumPaidModel
                                        {
                                            HONORARIUM_CODE = row["HONORARIUM_CODE"].ToString(),
                                            HONORARIUM_NAME = row["HONORARIUM_NAME"].ToString()
                                        }).ToList();

            return doctorHonorariumPaidList;
        }
        public List<DoctorHonorariumPaidModel> GetSocietyAssociationList()
        {
            string Qry = "SELECT SOCIETY_ID,SOCITY_NAME FROM DOCTOR_SOCITY ORDER BY SOCITY_NAME";

            DataTable dt = _dbHelper.GetDataTable(Qry);

            doctorHonorariumPaidList = (from DataRow row in dt.Rows
                                        select new DoctorHonorariumPaidModel
                                        {
                                            SOCIETY_ID = row["SOCIETY_ID"].ToString(),
                                            SOCITY_NAME = row["SOCITY_NAME"].ToString()
                                        }).ToList();

            return doctorHonorariumPaidList;
        }
        public string GetCeilingAmount(string MGMT_CODE, string HONORARIUM_CODE, string GRP_DSIG_CODE, string month, string year)
        {
            string qry = "SELECT HWC.CEILING_AMT,NVL(MTA.TOT_APPROVED_AMT,0)TOT_APPROVED_AMT FROM HON_WISE_CEILING HWC,MONTHLY_TOT_APPROVED_AMT MTA " +
            "WHERE HWC.HONORARIUM_CODE = MTA.HONORARIUM_CODE(+) AND MTA.MONTH (+) = '" + month + "' AND MTA.YEAR (+) = '" + year + "' AND MTA.MGMT_CODE(+) = '" + MGMT_CODE + "' AND HWC.HONORARIUM_CODE(+) = '" + HONORARIUM_CODE + "' AND HWC.GRP_DSIG_CODE = " + GRP_DSIG_CODE + "";

            DataTable dt = _dbHelper.GetDataTable(qry);
            if (dt.Rows.Count > 0)
            {
                string ceilingAmount = dt.Rows[0]["CEILING_AMT"].ToString();
                string approvedAmount = dt.Rows[0]["TOT_APPROVED_AMT"].ToString();
                string dueAmount = (Convert.ToInt64(ceilingAmount) - Convert.ToInt64(approvedAmount)).ToString();
                return ceilingAmount + "/" + approvedAmount + "/" + dueAmount;
            }
            else
                return string.Empty;
        }
        public List<DoctorHonorariumPaidModel> GetProductList()
        {
            string Qry = "SELECT PROD_ID,SAP_CODE,PRODUCT_NAME FROM PRODUCT where MANUFACTURER_CODE ='SQA' ORDER BY PROD_ID";

            DataTable dt = _dbHelper.GetDataTable(Qry);

            doctorHonorariumPaidList = (from DataRow row in dt.Rows
                                        select new DoctorHonorariumPaidModel
                                        {
                                            PROD_ID = row["PROD_ID"].ToString(),
                                            SAP_CODE = row["SAP_CODE"].ToString(),
                                            PRODUCT_NAME = row["PRODUCT_NAME"].ToString()
                                        }).ToList();

            return doctorHonorariumPaidList;
        }
        public List<DoctorHonorariumPaidModel> GetInstitutionList(string regionCode, string DivisionCode, int userID, int roleID, int groupDsigCode)
        {
            string qry = "";
            if(roleID == 15)
            {
                if (groupDsigCode == 5)
                {
                    qry = "SELECT A.INSTI_CODE,A.INSTI_NAME, C.MARKET_CODE, E.MARKET_NAME, E.REGION_CODE,E.REGION_NAME FROM INSTITUTION A, INSTI_MKT_MAS B, INSTI_MKT_DTL C, LOCATION_VUE E " +
                  "  where B.INSTI_MKT_MAS_SLNO = C.INSTI_MKT_MAS_SLNO " +
                   " AND B.INSTI_CODE = A.INSTI_CODE " +
                   " AND C.MARKET_CODE = E.MARKET_CODE " +
                   " AND E.DIVISION_CODE = '" + DivisionCode + "'";
                }
                else
                {
                     qry = "SELECT A.INSTI_CODE,A.INSTI_NAME, C.MARKET_CODE, E.MARKET_NAME, E.REGION_CODE,E.REGION_NAME FROM INSTITUTION A, INSTI_MKT_MAS B, INSTI_MKT_DTL C, LOCATION_VUE E " +
                  "  where B.INSTI_MKT_MAS_SLNO = C.INSTI_MKT_MAS_SLNO " +
                   " AND B.INSTI_CODE = A.INSTI_CODE " +
                   " AND C.MARKET_CODE = E.MARKET_CODE " +
                   " AND E.REGION_CODE = '" + regionCode + "'";
                }
                //if ((userID == 1053) || (userID == 1013) || (userID == 1813) || (userID == 513) || (userID == 1488) || (userID == 743) || (userID == 663) || (userID == 1090) || (userID == 664) || (userID == 2164))
                //{
                //    qry = "SELECT A.INSTI_CODE,A.INSTI_NAME, C.MARKET_CODE, E.MARKET_NAME, E.REGION_CODE,E.REGION_NAME FROM INSTITUTION A, INSTI_MKT_MAS B, INSTI_MKT_DTL C, LOCATION_VUE E " +
                //  "  where B.INSTI_MKT_MAS_SLNO = C.INSTI_MKT_MAS_SLNO " +
                //   " AND B.INSTI_CODE = A.INSTI_CODE " +
                //   " AND C.MARKET_CODE = E.MARKET_CODE " +
                //   " AND E.DIVISION_CODE = '" + DivisionCode + "'";
                //}
                //else{
                //    qry = "SELECT A.INSTI_CODE,A.INSTI_NAME, C.MARKET_CODE, E.MARKET_NAME, E.REGION_CODE,E.REGION_NAME FROM INSTITUTION A, INSTI_MKT_MAS B, INSTI_MKT_DTL C, LOCATION_VUE E " +
                //  "  where B.INSTI_MKT_MAS_SLNO = C.INSTI_MKT_MAS_SLNO " +
                //   " AND B.INSTI_CODE = A.INSTI_CODE " +
                //   " AND C.MARKET_CODE = E.MARKET_CODE " +
                //   " AND E.REGION_CODE = '" + regionCode + "'";
                //}
                
            }
            else
            {
                //qry = "SELECT A.INSTI_CODE,A.INSTI_NAME, C.MARKET_CODE, E.MARKET_NAME, E.REGION_CODE,E.REGION_NAME FROM INSTITUTION A, INSTI_MKT_MAS B, INSTI_MKT_DTL C, LOCATION_VUE E " +
                //  "  where B.INSTI_MKT_MAS_SLNO = C.INSTI_MKT_MAS_SLNO " +
                //   " AND B.INSTI_CODE = A.INSTI_CODE " +
                //   " AND C.MARKET_CODE = E.MARKET_CODE ";
                qry = "SELECT INSTI_CODE,INSTI_NAME FROM INSTITUTION ";
            }
                   
            //qry = "SELECT INSTI_CODE,INSTI_NAME FROM INSTITUTION";
            qry = qry + " ORDER BY INSTI_CODE";

            DataTable dt = _dbHelper.GetDataTable(qry);

            doctorHonorariumPaidList = (from DataRow row in dt.Rows
                                        select new DoctorHonorariumPaidModel
                                        {
                                            INSTI_CODE = row["INSTI_CODE"].ToString(),
                                            INSTI_NAME = row["INSTI_NAME"].ToString()
                                        }).ToList();

            return doctorHonorariumPaidList;
        }

        //****************************** END  LIST OF VALOE/POP UP **************************//

        public List<DoctorHonorariumPaidModel> GetSearchList(string approvNo, int doctorId, string doctorName, int donAmount, int roleID, int userID, int groupDsigCode)
        {
            string qry = "";
            if (roleID == 15)
            {
                //qry = "SELECT DH.HONOR_PAID_SLNO, DH.HONOR_APROV_NO, DH.HONOR_APROV_DATE, DH.NATIONAL, DH.SOCIETY_ID,DC.SOCITY_NAME, DH.EXPENSE_DETAILS, DH.EXPENSE_AMT, DH.EXPENSE_FROM, DH.EXPENSE_TO, DH.PRESC_SHARE_PCT, " +
                //       "DH.PRESC_SHARE_FROM, DH.PRESC_SHARE_TO, DH.COMITMENT_SHARE_PCT, DH.HONORARIUM_DURATION, DH.DURATION_FROM, DH.DURATION_TO, DH.MON_CELI_AMT, DH.TOT_APPROVED_AMT,DH.TOT_DUE_AMT, " +
                //       "DH.EXPENSE_DURATION, DH.PRESENT_SHARE_DURATION, DH.MGMT_CODE, MT.MGMT_NAME, DH.DOCTOR_ID, DOC.DOCTOR_NAME, DH.HONORARIUM_CODE, HT.HONORARIUM_NAME, DH.INSTI_CODE, I.INSTI_NAME, DH.OTHER_REMARKS, DH.ADDITIONAL_COMMITMENT, " +
                //       "DH.ZONE_CODE, Z.ZONE_NAME, DH.DIVISION_CODE, D.DIVISION_NAME, DH.REGION_CODE, R.REGION_NAME, DH.TERRITORY_CODE, T.TERRITORY_NAME, DH.MARKET_CODE, M.MARKET_NAME, DH.GRP_DSIG_CODE, DH.ENTERED_BY " +
                //       "FROM DOC_HONOR_PAID_INFO DH, MANAGEMENT_TEAM MT, Doctor Doc, HONORARIUM_TYPE HT, INSTITUTION I, ZONE Z, DIVISION D, REGION R, TERRITORY T, MARKET M, DOCTOR_SOCITY DC " +
                //       "WHERE DH.MGMT_CODE = MT.MGMT_CODE(+) AND DH.DOCTOR_ID = Doc.DOCTOR_ID(+) AND DH.HONORARIUM_CODE = HT.HONORARIUM_CODE(+)AND DH.INSTI_CODE = I.INSTI_CODE(+) " +
                //       "AND DH.ZONE_CODE = Z.ZONE_CODE(+) AND DH.DIVISION_CODE = D.DIVISION_CODE(+) AND DH.REGION_CODE = R.REGION_CODE(+) AND DH.TERRITORY_CODE = T.TERRITORY_CODE(+) AND DH.MARKET_CODE = M.MARKET_CODE(+) AND DH.SOCIETY_ID = DC.SOCIETY_ID(+) "+
                //       "AND DH.HONOR_APROV_DATE BETWEEN ADD_MONTHS(TRUNC(SYSDATE, 'MM'), -13) AND LAST_DAY(TRUNC(SYSDATE, 'MM')) AND DH.ENTERED_BY = '" + userID + "' order by DH.HONOR_PAID_SLNO desc";


                qry = "SELECT DH.HONOR_PAID_SLNO, DH.HONOR_APROV_NO, DH.HONOR_APROV_DATE, DH.NATIONAL, DH.SOCIETY_ID,DC.SOCITY_NAME, DH.EXPENSE_DETAILS, DH.EXPENSE_AMT, DH.EXPENSE_FROM, DH.EXPENSE_TO, DH.PRESC_SHARE_PCT, " +
                       "DH.PRESC_SHARE_FROM, DH.PRESC_SHARE_TO, DH.COMITMENT_SHARE_PCT, DH.HONORARIUM_DURATION, DH.DURATION_FROM, DH.DURATION_TO, DH.MON_CELI_AMT, DH.TOT_APPROVED_AMT,DH.TOT_DUE_AMT, " +
                       "DH.EXPENSE_DURATION, DH.PRESENT_SHARE_DURATION, DH.MGMT_CODE, MT.MGMT_NAME, DH.DOCTOR_ID, DOC.DOCTOR_NAME, DH.HONORARIUM_CODE, HT.HONORARIUM_NAME, DH.INSTI_CODE, I.INSTI_NAME, DH.OTHER_REMARKS, DH.ADDITIONAL_COMMITMENT, " +
                       "DH.ZONE_CODE, Z.ZONE_NAME, DH.DIVISION_CODE, D.DIVISION_NAME, DH.REGION_CODE, R.REGION_NAME, DH.TERRITORY_CODE, T.TERRITORY_NAME, DH.MARKET_CODE, M.MARKET_NAME, DH.GRP_DSIG_CODE, DH.ENTERED_BY " +
                       "FROM DOC_HONOR_PAID_INFO DH, MANAGEMENT_TEAM MT, Doctor Doc, HONORARIUM_TYPE HT, INSTITUTION I, ZONE Z, DIVISION D, REGION R, TERRITORY T, MARKET M, DOCTOR_SOCITY DC " +
                       "WHERE DH.MGMT_CODE = MT.MGMT_CODE(+) AND DH.DOCTOR_ID = Doc.DOCTOR_ID(+) AND DH.HONORARIUM_CODE = HT.HONORARIUM_CODE(+)AND DH.INSTI_CODE = I.INSTI_CODE(+) " +
                       "AND DH.ZONE_CODE = Z.ZONE_CODE(+) AND DH.DIVISION_CODE = D.DIVISION_CODE(+) AND DH.REGION_CODE = R.REGION_CODE(+) AND DH.TERRITORY_CODE = T.TERRITORY_CODE(+) AND DH.MARKET_CODE = M.MARKET_CODE(+) AND DH.SOCIETY_ID = DC.SOCIETY_ID(+) " +
                       "AND DH.ENTERED_BY = '" + userID + "'";
                        if (doctorName != "" && doctorName != null)
                        {
                            qry += " AND UPPER(DOC.DOCTOR_NAME) LIKE '%" + doctorName.ToUpper() + "%'";
                        }

                        if (doctorId != 0)
                        {
                            qry += " AND DH.DOCTOR_ID =" + doctorId + "";
                        }

                        if (approvNo != "")
                        {
                            qry += " AND DH.HONOR_APROV_NO = '" + approvNo + "'";
                        }

                        if (donAmount != 0)
                        {
                            qry += " AND DH.EXPENSE_AMT = " + donAmount + "";
                        }

                        qry += " ORDER By DH.HONOR_PAID_SLNO desc";
            }
            else
            {
                qry = "SELECT DH.HONOR_PAID_SLNO, DH.HONOR_APROV_NO, DH.HONOR_APROV_DATE, DH.NATIONAL, DH.SOCIETY_ID,DC.SOCITY_NAME, DH.EXPENSE_DETAILS, DH.EXPENSE_AMT, DH.EXPENSE_FROM, DH.EXPENSE_TO, DH.PRESC_SHARE_PCT, " +
                      "DH.PRESC_SHARE_FROM, DH.PRESC_SHARE_TO, DH.COMITMENT_SHARE_PCT, DH.HONORARIUM_DURATION, DH.DURATION_FROM, DH.DURATION_TO, DH.MON_CELI_AMT, DH.TOT_APPROVED_AMT,DH.TOT_DUE_AMT, " +
                      "DH.EXPENSE_DURATION, DH.PRESENT_SHARE_DURATION, DH.MGMT_CODE, MT.MGMT_NAME, DH.DOCTOR_ID, DOC.DOCTOR_NAME, DH.HONORARIUM_CODE, HT.HONORARIUM_NAME, DH.INSTI_CODE, I.INSTI_NAME, DH.OTHER_REMARKS, DH.ADDITIONAL_COMMITMENT, " +
                      "DH.ZONE_CODE, Z.ZONE_NAME, DH.DIVISION_CODE, D.DIVISION_NAME, DH.REGION_CODE, R.REGION_NAME, DH.TERRITORY_CODE, T.TERRITORY_NAME, DH.MARKET_CODE, M.MARKET_NAME, DH.GRP_DSIG_CODE, DH.ENTERED_BY " +
                      "FROM DOC_HONOR_PAID_INFO DH, MANAGEMENT_TEAM MT, Doctor Doc, HONORARIUM_TYPE HT, INSTITUTION I, ZONE Z, DIVISION D, REGION R, TERRITORY T, MARKET M, DOCTOR_SOCITY DC " +
                      "WHERE DH.MGMT_CODE = MT.MGMT_CODE(+) AND DH.DOCTOR_ID = Doc.DOCTOR_ID(+) AND DH.HONORARIUM_CODE = HT.HONORARIUM_CODE(+)AND DH.INSTI_CODE = I.INSTI_CODE(+) " +
                      "AND DH.ZONE_CODE = Z.ZONE_CODE(+) AND DH.DIVISION_CODE = D.DIVISION_CODE(+) AND DH.REGION_CODE = R.REGION_CODE(+) AND DH.TERRITORY_CODE = T.TERRITORY_CODE(+) AND DH.MARKET_CODE = M.MARKET_CODE(+) AND DH.SOCIETY_ID = DC.SOCIETY_ID(+) ";
                //if ((doctorName != "" && doctorName != null) && (degree != "" && degree != null))
                //{
                //    qry += " AND UPPER(name) LIKE '%" + doctorName.ToUpper() + "%' AND UPPER(DEGREE) LIKE '%" + degree.ToUpper() + "%'";
                //}
                if (doctorName != "" && doctorName != null)
                {
                    qry += " AND UPPER(DOC.DOCTOR_NAME) LIKE '%" + doctorName.ToUpper() + "%'";
                }

                if (doctorId != 0)
                {
                    qry += " AND DH.DOCTOR_ID =" + doctorId + "";
                }

                if (approvNo != "")
                {
                    qry += " AND DH.HONOR_APROV_NO = '" + approvNo + "'";
                }

                if (donAmount != 0)
                {
                    qry += " AND DH.EXPENSE_AMT = " + donAmount + "";
                }


                qry += " ORDER By DH.HONOR_PAID_SLNO desc";
                       //"AND DH.HONOR_APROV_DATE BETWEEN ADD_MONTHS(TRUNC(SYSDATE, 'MM'), -13) AND LAST_DAY(TRUNC(SYSDATE, 'MM')) order by DH.HONOR_PAID_SLNO desc";
            }

            //DataTable dt = _dbHelper.GetDataTable(qry);
            OracleConnection con = new OracleConnection(dbConnection.StringRead());
            con.Open();
            OracleCommand cmd = new OracleCommand(qry, con);
            OracleDataReader reader = cmd.ExecuteReader();
            List<DoctorHonorariumPaidModel> items = new List<DoctorHonorariumPaidModel>();
            //items = (from DataRow row in dt.Rows
            //         select new DoctorHonorariumPaidModel
            while (reader.Read())
                     {
                        DoctorHonorariumPaidModel modelData = new DoctorHonorariumPaidModel();
                         //foreach(DataRow row in dt.Rows)
                         //{
                         //    DoctorHonorariumPaidModel objDoctorHonorariumPaidModel = new DoctorHonorariumPaidModel();
                         //objDoctorHonorariumPaidModel.HONOR_PAID_SLNO = row["HONOR_PAID_SLNO"].ToString();
                         modelData.HONOR_PAID_SLNO = reader["HONOR_PAID_SLNO"].ToString();
                         modelData.HONOR_APROV_NO = reader["HONOR_APROV_NO"].ToString();
                         modelData.HONOR_APROV_DATE = Convert.ToDateTime(reader["HONOR_APROV_DATE"]).ToString("dd/MM/yyyy");
                         modelData.MGMT_CODE = reader["MGMT_CODE"].ToString();// == string.IsNullOrEmpty(row["MGMT_CODE"].ToString()) ? string.Empty : row["MGMT_CODE"].ToString(),
                         modelData.MGMT_NAME = reader["MGMT_NAME"].ToString();
                         modelData.MARKET_CODE = reader["MARKET_CODE"].ToString();
                         modelData.MARKET_NAME = reader["MARKET_NAME"].ToString();
                         modelData.TERRITORY_CODE = reader["TERRITORY_CODE"].ToString();
                         modelData.TERRITORY_NAME = reader["TERRITORY_NAME"].ToString();
                         modelData.REGION_CODE = reader["REGION_CODE"].ToString();
                         modelData.REGION_NAME = reader["REGION_NAME"].ToString();
                         modelData.ZONE_CODE = reader["ZONE_CODE"].ToString();
                         modelData.ZONE_NAME = reader["ZONE_NAME"].ToString();
                         modelData.DIVISION_CODE = reader["DIVISION_CODE"].ToString();
                         modelData.DIVISION_NAME = reader["DIVISION_NAME"].ToString();
                         modelData.NATIONAL = reader["NATIONAL"].ToString();
                         modelData.SOCIETY_ID = reader["SOCIETY_ID"].ToString();
                         modelData.SOCITY_NAME = reader["SOCITY_NAME"].ToString();
                         modelData.INSTI_CODE = reader["INSTI_CODE"].ToString();
                         modelData.INSTI_NAME = reader["INSTI_NAME"].ToString();
                         modelData.DOCTOR_ID = reader["DOCTOR_ID"].ToString();
                         modelData.DOCTOR_NAME = reader["DOCTOR_NAME"].ToString();
                         modelData.MON_CELI_AMT = reader["MON_CELI_AMT"].ToString();
                         modelData.TOT_APPROVED_AMT = reader["TOT_APPROVED_AMT"].ToString();
                         modelData.TOT_DUE_AMT = reader["TOT_DUE_AMT"].ToString();
                         modelData.EXPENSE_DURATION = reader["EXPENSE_DURATION"].ToString();
                         modelData.PRESENT_SHARE_DURATION = reader["PRESENT_SHARE_DURATION"].ToString();
                         modelData.HONORARIUM_CODE = reader["HONORARIUM_CODE"].ToString();
                         modelData.HONORARIUM_NAME = reader["HONORARIUM_NAME"].ToString();
                         modelData.EXPENSE_DETAILS = reader["EXPENSE_DETAILS"].ToString();
                         modelData.EXPENSE_AMT = reader["EXPENSE_AMT"].ToString();
                         //modelData.EXPENSE_FROM = Convert.ToDateTime(reader["EXPENSE_FROM"]).ToString("dd/MM/yyyy");//row["EXPENSE_FROM"].ToString(),
                         //modelData.EXPENSE_TO = Convert.ToDateTime(reader["EXPENSE_TO"]).ToString("dd/MM/yyyy");//row["EXPENSE_TO"].ToString(),
                         modelData.PRESC_SHARE_PCT = reader["PRESC_SHARE_PCT"].ToString();
                         if (!reader.IsDBNull(8))
                         {
                             modelData.EXPENSE_FROM = Convert.ToDateTime(reader.GetDateTime(8)).ToString("dd/MM/yyyy");

                         }
                         else
                         {
                             modelData.EXPENSE_FROM = "";
                         }

                         if (!reader.IsDBNull(9))
                         {
                             modelData.EXPENSE_TO = Convert.ToDateTime(reader.GetDateTime(9)).ToString("dd/MM/yyyy");

                         }
                         else
                         {
                             modelData.EXPENSE_TO = "";
                         }


                        if (!reader.IsDBNull(11))
                        {
                            modelData.PRESC_SHARE_FROM = Convert.ToDateTime(reader.GetDateTime(11)).ToString("dd/MM/yyyy");

                        }
                        else{
                            modelData.PRESC_SHARE_FROM = "";
                        }

                        if (!reader.IsDBNull(12))
                        {
                            modelData.PRESC_SHARE_TO = Convert.ToDateTime(reader.GetDateTime(12)).ToString("dd/MM/yyyy");

                        }
                        else
                        {
                            modelData.PRESC_SHARE_TO = "";
                        }
                        modelData.COMITMENT_SHARE_PCT = reader["COMITMENT_SHARE_PCT"].ToString();
                        modelData.GRP_DSIG_CODE = reader["GRP_DSIG_CODE"].ToString();
                        modelData.ENTERED_BY = reader["ENTERED_BY"].ToString();
                        modelData.HONORARIUM_DURATION = reader["HONORARIUM_DURATION"].ToString();
                        modelData.ADDITIONAL_COMITMENT = reader["ADDITIONAL_COMMITMENT"].ToString();
                        modelData.OTHER_REMARKS = reader["OTHER_REMARKS"].ToString();
                         //,
                         //DURATION_FROM = Convert.ToDateTime(row["DURATION_FROM"]).ToString("dd/MM/yyyy"),//row["DURATION_FROM"].ToString(),
                         //DURATION_TO = Convert.ToDateTime(row["DURATION_TO"]).ToString("dd/MM/yyyy")//row["DURATION_TO"].ToString()
                         items.Add(modelData);
                     }
            con.Close();      
            return items;
        }
        public ValidationMsg Delete(string honorariumPaidSlNo)
        {
            try
            {
                DataTable dt = _dbHelper.GetDataTable("select * From DOC_COMMITMENT_PROD  WHERE HONOR_PAID_SLNO = " + honorariumPaidSlNo + "");
                if (dt.Rows.Count > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Child Record Found.";
                }
                else
                {
                    string deleteQuery = "DELETE FROM DOC_HONOR_PAID_INFO WHERE HONOR_PAID_SLNO = '" + honorariumPaidSlNo + "' ";
                    if (_dbHelper.CmdExecute(deleteQuery) > 0)
                    {
                        _validationMsg.Type = Enums.MessageType.Success;
                        _validationMsg.Msg = "Data Deleted Successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to Delete Data.";
            }
            return _validationMsg;
        }
        public ValidationMsg DeletedDocCommitProduct(string DOC_COMMI_PROD_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM DOC_COMMITMENT_PROD WHERE DOC_COMMI_PROD_SLNO = " + DOC_COMMI_PROD_SLNO + "";
                if (_dbHelper.CmdExecute(qry) > 0)
                {
                    vmMsg.Type = Enums.MessageType.Success;
                    vmMsg.Msg = "Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                vmMsg.Type = Enums.MessageType.Error;
                vmMsg.Msg = "Failed to Delete.";
            }
            return vmMsg;
        }
        public List<DoctorCommitProductModel> GetSaveDocCommitProductList(string HONOR_PAID_SLNO)
        {
            var items = new List<DoctorCommitProductModel>();
            DataTable dt = _dbHelper.GetDataTable("select DCP.DOC_COMMI_PROD_SLNO,DCP.HONOR_PAID_SLNO,DCP.PROD_ID,PROD.PRODUCT_NAME, DCP.PRODUCT_DETAILS from DOC_COMMITMENT_PROD DCP,PRODUCT PROD where DCP.PROD_ID = PROD.PROD_ID and DCP.HONOR_PAID_SLNO  = '" + HONOR_PAID_SLNO + "'");
            items = (from DataRow row in dt.Rows
                     select new DoctorCommitProductModel
                     {
                         DOC_COMMI_PROD_SLNO = Convert.ToInt32(row["DOC_COMMI_PROD_SLNO"].ToString()),
                         HONOR_PAID_SLNO = row["HONOR_PAID_SLNO"].ToString(),
                         PROD_ID = row["PROD_ID"].ToString(),
                         PRODUCT_NAME = row["PRODUCT_NAME"].ToString(),
                         PRODUCT_DETAILS = row["PRODUCT_DETAILS"].ToString()
                     }).ToList();

            return items;
        }
        public List<string> GetSuggestAprovNoList()
        {
            string Qry = "SELECT HONOR_APROV_NO FROM DOC_HONOR_PAID_INFO ORDER BY HONOR_APROV_NO";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<DoctorHonorariumPaidModel> items;
            items = (from DataRow row in dt.Rows
                     select new DoctorHonorariumPaidModel
                     {
                         HONOR_APROV_NO = row["HONOR_APROV_NO"].ToString()
                     }).ToList();
            return items.Select(m => m.HONOR_APROV_NO).ToList();
        }
    }
}