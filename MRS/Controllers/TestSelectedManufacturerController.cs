using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class TestSelectedManufacturerController : Controller
    {
        SelectedManufacturerInfoDAO Dalobject;
        private ValidationMsg _vmMsg;
        public TestSelectedManufacturerController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new SelectedManufacturerInfoDAO();
        }
        public ActionResult frmTestSelectedManufacturerInfo()
        {
            ViewBag.formTitle = "Test Selected Manufacturer Information";
            return View();
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
        public ActionResult GetSelectedManufacturerList(string selectManufacturerList)
        {
            var allData = Dalobject.GetSelectedManufacturerList(selectManufacturerList);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}