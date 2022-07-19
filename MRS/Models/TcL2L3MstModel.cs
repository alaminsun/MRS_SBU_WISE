using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class TcL2L3MstModel:BaseModel
    {
        public string TC_L2_CODE { get; set; }
        public string TC_L2_L3_MST_SLNO { get; set; }
        public string TC_L2_DESC { get; set; }
        public List<TcL2L3DtlModel> TcL2L3DtlList { get; set; } 
    }
}