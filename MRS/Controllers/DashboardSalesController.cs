using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;

namespace MRS.Controllers
{
    public class DashboardSalesController : Controller
    {
        private readonly DashboardSalesDAO _dashboardDao = new DashboardSalesDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        MenuItemsDAO ObjMenuItemsDAO = new MenuItemsDAO();
        DBConnection dbConnection = new DBConnection();
        DBHelper db = new DBHelper();
        //
        // GET: /DashboardSales/
        public ActionResult frmDashBoardMonthlyInvestment()
        { 
            if (Session["UserId"] != null)
            {
                int roleId = ObjMenuItemsDAO.GetRoleId(dbConnection.StringRead(), Session["mappingUserID"].ToString());
                Session["roleID"] = roleId;
                ViewBag.formTitle = "Sales Dashboard";
                Session["MiniName"] = "IDS";
                Session["Name"] = "Investment Database System";
                //ViewBag.name = "IDS";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMonthlyInvestmentComparison(DashBoardSalesModel model)
        {
            var data = _dashboardDao.GetMonthlyInvestmentComparison(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDesignationWiseSummery(DashBoardSalesModel model)
        {
            var data = _dashboardDao.GetDesignationWiseSummery(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTypeWiseExpense(DashBoardSalesModel model)
        {
            var data = _dashboardDao.GetTypeWiseExpense(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDoctorLeaderDashBoard(DashBoardSalesModel model)
        {
            var data = _dashboardDao.GetDoctorLeaderDashBoard(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetCommitmentInfoDashBoard(DashBoardSalesModel model)
        {
            var data = _dashboardDao.GetCommitmentInfoDashBoard(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMonthWiseMarketShare(DashBoardSalesModel model)
        {
            var data = _dashboardDao.GetMonthWiseMarketShare(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMonthWiseLeaderSts(DashBoardSalesModel model)
        {
            var data = _dashboardDao.GetMonthWiseLeaderSts(model);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}