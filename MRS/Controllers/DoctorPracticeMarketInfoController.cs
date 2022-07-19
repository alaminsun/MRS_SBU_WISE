using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class DoctorPracticeMarketInfoController : Controller
    {
        DoctorMarketInfoDAO doctorMarketInfo = new DoctorMarketInfoDAO();
        ValidationMsg _vmMsg;
        // GET: /DoctorPracticeMarketInfo/
        public ActionResult FrmDoctorPracticeMarketInfo()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Doctor Practice Market Information";
                ViewBag.USER_ID = Session["USER_ID"].ToString();
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        public JsonResult InstitutionList()
        {
            List<Institution> instituonList = doctorMarketInfo.GetInstitionList();
            return Json(instituonList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpazilaDistrictList()
        {
            List<DistrictUpazila> listData = doctorMarketInfo.GetDistrictUpazila();
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLoactionList(string marketCode)
        {
            List<Loacation> listData = doctorMarketInfo.GetLocation(marketCode);
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DoctorPractice(DoctorMarketInfoModel model)
        {
            _vmMsg = model.SUStatus == 0 ? doctorMarketInfo.Save(model) : doctorMarketInfo.Update(model);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public JsonResult Search(int doctorId)
        {
            List<DoctorMarketDetailsModel> listData = doctorMarketInfo.GetDoctorPracticeList(doctorId);
            return Json(listData);
        }

        public JsonResult GetSearchPopUpMktData(int doctorId)
        {
            var listData = doctorMarketInfo.GetSearchPopUpMktData(doctorId);
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int slNo)
        {
            _vmMsg = doctorMarketInfo.Delete(slNo);
            return Json(new { msg = _vmMsg });
        }

        public ActionResult GetMarket()
        {
            var marketList = doctorMarketInfo.MarketList();
            return Json(marketList, JsonRequestBehavior.AllowGet);
        }
    }
}