using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SelectedManufacturerInfoController : Controller
    {
        SelectedManufacturerInfoDAO Dalobject;
        private ValidationMsg _vmMsg;
        public SelectedManufacturerInfoController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new SelectedManufacturerInfoDAO();
        }
        public ActionResult frmSelectedManufacturerInfo()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Selected Manufacturer Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult SelectedManufacturerInfo(ManufacturerInfoModel model)
        //{
        //    _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
        //    return Json(new { msg = _vmMsg });
        //}

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string ManufacturerCode)
        {
            _vmMsg = Dalobject.Delete(ManufacturerCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllSelectedManufacturerList()
        {
            var allData = Dalobject.GetAllSelectedManufacturerList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}