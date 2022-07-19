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
    public class DoctorDesignationController : Controller
    {
        DoctorDesignationDAO _doctorDesignationDao = new DoctorDesignationDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        public ActionResult FrmDoctorDesignation()
        {
            {
                if (Session["UserId"] != null)
                {
                    ViewBag.formTitle = "Doctor Designation Information";
                    return View();
                }
                return RedirectToAction("Login", "Home");
            }
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDoctorDesignationInformation()
        {
            var data = _doctorDesignationDao.GetDoctorDesignationData().OrderBy(m => m.DESIGNATION_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(DoctorDesignationModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _doctorDesignationDao.Save(model, Session["mappingUserID"].ToString()) : _doctorDesignationDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _doctorDesignationDao.GetCode(), msg = _validationMsg });
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string designationCode)
        {
            _validationMsg = _doctorDesignationDao.Delete(designationCode);
            return Json(new { msg = _validationMsg });
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestDocDesignationList()
        {
            var allData = _doctorDesignationDao.GetSuggestDocDesignationList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

	}
}