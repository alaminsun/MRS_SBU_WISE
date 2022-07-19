using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class InstitutionInformationModel : BaseModel
    {
        public string INSTI_CODE { get; set; }
        public string INSTI_NAME { get; set; }
        public string INSTI_TYPE_CODE { get; set; }
        public string INSTI_TYPE_NAME { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string ADDRESS3 { get; set; }
        public string ADDRESS4 { get; set; }
        public string INST_PHONE { get; set; }
        public string UPAZILA_CODE { get; set; }
        public string UPAZILA_NAME { get; set; }
        public string DISTC_CODE { get; set; }
        public string DISTC_NAME { get; set; }
        public int NO_OF_BEDS { get; set; }
        public int AVG_NO_ADMT_PATI { get; set; }
        public int AVG_NO_OD_PATI { get; set; }
        public string REMARKS { get; set; }


        //add jia
        public string InstiCode { get; set; }
        public string InstitutionName { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }

    }


    //public class UpazilaModel
    //{
    //    public string UPAZILA_CODE { get; set; }
    //    public string UPAZILA_NAME { get; set; }
    //    public string UPAZILA_ORD_SLNO { get; set; }

    //}


}