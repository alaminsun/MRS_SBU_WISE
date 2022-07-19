using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class TabManufacturerController : Controller
    {
        ManufacturerInfoDAO Dalobject;
        SelectedManufacturerInfoDAO DalSelectedobject;
        private ValidationMsg _vmMsg;
        public TabManufacturerController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new ManufacturerInfoDAO();
            DalSelectedobject = new SelectedManufacturerInfoDAO();
        }
        public ActionResult FrmTabManufacturer()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Manufacturer Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ManufacturerInfo(ManufacturerInfoModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult SelectedManufacturerInfo(ManufacturerInfoModel model)
        //{
        //    _vmMsg = DalSelectedobject.Save(model, Session["mappingUserID"].ToString());
        //    return Json(new { msg = _vmMsg });
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string ManufacturerCode)
        {
            _vmMsg = Dalobject.Delete(ManufacturerCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllManufacturerList()
        {
            var allData = Dalobject.GetAllManufacturerList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}