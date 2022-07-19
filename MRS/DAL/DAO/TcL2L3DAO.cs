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
    public class TcL2L3DAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg = new ValidationMsg();
        List<TcL2L3DtlModel> items;
        public int TcL2L3Sl = 0;
        private List<TcL2L3MstModel> itemMasterList;
        private List<TcL2L3DtlModel> itemDetailList;

        public List<TcL2L3MstModel> GetTcL2L3MstList()
        {
            string Qry = "";
            Qry = "SELECT TC_L2_CODE ,TC_L2_DESC FROM TC_LEVEL2";
            Qry = Qry + " ORDER BY TC_L2_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<TcL2L3MstModel> items;
            items = (from DataRow row in dt.Rows
                     select new TcL2L3MstModel
                     {
                         TC_L2_CODE = row["TC_L2_CODE"].ToString(),
                         TC_L2_DESC = row["TC_L2_DESC"].ToString()
                     }).ToList();

            return items;
        }

        public List<TcL2L3DtlModel> GetTcL2L3DtlList()
        {
            string Qry = "";
            Qry = "SELECT TC_L3_CODE ,TC_L3_DESC FROM TC_LEVEL3 ";
            Qry = Qry + " ORDER BY TC_L3_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<TcL2L3DtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new TcL2L3DtlModel
                     {
                         TC_L3_CODE = row["TC_L3_CODE"].ToString(),
                         TC_L3_DESC = row["TC_L3_DESC"].ToString()
                     }).ToList();

            return items;
        }


        public ValidationMsg Save(TcL2L3MstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    DataTable dataTable =
                        dbHelper.GetDataTable(
                            "SELECT (NVL(MAX(TC_L2_L3_MST_SLNO),0)+1)TC_L2_L3_MST_SLNO FROM TC_L2_L3_MST");
                    if (dataTable.Rows.Count > 0)
                    {
                        model.TC_L2_L3_MST_SLNO = dataTable.Rows[0][0].ToString();
                    }
                    string query = "INSERT INTO TC_L2_L3_MST(TC_L2_L3_MST_SLNO,TC_L2_CODE)" +
                                   "VALUES(" + model.TC_L2_L3_MST_SLNO + ", '" + model.TC_L2_CODE + "')";
                    if (dbHelper.CmdExecute(query) > 0)
                    {
                        TcL2L3Sl = Convert.ToInt16(model.TC_L2_L3_MST_SLNO);
                    }
                    if (model.TcL2L3DtlList != null)
                    {
                        foreach (TcL2L3DtlModel objTcL2L3DtlModel in model.TcL2L3DtlList)
                        {
                            DataTable dataTableDtl =
                                dbHelper.GetDataTable(
                                    "SELECT (NVL(MAX(TC_L2_L3_DTL_SLNO),0)+1)TC_L2_L3_DTL_SLNO FROM TC_L2_L3_DTL");
                            if (dataTableDtl.Rows.Count > 0)
                            {
                                objTcL2L3DtlModel.TC_L2_L3_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
                                objTcL2L3DtlModel.TC_L2_L3_MST_SLNO = TcL2L3Sl.ToString();//New added Line which is in Query Also
                            }
                            string querydtl =
                                "INSERT INTO TC_L2_L3_DTL(TC_L2_L3_DTL_SLNO,TC_L2_L3_MST_SLNO,TC_L3_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objTcL2L3DtlModel.TC_L2_L3_DTL_SLNO + "', '" + model.TC_L2_L3_MST_SLNO +
                                "', '" + objTcL2L3DtlModel.TC_L3_CODE + "', '" + userid + "',(TO_DATE('" +
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

        public ValidationMsg Update(TcL2L3MstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string query = "";
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    if (model.TC_L2_L3_MST_SLNO != "")
                    {
                        string qry = "SELECT TC1.TC_L2_L3_MST_SLNO,TC1.TC_L2_CODE,TL1.TC_L2_DESC FROM TC_L2_L3_MST TC1 INNER JOIN TC_LEVEL2 TL1 ON TC1.TC_L2_CODE=TL1.TC_L2_CODE";
                        qry = qry + " ORDER BY TC1.TC_L2_L3_MST_SLNO";

                        DataTable dt = dbHelper.GetDataTable(qry);
                        itemMasterList = (from DataRow row in dt.Rows
                                          select new TcL2L3MstModel
                                          {
                                              TC_L2_CODE = row["TC_L2_CODE"].ToString(),
                                              TC_L2_DESC = row["TC_L2_DESC"].ToString(),
                                              TC_L2_L3_MST_SLNO = row["TC_L2_L3_MST_SLNO"].ToString()
                                          }).ToList();
                        if (itemMasterList.Count > 0)
                        {
                            query = "UPDATE TC_L2_L3_MST SET TC_L2_CODE = '" + model.TC_L2_CODE + "' WHERE TC_L2_L3_MST_SLNO = '" + model.TC_L2_L3_MST_SLNO + "'";
                        }

                        dbHelper.CmdExecute(query);
                    }
                    if (model.TcL2L3DtlList != null)
                    {
                        foreach (TcL2L3DtlModel objTcL1L2DtlUpdateModel in model.TcL2L3DtlList)
                        {
                            if (objTcL1L2DtlUpdateModel.TC_L2_L3_DTL_SLNO != null)
                            {

                                string querydtl =
                                                "UPDATE TC_L2_L3_DTL SET TC_L3_CODE = '" + objTcL1L2DtlUpdateModel.TC_L3_CODE + "'," +
                                                "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                                "WHERE TC_L2_L3_MST_SLNO = '" + model.TC_L2_L3_MST_SLNO + "' AND TC_L2_L3_DTL_SLNO = '" + objTcL1L2DtlUpdateModel.TC_L2_L3_DTL_SLNO + "'";

                                dbHelper.CmdExecute(querydtl);
                            }
                            else
                            {
                                foreach (TcL2L3DtlModel objTcL2L3DtlModel in model.TcL2L3DtlList)
                                {
                                    DataTable dataTableDtl =
                                        dbHelper.GetDataTable(
                                            "SELECT (NVL(MAX(TC_L2_L3_DTL_SLNO),0)+1)TC_L2_L3_DTL_SLNO FROM TC_L2_L3_DTL");
                                    if (dataTableDtl.Rows.Count > 0)
                                    {
                                        objTcL2L3DtlModel.TC_L2_L3_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
                                        objTcL2L3DtlModel.TC_L2_L3_MST_SLNO = TcL2L3Sl.ToString();//New added Line which is in Query Also
                                    }
                                    string querydtl =
                                        "INSERT INTO TC_L2_L3_DTL(TC_L2_L3_DTL_SLNO,TC_L2_L3_MST_SLNO,TC_L3_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                        "VALUES('" + objTcL2L3DtlModel.TC_L2_L3_DTL_SLNO + "', '" + model.TC_L2_L3_MST_SLNO +
                                        "', '" + objTcL2L3DtlModel.TC_L3_CODE + "', '" + userid + "',(TO_DATE('" +
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

        public List<TcL2L3MstModel> GetSearchMasterList()
        {
            string qry = "SELECT TC1.TC_L2_L3_MST_SLNO,TC1.TC_L2_CODE,TL1.TC_L2_DESC FROM TC_L2_L3_MST TC1 INNER JOIN TC_LEVEL2 TL1 ON TC1.TC_L2_CODE=TL1.TC_L2_CODE";
            qry = qry + " ORDER BY TC1.TC_L2_L3_MST_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemMasterList = (from DataRow row in dt.Rows
                              select new TcL2L3MstModel
                              {
                                  TC_L2_CODE = row["TC_L2_CODE"].ToString(),
                                  TC_L2_DESC = row["TC_L2_DESC"].ToString(),
                                  TC_L2_L3_MST_SLNO = row["TC_L2_L3_MST_SLNO"].ToString()
                              }).ToList();
            return itemMasterList;
        }

        public List<TcL2L3DtlModel> GetSearchDtliList(string tcL2L3MstSlNo)
        {
            string qry = "SELECT TC2.TC_L2_L3_DTL_SLNO,TC2.TC_L2_L3_MST_SLNO,TC2.TC_L3_CODE,TL2.TC_L3_DESC FROM TC_L2_L3_DTL TC2 INNER JOIN TC_LEVEL3 TL2 ON TC2.TC_L3_CODE=TL2.TC_L3_CODE WHERE TC2.TC_L2_L3_MST_SLNO='" + tcL2L3MstSlNo + "'";
            qry = qry + " ORDER BY TC2.TC_L2_L3_MST_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemDetailList = (from DataRow row in dt.Rows
                              select new TcL2L3DtlModel
                              {
                                  TC_L3_CODE = row["TC_L3_CODE"].ToString(),
                                  TC_L3_DESC = row["TC_L3_DESC"].ToString(),
                                  TC_L2_L3_DTL_SLNO = row["TC_L2_L3_DTL_SLNO"].ToString()
                              }).ToList();
            return itemDetailList;
            //DataTable dt = dbHelper.GetDataTable(qry);
            // TcL2L3DtlModel objTcL1L2DtllModel = new TcL2L3DtlModel();
            // foreach (DataRow row in dt.Rows)
            // {
            //     objTcL1L2DtllModel.TC_L3_CODE = row["TC_L3_CODE"].ToString();
            //     objTcL1L2DtllModel.TC_L3_DESC = row["TC_L3_DESC"].ToString();
            //     //objTcL1L2DtllModel.TC_L2_L3_MST_SLNO = row["TC_L2_L3_MST_SLNO"].ToString();
            //     objTcL1L2DtllModel.TC_L2_L3_DTL_SLNO = row["TC_L2_L3_DTL_SLNO"].ToString();
            //     itemDetailList.Add(objTcL1L2DtllModel);
            // }

            // return itemDetailList;
        }


        public ValidationMsg Delete(string tcL2L3MstSlNo)
        {
            try
            {
                string querymaster = "DELETE FROM TC_L2_L3_MST WHERE TC_L2_L3_MST_SLNO = '" +
                  tcL2L3MstSlNo + "'";

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

        public ValidationMsg DeleteDetailData(string tcL2L3DtlSlNo)
        {
            try
            {

                string deleteDetailQuery = "DELETE FROM TC_L2_L3_DTL WHERE TC_L2_L3_DTL_SLNO = '" + tcL2L3DtlSlNo + "'";
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