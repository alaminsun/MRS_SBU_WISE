using MRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Controllers
{
    public class InstituteCoverMarketInformationMasModel : BaseModel
    {
        public long INSTI_MKT_MAS_SLNO { get; set; }
        public long INSTI_MKT_MAS_SLNO_1 { get; set; }
        public string INSTI_CODE { get; set; }
        public string INSTI_NAME { get; set; }
        public string INSTI_TYPE_CODE { get; set; }
        public string INSTI_TYPE_NAME { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string ADDRESS3 { get; set; }
        public string ADDRESS4 { get; set; }
        public string IsInsert { get; set; }
        public List<InstituteCoverMarketInformationDtlModel> MarketList { get; set; }

    }
}