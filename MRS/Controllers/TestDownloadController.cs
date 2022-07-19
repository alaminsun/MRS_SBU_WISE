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
    public class TestDownloadController : Controller
    {
        //
        // GET: /TestDownload/
        public ActionResult frmTestDownload()
        {

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string RegionCode)
        {

            //Response.ContentType = "Application/pdf";
            //Response.AppendHeader("Content-Disposition", "attachment; filename=help.pdf");
            //Response.TransmitFile(Server.MapPath("~/Content/help.pdf"));
            //Response.End();

            List<test> df = new List<test>();
            test obj = new test();
            obj.id = "1";
            obj.name = "Test1";
            df.Add(obj);
            test obj1 = new test();
            obj1.id = "2";
            obj1.name = "Test2";
            df.Add(obj1);
            test obj2 = new test();
            obj2.id = "3";
            obj2.name = "Test3";
            df.Add(obj2);


            System.Data.DataTable dtExcel = ConvertToDataTable(df);

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

            return View();
            //_vmMsg = Dalobject.Delete(RegionCode);
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

        public class test
        {
            public string id { get; set; }
            public string name { get; set; }
        }
    }

}