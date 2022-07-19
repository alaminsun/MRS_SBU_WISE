using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class DivisionRegiMasModel : BaseModel
    {
        public string DIVI_REGI_MAS_SLNO { get; set; }
        public string DIVISION_CODE { get; set; }
        public string DIVISION_NAME { get; set; }
        public virtual IList<DivisionRegiDtlModel> RegionList { get; set; }
    }
}