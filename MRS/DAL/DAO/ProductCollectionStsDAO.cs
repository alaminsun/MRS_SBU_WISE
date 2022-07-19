using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MRS.DAL.DAO
{
        
    public class ProductCollectionStsDAO
    {
        DBHelper dbHelper = new DBHelper();
        DBConnection dbCon = new DBConnection();

        public DataTable GetProductCollectionReport(ReportModel model)
        {
            String Qry = "select g.region_name, h.territory_name, i.market_name, nvl(b.no_of_prod,0) pres_no, nvl(c.no_of_prod,0) ins_no," +
            " nvl(d.no_of_prod,0) slip_no, nvl(e.no_of_prod,0) otc_no, (nvl(b.no_of_prod,0) +nvl(c.no_of_prod,0) + nvl(d.no_of_prod,0) + nvl(e.no_of_prod,0) ) tot_no " +
            " from (select a.market_code,  count(c.purchase_qty) no_of_prod from prescription_master a, user_working_session b, prescription_detail c " +
            " where b.session_slno=a.session_slno and a.presc_mas_slno=c.presc_mas_slno and (b.entry_date) BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') " +
            " and To_Date('" + model.ToDate + "','dd/MM/yyyy')  and a.presc_cate_code='P' group by a.market_code order by a.market_code) b, " +
            " (select a.market_code,  count(c.purchase_qty) no_of_prod from prescription_master a, user_working_session b, prescription_detail c " +
            " where b.session_slno=a.session_slno and a.presc_mas_slno=c.presc_mas_slno and (b.entry_date) BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') " +
            " and To_Date('" + model.ToDate + "','dd/MM/yyyy') and a.presc_cate_code='O' group by a.market_code order by a.market_code) c, " +
            " (select a.market_code,  count(c.purchase_qty) no_of_prod from prescription_master a, user_working_session b, prescription_detail c where b.session_slno=a.session_slno " +
            " and a.presc_mas_slno=c.presc_mas_slno and (b.entry_date) BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') and  To_Date('" + model.ToDate + "','dd/MM/yyyy') " +
            " and a.presc_cate_code='S' group by a.market_code order by a.market_code) d, (select a.market_code, count(c.purchase_qty) no_of_prod " +
            " from prescription_master a, user_working_session b, prescription_detail c where b.session_slno=a.session_slno and a.presc_mas_slno=c.presc_mas_slno " +
            " and (b.entry_date) BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') and  To_Date('" + model.ToDate + "','dd/MM/yyyy') and a.presc_cate_code='T' " +
            " group by a.market_code order by a.market_code) e, location_vue f, region g, territory h,  market i where f.market_code=b.market_code(+) and f.market_code=c.market_code(+)" +
            " and f.market_code=d.market_code(+) and f.market_code=e.market_code(+) and f.market_code=i.market_code and f.territory_code=h.territory_code " +
            " and f.region_code=g.region_code order by f.region_code, f.territory_code, f.market_code ";


            DataTable dt = dbHelper.GetDataTable(Qry);
            return dt;
        }

        public DataTable GetPreOrgSlpOtcColectionReport(ReportModel model)
        {
            string Qry = " select g.region_name, h.territory_name, i.market_name, nvl(b.no_of_pres,0) pres_no, nvl(c.no_of_pres,0) ins_no, " +
" nvl(d.no_of_pres,0) slip_no, nvl(e.no_of_pres,0) otc_no from (select a.market_code,  count(a.presc_cate_code) no_of_pres from prescription_master a, user_working_session b " +
" where b.session_slno=a.session_slno and a.presc_cate_code='P' and (b.entry_date) BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') " +
" and To_Date('" + model.ToDate + "','dd/MM/yyyy') group by a.market_code order by a.market_code) b, (select a.market_code,  count(a.presc_cate_code) no_of_pres " +
" from prescription_master a, user_working_session b where b.session_slno=a.session_slno and a.presc_cate_code='O' " +
" and (b.entry_date) BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') and  To_Date('" + model.ToDate + "','dd/MM/yyyy') group by a.market_code " +
" order by a.market_code) c, (select a.market_code, count(a.presc_cate_code) no_of_pres from prescription_master a, user_working_session b " +
" where b.session_slno=a.session_slno and a.presc_cate_code='S' and (b.entry_date) BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') " +
" and  To_Date('" + model.ToDate + "','dd/MM/yyyy') group by a.market_code order by a.market_code) d, (select a.market_code, count(a.presc_cate_code) no_of_pres " +
" from prescription_master a, user_working_session b where b.session_slno=a.session_slno and a.presc_cate_code='T' " +
" and (b.entry_date) BETWEEN To_Date('" + model.FromDate + "','dd/MM/yyyy') and  To_Date('" + model.ToDate + "','dd/MM/yyyy') group by a.market_code " +
" order by a.market_code) e, location_vue f, region g, territory h,  market i where f.market_code=b.market_code(+) and f.market_code=c.market_code(+) " +
" and f.market_code=d.market_code(+) and f.market_code=e.market_code(+) and f.market_code=i.market_code and f.territory_code=h.territory_code " +
" and f.region_code=g.region_code order by f.region_code, f.territory_code, f.market_code";

            DataTable dt = dbHelper.GetDataTable(Qry);
            return dt;
        }
    }
}