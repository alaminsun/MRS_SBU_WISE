using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class PrescriptionInformationController : Controller
    {
        public ActionResult frmPrescriptionInformation()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Prescription Information";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }
    }
}