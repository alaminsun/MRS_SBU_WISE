using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class MarketSegmentProductController : Controller
    {
        MarketSegmentProductDAO Dalobject;
        private ValidationMsg _vmMsg;
        public MarketSegmentProductController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new MarketSegmentProductDAO();
        }
        public ActionResult frmMarketSegmentProduct()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Segment-Product Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MarketSegmentProduct(MarketSegmentProductMasModel model)
        {
            model.SUStatus = Convert.ToInt32(model.MKT_SEGM_PROD_MAS_SLNO);
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSegmentList()
        {
            var zoneList = Dalobject.GetSegmentList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSegmentSearchList()
        {
            var zoneList = Dalobject.GetSegmentSearchList();
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProductInfoList()
        {
            var zoneList = Dalobject.GetProductInfoList();
            var instituteDetailList = Json(zoneList, JsonRequestBehavior.AllowGet);
            instituteDetailList.MaxJsonLength = int.MaxValue;
            return instituteDetailList;
            //return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSaveProductList(string MKT_SEGM_PROD_MAS_SLNO)
        {
            var zoneList = Dalobject.GetSaveProductList(MKT_SEGM_PROD_MAS_SLNO);
            return Json(zoneList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string MKT_SEGM_PROD_MAS_SLNO)
        {
            _vmMsg = Dalobject.Delete(MKT_SEGM_PROD_MAS_SLNO);
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeletedProductInfo(string MKT_SEGM_PROD_DTL_SLNO)
        {
            _vmMsg = Dalobject.DeletedProductInfo(MKT_SEGM_PROD_DTL_SLNO);
            return Json(new { msg = _vmMsg });
        }
    }
}