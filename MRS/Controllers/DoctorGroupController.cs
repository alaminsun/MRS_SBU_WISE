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
    public class DoctorGroupController : Controller
    {
        DoctorGroupDAO _doctorGroupDao = new DoctorGroupDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        public ActionResult FrmDoctorGroup()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Doctor Group Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDoctorGroup()
        {
            var data = _doctorGroupDao.GetDoctorGroupInformationData().OrderBy(m => m.GROUP_ID);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(DoctorGroupModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _doctorGroupDao.Save(model, Session["mappingUserID"].ToString()) : _doctorGroupDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _doctorGroupDao.GetCode(), msg = _validationMsg });
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string doctorGroupCode)
        {
            _validationMsg = _doctorGroupDao.Delete(doctorGroupCode);
            return Json(new { msg = _validationMsg });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestDocGroupList()
        {
            var allData = _doctorGroupDao.GetSuggestDocGroupList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}