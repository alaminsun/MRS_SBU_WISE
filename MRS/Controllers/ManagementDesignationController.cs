using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class ManagementDesignationController : Controller
    {
        DBConnection dbConnection = new DBConnection();
        ManagementDesignationDAO Dalobject;
        private ValidationMsg _vmMsg;
        public ManagementDesignationController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new ManagementDesignationDAO();
        }
        public ActionResult frmManagementDesignation()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Personnel Designation Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ManagementDesignation(ManagementDesignationModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
            //return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string MGMT_CODE)
        {
            _vmMsg = Dalobject.Delete(MGMT_CODE);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllManagementDesignationList()
        {
            var allData = Dalobject.GetAllManagementDesignationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestManagementDesignationList()
        {
            var allData = Dalobject.GetSuggestManagementDesignationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetGroupDesignationList()
        {
            var allData = Dalobject.GetGroupDesignationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetRegionList()
        {
            var allData = Dalobject.GetRegionList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDivisionList()
        {
            var allData = Dalobject.GetDivisionList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetManagementInfo()
        {
            var data = Dalobject.GetManagementInfo();
            return Json(data, JsonRequestBehavior.AllowGet);
            //string Qry = "Select * from MANAGEMENT_TEAM";
            //OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(Qry, dbConnection.StringRead());
            //DataTable dt = new DataTable();
            //oracleDataAdapter.Fill(dt);
            //if (dt.Rows.Count > 0)
            //{
            //    Session["MGMT_CODE"] = dt.Rows[0]["MGMT_CODE"].ToString();
            //    Session["MGMT_NAME"] = dt.Rows[0]["MGMT_NAME"].ToString();
            //    Session["GRP_DSIG_CODE"] = dt.Rows[0]["GRP_DSIG_CODE"].ToString();
            //    Session["LOCATION_CODE"] = dt.Rows[0]["LOCATION_CODE"].ToString();
            //    //Session["ProjectName"] = "MARKET RESEARCH & PLANNING CELL";
            //}

            //return dt;
        }
    }
}