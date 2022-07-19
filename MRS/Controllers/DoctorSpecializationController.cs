using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MRS.DAL.Gateway;
using MRS.Models;
using System.Data;
using MRS.DAL.Common;
using System.Web.Mvc;
using MRS.DAL.DAO;

namespace MRS.Controllers
{
    public class DoctorSpecializationController : Controller
    {
        DoctorSpecializationDAO _doctorSpecializationDao = new DoctorSpecializationDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        public ActionResult FrmDoctorSpecialization()
        {
            {
                if (Session["UserId"] != null)
                {
                    ViewBag.formTitle = "Doctor Specialization Information";
                    return View();
                }
                return RedirectToAction("Login", "Home");
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDoctorSpecializationInformation()
        {
            var data = _doctorSpecializationDao.GetDoctorSpecializationData().OrderBy(m => m.SPECIALIZATION_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(DoctorSpecializationModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _doctorSpecializationDao.Save(model, Session["mappingUserID"].ToString()) : _doctorSpecializationDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _doctorSpecializationDao.GetCode(), msg = _validationMsg });
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string specializationCode)
        {
            _validationMsg = _doctorSpecializationDao.Delete(specializationCode);
            return Json(new { msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestDocSpecializationList()
        {
            var allData = _doctorSpecializationDao.GetSuggestDocSpecializationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

	}
}