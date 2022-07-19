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
    public class TCLevel3Controller : Controller
    {
        TC_Level3DAO _tcLevel3Dao = new TC_Level3DAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        public ActionResult frmTCLEVEL3()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Therapeutic Class Level-3 Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTherapeuticLevel3List()
        {
            var data = _tcLevel3Dao.GetTherapeuticLevel3List().OrderByDescending(m => m.TC_L3_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(TC_LEVEL3Model model)
        {
            _validationMsg = model.SUStatus == 0 ? _tcLevel3Dao.Save(model, Session["mappingUserID"].ToString()) : _tcLevel3Dao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _tcLevel3Dao.GetCode(), msg = _validationMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string tcL3Code)
        {
            _validationMsg = _tcLevel3Dao.Delete(tcL3Code);
            return Json(new { msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestTCLevel3List()
        {
            var allData = _tcLevel3Dao.GetSuggestTCLevel3List();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}