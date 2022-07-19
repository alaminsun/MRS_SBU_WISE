using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class MarketTypeController : Controller
    {
        MarketTypeDAO Dalobject;
        private ValidationMsg _vmMsg;

        public MarketTypeController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new MarketTypeDAO();
        }

        public ActionResult frmMarketType()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Market Type";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MarketType(MarketTypeModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string MarketTypeCode)
        {
            _vmMsg = Dalobject.Delete(MarketTypeCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllMarketTypeList()
        {
            var allData = Dalobject.GetAllMarketTypeList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestMarketTypeList()
        {
            var allData = Dalobject.GetSuggestMarketTypeList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}