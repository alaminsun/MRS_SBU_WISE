// Decompiled with JetBrains decompiler
// Type: CompulsoryLimb.Thread.RetrivedData
// Assembly: CompulsoryLimb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0F8AC768-09B2-443C-BEE0-1B513DA2755E
// Assembly location: E:\Development\Development\Office\2021\SPA_SFBL_23_03_2021_home\SPA_SFBL\SPAs\SPAs\bin\CompulsoryLimb.dll

using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Linq;

namespace CompulsoryLimb.Thread
{
  public class RetrivedData
  {
    //public List<MenuItemView> GetMenuItem(DataTable dtTable) => dtTable.Rows.Cast<DataRow>().Select<DataRow, MenuItemView>((Func<DataRow, MenuItemView>) (row => new MenuItemView()
    //{
    //  MenuID = new long?(Convert.ToInt64(row["MENUID"].ToString())),
    //  ModuleTitle = row["MenuName"].ToString(),
    //  URL = row["URL"].ToString(),
    //  ItemOrder = Convert.ToInt64(row["ITEMORDER"].ToString()),
    //  ParentID = Convert.ToInt64(row["PARENTID"].ToString()) == 0L ? new long?() : new long?(Convert.ToInt64(row["PARENTID"].ToString()))
    //})).ToList<MenuItemView>();

    //public DataTable GetMenuItem(
    //  string Conn,
    //  string Qry,
    //  string ParentField,
    //  long? ParentValue,
    //  string OrderBy)
    //{
    //  if (!ParentValue.HasValue)
    //    Qry = Qry + " and ltrim(rtrim(" + ParentField + ")) is null ";
    //  if (ParentValue.HasValue)
    //    Qry = Qry + " and " + ParentField + "=" + (object) ParentValue;
    //  Qry += OrderBy;
    //  return this.GetDataTable(Conn, Qry);
    //}

    //public DataTable GetDataTable(string Conn, string Qry)
    //{
    //  DataTable dataTable = new DataTable();
    //  using (OracleConnection oracleConnection = new OracleConnection(Conn))
    //  {
    //    OracleCommand oracleCommand = new OracleCommand();
    //    oracleCommand.CommandText = Qry;
    //    oracleCommand.Connection = oracleConnection;
    //    oracleConnection.Open();
    //    oracleCommand.ExecuteNonQuery();
    //    using (OracleDataReader oracleDataReader = oracleCommand.ExecuteReader())
    //    {
    //      if (oracleDataReader.HasRows)
    //        dataTable.Load((IDataReader) oracleDataReader);
    //    }
    //  }
    //  return dataTable;
    //}

    //public string GetValue(string Conn, string Qry)
    //{
    //  string str = "";
    //  using (OracleConnection connection = new OracleConnection(Conn))
    //  {
    //    connection.Open();
    //    using (OracleDataReader oracleDataReader = new OracleCommand(Qry, connection).ExecuteReader())
    //    {
    //      if (oracleDataReader.Read())
    //        str = oracleDataReader[0].ToString();
    //    }
    //  }
    //  return str;
    //}
  }
}
