using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class OrganizationController : Controller
    {
        OrganizationDAO organizations = new OrganizationDAO();
        ValidationMsg _vmMsg;
        //public ActionResult frmOrganization()
        //{
        //    return View();
        //}

        public ActionResult frmOrganization()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Organization Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
        public JsonResult InstitutionList()
        {
            var result = organizations.GetInstitutes();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult MarketList(string prescNo)
        {
            var result = organizations.GetMarkets(prescNo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrgcMarketList()
        {
            var marketList = organizations.GetOrgMarketList();
            return Json(marketList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Organization(PrescriptionModel model)
        {
            _vmMsg = model.SUStatus == 0 ? organizations.Save(model) : organizations.Update(model);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public JsonResult SeesionInstituteList(int sessionSl)
        {
            var result = organizations.GetSessionInstitutes(sessionSl);
            return Json(result);
        }

        [HttpPost]
        public JsonResult GetSessionProduct(int instituteId)
        {
            var result = organizations.GetSearchProduct(instituteId);
            return Json(result);
        }

        [HttpPost]
        public ActionResult Delete(long sl)
        {
            _vmMsg = organizations.DeleteData(sl);
            return Json(new { msg = _vmMsg });
        }

        [HttpPost]
        public ActionResult DeleteMaster(long sessionSl, int instituteCode)
        {
            _vmMsg = organizations.Delete(sessionSl, instituteCode);
            return Json(new { msg = _vmMsg });
        }
    }
}