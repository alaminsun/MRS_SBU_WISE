using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{

    public class ChangeDocIdController : Controller
    {
        ChangeDocIdDAO changeDocIdDAO = new ChangeDocIdDAO();
        MenuItemsDAO ObjMenuItemsDAO = new MenuItemsDAO();
        DBConnection dbConnection = new DBConnection();
        ValidationMsg _validationMsg = new ValidationMsg();
        //
        // GET: /ChangeDocId/
        public ActionResult frmChangeDocId()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Change Doctor Id";
                ViewBag.USER_ID = Session["USER_ID"].ToString();
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        //[HttpGet]
        //public JsonResult GetSepecializeData()
        //{
        //    var listData = doctorInfoDao.GetSpecializationData();
        //    return Json(listData, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetDesignationData()
        //{
        //    var listData = doctorInfoDao.GetDesignationCategory();
        //    return Json(listData, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult GetDegreeData()
        //{
        //    var listData = doctorInfoDao.GetDegreeInfo();
        //    return Json(listData, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult GetOldDoctorsData(int doctorOldId, string doctorOldName, string Olddegree)
        {
            var listData = changeDocIdDAO.GetOldDoctorList(doctorOldId, doctorOldName, Olddegree);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }

        [HttpPost]
        public JsonResult GetNewDoctorsData(int doctorNewId, string doctorNewName, string Newdegree)
        {
            var listData = changeDocIdDAO.GetNewDoctorList(doctorNewId, doctorNewName, Newdegree);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }

        public ActionResult GetOldPrescrCount(string OldId)
        {
            string data = changeDocIdDAO.GetOldPrescriptioncont(OldId);
            //return Json(new { distName = data });
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetNewPrescrCount(string NewId)
        {
            string data = changeDocIdDAO.GetNewPrescriptioncont(NewId);
            //return Json(new { distName = data });
            return Json(data, JsonRequestBehavior.AllowGet);

        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdateData(ChangeDocIdModel model)
        {
            _validationMsg = changeDocIdDAO.Update(model);
            return Json(new { Code = changeDocIdDAO.GetCode(), msg = _validationMsg });
            //return Json(new { msg = _validationMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string OldId)
        {
            _validationMsg = changeDocIdDAO.Delete(OldId);
            return Json(new { msg = _validationMsg });
        }
        //[HttpPost]
        //public JsonResult GetDoctorRelativeData(int doctorId)
        //{
        //    var listData = doctorInfoDao.GetDoctorRelativeList(doctorId);
        //    return Json(listData, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //public ActionResult Doctor(DoctorInformationModel model)
        //{
        //    _vmMsg = model.SUStatus == 0 ? doctorInfoDao.Save(model, Session["mappingUserID"].ToString()) : doctorInfoDao.Update(model, Session["mappingUserID"].ToString());
        //    return Json(new { Code = doctorInfoDao.GetCode(), msg = _vmMsg });
        //}
        //[HttpPost]
        //public ActionResult DeleteRelative(int relativeId)
        //{
        //    _vmMsg = doctorInfoDao.DeleteRelative(relativeId);
        //    return Json(new { msg = _vmMsg });
        //}
        //[HttpPost]
        //public ActionResult Delete(int doctorId)
        //{
        //    _vmMsg = doctorInfoDao.Delete(doctorId);
        //    return Json(new { msg = _vmMsg });
        //}
    }
}