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
    [Export("wf.plugin.wf", typeof(IDaoUserCaSign))]
    public class DaoUserCaSign : IDaoUserCaSign
    {
        public bool InsertCaSign(List<EntityCaSign> CaSignList)
        {
            try
            {

                DBManager helper = new DBManager();
                foreach (EntityCaSign CaSign in CaSignList)
                {
                    Dictionary<string, object> values = new Dictionary<string, object>();
                    values.Add("ca_date", CaSign.CaDate);
                    values.Add("ca_entity_id", CaSign.CaEntityId);
                    values.Add("ca_cerid", CaSign.CaCerId);
                    values.Add("ca_event", CaSign.CaEvent);
                    values.Add("ca_login_id", CaSign.CaLoginId);
                    values.Add("ca_name", CaSign.CaName);
                    values.Add("ca_remark", CaSign.CaRemark);
                    values.Add("ca_source_content", CaSign.CaSourceContent);
                    values.Add("ca_sign_content", CaSign.CaSignContent);
                    values.Add("ca_source_timestamp", CaSign.CaSourceTimestamp);
                    values.Add("ca_sign_timestamp", CaSign.CaSignTimestamp);

                    helper.InsertOperation("ca_sign", values);
                }

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityCaSign> GetCaSign(string CerId,string EntityId)
        {
            try
            {
                string strUser = "";
                if (!string.IsNullOrEmpty(EntityId))
                {
                    strUser= string.Format(" or ca_entity_id = '{0}' ", EntityId);
                }
                String sql = string.Format("SELECT * FROM ca_sign WHERE ca_cerid='{0}' {1} order by ca_date", CerId, strUser);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityCaSign> list = EntityManager<EntityCaSign>.ConvertToList(dt).OrderBy(i => i.CaDate).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityCaSign>();
            }
        }
    }
}
