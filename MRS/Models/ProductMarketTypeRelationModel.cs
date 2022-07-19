using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ProductMarketTypeRelationMasModel : BaseModel
    {
        public string ProductMarketTypeMstSl { get; set; }
        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        //public string DosageForm { get; set; }

        //public string DosageStrength { get; set; }

        public string PackSize { get; set; }

        public string GenericCode { get; set; }

        public string GenericName { get; set; }

        public string DosageFormCode { get; set; }

        public string DosageFormName { get; set; }

        public string DosageStrengthName { get; set; }

        public string DosageStrengthCode { get; set; }
        public virtual IList<ProductMarketTypeRelationDtlModel> ProductMarketList { get; set; }

    }

    public class ProductMarketTypeRelationDtlModel : BaseModel
    {
        public string ProductMarketTypeDtlSl { get; set; }
        public string ProductMarketTypeMstSl { get; set; }
        public string MarketTypeCode { get; set; }
        public string MarketTypeName { get; set; }
        public string ProductGroup { get; set; }

    }
}