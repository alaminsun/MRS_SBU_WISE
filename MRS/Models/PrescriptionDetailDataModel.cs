using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class PrescriptionDetailDataModel
    {
        public string PrescDetSlno { get; set; }
        public string PrescMasSlno { get; set; }
        public string ProdId { get; set; }
        public string PurchaseQty { get; set; }
        public string UnitPrice { get; set; }
        public string IndiCode { get; set; }
        public string Remarks { get; set; }
    }
}