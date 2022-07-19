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
    public class ProductMarketTypeRelationDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg = new ValidationMsg();
        //private readonly ValidationMsg _validationMsg = new ValidationMsg();
        DBConnection dbCon = new DBConnection();
        IDGenerated idGenerated = new IDGenerated();

        public List<ProductMarketTypeRelationMasModel> GetProductList()
        {
            string Qry = "";
            //Qry = " SELECT * From PRODUCT ORDER BY PROD_ID";
            Qry = " SELECT a.PROD_ID,a.PRODUCT_NAME,a.GENERIC_CODE,B.GENERIC_NAME,a.DOSAGE_FORM_CODE,C.DOSAGE_FORM_NAME,a.DSG_STRENGTH_CODE,D.DSG_STRENGTH_NAME, A.PACK_SIZE From PRODUCT a " +
                  " Left Join Generic b on A.GENERIC_CODE = B.GENERIC_CODE Left Join DOSAGE_FORM c on A.DOSAGE_FORM_CODE = c.DOSAGE_FORM_CODE Left Join DOSAGE_STRENGTH d on A.DSG_STRENGTH_CODE = D.DSG_STRENGTH_CODE ORDER BY PROD_ID";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ProductMarketTypeRelationMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new ProductMarketTypeRelationMasModel
                     {
                         ProductCode = row["PROD_ID"].ToString(),
                         ProductName = row["PRODUCT_NAME"].ToString(),
                         GenericCode = row["GENERIC_CODE"].ToString(),
                         GenericName = row["GENERIC_NAME"].ToString(),
                         DosageFormCode = row["DOSAGE_FORM_CODE"].ToString(),
                         DosageFormName = row["DOSAGE_FORM_NAME"].ToString(),
                         DosageStrengthCode = row["DSG_STRENGTH_CODE"].ToString(),
                         DosageStrengthName = row["DSG_STRENGTH_NAME"].ToString(),
                         PackSize = row["PACK_SIZE"].ToString()
                         
                     }).ToList();

            return items;
        }
        public List<ProductMarketTypeRelationDtlModel> GetMarketTypeList()
        {
            string Qry = "";
            Qry = " SELECT * From MKT_TYPE ORDER BY MKT_TYPE_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ProductMarketTypeRelationDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new ProductMarketTypeRelationDtlModel
                     {
                         MarketTypeCode = row["MKT_TYPE_CODE"].ToString(),
                         MarketTypeName = row["MKT_TYPE_NAME"].ToString()

                     }).ToList();

            return items;
        }

        //public List<ProductMarketTypeRelationDtlModel> GetDataForProductGroupDropDown(string MarketTypeCode, string ProductGroup)
        //{
        //    string qry = "Select *  from MKT_TYPE Where MKT_TYPE_CODE='" + MarketTypeCode + "' and PRODUCT_GROUP  in (Select MP from MPO_MKT_MAPPING Where  MARKET_CODE='" + mktCode + "') Order by PRODUCT_GROUP";
        //    //string qry = "Select (MARKET_GROUP||PRODUCT_GROUP) MPGroup  from MP_GROUP Where MARKET_GROUP='" + mktGroup + "' and MARKET_GROUP||PRODUCT_GROUP  in (Select MP from MPO_MKT_MAPPING Where  MARKET_CODE='" + mktCode + "') Order by PRODUCT_GROUP";
        //    DataTable dt = dbHelper.GetDataTable(dbConn.SAConnStrReader(), qry);
        //    List<MPOMarketMappingBEL> item;
        //    item = (from DataRow row in dt.Rows
        //            select new MPOMarketMappingBEL
        //            {

        //                MPGroup = row["MPGroup"].ToString()

        //            }).ToList();
        //    int c = item.Count;
        //    if (c < 1)
        //    {
        //        MPOMarketMappingBEL mpoMarketMappingBel = new MPOMarketMappingBEL();
        //        mpoMarketMappingBel.MPGroup = MP;


        //        item.Add(mpoMarketMappingBel);

        //    }
        //    return item;
        //}

        public ValidationMsg Save(ProductMarketTypeRelationMasModel Master, string userid)
        {
            OracleConnection oracleConnection = new OracleConnection(dbCon.StringRead());
            try
            {
                if (IsMSTExitsByProductCode(Master.ProductCode))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Data Already Exist.";
                }
                else
                {
                    Master.ProductMarketTypeMstSl = idGenerated.getMAXSL("PROD_MKT_TYPE_MAS_SLNO", "PROD_MKT_TYPE_MAS").ToString();
                    if (Master.ProductMarketList.Count() > 0)
                    {
                        string qry = "INSERT INTO PROD_MKT_TYPE_MAS(PROD_MKT_TYPE_MAS_SLNO,PROD_ID) VALUES(" + Master.ProductMarketTypeMstSl + ", '" + Master.ProductCode + "')";
                        dbHelper.CmdExecute(qry);
                        foreach (var detailModel in Master.ProductMarketList)
                        {
                            detailModel.ProductMarketTypeDtlSl = idGenerated.getMAXSL("PROD_MKT_TYPE_DTL_SLNO", "PROD_MKT_TYPE_DTL").ToString();
                            qry = "INSERT INTO PROD_MKT_TYPE_DTL(PROD_MKT_TYPE_MAS_SLNO,PROD_MKT_TYPE_DTL_SLNO, MKT_TYPE_CODE,PROD_GROUP,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + Master.ProductMarketTypeMstSl + "', '" + detailModel.ProductMarketTypeDtlSl + "', '" + detailModel.MarketTypeCode + "', '" + detailModel.ProductGroup + "', '" + userid + "',(TO_DATE('" + detailModel.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                            dbHelper.CmdExecute(qry);
                        }
                        _vmMsg.Type = Enums.MessageType.Success;
                        _vmMsg.Msg = "Save Successful.";
                    }
                }


            }

            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed To Save Data";
                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "This Data Already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }

        public ValidationMsg Update(ProductMarketTypeRelationMasModel Master, string userid)
        {
            string MstSL = "";
            OracleConnection oracleConnection = new OracleConnection(dbCon.StringRead());
            //_vmMsg = new ValidationMsg();
            MstSL = GetMstSLNO(Master.ProductCode).ToString();
            foreach (var detailModel in Master.ProductMarketList)
            {
                if (IsDTLExistByMarketType(detailModel.MarketTypeCode, MstSL))
                {
                    string query = "Update PROD_MKT_TYPE_DTL Set MKT_TYPE_CODE='" + detailModel.MarketTypeCode + "', PROD_GROUP='" + detailModel.ProductGroup + "' WHERE PROD_MKT_TYPE_DTL_SLNO = '" + detailModel.ProductMarketTypeDtlSl + "'";
                    dbHelper.CmdExecute(query);
                }
                else
                {
                    detailModel.ProductMarketTypeDtlSl = idGenerated.getMAXSL("PROD_MKT_TYPE_DTL_SLNO", "PROD_MKT_TYPE_DTL").ToString();
                    string queryDtl = "INSERT INTO PROD_MKT_TYPE_DTL(PROD_MKT_TYPE_MAS_SLNO,PROD_MKT_TYPE_DTL_SLNO, MKT_TYPE_CODE,PROD_GROUP,UPDATED_BY,UPDATED_DATE,UPDATED_TERMINAL)" +
                                      "VALUES('" + MstSL + "', '" + detailModel.ProductMarketTypeDtlSl + "', '" + detailModel.MarketTypeCode + "', '" + detailModel.ProductGroup + "', '" + userid + "',(TO_DATE('" + detailModel.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "')";
                    dbHelper.CmdExecute(queryDtl);

                }
            }

            _vmMsg.Type = Enums.MessageType.Success;
            _vmMsg.Msg = "Update Successful.";
            return _vmMsg;
        }

        private bool IsDTLExistByMarketType(string MarketTypeCode, string MstSL)
        {
            bool isTrue = false;
            string Qry = "SELECT PROD_MKT_TYPE_DTL_SLNO FROM PROD_MKT_TYPE_DTL WHERE MKT_TYPE_CODE = '" + MarketTypeCode + "' AND PROD_MKT_TYPE_MAS_SLNO = '" + MstSL + "'";
            DataTable dt = dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }

        private string GetMstSLNO(string ProductCode)
        {
            string SL = "0";
            string Qry = "Select PROD_MKT_TYPE_MAS_SLNO from PROD_MKT_TYPE_MAS Where PROD_ID='" + ProductCode + "'";
            DataTable dt = dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                SL = dt.Rows[0][0].ToString();
            }

            return SL;
        }

        private bool IsMSTExitsByProductCode(string ProductCode)
        {
            bool isTrue = false;
            string Qry = "SELECT * FROM PROD_MKT_TYPE_MAS WHERE PROD_ID = '" + ProductCode + "'";
            DataTable dt = dbHelper.GetDataTable(Qry);
            if (dt.Rows.Count > 0)
            {
                isTrue = true;
            }

            return isTrue;
        }



        public List<ProductMarketTypeRelationMasModel> GetProductSearchList()
        {
            string Qry = "";
            //Qry = " SELECT * From PRODUCT ORDER BY PROD_ID";
            Qry = " SELECT M.PROD_MKT_TYPE_MAS_SLNO,a.PROD_ID,a.PRODUCT_NAME,a.GENERIC_CODE,B.GENERIC_NAME, " +
                  "   a.DOSAGE_FORM_CODE,C.DOSAGE_FORM_NAME,a.DSG_STRENGTH_CODE,D.DSG_STRENGTH_NAME, A.PACK_SIZE From PRODUCT a "+
                  "   Left Join Generic b on A.GENERIC_CODE = B.GENERIC_CODE "+
                  "   Left Join DOSAGE_FORM c on A.DOSAGE_FORM_CODE = c.DOSAGE_FORM_CODE "+
                  "   Left Join DOSAGE_STRENGTH d on A.DSG_STRENGTH_CODE = D.DSG_STRENGTH_CODE "+
                  "   inner join PROD_MKT_TYPE_MAS M on A.PROD_ID = M.PROD_ID ORDER BY PROD_ID";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ProductMarketTypeRelationMasModel> items;
            items = (from DataRow row in dt.Rows
                     select new ProductMarketTypeRelationMasModel
                     {
                         ProductMarketTypeMstSl = row["PROD_MKT_TYPE_MAS_SLNO"].ToString(),
                         ProductCode = row["PROD_ID"].ToString(),
                         ProductName = row["PRODUCT_NAME"].ToString(),
                         GenericCode = row["GENERIC_CODE"].ToString(),
                         GenericName = row["GENERIC_NAME"].ToString(),
                         DosageFormCode = row["DOSAGE_FORM_CODE"].ToString(),
                         DosageFormName = row["DOSAGE_FORM_NAME"].ToString(),
                         DosageStrengthCode = row["DSG_STRENGTH_CODE"].ToString(),
                         DosageStrengthName = row["DSG_STRENGTH_NAME"].ToString(),
                         PackSize = row["PACK_SIZE"].ToString()

                     }).ToList();

            return items;
        }

        public List<ProductMarketTypeRelationDtlModel> GetMarketTypeSearchList(string ProductMarketTypeMstSl)
        {
            string Qry = "";
            //Qry = " SELECT * From PRODUCT ORDER BY PROD_ID";
            Qry = "Select A.PROD_MKT_TYPE_MAS_SLNO, A.PROD_MKT_TYPE_DTL_SLNO, A.MKT_TYPE_CODE  ,B.MKT_TYPE_NAME, A.PROD_GROUP From PROD_MKT_TYPE_DTL a , MKT_TYPE b where A.MKT_TYPE_CODE = B.MKT_TYPE_CODE AND A.PROD_MKT_TYPE_MAS_SLNO = '" + ProductMarketTypeMstSl + "'  order by MKT_TYPE_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ProductMarketTypeRelationDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new ProductMarketTypeRelationDtlModel
                     {
                         ProductMarketTypeMstSl = row["PROD_MKT_TYPE_MAS_SLNO"].ToString(),
                         MarketTypeCode = row["MKT_TYPE_CODE"].ToString(),
                         MarketTypeName = row["MKT_TYPE_NAME"].ToString(),
                         ProductGroup = row["PROD_GROUP"].ToString(),
                         ProductMarketTypeDtlSl = row["PROD_MKT_TYPE_DTL_SLNO"].ToString(),
                         //DosageFormName = row["DOSAGE_FORM_NAME"].ToString(),
                         //DosageStrengthCode = row["DSG_STRENGTH_CODE"].ToString(),
                         //DosageStrengthName = row["DSG_STRENGTH_NAME"].ToString(),
                         //PackSize = row["PACK_SIZE"].ToString()

                     }).ToList();

            return items;
        }

        public ValidationMsg Delete(string model)
        {

            try
            {
                var detailData = GetAllMarketTypeDtlDataById(model);
                if (detailData.Count > 0)
                {
                    foreach (var data in detailData)
                    {
                        string queryDtl = "DELETE FROM PROD_MKT_TYPE_DTL WHERE PROD_MKT_TYPE_DTL_SLNO='" + model + "'";
                        dbHelper.CmdExecute(queryDtl);
                    }
                }

                string queryMst = "DELETE FROM PROD_MKT_TYPE_MAS WHERE PROD_MKT_TYPE_MAS_SLNO='" + model + "'";
                if (dbHelper.CmdExecute(queryMst) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Delete Data.";
            }
            return _vmMsg;

        }



        public List<ProductMarketTypeRelationDtlModel> GetAllMarketTypeDtlDataById(string model)
        {
            string Qry = "";
            //Qry = " SELECT * From PRODUCT ORDER BY PROD_ID";
            Qry = "Select A.PROD_MKT_TYPE_MAS_SLNO, A.PROD_MKT_TYPE_DTL_SLNO, A.MKT_TYPE_CODE  ,B.MKT_TYPE_NAME, A.PROD_GROUP From PROD_MKT_TYPE_DTL a , MKT_TYPE b where A.MKT_TYPE_CODE = B.MKT_TYPE_CODE AND A.PROD_MKT_TYPE_MAS_SLNO = '" + model + "'  order by MKT_TYPE_NAME";
            DataTable dt = dbHelper.GetDataTable(Qry);
            List<ProductMarketTypeRelationDtlModel> items;
            items = (from DataRow row in dt.Rows
                     select new ProductMarketTypeRelationDtlModel
                     {
                         ProductMarketTypeMstSl = row["PROD_MKT_TYPE_MAS_SLNO"].ToString(),
                         MarketTypeCode = row["MKT_TYPE_CODE"].ToString(),
                         MarketTypeName = row["MKT_TYPE_NAME"].ToString(),
                         ProductGroup = row["PROD_GROUP"].ToString(),
                         ProductMarketTypeDtlSl = row["PROD_MKT_TYPE_DTL_SLNO"].ToString(),
                         //DosageFormName = row["DOSAGE_FORM_NAME"].ToString(),
                         //DosageStrengthCode = row["DSG_STRENGTH_CODE"].ToString(),
                         //DosageStrengthName = row["DSG_STRENGTH_NAME"].ToString(),
                         //PackSize = row["PACK_SIZE"].ToString()

                     }).ToList();

            return items;
        }
        public ValidationMsg DeleteByDtlId(string dtlSlNo)
        {

            try
            {
                string query = "DELETE FROM PROD_MKT_TYPE_DTL WHERE PROD_MKT_TYPE_DTL_SLNO='" + dtlSlNo + "'";
                if (dbHelper.CmdExecute(query) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Delete Data.";
            }
            return _vmMsg;
        }
    }
}