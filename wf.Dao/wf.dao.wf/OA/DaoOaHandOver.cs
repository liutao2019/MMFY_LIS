using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoOaHandOver))]
    public class DaoOaHandOver : IDaoOaHandOver
    {

        public List<EntityDicHandOver> GetDictHandoverList()
        {
            try
            {
                string sql = string.Format(@"SELECT
ho_id,
ho_type_id,
ho_time1,
ho_time2,
ho_time3,
ho_timeInter,Dict_profession.Dpro_name AS typename
FROM
dict_hand_over 
LEFT JOIN Dict_profession ON dict_hand_over.ho_type_id=Dict_profession.Dpro_id ");
                List<EntityDicHandOver> list = new List<EntityDicHandOver>();
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    EntityDicHandOver rule = new EntityDicHandOver();
                    rule.HoId = Convert.ToInt32(dt.Rows[i]["ho_id"]);
                    rule.HoTypeId = dt.Rows[i]["ho_type_id"].ToString();
                    rule.HoTime1 = dt.Rows[i]["ho_time1"].ToString();
                    rule.HoTime2 = dt.Rows[i]["ho_time2"].ToString();
                    rule.HoTime3 = dt.Rows[i]["ho_time3"].ToString();
                    rule.HoTimeInter = dt.Rows[i]["ho_timeInter"].ToString();
                    rule.TypeName = dt.Rows[i]["typename"].ToString();
                    list.Add(rule);
                }
                return list;


            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicHandOver>();
            }
        }

        public bool UpdateDictHandoverList(List<EntityDicHandOver> list)
        {
            try
            {
                DBManager helper = new DBManager();
                foreach (EntityDicHandOver entity in list)
                {
                    if (entity.IsNew)
                    {
                        string strSql =string.Format( @"insert into dict_hand_over
                                  (ho_type_id,ho_time1,ho_time2,ho_time3,ho_timeInter)
                                  values ('{0}','{1}','{2}','{3}','{4}')",entity.HoTypeId,entity.HoTime1,entity.HoTime2,entity.HoTime3,entity.HoTimeInter);

                        helper.ExecSql(strSql);
                    }
                    else
                    {
                        string strSql =string.Format(@"update dict_hand_over set ho_time1='{0}',ho_time2='{1}',ho_time3='{2}'
                                  ,ho_timeInter='{3}'
                                  where ho_type_id='{4}' ",entity.HoTime1,entity.HoTime2,entity.HoTime3,entity.HoTimeInter,entity.HoTypeId);
                        helper.ExecSql(strSql);
                    }
                }
                //DictAuditRuleCache.Current.Refresh();
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteDictHandover(string typeid)
        {
            try
            {
                DBManager helper = new DBManager();

                string strSql =string.Format( @"delete from dict_hand_over where ho_type_id='{0}'",typeid);
                helper.ExecSql(strSql);
                //DictAuditRuleCache.Current.Refresh();
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
