using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class InstitutionInformationController : Controller
    {
        private readonly InstitutionInformationDAO _institutionInformationDao = new InstitutionInformationDAO();
        ValidationMsg _validationMsg = new ValidationMsg();

        //
        // GET: /InstitutionInformation/
        public ActionResult frmInstitutionInformation()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Institute Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetInstituteTypeList()
        {
            var data = _institutionInformationDao.GetInstituteTypeList().OrderByDescending(m => m.INSTI_TYPE_CODE);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetUpazilaList()
        {
            List<UpazilaModel> list = _institutionInformationDao.GetDataList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDistrictName(string Upazilacode)
        {
            string data = _institutionInformationDao.GetDistrictName(Upazilacode);
            //return Json(new { distName = data });
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(InstitutionInformationModel model)
        {
            _validationMsg = model.SUStatus == 0 ? _institutionInformationDao.Save(model, Session["mappingUserID"].ToString()) : _institutionInformationDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = _institutionInformationDao.GetCode(), msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetInstituteformation()
        {
            var data = _institutionInformationDao.GetInstituteformationList().OrderByDescending(m => m.INSTI_CODE);
            var instituteDetailList = Json(data, JsonRequestBehavior.AllowGet);
            instituteDetailList.MaxJsonLength = int.MaxValue;
            return instituteDetailList;

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string instituteCode)
        {
            _validationMsg = _institutionInformationDao.Delete(instituteCode);
            return Json(new { msg = _validationMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetSuggestInsInfoList()
        {
            var allData = _institutionInformationDao.GetSuggestInsInfoList();
            return Json(allData, JsonRequestBehavior.AllowGet);
        }
    }
}