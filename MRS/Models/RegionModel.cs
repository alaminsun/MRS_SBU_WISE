using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class RegionModel : BaseModel
    {
        public string RegionCode { get; set; }
        public string SAPRegionCode { get; set; }
        public string RegionName { get; set; }
        public string SlNo { get; set; }
    }
}