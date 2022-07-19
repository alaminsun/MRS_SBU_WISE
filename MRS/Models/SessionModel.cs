using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class SessionModel : BaseModel
    {
        public string SessionSl { get; set; }
        public string EntryDate { get; set; }
        public string PurchaseDate { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string DataSource { get; set; }
        public string Tag { get; set; }
        public int PRESC_MAS_SLNO { get; set; }
    }
}