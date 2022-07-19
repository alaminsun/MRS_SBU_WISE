using MRS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class InstituteCoverMarketInformationDtlModel : BaseModel
    {
        public long INSTI_MKT_DET_SLNO { get; set; }
        public long INSTI_MKT_MAS_SLNO { get; set; }
        public string MARKET_CODE { get; set; }
        public string MARKET_NAME { get; set; }

        public string SBU_GROUP { get; set; }
    }
}