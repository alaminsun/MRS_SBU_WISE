using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class DoctorMarketInfoModel : BaseModel
    {
        public long DoctorMstSl { get; set; }
        public long DoctorId { get; set; }
        public string RegistrationNo { get; set; }
        public string PotentialCategory { get; set; }
        public string Honorium { get; set; }
        public string DoctorName { get; set; }
        public string Degree { get; set; }
        public int DegreeCategory { get; set; }
        public string DegreeName { get; set; }
        public string Designation { get; set; }
        public int DesignationCategory { get; set; }
        public string DesignationName { get; set; }
        public int SpeciFirstCode { get; set; }
        public string SpeciFirstName { get; set; }
        public string SpeciSecondName { get; set; }
        public int SpeciSecondCode { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Remarks { get; set; }
        public List<DoctorMarketDetailsModel> DoctorMarketDetailsModels { get; set; }
    }

    public class DoctorMarketDetailsModel
    {
        public long DoctorDetailSl { get; set; }
        public long DoctorMstSl { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public int InstituteCode { get; set; }
        public string InstituteName { get; set; }
        public string ChamberAddress1 { get; set; }
        public string ChamberAddress2 { get; set; }
        public string ChamberAddress3 { get; set; }
        public string ChamberAddress4 { get; set; }
        public string Phone { get; set; }
        public string UpazilaCode { get; set; }
        public string UpazilaName { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string MorningLocCode { get; set; }
        public string MorningLocName { get; set; }
        public string MorningLocTextName { get; set; }
        public string EveningLocCode { get; set; }
        public string EveningLocName { get; set; }
        public string EveningTextLocName { get; set; }

        public string SBU_GROUP { get; set; }
    }
}
