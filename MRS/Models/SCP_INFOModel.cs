using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class SCP_INFOModel:BaseModel
    {
        public string SCP_CODE { get; set; }
        public string SCP_NAME { get; set; }
        public string SCP_PURPOSE { get; set; }
        public string STATUS { get; set; }
        public string ENTERED_BY { get; set; }
        public DateTime? ENTRY_DATE { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DATE { get; set; }
        public string WORK_STATION { get; set; }
    }
}