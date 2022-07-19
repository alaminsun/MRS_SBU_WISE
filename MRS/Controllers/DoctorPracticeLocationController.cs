using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class DoctorPracticeLocationController : Controller
    {

        private readonly DoctorPracticeLocationDAO _doctorPracticeLocationDao = new DoctorPracticeLocationDAO();
        ValidationMsg _validationMsg = new ValidationMsg();

        public ActionResult FrmDoctorPracticeLocation()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Doctor Practice Location Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDoctorPracticeLocationInformation()
        {
            var data = _doctorPracticeLocationDao.GetDoctorPracticeLocationData().OrderBy(m => m.DP_LOC_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(DoctorPracticeLocationModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _doctorPracticeLocationDao.Save(model, Session["mappingUserID"].ToString()) : _doctorPracticeLocationDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _doctorPracticeLocationDao.GetCode(), msg = _validationMsg });
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string doctorPracticeLocCode)
        {
            _validationMsg = _doctorPracticeLocationDao.Delete(doctorPracticeLocCode);
            return Json(new { msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestDocPracLocationList()
        {
            var allData = _doctorPracticeLocationDao.GetSuggestDocPracLocationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

	}
}