using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ZoneDiviMasModel : BaseModel
    {
        public string ZONE_DIVI_MAS_SLNO { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAME { get; set; }
        public virtual IList<ZoneDiviDtlModel> DivisonList { get; set; }
    }
}