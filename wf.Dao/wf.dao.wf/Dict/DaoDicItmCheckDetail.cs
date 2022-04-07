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

    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicItmCheckDetail>))]
    public class DaoDicItmCheckDetail : IDaoDic<EntityDicItmCheckDetail>
    {
        public bool Save(EntityDicItmCheckDetail check)
        {
            try
            {
                int idDetail = IdentityHelper.GetMedIdentity("Dict_itm_verifi_detail");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
               // values.Add("det_id", idDetail);
                values.Add("Divd_Dverifi_id", check.CheckIdDetial);
                values.Add("Divd_name", check.CheckNameDetail);
                values.Add("Divd_type", check.CheckTypeDetail);
                values.Add("Divd_expression", check.CheckExpression);
                values.Add("Divd_display_variable", check.CheckDisplayVariable);
                values.Add("Divd_variable", check.CheckVariable);
                values.Add("seq_no", check.SeqNoDetail);
                values.Add("Divd_Ditm_id", check.CheckItmId);
                values.Add("Divd_audit_flag", check.CheckAuditFlag);

                helper.InsertOperation("Dict_itm_verifi_detail", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDicItmCheckDetail check)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Divd_Dverifi_id", check.CheckIdDetial);
                values.Add("Divd_name", check.CheckNameDetail);
                values.Add("Divd_type", check.CheckTypeDetail);
                values.Add("Divd_expression", check.CheckExpression);
                values.Add("Divd_display_variable", check.CheckDisplayVariable);
                values.Add("Divd_variable", check.CheckVariable);
                values.Add("seq_no", check.SeqNoDetail);
                values.Add("Divd_Ditm_id", check.CheckItmId);
                values.Add("Divd_audit_flag", check.CheckAuditFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Divd_id", check.DetId);

                helper.UpdateOperation("Dict_itm_verifi_detail", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDicItmCheckDetail check)
        {
            try
            {
                DBManager helper = new DBManager();

                string checkID = check.CheckIdDetial;

                //Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("del_flag", "1");

                //Dictionary<string, object> keys = new Dictionary<string, object>();
                //keys.Add("check_id", check.CheckId);

                //helper.UpdateOperation("dic_itm_check", values, keys);

                string sql = string.Format(@" delete from Dict_itm_verifi_detail where  Dict_itm_verifi_detail.Divd_Dverifi_id='{0}'", checkID);

                helper.ExecSql(sql);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicItmCheckDetail> Search(Object obj)
        {
            try
            {
                string itrID = string.Empty;
                EntityDicItmCheck info = obj as EntityDicItmCheck;
                if (info != null && !string.IsNullOrEmpty(info.CheckItrId))
                {
                    itrID = info.CheckItrId;
                }

                String sql = string.Format(@"SELECT  Dict_itm_verifi_detail.Divd_id,
Dict_itm_verifi_detail.Divd_Dverifi_id,
Dict_itm_verifi_detail.Divd_name,
Dict_itm_verifi_detail.Divd_type,
Dict_itm_verifi_detail.Divd_expression, 
Dict_itm_verifi_detail.Divd_display_variable,
Dict_itm_verifi_detail.Divd_variable,
Dict_itm_verifi_detail.seq_no,
Dict_itm_verifi_detail.Divd_Ditm_id,
Dict_itm_verifi_detail.Divd_audit_flag,
0 as isNew,
Dict_itm.Ditm_name
FROM Dict_itm_verifi_detail 
INNER JOIN Dict_itm_verifi ON Dict_itm_verifi_detail.Divd_Dverifi_id=Dict_itm_verifi.Dverifi_id AND ISNULL(Dict_itm_verifi.del_flag,'')<>'1'
left join Dict_itm on Dict_itm.Ditm_id=Dict_itm_verifi_detail.Divd_Ditm_id
WHERE Dict_itm_verifi.Dverifi_Ditr_id='{0}'"
                                     , itrID);
                if (obj is string && obj.ToString() == "cache")
                {
                    sql = @"select Dict_itm_verifi_detail.*,Dict_itm_verifi.Dverifi_Ditr_id,Dict_itm.Ditm_name,Dict_itm.Ditm_ecode
from Dict_itm_verifi_detail 
JOIN Dict_itm_verifi ON Dict_itm_verifi.Dverifi_id=Dict_itm_verifi_detail.Divd_Dverifi_id
left join Dict_itm on Dict_itm.Ditm_id=Dict_itm_verifi_detail.Divd_Ditm_id";
                }
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicItmCheckDetail>();
            }
        }

        public List<EntityDicItmCheckDetail> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityDicItmCheckDetail> list = EntityManager<EntityDicItmCheckDetail>.ConvertToList(dtSour);
            return list.ToList();
        }
    }
}
