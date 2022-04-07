using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 接口参数
    /// 旧表名:dict_DataInterfaceCommand 新表名:dict_DataInterfaceCommand
    /// </summary>
    [Serializable]
    public class EntityDicDataInterfaceCommand : EntityBase
    {
        /// <summary>
        /// ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_id", MedName = "cmd_id", WFName = "cmd_id")]
        public String CmdId { get; set; }

        /// <summary>
        ///连接参数ID(对应dict_DataInterfaceConnection.conn_id)
        /// </summary>   
        [FieldMapAttribute(ClabName = "conn_id", MedName = "conn_id", WFName = "conn_id")]
        public String ConnId { get; set; }

        /// <summary>
        ///名称
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_name", MedName = "cmd_name", WFName = "cmd_name")]
        public String CmdName { get; set; }

        /// <summary>
        ///备注
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_desc", MedName = "cmd_desc", WFName = "cmd_desc")]
        public String CmdDesc { get; set; }

        /// <summary>
        ///SQL语句
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_command_text", MedName = "cmd_command_text", WFName = "cmd_command_text")]
        public String CmdCommandText { get; set; }

        /// <summary>
        ///命令类型
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_command_type", MedName = "cmd_command_type", WFName = "cmd_command_type")]
        public String CmdCommandType { get; set; }

        /// <summary>
        ///执行方式
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_fetch_type", MedName = "cmd_fetch_type", WFName = "cmd_fetch_type")]
        public String CmdFetchType { get; set; }

        /// <summary>
        ///启用
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_enabled", MedName = "cmd_enabled", WFName = "cmd_enabled")]
        public Boolean CmdEnabled { get; set; }

        /// <summary>
        ///系统预置
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_sysdefault", MedName = "cmd_sysdefault", WFName = "cmd_sysdefault")]
        public Boolean CmdSysdefault { get; set; }

        /// <summary>
        ///分组
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_group", MedName = "cmd_group", WFName = "cmd_group")]
        public String CmdGroup { get; set; }

        /// <summary>
        ///日志
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_enabled_log", MedName = "cmd_enabled_log", WFName = "cmd_enabled_log")]
        public Boolean CmdEnabledLog { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_running_side", MedName = "cmd_running_side", WFName = "cmd_running_side")]
        public String CmdRunningSide { get; set; }

        /// <summary>
        ///顺序
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_exec_seq", MedName = "cmd_exec_seq", WFName = "cmd_exec_seq")]
        public Int32 CmdExecSeq { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_asyn", MedName = "cmd_asyn", WFName = "cmd_asyn")]
        public Int32 CmdAsyn { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "sys_module", MedName = "sys_module", WFName = "sys_module")]
        public String SysModule { get; set; }

        /// <summary>
        ///系统预置
        /// </summary>   
        [FieldMapAttribute(ClabName = "sys_default", MedName = "sys_default", WFName = "sys_default")]
        public Boolean SysDefault { get; set; }

        /// <summary>
        ///
        /// </summary>   
        [FieldMapAttribute(ClabName = "cmd_exec_asyn", MedName = "cmd_exec_asyn", WFName = "cmd_exec_asyn")]
        public Boolean CmdExecAsyn { get; set; }
    }
}
