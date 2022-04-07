using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoOaOrderTableField))]
    public class DaoOaOrderTableField : IDaoOaOrderTableField
    {

        public bool DeleteOrderTableFiled(EntityOaTableField sample)
        {
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> key = new Dictionary<string, object>();
                key.Add("Rof_code", sample.FieldCode);
                helper.DeleteOperation("Rel_oa_field", key);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteOrderTableFiledByTypeCode(string typeCode)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql = String.Format("delete from Rel_oa_field where Rof_Dot_code={0}", typeCode);
                helper.ExecSql(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntityOaTableField> GetOrderTableFiled(string tabcode)
        {
            try
            {
                if (!string.IsNullOrEmpty(tabcode))
                {
                    String sql = String.Format(@"select Rof_Dot_code,Rof_code,Rof_name,Rof_type,Rof_index,
Rof_list_display,Rof_dict_list,Rof_dict_sql from Rel_oa_field  Where Rof_Dot_code='" + tabcode + "'");
                    DBManager helper = new DBManager();

                    DataTable dt = helper.ExecuteDtSql(sql);
                    List<EntityOaTableField> list = EntityManager<EntityOaTableField>.ConvertToList(dt).OrderBy(i => i.FieldIndex).ToList();
                    return list;
                }
                else return new List<EntityOaTableField>();
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityOaTableField>();
            }
        }
        public bool SaveOrderTableFiled(EntityOaTableField sample)
        {
            try
            {

                DBManager helper = new DBManager();
                int id = IdentityHelper.GetMedIdentity("Rel_oa_field");
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rof_Dot_code", sample.TabCode);
                values.Add("Rof_code", id.ToString());
                values.Add("Rof_name", sample.FieldName);
                values.Add("Rof_type", sample.FieldType);
                values.Add("Rof_index", sample.FieldIndex);
                values.Add("Rof_list_display", sample.FieldListDisplay);
                values.Add("Rof_dict_list", sample.FieldDictList);
                values.Add("Rof_dict_sql", sample.FieldDictSql);
                helper.InsertOperation("Rel_oa_field", values);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateOrderTableFiled(EntityOaTableField sample)
        {
            try
            {

                DBManager helper = new DBManager();
                string sql = string.Format(@"update Rel_oa_field set Rof_Dot_code = '{0}',Rof_name = '{1}',
                                                           Rof_type = '{2}',Rof_index = '{3}',Rof_list_display = '{4}',Rof_dict_list = '{5}',Rof_dict_sql = '{6}' 
                                                           where Rof_code = '{7}'",
                                            sample.TabCode, sample.FieldName, sample.FieldType, sample.FieldIndex, sample.FieldListDisplay,sample.FieldDictList,sample.FieldDictSql, sample.FieldCode);
                helper.ExecSql(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool UpdateOrderTableFiledIndex(EntityOaTableField sample)
        {
            try
            {

                DBManager helper = new DBManager();
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("Rof_code", sample.FieldCode);
                values.Add("Rof_index", sample.FieldIndex);
                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("Rof_code", sample.FieldCode); 
                helper.UpdateOperation("Rel_oa_field", values, keys);
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
