using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRS.Controllers
{
    public class PrescriptionDetailDataController : Controller
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        public PrescriptionDetailDataController()
        {
            _vmMsg = new ValidationMsg();
        }
        public ActionResult frmPrescriptionDetailData()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Prescription Detail Data";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult PrescriptionDetailSaveData()
        {
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
            //{
            System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_PRESCRIPTION_DETAIL");
            foreach (DataRow dr in dt.Rows)
            {
                System.Data.DataTable dtunitPrice = dbHelper.GetDataTable("select UNIT_PRICE from product where PROD_ID ='" + dr["PROD_ID"].ToString() + "'");
                var unitPrice = dtunitPrice.Rows[0]["UNIT_PRICE"].ToString();
                string qry = "INSERT INTO PRESCRIPTION_DETAIL(PRESC_DET_SLNO,PRESC_MAS_SLNO,PROD_ID,PURCHASE_QTY,UNIT_PRICE,INDI_CODE)" +
                "VALUES('" + dr["PRESC_DET_SLNO"].ToString() + "', '" + dr["PRESC_MAS_SLNO"].ToString() + "', '" + dr["PROD_ID"].ToString() + "','" + dr["PURCHASE_QTY"].ToString() + "','" + unitPrice + "','" + dr["INDI_CODE"].ToString() + "')";
                try
                {
                    dbHelper.CmdExecute(qry);
                }
                catch (Exception ex)
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Error in Tempdata table Data." + dr["PRESC_DET_SLNO"].ToString() + dr["PRESC_MAS_SLNO"].ToString() + dr["PROD_ID"].ToString() + dr["PURCHASE_QTY"].ToString() + dr["PRESC_MAS_SLNO"].ToString();
                    ////if (ex.Message.Contains("MRS.FK_PD_PRESC_MAS_SLNO"))
                    ////{
                    //string a = dr["PRESC_DET_SLNO"].ToString();
                    //string a1 = dr["PRESC_MAS_SLNO"].ToString();
                    //string a2 = dr["PROD_ID"].ToString();
                    //string a3 = dr["PURCHASE_QTY"].ToString();
                    //string a4 = dr["PRESC_MAS_SLNO"].ToString();
                    ////}
                }
            }
            //    scope.Complete();
            //}
            //}
            //ViewBag.Msg = "Data Successfully Loaded to Prescription Detail.";

            _vmMsg.Type = Enums.MessageType.Success;//dataSet.Tables[0].Rows.Count.ToString()--Count Number of Rows
            _vmMsg.Msg = " Data Successfully Loaded to Prescription Detail.";

            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult frmPrescriptionDetailData(PrescriptionDetailDataModel model)
        //public ActionResult CheckPrescriptionDetailData()
        {
            // Get Temp Prescription Detail Table Data
            System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_PRESCRIPTION_DETAIL");
            //System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM PRESCRIPTION_DETAIL");
            List<PrescriptionDetailDataModel> PrescriptionDetailDataList;
            PrescriptionDetailDataList = (from DataRow row in dt.Rows
                                          select new PrescriptionDetailDataModel
                                          {
                                              PrescDetSlno = row["PRESC_DET_SLNO"].ToString(),
                                              PrescMasSlno = row["PRESC_MAS_SLNO"].ToString(),
                                              ProdId = row["PROD_ID"].ToString(),
                                              PurchaseQty = row["PURCHASE_QTY"].ToString(),
                                              IndiCode = row["INDI_CODE"].ToString(),
                                              UnitPrice = row["UNIT_PRICE"].ToString()
                                          }).ToList();

            // Get Temp Prescription Master Table Data
            System.Data.DataTable dtPrescMst = dbHelper.GetDataTable("SELECT * FROM TEMP_PRESCRIPTION_MASTER");
            //System.Data.DataTable dtPrescMst = dbHelper.GetDataTable("SELECT * FROM PRESCRIPTION_MASTER");
            List<PrescriptionDetailDataModel> PrescriptionMasterDataList;
            PrescriptionMasterDataList = (from DataRow row in dtPrescMst.Rows
                                          select new PrescriptionDetailDataModel
                                          {
                                              PrescMasSlno = row["PRESC_MAS_SLNO"].ToString()
                                          }).ToList();
            // Get Indication Table Data
            System.Data.DataTable dtIndication = dbHelper.GetDataTable("SELECT * FROM INDICATION");
            List<PrescriptionDetailDataModel> IndicationList;
            IndicationList = (from DataRow row in dtIndication.Rows
                              select new PrescriptionDetailDataModel
                              {
                                  IndiCode = row["INDI_CODE"].ToString()
                              }).ToList();

            // Get Prod Id Data
            System.Data.DataTable dtProdId = dbHelper.GetDataTable("SELECT PROD_ID FROM PRODUCT");
            List<PrescriptionDetailDataModel> ProdIdList;
            ProdIdList = (from DataRow row in dtProdId.Rows
                          select new PrescriptionDetailDataModel
                          {
                              ProdId = row["PROD_ID"].ToString(),
                          }).ToList();

            List<PrescriptionDetailDataModel> ExcelPrescriptionDetailDataList = new List<PrescriptionDetailDataModel>();

            // Check No 3 Existing in PrescMasSlno
            var notInPrescMasSlnoList = PrescriptionDetailDataList.Where(p => !PrescriptionMasterDataList.Any(p2 => p2.PrescMasSlno == p.PrescMasSlno));
            foreach (var objExit in notInPrescMasSlnoList)
            {
                if (string.IsNullOrEmpty(objExit.Remarks))
                {
                    objExit.Remarks = "PMSNE-PM";// PrescMasSlno Not Exist in Prescription Master
                    ExcelPrescriptionDetailDataList.Add(objExit);
                }
                else
                    objExit.Remarks = objExit.Remarks + "," + "PMSNE-PM";// PrescMasSlno Not Exist in Prescription Master
            }

            // Check No ## Existing in IndiCode
            var PrescriptionDetailDataListIndiCodeNotNuLL = PrescriptionDetailDataList.Where(m => !string.IsNullOrEmpty(m.IndiCode));
            //var notInIndiCodeList = PrescriptionDetailDataList.Where(p => !IndicationList.Any(p2 => p2.IndiCode == p.IndiCode));
            var notInIndiCodeList = PrescriptionDetailDataListIndiCodeNotNuLL.Where(p => !IndicationList.Any(p2 => p2.IndiCode == p.IndiCode));
            foreach (var objExit in notInIndiCodeList)
            {
                if (string.IsNullOrEmpty(objExit.Remarks))
                {
                    objExit.Remarks = "IIC";//Invalid Indication Code Not Exist in I
                    ExcelPrescriptionDetailDataList.Add(objExit);
                }
                else
                    objExit.Remarks = objExit.Remarks + "," + "IIC";//Invalid Indication Code Not Exist in I
            }

            //Check No 4 Duplicate Session Serial No
            if (PrescriptionDetailDataList.GroupBy(n => n.PrescDetSlno).Any(c => c.Count() > 1))
            {
                var list = PrescriptionDetailDataList.GroupBy(m => m.PrescDetSlno).ToList();
                foreach (var obj in list)
                {
                    if (obj.Count() > 1)
                    {
                        foreach (var objDuplicate in obj)
                        {
                            if (string.IsNullOrEmpty(objDuplicate.Remarks))
                            {
                                objDuplicate.Remarks = "PDSD";//Prescription Detail Serial No Duplicate Found
                                ExcelPrescriptionDetailDataList.Add(objDuplicate);
                            }
                            else
                                objDuplicate.Remarks = objDuplicate.Remarks + "," + "PDSD";//Prescription Detail Serial No Duplicate Found
                        }
                    }
                }
            }

            // Check No 5 Existing in ProdId
            var notInProdIdList = PrescriptionDetailDataList.Where(p => !ProdIdList.Any(p2 => p2.ProdId == p.ProdId));
            foreach (var objExit in notInProdIdList)
            {
                if (string.IsNullOrEmpty(objExit.Remarks))
                {
                    objExit.Remarks = "IP";// Invalid Product(Product Not Found in Product Table)
                    ExcelPrescriptionDetailDataList.Add(objExit);
                }
                else
                    objExit.Remarks = objExit.Remarks + "," + "IP";// Invalid Product(Product Not Found in Product Table)
            }

            //Check No 7 Get Unit Price Data
            //System.Data.DataTable DtUnitPrice = dbHelper.GetDataTable("Select PD.PRESC_DET_SLNO,PD.PRESC_MAS_SLNO,PD.PURCHASE_QTY,P.PROD_ID,P.UNIT_PRICE from TEMP_PRESCRIPTION_DETAIL PD inner join PRODUCT P on P.PROD_ID = PD.PROD_ID where P.UNIT_PRICE<=0");
            System.Data.DataTable DtUnitPrice = dbHelper.GetDataTable("Select PD.PRESC_DET_SLNO,PD.PRESC_MAS_SLNO,PD.PURCHASE_QTY,P.PROD_ID,P.UNIT_PRICE from PRESCRIPTION_DETAIL PD inner join PRODUCT P on P.PROD_ID = PD.PROD_ID where P.UNIT_PRICE<=0");
            List<PrescriptionDetailDataModel> UnitPriceList = new List<PrescriptionDetailDataModel>();
            UnitPriceList = (from DataRow row in DtUnitPrice.Rows
                             select new PrescriptionDetailDataModel
                             {
                                 PrescDetSlno = row["PRESC_DET_SLNO"].ToString(),
                                 PrescMasSlno = row["PRESC_MAS_SLNO"].ToString(),
                                 ProdId = row["PROD_ID"].ToString(),
                                 PurchaseQty = row["PURCHASE_QTY"].ToString(),
                                 IndiCode = row["INDI_CODE"].ToString(),
                                 UnitPrice = row["UNIT_PRICE"].ToString()
                             }).ToList();

            foreach (var objUnitPrice in UnitPriceList)
            {
                if (string.IsNullOrEmpty(objUnitPrice.Remarks))
                {
                    objUnitPrice.Remarks = "IPUP";//Invalid Product Unit Price
                    ExcelPrescriptionDetailDataList.Add(objUnitPrice);
                }
                else
                    objUnitPrice.Remarks = objUnitPrice.Remarks + "," + "IPUP";//Invalid Product Unit Price
            }

            // Check No 8 Product Qty Less Zero/Zero
            var ProductQtyZeroList = PrescriptionDetailDataList.Where(m => Convert.ToInt64(m.PurchaseQty) <= 0);
            foreach (var objProductQtyZero in ProductQtyZeroList)
            {
                if (string.IsNullOrEmpty(objProductQtyZero.Remarks))
                {
                    objProductQtyZero.Remarks = "IPQ";//Invalid Product Quentity
                    ExcelPrescriptionDetailDataList.Add(objProductQtyZero);
                }
                else
                    objProductQtyZero.Remarks = objProductQtyZero.Remarks + "," + "IPQ";//Prescription Detail Serial No Duplicate
            }

            // Check NULL/EMPTY 
            var PrescriptionDetailDataListNuLL = PrescriptionDetailDataList.Where(m => string.IsNullOrEmpty(m.PrescDetSlno) || string.IsNullOrEmpty(m.PrescMasSlno) || string.IsNullOrEmpty(m.ProdId) || string.IsNullOrEmpty(m.PurchaseQty));
            foreach (var objNuLL in PrescriptionDetailDataListNuLL)
            {
                if (string.IsNullOrEmpty(objNuLL.Remarks))
                {
                    objNuLL.Remarks = "NDO";//Null Data Occured
                    ExcelPrescriptionDetailDataList.Add(objNuLL);
                }
                else
                    objNuLL.Remarks = objNuLL.Remarks + "," + "NDO";//Null Data Occured
            }

            // Create Excel File
            //if ((PrescriptionDetailDataList.GroupBy(n => n.SessionSlno).Any(c => c.Count() > 0)) || (exitDuplicateUserWorkinngSessionList.Count() > 0) || (UserWorkinngSessionListNuLL.Count() > 0))
            if (ExcelPrescriptionDetailDataList.Count > 0)//.GroupBy(n => n.PrescriptionSlno).Any(c => c.Count() > 0)))
            {
                System.Data.DataTable dtExcel = ConvertToDataTable(ExcelPrescriptionDetailDataList);

                Response.Clear();

                Response.Buffer = true;

                Response.AddHeader("content-disposition", "attachment;filename=PrescriptionDetailData.xls");

                Response.Charset = "";

                Response.ContentType = "application/vnd.ms-excel";

                StringWriter sw = new StringWriter();

                HtmlTextWriter hw = new HtmlTextWriter(sw);

                GridView GridView1 = new GridView();

                GridView1.DataSource = dtExcel;
                GridView1.DataBind();

                GridView1.RenderControl(hw);

                Response.Output.Write(sw.ToString());

                Response.Flush();

                Response.End();

                //string fileName = "D:\\PrescriptionDetailData-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls";
                //Excel.ExcelUtlity objExcel = new Excel.ExcelUtlity();
                //System.Data.DataTable dtExcel = ConvertToDataTable(ExcelPrescriptionDetailDataList);
                //objExcel.WriteDataTableToExcel(dtExcel, "Pres_mast", fileName, "UserWorkinngSession");

                //_vmMsg.Type = Enums.MessageType.Error;
                //_vmMsg.Msg = "Sorry,Some Error Found, Please See the Excel File " + fileName;
            }
            else
            {
                //DataTable dtPresDeltSlNo = dbHelper.GetDataTable("SELECT PRESC_DET_SLNO FROM PRESCRIPTION_DETAIL ORDER BY PRESC_DET_SLNO DESC");

                //if (dtPresDeltSlNo.Rows.Count > 0)
                //{
                //    if (DateTime.Now.ToString("yyyyMMdd") == dtPresDeltSlNo.Rows[0]["PRESC_DET_SLNO"].ToString().Substring(0, 8))
                //    {
                //        long presDeltSlNo = Convert.ToInt64(dtPresDeltSlNo.Rows[0]["PRESC_DET_SLNO"].ToString().Substring(8, dtPresDeltSlNo.Rows[0]["PRESC_DET_SLNO"].ToString().Length - 8)) + 1;
                //        foreach (DataRow dr in dt.Rows)
                //        {
                //            string strPresDeltSlNo = DateTime.Now.ToString("yyyyMMdd") + presDeltSlNo.ToString();

                //            System.Data.DataTable dtunitPrice = dbHelper.GetDataTable("select UNIT_PRICE from product where PROD_ID ='" + dr["PROD_ID"].ToString() + "'");
                //            var unitPrice = dtunitPrice.Rows[0]["UNIT_PRICE"].ToString();
                //            string qry = "INSERT INTO PRESCRIPTION_DETAIL(PRESC_DET_SLNO,PRESC_MAS_SLNO,PROD_ID,PURCHASE_QTY,UNIT_PRICE,INDI_CODE)" +
                //            "VALUES('" + strPresDeltSlNo + "', '" + dr["PRESC_MAS_SLNO"].ToString() + "', '" + dr["PROD_ID"].ToString() + "','" + dr["PURCHASE_QTY"].ToString() + "','" + unitPrice + "','" + dr["INDI_CODE"].ToString() + "')";
                //            dbHelper.CmdExecute(qry);
                //            presDeltSlNo++;
                //        }
                //    }
                //}
                //else
                //{
                //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        System.Data.DataTable dtunitPrice = dbHelper.GetDataTable("select UNIT_PRICE from product where PROD_ID ='" + dr["PROD_ID"].ToString() + "'");
                //        var unitPrice = dtunitPrice.Rows[0]["UNIT_PRICE"].ToString();
                //        string qry = "INSERT INTO PRESCRIPTION_DETAIL(PRESC_DET_SLNO,PRESC_MAS_SLNO,PROD_ID,PURCHASE_QTY,UNIT_PRICE,INDI_CODE)" +
                //        "VALUES('" + dr["PRESC_DET_SLNO"].ToString() + "', '" + dr["PRESC_MAS_SLNO"].ToString() + "', '" + dr["PROD_ID"].ToString() + "','" + dr["PURCHASE_QTY"].ToString() + "','" + unitPrice + "','" + dr["INDI_CODE"].ToString() + "')";
                //        try
                //        {
                //            dbHelper.CmdExecute(qry);
                //        }
                //        catch (Exception ex)
                //        {
                //            if (ex.Message.Contains("MRS.FK_PD_PRESC_MAS_SLNO"))
                //            {
                //                ViewBag.Msg = dr["PRESC_MAS_SLNO"].ToString() + " is not Found";
                //                return View();
                //            }
                //        }
                //    }
                //    scope.Complete();
                //}
                ////}
                //ViewBag.Msg = "Data Successfully Loaded to Prescription Detail.";
                ViewBag.Msg = "No Error Then Click Final Load Button.";
            }
            return View();
        }

        public System.Data.DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            System.Data.DataTable table = new System.Data.DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string PrescriptionDetailDataCode)
        {
            //_vmMsg = Dalobject.Delete(PrescriptionDetailDataCode);
            return Json(new { msg = _vmMsg });
        }

        public ActionResult LoadExcelFile(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(TempData["file"].ToString()))
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

                var results = new List<PrescriptionDetailDataModel>();

                string connectionString = "";
                string filename = Path.Combine(Server.MapPath("~/Content"), TempData["file"].ToString());
                //string filename = @"C:\Test\" + TempData["file"].ToString();
                string[] d = filename.Split('.');
                string fileExtension = "." + d[d.Length - 1].ToString();
                if (d.Length > 0)
                {
                    if (fileExtension == ".xls")
                    {
                        connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {
                        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                    }
                }

                string query = String.Format("SELECT * from [{0}$]", "Pres_det");
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connectionString);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                System.Data.DataTable myDataTable = dataSet.Tables[0];

                //string presDetlSlNo = string.Empty;
                //string presDetlUpDate = DateTime.Now.ToString("yyyyMMdd");
                //long presDelNo = 1;
                //Check No 1
                //if (dataSet.Tables[0].Columns.Count == 4)
                if (dataSet.Tables[0].Columns.Count == 5)
                //if (dataSet.Tables[0].Columns.Count == 3)
                {
                    Int64 i = 0;
                    dbHelper.CmdExecute("DELETE FROM TEMP_PRESCRIPTION_DETAIL");
                    //Check No 2
                    foreach (DataRow dr in myDataTable.Rows)
                    {
                        //Check No 6
                        string unitPrice = string.Empty;
                        System.Data.DataTable dtunitPrice = dbHelper.GetDataTable("select UNIT_PRICE from product where PROD_ID ='" + dr["Prod Id"].ToString() + "'");
                        if (dtunitPrice.Rows.Count > 0)
                            unitPrice = dtunitPrice.Rows[0]["UNIT_PRICE"].ToString();

                        // Without indication code
                        //string qry = "INSERT INTO TEMP_PRESCRIPTION_DETAIL(PRESC_DET_SLNO,PRESC_MAS_SLNO,PROD_ID,PURCHASE_QTY,UNIT_PRICE)" +
                        //"VALUES('" + dr["Presc Det Slno"].ToString() + "', '" + dr["Presc Mas Slno"].ToString() + "', '" + dr["Prod Id"].ToString() + "','" + dr["Purchase Qty"].ToString() + "','" + unitPrice + "')";

                        // With indication code
                        //string qry = "INSERT INTO TEMP_PRESCRIPTION_DETAIL(PRESC_DET_SLNO,PRESC_MAS_SLNO,PROD_ID,PURCHASE_QTY,UNIT_PRICE,INDI_CODE)" +
                        //"VALUES('" + dr["Presc Det Slno"].ToString() + "', '" + dr["Presc Mas Slno"].ToString() + "', '" + dr["Prod Id"].ToString() + "','" + dr["Purchase Qty"].ToString() + "','" + unitPrice + "','" + dr["IndiCode"].ToString() + "')";

                        //presDetlSlNo = presDetlUpDate + presDelNo.ToString();

                        //// With indication code + presDetlSlNo Auto
                        //string qry = "INSERT INTO TEMP_PRESCRIPTION_DETAIL(PRESC_DET_SLNO,PRESC_MAS_SLNO,PROD_ID,PURCHASE_QTY,UNIT_PRICE,INDI_CODE)" +
                        //"VALUES('" + presDetlSlNo + "', '" + dr["Prescription Slno"].ToString() + "', '" + dr["Prod Id"].ToString() + "','" + dr["Purchase Qty"].ToString() + "','" + unitPrice + "','" + dr["IndiCode"].ToString() + "')";


                        // With indication code + presDetlSlNo Auto
                        string qry = "INSERT INTO TEMP_PRESCRIPTION_DETAIL(PRESC_DET_SLNO,PRESC_MAS_SLNO,PROD_ID,PURCHASE_QTY,UNIT_PRICE,INDI_CODE)" +
                        "VALUES('" + dr["Detail Slno"].ToString() + "', '" + dr["Prescription Slno"].ToString() + "', '" + dr["Prod Id"].ToString() + "','" + dr["Purchase Qty"].ToString() + "','" + unitPrice + "','" + dr["IndiCode"].ToString() + "')";

                        dbHelper.CmdExecute(qry);
                        i++;
                        //presDelNo++;
                    }
                    _vmMsg.Type = Enums.MessageType.Success;//dataSet.Tables[0].Rows.Count.ToString()--Count Number of Rows
                    _vmMsg.Msg = i.ToString() + " Data Successfully Exported from Excel to Temp Database File, Please Click the Check Button.";
                }
                else
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Excel File Columns are Mismatch";
                }
                Remove(TempData["file"].ToString());
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Fail to Save Excel File Data";
            }

            return Json(new { msg = _vmMsg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadFile(IEnumerable<HttpPostedFileBase> files)
        {
            ViewBag.FileName = "";
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content"), fileName);
                    file.SaveAs(physicalPath);
                    TempData["file"] = fileName;
                    break;
                }
            }
            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Remove(string fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (!string.IsNullOrEmpty(fileNames))
            {
                var fileName = Path.GetFileName(fileNames);
                var physicalPath = Path.Combine(Server.MapPath("~/Content"), fileName);
                // TODO: Verify user permissions
                if (System.IO.File.Exists(physicalPath))
                {
                    // The files are not actually removed in this demo
                    System.IO.File.Delete(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
    }
}
