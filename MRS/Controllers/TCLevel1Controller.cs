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
    public class TCLevel1Controller : Controller
    {
        private readonly TC_Level1DAO _tcLevel1Dao = new TC_Level1DAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        
        public ActionResult frmTCLEVEL1()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Therapeutic Class Level-1 Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTherapeuticLevel1List()
        {
            var data = _tcLevel1Dao.GetTherapeuticLevel1List().OrderByDescending(m => m.TC_L1_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(TC_LEVEL1Model model)
        {
            _validationMsg = model.SUStatus == 0 ? _tcLevel1Dao.Save(model, Session["mappingUserID"].ToString()) : _tcLevel1Dao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _tcLevel1Dao.GetCode(), msg = _validationMsg });
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string tcL1Code)
        {
            _validationMsg = _tcLevel1Dao.Delete(tcL1Code);
            return Json(new { msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestTCLevel1List()
        {
            var allData = _tcLevel1Dao.GetSuggestTCLevel1List();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}