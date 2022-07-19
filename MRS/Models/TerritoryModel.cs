using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class TerritoryModel : BaseModel
    {
        public string TerritoryCode { get; set; }
        public string SAPTerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string SlNo { get; set; }
    }
}