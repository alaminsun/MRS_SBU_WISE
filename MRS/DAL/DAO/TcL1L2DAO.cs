using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Data.OracleClient;
using System.Web.Mvc;

namespace MRS.DAL.DAO
{
    public class TcL1L2DAO
    {
        private DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg = new ValidationMsg();
        private List<TcL1L2MstModel> itemMasterList;
        private List<TcL1L2DtlModel> itemDetailList;
        public int TcL1L2Sl = 0;
        

        public List<TcL1L2MstModel> GetTcL1L2MstList()
        {
            string Qry = "";
            Qry = "SELECT TC_L1_CODE ,TC_L1_DESC FROM TC_LEVEL1 ";
            Qry = Qry + "ORDER BY TC_L1_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);

            itemMasterList = (from DataRow row in dt.Rows
                select new TcL1L2MstModel
                {
                    TC_L1_CODE = row["TC_L1_CODE"].ToString(),
                    TC_L1_DESC = row["TC_L1_DESC"].ToString()
                }).ToList();

            return itemMasterList;
        }

        public List<TcL1L2DtlModel> GetTcL1L2DtlList()
        {
            string Qry = "";
            Qry = "SELECT TC_L2_CODE ,TC_L2_DESC FROM TC_LEVEL2 ";
            Qry = Qry + "ORDER BY TC_L2_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            itemDetailList = (from DataRow row in dt.Rows
                select new TcL1L2DtlModel
                {
                    TC_L2_CODE = row["TC_L2_CODE"].ToString(),
                    TC_L2_DESC = row["TC_L2_DESC"].ToString()
                }).ToList();

            return itemDetailList;
        }

        public ValidationMsg Save(TcL1L2MstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    DataTable dataTable =
                        dbHelper.GetDataTable(
                            "SELECT (NVL(MAX(TC_L1_L2_MST_SLNO),0)+1)TC_L1_L2_MST_SLNO FROM TC_L1_L2_MST");
                    if (dataTable.Rows.Count > 0)
                    {
                        model.TC_L1_L2_MST_SLNO = dataTable.Rows[0][0].ToString();
                    }
                    string query = "INSERT INTO TC_L1_L2_MST(TC_L1_L2_MST_SLNO,TC_L1_CODE)" +
                                   "VALUES(" + model.TC_L1_L2_MST_SLNO + ", '" + model.TC_L1_CODE + "')";
                    if (dbHelper.CmdExecute(query) > 0)
                    {
                        TcL1L2Sl = Convert.ToInt16(model.TC_L1_L2_MST_SLNO);
                    }
                    if (model.TcL1L2DtlList != null)
                    {
                        foreach (TcL1L2DtlModel objTcL1L2DtlModel in model.TcL1L2DtlList)
                        {
                            DataTable dataTableDtl =
                                dbHelper.GetDataTable(
                                    "SELECT (NVL(MAX(TC_L1_L2_DTL_SLNO),0)+1)TC_L1_L2_DTL_SLNO FROM TC_L1_L2_DTL");
                            if (dataTableDtl.Rows.Count > 0)
                            {
                                objTcL1L2DtlModel.TC_L1_L2_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
                                objTcL1L2DtlModel.TC_L1_L2_MST_SLNO = TcL1L2Sl.ToString();//New added Line which is in Query Also
                            }
                            string querydtl =
                                "INSERT INTO TC_L1_L2_DTL(TC_L1_L2_DTL_SLNO,TC_L1_L2_MST_SLNO,TC_L2_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objTcL1L2DtlModel.TC_L1_L2_DTL_SLNO + "', '" + model.TC_L1_L2_MST_SLNO +
                                "', '" + objTcL1L2DtlModel.TC_L2_CODE + "', '" + userid + "',(TO_DATE('" +
                                model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() +
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
                    _vmMsg.Msg = "Therapeutic Class Level-1 Code Already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }

            return _vmMsg;
        }

        public ValidationMsg Update(TcL1L2MstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string query="";
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    if (model.TC_L1_L2_MST_SLNO !="")
                    {
                        string qry = "SELECT TC1.TC_L1_L2_MST_SLNO,TC1.TC_L1_CODE,TL1.TC_L1_DESC FROM TC_L1_L2_MST TC1 INNER JOIN TC_LEVEL1 TL1 ON TC1.TC_L1_CODE=TL1.TC_L1_CODE";
                        qry = qry + " ORDER BY TC1.TC_L1_L2_MST_SLNO";

                        DataTable dt = dbHelper.GetDataTable(qry);
                        itemMasterList = (from DataRow row in dt.Rows
                                          select new TcL1L2MstModel
                                          {
                                              TC_L1_CODE = row["TC_L1_CODE"].ToString(),
                                              TC_L1_DESC = row["TC_L1_DESC"].ToString(),
                                              TC_L1_L2_MST_SLNO = row["TC_L1_L2_MST_SLNO"].ToString()
                                          }).ToList();
                        if (itemMasterList.Count > 0)
                        {
                            query = "UPDATE TC_L1_L2_MST SET TC_L1_CODE = '" + model.TC_L1_CODE + "' WHERE TC_L1_L2_MST_SLNO = '" + model.TC_L1_L2_MST_SLNO + "'";
                        }

                        dbHelper.CmdExecute(query);
                    } 
                    if (model.TcL1L2DtlList != null)
                    {
                        foreach (TcL1L2DtlModel objTcL1L2DtlUpdateModel in model.TcL1L2DtlList)
                        {
                            if (objTcL1L2DtlUpdateModel.TC_L1_L2_DTL_SLNO != null)
                            {

                                string querydtl =
                                                "UPDATE TC_L1_L2_DTL SET TC_L2_CODE = '" + objTcL1L2DtlUpdateModel.TC_L2_CODE + "'," +
                                                "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                                "WHERE TC_L1_L2_MST_SLNO = '" + model.TC_L1_L2_MST_SLNO + "' AND TC_L1_L2_DTL_SLNO = '" + objTcL1L2DtlUpdateModel.TC_L1_L2_DTL_SLNO + "'";

                                dbHelper.CmdExecute(querydtl);      
                            }
                            else
                            {
                                foreach (TcL1L2DtlModel objTcL1L2DtlModel in model.TcL1L2DtlList)
                                {
                                    DataTable dataTableDtl =
                                        dbHelper.GetDataTable(
                                            "SELECT (NVL(MAX(TC_L1_L2_DTL_SLNO),0)+1)TC_L1_L2_DTL_SLNO FROM TC_L1_L2_DTL");
                                    if (dataTableDtl.Rows.Count > 0)
                                    {
                                        objTcL1L2DtlModel.TC_L1_L2_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
                                        objTcL1L2DtlModel.TC_L1_L2_MST_SLNO = TcL1L2Sl.ToString();//New added Line which is in Query Also
                                    }
                                    string querydtl =
                                        "INSERT INTO TC_L1_L2_DTL(TC_L1_L2_DTL_SLNO,TC_L1_L2_MST_SLNO,TC_L2_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                        "VALUES('" + objTcL1L2DtlModel.TC_L1_L2_DTL_SLNO + "', '" + model.TC_L1_L2_MST_SLNO +
                                        "', '" + objTcL1L2DtlModel.TC_L2_CODE + "', '" + userid + "',(TO_DATE('" +
                                        DateTime.Now.ToShortDateString() + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() +
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
                _vmMsg.Msg = "Failed to Update.";

                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg; 
        }

        public List<TcL1L2MstModel> GetSearchMasterList()
        {
            string qry = "SELECT TC1.TC_L1_L2_MST_SLNO,TC1.TC_L1_CODE,TL1.TC_L1_DESC FROM TC_L1_L2_MST TC1 INNER JOIN TC_LEVEL1 TL1 ON TC1.TC_L1_CODE=TL1.TC_L1_CODE";
            qry = qry + " ORDER BY TC1.TC_L1_L2_MST_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemMasterList = (from DataRow row in dt.Rows
                              select new TcL1L2MstModel
                     {
                        TC_L1_CODE = row["TC_L1_CODE"].ToString(),
                        TC_L1_DESC = row["TC_L1_DESC"].ToString(),
                        TC_L1_L2_MST_SLNO = row["TC_L1_L2_MST_SLNO"].ToString()
                     }).ToList();
            return itemMasterList;
        }

        public List<TcL1L2DtlModel> GetSearchDtliList(string tcL1L2MstSlNo)
        {
            string qry = "SELECT TC2.TC_L1_L2_DTL_SLNO,TC2.TC_L1_L2_MST_SLNO,TC2.TC_L2_CODE,TL2.TC_L2_DESC FROM TC_L1_L2_DTL TC2 INNER JOIN TC_LEVEL2 TL2 ON TC2.TC_L2_CODE=TL2.TC_L2_CODE WHERE TC2.TC_L1_L2_MST_SLNO='" + tcL1L2MstSlNo + "'";
           qry = qry + " ORDER BY TC2.TC_L1_L2_MST_SLNO";

           DataTable dt = dbHelper.GetDataTable(qry);
           itemDetailList = (from DataRow row in dt.Rows
                             select new TcL1L2DtlModel
                             {
                                 TC_L2_CODE = row["TC_L2_CODE"].ToString(),
                                 TC_L2_DESC = row["TC_L2_DESC"].ToString(),
                                 TC_L1_L2_DTL_SLNO = row["TC_L1_L2_DTL_SLNO"].ToString()
                             }).ToList();
           return itemDetailList;
           //DataTable dt = dbHelper.GetDataTable(qry);
           // TcL1L2DtlModel objTcL1L2DtllModel = new TcL1L2DtlModel();
           // foreach (DataRow row in dt.Rows)
           // {
           //     objTcL1L2DtllModel.TC_L2_CODE = row["TC_L2_CODE"].ToString();
           //     objTcL1L2DtllModel.TC_L2_DESC = row["TC_L2_DESC"].ToString();
           //     //objTcL1L2DtllModel.TC_L1_L2_MST_SLNO = row["TC_L1_L2_MST_SLNO"].ToString();
           //     objTcL1L2DtllModel.TC_L1_L2_DTL_SLNO = row["TC_L1_L2_DTL_SLNO"].ToString();
           //     itemDetailList.Add(objTcL1L2DtllModel);
           // }

           // return itemDetailList;
        }


        public ValidationMsg Delete(string tcL1L2MstSlNo)
        {
            try
            {
                string querymaster = "DELETE FROM TC_L1_L2_MST WHERE TC_L1_L2_MST_SLNO = '" +
                  tcL1L2MstSlNo + "'";

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

        public ValidationMsg DeleteDetailData(string tcL1L2DtlSlNo)
        {
            try
            {

                string deleteDetailQuery = "DELETE FROM TC_L1_L2_DTL WHERE TC_L1_L2_DTL_SLNO = '" + tcL1L2DtlSlNo + "'";
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






    }
}