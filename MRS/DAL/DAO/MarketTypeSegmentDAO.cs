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
    public class MarketTypeSegmentDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;

        public int TypeSl = 0;

        public ValidationMsg Save(MarketTypeSegmentMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                //{
                #region MarketTypeSegmentMas

                string query = "select NVL(MAX(MKT_TYPE_SEGM_MAS_SLNO),0)+1 from MKT_TYPE_SEGM_MAS";
                DataTable dt = dbHelper.GetDataTable(query);
                if (dt.Rows.Count > 0)
                    model.MKT_TYPE_SEGM_MAS_SLNO = dt.Rows[0][0].ToString();

                string qry = "INSERT INTO MKT_TYPE_SEGM_MAS(MKT_TYPE_SEGM_MAS_SLNO,MKT_TYPE_CODE)" +
                "VALUES(" + model.MKT_TYPE_SEGM_MAS_SLNO + ", '" + model.MKT_TYPE_CODE + "')";
                if (dbHelper.CmdExecute(qry) > 0)
                {
                    TypeSl = Convert.ToInt16(model.MKT_TYPE_SEGM_MAS_SLNO);
                }

                #endregion

                #region MarketTypeSegmentDtl

                if (model.MktSegmentList != null)
                {
                    foreach (MarketTypeSegmentDtlModel objSegmentsonModel in model.MktSegmentList)
                    {
                        DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(MKT_TYPE_SEGM_DTL_SLNO),0)+1 from MKT_TYPE_SEGM_DTL");
                        if (dtDtl.Rows.Count > 0)
                            objSegmentsonModel.MKT_TYPE_SEGM_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                        objSegmentsonModel.MKT_TYPE_SEGM_MAS_SLNO = TypeSl.ToString();

                        qry = "INSERT INTO MKT_TYPE_SEGM_DTL(MKT_TYPE_SEGM_DTL_SLNO,MKT_TYPE_SEGM_MAS_SLNO,MKT_SEGMENT_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                        "VALUES('" + objSegmentsonModel.MKT_TYPE_SEGM_DTL_SLNO + "', '" + model.MKT_TYPE_SEGM_MAS_SLNO + "', '" + objSegmentsonModel.MKT_SEGMENT_CODE + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                        if (dbHelper.CmdExecute(qry) > 0)
                        {
                            _vmMsg.Type = Enums.MessageType.Success;

                        }
                    }
                }

                #endregion

                //scope.Complete();

                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Saved Successfully.";
                //}
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Save.";

                if (ex.Message.Contains("UK_MKT_TYPE_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Type already Exist.";
                }
                else if (ex.Message.Contains("UK_MKT_SEGMENT_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Segment already Exist.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Update(MarketTypeSegmentMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                #region Update

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region MarketTypeSegmentMas

                    string qry = "UPDATE MKT_TYPE_SEGM_MAS SET MKT_TYPE_CODE = '" + model.MKT_TYPE_CODE + "' WHERE MKT_TYPE_SEGM_MAS_SLNO = '" + model.MKT_TYPE_SEGM_MAS_SLNO + "'";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        _vmMsg.Type = Enums.MessageType.Update;
                        _vmMsg.Msg = "Updated Successfully.";
                    }

                    #endregion

                    #region MarketTypeSegmentDtl

                    if (model.MktSegmentList != null)
                    {
                        foreach (MarketTypeSegmentDtlModel objSegmentsonModel in model.MktSegmentList)
                        {
                            if (string.IsNullOrEmpty(objSegmentsonModel.MKT_TYPE_SEGM_DTL_SLNO))
                            {
                                DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(MKT_TYPE_SEGM_DTL_SLNO),0)+1 from MKT_TYPE_SEGM_DTL");
                                if (dtDtl.Rows.Count > 0)
                                    objSegmentsonModel.MKT_TYPE_SEGM_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                                objSegmentsonModel.MKT_TYPE_SEGM_MAS_SLNO = TypeSl.ToString();

                                qry = "INSERT INTO MKT_TYPE_SEGM_DTL(MKT_TYPE_SEGM_DTL_SLNO,MKT_TYPE_SEGM_MAS_SLNO,MKT_SEGMENT_CODE,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objSegmentsonModel.MKT_TYPE_SEGM_DTL_SLNO + "', '" + model.MKT_TYPE_SEGM_MAS_SLNO + "', '" + objSegmentsonModel.MKT_SEGMENT_CODE + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                                if (dbHelper.CmdExecute(qry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Success;
                                }
                            }
                            else
                            {
                                string updateqry = "UPDATE MKT_TYPE_SEGM_DTL SET MKT_SEGMENT_CODE = '" + objSegmentsonModel.MKT_SEGMENT_CODE + "'," +
                                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                             "WHERE MKT_TYPE_SEGM_DTL_SLNO = '" + objSegmentsonModel.MKT_TYPE_SEGM_DTL_SLNO + "'";
                                if (dbHelper.CmdExecute(updateqry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Update;
                                    _vmMsg.Msg = "Updated Successfully.";
                                }
                            }
                        }
                    }

                    #endregion

                    scope.Complete();

                    _vmMsg.Type = Enums.MessageType.Update;
                    _vmMsg.Msg = "Updated Successfully.";
                }

                #endregion
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Update.";

                if (ex.Message.Contains("UK_MKT_TYPE_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Type already Exist.";
                }
                else if (ex.Message.Contains("UK_MKT_SEGMENT_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Segment already Exist.";
                }
            }
            return _vmMsg;
        }

        public List<MarketTypeSegmentMasModel> GetTypeList()
        {
            string Qry = "";
            Qry = "SELECT MKT_TYPE_CODE ,MKT_TYPE_NAME FROM MKT_TYPE ";
            Qry = Qry + "ORDER BY MKT_TYPE_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketTypeSegmentMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketTypeSegmentMasModel
                     {
                         MKT_TYPE_CODE = row["MKT_TYPE_CODE"].ToString(),
                         MKT_TYPE_NAME = row["MKT_TYPE_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<MarketTypeSegmentMasModel> GetTypeSearchList()
        {
            DataTable dt = dbHelper.GetDataTable("select ZM.MKT_TYPE_SEGM_MAS_SLNO,Z.MKT_TYPE_CODE,Z.MKT_TYPE_NAME from MKT_TYPE Z inner join MKT_TYPE_SEGM_MAS ZM on Z.MKT_TYPE_CODE = ZM.MKT_TYPE_CODE ORDER BY Z.MKT_TYPE_CODE");
            List<MarketTypeSegmentMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketTypeSegmentMasModel
                     {
                         MKT_TYPE_SEGM_MAS_SLNO = row["MKT_TYPE_SEGM_MAS_SLNO"].ToString(),
                         MKT_TYPE_CODE = row["MKT_TYPE_CODE"].ToString(),
                         MKT_TYPE_NAME = row["MKT_TYPE_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<MarketTypeSegmentDtlModel> GetSegmentInfoList()
        {
            string Qry = "";
            Qry = "SELECT MKT_SEGMENT_CODE ,MKT_SEGMENT_NAME FROM MKT_SEGMENT ";
            Qry = Qry + "ORDER BY MKT_SEGMENT_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketTypeSegmentDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketTypeSegmentDtlModel
                     {
                         MKT_SEGMENT_CODE = row["MKT_SEGMENT_CODE"].ToString(),
                         MKT_SEGMENT_NAME = row["MKT_SEGMENT_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<MarketTypeSegmentDtlModel> GetSaveSegmentList(string MKT_TYPE_SEGM_MAS_SLNO)
        {
            var items = new List<MarketTypeSegmentDtlModel>();
            DataTable dt = dbHelper.GetDataTable("select ZM.MKT_TYPE_SEGM_DTL_SLNO,Z.MKT_SEGMENT_CODE,Z.MKT_SEGMENT_NAME from MKT_SEGMENT Z inner join MKT_TYPE_SEGM_DTL ZM on Z.MKT_SEGMENT_CODE = ZM.MKT_SEGMENT_CODE  WHERE MKT_TYPE_SEGM_MAS_SLNO = " + MKT_TYPE_SEGM_MAS_SLNO + " ORDER BY ZM.MKT_TYPE_SEGM_DTL_SLNO");
            items = (from DataRow row in dt.Rows
                     select new MarketTypeSegmentDtlModel
                     {
                         MKT_SEGMENT_CODE = row["MKT_SEGMENT_CODE"].ToString(),
                         MKT_SEGMENT_NAME = row["MKT_SEGMENT_NAME"].ToString(),
                         MKT_TYPE_SEGM_DTL_SLNO = row["MKT_TYPE_SEGM_DTL_SLNO"].ToString()
                     }).ToList();

            return items;
        }

        public ValidationMsg Delete(string MKT_TYPE_SEGM_MAS_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM MKT_TYPE_SEGM_MAS WHERE MKT_TYPE_SEGM_MAS_SLNO = " + MKT_TYPE_SEGM_MAS_SLNO + "";
                if (dbHelper.CmdExecute(qry) > 0)
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

        public ValidationMsg DeletedSegmentInfo(string MKT_TYPE_SEGM_DTL_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM MKT_TYPE_SEGM_DTL WHERE MKT_TYPE_SEGM_DTL_SLNO = " + MKT_TYPE_SEGM_DTL_SLNO + "";
                if (dbHelper.CmdExecute(qry) > 0)
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
    }
}