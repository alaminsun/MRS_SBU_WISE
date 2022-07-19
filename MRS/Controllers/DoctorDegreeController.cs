using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;

namespace MRS.Controllers
{
    public class DoctorDegreeController : Controller
    {

        DoctorDegreeDAO _doctorDegreeaoDao = new DoctorDegreeDAO();
        ValidationMsg _validationMsg = new ValidationMsg();

        public ActionResult FrmDoctorDegree()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Doctor Degree Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDoctorDegreeInformation()
        {
            var data = _doctorDegreeaoDao.GetDoctorDegreeData().OrderBy(m => m.DEGREE_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(DoctorDegreeModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _doctorDegreeaoDao.Save(model, Session["mappingUserID"].ToString()) : _doctorDegreeaoDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _doctorDegreeaoDao.GetCode(), msg = _validationMsg });
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string degreeCode)
        {
            _validationMsg = _doctorDegreeaoDao.Delete(degreeCode);
            return Json(new { msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestDocDegreeList()
        {
            var allData = _doctorDegreeaoDao.GetSuggestDocDegreeList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}