using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SelectedTCLevel4InfoController : Controller
    {
        SelectedTCLevel4InfoDAO Dalobject;
        private ValidationMsg _vmMsg;
        public SelectedTCLevel4InfoController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new SelectedTCLevel4InfoDAO();
        }
        public ActionResult frmSelectedTCLevel4Info()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Selected Therapeutic Class Level-4 Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SelectedTCLevel4Info(TC_LEVEL4Model model)
        {
            _vmMsg = Dalobject.Save(model);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string TC_L4_CODE)
        {
            _vmMsg = Dalobject.Delete(TC_L4_CODE);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllSelectedTCLevel4List()
        {
            var allData = Dalobject.GetAllSelectedTCLevel4List();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}