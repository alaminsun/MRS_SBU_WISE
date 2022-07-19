using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
using System.ComponentModel;
using MRS.DAL.Common;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Xml;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRS.Controllers
{
    public class UserWorkingSessionController : Controller
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg;
        public UserWorkingSessionController()
        {
            _vmMsg = new ValidationMsg();
        }
        public ActionResult frmUserWorkingSession()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "User Working Session";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UserWorkingSession()
        {
            try
            {
                //string tag = string.Empty;
                //System.Data.DataTable dtTag = dbHelper.GetDataTable("select NVL(MAX(TAG),0)+1 from USER_WORKING_SESSION");
                //if (dtTag.Rows.Count > 0)
                //    tag = dtTag.Rows[0][0].ToString();

                string tag = string.Empty;
                System.Data.DataTable dtTag = dbHelper.GetDataTable("select NVL(MAX(to_number(TAG)),0)+1 from USER_WORKING_SESSION");
                if (dtTag.Rows.Count > 0)
                    tag = dtTag.Rows[0][0].ToString();


                System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_USER_WORKING_SESSION");
                foreach (DataRow dr in dt.Rows)
                {
                    string qry = "INSERT INTO USER_WORKING_SESSION(SESSION_SLNO,ENTRY_DATE,PURCHASE_DATE,USER_CODE,DATA_SOURCE,TAG)" +
                    "VALUES('" + dr["SESSION_SLNO"].ToString() + "',(TO_DATE('" + Convert.ToDateTime(dr["ENTRY_DATE"]).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),(TO_DATE('" + Convert.ToDateTime(dr["PURCHASE_DATE"]).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),'" + dr["USER_CODE"].ToString() + "','" + dr["DATA_SOURCE"].ToString() + "','" + tag + "')";
                    dbHelper.CmdExecute(qry);
                }
                _vmMsg.Tag = tag;
                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Data Successfully Loaded to User Workinng Session.";
            }
            catch
            {
                _vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Fail to Save User Workinng Session.";
            }
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult frmUserWorkingSession(UserWorkingSessionModel model)
        {
            // Get Temp User Session Table Data
            System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_USER_WORKING_SESSION");
            List<UserWorkingSessionModel> UserWorkinngSessionList;
            UserWorkinngSessionList = (from DataRow row in dt.Rows
                                       select new UserWorkingSessionModel
                                       {
                                           SessionSlno = row["SESSION_SLNO"].ToString(),
                                           EntryDate = Convert.ToDateTime(row["ENTRY_DATE"].ToString()),
                                           PurchaseDate = Convert.ToDateTime(row["PURCHASE_DATE"].ToString()),
                                           //EntryDate = Convert.ToDateTime(row["ENTRY_DATE"].ToString()),
                                           //PurchaseDate = row["PURCHASE_DATE"].ToString(),
                                           DataSource = row["DATA_SOURCE"].ToString()
                                       }).ToList();

            // Get Survey Company Data
            System.Data.DataTable dtSurveyCom = dbHelper.GetDataTable("SELECT * FROM SURVEY_COMP");
            List<UserWorkingSessionModel> SurveyComList;
            SurveyComList = (from DataRow row in dtSurveyCom.Rows
                             select new UserWorkingSessionModel
                             {
                                 DataSource = row["SURVEY_COMP_CODE"].ToString()
                             }).ToList();

            // Get User Session Data
            System.Data.DataTable exitDt = dbHelper.GetDataTable("SELECT * from USER_WORKING_SESSION");
            List<UserWorkingSessionModel> ExitUserWorkinngSessionList = new List<UserWorkingSessionModel>();
            ExitUserWorkinngSessionList = (from DataRow row in exitDt.Rows
                                           select new UserWorkingSessionModel
                                           {
                                               SessionSlno = row["SESSION_SLNO"].ToString(),
                                               EntryDate = Convert.ToDateTime(row["ENTRY_DATE"].ToString()),
                                               PurchaseDate = Convert.ToDateTime(row["PURCHASE_DATE"].ToString()),
                                               DataSource = row["DATA_SOURCE"].ToString()
                                           }).ToList();

            List<UserWorkingSessionModel> ExcelUserWorkinngSessionList = new List<UserWorkingSessionModel>();

            //Check No 3 Duplicate Session Serial No
            if (UserWorkinngSessionList.GroupBy(n => n.SessionSlno).Any(c => c.Count() > 1))
            {
                var list = UserWorkinngSessionList.GroupBy(m => m.SessionSlno).ToList();
                foreach (var obj in list)
                {
                    if (obj.Count() > 1)
                    {
                        foreach (var objDuplicate in obj)
                        {
                            objDuplicate.Remarks = "SSND";//Session Serial Number Duplicate
                            ExcelUserWorkinngSessionList.Add(objDuplicate);
                        }
                    }
                }
            }

            // Check No 4 Existing in SessionSlno
            var notInUserWorkinngSessionList = ExitUserWorkinngSessionList.Select(a => a.SessionSlno).Intersect(UserWorkinngSessionList.Select(b => b.SessionSlno));
            foreach (var objExit in notInUserWorkinngSessionList)
            {
                var avd = UserWorkinngSessionList.Where(m => m.SessionSlno == objExit).FirstOrDefault();
                if (string.IsNullOrEmpty(avd.Remarks))
                {
                    avd.Remarks = "ESN-UWS";// Exist Sesson Number in User Working Session
                    ExcelUserWorkinngSessionList.Add(avd);
                }
                else
                    avd.Remarks = avd.Remarks + "," + "ESN-UWS";// Exist Sesson Number in User Working Session
            }
            // Check No 5,6 NULL/EMPTY 
            var UserWorkinngSessionListNuLL = UserWorkinngSessionList.Where(m => string.IsNullOrEmpty(m.SessionSlno));
            foreach (var objNuLL in UserWorkinngSessionListNuLL)
            {
                if (string.IsNullOrEmpty(objNuLL.Remarks))
                {
                    objNuLL.Remarks = "SSNE";//Session Serial Number Empty
                    ExcelUserWorkinngSessionList.Add(objNuLL);
                }
                else
                    objNuLL.Remarks = objNuLL.Remarks + "," + "SSNE";//Session Serial Number Empty
            }

            // Check No 5,6 NULL/EMPTY 
            var EntryDateNuLL = UserWorkinngSessionList.Where(m => string.IsNullOrEmpty(m.EntryDate.ToString()));
            foreach (var objNuLL in EntryDateNuLL)
            {
                if (string.IsNullOrEmpty(objNuLL.Remarks))
                {
                    objNuLL.Remarks = "EDE";//Entry Date Empty
                    ExcelUserWorkinngSessionList.Add(objNuLL);
                }
                else
                    objNuLL.Remarks = objNuLL.Remarks + "," + "EDE";//Entry Date Empty
            }

            // Check No 5,6 NULL/EMPTY 
            var DataSourceNuLL = UserWorkinngSessionList.Where(m => string.IsNullOrEmpty(m.DataSource));
            foreach (var objNuLL in DataSourceNuLL)
            {
                if (string.IsNullOrEmpty(objNuLL.Remarks))
                {
                    objNuLL.Remarks = "DSE";//Data Source Empty
                    ExcelUserWorkinngSessionList.Add(objNuLL);
                }
                else
                    objNuLL.Remarks = objNuLL.Remarks + "," + "DSE";//Data Source Empty
            }

            // Check No 5,6 NULL/EMPTY 
            var PurchaseDateListNuLL = UserWorkinngSessionList.Where(m => string.IsNullOrEmpty(m.PurchaseDate.ToString()));
            foreach (var objNuLL in PurchaseDateListNuLL)
            {
                if (string.IsNullOrEmpty(objNuLL.Remarks))
                {
                    objNuLL.Remarks = "PDE";//Purchase Date Empty
                    ExcelUserWorkinngSessionList.Add(objNuLL);
                }
                else
                    objNuLL.Remarks = objNuLL.Remarks + "," + "PDE";//Purchase Date Empty
            }

            // Check No 7 Over Date Range
            if (UserWorkinngSessionList.Where(m => string.IsNullOrEmpty(m.EntryDate.ToString())).ToList().Count == 0)
            {
                //string fromDate = Convert.ToDateTime(model.EntryDate).ToString("dd/MM/yyyy");
                //string toDate = Convert.ToDateTime(model.PurchaseDate).ToString("dd/MM/yyyy");
                //DateTime fromDate = DateTime.ParseExact(model.EntryDate, "dd/MM/yyyy", null);
                //DateTime toDate = DateTime.ParseExact(model.PurchaseDate, "dd/MM/yyyy", null);
                var UserWorkinngSessionListODR = UserWorkinngSessionList.Where(m => Convert.ToDateTime(m.EntryDate).Date <= model.PurchaseDate.Date).Where(m => Convert.ToDateTime(m.EntryDate).Date >= model.EntryDate.Date);
                var UserWorkinngSessionListODR1 = UserWorkinngSessionList.Union(UserWorkinngSessionListODR).Except(UserWorkinngSessionList.Intersect(UserWorkinngSessionListODR));
                foreach (var objODR in UserWorkinngSessionListODR1)
                {
                    if (string.IsNullOrEmpty(objODR.Remarks))
                    {
                        objODR.Remarks = "ODR-ED";//Over Date Range
                        ExcelUserWorkinngSessionList.Add(objODR);
                    }
                    else
                        objODR.Remarks = objODR.Remarks + "," + "ODR-ED";//Over Date Range
                }
            }

            // Check No 7 Over Date Range
            if (UserWorkinngSessionList.Where(m => string.IsNullOrEmpty(m.PurchaseDate.ToString())).ToList().Count == 0)
            {
                //string fromDate = Convert.ToDateTime(model.EntryDate).ToString("dd/MM/yyyy");
                //string toDate = Convert.ToDateTime(model.PurchaseDate).ToString("dd/MM/yyyy");
                //var UserWorkinngSessionListODR = UserWorkinngSessionList.Where(m => Convert.ToDateTime(m.PurchaseDate).Date <= Convert.ToDateTime(toDate).Date).Where(m => Convert.ToDateTime(m.PurchaseDate).Date >= Convert.ToDateTime(fromDate).Date);
                var UserWorkinngSessionListODR = UserWorkinngSessionList.Where(m => Convert.ToDateTime(m.PurchaseDate).Date <= model.PurchaseDate.Date).Where(m => Convert.ToDateTime(m.EntryDate).Date >= model.EntryDate.Date);
                var UserWorkinngSessionListODR1 = UserWorkinngSessionList.Union(UserWorkinngSessionListODR).Except(UserWorkinngSessionList.Intersect(UserWorkinngSessionListODR));
                foreach (var objODR in UserWorkinngSessionListODR1)
                {
                    if (string.IsNullOrEmpty(objODR.Remarks))
                    {
                        objODR.Remarks = "ODR-PD";//Over Date Range
                        ExcelUserWorkinngSessionList.Add(objODR);
                    }
                    else
                        objODR.Remarks = objODR.Remarks + "," + "ODR-PD";//Over Date Range
                }
            }

            // Check No 8 Existing in SessionSlno
            if (UserWorkinngSessionList.Where(m => string.IsNullOrEmpty(m.DataSource)).ToList().Count == 0)
            {
                var notInDataSourceList = UserWorkinngSessionList.Where(p => !SurveyComList.Any(p2 => p2.DataSource == p.DataSource));
                foreach (var objExit in notInDataSourceList)
                {
                    if (string.IsNullOrEmpty(objExit.Remarks))
                    {
                        objExit.Remarks = "IDS";// Invalid Data Source
                        ExcelUserWorkinngSessionList.Add(objExit);
                    }
                    else
                        objExit.Remarks = objExit.Remarks + "," + "IDS";// Invalid Data Source
                }
            }

            // Create Excel File
            //if ((UserWorkinngSessionList.GroupBy(n => n.SessionSlno).Any(c => c.Count() > 0)) || (exitDuplicateUserWorkinngSessionList.Count() > 0) || (UserWorkinngSessionListODR1.Count() > 0) || (UserWorkinngSessionListNuLL.Count() > 0))
            if (ExcelUserWorkinngSessionList.Count > 0)//.GroupBy(n => n.SessionSlno).Any(c => c.Count() > 0)) || (exitDuplicateUserWorkinngSessionList.Count() > 0) || (UserWorkinngSessionListODR1.Count() > 0) || (UserWorkinngSessionListNuLL.Count() > 0))
            {
                //string fileName = "D:\\UserWorkinngSession-" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xls";

                //Excel.ExcelUtlity objExcel = new Excel.ExcelUtlity();
                System.Data.DataTable dtExcel = ConvertToDataTable(ExcelUserWorkinngSessionList);
                //objExcel.WriteDataTableToExcel(dtExcel, "Session", fileName, "UserWorkinngSession");

                //_vmMsg.Type = Enums.MessageType.Error;
                //_vmMsg.Msg = "Sorry,Some Error Found, Please See the Excel File " + fileName;

                Response.Clear();

                Response.Buffer = true;

                Response.AddHeader("content-disposition", "attachment;filename=UserWorkinngSession.xls");

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
            }
            else
            {
                //string tag = string.Empty;
                //System.Data.DataTable dtTag = dbHelper.GetDataTable("select NVL(MAX(TAG),0)+1 from USER_WORKING_SESSION");
                //if (dtTag.Rows.Count > 0)
                //    tag = dtTag.Rows[0][0].ToString();

                string tag = string.Empty;
                System.Data.DataTable dtTag = dbHelper.GetDataTable("select NVL(MAX(to_number(TAG)),0)+1 from USER_WORKING_SESSION");
                if (dtTag.Rows.Count > 0)
                    tag = dtTag.Rows[0][0].ToString();


                //System.Data.DataTable dt = dbHelper.GetDataTable("SELECT * FROM TEMP_USER_WORKING_SESSION");
                foreach (DataRow dr in dt.Rows)
                {
                    string qry = "INSERT INTO USER_WORKING_SESSION(SESSION_SLNO,ENTRY_DATE,PURCHASE_DATE,USER_CODE,DATA_SOURCE,TAG)" +
                    "VALUES('" + dr["SESSION_SLNO"].ToString() + "',(TO_DATE('" + Convert.ToDateTime(dr["ENTRY_DATE"]).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),(TO_DATE('" + Convert.ToDateTime(dr["PURCHASE_DATE"]).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')),'" + dr["USER_CODE"].ToString() + "','" + dr["DATA_SOURCE"].ToString() + "','" + tag + "')";
                    dbHelper.CmdExecute(qry);
                }
                ViewBag.Tag = tag;
                ViewBag.Msg = "Data Successfully Loaded to User Workinng Session.";
                //_vmMsg.Tag = tag;
                //_vmMsg.Type = Enums.MessageType.Success;
                //_vmMsg.Msg = "Data Successfully Loaded to User Workinng Session.";
            }
            return View();
            //return Json(new { msg = _vmMsg });
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

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Delete(string UserWorkingSessionCode)
        //{
        //    //_vmMsg = Dalobject.Delete(UserWorkingSessionCode);
        //    return Json(new { msg = _vmMsg });
        //}

        public ActionResult LoadExcelFile(string fileName)
        {
            try
            {
                //dbHelper.CmdExecute("DELETE FROM TEMP_USER_WORKING_SESSION");

                string slNo = string.Empty, entryDate = string.Empty, purchaseDate = string.Empty, datasource = string.Empty;
                if (string.IsNullOrEmpty(TempData["file"].ToString()))
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }

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

                string query = String.Format("SELECT * from [{0}$]", "Session");
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, connectionString);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                System.Data.DataTable myDataTable = dataSet.Tables[0];

                //Check No 1
                if (dataSet.Tables[0].Columns.Count == 4)
                {
                    Int64 i = 0;
                    dbHelper.CmdExecute("DELETE FROM TEMP_USER_WORKING_SESSION");
                    //Check No 2
                    foreach (DataRow dr in myDataTable.Rows)
                    {
                        //if ((!string.IsNullOrEmpty(dr["Session Slno"].ToString())) && (!string.IsNullOrEmpty(dr["Entry Date"].ToString())) && (!string.IsNullOrEmpty(dr["Purchase Date"].ToString())) && (!string.IsNullOrEmpty(dr["Data Source"].ToString())))
                        //{
                        if (string.IsNullOrEmpty(dr["Session Slno"].ToString()))
                            slNo = string.Empty;
                        else
                            slNo = dr["Session Slno"].ToString();

                        if (string.IsNullOrEmpty(dr["Entry Date"].ToString()))
                            entryDate = string.Empty;
                        else
                            //string dfd = dr["Entry Date"].ToString();
                            entryDate = Convert.ToDateTime(dr["Entry Date"]).ToString("dd/MM/yyyy");
                            //entryDate = DateTime.ParseExact(dr["Entry Date"].ToString(), "MM/dd/yyyy", null).ToString("dd/MM/yyyy");
                        //string fdf = DateTime.ParseExact(dr["Entry Date"].ToString(), "MM/dd/yyyy", null).ToString("dd/MM/yyyy");

                        if (string.IsNullOrEmpty(dr["Purchase Date"].ToString()))
                            purchaseDate = string.Empty;
                        else
                            purchaseDate = Convert.ToDateTime(dr["Purchase Date"]).ToString("dd/MM/yyyy");
                            //purchaseDate = DateTime.ParseExact(dr["Purchase Date"].ToString(), "MM/dd/yyyy", null).ToString("dd/MM/yyyy");

                        if (string.IsNullOrEmpty(dr["Data Source"].ToString()))
                            datasource = string.Empty;
                        else
                            datasource = dr["Data Source"].ToString();

                        string qry = "INSERT INTO TEMP_USER_WORKING_SESSION (SESSION_SLNO,ENTRY_DATE,PURCHASE_DATE,USER_CODE,DATA_SOURCE)" +
                        "VALUES('" + slNo + "',(TO_DATE('" + entryDate + "','dd/MM/yyyy')),(TO_DATE('" + purchaseDate + "','dd/MM/yyyy')),'" + Session["mappingUserID"] + "','" + datasource + "')";

                        dbHelper.CmdExecute(qry);
                        i++;
                        //}
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
                _vmMsg.Msg = ex.Message;//  "Fail to Save Excel File Data";
                if (ex.Message.Contains("object"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "";
                }
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