using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SpecCamDocInsConfigController : Controller
    {
        SpecCamDocInsConfigDAO _dalSpecCamDocInsConfigDAO;
        private ValidationMsg _vmMsg;

        public SpecCamDocInsConfigController()
        {
            _vmMsg = new ValidationMsg();
            _dalSpecCamDocInsConfigDAO = new SpecCamDocInsConfigDAO();
        }
        //
        // GET: /SpecCamDocInsConfig/
        public ActionResult SpecCamDocInsConfig()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Special Campaign Doctor & Institution Configuration";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult SpecCamDocInsConfig(SpecCamDocInsConfigModel model)
        {
            _vmMsg = model.SDIC_ID == 0 ? _dalSpecCamDocInsConfigDAO.Save(model, Session["USER_ID"].ToString()) : _dalSpecCamDocInsConfigDAO.Update(model, Session["USER_ID"].ToString());
            return Json(new { msg = _vmMsg }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAll()
        {
            var data = _dalSpecCamDocInsConfigDAO.GetAll(0);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllItemListBySCId(string SDIC_ID)
        {
            int sdicId = Convert.ToInt32(SDIC_ID);
            var ScpGroupData = _dalSpecCamDocInsConfigDAO.GetScpGroupItemList(sdicId);

            if (ScpGroupData.Any())
            {
                int sdicgId = Convert.ToInt32(ScpGroupData.Where(o => o.SDIC_ID == sdicId).FirstOrDefault().SDICG_ID);
                var dataDoc = _dalSpecCamDocInsConfigDAO.GetDocItemList(sdicgId);
                var dataInst = _dalSpecCamDocInsConfigDAO.GetInstiItemList(sdicgId);
                return Json(new { ScpGroup = ScpGroupData, DoctorData = dataDoc, InstiData = dataInst }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ScpGroup = "", DoctorData = "", InstiData = "" }, JsonRequestBehavior.AllowGet);
            }


        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDoctorAndInstituteList(string SDICG_ID)
        {
            int sdicgId = Convert.ToInt32(SDICG_ID);

            var dataDoc = _dalSpecCamDocInsConfigDAO.GetDocItemList(sdicgId);
            var dataInst = _dalSpecCamDocInsConfigDAO.GetInstiItemList(sdicgId);
            return Json(new { DoctorData = dataDoc, InstiData = dataInst }, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllTerritoryList()
        {
            var allData = _dalSpecCamDocInsConfigDAO.GetAllTerritoryList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllDoctorInfo()
        {
            var data = _dalSpecCamDocInsConfigDAO.GetAllDoctorInfo();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetDoctorListForSearch()
        //{
        //    var data = _dalSpecCamDocInsConfigDAO.GetDoctorListForSearch();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetDoctorBySearch(string doctorCode)
        {
            var data = _dalSpecCamDocInsConfigDAO.GetDoctorBySearch(doctorCode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllInstituteInfo()
        {
            var data = _dalSpecCamDocInsConfigDAO.GetAllInstituteInfo();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInstituteBySearch(string instituteCode)
        {
            var data = _dalSpecCamDocInsConfigDAO.GetInstituteBySearch(instituteCode);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string SDIC_ID)
        {
            _vmMsg = _dalSpecCamDocInsConfigDAO.Delete(Convert.ToInt32(SDIC_ID));
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteGroupGrid(string SDICG_ID)
        {
            _vmMsg = _dalSpecCamDocInsConfigDAO.DeleteScpGroupGrid(Convert.ToInt32(SDICG_ID));
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedDoctorGrid(string SDI_ID)
        {
            _vmMsg = _dalSpecCamDocInsConfigDAO.DeletedDoctorGrid(Convert.ToInt32(SDI_ID));
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedInstiGrid(string SDII_ID)
        {
            _vmMsg = _dalSpecCamDocInsConfigDAO.DeletedInstiGrid(Convert.ToInt32(SDII_ID));
            return Json(new { msg = _vmMsg });
        }

    }
}