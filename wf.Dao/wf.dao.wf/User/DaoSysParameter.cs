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
    [Export("wf.plugin.wf", typeof(IDaoSysParameter))]
    public class DaoSysParameter : IDaoSysParameter
    {
        public List<EntitySysParameter> GetSysParaByConfigCode(string configCode)
        {
            try
            {

                String sql = string.Format(@"SELECT distinct * from Base_parameter where Bparm_code ='{0}'", configCode);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntitySysParameter>.ConvertToList(dt).OrderBy(i => i.ParmId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysParameter>();
            }
        }

        public List<EntitySysParameter> GetSysParaCache()
        {
            try
            {
                String sql = @"SELECT * from Base_parameter ";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntitySysParameter>.ConvertToList(dt).OrderBy(i => i.ParmId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysParameter>();
            }
        }
        public List<EntitySysParameter> GetSysParaByConfigType(string configType)
        {
            try
            {
                String sql = string.Format(@"SELECT * from Base_parameter where Bparm_type ='{0}'", configType);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntitySysParameter>.ConvertToList(dt).OrderBy(i => i.ParmId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysParameter>();
            }
        }

        public List<EntitySysParameter> GetSysParaByType(string parmType)
        {
            try
            {

                String sql = string.Format(@"SELECT * from Base_parameter where Bparm_type ='{0}'", parmType);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntitySysParameter>.ConvertToList(dt).OrderBy(i => i.ParmId).ToList();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysParameter>();
            }
        }

        public bool InsertSysPara(EntitySysParameter para)
        {
            try
            {

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Bparm_group", para.ParmGroup);
                values.Add("Bparm_field_name", para.ParmFieldName);
                values.Add("Bparm_field_type", para.ParmFieldType);
                values.Add("Bparm_field_value", para.ParmFieldValue);
                values.Add("Bparm_type", para.ParmType);
                values.Add("Bparm_dic_list", para.ParmDictList);
                values.Add("Bparm_code", para.ParmCode);

                helper.InsertOperation("Base_parameter", values);

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateSysParaByConfigId(EntitySysParameter para)
        {
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Bparm_field_value", para.ParmFieldValue);


                int updateFlag = 0;
                Dictionary<string, object> key = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(para.ParmCode))
                {
                    key.Add("Bparm_code", para.ParmCode);
                    updateFlag = helper.UpdateOperation("Base_parameter", values, key);
                }

                if (updateFlag == 0)
                {
                    key.Clear();
                    key.Add("Bparm_id", para.ParmId);
                    updateFlag = helper.UpdateOperation("Base_parameter", values, key);
                }
                return updateFlag >0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }
    }
}
