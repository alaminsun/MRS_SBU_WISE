﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class SpecCamDocInsConfigModel:BaseModel
    {
        public int SDIC_ID { get; set; }
        public string SCP_CODE { get; set; }
        public string SCP_NAME { get; set; }
        public string ENTERED_BY { get; set; }
        public DateTime? ENTERED_DATE { get; set; }
        public string ENTERED_TERMINAL { get; set; }
        public string UPDATED_BY { get; set; }
        public DateTime? UPDATED_DATE { get; set; }

        public List<SCPGroupInfoModel> ScpGroupInfoList { get; set; }
        public List<SCPCDoctorInfoModel> DoctorInfoList { get; set; }
        public List<SCPInstitutionInfoModel> InstitutionInfoList { get; set; }

    }
}