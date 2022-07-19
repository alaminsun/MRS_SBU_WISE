using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class OTCInfoController : Controller
    {

        private readonly OTCInfoDAO otcInfoDao = new OTCInfoDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        //
        // GET: /OTCInfo/
        public ActionResult frmOTCInfo()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.Title = " OTC Information ";
                return View();
            }
            return RedirectToAction("Login", "Home");
            //return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OTCInfo(OTCInfoModel master)
        {
            _validationMsg = master.SUStatus == 0 ? otcInfoDao.Save(master, Session["mappingUserID"].ToString()) : otcInfoDao.Update(master, Session["mappingUserID"].ToString());
            return Json(new { msg = _validationMsg });
            //_validationMsg = slipDao.Save(master, Session["mappingUserID"].ToString()); 
            //return Json(new { msg = _validationMsg });

        }


        public ActionResult GetMarketInfo()
        {
            var MarketList = otcInfoDao.GetMarketInfoList();
            return Json(MarketList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductInfo()
        {
            var data = otcInfoDao.GetProductInfoList().OrderByDescending(m => m.PROD_ID);
            var instituteDetailList = Json(data, JsonRequestBehavior.AllowGet);
            instituteDetailList.MaxJsonLength = int.MaxValue;
            return instituteDetailList;

        }


        public JsonResult GetSessionSl()
        {
            var sessionList = otcInfoDao.GetSessionSL();
            return Json(sessionList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetSesstionMarketList(long sessionSl)
        {
            var listData = otcInfoDao.GetSessionMarketData(sessionSl);
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSessionProduct(string marketCode, string sessionSl)
        {
            var listData = otcInfoDao.GetSearchProduct(marketCode, sessionSl);
            return Json(listData);
        }


        [HttpPost]
        public ActionResult DeleteByDtlId(long? sl)
        {
            _validationMsg = otcInfoDao.DeleteData(sl);
            return Json(new { msg = _validationMsg });
        }

        [HttpPost]
        public ActionResult DeleteMasterOT(string marketCode)
        {
            _validationMsg = otcInfoDao.Delete(marketCode);
            return Json(new { msg = _validationMsg });
        }


    }
}