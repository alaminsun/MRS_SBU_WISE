using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SelectedManufacturerInfoTestController : Controller
    {
        SelectedManufacturerInfoDAO Dalobject;
        private ValidationMsg _vmMsg;
        public SelectedManufacturerInfoTestController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new SelectedManufacturerInfoDAO();
        }
        public ActionResult frmSelectedManufacturerInfoTest()
        {
            ViewBag.formTitle = "Selected Manufacturer Information";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SelectedManufacturerInfo(ManufacturerInfoModel model)
        {
            _vmMsg =  Dalobject.Save(model);
            return Json(new { msg = _vmMsg });
        }

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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSelectedManufacturerList(string selectManufacturerList)
        {
            var allData = Dalobject.GetSelectedManufacturerList(selectManufacturerList);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}