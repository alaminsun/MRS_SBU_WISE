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
    public class UpazilaController : Controller
    {
        private readonly UpazilaDAO _upazilaDao = new UpazilaDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        public ActionResult FrmUpazila()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Upazila Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetUpazilaList()
        {
            var allData = _upazilaDao.GetUpazilaList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(UpazilaModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _upazilaDao.Save(model, Session["mappingUserID"].ToString()) : _upazilaDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _upazilaDao.GetCode(), msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string upazilaCode)
        {
            _validationMsg = _upazilaDao.Delete(upazilaCode);
            return Json(new { msg = _validationMsg });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestUpazilaList()
        {
            var allData = _upazilaDao.GetSuggestUpazilaList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}