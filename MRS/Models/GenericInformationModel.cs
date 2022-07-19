using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class GenericInformationModel:BaseModel
    {
        public string GENERIC_CODE { get; set; }
        public string GENERIC_NAME { get; set; }
        public string GEN_ORD_SLNO { get; set; }
        public string ENTERED_BY { get; set; }
        public DateTime? ENTERED_DATE { get; set; }
        public string ENTERED_TERMINAL { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string UPDATED_TERMINAL { get; set; }
    }
}