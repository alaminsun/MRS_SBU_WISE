using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class MarketController : Controller
    {
        MarketDAO Dalobject;
        private ValidationMsg _vmMsg;
        public MarketController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new MarketDAO();
        }
        public ActionResult frmMarket()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Market Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Market(MarketModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string MarketCode)
        {
            _vmMsg = Dalobject.Delete(MarketCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllMarketList()
        {
            var allData = Dalobject.GetAllMarketList();
            var MarketList = Json(allData, JsonRequestBehavior.AllowGet);
            MarketList.MaxJsonLength = int.MaxValue;
            return MarketList;
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestMarketList()
        {
            var allData = Dalobject.GetSuggestMarketList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMarketType()
        {
            var listData = Dalobject.GetMarketType();
            return Json(listData, JsonRequestBehavior.AllowGet);
        }
    }
}