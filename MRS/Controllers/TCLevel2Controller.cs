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
    public class TCLevel2Controller : Controller
    {
        TC_Level2DAO _tcLevel2Dao = new TC_Level2DAO();
        ValidationMsg _validationMsg = new ValidationMsg();

        public ActionResult frmTCLEVEL2()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Therapeutic Class Level-2 Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTherapeuticLevel2List()
        {
            var data = _tcLevel2Dao.GetTherapeuticLevel2List().OrderByDescending(m => m.TC_L2_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(TC_LEVEL2Model model)
        {
            _validationMsg = model.SUStatus == 0 ? _tcLevel2Dao.Save(model, Session["mappingUserID"].ToString()) : _tcLevel2Dao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _tcLevel2Dao.GetCode(), msg = _validationMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string tcL2Code)
        {
            _validationMsg = _tcLevel2Dao.Delete(tcL2Code);
            return Json(new { msg = _validationMsg });
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestTCLevel2List()
        {
            var allData = _tcLevel2Dao.GetSuggestTCLevel2List();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
	}
}