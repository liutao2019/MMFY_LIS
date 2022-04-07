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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityReportStat>))]
    public class DaoReportStat : IDaoDic<EntityReportStat>
    { 
        public bool Delete(EntityReportStat sample)
        {
            throw new NotImplementedException();
        }

        public bool Save(EntityReportStat sample)
        {
            throw new NotImplementedException();
        }

        public List<EntityReportStat> Search(object obj)
        {
            try
            {
                String sql = String.Format(@"select * from dict_report_stat");
                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);
                List<EntityReportStat> list = EntityManager<EntityReportStat>.ConvertToList(dt).OrderBy(i => i.RepId).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityReportStat>();
            }
        }

        public bool Update(EntityReportStat sample)
        {
            throw new NotImplementedException();
        }
    }
}
