using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class InstitutionCoverMarketInformationController : Controller
    {

        private readonly InstitutionCoverMarketInformationDAO institutionCoverMarketInformationDao = new InstitutionCoverMarketInformationDAO();
        ValidationMsg _validationMsg = new ValidationMsg();
        //
        // GET: /InstitutionCoverMarketInformation/
        public ActionResult frmInstitutionCoverMarketInformation()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Institution Cover Market Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        public ActionResult GetInstitutionInfo(int instCode)
        {
            var InstitutionList = institutionCoverMarketInformationDao.GetInstitutionInfoList(instCode);
            var instituteDetailList = Json(InstitutionList, JsonRequestBehavior.AllowGet);
            instituteDetailList.MaxJsonLength = int.MaxValue;
            return instituteDetailList;
            //return Json(InstitutionList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetMarketInfo()
        {
            var MarketList = institutionCoverMarketInformationDao.GetMarketInfoList();
            return Json(MarketList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult InstitutionMarketInfo(InstituteCoverMarketInformationMasModel master)
        {
            _validationMsg = master.SUStatus == 0 ? institutionCoverMarketInformationDao.Save(master, Session["mappingUserID"].ToString()) : institutionCoverMarketInformationDao.Update(master, Session["mappingUserID"].ToString());
            return Json(new { msg = _validationMsg });

        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSaveMarketInfo(string instituteCode)
        {
            var marketList = institutionCoverMarketInformationDao.GetSaveMarketList(instituteCode);
            return Json(marketList, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult GetInstituteSearchList(string instituteCode)
        public ActionResult GetInstituteSearchList(int searchCode)
        {
            //var instituteList = institutionCoverMarketInformationDao.GetInstituteSearchList(instituteCode);
            var instituteList = institutionCoverMarketInformationDao.GetInstituteSearchList(searchCode);
            var instituteDetailList = Json(instituteList, JsonRequestBehavior.AllowGet);
            instituteDetailList.MaxJsonLength = int.MaxValue;
            return instituteDetailList;
        }


        //Delete all data from retailer collection info 
        [HttpPost]
        public ActionResult DeleteAllData(string model)
        {
            _validationMsg = institutionCoverMarketInformationDao.Delete(model);
            return Json(new { msg = _validationMsg });
        }

        //Delete from retailer collection info through detail id
        [HttpPost]

        public ActionResult DeleteByDtlId(string dtlSlNo)
        {
            _validationMsg = institutionCoverMarketInformationDao.DeleteByDtlId(dtlSlNo);
            return Json(new { msg = _validationMsg });
        }


    }
}