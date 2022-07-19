using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class DepoMarMasModel : BaseModel
    {
        public string DEPOT_MKT_MAS_SLNO { get; set; }
        public string DEPOT_CODE { get; set; }
        public string DEPOT_NAME { get; set; }
        public virtual IList<DepoMarDtlModel> MarketList { get; set; }
    }
}