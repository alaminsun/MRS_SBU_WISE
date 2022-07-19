using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class TerritoryMarController : Controller
    {
        TerritoryMarDAO Dalobject;
        private ValidationMsg _vmMsg;
        public TerritoryMarController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new TerritoryMarDAO();
        }
        public ActionResult frmTerritoryMar()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Territory-Market Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TerritoryMar(TerritoryMarMasModel model)
        {
            model.SUStatus = Convert.ToInt32(model.TERRI_MKT_MAS_SLNO);
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTerritoryList()
        {
            var zoneList = Dalobject.GetTerritoryList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTerritorySearchList()
        {
            var zoneList = Dalobject.GetTerritorySearchList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMarInfoList()
        {
            var zoneList = Dalobject.GetMarInfoList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSaveMarList(string TERRI_MKT_MAS_SLNO)
        {
            var zoneList = Dalobject.GetSaveMarList(TERRI_MKT_MAS_SLNO);
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string TERRI_MKT_MAS_SLNO)
        {
            _vmMsg = Dalobject.Delete(TERRI_MKT_MAS_SLNO);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedMarInfo(string TERRI_MKT_DTL_SLNO)
        {
            _vmMsg = Dalobject.DeletedMarInfo(TERRI_MKT_DTL_SLNO);
            return Json(new { msg = _vmMsg });
        }
	}
}