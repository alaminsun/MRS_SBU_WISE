using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class UserWorkingSessionModel// : BaseModel
    {
        public string SessionSlno { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string DataSource { get; set; }
        public string Remarks { get; set; }
    }
}