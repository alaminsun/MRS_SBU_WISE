using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class DivisionRegiController : Controller
    {

        DivisionRegiDAO Dalobject;
        private ValidationMsg _vmMsg;
        public DivisionRegiController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new DivisionRegiDAO();
        }
        public ActionResult frmDivisionRegi()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Division-Region Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DivisionRegi(DivisionRegiMasModel model)
        {
            model.SUStatus = Convert.ToInt32(model.DIVI_REGI_MAS_SLNO);
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDivisionList()
        {
            var zoneList = Dalobject.GetDivisionList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDivisionSearchList()
        {
            var zoneList = Dalobject.GetDivisionSearchList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetRegiInfoList()
        {
            var zoneList = Dalobject.GetRegiInfoList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSaveRegiList(string DIVI_REGI_MAS_SLNO)
        {
            var zoneList = Dalobject.GetSaveRegiList(DIVI_REGI_MAS_SLNO);
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string DIVI_REGI_MAS_SLNO)
        {
            _vmMsg = Dalobject.Delete(DIVI_REGI_MAS_SLNO);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedRegiInfo(string DIVI_REGI_DTL_SLNO)
        {
            _vmMsg = Dalobject.DeletedRegiInfo(DIVI_REGI_DTL_SLNO);
            return Json(new { msg = _vmMsg });
        }
    }
}