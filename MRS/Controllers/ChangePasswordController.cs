using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class ChangePasswordController : Controller
    {
        ChangePasswordDAO Dalobject;
        private ValidationMsg _vmMsg;
        public ChangePasswordController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new ChangePasswordDAO();
        }
        public ActionResult frmChangePassword()
        {
            //if (Session["UserId"] != null)
            //{
            ViewBag.formTitle = "Change Password Information";
            return View();
            //}
            //return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            _vmMsg = Dalobject.Update(model, Session["USER_ID"].ToString(), Session["PASSWORD"].ToString());
            return Json(new { msg = _vmMsg });
        }

        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult Delete(string MGMT_CODE)
        //{
        //    _vmMsg = Dalobject.Delete(MGMT_CODE);
        //    return Json(new { msg = _vmMsg });
        //}

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult GetAllChangePasswordList()
        //{
        //    var allData = Dalobject.GetAllChangePasswordList();
        //    return Json(allData, JsonRequestBehavior.AllowGet);
        //}

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult GetSuggestChangePasswordList()
        //{
        //    var allData = Dalobject.GetSuggestChangePasswordList();
        //    return Json(allData, JsonRequestBehavior.AllowGet);
        //}
    }
}