using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class MarketSegmentProductMasModel : BaseModel
    {
        public string MKT_SEGM_PROD_MAS_SLNO { get; set; }
        public string MKT_SEGMENT_CODE { get; set; }
        public string MKT_SEGMENT_NAME { get; set; }
        public virtual IList<MarketSegmentProductDtlModel> MktSegmentList { get; set; }
    }
}