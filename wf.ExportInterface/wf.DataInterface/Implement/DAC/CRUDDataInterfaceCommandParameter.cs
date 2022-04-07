using System.Collections.Generic;
using Lib.DAC;
using Lib.EntityCore;

namespace Lib.DataInterface.Implement
{
    public class CRUDDataInterfaceCommandParameter
    {
        public CRUDDataInterfaceCommandParameter()
        { }

        public List<EntityDictDataInterfaceCommandParameter> SelectAll()
        {
            EntityHelper dac = new EntityHelper();
            string sql = "select * from dict_DataInterfaceCommandParameter order by cmd_id asc, param_seq asc";
            List<EntityDictDataInterfaceCommandParameter> list = dac.SelectMany<EntityDictDataInterfaceCommandParameter>(sql);
            return list;
        }

        public List<EntityDictDataInterfaceCommandParameter> SelectAll(string cmd_id)
        {
            DbCommandEx cmd;
            EntityHelper dac = new EntityHelper();

            string sql = "select * from dict_DataInterfaceCommandParameter where cmd_id = ? order by param_seq asc";
            cmd = dac.CreateCommandEx(sql);
            cmd.AddParameterValue(cmd_id);

            List<EntityDictDataInterfaceCommandParameter> list = dac.SelectMany<EntityDictDataInterfaceCommandParameter>(cmd);
            return list;
        }


        internal void AddNewBatch(List<EntityDictDataInterfaceCommandParameter> list, ITransaction tran)
        {
            if (list == null)
                return;

            if (tran == null)
            {
                using (ITransaction t = DacEnviroment.BeginTransaction())
                {
                    EntityHelper dac = new EntityHelper(t);
                    foreach (EntityDictDataInterfaceCommandParameter item in list)
                    {
                        dac.Insert(item);
                    }
                    t.Commit();
                }
            }
            else
            {
                EntityHelper dac = new EntityHelper(tran);
                foreach (EntityDictDataInterfaceCommandParameter item in list)
                {
                    dac.Insert(item);
                }
            }
            //RefreshCache();
        }

    
        internal void DeleteBatch(string cmd_id, ITransaction tran)
        {
            SqlHelper helper = new SqlHelper(tran);
            DbCommandEx cmd = helper.CreateCommandEx("delete from dict_DataInterfaceCommandParameter where cmd_id = ?");
            cmd.AddParameterValue(cmd_id);
            helper.ExecuteNonQuery(cmd);

            //RefreshCache();
        }

        void RefreshCache()
        {
            //CacheDictDataInterfaceParameter.Current.Refresh();
        }
    }
}
