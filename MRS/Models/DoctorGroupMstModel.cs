using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class DoctorGroupMstModel:BaseModel
    {
        public string DOC_GRP_MAS_SLNO { get; set; }
        public string GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public List<DoctorGroupDtlModel> DoctorGroupDtlList { get; set; }
    }
}