using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace MRS.Models
{
    public class MenuItemView
    {
        public int? ID { get; set; }
        public int? MENUID { get; set; }
        public string URL { get; set; }
        public string Link { get; set; }
        public int ItemOrder { get; set; }
        public int? ParentID { get; set; }
        public string MenuName { get; set; }
        public int SetBy { get; set; }
        public bool HasChild { get; set; }
        public List<MenuItemView> MenuItemList { get; set; }

    }
}