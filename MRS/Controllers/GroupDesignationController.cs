using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class GroupDesignationController : Controller
    {
        GroupDesignationDAO Dalobject;
        private ValidationMsg _vmMsg;
        public GroupDesignationController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new GroupDesignationDAO();
        }
        public ActionResult frmGroupDesignation()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Designation Group Name";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GroupDesignation(GroupDesignationModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model) : Dalobject.Update(model);
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string GRP_DSIG_CODE)
        {
            _vmMsg = Dalobject.Delete(GRP_DSIG_CODE);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllGroupDesignationList()
        {
            var allData = Dalobject.GetAllGroupDesignationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestGroupDesignationList()
        {
            var allData = Dalobject.GetSuggestGroupDesignationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}