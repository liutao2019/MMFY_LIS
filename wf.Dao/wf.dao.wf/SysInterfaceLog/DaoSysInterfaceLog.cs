using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.ComponentModel.Composition;
using dcl.dao.core;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoSysInterfaceLog))]
    public class DaoSysInterfaceLog : IDaoSysInterfaceLog
    {
        public List<EntitySysInterfaceLog> GetSysInterfaceLogInNumber(EntitySysInterfaceLog eySysInfeLog, int number)
        {
            List<EntitySysInterfaceLog> listSysInfeLog = new List<EntitySysInterfaceLog>();

            if (eySysInfeLog != null)
            {
                try
                {
                    DBManager helper = new DBManager();

                    string strTop = string.Empty;

                    if (number > 0)
                    {
                        strTop = " top " + number.ToString();
                    }

                    string sqlStr = string.Format(@"select {0} 
                                                         operation_key,samp_bar_id,order_sn,rep_id,operation_name,
                                                         operation_user_code,operation_user_name,operation_time,
                                                         operation_success,operation_content,repeat_count
                                                    from sys_interface_log where 1=1 ", strTop);
                    //操作时间
                    if (eySysInfeLog.OperaBeginDateTime != null && eySysInfeLog.OperaEndDateTime != null)
                    {
                        sqlStr += string.Format(@" and operation_time>='{0}' and operation_time<'{1}' ",
                            eySysInfeLog.OperaBeginDateTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                            eySysInfeLog.OperaEndDateTime?.ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    //条码号
                    if (!string.IsNullOrEmpty(eySysInfeLog.SampBarId))
                    {
                        sqlStr += string.Format(@" and samp_bar_id='{0}' ", eySysInfeLog.SampBarId);
                    }

                    //医嘱ID
                    if (!string.IsNullOrEmpty(eySysInfeLog.OrderSn))
                    {
                        sqlStr += string.Format(@" and order_sn='{0}' ", eySysInfeLog.OrderSn);
                    }
                    //报告标识ID
                    if (!string.IsNullOrEmpty(eySysInfeLog.RepId))
                    {
                        sqlStr += string.Format(@" and rep_id='{0}' ", eySysInfeLog.RepId);
                    }
                    //操作名称
                    if (!string.IsNullOrEmpty(eySysInfeLog.OperationName))
                    {
                        sqlStr += string.Format(@" and operation_name='{0}' ", eySysInfeLog.OperationName);
                    }
                    //操作人代码
                    if (!string.IsNullOrEmpty(eySysInfeLog.OperationUserCode))
                    {
                        sqlStr += string.Format(@" and operation_user_code='{0}' ", eySysInfeLog.OperationUserCode);
                    }
                    //操作人名称
                    if (!string.IsNullOrEmpty(eySysInfeLog.OperationUserName))
                    {
                        sqlStr += string.Format(@" and operation_user_name='{0}' ", eySysInfeLog.OperationUserName);
                    }
                    //操作状态
                    if (eySysInfeLog.OperationSuccess != null)
                    {
                        sqlStr += string.Format(@" and operation_success={0} ", eySysInfeLog.OperationSuccess);
                    }

                    if (number > 0)
                    {
                        sqlStr += " order by operation_time desc";
                    }

                    DataTable dtSysLog = helper.ExecuteDtSql(sqlStr);
                    listSysInfeLog = EntityManager<EntitySysInterfaceLog>.ConvertToList(dtSysLog).OrderBy(i => i.OperationKey).ToList();

                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                    throw;
                }
            }

            return listSysInfeLog;
        }

        public List<EntitySysInterfaceLog> GetSysInterfaceLogData(EntitySysInterfaceLog eySysInfeLog)
        {
            return GetSysInterfaceLogInNumber(eySysInfeLog, 0);
        }

        public bool SaveSysInterfaceLog(EntitySysInterfaceLog eySysInfeLog)
        {
            bool isSave = false;
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("operation_key", eySysInfeLog.OperationKey);//自增ID
                values.Add("samp_bar_id", eySysInfeLog.SampBarId);
                values.Add("order_sn", eySysInfeLog.OrderSn);
                values.Add("rep_id", eySysInfeLog.RepId);
                values.Add("operation_name", eySysInfeLog.OperationName);

                values.Add("operation_user_code", eySysInfeLog.OperationUserCode);
                values.Add("operation_user_name", eySysInfeLog.OperationUserName);
                values.Add("operation_time", eySysInfeLog.OperationTime.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("operation_success", eySysInfeLog.OperationSuccess);
                values.Add("operation_content", eySysInfeLog.OperationContent);

                values.Add("repeat_count", eySysInfeLog.RepeatCount);

                if (helper.InsertOperation("sys_interface_log", values) > 0)
                    isSave = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                isSave = false;
            }

            return isSave;
        }

        public bool DeleteSysInterfaceLog(int strOperationKey)
        {
            bool success = false;
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("operation_key", strOperationKey);

                if (helper.DeleteOperation("sys_interface_log", values) > 0)
                    success = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                success = false;
            }

            return success;
        }

    }
}
