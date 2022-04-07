using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSampMergeRule))]
    public class DaoSampMergeRule : IDaoSampMergeRule
    {
        public bool Delete(EntitySampMergeRule mergeRule)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Rsmr_id", mergeRule.ComRulId);

                helper.UpdateOperation("Rel_sample_merge_rule", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySampMergeRule> GetAllCombineSplitBarCode()
        {
            List<EntitySampMergeRule> listRule = new List<EntitySampMergeRule>();
            try
            {
                DBManager helper = new DBManager();
                string sql = @"SELECT     
dc.Dcom_id as Rsmr_Dcom_id , 
dc.Dcom_name ,
dc.com_his_name as Rsmr_com_his_name , 
dcb.Rsmr_his_fee_code, 
ISNULL(dcb.Rsmr_print_name, dc.Dcom_name) Rsmr_print_name,
dcb.Rsmr_split_code, 
Dict_profession.Dpro_name, 
Dict_test_tube.Dtub_name, 
Dict_sample.Dsam_name, 
dcb.sort_no, 
dcb.Rsmr_sam_notice, 
dcb.Rsmr_save_notice, 
dcb.Rsmr_split_flag, 
ori.Dsorc_name, 
dcb.Rsmr_Dtub_code,
dc.Dcom_report_time,
dcb.Rsmr_Dsam_id
FROM  Dict_itm_combine AS dc 
inner JOIN Rel_sample_merge_rule AS dcb ON dc.Dcom_id = dcb.Rsmr_Dcom_id 
LEFT OUTER JOIN Dict_sample ON Dict_sample.Dsam_id = dcb.Rsmr_Dsam_id 
LEFT OUTER JOIN Dict_profession ON dcb.Rsmr_exec_Dpro_id = Dict_profession.Dpro_id 
LEFT OUTER JOIN Dict_test_tube ON Dict_test_tube.Dtub_code = dcb.Rsmr_Dtub_code 
left join Dict_source as ori  on ori.Dsorc_id = dcb.Rsmr_Dsorc_id 
WHERE (dc.del_flag <> '1') and (dcb.del_flag is null or dcb.del_flag = '0' or dcb.del_flag = '')";

                DataTable dt = helper.ExecuteDtSql(sql);
                listRule = EntityManager<EntitySampMergeRule>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listRule;
        }

        public bool Save(EntitySampMergeRule mergeRule)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_sample_merge_rule");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("rul_id", id);
                values.Add("Rsmr_Dcom_id", mergeRule.ComId);
                values.Add("Rsmr_com_his_code", mergeRule.ComHisCode);
                values.Add("Rsmr_merge_type", mergeRule.ComMergeType);
                values.Add("Rsmr_Dtub_code", mergeRule.ComTubeCode);
                values.Add("py_code", mergeRule.ComPyCode);
                values.Add("wb_code", mergeRule.ComWbCode);
                values.Add("Rsmr_collect_quantity", mergeRule.ComCollectQuantity);
                values.Add("Rsmr_collect_unit", mergeRule.ComCollectUnit);
                values.Add("Rsmr_Dsam_id", mergeRule.ComSamId);
                values.Add("Rsmr_Dsorc_id", mergeRule.ComSrcId);
                values.Add("Rsmr_price", mergeRule.ComPrice);
                values.Add("Rsmr_unit", mergeRule.ComUnit);
                values.Add("Rsmr_exec_Dpro_id", mergeRule.ComExecCode);
                values.Add("del_flag", mergeRule.ComDelFlag);
                values.Add("Rsmr_print_name", mergeRule.ComPrintName);
                values.Add("Rsmr_barcode_amount", mergeRule.ComBarcodeAmount);
                values.Add("Rsmr_report_time", mergeRule.ComReportTime);
                values.Add("Rsmr_Dorg_id", mergeRule.ComOrgId);
                values.Add("Rsmr_barcode_type", mergeRule.ComBarcodeType);
                values.Add("Rsmr_split_flag", mergeRule.ComSplitFlag);
                values.Add("Rsmr_lis_code", mergeRule.ComLisCode);
                values.Add("Rsmr_source", mergeRule.ComSource);
                values.Add("Rsmr_sam_notice", mergeRule.ComSamNotice);
                values.Add("Rsmr_save_notice", mergeRule.ComSaveNotice);
                values.Add("Rsmr_split_code", mergeRule.ComSplitCode);
                values.Add("Rsmr_his_fee_code", mergeRule.ComHisFeeCode);
                values.Add("Rsmr_sam_remark", mergeRule.ComSamRemark);
                values.Add("Rsmr_report_unit", mergeRule.ComReportUnit);
                values.Add("Rsmr_print_count", mergeRule.ComPrintCount);
                values.Add("Rsmr_test_dest", mergeRule.ComTestDest);
                values.Add("sort_no", mergeRule.ComSortNo);
                values.Add("Rsmr_deadspace_volume", mergeRule.ComDeadspaceVolume);
                values.Add("Rsmr_report_unit2", mergeRule.ComRepUnit2);
                values.Add("Rsmr_urgent_report_time", mergeRule.ComOutTime2);
                values.Add("Rsmr_father_flag", mergeRule.ComFatherFlag);

                helper.InsertOperation("Rel_sample_merge_rule", values);

                mergeRule.ComRulId = id;

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySampMergeRule> Search(string comId)
        {
            List<EntitySampMergeRule> listRule = new List<EntitySampMergeRule>();
            if (!string.IsNullOrEmpty(comId))
            {
                try
                {
                    String sql = string.Format(@"SELECT Rel_sample_merge_rule.*,Dict_source.Dsorc_name,
Dict_sample_remark.Dsamr_content,
Dict_test_tube.Dtub_name,Dict_profession.Dpro_name,Dict_sample.Dsam_name
FROM Rel_sample_merge_rule 
left outer join Dict_source on Dict_source.Dsorc_id = Rel_sample_merge_rule.Rsmr_Dsorc_id
left outer join Dict_sample_remark on (Rel_sample_merge_rule.Rsmr_sam_remark= Dict_sample_remark.Dsamr_id) 
left outer join Dict_test_tube on Rel_sample_merge_rule.Rsmr_Dtub_code=Dict_test_tube.Dtub_code
left outer join Dict_profession on Rel_sample_merge_rule.Rsmr_exec_Dpro_id=Dict_profession.Dpro_id
left outer join Dict_sample on Rel_sample_merge_rule.Rsmr_Dsam_id=Dict_sample.Dsam_id
  WHERE  (Rsmr_Dcom_id = '{0}' and (Rel_sample_merge_rule.del_flag = 0 or Rel_sample_merge_rule.del_flag = '' or Rel_sample_merge_rule.del_flag is null)) ", comId);

                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);

                    listRule = EntityManager<EntitySampMergeRule>.ConvertToList(dt).OrderBy(i => i.ComSortNo).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return listRule;
        }

        public List<EntitySampMergeRule> SearchRuleByHisCode(List<string> listHisFeeCode, string strOriId)
        {
            List<EntitySampMergeRule> list = new List<EntitySampMergeRule>();

            if (listHisFeeCode != null && listHisFeeCode.Count > 0)
            {
                try
                {
                    String sql = string.Empty;

                    string strWhere = string.Empty;

                    foreach (string item in listHisFeeCode)
                    {
                        strWhere += string.Format(",'{0}'", item);
                    }

                    strWhere = strWhere.Remove(0, 1);

                    sql = string.Format(@"SELECT Rel_sample_merge_rule.*,Dict_itm_combine.Dcom_name,Dict_itm_combine.sort_no
FROM Rel_sample_merge_rule
LEFT JOIN Dict_itm_combine on Dict_itm_combine.Dcom_id = Rel_sample_merge_rule.Rsmr_Dcom_id and Dict_itm_combine.del_flag=0
WHERE
(Dict_itm_combine.del_flag = '0' or Dict_itm_combine.del_flag is null) and 
(Rel_sample_merge_rule.del_flag = '0' or Rel_sample_merge_rule.del_flag is null) and 
Dict_itm_combine.Dcom_id is not null and 
Rel_sample_merge_rule.Rsmr_his_fee_code IN ({0}) and
(Rel_sample_merge_rule.Rsmr_Dsorc_id='{1}' or isnull(Rel_sample_merge_rule.Rsmr_Dsorc_id,'') ='')",
                                                 strWhere, strOriId);


                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);

                    list = EntityManager<EntitySampMergeRule>.ConvertToList(dt);

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return list;
        }

        public List<EntitySampMergeRule> SearchRuleByLisCode(List<string> listLisFeeCode, string strOriId)
        {
            List<EntitySampMergeRule> list = new List<EntitySampMergeRule>();

            if (listLisFeeCode != null && listLisFeeCode.Count > 0)
            {
                try
                {
                    String sql = string.Empty;

                    string strWhere = string.Empty;

                    foreach (string item in listLisFeeCode)
                    {
                        strWhere += string.Format(",'{0}'", item);
                    }

                    strWhere = strWhere.Remove(0, 1);

                    sql = string.Format(@"SELECT Rel_sample_merge_rule.*,Dict_itm_combine.Dcom_name,Dict_itm_combine.sort_no 
FROM Rel_sample_merge_rule
LEFT JOIN Dict_itm_combine on Dict_itm_combine.Dcom_id = Rel_sample_merge_rule.Rsmr_Dcom_id and Dict_itm_combine.del_flag=0
WHERE (Rel_sample_merge_rule.del_flag <> '1' or Dict_itm_combine.del_flag is null) and 
Dict_itm_combine.Dcom_id is not null and 
Rel_sample_merge_rule.Rsmr_Dcom_id IN ({0}) and
(Rel_sample_merge_rule.Rsmr_Dsorc_id='{1}' or isnull(Rel_sample_merge_rule.Rsmr_Dsorc_id,'') ='')",
                                                 strWhere, strOriId);


                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);

                    list = EntityManager<EntitySampMergeRule>.ConvertToList(dt);

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return list;
        }

        public List<EntitySampMergeRule> SearchRuleBySplitComId(string strComId, string strOriId)
        {
            List<EntitySampMergeRule> list = new List<EntitySampMergeRule>();

            if (!string.IsNullOrEmpty(strComId))
            {
                try
                {
                    string sql = string.Format(@"select
Rel_sample_merge_rule.*,Dict_itm_combine.Dcom_name,Dict_itm_combine.sort_no
from Rel_sample_diverge_rule
INNER JOIN Rel_sample_merge_rule ON Rel_sample_merge_rule.Rsmr_Dcom_id = Rel_sample_diverge_rule.Rsdr_split_id
LEFT JOIN Dict_itm_combine on Dict_itm_combine.Dcom_id = Rel_sample_merge_rule.Rsmr_Dcom_id and Dict_itm_combine.del_flag=0
where Rel_sample_diverge_rule.Rsdr_Dcom_id ='{0}' and
(Rel_sample_merge_rule.Rsmr_Dsorc_id='{1}' or isnull(Rel_sample_merge_rule.Rsmr_Dsorc_id,'') ='')", strComId, strOriId);

                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);

                    list = EntityManager<EntitySampMergeRule>.ConvertToList(dt);

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }


            return list;
        }

        public bool Update(EntitySampMergeRule mergeRule)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rsmr_Dcom_id", mergeRule.ComId);
                values.Add("Rsmr_com_his_code", mergeRule.ComHisCode);
                values.Add("Rsmr_merge_type", mergeRule.ComMergeType);
                values.Add("Rsmr_Dtub_code", mergeRule.ComTubeCode);
                values.Add("py_code", mergeRule.ComPyCode);
                values.Add("wb_code", mergeRule.ComWbCode);
                values.Add("Rsmr_collect_quantity", mergeRule.ComCollectQuantity);
                values.Add("Rsmr_collect_unit", mergeRule.ComCollectUnit);
                values.Add("Rsmr_Dsam_id", mergeRule.ComSamId);
                values.Add("Rsmr_Dsorc_id", mergeRule.ComSrcId);
                values.Add("Rsmr_price", mergeRule.ComPrice);
                values.Add("Rsmr_unit", mergeRule.ComUnit);
                values.Add("Rsmr_exec_Dpro_id", mergeRule.ComExecCode);
                values.Add("del_flag", mergeRule.ComDelFlag);
                values.Add("Rsmr_print_name", mergeRule.ComPrintName);
                values.Add("Rsmr_barcode_amount", mergeRule.ComBarcodeAmount);
                values.Add("Rsmr_report_time", mergeRule.ComReportTime);
                values.Add("Rsmr_Dorg_id", mergeRule.ComOrgId);
                values.Add("Rsmr_barcode_type", mergeRule.ComBarcodeType);
                values.Add("Rsmr_split_flag", mergeRule.ComSplitFlag);
                values.Add("Rsmr_lis_code", mergeRule.ComLisCode);
                values.Add("Rsmr_source", mergeRule.ComSource);
                values.Add("Rsmr_sam_notice", mergeRule.ComSamNotice);
                values.Add("Rsmr_save_notice", mergeRule.ComSaveNotice);
                values.Add("Rsmr_split_code", mergeRule.ComSplitCode);
                values.Add("Rsmr_his_fee_code", mergeRule.ComHisFeeCode);
                values.Add("Rsmr_sam_remark", mergeRule.ComSamRemark);
                values.Add("Rsmr_report_unit", mergeRule.ComReportUnit);
                values.Add("Rsmr_print_count", mergeRule.ComPrintCount);
                values.Add("Rsmr_test_dest", mergeRule.ComTestDest);
                values.Add("sort_no", mergeRule.ComSortNo);
                values.Add("Rsmr_deadspace_volume", mergeRule.ComDeadspaceVolume);
                values.Add("Rsmr_report_unit2", mergeRule.ComRepUnit2);
                values.Add("Rsmr_urgent_report_time", mergeRule.ComOutTime2);
                values.Add("Rsmr_father_flag", mergeRule.ComFatherFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rsmr_id", mergeRule.ComRulId);

                helper.UpdateOperation("Rel_sample_merge_rule", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
    }
}
