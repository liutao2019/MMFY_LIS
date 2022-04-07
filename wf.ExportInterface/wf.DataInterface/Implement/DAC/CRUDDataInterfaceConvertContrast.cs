using System.Collections.Generic;
using Lib.DAC;
using Lib.EntityCore;

namespace Lib.DataInterface.Implement
{
    public class CRUDDataInterfaceConvertContrast
    {
        public CRUDDataInterfaceConvertContrast()
        { }

        public List<EntityDictDataConvertContrast> SelectAll()
        {
            EntityHelper dac = new EntityHelper();
            string sql = "select * from dict_DataConvertContrast order by con_seq asc";
            List<EntityDictDataConvertContrast> list = dac.SelectMany<EntityDictDataConvertContrast>(sql);
            return list;
        }

        public List<EntityDictDataConvertContrast> SelectAll(string rule_id)
        {
            DbCommandEx cmd;
            EntityHelper dac = new EntityHelper();

            string sql = "select * from dict_DataConvertContrast where rule_id = ? order by con_seq asc";
            cmd = dac.CreateCommandEx(sql);
            cmd.AddParameterValue(rule_id);

            List<EntityDictDataConvertContrast> list = dac.SelectMany<EntityDictDataConvertContrast>(cmd);
            return list;
        }

        internal void AddNewBatch(List<EntityDictDataConvertContrast> list, ITransaction tran)
        {
            if (list == null)
                return;

            if (tran == null)
            {
                using (ITransaction t = DacEnviroment.BeginTransaction())
                {
                    EntityHelper dac = new EntityHelper(t);
                    foreach (EntityDictDataConvertContrast item in list)
                    {
                        dac.Insert(item);
                    }
                    t.Commit();
                }
            }
            else
            {
                EntityHelper dac = new EntityHelper(tran);
                foreach (EntityDictDataConvertContrast item in list)
                {
                    dac.Insert(item);
                }
            }
        }


        internal void DeleteBatch(string rule_id, ITransaction tran)
        {
            SqlHelper helper = new SqlHelper(tran);
            DbCommandEx cmd = helper.CreateCommandEx("delete from dict_DataConvertContrast where rule_id = ?");
            cmd.AddParameterValue(rule_id);
            helper.ExecuteNonQuery(cmd);

        }

    }
}
