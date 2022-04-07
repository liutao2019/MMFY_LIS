using System.Collections.Generic;
using Lib.DAC;
using Lib.EntityCore;

namespace Lib.DataInterface.Implement
{
    public class CRUDDataInterfaceConnection
    {
        public CRUDDataInterfaceConnection()
        {

        }

        public List<EntityDictDataInterfaceConnection> SelectAll()
        {
            EntityHelper dac = new EntityHelper();
            string sql = "select * from dict_DataInterfaceConnection";
            List<EntityDictDataInterfaceConnection> list = dac.SelectMany<EntityDictDataInterfaceConnection>(sql);
            return list;
        }

        public List<EntityDictDataInterfaceConnection> SelectAll(string moduleName)
        {

            if (string.IsNullOrEmpty(moduleName))
            {
                List<EntityDictDataInterfaceConnection> list = SelectAll();
                return list;
            }
            else
            {
                DbCommandEx cmd;
                EntityHelper dac = new EntityHelper();
                string sql = "select * from dict_DataInterfaceConnection where sys_module = ?";
                cmd = dac.CreateCommandEx(sql);
                cmd.AddParameterValue(moduleName);

                List<EntityDictDataInterfaceConnection> list = dac.SelectMany<EntityDictDataInterfaceConnection>(cmd);
                return list;
            }
        }

        public EntityDictDataInterfaceConnection SelectByID(string conn_id)
        {
            DbCommandEx cmd;
            EntityHelper dac = new EntityHelper();
            string sql = "select * from dict_DataInterfaceConnection where conn_id = ?";
            cmd = dac.CreateCommandEx(sql);
            cmd.AddParameterValue(conn_id);

            EntityDictDataInterfaceConnection connCfg = dac.SelectSingle<EntityDictDataInterfaceConnection>(cmd);
            return connCfg;
        }


        private void Insert(EntityDictDataInterfaceConnection item)
        {
            new EntityHelper().Insert(item);

            RefreshCache();
        }

        private void Update(EntityDictDataInterfaceConnection item)
        {
            EntityHelper dac = new EntityHelper();
            dac.UpdateWithChangeLog = false;
            int i = dac.Update(item);

            RefreshCache();
        }

        public void Save(EntityDictDataInterfaceConnection item)
        {
            if (string.IsNullOrEmpty(item.conn_id))
            {
                Insert(item);
            }
            else
            {
                Update(item);
            }
        }

        public void Delete(EntityDictDataInterfaceConnection item)
        {
            EntityHelper dac = new EntityHelper();
            dac.Delete(item);

            RefreshCache();
        }

        void RefreshCache()
        {
            CacheDirectDBDataInterface.Current.RefreshConnection();
        }
    }
}
