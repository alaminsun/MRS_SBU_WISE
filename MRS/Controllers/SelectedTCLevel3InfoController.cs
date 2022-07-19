using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SelectedTCLevel3InfoController : Controller
    {
        SelectedTCLevel3InfoDAO Dalobject;
        private ValidationMsg _vmMsg;
        public SelectedTCLevel3InfoController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new SelectedTCLevel3InfoDAO();
        }
        public ActionResult frmSelectedTCLevel3Info()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Selected Therapeutic Class Level-3 Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SelectedTCLevel3Info(TC_LEVEL3Model model)
        {
            _vmMsg = Dalobject.Save(model);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string TC_L3_CODE)
        {
            _vmMsg = Dalobject.Delete(TC_L3_CODE);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllSelectedTCLevel3List()
        {
            var allData = Dalobject.GetAllSelectedTCLevel3List();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}