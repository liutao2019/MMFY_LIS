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
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityDicRealTimeReportStat>))]
    class DaoDicRealTimeReportStat : IDaoDic<EntityDicRealTimeReportStat>
    {
        public bool Delete(EntityDicRealTimeReportStat sample)
        {
            throw new NotImplementedException();
        }

        public bool Save(EntityDicRealTimeReportStat sample)
        {
            throw new NotImplementedException();
        }

        public List<EntityDicRealTimeReportStat> Search(object obj)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql = "select * from Dict_realtimereport_stat";
                DataTable dtData = helper.ExecSel(sql);
                List<EntityDicRealTimeReportStat> list =
                    EntityManager<EntityDicRealTimeReportStat>.ConvertToList(dtData).OrderBy(i => i.SornNo).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicRealTimeReportStat>();
            }
        }

        public bool Update(EntityDicRealTimeReportStat sample)
        {
            throw new NotImplementedException();
        }
    }
}
