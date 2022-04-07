using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDicCombine))]
    public class DaoDicCombine : IDaoDicCombine
    {
        public bool Save(EntityDicCombine com)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_itm_combine");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dcom_id", id);
                values.Add("Dcom_name", com.ComName);
                values.Add("Dcom_lab_id", com.ComLabId);
                values.Add("Dcom_price", com.ComPrice);
                values.Add("Dcom_cost", com.ComCost);
                values.Add("Dcom_item_amount", com.ComItemAmount);
                values.Add("Dcom_remark", com.ComRemark);
                values.Add("Dcom_code", com.ComCode);
                values.Add("Dcom_Dpro_id", com.ComPriId);
                values.Add("Dcom_Dsam_id", com.ComSamId);
                values.Add("Dcom_Ditr_id", com.ComItrId);
                values.Add("Dcom_c_code", com.ComCCode);
                values.Add("Dcom_report_time", com.ComReportTime);
                values.Add("Dcom_time_unit", com.ComTimeUnit);
                values.Add("py_code", com.ComPyCode);
                values.Add("wb_code", com.ComWbCode);
                values.Add("del_flag", com.ComDelFlag);
                values.Add("Dcom_his_code", com.ComHisCode);
                values.Add("Dcom_split_flag", com.ComSplitFlag);
                values.Add("Dcom_get_report_time", com.ComGetReportTime);
                values.Add("Dcom_urgent_report_time", com.ComUrgentReportTime);
                values.Add("Dcom_urgent_blood_send_times", com.ComUrgentBloodSendTimes);
                values.Add("Dcom_routine_blood_send_times", com.ComRoutineBloodSendTimes);
                values.Add("Dcom_qc_flag", com.ComQcFlag);
                values.Add("Dcom_laboratory_code", com.ComLaboratoryCode);
                values.Add("Dcom_urgent_flag", com.ComUrgentFlag);
                values.Add("Dcom_sme_flag", com.ComSmeFlag);
                values.Add("Dcom_online_status", com.ComOnlineStatus);
                values.Add("Dcom_online_clolr", com.ComOnlineClolr);
                values.Add("sort_no", com.ComSortNo);
                values.Add("Dcom_classify", com.ComClassify);
                values.Add("Dcom_mt_tpe", com.ComMtTpe);
                values.Add("Dcom_rsl_flag", com.ComRslFlag);
                values.Add("Dcom_rpt_type", com.ComRptType);
                values.Add("Dcom_rpt_cname", com.ComRptCname);
                values.Add("Dcom_sam_notice", com.ComSamNotice1);
                values.Add("Dcom_content", com.ComContent);
                values.Add("Dcom_collection_notice", com.ComCollectionNotice);
                values.Add("Dcom_save_notice", com.ComSaveNotice1);
                values.Add("Dcom_influence", com.ComInfluence);

                helper.InsertOperation("Dict_itm_combine", values);
                com.ComId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicCombine com)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dcom_id", com.ComId);
                values.Add("Dcom_name", com.ComName);
                values.Add("Dcom_lab_id", com.ComLabId);
                values.Add("Dcom_price", com.ComPrice);
                values.Add("Dcom_cost", com.ComCost);
                values.Add("Dcom_item_amount", com.ComItemAmount);
                values.Add("Dcom_remark", com.ComRemark);
                values.Add("Dcom_code", com.ComCode);
                values.Add("Dcom_Dpro_id", com.ComPriId);
                values.Add("Dcom_Dsam_id", com.ComSamId);
                values.Add("Dcom_Ditr_id", com.ComItrId);
                values.Add("Dcom_c_code", com.ComCCode);
                values.Add("Dcom_report_time", com.ComReportTime);
                values.Add("Dcom_time_unit", com.ComTimeUnit);
                values.Add("py_code", com.ComPyCode);
                values.Add("wb_code", com.ComWbCode);
                values.Add("del_flag", com.ComDelFlag);
                values.Add("Dcom_his_code", com.ComHisCode);
                values.Add("Dcom_split_flag", com.ComSplitFlag);
                values.Add("Dcom_get_report_time", com.ComGetReportTime);
                values.Add("Dcom_urgent_report_time", com.ComUrgentReportTime);
                values.Add("Dcom_urgent_blood_send_times", com.ComUrgentBloodSendTimes);
                values.Add("Dcom_routine_blood_send_times", com.ComRoutineBloodSendTimes);
                values.Add("Dcom_qc_flag", com.ComQcFlag);
                values.Add("Dcom_laboratory_code", com.ComLaboratoryCode);
                values.Add("Dcom_urgent_flag", com.ComUrgentFlag);
                values.Add("Dcom_sme_flag", com.ComSmeFlag);
                values.Add("Dcom_online_status", com.ComOnlineStatus);
                values.Add("Dcom_online_clolr", com.ComOnlineClolr);
                values.Add("sort_no", com.ComSortNo);
                values.Add("Dcom_classify", com.ComClassify);
                values.Add("Dcom_mt_tpe", com.ComMtTpe);
                values.Add("Dcom_rsl_flag", com.ComRslFlag);
                values.Add("Dcom_rpt_type", com.ComRptType);
                values.Add("Dcom_rpt_cname", com.ComRptCname);
                values.Add("Dcom_sam_notice", com.ComSamNotice1);
                values.Add("Dcom_content", com.ComContent);
                values.Add("Dcom_collection_notice", com.ComCollectionNotice);
                values.Add("Dcom_save_notice", com.ComSaveNotice1);
                values.Add("Dcom_influence", com.ComInfluence);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dcom_id", com.ComId);

                helper.UpdateOperation("Dict_itm_combine", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicCombine com)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dcom_id", com.ComId);

                helper.UpdateOperation("Dict_itm_combine", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicCombine> Search(string hosId)
        {
            try
            {
                String sql = @"SELECT 
Dcom_id, Dcom_name, Dcom_lab_id, Dcom_price, Dcom_cost,0 sp_select,Dcom_sam_notice,Dcom_content,Dcom_collection_notice,Dcom_save_notice,Dcom_influence,
Dcom_item_amount, Dcom_remark, Dcom_code, Dcom_Dpro_id, Dcom_Dsam_id, Dcom_Ditr_id, 
Dict_itm_combine.Dcom_c_code, Dcom_report_time, Dict_itm_combine.py_code, Dict_itm_combine.wb_code,cast(Dict_itm_combine.sort_no as varchar(10)) sort_no, Dict_itm_combine.del_flag, 
Dcom_time_unit, com_his_name, Dcom_his_code, Dcom_split_flag, 
Dcom_get_report_time, Dcom_urgent_report_time, Dcom_urgent_blood_send_times,
Dcom_routine_blood_send_times, Dcom_qc_flag,Dcom_urgent_flag,Dcom_sme_flag, Dcom_laboratory_code,
Dict_sample.Dsam_name, dic_pub_profession_1.Dpro_name AS ctype_name, Dict_sample.Dsam_id,
Dict_profession.Dpro_name AS ptype_name, Dict_itr_instrument.Ditr_name,
isnull((select top 1 '0' from Rel_sample_merge_rule where Rel_sample_merge_rule.Rsmr_Dcom_id=Dict_itm_combine.Dcom_id),'1') bar_type
,isnull((select top 1 '1' from Rel_sample_merge_rule where Rel_sample_merge_rule.Rsmr_Dcom_id=Dict_itm_combine.Dcom_id
and Rel_sample_merge_rule.Rsmr_split_flag=1),'0') bar_split,Dcom_online_status,Dcom_online_clolr,Dcom_class,Dcom_classify,Dcom_mt_tpe,Dcom_rsl_flag,Dcom_rpt_type,Dcom_rpt_cname
FROM Dict_itm_combine 
LEFT OUTER JOIN Dict_profession ON Dict_itm_combine.Dcom_Dpro_id = Dict_profession.Dpro_id  
LEFT OUTER JOIN Dict_profession AS dic_pub_profession_1 ON Dict_itm_combine.Dcom_lab_id = dic_pub_profession_1.Dpro_id  
LEFT OUTER JOIN Dict_itr_instrument ON Dict_itm_combine.Dcom_Ditr_id = Dict_itr_instrument.Ditr_id  
LEFT OUTER JOIN Dict_sample ON Dict_itm_combine.Dcom_Dsam_id = Dict_sample.Dsam_id 
WHERE Dict_itm_combine.del_flag = '0'  ";

                //院区id
                if (hosId != null && !string.IsNullOrEmpty(hosId))
                {
                    sql += string.Format("and Dict_profession.Dpro_Dorg_id='{0}'", hosId);
                }
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                return EntityManager<EntityDicCombine>.ConvertToList(dt).OrderBy(i => i.ComId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicCombine>();
            }
        }

        public List<EntityDicCombine> ConvertToCombine(DataTable dtSour)
        {
            List<EntityDicCombine> list = new List<EntityDicCombine>();
            foreach (DataRow item in dtSour.Rows)
            {

                EntityDicCombine com = new EntityDicCombine();
                com.ComId = item["Dcom_id"].ToString();
                com.ComName = item["Dcom_name"].ToString();
                com.ComLabId = item["Dcom_lab_id"].ToString();
                decimal comprice = 0.00m;
                decimal comcost = 0.00m;
                if (item["Dcom_price"] != DBNull.Value && item["Dcom_price"] != null)
                    decimal.TryParse(item["Dcom_price"].ToString(), out comprice);
                com.ComPrice = comprice;
                if (item["Dcom_cost"] != DBNull.Value)
                    decimal.TryParse(item["Dcom_cost"].ToString(), out comcost);
                com.ComCost = comcost;
                com.ComRemark = item["Dcom_remark"].ToString();
                com.ComCode = item["Dcom_code"].ToString();
                com.ComPriId = item["Dcom_Dpro_id"].ToString();
                com.ComSamId = item["Dcom_Dsam_id"].ToString();
                com.ComItrId = item["Dcom_Ditr_id"].ToString();
                com.ComCCode = item["Dcom_c_code"].ToString();
                com.ComTimeUnit = item["Dcom_time_unit"].ToString();
                com.ComPyCode = item["py_code"].ToString();
                com.ComWbCode = item["wb_code"].ToString();
                com.ComDelFlag = item["del_flag"].ToString();
                com.ComHisCode = item["Dcom_his_code"].ToString();
                com.ComGetReportTime = item["Dcom_get_report_time"].ToString();
                com.ComUrgentReportTime = item["Dcom_urgent_report_time"].ToString();
                com.ComUrgentBloodSendTimes = item["Dcom_urgent_blood_send_times"].ToString();
                com.ComRoutineBloodSendTimes = item["Dcom_routine_blood_send_times"].ToString();
                com.ComLaboratoryCode = item["Dcom_laboratory_code"].ToString();
                com.ComOnlineStatus = item["Dcom_online_status"].ToString();
                com.ComSamNotice1 = item["Dcom_sam_notice"].ToString();
                com.ComContent = item["Dcom_content"].ToString();
                com.ComCollectionNotice = item["Dcom_collection_notice"].ToString();
                com.ComSaveNotice1 = item["Dcom_save_notice"].ToString();
                com.ComInfluence = item["Dcom_influence"].ToString();
                int sort = 0, count = 0, time = 0, splitflag = 0, qcflag = 0, urgentflag = 0, linecolor = 0,smeflag = 0;
                if (item["sort_no"] != null && item["sort_no"] != DBNull.Value)
                    int.TryParse(item["sort_no"].ToString(), out sort);
                com.ComSortNo = sort;
                if (item["Dcom_item_amount"] != null && item["Dcom_item_amount"] != DBNull.Value)
                    int.TryParse(item["Dcom_item_amount"].ToString(), out count);
                com.ComItemAmount = count;
                if (item["Dcom_report_time"] != null && item["Dcom_report_time"] != DBNull.Value)
                    int.TryParse(item["Dcom_report_time"].ToString(), out time);
                com.ComReportTime = time;
                if (item["Dcom_split_flag"] != null && item["Dcom_split_flag"] != DBNull.Value)
                    int.TryParse(item["Dcom_split_flag"].ToString(), out splitflag);
                com.ComSplitFlag = splitflag;
                if (item["Dcom_qc_flag"] != null && item["Dcom_qc_flag"] != DBNull.Value)
                    int.TryParse(item["Dcom_qc_flag"].ToString(), out qcflag);
                com.ComSplitFlag = qcflag;
                if (item["Dcom_urgent_flag"] != null && item["Dcom_urgent_flag"] != DBNull.Value)
                    int.TryParse(item["Dcom_urgent_flag"].ToString(), out urgentflag);
                com.ComUrgentFlag = urgentflag;
                if (item["Dcom_sme_flag"] != null && item["Dcom_sme_flag"] != DBNull.Value)
                    int.TryParse(item["Dcom_sme_flag"].ToString(), out smeflag);
                com.ComSmeFlag = smeflag;
                if (item["Dcom_online_clolr"] != null && item["Dcom_online_clolr"] != DBNull.Value)
                    int.TryParse(item["Dcom_online_clolr"].ToString(), out linecolor);
                if (item.Table.Columns.Contains("ctype_name") && item["ctype_name"] != null && item["ctype_name"] != DBNull.Value)
                {
                    com.CTypeName = item["ctype_name"].ToString();
                }
                list.Add(com);
            }
            return list.OrderBy(i => i.ComSortNo).ToList();
        }

        public List<EntityDicCombine> ConvertToNobactCombine(DataTable dtsour)
        {
            List<EntityDicCombine> list = new List<EntityDicCombine>();
            foreach (DataRow item in dtsour.Rows)
            {
                EntityDicCombine combine = new EntityDicCombine();
                combine.Checked = (bool)item["sp_select"];
                combine.ComId = item["Dcom_id"].ToString();
                combine.ComName = item["Dcom_name"].ToString();
                list.Add(combine);
            }
            return list;
        }

        /// <summary>
        /// 获取无菌涂片组合
        /// </summary>
        /// <returns></returns>
        public List<EntityDicCombine> GetNoBactCombine()
        {
            List<EntityDicCombine> list = new List<EntityDicCombine>();
            try
            {
               string  sql = @"select distinct cast(0 as bit) as sp_select,Dict_itm_combine.Dcom_id,Dcom_name from Dict_itr_instrument with(nolock)
inner join Rel_itr_combine with(nolock) on Dict_itr_instrument.Ditr_id=Rel_itr_combine.Ric_Ditr_id
inner join Dict_itm_combine with(nolock) on Dict_itm_combine.Dcom_id=Rel_itr_combine.Ric_Dcom_id and Dict_itm_combine.del_flag='0'
where Ditr_report_type='3' and Dict_itr_instrument.del_flag='0'";
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                if (dt != null && dt.Rows.Count > 0)
                    list = ConvertToNobactCombine(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }
        /// <summary>
        /// 获取大小组合
        /// </summary>
        /// <returns></returns>
        public List<EntityDicCombine> GetCombineSplit()
        {
            List<EntityDicCombine> list = new List<EntityDicCombine>();
            try
            {
               string  sql = @"SELECT  Dict_itm_combine.*, Dict_profession.Dpro_name AS ctype_name     
FROM Dict_itm_combine 
LEFT OUTER JOIN Dict_profession ON Dict_itm_combine.Dcom_Dpro_id = Dict_profession.Dpro_id      
WHERE (Dict_itm_combine.del_flag = '0')
and Dict_itm_combine.Dcom_id in
(select Rsmr_Dcom_id from Rel_sample_merge_rule where Rsmr_split_flag='1' group by Rsmr_Dcom_id)";
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                if (dt != null && dt.Rows.Count > 0) 
                    list = ConvertToCombine(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }
    }
}
