using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SelectedTCLevel1InfoController : Controller
    {
        SelectedTCLevel1InfoDAO Dalobject;
        private ValidationMsg _vmMsg;
        public SelectedTCLevel1InfoController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new SelectedTCLevel1InfoDAO();
        }
        public ActionResult frmSelectedTCLevel1Info()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Selected Therapeutic Class Level-1 Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SelectedTCLevel1Info(TC_LEVEL1Model model)
        {
            _vmMsg = Dalobject.Save(model);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string TC_L1_CODE)
        {
            _vmMsg = Dalobject.Delete(TC_L1_CODE);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllSelectedTCLevel1List()
        {
            var allData = Dalobject.GetAllSelectedTCLevel1List();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}