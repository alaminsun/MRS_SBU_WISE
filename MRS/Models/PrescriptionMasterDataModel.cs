using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class PrescriptionMasterDataModel// : BaseModel
    {
        public string SessionSlno { get; set; }
        public string PrescriptionSlno { get; set; }
        public string PrescriptionType { get; set; }
        public string InstitutionCode { get; set; }
        public string DoctorId { get; set; }
        public string Market { get; set; }
        public string Remarks { get; set; }
    }
}