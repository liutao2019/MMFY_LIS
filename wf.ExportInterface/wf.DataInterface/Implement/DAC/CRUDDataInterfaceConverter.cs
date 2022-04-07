using System.Collections.Generic;
using Lib.DAC;
using Lib.EntityCore;

namespace Lib.DataInterface.Implement
{
    public class CRUDDataInterfaceConverter
    {
        public CRUDDataInterfaceConverter()
        {

        }

        public List<EntityDictDataConverter> SelectAll()
        {
            DbCommandEx cmd;
            EntityHelper dac = new EntityHelper();

            string sql = "select * from dict_DataConverter order by rule_seq asc";
            cmd = dac.CreateCommandEx(sql);

            List<EntityDictDataConverter> list = dac.SelectMany<EntityDictDataConverter>(cmd);
            return list;
        }

        public List<EntityDictDataConverter> SelectAll(string moduleName)
        {

            if (string.IsNullOrEmpty(moduleName))
            {
                List<EntityDictDataConverter> list = SelectAll();
                return list;
            }
            else
            {
                DbCommandEx cmd;
                EntityHelper dac = new EntityHelper();
                string sql = "select * from dict_DataConverter where sys_module = ? order by rule_seq asc";
                cmd = dac.CreateCommandEx(sql);
                cmd.AddParameterValue(moduleName);

                List<EntityDictDataConverter> list = dac.SelectMany<EntityDictDataConverter>(cmd);
                return list;
            }
        }

        public EntityDictDataConverter SelectByID(string rule_id)
        {
            string sql = "select * from dict_DataConverter where rule_id = ?";
            EntityHelper dac = new EntityHelper();
            DbCommandEx cmd = dac.CreateCommandEx(sql);
            cmd.AddParameterValue(rule_id, System.Data.DbType.AnsiString);

            EntityDictDataConverter rule = dac.SelectSingle<EntityDictDataConverter>(cmd);
            return rule;
        }


        private void SaveContrast(EntityDictDataConverter converter, List<EntityDictDataConvertContrast> list, ITransaction tran)
        {
            CRUDDataInterfaceConvertContrast biz = new CRUDDataInterfaceConvertContrast();

            if (list != null)
            {
                foreach (EntityDictDataConvertContrast c in list)
                {
                    c.rule_id = converter.rule_id;
                }
            }

            if (!string.IsNullOrEmpty(converter.rule_id))
                biz.DeleteBatch(converter.rule_id, tran);

            biz.AddNewBatch(list, tran);
        }

        public void Save(EntityDictDataConverter item, List<EntityDictDataConvertContrast> listContrast)
        {
            if (item == null)
                return;

            using (ITransaction tran = DacEnviroment.BeginTransaction())
            {
                EntityHelper dac = new EntityHelper(tran);
                dac.UpdateWithChangeLog = false;

                if (string.IsNullOrEmpty(item.rule_id))
                {
                    //新增
                    dac.Insert(item);
                }
                else
                {
                    //修改
                    dac.Update(item);
                }

                SaveContrast(item, listContrast, tran);
                tran.Commit();
            }
            RefreshCache();
        }

        #region delete

        public void Delete(EntityDictDataConverter item)
        {
            Delete(item.rule_id);
        }

        public void Delete(string rule_id)
        {
            string sqlDelCmd = "delete from dict_DataConverter where rule_id = ?";
            string sqlDelCmdParam = "delete from dict_DataConvertContrast where rule_id = ?";
            SqlHelper helper = new SqlHelper();
            DbCommandEx cmd1 = helper.CreateCommandEx(sqlDelCmd);
            cmd1.AddParameterValue(rule_id);

            DbCommandEx cmd2 = helper.CreateCommandEx(sqlDelCmdParam);
            cmd2.AddParameterValue(rule_id);

            using (ITransaction tran = helper.BeginTransaction())
            {
                helper.ExecuteNonQuery(cmd1);
                helper.ExecuteNonQuery(cmd2);
                tran.Commit();
            }

            RefreshCache();
        }
        #endregion


        void RefreshCache()
        {
            CacheDirectDBDataInterface.Current.RefreshConverter();
        }
    }
}
