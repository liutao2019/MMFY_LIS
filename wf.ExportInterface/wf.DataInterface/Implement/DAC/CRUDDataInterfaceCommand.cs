using System.Collections.Generic;
using Lib.DAC;
using Lib.EntityCore;

namespace Lib.DataInterface.Implement
{
    public class CRUDDataInterfaceCommand
    {
        public CRUDDataInterfaceCommand()
        {

        }

        public List<EntityDictDataInterfaceCommand> SelectAll()
        {
            EntityHelper dac = new EntityHelper();
            string sql = "select * from dict_DataInterfaceCommand order by cmd_exec_seq asc";
            List<EntityDictDataInterfaceCommand> list = dac.SelectMany<EntityDictDataInterfaceCommand>(sql);
            return list;
        }

        public List<EntityDictDataInterfaceCommand> SelectAll(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                List<EntityDictDataInterfaceCommand> list = SelectAll();
                return list;
            }
            else
            {
                DbCommandEx cmd;
                EntityHelper dac = new EntityHelper();
                string sql = "select * from dict_DataInterfaceCommand where sys_module = ? order by cmd_exec_seq asc";
                cmd = dac.CreateCommandEx(sql);
                cmd.AddParameterValue(moduleName);
                List<EntityDictDataInterfaceCommand> list = dac.SelectMany<EntityDictDataInterfaceCommand>(cmd);
                return list;
            }
        }

        public List<EntityDictDataInterfaceCommand> SelectByGroupName(string cmd_group)
        {
            DbCommandEx cmd;
            EntityHelper dac = new EntityHelper();
            string sql = "select * from dict_DataInterfaceCommand where cmd_group = ? order by cmd_exec_seq asc";
            cmd = dac.CreateCommandEx(sql);
            cmd.AddParameterValue(cmd_group);
            List<EntityDictDataInterfaceCommand> list = dac.SelectMany<EntityDictDataInterfaceCommand>(cmd);
            return list;
        }

        public EntityDictDataInterfaceCommand SelectByID(string cmd_id)
        {
            DbCommandEx cmd;
            EntityHelper dac = new EntityHelper();
            string sql = "select * from dict_DataInterfaceCommand where cmd_id = ?";
            cmd = dac.CreateCommandEx(sql);
            cmd.AddParameterValue(cmd_id);
            EntityDictDataInterfaceCommand cmdCfg = dac.SelectSingle<EntityDictDataInterfaceCommand>(cmd);
            return cmdCfg;
        }

        public void Insert(EntityDictDataInterfaceCommand item, List<EntityDictDataInterfaceCommandParameter> listParam)
        {
            using (ITransaction tran = DacEnviroment.BeginTransaction())
            {
                EntityHelper dac = new EntityHelper(tran);
                dac.UpdateWithChangeLog = false;
                dac.Insert(item);

                SaveParameters(item, listParam, tran);

                tran.Commit();
            }
            RefreshCache();
        }

        private void SaveParameters(EntityDictDataInterfaceCommand cmd, List<EntityDictDataInterfaceCommandParameter> list, ITransaction tran)
        {

            CRUDDataInterfaceCommandParameter biz = new CRUDDataInterfaceCommandParameter();
            if (list != null)
            {
                foreach (EntityDictDataInterfaceCommandParameter p in list)
                {
                    p.cmd_id = cmd.cmd_id;
                }
            }
            biz.DeleteBatch(cmd.cmd_id, tran);
            biz.AddNewBatch(list, tran);
        }

        public void Update(EntityDictDataInterfaceCommand item, List<EntityDictDataInterfaceCommandParameter> listParam)
        {
            using (ITransaction tran = DacEnviroment.BeginTransaction())
            {
                EntityHelper dac = new EntityHelper(tran);
                dac.UpdateWithChangeLog = false;
                int i = dac.Update(item);

                SaveParameters(item, listParam, tran);

                tran.Commit();
            }
            RefreshCache();
        }

        public void Delete(EntityDictDataInterfaceCommand item)
        {
            Delete(item.cmd_id);
        }

        public void Delete(string cmd_id)
        {
            string sqlDelCmd = "delete from dict_DataInterfaceCommand where cmd_id = ?";
            string sqlDelCmdParam = "delete from dict_DataInterfaceCommandParameter where cmd_id = ?";
            SqlHelper helper = new SqlHelper();
            DbCommandEx cmd1 = helper.CreateCommandEx(sqlDelCmd);
            cmd1.AddParameterValue(cmd_id);

            DbCommandEx cmd2 = helper.CreateCommandEx(sqlDelCmdParam);
            cmd2.AddParameterValue(cmd_id);

            using (ITransaction tran = helper.BeginTransaction())
            {
                helper.ExecuteNonQuery(cmd1);
                helper.ExecuteNonQuery(cmd2);
                tran.Commit();
            }

            RefreshCache();
        }

        public void Save(EntityDictDataInterfaceCommand item, List<EntityDictDataInterfaceCommandParameter> listParam)
        {
            if (item == null)
                return;

            if (string.IsNullOrEmpty(item.cmd_id))
            {
                Insert(item, listParam);
            }
            else
            {
                Update(item, listParam);
            }
        }

        void RefreshCache()
        {
            CacheDirectDBDataInterface.Current.RefreshCommand();
            CacheDirectDBDataInterface.Current.RefreshParameter();
        }
    }
}
