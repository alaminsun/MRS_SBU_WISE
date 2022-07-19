using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SessionInformationController : Controller
    {
        ValidationMsg _vmMsg;
        SessionDAO sessions = new SessionDAO();
        public ActionResult frmSessionInfo()
        {
            return View();
        }

        public ActionResult frmAllInOne()
        {
            return View();
        }
        public JsonResult FillUserId()
        {
            LoginModel model = new LoginModel();
            model.UserID = Session["mappingUserID"].ToString();
            model.UserName = Session["UserId"].ToString();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SessionInfo(SessionModel model)
        {
            _vmMsg = model.SUStatus == 0 ? sessions.Save(model) : sessions.Update(model);
            Session["SessionSL"] = _vmMsg.ReturnId;
            return Json(new { msg = _vmMsg });
        }

        public JsonResult GetSessionData()
        {
            var result = sessions.GetSessionData();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(int slNo)
        {
            _vmMsg = sessions.Delete(slNo);
            return Json(new { msg = _vmMsg });
        }
    }
}