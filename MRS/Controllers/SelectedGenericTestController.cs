using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SelectedGenericTestController : Controller
    {
        private readonly SelectedGenericDAO _selectedGenericDao;
        private ValidationMsg _vmMsg;
        public SelectedGenericTestController()
        {
            _vmMsg = new ValidationMsg();
            _selectedGenericDao = new SelectedGenericDAO();
        }

        public ActionResult FrmSelectedGenericTest()
        {
            //if (Session["UserId"] != null)
            //{
                ViewBag.formTitle = "Selected Generic Information";
                return View();
            //}
            //return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllSelectedGenericList()
        {
            var allData = _selectedGenericDao.GetAllSelectedGenericList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Save(SelectedGenericModel model)
        {
            _vmMsg = _selectedGenericDao.Save(model);
            return Json(new { msg = _vmMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string genericCode)
        {
            _vmMsg = _selectedGenericDao.Delete(genericCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSelectedGenericList(string selectGenericList)
        {
            var allData = _selectedGenericDao.GetSelectedGenericList(selectGenericList);
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}