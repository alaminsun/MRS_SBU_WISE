using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class SelectedManufacturerInfoModel : BaseModel
    {
        public string ManufacturerCode { get; set; }
        public string ManufacturerName { get; set; }
        public string MfcNicName { get; set; }
        public int? SlNo { get; set; }
    }
}