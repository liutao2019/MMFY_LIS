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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicMicAntitype>))]
    public class DaoDicMicAntitype : IDaoDic<EntityDicMicAntitype>
    {
        public bool Delete(EntityDicMicAntitype antiType)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dantitype_id", antiType.AtypeId);

                helper.UpdateOperation("Dic_mic_antitype", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicMicAntitype antiType)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dic_mic_antitype");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dantitype_id", id);
                values.Add("Dantitype_name", antiType.AtypeName);
                values.Add("Dantitype_c_code", antiType.ACCode);
                values.Add("Dantitype_code", antiType.AtypeCode);
                values.Add("py_code", antiType.APyCode);
                values.Add("wb_code", antiType.AWbCode);
                values.Add("sort_no", antiType.ASortNo);
                values.Add("del_flag", antiType.ADelFlag);

                helper.InsertOperation("Dic_mic_antitype", values);

                antiType.AtypeId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicMicAntitype> Search(object obj)
        {
            try
            {
                String sql = @"SELECT * FROM Dic_mic_antitype WHERE del_flag = 0";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicMicAntitype>.ConvertToList(dt).OrderBy(i => i.ASortNo).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMicAntitype>();
            }
        }

        public bool Update(EntityDicMicAntitype antiType)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dantitype_name", antiType.AtypeName);
                values.Add("Dantitype_c_code", antiType.ACCode);
                values.Add("Dantitype_code", antiType.AtypeCode);
                values.Add("py_code", antiType.APyCode);
                values.Add("wb_code", antiType.AWbCode);
                values.Add("sort_no", antiType.ASortNo);
                values.Add("del_flag", antiType.ADelFlag);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Dantitype_id", antiType.AtypeId);

                helper.UpdateOperation("Dic_mic_antitype", values, keys);

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
