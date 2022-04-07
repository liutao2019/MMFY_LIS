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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicPubStatus>))]
    public class DaoDicPubStatus : IDaoDic<EntityDicPubStatus>
    {
        public bool Delete(EntityDicPubStatus sample)
        {
            throw new NotImplementedException();
        }

        public bool Save(EntityDicPubStatus sample)
        {
            throw new NotImplementedException();
        }

        public List<EntityDicPubStatus> Search(Object obj)
        {
            try
            {
                String sql = @"select * from Dict_status";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityDicPubStatus> list = EntityManager<EntityDicPubStatus>.ConvertToList(dt).OrderBy(i => i.StatusId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicPubStatus>();
            }
        }

        public bool Update(EntityDicPubStatus sample)
        {
            throw new NotImplementedException();
        }
    }
}
