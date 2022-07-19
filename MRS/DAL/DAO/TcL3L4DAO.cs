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
    public class TcL3L4DAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg = new ValidationMsg();
        List<TcL3L4DtlModel> items;
        public int TcL3L4Sl = 0;
        private List<TcL3L4MstModel> itemMasterList;
        private List<TcL3L4DtlModel> itemDetailList;



        public List<TcL3L4MstModel> GetTcL3L4MstList()
        {
            string Qry = "";
            Qry = "SELECT TC_L3_CODE ,TC_L3_DESC FROM TC_LEVEL3 ";
            Qry = Qry + "ORDER BY TC_L3_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<TcL3L4MstModel> items;
            items = (from DataRow row in dt.Rows
                     select new TcL3L4MstModel
                     {
                         TC_L3_CODE = row["TC_L3_CODE"].ToString(),
                         TC_L3_DESC = row["TC_L3_DESC"].ToString()
                     }).ToList();

            return items;
        }

        public List<TcL3L4DtlModel> GetTcL3L4DtlList()
        {
            string Qry = "";
            Qry = "SELECT TC_L4_CODE ,TC_L4_DESC FROM TC_LEVEL4 ";
            Qry = Qry + "ORDER BY TC_L4_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<TcL3L4DtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new TcL3L4DtlModel
                     {
                         TC_L4_CODE = row["TC_L4_CODE"].ToString(),
                         TC_L4_DESC = row["TC_L4_DESC"].ToString()
                     }).ToList();

            return items;
        }



        public ValidationMsg Save(TcL3L4MstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    DataTable dataTable =
                        dbHelper.GetDataTable(
                            "SELECT (NVL(MAX(TC_L3_L4_MST_SLNO),0)+1)TC_L3_L4_MST_SLNO FROM TC_L3_L4_MST");
                    if (dataTable.Rows.Count > 0)
                    {
                        model.TC_L3_L4_MST_SLNO = dataTable.Rows[0][0].ToString();
                    }
                    string query = "INSERT INTO TC_L3_L4_MST(TC_L3_L4_MST_SLNO,TC_L3_CODE)" +
                                   "VALUES(" + model.TC_L3_L4_MST_SLNO + ", '" + model.TC_L3_CODE + "')";
                    if (dbHelper.CmdExecute(query) > 0)
                    {
                        TcL3L4Sl = Convert.ToInt16(model.TC_L3_L4_MST_SLNO);
                    }
                    if (model.TcL3L4DtlList != null)
                    {
                        foreach (TcL3L4DtlModel objTcL3L4DtlModel in model.TcL3L4DtlList)
                        {
                            DataTable dataTableDtl =
                                dbHelper.GetDataTable(
                                    "SELECT (NVL(MAX(TC_L3_L4_DTL_SLNO),0)+1)TC_L3_L4_DTL_SLNO FROM TC_L3_L4_DTL");
                            if (dataTableDtl.Rows.Count > 0)
                            {
                                objTcL3L4DtlModel.TC_L3_L4_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
                                objTcL3L4DtlModel.TC_L3_L4_MST_SLNO = TcL3L4Sl.ToString();//New added Line which is in Query Also
                            }
                            string querydtl =
                                "INSERT INTO TC_L3_L4_DTL(TC_L3_L4_DTL_SLNO,TC_L3_L4_MST_SLNO,TC_L4_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objTcL3L4DtlModel.TC_L3_L4_DTL_SLNO + "', '" + model.TC_L3_L4_MST_SLNO +
                                "', '" + objTcL3L4DtlModel.TC_L4_CODE + "', '" + userid + "',(TO_DATE('" +
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

        public ValidationMsg Update(TcL3L4MstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string query = "";
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    if (model.TC_L3_L4_MST_SLNO != "")
                    {
                        string qry = "SELECT TC1.TC_L3_L4_MST_SLNO,TC1.TC_L3_CODE,TL1.TC_L3_DESC FROM TC_L3_L4_MST TC1 INNER JOIN TC_LEVEL3 TL1 ON TC1.TC_L3_CODE=TL1.TC_L3_CODE";
                        qry = qry + " ORDER BY TC1.TC_L3_L4_MST_SLNO";

                        DataTable dt = dbHelper.GetDataTable(qry);
                        itemMasterList = (from DataRow row in dt.Rows
                                          select new TcL3L4MstModel
                                          {
                                              TC_L3_CODE = row["TC_L3_CODE"].ToString(),
                                              TC_L3_DESC = row["TC_L3_DESC"].ToString(),
                                              TC_L3_L4_MST_SLNO = row["TC_L3_L4_MST_SLNO"].ToString()
                                          }).ToList();
                        if (itemMasterList.Count > 0)
                        {
                            query = "UPDATE TC_L3_L4_MST SET TC_L3_CODE = '" + model.TC_L3_CODE + "' WHERE TC_L3_L4_MST_SLNO = '" + model.TC_L3_L4_MST_SLNO + "'";
                        }

                        dbHelper.CmdExecute(query);
                    }
                    if (model.TcL3L4DtlList != null)
                    {
                        foreach (TcL3L4DtlModel objTcL1L2DtlUpdateModel in model.TcL3L4DtlList)
                        {
                            if (objTcL1L2DtlUpdateModel.TC_L3_L4_DTL_SLNO != null)
                            {

                                string querydtl =
                                                "UPDATE TC_L3_L4_DTL SET TC_L4_CODE = '" + objTcL1L2DtlUpdateModel.TC_L4_CODE + "'," +
                                                "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                                "WHERE TC_L3_L4_MST_SLNO = '" + model.TC_L3_L4_MST_SLNO + "' AND TC_L3_L4_DTL_SLNO = '" + objTcL1L2DtlUpdateModel.TC_L3_L4_DTL_SLNO + "'";

                                dbHelper.CmdExecute(querydtl);
                            }
                            else
                            {
                                foreach (TcL3L4DtlModel objTcL3L4DtlModel in model.TcL3L4DtlList)
                                {
                                    DataTable dataTableDtl =
                                        dbHelper.GetDataTable(
                                            "SELECT (NVL(MAX(TC_L3_L4_DTL_SLNO),0)+1)TC_L3_L4_DTL_SLNO FROM TC_L3_L4_DTL");
                                    if (dataTableDtl.Rows.Count > 0)
                                    {
                                        objTcL3L4DtlModel.TC_L3_L4_DTL_SLNO = dataTableDtl.Rows[0][0].ToString();
                                        objTcL3L4DtlModel.TC_L3_L4_MST_SLNO = TcL3L4Sl.ToString();//New added Line which is in Query Also
                                    }
                                    string querydtl =
                                        "INSERT INTO TC_L3_L4_DTL(TC_L3_L4_DTL_SLNO,TC_L3_L4_MST_SLNO,TC_L4_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                        "VALUES('" + objTcL3L4DtlModel.TC_L3_L4_DTL_SLNO + "', '" + model.TC_L3_L4_MST_SLNO +
                                        "', '" + objTcL3L4DtlModel.TC_L4_CODE + "', '" + userid + "',(TO_DATE('" +
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

        public List<TcL3L4MstModel> GetSearchMasterList()
        {
            string qry = "SELECT TC1.TC_L3_L4_MST_SLNO,TC1.TC_L3_CODE,TL1.TC_L3_DESC FROM TC_L3_L4_MST TC1 INNER JOIN TC_LEVEL3 TL1 ON TC1.TC_L3_CODE=TL1.TC_L3_CODE";
            qry = qry + " ORDER BY TC1.TC_L3_L4_MST_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemMasterList = (from DataRow row in dt.Rows
                              select new TcL3L4MstModel
                              {
                                  TC_L3_CODE = row["TC_L3_CODE"].ToString(),
                                  TC_L3_DESC = row["TC_L3_DESC"].ToString(),
                                  TC_L3_L4_MST_SLNO = row["TC_L3_L4_MST_SLNO"].ToString()
                              }).ToList();
            return itemMasterList;
        }

        public List<TcL3L4DtlModel> GetSearchDtliList(string tcL3L4MstSlNo)
        {
            string qry = "SELECT TC2.TC_L3_L4_DTL_SLNO,TC2.TC_L3_L4_MST_SLNO,TC2.TC_L4_CODE,TL2.TC_L4_DESC FROM TC_L3_L4_DTL TC2 INNER JOIN TC_LEVEL4 TL2 ON TC2.TC_L4_CODE=TL2.TC_L4_CODE WHERE TC2.TC_L3_L4_MST_SLNO='" + tcL3L4MstSlNo + "'";
            qry = qry + " ORDER BY TC2.TC_L3_L4_MST_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemDetailList = (from DataRow row in dt.Rows
                              select new TcL3L4DtlModel
                              {
                                  TC_L4_CODE = row["TC_L4_CODE"].ToString(),
                                  TC_L4_DESC = row["TC_L4_DESC"].ToString(),
                                  TC_L3_L4_DTL_SLNO = row["TC_L3_L4_DTL_SLNO"].ToString()
                              }).ToList();
            return itemDetailList;
            //DataTable dt = dbHelper.GetDataTable(qry);
            // TcL3L4DtlModel objTcL1L2DtllModel = new TcL3L4DtlModel();
            // foreach (DataRow row in dt.Rows)
            // {
            //     objTcL1L2DtllModel.TC_L4_CODE = row["TC_L4_CODE"].ToString();
            //     objTcL1L2DtllModel.TC_L4_DESC = row["TC_L4_DESC"].ToString();
            //     //objTcL1L2DtllModel.TC_L3_L4_MST_SLNO = row["TC_L3_L4_MST_SLNO"].ToString();
            //     objTcL1L2DtllModel.TC_L3_L4_DTL_SLNO = row["TC_L3_L4_DTL_SLNO"].ToString();
            //     itemDetailList.Add(objTcL1L2DtllModel);
            // }

            // return itemDetailList;
        }


        public ValidationMsg Delete(string tcL3L4MstSlNo)
        {
            try
            {
                string querymaster = "DELETE FROM TC_L3_L4_MST WHERE TC_L3_L4_MST_SLNO = '" +
                  tcL3L4MstSlNo + "'";

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

        public ValidationMsg DeleteDetailData(string tcL3L4DtlSlNo)
        {
            try
            {

                string deleteDetailQuery = "DELETE FROM TC_L3_L4_DTL WHERE TC_L3_L4_DTL_SLNO = '" + tcL3L4DtlSlNo + "'";
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