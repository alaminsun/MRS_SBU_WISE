using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class RegionController : Controller
    {
        RegionDAO Dalobject;
        private ValidationMsg _vmMsg;
        public RegionController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new RegionDAO();
        }
        public ActionResult frmRegion()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Region Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Region(RegionModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string RegionCode)
        {
            _vmMsg = Dalobject.Delete(RegionCode);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllRegionList()
        {
            var allData = Dalobject.GetAllRegionList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestRegionList()
        {
            var allData = Dalobject.GetSuggestRegionList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}