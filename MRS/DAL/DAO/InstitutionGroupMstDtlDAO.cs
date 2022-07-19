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
    public class InstitutionGroupMstDtlDAO
    {
        DBHelper dbHelper = new DBHelper();
        private ValidationMsg _vmMsg = new ValidationMsg();
        public int InstitutionGroupMasterSlNo = 0;
        private List<InstitutionGroupMstModel> itemMasterList;
        private List<InstitutionGroupDtlModel> itemDetailList;


        public List<InstitutionGroupMstModel> GetGroupList()
        {
            string Qry = "";
            Qry = "SELECT GROUP_ID ,GROUP_NAME FROM INSTITUTION_GROUP ORDER BY GROUP_ID ";

            DataTable dt = dbHelper.GetDataTable(Qry);

            itemMasterList = (from DataRow row in dt.Rows
                              select new InstitutionGroupMstModel
                              {
                                  GROUP_ID = row["GROUP_ID"].ToString(),
                                  GROUP_NAME = row["GROUP_NAME"].ToString()
                              }).ToList();

            return itemMasterList;
        }

        public List<InstitutionGroupDtlModel> GetInstitutionList()
        {
            string Qry = "";
            Qry = "SELECT I.INSTI_CODE,I.INSTI_NAME,I.INSTI_TYPE_CODE,IT.INSTI_TYPE_NAME,(  I.ADDRESS1||' '||I.ADDRESS2||' '||I.ADDRESS3||' '||I.ADDRESS4 ) AS ADDRESS  FROM INSTITUTION I LEFT JOIN INSTITUTION_TYPE IT  ON I.INSTI_TYPE_CODE = IT.INSTI_TYPE_CODE";
            Qry = Qry + " ORDER BY I.INSTI_CODE";

            DataTable dt = dbHelper.GetDataTable(Qry);
            itemDetailList = (from DataRow row in dt.Rows
                              select new InstitutionGroupDtlModel
                              {
                                  INSTI_CODE = row["INSTI_CODE"].ToString(),
                                  INSTI_NAME = row["INSTI_NAME"].ToString(),
                                  INSTI_TYPE_CODE = row["INSTI_TYPE_CODE"].ToString(),
                                  INSTI_TYPE_NAME = row["INSTI_TYPE_NAME"].ToString(),
                                  ADDRESS = row["ADDRESS"].ToString()
                              }).ToList();

            return itemDetailList;
        }

        //public ValidationMsg Save(InstitutionGroupMstModel model, string userid)
        public ValidationMsg Save(List<InstitutionGroupDtlModel> InstitutionGroupDtlList, string GROUP_ID, string userid)
        {
            InstitutionGroupMstModel model = new InstitutionGroupMstModel();
            _vmMsg = new ValidationMsg();
            try
            {
                //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                //{
                DataTable dataTable =
                    dbHelper.GetDataTable(
                        "SELECT (NVL(MAX(INSTI_GRP_MAS_SLNO),0)+1)INSTI_GRP_MAS_SLNO FROM INSTITUTION_GROUP_MAS");
                if (dataTable.Rows.Count > 0)
                {
                    model.INSTI_GRP_MAS_SLNO = dataTable.Rows[0][0].ToString();
                }
                string query = "INSERT INTO INSTITUTION_GROUP_MAS(INSTI_GRP_MAS_SLNO,GROUP_ID)" +
                               "VALUES(" + model.INSTI_GRP_MAS_SLNO + ", '" + GROUP_ID + "')";
                dbHelper.CmdExecute(query);
                //if (dbHelper.CmdExecute(query) > 0)
                //{
                //    InstitutionGroupMasterSlNo = Convert.ToInt16(model.INSTI_GRP_MAS_SLNO);
                //}
                if (InstitutionGroupDtlList != null)
                {
                    int i = 0;
                    DataTable dataTableDtl =
                            dbHelper.GetDataTable(
                                "SELECT (NVL(MAX(INSTI_GRP_DET_SLNO),0)+1)INSTI_GRP_DET_SLNO FROM INSTITUTION_GROUP_DTL");
                    if (dataTableDtl.Rows.Count > 0)
                    {
                        i = Convert.ToInt32(dataTableDtl.Rows[0][0].ToString());
                        //objInstitutionGroupDtlModel.INSTI_GRP_DET_SLNO = dataTableDtl.Rows[0][0].ToString();
                        //objInstitutionGroupDtlModel.INSTI_GRP_MAS_SLNO = InstitutionGroupMasterSlNo.ToString();//New added Line which is in Query Also
                    }
                    foreach (InstitutionGroupDtlModel objInstitutionGroupDtlModel in InstitutionGroupDtlList)
                    {

                        string querydtl =
                            "INSERT INTO INSTITUTION_GROUP_DTL(INSTI_GRP_DET_SLNO,INSTI_GRP_MAS_SLNO,INSTI_CODE)" +
                            "VALUES('" + i + "', '" + model.INSTI_GRP_MAS_SLNO +
                            "', " + objInstitutionGroupDtlModel.INSTI_CODE + ")";
                        if (dbHelper.CmdExecute(querydtl) > 0)
                        {
                            i++;
                            _vmMsg.Type = Enums.MessageType.Success;
                        }
                    }
                }
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
                    _vmMsg.Msg = "Institution or Group Id is Already Exist.";
                }
                if (ex.Message.Contains("ORA-01438"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Value larger than specified precision allowed.";
                }
            }

            return _vmMsg;
        }








        public ValidationMsg Update(InstitutionGroupMstModel model, string userid)
        {
            _vmMsg = new ValidationMsg();
            try
            {
                #region Update

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew))
                {
                    #region InstitutionGroupMaster

                    string qry = "UPDATE INSTITUTION_GROUP_MAS SET GROUP_ID = '" + model.GROUP_ID + "' WHERE INSTI_GRP_MAS_SLNO = '" + model.INSTI_GRP_MAS_SLNO + "'";
                    if (dbHelper.CmdExecute(qry) > 0)
                    {
                        _vmMsg.Type = Enums.MessageType.Update;
                        _vmMsg.Msg = "Updated Successfully.";
                    }

                    #endregion

                    #region InstitutionGroupDetail

                    if (model.InstitutionGroupDtlList != null)
                    {
                        foreach (InstitutionGroupDtlModel objInstitutionGroupDtlModel in model.InstitutionGroupDtlList)
                        {
                            if (string.IsNullOrEmpty(objInstitutionGroupDtlModel.INSTI_GRP_DET_SLNO))
                            {
                                DataTable dtDtl = dbHelper.GetDataTable("select NVL(MAX(INSTI_GRP_DET_SLNO),0)+1 from INSTITUTION_GROUP_DTL");
                                if (dtDtl.Rows.Count > 0)
                                    objInstitutionGroupDtlModel.INSTI_GRP_DET_SLNO = dtDtl.Rows[0][0].ToString();

                                objInstitutionGroupDtlModel.INSTI_GRP_DET_SLNO = InstitutionGroupMasterSlNo.ToString();


                                qry = "INSERT INTO INSTITUTION_GROUP_DTL(INSTI_GRP_DET_SLNO,INSTI_GRP_MAS_SLNO,INSTI_CODE)" +
                                "VALUES('" + objInstitutionGroupDtlModel.INSTI_GRP_DET_SLNO + "', '" + model.INSTI_GRP_MAS_SLNO + "', '" + objInstitutionGroupDtlModel.INSTI_CODE + "')";
                                if (dbHelper.CmdExecute(qry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Success;
                                }
                            }
                            else
                            {
                                string updateqry = "UPDATE INSTITUTION_GROUP_DTL SET INSTI_CODE = '" + objInstitutionGroupDtlModel.INSTI_CODE + "'," +
                                             "WHERE INSTI_GRP_DET_SLNO = '" + objInstitutionGroupDtlModel.INSTI_GRP_DET_SLNO + "'";
                                if (dbHelper.CmdExecute(updateqry) > 0)
                                {
                                    _vmMsg.Type = Enums.MessageType.Update;
                                    _vmMsg.Msg = "Updated Successfully.";
                                }
                            }
                        }
                    }

                    #endregion

                    scope.Complete();

                    _vmMsg.Type = Enums.MessageType.Update;
                    _vmMsg.Msg = "Updated Successfully.";
                }

                #endregion
            }
            catch (Exception ex)
            {
                _vmMsg.Type = Enums.MessageType.Error;
                _vmMsg.Msg = "Failed to Update.";

                if (ex.Message.Contains("UK_GROUP_ID"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Division already Exist.";
                }
                else if (ex.Message.Contains("UK_INSTI_CODE"))
                {
                    _vmMsg.Type = Enums.MessageType.Error;
                    _vmMsg.Msg = "Institution have already Exist.";
                }
            }
            return _vmMsg;
        }


        public List<InstitutionGroupMstModel> GetSearchMasterList()
        {
            string qry = "SELECT TC1.INSTI_GRP_MAS_SLNO,TC1.GROUP_ID,TL1.GROUP_NAME FROM INSTITUTION_GROUP_MAS TC1 INNER JOIN INSTITUTION_GROUP TL1 ON TC1.GROUP_ID=TL1.GROUP_ID";
            qry = qry + " ORDER BY TC1.INSTI_GRP_MAS_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemMasterList = (from DataRow row in dt.Rows
                              select new InstitutionGroupMstModel
                              {
                                  GROUP_ID = row["GROUP_ID"].ToString(),
                                  GROUP_NAME = row["GROUP_NAME"].ToString(),
                                  INSTI_GRP_MAS_SLNO = row["INSTI_GRP_MAS_SLNO"].ToString()
                              }).ToList();
            return itemMasterList;
        }

        public List<InstitutionGroupDtlModel> GetSearchDtliList(string institutionGroupMstSlNo)
        {
            string qry = "SELECT  IGD.INSTI_GRP_DET_SLNO,IGD.INSTI_GRP_MAS_SLNO,I.INSTI_CODE,I.INSTI_NAME,I.INSTI_TYPE_CODE,IT.INSTI_TYPE_NAME,(  I.ADDRESS1||' '||I.ADDRESS2||' '||I.ADDRESS3||' '||I.ADDRESS4 ) AS ADDRESS  FROM        INSTITUTION_GROUP_DTL IGD   LEFT JOIN   INSTITUTION I ON IGD.INSTI_CODE = I.INSTI_CODE(+) LEFT JOIN   INSTITUTION_TYPE IT  ON I.INSTI_TYPE_CODE = IT.INSTI_TYPE_CODE  WHERE IGD.INSTI_GRP_MAS_SLNO ='" + institutionGroupMstSlNo + "'";
            qry = qry + " ORDER BY IGD.INSTI_GRP_DET_SLNO";

            DataTable dt = dbHelper.GetDataTable(qry);
            itemDetailList = (from DataRow row in dt.Rows
                              select new InstitutionGroupDtlModel
                              {
                                  INSTI_CODE = row["INSTI_CODE"].ToString(),
                                  INSTI_NAME = row["INSTI_NAME"].ToString(),
                                  INSTI_TYPE_CODE = row["INSTI_TYPE_CODE"].ToString(),
                                  INSTI_TYPE_NAME = row["INSTI_TYPE_NAME"].ToString(),
                                  INSTI_GRP_MAS_SLNO = row["INSTI_GRP_MAS_SLNO"].ToString(),
                                  INSTI_GRP_DET_SLNO = row["INSTI_GRP_DET_SLNO"].ToString(),
                                  ADDRESS = row["ADDRESS"].ToString()
                              }).ToList();
            return itemDetailList;
        }





        public ValidationMsg Delete(string institutionGroupMstSlNo)
        {
            ValidationMsg _vmMsg = new ValidationMsg();
            try
            {
                string querydtl = "DELETE FROM INSTITUTION_GROUP_MAS WHERE INSTI_GRP_MAS_SLNO = '" +
                                  institutionGroupMstSlNo + "'";
                if (dbHelper.CmdExecute(querydtl) > 0)
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

        public ValidationMsg DeleteDetailData(string institutionGroupDtlSlNo)
        {
            ValidationMsg _vmMsg = new ValidationMsg();
            try
            {

                string deleteDetailQuery = "DELETE FROM INSTITUTION_GROUP_DTL WHERE INSTI_GRP_DET_SLNO = '" + institutionGroupDtlSlNo + "'";
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


    }
}