using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class TcL2L3Controller : Controller
    {
        TcL2L3DAO tcL2L3Dao;
        private ValidationMsg _vmMsg;
        public TcL2L3Controller()
        {
            _vmMsg = new ValidationMsg();
            tcL2L3Dao = new TcL2L3DAO();
        }
        public ActionResult FrmTcL2L3()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Therapeutic Class Level-2 and Therapeutic Class Level-3 Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTcL2L3MstData()
        {
            var tcL2L3List = tcL2L3Dao.GetTcL2L3MstList();
            return Json(tcL2L3List, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTcL2L3DtlData()
        {
            var tcL2L3List = tcL2L3Dao.GetTcL2L3DtlList();
            return Json(tcL2L3List, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdate(TcL2L3MstModel model)
        {
            _vmMsg = model.SUStatus == 0 ? tcL2L3Dao.Save(model, Session["mappingUserID"].ToString()) : tcL2L3Dao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTcL2L3SearchMasterList()
        {
            var tcL1L2SearchList = tcL2L3Dao.GetSearchMasterList();
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTcL2L3SearchDetailList(string tcL2L3MstSlNo)
        {
            var tcL1L2SearchList = tcL2L3Dao.GetSearchDtliList(tcL2L3MstSlNo);
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAllList(string tcL2L3MstSlNo)
        {
            _vmMsg = tcL2L3Dao.Delete(tcL2L3MstSlNo);
            return Json(new { msg = _vmMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDetailList(string tcL2L3DtlSlNo)
        {
            _vmMsg = tcL2L3Dao.DeleteDetailData(tcL2L3DtlSlNo);
            return Json(new { msg = _vmMsg });
        }


	}
}