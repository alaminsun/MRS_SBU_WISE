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
    public class DosageStrengthController : Controller
    {
        private readonly DosageStrengthDAO _dosageStrengthDao = new DosageStrengthDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        
        public ActionResult frmDosageStrength()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Dosage Form Strength Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDosageStrengthInformation()
        {
            var data = _dosageStrengthDao.GetDosageStrengthInformation().OrderByDescending(m => m.DSG_STRENGTH_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(DosageStrengthModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _dosageStrengthDao.Save(model, Session["mappingUserID"].ToString()) : _dosageStrengthDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _dosageStrengthDao.GetCode(), msg = _validationMsg });
        }

   
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string dosageStrengthCode)
        {
            _validationMsg = _dosageStrengthDao.Delete(dosageStrengthCode);
            return Json(new { msg = _validationMsg });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestDosageStrengthList()
        {
            var allData = _dosageStrengthDao.GetSuggestDosageStrengthList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}