using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ChangePasswordModel
    {
        public string UserID { get; set; }
        public string User_ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}