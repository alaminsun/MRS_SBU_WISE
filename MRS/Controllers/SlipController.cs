using MRS.DAL;
using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SlipController : Controller
    {


        private readonly SlipDAO slipDao = new SlipDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        //
        // GET: /Slip/
        public ActionResult frmSlip()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.Title = "Slip Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
            //return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SlipInfo(SlipMasterModel master)
        {
            _validationMsg = master.SUStatus == 0 ? slipDao.Save(master, Session["mappingUserID"].ToString()) : slipDao.Update(master, Session["mappingUserID"].ToString());
            return Json(new { msg = _validationMsg });
            //_validationMsg = slipDao.Save(master, Session["mappingUserID"].ToString()); 
            //return Json(new { msg = _validationMsg });

        }


        public ActionResult GetMarketInfo()
        {
            var MarketList = slipDao.GetMarketInfoList();
            return Json(MarketList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductInfo()
        {
            var data = slipDao.GetProductInfoList().OrderByDescending(m => m.PROD_ID);
            var instituteDetailList = Json(data, JsonRequestBehavior.AllowGet);
            instituteDetailList.MaxJsonLength = int.MaxValue;
            return instituteDetailList;

        }


        public JsonResult GetSessionSl()
        {
            var sessionList = slipDao.GetSessionSL();
            return Json(sessionList, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetSesstionMarketList(int sessionSl)
        {
            var listData = slipDao.GetSessionMarketData(sessionSl);
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSessionProduct(string marketCode, string sessionSl)
        {
            var listData = slipDao.GetSearchProduct(marketCode, sessionSl);
            return Json(listData);
        }


        [HttpPost]
        public ActionResult DeleteByDtlId(long? sl)
        {
            _validationMsg = slipDao.DeleteData(sl);
            return Json(new { msg = _validationMsg });
        }

        [HttpPost]
        public ActionResult DeleteMasterSL(string marketCode)
        {
            _validationMsg = slipDao.Delete(marketCode);
            return Json(new { msg = _validationMsg });
        }


    }
}