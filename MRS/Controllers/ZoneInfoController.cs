using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class ZoneInfoController : Controller
    {
        ZoneInfoDAO Dalobject;
        private ValidationMsg _vmMsg;
        public ZoneInfoController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new ZoneInfoDAO();
        }
        public ActionResult frmZoneInfo()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Zone Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ZoneInfo(ZoneInfoModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string ZoneCode)
        {
            _vmMsg = Dalobject.Delete(ZoneCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllZoneList()
        {
            var allData = Dalobject.GetAllZoneList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestZoneList()
        {
            var allData = Dalobject.GetSuggestZoneList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}