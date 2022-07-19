using MRS.DAL.DAO;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MRS.Controllers
{
    public class HomeController : Controller
    {
        MenuItemsDAO ObjMenuItemsDAO = new MenuItemsDAO();
        UserCreationDAO dalUserLogin = new UserCreationDAO();
        DBConnection dbConnection = new DBConnection();
        DBHelper db = new DBHelper();
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                int roleId = ObjMenuItemsDAO.GetRoleId(dbConnection.StringRead(), Session["mappingUserID"].ToString());
                Session["roleID"] = roleId;

                if (roleId == 4 || roleId == 5 || roleId == 8 || roleId == 20 || roleId == 11 || roleId == 15)
                {
                    Session["MiniName"] = "IDS";
                    Session["Name"] = "Investment Database System";
                }
                else
                {
                    Session["MiniName"] = "MRS";
                    Session["Name"] = "Market Research System";
                }
                ViewBag.formTitle = Session["Name"].ToString();
                return View();              
            }

            return RedirectToAction("Index", "Login");
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if ((model.UserID != null) && (model.Password != null))
            {
                var verifiedUser = dalUserLogin.CheckAuthirizedUser().Where(m => m.User_ID.Equals(model.UserID) && m.Password.Equals(model.Password)).FirstOrDefault();
                
                if (verifiedUser != null)
                {
                    Session["mappingUserID"] = verifiedUser.UserID;
                    Session["USER_ID"] = verifiedUser.User_ID;
                    Session["PASSWORD"] = verifiedUser.Password;
                    Session["UserId"] = verifiedUser.UserName;

                    int roleId = ObjMenuItemsDAO.GetRoleId(dbConnection.StringRead(), Session["mappingUserID"].ToString());
                    Session["roleID"] = roleId;

                    string Qry = "Select * from SA_COMPANY_INFO";
                    OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(Qry, dbConnection.StringRead());
                    DataTable dt = new DataTable();
                    oracleDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Session["COMPANY_NAME"] = dt.Rows[0]["COMPANY_NAME"].ToString();
                        Session["DEVBY"] = dt.Rows[0]["DEVBY"].ToString();
                        Session["ProjectName"] = "MARKET RESEARCH & PLANNING CELL";
                    }

                    string entryDate = ObjMenuItemsDAO.LastSesionDate(dbConnection.StringRead());
                    Session["LastSessionDate"] = entryDate;

                    string userID = Session["USER_ID"].ToString();
                    if ((userID.ToUpper() != "MRS") && (userID.ToUpper() != "MRS_1") && (userID.ToUpper() != "MRS_2") && (userID.ToUpper() != "MRS_3") && (userID.ToUpper() != "MRS_4") && (userID.ToUpper() != "MRS_5") && (userID.ToUpper() != "MRS_6") && (userID.ToUpper() != "MRS_7") && (userID.ToUpper() != "MRS_8") && (userID.ToUpper() != "MRS_9") && (userID.ToUpper() != "MRS_10") && (userID.ToUpper() != "MRS_20"))
                    {
                        int grpDesigCode = ObjMenuItemsDAO.GetGroupDesgCode(dbConnection.StringRead(), Session["USER_ID"].ToString());
                        Session["GRP_DSIG_CODE"] = grpDesigCode;
                    }
                  

                    if (roleId == 4)
                    {                        
                        return RedirectToAction("frmDashBoardMonthlyInvestment", "DashboardSales");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                    ViewBag.Msg = "Incorrect User Information!";
            }
            return RedirectToAction("Login", "Home");
        }


        public ActionResult LogOut()
        {
            Session["IsLogged"] = false;
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("login", "Home");
        }
        public ActionResult ChangePassword()
        {
            return View();

        }
        [HttpPost]
        public ActionResult ChangePassword(int userID, string password)
        {
            //var count = objUser.ChangePassword(userID, Authenticator.GetHashPassword(password));
            //return count > 0
            //    ? (ActionResult)Json(new { result = "Password has been changed", url = Url.Action("Index", "Home") })
            //    : RedirectToAction("Error", "Home");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LoadMenu(int? id)
        {
            string val = Session["UserId"].ToString();
            object nodes = null;
            if (Session["UserId"] != null)
            {
                
                //var nodes = ObjMenuItemsDAO.GetMenuItemDataTable(Session["roleID"].ToString());//,id
                if (Session["roleID"].ToString() == "4")
                {
                     //ViewBag.name = "IDS";
                    Session["Name"] = "Investment Database System";
                     nodes = ObjMenuItemsDAO.GetMenuItemDataTable("4");//,id
                    //return Json(nodes, JsonRequestBehavior.AllowGet);
                }
                else if (Session["roleID"].ToString() == "5" )
                {
                    //ViewBag.name = "IDS";
                    Session["Name"] = "Investment Database System";
                    nodes = ObjMenuItemsDAO.GetMenuItemDataTable("5");//,id
                }
                else if (Session["roleID"].ToString() == "8")
                {
                    //ViewBag.name = "IDS";
                    Session["Name"] = "Investment Database System";
                    nodes = ObjMenuItemsDAO.GetMenuItemDataTable("8");//,id
                }
                else if (Session["roleID"].ToString() == "11")
                {
                    //ViewBag.name = "IDS";
                    Session["Name"] = "Investment Database System";
                    nodes = ObjMenuItemsDAO.GetMenuItemDataTable("11");//,id
                }
                else if (Session["roleID"].ToString() == "15")
                {
                    //ViewBag.name = "IDS";
                    //Session["Name"] = "IDS";
                    Session["Name"] = "Investment Database System";
                    nodes = ObjMenuItemsDAO.GetMenuItemDataTable("15");//,id
                }
                else if (Session["roleID"].ToString() == "20")
                {
                    //ViewBag.name = "IDS";
                    Session["Name"] = "Investment Database System";
                    nodes = ObjMenuItemsDAO.GetMenuItemDataTable("20");//,id
                }
                else
                {
                     //ViewBag.name = "Market Research System";
                     Session["Name"] = "Market Research System";
                     nodes = ObjMenuItemsDAO.GetMenuItemDataTable(Session["roleID"].ToString());//,id
              
                }           
            }
            return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        //[AcceptVerbs(HttpVerbs.Get)]
        //public ActionResult LoadMenu(int? id)
        //{
        //    if (Session["UserId"] != null)
        //    {
        //        List<MenuItemView> menuItem = ObjMenuItemsDAO.GetMenuItem(ObjMenuItemsDAO.GetMenuItemDataTable(dbConnection.StringRead(), Session["roleID"].ToString(), id));
        //        var nodes = (from n in menuItem
        //                     where (id.HasValue ? n.ParentID == id.Value : n.ParentID == null)
        //                     select n).ToList();
        //        foreach (var node in nodes)
        //        {
        //            string Qry = "Select  MENUID,MenuName from VW_MENUINSCREEN Where ParentID=" + node.ID + "";
        //            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(Qry, dbConnection.StringRead());
        //            DataTable dt = new DataTable();
        //            oracleDataAdapter.Fill(dt);
        //            if (dt.Rows.Count > 0)
        //                node.HasChild = true;
        //            else
        //                node.HasChild = false;
        //        }
        //        return Json(nodes, JsonRequestBehavior.AllowGet);
        //    }
        //    return RedirectToAction("Index", "Login");
        //}


        public ActionResult Error()
        {
            return View();
        }
        public ActionResult SessionOutMsg()
        {
            return View();
        }
    }
}