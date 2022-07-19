using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SelectedTCLevel2InfoController : Controller
    {
        SelectedTCLevel2InfoDAO Dalobject;
        private ValidationMsg _vmMsg;
        public SelectedTCLevel2InfoController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new SelectedTCLevel2InfoDAO();
        }
        public ActionResult frmSelectedTCLevel2Info()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Selected Therapeutic Class Level-2 Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SelectedTCLevel2Info(TC_LEVEL2Model model)
        {
            _vmMsg = Dalobject.Save(model);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string TC_L2_CODE)
        {
            _vmMsg = Dalobject.Delete(TC_L2_CODE);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllSelectedTCLevel2List()
        {
            var allData = Dalobject.GetAllSelectedTCLevel2List();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}