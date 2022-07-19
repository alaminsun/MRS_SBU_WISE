using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class ProductMarketTypeRelationController : Controller
    {

        ProductMarketTypeRelationDAO Dalobject;
        private ValidationMsg _vmMsg;
        public ProductMarketTypeRelationController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new ProductMarketTypeRelationDAO();
        }
        //
        // GET: /ProductMarketTypeRelation/
        public ActionResult frmProductMarketTypeRelation()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Product Market Type Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
            //return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProductList()
        {
            var productList = Dalobject.GetProductList();
            var data = Json(productList, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMarketTypeList()
        {
            var DataList = Dalobject.GetMarketTypeList();
            return Json(DataList, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetProductSearchList()
        {
            var DataList = Dalobject.GetProductSearchList();
            return Json(DataList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetMarketTypeSearchList(string ProductMarketTypeMstSl)
        {
            var DataList = Dalobject.GetMarketTypeSearchList(ProductMarketTypeMstSl);
            return Json(DataList, JsonRequestBehavior.AllowGet);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult OperationsMode(ProductMarketTypeRelationMasModel Master)
        {
            _vmMsg = Master.SUStatus == 0 ? Dalobject.Save(Master, Session["mappingUserID"].ToString()) : Dalobject.Update(Master, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });

        }
        //public ActionResult GetDataForGroupDropDown(string MarketTypeCode, string ProductGroup)
        //{

        //    var data = Dalobject.GetDataForProductGroupDropDown(MarketTypeCode, ProductGroup);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        //Delete all data from retailer collection info 
        [HttpPost]
        public ActionResult DeleteAllData(string model)
        {
            _vmMsg = Dalobject.Delete(model);
            return Json(new { msg = _vmMsg });
        }

        //Delete from retailer collection info through detail id
        [HttpPost]

        public ActionResult DeleteByDtlId(string dtlSlNo)
        {
            _vmMsg = Dalobject.DeleteByDtlId(dtlSlNo);
            return Json(new { msg = _vmMsg });
        }
	}
}