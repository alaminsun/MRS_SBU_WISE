using MRS.DAL.DAO;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class TcL3L4Controller : Controller
    {
        private readonly TcL3L4DAO _tcL3L4Dao;
        private ValidationMsg _vmMsg;

        public TcL3L4Controller()
        {
            _vmMsg = new ValidationMsg();
            _tcL3L4Dao = new TcL3L4DAO();
        }
        public ActionResult FrmTcL3L4()
        {
            if (Session["UserId"] != null)
            {
                ViewBag.formTitle = "Therapeutic Class Level-3 and Therapeutic Class Level-4 Relation";
                return View();
            }
            return RedirectToAction("Login", "Home");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTcL3L4MstData()
        {
            var tcL3L4List = _tcL3L4Dao.GetTcL3L4MstList();
            return Json(tcL3L4List, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTcL3L4DtlData()
        {
            var tcL3L4List = _tcL3L4Dao.GetTcL3L4DtlList();
            return Json(tcL3L4List, JsonRequestBehavior.AllowGet);
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveAndUpdate(TcL3L4MstModel model)
        {
            _vmMsg = model.SUStatus == 0 ? _tcL3L4Dao.Save(model, Session["mappingUserID"].ToString()) : _tcL3L4Dao.Update(model, Session["mappingUserID"].ToString());
            return Json(new { msg = _vmMsg });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetTcL3L4SearchMasterList()
        {
            var tcL1L2SearchList = _tcL3L4Dao.GetSearchMasterList();
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetTcL3L4SearchDetailList(string tcL3L4MstSlNo)
        {
            var tcL1L2SearchList = _tcL3L4Dao.GetSearchDtliList(tcL3L4MstSlNo);
            return Json(tcL1L2SearchList, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteAllList(string tcL3L4MstSlNo)
        {
            _vmMsg = _tcL3L4Dao.Delete(tcL3L4MstSlNo);
            return Json(new { msg = _vmMsg });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult DeleteDetailList(string tcL3L4DtlSlNo)
        {
            _vmMsg = _tcL3L4Dao.DeleteDetailData(tcL3L4DtlSlNo);
            return Json(new { msg = _vmMsg });
        }

	}
}