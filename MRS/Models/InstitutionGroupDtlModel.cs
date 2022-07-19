using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class InstitutionGroupDtlModel:BaseModel
    {
        public string INSTI_GRP_MAS_SLNO { get; set; }
        public string INSTI_CODE { get; set; }
        public string INSTI_NAME { get; set; }
        public string INSTI_GRP_DET_SLNO { get; set; }
        public string ADDRESS { get; set; }
        public string INSTI_TYPE_CODE { get; set; }
        public string INSTI_TYPE_NAME { get; set; }
    }
}