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
    public class InstitutionGroupController : Controller
    {
        InstitutionGroupDAO _institutionGroupDao = new InstitutionGroupDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        public ActionResult FrmInstitutionGroup()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Institution Group Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetInstitutionGroup()
        {
            var data = _institutionGroupDao.GetInstitutionGroupInformationData().OrderBy(m => m.GROUP_ID);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(InstitutionGroupModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _institutionGroupDao.Save(model, Session["mappingUserID"].ToString()) : _institutionGroupDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _institutionGroupDao.GetCode(), msg = _validationMsg });
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string institutionGroupCode)
        {
            _validationMsg = _institutionGroupDao.Delete(institutionGroupCode);
            return Json(new { msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestInsGroupList()
        {
            var allData = _institutionGroupDao.GetSuggestInsGroupList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}