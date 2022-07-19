using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class PrescriptionModel : BaseModel
    {
        public long PrescriptionMstSl { get; set; }
        public long SessionSl { get; set; }
        public string CategoryCode { get; set; }
        public string ZoneCode { get; set; }
        public string DivisionCode { get; set; }
        public string RegionCode { get; set; }
        public string TerritoryCode { get; set; }
        public string MarketCode { get; set; }
        public string ZONE_CODE_O { get; set; }
        public string ZONE_NAME_O { get; set; }
        public string DIVISION_CODE_O { get; set; }
        public string DIVISION_NAME_O { get; set; }
        public string REGION_CODE_O { get; set; }
        public string REGION_NAME_O { get; set; }
        public string TERRITORY_CODE_O { get; set; }
        public string TERRITORY_NAME_O { get; set; }
        public string MARKET_CODE_O { get; set; }
        public string MARKET_NAME_O { get; set; }
        public int DoctorId { get; set; }
        public int InstituteCode { get; set; }
        public List<PrescriptionDetailsModel> Details { get; set; }
    }
    public class PrescriptionDetailsModel
    {
        public long PrescriptionDtlSl { get; set; }
        public long PrescriptionMstSl { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}