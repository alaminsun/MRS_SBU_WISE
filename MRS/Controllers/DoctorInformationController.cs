using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class DoctorInformationController : Controller
    {
        DoctorInformationDAO doctorInfoDao = new DoctorInformationDAO();
        ValidationMsg _vmMsg = new ValidationMsg();
        public ActionResult FrmDoctorInformation()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Doctor Information";
                ViewBag.USER_ID = Session["USER_ID"].ToString();
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [HttpGet]
        public JsonResult GetSepecializeData()
        {
            var listData = doctorInfoDao.GetSpecializationData();
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDesignationData()
        {
            var listData = doctorInfoDao.GetDesignationCategory();
            return Json(listData, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDegreeData()
        {
            var listData = doctorInfoDao.GetDegreeInfo();
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDoctorsData(int doctorId, string doctorName, string degree)
        {
            var listData = doctorInfoDao.GetDoctorList(doctorId, doctorName, degree);
            var data = Json(listData, JsonRequestBehavior.AllowGet);
            data.MaxJsonLength = int.MaxValue;
            return data;
        }

        [HttpPost]
        public JsonResult GetDoctorRelativeData(int doctorId)
        {
            var listData = doctorInfoDao.GetDoctorRelativeList(doctorId);
            return Json(listData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Doctor(DoctorInformationModel model)
        {
            _vmMsg = model.SUStatus == 0 ? doctorInfoDao.Save(model, Session["mappingUserID"].ToString()) : doctorInfoDao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { Code = doctorInfoDao.GetCode(), msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeleteRelative(int relativeId)
        {
            _vmMsg = doctorInfoDao.DeleteRelative(relativeId);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult Delete(int doctorId)
        {
            _vmMsg = doctorInfoDao.Delete(doctorId);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeleteDetail(int slNo)
        {
            _vmMsg = doctorInfoDao.DeleteDetail(slNo);
            return Json(new { msg = _vmMsg });
        }
    }
}