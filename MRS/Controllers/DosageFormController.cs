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
    public class DosageFormController : Controller
    {
        private readonly DosageFormDAO _dosageFormDao = new DosageFormDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        
        
        public ActionResult frmDosageForm()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Dosage Form Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDosageFormInformation()
        {
            var data = _dosageFormDao.GetDosageFormInformationData().OrderByDescending(m => m.DOSAGE_FORM_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(DosageFormModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _dosageFormDao.Save(model, Session["mappingUserID"].ToString()) : _dosageFormDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _dosageFormDao.GetCode(), msg = _validationMsg });
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string dosageFormCode)
        {
            _validationMsg = _dosageFormDao.Delete(dosageFormCode);
            return Json(new { msg = _validationMsg });
        }
        public ActionResult GetSuggestDosageFormList()
        {
            var allData = _dosageFormDao.GetSuggestDosageFormList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}