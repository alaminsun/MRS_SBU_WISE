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
    public class TCLevel4Controller : Controller
    {
        TC_Level4DAO _tcLevel4Dao = new TC_Level4DAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        public ActionResult frmTCLEVEL4()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Therapeutic Class Level-4 Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTherapeuticLevel4List()
        {
            var data = _tcLevel4Dao.GetTherapeuticLevel4List().OrderByDescending(m => m.TC_L4_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(TC_LEVEL4Model model)
        {
            _validationMsg = model.SUStatus == 0 ? _tcLevel4Dao.Save(model, Session["mappingUserID"].ToString()) : _tcLevel4Dao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _tcLevel4Dao.GetCode(), msg = _validationMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string tcL4Code)
        {
            _validationMsg = _tcLevel4Dao.Delete(tcL4Code);
            return Json(new { msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestTCLevel4List()
        {
            var allData = _tcLevel4Dao.GetSuggestTCLevel4List();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}