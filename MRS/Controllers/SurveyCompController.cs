using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SurveyCompController : Controller
    {
        SurveyCompDAO Dalobject;
        private ValidationMsg _vmMsg;
        public SurveyCompController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new SurveyCompDAO();
        }
        public ActionResult frmSurveyComp()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Survey Company";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SurveyComp(SurveyCompModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string SurveyCompCode)
        {
            _vmMsg = Dalobject.Delete(SurveyCompCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllSurveyCompList()
        {
            var allData = Dalobject.GetAllSurveyCompList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestSurveyCompList()
        {
            var allData = Dalobject.GetSuggestSurveyCompList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}