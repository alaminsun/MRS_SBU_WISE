using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class DivisonModel : BaseModel
    {
        public string DivisonCode { get; set; }
        public string SAPDivisonCode { get; set; }
        public string DivisonName { get; set; }
        public string SlNo { get; set; }
    }
}