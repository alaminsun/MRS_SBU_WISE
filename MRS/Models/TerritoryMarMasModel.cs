using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class TerritoryMarMasModel : BaseModel
    {
        public string TERRI_MKT_MAS_SLNO { get; set; }
        public string TERRITORY_CODE { get; set; }
        public string TERRITORY_NAME { get; set; }
        public virtual IList<TerritoryMarDtlModel> MarketList { get; set; }
    }
}