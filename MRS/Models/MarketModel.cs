using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class MarketModel : BaseModel
    {
        public string MarketCode { get; set; }
        public string SAPMarketCode { get; set; }
        public string MarketName { get; set; }
        public string SlNo { get; set; }

        public string MarketTypeName { get; set; }

        public string MarketTypeCode { get; set; }

        public string ddlMarketType { get; set; }

        public string SBU_GROUP { get; set; }
    }
}