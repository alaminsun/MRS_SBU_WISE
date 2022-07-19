using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class OTCInfoModel : BaseModel
    {
        public long OT_PRESC_MAS_SLNO { get; set; }
        public long SessionSl { get; set; }
        public string OT_PRESC_CAT_CODE { get; set; }
        public string OT_MARKET_CODE { get; set; }
        public string OT_MARKET_NAME { get; set; }
        public string EntryDate { get; set; }
        public string PurchaseDate { get; set; }
        public List<OTCInfoDetailModel> OTCInfoDetailModels { get; set; }


    }
    public class OTCInfoDetailModel
    {
        public long OT_PRESC_DET_SLNO { get; set; }
        public long OT_PRESC_MAS_SLNO { get; set; }
        public string PROD_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public int PURCHASE_QTY { get; set; }
        public string DOSAGE_FORM_CODE { get; set; }
        public string DSG_STRENGTH_CODE { get; set; }
        public decimal UNIT_PRICE { get; set; }
        public string DOSAGE_FORM_NAME { get; set; }
        public string DSG_STRENGTH_NAME { get; set; }
    }
}