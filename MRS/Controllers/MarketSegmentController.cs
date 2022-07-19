using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class MarketSegmentController : Controller
    {
        MarketSegmentDAO Dalobject;
        private ValidationMsg _vmMsg;

        public MarketSegmentController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new MarketSegmentDAO();
        }

        public ActionResult frmMarketSegment()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Market Segment";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MarketSegment(MarketSegmentModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string MarketSegmentCode)
        {
            _vmMsg = Dalobject.Delete(MarketSegmentCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllMarketSegmentList()
        {
            var allData = Dalobject.GetAllMarketSegmentList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestMarketSegmentList()
        {
            var allData = Dalobject.GetSuggestMarketSegmentList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}