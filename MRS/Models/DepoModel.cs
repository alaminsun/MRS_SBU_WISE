using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class DepoModel : BaseModel
    {
        public string DepoCode { get; set; }
        public string SAPDepoCode { get; set; }
        public string DepoName { get; set; }
        public string SlNo { get; set; }
    }
}