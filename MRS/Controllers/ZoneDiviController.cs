using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class ZoneDiviController : Controller
    {
        ZoneDiviDAO Dalobject;
        private ValidationMsg _vmMsg;
        public ZoneDiviController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new ZoneDiviDAO();
        }
        public ActionResult frmZoneDivi()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Zone-Division Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ZoneDivi(ZoneDiviMasModel model)
        {
            model.SUStatus = Convert.ToInt32(model.ZONE_DIVI_MAS_SLNO);
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetZoneList()
        {
            var zoneList = Dalobject.GetZoneList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetZoneSearchList()
        {
            var zoneList = Dalobject.GetZoneSearchList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDiviInfoList()
        {
            var zoneList = Dalobject.GetDiviInfoList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSaveDiviList(string ZONE_DIVI_MAS_SLNO)
        {
            var zoneList = Dalobject.GetSaveDiviList(ZONE_DIVI_MAS_SLNO);
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string ZONE_DIVI_MAS_SLNO)
        {
            _vmMsg = Dalobject.Delete(ZONE_DIVI_MAS_SLNO);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedDiviInfo(string ZONE_DIVI_DTL_SLNO)
        {
            _vmMsg = Dalobject.DeletedDiviInfo(ZONE_DIVI_DTL_SLNO);
            return Json(new { msg = _vmMsg });
        }
    }
}