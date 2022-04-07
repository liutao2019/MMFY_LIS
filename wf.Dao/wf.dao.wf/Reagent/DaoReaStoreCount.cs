/*  
 * 警告：
 * 本源代码所有权归广州慧扬健康科技有限公司(下称“本公司”)所有，已采取保密措施加以保护。  受《中华人民共和国刑法》、
 * 《反不正当竞争法》和《国家工商行政管理局关于禁止侵犯商业秘密行为的若干规定》等相关法律法规的保护。未经本公司书面
 * 许可，任何人披露、使用或者允许他人使用本源代码，必将受到相关法律的严厉惩罚。
 * Warning: 
 * The ownership of this source code belongs to Guangzhou Wisefly Technology Co., Ltd.(hereinafter referred to as "the company"), 
 * which is protected by the criminal law of the People's Republic of China, the anti unfair competition law and the 
 * provisions of the State Administration for Industry and Commerce on prohibiting the infringement of business secrets, etc. 
 * Without the written permission of the company, anyone who discloses, uses or allows others to use this source code 
 * will be severely punished by the relevant laws.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoReaStoreCount))]
    public class DaoReaStoreCount : DclDaoBase, IDaoReaStoreCount
    {
        public bool UpdateReaStoreCount(EntityReaStoreCount reaStoreCount)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();

                values.Add("Rri_Count", reaStoreCount.Rri_Count);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rri_Drea_id", reaStoreCount.Rri_Drea_id);
                helper.UpdateOperation("Rel_rea_inventory", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
        public EntityResponse SaveReaStoreCount(EntityReaStoreCount reaStoreCount)
        {
            EntityResponse result = new EntityResponse();
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rri_Count", reaStoreCount.Rri_Count);
                values.Add("Rri_Drea_id", reaStoreCount.Rri_Drea_id);
                helper.InsertOperation("Rel_rea_inventory", values);
                result.Scusess = true;
                result.SetResult(reaStoreCount);
                return result;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result.Scusess = false;
                return result;
            }
        }
        public List<EntityReaStoreCount> SearchByQC(EntityReaQC reaQC)
        {
            try
            {
                String sql = @"select * from Rel_rea_inventory where 1=1 and Rri_Drea_id = '{0}'";
                sql = string.Format(sql, reaQC.ReaId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityReaStoreCount>();
            }
        }
        public List<EntityReaStoreCount> SearchAll()
        {
            try
            {
                String sql = @"select * from Rel_rea_inventory";
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityReaStoreCount>();
            }
        }
        public List<EntityReaStoreCount> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityReaStoreCount> list = EntityManager<EntityReaStoreCount>.ConvertToList(dtSour);
            return list.OrderBy(i => i.Rri_Drea_id).ToList();
        }
    }
}
