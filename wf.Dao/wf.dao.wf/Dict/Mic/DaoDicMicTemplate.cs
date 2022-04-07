using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicMicTemplate>))]
    public class DaoDicMicTemplate : IDaoDic<EntityDicMicTemplate>
    {
        public bool Delete(EntityDicMicTemplate temp)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", 1);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dtmp_id", temp.TmpId);

                helper.UpdateOperation("Dict_mic_template", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicMicTemplate temp)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_mic_template");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dtmp_id", id);
                values.Add("Dtmp_type", temp.TmpType);
                values.Add("Dtmp_content", temp.TmpContent);
                values.Add("Dtmp_negative_flag", temp.TmpNegativeFlag);
                values.Add("Dtmp_remark", temp.TmpRemark);
                values.Add("del_flag", temp.DelFlag);
                values.Add("sort_no", temp.SortNo);
                values.Add("Dtmp_Dcom_id", temp.TmpComId);

                helper.InsertOperation("Dict_mic_template", values);

                temp.TmpId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicMicTemplate> Search(object obj)
        {
            try
            {
                string sql = string.Empty;
                if (obj is string && obj.ToString() == "cache")
                {
                    sql = "SELECT * from Dict_mic_template WHERE del_flag = 0 ";
                }
                else
                {
                    sql = string.Format("select * from Dict_mic_template ");

                    if (obj is string && obj != null)
                    {
                        sql += string.Format("  where Dtmp_type='{0}'", obj.ToString());
                    }
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicMicTemplate>.ToList(dt).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMicTemplate>();
            }
        }

        public bool Update(EntityDicMicTemplate temp)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dtmp_type", temp.TmpType);
                values.Add("Dtmp_content", temp.TmpContent);
                values.Add("Dtmp_negative_flag", temp.TmpNegativeFlag);
                values.Add("Dtmp_remark", temp.TmpRemark);
                values.Add("del_flag", temp.DelFlag);
                values.Add("sort_no", temp.SortNo);
                values.Add("Dtmp_Dcom_id", temp.TmpComId);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dtmp_id", temp.TmpId);

                helper.UpdateOperation("Dict_mic_template", values, keys);
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