using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class InstitutionInformationDAO
    {
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        IDGenerated idGenerated = new IDGenerated();
        string code = string.Empty;
        long mxSl = 0;
        public List<InstitutionTypeModel> GetInstituteTypeList()
        {
            string Qry = "SELECT INSTI_TYPE_CODE,INSTI_TYPE_NAME FROM INSTITUTION_TYPE ORDER BY INSTI_TYPE_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<InstitutionTypeModel> itemList = new List<InstitutionTypeModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new InstitutionTypeModel
                        {
                            INSTI_TYPE_CODE = row["INSTI_TYPE_CODE"].ToString(),
                            INSTI_TYPE_NAME = row["INSTI_TYPE_NAME"].ToString()
                            //TC_L1_SLNO = row["TC_L1_SLNO"].ToString()

                        }).ToList();
            return itemList;
        }

        public string GetDistrictName(string Upazilacode)
        {
            string Qry = "SELECT DISTC_UPAZILA_MAS_SLNO FROM DISTC_UPAZILA_DTL WHERE UPAZILA_CODE = '" + Upazilacode + "' ";
            _dataTable = _dbHelper.GetDataTable(Qry);
            string distmassllno = string.Empty, distcode = string.Empty, distName = string.Empty;

            foreach (DataRow row in _dataTable.Rows)
            {
                distmassllno = row["DISTC_UPAZILA_MAS_SLNO"].ToString();
            }

            Qry = "SELECT DISTC_CODE FROM DISTC_UPAZILA_MAS WHERE DISTC_UPAZILA_MAS_SLNO = '" + distmassllno + "' ";
            _dataTable = _dbHelper.GetDataTable(Qry);

            foreach (DataRow row in _dataTable.Rows)
            {
                distcode = row["DISTC_CODE"].ToString();
            }

            Qry = "SELECT DISTC_NAME FROM DISTRICT WHERE DISTC_CODE = '" + distcode + "' ";
            _dataTable = _dbHelper.GetDataTable(Qry);

            foreach (DataRow row in _dataTable.Rows)
            {
                distName = row["DISTC_NAME"].ToString();
            }

            return distName;
        }

        public List<UpazilaModel> GetDataList()
        {
            string Qry = "SELECT UPAZILA_CODE,UPAZILA_NAME,UPAZILA_ORD_SLNO FROM UPAZILA ";

            DataTable dt = _dbHelper.GetDataTable(Qry);

            List<UpazilaModel> item;
            //using lamdaexpression
            item = (from DataRow row in dt.Rows
                    select new UpazilaModel
                    {

                        UPAZILA_CODE = row["UPAZILA_CODE"].ToString(),
                        UPAZILA_NAME = row["UPAZILA_NAME"].ToString()

                    }).ToList();

            return item;
        }

        public ValidationMsg Save(InstitutionInformationModel model, string userid)
        {
            try
            {
                mxSl = idGenerated.getMAXSL("INSTI_CODE", "INSTITUTION Where INSTI_CODE not in (99999)");
                string saveQuery = "INSERT INTO INSTITUTION (INSTI_CODE,INSTI_NAME,INSTI_TYPE_CODE,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,INST_PHONE,UPAZILA_CODE,NO_OF_BEDS,AVG_NO_ADMT_PATI,AVG_NO_OD_PATI,REMARKS,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES(" + mxSl + ",'" + model.INSTI_NAME + "','" + model.INSTI_TYPE_CODE + "','" + model.ADDRESS1 + "','" + model.ADDRESS2 + "','" + model.ADDRESS3 + "','" + model.ADDRESS4 + "','" + model.INST_PHONE + "','" + model.UPAZILA_CODE + "','" + model.NO_OF_BEDS + "','" + model.AVG_NO_ADMT_PATI + "','" + model.AVG_NO_OD_PATI + "','" + model.REMARKS + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";

                if (_dbHelper.CmdExecute(saveQuery) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Saved Successfully";
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

        public ValidationMsg Update(InstitutionInformationModel model, string userid)
        {
            try
            {
                string updateQuery = "UPDATE INSTITUTION SET INSTI_NAME = '" + model.INSTI_NAME + "',INSTI_TYPE_CODE = '" + model.INSTI_TYPE_CODE + "',ADDRESS1 = '" + model.ADDRESS1 + "',ADDRESS2 = '" + model.ADDRESS2 + "',ADDRESS3 = '" + model.ADDRESS3 + "',ADDRESS4 = '" + model.ADDRESS4 + "',INST_PHONE = '" + model.INST_PHONE + "'," +
                                    "UPAZILA_CODE = '" + model.UPAZILA_CODE + "',NO_OF_BEDS = '" + model.NO_OF_BEDS + "',AVG_NO_ADMT_PATI = '" + model.AVG_NO_ADMT_PATI + "',AVG_NO_OD_PATI = '" + model.AVG_NO_OD_PATI + "',REMARKS = '" + model.REMARKS + "',UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                    "WHERE INSTI_CODE = '" + model.INSTI_CODE + "'";
                if (_dbHelper.CmdExecute(updateQuery) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Update;
                    _validationMsg.Msg = "Data Updated Successfully";

                }
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

        public List<InstitutionInformationModel> GetInstituteformationList()
        {
            //string Qry = "SELECT INSTI_CODE,INSTI_NAME,INSTI_TYPE_CODE,ADDRESS1,ADDRESS2,ADDRESS3,ADDRESS4,INST_PHONE,UPAZILA_CODE,NO_OF_BEDS,AVG_NO_ADMT_PATI,AVG_NO_OD_PATI,REMARKS FROM INSTITUTION ORDER BY INSTI_CODE";
            //string Qry = "select  INS.INSTI_CODE,INS.INSTI_NAME,INS.INSTI_TYPE_CODE,F.INSTI_TYPE_NAME,INS.ADDRESS1,INS.ADDRESS2,INS.ADDRESS3,INS.ADDRESS4,INS.INST_PHONE, INS.UPAZILA_CODE,d.UPAZILA_NAME,D.DISTC_CODE,D.DISTC_NAME, INS.NO_OF_BEDS , INS.AVG_NO_ADMT_PATI , INS.AVG_NO_OD_PATI ,INS.REMARKS from INSTITUTION INS,VW_DISTRICT_UPAZILA d,INSTITUTION_TYPE f Where INS.UPAZILA_CODE=d.UPAZILA_CODE and INS.INSTI_TYPE_CODE=F.INSTI_TYPE_CODE";
            string Qry = " select  INS.INSTI_CODE,INS.INSTI_NAME,INS.INSTI_TYPE_CODE,F.INSTI_TYPE_NAME,INS.ADDRESS1,INS.ADDRESS2  , " +
                         " INS.ADDRESS3,INS.ADDRESS4,INS.INST_PHONE, INS.UPAZILA_CODE,  d.UPAZILA_NAME,  D.DISTC_CODE,D.DISTC_NAME, NVL(INS.NO_OF_BEDS,0) NO_OF_BEDS,  " +
                         " nvl(INS.AVG_NO_ADMT_PATI,0)AVG_NO_ADMT_PATI, nvl(INS.AVG_NO_OD_PATI,0)AVG_NO_OD_PATI ,INS.REMARKS  from INSTITUTION INS  " +
                         " Left Join VW_DISTRICT_UPAZILA d on INS.UPAZILA_CODE=d.UPAZILA_CODE  Left Join INSTITUTION_TYPE f on INS.INSTI_TYPE_CODE=F.INSTI_TYPE_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);

            List<InstitutionInformationModel> itemList = new List<InstitutionInformationModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new InstitutionInformationModel
                        {
                            INSTI_CODE = row["INSTI_CODE"].ToString(),
                            INSTI_NAME = row["INSTI_NAME"].ToString(),
                            INSTI_TYPE_CODE = row["INSTI_TYPE_CODE"].ToString(),
                            INSTI_TYPE_NAME = row["INSTI_TYPE_NAME"].ToString(),
                            ADDRESS1 = row["ADDRESS1"].ToString(),
                            ADDRESS2 = row["ADDRESS2"].ToString(),
                            ADDRESS3 = row["ADDRESS3"].ToString(),
                            ADDRESS4 = row["ADDRESS4"].ToString(),
                            INST_PHONE = row["INST_PHONE"].ToString(),
                            UPAZILA_CODE = row["UPAZILA_CODE"].ToString(),
                            UPAZILA_NAME = row["UPAZILA_NAME"].ToString(),
                            DISTC_CODE = row["DISTC_CODE"].ToString(),
                            DISTC_NAME = row["DISTC_NAME"].ToString(),
                            NO_OF_BEDS = Convert.ToInt32(row["NO_OF_BEDS"].ToString()),
                            AVG_NO_ADMT_PATI = Convert.ToInt32(row["AVG_NO_ADMT_PATI"].ToString()),
                            AVG_NO_OD_PATI = Convert.ToInt32(row["AVG_NO_OD_PATI"].ToString()),
                            REMARKS = row["REMARKS"].ToString()

                        }).ToList();
            return itemList;
        }


        public ValidationMsg Delete(string instituteCode)
        {
            try
            {
                string deleteQuery = "DELETE FROM INSTITUTION WHERE INSTI_CODE = '" + instituteCode + "' ";
                if (_dbHelper.CmdExecute(deleteQuery) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("ORA-02292"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "child record found";
                }
                else
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Failed to Delete Data.";
                }
            }
            return _validationMsg;

        }

        public List<string> GetSuggestInsInfoList()
        {
            string Qry = "SELECT INSTI_NAME FROM INSTITUTION ORDER BY INSTI_NAME";
            DataTable dt = _dbHelper.GetDataTable(Qry);
            List<InstitutionInformationModel> items;
            items = (from DataRow row in dt.Rows
                     select new InstitutionInformationModel
                     {
                         INSTI_NAME = row["INSTI_NAME"].ToString()
                     }).ToList();
            return items.Select(m => m.INSTI_NAME).ToList();
        }

        public string GetCode()
        {
            code = mxSl.ToString();
            return code;
        }
    }
}