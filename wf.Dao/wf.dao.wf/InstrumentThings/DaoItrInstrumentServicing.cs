using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using dcl.common;

namespace dcl.dao.clab
{
    [Export("wf.plugin.wf", typeof(IDaoItrInstrumentServicing))]
    public class DaoItrInstrumentServicing : IDaoItrInstrumentServicing
    {
        public List<EntityDicItrInstrumentServicing> GetServicing(string itrId)
        {
            try
            {
                string strSQL = string.Format(@"select Dis_id, Dis_Ditr_id, Dis_content,
poweruserinfo1.Buser_name Dis_putin_code, 
Dis_putin_date,poweruserinfo2.Buser_name Dis_handler_code, 
Dis_handle_date, Dis_handle_result,
poweruserinfo3.Buser_name Dis_chk_code, Dis_chk_date, 
Dis_chk_content, Dis_price, Dis_interval
from Dict_instrmt_servicing
left join Base_user as poweruserinfo1 on poweruserinfo1.Buser_loginid=Dict_instrmt_servicing.Dis_putin_code
left join Base_user as poweruserinfo2 on poweruserinfo2.Buser_loginid=Dict_instrmt_servicing.Dis_handler_code
left join Base_user as poweruserinfo3 on poweruserinfo3.Buser_loginid=Dict_instrmt_servicing.Dis_chk_code 
where Dis_Ditr_id ={0}   "
                                          , itrId);

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(strSQL);

                List<EntityDicItrInstrumentServicing> list = EntityManager<EntityDicItrInstrumentServicing>.ConvertToList(dt).OrderBy(i => i.SerId).ToList();

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicItrInstrumentServicing>();
            }
        }

        public List<EntityDicItrInstrumentServicing> GetServicingData(EntityDicItrInstrumentServicing strWhere)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append(@"select Ditr_ename,Dis_id, Dis_Ditr_id,Dpro_name, Dis_content,
poweruserinfo1.Buser_name Dis_putin_code, Dis_putin_date,
poweruserinfo2.Buser_name Dis_handler_code, Dis_handle_date, 
Dis_handle_result,poweruserinfo3.Buser_name Dis_chk_code, 
Dis_chk_date, Dis_chk_content, Dis_price, Dis_interval
from Dict_instrmt_servicing
left join Dict_itr_instrument on Dict_instrmt_servicing.Dis_Ditr_id=Dict_itr_instrument.Ditr_id
left join Base_user as poweruserinfo1 on poweruserinfo1.Buser_loginid=Dict_instrmt_servicing.Dis_putin_code
left join Base_user as poweruserinfo2 on poweruserinfo2.Buser_loginid=Dict_instrmt_servicing.Dis_handler_code
left join Base_user as poweruserinfo3 on poweruserinfo3.Buser_loginid=Dict_instrmt_servicing.Dis_chk_code 
left join Dict_profession on Dict_itr_instrument.Ditr_lab_id=Dict_profession.Dpro_id     ");
                strSql.Append(" where");
                strSql.Append(" Dis_putin_date>='" + strWhere.DeStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                strSql.Append(" and Dis_putin_date<='" + strWhere.DeEndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                if (strWhere.SerItrId != null && strWhere.SerItrId.Trim() != string.Empty)
                    strSql.Append(" and Dis_Ditr_id='" + strWhere.SerItrId.Trim() + "'");
                if (strWhere.StrCtypeId != null && strWhere.StrCtypeId.Trim() != string.Empty)
                    strSql.Append(" and Dict_profession.Dpro_id='" + strWhere.StrCtypeId.Trim() + "'");


                DBManager helper = new DBManager();

                DataTable dtServicing = helper.ExecuteDtSql(strSql.ToString());

                List<EntityDicItrInstrumentServicing> list = EntityManager<EntityDicItrInstrumentServicing>.ConvertToList(dtServicing).OrderBy(i => i.SerId).ToList();

                //DataTable dtServicing = helper.GetTable(strSql.ToString());

                return list;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityDicItrInstrumentServicing>();
            }
        }
        
        public bool ServicingPutIn(EntityDicItrInstrumentServicing servicing)
        {
            try
            {
                int id = IdentityHelper.GetMedIdentity("Dict_instrmt_servicing");

                string sqlStr = string.Format(@"insert into Dict_instrmt_servicing(Dis_id,Dis_Ditr_id,Dis_content,Dis_putin_code,Dis_putin_date)
                                                       values ({0},'{1}','{2}','{3}','{4}') ",
                                        id, servicing.SerItrId, servicing.SerContent, servicing.SerPutinCode, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                DBManager helper = new DBManager();

                int rows = helper.ExecCommand(sqlStr);

                string strUpdateInstrmtStatus = string.Format("update Dict_itr_instrument set Ditr_status='故障' where Ditr_id='{0}' ", servicing.SerItrId);
                rows += helper.ExecCommand(strUpdateInstrmtStatus);

                new DaoItrInstrumentRegistration().DelMes(0, servicing.SerItrId);

                new DaoItrInstrumentRegistration().AddMes(0, DateTime.Now, servicing.SerItrId);

                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool ServingAudit(EntityDicItrInstrumentServicing servicing)
        {
            try
            {
                string sqlStr = string.Format(@"update Dict_instrmt_servicing set 
                                                    Dis_chk_code='{0}',
                                                    Dis_chk_date='{1}',
                                                    Dis_chk_content='{2}'
                                                where Dis_id='{3}'",
                                    servicing.SerChkCode, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), servicing.SerChkContent,
                                       servicing.SerId);

                DBManager helper = new DBManager();
                int rows = helper.ExecCommand(sqlStr);

                string strUpdateInstrmtStatus = string.Format("update Dict_itr_instrument set Ditr_status='正常' where Ditr_id='{0}' ", servicing.SerItrId);
                rows += helper.ExecCommand(strUpdateInstrmtStatus);

                new DaoItrInstrumentRegistration().DelMes(0, servicing.SerItrId);

                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool ServingHandle(EntityDicItrInstrumentServicing servicing)
        {
            try
            {
                string sqlStr = string.Format(@"update Dict_instrmt_servicing set 
                                                    Dis_handler_code='{0}',
                                                    Dis_handle_date='{1}',
                                                    Dis_handle_result='{2}',
                                                    Dis_price={3},
                                                    Dis_interval='{4}'
                                                where Dis_id='{5}'",
                                servicing.SerHandlerCode, servicing.SerHandleDate, servicing.SerHandleResult,
                                  servicing.SerPrice, servicing.SerInterval, servicing.SerId);

                DBManager helper = new DBManager();
                int rows = helper.ExecCommand(sqlStr);

                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

    }
}
