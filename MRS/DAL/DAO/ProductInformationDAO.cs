using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using MRS.DAL.Gateway;
using MRS.Models;
using System.Data;
using MRS.DAL.Common;
using System.Text.RegularExpressions;

namespace MRS.DAL.DAO
{
    public class ProductInformationDAO
    {
        private readonly DBHelper _dbHelper = new DBHelper();
        DataTable _dataTable = new DataTable();
        private readonly ValidationMsg _validationMsg = new ValidationMsg();
        string code = string.Empty;

        public List<ProductInformationModel> GetProductInformationList()
        {
            string Qry =
                "SELECT P.PROD_ID,P.PRODUCT_NAME,TL1.TC_L1_CODE,TL1.TC_L1_DESC,TL2.TC_L2_CODE,TL2.TC_L2_DESC,TL3.TC_L3_CODE,TL3.TC_L3_DESC,TL4.TC_L4_CODE,TL4.TC_L4_DESC,G.GENERIC_CODE,G.GENERIC_NAME,DF.DOSAGE_FORM_CODE,DF.DOSAGE_FORM_NAME," +
                " DS.DSG_STRENGTH_CODE,DS.DSG_STRENGTH_NAME,M.MANUFACTURER_CODE,M.MANUFACTURER_NAME,P.UNIT_PRICE,P.AVG_PRESCRIBE_QTY,P.AVG_BOUGHT_QTY,P.PACK_SIZE," +
                " P.SAP_CODE,P.PROD_STATUS,P.SBU_GROUP FROM   PRODUCT P LEFT JOIN TC_LEVEL1 TL1 ON P.TC_L1_CODE = TL1.TC_L1_CODE LEFT JOIN TC_LEVEL2 TL2 ON P.TC_L2_CODE = TL2.TC_L2_CODE LEFT JOIN TC_LEVEL3 TL3 ON P.TC_L3_CODE = TL3.TC_L3_CODE" +
                " LEFT JOIN TC_LEVEL4 TL4 ON P.TC_L4_CODE = TL4.TC_L4_CODE LEFT JOIN GENERIC G ON P.GENERIC_CODE = G.GENERIC_CODE LEFT JOIN DOSAGE_FORM DF ON P.DOSAGE_FORM_CODE = DF.DOSAGE_FORM_CODE LEFT JOIN DOSAGE_STRENGTH DS ON P.DSG_STRENGTH_CODE = DS.DSG_STRENGTH_CODE" +
                " LEFT JOIN MANUFACTURER M ON P.MANUFACTURER_CODE = M.MANUFACTURER_CODE ORDER BY PROD_ID";
            _dataTable = _dbHelper.GetDataTable(Qry);
            List<ProductInformationModel> itemList = new List<ProductInformationModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new ProductInformationModel
                        {
                            PROD_ID = row["PROD_ID"].ToString(),
                            PRODUCT_NAME = row["PRODUCT_NAME"].ToString(),
                            TC_L1_CODE = row["TC_L1_CODE"].ToString(),
                            TC_L2_CODE = row["TC_L2_CODE"].ToString(),
                            TC_L3_CODE = row["TC_L3_CODE"].ToString(),
                            TC_L4_CODE = row["TC_L4_CODE"].ToString(),

                            TC_L1_DESC = row["TC_L1_DESC"].ToString(),
                            TC_L2_DESC = row["TC_L2_DESC"].ToString(),
                            TC_L3_DESC = row["TC_L3_DESC"].ToString(),
                            TC_L4_DESC = row["TC_L4_DESC"].ToString(),
                            GENERIC_NAME = row["GENERIC_NAME"].ToString(),
                            DOSAGE_FORM_NAME = row["DOSAGE_FORM_NAME"].ToString(),
                            DSG_STRENGTH_NAME = row["DSG_STRENGTH_NAME"].ToString(),
                            MANUFACTURER_NAME = row["MANUFACTURER_NAME"].ToString(),

                            GENERIC_CODE = row["GENERIC_CODE"].ToString(),
                            DOSAGE_FORM_CODE = row["DOSAGE_FORM_CODE"].ToString(),
                            DSG_STRENGTH_CODE = row["DSG_STRENGTH_CODE"].ToString(),
                            MANUFACTURER_CODE = row["MANUFACTURER_CODE"].ToString(),
                            UNIT_PRICE = row["UNIT_PRICE"].ToString(),
                            AVG_PRESCRIBE_QTY = row["AVG_PRESCRIBE_QTY"].ToString(),
                            AVG_BOUGHT_QTY = row["AVG_BOUGHT_QTY"].ToString(),
                            PACK_SIZE = row["PACK_SIZE"].ToString(),
                            SAP_CODE = row["SAP_CODE"].ToString(),
                            PROD_STATUS = row["PROD_STATUS"].ToString(),
                            SBU_GROUP = row["SBU_GROUP"].ToString()
                        }).ToList();
            return itemList;
        }

        public ValidationMsg Save(ProductInformationModel model, string userid)
        {
            try
            {
                /////////
                string firstCharacter = string.Empty;
                if (model.MFC_NIC_NAME.Length == 3)
                    firstCharacter = model.MFC_NIC_NAME;
                else
                    firstCharacter = model.MFC_NIC_NAME.Substring(1, 3);
                string Qry = "SELECT PROD_ID FROM PRODUCT WHERE PROD_ID LIKE '" + firstCharacter + "%' AND LENGTH(PROD_ID) =10 ORDER BY PROD_ID DESC";
                DataTable dt = _dbHelper.GetDataTable(Qry);
                if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0]["PROD_ID"].ToString().Length == 10)
                    //{
                    string lastNum = dt.Rows[0]["PROD_ID"].ToString().Substring(3, dt.Rows[0]["PROD_ID"].ToString().Length - 3);
                    long lastNumber = Convert.ToInt64(lastNum) + 1;

                    if (lastNumber < 10)
                        code = firstCharacter + "000000" + lastNumber.ToString();
                    else if (lastNumber < 100)
                        code = firstCharacter + "00000" + lastNumber.ToString();
                    else if (lastNumber < 1000)
                        code = firstCharacter + "0000" + lastNumber.ToString();
                    else if (lastNumber < 10000)
                        code = firstCharacter + "000" + lastNumber.ToString();
                    else if (lastNumber < 100000)
                        code = firstCharacter + "00" + lastNumber.ToString();
                    else if (lastNumber < 1000000)
                        code = firstCharacter + "0" + lastNumber.ToString();
                    else if (lastNumber < 10000000)
                        code = firstCharacter + lastNumber.ToString();
                    //}
                    //else
                    //    code = firstCharacter + "0000001";
                }
                else
                    code = firstCharacter + "0000001";
                /////////
                if (model.PROD_STATUS == "A")
                    model.PROD_STATUS = "A";
                else
                    model.PROD_STATUS = "I";

                string saveQuery = "INSERT INTO PRODUCT (PROD_ID,PRODUCT_NAME,TC_L1_CODE,TC_L2_CODE,TC_L3_CODE,TC_L4_CODE,GENERIC_CODE,DOSAGE_FORM_CODE,DSG_STRENGTH_CODE,MANUFACTURER_CODE,UNIT_PRICE,AVG_PRESCRIBE_QTY,AVG_BOUGHT_QTY,PACK_SIZE,SAP_CODE,PROD_STATUS,SBU_GROUP,ENTERED_BY,ENTERED_DATE,ENTERED_TERMINAL) VALUES('" + code + "','" + model.PRODUCT_NAME + "','" + model.TC_L1_CODE + "','" + model.TC_L2_CODE + "','" + model.TC_L3_CODE + "','" + model.TC_L4_CODE + "','" + model.GENERIC_CODE + "','" + model.DOSAGE_FORM_CODE + "','" + model.DSG_STRENGTH_CODE + "','" + model.MANUFACTURER_CODE + "','" + model.UNIT_PRICE + "','" + model.AVG_PRESCRIBE_QTY + "','" + model.AVG_BOUGHT_QTY + "','" + model.PACK_SIZE + "','" + model.SAP_CODE + "','" + model.PROD_STATUS + "', '" + model.SBU_GROUP + "', '" + userid + "',(TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),'" + GetIPAddress.LocalIPAddress() + "') ";
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
                    _validationMsg.Msg = "Product Code Already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _validationMsg.Type = Enums.MessageType.Error;
                    _validationMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _validationMsg;
        }
        public string GetCode()
        {
            return code;
        }
        public ValidationMsg Update(ProductInformationModel model, string userid)
        {
            try
            {
                if (model.PROD_STATUS == "A")
                    model.PROD_STATUS = "A";
                else
                    model.PROD_STATUS = "I";


                string updateQuery = "UPDATE PRODUCT SET PROD_ID = '" + model.PROD_ID + "',PRODUCT_NAME = '" + model.PRODUCT_NAME + "',TC_L1_CODE = '" + model.TC_L1_CODE + "',TC_L2_CODE = '" + model.TC_L2_CODE + "',TC_L3_CODE = '" + model.TC_L3_CODE + "',TC_L4_CODE = '" + model.TC_L4_CODE + "',GENERIC_CODE = '" + model.GENERIC_CODE + "',DOSAGE_FORM_CODE = '" + model.DOSAGE_FORM_CODE + "'," +
                                    "DSG_STRENGTH_CODE = '" + model.DSG_STRENGTH_CODE + "',MANUFACTURER_CODE = '" + model.MANUFACTURER_CODE + "',UNIT_PRICE = '" + model.UNIT_PRICE + "',AVG_PRESCRIBE_QTY = '" + model.AVG_PRESCRIBE_QTY + "',AVG_BOUGHT_QTY = '" + model.AVG_BOUGHT_QTY + "',PACK_SIZE = '" + model.PACK_SIZE + "',SAP_CODE = '" + model.SAP_CODE + "',PROD_STATUS = '" + model.PROD_STATUS + "', SBU_GROUP = '" + model.SBU_GROUP + "', UPDATED_BY = '" + userid + "', UPDATED_DATE = (TO_DATE('" + model.CurrentDate + "','dd/MM/yyyy')),UPDATED_TERMINAL = '" + GetIPAddress.LocalIPAddress() + "'" +
                                    " WHERE PROD_ID = '" + model.PROD_ID + "'";
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

        public ValidationMsg Delete(string productId)
        {
            try
            {
                string deleteQuery = "DELETE FROM PRODUCT WHERE PROD_ID = '" + productId + "' ";
                if (_dbHelper.CmdExecute(deleteQuery) > 0)
                {
                    _validationMsg.Type = Enums.MessageType.Success;
                    _validationMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                _validationMsg.Type = Enums.MessageType.Error;
                _validationMsg.Msg = "Failed to Delete Data.";
            }
            return _validationMsg;
        }

        public List<TC_LEVEL1Model> GetTherapeuticLevel1List()
        {
            //string Qry = "SELECT TCLM.TC_L1_CODE,TCLM.TC_L1_L2_MST_SLNO,TL1.TC_L1_DESC FROM TC_L1_L2_MST TCLM INNER JOIN TC_LEVEL1 TL1 ON TCLM.TC_L1_CODE = TL1.TC_L1_CODE";
            string Qry = "Select * From TC_LEVEL1";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<TC_LEVEL1Model> itemList = new List<TC_LEVEL1Model>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new TC_LEVEL1Model
                        {
                            TC_L1_CODE = row["TC_L1_CODE"].ToString(),
                            TC_L1_DESC = row["TC_L1_DESC"].ToString()


                        }).ToList();
            return itemList;

        }

        public List<TC_LEVEL2Model> GetTherapeuticLevel2List()
        {
            //string Qry = "SELECT TCLD.TC_L1_L2_MST_SLNO,TCLD.TC_L2_CODE,TL2.TC_L2_DESC FROM  TC_L1_L2_DTL TCLD INNER JOIN TC_L1_L2_MST TCLM   ON TCLM.TC_L1_L2_MST_SLNO = TCLD.TC_L1_L2_MST_SLNO INNER JOIN TC_LEVEL2 TL2 ON TCLD.TC_L2_CODE = TL2.TC_L2_CODE";
            string Qry = "Select * From TC_LEVEL2";
            _dataTable = _dbHelper.GetDataTable(Qry);
            List<TC_LEVEL2Model> itemList = new List<TC_LEVEL2Model>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new TC_LEVEL2Model
                        {
                            TC_L2_CODE = row["TC_L2_CODE"].ToString(),
                            TC_L2_DESC = row["TC_L2_DESC"].ToString()


                        }).ToList();
            return itemList;
        }

        public List<TC_LEVEL3Model> GetTherapeuticLevel3List()
        {
            //string Qry = "SELECT TCLD.TC_L2_L3_MST_SLNO,TCLD.TC_L3_CODE,TL3.TC_L3_DESC FROM  TC_L2_L3_DTL TCLD INNER JOIN TC_L2_L3_MST TCLM   ON TCLM.TC_L2_L3_MST_SLNO = TCLD.TC_L2_L3_MST_SLNO INNER JOIN TC_LEVEL3 TL3 ON TCLD.TC_L3_CODE = TL3.TC_L3_CODE";

            string Qry = "Select * From TC_LEVEL3";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<TC_LEVEL3Model> itemList = new List<TC_LEVEL3Model>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new TC_LEVEL3Model
                        {
                            TC_L3_CODE = row["TC_L3_CODE"].ToString(),
                            TC_L3_DESC = row["TC_L3_DESC"].ToString()


                        }).ToList();
            return itemList;
        }

        public List<TC_LEVEL4Model> GetTherapeuticLevel4List()
        {
            //string Qry = "SELECT TCLD.TC_L3_L4_MST_SLNO,TCLD.TC_L4_CODE,TL4.TC_L4_DESC FROM  TC_L3_L4_DTL TCLD INNER JOIN TC_L3_L4_MST TCLM   ON TCLM.TC_L3_L4_MST_SLNO = TCLD.TC_L3_L4_MST_SLNO INNER JOIN TC_LEVEL4 TL4 ON TCLD.TC_L4_CODE = TL4.TC_L4_CODE";
            string Qry = "Select * From TC_LEVEL4";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<TC_LEVEL4Model> itemList = new List<TC_LEVEL4Model>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new TC_LEVEL4Model
                        {
                            TC_L4_CODE = row["TC_L4_CODE"].ToString(),
                            TC_L4_DESC = row["TC_L4_DESC"].ToString()


                        }).ToList();
            return itemList;
        }

        public List<GenericInformationModel> GetGenericInformationData()
        {
            string Qry = "SELECT GENERIC_CODE,GENERIC_NAME,GEN_ORD_SLNO FROM GENERIC ORDER BY GENERIC_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<GenericInformationModel> itemList = new List<GenericInformationModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new GenericInformationModel
                        {
                            GENERIC_CODE = row["GENERIC_CODE"].ToString(),
                            GENERIC_NAME = row["GENERIC_NAME"].ToString(),
                            GEN_ORD_SLNO = row["GEN_ORD_SLNO"].ToString()

                        }).ToList();
            return itemList;

        }

        public List<DosageFormModel> GetDosageFormInformationData()
        {
            string Qry = "SELECT DOSAGE_FORM_CODE,DOSAGE_FORM_NAME,DF_ODR_SLNO FROM DOSAGE_FORM ORDER BY DOSAGE_FORM_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DosageFormModel> itemList = new List<DosageFormModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DosageFormModel
                        {
                            DOSAGE_FORM_CODE = row["DOSAGE_FORM_CODE"].ToString(),
                            DOSAGE_FORM_NAME = row["DOSAGE_FORM_NAME"].ToString(),
                            DF_ODR_SLNO = row["DF_ODR_SLNO"].ToString()

                        }).ToList();
            return itemList;
        }

        public List<DosageStrengthModel> GetDosageStrengthInformation()
        {
            string Qry = "SELECT DSG_STRENGTH_CODE,DSG_STRENGTH_NAME,DF_STRENGTH_SLNO FROM DOSAGE_STRENGTH ORDER BY DSG_STRENGTH_CODE ";

            _dataTable = _dbHelper.GetDataTable(Qry);
            List<DosageStrengthModel> itemList = new List<DosageStrengthModel>();
            itemList = (from DataRow row in _dataTable.Rows
                        select new DosageStrengthModel
                        {
                            DSG_STRENGTH_CODE = row["DSG_STRENGTH_CODE"].ToString(),
                            DSG_STRENGTH_NAME = row["DSG_STRENGTH_NAME"].ToString(),
                            DF_STRENGTH_SLNO = row["DF_STRENGTH_SLNO"].ToString()
                        }).ToList();

            return itemList;

        }

        public List<ProductInformationModel> GetAllManufacturerList()
        {
            string qry = "";
            qry = "SELECT MANUFACTURER_CODE ,MANUFACTURER_NAME,MFC_NIC_NAME,MFC_ORDER_SLNO FROM MANUFACTURER ";
            qry = qry + "ORDER BY MANUFACTURER_CODE";

            _dataTable = _dbHelper.GetDataTable(qry);
            List<ProductInformationModel> items;
            items = (from DataRow row in _dataTable.Rows
                     select new ProductInformationModel
                     {
                         MANUFACTURER_CODE = row["MANUFACTURER_CODE"].ToString(),
                         MFC_NIC_NAME = row["MFC_NIC_NAME"].ToString(),
                         MANUFACTURER_NAME = row["MANUFACTURER_NAME"].ToString()
                     }).ToList();

            return items;
        }

    }
}