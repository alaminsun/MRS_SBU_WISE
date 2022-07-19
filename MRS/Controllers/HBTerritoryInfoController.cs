using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class HBTerritoryInfoController : Controller
    {
       HBTerritoryDAO Dalobject;
        private ValidationMsg _vmMsg;
        public HBTerritoryInfoController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new HBTerritoryDAO();
        }
        public ActionResult frmHBTerritory()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "HB Territory Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult HBTerritory(HBTerritoryModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string HBTerritoryCode)
        {
            _vmMsg = Dalobject.Delete(HBTerritoryCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllHBTerritoryList()
        {
            var allData = Dalobject.GetAllHBTerritoryList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestHBTerritoryList()
        {
            var allData = Dalobject.GetSuggestHBTerritoryList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}