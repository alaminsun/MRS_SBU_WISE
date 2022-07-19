using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MRS.Controllers
{
    public class TestController : Controller
    {
        [HttpGet]
        public ActionResult UserActivityStatus()
        {
            ViewBag.error = "TEst";
            return View();
        }

        [HttpGet]
        public ActionResult ReferanceDocumentsDetails()
        {
            ViewBag.error = "TEst";
            return View();
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.error = "TEst";
            return View();
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete()
        {
            //_vmMsg = Dalobject.Delete(RegionCode);
            return View("Index");
        }
        [HttpPost]
        public ActionResult frmIndex(MRS.Models.ReportModel model)
        {
            ViewBag.error = "TEst Error Message";

            return View("Index");
            ////////////////////////
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
        public class test
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }
}