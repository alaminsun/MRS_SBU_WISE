using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class SCPCDoctorInfoModel
    {
        public int SDI_ID { get; set; }
        public string DoctorId { get; set; }
        public string DoctorCode { get; set; }
        public string DoctorName { get; set; }
        public string Degree { get; set; }
        public string SpeciFirstName { get; set; }
        public string Address { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
    }
}