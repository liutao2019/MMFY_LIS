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
    [Export("wf.plugin.wf", typeof(IDaoDicCombineDetail))]
    public class DaoDicCombineDetail : IDaoDicCombineDetail
    {
        public bool Save(EntityDicCombineDetail com)
        {
            try
            {


                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rici_Dcom_id", com.ComId);
                values.Add("Rici_Ditm_id", com.ComItmId);
                values.Add("Rici_Ditm_ecode", com.ComItmEname);
                values.Add("Rici_flag", com.ComFlag);
                values.Add("Rici_must_item", com.ComMustItem);
                values.Add("sort_no", com.ComSortNo);
                values.Add("Rici_print_flag", com.ComPrintFlag);
                helper.InsertOperation("Rel_itm_combine_item", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicCombineDetail com)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rici_Ditm_id", com.ComItmId);
                values.Add("Rici_Ditm_ecode", com.ComItmId);
                values.Add("Rici_flag", com.ComFlag);
                values.Add("com_must_item", com.ComMustItem);
                values.Add("sort_no", com.ComSortNo);
                values.Add("Rici_print_flag", com.ComPrintFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rici_Dcom_id", com.ComId);

                helper.UpdateOperation("Rel_itm_combine_item", values, keys);
              
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicCombineDetail com)
        {
            try
            {

                DBManager helper = new DBManager();
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rici_Dcom_id", com.ComId);
                helper.DeleteOperation("Rel_itm_combine_item", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicCombineDetail> Search(Object obj)
        {
            try
            {
                string sql = string.Empty;
                if (obj is List<string>)
                {
                    List<string> listStr = obj as List<string>;
                    if (listStr.Count == 1)
                    {
                        sql = string.Format(@"select * from Rel_itm_combine_item
left join Pat_lis_detail on Pat_lis_detail.Pdet_Dcom_id=Rel_itm_combine_item.Rici_Dcom_id
where Rici_must_item ='1' and Pdet_id='{0}'
and  Rici_Ditm_id not in (select Lres_Ditm_id from Lis_result where Lres_Pma_rep_id='{0}' and Lres_flag='1')", listStr[0]);
                    }
                    if (listStr.Count == 2)
                    {
                        sql = string.Format(@"select Rici_Ditm_id,
(select top 1 Ritm_default from Rel_itm_sample where Rel_itm_sample.Ritm_id=Rel_itm_combine_item.Rici_Ditm_id 
and Rel_itm_sample.Ritm_sam_id=Dict_itm_combine.Dcom_Dsam_id and (Ritm_Ditr_id='{0}' or Ritm_Ditr_id='-1') order by Ritm_Ditr_id desc) itm_default
from Rel_itm_combine_item 
left join Dict_itm on Rel_itm_combine_item.Rici_Ditm_id=Dict_itm.Ditm_id 
left join Dict_itm_combine on Rel_itm_combine_item.Rici_Dcom_id=Dict_itm_combine.Dcom_id 
where Rel_itm_combine_item.Rici_Dcom_id='{1}'order by Dict_itm.sort_no", listStr[0], listStr[1]);
                    }
                }
                else
                {
                    if (obj == null||(obj != null && obj.ToString() == "Cache"))
                    {
                        sql = string.Format(@"SELECT 
Dict_itm_combine.Dcom_name,
Rel_itm_combine_item.*,
Dict_itm.Ditm_match_sex,
isnull(Dict_itm.sort_no,0) sort_no
FROM 
Dict_itm_combine 
inner JOIN Rel_itm_combine_item ON Dict_itm_combine.Dcom_id = Rel_itm_combine_item.Rici_Dcom_id
inner JOIN Dict_itm on Dict_itm.Ditm_id = Rel_itm_combine_item.Rici_Ditm_id
WHERE (Dict_itm_combine.del_flag = '0' and Dict_itm.del_flag = '0')");
                    }
                    else
                    {
                        sql = string.Format(@"select Rici_Dcom_id, Rici_Ditm_id, Rici_Ditm_ecode, Rici_flag, Rici_must_item, sort_no, Rici_print_flag,'' as itm_default  from Rel_itm_combine_item
WHERE Rici_Dcom_id='{0}'", obj.ToString());
                    }
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityDicCombineDetail> list = EntityManager<EntityDicCombineDetail>.ConvertToList(dt);
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicCombineDetail>();
            }
        }

        public List<EntityDicCombineDetail> GetCombineDefData(string itrId, string comId)
        {
            List<EntityDicCombineDetail> list = new List<EntityDicCombineDetail>();
            try
            {
                string sql = string.Format(@"select Rici_Dcom_id,isnull(Ritm_default,'') as def_data,Rel_itm_sample.Ritm_Ditr_id 
from Rel_itm_combine_item 
left join Dict_itm on Rel_itm_combine_item.Rici_Ditm_id=Dict_itm.Ditm_id 
left join Rel_itm_sample on Rel_itm_sample.Ritm_id=Dict_itm.Ditm_id
where isnull(Ritm_default,'')<>''  and Rel_itm_sample.Ritm_Ditr_id='{0}' and Rel_itm_combine_item.Rici_Dcom_id='{1}'
group by Rici_Dcom_id,isnull(Ritm_default,''),Rel_itm_sample.Ritm_Ditr_id,Rel_itm_combine_item.sort_no 
                                ", itrId, comId);
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntityDicCombineDetail>.ConvertToList(dt).OrderBy(w => w.ComSortNo).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public List<EntityDicCombineDetail> GetCombineMiWdthDefault(string comId, string patSamId, string itrId)
        {
            List<EntityDicCombineDetail> list = new List<EntityDicCombineDetail>();
            try
            {
                string sql = string.Format(@"select Rel_itm_sample.Ritm_sam_id,
Rel_itm_sample.Ritm_unit,
Rel_itm_combine_item.Rici_Ditm_id ,
Dict_itm.Ditm_ecode,
Dict_itm.Ditm_rep_code,
Rel_itm_sample.Ritm_default
from Rel_itm_combine_item
left join Rel_itm_sample on Rel_itm_sample.Ritm_id = Rel_itm_combine_item.Rici_Ditm_id
left join Dict_itm on Dict_itm.Ditm_id = Rel_itm_combine_item.Rici_Ditm_id
where
Rel_itm_combine_item.Rici_Dcom_id = '{0}'
and (Rel_itm_sample.Ritm_sam_id = '{1}' or Rel_itm_sample.Ritm_sam_id = '-1')
and (Rel_itm_sample.Ritm_Ditr_id = '{2}' )
and (Ritm_default is not null and Ritm_default <> '')", comId, patSamId, itrId);
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                if (dt.Rows.Count == 0)
                {
                    sql = string.Format(@"select Rel_itm_sample.Ritm_sam_id,
Rel_itm_sample.Ritm_unit,
Rel_itm_combine_item.Rici_Ditm_id,
Dict_itm.Ditm_ecode,
Dict_itm.Ditm_rep_code,
Rel_itm_sample.Ritm_default
from Rel_itm_combine_item
left join Rel_itm_sample on Rel_itm_sample.Ritm_id = Rel_itm_combine_item.Rici_Ditm_id
left join Dict_itm on Dict_itm.Ditm_id = Rel_itm_combine_item.Rici_Ditm_id
where Rel_itm_combine_item.Rici_Dcom_id = '{0}'
and (Rel_itm_sample.Ritm_sam_id = '{1}' or Rel_itm_sample.Ritm_sam_id = '-1')
and (Rel_itm_sample.Ritm_Ditr_id = '-1')
and (Ritm_default is not null and Ritm_default <> '')", comId, patSamId);
                    dt = helper.ExecuteDtSql(sql);
                }
                list = EntityManager<EntityDicCombineDetail>.ConvertToList(dt).OrderByDescending(w=>w.ItmSamId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public List<EntityDicCombineDetail> GetItmNameByRepId(string RepId, bool addSql)
        {
            List<EntityDicCombineDetail> list = new List<EntityDicCombineDetail>();
            try
            {
                string sql = string.Format(@"select Rici_Ditm_id,Dict_itm.Ditm_name 
from Rel_itm_combine_item
left join Pat_lis_detail with(nolock) on Pat_lis_detail.Pdet_Dcom_id=Rel_itm_combine_item.Rici_Dcom_id
left join Dict_itm on Dict_itm.Ditm_id=Rel_itm_combine_item.Rici_Ditm_id
where 1=1 and Dict_itm.del_flag<>'1'");
                if (addSql)
                {
                    sql += " and Rici_must_item ='1' AND Rici_Ditm_id not in (select Rcalfor_Ditm_id from Rel_itm_calculaformula )";
                }
                if (!string.IsNullOrEmpty(RepId))
                {
                    sql += string.Format(@" and Rici_must_item ='1' and Pdet_id ='{0}'", RepId);
                }
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntityDicCombineDetail>.ConvertToList(dt).ToList(); 
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return list;
        }

        public List<EntityDicCombineDetail> GetDictCombineItem(string repId)
        {
            List<EntityDicCombineDetail> list = new List<EntityDicCombineDetail>();
            try
            {
                string sql = string.Format(@"select Rel_itm_combine_item.* 
from Pat_lis_detail with(nolock)
left join Rel_itm_combine_item ON Pat_lis_detail.Pdet_Dcom_id = Rel_itm_combine_item.Rici_Dcom_id
where Pat_lis_detail.Pdet_id='{0}'", repId);
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                list = EntityManager<EntityDicCombineDetail>.ConvertToList(dt).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取病人组合项目信息出错,patID=" + repId, ex);
            }
            return list;
        }

        public List<EntityDicCombineDetail> GetComDetailByComIds(List<string> listComIds)
        {
            List<EntityDicCombineDetail> listDetail = new List<EntityDicCombineDetail>();
            try
            {
                string comIds = string.Empty;
                foreach (string comId in listComIds)
                {
                    comIds += string.Format(",{0}", comId);
                }
                comIds.Remove(0, 1);
                string sql = string.Format(@"select
Rici_Dcom_id,
Rici_Ditm_id,
Rici_must_item,
sort_no
from Rel_itm_combine_item with(nolock)
where Rici_Dcom_id in ({0})", comIds);
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                listDetail = EntityManager<EntityDicCombineDetail>.ConvertToList(dt).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return listDetail;
        }
    }
}
