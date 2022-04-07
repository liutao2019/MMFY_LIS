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
using System.Linq;
using System.Text;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoReaApplyDetail))]
    public class DaoReaApplyDetail : DclDaoBase, IDaoReaApplyDetail
    {
        public bool CancelReaApplyDetail(EntityReaApplyDetail detail)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rdet_no", detail.Rdet_no);

                helper.UpdateOperation("Rea_apply_detail", values, keys);


                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteReaApplyDetail(string applyId)
        {
            bool result = false;
            try
            {
                DBManager helper = GetDbManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rdet_no", applyId);

                helper.DeleteOperation("Rea_apply_detail", keys);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return result;
            }
            return true;
        }

        public bool DeleteReaApplyDetailByIN(string applyId, string rea_id)
        {
            bool result = false;
            try
            {
                DBManager helper = GetDbManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rdet_no", applyId);
                keys.Add("Rdet_reaid", rea_id);

                helper.DeleteOperation("Rea_apply_detail", keys);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return result;
            }
            return true;
        }

        public bool InsertNewReaApplyDetail(EntityReaApplyDetail apply)
        {
            bool result = false;

            DBManager helper = GetDbManager();

            if (apply != null)
            {
                try
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values = helper.ConverToDBSaveParameter(apply);

                    helper.InsertOperation("Rea_apply_detail", values);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }
            return result;
        }

        public List<EntityReaApplyDetail> GetReaApplyDetailByReaId(string reaId)
        {
            List<EntityReaApplyDetail> detailList = new List<EntityReaApplyDetail>();
            DataTable dtDetail = new DataTable();
            if (!string.IsNullOrEmpty(reaId))
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"
SELECT 
Dict_rea_setting.Drea_name as ReagentName,
Rea_apply_detail.package as ReagentPackage,
Rea_apply.Ray_applier as Rdet_applier,
Rea_apply.Ray_auditor as Rdet_auditor,
Rea_apply_detail.*
FROM Rea_apply_detail 
inner join Rea_apply on Rea_apply.Ray_no = Rea_apply_detail.Rdet_no
INNER JOIN Dict_rea_setting ON Rea_apply_detail.Rdet_reaid = Dict_rea_setting.Drea_id
WHERE (Rea_apply_detail.Rdet_no = '{0}' and Rea_apply_detail.del_flag = '0')
", reaId);
                try
                {
                    dtDetail = helper.ExecuteDtSql(sql);
                    detailList = EntityManager<EntityReaApplyDetail>.ConvertToList(dtDetail).OrderBy(i => i.sort_no).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return detailList;
        }

        public List<EntityReaApplyDetail> GetReaApplyDetailByReaIN(string reaId, string rayno)
        {
            List<EntityReaApplyDetail> detailList = new List<EntityReaApplyDetail>();
            DataTable dtDetail = new DataTable();
            if (!string.IsNullOrEmpty(reaId))
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"
SELECT 
Dict_rea_setting.Drea_name as ReagentName,
Rea_apply_detail.package as ReagentPackage,
Rea_apply.Ray_applier as Rdet_applier,
Rea_apply.Ray_auditor as Rdet_auditor,
Rea_apply_detail.*
FROM Rea_apply_detail 
inner join Rea_apply on Rea_apply.Ray_no = Rea_apply_detail.Rdet_no
INNER JOIN Dict_rea_setting ON Rea_apply_detail.Rdet_reaid = Dict_rea_setting.Drea_id
WHERE (Rea_apply_detail.Rdet_no = '{0}' and Rea_apply_detail.Rdet_reaid = '{1}' and Rea_apply_detail.del_flag = '0')
", rayno, reaId);
                try
                {
                    dtDetail = helper.ExecuteDtSql(sql);
                    detailList = EntityManager<EntityReaApplyDetail>.ConvertToList(dtDetail).OrderBy(i => i.sort_no).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return detailList;
        }
    }
}
