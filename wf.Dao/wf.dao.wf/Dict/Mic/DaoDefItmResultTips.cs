using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDefItmResultTips>))]
    public class DaoDefItmResultTips : IDaoDic<EntityDefItmResultTips>
    {
        public bool Delete(EntityDefItmResultTips sample)
        {
            throw new NotImplementedException();
        }

        public bool Save(EntityDefItmResultTips sample)
        {
            throw new NotImplementedException();
        }

        public List<EntityDefItmResultTips> Search(Object obj)
        {
            try
            {
                string sql = @"SELECT * FROM Rel_itm_result_tips WHERE del_flag = 0";
                DBManager helper = new DBManager();
                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityDefItmResultTips> list = EntityManager<EntityDefItmResultTips>.ToList(dt);
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDefItmResultTips>();
            }
        }

        public bool Update(EntityDefItmResultTips sample)
        {
            throw new NotImplementedException();
        }
    }
}