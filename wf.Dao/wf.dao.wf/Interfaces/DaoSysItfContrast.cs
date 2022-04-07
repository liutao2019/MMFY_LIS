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
    [Export("wf.plugin.wf", typeof(IDaoSysItfContrast))]
    public class DaoSysContrast : IDaoSysItfContrast
    {
        public bool SaveSysContrast(EntitySysItfContrast con)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                int conId = IdentityHelper.GetMedIdentity("Base_itf_contrast");
                values.Add("Bitfcon_id", conId);
                values.Add("Bitfcon_Bitf_id", con.ContItfaceId);
                values.Add("Bitfcon_his_column", con.ContInterfaceColumn);
                values.Add("Bitfcon_column", con.ContSysColumn);
                values.Add("Bitfcon_column_rule", con.ContColumnRule);
                values.Add("Bitfcon_type", con.ContType);
                values.Add("Bitfcon_data_type", con.ContDataType);
                values.Add("Bitfcon_tablename", con.ContTablename);
                values.Add("Bitfcon_remark", con.ContRemark);
                values.Add("Bitfcon_search_seq", con.ContSearchSeq);
                values.Add("Bitfcon_script_for_search", con.ContScriptForSearch);
                values.Add("Bitfcon_search_para", con.ContSearchPara);
                values.Add("Bitfcon_search_para_text", con.ContSearchParaText);
                values.Add("Bitfcon_converter", con.ContConverter);
                helper.InsertOperation("Base_itf_contrast", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }

        }

        public bool UpdateSysContrast(EntitySysItfContrast con)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Bitfcon_Bitf_id", con.ContItfaceId);
                values.Add("Bitfcon_his_column", con.ContInterfaceColumn);
                values.Add("Bitfcon_column", con.ContSysColumn);
                values.Add("Bitfcon_column_rule", con.ContColumnRule);
                values.Add("Bitfcon_type", con.ContType);
                values.Add("Bitfcon_data_type", con.ContDataType);
                values.Add("Bitfcon_tablename", con.ContTablename);
                values.Add("Bitfcon_remark", con.ContRemark);
                values.Add("Bitfcon_search_seq", con.ContSearchSeq);
                values.Add("Bitfcon_script_for_search", con.ContScriptForSearch);
                values.Add("Bitfcon_search_para", con.ContSearchPara);
                values.Add("Bitfcon_search_para_text", con.ContSearchParaText);
                values.Add("Bitfcon_converter", con.ContConverter);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Bitfcon_id", con.ContId);
                helper.UpdateOperation("Base_itf_contrast", values, keys);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }

        }

        public bool DeleteSysContrast(int conId)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(conId.ToString()))
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sql = string.Format("delete Base_itf_contrast where Bitfcon_id={0} ", conId);

                    helper.ExecSql(sql);

                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }

            return result;
        }

        public List<EntitySysItfContrast> GetSysContrast(string par)
        {
            List<EntitySysItfContrast> result = new List<EntitySysItfContrast>();

            if (!string.IsNullOrEmpty(par))
            {
                try
                {
                    String sql = String.Format(@"SELECT Base_itf_contrast.* 
FROM Base_itf_contrast  
LEFT JOIN Base_itf_interface ON Base_itf_contrast.Bitfcon_Bitf_id=Base_itf_interface.Bitf_id 
where Bitfcon_Bitf_id='{0}'", par);
                    DBManager helper = new DBManager();
                    DataTable dt = helper.ExecuteDtSql(sql);
                    result = EntityManager<EntitySysItfContrast>.ConvertToList(dt).OrderBy(i => i.ContId).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }

            return result;

        }

        public List<EntitySysItfContrast> GetSysContrast()
        {
            List<EntitySysItfContrast> result = new List<EntitySysItfContrast>();
            try
            { 
                String sql = @"select * from Base_itf_contrast";
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                result = EntityManager<EntitySysItfContrast>.ConvertToList(dt).OrderBy(i => i.ContId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
    }
}
