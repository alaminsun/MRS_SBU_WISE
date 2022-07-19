using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ManagDesigCeilingModel : BaseModel
    {
        public string HON_WISE_CEILING_CODE { get; set; }
        //public string MGMT_CODE { get; set; }
        //public string MGMT_NAME { get; set; }
        public string GRP_DSIG_CODE { get; set; }
        public string GRP_DSIG_NAME { get; set; }
        public string HONORARIUM_CODE { get; set; }
        public string HONORARIUM_NAME { get; set; }
        public string CEILING_AMT { get; set; }
        public List<ManagDesigCeilingModel> HonCeilingAmountList { get; set; }
    }
}