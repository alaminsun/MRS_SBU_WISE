using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class RegionTerriController : Controller
    {
        RegionTerriDAO Dalobject;
        private ValidationMsg _vmMsg;
        public RegionTerriController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new RegionTerriDAO();
        }
        public ActionResult frmRegionTerri()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Region-Territory Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RegionTerri(RegionTerriMasModel model)
        {
            model.SUStatus = Convert.ToInt32(model.REGI_TERRI_MAS_SLNO);
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetRegionList()
        {
            var zoneList = Dalobject.GetRegionList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetRegionSearchList()
        {
            var zoneList = Dalobject.GetRegionSearchList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTerriInfoList()
        {
            var zoneList = Dalobject.GetTerriInfoList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSaveTerriList(string REGI_TERRI_MAS_SLNO)
        {
            var zoneList = Dalobject.GetSaveTerriList(REGI_TERRI_MAS_SLNO);
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string REGI_TERRI_MAS_SLNO)
        {
            _vmMsg = Dalobject.Delete(REGI_TERRI_MAS_SLNO);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedTerriInfo(string REGI_TERRI_DTL_SLNO)
        {
            _vmMsg = Dalobject.DeletedTerriInfo(REGI_TERRI_DTL_SLNO);
            return Json(new { msg = _vmMsg });
        }
    }
}