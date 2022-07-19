using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class TcL1L2DtlModel:BaseModel
    {
        public string TC_L2_CODE { get; set; }
        public string TC_L1_L2_MST_SLNO { get; set; }
        public string TC_L1_L2_DTL_SLNO { get; set; }
        public string TC_L2_DESC { get; set; }
        public string ENTERED_BY { get; set; }
        public DateTime? ENTERED_DATE { get; set; }
        public string ENTERED_TERMINAL { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string UPDATED_TERMINAL { get; set; }

    }
}