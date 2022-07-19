using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class SpecialCamProdConfigController : Controller
    {
        SpecCamProdConfigDAO _dalSpecCamProdConfigDAO;
        private ValidationMsg _vmMsg;

        public SpecialCamProdConfigController()
        {
            _vmMsg = new ValidationMsg();
            _dalSpecCamProdConfigDAO = new SpecCamProdConfigDAO();
        }
        //
        // GET: /SpecialCamProdConfig/
        public ActionResult SpecialCamProdConfig()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Special Campaign Product Configuration";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        [HttpPost]
        public ActionResult SpecialCamProdConfig(SpecCamProdConfigModel model)
        {
            _vmMsg = model.SC_ID == 0 ? _dalSpecCamProdConfigDAO.Save(model, Session["USER_ID"].ToString()) : _dalSpecCamProdConfigDAO.Update(model, Session["USER_ID"].ToString());
            return Json(new { msg = _vmMsg }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAll()
        {
            var data = _dalSpecCamProdConfigDAO.GetAll(0);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetGenericItemList(string SC_ID)
        {
            int scId = Convert.ToInt32(SC_ID);

            var dataGen = _dalSpecCamProdConfigDAO.GetGenericItemList(scId);
            int scgId = Convert.ToInt32(dataGen.Where(o => o.SC_ID == scId).FirstOrDefault().SCG_ID);
            var dataDos = _dalSpecCamProdConfigDAO.GetDosageItemList(scgId);
            if (dataDos.Any())
            {
                var dataStn = _dalSpecCamProdConfigDAO.GetStrengthItemList(Convert.ToInt32(dataDos.Where(o => o.SCG_ID == scgId).FirstOrDefault().DF_ID));
                return Json(new { Generic = dataGen, Dosage = dataDos, Strength = dataStn }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Generic = dataGen, Dosage = dataDos, Strength = "" }, JsonRequestBehavior.AllowGet);

            }


        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetDosageAndStrengthList(string SCG_ID)
        {
            int scgId = Convert.ToInt32(SCG_ID);
            var dataDos = _dalSpecCamProdConfigDAO.GetDosageItemList(scgId);
            if (dataDos.Any())
            {
                var dataStn = _dalSpecCamProdConfigDAO.GetStrengthItemList(Convert.ToInt32(dataDos.Where(o => o.SCG_ID == scgId).FirstOrDefault().DF_ID));
                return Json(new { Dosage = dataDos, Strength = dataStn }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Dosage = dataDos, Strength = "" }, JsonRequestBehavior.AllowGet);

            }
            
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetStrengthList(string DF_ID)
        {
            int dfId = Convert.ToInt32(DF_ID);
            var dataStn = _dalSpecCamProdConfigDAO.GetStrengthItemList(dfId);
            return Json(dataStn, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(string SC_ID)
        {
            _vmMsg = _dalSpecCamProdConfigDAO.Delete(Convert.ToInt32(SC_ID));
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteGenericGrid(string SCG_ID)
        {
            _vmMsg = _dalSpecCamProdConfigDAO.DeleteGenericGrid(Convert.ToInt32(SCG_ID));
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDosageGrid(string DF_ID)
        {
            _vmMsg = _dalSpecCamProdConfigDAO.DeleteDosageGrid(Convert.ToInt32(DF_ID));
            return Json(new { msg = _vmMsg });
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteStrengthGrid(string DFS_ID)
        {
            _vmMsg = _dalSpecCamProdConfigDAO.DeleteStrengthGrid(Convert.ToInt32(DFS_ID));
            return Json(new { msg = _vmMsg });
        }

    }
}
