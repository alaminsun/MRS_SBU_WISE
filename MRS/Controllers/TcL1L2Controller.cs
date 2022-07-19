using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class TcL1L2Controller : Controller
    {
        TcL1L2DAO tcL1L2Dao;
        private ValidationMsg _vmMsg;

        public TcL1L2Controller()
        {
            _vmMsg = new ValidationMsg();
            tcL1L2Dao = new TcL1L2DAO();
        }
        public ActionResult FrmTcL1L2()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Therapeutic Class Level-1 and Therapeutic Class Level-2 Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTcL1L2MstData()
        {
            var tcL1L2List = tcL1L2Dao.GetTcL1L2MstList();
            return Json(tcL1L2List, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTcL1L2DtlData()
        {
            var tcL1L2List = tcL1L2Dao.GetTcL1L2DtlList();
            return Json(tcL1L2List, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdate(TcL1L2MstModel model)
        {
            _vmMsg = model.SUStatus == 0 ? tcL1L2Dao.Save(model, Session["mappingUserID"].ToString()) : tcL1L2Dao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTcL1L2SearchMasterList()
        {
            var tcL1L2SearchList = tcL1L2Dao.GetSearchMasterList();
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTcL1L2SearchDetailList(string tcL1L2MstSlNo)
        {
            var tcL1L2SearchList = tcL1L2Dao.GetSearchDtliList(tcL1L2MstSlNo);
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAllList(string tcL1L2MstSlNo)
        {
            _vmMsg = tcL1L2Dao.Delete(tcL1L2MstSlNo);
            return Json(new { msg = _vmMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDetailList(string tcL1L2DtlSlNo)
        {
            _vmMsg = tcL1L2Dao.DeleteDetailData(tcL1L2DtlSlNo);
            return Json(new { msg = _vmMsg });
        }
    
    }
}