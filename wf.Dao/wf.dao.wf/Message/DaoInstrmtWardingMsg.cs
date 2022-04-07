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
    [Export("wf.plugin.wf", typeof(IDaoInstrmtWardingMsg))]
    class DaoInstrmtWardingMsg : IDaoInstrmtWardingMsg
    {
        public List<EntityInstrmtWarningMsg> CheckHasInstrmtWardMsgByPatItrId(string patItrId)
        {
            List<EntityInstrmtWarningMsg> listMsg = new List<EntityInstrmtWarningMsg>();
            if (!string.IsNullOrEmpty(patItrId))
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sql = string.Format(@"select 
                    instrmt_warning_msg.*, Dict_itm.Ditm_ecode 
                    from instrmt_warning_msg
                     left join Dict_itm on warn_item_id = Dict_itm.Ditm_id
                    where warn_pat_id = '{0}'", patItrId);
                    DataTable dt = helper.ExecuteDtSql(sql);
                    listMsg = EntityManager<EntityInstrmtWarningMsg>.ConvertToList(dt);
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return listMsg;
        }

        public bool DeleteInstrmtWardMsgByPatItrId(string patItrId)
        {
            bool result = false; 
            if (!string.IsNullOrEmpty(patItrId))
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sql = string.Format(@"delete instrmt_warning_msg where warn_pat_id = '{0}'", patItrId);
                    helper.ExecSql(sql);
                    result = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return result;
        }
    }
}
