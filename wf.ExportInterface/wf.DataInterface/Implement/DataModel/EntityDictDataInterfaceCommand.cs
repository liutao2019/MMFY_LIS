using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Lib.EntityCore;
using System.Reflection;

namespace Lib.DataInterface.Implement
{
    /// <summary>
    /// dict_DataInterfaceCommand
    /// </summary>
    [Lib.EntityCore.EntityTableAttribute(TableName = "dict_DataInterfaceCommand", DisplayName = "dict_DataInterfaceCommand")]
    [Serializable]
    public class EntityDictDataInterfaceCommand : ICloneable//: Lib.EntityCore.BaseEntity
    {
        //public EntityDictDataInterfaceConnection Connection { get; set; }
        //public List<EntityDictDataInterfaceCommandParameter> Parameters { get; set; }

        public EntityDictDataInterfaceCommand()
        {
            this.cmd_enabled = 1;
            this.cmd_enabled_log = 0;
            this.cmd_exec_seq = 0;
            this.sys_default = 0;
            this.cmd_exec_asyn = 0;
            this.cmd_running_side = EnumDeploymentMode.Server.ToString();
            this.cmd_fetch_type = EnumCommandExecuteType.ExecuteNonQuery.ToString();
            this.cmd_command_type = EnumCommandType.Text.ToString();
        }

        public override string ToString()
        {
            return this.cmd_name;
        }

        #region ICloneable 成员

        public object Clone()
        {
            EntityDictDataInterfaceCommand cmd = this.MemberwiseClone() as EntityDictDataInterfaceCommand;

            //if (this.Parameters != null)
            //{
            //    cmd.Parameters = new List<EntityDictDataInterfaceCommandParameter>();
            //    foreach (EntityDictDataInterfaceCommandParameter p in this.Parameters)
            //    {
            //        cmd.Parameters.Add(p.Clone() as EntityDictDataInterfaceCommandParameter);
            //    }
            //}
            return cmd;
        }

        #endregion

        public static void PropertiesClone(EntityDictDataInterfaceCommand source, EntityDictDataInterfaceCommand dest)
        {
            PropertyInfo[] props = typeof(EntityDictDataInterfaceCommand).GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (p.CanWrite)
                {
                    object val = p.GetValue(source, null);
                    p.SetValue(dest, val, null);
                }
            }
        }

        #region cmd_id

        private System.String _cmd_id;

        /// <summary>
        /// cmd_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_id", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
        [SysTableIDGenerate]
        public System.String cmd_id
        {
            get
            {
                return this._cmd_id;
            }
            set
            {
                this._cmd_id = value;
            }
        }

        #endregion

        #region conn_id

        private System.String _conn_id;

        /// <summary>
        /// conn_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_id
        {
            get
            {
                return this._conn_id;
            }
            set
            {
                this._conn_id = value;
            }
        }

        #endregion

        #region cmd_name

        private System.String _cmd_name;

        /// <summary>
        /// cmd_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_name", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String cmd_name
        {
            get
            {
                return this._cmd_name;
            }
            set
            {
                this._cmd_name = value;
            }
        }

        #endregion

        #region cmd_desc

        private System.String _cmd_desc;

        /// <summary>
        /// cmd_desc
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_desc", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String cmd_desc
        {
            get
            {
                return this._cmd_desc;
            }
            set
            {
                this._cmd_desc = value;
            }
        }

        #endregion

        #region cmd_command_text

        private System.String _cmd_command_text;

        /// <summary>
        /// cmd_command_text
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_command_text", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String cmd_command_text
        {
            get
            {
                return this._cmd_command_text;
            }
            set
            {
                this._cmd_command_text = value;
            }
        }

        #endregion

        #region cmd_command_type

        private System.String _cmd_command_type;

        /// <summary>
        /// cmd_command_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_command_type", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String cmd_command_type
        {
            get
            {
                return this._cmd_command_type;
            }
            set
            {
                this._cmd_command_type = value;
            }
        }

        #endregion

        #region cmd_fetch_type

        private System.String _cmd_fetch_type;

        /// <summary>
        /// cmd_fetch_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_fetch_type", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String cmd_fetch_type
        {
            get
            {
                return this._cmd_fetch_type;
            }
            set
            {
                this._cmd_fetch_type = value;
            }
        }

        #endregion

        #region cmd_enabled

        private System.Byte? _cmd_enabled;

        /// <summary>
        /// cmd_enabled
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_enabled", DbType = DbType.Byte, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Byte? cmd_enabled
        {
            get
            {
                return this._cmd_enabled;
            }
            set
            {
                this._cmd_enabled = value;
            }
        }

        #endregion

        #region cmd_group

        private System.String _cmd_group;

        /// <summary>
        /// cmd_group
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_group", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String cmd_group
        {
            get
            {
                return this._cmd_group;
            }
            set
            {
                this._cmd_group = value;
            }
        }

        #endregion

        #region cmd_running_side

        private System.String _cmd_running_side;

        /// <summary>
        /// cmd_running_side
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_running_side", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String cmd_running_side
        {
            get
            {
                return this._cmd_running_side;
            }
            set
            {
                this._cmd_running_side = value;
            }
        }

        #endregion

        #region cmd_exec_seq

        private System.Int32 _cmd_exec_seq;

        /// <summary>
        /// cmd_exec_seq
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_exec_seq", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32 cmd_exec_seq
        {
            get
            {
                return this._cmd_exec_seq;
            }
            set
            {
                this._cmd_exec_seq = value;
            }
        }

        #endregion

        #region sys_module

        private System.String _sys_module;

        /// <summary>
        /// sys_module
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "sys_module", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String sys_module
        {
            get
            {
                return this._sys_module;
            }
            set
            {
                this._sys_module = value;
            }
        }

        #endregion

        #region sys_default

        private System.Byte? _sys_default;

        /// <summary>
        /// sys_default
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "sys_default", DbType = DbType.Byte, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Byte? sys_default
        {
            get
            {
                return this._sys_default;
            }
            set
            {
                this._sys_default = value;
            }
        }

        #endregion

        #region cmd_exec_asyn

        private System.Byte _cmd_exec_asyn;

        /// <summary>
        /// cmd_exec_asyn
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_exec_asyn", DbType = DbType.Byte, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Byte cmd_exec_asyn
        {
            get
            {
                return this._cmd_exec_asyn;
            }
            set
            {
                this._cmd_exec_asyn = value;
            }
        }

        #endregion

        #region cmd_enabled_log

        private System.Byte? _cmd_enabled_log;

        /// <summary>
        /// cmd_enabled_log
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_enabled_log", DbType = DbType.Byte, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Byte? cmd_enabled_log
        {
            get
            {
                return this._cmd_enabled_log;
            }
            set
            {
                this._cmd_enabled_log = value;
            }
        }

        #endregion

    }
}
