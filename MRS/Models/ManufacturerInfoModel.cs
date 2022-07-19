using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ManufacturerInfoModel : BaseModel
    {
        public string IsSelect { get; set; }
        public string ManufacturerCode { get; set; }
        public string ManufacturerName { get; set; }
        public string MfcNicName { get; set; }
        public string SlNo { get; set; }
        public string Status { get; set; }
        public List<ManufacturerInfoModel> SelectedManufacturerList { get; set; }
    }
}