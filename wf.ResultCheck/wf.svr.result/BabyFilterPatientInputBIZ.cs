using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dcl.svr.root.com;
using dcl.root.dto;
using System.Collections;
using lis.dto;
using System.Data.OleDb;
using System.Data.SqlClient;
using dcl.root.dac;
using dcl.common;
using dcl.pub.entities;
using Lib.DAC;
using dcl.svr.cache;

namespace dcl.svr.result
{
    public class BabyFilterPatientInputBIZ : ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

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
            DataSet dsResult = null;
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtWhere"] != null)
                {
                    string strSqlWhere = ds.Tables["dtWhere"].Rows[0]["colWhere"].ToString(); //添加

                    string strSQL = string.Format(@"select patients.pat_id,0 as pat_select,
patients.pat_itr_id,
patients.pat_sid,
cast(pat_sid as bigint) as sid,
patients.pat_name,
pat_sex_name= case when pat_sex = '1' then '男'
                                   when pat_sex = '2' then '女'
                                   else '' end,
(case patients.pat_sex when '1' then '男' when '2' then '女' else '未知' end) pat_sex_text,
pat_flag_name = case when pat_flag = 0 then '未审核'
                                     when pat_flag = 1 then '已审核'
                                     when pat_flag = 2 then '已报告'
                                     when pat_flag = 4 then '已打印'
                                     else '未审核' end,
patients.pat_sex,
dbo.getAge(patients.pat_age_exp) pat_age,
patients.pat_age_exp,
patients.pat_dep_id,
dict_depart.dep_name,
patients.pat_no_id,
(case when patients.pat_no_id='222' then patients.pat_in_no else '' end) as 'pat_in_no',
patients.pat_bed_no,
patients.pat_c_name,
patients.pat_sam_id,
dict_sample.sam_name as pat_sam_name,
patients.pat_flag,
patients.pat_date,
patients.pat_sdate,
patients.pat_social_no,
patients.pat_sam_rem,
patients.pat_dep_name,
patients.pat_i_code,
patients.pat_rem,
PowerUserInfo1.userName as 'pat_i_name',
patients.pat_tel,
 pat_host_order,
patients_ext.pat_birthday,
patients.pat_sample_date,
pat_ctype,
pat_ctype_name = case when pat_ctype = '1' then '常规'
                                      when pat_ctype = '2' then '急查'
                                      when pat_ctype = '3' then '保密'
                                      else '' end,
(select top 1 patients_mi.pat_com_id from patients_mi where patients_mi.pat_id=patients.pat_id) as pat_com_id,
(case when patients.pat_flag=0 then '未检验'
 when patients.pat_flag=1 then '已检验'
 when patients.pat_flag=2 then '已审核'
 when patients.pat_flag=4 then '已打印' else '' end) as 'pat_flag_caption',pat_upid,pat_prt_flag
from patients_newborn  patients
left join patients_ext on patients_ext.pat_id=patients.pat_id
left join dict_depart on patients.pat_dep_id=dict_depart.dep_code
left join dict_sample on patients.pat_sam_id=dict_sample.sam_id
left join PowerUserInfo PowerUserInfo1 on patients.pat_i_code=PowerUserInfo1.loginId
where 1=1
{0}", strSqlWhere);


                    dsResult = dao.GetDataSet(strSQL);
                    dsResult.Tables[0].TableName = "dtPatients";
                }


                if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtTime"] != null)
                {
                    string strSqlWhere = ds.Tables["dtTime"].Rows[0]["colWhere"].ToString(); //添加

                    string strSQL = string.Format(@"SELECT distinct DATENAME(Year,pat_date)+'-'+DATENAME(Month,pat_date)+'-'+DATENAME(Day,pat_date) as sel_date,pat_itr_id from patients_newborn
                                                        where 1=1
                                                         {0} ", strSqlWhere);


                    dsResult = dao.GetDataSet(strSQL);
                    dsResult.Tables[0].TableName = "dtPatientsTime";

                }

            }
            catch (Exception objEx)
            {
                dcl.root.logon.Logger.WriteException("BabyFilterPatientInputBIZ", "doSearch", objEx.ToString());
            }

            return dsResult;
        }

        public DataSet doUpdate(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doView(DataSet ds)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
