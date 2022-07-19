using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class MarketTypeSegmentController : Controller
    {
        MarketTypeSegmentDAO Dalobject;
        private ValidationMsg _vmMsg;
        public MarketTypeSegmentController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new MarketTypeSegmentDAO();
        }
        public ActionResult frmMarketTypeSegment()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Type-Segment Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MarketTypeSegment(MarketTypeSegmentMasModel model)
        {
            model.SUStatus = Convert.ToInt32(model.MKT_TYPE_SEGM_MAS_SLNO);
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTypeList()
        {
            var zoneList = Dalobject.GetTypeList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTypeSearchList()
        {
            var zoneList = Dalobject.GetTypeSearchList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSegmentInfoList()
        {
            var zoneList = Dalobject.GetSegmentInfoList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSaveSegmentList(string MKT_TYPE_SEGM_MAS_SLNO)
        {
            var zoneList = Dalobject.GetSaveSegmentList(MKT_TYPE_SEGM_MAS_SLNO);
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string MKT_TYPE_SEGM_MAS_SLNO)
        {
            _vmMsg = Dalobject.Delete(MKT_TYPE_SEGM_MAS_SLNO);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedSegmentInfo(string MKT_TYPE_SEGM_DTL_SLNO)
        {
            _vmMsg = Dalobject.DeletedSegmentInfo(MKT_TYPE_SEGM_DTL_SLNO);
            return Json(new { msg = _vmMsg });
        }
    }
}