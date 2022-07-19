using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class SCPInstitutionInfoModel
    {
        public int SDII_ID { get; set; }
        public string INSTI_CODE { get; set; }
        public string InstitutionName { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
    }
}