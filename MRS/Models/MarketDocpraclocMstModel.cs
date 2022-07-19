using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class MarketDocpraclocMstModel:BaseModel
    {
        public string MARKET_CODE { get; set; }//# SEID-129761.
        public string MKT_DP_LOC_MAS_SLNO { get; set; }
        public string MARKET_NAME { get; set; }
        public List<MarketDocpraclocDtlModel> MarketDocpraclocDtlList { get; set; } 
    }
}