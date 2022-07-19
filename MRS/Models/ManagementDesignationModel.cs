using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ManagementDesignationModel : BaseModel
    {
        public string MGMT_CODE { get; set; } 
        public string MGMT_NAME { get; set; }
        public string GRP_DSIG_CODE { get; set; }
        public string GRP_DSIG_NAME { get; set; }

        public string REGION_CODE { get; set; }

        public string REGION_NAME { get; set; }

        public string DIVISION_CODE { get; set; }

        public string DIVISION_NAME { get; set; }
    }
}