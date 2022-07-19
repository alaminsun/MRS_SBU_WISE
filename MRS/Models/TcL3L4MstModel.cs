using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class TcL3L4MstModel:BaseModel
    {
        public string TC_L3_CODE { get; set; }
        public string TC_L3_L4_MST_SLNO { get; set; }
        public string TC_L3_DESC { get; set; }
        public List<TcL3L4DtlModel> TcL3L4DtlList { get; set; } 
    }
}