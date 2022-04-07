using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.ComponentModel.Composition;
using dcl.dao.core;
using dcl.common;
using System.Data;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDicDataInterfaceCommand))]
    public class DaoDicDataInterfaceCommand : IDaoDicDataInterfaceCommand
    {
        public bool DeleteDicDataInterfaceCommand(string cmdID)
        {

            bool isDelete = false;
            if (!string.IsNullOrEmpty(cmdID))
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sql = string.Format(@"delete from dict_DataInterfaceCommand where cmd_id = '{0}' ", cmdID);
                    helper.ExecCommand(sql);
                    isDelete = true;
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return isDelete;
        }

        public bool SaveDicDataInterfaceCommand(EntityDicDataInterfaceCommand interCommand)
        {
            bool isSave = false;
            try
            {
                int id = IdentityHelper.GetMedIdentity("dict_DataInterfaceCommand"); //获取主键值

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("cmd_id", id);
                values.Add("conn_id", interCommand.ConnId);
                values.Add("cmd_name", interCommand.CmdName);
                values.Add("cmd_desc", interCommand.CmdDesc);
                values.Add("cmd_command_text", interCommand.CmdCommandText);

                values.Add("cmd_command_type", interCommand.CmdCommandType);
                values.Add("cmd_fetch_type", interCommand.CmdFetchType);
                values.Add("cmd_enabled", interCommand.CmdEnabled);
                values.Add("cmd_sysdefault", interCommand.CmdSysdefault);
                values.Add("cmd_group", interCommand.CmdGroup);

                values.Add("cmd_enabled_log", interCommand.CmdEnabledLog);
                values.Add("cmd_running_side", interCommand.CmdRunningSide);
                values.Add("cmd_exec_seq", interCommand.CmdExecSeq);
                values.Add("cmd_asyn", interCommand.CmdAsyn);
                values.Add("sys_module", interCommand.SysModule);

                values.Add("sys_default", interCommand.SysDefault);
                values.Add("cmd_exec_asyn", interCommand.CmdExecAsyn);

                helper.InsertOperation("dict_DataInterfaceCommand ", values);

                interCommand.CmdId = id.ToString();
                isSave = true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isSave;
        }

        public List<EntityDicDataInterfaceCommand> SearchDicDataInterfaceCommand(EntityDicDataInterfaceCommand interCommand)
        {
            List<EntityDicDataInterfaceCommand> listInterCommand = new List<EntityDicDataInterfaceCommand>();
            if (interCommand != null)
            {
                try
                {
                    DBManager helper = new DBManager();
                    string sqlStr = "select * from dict_DataInterfaceCommand where 1=1";
                    if (!string.IsNullOrEmpty(interCommand.CmdId))
                        sqlStr += string.Format(" and cmd_id ='{0}' ", interCommand.CmdId);
                    if (!string.IsNullOrEmpty(interCommand.CmdGroup))
                        sqlStr += string.Format(" and cmd_group='{0}' ", interCommand.CmdGroup);
                    if (!string.IsNullOrEmpty(interCommand.SysModule))
                        sqlStr += string.Format(" and sys_module='{0}' ", interCommand.SysModule);

                    DataTable dt = helper.ExecuteDtSql(sqlStr);
                    listInterCommand = EntityManager<EntityDicDataInterfaceCommand>.ConvertToList(dt).OrderBy(i => i.CmdId.Length).ThenBy(i => i.CmdId).ThenBy(i => i.CmdExecSeq).ToList();
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
            }
            return listInterCommand;
        }

        public bool UpdateDicDataInterfaceCommand(EntityDicDataInterfaceCommand interCommand)
        {
            bool isUpdate = false;
            try
            {
                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("cmd_id", id);
                values.Add("conn_id", interCommand.ConnId);
                values.Add("cmd_name", interCommand.CmdName);
                values.Add("cmd_desc", interCommand.CmdDesc);
                values.Add("cmd_command_text", interCommand.CmdCommandText);

                values.Add("cmd_command_type", interCommand.CmdCommandType);
                values.Add("cmd_fetch_type", interCommand.CmdFetchType);
                values.Add("cmd_enabled", interCommand.CmdEnabled);
                values.Add("cmd_sysdefault", interCommand.CmdSysdefault);
                values.Add("cmd_group", interCommand.CmdGroup);

                values.Add("cmd_enabled_log", interCommand.CmdEnabledLog);
                values.Add("cmd_running_side", interCommand.CmdRunningSide);
                values.Add("cmd_exec_seq", interCommand.CmdExecSeq);
                values.Add("cmd_asyn", interCommand.CmdAsyn);
                values.Add("sys_module", interCommand.SysModule);

                values.Add("sys_default", interCommand.SysDefault);
                values.Add("cmd_exec_asyn", interCommand.CmdExecAsyn);

                Dictionary<string, object> keys = new Dictionary<string, object>();
                keys.Add("cmd_id", interCommand.CmdId);

                helper.UpdateOperation("dict_DataInterfaceCommand ", values, keys);

                isUpdate = true;
            } 
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }
            return isUpdate;
        }
    }
}
