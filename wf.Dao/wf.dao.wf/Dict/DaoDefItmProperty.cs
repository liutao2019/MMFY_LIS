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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDefItmProperty>))]
    public class DaoDefItmProperty : IDaoDic<EntityDefItmProperty>
    {
        public bool Save(EntityDefItmProperty prop)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Rel_itm_property");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rproty_id", id);
                values.Add("Rproty_Ditm_id", prop.PtyItmId);
                values.Add("Rproty_Ditm_ecode", prop.PtyItmEname);
                values.Add("Rproty_name", prop.PtyItmProperty);
                values.Add("Rproty_c_code", prop.PtyCCode);
                values.Add("Rproty_Dpro_id", prop.PtyProId);
                values.Add("py_code", prop.PtyPyCode);
                values.Add("wb_code", prop.PtyWbCode);
                values.Add("sort_no", prop.PtySortNo);
                values.Add("Rproty_flag", prop.PtyItmFlag);

                helper.InsertOperation("Rel_itm_property", values);

                prop.PtyId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Update(EntityDefItmProperty prop)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rproty_Ditm_id", prop.PtyItmId);
                values.Add("Rproty_Ditm_ecode", prop.PtyItmEname);
                values.Add("Rproty_name", prop.PtyItmProperty);
                values.Add("Rproty_c_code", prop.PtyCCode);
                values.Add("Rproty_Dpro_id", prop.PtyProId);
                values.Add("py_code", prop.PtyPyCode);
                values.Add("wb_code", prop.PtyWbCode);
                values.Add("sort_no", prop.PtySortNo);
                values.Add("Rproty_flag", prop.PtyItmFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rproty_id", prop.PtyId);

                helper.UpdateOperation("Rel_itm_property", values, keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(EntityDefItmProperty prop)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rproty_id", prop.PtyId);

                helper.DeleteOperation("Rel_itm_property", keys);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDefItmProperty> Search(Object obj) 
        {
            try
            {
                string where = string.Empty;
                String sql = string.Empty;
                if (obj != null && obj is string)
                {
                    sql = string.Format(@"SELECT Rel_itm_property.*
                                      FROM Rel_itm_property  where Rel_itm_property.Rproty_Ditm_id='{0}'", obj.ToString());
                }
                else if (obj is List<string>)
                {
                    List<string> listStr = obj as List<string>;
                    if (!string.IsNullOrEmpty(listStr[0]))
                    {
                        where = string.Format(" where Rproty_flag=1 or Rproty_Ditm_id='{0}'", listStr[1]);
                    }
                    else
                    {
                        where = string.Format(" where Rproty_flag=1");
                    }
                    sql = string.Format("select Rproty_id,Rproty_name,Dict_itm.Ditm_name from Rel_itm_property left join Dict_itm on Rel_itm_property.Rproty_Ditm_id = Dict_itm.Ditm_id {0} ", where);
                }
                else
                {
                    sql = "select * from Rel_itm_property";
                }

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                List<EntityDefItmProperty> list = EntityManager<EntityDefItmProperty>.ConvertToList(dt).OrderBy(i => i.PtySortNo).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDefItmProperty>();
            }
        }
    }
}
