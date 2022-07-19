using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class PrescriptionInfoController : Controller
    {
        PrescriptionDAO prescriptions = new PrescriptionDAO();
        ValidationMsg _vmMsg = new ValidationMsg();

        public ActionResult frmPrescriptionInfo()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Prescription Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [HttpPost]
        public JsonResult GetMarketsOrg(string prescNo)
        {
            var marketList = prescriptions.LoadMarketO(prescNo);
            return Json(marketList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetOrgcMarketList()
        {
            var marketList = prescriptions.OMarketList();
            return Json(marketList, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMarkets(string prescNo)
        {
            var marketList = prescriptions.LoadMarket(prescNo);
            return Json(marketList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPrescMarketList()
        {
            var marketList = prescriptions.MarketList();
            return Json(marketList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetProducts()
        {
            var productList = prescriptions.ProductList();
            var data = Json(productList, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }

        public ActionResult Prescription(PrescriptionModel model)
        {
            _vmMsg = model.SUStatus == 0 ? prescriptions.Save(model) : prescriptions.Update(model);
            return Json(new { msg = _vmMsg });
        }

        public JsonResult GetSessionSl()
        {
            var sessionList = prescriptions.GetSessionSL();
            return Json(sessionList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSesstionDoctorList(int sessionSl)
        {
            var listData = prescriptions.GetSessionDoctorsData(sessionSl);
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSessionProduct(int doctorId, long sessionSl)
        {
            var listData = prescriptions.GetSearchProduct(doctorId, sessionSl);
            return Json(listData);
        }

        [HttpPost]
        public ActionResult Delete(long? sl)
        {
            _vmMsg = prescriptions.DeleteData(sl);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeleteMaster(int doctorId, long sessionSl)
        {
            _vmMsg = prescriptions.Delete(doctorId, sessionSl);
            return Json(new { msg = _vmMsg });
        }

        [HttpGet]
        public JsonResult GetPrescriptionList(string prscNo)
        {
            var listData = prescriptions.GetPrescriptionList(prscNo);
            var jsonResult = Json(listData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetDoctorInfo(int DoctorID)
        {
            var listData = prescriptions.GetDoctorInfo(DoctorID);
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInstPrescriptionList(string prscNo)
        {
            var listData = prescriptions.GetInstPrescriptionList(prscNo);
            var jsonResult = Json(listData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public ActionResult GetInstInfo(int InstituteCode)
        {
            var listData = prescriptions.GetInstInfo(InstituteCode);
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSInstituteProduct(int InstituteCode, long sessionSl)
        {
            var listData = prescriptions.GetSInstituteProduct(InstituteCode, sessionSl);
            return Json(listData, JsonRequestBehavior.AllowGet); 
        }

        [HttpGet]
        public JsonResult GetOTCPrescriptionList()
        {
            var listData = prescriptions.GetOTCPrescriptionList();
            var jsonResult = Json(listData, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}