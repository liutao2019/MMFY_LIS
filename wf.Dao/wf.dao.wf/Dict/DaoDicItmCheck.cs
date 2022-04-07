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

    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicItmCheck>))]
    public class DaoDicItmCheck : IDaoDic<EntityDicItmCheck>
    {
        public bool Save(EntityDicItmCheck check)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_itm_verifi");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dverifi_id", id);
                //values.Add("check_id", check.CheckId);
                values.Add("Dverifi_Ditr_id", check.CheckItrId);
                values.Add("Dverifi_name", check.CheckName);
                values.Add("seq_no", check.SeqNo);
                values.Add("del_flag", check.DelFlag);
                values.Add("Dverifi_remark", check.CheckRemark);

                helper.InsertOperation("Dict_itm_verifi", values);

                // check.CheckId = id.ToString();

                //check.CheckId = check.CheckId;

                if (check.DetailList != null)
                {
                    DaoDicItmCheckDetail dtDao = new DaoDicItmCheckDetail();

                    foreach (var checkDt in check.DetailList)
                    {
                        checkDt.CheckIdDetial = id.ToString();
                        dtDao.Save(checkDt);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicItmCheck check)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dverifi_Ditr_id", check.CheckItrId);
                values.Add("Dverifi_name", check.CheckName);
                values.Add("seq_no", check.SeqNo);
                values.Add("del_flag", check.DelFlag);
                values.Add("Dverifi_remark", check.CheckRemark);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dverifi_id", check.CheckId);

                helper.UpdateOperation("Dict_itm_verifi", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicItmCheck check)
        {
            try
            {
                DBManager helper = new DBManager();

                //Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("del_flag", "1");

                //Dictionary<string, object> keys = new Dictionary<string, object>();
                //keys.Add("check_id", check.CheckId);

                //helper.UpdateOperation("dic_itm_check", values, keys);
                string sql = string.Format(@"DELETE FROM Dict_itm_verifi WHERE Dverifi_id='{0}'", check.CheckItrId);

                helper.ExecSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicItmCheck> Search(Object obj)
        {
            try
            {
                string itrID = string.Empty;
                EntityDicItmCheck info = obj as EntityDicItmCheck;
                if (info != null && !string.IsNullOrEmpty(info.CheckItrId))
                {
                    itrID = info.CheckItrId;
                }
                String sql =string.Format(@"SELECT  Dict_itm_verifi.Dverifi_id,
Dict_itm_verifi.Dverifi_Ditr_id,
Dict_itm_verifi.Dverifi_name,
Dict_itm_verifi.seq_no,
Dict_itm_verifi.del_flag,
Dict_itm_verifi.Dverifi_remark,
Dict_itm_verifi.Dverifi_id  as display_group_id 
FROM Dict_itm_verifi 
WHERE ISNULL(Dict_itm_verifi.del_flag,'')<>'1' AND Dict_itm_verifi.Dverifi_Ditr_id='{0}' "
                           , itrID);

                if (obj is string && obj.ToString() == "cache")
                {
                    sql = @"select * from Dict_itm_verifi ";
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicItmCheck>();
            }
        }

        public List<EntityDicItmCheck> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicItmCheck> list = EntityManager<EntityDicItmCheck>.ConvertToList(dtSour);
            return list.ToList();
        }
    }
}
