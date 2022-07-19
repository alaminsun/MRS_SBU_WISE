using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class RegionTerriMasModel : BaseModel
    {
        public string REGI_TERRI_MAS_SLNO { get; set; }
        public string REGION_CODE { get; set; }
        public string REGION_NAME { get; set; }
        public virtual IList<RegionTerriDtlModel> TerritoryList { get; set; }
    }
}