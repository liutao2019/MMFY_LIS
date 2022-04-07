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
    [Export("wf.plugin.wf", typeof(IDaoDicCombineTimeRule))]
    public class DaoDicComTimeRule : IDaoDicCombineTimeRule
    {
        public bool Save(EntityDicCombineTimeRule timerule)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_itm_combine_timerule");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dtr_id", id);
                values.Add("Dtr_type", timerule.ComTimeType);
                values.Add("Dtr_start_type", timerule.ComTimeStartType);
                values.Add("Dtr_end_type", timerule.ComTimeEndType);
                values.Add("Dtr_Dsorc_id", timerule.ComTimeOriId);
                values.Add("Dtr_time", timerule.ComTime);
                values.Add("Dtr_rea_time", timerule.ComReaTime);
                values.Add("del_flag", timerule.DelFlag);

                helper.InsertOperation("Dict_itm_combine_timerule", values);

                timerule.ComTimeId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicCombineTimeRule timerule)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dtr_type", timerule.ComTimeType);
                values.Add("Dtr_start_type", timerule.ComTimeStartType);
                values.Add("Dtr_end_type", timerule.ComTimeEndType);
                values.Add("Dtr_Dsorc_id", timerule.ComTimeOriId);
                values.Add("Dtr_time", timerule.ComTime);
                values.Add("Dtr_rea_time", timerule.ComReaTime);
                values.Add("del_flag", timerule.DelFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dtr_id", timerule.ComTimeId);

                helper.UpdateOperation("Dict_itm_combine_timerule", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicCombineTimeRule timerule)
        {
            try
            {
                DBManager helper = new DBManager();
                string delStr = string.Format("delete from Dict_itm_combine_timerule where Dtr_id='{0}'", timerule.ComTimeId);
                helper.ExecCommand(delStr);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicCombineTimeRule> Search(Object obj)
        {
            try
            {
                String sql = @"select Dict_itm_combine_timerule.*,Dict_source.Dsorc_name,Dict_status.Dsta_content startType,Dic_pub_status2.Dsta_content endType 
from Dict_itm_combine_timerule
left join Dict_source on Dtr_Dsorc_id=Dsorc_id
left join Dict_status on Dtr_start_type=Dict_status.Dsta_name
left join Dict_status as Dic_pub_status2 on Dtr_end_type= Dic_pub_status2.Dsta_name where Dict_itm_combine_timerule.del_flag = '0'";


                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);

            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicCombineTimeRule>();
            }
        }
        public List<EntityDicCombineTimeRule> GetTATComTimeByRepId(List<string> listRepId)
        {
            List<EntityDicCombineTimeRule> list = new List<EntityDicCombineTimeRule>();
            try
            {
                if (listRepId != null && listRepId.Count > 0)
                {
                    string strRepId = string.Empty;
                    foreach (string repId in listRepId)
                    {
                        strRepId += string.Format(",'{0}'", repId);
                    }
                    strRepId = strRepId.Remove(0, 1);
                    string sql = string.Format(@"select Pat_lis_detail.Pdet_id ,Dict_itm_combine_timerule.* from Pat_lis_detail
left join  Dict_itm_combine_timerule_related on Dict_itm_combine_timerule_related.Dtrr_Dcom_id=Pat_lis_detail.Pdet_Dcom_id
left join  Dict_itm_combine_timerule on Dict_itm_combine_timerule_related.Dtrr_Dtr_id=Dict_itm_combine_timerule.Dtr_id 
where Pdet_id in ({0}) and Dtr_start_type='5' and Dtr_end_type='60'", strRepId);

                    DBManager helper = new DBManager(); 

                    DataTable dt = helper.ExecuteDtSql(sql);
                    list = EntityManager<EntityDicCombineTimeRule>.ConvertToList(dt);
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }
        public List<EntityDicCombineTimeRule> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicCombineTimeRule> list = new List<EntityDicCombineTimeRule>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityDicCombineTimeRule value = new EntityDicCombineTimeRule();

                value.ComTimeId = item["Dtr_id"].ToString();
                value.ComTimeType = item["Dtr_type"].ToString();
                value.ComTimeStartType = item["Dtr_start_type"].ToString();
                value.ComTimeEndType = item["Dtr_end_type"].ToString();
                value.ComTimeOriId = item["Dtr_Dsorc_id"].ToString();
                value.ComTime = item["Dtr_time"].ToString();
                value.ComReaTime = item["Dtr_rea_time"].ToString();

                value.OriName = item["Dsorc_name"].ToString();
                value.StartType = item["startType"].ToString();
                value.EndType = item["endType"].ToString();

                list.Add(value);
            }
            return list.OrderBy(i => i.ComTimeId).ToList();
        }
    }
}
