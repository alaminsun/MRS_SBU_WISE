using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class MarketDocPracLocController : Controller
    {
        private MarketDocPracLocDAO _marketDocPracLocDao;
        private ValidationMsg _vmMsg;

        public MarketDocPracLocController()
        {
            _vmMsg = new ValidationMsg();
            _marketDocPracLocDao = new MarketDocPracLocDAO();
        }
        public ActionResult FrmMarketDocPracLoc()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Market-practicing location relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMarketData()
        {
            var tcL1L2List = _marketDocPracLocDao.GetMarketList();
            return Json(tcL1L2List, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDoctorPracticeLocationData()
        {
            var tcL1L2List = _marketDocPracLocDao.GetDoctorPracticeLocationList();
            return Json(tcL1L2List, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdate(MarketDocpraclocMstModel model)
        {
            _vmMsg = model.SUStatus == 0 ? _marketDocPracLocDao.Save(model, Session["mappingUserID"].ToString()) : _marketDocPracLocDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSearchMasterList()
        {
            var tcL1L2SearchList = _marketDocPracLocDao.GetSearchMasterList();
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSearchDetailList(string marketDoctorPracLocMstSlNo)
        {
            var tcL1L2SearchList = _marketDocPracLocDao.GetSearchDtliList(marketDoctorPracLocMstSlNo);
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAllList(string marketDocPracLocMstSlNo)
        {
            _vmMsg = _marketDocPracLocDao.Delete(marketDocPracLocMstSlNo);
            return Json(new { msg = _vmMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDetailList(string marketDocPracLocDtlSlNo)
        {
            _vmMsg = _marketDocPracLocDao.DeleteDetailData(marketDocPracLocDtlSlNo);
            return Json(new { msg = _vmMsg });
        }

	}
}