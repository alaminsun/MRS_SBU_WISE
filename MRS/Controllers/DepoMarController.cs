using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class DepoMarController : Controller
    {
        DepoMarDAO Dalobject;
        private ValidationMsg _vmMsg;
        public DepoMarController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new DepoMarDAO();
        }
        public ActionResult frmDepoMar()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Depo-Market Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DepoMar(DepoMarMasModel model)
        {
            model.SUStatus = Convert.ToInt32(model.DEPOT_MKT_MAS_SLNO);
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDepoList()
        {
            var zoneList = Dalobject.GetDepoList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDepoSearchList()
        {
            var zoneList = Dalobject.GetDepoSearchList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMarInfoList()
        {
            var zoneList = Dalobject.GetMarInfoList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSaveMarList(string DEPOT_MKT_MAS_SLNO)
        {
            var zoneList = Dalobject.GetSaveMarList(DEPOT_MKT_MAS_SLNO);
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string DEPOT_MKT_MAS_SLNO)
        {
            _vmMsg = Dalobject.Delete(DEPOT_MKT_MAS_SLNO);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedMarInfo(string DEPOT_MKT_DTL_SLNO)
        {
            _vmMsg = Dalobject.DeletedMarInfo(DEPOT_MKT_DTL_SLNO);
            return Json(new { msg = _vmMsg });
        }
    }
}