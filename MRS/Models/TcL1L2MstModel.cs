using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class TcL1L2MstModel:BaseModel
    {
        public string TC_L1_CODE { get; set; }//# SEID-129761.
        public string TC_L1_L2_MST_SLNO { get; set; }
        public string TC_L1_DESC { get; set; }
        public List<TcL1L2DtlModel> TcL1L2DtlList { get; set; } 
    }
}