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
    public class BabyFilterPatViewBIZ : ICommonBIZ
    {
        private DbBase dao = DbBase.InConn();

        #region ICommonBIZ 成员

        public DataSet doDel(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doNew(DataSet ds)
        {
            DataSet dsRv = null;
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtSave"] != null && ds.Tables["dtSave"].Rows.Count > 0)
                {

                }
                else
                {
                    throw new Exception("传入参数有误");
                }

                DataRow drSave = ds.Tables["dtSave"].Rows[0];

                #region 字段

                string pat_id = drSave["pat_id"].ToString();//
                string pat_itr_id = drSave["pat_itr_id"].ToString();//仪器ID
                string pat_sid = drSave["pat_sid"].ToString();//样本号
                string pat_name = drSave["pat_name"].ToString();//姓名
                string pat_sex = drSave["pat_sex"].ToString();//性别
                string pat_age = drSave["pat_age"].ToString();//年龄
                string pat_age_exp = drSave["pat_age_exp"].ToString();//
                string pat_dep_id = drSave["pat_dep_id"].ToString();//科室代码(现用医院信息)
                string pat_no_id = "'222'";//
                string pat_in_no = "";//筛查号
                string pat_bed_no = drSave["pat_bed_no"].ToString();//床号
                string pat_c_name = drSave["pat_c_name"].ToString();//组合名称
                string pat_tel = drSave["pat_tel"].ToString();//电话号码
                string pat_sam_id = drSave["pat_sam_id"].ToString();//标本ID
                string pat_i_code = drSave["pat_i_code"].ToString();//录入者工号
                DateTime time_pat_date = (DateTime)drSave["pat_date"];//生成日期
                string pat_date = time_pat_date.Date.AddHours(8).AddMinutes(3).ToString();
                DateTime time_pat_sample_date = (DateTime)drSave["pat_sample_date"];//采集日期
                string pat_sample_date = time_pat_sample_date.Date.AddHours(8).ToString();
                string pat_jy_date = time_pat_date.Date.AddHours(8).AddMinutes(4).ToString();//检验日期
                string pat_sam_rem = drSave["pat_sam_rem"].ToString();//标本备注
                string pat_dep_name = drSave["pat_dep_name"].ToString();//科室名称(现用医院信息)
                string pat_rem = drSave["pat_rem"].ToString();//标本状态

                string pat_birthday = "";//出生日期
                if (!string.IsNullOrEmpty(drSave["pat_birthday"].ToString()))
                {
                    pat_birthday = drSave["pat_birthday"].ToString();
                }

                string pat_com_id = drSave["pat_com_id"].ToString();//组合id


                #endregion

                #region 赋值字段

                pat_name = "'" + pat_name + "'";
                pat_sex = "'" + pat_sex + "'";
                pat_age_exp = "'" + pat_age_exp + "'";
                pat_dep_id = "'" + pat_dep_id + "'";
                pat_bed_no = "'" + pat_bed_no + "'";
                pat_c_name = "'" + pat_c_name + "'";
                pat_tel = "'" + pat_tel + "'";
                pat_sam_id = "'" + pat_sam_id + "'";
                pat_i_code = "'" + pat_i_code + "'";
                pat_date = "'" + pat_date + "'";
                pat_sample_date = "'" + pat_sample_date + "'";
                pat_jy_date = "'" + pat_jy_date + "'";
                pat_sam_rem = "'" + pat_sam_rem + "'";
                pat_dep_name = "'" + pat_dep_name + "'";
                pat_rem = "'" + pat_rem + "'";

                #endregion

                #region 检查pat_id是否存在

                DataTable dtPatientData = this.GetPatientDataByID(pat_id);//获取当前pat_id的信息

                if (dtPatientData != null && dtPatientData.Rows.Count > 0)
                {
                    throw new Exception("当前样本号[" + pat_sid + "]已存在");
                }
                else
                {
                    //获取一个新筛查号
                    pat_in_no = new dcl.svr.result.PatientEnterService().GetPatFiltercodeNewNo();

                    if (string.IsNullOrEmpty(pat_in_no))
                    {
                        throw new Exception("获取一个新筛查号失败");
                    }
                }

                #endregion


                #region 新增病人信息SQL语句

                string sqlInsertPatientInfo = string.Format(@"
INSERT INTO patients_newborn
           ([pat_id]
           ,[pat_itr_id]
           ,[pat_sid]
           ,[pat_name]
           ,[pat_sex]
           ,[pat_age]
           ,[pat_age_exp]
           ,[pat_dep_id]
           ,[pat_no_id]
           ,[pat_in_no]
           ,[pat_admiss_times]
           ,[pat_bed_no]
           ,[pat_c_name]
           ,[pat_diag]
           ,[pat_tel]
           ,[pat_address]
           ,[pat_pre_week]
           ,[pat_sam_id]
           ,[pat_i_code]
           ,[pat_ctype]
           ,[pat_flag]
           ,[pat_exp]
           ,[pat_pid]
           ,[pat_date]
           ,[pat_sdate]
           ,[pat_emp_id]
           ,[pat_bar_code]
           ,[pat_sample_date]
           ,[pat_apply_date]
           ,[pat_jy_date]
           ,[pat_sample_part]
           ,[pat_ori_id]
           ,[pat_comment]
           ,[pat_modified_times]
           ,[pat_sam_rem]
           ,[pat_sample_receive_date]
           ,[pat_dep_name]
           ,[pat_ward_name]
           ,[pat_ward_id]
           ,[pat_app_no]
           ,[pat_emp_company_name]
           ,[pat_reach_date]
           ,[pat_rem],pat_upid,pat_prt_flag)
     VALUES
           ('{0}'--[pat_id]
           ,'{1}'--[pat_itr_id]
           ,'{2}'--[pat_sid]
           ,{3}--[pat_name]
           ,{4}--[pat_sex]
           ,{5}--[pat_age]
           ,{6}--[pat_age_exp]
           ,{7}--[pat_dep_id]
           ,{8}--[pat_no_id]
           ,'{9}'--[pat_in_no]
           ,0
           ,{10}--[pat_bed_no]
           ,{11}--[pat_c_name]
           ,''
           ,''
           ,{12}--[pat_tel]
           ,''
           ,{13}--[pat_sam_id]
           ,{14}--[pat_i_code]
           ,'1'
           ,0--[pat_flag]
           ,''--[pat_exp]
           ,''--[pat_pid]
           ,{15}--[pat_date]
           ,{16}--[pat_sdate]
           ,''--[pat_emp_id]
           ,''--[pat_bar_code]
           ,{17}--[pat_sample_date]
           ,{18}--[pat_apply_date]
           ,{19}--[pat_jy_date]
           ,''--[pat_sample_part]
           ,'107'--[pat_ori_id]
           ,''--[pat_comment]
           ,0--[pat_modified_times]
           ,{20}--[pat_sam_rem]
           ,{21}--[pat_sample_receive_date]
           ,{22}--[pat_dep_name]
           ,''--[pat_ward_name]
           ,''--[pat_ward_id]
           ,''--[pat_app_no]
           ,''--[pat_emp_company_name]
           ,{23}--[pat_reach_date]
           ,{24}--[pat_rem]
           ,{25},1
)", pat_id
                              , pat_itr_id
                              , pat_sid
                              , pat_name
                              , pat_sex
                              , pat_age
                              , pat_age_exp
                              , pat_dep_id
                              , pat_no_id
                              , pat_in_no
                              , pat_bed_no
                              , pat_c_name
                              , pat_tel
                              , pat_sam_id
                              , pat_i_code
                              , pat_date
                              , pat_date
                              , pat_sample_date
                              , pat_sample_date
                              , pat_jy_date
                              , pat_sam_rem
                              , pat_sample_date
                              , pat_dep_name
                              , pat_sample_date
                              , pat_rem, pat_in_no);

                #endregion

                #region 新增patients_mi表SQL语句

                string sqlInsertPatientMiInfo = "";

                if (!string.IsNullOrEmpty(pat_com_id))
                {
                    sqlInsertPatientMiInfo = string.Format(@"
INSERT INTO patients_mi_newborn
           ([pat_id]
           ,[pat_com_id]
           ,[pat_seq])
     VALUES
           ({0}
           ,{1}
           ,0)", pat_id, pat_com_id);
                }

                #endregion

                //新增病人表patients的语句
                SqlCommand cmdAddPatients = new SqlCommand(sqlInsertPatientInfo);

                using (DBHelper helper = DBHelper.BeginTransaction())//事务
                {
                    helper.ExecuteNonQuery(cmdAddPatients);//新增病人表信息

                    if (!string.IsNullOrEmpty(sqlInsertPatientMiInfo))
                    {
                        helper.ExecuteNonQuery(new SqlCommand(sqlInsertPatientMiInfo));//新增patients_mi信息
                    }

                    helper.Commit();//提交事务
                }

                #region 新增扩展信息

                string[] patExtColName = new string[1];//列名
                string[] patExtColValue = new string[1];//列值

                patExtColName[0] = "pat_birthday";//出生日期--列名
                patExtColValue[0] = "null";//列值

                if (!string.IsNullOrEmpty(pat_birthday))
                {
                    patExtColValue[0] = "'" + pat_birthday + "'";
                }

                new dcl.svr.result.PatInsertBLL().AddOrUpdatePatientExt(patExtColName, patExtColValue, pat_id);

                #endregion

                drSave["pat_in_no"] = pat_in_no;//返回筛查号
                drSave["rv_flag"] = "1";

                DataTable dtRv = drSave.Table.Clone();
                dtRv.TableName = "dtSave";

                dtRv.Rows.Add(drSave.ItemArray);
                dsRv = new DataSet();
                dsRv.Tables.Add(dtRv);
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "doNew", ex.ToString());
                DataTable dtEx = new DataTable("dtEx");
                dtEx.Columns.Add("msg");
                dtEx.Rows.Add(new object[]{ex.Message});
                if (dsRv == null) dsRv = new DataSet();
                dsRv.Tables.Clear();//清除
                dsRv.Tables.Add(dtEx);
            }

            return dsRv;
        }

        /// <summary>
        /// 获取病人表patients的信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        private System.Data.DataTable GetPatientDataByID(string pat_id)
        {
            if (string.IsNullOrEmpty(pat_id))
                pat_id = "";

            string sqlSelect = string.Format("select pat_id,pat_itr_id,pat_sid,pat_flag,pat_date from patients_newborn where pat_id='{0}'", pat_id);
            DBHelper helper = new DBHelper();
            DataTable dtPatientExt = helper.GetTable(sqlSelect);
            dtPatientExt.TableName = "patients";

            return dtPatientExt;
        }

        public DataSet doOther(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doSearch(DataSet ds)
        {
            throw new NotImplementedException();
        }

        public DataSet doUpdate(DataSet ds)
        {
            DataSet dsRv = null;
            try
            {
                if (ds != null && ds.Tables.Count > 0 && ds.Tables["dtSave"] != null && ds.Tables["dtSave"].Rows.Count > 0)
                {

                }
                else
                {
                    throw new Exception("传入参数有误");
                }

                DataRow drSave = ds.Tables["dtSave"].Rows[0];

                #region 字段

                string pat_id = drSave["pat_id"].ToString();//
                string pat_itr_id = drSave["pat_itr_id"].ToString();//仪器ID
                string pat_sid = drSave["pat_sid"].ToString();//样本号
                string pat_name = drSave["pat_name"].ToString();//姓名
                string pat_sex = drSave["pat_sex"].ToString();//性别
                string pat_age = drSave["pat_age"].ToString();//年龄
                string pat_age_exp = drSave["pat_age_exp"].ToString();//
                string pat_dep_id = drSave["pat_dep_id"].ToString();//科室代码(现用医院信息)
                string pat_no_id = "'222'";//
                string pat_in_no = "";//筛查号
                string pat_bed_no = drSave["pat_bed_no"].ToString();//床号
                string pat_c_name = drSave["pat_c_name"].ToString();//组合名称
                string pat_tel = drSave["pat_tel"].ToString();//电话号码
                string pat_sam_id = drSave["pat_sam_id"].ToString();//标本ID
                string pat_i_code = drSave["pat_i_code"].ToString();//录入者工号
                DateTime time_pat_date = (DateTime)drSave["pat_date"];//生成日期
                string pat_date = time_pat_date.Date.AddHours(8).AddMinutes(3).ToString();
                DateTime time_pat_sample_date = (DateTime)drSave["pat_sample_date"];//采集日期
                string pat_sample_date = time_pat_sample_date.Date.AddHours(8).ToString();
                string pat_jy_date = time_pat_date.Date.AddHours(8).AddMinutes(4).ToString();//检验日期
                string pat_sam_rem = drSave["pat_sam_rem"].ToString();//标本备注
                string pat_dep_name = drSave["pat_dep_name"].ToString();//科室名称(现用医院信息)
                string pat_rem = drSave["pat_rem"].ToString();//标本状态

                string pat_birthday = "";//出生日期
                if (!string.IsNullOrEmpty(drSave["pat_birthday"].ToString()))
                {
                    pat_birthday = drSave["pat_birthday"].ToString();
                }

                string pat_com_id = drSave["pat_com_id"].ToString();//组合id


                #endregion

                #region 赋值字段

                pat_name = "'" + pat_name + "'";
                pat_sex = "'" + pat_sex + "'";
                pat_age_exp = "'" + pat_age_exp + "'";
                pat_dep_id = "'" + pat_dep_id + "'";
                pat_bed_no = "'" + pat_bed_no + "'";
                pat_c_name = "'" + pat_c_name + "'";
                pat_tel = "'" + pat_tel + "'";
                pat_sam_id = "'" + pat_sam_id + "'";
                pat_i_code = "'" + pat_i_code + "'";
                pat_date = "'" + pat_date + "'";
                pat_sample_date = "'" + pat_sample_date + "'";
                pat_jy_date = "'" + pat_jy_date + "'";
                pat_sam_rem = "'" + pat_sam_rem + "'";
                pat_dep_name = "'" + pat_dep_name + "'";
                pat_rem = "'" + pat_rem + "'";


                #endregion

                #region 检查pat_id是否存在

                DataTable dtPatientData = this.GetPatientDataByID(pat_id);//获取当前pat_id的信息

                if (dtPatientData != null && dtPatientData.Rows.Count > 0)
                {
                    if (dtPatientData.Rows[0]["pat_flag"].ToString() != "0")
                    {
                        throw new Exception("当前样本号[" + pat_sid + "],不是【未检验】信息");
                    }
                }
                else
                {
                    throw new Exception("当前样本号[" + pat_sid + "]不存在");
                }

                #endregion


                #region 修改病人信息SQL语句

                string sqlUpdatePatientInfo = string.Format(@"
UPDATE patients_newborn
   SET 
       [pat_name] ={1}
      ,[pat_sex] ={2}
      ,[pat_age] ={3}
      ,[pat_age_exp] = {4}
      ,[pat_dep_id] = {5}
      ,[pat_bed_no] = {6}
      ,[pat_c_name] = {7}
      ,[pat_tel] = {8}
      ,[pat_sam_id] = {9}
      ,[pat_i_code] = {10}
      ,[pat_sdate] = {11}
      ,[pat_sample_date] = {12}
      ,[pat_apply_date] = {13}
      ,[pat_sample_receive_date] = {14}
      ,[pat_dep_name] = {15}
      ,[pat_reach_date] = {16}
      ,[pat_rem] = {17}
 WHERE pat_id='{0}' ", pat_id
                              , pat_name
                              , pat_sex
                              , pat_age
                              , pat_age_exp
                              , pat_dep_id
                              , pat_bed_no
                              , pat_c_name
                              , pat_tel
                              , pat_sam_id
                              , pat_i_code
                              , pat_sample_date
                              , pat_sample_date
                              , pat_sample_date
                              , pat_sample_date
                              , pat_dep_name
                              , pat_sample_date
                              , pat_rem);

                #endregion

                #region 修改patients_mi表SQL语句

//                string sqlUpdatePatientMiInfo = "";

//                if (!string.IsNullOrEmpty(pat_com_id))
//                {
//                    sqlUpdatePatientMiInfo = string.Format(@"
//UPDATE patients_mi
//   SET 
//      [pat_com_id] = '{1}'
// WHERE pat_id='{0}'", pat_id, pat_com_id);
//                }

                string sqlDeletePatientMiInfo = "";
                string sqlInsertPatientMiInfo = "";

                sqlDeletePatientMiInfo = string.Format(@"
DELETE FROM patients_mi_newborn
      WHERE pat_id='{0}' ", pat_id);

                if (!string.IsNullOrEmpty(pat_com_id))
                {
                    sqlInsertPatientMiInfo = string.Format(@"
INSERT INTO patients_mi_newborn
           ([pat_id]
           ,[pat_com_id]
           ,[pat_seq])
     VALUES
           ({0}
           ,{1}
           ,0)", pat_id, pat_com_id);
                }

                #endregion

                //修改病人表patients的语句
                SqlCommand cmdUpdatePatients = new SqlCommand(sqlUpdatePatientInfo);

                using (DBHelper helper = DBHelper.BeginTransaction())//事务
                {
                    helper.ExecuteNonQuery(cmdUpdatePatients);//修改病人表信息

                    if (!string.IsNullOrEmpty(sqlDeletePatientMiInfo))
                    {
                        helper.ExecuteNonQuery(new SqlCommand(sqlDeletePatientMiInfo));//删除patients_mi信息
                    }

                    if (!string.IsNullOrEmpty(sqlInsertPatientMiInfo))
                    {
                        helper.ExecuteNonQuery(new SqlCommand(sqlInsertPatientMiInfo));//添加patients_mi信息
                    }

                    helper.Commit();//提交事务
                }

                #region 修改扩展信息

                string[] patExtColName = new string[1];//列名
                string[] patExtColValue = new string[1];//列值

                patExtColName[0] = "pat_birthday";//出生日期--列名
                patExtColValue[0] = "null";//列值

                if (!string.IsNullOrEmpty(pat_birthday))
                {
                    patExtColValue[0] = "'" + pat_birthday + "'";
                }

                new dcl.svr.result.PatInsertBLL().AddOrUpdatePatientExt(patExtColName, patExtColValue, pat_id);

                #endregion

                drSave["rv_flag"] = "1";

                DataTable dtRv = drSave.Table.Clone();
                dtRv.TableName = "dtSave";

                dtRv.Rows.Add(drSave.ItemArray);
                dsRv = new DataSet();
                dsRv.Tables.Add(dtRv);
            }
            catch (Exception ex)
            {
                dcl.root.logon.Logger.WriteException(this.GetType().Name, "doNew", ex.ToString());
                DataTable dtEx = new DataTable("dtEx");
                dtEx.Columns.Add("msg");
                dtEx.Rows.Add(new object[] { ex.Message });
                if (dsRv == null) dsRv = new DataSet();
                dsRv.Tables.Clear();//清除
                dsRv.Tables.Add(dtEx);
            }

            return dsRv;
        }

        public DataSet doView(DataSet ds)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
