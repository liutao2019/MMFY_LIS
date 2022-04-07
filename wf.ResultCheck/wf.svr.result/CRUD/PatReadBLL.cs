using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using Lib.DAC;
using dcl.svr.cache;
using dcl.root.dac;
using dcl.root.logon;
using lis.dto;
using System.Diagnostics;
using dcl.common;
using System.Data.SqlClient;
using dcl.svr.framedic;
using dcl.pub.entities;
using System.Collections;
using dcl.svr.result.CRUD;
using dcl.svr.frame;

namespace dcl.svr.result
{
    public class PatReadBLL
    {
        public static PatReadBLL NewInstance
        {
            get
            {
                return new PatReadBLL();
            }
        }

        /// <summary>
        /// 获取病人信息和检验组合信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public DataSet GetPatientWithCombine(string pat_id)
        {
            DataSet dsData = new DataSet();
            DataTable dtPatInfo = GetPatientInfo(pat_id);
            DataTable dtPatCombine = GetPatientCombine(pat_id);

            dsData.Tables.Add(dtPatInfo);
            dsData.Tables.Add(dtPatCombine);

            return dsData;
        }

        /// <summary>
        /// 获取普通结果病人资料（病人基本资料、病人检验组合、病人普通结果、病人历史三次的历史结果）
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataSet GetCommonPatinentData(string pat_id)
        {
            DataSet dsData = GetPatientWithCombine(pat_id);
            DataTable dtPatHistorRes = new DataTable();//历史结果信息
            DataTable dtPatResult = GetPatientCommonResult(pat_id, false, out dtPatHistorRes, dsData);


            dsData.Tables.Add(dtPatResult);
            dsData.Tables.Add(dtPatHistorRes);//新加入历史结果


            return dsData;
        }


        /// <summary>
        /// 新生儿获取普通结果病人资料（病人基本资料、病人检验组合、病人普通结果、病人历史三次的历史结果）
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataSet GetCommonPatinentDataForBf(string pat_id)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            DataSet dsData = new DataSet();
            DataTable dtPatInfo = GetPatientInfoForBf(pat_id);
            DataTable dtPatCombine = GetPatientCombineForBf(pat_id);
            dsData.Tables.Add(dtPatInfo);
            dsData.Tables.Add(dtPatCombine);
            DataTable dtPatHistorRes = new DataTable();//历史结果信息
            DataTable dtPatResult = GetPatientCommonResultForBf(pat_id, true, out dtPatHistorRes);


            dsData.Tables.Add(dtPatResult);
            dsData.Tables.Add(dtPatHistorRes);//新加入历史结果

            sw.Stop();
            Logger.Debug(string.Format("服务器端获取病人资料 {0}ms", sw.ElapsedMilliseconds));

            return dsData;
        }
        //public byte[] GetCommonPatientData_compress(string pat_id)
        //{
        //    DataSet ds = GetCommonPatinentData(pat_id);
        //    byte[] data = DataSetSerialization.Compress(ds);
        //    return data;
        //}

        /// <summary>
        /// 获取描述结果病人资料（病人基本资料、病人检验组合、病人描述结果）
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataSet GetDescPatientData(string patID)
        {
            DataSet dsData = new DataSet();
            DataTable dtPatInfo = GetPatientInfo(patID);
            DataTable dtPatCombine = GetPatientCombine(patID);
            DataTable dtPatDescResult = GetPatDescResult(patID);

            dsData.Tables.Add(dtPatInfo);
            dsData.Tables.Add(dtPatCombine);
            dsData.Tables.Add(dtPatDescResult);

            return dsData;
        }

        /// <summary>
        /// 获取病人基本信息
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetPatientInfo(string patID)
        {
            try
            {
                //2013-09-06 李进 金域接口 第三方的检验者在系统找不到对应的记录，如果发现为空，直接赋值为：pat_i_code
                //修改地方：PowerUserInfo.userName AS pat_i_code_name,
                string sqlSelect =
    string.Format(@"
SELECT
patients.*,
dict_instrmt.itr_ptype, 
dict_instrmt.itr_mid,
dict_instrmt.itr_name,
dict_sample.sam_name,
dict_checkb.chk_cname,
dict_doctor.doc_name, 
--PowerUserInfo.userName AS pat_i_code_name,
case 
when PowerUserInfo.userName is null then patients.pat_i_code 
else PowerUserInfo.userName  end as pat_i_code_name,
dict_no_type.no_name, 
dict_origin.ori_name,
pat_ctype_name = '',--检查类型名称
bc_status = '',
bc_ctype = '',
bc_print_flag = 0,
dict_depart.dep_tel,
'' as bc_merge_comid,
dbo.getAge(patients.pat_age_exp) pat_age_txt,
pat_sex_name = case when pat_sex = '1' then '男'
                    when pat_sex = '2' then '女'
                    else '' end
FROM patients 
    LEFT OUTER JOIN dict_origin ON patients.pat_ori_id = dict_origin.ori_id 
    LEFT OUTER JOIN dict_no_type ON patients.pat_no_id = dict_no_type.no_id
    LEFT OUTER JOIN PowerUserInfo ON patients.pat_i_code = PowerUserInfo.loginId
    LEFT OUTER JOIN dict_doctor ON patients.pat_doc_id = dict_doctor.doc_id
    LEFT OUTER JOIN dict_checkb ON patients.pat_chk_id = dict_checkb.chk_id
    LEFT OUTER JOIN dict_sample ON patients.pat_sam_id = dict_sample.sam_id
    LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
    LEFT OUTER JOIN dict_depart ON patients.pat_dep_id = dict_depart.dep_code
    
WHERE patients.pat_id ='{0}'", patID);

                DBHelper helper = new DBHelper();

                Logger.Trace("pat_sid=" + patID);
                DataTable dtPat = helper.GetTable(sqlSelect);

                FuncLibBIZ bllFuncLibBIZ = new FuncLibBIZ();
                DataTable dtPatCType = bllFuncLibBIZ.getPat_ctype();

                foreach (DataRow drPat in dtPat.Rows)
                {
                    if (drPat["pat_ctype"] != null && drPat["pat_ctype"] != DBNull.Value && drPat["pat_ctype"].ToString().Trim(null) != string.Empty)
                    {
                        string pat_ctype = drPat["pat_ctype"].ToString();
                        DataRow[] drs = dtPatCType.Select(string.Format("id='{0}'", pat_ctype));
                        if (drs.Length > 0)
                        {
                            drPat["pat_ctype_name"] = drs[0]["value"];
                        }
                    }
                }

                dtPat.TableName = PatientTable.PatientInfoTableName;

                return dtPat;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人信息出错,病人ID:" + patID, ex.ToString());
                throw;
            }
        }



        /// <summary>
        /// 获取病人组合明细
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetPatientCombine(string patID)
        {
            try
            {
                //2012-03-29 李杰 添加查询dict_combine.com_seq这一列
                //2012-04-19 林志宏 添加这一列会有问题，如果添加了五组合项目则会导致排序错误
                string sql = string.Format(@"
SELECT 
dict_combine.com_name as pat_com_name,
dict_combine.com_seq,
patients_mi.*
FROM patients_mi 
INNER JOIN dict_combine ON patients_mi.pat_com_id = dict_combine.com_id
WHERE (patients_mi.pat_id = '{0}')
order by pat_seq asc
", patID);
                DBHelper helper = new DBHelper();

                Logger.Debug(string.Format("获取病人组合信息patID={0}", patID));
                DataTable dtCombine = helper.GetTable(sql);
                dtCombine.TableName = PatientTable.PatientCombineTableName;

                return dtCombine;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人组合信息出错,patID=" + patID, ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 获取病人普通结果
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetPatientCommonResult(string patID, bool getHistory)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DBHelper helper = new DBHelper();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string selectPatState = string.Format("select top 1 pat_id,pat_sid,pat_flag,pat_sam_id,pat_itr_id,pat_sam_rem,pat_sex,pat_age,pat_weight from patients where pat_id = '{0}'", patID);

                DataTable dtPat = helper.GetTable(selectPatState);

                EntityPatient2 entityPatient = new EntityPatient2();

                if (dtPat.Rows.Count > 0)
                {
                    entityPatient.pat_id = dtPat.Rows[0]["pat_id"].ToString();
                    entityPatient.pat_sid = dtPat.Rows[0]["pat_sid"].ToString();
                    entityPatient.pat_itr_id = dtPat.Rows[0]["pat_itr_id"].ToString();
                    entityPatient.pat_sam_id = dtPat.Rows[0]["pat_sam_id"].ToString();
                    entityPatient.pat_sex = dtPat.Rows[0]["pat_sex"].ToString();
                    entityPatient.pat_weight = dtPat.Rows[0]["pat_weight"].ToString();
                    try
                    {
                        if (!Compare.IsEmpty(dtPat.Rows[0]["pat_age"]))
                        {
                            entityPatient.pat_age = Convert.ToInt32(dtPat.Rows[0]["pat_age"]);
                        }
                    }
                    catch
                    {
                    }


                    if (!Compare.IsEmpty(dtPat.Rows[0]["pat_flag"]))
                    {
                        entityPatient.pat_flag = Convert.ToInt32(dtPat.Rows[0]["pat_flag"]);
                    }
                    else
                    {
                        entityPatient.pat_flag = 0;
                    }


                    if (entityPatient.pat_flag == 0)
                    {
                        string sampRem = string.Empty;
                        if (!Compare.IsEmpty(dtPat.Rows[0]["pat_sam_rem"]))
                        {
                            sampRem = dtPat.Rows[0]["pat_sam_rem"].ToString();
                        }
                        GenerateAutoCalItem(entityPatient, entityPatient.pat_sam_id, sampRem);
                        //FillNotNullResult(entityPatient);
                        dic = UpdateResultNotCombineItem(patID);
                    }
                }

                //if (Compare.IsNullOrDBNull(objState) || objState.ToString() == "0")
                //{
                //    GenerateAutoCalItem(patID);
                //    FillNotNullResult(patID);
                //    UpdateResultNotCombineItem(patID);
                //}

                string sql = string.Format(@"
select
resulto.res_key,--结果主键标识
resulto.res_id,--结果ID(病人ID)
resulto.res_itr_id,--仪器ID
resulto.res_sid,--样本号
resulto.res_itm_id,--项目ID
dict_item.itm_ecd as res_itm_ecd,--项目代码
dict_item.itm_name as res_itm_name,--项目名称
resulto.res_itm_rep_ecd,--项目报告编号
resulto.res_chr,--结果
resulto.res_od_chr,--OD结果
resulto.res_cast_chr,--数值结果
resulto.res_price,--价格
resulto.res_ref_exp,--阳性标志 （提示）
resulto.res_ref_flag,--阳性标志(偏高，偏低，阳性，正常,etc)
resulto.res_meams,--实验方法
resulto.res_date,--结果日期
resulto.res_flag,--有效标志
dict_instrmt_warningsigns.itr_warn_trandate,--项目状态
resulto.res_itr_ori_id,--原始仪器id
resulto.res_ref_type,--参考值类型
res_type = case when dict_item.itm_cal_flag = 1 then 2
                else resulto.res_type end, --结果类型
resulto.res_rep_type,--报告类型
resulto.res_com_id,--组合ID
res_com_name = case when (resulto.res_com_id is null or resulto.res_com_id ='') then '无组合'
                    else  dict_combine.com_name
                    end,--组合名称
--res_com_seq = case when patients_mi.pat_seq is null then 9999
--                   else patients_mi.pat_seq
--                   end,
--res_com_seq = isnull(dict_combine.com_seq,99999),
res_com_seq = isnull(dict_combine.com_seq,isnull(patients_mi.pat_seq,99999)),
resulto.res_unit,--单位
itm_dtype = '',--项目数据类型
itm_max_digit = 0,--小数位数
res_max = '',--极大阈值
res_min = '',--极小阈值
res_pan_h = '',--危急值上限
res_pan_l = '',--危急值下限
res_ref_h = '',--参考值上限
res_ref_l = '',--参考值下限
res_max_cal = '',--极大阈值
res_min_cal = '',--极小阈值
res_pan_h_cal = '',--危急值上限
res_pan_l_cal = '',--危急值下限
res_ref_h_cal = '',--参考值上限
res_ref_l_cal = '',--参考值下限
res_ref_range = '',--参考值范围
res_allow_values = '',--允许出现的结果
res_positive_result = '',--阳性提示结果
res_custom_critical_result = '',--自定义危急值
history_result1='',--历史结果,
history_date1 = '',--历史结果时间
history_result2='',
history_date2 = '',
history_result3='',
history_date3 = '',
patients.pat_sam_id,
patients.pat_sex,
patients.pat_age,
patients.pat_sam_rem,
patients.pat_urgent_flag,
patients.pat_flag,--审核状态
resulto.res_exp,
patients.pat_dep_id,
isnew=0,
resulto.res_recheck_flag,
res_ref_flag_h1=0,
res_ref_flag_h2=0,
res_ref_flag_h3=0,
res_origin_record = 1,
dict_combine_mi.com_popedom as isnotempty,--是否为必录项目
calculate_source_itm_id = '',
calculate_dest_itm_id = '',
res_itm_seq = dict_combine_mi.com_sort,
isnull(dict_item.itm_seq,0) as itm_seq
,resulto.res_chr2,--结果2
resulto.res_chr3, --结果3
dict_instrmt.itr_mid,res_Alarm_a,res_Alarm_b,res_Alarm_c,res_Alarm_d,res_Alarm_e,res_Alarm_f,res_Alarm_g,itm_con_ftor,dict_instrmt2.itr_mid as  itr_mid2,resulto.res_cri_exp,resulto.res_cri_flag,
'' AS res_verify
from resulto
left join patients on patients.pat_id = resulto.res_id--join结果表 edit by sink 2010-6-9 inner join 改为left join
left join dict_item on dict_item.itm_id = resulto.res_itm_id--join项目表
left join dict_combine on dict_combine.com_id = resulto.res_com_id--join项目组合表
left join patients_mi on (patients_mi.pat_com_id = resulto.res_com_id and patients_mi.pat_id = resulto.res_id)--join病人检验组合
left join dict_combine_mi on  patients_mi.pat_com_id = dict_combine_mi.com_id and resulto.res_itm_id = dict_combine_mi.com_itm_id
left join dict_instrmt_warningsigns on dict_instrmt_warningsigns.itr_warn_origdate=resulto.res_exp
left join dict_instrmt on resulto.res_itr_ori_id=dict_instrmt.itr_id
left join dict_instrmt  dict_instrmt2 on resulto.res_itr_id=dict_instrmt2.itr_id
where resulto.res_id = '{0}' and res_flag = 1
order by dict_combine.com_seq asc, dict_combine_mi.com_sort asc,dict_item.itm_seq ,resulto.res_itm_ecd asc
", patID); //加上项目排序码排序 edit by sink 2010-6-8

                DataTable dtResult = helper.GetTable(sql);

                dtResult = ResultValidate(dtResult);

                dtResult.TableName = PatientTable.PatientResultTableName;

                if (dtResult.Rows.Count > 0)
                {
                    List<string> itemsID = new List<string>();
                    foreach (DataRow drResult in dtResult.Rows)
                    {
                        if (!Compare.IsEmpty(drResult["res_itm_id"]))
                        {
                            if (
                                (Compare.IsEmpty(drResult["res_com_id"]) || drResult["res_com_id"].ToString() == "-1")
                                &&
                                dic.ContainsKey(drResult["res_itm_id"].ToString())
                                )
                            {
                                string comid = dic[drResult["res_itm_id"].ToString()];
                                drResult["res_com_id"] = comid;


                                //获取组合排序
                                var combine = DictCombineCache.Current.GetCombineByID(comid, true);

                                if (combine != null && combine.com_seq != null)
                                {
                                    drResult["res_com_seq"] = combine.com_seq;
                                }

                                var combineMi = DictCombineMiCache2.Current.GetAll().Find(
                                    a => a.com_id == comid && a.com_itm_id == drResult["res_itm_id"].ToString());

                                if (combineMi != null)
                                {
                                    if (combineMi.com_sort != null)
                                        drResult["res_itm_seq"] = combineMi.com_sort;

                                    if (combineMi.com_popedom != null)
                                        drResult["isnotempty"] = combineMi.com_popedom;
                                }
                            }

                            itemsID.Add(drResult["res_itm_id"].ToString());
                        }
                    }


                    if (dic.Keys.Count > 0)
                        dtResult = OrderComSort(dtResult);

                    string pat_sam_id = dtResult.Rows[0]["pat_sam_id"].ToString();

                    string sam_rem = dtResult.Rows[0]["pat_sam_rem"].ToString();

                    string pat_depcode = string.Empty;
                    if (!Compare.IsNullOrDBNull(dtResult.Rows[0]["pat_dep_id"]))
                    {
                        pat_depcode = dtResult.Rows[0]["pat_dep_id"].ToString();
                    }

                    int pat_age = PatCommonBll.GetConfigAge(dtResult.Rows[0]["pat_age"]);
                    string pat_sex = PatCommonBll.GetConfigSex(dtResult.Rows[0]["pat_sex"]);
                    string res_itr_id = dtResult.Rows[0]["res_itr_id"].ToString();

                    DataTable dtItmRef = DictItemBLL.NewInstance.GetItemsWithSamRef(itemsID, pat_sam_id, pat_age, pat_sex, sam_rem, res_itr_id, pat_depcode);

                    if (dtItmRef.Rows.Count > 0)
                    {
                        foreach (DataRow drResult in dtResult.Rows)//循环结果表
                        {
                            if (!Compare.IsEmpty(drResult["res_itm_id"]))
                            {
                                DataRow[] drsItmRef = dtItmRef.Select(string.Format("itm_id = '{0}'", drResult["res_itm_id"].ToString()));

                                if (drsItmRef.Length > 0)
                                {
                                    //res_max = '',--极大阈值
                                    //res_min = '',--极小阈值
                                    //res_pan_h = '',--危急值上限
                                    //res_pan_l = '',--危急值下限
                                    //res_ref_h = '',--参考值上限
                                    //res_ref_l = '',--参考值下限

                                    //res_max_cal = '',--极大阈值
                                    //res_min_cal = '',--极小阈值
                                    //res_pan_h_cal = '',--危急值上限
                                    //res_pan_l_cal = '',--危急值下限
                                    //res_ref_h_cal = '',--参考值上限
                                    //res_ref_l_cal = '',--参考值下限

                                    drResult["res_ref_l"] = drsItmRef[0]["itm_ref_l"];
                                    drResult["res_ref_h"] = drsItmRef[0]["itm_ref_h"];

                                    drResult["res_pan_l"] = drsItmRef[0]["itm_pan_l"];
                                    drResult["res_pan_h"] = drsItmRef[0]["itm_pan_h"];

                                    drResult["res_min"] = drsItmRef[0]["itm_min"];
                                    drResult["res_max"] = drsItmRef[0]["itm_max"];

                                    //允许出现的结果
                                    if (drsItmRef[0].Table.Columns.Contains("itm_allow_result"))
                                    {
                                        drResult["res_allow_values"] = drsItmRef[0]["itm_allow_result"];
                                    }

                                    //阳性提示结果
                                    if (drsItmRef[0].Table.Columns.Contains("itm_positive_result"))
                                    {
                                        drResult["res_positive_result"] = drsItmRef[0]["itm_positive_result"];
                                    }

                                    //自定义危急值
                                    if (drsItmRef[0].Table.Columns.Contains("itm_urgent_result"))
                                    {
                                        drResult["res_custom_critical_result"] = drsItmRef[0]["itm_urgent_result"];
                                    }

                                    drResult["res_ref_l_cal"] = drsItmRef[0]["itm_ref_l"];
                                    drResult["res_ref_h_cal"] = drsItmRef[0]["itm_ref_h"];

                                    if (
                                        !string.IsNullOrEmpty(drResult["res_ref_l_cal"].ToString().Trim())
                                        && !string.IsNullOrEmpty(drResult["res_ref_h_cal"].ToString().Trim()))
                                    {
                                        drResult["res_ref_range"] = drResult["res_ref_l_cal"].ToString() + " - " + drResult["res_ref_h_cal"].ToString();
                                    }
                                    else
                                    {
                                        drResult["res_ref_range"] = drResult["res_ref_l_cal"].ToString() + drResult["res_ref_h_cal"].ToString();
                                    }


                                    drResult["res_pan_l_cal"] = drsItmRef[0]["itm_pan_l"];
                                    drResult["res_pan_h_cal"] = drsItmRef[0]["itm_pan_h"];

                                    drResult["res_min_cal"] = drsItmRef[0]["itm_min"];
                                    drResult["res_max_cal"] = drsItmRef[0]["itm_max"];


                                    drResult["res_meams"] = drsItmRef[0]["itm_meams"];
                                    drResult["itm_dtype"] = drsItmRef[0]["itm_dtype"];
                                    drResult["itm_max_digit"] = drsItmRef[0]["itm_max_digit"];
                                    drResult["res_unit"] = drsItmRef[0]["itm_unit"];
                                    //resulto.res_unit,--单位
                                    //itm_dtype = '',--项目数据类型
                                    //itm_max_digit = 0,--小数位数
                                }
                            }
                        }
                    }
                }

                foreach (DataRow drResult in dtResult.Rows)
                {
                    ItemRefFormatter.Format(drResult, "res_ref_l_cal", "res_ref_h_cal", "res_min_cal", "res_max_cal", "res_pan_l_cal", "res_pan_h_cal");
                }

                if (getHistory)
                {
                    //获取最近3次历史记录
                    #region 获取最近3次结果
                    DataTable dtPatientInfo = new DBHelper().GetTable(string.Format(@"
select pat_name,pat_itr_id,pat_social_no,pat_sam_id,pat_no_id,pat_in_no,pat_pid,pat_age,pat_sex,pat_date,pat_upid from patients where pat_id ='{0}'", patID));
                    if (dtPatientInfo.Rows.Count > 0)
                    {
                        DataRow drPatient = dtPatientInfo.Rows[0];

                        string pat_name = string.Empty;
                        string pat_itr_id = string.Empty;
                        string pat_sam_id = string.Empty;
                        string pat_no_id = string.Empty;
                        string pat_in_no = string.Empty;
                        string pat_sex = string.Empty;
                        int ageMinute = -1;
                        DateTime? patDate = null;

                        if (!Compare.IsNullOrDBNull(drPatient["pat_date"]))
                        {
                            patDate = DateTime.Parse(drPatient["pat_date"].ToString());

                        }


                        //string itr_id, string pat_sam_id, string pat_no_id, string pat_in_no, string pat_name, int resultCount
                        if (drPatient["pat_name"] != null && drPatient["pat_name"] != DBNull.Value)
                        {
                            pat_name = drPatient["pat_name"].ToString();
                        }

                        if (drPatient["pat_itr_id"] != null && drPatient["pat_itr_id"] != DBNull.Value)
                        {
                            pat_itr_id = drPatient["pat_itr_id"].ToString();
                        }

                        if (drPatient["pat_sam_id"] != null && drPatient["pat_sam_id"] != DBNull.Value)
                        {
                            pat_sam_id = drPatient["pat_sam_id"].ToString();
                        }

                        if (drPatient["pat_no_id"] != null && drPatient["pat_no_id"] != DBNull.Value)
                        {
                            pat_no_id = drPatient["pat_no_id"].ToString();
                        }

                        if (drPatient["pat_in_no"] != null && drPatient["pat_in_no"] != DBNull.Value)
                        {
                            pat_in_no = drPatient["pat_in_no"].ToString();
                        }

                        if (!Compare.IsNullOrDBNull(drPatient["pat_age"]))
                        {
                            ageMinute = Convert.ToInt32(drPatient["pat_age"]);
                        }

                        if (!Compare.IsNullOrDBNull(drPatient["pat_sex"]))
                        {
                            pat_sex = drPatient["pat_sex"].ToString();
                        }

                        if (pat_name != string.Empty
                            && pat_itr_id != string.Empty
                            && pat_sam_id != string.Empty
                            && pat_no_id != string.Empty
                       //&& pat_in_no != string.Empty //edit by sink 2010-6-23 不判断pat_in_no,因为历史结果有可能不以此字段作查询                            )
                       )
                        {
                            #region //获取历史结果自定义查询列 edit by sink 2010-6-18
                            string historySelectColumnValue = "";

                            string historySelectColumn = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");// dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");
                            if (string.IsNullOrEmpty(historySelectColumn))
                                historySelectColumn = "pat_in_no";
                            else
                            {
                                historySelectColumn = historySelectColumn.Replace("patients.", "");
                                if (!Compare.IsNullOrDBNull(drPatient[historySelectColumn]))
                                {
                                    historySelectColumnValue = drPatient[historySelectColumn].ToString();
                                }
                            }
                            #endregion

                            if (dtResult.Rows.Count > 0)
                            {
                                //PatientEnterService pEnter = new PatientEnterService();
                                DataTable dthistoryResult = GetPatCommonResultHistory(patID, pat_itr_id, pat_sam_id, pat_no_id, pat_in_no, pat_name, pat_sex, patDate, 3, historySelectColumn, historySelectColumnValue);
                                StringBuilder sbResultId = new StringBuilder();
                                foreach (DataRow drResult in dtResult.Rows)
                                {
                                    sbResultId.Append(string.Format(",'{0}'", drResult["res_itm_id"]));
                                }
                                sbResultId.Remove(0, 1);

                                DataView dvhistoryResult = new DataView(dthistoryResult);
                                dvhistoryResult.RowFilter = string.Format("res_itm_id in ({0})", sbResultId);
                                dvhistoryResult.Sort = "res_date desc";
                                DataTable dtTimes = dvhistoryResult.ToTable(true, "res_id");


                                try
                                {
                                    if (dtTimes.Rows.Count > 0)
                                    {
                                        for (int j = 0; j < dtTimes.Rows.Count; j++)
                                        {
                                            DataRow drTime = dtTimes.Rows[j];
                                            foreach (DataRow drResult in dtResult.Rows)
                                            {
                                                if (drResult["res_itm_id"] != null && drResult["res_itm_id"] != DBNull.Value)
                                                {
                                                    string itm_id = drResult["res_itm_id"].ToString();

                                                    DataRow[] drs = dthistoryResult.Select(string.Format("res_itm_id ='{0}' and res_id='{1}'", itm_id, drTime["res_id"]), "res_date desc");

                                                    if (drs.Length > 0)
                                                    {
                                                        drResult["history_result" + (j + 1).ToString()] = drs[0]["res_chr"];
                                                        drResult["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                        dtResult.Rows[0]["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.WriteException(this.GetType().Name, "GetPatientResult", ex.ToString());
                                }
                            }
                        }
                    }
                    #endregion
                }

                sw.Stop();
                Logger.Debug(string.Format("数据库:GetPatientResult,获取病人结果表,耗时 {0}ms", sw.ElapsedMilliseconds));

                return dtResult;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人结果出错,patID=" + patID, ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// 获取病人普通结果、历史结果
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="dthistoryResult">返回病人历史结果</param>
        /// <returns></returns>
        public DataTable GetPatientCommonResult(string patID, bool getHistory, out DataTable dthistoryResult, DataSet dsData)
        {
            try
            {
                dthistoryResult = new DataTable();

                DBHelper helper = new DBHelper();
                DataTable dtPat = dsData.Tables["patients"];

                //string selectPatState = string.Format("select top 1 pat_id,pat_sid,pat_flag,pat_sam_id,pat_itr_id,pat_sam_rem,pat_sex,pat_age,pat_weight,pat_itr_audit_flag from patients where pat_id = '{0}'", patID);

                //DataTable dtPat = helper.GetTable(selectPatState);

                Dictionary<string, string> dic = new Dictionary<string, string>();

                EntityPatient2 entityPatient = new EntityPatient2();

                if (dtPat.Rows.Count > 0)
                {
                    entityPatient.pat_id = dtPat.Rows[0]["pat_id"].ToString();
                    entityPatient.pat_sid = dtPat.Rows[0]["pat_sid"].ToString();
                    entityPatient.pat_itr_id = dtPat.Rows[0]["pat_itr_id"].ToString();
                    entityPatient.pat_sam_id = dtPat.Rows[0]["pat_sam_id"].ToString();
                    entityPatient.pat_weight = dtPat.Rows[0]["pat_weight"].ToString();
                    entityPatient.pat_sex = dtPat.Rows[0]["pat_sex"].ToString();
                    string entityPatient_pat_itr_audit_flag = dtPat.Rows[0]["pat_itr_audit_flag"].ToString();
                    try
                    {
                        if (!Compare.IsEmpty(dtPat.Rows[0]["pat_age"]))
                        {
                            entityPatient.pat_age = Convert.ToInt32(dtPat.Rows[0]["pat_age"]);
                        }
                    }
                    catch
                    {
                    }

                    if (!Compare.IsEmpty(dtPat.Rows[0]["pat_flag"]))
                    {
                        entityPatient.pat_flag = Convert.ToInt32(dtPat.Rows[0]["pat_flag"]);
                    }
                    else
                    {
                        entityPatient.pat_flag = 0;
                    }


                    if (entityPatient.pat_flag == 0)
                    {
                        string sampRem = string.Empty;
                        if (!Compare.IsEmpty(dtPat.Rows[0]["pat_sam_rem"]))
                        {
                            sampRem = dtPat.Rows[0]["pat_sam_rem"].ToString();
                        }
                        GenerateAutoCalItem(entityPatient, entityPatient.pat_sam_id, sampRem);
                        //FillNotNullResult(entityPatient);
                        dic = UpdateResultNotCombineItem(patID);
                    }
                    else if (entityPatient_pat_itr_audit_flag == "1" && entityPatient.pat_flag == 1)
                    {
                        dic = UpdateResultNotCombineItem(patID);
                    }
                }

                //if (Compare.IsNullOrDBNull(objState) || objState.ToString() == "0")
                //{
                //    GenerateAutoCalItem(patID);
                //    FillNotNullResult(patID);
                //    UpdateResultNotCombineItem(patID);
                //}
                /* 
                 * 2013-09-09 修改 李进
                 * 如果检验结果为第三方检验结果，对res_ref_l进行下面的修改。
                 * res_ref_l = (case when patients.pat_flag='2' or patients.pat_flag='4' or patients.pat_hospital_id='outreport' then resulto.res_ref_l else '' end),--参考值下限
                 * 并添加一个输出项：dict_instrmt.itr_mid
                 */
                string sql = string.Format(@"
select
resulto.res_key,--结果主键标识
resulto.res_id,--结果ID(病人ID)
resulto.res_itr_id,--仪器ID
resulto.res_sid,--样本号
resulto.res_itm_id,--项目ID
dict_item.itm_ecd as res_itm_ecd,--项目代码
dict_item.itm_name as res_itm_name,--项目名称
resulto.res_itm_rep_ecd,--项目报告编号
resulto.res_chr,--结果
resulto.res_od_chr,--OD结果
resulto.res_cast_chr,--数值结果
resulto.res_price,--价格
resulto.res_ref_exp,--阳性标志 （提示）
resulto.res_ref_flag,--阳性标志(偏高，偏低，阳性，正常,etc)
resulto.res_meams,--实验方法
resulto.res_date,--结果日期
resulto.res_flag,--有效标志
dict_instrmt_warningsigns.itr_warn_trandate,--项目状态
resulto.res_itr_ori_id,--原始仪器id
resulto.res_ref_type,--参考值类型
res_type = case when dict_item.itm_cal_flag = 1 then 2
                else resulto.res_type end, --结果类型
resulto.res_rep_type,--报告类型
resulto.res_com_id,--组合ID
res_com_name = case when (resulto.res_com_id is null or resulto.res_com_id ='') then '无组合'
                    else  dict_combine.com_name
                    end,--组合名称
--res_com_seq = isnull(dict_combine.com_seq,99999),
res_com_seq = isnull((case when dict_combine.com_seq is null or dict_combine.com_seq=0 then null else dict_combine.com_seq end),isnull(patients_mi.pat_seq,99999)),
--res_com_seq = case when patients_mi.pat_seq is null then 9999
--                   else patients_mi.pat_seq
--                   end,
resulto.res_unit,--单位
itm_dtype = '',--项目数据类型
itm_max_digit = 0,--小数位数
res_max = '',--极大阈值
res_min = '',--极小阈值
res_pan_h = '',--危急值上限
res_pan_l = '',--危急值下限
res_ref_h = (case when patients.pat_flag='2' or patients.pat_flag='4' then resulto.res_ref_h else '' end),--参考值上限
res_ref_l = (case when patients.pat_flag='2' or patients.pat_flag='4' or patients.pat_hospital_id='outreport' then resulto.res_ref_l else '' end),--参考值下限
res_max_cal = '',--极大阈值
res_min_cal = '',--极小阈值
res_pan_h_cal = '',--危急值上限
res_pan_l_cal = '',--危急值下限
res_ref_h_cal = (case when patients.pat_flag='2' or patients.pat_flag='4' then resulto.res_ref_h else '' end),--参考值上限
res_ref_l_cal = (case when patients.pat_flag='2' or patients.pat_flag='4' then resulto.res_ref_l else '' end),--参考值下限
res_ref_range = '',--参考值范围
res_allow_values = '',--允许出现的结果
res_positive_result = '',--阳性提示结果
res_custom_critical_result = '',--自定义危急值
history_result1='',--历史结果,
history_date1 = '',--历史结果时间
history_result2='',
history_date2 = '',
history_result3='',
history_date3 = '',
patients.pat_sam_id,
patients.pat_sex,
patients.pat_age,
patients.pat_sam_rem,
patients.pat_urgent_flag,
patients.pat_flag,--审核状态
resulto.res_exp,
patients.pat_dep_id,
isnew=0,
resulto.res_recheck_flag,
res_ref_flag_h1=0,
res_ref_flag_h2=0,
res_ref_flag_h3=0,
res_origin_record = 1,
dict_combine_mi.com_popedom as isnotempty,--是否为必录项目
calculate_source_itm_id = '',
calculate_dest_itm_id = '',
res_itm_seq = dict_combine_mi.com_sort,
isnull(dict_item.itm_seq,0) as itm_seq
,resulto.res_chr2,--结果2
resulto.res_chr3, --结果3
dict_instrmt.itr_mid,itm_con_ftor,dict_instrmt2.itr_mid as  itr_mid2,
pat_hospital_id,res_Alarm_a,res_Alarm_b,res_Alarm_c,res_Alarm_d,res_Alarm_e,res_Alarm_f,res_Alarm_g,resulto.res_cri_exp,resulto.res_cri_flag,
'' AS res_verify
from resulto
left join patients on patients.pat_id = resulto.res_id--join结果表 edit by sink 2010-6-9 inner join 改为left join
left join dict_item on dict_item.itm_id = resulto.res_itm_id--join项目表
left join dict_combine on dict_combine.com_id = resulto.res_com_id--join项目组合表
left join patients_mi on (patients_mi.pat_com_id = resulto.res_com_id and patients_mi.pat_id = resulto.res_id)--join病人检验组合
left join dict_combine_mi on  patients_mi.pat_com_id = dict_combine_mi.com_id and resulto.res_itm_id = dict_combine_mi.com_itm_id
left join dict_instrmt_warningsigns on dict_instrmt_warningsigns.itr_warn_origdate=resulto.res_exp and patients.pat_itr_id=dict_instrmt_warningsigns.itr_id
left join dict_instrmt on resulto.res_itr_ori_id=dict_instrmt.itr_id
left join dict_instrmt dict_instrmt2 on resulto.res_itr_id=dict_instrmt2.itr_id
where resulto.res_id = '{0}' and res_flag = 1
order by dict_combine.com_seq asc, dict_combine_mi.com_sort asc,dict_item.itm_seq ,resulto.res_itm_ecd asc
", patID); //加上项目排序码排序 edit by sink 2010-6-8

                #region 旧代码
                //                string sql = string.Format(@"
                //select
                //resulto.res_key,--结果主键标识
                //resulto.res_id,--结果ID(病人ID)
                //resulto.res_itr_id,--仪器ID
                //resulto.res_sid,--样本号
                //resulto.res_itm_id,--项目ID
                //resulto.res_itm_ecd,--项目代码
                //dict_item.itm_name as res_itm_name,--项目名称
                //resulto.res_itm_rep_ecd,--项目报告编号
                //resulto.res_chr,--结果
                //resulto.res_od_chr,--OD结果
                //resulto.res_cast_chr,--数值结果
                //dict_item_sam.itm_unit as res_unit,--单位
                //resulto.res_price,--价格
                //resulto.res_ref_exp,--阳性标志 （提示）
                //resulto.res_ref_flag,--阳性标志(偏高，偏低，阳性，正常,etc)
                //resulto.res_meams,--实验方法
                //resulto.res_date,--结果日期
                //resulto.res_flag,--有效标志
                //
                //resulto.res_itr_ori_id,--原始仪器id
                //
                //resulto.res_ref_type,--参考值类型
                //
                //res_type = case when dict_item.itm_cal_flag = 1 then 2
                //                else resulto.res_type end, --结果类型
                //
                //resulto.res_rep_type,--报告类型
                //resulto.res_com_id,--组合ID
                //
                //res_com_name = case when (resulto.res_com_id is null or resulto.res_com_id ='') then '无组合'
                //                    else  dict_combine.com_name
                //                    end,--组合名称
                //
                //res_com_seq = case when patients_mi.pat_seq is null then 9999
                //                   else patients_mi.pat_seq
                //                   end,
                //
                //dict_item_sam.itm_dtype,--项目数据类型
                //dict_item_sam.itm_max_digit,--最大小数位
                //
                //res_max = dict_item_mi.itm_max,--极大阈值
                //res_min = dict_item_mi.itm_min,--极小阈值
                //res_pan_h = dict_item_mi.itm_pan_h,--危急值上限
                //res_pan_l = dict_item_mi.itm_pan_l,--危急值下限
                //res_ref_h = dict_item_mi.itm_ref_h,--参考值上限
                //res_ref_l = dict_item_mi.itm_ref_l,--参考值下限
                //
                //res_max_cal = dict_item_mi.itm_max,--极大阈值
                //res_min_cal = dict_item_mi.itm_min,--极小阈值
                //res_pan_h_cal = dict_item_mi.itm_pan_h,--危急值上限
                //res_pan_l_cal = dict_item_mi.itm_pan_l,--危急值下限
                //res_ref_h_cal = dict_item_mi.itm_ref_h,--参考值上限
                //res_ref_l_cal = dict_item_mi.itm_ref_l,--参考值下限
                //
                //history_result1='',--历史结果,
                //history_date1 = '',--历史结果时间
                //history_result2='',
                //history_date2 = '',
                //history_result3='',
                //history_date3 = '',
                //isnew=0,
                //res_chr_flag=0,
                //res_origin_record = 1,
                //dict_combine_mi.com_popedom as isnotempty,
                //calculate_source_itm_id = '',
                //calculate_dest_itm_id = '',
                //res_itm_seq = dict_combine_mi.com_sort
                //
                //from resulto
                //inner join patients on patients.pat_id = resulto.res_id--join结果表
                //inner join dict_instrmt on dict_instrmt.itr_id = patients.pat_itr_id--join仪器表
                //left join dict_item on dict_item.itm_id = resulto.res_itm_id--join项目表
                //left join dict_item_sam on dict_item_sam.itm_id = resulto.res_itm_id and dict_item_sam.itm_sam_id = patients.pat_sam_id--join项目样本表
                //left join dict_combine on dict_combine.com_id = resulto.res_com_id--join项目组合表
                //
                //left join patients_mi on (patients_mi.pat_com_id = resulto.res_com_id and patients_mi.pat_id = resulto.res_id)--join病人检验组合
                //left join dict_combine_mi on  patients_mi.pat_com_id = dict_combine_mi.com_id and resulto.res_itm_id = dict_combine_mi.com_itm_id
                //
                //left join dict_item_mi on (dict_item_mi.itm_id = resulto.res_itm_id and itm_mi_id=
                //(select top 1 itm_mi_id from dict_item_mi where  dict_item_mi.itm_id = resulto.res_itm_id --join出参考值
                //                           and dict_item_mi.itm_sam_id in(patients.pat_sam_id,'-1') --样本类型
                //                           and 
                //                               (
                //                               dict_item_mi.itm_sex = case when patients.pat_sex <> '2' and patients.pat_sex<>'1' then '0'
                //                                                           else patients.pat_sex end
                //                               or
                //                               dict_item_mi.itm_sex = '0') and dict_item_mi.itm_flag='0' and (dict_item_mi.itm_sam_rem='' or dict_item_mi.itm_sam_rem is null or dict_item_mi.itm_sam_rem=patients.pat_sam_rem )-- 对应的性别或无限制的性别
                //                           and patients.pat_age >= dict_item_mi.itm_age_ls and patients.pat_age <= dict_item_mi.itm_age_hs order by itm_sam_id desc,itm_sex desc,dict_item_mi.itm_sam_rem desc))--年龄
                //
                //
                //where resulto.res_id = '{0}' and res_flag = 1
                //order by res_com_seq asc, dict_combine_mi.com_sort asc,resulto.res_itm_ecd asc
                //",
                // patID); 
                #endregion

                #region MyRegion
                //                string sql = string.Format(@"
                //select
                //resulto.res_key,--结果主键标识
                //resulto.res_id,--结果ID(病人ID)
                //resulto.res_itr_id,--仪器ID
                //resulto.res_sid,--样本号
                //resulto.res_itm_id,--项目ID
                //resulto.res_itm_ecd,--项目代码
                //dict_item.itm_name as res_itm_name,--项目名称
                //resulto.res_itm_rep_ecd,--项目报告编号
                //resulto.res_chr,--结果
                //resulto.res_od_chr,--OD结果
                //resulto.res_cast_chr,--数值结果
                //dict_item_sam.itm_unit as res_unit,--单位
                //resulto.res_price,--价格
                //resulto.res_ref_exp,--阳性标志 （提示）
                //resulto.res_ref_flag,--阳性标志(偏高，偏低，阳性，正常,etc)
                //resulto.res_meams,--实验方法
                //resulto.res_date,--结果日期
                //resulto.res_flag,--有效标志
                //resulto.res_type,--结果类型
                //resulto.res_rep_type,--报告类型
                //resulto.res_com_id,--组合ID
                //
                //res_com_name = case when (resulto.res_com_id is null or resulto.res_com_id ='') then '无组合'
                //                    else  dict_combine.com_name
                //                    end,--组合名称
                //
                //res_com_seq = case when patients_mi.pat_seq is null then 9999
                //                   else patients_mi.pat_seq
                //                   end,--组合顺序
                //
                //dict_item_sam.itm_dtype,--项目数据类型
                //dict_item_sam.itm_max_digit,--最大小数位
                //res_max ,--极大阈值
                //res_min ,--极小阈值
                //res_pan_h ,--危急值上限
                //res_pan_l ,--危急值下限
                //res_ref_h ,--参考值上限
                //res_ref_l ,--参考值下限
                //
                //history_result1='',--历史结果,
                //history_date1 = '',--历史结果时间
                //history_result2='',
                //history_date2 = '',
                //history_result3='',
                //history_date3 = '',
                //
                //isnew=0,
                //res_chr_flag=0
                //from resulto
                //inner join patients on patients.pat_id = resulto.res_id--join结果表
                //inner join dict_instrmt on dict_instrmt.itr_id = patients.pat_itr_id--join仪器表
                //left join dict_item on dict_item.itm_id = resulto.res_itm_id--join项目表
                //left join dict_item_sam on dict_item_sam.itm_id = resulto.res_itm_id and dict_item_sam.itm_sam_id = patients.pat_sam_id--join项目样本表
                //left join dict_combine on dict_combine.com_id = resulto.res_com_id--join项目组合表
                //left join patients_mi on (patients_mi.pat_com_id = resulto.res_com_id and patients_mi.pat_id = resulto.res_id)--join病人检验组合
                //where resulto.res_id = '{0}'
                //order by res_com_seq, dict_item.itm_seq
                //", patID); 
                #endregion

                DataTable dtResult = helper.GetTable(sql);

                dtResult = ResultValidate(dtResult);

                dtResult.TableName = PatientTable.PatientResultTableName;

                if (dtResult.Rows.Count > 0)
                {
                    List<string> itemsID = new List<string>();
                    foreach (DataRow drResult in dtResult.Rows)
                    {
                        if (!Compare.IsEmpty(drResult["res_itm_id"]))
                        {


                            if (
                                 (
                                    Compare.IsEmpty(drResult["res_com_id"]) ||
                                    drResult["res_com_id"].ToString() == "-1"
                                 ) &&
                                    dic.ContainsKey(drResult["res_itm_id"].ToString())
                                )
                            {
                                string comid = dic[drResult["res_itm_id"].ToString()];
                                drResult["res_com_id"] = comid;


                                //获取组合排序
                                var combine = DictCombineCache.Current.GetCombineByID(comid, true);

                                if (combine != null && combine.com_seq != null)
                                {
                                    drResult["res_com_seq"] = combine.com_seq;
                                }

                                var combineMi = DictCombineMiCache2.Current.GetAll().Find(
                                    a => a.com_id == comid && a.com_itm_id == drResult["res_itm_id"].ToString());

                                if (combineMi != null)
                                {
                                    if (combineMi.com_sort != null)
                                        drResult["res_itm_seq"] = combineMi.com_sort;

                                    if (combineMi.com_popedom != null)
                                        drResult["isnotempty"] = combineMi.com_popedom;
                                }
                            }


                            itemsID.Add(drResult["res_itm_id"].ToString());
                        }
                    }

                    if (dic.Keys.Count > 0)
                        dtResult = OrderComSort(dtResult);
                    //int? pat_age = null;

                    //if (!Compare.IsEmpty(dtResult.Rows[0]["pat_age"]))
                    //{
                    //    pat_age = Convert.ToInt32(dtResult.Rows[0]["pat_age"]);
                    //}
                    //else
                    //{
                    //    pat_age = PatCommonBll.GetConfigOnNullAge();
                    //}

                    ////if (pat_age == null)
                    ////{
                    ////    pat_age = AgeConverter.YearToMinute(20);
                    ////}

                    //string pat_sex = PatCommonBll.GetConfigOnNullSex(dtResult.Rows[0]["pat_sex"].ToString());

                    string pat_depcode = string.Empty;
                    if (!Compare.IsNullOrDBNull(dtResult.Rows[0]["pat_dep_id"]))
                    {
                        pat_depcode = dtResult.Rows[0]["pat_dep_id"].ToString();
                    }
                    string pat_sam_id = dtResult.Rows[0]["pat_sam_id"].ToString();

                    string sam_rem = dtResult.Rows[0]["pat_sam_rem"].ToString();

                    string res_itr_id = dtResult.Rows[0]["res_itr_id"].ToString();

                    int pat_age = PatCommonBll.GetConfigAge(dtResult.Rows[0]["pat_age"]);
                    string pat_sex = PatCommonBll.GetConfigSex(dtResult.Rows[0]["pat_sex"]);
                    /*
                     * 开始 2013-09-09 李进
                     * 由于要显示第三方检验结果中的参考值,在此添加一个判断。如果为第三方的检验结果。
                     * 将res_ref_l中的值赋给drResult["res_ref_range"]，用于显示。
                     *  如果为非第三方检验结果，则依原来进行处理。
                     */
                    object pat_hospital_id = dtResult.Rows[0]["pat_hospital_id"];
                    if (pat_hospital_id != null && pat_hospital_id.ToString() == "outreport")
                    {
                        foreach (DataRow drResult in dtResult.Rows)//循环结果表
                        {
                            drResult["res_ref_range"] = drResult["res_ref_l"];
                        }
                    }
                    else
                    {

                        #region 参考值

                        DataTable dtItmRef = DictItemBLL.NewInstance.GetItemsWithSamRef(itemsID, pat_sam_id, pat_age, pat_sex, sam_rem, res_itr_id, pat_depcode);

                        if (dtItmRef.Rows.Count > 0)
                        {
                            foreach (DataRow drResult in dtResult.Rows)//循环结果表
                            {
                                if (!Compare.IsEmpty(drResult["res_itm_id"]))
                                {
                                    DataRow[] drsItmRef = dtItmRef.Select(string.Format("itm_id = '{0}'", drResult["res_itm_id"].ToString()));

                                    if (drsItmRef.Length > 0)
                                    {
                                        //res_max = '',--极大阈值
                                        //res_min = '',--极小阈值
                                        //res_pan_h = '',--危急值上限
                                        //res_pan_l = '',--危急值下限
                                        //res_ref_h = '',--参考值上限
                                        //res_ref_l = '',--参考值下限

                                        //res_max_cal = '',--极大阈值
                                        //res_min_cal = '',--极小阈值
                                        //res_pan_h_cal = '',--危急值上限
                                        //res_pan_l_cal = '',--危急值下限
                                        //res_ref_h_cal = '',--参考值上限
                                        //res_ref_l_cal = '',--参考值下限

                                        if (drResult.Table.Columns.Contains("pat_flag") && (drResult["pat_flag"].ToString() == "2" || drResult["pat_flag"].ToString() == "4"))
                                        {
                                            //如果pat_flag等于2时,为二审结果,则参考值取结果表数据
                                            //如果pat_flag等于4时,为已打印报告,则参考值取结果表数据
                                        }
                                        else
                                        {
                                            drResult["res_ref_l"] = drsItmRef[0]["itm_ref_l"];
                                            drResult["res_ref_h"] = drsItmRef[0]["itm_ref_h"];
                                        }

                                        drResult["res_pan_l"] = drsItmRef[0]["itm_pan_l"];
                                        drResult["res_pan_h"] = drsItmRef[0]["itm_pan_h"];

                                        drResult["res_min"] = drsItmRef[0]["itm_min"];
                                        drResult["res_max"] = drsItmRef[0]["itm_max"];

                                        //允许出现的结果
                                        if (drsItmRef[0].Table.Columns.Contains("itm_allow_result"))
                                        {
                                            drResult["res_allow_values"] = drsItmRef[0]["itm_allow_result"];
                                        }

                                        //阳性提示结果
                                        if (drsItmRef[0].Table.Columns.Contains("itm_positive_result"))
                                        {
                                            drResult["res_positive_result"] = drsItmRef[0]["itm_positive_result"];
                                        }

                                        //自定义危急值
                                        if (drsItmRef[0].Table.Columns.Contains("itm_urgent_result"))
                                        {
                                            drResult["res_custom_critical_result"] = drsItmRef[0]["itm_urgent_result"];
                                        }

                                        if (drResult.Table.Columns.Contains("pat_flag") && (drResult["pat_flag"].ToString() == "2" || drResult["pat_flag"].ToString() == "4"))
                                        {
                                            //如果pat_flag等于2时,为二审结果,则参考值取结果表数据
                                            //如果pat_flag等于4时,为已打印报告,则参考值取结果表数据
                                        }
                                        else
                                        {
                                            drResult["res_ref_l_cal"] = drsItmRef[0]["itm_ref_l"];
                                            drResult["res_ref_h_cal"] = drsItmRef[0]["itm_ref_h"];
                                        }

                                        if (
                                           !string.IsNullOrEmpty(drResult["res_ref_l_cal"].ToString().Trim())
                                           && !string.IsNullOrEmpty(drResult["res_ref_h_cal"].ToString().Trim()))
                                        {
                                            drResult["res_ref_range"] = drResult["res_ref_l_cal"].ToString() + " - " + drResult["res_ref_h_cal"].ToString();
                                        }
                                        else
                                        {
                                            drResult["res_ref_range"] = drResult["res_ref_l_cal"].ToString() + drResult["res_ref_h_cal"].ToString();
                                        }

                                        drResult["res_pan_l_cal"] = drsItmRef[0]["itm_pan_l"];
                                        drResult["res_pan_h_cal"] = drsItmRef[0]["itm_pan_h"];

                                        drResult["res_min_cal"] = drsItmRef[0]["itm_min"];
                                        drResult["res_max_cal"] = drsItmRef[0]["itm_max"];


                                        drResult["res_meams"] = drsItmRef[0]["itm_meams"];
                                        drResult["itm_dtype"] = drsItmRef[0]["itm_dtype"];
                                        drResult["itm_max_digit"] = drsItmRef[0]["itm_max_digit"];
                                        drResult["res_unit"] = drsItmRef[0]["itm_unit"];
                                        //resulto.res_unit,--单位
                                        //itm_dtype = '',--项目数据类型
                                        //itm_max_digit = 0,--小数位数
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }

                foreach (DataRow drResult in dtResult.Rows)
                {
                    ItemRefFormatter.Format(drResult, "res_ref_l_cal", "res_ref_h_cal", "res_min_cal", "res_max_cal", "res_pan_l_cal", "res_pan_h_cal");
                }

                if (getHistory)
                {
                    //获取最近3次历史记录
                    #region 获取最近3次结果
                    //DataTable dtPatientInfo = GetPatientInfo(patID);
                    DataTable dtPatientInfo = new DBHelper().GetTable(string.Format(@"
select pat_name,pat_itr_id,pat_sam_id,pat_no_id,pat_in_no,pat_social_no,pat_pid,pat_age,pat_sex,pat_date,pat_upid from patients where pat_id ='{0}'", patID));
                    if (dtPatientInfo.Rows.Count > 0)
                    {
                        DataRow drPatient = dtPatientInfo.Rows[0];

                        string pat_name = string.Empty;
                        string pat_itr_id = string.Empty;
                        string pat_sam_id = string.Empty;
                        string pat_no_id = string.Empty;
                        string pat_in_no = string.Empty;
                        string pat_sex = string.Empty;
                        int ageMinute = -1;
                        DateTime? patDate = null;

                        if (!Compare.IsNullOrDBNull(drPatient["pat_date"]))
                        {
                            patDate = DateTime.Parse(drPatient["pat_date"].ToString());

                        }

                        //string itr_id, string pat_sam_id, string pat_no_id, string pat_in_no, string pat_name, int resultCount
                        if (drPatient["pat_name"] != null && drPatient["pat_name"] != DBNull.Value)
                        {
                            pat_name = drPatient["pat_name"].ToString();
                        }

                        if (drPatient["pat_itr_id"] != null && drPatient["pat_itr_id"] != DBNull.Value)
                        {
                            pat_itr_id = drPatient["pat_itr_id"].ToString();
                        }

                        if (drPatient["pat_sam_id"] != null && drPatient["pat_sam_id"] != DBNull.Value)
                        {
                            pat_sam_id = drPatient["pat_sam_id"].ToString();
                        }

                        if (drPatient["pat_no_id"] != null && drPatient["pat_no_id"] != DBNull.Value)
                        {
                            pat_no_id = drPatient["pat_no_id"].ToString();
                        }

                        if (drPatient["pat_in_no"] != null && drPatient["pat_in_no"] != DBNull.Value)
                        {
                            pat_in_no = drPatient["pat_in_no"].ToString();
                        }

                        if (!Compare.IsNullOrDBNull(drPatient["pat_age"]))
                        {
                            ageMinute = Convert.ToInt32(drPatient["pat_age"]);
                        }

                        if (!Compare.IsNullOrDBNull(drPatient["pat_sex"]))
                        {
                            pat_sex = drPatient["pat_sex"].ToString();
                        }

                        if (pat_name != string.Empty
                            && pat_itr_id != string.Empty
                            && pat_sam_id != string.Empty
                            && pat_no_id != string.Empty
                       //&& pat_in_no != string.Empty //edit by sink 2010-6-23 不判断pat_in_no,因为历史结果有可能不以此字段作查询                            )
                       )
                        {
                            #region //获取历史结果自定义查询列 edit by sink 2010-6-18
                            string historySelectColumnValue = "";

                            string historySelectColumn = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn"); //dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");
                            if (string.IsNullOrEmpty(historySelectColumn))
                                historySelectColumn = "pat_in_no";
                            else
                            {
                                historySelectColumn = historySelectColumn.Replace("patients.", "");
                                if (drPatient.Table.Columns.Contains(historySelectColumn) && !Compare.IsNullOrDBNull(drPatient[historySelectColumn]))
                                {
                                    historySelectColumnValue = drPatient[historySelectColumn].ToString();
                                }
                            }
                            #endregion

                            if (dtResult.Rows.Count > 0)
                            {
                                //PatientEnterService pEnter = new PatientEnterService();
                                dthistoryResult = GetPatCommonResultHistory(patID, pat_itr_id, pat_sam_id, pat_no_id, pat_in_no, pat_name, pat_sex, patDate, 10, historySelectColumn, historySelectColumnValue);
                                StringBuilder sbResultId = new StringBuilder();//项目编码
                                StringBuilder sbResultEcd = new StringBuilder();//项目代码
                                foreach (DataRow drResult in dtResult.Rows)
                                {
                                    sbResultId.Append(string.Format(",'{0}'", drResult["res_itm_id"]));
                                    if (!string.IsNullOrEmpty(drResult["res_itm_ecd"].ToString()))
                                    {
                                        sbResultEcd.Append(string.Format(",'{0}'", drResult["res_itm_ecd"]));
                                    }
                                }
                                sbResultId.Remove(0, 1);
                                if (sbResultEcd.Length > 0) { sbResultEcd.Remove(0, 1); }

                                DataView dvhistoryResult = new DataView(dthistoryResult);

                                //是否用代码关联
                                //系统配置：历史结果123使用[项目代码]关联
                                bool IsResItmEcdConnected = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_History123WithItmEcd") == "是";

                                if (IsResItmEcdConnected)
                                {
                                    dvhistoryResult.RowFilter = string.Format("res_itm_ecd in ({0})", sbResultEcd);
                                }
                                else
                                {
                                    dvhistoryResult.RowFilter = string.Format("res_itm_id in ({0})", sbResultId);
                                }
                                dvhistoryResult.Sort = "res_date desc";
                                DataTable dtTimes = dvhistoryResult.ToTable(true, "res_id");

                                //如果用代码关联项目,则把相同代码的项目的编码更新为当前项目的编码
                                if (IsResItmEcdConnected && dtTimes != null && dtTimes.Rows.Count > 0)
                                {
                                    foreach (DataRow drResult in dtResult.Rows)
                                    {
                                        if (!string.IsNullOrEmpty(drResult["res_itm_ecd"].ToString()))
                                        {
                                            DataRow[] drTempSelTimes = dthistoryResult.Select(string.Format("res_itm_ecd='{0}'", drResult["res_itm_ecd"].ToString()));
                                            for (int tempn = 0; tempn < drTempSelTimes.Length; tempn++)
                                            {
                                                drTempSelTimes[tempn]["res_itm_id"] = drResult["res_itm_id"].ToString();
                                            }
                                        }
                                    }
                                    dthistoryResult.AcceptChanges();
                                }

                                try
                                {
                                    if (dtTimes.Rows.Count > 0)
                                    {
                                        //13-07-11 注释掉 by ou
                                        /*********************************************
                                        for (int j = 0; j < dtTimes.Rows.Count; j++)
                                        {
                                            DataRow drTime = dtTimes.Rows[j];
                                            foreach (DataRow drResult in dtResult.Rows)
                                            {
                                                if (drResult["res_itm_id"] != null && drResult["res_itm_id"] != DBNull.Value)
                                                {
                                                    string itm_id = drResult["res_itm_id"].ToString();

                                                    DataRow[] drs = dthistoryResult.Select(string.Format("res_itm_id ='{0}' and res_id='{1}'", itm_id, drTime["res_id"]), "res_date desc");

                                                    if (drs.Length > 0)
                                                    {
                                                        drResult["history_result" + (j + 1).ToString()] = drs[0]["res_chr"];
                                                        drResult["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                        dtResult.Rows[0]["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");

                                                    }
                                                }
                                            }
                                        }
                                        *********************************************/
                                        List<string> tempHistoryResIDs = new List<string>();//最近3次历史报告ID
                                        #region 最近3次历史报告ID
                                        if (dthistoryResult != null && dthistoryResult.Rows.Count > 0)
                                        {
                                            DataRow[] drs = dthistoryResult.Select("1=1", "res_date desc");
                                            for (int k = 0; k < drs.Length; k++)
                                            {
                                                if (tempHistoryResIDs.Count >= 3)
                                                {
                                                    break;
                                                }
                                                if (!string.IsNullOrEmpty(drs[k]["res_id"].ToString())
                                                    && !tempHistoryResIDs.Contains(drs[k]["res_id"].ToString()))
                                                {
                                                    tempHistoryResIDs.Add(drs[k]["res_id"].ToString());
                                                }
                                            }
                                        }
                                        #endregion

                                        //系统配置：历史结果123每列关联一份报告
                                        bool IsLabHistory123ByResID = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_History123ByResID") == "是";

                                        //13-07-11 新 查找每项目最近三次结果 by ou
                                        foreach (DataRow drResult in dtResult.Rows)
                                        {
                                            if (drResult["res_itm_id"] != null && drResult["res_itm_id"] != DBNull.Value)
                                            {
                                                string itm_id = drResult["res_itm_id"].ToString();

                                                if (IsLabHistory123ByResID)
                                                {
                                                    #region 最近3份历史报告对应项目的结果

                                                    if (tempHistoryResIDs.Count <= 0) { break; }//如果最近没有历史报告,则跳出遍历
                                                    for (int j = 0; j < tempHistoryResIDs.Count; j++)
                                                    {
                                                        DataRow[] drs = dthistoryResult.Select(string.Format("res_itm_id ='{0}' and res_id='{1}'", itm_id, tempHistoryResIDs[j]), "res_date desc");
                                                        if (drs.Length > 0)
                                                        {
                                                            drResult["history_result" + (j + 1).ToString()] = drs[0]["res_chr"];
                                                            drResult["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                        }
                                                        if (j >= 2) break;
                                                    }

                                                    #endregion
                                                }
                                                else
                                                {
                                                    #region 每个项目最近3次结果
                                                    DataRow[] drs = dthistoryResult.Select(string.Format("res_itm_id ='{0}'", itm_id), "res_date desc");

                                                    if (drs.Length > 0)
                                                    {
                                                        for (int j = 0; j < drs.Length; j++)
                                                        {
                                                            drResult["history_result" + (j + 1).ToString()] = drs[j]["res_chr"];
                                                            drResult["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[j]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                            //dtResult.Rows[0]["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[j]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");

                                                            if (j >= 2) break;//如果等于大于2则跳出
                                                        }
                                                    }
                                                    #endregion
                                                }
                                            }
                                        }

                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.WriteException(this.GetType().Name, "GetPatientResult", ex.ToString());
                                }
                            }
                        }
                    }
                    #endregion
                }

                return dtResult;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人结果出错,patID=" + patID, ex.ToString());
                throw;
            }
        }

        private DataTable OrderComSort(DataTable input)
        {
            if (input != null && input.Rows.Count > 1)
            {
                //order by dict_combine.com_seq asc, dict_combine_mi.com_sort asc,dict_item.itm_seq ,resulto.res_itm_ecd asc
                input.DefaultView.Sort = "res_com_seq,res_itm_seq,itm_seq,res_itm_ecd";

                return input.DefaultView.ToTable();
            }
            else
            {
                return input;
            }
        }

        private DataTable ResultValidate(DataTable dtResulto)
        {
            return dtResulto;

            if (dtResulto.Rows.Count == 0)
                return dtResulto;

            string strItrId = dtResulto.Rows[0]["res_itr_id"].ToString();

            string strSql = "select * from Sys_instrmt where itr_id='" + strItrId + "'";

            DBHelper helper = new DBHelper();

            DataTable dtInstrmt = helper.GetTable(strSql);

            if (dtInstrmt.Rows.Count > 0)
            {
                //if (Lis.InstrmtEncrypt.InstrmtEncrypt.GetInstrmtEncrypt(strItrId) == dtInstrmt.Rows[0]["Itr_reg_code"].ToString())
                //    return dtResulto;
            }

            DataTable dtRenturn = dtResulto.Clone();

            foreach (DataRow item in dtResulto.Rows)
            {
                if (item["res_verify"] != null && item["res_verify"].ToString().Trim() != string.Empty)
                {
                    //string strVerify = Lis.InstrmtEncrypt.InstrmtEncrypt.GetInstrmtEncrypt(item["res_itm_id"].ToString() + ";" + item["res_chr"].ToString());
                    //if (strVerify == item["res_verify"].ToString())
                    //    dtRenturn.Rows.Add(item.ItemArray);
                }
            }

            return dtRenturn;
        }

        /// <summary>
        /// 获取单个病人普通结果
        /// </summary>
        /// <param name="res_key"></param>
        /// <returns></returns>
        public DataTable GetPatientCommonResultByKey(long res_key)
        {
            string sqlSelect = "select *  from resulto where res_key = @res_key";

            SqlCommand cmdSelect = new SqlCommand(sqlSelect);
            cmdSelect.Parameters.AddWithValue("res_key", res_key);

            DBHelper helper = new DBHelper();
            DataTable table = helper.GetTable(cmdSelect);
            return table;
        }

        public DataTable GetPatientCommonResultByItemID(string res_id, string res_itm_id)
        {
            string sqlSelect = string.Format("select *  from resulto where res_id ='{0}' and res_itm_id='{1}'", res_id, res_itm_id);


            DBHelper helper = new DBHelper();
            DataTable table = helper.GetTable(sqlSelect);
            return table;
        }

        /// <summary>
        /// 补充有缺省值必录项目
        /// </summary>
        /// <param name="pat_id"></param>
        public void FillNotNullResult(EntityPatient2 entityPatient)
        {
            //获取病人结果记录数
            string sqlSelectResulto = string.Format("select * from resulto where res_id = '{0}' and res_flag = 1", entityPatient.pat_id);
            DBHelper helper = new DBHelper();

            DataTable dtResulto = helper.GetTable(sqlSelectResulto);

            //没有结果才补充
            if (dtResulto.Rows.Count == 0)
            {
                //                //获取病人标本类别
                //                string sqlSelectPatient = string.Format(@"
                //select top 1
                //pat_sam_id,
                //pat_itr_id,
                //pat_sid
                //from patients where pat_id = '{0}'", entityPatient.pat_id);

                //                DataTable dtPat = helper.GetTable(sqlSelectPatient);

                //                if (dtPat.Rows.Count > 0 && !Compare.IsEmpty(dtPat.Rows[0]["pat_sam_id"]))
                //                {
                //string pat_sam_id = dtPat.Rows[0]["pat_sam_id"].ToString();
                //string pat_itr_id = dtPat.Rows[0]["pat_itr_id"].ToString();
                //string pat_sid = dtPat.Rows[0]["pat_sid"].ToString();




                DateTime dtToday = ServerDateTime.GetDatabaseServerDateTime();

                //获取病人检验组合
                string sqlSelectPatientsMi = string.Format(@"
select
pat_com_id
from patients_mi
where pat_id = '{0}' and pat_com_id is not null and pat_com_id <> ''
order by patients_mi.pat_seq asc
", entityPatient.pat_id);
                DataTable dtPatMi = helper.GetTable(sqlSelectPatientsMi);

                if (dtPatMi.Rows.Count > 0)
                {
                    foreach (DataRow drCom in dtPatMi.Rows)
                    {
                        string com_id = drCom["pat_com_id"].ToString();

                        DataTable dtItem = DictCombineMi.NewInstance.GetCombineMiWdthDefault(com_id, entityPatient.pat_sam_id, false);

                        foreach (DataRow drItem in dtItem.Rows)
                        {
                            string itm_id = drItem["itm_id"].ToString();

                            if (dtResulto.Select(string.Format("res_itm_id = '{0}' ", itm_id)).Length == 0)
                            {

                                DataRow drResulto = dtResulto.NewRow();
                                drResulto["res_id"] = entityPatient.pat_id;
                                drResulto["res_itr_id"] = entityPatient.pat_itr_id;
                                drResulto["res_sid"] = entityPatient.pat_sid;
                                drResulto["res_itm_id"] = itm_id;
                                drResulto["res_itm_ecd"] = drItem["itm_ecd"];
                                drResulto["res_chr"] = drItem["itm_defa"];
                                drResulto["res_unit"] = drItem["itm_unit"];
                                drResulto["res_date"] = dtToday;
                                drResulto["res_flag"] = 1;
                                drResulto["res_type"] = 0;
                                drResulto["res_com_id"] = com_id;
                                drResulto["res_itm_rep_ecd"] = drItem["itm_rep_ecd"];

                                dtResulto.Rows.Add(drResulto);
                            }
                        }

                    }
                }
                //}

                if (dtResulto.Rows.Count > 0)
                {
                    List<SqlCommand> listCmdResultoInsert = DBTableHelper.GenerateInsertCommand("resulto", new string[] { "res_key" }, dtResulto);

                    //using (DBHelper transHelper = DBHelper.BeginTransaction())
                    //{
                    DBHelper transHelper = new DBHelper();
                    foreach (SqlCommand cmd in listCmdResultoInsert)
                    {
                        transHelper.ExecuteNonQuery(cmd);
                    }

                    //    transHelper.Commit();
                    //}
                }
            }
        }

        /// <summary>
        /// 更新所有没有组合id的项目
        /// </summary>
        /// <param name="pat_id"></param>
        public Dictionary<string, string> UpdateResultNotCombineItem(string pat_id)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string sqlSelect = string.Format(@"
select
res_id,
res_itm_id,
res_itm_ecd,
res_com_id
from resulto with(nolock)
where res_id = '{0}' and (res_com_id is null or res_com_id = '' or res_com_id='-1') and res_itm_id is not null and res_itm_id <> ''
", pat_id);

            DBHelper helper = new DBHelper();

            DataTable dtNullComIDItems = helper.GetTable(sqlSelect);

            if (dtNullComIDItems.Rows.Count > 0)
            {
                string sqlSelectPatComMi = string.Format(@"
select pat_com_id from patients_mi with(nolock) where pat_id = '{0}' order by pat_seq asc
", pat_id);

                DataTable dtpatcom = helper.GetTable(sqlSelectPatComMi);

                if (dtpatcom.Rows.Count > 0)
                {
                    string com_id_in = string.Empty;
                    bool needComma = false;
                    foreach (DataRow drPatCom in dtpatcom.Rows)
                    {
                        string pat_com_id = drPatCom["pat_com_id"].ToString();

                        if (needComma)
                        {
                            com_id_in += ",";
                        }

                        com_id_in += string.Format("'{0}'", pat_com_id);

                        needComma = true;
                    }

                    string sqlSelectComItems = string.Format(@"
select
com_id,
com_itm_id,
com_popedom,
com_sort
from dict_combine_mi with(nolock)
where com_id in ({0})
", com_id_in);

                    DataTable dtComMi = helper.GetTable(sqlSelectComItems);

                    if (dtComMi.Rows.Count > 0)
                    {
                        List<SqlCommand> listCmd = new List<SqlCommand>();
                        string sqlUpdateResultComID = string.Format("update resulto set res_com_id = @res_com_id where res_id = @res_id and res_itm_id = @res_itm_id");

                        foreach (DataRow rowNullComIDItem in dtNullComIDItems.Rows)
                        {
                            string itm_id = rowNullComIDItem["res_itm_id"].ToString();

                            DataRow[] drsComMi = dtComMi.Select(string.Format("com_itm_id = '{0}'", itm_id));

                            if (drsComMi.Length > 0)
                            {
                                //rowNullComIDItem["res_com_id"] = drsComMi[0]["com_id"];

                                if (!dic.ContainsKey(itm_id)
                                   && !string.IsNullOrEmpty(drsComMi[0]["com_id"].ToString()))
                                {
                                    dic.Add(itm_id, drsComMi[0]["com_id"].ToString());
                                }


                                SqlCommand cmd = new SqlCommand(sqlUpdateResultComID);
                                SqlParameter p1 = cmd.Parameters.AddWithValue("res_com_id", drsComMi[0]["com_id"]);
                                p1.DbType = DbType.AnsiString;

                                SqlParameter p2 = cmd.Parameters.AddWithValue("res_id", pat_id);
                                p2.DbType = DbType.AnsiString;

                                SqlParameter p3 = cmd.Parameters.AddWithValue("res_itm_id", itm_id);
                                p3.DbType = DbType.AnsiString;

                                listCmd.Add(cmd);
                            }
                        }

                        System.Threading.Thread tempThr =
                            new System.Threading.Thread(ThreadUpdateResultNotCombineItem);
                        tempThr.Start((object)listCmd);

                        //using (DBHelper transhelper = DBHelper.BeginTransaction())
                        //{
                        //DBHelper transhelper = new DBHelper();
                        //foreach (SqlCommand cmd in listCmd)
                        //{
                        //    transhelper.ExecuteNonQuery(cmd);
                        //}
                        //    transhelper.Commit();
                        //}
                    }
                }
            }

            return dic;
        }

        /// <summary>
        /// 多线程更新所有没有组合id的项目
        /// </summary>
        /// <param name="ltcmd"></param>
        public void ThreadUpdateResultNotCombineItem(object ltcmd)
        {
            try
            {
                if (ltcmd != null && ltcmd is List<SqlCommand>)
                {
                    List<SqlCommand> listCmd = ltcmd as List<SqlCommand>;
                    if (listCmd != null && listCmd.Count > 0)
                    {
                        Lib.DAC.SqlHelper sqlhelper = new SqlHelper();
                        foreach (SqlCommand cmd in listCmd)
                        {
                            sqlhelper.ExecuteNonQuery(cmd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("多线程更新没有组合id项目结果", ex);
            }
        }

        /// <summary>
        /// 生成自动关联计算项目
        /// </summary>
        /// <param name="pat_id"></param>
        public void GenerateAutoCalItem(EntityPatient2 entityPatient, string sampName, string sampRem)
        {
            //系统配置：关闭自动关联计算项目的功能
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_stopCalItem") == "是")
            {
                return;//不自动关联计算项目
            }

            DBHelper helper = new DBHelper();

            //            //获取病人标本类别
            //            string sqlSelectPatient = string.Format(@"
            //select top 1
            //pat_sam_id,
            //pat_itr_id,
            //pat_sid
            //from patients where pat_id = '{0}'", pat_id);

            //DataTable dtPat = helper.GetTable(sqlSelectPatient);

            //if (dtPat.Rows.Count > 0)
            //{
            //string pat_sam_id = dtPat.Rows[0]["pat_sam_id"].ToString();
            //string pat_itr_id = dtPat.Rows[0]["pat_itr_id"].ToString();
            //string pat_sid = dtPat.Rows[0]["pat_sid"].ToString();

            //查找当前病人结果
            string sqlSelectResulto = string.Format(@"select
resulto.*,
dict_item.itm_cal_flag,
dict_item.itm_ecd
from resulto with(nolock)
left join dict_item on dict_item.itm_id =  resulto.res_itm_id
where res_id = '{0}' and res_flag = 1", entityPatient.pat_id);
            DataTable dtPatientResulto = helper.GetTable(sqlSelectResulto);

            if (dtPatientResulto == null || dtPatientResulto.Rows.Count == 0) return;
            //生成关联计算参数表
            Hashtable ht = new Hashtable();
            foreach (DataRow drSource in dtPatientResulto.Rows)
            {
                if (drSource["res_chr"] != null && drSource["res_chr"] != DBNull.Value && drSource["res_chr"].ToString().Trim(null) != string.Empty)
                {
                    string item_ecd = drSource["res_itm_ecd"].ToString();

                    if (!ht.Contains(item_ecd))
                    {
                        ht.Add(item_ecd, drSource["res_chr"]);
                    }
                }
            }

            //            string selectCalItem = string.Format(@"
            //select 
            //dict_cl_item.*,
            //dict_item.itm_ecd
            //from dict_cl_item 
            //inner join dict_item on dict_cl_item.cal_itm_ecd = dict_item.itm_id
            //where dict_cl_item.cal_flag = 1 and dict_item.itm_cal_flag = 1
            //ORDER BY cal_seq
            //                ");

            DataTable dtCalItem = dcl.svr.cache.DictClItemCache.Current.GetAllData();// helper.GetTable(selectCalItem);

            DataSet dsResult = Variable(ht, dtCalItem, sampName, sampRem, entityPatient);
            DataTable dtCalResult = dsResult.Tables[0];

            List<SqlCommand> listCmdUpdate = new List<SqlCommand>();
            string sqlUpdate = "update resulto set res_chr = @res_chr,res_cast_chr = @res_cast_chr where res_key = @res_key";

            if (dtCalResult.Rows.Count > 0)
            {
                DateTime now = ServerDateTime.GetDatabaseServerDateTime();

                DataTable dtComItem = GetDictCombineItem(entityPatient.pat_id);

                DataTable dtInsert = dtPatientResulto.Clone();
                DataTable dtUpdate = dtPatientResulto.Clone();

                foreach (DataRow drResult in dtCalResult.Rows)
                {

                    string itm_id = drResult["cal_item_ecd"].ToString();
                    string itm_ecd = drResult["itm_ecd"].ToString();
                    string value = drResult["retu"].ToString();


                    string valueINItmProp = value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        decimal dec = 0;

                        if (decimal.TryParse(value, out dec))
                        {
                            dec = decimal.Round(dec, 2);

                            valueINItmProp = dec.ToString();
                        }
                    }
                    string strProp = dcl.svr.cache.DictItemPropCache.Current.GetItmProp(itm_id, valueINItmProp);

                    value = strProp == string.Empty ? value : strProp;

                    DataRow[] existItems = dtPatientResulto.Select(string.Format("res_itm_id='{0}'", itm_id));

                    DataRow[] drComItems = dtComItem.Select("com_itm_id='" + itm_id + "'");

                    if (drComItems.Length > 0 || dtComItem.Rows.Count == 0)
                    {
                        if (existItems.Length == 0 && dtInsert.Select(string.Format("res_itm_id='{0}'", itm_id)).Length == 0)//项目不存在：添加
                        {
                            DataRow drInsert = dtInsert.NewRow();

                            drInsert["res_id"] = entityPatient.pat_id;
                            drInsert["res_itm_id"] = itm_id;
                            drInsert["res_itm_ecd"] = itm_ecd;
                            drInsert["res_flag"] = 1;
                            drInsert["res_itr_id"] = entityPatient.pat_itr_id;
                            drInsert["res_sid"] = entityPatient.pat_sid;
                            //drInsert["res_sam_id"] = pat_sam_id;d
                            drInsert["res_date"] = now;
                            drInsert["res_chr"] = value;
                            drInsert["res_type"] = LIS_Const.PatResultType.Cal;
                            drInsert["res_itm_rep_ecd"] = itm_ecd;

                            dtInsert.Rows.Add(drInsert);
                        }
                        else//存在：更新结果
                        {
                            ////if (existItems[0]["itm_cal_flag"].ToString() == "1")
                            //{
                            //SqlCommand cmUpdate = new SqlCommand(sqlUpdate);
                            //cmUpdate.Parameters.AddWithValue("res_chr", value);

                            //if (Compare.IsEmpty(value))
                            //{
                            //    cmUpdate.Parameters.AddWithValue("res_cast_chr", DBNull.Value);
                            //}
                            //else
                            //{
                            //    decimal decValue = 0;
                            //    if (decimal.TryParse(value, out decValue))
                            //        cmUpdate.Parameters.AddWithValue("res_cast_chr", value);
                            //    else
                            //        cmUpdate.Parameters.AddWithValue("res_cast_chr", DBNull.Value);
                            //}

                            //cmUpdate.Parameters.AddWithValue("res_key", existItems[0]["res_key"]);

                            //listCmdUpdate.Add(cmUpdate);
                            //}
                            //如果结果为空才进行关联计算（只计算一次）
                            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_AllowEditCalItem") == "是" && !string.IsNullOrEmpty(existItems[0]["res_chr"].ToString().Trim()))
                                continue;
                            SqlCommand cmUpdate = new SqlCommand(sqlUpdate);
                            cmUpdate.Parameters.AddWithValue("res_chr", value);

                            if (Compare.IsEmpty(value))
                            {
                                cmUpdate.Parameters.AddWithValue("res_cast_chr", DBNull.Value);
                            }
                            else
                            {
                                decimal decValue = 0;
                                if (decimal.TryParse(value, out decValue))
                                    cmUpdate.Parameters.AddWithValue("res_cast_chr", value);
                                else
                                    cmUpdate.Parameters.AddWithValue("res_cast_chr", DBNull.Value);
                            }

                            cmUpdate.Parameters.AddWithValue("res_key", existItems[0]["res_key"]);

                            listCmdUpdate.Add(cmUpdate);

                        }
                    }
                }

                List<SqlCommand> cmdsResult = DBTableHelper.GenerateInsertCommand(PatientTable.PatientResultTableName, new string[] { "res_key" }, dtInsert);

                DBHelper transHelper = new DBHelper();
                //using (DBHelper transHelper = DBHelper.BeginTransaction())
                //{
                foreach (SqlCommand cmd in cmdsResult)
                {
                    transHelper.ExecuteNonQuery(cmd);
                }

                foreach (SqlCommand cmd in listCmdUpdate)
                {
                    transHelper.ExecuteNonQuery(cmd);
                }

                //transHelper.Commit();
                //}
            }

            //}
        }

        /// <summary>
        /// 生成自动关联计算项目(供结果修正使用)
        /// </summary>
        /// <param name="entityPatient"></param>
        /// <param name="sampName"></param>
        /// <param name="sampRem"></param>
        public void GenerateAutoCalItemToOut(EntityPatient2 entityPatient, string sampName, string sampRem)
        {
            //系统配置：关闭自动关联计算项目的功能
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_stopCalItem") == "是")
            {
                return;//不自动关联计算项目
            }

            DBHelper helper = new DBHelper();

            //            //获取病人标本类别
            //            string sqlSelectPatient = string.Format(@"
            //select top 1
            //pat_sam_id,
            //pat_itr_id,
            //pat_sid
            //from patients where pat_id = '{0}'", pat_id);

            //DataTable dtPat = helper.GetTable(sqlSelectPatient);

            //if (dtPat.Rows.Count > 0)
            //{
            //string pat_sam_id = dtPat.Rows[0]["pat_sam_id"].ToString();
            //string pat_itr_id = dtPat.Rows[0]["pat_itr_id"].ToString();
            //string pat_sid = dtPat.Rows[0]["pat_sid"].ToString();

            //查找当前病人结果
            string sqlSelectResulto = string.Format(@"select
resulto.*,
dict_item.itm_cal_flag,
dict_item.itm_ecd
from resulto with(nolock)
left join dict_item on dict_item.itm_id =  resulto.res_itm_id
where res_id = '{0}' and res_flag = 1", entityPatient.pat_id);
            DataTable dtPatientResulto = helper.GetTable(sqlSelectResulto);


            //生成关联计算参数表
            Hashtable ht = new Hashtable();
            foreach (DataRow drSource in dtPatientResulto.Rows)
            {
                if (drSource["res_chr"] != null && drSource["res_chr"] != DBNull.Value && drSource["res_chr"].ToString().Trim(null) != string.Empty)
                {
                    string item_ecd = drSource["res_itm_ecd"].ToString();

                    if (!ht.Contains(item_ecd))
                    {
                        ht.Add(item_ecd, drSource["res_chr"]);
                    }
                }
            }

            //            string selectCalItem = string.Format(@"
            //select 
            //dict_cl_item.*,
            //dict_item.itm_ecd
            //from dict_cl_item 
            //inner join dict_item on dict_cl_item.cal_itm_ecd = dict_item.itm_id
            //where dict_cl_item.cal_flag = 1 and dict_item.itm_cal_flag = 1
            //ORDER BY cal_seq
            //                ");

            DataTable dtCalItem = dcl.svr.cache.DictClItemCache.Current.GetAllData();// helper.GetTable(selectCalItem);

            DataSet dsResult = Variable(ht, dtCalItem, sampName, sampRem, entityPatient);
            DateTime now = ServerDateTime.GetDatabaseServerDateTime();
            DataTable dtCalResult = dsResult.Tables[0];

            List<SqlCommand> listCmdUpdate = new List<SqlCommand>();
            string sqlUpdate = "update resulto set res_chr = @res_chr,res_cast_chr = @res_cast_chr where res_key = @res_key";

            if (dtCalResult.Rows.Count > 0)
            {
                DataTable dtComItem = GetDictCombineItem(entityPatient.pat_id);

                DataTable dtInsert = dtPatientResulto.Clone();
                DataTable dtUpdate = dtPatientResulto.Clone();

                foreach (DataRow drResult in dtCalResult.Rows)
                {

                    string itm_id = drResult["cal_item_ecd"].ToString();
                    string itm_ecd = drResult["itm_ecd"].ToString();
                    string value = drResult["retu"].ToString();


                    string valueINItmProp = value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        decimal dec = 0;

                        if (decimal.TryParse(value, out dec))
                        {
                            dec = decimal.Round(dec, 2);

                            valueINItmProp = dec.ToString();
                        }
                    }
                    string strProp = dcl.svr.cache.DictItemPropCache.Current.GetItmProp(itm_id, valueINItmProp);

                    value = strProp == string.Empty ? value : strProp;

                    DataRow[] existItems = dtPatientResulto.Select(string.Format("res_itm_id='{0}'", itm_id));

                    DataRow[] drComItems = dtComItem.Select("com_itm_id='" + itm_id + "'");

                    if (drComItems.Length > 0 || dtComItem.Rows.Count == 0)
                    {
                        if (existItems.Length == 0)//项目不存在：添加
                        {
                            DataRow drInsert = dtInsert.NewRow();

                            drInsert["res_id"] = entityPatient.pat_id;
                            drInsert["res_itm_id"] = itm_id;
                            drInsert["res_itm_ecd"] = itm_ecd;
                            drInsert["res_flag"] = 1;
                            drInsert["res_itr_id"] = entityPatient.pat_itr_id;
                            drInsert["res_sid"] = entityPatient.pat_sid;
                            //drInsert["res_sam_id"] = pat_sam_id;d
                            drInsert["res_date"] = now;
                            drInsert["res_chr"] = value;
                            drInsert["res_type"] = LIS_Const.PatResultType.Cal;
                            drInsert["res_itm_rep_ecd"] = itm_ecd;

                            dtInsert.Rows.Add(drInsert);
                        }
                        else//存在：更新结果
                        {
                            ////if (existItems[0]["itm_cal_flag"].ToString() == "1")
                            //{
                            //SqlCommand cmUpdate = new SqlCommand(sqlUpdate);
                            //cmUpdate.Parameters.AddWithValue("res_chr", value);

                            //if (Compare.IsEmpty(value))
                            //{
                            //    cmUpdate.Parameters.AddWithValue("res_cast_chr", DBNull.Value);
                            //}
                            //else
                            //{
                            //    decimal decValue = 0;
                            //    if (decimal.TryParse(value, out decValue))
                            //        cmUpdate.Parameters.AddWithValue("res_cast_chr", value);
                            //    else
                            //        cmUpdate.Parameters.AddWithValue("res_cast_chr", DBNull.Value);
                            //}

                            //cmUpdate.Parameters.AddWithValue("res_key", existItems[0]["res_key"]);

                            //listCmdUpdate.Add(cmUpdate);
                            //}

                            /**由于供给外部调用,因此注释掉此限制
                            //如果结果为空才进行关联计算（只计算一次）
                            //if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_AllowEditCalItem") == "是" && !string.IsNullOrEmpty(existItems[0]["res_chr"].ToString().Trim()))
                            //    continue;
                            **/

                            SqlCommand cmUpdate = new SqlCommand(sqlUpdate);
                            cmUpdate.Parameters.AddWithValue("res_chr", value);

                            if (Compare.IsEmpty(value))
                            {
                                cmUpdate.Parameters.AddWithValue("res_cast_chr", DBNull.Value);
                            }
                            else
                            {
                                decimal decValue = 0;
                                if (decimal.TryParse(value, out decValue))
                                    cmUpdate.Parameters.AddWithValue("res_cast_chr", value);
                                else
                                    cmUpdate.Parameters.AddWithValue("res_cast_chr", DBNull.Value);
                            }

                            cmUpdate.Parameters.AddWithValue("res_key", existItems[0]["res_key"]);

                            listCmdUpdate.Add(cmUpdate);

                        }
                    }
                }

                List<SqlCommand> cmdsResult = DBTableHelper.GenerateInsertCommand(PatientTable.PatientResultTableName, new string[] { "res_key" }, dtInsert);

                DBHelper transHelper = new DBHelper();
                //using (DBHelper transHelper = DBHelper.BeginTransaction())
                //{
                foreach (SqlCommand cmd in cmdsResult)
                {
                    transHelper.ExecuteNonQuery(cmd);
                }

                foreach (SqlCommand cmd in listCmdUpdate)
                {
                    transHelper.ExecuteNonQuery(cmd);
                }

                //transHelper.Commit();
                //}
            }

            //}
        }

        /// <summary>
        /// 更新所有没有组合id的项目
        /// </summary>
        /// <param name="pat_id"></param>
        public void UpdateResultNotCombineItemForBf(string pat_id)
        {
            string sqlSelect = string.Format(@"
select
res_id,
res_itm_id,
res_itm_ecd,
res_com_id
from resulto_newborn with(nolock)
where res_id = '{0}' and (res_com_id is null or res_com_id = '') and res_itm_id is not null and res_itm_id <> ''
", pat_id);

            DBHelper helper = new DBHelper();

            DataTable dtNullComIDItems = helper.GetTable(sqlSelect);

            if (dtNullComIDItems.Rows.Count > 0)
            {
                string sqlSelectPatComMi = string.Format(@"
select pat_com_id from patients_mi_newborn with(nolock) where pat_id = '{0}' order by pat_seq asc
", pat_id);

                DataTable dtpatcom = helper.GetTable(sqlSelectPatComMi);

                if (dtpatcom.Rows.Count > 0)
                {
                    string com_id_in = string.Empty;
                    bool needComma = false;
                    foreach (DataRow drPatCom in dtpatcom.Rows)
                    {
                        string pat_com_id = drPatCom["pat_com_id"].ToString();

                        if (needComma)
                        {
                            com_id_in += ",";
                        }

                        com_id_in += string.Format("'{0}'", pat_com_id);

                        needComma = true;
                    }

                    string sqlSelectComItems = string.Format(@"
select
com_id,
com_itm_id,
com_popedom,
com_sort
from dict_combine_mi with(nolock)
where com_id in ({0})
", com_id_in);

                    DataTable dtComMi = helper.GetTable(sqlSelectComItems);

                    if (dtComMi.Rows.Count > 0)
                    {
                        List<SqlCommand> listCmd = new List<SqlCommand>();
                        string sqlUpdateResultComID = string.Format("update resulto_newborn set res_com_id = @res_com_id where res_id = @res_id and res_itm_id = @res_itm_id");

                        foreach (DataRow rowNullComIDItem in dtNullComIDItems.Rows)
                        {
                            string itm_id = rowNullComIDItem["res_itm_id"].ToString();

                            DataRow[] drsComMi = dtComMi.Select(string.Format("com_itm_id = '{0}'", itm_id));

                            if (drsComMi.Length > 0)
                            {
                                //rowNullComIDItem["res_com_id"] = drsComMi[0]["com_id"];

                                SqlCommand cmd = new SqlCommand(sqlUpdateResultComID);
                                SqlParameter p1 = cmd.Parameters.AddWithValue("res_com_id", drsComMi[0]["com_id"]);
                                p1.DbType = DbType.AnsiString;

                                SqlParameter p2 = cmd.Parameters.AddWithValue("res_id", pat_id);
                                p2.DbType = DbType.AnsiString;

                                SqlParameter p3 = cmd.Parameters.AddWithValue("res_itm_id", itm_id);
                                p3.DbType = DbType.AnsiString;

                                listCmd.Add(cmd);
                            }
                        }


                        //using (DBHelper transhelper = DBHelper.BeginTransaction())
                        //{
                        DBHelper transhelper = new DBHelper();
                        foreach (SqlCommand cmd in listCmd)
                        {
                            transhelper.ExecuteNonQuery(cmd);
                        }
                        //    transhelper.Commit();
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// 生成自动关联计算项目
        /// </summary>
        /// <param name="pat_id"></param>
        public void GenerateAutoCalItemForBf(EntityPatient2 entityPatient, string sampName, string sampRem)
        {
            //系统配置：关闭自动关联计算项目的功能
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_stopCalItem") == "是")
            {
                return;//不自动关联计算项目
            }

            DBHelper helper = new DBHelper();

            //            //获取病人标本类别
            //            string sqlSelectPatient = string.Format(@"
            //select top 1
            //pat_sam_id,
            //pat_itr_id,
            //pat_sid
            //from patients where pat_id = '{0}'", pat_id);

            //DataTable dtPat = helper.GetTable(sqlSelectPatient);

            //if (dtPat.Rows.Count > 0)
            //{
            //string pat_sam_id = dtPat.Rows[0]["pat_sam_id"].ToString();
            //string pat_itr_id = dtPat.Rows[0]["pat_itr_id"].ToString();
            //string pat_sid = dtPat.Rows[0]["pat_sid"].ToString();

            //查找当前病人结果
            string sqlSelectResulto = string.Format(@"select
resulto_newborn.*,
dict_item.itm_cal_flag,
dict_item.itm_ecd
from resulto_newborn with(nolock)
left join dict_item on dict_item.itm_id =  resulto_newborn.res_itm_id
where res_id = '{0}' and res_flag = 1", entityPatient.pat_id);
            DataTable dtPatientResulto = helper.GetTable(sqlSelectResulto);


            //生成关联计算参数表
            Hashtable ht = new Hashtable();
            foreach (DataRow drSource in dtPatientResulto.Rows)
            {
                if (drSource["res_chr"] != null && drSource["res_chr"] != DBNull.Value && drSource["res_chr"].ToString().Trim(null) != string.Empty)
                {
                    string item_ecd = drSource["res_itm_ecd"].ToString();

                    if (!ht.Contains(item_ecd))
                    {
                        ht.Add(item_ecd, drSource["res_chr"]);
                    }
                }
            }

            //            string selectCalItem = string.Format(@"
            //select 
            //dict_cl_item.*,
            //dict_item.itm_ecd
            //from dict_cl_item 
            //inner join dict_item on dict_cl_item.cal_itm_ecd = dict_item.itm_id
            //where dict_cl_item.cal_flag = 1 and dict_item.itm_cal_flag = 1
            //ORDER BY cal_seq
            //                ");

            DataTable dtCalItem = dcl.svr.cache.DictClItemCache.Current.GetAllData();// helper.GetTable(selectCalItem);

            DataSet dsResult = Variable(ht, dtCalItem, sampName, sampRem, entityPatient);

            DataTable dtCalResult = dsResult.Tables[0];

            List<SqlCommand> listCmdUpdate = new List<SqlCommand>();
            string sqlUpdate = "update resulto_newborn set res_chr = @res_chr,res_cast_chr = @res_cast_chr where res_key = @res_key";

            if (dtCalResult.Rows.Count > 0)
            {
                DataTable dtComItem = GetDictCombineItemForBf(entityPatient.pat_id);

                DataTable dtInsert = dtPatientResulto.Clone();
                DataTable dtUpdate = dtPatientResulto.Clone();

                foreach (DataRow drResult in dtCalResult.Rows)
                {

                    string itm_id = drResult["cal_item_ecd"].ToString();
                    string itm_ecd = drResult["itm_ecd"].ToString();
                    string value = drResult["retu"].ToString();


                    string valueINItmProp = value;
                    if (!string.IsNullOrEmpty(value))
                    {
                        decimal dec = 0;

                        if (decimal.TryParse(value, out dec))
                        {
                            dec = decimal.Round(dec, 2);

                            valueINItmProp = dec.ToString();
                        }
                    }
                    string strProp = dcl.svr.cache.DictItemPropCache.Current.GetItmProp(itm_id, valueINItmProp);

                    value = strProp == string.Empty ? value : strProp;

                    DataRow[] existItems = dtPatientResulto.Select(string.Format("res_itm_id='{0}'", itm_id));

                    DataRow[] drComItems = dtComItem.Select("com_itm_id='" + itm_id + "'");

                    if (drComItems.Length > 0 || dtComItem.Rows.Count == 0)
                    {
                        if (existItems.Length == 0)//项目不存在：添加
                        {
                            DataRow drInsert = dtInsert.NewRow();

                            drInsert["res_id"] = entityPatient.pat_id;
                            drInsert["res_itm_id"] = itm_id;
                            drInsert["res_itm_ecd"] = itm_ecd;
                            drInsert["res_flag"] = 1;
                            drInsert["res_itr_id"] = entityPatient.pat_itr_id;
                            drInsert["res_sid"] = entityPatient.pat_sid;
                            //drInsert["res_sam_id"] = pat_sam_id;d
                            drInsert["res_date"] = DateTime.Now;
                            drInsert["res_chr"] = value;
                            drInsert["res_type"] = LIS_Const.PatResultType.Cal;
                            drInsert["res_itm_rep_ecd"] = itm_ecd;

                            dtInsert.Rows.Add(drInsert);
                        }
                        else//存在：更新结果
                        {

                            //如果结果为空才进行关联计算（只计算一次）
                            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_AllowEditCalItem") == "是" && !string.IsNullOrEmpty(existItems[0]["res_chr"].ToString().Trim()))
                                continue;
                            SqlCommand cmUpdate = new SqlCommand(sqlUpdate);
                            cmUpdate.Parameters.AddWithValue("res_chr", value);

                            if (Compare.IsEmpty(value))
                            {
                                cmUpdate.Parameters.AddWithValue("res_cast_chr", DBNull.Value);
                            }
                            else
                            {
                                decimal decValue = 0;
                                if (decimal.TryParse(value, out decValue))
                                    cmUpdate.Parameters.AddWithValue("res_cast_chr", value);
                                else
                                    cmUpdate.Parameters.AddWithValue("res_cast_chr", DBNull.Value);
                            }

                            cmUpdate.Parameters.AddWithValue("res_key", existItems[0]["res_key"]);

                            listCmdUpdate.Add(cmUpdate);

                        }
                    }
                }

                List<SqlCommand> cmdsResult = DBTableHelper.GenerateInsertCommand("resulto_newborn", new string[] { "res_key" }, dtInsert);

                DBHelper transHelper = new DBHelper();
                //using (DBHelper transHelper = DBHelper.BeginTransaction())
                //{
                foreach (SqlCommand cmd in cmdsResult)
                {
                    transHelper.ExecuteNonQuery(cmd);
                }

                foreach (SqlCommand cmd in listCmdUpdate)
                {
                    transHelper.ExecuteNonQuery(cmd);
                }

                //transHelper.Commit();
                //}
            }

            //}
        }

        /// <summary>
        /// 获取病人组合项目
        /// </summary>
        /// <param name="pat_id"></param>
        public DataTable GetDictCombineItem(string pat_id)
        {
            try
            {
                string sql = string.Format(@"
select dict_combine_mi.* from patients_mi 
left join dict_combine_mi ON patients_mi.pat_com_id = dict_combine_mi.com_id
where patients_mi.pat_id='{0}'
", pat_id);
                DBHelper helper = new DBHelper();

                Logger.Debug(string.Format("获取病人组合项目patID={0}", pat_id));
                DataTable dtCombine = helper.GetTable(sql);
                dtCombine.TableName = "dict_combine_mi";

                return dtCombine;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人组合项目信息出错,patID=" + pat_id, ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 获取病人组合项目
        /// </summary>
        /// <param name="pat_id"></param>
        public DataTable GetDictCombineItemForBf(string pat_id)
        {
            try
            {
                string sql = string.Format(@"
select dict_combine_mi.* from patients_mi_newborn 
left join dict_combine_mi ON patients_mi_newborn.pat_com_id = dict_combine_mi.com_id
where patients_mi_newborn.pat_id='{0}'
", pat_id);
                DBHelper helper = new DBHelper();

                Logger.Debug(string.Format("获取病人组合项目patID={0}", pat_id));
                DataTable dtCombine = helper.GetTable(sql);
                dtCombine.TableName = "dict_combine_mi";

                return dtCombine;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人组合项目信息出错,patID=" + pat_id, ex.ToString());
                throw;
            }
        }

        //公用效验方法
        private DataSet Variable(Hashtable ht, DataTable dtCalItem, string sampName, string sampRem, EntityPatient2 entityPatient)
        {
            string Pat_itr_id = entityPatient.pat_itr_id;
            DataTable dci = dtCalItem;
            string[] parm = new string[ht.Count];
            string[] value = new string[ht.Count];
            ht.Keys.CopyTo(parm, 0);
            ht.Values.CopyTo(value, 0);
            ArrayList list = new ArrayList();
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("cal_fmla");
            pb.Columns.Add("cal_flag");
            pb.Columns.Add("cal_item_ecd");//存ID
            pb.Columns.Add("itm_ecd");//存ECD
            pb.Columns.Add("cal_sp_formula");
            pb.Columns.Add("retu");

            //if (ht.ContainsKey("CysC") && ht.ContainsKey("CRE(酶法)") &&
            //  dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("LAB_EnableCalcCKD_EPI") == "是")
            //{
            //    return CalcCKD_EPI(ht, entityPatient);
            //}

            List<string> fmla = new List<string>();
            foreach (DataRow dr in dci.Rows)
            {
                string cal_item_ecd = string.Empty;
                if (dr["cal_itm_ecd"] != null && dr["cal_itm_ecd"] != DBNull.Value)
                {
                    cal_item_ecd = dr["cal_itm_ecd"].ToString();
                }
                if (dci.Columns.Contains("cal_itr_id") && dr["cal_itr_id"] != null &&
                  !string.IsNullOrEmpty(dr["cal_itr_id"].ToString()) && !string.IsNullOrEmpty(Pat_itr_id)
                  && dr["cal_itr_id"].ToString() != Pat_itr_id)
                {
                    continue;
                }
                if (dr["cal_fmla"] != null && !string.IsNullOrEmpty(dr["cal_fmla"].ToString()) &&
                   fmla.Contains(dr["cal_fmla"].ToString() + cal_item_ecd))
                {
                    continue;
                }
                if (dr["cal_fmla"] != null && !string.IsNullOrEmpty(dr["cal_fmla"].ToString()))
                {
                    fmla.Add(dr["cal_fmla"].ToString() + cal_item_ecd);
                }
                if (dr["cal_variable"].ToString() != "")
                {
                    string[] varpr = dr["cal_variable"].ToString().Split(',');
                    int count = 0;
                    for (int i = 0; i < parm.Length; i++)
                    {
                        for (int j = 0; j < varpr.Length; j++)
                        {
                            if (varpr[j].ToString() == parm[i].ToString())
                                count++;
                        }
                    }
                    if (count == varpr.Length && count > 0)
                    {
                        pb.Rows.Add(dr["cal_fmla"], dr["cal_flag"], dr["cal_itm_ecd"], dr["itm_ecd"], dr["cal_sp_formula"]);

                    }
                }
            }

            for (int i = 0; i < pb.Rows.Count; i++)
            {
                string methAll = pb.Rows[i]["cal_fmla"].ToString();
                string itmID = pb.Rows[i]["cal_item_ecd"].ToString();

                if (pb.Rows[i]["cal_sp_formula"] != null &&
                 !string.IsNullOrEmpty(pb.Rows[i]["cal_sp_formula"].ToString()))
                {
                    pb.Rows[i]["retu"] = new CalcItemResHelper().GetCalcRes(pb.Rows[i]["cal_sp_formula"].ToString(), ht, entityPatient);
                    continue;
                }

                for (int j = 0; j < ht.Count; j++)
                {
                    string fam = "[" + parm[j] + "]";

                    double dValue = 0;
                    try
                    {
                        if (!double.TryParse(value[j], out dValue))
                        {
                            for (int n = 0; n < value[j].Length; n++)
                            {

                                if (double.TryParse(value[j].Substring(n, 1), out dValue))
                                {
                                    value[j] = value[j].Substring(n);
                                    break;

                                }

                            }

                        }
                    }
                    catch
                    { }

                    double.TryParse(value[j], out dValue);

                    string va = dValue.ToString("0.0000");

                    methAll = methAll.Replace(fam, va);
                }

                DataTable dt = new DataTable();
                try
                {
                    object objValue = dt.Compute(methAll, string.Empty);

                    decimal decVal = 0;

                    if (decimal.TryParse(objValue.ToString(), out decVal))
                    {
                        //decVal = decimal.Round(decVal, 4);
                        //pb.Rows[i]["retu"] = decVal.ToString();
                        int? itm_max_digit = null;
                        dcl.pub.entities.dict.EntityDictItemSam itemSam = dcl.svr.cache.DictItemSamCache.Current.Cache.Find(k => k.itm_id == itmID && k.itm_sam_id == sampName);
                        if (itemSam != null)
                        {
                            itm_max_digit = itemSam.itm_max_digit;
                        }
                        if (itm_max_digit == null || itm_max_digit < 0)
                        {
                            decVal = decimal.Round(decVal, 4);
                            pb.Rows[i]["retu"] = decVal.ToString("0.00");
                        }
                        else
                        {
                            decVal = decimal.Round(decVal, itm_max_digit.Value);

                            if (itm_max_digit == 0)
                            {
                                pb.Rows[i]["retu"] = decVal.ToString();
                            }
                            else
                            {

                                pb.Rows[i]["retu"] = decVal.ToString(string.Format("0.{0}", new string('0', itm_max_digit.Value)));
                            }


                        }
                    }

                    //pb.Rows[i]["retu"] = .ToString();
                }
                catch
                {

                    //2013年2月28日16:45:32 叶
                    //当使用DataTable.Compute无法计算表达式的值时,比如带Math.Log()的表达式
                    //用动态编译后进行计算 
                    try
                    {
                        //2013年5月14日14:20:41 叶
                        if (methAll.Contains("[标本]"))
                        {

                            methAll = methAll.Replace("[标本]", string.Format("\"{0}\"", sampName));

                        }
                        if (methAll.Contains("[标本备注]"))
                        {
                            methAll = methAll.Replace("[标本备注]", string.Format("\"{0}\"", sampRem));

                        }
                        object objValue = ExpressionCompute.CalExpression(methAll);
                        if (objValue != null)
                        {
                            decimal decVal = 0;

                            if (decimal.TryParse(objValue.ToString(), out decVal))
                            {
                                //decVal = decimal.Round(decVal, 4);
                                //pb.Rows[i]["retu"] = decVal.ToString();
                                int? itm_max_digit = null;
                                dcl.pub.entities.dict.EntityDictItemSam itemSam = dcl.svr.cache.DictItemSamCache.Current.Cache.Find(k => k.itm_id == itmID && k.itm_sam_id == sampName);
                                if (itemSam != null)
                                {
                                    itm_max_digit = itemSam.itm_max_digit;
                                }
                                if (itm_max_digit == null || itm_max_digit < 0)
                                {
                                    decVal = decimal.Round(decVal, 4);
                                    pb.Rows[i]["retu"] = decVal.ToString("0.00");
                                }
                                else
                                {
                                    decVal = decimal.Round(decVal, itm_max_digit.Value);
                                    if (itm_max_digit == 0)
                                    {
                                        pb.Rows[i]["retu"] = decVal.ToString();
                                    }
                                    else
                                    {

                                        pb.Rows[i]["retu"] = decVal.ToString(string.Format("0.{0}", new string('0', itm_max_digit.Value)));
                                    }

                                }
                            }
                        }
                        else
                        {


                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }
            }

            DataSet dss = new DataSet();
            dss.Tables.Add(pb);
            return dss;
        }

        private DataSet CalcCKD_EPI(Hashtable ht, EntityPatient2 entityPatient)
        {
            DataTable pb = new DataTable();
            pb.TableName = "result";
            pb.Columns.Add("cal_fmla");
            pb.Columns.Add("cal_flag");
            pb.Columns.Add("cal_item_ecd"); //存ID
            pb.Columns.Add("itm_ecd"); //存ECD
            pb.Columns.Add("retu");
            try
            {

                if (entityPatient.pat_age.HasValue)
                {
                    string sex = entityPatient.pat_sex;

                    double age = 1;
                    int minute = entityPatient.pat_age.Value % 60;

                    int hour = entityPatient.pat_age.Value / 60;

                    int day = hour / 24;
                    hour = hour % 24;

                    int month = day / 30;
                    day = day % 30;

                    int year = month / 12;
                    month = month % 12;

                    if (year > 0)
                    {
                        age = year;
                    }
                    else
                    {
                        if (month > 0)
                            age = month / 12.0;
                    }
                    // PatAge age =AgeConverter.MinuteToAge(entityPatient.pat_age.Value)


                    string mathall = string.Empty;


                    //Math.Pow()
                    string cre = ht["CRE(酶法)"].ToString();
                    string csys = ht["CysC"].ToString();
                    double crevalue;
                    double csysvalue;
                    if (double.TryParse(cre, out crevalue) && double.TryParse(csys, out csysvalue))
                    {

                        if (sex == "1")
                        {
                            if ((crevalue / 88.4) < 0.9)
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                            Math.Round(crevalue / 88.4, 2), "0.207", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                            Math.Round(crevalue / 88.4, 2), "0.207", csysvalue, "0.711", age);
                                }
                            }
                            else
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "135* (Math.Pow(({0}/0.9),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.711", age);
                                }
                            }
                        }
                        else
                        {
                            if ((crevalue / 88.4) < 0.7)
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.248", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.248", csysvalue, "0.711", age);
                                }
                            }
                            else
                            {
                                if (csysvalue <= 0.8)
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.375", age);
                                }
                                else
                                {
                                    mathall =
                                        string.Format(
                                            "130* (Math.Pow(({0}/0.7),-{1}))*(Math.Pow(({2}/0.8),-{3}))*(Math.Pow(0.995,{4}))",
                                             Math.Round(crevalue / 88.4, 2), "0.601", csysvalue, "0.711", age);
                                }
                            }
                        }
                    }


                    if (!string.IsNullOrEmpty(mathall))
                    {
                        pb.Rows.Add("1", "1", "50753", "肌酐与scys");

                        object objValue = ExpressionCompute.CalExpression(mathall);
                        if (objValue != null)
                        {
                            decimal decVal = 0;

                            if (decimal.TryParse(objValue.ToString(), out decVal))
                            {

                                decVal = decimal.Round(decVal, 4);
                                pb.Rows[0]["retu"] = decVal.ToString("0.00");
                            }
                            else
                            {

                                pb.Rows[0]["retu"] = string.Empty;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pb.Rows.Add("1", "1", "50753", "肌酐与scys");
                dcl.root.logon.Logger.WriteException("", "", ex.ToString());
            }







            DataSet dss = new DataSet();
            dss.Tables.Add(pb);
            return dss;
        }

        /// <summary>
        /// 获取普通结果：仪器中间表数据
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="resulto"></param>
        public void CopyResultoMidToResulto(string pat_id)
        {
            string sqlSelectResultoMid = string.Format(@"
select
    patients.pat_id,
    dict_mitm_no.mit_itm_id,
    dict_item.itm_name,
    resulto_mid.*,
    dict_item.itm_ecd

from resulto_mid
    inner join dict_mitm_no on dict_mitm_no.mit_itr_id = resulto_mid.res_itr_id and dict_mitm_no.mit_cno = resulto_mid.res_cno
    inner join dict_item on dict_item.itm_id = dict_mitm_no.mit_itm_id
    inner join patients on patients.pat_id = resulto_mid.res_itr_id + convert(varchar(8),resulto_mid.res_date,112) + resulto_mid.res_sid
where 
    patients.pat_id = '{0}'
    and (res_read_flag = 0 or res_read_flag is null)
    and (res_data_type = 0 or res_data_type = 1)
    and (patients.pat_flag = 0 or patients.pat_flag is null)
order by res_date asc
", pat_id);
            try
            {
                using (DBHelper transHelper = DBHelper.BeginTransaction())
                {
                    DataTable dtResultoMid = transHelper.GetTable(sqlSelectResultoMid);

                    if (dtResultoMid.Rows.Count > 0)
                    {
                        string sqlSelectResulto = string.Format(@"
select
    resulto.res_key,
    resulto.res_id,
    resulto.res_itr_id,
    resulto.res_sid,
    resulto.res_itm_id,
    resulto.res_itm_ecd,
    resulto.res_date,
    resulto.res_flag,
    resulto.res_type,
    resulto.res_rep_type,
    resulto.res_chr,
    resulto.res_od_chr,
    resulto.res_chr2,
    resulto.res_chr3,
    res_origin_record = 1
from resulto
where res_id = '{0}' ", pat_id);

                        DataTable dtResulto = transHelper.GetTable(sqlSelectResulto);

                        StringBuilder sb = new StringBuilder();
                        bool needComma = false;
                        foreach (DataRow row in dtResultoMid.Rows)
                        {
                            if (needComma)
                            {
                                sb.Append(",");
                            }

                            sb.Append(row["res_key"].ToString());

                            needComma = true;
                        }

                        string sqlUpdate = string.Format("update resulto_mid set res_read_flag = 1 where res_key in ({0})", sb.ToString());

                        transHelper.ExecuteNonQuery(sqlUpdate);


                        DataTable dtResultoInsert = dtResulto.Clone();//要插入到结果表的数据
                        DataTable dtResultoBak = dtResulto.Clone();//要备份的数据

                        foreach (DataRow drMidResulto in dtResultoMid.Rows)//遍历中间表所有行
                        {
                            int res_data_type = Convert.ToInt32(drMidResulto["res_data_type"]);

                            DataRow drResulto = null;

                            string res_itm_id = drMidResulto["mit_itm_id"].ToString();

                            //res_origin_record：从resulto表中查出来的都为1，为1的时候如果仪器中间表有相同的项目ID，则此条数据写进备份表中，并置为0
                            //，防止仪器中间表中有多个相同的项目id引起多次结果表备份


                            //检查原有结果表中项目是否在中间表中存在
                            DataRow[] drs = dtResulto.Select(string.Format("res_itm_id = '{0}' ", res_itm_id));
                            if (drs.Length > 0)
                            {
                                drResulto = drs[0];

                                if (Convert.ToInt32(drResulto["res_origin_record"]) == 1)
                                {
                                    transHelper.ExecuteNonQuery(string.Format("delete from resulto where res_key = {0} ", drResulto["res_key"]));

                                    dtResultoBak.Rows.Add(drResulto.ItemArray);
                                }

                                dtResulto.Rows.Remove(drResulto);
                            }
                            //else
                            //{
                            drResulto = dtResulto.NewRow();
                            //}

                            drResulto["res_id"] = drMidResulto["pat_id"];
                            drResulto["res_itr_id"] = drMidResulto["res_itr_id"];
                            drResulto["res_sid"] = drMidResulto["res_sid"];
                            drResulto["res_itm_id"] = res_itm_id;
                            drResulto["res_itm_ecd"] = drMidResulto["itm_ecd"];
                            drResulto["res_date"] = drMidResulto["res_date"];
                            drResulto["res_flag"] = 1;
                            drResulto["res_type"] = 1;//结果类型：仪器传输
                            drResulto["res_rep_type"] = drMidResulto["res_data_type"];
                            drResulto["res_origin_record"] = 0;

                            switch (Convert.ToInt32(drMidResulto["res_data_type"]))
                            {
                                case 0:
                                    drResulto["res_chr"] = drMidResulto["res_chr_a"];
                                    break;

                                case 1:
                                    drResulto["res_chr"] = drMidResulto["res_chr_a"];
                                    drResulto["res_od_chr"] = drMidResulto["res_chr_b"];
                                    break;
                            }

                            DataRow[] drsInsert = dtResultoInsert.Select(string.Format("res_itm_id = '{0}' ", res_itm_id));
                            if (drsInsert.Length > 0)
                            {
                                foreach (DataRow dr in drsInsert)
                                {
                                    dtResultoInsert.Rows.Remove(dr);
                                }
                            }

                            dtResultoInsert.Rows.Add(drResulto.ItemArray);

                            dtResulto.Rows.Add(drResulto.ItemArray);
                        }

                        List<SqlCommand> listCmdResultoInsert = DBTableHelper.GenerateInsertCommand("resulto", new string[] { "res_key" }, dtResultoInsert);

                        foreach (SqlCommand cmd in listCmdResultoInsert)
                        {
                            transHelper.ExecuteNonQuery(cmd);
                        }

                        List<SqlCommand> listCmdResultoBakInsert = DBTableHelper.GenerateInsertCommand("resulto_bak", new string[] { "res_key" }, dtResultoBak);

                        foreach (SqlCommand cmd in listCmdResultoBakInsert)
                        {
                            transHelper.ExecuteNonQuery(cmd);
                        }
                    }

                    transHelper.Commit();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "CopyResultoMidToResulto", ex.ToString());
                //throw;
            }
        }

        /// <summary>
        /// 获取描述报告结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public DataTable GetPatDescResult(string patID)
        {
            string sqlSelect = string.Format(@"select top 1 * from {0} where bsr_res_flag=1 and bsr_id='{1}'", PatientTable.PatientDescResultTableName, patID);
            DBHelper helper = new DBHelper();
            DataTable dt = helper.GetTable(sqlSelect);
            dt.TableName = PatientTable.PatientDescResultTableName;
            return dt;
        }

        #region 获取病人普通结果历史结果

        /// <summary>
        /// 获取病人历史结果(包括参考值,包括病人资料表)
        /// </summary>
        /// <param name="patID">病人ID</param>
        /// <param name="resultCount"></param>
        /// <param name="patDate"></param>
        /// <returns></returns>
        public DataSet GetPatCommonResultHistoryWithRef(string patID, int resultCount, DateTime? patDate)
        {

            if (patDate != null && patDate.Value.Year == 2100)
            {
                return GetPatCommonResultHistoryWithRefForThread(patID, 10, patDate);
            }
            DataSet dsResult = new DataSet();
            DataTable dtInfo = LisHistoryResultController.GetPatientInfoForHistory(patID, patDate);
            if (dtInfo.Rows.Count > 0)
            {
                string itr_id = string.Empty;
                string pat_sam_id = string.Empty;
                string pat_no_id = string.Empty;
                string pat_in_no = string.Empty;
                string pat_name = string.Empty;
                int ageMinute = -1;
                string sex = "0";
                DateTime? pat_Date = null;

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_date"]))
                {
                    pat_Date = DateTime.Parse(dtInfo.Rows[0]["pat_date"].ToString());

                }


                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_itr_id"]))
                {
                    itr_id = dtInfo.Rows[0]["pat_itr_id"].ToString();
                }
                string pat_depcode = string.Empty;
                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_dep_id"]))
                {
                    pat_depcode = dtInfo.Rows[0]["pat_dep_id"].ToString();
                }
                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_sam_id"]))
                {
                    pat_sam_id = dtInfo.Rows[0]["pat_sam_id"].ToString();
                }

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_no_id"]))
                {
                    pat_no_id = dtInfo.Rows[0]["pat_no_id"].ToString();
                }

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_in_no"]))
                {
                    pat_in_no = dtInfo.Rows[0]["pat_in_no"].ToString();
                }

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_name"]))
                {
                    pat_name = dtInfo.Rows[0]["pat_name"].ToString();
                }

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_age"]))
                {
                    ageMinute = Convert.ToInt32(dtInfo.Rows[0]["pat_age"]);
                    if (ageMinute == 0)
                    {
                        ageMinute = -1;
                    }
                }

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_sex"]) && dtInfo.Rows[0]["pat_sex"].ToString().Trim() != string.Empty)
                {
                    sex = dtInfo.Rows[0]["pat_sex"].ToString();
                }

                #region //获取历史结果自定义查询列 edit by sink 2010-6-18
                string historySelectColumnValue = "";

                string historySelectColumn = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");// dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");
                if (string.IsNullOrEmpty(historySelectColumn))
                    historySelectColumn = "pat_in_no";
                else
                {
                    historySelectColumn = historySelectColumn.Replace("patients.", "");
                    if (!Compare.IsNullOrDBNull(dtInfo.Rows[0][historySelectColumn]))
                    {
                        historySelectColumnValue = dtInfo.Rows[0][historySelectColumn].ToString();
                    }
                }

                #endregion

                DataTable dtHistory = GetPatCommonResultHistory(patID, itr_id, pat_sam_id, pat_no_id, pat_in_no, pat_name, sex, pat_Date, resultCount, historySelectColumn, historySelectColumnValue);

                List<string> itemsID = new List<string>();
                foreach (DataRow drHistory in dtHistory.Rows)
                {
                    itemsID.Add(drHistory["res_itm_id"].ToString());
                }

                DataTable dtItemsRef = DictItemBLL.NewInstance.GetItemsRef(itemsID, pat_sam_id, ageMinute, sex, pat_depcode);

                //dtHistory.Rows.Add("itm_ref_l");
                //dtHistory.Rows.Add("itm_ref_h");
                //dtHistory.Rows.Add("itm_max");
                //dtHistory.Rows.Add("itm_min");
                //dtHistory.Rows.Add("itm_pan_h");
                //dtHistory.Rows.Add("itm_pan_l");

                //if (dtHistory.Rows.Count > 0)
                //{
                //    List<string> itemsID = new List<string>();
                //    foreach (DataRow drHistory in dtHistory.Rows)
                //    {
                //        itemsID.Add(drHistory["res_itm_id"].ToString());
                //    }
                //    DataTable dtItemsRef = DictItemBLL.NewInstance.GetItemsRef(itemsID, pat_sam_id, ageMinute, sex);

                //    foreach (DataRow drHistory in dtHistory.Rows)
                //    {
                //        string itm_id = drHistory["res_itm_id"].ToString();
                //        DataRow[] drsItemRef = dtItemsRef.Select(string.Format("itm_id='{0}'", itm_id));
                //        if (drsItemRef.Length > 0)
                //        {
                //            drHistory["itm_ref_l"] = drsItemRef[0]["itm_ref_l"];
                //            drHistory["itm_ref_h"] = drsItemRef[0]["itm_ref_h"];
                //            drHistory["itm_max"] = drsItemRef[0]["itm_max"];
                //            drHistory["itm_min"] = drsItemRef[0]["itm_min"];
                //            drHistory["itm_pan_h"] = drsItemRef[0]["itm_pan_h"];
                //            drHistory["itm_pan_l"] = drsItemRef[0]["itm_pan_l"];
                //        }
                //    }
                //}

                //DataTable dtPatInfo = dtInfo.Copy();
                //dtPatInfo.TableName = PatientTable.PatientInfoTableName;

                dsResult.Tables.Add(dtInfo);
                dsResult.Tables.Add(dtHistory);
                dsResult.Tables.Add(dtItemsRef);
            }
            return dsResult;
        }


        public DataSet GetPatCommonResultHistoryWithRefForThread(string patID, int resultCount, DateTime? patDate)
        {
            DataSet dsResult = new DataSet();

            string sqlSelect =
                string.Format(@" SELECT top 1 pat_name,pat_date, pat_itr_id,  pat_dep_id, pat_sam_id,
                                       pat_no_id,pat_in_no,pat_age, pat_sex
                                       FROM patients with(nolock)
                                       WHERE patients.pat_id ='{0}'  ", patID);
            SqlHelper helper = new SqlHelper();
            DataTable dtInfo = helper.GetTable(sqlSelect);


            dtInfo.TableName = PatientTable.PatientInfoTableName;

            if (dtInfo.Rows.Count > 0)
            {
                string itr_id = string.Empty;
                string pat_sam_id = string.Empty;
                string pat_no_id = string.Empty;
                string pat_in_no = string.Empty;
                string pat_name = string.Empty;
                int ageMinute = -1;
                string sex = "0";
                DateTime? pat_Date = null;

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_date"]))
                {
                    pat_Date = DateTime.Parse(dtInfo.Rows[0]["pat_date"].ToString());

                }


                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_itr_id"]))
                {
                    itr_id = dtInfo.Rows[0]["pat_itr_id"].ToString();
                }
                string pat_depcode = string.Empty;
                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_dep_id"]))
                {
                    pat_depcode = dtInfo.Rows[0]["pat_dep_id"].ToString();
                }
                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_sam_id"]))
                {
                    pat_sam_id = dtInfo.Rows[0]["pat_sam_id"].ToString();
                }

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_no_id"]))
                {
                    pat_no_id = dtInfo.Rows[0]["pat_no_id"].ToString();
                }

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_in_no"]))
                {
                    pat_in_no = dtInfo.Rows[0]["pat_in_no"].ToString();
                }

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_name"]))
                {
                    pat_name = dtInfo.Rows[0]["pat_name"].ToString();
                }

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_age"]))
                {
                    ageMinute = Convert.ToInt32(dtInfo.Rows[0]["pat_age"]);
                    if (ageMinute == 0)
                    {
                        ageMinute = -1;
                    }
                }

                if (!Compare.IsNullOrDBNull(dtInfo.Rows[0]["pat_sex"]) && dtInfo.Rows[0]["pat_sex"].ToString().Trim() != string.Empty)
                {
                    sex = dtInfo.Rows[0]["pat_sex"].ToString();
                }

                #region //获取历史结果自定义查询列 edit by sink 2010-6-18
                string historySelectColumnValue = "";

                string historySelectColumn = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");// dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportSelectColumn");
                if (string.IsNullOrEmpty(historySelectColumn))
                    historySelectColumn = "pat_in_no";
                else
                {
                    historySelectColumn = historySelectColumn.Replace("patients.", "");
                    if (!Compare.IsNullOrDBNull(dtInfo.Rows[0][historySelectColumn]))
                    {
                        historySelectColumnValue = dtInfo.Rows[0][historySelectColumn].ToString();
                    }
                }

                #endregion

                DataTable dtHistory = GetPatCommonResultHistory(patID, itr_id, pat_sam_id, pat_no_id, pat_in_no, pat_name, sex, pat_Date, resultCount, historySelectColumn, historySelectColumnValue);


                dtInfo.Clear();
                dsResult.Tables.Add(dtHistory);
            }
            return dsResult;
        }

        //        /// <summary>
        //        /// 获取病人历史结果
        //        /// </summary>
        //        /// <param name="pat_id"></param>
        //        /// <param name="itr_id"></param>
        //        /// <param name="pat_sam_id"></param>
        //        /// <param name="pat_no_id"></param>
        //        /// <param name="pat_in_no"></param>
        //        /// <param name="pat_name"></param>
        //        /// <param name="resultCount"></param>
        //        /// <returns></returns>
        //        public DataTable GetPatCommonResultHistory(string pat_id, string itr_id, string pat_sam_id, string pat_no_id, string pat_in_no, string pat_name, string pat_sex, int resultCount)
        //        {
        //            try
        //            {
        //                if (resultCount < 0)
        //                {
        //                    resultCount = 0;
        //                }

        //                DBHelper helper = new DBHelper();
        //                DateTime dtPat_date;
        //                string sql = string.Format("select top 1 pat_date from patients where pat_id = '{0}'", pat_id);
        //                object objPatDate = helper.ExecuteScalar(sql);
        //                if (objPatDate != null && objPatDate != DBNull.Value)
        //                {
        //                    dtPat_date = (DateTime)objPatDate;
        //                }
        //                else
        //                {
        //                    dtPat_date = DateTime.Now;
        //                }

        //                string itr_id_where = string.Empty;
        //                string strPatientInfo_where = string.Empty;//病人过滤条件

        //                if (itr_id != null && itr_id != string.Empty)
        //                    itr_id_where = string.Format("pat_itr_id = '{0}' and", itr_id);

        //                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_ResultHistoryContrainName") == "是")
        //                {
        //                    if (string.IsNullOrEmpty(pat_name))
        //                    {
        //                        pat_in_no = "pat_nameIsNull";
        //                    }
        //                    strPatientInfo_where = string.Format(" and pat_name= '{0}'", new string[] { pat_sex, pat_name });
        //                }
        //                else
        //                {
        //                    if (string.IsNullOrEmpty(pat_in_no))
        //                    {
        //                        pat_in_no = "pat_in_noIsNull";
        //                    }
        //                    //strPatientInfo_where = string.Format("and pat_name= '{0}' and pat_no_id = '{1}'  and pat_in_no ='{2}'", new string[] { pat_name, pat_no_id, pat_in_no });
        //                    strPatientInfo_where = string.Format("and pat_name= '{0}'  and pat_in_no ='{1}'", new string[] { pat_name, pat_in_no });
        //                }


        //                sql = string.Format(@"
        //SELECT
        //res.res_sid,
        //res.res_chr,
        //res.res_itm_id,
        //res.res_itm_ecd, 
        //case when pat_report_date is null then patients.pat_date else pat_report_date end as res_date
        //FROM patients INNER JOIN
        //resulto AS res ON patients.pat_id = res.res_id inner join dict_item on res.res_itm_id = dict_item.itm_id where 
        //res_id in
        //	(
        //	SELECT top {0} pat_id from patients
        //	where {1}
        //    pat_sam_id='{2}'

        //    and pat_id <> '{3}'
        //    {4}
        //    and pat_date <= @pat_date
        //    --and pat_flag in ('2','4')
        //    order by pat_date desc
        //	)
        //and res_flag = 1
        //order by patients.pat_date desc , dict_item.itm_seq",
        //resultCount,
        //itr_id_where,
        //pat_sam_id,
        //resultCount == 3 ? pat_id : "0",
        //strPatientInfo_where
        //);
        //                SqlCommand cmd = new SqlCommand();
        //                cmd.CommandText = sql;
        //                cmd.Parameters.AddWithValue("pat_date", dtPat_date);
        //                DataTable dt = helper.GetTable(cmd);

        //                dt.TableName = "PatientHistory";
        //                return dt;
        //            }
        //            catch (Exception ex)
        //            {
        //                dcl.root.logon.Logger.WriteException(this.GetType().Name, "获取病人历史记录出错GetPatHistoryResult", ex.ToString());
        //                throw;
        //            }

        //        }


        #region 支持自定义查询列的历史结果

        /// <summary>
        /// 获取病人历史结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="itr_id"></param>
        /// <param name="pat_sam_id"></param>
        /// <param name="pat_no_id"></param>
        /// <param name="pat_in_no"></param>
        /// <param name="pat_name"></param>
        /// <param name="resultCount"></param>
        /// <returns></returns>
        public DataTable GetPatCommonResultHistory(string pat_id, string itr_id, string pat_sam_id, string pat_no_id,
                                                   string pat_in_no, string pat_name, string pat_sex, DateTime? patDate,
                                                   int resultCount, string historySelectColumn,
                                                   string historySelectColumnValue)
        {
            //不限仪器查询历史结果
            //itr_id = "";
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_HistoryWithItr") == "否")
            {
                itr_id = "";
            }

            try
            {
                if (resultCount < 0)
                {
                    resultCount = 0;
                }

                DBHelper helper = new DBHelper();
                DateTime dtPat_date;
                string sql;
                //string sql = string.Format("select top 1 pat_date from patients where pat_id = '{0}'", pat_id);
                //object objPatDate = helper.ExecuteScalar(sql);
                if (patDate.HasValue)
                {
                    dtPat_date = patDate.Value;
                }
                else
                {
                    dtPat_date = DateTime.Now;
                }

                string sqlItrID_IN = string.Empty; //相同专业组的仪器ID
                //历史结果关联项目专业组
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_HistoryWithPType") == "是")
                {
                    string sqlSelect = string.Format(@"select bins.itr_id from patients as pat,
dict_instrmt as ains,dict_instrmt as bins
where bins.itr_ptype=ains.itr_ptype
and ains.itr_id=pat.pat_itr_id
and pat.pat_id='{0}'", pat_id);

                    DataTable dtItrID_List = helper.GetTable(sqlSelect);

                    StringBuilder sb = new StringBuilder();
                    foreach (DataRow dr in dtItrID_List.Rows)
                    {
                        sb.Append(string.Format(",'{0}'", dr["itr_id"]));
                    }

                    string temp_itrID = sb.ToString(); //临时仪器ID字符串
                    if (!string.IsNullOrEmpty(temp_itrID))
                    {
                        temp_itrID = temp_itrID.Substring((temp_itrID.IndexOf(",") + 1));
                        sqlItrID_IN = string.Format("and pat_itr_id in({0}) ", temp_itrID);
                    }
                    else
                    {
                        sqlItrID_IN = string.Empty;
                    }
                }

                string strIn_Is_Audit = ""; //只获取已审核的结果
                //历史结果只显示审核后的结果
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_HistoryNeedAudit") == "是")
                {
                    strIn_Is_Audit = " and pat_flag in (2,4) ";
                }

                string customerColumnSQL = string.Empty;
                //如果开启只按姓名与性别查询病人结果，按选择列查询功能无效。
                if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_ResultHistoryContrainName") == "是")
                {
                    //customerColumnSQL = string.Format(@" and pat_sex='{0}'", pat_sex);
                }
                else
                {

                    if (string.IsNullOrEmpty(pat_in_no))
                    {
                        pat_in_no = "pat_in_noisnnull";
                    }
                    //                    customerColumnSQL = historySelectColumn.Contains("pat_in_no") ? string.Format(@"     and pat_no_id = '{0}'
                    //    and pat_in_no ='{1}' ", pat_no_id, pat_in_no) : string.Format(" and {0} = '{1}' ", historySelectColumn, historySelectColumnValue);
                    customerColumnSQL = historySelectColumn.Contains("pat_in_no")
                                            ? string.Format(@"     
                        and pat_in_no ='{0}' ", pat_in_no)
                                            : string.Format(" and {0} = '{1}' ", historySelectColumn,
                                                            historySelectColumnValue);
                }
                string itr_id_where = string.Empty;
                string or_pat_id = string.Format("or  pat_id='{0}'", pat_id); //包括本身结果

                if (itr_id != null && itr_id != string.Empty)
                    itr_id_where = string.Format("pat_itr_id = '{0}' and", itr_id);

                sql = string.Format(@"SELECT res.res_id,res.res_sid,res.res_chr,res.res_itm_id,res.res_itm_ecd, case when pat_report_date is null then patients.pat_date else pat_report_date end as res_date
                                             FROM patients INNER JOIN
                                            resulto AS res ON patients.pat_id = res.res_id inner join dict_item on res.res_itm_id = dict_item.itm_id where 
                                             res_id in
	                                      (SELECT top {0} pat_id from patients
	                                        where {1} pat_sam_id='{2}'
                                         and pat_name= '{3}'  {4}  {5}
                                            and pat_id <> '{6}'
                                            and pat_date <= @pat_date
                                            {7}  {8}
                                            order by pat_date desc)
                                        and res_flag = 1 order by patients.pat_date desc , dict_item.itm_seq",
                                    resultCount,
                                    itr_id_where,
                                    pat_sam_id,
                                    pat_name,
                                    customerColumnSQL,
                                    sqlItrID_IN,
                                    resultCount == 3 || resultCount == 10 ? pat_id : "0",
                                    strIn_Is_Audit,
                                    resultCount == 3 || resultCount == 10 ? "" : or_pat_id
                    );

                //获取历史数据库数据 HB 2013-09-02
                bool enableRead =
               dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_EnableReadHistoryFromOldDB") == "是";
                string lisHistoryConnectionString = ConfigurationManager.AppSettings["LisHistoryConnectionString"];
                ReadHistoryDelegate readerDg = null;
                IAsyncResult iResult = null;
                if (!string.IsNullOrEmpty(lisHistoryConnectionString) && enableRead)
                {
                    readerDg = GetPatHistoryResultFromHistoryDataBase;
                    iResult = readerDg.BeginInvoke(sql, dtPat_date, lisHistoryConnectionString, null, null);
                }


                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("pat_date", dtPat_date);
                DataTable dt = helper.GetTable(cmd);
                dt.TableName = "PatientHistory";



                if (readerDg != null)
                {
                    DataTable lisHistoryResData = readerDg.EndInvoke(iResult);
                    if (lisHistoryResData != null && lisHistoryResData.Rows.Count > 0)
                    {
                        dt.Merge(lisHistoryResData);
                    }
                }

                return dt;
            }
            catch (Exception ex)
            {
                Logger.WriteException(GetType().Name, "获取病人历史记录出错GetPatHistoryResult", ex.ToString());
                throw;
            }

        }
        public delegate DataTable ReadHistoryDelegate(string sql, DateTime dtPatDate, string lisHistoryConnectionString);
        /// <summary>
        /// 读取历史数据库检验结果数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dtPatDate"></param>
        /// <param name="lisHistoryConnectionString"></param>
        private DataTable GetPatHistoryResultFromHistoryDataBase(string sql, DateTime dtPatDate, string lisHistoryConnectionString)
        {
            try
            {
                DBHelper helper = new DBHelper(lisHistoryConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("pat_date", dtPatDate);
                DataTable dt = helper.GetTable(cmd);
                dt.TableName = "PatientHistory";
                return dt;
            }
            catch (Exception ex)
            {
                Logger.WriteException(GetType().Name, "读取历史数据库数据出错GetPatHistoryResultFromHistoryDataBase", ex.ToString());
            }
            return null;
        }




        /// <summary>
        /// 获取病人历史结果
        /// </summary>
        /// <returns></returns>
        public DataTable GetPatCommonResultHistoryForBf(string pat_id, string itr_id, string pat_upid, DateTime? patDate)
        {

            try
            {

                DBHelper helper = new DBHelper();
                DateTime dtPat_date;
                string sql;
                if (patDate.HasValue)
                {
                    dtPat_date = patDate.Value;
                }
                else
                {
                    dtPat_date = DateTime.Now;
                }



                string itr_id_where = string.Empty;


                if (itr_id != null && itr_id != string.Empty)
                    itr_id_where = string.Format("pat_itr_id = '{0}' ", itr_id);

                sql = string.Format(@"SELECT res.res_id,res.res_sid,res.res_chr,res.res_itm_id,res.res_itm_ecd, patients_newborn.pat_date as res_date
                                             FROM patients_newborn INNER JOIN
                                            resulto_newborn AS res ON patients_newborn.pat_id = res.res_id inner join dict_item on res.res_itm_id = dict_item.itm_id where 
                                             res_id in
	                                      (SELECT top {0} pat_id from patients_newborn
	                                        where {1} 
                                            and pat_id <> '{2}'
                                            and pat_upid='{3}'
                                            and pat_date <= @pat_date
                                            order by pat_date desc)
                                        and res_flag = 1 order by patients_newborn.pat_date desc , dict_item.itm_seq",
                                    2,
                                    itr_id_where,
                                    pat_id, pat_upid
                    );



                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("pat_date", dtPat_date);
                DataTable dt = helper.GetTable(cmd);
                dt.TableName = "PatientHistory";




                return dt;
            }
            catch (Exception ex)
            {
                Logger.WriteException(GetType().Name, "获取病人历史记录出错GetPatHistoryResult", ex.ToString());
                throw;
            }

        }


        #endregion


        #endregion

        /// <summary>
        /// 获取病人图像结果
        /// </summary>
        /// <param name="pat_id">病人ID</param>
        /// <returns></returns>
        public DataTable GetPatResultImage(string pat_id)
        {
            try
            {
                string sql = string.Format("select * from resulto_p where pres_id='{0}' and pres_flag = 1 order by pres_key asc", pat_id);

                DBHelper helper = new DBHelper();
                DataTable dtPatImage = helper.GetTable(sql);
                dtPatImage.TableName = lis.dto.PatientTable.PatientImageResultTableName;
                return dtPatImage;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人图像结果出错：病人ID=" + pat_id, ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 获取病人列表(详细信息)
        /// 查找指定组别的所有病人资料时,把itr_id赋空字串
        /// </summary>
        /// <param name="dtFrom">起始日期</param>
        /// <param name="dtTo">结束日期</param>
        /// <param name="type_id">物理组ID</param>
        /// <param name="itr_id">仪器ID</param>
        /// <returns></returns>
        public DataTable GetPatientsList_Details(DateTime dtFrom, DateTime dtTo, string type_id, string itr_id)
        {
            string sqlItrID_IN = string.Empty;

            string sqlSelect;

            DBHelper helper = new DBHelper();
            //没有输入仪器，则查找物理组别中的所有仪器
            if (itr_id == string.Empty || itr_id == null)
            {
                sqlItrID_IN = "'-1'";

                sqlSelect = string.Format("select itr_id from dict_instrmt where itr_type='{0}'", type_id);
                DataTable dtItrID_List = helper.GetTable(sqlSelect);

                StringBuilder sb = new StringBuilder();
                foreach (DataRow dr in dtItrID_List.Rows)
                {
                    sb.Append(string.Format(",'{0}'", dr["itr_id"]));
                }

                sqlItrID_IN = sqlItrID_IN + sb.ToString();
            }
            else
            {
                sqlItrID_IN = string.Format("'{0}'", itr_id);
            }

            sqlSelect = string.Format(@"
select 
distinct pat_sid,
cast(0 as bit) as pat_select, 
cast(pat_sid as bigint) as pat_sid_int,
patients.*,
--pat_sex,
pat_sex_name = case when pat_sex = '1' then '男'
                    when pat_sex = '2' then '女'
                    else '' end,
pat_age_display='',
--pat_date,
--pat_name,
--pat_age_exp,
--pat_bed_no,
--pat_id,
--pat_itr_id,
--pat_i_code,
--pat_sam_id,
dict_sample.sam_name as pat_sam_name,
dict_instrmt.itr_mid,

--pat_flag,
pat_flag_name = '',

--pat_ctype,
--pat_modified_times,
hasresult = case when resulto.res_id is not null then 1
                 else 0 end
--,resulto.res_id as hasresult
from patients
Left join dict_sample on patients.pat_sam_id = dict_sample.sam_id
LEFT OUTER JOIN dict_instrmt ON patients.pat_itr_id = dict_instrmt.itr_id
left join resulto on patients.pat_id = resulto.res_id


where 
pat_date >= @pat_date_from 
and pat_date < @pat_date_to 
and pat_itr_id in ({0})
order by dict_instrmt.itr_mid asc, cast(pat_sid as bigint) asc
", sqlItrID_IN);


            SqlCommand cmdSelect = new SqlCommand(sqlSelect);
            cmdSelect.Parameters.AddWithValue("pat_date_from", dtFrom.Date);
            cmdSelect.Parameters.AddWithValue("pat_date_to", dtTo.Date.AddDays(1));

            DataTable dt = helper.GetTable(cmdSelect);
            dt.TableName = "GetPatientsList_Details";

            foreach (DataRow drPat in dt.Rows)
            {
                if (drPat["pat_age_exp"] != null && drPat["pat_age_exp"] != DBNull.Value)
                {
                    string patage = drPat["pat_age_exp"].ToString();

                    patage = AgeConverter.TrimZeroValue(patage);
                    patage = AgeConverter.ValueToText(patage);
                    drPat["pat_age_display"] = patage;
                }

                if (drPat["pat_flag"] != null && drPat["pat_flag"] != DBNull.Value)
                {
                    string patflag = drPat["pat_flag"].ToString();
                    if (patflag == LIS_Const.PATIENT_FLAG.Audited)
                    {
                        drPat["pat_flag_name"] = "已" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Printed)
                    {
                        drPat["pat_flag_name"] = "已打印";
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Reported)
                    {
                        drPat["pat_flag_name"] = "已" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("ReportWord");
                    }
                    else if (patflag == LIS_Const.PATIENT_FLAG.Natural || patflag == string.Empty)
                    {
                        drPat["pat_flag_name"] = "未" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                    }
                }
                else
                {
                    drPat["pat_flag_name"] = "未" + dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("AuditWord");
                }


                if (drPat["hasresult"].ToString() == "1" && drPat["pat_flag"].ToString() == "0")
                {
                    string sql = string.Format(@"select * from dict_combine_mi
                                left join patients_mi on patients_mi.pat_com_id=dict_combine_mi.com_id
                                where com_popedom ='1' and pat_id='{0}'
                                and  com_itm_id not in (select res_itm_id from resulto where res_id='{0}' and res_flag='1')", drPat["pat_id"]);
                    if (helper.GetTable(sql).Rows.Count > 0)
                        drPat["hasresult"] = "2";
                }

            }
            return dt;
        }

        /// <summary>
        /// 获取细菌报告结果类型 无菌=0 or 细菌 = 1
        /// </summary>
        /// <param name="pat_id"></param>
        public int GetBacResultType(string pat_id)
        {
            string sqlSelect = string.Format("select top 1 bsr_id from cs_rlts where bsr_id = '{0}'", pat_id);
            DBHelper helper = new DBHelper();
            object obj = helper.ExecuteScalar(sqlSelect);

            if (Compare.IsNullOrDBNull(obj))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 从院网接口/条码接口获取病人信息，医嘱信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="interfaceID"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public InterfacePatientInfo GetPatientFromInterface(string id, string interfaceID, NetInterfaceType interfaceType)
        {
            InterfacePatientInfo info = InterfaceProxy.GetInterfacePatient(id, interfaceID, interfaceType);
            return info;
        }

        public System.Data.DataTable GetPatHistoryExp(string pat_in_no)
        {
            string sqlSelect = string.Format("select pat_id,pat_date,pat_exp,pat_c_name from patients where pat_exp <>'' and pat_in_no='{0}' order by pat_date desc", pat_in_no);
            DBHelper helper = new DBHelper();
            DataTable dtHistoryExp = helper.GetTable(sqlSelect);
            dtHistoryExp.TableName = "patients";

            return dtHistoryExp;
        }

        /// <summary>
        /// 获取病人扩展表patients_ext的信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        public System.Data.DataTable GetPatientExtData(string pat_id)
        {
            if (string.IsNullOrEmpty(pat_id))
                pat_id = "";

            string sqlSelect = string.Format("select * from patients_ext where pat_id='{0}'", pat_id);
            DBHelper helper = new DBHelper();
            DataTable dtPatientExt = helper.GetTable(sqlSelect);
            dtPatientExt.TableName = "patients_ext";

            return dtPatientExt;
        }

        /// <summary>
        /// 获取组合相关的无菌与涂片
        /// </summary>
        /// <param name="strComIDs"></param>
        /// <returns></returns>
        public System.Data.DataTable GetDictNobactByComID(string strComIDs)
        {
            DataTable dtDictNobact = new DataTable("dict_nobact");

            if (string.IsNullOrEmpty(strComIDs))
                return dtDictNobact;

            try
            {
                string sqlInComID = "";

                foreach (string strtemp in strComIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (string.IsNullOrEmpty(sqlInComID))
                    {
                        sqlInComID = "'" + strtemp + "'";
                    }
                    else
                    {
                        sqlInComID += ",'" + strtemp + "'";
                    }
                }

                string sqlSelect = string.Format(@"select nob_id from 
dict_nobact with(nolock)
where nob_pulbic=1 
or nob_id in(select dict_nobact_com.nob_id from dict_nobact_com with(nolock) where com_id in({0}))", sqlInComID);
                DBHelper helper = new DBHelper();
                dtDictNobact = helper.GetTable(sqlSelect);
                dtDictNobact.TableName = "dict_nobact";
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }

            return dtDictNobact;
        }

        /// <summary>
        /// 获取条码中间表bc_barcode_mid信息
        /// </summary>
        /// <param name="sqlWhere"></param>
        /// <returns></returns>
        public System.Data.DataTable GetBCBarcodeMidData(string sqlWhere)
        {
            if (string.IsNullOrEmpty(sqlWhere))
                sqlWhere = "and 1=2 ";

            string sqlSelect = string.Format("select * from bc_barcode_mid where 1=1 {0}", sqlWhere);
            DBHelper helper = new DBHelper();
            DataTable dtPatientExt = helper.GetTable(sqlSelect);
            dtPatientExt.TableName = "bc_barcode_mid";

            return dtPatientExt;
        }

        /// <summary>
        /// 获取条码信息中的注意事项(bc_exp)
        /// </summary>
        /// <param name="BcSQLWhere"></param>
        /// <returns></returns>
        public System.String GetBarcodeExpNotice(string BcSQLWhere)
        {
            string strRv = "";

            if (string.IsNullOrEmpty(BcSQLWhere))
                return strRv;

            string sqlSelect = string.Format("select TOP 1 bc_exp from bc_patients where 1=1 {0} ", BcSQLWhere);
            DBHelper helper = new DBHelper();
            DataTable dtPatExpNotice = helper.GetTable(sqlSelect);

            if (dtPatExpNotice != null && dtPatExpNotice.Rows.Count > 0 && dtPatExpNotice.Columns.Count > 0)
            {
                strRv = dtPatExpNotice.Rows[0][0].ToString();
            }

            return strRv;
        }
        /// <summary>
        /// 获取病人组合明细
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetPatientCombineForFee(string patID)
        {
            try
            {
                SqlHelper helper = new SqlHelper();
                string sql = @"select   0 as isselected,
                            dict_combine.*,
                            com_fees=isnull((select top 1 1 from dict_combine_fees),0),
                            dict_type_1.type_name AS ctype_name, 
                            dict_type.type_name AS ptype_name
                            from dict_combine
                            LEFT OUTER JOIN dict_type ON dict_combine.com_ptype = dict_type.type_id  
                            LEFT OUTER JOIN dict_type AS dict_type_1 ON dict_combine.com_ctype = dict_type_1.type_id";
                DataTable dt = helper.GetTable(sql, "dict_combine");
                return dt;

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人组合信息出错,patID=" + patID, ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 获取病人组合明细
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetPatientCombineForSY(string patID)
        {
            try
            {
                SqlHelper helper = new SqlHelper();
                string sql = @"SELECT patients.pat_id,patients.pat_sid,patients.pat_name,
(case patients.pat_sex when '1' then '男' when '2' then '女' else '未知' end) pat_sex,
dbo.getAge(patients.pat_age_exp) pat_age,
pat_in_no,0 AS pat_select,sam_name,pat_bar_code,
pat_c_name,patients.pat_social_no
 FROM patients 
 LEFT OUTER JOIN dict_sample ON patients.pat_sam_id = dict_sample.sam_id
WHERE 1=1 " + patID;
                DataTable dt = helper.GetTable(sql, "patients");
                return dt;

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人组合信息出错,patID=" + patID, ex.ToString());
                throw;
            }
        }

        public DataTable GetCombineFeeInfo(string patIDAndOriID)
        {

            if (patIDAndOriID.Split('|').Length == 2 && patIDAndOriID.Split('|')[1] == "SZ")
            {
                return GetPatientCombineForFee(patIDAndOriID.Split('|')[0]);
            }
            if (patIDAndOriID.Split('|').Length == 2 && patIDAndOriID.Split('|')[1] == "SY")
            {
                return GetPatientCombineForSY(patIDAndOriID.Split('|')[0]);
            }
            DataTable table;

            try
            {
                string childHosConnectStr = ConfigurationManager.AppSettings["HisDBConnectStr"];
                if (string.IsNullOrEmpty(childHosConnectStr))
                {
                    throw new Exception("请在中间层Webconfig 添加HIS数据连接字符串[HisDBConnectStr]");
                }

                string patID = patIDAndOriID.Split('|')[0];
                string oriID = patIDAndOriID.Split('|')[1];
                string depcode = patIDAndOriID.Split('|')[2];
                table = GetPatientCombine(patID);
                DataTable doc = DictDoctorCache.Current.GetDoctors();

                if (table != null && table.Rows.Count > 0)
                {
                    table.Columns.Add("HisPrice", typeof(decimal));
                    table.Columns.Add("IsCharge");
                    table.Columns.Add("op_id_code");

                    string yzSql = string.Empty;
                    foreach (DataRow row in table.Rows)
                    {
                        if (row["pat_yz_id"] != null && !string.IsNullOrEmpty(row["pat_yz_id"].ToString()))
                        {
                            row["IsCharge"] = "未扣费";

                            yzSql += "'" + row["pat_yz_id"] + "',";
                        }
                    }
                    yzSql = yzSql.TrimEnd(',');
                    if (!string.IsNullOrEmpty(yzSql))
                    {
                        DBHelper dbChild = new DBHelper(childHosConnectStr);


                        if (oriID == "108")
                        {
                            DataTable hisFee =
                                dbChild.GetTable(
                                    string.Format("select * from VIEW_LIS_ZY_CHARGE where act_order_no in ({0})", yzSql));


                            foreach (DataRow hisRow in hisFee.Rows)
                            {
                                DataRow[] rows = table.Select(string.Format("pat_yz_id='{0}'", hisRow["act_order_no"]));
                                if (rows.Length > 0)
                                {
                                    rows[0]["IsCharge"] = "扣费成功";
                                    rows[0]["HisPrice"] = hisRow["charge_fee"];

                                    DataRow[] docRows =
                                        doc.Select(string.Format("doc_code='{0}' ", hisRow["op_id_code"]));

                                    if (docRows.Length > 0)
                                    {
                                        rows[0]["op_id_code"] = docRows[0]["doc_name"] != null
                                                                    ? docRows[0]["doc_name"].ToString()
                                                                    : "";
                                    }
                                }
                            }
                        }

                        if (oriID == "107")
                        {
                            DataTable hisFee =
                                dbChild.GetTable(
                                    string.Format("select req_no,op_id,sum(charge_fee) charge_fee from VIEW_LIS_MZ_CHARGE where req_no in ({0}) group by req_no,op_id ", yzSql));


                            foreach (DataRow hisRow in hisFee.Rows)
                            {
                                DataRow[] rows = table.Select(string.Format("pat_yz_id='{0}'", hisRow["req_no"]));
                                if (rows.Length > 0)
                                {
                                    rows[0]["IsCharge"] = "扣费成功";
                                    rows[0]["HisPrice"] = hisRow["charge_fee"];

                                    DataRow[] docRows =
                                        doc.Select(string.Format("doc_code='{0}' ", hisRow["op_id"]));

                                    if (docRows.Length > 0)
                                    {
                                        rows[0]["op_id_code"] = docRows[0]["doc_name"] != null
                                                                    ? docRows[0]["doc_name"].ToString()
                                                                    : "";
                                    }
                                }
                            }
                        }

                    }


                }
                else
                {
                    if (oriID == "107" && !string.IsNullOrEmpty(depcode))
                    {
                        DataTable patdata = GetPatientInfo(patID);
                        string patinno = patdata.Rows[0]["pat_in_no"].ToString();
                        DBHelper dbChild = new DBHelper(childHosConnectStr);
                        DataTable hisFee =
                            dbChild.GetTable(
                                string.Format("select req_no,op_id,sum(charge_fee) charge_fee from VIEW_LIS_MZ_CHARGE where  exec_dept ='{0}' and patient_id='{1}' and confirm_time>getdate()-7 group by req_no,op_id ", depcode, patinno));

                        string yzSql2 = string.Empty;
                        foreach (DataRow hisRow in hisFee.Rows)
                        {
                            if (hisRow["req_no"] != null && !string.IsNullOrEmpty(hisRow["req_no"].ToString()))
                            {
                                yzSql2 += "'" + hisRow["req_no"] + "',";
                            }
                        }
                        yzSql2 = yzSql2.TrimEnd(',');

                        DataTable bctable = GetCombineByYzid(yzSql2);
                        bctable.Columns.Add("HisPrice", typeof(decimal));
                        bctable.Columns.Add("IsCharge");
                        bctable.Columns.Add("op_id_code");
                        foreach (DataRow hisRow in hisFee.Rows)
                        {
                            DataRow[] rows = bctable.Select(string.Format("pat_yz_id='{0}'", hisRow["req_no"]));
                            if (rows.Length > 0)
                            {
                                rows[0]["IsCharge"] = "扣费成功";
                                rows[0]["HisPrice"] = hisRow["charge_fee"];

                                DataRow[] docRows =
                                    doc.Select(string.Format("doc_code='{0}' ", hisRow["op_id"]));

                                if (docRows.Length > 0)
                                {
                                    rows[0]["op_id_code"] = docRows[0]["doc_name"] != null
                                                                ? docRows[0]["doc_name"].ToString()
                                                                : "";
                                }
                            }
                        }

                        return bctable;

                    }
                }
                return table;
            }
            catch (Exception ex)
            {

                Logger.WriteException(this.GetType().ToString(), "获取费用清单", ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 获取病人组合明细
        /// </summary>
        /// <param name="yzids"></param>
        /// <returns></returns>
        public DataTable GetCombineByYzid(string yzids)
        {
            try
            {
                if (string.IsNullOrEmpty(yzids))
                    yzids = "'565656565'";
                string sql = string.Format(@"
SELECT bc_name AS pat_com_name,bc_cname.bc_yz_id AS pat_yz_id,bc_cname.bc_lis_code AS  pat_com_id
,bc_cname.bc_his_code AS pat_his_code,1 AS pat_seq,1 AS com_seq
  FROM bc_cname WHERE bc_yz_id IN({0}) AND bc_del<>1
", yzids);
                DBHelper helper = new DBHelper();

                DataTable dtCombine = helper.GetTable(sql);
                dtCombine.TableName = PatientTable.PatientCombineTableName;

                return dtCombine;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人组合信息出错,GetCombineByYzid=" + yzids, ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 获取病人基本信息
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetPatientInfoForBf(string patID)
        {
            try
            {
                string sqlSelect =
    string.Format(@"
SELECT
patients_newborn.*,
dict_instrmt.itr_ptype, 
dict_instrmt.itr_mid,
dict_instrmt.itr_name,
dict_sample.sam_name,
dict_checkb.chk_cname,
dict_doctor.doc_name, 
--PowerUserInfo.userName AS pat_i_code_name,
case 
when PowerUserInfo.userName is null then patients_newborn.pat_i_code 
else PowerUserInfo.userName  end as pat_i_code_name,
dict_no_type.no_name, 
dict_origin.ori_name,
pat_ctype_name = '',--检查类型名称
bc_status = '',
bc_print_flag = 0,
'' as bc_merge_comid,
pat_sex_name = case when pat_sex = '1' then '男'
                    when pat_sex = '2' then '女'
                    else '' end
FROM patients_newborn 
    LEFT OUTER JOIN dict_origin ON patients_newborn.pat_ori_id = dict_origin.ori_id 
    LEFT OUTER JOIN dict_no_type ON patients_newborn.pat_no_id = dict_no_type.no_id
    LEFT OUTER JOIN PowerUserInfo ON patients_newborn.pat_i_code = PowerUserInfo.loginId
    LEFT OUTER JOIN dict_doctor ON patients_newborn.pat_doc_id = dict_doctor.doc_id
    LEFT OUTER JOIN dict_checkb ON patients_newborn.pat_chk_id = dict_checkb.chk_id
    LEFT OUTER JOIN dict_sample ON patients_newborn.pat_sam_id = dict_sample.sam_id
    LEFT OUTER JOIN dict_instrmt ON patients_newborn.pat_itr_id = dict_instrmt.itr_id
    
WHERE patients_newborn.pat_id ='{0}'", patID);

                DBHelper helper = new DBHelper();


                DataTable dtPat = helper.GetTable(sqlSelect);

                FuncLibBIZ bllFuncLibBIZ = new FuncLibBIZ();
                DataTable dtPatCType = bllFuncLibBIZ.getPat_ctype();

                foreach (DataRow drPat in dtPat.Rows)
                {
                    if (drPat["pat_ctype"] != null && drPat["pat_ctype"] != DBNull.Value && drPat["pat_ctype"].ToString().Trim(null) != string.Empty)
                    {
                        string pat_ctype = drPat["pat_ctype"].ToString();
                        DataRow[] drs = dtPatCType.Select(string.Format("id='{0}'", pat_ctype));
                        if (drs.Length > 0)
                        {
                            drPat["pat_ctype_name"] = drs[0]["value"];
                        }
                    }
                }

                dtPat.TableName = PatientTable.PatientInfoTableName;

                return dtPat;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人信息出错,病人ID:" + patID, ex.ToString());
                throw;
            }
        }



        /// <summary>
        /// 获取病人组合明细
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetPatientCombineForBf(string patID)
        {
            try
            {
                string sql = string.Format(@"
SELECT 
dict_combine.com_name as pat_com_name,
dict_combine.com_seq,
patients_mi_newborn.*
FROM patients_mi_newborn 
INNER JOIN dict_combine ON patients_mi_newborn.pat_com_id = dict_combine.com_id
WHERE (patients_mi_newborn.pat_id = '{0}')
order by pat_seq asc
", patID);
                DBHelper helper = new DBHelper();


                DataTable dtCombine = helper.GetTable(sql);
                dtCombine.TableName = PatientTable.PatientCombineTableName;

                return dtCombine;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人组合信息出错,patID=" + patID, ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 获取病人普通结果
        /// </summary>
        /// <param name="patID"></param>
        /// <returns></returns>
        public DataTable GetPatientCommonResultForBf(string patID, bool getHistory)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DBHelper helper = new DBHelper();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string selectPatState = string.Format("select top 1 pat_id,pat_sid,pat_flag,pat_sam_id,pat_itr_id,pat_sam_rem,pat_sex,pat_age,pat_weight from patients_newborn where pat_id = '{0}'", patID);

                DataTable dtPat = helper.GetTable(selectPatState);

                EntityPatient2 entityPatient = new EntityPatient2();

                if (dtPat.Rows.Count > 0)
                {
                    entityPatient.pat_id = dtPat.Rows[0]["pat_id"].ToString();
                    entityPatient.pat_sid = dtPat.Rows[0]["pat_sid"].ToString();
                    entityPatient.pat_itr_id = dtPat.Rows[0]["pat_itr_id"].ToString();
                    entityPatient.pat_sam_id = dtPat.Rows[0]["pat_sam_id"].ToString();
                    entityPatient.pat_sam_id = dtPat.Rows[0]["pat_sam_id"].ToString();
                    entityPatient.pat_weight = dtPat.Rows[0]["pat_weight"].ToString();
                    try
                    {
                        if (!Compare.IsEmpty(dtPat.Rows[0]["pat_age"]))
                        {
                            entityPatient.pat_age = Convert.ToInt32(dtPat.Rows[0]["pat_age"]);
                        }
                    }
                    catch
                    {
                    }

                    if (!Compare.IsEmpty(dtPat.Rows[0]["pat_flag"]))
                    {
                        entityPatient.pat_flag = Convert.ToInt32(dtPat.Rows[0]["pat_flag"]);
                    }
                    else
                    {
                        entityPatient.pat_flag = 0;
                    }


                    if (entityPatient.pat_flag == 0)
                    {
                        string sampRem = string.Empty;
                        if (!Compare.IsEmpty(dtPat.Rows[0]["pat_sam_rem"]))
                        {
                            sampRem = dtPat.Rows[0]["pat_sam_rem"].ToString();
                        }
                        GenerateAutoCalItem(entityPatient, entityPatient.pat_sam_id, sampRem);
                        //FillNotNullResult(entityPatient);
                        dic = UpdateResultNotCombineItem(patID);
                    }
                }

                //if (Compare.IsNullOrDBNull(objState) || objState.ToString() == "0")
                //{
                //    GenerateAutoCalItem(patID);
                //    FillNotNullResult(patID);
                //    UpdateResultNotCombineItem(patID);
                //}

                string sql = string.Format(@"
select
resulto_newborn.res_key,--结果主键标识
resulto_newborn.res_id,--结果ID(病人ID)
resulto_newborn.res_itr_id,--仪器ID
resulto_newborn.res_sid,--样本号
resulto_newborn.res_itm_id,--项目ID
dict_item.itm_ecd as res_itm_ecd,--项目代码
dict_item.itm_name as res_itm_name,--项目名称
resulto_newborn.res_itm_rep_ecd,--项目报告编号
resulto_newborn.res_chr,--结果
resulto_newborn.res_od_chr,--OD结果
resulto_newborn.res_cast_chr,--数值结果
resulto_newborn.res_price,--价格
resulto_newborn.res_ref_exp,--阳性标志 （提示）
resulto_newborn.res_ref_flag,--阳性标志(偏高，偏低，阳性，正常,etc)
resulto_newborn.res_meams,--实验方法
resulto_newborn.res_date,--结果日期
resulto_newborn.res_flag,--有效标志
dict_instrmt_warningsigns.itr_warn_trandate,--项目状态
resulto_newborn.res_itr_ori_id,--原始仪器id
resulto_newborn.res_ref_type,--参考值类型
res_type = case when dict_item.itm_cal_flag = 1 then 2
                else resulto_newborn.res_type end, --结果类型
resulto_newborn.res_rep_type,--报告类型
resulto_newborn.res_com_id,--组合ID
res_com_name = case when (resulto_newborn.res_com_id is null or resulto_newborn.res_com_id ='') then '无组合'
                    else  dict_combine.com_name
                    end,--组合名称
res_com_seq = isnull(dict_combine.com_seq,isnull(patients_mi_newborn.pat_seq,99999)),
resulto_newborn.res_unit,--单位
itm_dtype = '',--项目数据类型
itm_max_digit = 0,--小数位数
res_max = '',--极大阈值
res_min = '',--极小阈值
res_pan_h = '',--危急值上限
res_pan_l = '',--危急值下限
res_ref_h = '',--参考值上限
res_ref_l = '',--参考值下限
res_max_cal = '',--极大阈值
res_min_cal = '',--极小阈值
res_pan_h_cal = '',--危急值上限
res_pan_l_cal = '',--危急值下限
res_ref_h_cal = '',--参考值上限
res_ref_l_cal = '',--参考值下限
res_ref_range = '',--参考值范围
res_allow_values = '',--允许出现的结果
res_positive_result = '',--阳性提示结果
res_custom_critical_result = '',--自定义危急值
history_result1='',--历史结果,
history_date1 = '',--历史结果时间
history_result2='',
history_date2 = '',
history_result3='',
history_date3 = '',
patients_newborn.pat_sam_id,
patients_newborn.pat_sex,
patients_newborn.pat_age,
patients_newborn.pat_sam_rem,
patients_newborn.pat_urgent_flag,
patients_newborn.pat_flag,--审核状态
resulto_newborn.res_exp,
patients_newborn.pat_dep_id,
isnew=0,
resulto_newborn.res_recheck_flag,
res_ref_flag_h1=0,
res_ref_flag_h2=0,
res_ref_flag_h3=0,
res_origin_record = 1,
dict_combine_mi.com_popedom as isnotempty,--是否为必录项目
calculate_source_itm_id = '',
calculate_dest_itm_id = '',
res_itm_seq = dict_combine_mi.com_sort,
isnull(dict_item.itm_seq,0) as itm_seq
,resulto_newborn.res_chr2,--结果2
resulto_newborn.res_chr3, --结果3
dict_instrmt.itr_mid
from resulto_newborn
left join patients_newborn on patients_newborn.pat_id = resulto_newborn.res_id
left join dict_item on dict_item.itm_id = resulto_newborn.res_itm_id--join项目表
left join dict_combine on dict_combine.com_id = resulto_newborn.res_com_id--join项目组合表
left join patients_mi_newborn on (patients_mi_newborn.pat_com_id = resulto_newborn.res_com_id and patients_mi_newborn.pat_id = resulto_newborn.res_id)--join病人检验组合
left join dict_combine_mi on  patients_mi_newborn.pat_com_id = dict_combine_mi.com_id and resulto_newborn.res_itm_id = dict_combine_mi.com_itm_id
left join dict_instrmt_warningsigns on dict_instrmt_warningsigns.itr_warn_origdate=resulto_newborn.res_exp
left join dict_instrmt on resulto_newborn.res_itr_ori_id=dict_instrmt.itr_id
where resulto_newborn.res_id = '{0}' and res_flag = 1
order by dict_combine.com_seq asc, dict_combine_mi.com_sort asc,dict_item.itm_seq ,resulto_newborn.res_itm_ecd asc
", patID); //加上项目排序码排序 edit by sink 2010-6-8




                DataTable dtResult = helper.GetTable(sql);
                dtResult.TableName = PatientTable.PatientResultTableName;

                if (dtResult.Rows.Count > 0)
                {
                    List<string> itemsID = new List<string>();
                    foreach (DataRow drResult in dtResult.Rows)
                    {
                        if (!Compare.IsEmpty(drResult["res_itm_id"]))
                        {
                            itemsID.Add(drResult["res_itm_id"].ToString());


                            if (Compare.IsEmpty(drResult["res_com_id"]) &&
                                dic.ContainsKey(drResult["res_itm_id"].ToString()))
                            {
                                string comid = dic[drResult["res_itm_id"].ToString()];
                                drResult["res_com_id"] = comid;


                                //获取组合排序
                                var combine = DictCombineCache.Current.GetCombineByID(comid, true);

                                if (combine != null && combine.com_seq != null)
                                {
                                    drResult["res_com_seq"] = combine.com_seq;
                                }

                                var combineMi = DictCombineMiCache2.Current.GetAll().Find(
                                    a => a.com_id == comid && a.com_itm_id == drResult["res_itm_id"].ToString());

                                if (combineMi != null)
                                {
                                    if (combineMi.com_sort != null)
                                        drResult["res_itm_seq"] = combineMi.com_sort;

                                    if (combineMi.com_popedom != null)
                                        drResult["isnotempty"] = combineMi.com_popedom;
                                }
                            }

                        }
                    }




                    string pat_sam_id = dtResult.Rows[0]["pat_sam_id"].ToString();

                    string sam_rem = dtResult.Rows[0]["pat_sam_rem"].ToString();

                    string pat_depcode = string.Empty;
                    if (!Compare.IsNullOrDBNull(dtResult.Rows[0]["pat_dep_id"]))
                    {
                        pat_depcode = dtResult.Rows[0]["pat_dep_id"].ToString();
                    }

                    int pat_age = PatCommonBll.GetConfigAge(dtResult.Rows[0]["pat_age"]);
                    string pat_sex = PatCommonBll.GetConfigSex(dtResult.Rows[0]["pat_sex"]);
                    string res_itr_id = dtResult.Rows[0]["res_itr_id"].ToString();

                    DataTable dtItmRef = DictItemBLL.NewInstance.GetItemsWithSamRef(itemsID, pat_sam_id, pat_age, pat_sex, sam_rem, res_itr_id, pat_depcode);

                    if (dtItmRef.Rows.Count > 0)
                    {
                        foreach (DataRow drResult in dtResult.Rows)//循环结果表
                        {
                            if (!Compare.IsEmpty(drResult["res_itm_id"]))
                            {
                                DataRow[] drsItmRef = dtItmRef.Select(string.Format("itm_id = '{0}'", drResult["res_itm_id"].ToString()));

                                if (drsItmRef.Length > 0)
                                {


                                    drResult["res_ref_l"] = drsItmRef[0]["itm_ref_l"];
                                    drResult["res_ref_h"] = drsItmRef[0]["itm_ref_h"];

                                    drResult["res_pan_l"] = drsItmRef[0]["itm_pan_l"];
                                    drResult["res_pan_h"] = drsItmRef[0]["itm_pan_h"];

                                    drResult["res_min"] = drsItmRef[0]["itm_min"];
                                    drResult["res_max"] = drsItmRef[0]["itm_max"];

                                    //允许出现的结果
                                    if (drsItmRef[0].Table.Columns.Contains("itm_allow_result"))
                                    {
                                        drResult["res_allow_values"] = drsItmRef[0]["itm_allow_result"];
                                    }

                                    //阳性提示结果
                                    if (drsItmRef[0].Table.Columns.Contains("itm_positive_result"))
                                    {
                                        drResult["res_positive_result"] = drsItmRef[0]["itm_positive_result"];
                                    }

                                    //自定义危急值
                                    if (drsItmRef[0].Table.Columns.Contains("itm_urgent_result"))
                                    {
                                        drResult["res_custom_critical_result"] = drsItmRef[0]["itm_urgent_result"];
                                    }

                                    drResult["res_ref_l_cal"] = drsItmRef[0]["itm_ref_l"];
                                    drResult["res_ref_h_cal"] = drsItmRef[0]["itm_ref_h"];

                                    if (
                                        !string.IsNullOrEmpty(drResult["res_ref_l_cal"].ToString().Trim())
                                        && !string.IsNullOrEmpty(drResult["res_ref_h_cal"].ToString().Trim()))
                                    {
                                        drResult["res_ref_range"] = drResult["res_ref_l_cal"].ToString() + " - " + drResult["res_ref_h_cal"].ToString();
                                    }
                                    else
                                    {
                                        drResult["res_ref_range"] = drResult["res_ref_l_cal"].ToString() + drResult["res_ref_h_cal"].ToString();
                                    }


                                    drResult["res_pan_l_cal"] = drsItmRef[0]["itm_pan_l"];
                                    drResult["res_pan_h_cal"] = drsItmRef[0]["itm_pan_h"];

                                    drResult["res_min_cal"] = drsItmRef[0]["itm_min"];
                                    drResult["res_max_cal"] = drsItmRef[0]["itm_max"];


                                    drResult["res_meams"] = drsItmRef[0]["itm_meams"];
                                    drResult["itm_dtype"] = drsItmRef[0]["itm_dtype"];
                                    drResult["itm_max_digit"] = drsItmRef[0]["itm_max_digit"];
                                    drResult["res_unit"] = drsItmRef[0]["itm_unit"];
                                    //resulto.res_unit,--单位
                                    //itm_dtype = '',--项目数据类型
                                    //itm_max_digit = 0,--小数位数
                                }
                            }
                        }
                    }
                }

                foreach (DataRow drResult in dtResult.Rows)
                {
                    ItemRefFormatter.Format(drResult, "res_ref_l_cal", "res_ref_h_cal", "res_min_cal", "res_max_cal", "res_pan_l_cal", "res_pan_h_cal");
                }

                if (getHistory)
                {
                    //获取最近3次历史记录
                    #region 获取最近3次结果
                    DataTable dtPatientInfo = new DBHelper().GetTable(string.Format(@"
select pat_name,pat_itr_id,pat_social_no,pat_sam_id,pat_no_id,pat_in_no,pat_pid,pat_age,pat_sex,pat_date,pat_upid from patients_newborn where pat_id ='{0}'", patID));
                    if (dtPatientInfo.Rows.Count > 0)
                    {
                        DataRow drPatient = dtPatientInfo.Rows[0];

                        string pat_upid = string.Empty;
                        string pat_itr_id = string.Empty;
                        //string pat_sam_id = string.Empty;
                        //string pat_no_id = string.Empty;
                        //string pat_in_no = string.Empty;
                        //string pat_sex = string.Empty;
                        //int ageMinute = -1;
                        DateTime? patDate = null;

                        if (!Compare.IsNullOrDBNull(drPatient["pat_date"]))
                        {
                            patDate = DateTime.Parse(drPatient["pat_date"].ToString());
                        }


                        //string itr_id, string pat_sam_id, string pat_no_id, string pat_in_no, string pat_name, int resultCount
                        if (drPatient["pat_upid"] != null && drPatient["pat_upid"] != DBNull.Value)
                        {
                            pat_upid = drPatient["pat_upid"].ToString();
                        }

                        if (drPatient["pat_itr_id"] != null && drPatient["pat_itr_id"] != DBNull.Value)
                        {
                            pat_itr_id = drPatient["pat_itr_id"].ToString();
                        }


                        if (pat_upid != string.Empty
                            && pat_itr_id != string.Empty
                       )
                        {

                            if (dtResult.Rows.Count > 0)
                            {
                                DataTable dthistoryResult = GetPatCommonResultHistoryForBf(patID, pat_itr_id, pat_upid, patDate);
                                StringBuilder sbResultId = new StringBuilder();//项目编码
                                StringBuilder sbResultEcd = new StringBuilder();//项目代码
                                foreach (DataRow drResult in dtResult.Rows)
                                {
                                    sbResultId.Append(string.Format(",'{0}'", drResult["res_itm_id"]));
                                    if (!string.IsNullOrEmpty(drResult["res_itm_ecd"].ToString()))
                                    {
                                        sbResultEcd.Append(string.Format(",'{0}'", drResult["res_itm_ecd"]));
                                    }
                                }
                                sbResultId.Remove(0, 1);
                                if (sbResultEcd.Length > 0) { sbResultEcd.Remove(0, 1); }

                                DataView dvhistoryResult = new DataView(dthistoryResult);
                                //是否用代码关联
                                //系统配置：历史结果123使用[项目代码]关联
                                bool IsResItmEcdConnected = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Lab_History123WithItmEcd") == "是";

                                if (IsResItmEcdConnected)
                                {
                                    dvhistoryResult.RowFilter = string.Format("res_itm_ecd in ({0})", sbResultEcd);
                                }
                                else
                                {
                                    dvhistoryResult.RowFilter = string.Format("res_itm_id in ({0})", sbResultId);
                                }
                                dvhistoryResult.Sort = "res_date desc";
                                DataTable dtTimes = dvhistoryResult.ToTable(true, "res_id");

                                //如果用代码关联项目,则把相同代码的项目的编码更新为当前项目的编码
                                if (IsResItmEcdConnected && dtTimes != null && dtTimes.Rows.Count > 0)
                                {
                                    foreach (DataRow drResult in dtResult.Rows)
                                    {
                                        if (!string.IsNullOrEmpty(drResult["res_itm_ecd"].ToString()))
                                        {
                                            DataRow[] drTempSelTimes = dthistoryResult.Select(string.Format("res_itm_ecd='{0}'", drResult["res_itm_ecd"].ToString()));
                                            for (int tempn = 0; tempn < drTempSelTimes.Length; tempn++)
                                            {
                                                drTempSelTimes[tempn]["res_itm_id"] = drResult["res_itm_id"].ToString();
                                            }
                                        }
                                    }
                                    dthistoryResult.AcceptChanges();
                                }


                                try
                                {
                                    if (dtTimes.Rows.Count > 0)
                                    {
                                        for (int j = 0; j < dtTimes.Rows.Count; j++)
                                        {
                                            DataRow drTime = dtTimes.Rows[j];
                                            foreach (DataRow drResult in dtResult.Rows)
                                            {
                                                if (drResult["res_itm_id"] != null && drResult["res_itm_id"] != DBNull.Value)
                                                {
                                                    string itm_id = drResult["res_itm_id"].ToString();

                                                    DataRow[] drs = dthistoryResult.Select(string.Format("res_itm_id ='{0}' and res_id='{1}'", itm_id, drTime["res_id"]), "res_date desc");

                                                    if (drs.Length > 0)
                                                    {
                                                        drResult["history_result" + (j + 1).ToString()] = drs[0]["res_chr"];
                                                        drResult["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                        dtResult.Rows[0]["history_date" + (j + 1).ToString()] = Convert.ToDateTime(drs[0]["res_date"]).ToString("yyyy-MM-dd HH:mm:ss");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Logger.WriteException(this.GetType().Name, "GetPatientResult", ex.ToString());
                                }
                            }
                        }
                    }
                    #endregion
                }

                sw.Stop();
                Logger.Debug(string.Format("数据库:GetPatientResult,获取病人结果表,耗时 {0}ms", sw.ElapsedMilliseconds));

                return dtResult;
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取病人结果出错,patID=" + patID, ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// 获取病人普通结果、历史结果
        /// </summary>
        /// <param name="patID"></param>
        /// <param name="dthistoryResult">返回病人历史结果</param>
        /// <returns></returns>
        public DataTable GetPatientCommonResultForBf(string patID, bool getHistory, out DataTable dthistoryResult)
        {
            dthistoryResult = null;
            return null;
        }

    }
}
