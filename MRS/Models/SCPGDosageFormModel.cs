using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class SCPGDosageFormModel
    {
        public int DF_ID { get; set; }
        public int? SCG_ID { get; set; }
        public string DOSAGE_FORM_CODE { get; set; }
        public string DOSAGE_FORM_NAME { get; set; }
    }
}