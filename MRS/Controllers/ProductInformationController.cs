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
	public class ProductInformationController : Controller
	{

		private readonly ProductInformationDAO _productInformationDao = new ProductInformationDAO();
		ValidationMsg _validationMsg = new ValidationMsg();

		public ActionResult frmProductInformation()
		{
			if (Session["UserId"] != null)
			{
				//ViewBag.CanDisplay = true; 
				ViewBag.formTitle = "Product Information";
				ViewBag.USER_ID = Session["USER_ID"].ToString();
				return View();
			}
			return RedirectToAction("Login", "Home");
		}


		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetProductInformationList()
		{
			var data = _productInformationDao.GetProductInformationList().OrderByDescending(m => m.PROD_ID);
			var productDetailList = Json(data, JsonRequestBehavior.AllowGet);
			productDetailList.MaxJsonLength = System.Int32.MaxValue;// int.MaxValue;
			return productDetailList;
			//return Json(data, JsonRequestBehavior.AllowGet);
		}



		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult SaveAndUpdateData(ProductInformationModel model)
		{
			_validationMsg = model.SUStatus == 0 ? _productInformationDao.Save(model, Session["mappingUserID"].ToString()) : _productInformationDao.Update(model, Session["mappingUserID"].ToString());
			return Json(new { Code = _productInformationDao.GetCode(), msg = _validationMsg });
			//return Json(new { msg = _validationMsg });
		}


		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult Delete(string productId)
		{
			_validationMsg = _productInformationDao.Delete(productId);
			return Json(new { msg = _validationMsg });
		}



		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetDosageFormInformation()
		{
			var data = _productInformationDao.GetDosageFormInformationData().OrderByDescending(m => m.DOSAGE_FORM_CODE);
			return Json(data, JsonRequestBehavior.AllowGet);

		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetDosageStrengthInformation()
		{
			var data = _productInformationDao.GetDosageStrengthInformation().OrderByDescending(m => m.DSG_STRENGTH_CODE);
			return Json(data, JsonRequestBehavior.AllowGet);

		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetGenericInformation()
		{
			var data = _productInformationDao.GetGenericInformationData().OrderBy(m => m.GENERIC_CODE);
			return Json(data, JsonRequestBehavior.AllowGet);

		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetAllManufacturerList()
		
		{
			var allData = _productInformationDao.GetAllManufacturerList();
			return Json(allData, JsonRequestBehavior.AllowGet);
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetTherapeuticLevel1List()
		{
			var data = _productInformationDao.GetTherapeuticLevel1List().OrderByDescending(m => m.TC_L1_CODE);
			return Json(data, JsonRequestBehavior.AllowGet);

		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetTherapeuticLevel2List()
		{
			var data = _productInformationDao.GetTherapeuticLevel2List().OrderByDescending(m => m.TC_L2_CODE);
			return Json(data, JsonRequestBehavior.AllowGet);

		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetTherapeuticLevel3List()
		{
			var data = _productInformationDao.GetTherapeuticLevel3List().OrderByDescending(m => m.TC_L3_CODE);
			return Json(data, JsonRequestBehavior.AllowGet);

		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult GetTherapeuticLevel4List()
		{
			var data = _productInformationDao.GetTherapeuticLevel4List().OrderByDescending(m => m.TC_L4_CODE);
			return Json(data, JsonRequestBehavior.AllowGet);

		}


	}
}