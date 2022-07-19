using MRS.DAL.Common;
using MRS.DAL.Gateway;
using MRS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web;

namespace MRS.DAL.DAO
{
    public class DoctorGroupMstDtlDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg = new ValidationMsg();
        public int DoctorGroupMstSlNo = 0;
        private List<DoctorGroupMstModel> itemMasterList;
        private List<DoctorGroupDtlModel> itemDetailList;



        public List<DoctorGroupMstModel> GetDoctorGroupList()
        {
            string Qry = "";
            Qry = "SELECT GROUP_ID ,GROUP_NAME FROM DOCTOR_GROUP ";
            Qry = Qry + "ORDER BY GROUP_ID";

            DataTable dt = dbHelper.GetDataTable(Qry);

            itemMasterList = (from DataRow row in dt.Rows
                              select new DoctorGroupMstModel
                              {
                                  GROUP_ID = row["GROUP_ID"].ToString(),
                                  GROUP_NAME = row["GROUP_NAME"].ToString()
                              }).ToList();

            return itemMasterList;
        }

        public List<DoctorGroupDtlModel> GetDoctorGroupDtlList()
        {
            string Qry = "";
            Qry = "SELECT DC.DOCTOR_ID,DC.DOCTOR_NAME,DC.DEGREE_CODE,DC.DESIGNATION_CODE,DC.SPECIA_1ST_CODE,DD.DEGREE,DCD.DESIGNATION,DS.SPECIALIZATION  FROM Doctor DC LEFT JOIN DOCTOR_DEGREE DD ON DC.DEGREE_CODE = DD.DEGREE_CODE  LEFT JOIN DOCTOR_DESIGNATION DCD ON DC.DESIGNATION_CODE = DCD.DESIGNATION_CODE  LEFT JOIN DOCTOR_SPECIALIZATION DS ON DC.SPECIA_1ST_CODE = DS.SPECIALIZATION_CODE";
            Qry = Qry + " ORDER BY DC.DOCTOR_ID";

            DataTable dt = dbHelper.GetDataTable(Qry);
            itemDetailList = (from DataRow row in dt.Rows
                              select new DoctorGroupDtlModel
                              {
                                  DOCTOR_ID = row["DOCTOR_ID"].ToString(),
                                  DOCTOR_NAME = row["DOCTOR_NAME"].ToString(),
                                  DEGREE = row["DEGREE"].ToString(),
                                  DESIGNATION = row["DESIGNATION"].ToString(),
                                  SPECIALIZATION = row["SPECIALIZATION"].ToString()
                              }).ToList();

            return itemDetailList;
        }

        public ValidationMsg Save(DoctorGroupMstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                //{

                #region GROUPMas

                string query = "SELECT (NVL(MAX(DOC_GRP_MAS_SLNO),0)+1)DOC_GRP_MAS_SLNO FROM DOCTOR_GROUP_MAS";
                DataTable dt = dbHelper.GetDataTable(query);
                if (dt.Rows.Count > 0)
                {
                    model.DOC_GRP_MAS_SLNO = dt.Rows[0][0].ToString();
                }

                string qry = "INSERT INTO DOCTOR_GROUP_MAS(DOC_GRP_MAS_SLNO,GROUP_ID)" +
                "VALUES(" + model.DOC_GRP_MAS_SLNO + ", '" + model.GROUP_ID + "')";

                dbHelper.CmdExecute(qry);

                //if (dbHelper.CmdExecute(qry) > 0)
                //{
                //    DoctorGroupMstSlNo = Convert.ToInt16(model.DOC_GRP_MAS_SLNO);
                //}

                #endregion

                #region DoctorGroupDtl

                if (model.DoctorGroupDtlList != null)
                {
                    int i = 0;
                    string query1 = "SELECT (NVL(MAX(DOC_GRP_DET_SLNO),0)+1)DOC_GRP_DET_SLNO FROM DOCTOR_GROUP_DTL";
                    DataTable dataTableDtl = dbHelper.GetDataTable(query1);
                    if (dataTableDtl.Rows.Count > 0)
                    {
                        i = Convert.ToInt32(dataTableDtl.Rows[0][0].ToString());
                        //objInstitutionGroupDtlModel.INSTI_GRP_DET_SLNO = dataTableDtl.Rows[0][0].ToString();
                        //objInstitutionGroupDtlModel.INSTI_GRP_MAS_SLNO = InstitutionGroupMasterSlNo.ToString();//New added Line which is in Query Also
                    }


                    foreach (DoctorGroupDtlModel objDoctorGroupDtlModel in model.DoctorGroupDtlList)
                    {
                        //DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(DOC_GRP_DET_SLNO),0)+1 from DOCTOR_GROUP_DTL");
                        //if (dtDtl.Rows.Count > 0)
                        //    objDoctorGroupDtlModel.DOC_GRP_DET_SLNO = dtDtl.Rows[0][0].ToString();
                        //    objDoctorGroupDtlModel.DOC_GRP_MAS_SLNO = DoctorGroupMstSlNo.ToString();


                        qry = "INSERT INTO DOCTOR_GROUP_DTL(DOC_GRP_DET_SLNO,DOC_GRP_MAS_SLNO,DOCTOR_ID)" +
                    "VALUES('" + i + "', '" + model.DOC_GRP_MAS_SLNO + "', " + objDoctorGroupDtlModel.DOCTOR_ID + ")";
                        if (dbHelper.CmdExecute(qry) > 0)
                        {
                            i++;
                            _vmMsg.Type = Enums.MessageType.Success;

                        }
                    }
                }

                #endregion




                //DataTable dataTable =
                //    dbHelper.GetDataTable(
                //        "SELECT (NVL(MAX(DOC_GRP_MAS_SLNO),0)+1)DOC_GRP_MAS_SLNO FROM DOCTOR_GROUP_MAS");
                //if (dataTable.Rows.Count > 0)
                //{
                //    model.DOC_GRP_MAS_SLNO = dataTable.Rows[0][0].ToString();
                //}
                //string query = "INSERT INTO DOCTOR_GROUP_MAS(DOC_GRP_MAS_SLNO,GROUP_ID)" +
                //               "VALUES(" + model.DOC_GRP_MAS_SLNO + ", '" + model.GROUP_ID + "')";
                //if (dbHelper.CmdExecute(query) > 0)
                //{
                //    DoctorGroupMstSlNo = Convert.ToInt16(model.DOC_GRP_MAS_SLNO);
                //}
                //if (model.DoctorGroupDtlList != null)
                //{
                //    foreach (DoctorGroupDtlModel objDoctorGroupDtlModel in model.DoctorGroupDtlList)
                //    {
                //        DataTable dataTableDtl =
                //            dbHelper.GetDataTable(
                //                "SELECT (NVL(MAX(DOC_GRP_DET_SLNO),0)+1)DOC_GRP_DET_SLNO FROM DOCTOR_GROUP_DTL");
                //        if (dataTableDtl.Rows.Count > 0)
                //        {
                //            objDoctorGroupDtlModel.DOC_GRP_DET_SLNO = dataTableDtl.Rows[0][0].ToString();
                //            objDoctorGroupDtlModel.DOC_GRP_MAS_SLNO = DoctorGroupMstSlNo.ToString();//New added Line which is in Query Also
                //        }
                //        string querydtl =
                //            "INSERT INTO DOCTOR_GROUP_DTL(DOC_GRP_DET_SLNO,DOC_GRP_MAS_SLNO,DOCTOR_ID)" +
                //            "VALUES('" + objDoctorGroupDtlModel.DOC_GRP_DET_SLNO + "', '" + model.DOC_GRP_MAS_SLNO +
                //            "', '" + objDoctorGroupDtlModel.DOCTOR_ID +
                //            "')";
                //        if (dbHelper.CmdExecute(querydtl) > 0)
                //        {
                //            _vmMsg.Type = Enums.MessageType.Success;
                //        }
                //    }
                //}
                //scope.Complete();
                //_vmMsg.Type = Enums.MessageType.Success;
                _vmMsg.Msg = "Saved Successfully.";
                //}
            }
            catch (Exception ex)
            {

                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to save.";

                if (ex.Message.Contains("ORA-00001"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Doctor or Group Id is Already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }

            return _vmMsg;
        }

        public ValidationMsg Update(DoctorGroupMstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            string query = "";
            try
            {
                    if (model.DOC_GRP_MAS_SLNO != "")
                    {
                        string qry = "SELECT TC1.DOC_GRP_MAS_SLNO,TC1.GROUP_ID,TL1.GROUP_NAME FROM DOCTOR_GROUP_MAS TC1 INNER JOIN DOCTOR_GROUP TL1 ON TC1.GROUP_ID=TL1.GROUP_ID";
                        qry = qry + " ORDER BY TC1.DOC_GRP_MAS_SLNO";

                        DataTable dt = dbHelper.GetDataTable(qry);
                        itemMasterList = (from DataRow row in dt.Rows
                                          select new DoctorGroupMstModel
                                          {
                                              GROUP_ID = row["GROUP_ID"].ToString(),
                                              GROUP_NAME = row["GROUP_NAME"].ToString(),
                                              DOC_GRP_MAS_SLNO = row["DOC_GRP_MAS_SLNO"].ToString()
                                          }).ToList();
                        if (itemMasterList.Count > 0)
                        {
                            query = "UPDATE DOCTOR_GROUP_MAS SET GROUP_ID = '" + model.GROUP_ID + "' WHERE DOC_GRP_MAS_SLNO = '" + model.DOC_GRP_MAS_SLNO + "'";
                        }

                        dbHelper.CmdExecute(query);
                    }
                    if (model.DoctorGroupDtlList != null)
                    {
                        foreach (DoctorGroupDtlModel objDoctorGroupUpdateDtlModel in model.DoctorGroupDtlList)
                        {
                            if (objDoctorGroupUpdateDtlModel.DOC_GRP_DET_SLNO != null)
                            {

                                string querydtl =
                                                "UPDATE DOCTOR_GROUP_DTL SET DOCTOR_ID = '" + objDoctorGroupUpdateDtlModel.DOCTOR_ID + "'" +
                                                "WHERE DOC_GRP_MAS_SLNO = '" + model.DOC_GRP_MAS_SLNO + "' AND DOC_GRP_DET_SLNO = '" + objDoctorGroupUpdateDtlModel.DOC_GRP_DET_SLNO + "'";

                                dbHelper.CmdExecute(querydtl);
                            }
                            else
                            {
                                    DataTable dataTableDtl =
                                        dbHelper.GetDataTable(
                                            "SELECT (NVL(MAX(DOC_GRP_DET_SLNO),0)+1)DOC_GRP_DET_SLNO FROM DOCTOR_GROUP_DTL");
                                    if (dataTableDtl.Rows.Count > 0)
                                    {
                                        objDoctorGroupUpdateDtlModel.DOC_GRP_DET_SLNO = dataTableDtl.Rows[0][0].ToString();
                                        objDoctorGroupUpdateDtlModel.DOC_GRP_MAS_SLNO = DoctorGroupMstSlNo.ToString();//New added Line which is in Query Also
                                    }
                                    string querydtl =
                                            "INSERT INTO DOCTOR_GROUP_DTL(DOC_GRP_DET_SLNO,DOC_GRP_MAS_SLNO,DOCTOR_ID)" +
                                            "VALUES('" + objDoctorGroupUpdateDtlModel.DOC_GRP_DET_SLNO + "', '" + model.DOC_GRP_MAS_SLNO +
                                            "', '" + objDoctorGroupUpdateDtlModel.DOCTOR_ID +
                                            "')";
                                    dbHelper.CmdExecute(querydtl);
                                
                            }
                        }
                    }
                    //scope.Complete();
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Updated Successfully.";
                
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Update.";

                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }
            return _vmMsg;
        }



        public List<DoctorGroupMstModel> GetSearchMasterList()
        {
            string qry = "SELECT TC1.DOC_GRP_MAS_SLNO,TC1.GROUP_ID,TL1.GROUP_NAME FROM DOCTOR_GROUP_MAS TC1 INNER JOIN DOCTOR_GROUP TL1 ON TC1.GROUP_ID=TL1.GROUP_ID";
            qry = qry + " ORDER BY TC1.DOC_GRP_MAS_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemMasterList = (from DataRow row in dt.Rows
                              select new DoctorGroupMstModel
                              {
                                  GROUP_ID = row["GROUP_ID"].ToString(),
                                  GROUP_NAME = row["GROUP_NAME"].ToString(),
                                  DOC_GRP_MAS_SLNO = row["DOC_GRP_MAS_SLNO"].ToString()
                              }).ToList();
            return itemMasterList;
        }

        public List<DoctorGroupDtlModel> GetSearchDtliList(string doctorGroupMstSlNo)
        {
            string qry = "SELECT DGD.DOC_GRP_DET_SLNO,DGD.DOC_GRP_MAS_SLNO,DGD.DOCTOR_ID,D.DOCTOR_NAME,DD.DEGREE,DCD.DESIGNATION,DS.SPECIALIZATION FROM DOCTOR_GROUP_DTL DGD  INNER JOIN DOCTOR D ON DGD.DOCTOR_ID=D.DOCTOR_ID LEFT JOIN DOCTOR_DEGREE DD ON D.DEGREE_CODE = DD.DEGREE_CODE  LEFT JOIN DOCTOR_DESIGNATION DCD ON D.DESIGNATION_CODE = DCD.DESIGNATION_CODE  LEFT JOIN DOCTOR_SPECIALIZATION DS ON D.SPECIA_1ST_CODE = DS.SPECIALIZATION_CODE  WHERE DGD.DOC_GRP_MAS_SLNO='" + doctorGroupMstSlNo + "'";
            qry = qry + " ORDER BY DGD.DOC_GRP_DET_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemDetailList = (from DataRow row in dt.Rows
                              select new DoctorGroupDtlModel
                              {
                                  DOCTOR_ID = row["DOCTOR_ID"].ToString(),
                                  DOCTOR_NAME = row["DOCTOR_NAME"].ToString(),
                                  DEGREE = row["DEGREE"].ToString(),
                                  DESIGNATION = row["DESIGNATION"].ToString(),
                                  SPECIALIZATION = row["SPECIALIZATION"].ToString(),
                                  DOC_GRP_MAS_SLNO = row["DOC_GRP_MAS_SLNO"].ToString(),
                                  DOC_GRP_DET_SLNO = row["DOC_GRP_DET_SLNO"].ToString()
                              }).ToList();
            return itemDetailList;
        }


        public ValidationMsg DeleteDetailData(string doctorGroupDtlSlNo)
        {
            try
            {

                string deleteDetailQuery = "DELETE FROM DOCTOR_GROUP_DTL WHERE DOC_GRP_DET_SLNO = '" + doctorGroupDtlSlNo + "'";
                if (dbHelper.CmdExecute(deleteDetailQuery) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }

            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Delete Grid Data.";
            }
            return _vmMsg;
        }


        public ValidationMsg Delete(string doctorGroupMstSlNo)
        {
            //string deleteMaster = "";
            try
            {
                string querymaster = "DELETE FROM DOCTOR_GROUP_MAS WHERE DOC_GRP_MAS_SLNO = '" +
                                         doctorGroupMstSlNo + "'";

                if (dbHelper.CmdExecute(querymaster) > 0)
                {
                    _vmMsg.Type = Enums.MessageType.Success;
                    _vmMsg.Msg = "Data Deleted Successfully.";
                }
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Delete All Data.";
            }
            return _vmMsg;
        }



    }
}