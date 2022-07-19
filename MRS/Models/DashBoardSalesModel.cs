using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MRS.Models
{
    public class DashBoardSalesModel : BaseModel
    {
        public string GRP_DSIG_CODE { get; set; }
        public string GRP_DSIG_NAME { get; set; }
        public string TOT_CEILING_AMOUNT { get; set; }
        public string TOT_GRP_EXP { get; set; }
        public string HONORARIUM_CODE { get; set; }
        public string HONORARIUM_NAME { get; set; }
        public string LEADER_NON_LEADER { get; set; }
        public string NO_OF_DOCTOR { get; set; }
        public string MONTH_NO { get; set; }
        public string MONTH_NAME { get; set; }
        public string PREV_YEAR_MON_WS_EXP { get; set; }
        public string CUR_YEAR_MON_WS_EXP { get; set; }
        public string COMMITMENT_MEETUP { get; set; }
        public string NO_OF_TRANSACTION { get; set; }
        public string SQA_SHARE_PCT { get; set; }

        public string Data_Type { get; set; }
        public string ToDate { get; set; }
        public string FromDate { get; set; }
    }
}