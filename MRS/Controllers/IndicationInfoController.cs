using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class IndicationInfoController : Controller
    {
        IndicationInfoDAO Dalobject;
        private ValidationMsg _vmMsg;
        public IndicationInfoController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new IndicationInfoDAO();
        }
        public ActionResult frmIndicationInfo()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Indication Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult IndicationInfo(IndicationInfoModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model) : Dalobject.Update(model);
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string IndicationCode)
        {
            _vmMsg = Dalobject.Delete(IndicationCode);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllIndicationList()
        {
            var allData = Dalobject.GetAllIndicationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestIndicationList()
        {
            var allData = Dalobject.GetSuggestIndicationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}