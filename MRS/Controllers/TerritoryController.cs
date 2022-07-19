using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class TerritoryController : Controller
    {
        TerritoryDAO Dalobject;
        private ValidationMsg _vmMsg;
        public TerritoryController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new TerritoryDAO();
        }
        public ActionResult frmTerritory()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Territory Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Territory(TerritoryModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string TerritoryCode)
        {
            _vmMsg = Dalobject.Delete(TerritoryCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllTerritoryList()
        {
            var allData = Dalobject.GetAllTerritoryList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestTerritoryList()
        {
            var allData = Dalobject.GetSuggestTerritoryList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}