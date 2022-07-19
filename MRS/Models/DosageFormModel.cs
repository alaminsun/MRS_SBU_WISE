using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class DosageFormModel:BaseModel
    {
        public string DOSAGE_FORM_CODE { get; set; }
        public string DOSAGE_FORM_NAME { get; set; }
        public string DF_ODR_SLNO { get; set; }
        public string ENTERED_BY { get; set; }
        public DateTime? ENTERED_DATE { get; set; }
        public string ENTERED_TERMINAL { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }
        public string UPDATED_TERMINAL { get; set; }
    }
}