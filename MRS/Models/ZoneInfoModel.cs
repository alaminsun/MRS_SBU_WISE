using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ZoneInfoModel : BaseModel
    {
        public string ZoneCode { get; set; }
        public string SAPZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string SlNo { get; set; }
        public virtual IList<DivisonModel> DivisonList { get; set; }
    }
}