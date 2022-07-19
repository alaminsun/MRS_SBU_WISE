using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SelectedGenericController : Controller
    {

        private readonly SelectedGenericDAO _selectedGenericDao;
        private  ValidationMsg _vmMsg;
        public SelectedGenericController()
        {
            _vmMsg = new ValidationMsg();
            _selectedGenericDao = new SelectedGenericDAO();
        }

        public ActionResult FrmSelectedGeneric()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Selected Generic Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllSelectedGenericList()
        {
            var allData = _selectedGenericDao.GetAllSelectedGenericList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }


        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult SaveAndUpdate(SelectedGenericModel model)
        //{
        //    _vmMsg = model.SUStatus == 0 ? _selectedGenericDao.Save(model, Session["mappingUserID"].ToString()) : _selectedGenericDao.Update(model, Session["mappingUserID"].ToString());
        //    return Json(new { msg = _vmMsg });
        //}


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string genericCode)
        {
            _vmMsg = _selectedGenericDao.Delete(genericCode);
            return Json(new { msg = _vmMsg });
        }

	}
}