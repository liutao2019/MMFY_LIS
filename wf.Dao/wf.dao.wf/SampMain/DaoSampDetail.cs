using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSampDetail))]
    public class DaoSampDetail : DclDaoBase, IDaoSampDetail
    {
        public List<EntitySampDetail> GetSampDetail(string sampBarId)
        {
            List<EntitySampDetail> list = new List<EntitySampDetail>();
            try
            {
                string sql = string.Format(@"select
Sdet_sn, Sdet_Sma_bar_id, Sdet_bar_code, Sdet_bar_batch_no, Sdet_order_name, Sdet_order_code, Sdet_order_sn, 
Sdet_type, Sdet_flag, Sdet_date, Sdet_order_date, Sdet_order_occ_date, Sdet_order_com_no, Sdet_confirm_flag, 
Sdet_confirm_user_id, Sdet_confirm_user_name, Sdet_order_price, Sdet_order_unit, Sdet_comm_flag, Sdet_comm_date, 
Sdet_comm_Ditr_id, Sdet_display_flag, Sdet_exec_code, Sdet_ecec_name, Sdet_plan_report_time, Sdet_report_flag, Sdet_report_date, 
Sdet_enrol_flag, Sdet_order_other, Sample_detail.del_flag, Sample_detail.Sdet_com_id, Sample_detail.Sdet_com_name, Sdet_blood_notice, Sdet_save_notice, 
Sdet_order_sn2, Sdet_lab_test_code, Sdet_lab_work_id, Sdet_apply_id, Dict_itm_combine.sort_no,
Dict_itm_combine.Dcom_name,Dict_itm_combine.Dcom_remark,isnull(Dict_itm_combine.Dcom_urgent_flag,0) Dcom_urgent_flag
from Sample_detail
left join Dict_itm_combine on Dict_itm_combine.Dcom_id=Sample_detail.Sdet_com_id
where Sdet_Sma_bar_id='{0}' and Sample_detail.del_flag = 0", sampBarId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntitySampDetail>.ConvertToList(dt).OrderBy(i => i.SortNo).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }


        public Boolean DeleteSampDetailAll(String sampBarId)
        {
            bool result = true;
            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = "update Sample_detail set del_flag='1' where Sdet_Sma_bar_id=@samp_bar_id";

                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarId));

                helper.ExecCommand(strUpdateSql, paramHt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }


        public Boolean DeleteSampDetail(List<EntitySampDetail> listSampDetail)
        {
            bool result = true;
            try
            {
                DBManager helper = GetDbManager();

                string strDetailId = string.Empty;

                foreach (EntitySampDetail item in listSampDetail)
                {
                    strDetailId += string.Format(",'{0}'", item.DetSn);
                }

                strDetailId = strDetailId.Remove(0, 1);

                string strUpdateSql = string.Format("update Sample_detail set del_flag='1' where Sdet_sn in ({0})", strDetailId);

                helper.ExecSql(strUpdateSql);

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public List<EntitySampDetail> GetSampDetailByYzId(List<String> listYzId, string srcName)
        {
            List<EntitySampDetail> list = new List<EntitySampDetail>();
            try
            {
                if (listYzId.Count > 0)
                {
                    string strYzId = string.Empty;

                    foreach (string item in listYzId)
                    {
                        strYzId += string.Format(",'{0}'", item);
                    }
                    strYzId = strYzId.Remove(0, 1);


                    string sql = string.Format(@"select 
                                                 Sdet_sn, Sdet_Sma_bar_id, Sdet_bar_code, Sdet_order_sn
                                                 from sample_main, Sample_detail
                                                 where sample_main.Sma_bar_id= Sample_detail.Sdet_Sma_bar_id and Sdet_order_sn in({0}) and sma_pat_src_name = '{1}' and  sample_detail.del_flag = 0", strYzId, srcName);

                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        list = EntityManager<EntitySampDetail>.ConvertToList(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }

            return list;
        }


        public List<String> CreateSampDetail(List<EntitySampDetail> listSampDetail, DBManager helper)
        {
            List<String> result = new List<string>();

            PropertyInfo[] propertys = listSampDetail[0].GetType().GetProperties();

            foreach (EntitySampDetail sampDetail in listSampDetail)
            {
                try
                {
                    result.Add(helper.ConverToDBSQL(sampDetail, propertys, "Sample_detail"));
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }

            return result;
        }

        public List<EntitySampDetailMachineCode> GetSampDetailMachineCodeByItrId(string sampBarId, string itrId)
        {
            List<EntitySampDetailMachineCode> list = new List<EntitySampDetailMachineCode>();
            try
            {
                string sql = string.Format(@"SELECT distinct Sample_detail.Sdet_order_name as order_name, Rel_itm_combine_item.Rici_Ditm_ecode as com_itm_ename, 
Rel_itm_combine_item.Rici_Ditm_id as com_itm_id, Rel_itr_combine.Ric_Dcom_id as itr_com_id, Sample_detail.Sdet_com_id as com_id, Rel_itr_channel_code.Ricc_code  as mac_code
FROM Sample_detail 
INNER JOIN Rel_itm_combine_item ON Sample_detail.Sdet_com_id = Rel_itm_combine_item.Rici_Dcom_id 
LEFT  JOIN Rel_itr_channel_code ON Rel_itm_combine_item.Rici_Ditm_id = Rel_itr_channel_code.Ricc_Ditm_id and Rel_itr_channel_code.Ricc_Ditr_id = '{0}' 
and Rel_itr_channel_code.Ricc_flag = 1  and Rel_itr_channel_code.del_flag = 0 
LEFT OUTER JOIN Rel_itr_combine ON Sample_detail.Sdet_com_id = Rel_itr_combine.Ric_Dcom_id AND Rel_itr_combine.Ric_Ditr_id = '{0}'
WHERE     
(Sample_detail.Sdet_bar_code = '{1}') and (Sample_detail.del_flag = '0')", itrId, sampBarId);
                DBManager helper = GetDbManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntitySampDetailMachineCode>.ConvertToList(dt);
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public bool UpdateSampDetailCommFlag(string commFlag, string sampBarCode, string orderCode)
        {
            bool result = false;
            try
            {
                DBManager helper = GetDbManager();
                List<DbParameter> paramHt = new List<DbParameter>();
                string strUpdateSql = "update Sample_detail set Sdet_comm_flag=@modify_flag where Sdet_Sma_bar_id=@samp_bar_id ";
                if (!string.IsNullOrEmpty(orderCode))
                {
                    strUpdateSql += " and Sdet_order_code=@order_code";
                    paramHt.Add(new SqlParameter("@order_code", orderCode));
                }
                paramHt.Add(new SqlParameter("@modify_flag", commFlag));
                paramHt.Add(new SqlParameter("@samp_bar_id", sampBarCode));
                helper.ExecCommand(strUpdateSql, paramHt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }



        public List<EntitySampDetail> GetSampDetailByBarCode(string sampBarId)
        {
            List<EntitySampDetail> list = new List<EntitySampDetail>();
            try
            {

                string sql = @"select Sample_detail.Sdet_bar_code,Sample_detail.Sdet_order_name,Sample_detail.Sdet_order_code,
Sample_main.Sma_pat_name as pid_name,Sample_detail.Sdet_comm_flag
from Sample_detail 
left join Sample_main on Sample_main.Sma_bar_code=Sample_detail.Sdet_bar_code
where Sample_detail.Sdet_bar_code = '" + sampBarId + "'";

                DBManager helper = GetDbManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntitySampDetail>.ConvertToList(dt).OrderBy(i => i.SampDate).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }

        public bool UpdateSampDetailSampFlagByComId(string sampBarCode, List<string> listComId, string flag)
        {
            bool result = true;
            try
            {
                DBManager helper = new DBManager();// GetDbManager();
                string strComId = string.Empty;
                foreach (string comId in listComId)
                {
                    strComId += string.Format(",'{0}'", comId);
                }
                strComId = strComId.Remove(0, 1);
                string strUpdateSql = string.Format(@"update Sample_detail set Sdet_flag = '{2}' where Sdet_bar_code = '{0}' and Sdet_com_id in ({1})", sampBarCode, strComId, flag);
                helper.ExecCommand(strUpdateSql);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public bool UpdateSampFlagByDetSn(string detSn)
        {
            bool result = true;
            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = @"update Sample_detail set Sdet_flag = 1 where Sdet_sn =@det_sn";
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@det_sn", detSn));
                helper.ExecCommand(strUpdateSql, paramHt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public List<EntitySampDetail> GetSampDetailByBarCodeAndComId(string sampBarId, List<string> listComId)
        {
            List<EntitySampDetail> result = new List<EntitySampDetail>();
            try
            {
                DBManager helper = GetDbManager();
                string strComId = string.Empty;
                if (listComId != null && listComId.Count > 0)
                {
                    foreach (string comId in listComId)
                    {
                        strComId += string.Format(",'{0}'", comId);
                    }
                }
                strComId = strComId.Remove(0, 1);
                string strSql = string.Format("select Sdet_order_sn,Sdet_com_id from Sample_detail with (nolock) where Sdet_bar_code = '{0}' and Sdet_com_id in ({1})", sampBarId, strComId);
                if (listComId.Count < 0)
                {
                    strSql = string.Format("select Sdet_order_sn,Sdet_com_id from Sample_detail with (nolock) where Sdet_bar_code = '{0}' ", sampBarId);
                }
                helper.ExecCommand(strSql);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }

        public bool UpdateSampFlagByBarCode(string barCode)
        {
            bool result = true;
            try
            {
                DBManager helper = GetDbManager();

                string strUpdateSql = @"update Sample_detail set Sdet_flag = 0 where Sdet_bar_code =@bar_code;
update Sample_detail set Sdet_flag = 1 where Sdet_sn in (
select Sample_detail.Sdet_sn
from
Pat_lis_detail with(nolock)
inner join Pat_lis_main with(nolock) on Pat_lis_main.Pma_rep_id = Pat_lis_detail.Pdet_id
inner join Sample_detail with(nolock) on Sample_detail.Sdet_bar_code = Pat_lis_main.Pma_bar_code and Pat_lis_detail.Pdet_Dcom_id = Sample_detail.Sdet_com_id
where Pat_lis_main.Pma_bar_code =@bar_code)";
                List<DbParameter> paramHt = new List<DbParameter>();
                paramHt.Add(new SqlParameter("@bar_code", barCode));
                helper.ExecCommand(strUpdateSql, paramHt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        public Int32 GetSampDetailCount(string barcode, List<string> listComIds)
        {
            int count = 0;
            try
            {
                string comIds = string.Empty;
                if (listComIds.Count > 0)
                {
                    foreach (string comid in listComIds)
                    {
                        comIds += string.Format(",{0}", comid);
                    }
                    comIds.Remove(0, 1);
                }
                if (!string.IsNullOrEmpty(barcode) && !string.IsNullOrEmpty(comIds))
                {
                    string sql = @"SELECT COUNT(1) FROM Sample_detail WHERE Sdet_bar_code=@samp_bar_code AND del_flag<>'1'
                            AND Sdet_com_id NOT IN(@com_id) ";
                    DBManager helper = GetDbManager();
                    List<DbParameter> paramHt = new List<DbParameter>();
                    paramHt.Add(new SqlParameter("@samp_bar_code", barcode));
                    paramHt.Add(new SqlParameter("@com_id", comIds));
                    count = Convert.ToInt32(helper.SelOne(sql, paramHt));
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return count;
        }

        public Boolean SaveSampDetail(List<EntitySampDetail> listSampDetail)
        {
            bool result = false;

            try
            {
                DBManager helper = GetDbManager();
                foreach (EntitySampDetail sampDetail in listSampDetail)
                {
                    Dictionary<string, object> value = helper.ConverToDBSaveParameter<EntitySampDetail>(sampDetail);
                    //string sgg=  helper.GetInsertSQL("Sample_detail", value);
                    //Lib.LogManager.Logger.LogInfo ("SQL:" + sgg);
                    result = helper.InsertOperation("Sample_detail", value) > 0;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }

        public bool ExistDifferentOCCDate(List<string> listSampBarId)
        {
            bool result = false;

            string strWhere = string.Empty;

            foreach (string sampBarId in listSampBarId)
            {
                strWhere += string.Format(",'{0}'", sampBarId);
            }

            strWhere = strWhere.Remove(0, 1);

            string strSql = string.Format(@"select CONVERT(varchar(100), Sample_detail.Sdet_order_occ_date, 1)  Sdet_order_occ_date 
                                            from 
                                            Sample_detail with(nolock)
                                            where Sdet_Sma_bar_id in ({0})
                                            group by CONVERT(varchar(100), Sample_detail.Sdet_order_occ_date, 1) ", strWhere);

            DBManager helper = new DBManager();// GetDbManager();

            DataTable dt = helper.ExecuteDtSql(strSql);

            if (dt != null && dt.Rows.Count > 1)
            {
                result = true;
            }

            return result;
        }

        public List<EntitySampDetail> GetSampDetailByListBarId(List<string> listSampBarId)
        {
            List<EntitySampDetail> list = new List<EntitySampDetail>();
            try
            {
                string strBarId = string.Empty;
                foreach (string barId in listSampBarId)
                {
                    strBarId += string.Format(",'{0}'", barId);
                }
                strBarId = strBarId.Remove(0, 1);
                string sql = string.Format(@"select
Sdet_sn, Sdet_Sma_bar_id, Sdet_bar_code, Sdet_bar_batch_no, Sdet_order_name, Sdet_order_code, Sdet_order_sn, 
Sdet_type, Sdet_flag, Sdet_date, Sdet_order_date, Sdet_order_occ_date, Sdet_order_com_no, Sdet_confirm_flag, 
Sdet_confirm_user_id, Sdet_confirm_user_name, Sdet_order_price, Sdet_order_unit, Sdet_comm_flag, Sdet_comm_date, 
Sdet_comm_Ditr_id, Sdet_display_flag, Sdet_exec_code, Sdet_ecec_name, Sdet_plan_report_time, Sdet_report_flag, Sdet_report_date, 
Sdet_enrol_flag, Sdet_order_other, Sample_detail.del_flag, Sample_detail.Sdet_com_id, Sample_detail.Sdet_com_name, Sdet_blood_notice, Sdet_save_notice, 
Sdet_order_sn2, Sdet_lab_test_code, Sdet_lab_work_id, Dict_itm_combine.sort_no,
Dict_itm_combine.Dcom_name,Dict_itm_combine.Dcom_remark,isnull(Dict_itm_combine.Dcom_urgent_flag,0) Dcom_urgent_flag
from Sample_detail
left join Dict_itm_combine on Dict_itm_combine.Dcom_id=Sample_detail.Sdet_com_id
where Sdet_Sma_bar_id in ({0}) and Sample_detail.del_flag = 0", strBarId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    list = EntityManager<EntitySampDetail>.ConvertToList(dt).OrderBy(i => i.SortNo).ToList();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }

        public List<string> GetPatOrderIDs(string RepId)
        {
            List<string> listOrderIds = new List<string>();
            try
            {
                string sql = string.Format(@"select distinct Sample_detail.Sdet_order_sn from 
Pat_lis_main with(nolock)
inner join Sample_detail with(nolock) on Sample_detail.Sdet_bar_code=Pat_lis_main.Pma_bar_code
where Sample_detail.del_flag='0'
and Pat_lis_main.Pma_rep_id='{0}'
and exists(select top 1 1 from Pat_lis_detail with(nolock) where Pat_lis_detail.Pdet_id=Pat_lis_main.Pma_rep_id and Pat_lis_detail.Pdet_Dcom_id= Sample_detail.Sdet_com_id)
", RepId);
                DBManager helper = new DBManager();

                DataTable table = helper.ExecuteDtSql(sql);

                if (table != null && table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        if (table.Rows[i]["Sdet_order_sn"].ToString().Length > 0)
                        {
                            listOrderIds.Add(table.Rows[i]["Sdet_order_sn"].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listOrderIds;
        }
    }
}
