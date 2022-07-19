using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class DoctorSocietyController : Controller
    {
        DoctorSocietyDAO Dalobject;
        private ValidationMsg _vmMsg;
        public DoctorSocietyController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new DoctorSocietyDAO();
        }
        public ActionResult frmDoctorSociety()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Society Association Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DoctorSociety(DoctorSocietyModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model) : Dalobject.Update(model);
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string SOCIETY_ID)
        {
            _vmMsg = Dalobject.Delete(SOCIETY_ID);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllDoctorSocietyList()
        {
            var allData = Dalobject.GetAllDoctorSocietyList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestDoctorSocietyList()
        {
            var allData = Dalobject.GetSuggestDoctorSocietyList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}