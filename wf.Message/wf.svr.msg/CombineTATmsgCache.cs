using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcl.root.dac;
using System.Data.SqlClient;

namespace dcl.svr.msg
{
    /// <summary>
    /// 组合TAT数据
    /// </summary>
    public class CombineTATmsgCache
    {
        #region singleton
        private static object objLock = new object();

        private static CombineTATmsgCache _instance = null;

        /// <summary>
        /// 当时是否没在处理
        /// </summary>
        private static bool IsCurrNotDisposal { get; set; }

        public static CombineTATmsgCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CombineTATmsgCache();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 组合TAT信息缓存
        /// </summary>
        private DataTable cache = null;

        /// <summary>
        /// 条码组合TAT信息缓存
        /// </summary>
        private DataTable cacheBc = null;

        /// <summary>
        /// 条码组合检验中TAT信息缓存
        /// </summary>
        private DataTable cacheBcLab = null;

        /// <summary>
        /// 条码TAT信息缓存(采集到签收)
        /// </summary>
        private DataTable cacheBcSamplToReceive = null;


        /// <summary>
        /// 仪器危急值数据
        /// </summary>
        private CombineTATmsgCache()
        {
            this.cache = new DataTable();
            this.cache.TableName = "ComTATMsgCache";

            this.cacheBc = new DataTable();
            this.cacheBc.TableName = "BcComTATMsgCache";

            this.cacheBcLab = new DataTable();
            this.cacheBcLab.TableName = "BcComLabTATMsgCache";

            this.cacheBcSamplToReceive = new DataTable();
            this.cacheBcSamplToReceive.TableName = "SamplToReceiveTATMsgCache";

            IsCurrNotDisposal = true;
        }
        #endregion

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public void Refresh()
        {
            this.cache = this.GetCombineTATmsgToCache();
            this.cacheBc = this.GetBcComTAtMsgToCache();
            this.cacheBcLab = this.GetBcComLabTAtMsgToCache();
            this.cacheBcSamplToReceive = this.GetBcSamplToReceiveTAtMsgToCache();
        }

        /// <summary>
        /// 获取组合TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        public DataTable GetCombineTATmsgToCache()
        {
            try
            {
                //总开关--是否启动组合TAT提醒
                bool Message_ShowTATMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Combine_TAT_IsNotify") == "是";

                //服务端web.config也可以控制关闭功能
                string webconfig_Combine_TAT_IsNotify = System.Configuration.ConfigurationManager.AppSettings["Combine_TAT_IsNotify"];
                if (!string.IsNullOrEmpty(webconfig_Combine_TAT_IsNotify)
                    && (webconfig_Combine_TAT_IsNotify.ToUpper() == "N" || webconfig_Combine_TAT_IsNotify.ToUpper() == "NO"))
                {
                    Message_ShowTATMsg = false;
                }


                if (!Message_ShowTATMsg)
                {
                    //当不启动时
                    return null;
                }
            }
            catch
            {
                return null;
            }

            DataTable dtbResult = null;

            #region 注释2015-5-15 改为 签收到报告
            /** 2015-5-15 改为 签收到报告
            string strSQL = @"select msg_tat.*,
(select isnull(max(dict_combine_timerule.com_time),0) 
from 
patients_mi with(nolock)
inner join dict_combine_timerule_related on patients_mi.pat_com_id=dict_combine_timerule_related.com_id
inner join dict_combine_timerule on dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
where 
dict_combine_timerule.com_time_start_type='30'
and dict_combine_timerule.com_time_end_type='60'
and dict_combine_timerule.com_time>0
and patients_mi.pat_id=msg_tat.pat_id) max_tat,
(case when msg_tat.time_mi>msg_tat.time_tat then 1 else 0 end) over_tat
from
(
select patients.pat_id,
patients.pat_itr_id,
patients.pat_sid,
patients.pat_host_order,
patients.pat_name,
patients.pat_date,
patients.pat_jy_date,
patients.pat_flag,
patients.pat_ori_id,
patients.pat_bar_code,
dict_instrmt.itr_mid,
dict_instrmt.itr_type, --物理组 
dict_type.type_name,
dict_combine_timerule.com_time as time_tat,
dict_combine_timerule.com_time*0.7 as time_tatw,
Datediff(mi,patients.pat_jy_date,getdate()) as time_mi
from patients_mi with(nolock)
inner join patients on patients_mi.pat_id=patients.pat_id
inner join dict_combine_timerule_related on patients_mi.pat_com_id=dict_combine_timerule_related.com_id
inner join dict_combine_timerule on  dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
left join dict_type on dict_instrmt.itr_type=dict_type.type_id
where patients.pat_jy_date>=(getdate()-1)
and (patients.pat_flag=0 or patients.pat_flag=1)
and dict_combine_timerule.com_time_start_type='30'
and dict_combine_timerule.com_time_end_type='60'
and dict_combine_timerule.com_time>0
and patients.pat_ori_id=dict_combine_timerule.com_time_ori_id
) as msg_tat
where msg_tat.time_mi>msg_tat.time_tatw";
            **/
            
            #endregion

            string strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tatw) as time_mi_over,
(select isnull(max(dict_combine_timerule.com_time),0)
from bc_cname with(nolock)
inner join dict_combine_timerule_related on bc_cname.bc_lis_code=dict_combine_timerule_related.com_id
inner join dict_combine_timerule on dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
where 
bc_cname.bc_del='0'
and dict_combine_timerule.com_time_start_type='5'
and dict_combine_timerule.com_time_end_type='60'
and dict_combine_timerule.com_time>0
and dict_combine_timerule.com_time_ori_id=msg_tat.bc_ori_id
and bc_cname.bc_bar_code=msg_tat.bc_bar_code) max_tat,
(case when msg_tat.time_mi>msg_tat.time_tat then 1 else 0 end) over_tat
from
(
select bc_patients.bc_bar_code,
bc_patients.bc_receiver_date,
bc_patients.bc_his_name as bc_com_name,
bc_patients.bc_name,
bc_patients.bc_bed_no,
bc_patients.bc_in_no,
bc_patients.bc_upid,
bc_patients.bc_ori_id,
patients.pat_id,
patients.pat_sid,
patients.pat_host_order,
patients.pat_date,
patients.pat_itr_id,
dict_instrmt.itr_mid,
bc_patients.bc_ctype as itr_type, --物理组 
isnull(bc_patients.bc_urgent_flag,cast(0 as bit)) as bc_urgent_flag,
dict_combine_timerule.com_time_type,
dict_type.type_name,
dict_combine_timerule.com_time as time_tat,
dict_combine_timerule.com_time*0.7 as time_tatw,
Datediff(mi,tpr_receiver_date,getdate()) as time_mi,
(case when bc_patients.bc_merge_comid is not null and bc_patients.bc_merge_comid<>'' then isnull((select top 1 1 from bc_patients as z_bc with(nolock) inner join patients z_pat with(nolock) on z_pat.pat_bar_code=z_bc.bc_bar_code where z_bc.bc_merge_comid=bc_patients.bc_merge_comid and z_bc.bc_bar_code<>bc_patients.bc_bar_code),0) else 0 end) as merge_flag,
((select bc_status.bc_cname from bc_status where bc_status.bc_name = dict_combine_timerule.com_time_start_type)+' - '+
(select bc_status.bc_cname from bc_status where bc_status.bc_name = dict_combine_timerule.com_time_end_type)) as status,
(select count(patients_mi.pat_id) from patients_mi with(nolock) where patients_mi.pat_id=patients.pat_id and patients_mi.pat_com_id=bc_cname.bc_lis_code) as pmi_c
from tat_pro_record with(nolock) 
left join  bc_patients with(nolock) on bc_patients.bc_bar_code=tat_pro_record.tpr_bar_code
left join patients with(nolock) on bc_patients.bc_bar_code=patients.pat_bar_code
inner join bc_cname with(nolock) on bc_patients.bc_bar_no=bc_cname.bc_bar_no
inner join dict_combine_timerule_related on bc_cname.bc_lis_code=dict_combine_timerule_related.com_id
inner join dict_combine_timerule on dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
left join dict_type on bc_patients.bc_ctype=dict_type.type_id
where tat_pro_record.tpr_createdate>=(getdate()-1)
and bc_cname.bc_del='0' 
and tpr_receiver_date is not null
and dict_combine_timerule.com_time_start_type='5'
and dict_combine_timerule.com_time_end_type='60'
and dict_combine_timerule.com_time>0
and bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id
and (patients.pat_flag is null or patients.pat_flag=0 or patients.pat_flag=1)
and not exists(select top 1 1 from resulto with(nolock) where resulto.res_id=patients.pat_id and res_recheck_flag=1 and resulto.res_id is not null)
and (patients.pat_recheck_flag is null or patients.pat_recheck_flag<>1)
and not exists(select top 1 1 from bc_sign with(nolock) where bc_sign.bc_bar_code=bc_patients.bc_bar_code and bc_sign.bc_status='560')
) as msg_tat
where msg_tat.time_mi>msg_tat.time_tatw and (pat_id is null or pmi_c>0)";

            //佛山市一,来源为空时,忽略来源过滤
            if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Combine_TAT_IsNotify") == "outlink")
            {
                //strSQL = strSQL.Replace("and bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id"
                //    , "and (bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id or dict_combine_timerule.com_time_ori_id='' or dict_combine_timerule.com_time_ori_id is null) ");


                #region 流程从送达开始监控

                strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tatw) as time_mi_over,
(select isnull(max(dict_combine_timerule.com_time),0)
from bc_cname with(nolock)
inner join dict_combine_timerule_related with(nolock) on bc_cname.bc_lis_code=dict_combine_timerule_related.com_id
inner join dict_combine_timerule with(nolock) on dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
where 
bc_cname.bc_del='0'
and dict_combine_timerule.com_time_start_type='4'
and dict_combine_timerule.com_time_end_type='60'
and dict_combine_timerule.com_time>0
and (dict_combine_timerule.com_time_ori_id=msg_tat.bc_ori_id or dict_combine_timerule.com_time_ori_id is null or dict_combine_timerule.com_time_ori_id='')
and bc_cname.bc_bar_code=msg_tat.bc_bar_code) max_tat,
(case when msg_tat.time_mi>msg_tat.time_tat then 1 else 0 end) over_tat
from
(
select bc_patients.bc_bar_code,
bc_patients.bc_receiver_date,
bc_patients.bc_his_name as bc_com_name,
bc_patients.bc_name,
bc_patients.bc_bed_no,
bc_patients.bc_in_no,
bc_patients.bc_upid,
bc_patients.bc_ori_id,
patients.pat_id,
patients.pat_sid,
patients.pat_host_order,
patients.pat_date,
patients.pat_itr_id,
dict_instrmt.itr_mid,
bc_patients.bc_ctype as itr_type, --物理组 
isnull(bc_patients.bc_urgent_flag,cast(0 as bit)) as bc_urgent_flag,
dict_combine_timerule.com_time_type,
dict_type.[type_name],
dict_combine_timerule.com_time as time_tat,
dict_combine_timerule.com_time*0.7 as time_tatw,
Datediff(mi,tpr_reach_date,getdate()) as time_mi,
0 as merge_flag,
((select top 1 bc_status.bc_cname from bc_status where bc_status.bc_name = dict_combine_timerule.com_time_start_type)+' - '+
(select top 1 bc_status.bc_cname from bc_status where bc_status.bc_name = dict_combine_timerule.com_time_end_type)) as status,
(select count(patients_mi.pat_id) from patients_mi with(nolock) where patients_mi.pat_id=patients.pat_id and patients_mi.pat_com_id=bc_cname.bc_lis_code) as pmi_c
from tat_pro_record with(nolock) 
left join  bc_patients with(nolock) on bc_patients.bc_bar_code=tat_pro_record.tpr_bar_code
left join patients with(nolock) on bc_patients.bc_bar_code=patients.pat_bar_code
inner join bc_cname with(nolock) on bc_patients.bc_bar_no=bc_cname.bc_bar_no
inner join dict_combine_timerule_related with(nolock) on bc_cname.bc_lis_code=dict_combine_timerule_related.com_id
inner join dict_combine_timerule with(nolock) on dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
left join dict_type on bc_patients.bc_ctype=dict_type.[type_id]
where tat_pro_record.tpr_createdate>=(getdate()-1)
and bc_cname.bc_del='0' 
and tpr_reach_date is not null
and dict_combine_timerule.com_time_start_type='4'
and dict_combine_timerule.com_time_end_type='60'
and dict_combine_timerule.com_time>0
and (bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id or dict_combine_timerule.com_time_ori_id is null or dict_combine_timerule.com_time_ori_id='') 
and (patients.pat_flag is null or patients.pat_flag=0 or patients.pat_flag=1)
) as msg_tat
where msg_tat.time_mi>msg_tat.time_tatw and (pat_id is null or pmi_c>0)";

                #endregion
            }

            try
            {
                DataSet dsResult = new DataSet();
                DBHelper objHelper = new DBHelper();
                dsResult = objHelper.GetDataSet(strSQL);
                dtbResult = dsResult.Tables[0];
                //过滤
                if (dtbResult != null && dtbResult.Rows.Count > 0)
                {
                    //用每条条码最大值进行过滤
                    DataView dvtemp = dtbResult.DefaultView.ToTable().DefaultView;
                    dvtemp.RowFilter = "time_mi>(max_tat*0.7) and merge_flag=0 and ((bc_urgent_flag=1 and com_time_type='急查') or (bc_urgent_flag=0 and com_time_type='常规'))";
                    dtbResult = dvtemp.ToTable();
                    dtbResult.AcceptChanges();
                }
                List<string> notebarcode = new List<string>();//记录条码
                //每条条码只取一次
                if (dtbResult != null && dtbResult.Rows.Count > 0)
                {
                    DataTable dtclone = dtbResult.Clone();
                    foreach (DataRow drtemp in dtbResult.Rows)
                    {
                        if (!string.IsNullOrEmpty(drtemp["bc_bar_code"].ToString())
                            && !notebarcode.Contains(drtemp["bc_bar_code"].ToString()))
                        {
                            notebarcode.Add(drtemp["bc_bar_code"].ToString());
                            drtemp["time_tat"] = drtemp["max_tat"];
                            drtemp["time_tatw"] = Convert.ToInt32(drtemp["max_tat"].ToString()) * 0.7;
                            drtemp["time_mi_over"] = Convert.ToInt32(drtemp["time_mi"].ToString()) - Convert.ToInt32(drtemp["max_tat"].ToString());
                            drtemp["over_tat"] = Convert.ToInt32(drtemp["time_mi"].ToString()) > Convert.ToInt32(drtemp["max_tat"].ToString()) ? 1 : 0;

                            dtclone.Rows.Add(drtemp.ItemArray);
                        }
                    }
                    dtbResult.Clear();
                    dtbResult = dtclone.Copy();
                    dtbResult.AcceptChanges();
                }
                dtbResult.TableName = "ComTATMsgCache";
            }
            catch (Exception objEx)
            {
                //throw;
                Lib.LogManager.Logger.LogException("获取组合TAT数据", objEx);
            }

            return dtbResult;
        }

        /// <summary>
        /// 获取条码组合检验中TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        public DataTable GetBcComLabTAtMsgToCache()
        {
            try
            {
                //总开关--检验是否启动条码组合TAT提醒
                bool Message_ShowTATMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BCCombine_TAT_IsNotify") == "是";

                //服务端web.config也可以控制关闭功能
                string webconfig_BCCombine_TAT_IsNotify = System.Configuration.ConfigurationManager.AppSettings["BCCombine_TAT_IsNotify"];
                if (!string.IsNullOrEmpty(webconfig_BCCombine_TAT_IsNotify)
                    && (webconfig_BCCombine_TAT_IsNotify.ToUpper() == "N" || webconfig_BCCombine_TAT_IsNotify.ToUpper() == "NO"))
                {
                    Message_ShowTATMsg = false;
                }

                if (!Message_ShowTATMsg)
                {
                    //当不启动时
                    return null;
                }
            }
            catch
            {
                return null;
            }

            DataTable dtbResult = null;

            string strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tat) over_tat
from
(
select bc_patients.bc_bar_code,
patients.pat_id,
patients.pat_date,
patients.pat_jy_date,
(case when isnull(bc_patients.bc_urgent_flag,'0')='1' then '急' else '' end) bc_urgent_flagStr,
bc_patients.bc_d_name,
bc_cname.bc_name as bc_com_name,
bc_patients.bc_name,
bc_patients.bc_age,
bc_patients.bc_sex,
bc_patients.bc_bed_no,
bc_patients.bc_in_no,
bc_patients.bc_upid,
bc_patients.bc_ctype, --物理组 
bc_patients.bc_doct_name,
bc_patients.bc_occ_date,
bc_patients.bc_diag,
bc_patients.bc_times,
patients.pat_flag,
dict_instrmt.itr_mid,
dict_type.type_name,
dict_cuvette.cuv_name,
dict_sample.sam_name,
isnull(bc_patients.bc_urgent_flag,cast(0 as bit)) as bc_urgent_flag,
dict_combine_timerule.com_time_type,
dict_combine_timerule.com_time as time_tat,
Datediff(mi,tpr_jy_date,getdate()) as time_mi
from tat_pro_record with(nolock) 
left join  bc_patients with(nolock) on bc_patients.bc_bar_code=tat_pro_record.tpr_bar_code
inner join bc_cname with(nolock) on bc_patients.bc_bar_no=bc_cname.bc_bar_no
inner join patients with(nolock) on patients.pat_bar_code=bc_patients.bc_bar_no
inner join dict_combine_timerule_related with(nolock) on bc_cname.bc_lis_code=dict_combine_timerule_related.com_id
inner join dict_combine_timerule with(nolock) on dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
left join dict_type with(nolock) on bc_patients.bc_ctype=dict_type.type_id
left join dict_instrmt with(nolock) on patients.pat_itr_id=dict_instrmt.itr_id
left join dict_sample with(nolock) on bc_patients.bc_sam_id=dict_sample.sam_id
left join dict_cuvette with(nolock) on bc_patients.bc_cuv_code=dict_cuvette.cuv_code
where tat_pro_record.tpr_createdate>=getdate()-1
and bc_cname.bc_del='0' and bc_cname.bc_flag=1 and bc_patients.bc_del='0'
and tpr_jy_date is not null
and dict_combine_timerule.com_time>0
and (dict_combine_timerule.com_time_start_type='20' and dict_combine_timerule.com_time_end_type='60')
and (bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id or dict_combine_timerule.com_time_ori_id is null or dict_combine_timerule.com_time_ori_id='')
and (patients.pat_flag=0 or patients.pat_flag=1)
) as msg_tat
where  msg_tat.time_mi>msg_tat.time_tat";

            try
            {
                DataSet dsResult = new DataSet();
                DBHelper objHelper = new DBHelper();
                dsResult = objHelper.GetDataSet(strSQL);
                dtbResult = dsResult.Tables[0];
                dtbResult.TableName = "BcComLabTATMsgCache";

                if (dtbResult != null && dtbResult.Rows.Count > 0)
                {
                    //过滤
                    DataView dvtemp = dtbResult.DefaultView.ToTable().DefaultView;
                    dvtemp.RowFilter = "(bc_urgent_flag=1 and com_time_type='急查') or (bc_urgent_flag=0 and com_time_type='常规')";
                    dtbResult = dvtemp.ToTable();
                    dtbResult.TableName = "BcComLabTATMsgCache";
                }

            }
            catch (Exception objEx)
            {
                //throw;
                Lib.LogManager.Logger.LogException("获取条码组合检验中TAT数据", objEx);
            }

            return dtbResult;
        }

        /// <summary>
        /// 获取条码组合TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        public DataTable GetBcComTAtMsgToCache()
        {
            try
            {
                //总开关--检验是否启动条码组合TAT提醒
                bool Message_ShowTATMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BCCombine_TAT_IsNotify") == "是";

                //服务端web.config也可以控制关闭功能
                string webconfig_BCCombine_TAT_IsNotify = System.Configuration.ConfigurationManager.AppSettings["BCCombine_TAT_IsNotify"];
                if (!string.IsNullOrEmpty(webconfig_BCCombine_TAT_IsNotify)
                    && (webconfig_BCCombine_TAT_IsNotify.ToUpper() == "N" || webconfig_BCCombine_TAT_IsNotify.ToUpper() == "NO"))
                {
                    Message_ShowTATMsg = false;
                }

                if (!Message_ShowTATMsg)
                {
                    //当不启动时
                    return null;
                }
            }
            catch
            {
                return null;
            }

            DataTable dtbResult = null;

            #region MyRegion
            //            string strSQL = @"select msg_tat.*,
            //(msg_tat.time_mi-msg_tat.time_tat) over_tat
            //from
            //(
            //select bc_patients.bc_bar_code,
            //bc_patients.bc_date,
            //bc_patients.bc_lastaction_time,
            //(case when isnull(bc_patients.bc_urgent_flag,'0')='1' then '急' else '' end) bc_urgent_flagStr,
            //bc_patients.bc_d_name,
            //bc_cname.bc_name as bc_com_name,
            //bc_patients.bc_name,
            //bc_patients.bc_age,
            //bc_patients.bc_sex,
            //bc_patients.bc_bed_no,
            //bc_patients.bc_in_no,
            //bc_patients.bc_upid,
            //bc_patients.bc_exp,
            //bc_patients.bc_ctype, --物理组 
            //bc_patients.bc_doct_name,
            //bc_patients.bc_occ_date,
            //bc_patients.bc_diag,
            //bc_patients.bc_times,
            //bc_patients.bc_status,
            //dict_type.type_name,
            //dict_cuvette.cuv_name,
            //dict_sample.sam_name,
            //isnull(bc_patients.bc_urgent_flag,cast(0 as bit)) as bc_urgent_flag,
            //dict_combine_timerule.com_time_type,
            //dict_combine_timerule.com_time as time_tat,
            //Datediff(mi,bc_patients.bc_lastaction_time,getdate()) as time_mi
            //from bc_patients with(nolock)
            //inner join bc_cname with(nolock) on bc_patients.bc_bar_no=bc_cname.bc_bar_no
            //inner join dict_combine_timerule_related on bc_cname.bc_lis_code=dict_combine_timerule_related.com_id
            //inner join dict_combine_timerule on  dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
            //left join dict_type on bc_patients.bc_ctype=dict_type.type_id
            //left join dict_sample with(nolock) on bc_patients.bc_sam_id=dict_sample.sam_id
            //left join dict_cuvette with(nolock) on bc_patients.bc_cuv_code=dict_cuvette.cuv_code
            //where bc_patients.bc_lastaction_time>=getdate()-1
            //and bc_cname.bc_del='0' and (bc_cname.bc_flag<>1 or bc_cname.bc_flag is null) and bc_patients.bc_del='0'
            //and dict_combine_timerule.com_time>0
            //and ((dict_combine_timerule.com_time_start_type='2'and dict_combine_timerule.com_time_end_type='3')
            //or (dict_combine_timerule.com_time_start_type='3'and dict_combine_timerule.com_time_end_type='4')
            //or (dict_combine_timerule.com_time_start_type='4'and dict_combine_timerule.com_time_end_type='5')
            //or (dict_combine_timerule.com_time_start_type='5'and dict_combine_timerule.com_time_end_type='20')
            //or (dict_combine_timerule.com_time_start_type='8'and dict_combine_timerule.com_time_end_type='20'))
            //and bc_patients.bc_status=dict_combine_timerule.com_time_start_type
            //and (bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id or dict_combine_timerule.com_time_ori_id is null or dict_combine_timerule.com_time_ori_id='')
            //) as msg_tat
            //where  msg_tat.time_mi>msg_tat.time_tat"; 
            #endregion
            string strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tat) over_tat
from
(
select bc_patients.bc_bar_code,
bc_patients.bc_ori_id,
bc_patients.bc_date,
bc_patients.bc_lastaction_time,
(case when isnull(bc_patients.bc_urgent_flag,'0')='1' then '急' else '' end) bc_urgent_flagStr,
bc_patients.bc_d_name,
bc_cname.bc_name as bc_com_name,
bc_patients.bc_name,
bc_patients.bc_age,
bc_patients.bc_sex,
bc_patients.bc_bed_no,
bc_patients.bc_in_no,
bc_patients.bc_upid,
bc_patients.bc_exp,
bc_patients.bc_ctype, --物理组 
bc_patients.bc_doct_name,
bc_patients.bc_occ_date,
bc_patients.bc_diag,
bc_patients.bc_times,
bc_patients.bc_status,
dict_type.type_name,
dict_cuvette.cuv_name,
dict_sample.sam_name,
isnull(bc_patients.bc_urgent_flag,cast(0 as bit)) as bc_urgent_flag,
dict_combine_timerule.com_time_type,
dict_combine_timerule.com_time as time_tat,
Datediff(mi,bc_patients.bc_lastaction_time,getdate()) as time_mi,
((select bc_status.bc_cname from bc_status where bc_status.bc_name = dict_combine_timerule.com_time_start_type)+' - '+
(select bc_status.bc_cname from bc_status where bc_status.bc_name = dict_combine_timerule.com_time_end_type)) as status,
patients.pat_sid,
patients.pat_ctype,
(case when isnull(patients.pat_ctype,'')='1' then '常规'
when isnull(patients.pat_ctype,'')='2' then '急查'
when isnull(patients.pat_ctype,'')='3' then '保密'
when isnull(patients.pat_ctype,'')='4' then '溶栓' else '常规' end) as pat_ctype_name,
dict_instrmt.itr_mid
from tat_pro_record with(nolock) 
left join  bc_patients with(nolock) on bc_patients.bc_bar_code=tat_pro_record.tpr_bar_code
inner join bc_cname with(nolock) on bc_patients.bc_bar_no=bc_cname.bc_bar_no
inner join dict_combine_timerule_related on bc_cname.bc_lis_code=dict_combine_timerule_related.com_id
inner join dict_combine_timerule on  dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
left join dict_type on bc_patients.bc_ctype=dict_type.type_id
left join dict_sample with(nolock) on bc_patients.bc_sam_id=dict_sample.sam_id
left join dict_cuvette with(nolock) on bc_patients.bc_cuv_code=dict_cuvette.cuv_code
left join bc_status on bc_patients.bc_status=bc_status.bc_name
left join patients with(nolock) on bc_patients.bc_bar_code=patients.pat_bar_code 
left join dict_instrmt with(nolock) on patients.pat_itr_id=dict_instrmt.itr_id
where tat_pro_record.tpr_createdate>=getdate()-1
and bc_cname.bc_del='0' and (bc_cname.bc_flag<>1 or bc_cname.bc_flag is null) and bc_patients.bc_del='0'
and dict_combine_timerule.com_time>0
and ((dict_combine_timerule.com_time_start_type='2'and dict_combine_timerule.com_time_end_type='3')
or (dict_combine_timerule.com_time_start_type='3'and dict_combine_timerule.com_time_end_type='4')
or (dict_combine_timerule.com_time_start_type='4'and dict_combine_timerule.com_time_end_type='5')
or (dict_combine_timerule.com_time_start_type='5'and dict_combine_timerule.com_time_end_type='20')
or (dict_combine_timerule.com_time_start_type='8'and dict_combine_timerule.com_time_end_type='20'))
and bc_patients.bc_status=dict_combine_timerule.com_time_start_type
and (bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id or dict_combine_timerule.com_time_ori_id is null or dict_combine_timerule.com_time_ori_id='')
) as msg_tat
where  msg_tat.time_mi>msg_tat.time_tat";
            try
            {
                DataSet dsResult = new DataSet();
                DBHelper objHelper = new DBHelper();
                dsResult = objHelper.GetDataSet(strSQL);
                dtbResult = dsResult.Tables[0];
                dtbResult.TableName = "BcComTATMsgCache";

                if (dtbResult != null && dtbResult.Rows.Count > 0)
                {
                    //过滤
                    DataView dvtemp = dtbResult.DefaultView.ToTable().DefaultView;
                    dvtemp.RowFilter = "(bc_urgent_flag=1 and com_time_type='急查') or (bc_urgent_flag=0 and com_time_type='常规')";
                    dtbResult = dvtemp.ToTable();
                    dtbResult.TableName = "BcComTATMsgCache";
                }

            }
            catch (Exception objEx)
            {
                //throw;
                Lib.LogManager.Logger.LogException("获取条码组合TAT数据", objEx);
            }

            return dtbResult;
        }


        /// <summary>
        /// 查询组合TAT信息
        /// </summary>
        /// <param name="receiver_date">签收开始日期</param>
        /// <param name="strWhere">过滤条件</param>
        /// <returns></returns>
        public DataTable SearchCombineTATmsg(DateTime receiver_date, string strWhere)
        {
            DataTable dtbResult = new DataTable("dtComTATMsg");

            if (string.IsNullOrEmpty(strWhere)) strWhere = "";

            string strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tatw) as time_mi_over,
(case when msg_tat.time_mi>msg_tat.time_tat then 1 else 0 end) over_tat
from
(
select bc_patients.bc_bar_code,
bc_patients.bc_receiver_date,
bc_patients.bc_d_name,
bc_cname.bc_name as bc_com_name,
bc_patients.bc_name,
bc_patients.bc_bed_no,
bc_patients.bc_in_no,
bc_patients.bc_upid,
patients.pat_jy_date,
patients.pat_chk_date,
patients.pat_report_date,
patients.pat_itr_id,
dict_instrmt.itr_mid,
bc_patients.bc_ctype, --物理组 
dict_type.type_name,
dict_combine_timerule.com_time as time_tat,
dict_combine_timerule.com_time*0.7 as time_tatw,
Datediff(mi,tpr_receiver_date,getdate()) as time_mi,
Datepart(mi,getdate())+Datepart(hh,getdate())*60 as time_today,
Datepart(weekday,getdate()) as time_weekday
from tat_pro_record with(nolock) 
left join  bc_patients with(nolock) on bc_patients.bc_bar_code=tat_pro_record.tpr_bar_code
left join patients with(nolock) on bc_patients.bc_bar_code=patients.pat_bar_code
inner join bc_cname with(nolock) on bc_patients.bc_bar_no=bc_cname.bc_bar_no
inner join dict_combine_timerule_related on bc_cname.bc_lis_code=dict_combine_timerule_related.com_id
inner join dict_combine_timerule on  dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
left join dict_instrmt on patients.pat_itr_id=dict_instrmt.itr_id
left join dict_type on bc_patients.bc_ctype=dict_type.type_id
where tat_pro_record.tpr_createdate>='{0}'
and bc_cname.bc_del='0' and bc_patients.bc_del='0'
and tpr_receiver_date is not null
and dict_combine_timerule.com_time_start_type='5'
and dict_combine_timerule.com_time_end_type='60'
and dict_combine_timerule.com_time>0
and bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id
and (patients.pat_flag is null or patients.pat_flag=0 or patients.pat_flag=1)
) as msg_tat
where ((msg_tat.time_today>=msg_tat.time_mi and msg_tat.time_weekday<>1 and msg_tat.time_weekday<>7)
or (msg_tat.time_mi>msg_tat.time_today))
and msg_tat.time_mi>msg_tat.time_tatw
{1}
";
            //bc_patients.bc_receiver_date>=(getdate()-100)
            strSQL = string.Format(strSQL, receiver_date.Date.ToString(), strWhere);

            try
            {
                DataSet dsResult = new DataSet();
                DBHelper objHelper = new DBHelper();
                dsResult = objHelper.GetDataSet(strSQL);
                dtbResult = dsResult.Tables[0];
                dtbResult.TableName = "dtComTATMsg";
            }
            catch (Exception objEx)
            {
                //throw;
                Lib.LogManager.Logger.LogException("查询组合TAT数据", objEx);
            }

            return dtbResult;
        }

        /// <summary>
        /// 获取条码(采集到签收)TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        public DataTable GetBcSamplToReceiveTAtMsgToCache()
        {
            try
            {
                //系统配置：检验是否启动条码[采集_签收]TAT提醒
                bool Message_ShowTATMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BCSamplToReceive_TAT_IsNotify") == "是";

                if (!Message_ShowTATMsg)
                {
                    //当不启动时
                    return null;
                }
            }
            catch
            {
                return null;
            }

            DataTable dtbResult = null;


            string strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tat) over_tat
from
(
select bc_patients.bc_bar_code,
bc_patients.bc_ori_id,
bc_patients.bc_date,
bc_patients.bc_lastaction_time,
bc_patients.bc_blood_date,
(case when isnull(bc_patients.bc_urgent_flag,'0')='1' then '急' else '' end) bc_urgent_flagStr,
bc_patients.bc_d_name,
bc_cname.bc_name as bc_com_name,
bc_patients.bc_name,
bc_patients.bc_age,
bc_patients.bc_sex,
bc_patients.bc_bed_no,
bc_patients.bc_in_no,
bc_patients.bc_upid,
bc_patients.bc_exp,
bc_patients.bc_ctype, --物理组 
bc_patients.bc_doct_name,
bc_patients.bc_occ_date,
bc_patients.bc_diag,
bc_patients.bc_times,
bc_patients.bc_status,
dict_type.type_name,
dict_cuvette.cuv_name,
dict_sample.sam_name,
isnull(bc_patients.bc_urgent_flag,cast(0 as bit)) as bc_urgent_flag,
dict_combine_timerule.com_time_type,
dict_combine_timerule.com_time as time_tat,
Datediff(mi,tpr_blood_date,getdate()) as time_mi
from tat_pro_record with(nolock) 
left join  bc_patients with(nolock) on bc_patients.bc_bar_code=tat_pro_record.tpr_bar_code
inner join bc_cname with(nolock) on bc_patients.bc_bar_no=bc_cname.bc_bar_no
inner join dict_combine_timerule_related on bc_cname.bc_lis_code=dict_combine_timerule_related.com_id
inner join dict_combine_timerule on  dict_combine_timerule_related.com_time_id=dict_combine_timerule.com_time_id
left join dict_type on bc_patients.bc_ctype=dict_type.type_id
left join dict_sample with(nolock) on bc_patients.bc_sam_id=dict_sample.sam_id
left join dict_cuvette with(nolock) on bc_patients.bc_cuv_code=dict_cuvette.cuv_code
left join bc_status on bc_patients.bc_status=bc_status.bc_name
where tat_pro_record.tpr_createdate>=getdate()-1
and bc_cname.bc_del='0' and bc_patients.bc_del='0'
and dict_combine_timerule.com_time>0
and (dict_combine_timerule.com_time_start_type='2' and dict_combine_timerule.com_time_end_type='5')
and bc_patients.bc_status=dict_combine_timerule.com_time_start_type
and (bc_patients.bc_ori_id=dict_combine_timerule.com_time_ori_id)
and bc_patients.bc_status in('2','3','4')
) as msg_tat
where  msg_tat.time_mi>msg_tat.time_tat";
            try
            {
                DataSet dsResult = new DataSet();
                DBHelper objHelper = new DBHelper();
                dsResult = objHelper.GetDataSet(strSQL);
                dtbResult = dsResult.Tables[0];
                dtbResult.TableName = "SamplToReceiveTATMsgCache";

                //if (dtbResult != null && dtbResult.Rows.Count > 0)
                //{
                //    //过滤
                //    DataView dvtemp = dtbResult.DefaultView.ToTable().DefaultView;
                //    dvtemp.RowFilter = "(bc_urgent_flag=1 and com_time_type='急查') or (bc_urgent_flag=0 and com_time_type='常规')";
                //    dtbResult = dvtemp.ToTable();
                //    dtbResult.TableName = "BcComTATMsgCache";
                //}

            }
            catch (Exception objEx)
            {
                //throw;
                Lib.LogManager.Logger.LogException("获取条码(采集到签收)TAT数据", objEx);
            }

            return dtbResult;
        }

        /// <summary>
        /// 根据筛选条件获取组合TAT数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetComTATMessage(string strWhere)
        {
            if (strWhere == null || string.IsNullOrEmpty(strWhere))
            {
                return this.cache;
            }
            else
            {
                try
                {
                    if (this.cache != null && this.cache.Rows.Count > 0)
                    {
                        DataTable dtCope = this.cache.Clone();
                        DataRow[] drArray = this.cache.Select(strWhere);

                        foreach (DataRow drItem in drArray)
                        {
                            dtCope.Rows.Add(drItem.ItemArray);
                        }
                        dtCope.TableName = "ComTATMsgCache";
                        return dtCope;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("获取缓存组合TAT数据", ex);
                }
                return this.cache;
            }
        }

        /// <summary>
        /// 根据筛选条件获取条码组合检验中TAT数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetBcComLabTATMessage(string strWhere)
        {
            if (strWhere == null || string.IsNullOrEmpty(strWhere))
            {
                return this.cacheBcLab;
            }
            else
            {
                try
                {
                    if (this.cacheBcLab != null && this.cacheBcLab.Rows.Count > 0)
                    {
                        DataTable dtCope = this.cacheBcLab.Clone();
                        DataRow[] drArray = this.cacheBcLab.Select(strWhere);

                        foreach (DataRow drItem in drArray)
                        {
                            dtCope.Rows.Add(drItem.ItemArray);
                        }
                        dtCope.TableName = "BcComLabTATMsgCache";
                        return dtCope;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("获取缓存条码组合检验中TAT数据", ex);
                }
                return this.cacheBcLab;
            }
        }

        /// <summary>
        /// 根据筛选条件获取条码组合TAT数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetBcComTATMessage(string strWhere)
        {
            if (strWhere == null || string.IsNullOrEmpty(strWhere))
            {
                return this.cacheBc;
            }
            else
            {
                try
                {
                    if (this.cacheBc != null && this.cacheBc.Rows.Count > 0)
                    {
                        DataTable dtCope = this.cacheBc.Clone();
                        DataRow[] drArray = this.cacheBc.Select(strWhere);

                        foreach (DataRow drItem in drArray)
                        {
                            dtCope.Rows.Add(drItem.ItemArray);
                        }
                        dtCope.TableName = "BcComTATMsgCache";
                        return dtCope;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("获取缓存条码组合TAT数据", ex);
                }
                return this.cacheBc;
            }
        }

        /// <summary>
        /// 根据筛选条件获取条码(采集到签收)TAT数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetBcBcSamplToReceiveTATMessage(string strWhere)
        {
            if (strWhere == null || string.IsNullOrEmpty(strWhere))
            {
                return this.cacheBcSamplToReceive;
            }
            else
            {
                try
                {
                    if (this.cacheBcSamplToReceive != null && this.cacheBcSamplToReceive.Rows.Count > 0)
                    {
                        DataTable dtCope = this.cacheBcSamplToReceive.Clone();
                        DataRow[] drArray = this.cacheBcSamplToReceive.Select(strWhere);

                        foreach (DataRow drItem in drArray)
                        {
                            dtCope.Rows.Add(drItem.ItemArray);
                        }
                        dtCope.TableName = "SamplToReceiveTATMsgCache";
                        return dtCope;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("获取缓存条码(采集到签收)TAT数据", ex);
                }
                return this.cacheBcSamplToReceive;
            }
        }
    }
}
