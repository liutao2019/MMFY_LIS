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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicResultTips>))]
    public class DaoResRefFlag : IDaoDic<EntityDicResultTips>
    {
        public bool Delete(EntityDicResultTips tips)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rtip_id", tips.TipId);

                helper.UpdateOperation("Rel_itm_result_tips", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicResultTips tips)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_itm_result_tips");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rtip_id", id);
                values.Add("Rtip_value", tips.TipValue);
                values.Add("del_flag", tips.DelFlag);
                values.Add("Rtip_content", tips.TipContent);

                helper.InsertOperation("Rel_itm_result_tips", values);

                tips.TipId = id;

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicResultTips> Search(object obj)
        {
            try
            {
                String sql = string.Format("SELECT Rtip_id,Rtip_value,del_flag,Rtip_content FROM Rel_itm_result_tips");

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityDicResultTips> list = new List<EntityDicResultTips>();
                foreach (DataRow item in dt.Rows)
                {
                    EntityDicResultTips ItmCom = new EntityDicResultTips();
                    if (item["Rtip_id"] != null)
                        ItmCom.TipId = int.Parse(item["Rtip_id"].ToString());
                    if (item["Rtip_value"] != null)
                        ItmCom.TipValue = item["Rtip_value"].ToString();
                    if (item["Rtip_content"] != null)
                        ItmCom.TipContent = item["Rtip_content"].ToString();
                    if ((bool)item["del_flag"] == false)
                        ItmCom.DelFlag = 0;
                    if ((bool)item["del_flag"] == true)
                        ItmCom.DelFlag = 1;
                    ItmCom.Checked = false;
                    list.Add(ItmCom);
                }
                return list.OrderBy(i => i.TipId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicResultTips>();
            }
        }

        public bool Update(EntityDicResultTips tips)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rtip_value", tips.TipValue);
                values.Add("del_flag", tips.DelFlag);
                values.Add("Rtip_content", tips.TipContent);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rtip_id", tips.TipId);

                helper.UpdateOperation("Rel_itm_result_tips", values, keys);

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
