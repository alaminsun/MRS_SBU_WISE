using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class DepoController : Controller
    {
        DepoDAO Dalobject;
        private ValidationMsg _vmMsg;
        public DepoController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new DepoDAO();
        }
        public ActionResult frmDepo()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Depo Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        //Amir Hamza
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Depo(DepoModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string DepoCode)
        {
            _vmMsg = Dalobject.Delete(DepoCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllDepoList()
        {
            var allData = Dalobject.GetAllDepoList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestDepoList()
        {
            var allData = Dalobject.GetSuggestDepoList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}