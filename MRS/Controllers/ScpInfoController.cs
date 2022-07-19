using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class ScpInfoController : Controller
    {
        private readonly SCP_InfoDAO _scpInfoDAO = new SCP_InfoDAO();
        private readonly DoctorInformationDAO _docDAO = new DoctorInformationDAO();
        ValidationMsg _validationMsg = new ValidationMsg();


        public ActionResult frmScpInfo()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Special Campaign Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        //public ActionResult frmSCPConfig()
        //{
        //    //if (Session["UserId"] != null)
        //    {
        //        ViewBag.formTitle = "Special Campaign Configuration";
        //        return View();
        //    }
        //    //return RedirectToAction("Login", "Home");
        //}


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSCPList()
        {
            var data = _scpInfoDAO.GetSCPInfoList().OrderByDescending(m => m.SCP_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(SCP_INFOModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _scpInfoDAO.Save(model, Session["mappingUserID"].ToString()) : _scpInfoDAO.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _scpInfoDAO.GetCode(), msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string scpCode)
        {
            _validationMsg = _scpInfoDAO.Delete(scpCode);
            return Json(new { msg = _validationMsg });
        }
	}
}