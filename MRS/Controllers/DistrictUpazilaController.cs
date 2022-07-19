using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class DistrictUpazilaController : Controller
    {
        private DistrictUpazilaDAO _districtUpazilaDao;
        private ValidationMsg _vmMsg;

        public DistrictUpazilaController()
        {
            _vmMsg = new ValidationMsg();
            _districtUpazilaDao = new DistrictUpazilaDAO();
        }

        public ActionResult FrmDistrictUpazila()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "District Upazila Relation Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDistrictData()
        {
            var tcL1L2List = _districtUpazilaDao.GetDistrictList();
            return Json(tcL1L2List, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetUpazilaData()
        {
            var tcL1L2List = _districtUpazilaDao.GetUpazilaList();
            return Json(tcL1L2List, JsonRequestBehavior.AllowGet);
        }



        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdate(DistrictUpazilaMstModel model)
        {
            _vmMsg = model.SUStatus == 0 ? _districtUpazilaDao.Save(model, Session["mappingUserID"].ToString()) : _districtUpazilaDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSearchMasterList()
        {
            var tcL1L2SearchList = _districtUpazilaDao.GetSearchMasterList();
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSearchDetailList(string districtUpazilaMstSlNo)
        {
            var tcL1L2SearchList = _districtUpazilaDao.GetSearchDtliList(districtUpazilaMstSlNo);
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAllList(string distrctUpazilaMstSlNo)
        {
            _vmMsg = _districtUpazilaDao.Delete(distrctUpazilaMstSlNo);
            return Json(new { msg = _vmMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDetailList(string districtUpazilaDtlSlNo)
        {
             _vmMsg = _districtUpazilaDao.DeleteDetailData(districtUpazilaDtlSlNo);
            return Json(new { msg = _vmMsg });
        }

	}
}