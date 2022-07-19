using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class DoctorHonorariumPaidModel : BaseModel
    {
        public string HONOR_PAID_SLNO { get; set; }
        public string HONOR_APROV_NO { get; set; }
        public string HONOR_APROV_DATE { get; set; }
        public string MGMT_CODE { get; set; }
        public string MGMT_NAME { get; set; }

        public string LOCATION_CODE { get; set; }
        public string GRP_DSIG_CODE { get; set; }
        public string GRP_DSIG_NAME { get; set; }
        public string MARKET_CODE { get; set; }
        public string TERRITORY_CODE { get; set; }
        public string REGION_CODE { get; set; }
        public string ZONE_CODE { get; set; }
        public string DIVISION_CODE { get; set; }
        public string NATIONAL { get; set; }
        public string SOCIETY_ID { get; set; }
        public string SOCITY_NAME { get; set; }
        public string PROD_ID { get; set; }
        public string SAP_CODE { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string INSTI_CODE { get; set; }
        public string DOCTOR_ID { get; set; }
        public string HONORARIUM_CODE { get; set; }
        public string EXPENSE_DETAILS { get; set; }
        public string EXPENSE_AMT { get; set; }
        public string EXPENSE_FROM { get; set; }
        public string EXPENSE_TO { get; set; }
        public string PRESC_SHARE_PCT { get; set; }
        public string PRESC_SHARE_FROM { get; set; }
        public string PRESC_SHARE_TO { get; set; }
        public string COMITMENT_SHARE_PCT { get; set; }
        public string HONORARIUM_DURATION { get; set; }
        public string DURATION_FROM { get; set; }
        public string DURATION_TO { get; set; }
        public string MARKET_NAME { get; set; }
        public string TERRITORY_NAME { get; set; }
        public string REGION_NAME { get; set; }
        public string DIVISION_NAME { get; set; }
        public string ZONE_NAME { get; set; }
        public string INSTI_NAME { get; set; }
        public string HONORARIUM_NAME { get; set; }
        public string DOCTOR_NAME { get; set; }
        public string DEGREE { get; set; }
        public string DESIGNATION { get; set; }
        public string MON_CELI_AMT { get; set; }
        public string TOT_APPROVED_AMT { get; set; }
        public string TOT_DUE_AMT { get; set; }
        public string OTHER_REMARKS { get; set; }
        public string ADDITIONAL_COMITMENT { get; set; }
        public string EXPENSE_DURATION { get; set; }
        public string PRESENT_SHARE_DURATION { get; set; }
        public string HONOR_TYPE_NAME { get; set; }
        public string HONOR_APPROVE_BY_NAME { get; set; }
        public string ENTERED_BY { get; set; }
        public string RowCount { get; set; }

        public List<DoctorCommitProductModel> DoctorCommitProductList { get; set; }
        public List<ManagementTeamModel> ManagementTeamList { get; set; }
        public List<HonorariumTypeModel> HonorariumTypeList { get; set; }

        

        public string REGION_NAME1 { get; set; }

        public string REGION_CODE1 { get; set; }

        public string DIVISION_CODE1 { get; set; }

        public string DIVISION_NAME1 { get; set; }
    }
    public class DoctorCommitProductModel
    {
        public int DOC_COMMI_PROD_SLNO { get; set; }
        public string HONOR_PAID_SLNO { get; set; }
        public string PROD_ID { get; set; }
        public string PRODUCT_NAME { get; set; }
        public string PRODUCT_DETAILS { get; set; }
        
    }
}