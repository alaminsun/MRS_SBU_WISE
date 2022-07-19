using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class DistrictUpazilaMstModel:BaseModel
    {
        public string DISTC_CODE { get; set; }//# SEID-129761.
        public string DISTC_UPAZILA_MAS_SLNO { get; set; }
        public string DISTC_NAME { get; set; }
        public List<DistrictUpazilaDtlModel> DistrictUpazilaDtlList { get; set; } 
    }
}