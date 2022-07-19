using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class MarketTypeSegmentMasModel : BaseModel
    {
        public string MKT_TYPE_SEGM_MAS_SLNO { get; set; }
        public string MKT_TYPE_CODE { get; set; }
        public string MKT_TYPE_NAME { get; set; }
        public virtual IList<MarketTypeSegmentDtlModel> MktSegmentList { get; set; }
    }
}