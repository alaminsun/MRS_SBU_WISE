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
    public class MarketSegmentProductDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;

        public int SegmentSl = 0;

        public ValidationMsg Save(MarketSegmentProductMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region MarketSegmentProductMas

                    string query = "select NVL(MAX(MKT_SEGM_PROD_MAS_SLNO),0)+1 from MKT_SEGM_PROD_MAS";
                    DataTable dt = dbHelper.GetDataTable(query);
                    if (dt.Rows.Count > 0)
                        model.MKT_SEGM_PROD_MAS_SLNO = dt.Rows[0][0].ToString();

                    string qry = "INSERT INTO MKT_SEGM_PROD_MAS(MKT_SEGM_PROD_MAS_SLNO,MKT_SEGMENT_CODE)" +
                    "VALUES(" + model.MKT_SEGM_PROD_MAS_SLNO + ", '" + model.MKT_SEGMENT_CODE + "')";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        SegmentSl = Convert.ToInt16(model.MKT_SEGM_PROD_MAS_SLNO);
                    }

                    #endregion

                    #region MarketSegmentProductDtl

                    if (model.MktSegmentList != null)
                    {
                        foreach (MarketSegmentProductDtlModel objProductsonModel in model.MktSegmentList)
                        {
                            DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(MKT_SEGM_PROD_DTL_SLNO),0)+1 from MKT_SEGM_PROD_DTL");
                            if (dtDtl.Rows.Count > 0)
                                objProductsonModel.MKT_SEGM_PROD_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                            objProductsonModel.MKT_SEGM_PROD_MAS_SLNO = SegmentSl.ToString();

                            qry = "INSERT INTO MKT_SEGM_PROD_DTL(MKT_SEGM_PROD_DTL_SLNO,MKT_SEGM_PROD_MAS_SLNO,PROD_ID,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                            "VALUES('" + objProductsonModel.MKT_SEGM_PROD_DTL_SLNO + "', '" + model.MKT_SEGM_PROD_MAS_SLNO + "', '" + objProductsonModel.PROD_ID + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                            if (dbHelper.CmdExecute(qry) > 0)
                            {
                                _vmMsg.Type = Enums.MessageType.Success;

                            }
                        }
                    }

                    #endregion

                    scope.Complete();

                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Saved Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Save.";

                if (ex.Message.Contains("UK_MKT_SEGMMENT_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Segment already Exist.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Update(MarketSegmentProductMasModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                #region Update

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region MarketSegmentProductMas

                    string qry = "UPDATE MKT_SEGM_PROD_MAS SET MKT_SEGMENT_CODE = '" + model.MKT_SEGMENT_CODE + "' WHERE MKT_SEGM_PROD_MAS_SLNO = '" + model.MKT_SEGM_PROD_MAS_SLNO + "'";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        _vmMsg.Type = Enums.MessageType.Update;
                        _vmMsg.Msg = "Updated Successfully.";
                    }

                    #endregion

                    #region MarketSegmentProductDtl

                    if (model.MktSegmentList != null)
                    {
                        foreach (MarketSegmentProductDtlModel objProductsonModel in model.MktSegmentList)
                        {
                            if (string.IsNullOrEmpty(objProductsonModel.MKT_SEGM_PROD_DTL_SLNO))
                            {
                                DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(MKT_SEGM_PROD_DTL_SLNO),0)+1 from MKT_SEGM_PROD_DTL");
                                if (dtDtl.Rows.Count > 0)
                                    objProductsonModel.MKT_SEGM_PROD_DTL_SLNO = dtDtl.Rows[0][0].ToString();

                                objProductsonModel.MKT_SEGM_PROD_MAS_SLNO = SegmentSl.ToString();

                                qry = "INSERT INTO MKT_SEGM_PROD_DTL(MKT_SEGM_PROD_DTL_SLNO,MKT_SEGM_PROD_MAS_SLNO,PROD_ID,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL)" +
                                "VALUES('" + objProductsonModel.MKT_SEGM_PROD_DTL_SLNO + "', '" + model.MKT_SEGM_PROD_MAS_SLNO + "', '" + objProductsonModel.PROD_ID + "','" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                                if (dbHelper.CmdExecute(qry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Success;
                                }
                            }
                            else
                            {
                                string updateqry = "UPDATE MKT_SEGM_PROD_DTL SET PROD_ID = '" + objProductsonModel.PROD_ID + "'," +
                                             "UPDATED_BY = '" + userid + "',UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                             "WHERE MKT_SEGM_PROD_DTL_SLNO = '" + objProductsonModel.MKT_SEGM_PROD_DTL_SLNO + "'";
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

                if (ex.Message.Contains("UK_MKT_SEGMMENT_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Segment already Exist.";
                }
            }
            return _vmMsg;
        }

        public List<MarketSegmentProductMasModel> GetSegmentList()
        {
            string Qry = "";
            Qry = "SELECT MKT_SEGMENT_CODE ,MKT_SEGMENT_NAME FROM MKT_SEGMENT ";
            Qry = Qry + "ORDER BY MKT_SEGMENT_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketSegmentProductMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketSegmentProductMasModel
                     {
                         MKT_SEGMENT_CODE = row["MKT_SEGMENT_CODE"].ToString(),
                         MKT_SEGMENT_NAME = row["MKT_SEGMENT_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<MarketSegmentProductMasModel> GetSegmentSearchList()
        {
            DataTable dt = dbHelper.GetDataTable("select ZM.MKT_SEGM_PROD_MAS_SLNO,Z.MKT_SEGMENT_CODE,Z.MKT_SEGMENT_NAME from MKT_SEGMENT Z inner join MKT_SEGM_PROD_MAS ZM on Z.MKT_SEGMENT_CODE = ZM.MKT_SEGMENT_CODE ORDER BY Z.MKT_SEGMENT_CODE");
            List<MarketSegmentProductMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketSegmentProductMasModel
                     {
                         MKT_SEGM_PROD_MAS_SLNO = row["MKT_SEGM_PROD_MAS_SLNO"].ToString(),
                         MKT_SEGMENT_CODE = row["MKT_SEGMENT_CODE"].ToString(),
                         MKT_SEGMENT_NAME = row["MKT_SEGMENT_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<MarketSegmentProductDtlModel> GetProductInfoList()
        {
            string Qry = "";
            Qry = "SELECT PROD_ID ,PRODUCT_NAME FROM PRODUCT ";
            Qry = Qry + "ORDER BY PROD_ID";

            DataTable dt = dbHelper.GetDataTable(Qry);
            List<MarketSegmentProductDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new MarketSegmentProductDtlModel
                     {
                         PROD_ID = row["PROD_ID"].ToString(),
                         PRODUCT_NAME = row["PRODUCT_NAME"].ToString()
                     }).ToList();

            return items;
        }

        public List<MarketSegmentProductDtlModel> GetSaveProductList(string MKT_SEGM_PROD_MAS_SLNO)
        {
            var items = new List<MarketSegmentProductDtlModel>();
            DataTable dt = dbHelper.GetDataTable("select ZM.MKT_SEGM_PROD_DTL_SLNO,Z.PROD_ID,Z.PRODUCT_NAME from PRODUCT Z inner join MKT_SEGM_PROD_DTL ZM on Z.PROD_ID = ZM.PROD_ID  WHERE MKT_SEGM_PROD_MAS_SLNO = " + MKT_SEGM_PROD_MAS_SLNO + " ORDER BY ZM.MKT_SEGM_PROD_DTL_SLNO");
            items = (from DataRow row in dt.Rows
                     select new MarketSegmentProductDtlModel
                     {
                         PROD_ID = row["PROD_ID"].ToString(),
                         PRODUCT_NAME = row["PRODUCT_NAME"].ToString(),
                         MKT_SEGM_PROD_DTL_SLNO = row["MKT_SEGM_PROD_DTL_SLNO"].ToString()
                     }).ToList();

            return items;
        }

        public ValidationMsg Delete(string MKT_SEGM_PROD_MAS_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM MKT_SEGM_PROD_MAS WHERE MKT_SEGM_PROD_MAS_SLNO = " + MKT_SEGM_PROD_MAS_SLNO + "";
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

        public ValidationMsg DeletedProductInfo(string MKT_SEGM_PROD_DTL_SLNO)
        {
            var vmMsg = new ValidationMsg();
            try
            {
                string qry = "DELETE FROM MKT_SEGM_PROD_DTL WHERE MKT_SEGM_PROD_DTL_SLNO = " + MKT_SEGM_PROD_DTL_SLNO + "";
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