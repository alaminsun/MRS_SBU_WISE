using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ProductInformationModel:BaseModel
    {
        public string PROD_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string TC_L1_CODE { get; set; }
       
        public string DOSAGE_FORM_NAME { get; set; }
        public string DSG_STRENGTH_NAME { get; set; }
        public string GENERIC_NAME { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public string MFC_NIC_NAME { get; set; }
        public string TC_L1_DESC { get; set; }
        public string TC_L2_DESC { get; set; }
        public string TC_L3_DESC { get; set; }
        public string TC_L4_DESC { get; set; }
        public string TC_L2_CODE { get; set; }
        public string TC_L3_CODE { get; set; }
        public string TC_L4_CODE { get; set; }
        public string GENERIC_CODE { get; set; }
        public string DOSAGE_FORM_CODE { get; set; }
        public string DSG_STRENGTH_CODE { get; set; }
        public string MANUFACTURER_CODE { get; set; }
        public string UNIT_PRICE { get; set; }
        public string AVG_PRESCRIBE_QTY { get; set; }
        public string AVG_BOUGHT_QTY { get; set; }
        public string PACK_SIZE { get; set; }
        public string SAP_CODE { get; set; }
        public string PROD_STATUS { get; set; }
        public string ENTERED_BY { get; set; }
        public string ENTERED_DATE { get; set; }
        public string ENTERED_TERMINAL { get; set; }
        public string UPDATED_BY { get; set; }
        public string UPDATED_DATE { get; set; }
        public string UPDATED_TERMINAL { get; set; }


        public string SBU_GROUP { get; set; }
    }
}