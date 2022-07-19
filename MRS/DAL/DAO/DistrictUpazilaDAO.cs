using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;


namespace MRS.DAL.DAO
{
    public class DistrictUpazilaDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg = new ValidationMsg();
        public int DistrictUpazilaSlNo = 0;
        private List<DistrictUpazilaMstModel> itemMasterList;
        private List<DistrictUpazilaDtlModel> itemDetailList;


        public List<DistrictUpazilaMstModel> GetDistrictList()
        {
            string Qry = "";
            Qry = "SELECT DISTC_CODE ,DISTC_NAME FROM DISTRICT ";
            Qry = Qry + "ORDER BY DISTC_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);

            itemMasterList = (from DataRow row in dt.Rows
                              select new DistrictUpazilaMstModel
                              {
                                  DISTC_CODE = row["DISTC_CODE"].ToString(),
                                  DISTC_NAME = row["DISTC_NAME"].ToString()
                              }).ToList();

            return itemMasterList;
        }

        public List<DistrictUpazilaDtlModel> GetUpazilaList()
        {
            string Qry = "";
            Qry = "SELECT UPAZILA_CODE ,UPAZILA_NAME FROM UPAZILA ";
            Qry = Qry + "ORDER BY UPAZILA_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            itemDetailList = (from DataRow row in dt.Rows
                              select new DistrictUpazilaDtlModel
                              {
                                  UPAZILA_CODE = row["UPAZILA_CODE"].ToString(),
                                  UPAZILA_NAME = row["UPAZILA_NAME"].ToString()
                              }).ToList();

            return itemDetailList;
        }


        //public ValidationMsg Save(DistrictUpazilaMstModel model, string userid)
        //{
        //    _vmMsg = new ValidationMsg();
        //    try
        //    {
        //        using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
        //        {
        //            DataTable dataTable =
        //                dbHelper.GetDataTable(
        //                    "SELECT (NVL(MAX(DISTC_UPAZILA_MAS_SLNO),0)+1)DISTC_UPAZILA_MAS_SLNO FROM DISTC_UPAZILA_MAS");
        //            if (dataTable.Rows.Count > 0)
        //            {
        //                model.DISTC_UPAZILA_MAS_SLNO = dataTable.Rows[0][0].ToString();
        //            }
        //            string query = "INSERT INTO DISTC_UPAZILA_MAS(DISTC_UPAZILA_MAS_SLNO,DISTC_CODE)" +
        //                           "VALUES('" + model.DISTC_UPAZILA_MAS_SLNO + "', '" + model.DISTC_CODE + "')";
        //            if (dbHelper.CmdExecute(query) > 0)
        //            {
        //                DistrictUpazilaSlNo = Convert.ToInt16(model.DISTC_UPAZILA_MAS_SLNO);
        //            }
        //            if (model.DistrictUpazilaDtlList != null)
        //            {
        //                foreach (DistrictUpazilaDtlModel objDistrictUpazilaDtlModel in model.DistrictUpazilaDtlList)
        //                {
        //                    DataTable dataTableDtl =
        //                        dbHelper.GetDataTable(
        //                            "SELECT (NVL(MAX(DISTC_UPAZILA_DTL_SLNO),0)+1)DISTC_UPAZILA_DTL_SLNO FROM DISTC_UPAZILA_DTL");
        //                    if (dataTableDtl.Rows.Count > 0)
        //                    {
        //                        objDistrictUpazilaDtlModel.DISTC_UPAZILA_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
        //                        objDistrictUpazilaDtlModel.DISTC_UPAZILA_MAS_SLNO = DistrictUpazilaSlNo.ToString();//New added Line which is in Query Also
        //                    }
        //                    string querydtl =
        //                        "INSERT INTO DISTC_UPAZILA_DTL(DISTC_UPAZILA_DTL_SLNO,DISTC_UPAZILA_MAS_SLNO,UPAZILA_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
        //                        "VALUES('" + objDistrictUpazilaDtlModel.DISTC_UPAZILA_DTL_SLNO + "', '" + model.DISTC_UPAZILA_MAS_SLNO +
        //                        "', '" + objDistrictUpazilaDtlModel.UPAZILA_CODE + "', '" + userid + "',(TO_DATE('" +
        //                        DateTime.Now.ToShortDateString() + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() +
        //                        "')";
        //                    if (dbHelper.CmdExecute(querydtl) > 0)
        //                    {
        //                        _vmMsg.Type = Enums.MessageType.Success;

        //                    }
        //                }
        //            }
        //            scope.Complete();
        //            //_vmMsg.Type = Enums.MessageType.Success;
        //            _vmMsg.Msg = "Saved Successfully.";
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _vmMsg.Type = Enums.MessageType.Error;
        //        _vmMsg.Msg = "Failed to save.";

        //        if (ex.Message.Contains("ORA-00001"))
        //        {
        //            _vmMsg.Type = Enums.MessageType.Error;
        //            _vmMsg.Msg = "District Upazila Code Already Exist.";
        //        }
        //        if (ex.Message.Contains("ORA-01438"))
        //        {
        //            _vmMsg.Type = Enums.MessageType.Error;
        //            _vmMsg.Msg = "Value larger than specified precision allowed.";
        //        }
        //    }

        //    return _vmMsg;
        //}





        public ValidationMsg Save(DistrictUpazilaMstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    DataTable dataTable =
                        dbHelper.GetDataTable(
                            "SELECT (NVL(MAX(DISTC_UPAZILA_MAS_SLNO),0)+1)DISTC_UPAZILA_MAS_SLNO FROM DISTC_UPAZILA_MAS");
                    if (dataTable.Rows.Count > 0)
                    {
                        model.DISTC_UPAZILA_MAS_SLNO = dataTable.Rows[0][0].ToString();
                    }
                    string query = "INSERT INTO DISTC_UPAZILA_MAS(DISTC_UPAZILA_MAS_SLNO,DISTC_CODE)" +
                                   "VALUES(" + model.DISTC_UPAZILA_MAS_SLNO + ", '" + model.DISTC_CODE + "')";
                    if (dbHelper.CmdExecute(query) > 0)
                    {
                        DistrictUpazilaSlNo = Convert.ToInt16(model.DISTC_UPAZILA_MAS_SLNO);
                    }
                    if (model.DistrictUpazilaDtlList != null)
                    {
                        foreach (DistrictUpazilaDtlModel objDistrictUpazilaDtlModel in model.DistrictUpazilaDtlList)
                        {
                            DataTable dataTableDtl =
                                dbHelper.GetDataTable(
                                    "SELECT (NVL(MAX(DISTC_UPAZILA_DTL_SLNO),0)+1)DISTC_UPAZILA_DTL_SLNO FROM DISTC_UPAZILA_DTL");
                            if (dataTableDtl.Rows.Count > 0)
                            {
                                objDistrictUpazilaDtlModel.DISTC_UPAZILA_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
                                objDistrictUpazilaDtlModel.DISTC_UPAZILA_MAS_SLNO = DistrictUpazilaSlNo.ToString();//New added Line which is in Query Also
                            }
                            string querydtl =
                                "INSERT INTO DISTC_UPAZILA_DTL(DISTC_UPAZILA_DTL_SLNO,DISTC_UPAZILA_MAS_SLNO,UPAZILA_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objDistrictUpazilaDtlModel.DISTC_UPAZILA_DTL_SLNO + "', '" + model.DISTC_UPAZILA_MAS_SLNO +
                                "', '" + objDistrictUpazilaDtlModel.UPAZILA_CODE + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() +
                                "')";
                            

                            if (dbHelper.CmdExecute(querydtl) > 0)
                            {
                                _vmMsg.Type = Enums.MessageType.Success;
                            }
                        }
                    }
                    scope.Complete();
                    //_vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Saved Successfully.";
                }
            }
            catch (Exception ex)
            {

                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Upazila Code Already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }

            return _vmMsg;
        }



        public ValidationMsg Update(DistrictUpazilaMstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string query = "";
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    if (model.DISTC_UPAZILA_MAS_SLNO != "")
                    {
                        string qry = "SELECT TC1.DISTC_UPAZILA_MAS_SLNO,TC1.DISTC_CODE,TL1.DISTC_NAME FROM DISTC_UPAZILA_MAS TC1 INNER JOIN DISTRICT TL1 ON TC1.DISTC_CODE=TL1.DISTC_CODE";
                        qry = qry + " ORDER BY TC1.DISTC_UPAZILA_MAS_SLNO";

                        DataTable dt = dbHelper.GetDataTable(qry);
                        itemMasterList = (from DataRow row in dt.Rows
                                          select new DistrictUpazilaMstModel
                                          {
                                              DISTC_CODE = row["DISTC_CODE"].ToString(),
                                              DISTC_NAME = row["DISTC_NAME"].ToString(),
                                              DISTC_UPAZILA_MAS_SLNO = row["DISTC_UPAZILA_MAS_SLNO"].ToString()
                                          }).ToList();
                        if (itemMasterList.Count > 0)
                        {
                            query = "UPDATE DISTC_UPAZILA_MAS SET DISTC_CODE = '" + model.DISTC_CODE + "' WHERE DISTC_UPAZILA_MAS_SLNO = '" + model.DISTC_UPAZILA_MAS_SLNO + "'";
                        }

                        dbHelper.CmdExecute(query);
                    }
                    if (model.DistrictUpazilaDtlList != null)
                    {
                        foreach (DistrictUpazilaDtlModel objDistrictUpazilaUpdateDtlModel in model.DistrictUpazilaDtlList)
                        {
                            if (objDistrictUpazilaUpdateDtlModel.DISTC_UPAZILA_DTL_SLNO != null)
                            {

                                string querydtl =
                                                "UPDATE DISTC_UPAZILA_DTL SET UPAZILA_CODE = '" + objDistrictUpazilaUpdateDtlModel.UPAZILA_CODE + "'," +
                                                "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                                "WHERE DISTC_UPAZILA_MAS_SLNO = '" + model.DISTC_UPAZILA_MAS_SLNO + "' AND DISTC_UPAZILA_DTL_SLNO = '" + objDistrictUpazilaUpdateDtlModel.DISTC_UPAZILA_DTL_SLNO + "'";

                                dbHelper.CmdExecute(querydtl);
                            }
                            else
                            {
                                foreach (DistrictUpazilaDtlModel objDistrictUpazilaDtlModel in model.DistrictUpazilaDtlList)
                                {
                                    DataTable dataTableDtl =
                                        dbHelper.GetDataTable(
                                            "SELECT (NVL(MAX(DISTC_UPAZILA_DTL_SLNO),0)+1)DISTC_UPAZILA_DTL_SLNO FROM DISTC_UPAZILA_DTL");
                                    if (dataTableDtl.Rows.Count > 0)
                                    {
                                        objDistrictUpazilaDtlModel.DISTC_UPAZILA_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
                                        objDistrictUpazilaDtlModel.DISTC_UPAZILA_MAS_SLNO = DistrictUpazilaSlNo.ToString();//New added Line which is in Query Also
                                    }
                                    string querydtl =
                                        "INSERT INTO DISTC_UPAZILA_DTL(DISTC_UPAZILA_DTL_SLNO,DISTC_UPAZILA_MAS_SLNO,UPAZILA_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                        "VALUES('" + objDistrictUpazilaDtlModel.DISTC_UPAZILA_DTL_SLNO + "', '" + model.DISTC_UPAZILA_MAS_SLNO +
                                        "', '" + objDistrictUpazilaDtlModel.UPAZILA_CODE + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() +
                                        "')";
                                    
                                    dbHelper.CmdExecute(querydtl);
                                }
                            }
                        }
                    }
                    scope.Complete();
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Updated Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Upazila Code Or District Code Already Exist.";

                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }


        public List<DistrictUpazilaMstModel> GetSearchMasterList()
        {
            string qry = "SELECT TC1.DISTC_UPAZILA_MAS_SLNO,TC1.DISTC_CODE,TL1.DISTC_NAME FROM DISTC_UPAZILA_MAS TC1 INNER JOIN DISTRICT TL1 ON TC1.DISTC_CODE=TL1.DISTC_CODE";
            qry = qry + " ORDER BY TC1.DISTC_UPAZILA_MAS_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemMasterList = (from DataRow row in dt.Rows
                              select new DistrictUpazilaMstModel
                              {
                                  DISTC_CODE = row["DISTC_CODE"].ToString(),
                                  DISTC_NAME = row["DISTC_NAME"].ToString(),
                                  DISTC_UPAZILA_MAS_SLNO = row["DISTC_UPAZILA_MAS_SLNO"].ToString()
                              }).ToList();
            return itemMasterList;
        }

        public List<DistrictUpazilaDtlModel> GetSearchDtliList(string districtUpazilaMstSlNo)
        {
            string qry = "SELECT TC2.DISTC_UPAZILA_DTL_SLNO,TC2.DISTC_UPAZILA_MAS_SLNO,TC2.UPAZILA_CODE,TL2.UPAZILA_NAME FROM DISTC_UPAZILA_DTL TC2 INNER JOIN UPAZILA TL2 ON TC2.UPAZILA_CODE=TL2.UPAZILA_CODE WHERE TC2.DISTC_UPAZILA_MAS_SLNO='" + districtUpazilaMstSlNo + "'";
            qry = qry + " ORDER BY TC2.DISTC_UPAZILA_MAS_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemDetailList = (from DataRow row in dt.Rows
                              select new DistrictUpazilaDtlModel
                              {
                                  UPAZILA_CODE = row["UPAZILA_CODE"].ToString(),
                                  UPAZILA_NAME = row["UPAZILA_NAME"].ToString(),
                                  DISTC_UPAZILA_DTL_SLNO = row["DISTC_UPAZILA_DTL_SLNO"].ToString()
                              }).ToList();
            return itemDetailList;
        }


        public ValidationMsg DeleteDetailData(string districtUpazilaDtlSlNo)
        {
            try
            {
                
                string deleteDetailQuery = "DELETE FROM DISTC_UPAZILA_DTL WHERE DISTC_UPAZILA_DTL_SLNO = '" + districtUpazilaDtlSlNo + "'";
                if (dbHelper.CmdExecute(deleteDetailQuery) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Delete Detail Data";
            }
            return _vmMsg;
        }


        public ValidationMsg Delete(string distrctUpazilaMstSlNo)
        {
            try
            {
                string querymaster = "DELETE FROM DISTC_UPAZILA_MAS WHERE DISTC_UPAZILA_MAS_SLNO = '" +
                  distrctUpazilaMstSlNo + "'";

                if (dbHelper.CmdExecute(querymaster) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Delete Child Data First.";
            }
            return _vmMsg;
        }



    }
}