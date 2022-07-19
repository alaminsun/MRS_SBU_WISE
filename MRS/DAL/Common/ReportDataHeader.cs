using MRS.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.DAL.Common
{
    public class ReportDataHeader
    {
        DBConnection dbConnection = new DBConnection();
        BaseReportDataHeaderModel reportDataHeader = new BaseReportDataHeaderModel();
        public ReportDataHeader()
        {
            reportDataHeader.GetReportHeaderData(dbConnection.StringRead());
        }


        public string ComName = BaseReportDataHeaderModel.ComName;
        public string ComAddress = BaseReportDataHeaderModel.ComAddress;
        public string ComPhone = BaseReportDataHeaderModel.ComPhone;
        public string DevCompany = BaseReportDataHeaderModel.DevCompany;
    }
}