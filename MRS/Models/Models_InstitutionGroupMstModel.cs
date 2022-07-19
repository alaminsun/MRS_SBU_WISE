using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class InstitutionGroupMstModel:BaseModel
    {
        public string INSTI_GRP_MAS_SLNO { get; set; }
        public string GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }

        //public List<InstitutionGroupDtlModel> InstitutionGroupDtlList { get; set; }
        public virtual IList<InstitutionGroupDtlModel> InstitutionGroupDtlList { get; set; }

    }
}