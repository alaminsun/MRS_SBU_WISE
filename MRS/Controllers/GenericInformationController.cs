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
    public class GenericInformationController : Controller
    {
        GeniricInformationDAO _genericDAO = new GeniricInformationDAO();
        ValidationMsg _validationMsg = new ValidationMsg();

        public ActionResult frmGenericInformation()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Generic Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetGenericInformation()
        {
            var data = _genericDAO.GetGenericInformationData().OrderBy(m => m.GENERIC_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(GenericInformationModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _genericDAO.Save(model, Session["mappingUserID"].ToString()) : _genericDAO.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _genericDAO.GetCode(), msg = _validationMsg });
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string genericCode)
        {
            _validationMsg = _genericDAO.Delete(genericCode);
            return Json(new { msg = _validationMsg });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestGenericList()
        {
            var allData = _genericDAO.GetSuggestGenericList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}