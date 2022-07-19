using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ProductWiseInvestmentModel
    {
        public string MARKET_CODE { get; set; }
        public string DOCTOR_ID { get; set; }
        public string PRODUCT_CODE { get; set; }
        public string COMMITMENT { get; set; }
        public string DURATION_FROM { get; set; }
        public string DURATION_TO { get; set; }
        public string DATA_REMARKS { get; set; }
        
    }
}