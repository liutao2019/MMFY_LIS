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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicMicBacttype>))]
    public class DaoDicMicBacttype : IDaoDic<EntityDicMicBacttype>
    {
        public bool Delete(EntityDicMicBacttype bactType)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("del_flag", "1");

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dbactt_id", bactType.BtypeId);

                helper.UpdateOperation("Dict_mic_bacttype", values, key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Save(EntityDicMicBacttype bactType)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_mic_bacttype");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dbactt_id", id);
                values.Add("Dbactt_ename", bactType.BtypeEname);
                values.Add("Dbactt_cname", bactType.BtypeCname);
                values.Add("Dbactt_Dantitype_id", bactType.BtypeAtypeId);
                values.Add("Dbactt_code", bactType.BtypeCode);
                values.Add("Dbactt_c_code", bactType.BtypeCCode);
                values.Add("py_code", bactType.BtypePyCode);
                values.Add("wb_code", bactType.BtypeWbCode);
                values.Add("sort_no", bactType.BtypeSortNo);
                values.Add("del_flag", bactType.BtypeDelFlag);
                values.Add("Dbactt_type", bactType.BtypeType);

                helper.InsertOperation("Dict_mic_bacttype", values);

                bactType.BtypeId = id.ToString();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityDicMicBacttype> Search(object obj)
        {
            try
            {
                String sql = @"SELECT  Dict_mic_bacttype.*, Dic_mic_antitype.Dantitype_name
FROM     Dict_mic_bacttype left OUTER JOIN
Dic_mic_antitype ON Dict_mic_bacttype.Dbactt_Dantitype_id = Dic_mic_antitype.Dantitype_id WHERE Dict_mic_bacttype.del_flag=0";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicMicBacttype>.ConvertToList(dt).OrderBy(i => i.BtypeSortNo).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicMicBacttype>();
            }
        }

        public bool Update(EntityDicMicBacttype bactType)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Dbactt_ename", bactType.BtypeEname);
                values.Add("Dbactt_cname", bactType.BtypeCname);
                values.Add("Dbactt_Dantitype_id", bactType.BtypeAtypeId);
                values.Add("Dbactt_code", bactType.BtypeCode);
                values.Add("Dbactt_c_code", bactType.BtypeCCode);
                values.Add("py_code", bactType.BtypePyCode);
                values.Add("wb_code", bactType.BtypeWbCode);
                values.Add("sort_no", bactType.BtypeSortNo);
                values.Add("del_flag", bactType.BtypeDelFlag);
                values.Add("Dbactt_type", bactType.BtypeType);

                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Dbactt_id", bactType.BtypeId);

                helper.UpdateOperation("Dict_mic_bacttype", values, key);
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
