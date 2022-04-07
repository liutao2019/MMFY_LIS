using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.svr.cache;
using dcl.servececontract;
using System.Data;
using dcl.svr.root.com;
using dcl.common;
using dcl.root.dto;

namespace dcl.svr.result
{
    class PatientEditToolBIZ : dcl.svr.root.com.ICommonBIZ
    {
        private DbBase dao = DbBase.InconTiem(300);

        #region ICommonBIZ 成员

        public DataSet doDel(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doNew(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doOther(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doSearch(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtPat = ds.Tables["dtPatData"];

                string pat_ID = dtPat.Rows[0]["patID"].ToString();
                string pat_Name = dtPat.Rows[0]["patName"].ToString();

                string sqlwhereID = "";
                string sqlwhereName = "";

                if (string.IsNullOrEmpty(pat_ID) || string.IsNullOrEmpty(pat_Name))
                {
                    if (string.IsNullOrEmpty(pat_ID) && string.IsNullOrEmpty(pat_Name))
                    {
                        sqlwhereID = " 1=2 ";//如果都为空,则查空
                    }
                    else if (string.IsNullOrEmpty(pat_Name))//如果名称为空,则用ID查
                    {
                        sqlwhereID = string.Format(" pat_in_no in('{0}') ", pat_ID);
                    }
                    else//如果ID为空,则用名称查
                    {
                        sqlwhereName = string.Format(" pat_name in('{0}') ", pat_Name);
                    }
                }
                else//如果都不为空用and查
                {
                    sqlwhereID = string.Format(" pat_in_no in('{0}') ", pat_ID);
                    sqlwhereName = string.Format(" and pat_name in('{0}') ", pat_Name);
                }


                string strSql = @"
SELECT 
    pat_sid,
    cast(pat_sid as bigint) as pat_sid_int,
    pat_sex,
    pat_sex_name= case when pat_sex = '1' then '男'
                       when pat_sex = '2' then '女'
                       else '' end,
    pat_date,
    pat_name,
    dbo.getAge(pat_age_exp) pat_age,
    pat_age_exp,
    pat_id,
    pat_itr_id,
    dict_sample.sam_name as pat_sam_name,
    pat_sam_id,
    dict_instrmt.itr_mid,
    pat_c_name,
    pat_in_no,
    pat_jy_date,
    patients.pat_diag,
    pat_chk_date,
    pat_report_date,
    pat_chk_code,pat_report_code,
    pat_doc_id,
    doc1.doc_name as pat_doc_name,
    pat_sdate,
    user1.username as pat_check_name,
    user2.username as pat_report_name,
    userRec.username as pat_i_name

FROM patients
    Left join dict_sample on patients.pat_sam_id = dict_sample.sam_id
    LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
    LEFT OUTER JOIN dict_no_type ON dict_no_type.no_id = patients.pat_no_id and dict_no_type.no_del = 0
    LEFT OUTER JOIN dict_doctor doc1 ON doc1.doc_id = patients.pat_doc_id
    LEFT OUTER JOIN poweruserinfo user1 on user1.loginid = patients.pat_chk_code
    LEFT OUTER JOIN poweruserinfo user2 on user2.loginid = patients.pat_report_code
    LEFT OUTER JOIN poweruserinfo userRec on userRec.loginid = patients.pat_i_code 

where 
    {0}
    {1}
order by pat_date desc";

                string sql = null;
                sql = string.Format(strSql, sqlwhereID, sqlwhereName);

                if (!string.IsNullOrEmpty(sql))
                {
                    DataTable an = dao.GetDataSet(sql).Tables[0];
                    an.TableName = "dtPatData";

                    //foreach (DataRow drPat in an.Rows)
                    //{
                    //    if (drPat["pat_age_exp"] != null && drPat["pat_age_exp"] != DBNull.Value)
                    //    {
                    //        string patage = drPat["pat_age_exp"].ToString();

                    //        patage = AgeConverter.TrimZeroValue(patage);
                    //        patage = AgeConverter.ValueToText(patage);
                    //        drPat["pat_age_exp"] = patage;
                    //    }

                    //}

                    result.Tables.Add(an.Copy());
                    return result;

                }

            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("获取信息出错!", ex.ToString())); ;
            }
            return result;
        }

        public DataSet doUpdate(DataSet ds)
        {
            DataSet result = new DataSet();
            try
            {
                DataTable dtUpdate = ds.Tables["patients"];
                ArrayList arrUpdate = dao.GetUpdateSql(dtUpdate, new string[] { "pat_id" });
                dao.DoTran(arrUpdate);

                DataTable dtSysOperationLog = ds.Tables["sysoperationlog"];
                if (dtSysOperationLog != null)
                {
                    DateTime now = ServerDateTime.GetDatabaseServerDateTime();
                    foreach (DataRow drSysOperationLog in dtSysOperationLog.Rows)
                    {
                        drSysOperationLog["OperationTime"] = now;
                    }

                    ArrayList arrInsert = dao.GetInsertSql(dtSysOperationLog);
                    dao.DoTran(arrInsert);
                }
            }
            catch (Exception ex)
            {
                result.Tables.Add(CommonBIZ.createErrorInfo("修改信息出错!", ex.ToString()));
            }
            return result;
        }

        public DataSet doView(DataSet ds)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
