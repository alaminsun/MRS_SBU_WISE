using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class DivisonController : Controller
    {
        DivisonDAO Dalobject;
        private ValidationMsg _vmMsg;
        public DivisonController()
        {
            _vmMsg = new ValidationMsg();
            Dalobject = new DivisonDAO();
        }
        public ActionResult frmDivison()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Divison Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Divison(DivisonModel model)
        {
            _vmMsg = model.SUStatus == 0 ? Dalobject.Save(model, Session["mappingUserID"].ToString()) : Dalobject.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = Dalobject.GetCode(), msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string DivisonCode)
        {
            _vmMsg = Dalobject.Delete(DivisonCode);
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllDivisonList()
        {
            var allData = Dalobject.GetAllDivisonList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestDivisonList()
        {
            var allData = Dalobject.GetSuggestDivisonList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}