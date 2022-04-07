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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicSampStoreStatus>))]
    public class DaoDicSampStoreStatus : IDaoDic<EntityDicSampStoreStatus>
    {
        public bool Delete(EntityDicSampStoreStatus sample)
        {
            throw new NotImplementedException();
        }

        public bool Save(EntityDicSampStoreStatus sample)
        {
            throw new NotImplementedException();
        }

        public List<EntityDicSampStoreStatus> Search(object obj)
        {
            try
            {
                String sql = string.Format(@"select * from Dict_samp_store_status");

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return EntityManager<EntityDicSampStoreStatus>.ConvertToList(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicSampStoreStatus>();
            }
        }

        public bool Update(EntityDicSampStoreStatus sample)
        {
            throw new NotImplementedException();
        }
    }
}
