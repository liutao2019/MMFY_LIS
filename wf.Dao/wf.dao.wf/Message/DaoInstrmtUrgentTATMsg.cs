using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoInstrmtUrgentTATMsg))]
    public class DaoInstrmtUrgentTATMsg : IDaoInstrmtUrgentTATMsg
    {
        public List<EntityDicMsgTAT> GetInstrmtUrgentMsgToCacheDao()
        {
            List<EntityDicMsgTAT> listInstrmtMsg = new List<EntityDicMsgTAT>();
            try
            {
                DeleteItrUrgentMsgData();//删除24小时后的数据

                string strSQL = @"select Pat_lis_main.Pma_rep_id,
Pat_lis_main.Pma_Ditr_id,
Pat_lis_main.Pma_sid,
Pat_lis_main.Pma_serial_num,
Pat_lis_main.Pma_pat_name,
Pat_lis_main.Pma_in_date,
Pat_lis_main.Pma_status,
Dict_itr_instrument.Ditr_ename,
Dict_itr_instrument.Ditr_lab_id itr_type, --物理组 
Dict_profession.Dpro_name
from Lis_message_main
inner join Pat_lis_main on Pat_lis_main.Pma_rep_id=Lis_message_main.Lmsgmain_Pma_rep_id and Pat_lis_main.Pma_status=0
left join Dict_itr_instrument on Pat_lis_main.Pma_Ditr_id=Dict_itr_instrument.Ditr_id
left join Dict_profession on Dict_itr_instrument.Ditr_lab_id=Dict_profession.Dpro_id
where Lis_message_main.Lmsgmain_create_date>=Dateadd(day,-1,getdate()) and Lis_message_main.Lmsgmain_create_date<=getdate()";

                //dtbResult.TableName = "ItrUrgentMsgCache";
                DBManager helper = new DBManager();
                DataTable dtInstrment = helper.ExecuteDtSql(strSQL);

                listInstrmtMsg = EntityManager<EntityDicMsgTAT>.ConvertToList(dtInstrment).OrderBy(i => i.RepId).ToList();
            }
            catch (Exception objEx)
            {
                //throw;
                Lib.LogManager.Logger.LogException("获取仪器危急值数据", objEx);
            }
            return listInstrmtMsg;
        }

        /// <summary>
        /// 删除24小时后的数据
        /// </summary>
        public void DeleteItrUrgentMsgData()
        {
            try
            {
                DBManager helper = new DBManager();

                string sqlDelete = @"delete from Lis_message_main where Lmsgmain_create_date<Dateadd(day,-1,getdate())";

                helper.ExecSql(sqlDelete);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("删除24小时后的仪器危急值信息", ex);
            }
        }

        public bool DeleteItrUrgentMsgDataByID(string msg_id)
        {
            bool isDelItr = false;

            //如果ID为空返回失败
            if (string.IsNullOrEmpty(msg_id)) return isDelItr;
            try
            {
                DBManager helper = new DBManager();

                string sqlDelete = string.Format(@"delete from Lis_message_main where Lmsgmain_Pma_rep_id='{0}'", msg_id);
                helper.ExecSql(sqlDelete);
                
                isDelItr = true; 
            }
            catch (Exception objEx)
            {
                Lib.LogManager.Logger.LogException("删除仪器危急值信息", objEx);
                return false;
            }

            return isDelItr;
        }

    }
}
