using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class ReportModel : BaseModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public string DOCTOR_ID { get; set; }
        public int SLNO { get; set; }
        [Required]
        public string DOCTOR_NAME { get; set; }
        public string DESIGNATION_CODE { get; set; }
        public string SPECIALIZATION_CODE { get; set; }
        public string DEGREE_CODE { get; set; }
        public string MANUFACTURER_CODE { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public string INSTI_TYPE_CODE { get; set; }
        public string PotencialCategory { get; set; }
        public string Orderby { get; set; }
        public string GENERIC_CODE { get; set; }
        public string GENERIC_NAME { get; set; }
        public string PRI_GENERIC_CODE { get; set; }
        public string PRI_GENERIC_NAME { get; set; }
        public string SECO_GENERIC_CODE { get; set; }
        public string SECO_GENERIC_NAME { get; set; }
        public string DOSAGE_FORM_CODE { get; set; }
        public string DOSAGE_FORM_NAME { get; set; }
        public string PRI_DOSAGE_FORM_CODE { get; set; }
        public string PRI_DOSAGE_FORM_NAME { get; set; }
        public string SECO_DOSAGE_FORM_CODE { get; set; }
        public string SECO_DOSAGE_FORM_NAME { get; set; }
        public string DSG_STRENGTH_CODE { get; set; }
        public string DSG_STRENGTH_NAME { get; set; }
        public string PRI_DSG_STRENGTH_CODE { get; set; }
        public string PRI_DSG_STRENGTH_NAME { get; set; }
        public string SECO_DSG_STRENGTH_CODE { get; set; }
        public string SECO_DSG_STRENGTH_NAME { get; set; }
        public string USER_ID { get; set; }

        public string TC_L4_CODE { get; set; }
        public string TC_L4_DESC { get; set; }
        public string REGION_CODE { get; set; }
        public string REGION_NAME { get; set; }
        public string TERRITORY_CODE { get; set; }
        public string TERRITORY_NAME { get; set; }
        public string DEGREE { get; set; }
        public string DESIGNATION { get; set; }
        public string DocAddress { get; set; }
        public string EntryDate { get; set; }
        public string INSTI_TYPE_NAME { get; set; }
        public string OrderByClause { get; set; }
        public string ddlGeneric { get; set; }
        public string ddlManufacturer { get; set; }
        public string PRESC_CATE_CODE { get; set; }
        public string PRESC_CATE_NAME { get; set; }
        public string GROUP_ID { get; set; }
        public string GROUP_NAME { get; set; }
        public string INSTI_CODE { get; set; }
        public string INSTI_NAME { get; set; }
        public string Address { get; set; }
        public string ddlCondition { get; set; }
        public string MARKET_CODE { get; set; }
        public string MARKET_NAME { get; set; }
        public string SharePerc { get; set; }
        public string NumberOfTran { get; set; }
        public string ddlPresCategory { get; set; }
        public string DIVISION_CODE { get; set; }
        public string DIVISION_NAME { get; set; }
        public string ZONE_CODE { get; set; }
        public string ZONE_NAME { get; set; }
        public string SURVEY_COMP_CODE { get; set; }
        public string SURVEY_COMP_NAME { get; set; }
        public string TherapeuticClassLevel { get; set; }
        public string ddlDataSource { get; set; }
        public string ddlTheraClassLevel { get; set; }
        public string ddlTherapeutic { get; set; }
        public string ddlTherapeuticSelection { get; set; }
        public List<InstitutionGroupDtlModel> instiList { get; set; }
        public List<DoctorGroupDtlModel> doctorList { get; set; }
        public string ddlYear { get; set; }
        public string TC_L1_CODE { get; set; }
        public string TC_L1_DESC { get; set; }
        public string TC_L2_CODE { get; set; }
        public string TC_L2_DESC { get; set; }
        public string TC_L3_CODE { get; set; }
        public string TC_L3_DESC { get; set; }
        public string dllOrderby { get; set; }
        public string Up_To_Rank { get; set; }
        public string SPECIALIZATION { get; set; }
        public string ddlGroup { get; set; }
        public string YEAR { get; set; }
        public string HONORARIUM_CODE { get; set; }
        public string SCP_CODE { get; set; }
        public string SCP_NAME { get; set; }
        public string SDICG_ID { get; set; }
    }
}