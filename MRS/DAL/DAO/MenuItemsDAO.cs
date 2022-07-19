using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
    public class MenuItemsDAO
    {
        private readonly DBConnection dbConnection = new DBConnection();


        public int GetRoleId(string Conn, string UserID)
        {
            int RoleID = 0;
            string Qry = "Select  USERID, ROLEID from Sa_USERINROLES Where USERID=" + UserID + "";
            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(Qry, Conn);
            DataTable dt = new DataTable();
            oracleDataAdapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                RoleID = Convert.ToInt16(dt.Rows[0]["ROLEID"].ToString());
            }

            return RoleID;
        }

        public int GetGroupDesgCode(string Conn, string UserID)
        {
            int grpDesigCode = 0;
            string Qry = "Select MGMT_CODE,GRP_DSIG_CODE from MANAGEMENT_TEAM Where MGMT_CODE = " + UserID + "";
            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(Qry, Conn);
            DataTable dt = new DataTable();
            oracleDataAdapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                grpDesigCode = Convert.ToInt16(dt.Rows[0]["GRP_DSIG_CODE"].ToString());
            }

            return grpDesigCode;
        }

        public string LastSesionDate(string Conn)
        {
            string sessionCode = "";
            string Qry = "SELECT TO_CHAR(max( ENTRY_DATE ),'dd/MM/yyyy') ENTRY_DATE from USER_WORKING_SESSION ";
            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(Qry, Conn);
            DataTable dt = new DataTable();
            oracleDataAdapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                sessionCode = dt.Rows[0]["ENTRY_DATE"].ToString();
            }

            return sessionCode;
        }

        public List<MenuItemView> GetMenuItemDataTable(string roleId)//, int? parentID
        {
            List<MenuItemView> listMenuItemViews = new List<MenuItemView>();
            //string Qry = "Select  MENUID,MenuName,URL,ITEMORDER,NVL(ltrim(rtrim(PARENTID)),0) PARENTID,ROLEID,SETBY from VW_MENUINSCREEN Where ROLEID=" + roleId + " ";
            string Qry = "Select  MENUID,MenuName,URL,ITEMORDER,NVL(ltrim(rtrim(PARENTID)),0) PARENTID,ROLEID,SETBY from VW_MENUINSCREEN Where ROLEID=" + roleId + " order by to_number(ITEMORDER)";

            using (OracleConnection Conn = new OracleConnection(dbConnection.StringRead()))
            {
                OracleCommand oracleCommand = new OracleCommand(Qry, Conn);
                Conn.Open();
                OracleDataReader oracleDataReader = oracleCommand.ExecuteReader();
                while (oracleDataReader.Read())
                {
                    MenuItemView menuItemView = new MenuItemView();
                    menuItemView.ID = Convert.ToInt16(oracleDataReader["MENUID"]);
                    menuItemView.MenuName = oracleDataReader["MenuName"].ToString();
                    menuItemView.ParentID = Convert.ToInt16(oracleDataReader["PARENTID"].ToString()) == 0
                       ? default(int?)
                       : Convert.ToInt16(oracleDataReader["PARENTID"].ToString()); // oracleDataReader["PARENTID"] != DBNull.Value ? Convert.ToInt32(oracleDataReader["PARENTID"]) : (int?)null;
                    //menuItemView.Link = "" + "/" + oracleDataReader["URL"].ToString().Substring(3) + "/" +
                    //                    oracleDataReader["URL"].ToString();//"" + "/" + n.URL 
                    menuItemView.Link = "" + "/" + oracleDataReader["URL"].ToString();
                    menuItemView.HasChild = true;
                    menuItemView.ItemOrder = Convert.ToInt16(oracleDataReader["ITEMORDER"].ToString());

                    listMenuItemViews.Add(menuItemView);
                }
            }

            List<MenuItemView> parentMenuTree = GetMenuTreeList(listMenuItemViews, null);
            return parentMenuTree.ToList();
        }

        public List<MenuItemView> GetMenuTreeList(List<MenuItemView> list, int? parentID)
        {
            return list.Where(x => x.ParentID == parentID).Select(x => new MenuItemView()
            {
                ID = x.ID,
                MenuName = x.MenuName,
                ParentID = x.ParentID,
                Link = x.Link,
                HasChild = x.HasChild,
                ItemOrder = x.ItemOrder,
                MenuItemList = GetMenuTreeList(list, x.ID)
            }).ToList();
        }



        public int GetMappingUserID(string Conn, string UserID)
        {
            int ID = 0;
            string Qry = "Select  NVL(USERID,0) USERID from USER_LOGIN_INFO Where USER_ID='" + UserID + "'";
            OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(Qry, Conn);
            DataTable dt = new DataTable();
            oracleDataAdapter.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ID = Convert.ToInt16(dt.Rows[0]["USERID"].ToString());
            }

            return ID;
        }
    }
}