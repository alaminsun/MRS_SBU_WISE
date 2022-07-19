using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class InstitutionTypeController : Controller
    {
        InstitutionTypeDAO _institutionTypeDao = new InstitutionTypeDAO();
        ValidationMsg _validationMsg = new ValidationMsg();

        public ActionResult FrmInstitutionType()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Institution Type Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetInstitutionTypeInformation()
        {
            var data = _institutionTypeDao.GetInstitutionTypeInformationData().OrderBy(m => m.INSTI_TYPE_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(InstitutionTypeInformationModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _institutionTypeDao.Save(model, Session["mappingUserID"].ToString()) : _institutionTypeDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _institutionTypeDao.GetCode(), msg = _validationMsg });
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string institutionTypeCode)
        {
            _validationMsg = _institutionTypeDao.Delete(institutionTypeCode);
            return Json(new { msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestInsTypeList()
        {
            var allData = _institutionTypeDao.GetSuggestInsTypeList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}