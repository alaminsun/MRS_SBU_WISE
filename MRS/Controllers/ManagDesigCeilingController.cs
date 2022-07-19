using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class ManagDesigCeilingController : Controller
    {
        ManagDesigCeilingDAO Dalobject;
        DoctorHonorariumPaidDAO doctorHonorariumPaidDao;
        private ValidationMsg _vmMsg;
        public ManagDesigCeilingController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new ManagDesigCeilingDAO();
            doctorHonorariumPaidDao = new DoctorHonorariumPaidDAO();
        }
        public ActionResult frmManagDesigCeiling()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Designation Group Wise Monthly Ceiling Amount";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ManagDesigCeiling(ManagDesigCeilingModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedHonInfo(string HON_WISE_CEILING_CODE)
        {
            _vmMsg = Dalobject.Delete(HON_WISE_CEILING_CODE);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllManagementDesignationList()
        {
            var allData = Dalobject.GetAllManagementDesignationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetHonDesigCeilingSearchList()
        {
            var allData = Dalobject.GetHonDesigCeilingSearchList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSaveCeilingHonList(string GRP_DSIG_CODE)
        {
            var allData = Dalobject.GetSaveCeilingHonList(GRP_DSIG_CODE);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetHonarariumTypeList()
        {
            var tcL1L2List = doctorHonorariumPaidDao.GetHonarariumTypeList();
            var doctorLocationList = Json(tcL1L2List, JsonRequestBehavior.AllowGet);
            doctorLocationList.MaxJsonLength = int.MaxValue;
            return doctorLocationList;
        }
    }
}