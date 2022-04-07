using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoCombineTATMsg))]
    public class DaoCombineTATMsg : IDaoCombineTATMsg
    {
        public List<EntityDicMsgTAT> GetBcComLabTAtMsgToCacheDao()
        {
            List<EntityDicMsgTAT> listLabMsgTAT = new List<EntityDicMsgTAT>();
            try
            {
                DBManager helper = new DBManager();

                #region 获取条码组合检验中TAT数据(仅取24小时内)
                string strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tat) over_tat
from
(
select Sample_main.Sma_bar_code,
Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_in_date,
Pat_lis_main.Pma_check_date,
(case when isnull(Sample_main.Sma_urgent_flag,'0')='1' then '急' else '' end) samp_urgent_flagStr,
Sample_main.Sma_pat_dept_name,
Sample_detail.Sdet_com_name as bc_com_name,
Sample_main.Sma_pat_name,
--Sample_main.bc_age,
dbo.getAge(Sample_main.Sma_pat_age) Sma_pat_age,--年龄需要转换
Sample_main.Sma_pat_sex,
Sample_main.Sma_pat_bed_no,
Sample_main.Sma_pat_in_no,
Sample_main.Sma_pat_unique_id,
Sample_main.Sma_type, --物理组 
Sample_main.Sma_doctor_name,
Sample_main.Sma_occ_date,
Sample_main.Sma_pat_diag,
Sample_main.Sma_pat_admiss_times,
Pat_lis_main.Pma_status,
Dict_itr_instrument.Ditr_ename,
Dict_profession.Dpro_name,
Dict_test_tube.Dtub_name,
Dict_sample.Dsam_name,
isnull(Sample_main.Sma_urgent_flag,cast(0 as bit)) as samp_urgent_flag,
Dict_itm_combine_timerule.Dtr_type,
Dict_itm_combine_timerule.Dtr_time as time_tat,
Datediff(mi,tpr_jy_date,getdate()) as time_mi
from tat_pro_record with(nolock) 
left join  Sample_main with(nolock) on Sample_main.Sma_bar_code=tat_pro_record.tpr_bar_code
inner join Sample_detail with(nolock) on Sample_main.Sma_bar_id=Sample_detail.Sdet_Sma_bar_id
inner join Pat_lis_main with(nolock) on Pat_lis_main.Pma_bar_code=Sample_main.Sma_bar_id
inner join Dict_itm_combine_timerule_related with(nolock) on Sample_detail.Sdet_com_id=Dict_itm_combine_timerule_related.Dtrr_Dcom_id
inner join Dict_itm_combine_timerule with(nolock) on Dict_itm_combine_timerule_related.Dtrr_Dtr_id=Dict_itm_combine_timerule.Dtr_id
left join Dict_profession with(nolock) on Sample_main.Sma_type=Dict_profession.Dpro_id
left join Dict_itr_instrument with(nolock) on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
left join Dict_sample with(nolock) on Sample_main.Sma_Dsam_id=Dict_sample.Dsam_id
left join Dict_test_tube with(nolock) on Sample_main.Sma_tub_code=Dict_test_tube.Dtub_code
where tat_pro_record.tpr_createdate>=getdate()-1
and Sample_detail.del_flag='0' and Sample_detail.Sdet_flag=1 and Sample_main.del_flag='0'
and tpr_jy_date is not null
and Dict_itm_combine_timerule.Dtr_time>0
and (Dict_itm_combine_timerule.Dtr_start_type='20' and Dict_itm_combine_timerule.Dtr_end_type='60')
and (Sample_main.Sma_pat_src_id=Dict_itm_combine_timerule.Dtr_Dsorc_id or Dict_itm_combine_timerule.Dtr_Dsorc_id is null or Dict_itm_combine_timerule.Dtr_Dsorc_id='')
and (Pat_lis_main.Pma_status=0 or Pat_lis_main.Pma_status=1)
) as msg_tat
where  msg_tat.time_mi>msg_tat.time_tat";
                #endregion

                DataTable dtLabMsgTAT = helper.ExecuteDtSql(strSQL);
                listLabMsgTAT = EntityManager<EntityDicMsgTAT>.ConvertToList(dtLabMsgTAT).OrderBy(i => i.SampBarCode).ToList();

                if (listLabMsgTAT.Count > 0)
                {
                    listLabMsgTAT = listLabMsgTAT.Where(w => (w.SampUrgentFlag == true && w.ComTimeType == "急查") || (w.SampUrgentFlag == false && w.ComTimeType == "常规")).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取条码组合检验中TAT数据", ex);
            }

            return listLabMsgTAT;
        }

        public List<EntityDicMsgTAT> GetBcComTAtMsgToCacheDao()
        {
            List<EntityDicMsgTAT> listMsgTAT = new List<EntityDicMsgTAT>();
            try
            {
                DBManager helper = new DBManager();

                //MyRegion 已注释

                #region SQL查询语句：获取条码组合TAT数据(仅取24小时内)
                string strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tat) over_tat
from
(
select Sample_main.Sma_bar_code,
Sample_main.Sma_pat_src_id,
Sample_main.Sma_date,
Sample_main.Sma_lastoper_date,
(case when isnull(Sample_main.Sma_urgent_flag,'0')='1' then '急' else '' end) samp_urgent_flagStr,
Sample_main.Sma_pat_dept_name,
Sample_detail.Sdet_com_name as bc_com_name,
Sample_main.Sma_pat_name,
--Sample_main.bc_age,
dbo.getAge(Sample_main.Sma_pat_age) pid_age,--年龄需要转换
Sample_main.Sma_pat_sex,
Sample_main.Sma_pat_bed_no,
Sample_main.Sma_pat_in_no,
Sample_main.Sma_pat_unique_id,
Sample_main.Sma_remark,
Sample_main.Sma_type, --物理组 
Sample_main.Sma_doctor_name,
Sample_main.Sma_occ_date,
Sample_main.Sma_pat_diag,
Sample_main.Sma_pat_admiss_times,
Sample_main.Sma_status_id,
Dict_profession.Dpro_name,
Dict_test_tube.Dtub_name,
Dict_sample.Dsam_name,
isnull(Sample_main.Sma_urgent_flag,cast(0 as bit)) as samp_urgent_flag,
Dict_itm_combine_timerule.Dtr_type,
Dict_itm_combine_timerule.Dtr_time as time_tat,
Datediff(mi,Sample_main.Sma_lastoper_date,getdate()) as time_mi,
((select Dict_status.Dsta_content from Dict_status where Dict_status.Dsta_name = Dict_itm_combine_timerule.Dtr_start_type)+' - '+
(select Dict_status.Dsta_content from Dict_status where Dict_status.Dsta_name = Dict_itm_combine_timerule.Dtr_end_type)) as status,
Pat_lis_main.Pma_sid,
Pat_lis_main.Pma_ctype,
(case when isnull(Pat_lis_main.Pma_ctype,'')='1' then '常规'
when isnull(Pat_lis_main.Pma_ctype,'')='2' then '急查'
when isnull(Pat_lis_main.Pma_ctype,'')='3' then '保密'
when isnull(Pat_lis_main.Pma_ctype,'')='4' then '溶栓' else '常规' end) as rep_ctype_name,
Dict_itr_instrument.Ditr_ename
from tat_pro_record with(nolock) 
left join  Sample_main with(nolock) on Sample_main.Sma_bar_code=tat_pro_record.tpr_bar_code
inner join Sample_detail with(nolock) on Sample_main.Sma_bar_id=Sample_detail.Sdet_Sma_bar_id
inner join Dict_itm_combine_timerule_related on Sample_detail.Sdet_com_id=Dict_itm_combine_timerule_related.Dtrr_Dcom_id
inner join Dict_itm_combine_timerule on  Dict_itm_combine_timerule_related.Dtrr_Dtr_id=Dict_itm_combine_timerule.Dtr_id
left join Dict_profession on Sample_main.Sma_type=Dict_profession.Dpro_id
left join Dict_sample with(nolock) on Sample_main.Sma_Dsam_id=Dict_sample.Dsam_id
left join Dict_test_tube with(nolock) on Sample_main.Sma_tub_code=Dict_test_tube.Dtub_code
left join Dict_status on Sample_main.Sma_status_id=Dict_status.Dsta_name
left join Pat_lis_main with(nolock) on Sample_main.Sma_bar_code=Pat_lis_main.Pma_bar_code 
left join Dict_itr_instrument with(nolock) on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
where tat_pro_record.tpr_createdate>=getdate()-1
and Sample_detail.del_flag='0' and (Sample_detail.Sdet_flag<>1 or Sample_detail.Sdet_flag is null) and Sample_main.del_flag='0'
and Dict_itm_combine_timerule.Dtr_time>0
and ((Dict_itm_combine_timerule.Dtr_start_type='2'and Dict_itm_combine_timerule.Dtr_end_type='3')
or (Dict_itm_combine_timerule.Dtr_start_type='3'and Dict_itm_combine_timerule.Dtr_end_type='4')
or (Dict_itm_combine_timerule.Dtr_start_type='4'and Dict_itm_combine_timerule.Dtr_end_type='5')
or (Dict_itm_combine_timerule.Dtr_start_type='5'and Dict_itm_combine_timerule.Dtr_end_type='20')
or (Dict_itm_combine_timerule.Dtr_start_type='8'and Dict_itm_combine_timerule.Dtr_end_type='20'))
and Sample_main.Sma_status_id=Dict_itm_combine_timerule.Dtr_start_type
and (Sample_main.Sma_pat_src_id=Dict_itm_combine_timerule.Dtr_Dsorc_id or Dict_itm_combine_timerule.Dtr_Dsorc_id is null or Dict_itm_combine_timerule.Dtr_Dsorc_id='')
) as msg_tat
where  msg_tat.time_mi>msg_tat.time_tat ";
                #endregion

                DataTable dtMsgTAT = helper.ExecuteDtSql(strSQL);

                listMsgTAT = EntityManager<EntityDicMsgTAT>.ConvertToList(dtMsgTAT).OrderBy(i => i.SampBarCode).ToList();

                if (listMsgTAT.Count > 0)
                {
                    listMsgTAT = listMsgTAT.Where(w => (w.SampUrgentFlag == true && w.ComTimeType == "急查") || (w.SampUrgentFlag == false && w.ComTimeType == "常规")).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取条码组合TAT数据", ex);
            }

            return listMsgTAT;
        }

        public List<EntityDicMsgTAT> GetCombineTATmsgToCacheDao(bool isOutLink)
        {
            List<EntityDicMsgTAT> listComTATMsg = new List<EntityDicMsgTAT>();
            try
            {
                DBManager helper = new DBManager();

                #region SQL语句:获取组合TAT
                string strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tatw) as time_mi_over,
(select isnull(max(Dict_itm_combine_timerule.Dtr_time),0)
from Sample_detail with(nolock)
inner join Dict_itm_combine_timerule_related on Sample_detail.Sdet_com_id=Dict_itm_combine_timerule_related.Dtrr_Dcom_id
inner join Dict_itm_combine_timerule on Dict_itm_combine_timerule_related.Dtrr_Dtr_id=Dict_itm_combine_timerule.Dtr_id
where 
Sample_detail.del_flag='0'
and Dict_itm_combine_timerule.Dtr_start_type='5'
and Dict_itm_combine_timerule.Dtr_end_type='60'
and Dict_itm_combine_timerule.Dtr_time>0
and Dict_itm_combine_timerule.Dtr_Dsorc_id=msg_tat.Sma_pat_src_id
and Sample_detail.Sdet_bar_code=msg_tat.Sma_bar_code) max_tat,
(case when msg_tat.time_mi>msg_tat.time_tat then 1 else 0 end) over_tat
from
(
select Sample_main.Sma_bar_code,
Sample_main.Sma_receiver_date,
Sample_main.Sma_com_name as bc_com_name,
Sample_main.Sma_pat_name,
Sample_main.Sma_pat_bed_no,
Sample_main.Sma_pat_in_no,
Sample_main.Sma_pat_unique_id,
Sample_main.Sma_pat_src_id,
Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_sid,
Pat_lis_main.Pma_serial_num,
Pat_lis_main.Pma_in_date,
Pat_lis_main.Pma_Ditr_id,
Dict_itr_instrument.Ditr_ename,
Sample_main.Sma_type as itr_type, --物理组 
isnull(Sample_main.Sma_urgent_flag,cast(0 as bit)) as samp_urgent_flag,
Dict_itm_combine_timerule.Dtr_type,
Dict_profession.Dpro_name,
Dict_itm_combine_timerule.Dtr_time as time_tat,
Dict_itm_combine_timerule.Dtr_time*0.7 as time_tatw,
Datediff(mi,tpr_receiver_date,getdate()) as time_mi,
(case when Sample_main.Sma_merge_com_id is not null and Sample_main.Sma_merge_com_id<>'' then isnull((select top 1 1 from Sample_main as z_bc with(nolock) inner join Pat_lis_main z_pat with(nolock) on z_pat.Pma_bar_code=z_bc.Sma_bar_code where z_bc.Sma_merge_com_id=Sample_main.Sma_merge_com_id and z_bc.Sma_bar_code<>Sample_main.Sma_bar_code),0) else 0 end) as merge_flag,
((select Dict_status.Dsta_content from Dict_status where Dict_status.Dsta_name = Dict_itm_combine_timerule.Dtr_start_type)+' - '+
(select Dict_status.Dsta_content from Dict_status where Dict_status.Dsta_name = Dict_itm_combine_timerule.Dtr_end_type)) as status,
(select count(Pat_lis_detail.Pdet_id) from Pat_lis_detail with(nolock) where Pat_lis_detail.Pdet_id=Pat_lis_main.Pma_rep_id and Pat_lis_detail.Pdet_Dcom_id=Sample_detail.Sdet_com_id) as pmi_c
from tat_pro_record with(nolock) 
left join  Sample_main with(nolock) on Sample_main.Sma_bar_code=tat_pro_record.tpr_bar_code
left join Pat_lis_main with(nolock) on Sample_main.Sma_bar_code=Pat_lis_main.Pma_bar_code
inner join Sample_detail with(nolock) on Sample_main.Sma_bar_id=Sample_detail.Sdet_Sma_bar_id
inner join Dict_itm_combine_timerule_related on Sample_detail.Sdet_com_id=Dict_itm_combine_timerule_related.Dtrr_Dcom_id
inner join Dict_itm_combine_timerule on Dict_itm_combine_timerule_related.Dtrr_Dtr_id=Dict_itm_combine_timerule.Dtr_id
left join Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
left join Dict_profession on Sample_main.Sma_type=Dict_profession.Dpro_id
where tat_pro_record.tpr_createdate>=(getdate()-1)
and Sample_detail.del_flag='0' 
and tpr_receiver_date is not null
and Dict_itm_combine_timerule.Dtr_start_type='5'
and Dict_itm_combine_timerule.Dtr_end_type='60'
and Dict_itm_combine_timerule.Dtr_time>0
and Sample_main.Sma_pat_src_id=Dict_itm_combine_timerule.Dtr_Dsorc_id
and (Pat_lis_main.Pma_status is null or Pat_lis_main.Pma_status=0 or Pat_lis_main.Pma_status=1)
and not exists(select top 1 1 from Lis_result with(nolock) where Lis_result.Lres_Pma_rep_id=Pat_lis_main.Pma_rep_id and Lres_recheck_flag=1 and Lis_result.Lres_Pma_rep_id is not null)
and (Pat_lis_main.Pma_recheck_flag is null or Pat_lis_main.Pma_recheck_flag<>1)
and not exists(select top 1 1 from Sample_process_detial with(nolock) where Sample_process_detial.Sproc_Sma_bar_code=Sample_main.Sma_bar_code and Sample_process_detial.Sproc_status='560')
) as msg_tat
where msg_tat.time_mi>msg_tat.time_tatw and (Pma_rep_id is null or pmi_c>0)";
                #endregion

                //佛山市一,来源为空时,忽略来源过滤
                if (isOutLink)
                {
                    #region SQL语句:流程从送达开始监控

                    strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tatw) as time_mi_over,
(select isnull(max(Dict_itm_combine_timerule.Dtr_time),0)
from Sample_detail with(nolock)
inner join Dict_itm_combine_timerule_related with(nolock) on Sample_detail.Sdet_com_id=Dict_itm_combine_timerule_related.Dtrr_Dcom_id
inner join Dict_itm_combine_timerule with(nolock) on Dict_itm_combine_timerule_related.Dtrr_Dtr_id=Dict_itm_combine_timerule.Dtr_id
where 
Sample_detail.del_flag='0'
and Dict_itm_combine_timerule.Dtr_start_type='4'
and Dict_itm_combine_timerule.Dtr_end_type='60'
and Dict_itm_combine_timerule.Dtr_time>0
and (Dict_itm_combine_timerule.Dtr_Dsorc_id=msg_tat.Sma_pat_src_id or Dict_itm_combine_timerule.Dtr_Dsorc_id is null or Dict_itm_combine_timerule.Dtr_Dsorc_id='')
and Sample_detail.Sdet_bar_code=msg_tat.Sma_bar_code) max_tat,
(case when msg_tat.time_mi>msg_tat.time_tat then 1 else 0 end) over_tat

from
(
select Sample_main.Sma_bar_code,
Sample_main.Sma_receiver_date,
Sample_main.Sma_com_name as bc_com_name,
Sample_main.Sma_pat_name,
Sample_main.Sma_pat_bed_no,
Sample_main.Sma_pat_in_no,
Sample_main.Sma_pat_unique_id,
Sample_main.Sma_pat_src_id,
Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_sid,
Pat_lis_main.Pma_serial_num,
Pat_lis_main.Pma_in_date,
Pat_lis_main.Pma_Ditr_id,
Dict_itr_instrument.Ditr_ename,
Sample_main.Sma_type as itr_type, --物理组 
isnull(Sample_main.Sma_urgent_flag,cast(0 as bit)) as samp_urgent_flag,
Dict_itm_combine_timerule.Dtr_type,
Dict_profession.Dpro_name,
Dict_itm_combine_timerule.Dtr_time as time_tat,
Dict_itm_combine_timerule.Dtr_time*0.7 as time_tatw,
Datediff(mi,tpr_reach_date,getdate()) as time_mi,
0 as merge_flag,
((select top 1 Dict_status.Dsta_content from Dict_status where Dict_status.Dsta_name = Dict_itm_combine_timerule.Dtr_start_type)+' - '+
(select top 1 Dict_status.Dsta_content from Dict_status where Dict_status.Dsta_name = Dict_itm_combine_timerule.Dtr_end_type)) as status,
(select count(Pat_lis_detail.Pdet_id) from Pat_lis_detail with(nolock) where Pat_lis_detail.Pdet_id=Pat_lis_main.Pma_rep_id and Pat_lis_detail.Pdet_Dcom_id=Sample_detail.Sdet_com_id) as pmi_c
from tat_pro_record with(nolock) 
left join  Sample_main with(nolock) on Sample_main.Sma_bar_code=tat_pro_record.tpr_bar_code
left join Pat_lis_main with(nolock) on Sample_main.Sma_bar_code=Pat_lis_main.Pma_bar_code
inner join Sample_detail with(nolock) on Sample_main.Sma_bar_code=Sample_detail.Sdet_Sma_bar_id
inner join Dict_itm_combine_timerule_related with(nolock) on Sample_detail.Sdet_com_id=Dict_itm_combine_timerule_related.Dtrr_Dcom_id
inner join Dict_itm_combine_timerule with(nolock) on Dict_itm_combine_timerule_related.Dtrr_Dtr_id=Dict_itm_combine_timerule.Dtr_id
left join Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
left join Dict_profession on Sample_main.Sma_type=Dict_profession.Dpro_id
where tat_pro_record.tpr_createdate>=(getdate()-1)
and Sample_detail.Sdet_flag='0' 
and tpr_reach_date is not null
and Dict_itm_combine_timerule.Dtr_start_type='4'
and Dict_itm_combine_timerule.Dtr_end_type='60'
and Dict_itm_combine_timerule.Dtr_time>0
and (Sample_main.Sma_pat_src_id=Dict_itm_combine_timerule.Dtr_Dsorc_id or Dict_itm_combine_timerule.Dtr_Dsorc_id is null or Dict_itm_combine_timerule.Dtr_Dsorc_id='') 
and (Pat_lis_main.Pma_status is null or Pat_lis_main.Pma_status=0 or Pat_lis_main.Pma_status=1)
) as msg_tat
where msg_tat.time_mi>msg_tat.time_tatw and (Pma_rep_id is null or pmi_c>0)";

                    #endregion
                }

                DataTable dtComTATMsg = helper.ExecuteDtSql(strSQL);
                listComTATMsg = EntityManager<EntityDicMsgTAT>.ConvertToList(dtComTATMsg).OrderBy(i => i.SampBarCode).ToList();

                //过滤
                if (listComTATMsg != null && listComTATMsg.Count > 0)
                {
                    //用每条条码最大值进行过滤
                    listComTATMsg = listComTATMsg.Where(w => w.TimeMi > (w.MaxTat * 0.7) &&
                                                            w.MergeFlag == 0 &&
                                                           ((w.SampUrgentFlag == true && w.ComTimeType == "急查") || (w.SampUrgentFlag == false && w.ComTimeType == "常规"))
                                                          ).ToList();
                }
                List<string> notebarcode = new List<string>();//记录条码
                //每条条码只取一次
                if (listComTATMsg != null && listComTATMsg.Count > 0)
                {
                    //DataTable dtclone = dtbResult.Clone();
                    List<EntityDicMsgTAT> listClone = new List<EntityDicMsgTAT>();
                    foreach (var infoTemp in listComTATMsg)
                    {
                        if (!string.IsNullOrEmpty(infoTemp.SampBarCode)
                            && !notebarcode.Contains(infoTemp.SampBarCode))
                        {
                            notebarcode.Add(infoTemp.SampBarCode);
                            infoTemp.TimeTat = infoTemp.MaxTat;
                            infoTemp.TimeTatw = Convert.ToDecimal(infoTemp.MaxTat * 0.7);
                            infoTemp.TimeMiOver = Convert.ToDecimal(Convert.ToInt32(infoTemp.TimeMi) - Convert.ToInt32(infoTemp.MaxTat));
                            infoTemp.OverTat = Convert.ToInt32(infoTemp.TimeMi) > Convert.ToInt32(infoTemp.MaxTat) ? 1 : 0;

                            listClone.Add(infoTemp);
                        }
                    }
                    listComTATMsg = listClone;
                }
            }
            catch (Exception objEx)
            {
                Lib.LogManager.Logger.LogException("获取组合TAT数据", objEx);
            }

            return listComTATMsg;
        }

        public List<EntityDicMsgTAT> GetBcSamplToReceiveTAtMsgToCacheDao()
        {
            List<EntityDicMsgTAT> listBcSamToReceive = new List<EntityDicMsgTAT>();
            try
            {
                DBManager helper = new DBManager();

                #region SQL语句:获取条码(采集到签收)TAT数据(仅取24小时内)
                string strSQL = @"select msg_tat.*,
(msg_tat.time_mi-msg_tat.time_tat) over_tat
from
(
select Sample_main.Sma_bar_code,
Sample_main.Sma_pat_src_id,
Sample_main.Sma_date,
Sample_main.Sma_lastoper_date,
Sample_main.Sma_collection_date,
(case when isnull(Sample_main.Sma_urgent_flag,'0')='1' then '急' else '' end) samp_urgent_flagStr,
Sample_main.Sma_pat_dept_name,
Sample_detail.Sdet_com_name as bc_com_name,
Sample_main.Sma_pat_name,
--Sample_main.bc_age,
dbo.getAge(Sample_main.Sma_pat_age) pid_age,--年龄需要转换
Sample_main.Sma_pat_sex,
Sample_main.Sma_pat_bed_no,
Sample_main.Sma_pat_in_no,
Sample_main.Sma_pat_unique_id,
Sample_main.Sma_remark,
Sample_main.Sma_type, --物理组 
Sample_main.Sma_doctor_name,
Sample_main.Sma_occ_date,
Sample_main.Sma_pat_diag,
Sample_main.Sma_pat_admiss_times,
Sample_main.Sma_status_id,
Dict_profession.Dpro_name,
Dict_test_tube.Dtub_name,
Dict_sample.Dsam_name,
isnull(Sample_main.Sma_urgent_flag,cast(0 as bit)) as samp_urgent_flag,
Dict_itm_combine_timerule.Dtr_type,
Dict_itm_combine_timerule.Dtr_time as time_tat,
Datediff(mi,tpr_blood_date,getdate()) as time_mi
from tat_pro_record with(nolock) 
left join  Sample_main with(nolock) on Sample_main.Sma_bar_code=tat_pro_record.tpr_bar_code
inner join Sample_detail with(nolock) on Sample_main.Sma_bar_id=Sample_detail.Sdet_Sma_bar_id
inner join Dict_itm_combine_timerule_related on Sample_detail.Sdet_com_id=Dict_itm_combine_timerule_related.Dtrr_Dcom_id
inner join Dict_itm_combine_timerule on  Dict_itm_combine_timerule_related.Dtrr_Dtr_id=Dict_itm_combine_timerule.Dtr_id
left join Dict_profession on Sample_main.Sma_type=Dict_profession.Dpro_id
left join Dict_sample with(nolock) on Sample_main.Sma_Dsam_id=Dict_sample.Dsam_id
left join Dict_test_tube with(nolock) on Sample_main.Sma_tub_code=Dict_test_tube.Dtub_code
left join Dict_status on Sample_main.Sma_status_id=Dict_status.Dsta_name
where tat_pro_record.tpr_createdate>=getdate()-1
and Sample_detail.del_flag='0' and Sample_main.del_flag='0'
and Dict_itm_combine_timerule.Dtr_time>0
and (Dict_itm_combine_timerule.Dtr_start_type='2' and Dict_itm_combine_timerule.Dtr_end_type='5')
and Sample_main.Sma_status_id=Dict_itm_combine_timerule.Dtr_start_type
and (Sample_main.Sma_pat_src_id=Dict_itm_combine_timerule.Dtr_Dsorc_id)
and Sample_main.Sma_status_id in('2','3','4')
) as msg_tat
where  msg_tat.time_mi>msg_tat.time_tat";
                #endregion

                DataTable dtBcSamToReceive = helper.ExecuteDtSql(strSQL);
                listBcSamToReceive = EntityManager<EntityDicMsgTAT>.ConvertToList(dtBcSamToReceive).OrderBy(i => i.SampBarCode).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取条码(采集到签收)TAT数据", ex);
            }

            return listBcSamToReceive;
        }

    }
}
