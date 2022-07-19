using MRS.DAL.Gateway;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.Common
{
    
    public class BaseReportDataHeaderModel
    {
        DBHelper dbHelper = new DBHelper();
    //private RetrivedData retrivedData = new RetrivedData();
    public static string comCode;
    public static string comName;
    public static string comAddress;
    public static string comPhone;
    public static string devCompany;
    public static long runningStatus;


    public static string ComAddress { get; set; }
    public static string ComCode { get; set; }
    public static string ComName { get; set; }
    public static string ComPhone { get; set; }
    public static string DevCompany { get; set; }
    public static long RunningStatus { get; set; }

    public void GetReportHeaderData(string Conn)
    {
      BaseReportDataHeaderModel.comCode = "";
      BaseReportDataHeaderModel.comName = "";
      BaseReportDataHeaderModel.comAddress = "";
      BaseReportDataHeaderModel.comPhone = "";
      BaseReportDataHeaderModel.devCompany = "";
      BaseReportDataHeaderModel.runningStatus = 0L;
      string Qry = "Select Company_Code,Company_Name,Company_Address,Company_Phone,DevBy,IsRunning from Sa_Company_Info Where IsRunning=1";
      DataTable dataTable = dbHelper.GetDataTable(Qry);
      if (dataTable.Rows.Count <= 0)
        return;
      BaseReportDataHeaderModel.comCode = dataTable.Rows[0]["Company_Code"].ToString();
      BaseReportDataHeaderModel.comName = dataTable.Rows[0]["Company_Name"].ToString();
      BaseReportDataHeaderModel.comAddress = dataTable.Rows[0]["Company_Address"].ToString();
      BaseReportDataHeaderModel.comPhone = dataTable.Rows[0]["Company_Phone"].ToString();
      BaseReportDataHeaderModel.devCompany = dataTable.Rows[0]["DevBy"].ToString();
      BaseReportDataHeaderModel.runningStatus = Convert.ToInt64(dataTable.Rows[0]["IsRunning"].ToString());
    }
    }
}