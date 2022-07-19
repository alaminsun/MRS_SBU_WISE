using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class DoctorGroupDtlModel:BaseModel
    {
        public string DOC_GRP_MAS_SLNO { get; set; }
        public string DOC_GRP_DET_SLNO { get; set; }
        public string DOCTOR_ID { get; set; }
        public string DOCTOR_NAME { get; set; }

        public string DEGREE { get; set; }
        public string DESIGNATION { get; set; }
        public string SPECIALIZATION { get; set; }


        public string DEGREE_CODE { get; set; }
        public string DESIGNATION_CODE { get; set; }
        public string SPECIA_1ST_CODE { get; set; }

        public List<DoctorGroupDtlModel>DoctorGroupDtlLoadList = new List<DoctorGroupDtlModel>(); 

    }
}