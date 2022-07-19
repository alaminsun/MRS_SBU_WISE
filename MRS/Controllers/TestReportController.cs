using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class TestReportController : Controller
    {
        ReportDAO Dalobject = new ReportDAO();
        DBHelper dbHelper = new DBHelper();

        public ActionResult Index()
        {
            return View();
        }
    }
}