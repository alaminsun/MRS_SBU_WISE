using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;

namespace MRS.Controllers
{
    public class DistrictController : Controller
    {
        private readonly DistrictDAO _districtDao = new DistrictDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        public ActionResult FrmDistrict()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "District Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDistrictList()
        {
            var data = _districtDao.GetDistrictList().OrderByDescending(m => m.DISTC_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(DistrictModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _districtDao.Save(model, Session["mappingUserID"].ToString()) : _districtDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _districtDao.GetCode(), msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string districtCode)
        {
            _validationMsg = _districtDao.Delete(districtCode);
            return Json(new { msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestDistrictList()
        {
            var allData = _districtDao.GetSuggestDistrictList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}