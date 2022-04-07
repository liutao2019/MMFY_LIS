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
    [Export("wf.plugin.wf", typeof(IDaoTatOverTime))]
    public class DaoTatOverTime : IDaoTatOverTime
    {
        public List<EntityTatOverTime> GetTatOverTime(string barCode)
        {
            List<EntityTatOverTime> list = new List<EntityTatOverTime>();
            if (!string.IsNullOrEmpty(barCode))
            {
                try
                {
                    string sql = string.Format(@"select * from Lis_tat_overtime_data where Ltat_Sma_bar_id='{0}'", barCode);
                    DBManager helper = new DBManager();
                    DataTable dt = helper.ExecuteDtSql(sql);
                    list = EntityManager<EntityTatOverTime>.ConvertToList(dt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return list;
        }

        public bool SaveTatOverTime(EntityTatOverTime overTime)
        {
            bool result = false;
            try
            {
                DBManager helper = new DBManager();
                Dictionary<string, object> value = helper.ConverToDBSaveParameter<EntityTatOverTime>(overTime);

                result = helper.InsertOperation("Lis_tat_overtime_data", value) > 0;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return result;
        }
    }
}
