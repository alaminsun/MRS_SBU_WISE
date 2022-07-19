using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class TerritoryMarDtlModel
    {
        public string TERRI_MKT_DTL_SLNO { get; set; }
        public string TERRI_MKT_MAS_SLNO { get; set; }
        public string MARKET_CODE { get; set; }
        public string MARKET_NAME { get; set; }
        public string TERRI_MKT_STATUS { get; set; }

        public string MarketTypeCode { get; set; }

        public string MarketTypeName { get; set; }

        public string SBU_GROUP { get; set; }
    }
}