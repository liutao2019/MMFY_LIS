using System;
using System.Data;
using Lib.EntityCore;
using Lib.DAC;
using System.Reflection;

namespace Lib.DataInterface.Implement
{
    /// <summary>
    /// dict_DataInterfaceConnection
    /// </summary>
    [Lib.EntityCore.EntityTableAttribute(TableName = "Dict_DataInterfaceConnection", DisplayName = "Dict_DataInterfaceConnection")]
    [Serializable]
    public class EntityDictDataInterfaceConnection : ICloneable 
    {
        public DataInterfaceConnection ToConnection()
        {
            DataInterfaceConnection conn = DataInterfaceConnection.FromDTO(this);
            return conn;
        }

        public SqlHelper GetSqlHelper()
        {
            DataInterfaceConnection conn = ToConnection();
            IDataInterfaceConnection execonn = conn.GetExecuteConnection();
            if (execonn is SqlDataInterfaceConnection)
            {
                SqlHelper helper = (execonn as SqlDataInterfaceConnection).CreateSqlHelper();
                return helper;
            }
            else
            {
                throw new NotSupportedException("指定的接口类型必须为sql");
            }
        }

        public EntityHelper GetEntityHelper()
        {
            DataInterfaceConnection conn = ToConnection();
            IDataInterfaceConnection execonn = conn.GetExecuteConnection();
            if (execonn is SqlDataInterfaceConnection)
            {
                EntityHelper helper = (execonn as SqlDataInterfaceConnection).CreateEntityHelper();
                return helper;
            }
            else
            {
                throw new NotSupportedException("指定的接口类型必须为sql");
            }
        }

        public EntityDictDataInterfaceConnection()
        {
            this.sys_default = 0;
            this.conn_address = string.Empty;
            this.conn_type = EnumDataInterfaceConnectionType.SQL.ToString();
            this.conn_running_side = EnumDeploymentMode.Server.ToString();
        }

        public override string ToString()
        {
            if (this.conn_id != null)
            {
                return this._conn_id.PadRight(3, ' ') + "|" + this._conn_name;
            }
            else
            {
                return string.Empty;
            }
        }

        #region ICloneable 成员

        public object Clone()
        {
            EntityDictDataInterfaceConnection conn = new EntityDictDataInterfaceConnection();
            EntityDictDataInterfaceConnection.PropertiesClone(this, conn);
            return conn;
        }

        #endregion

        public static void PropertiesClone(EntityDictDataInterfaceConnection source, EntityDictDataInterfaceConnection dest)
        {
            PropertyInfo[] props = typeof(EntityDictDataInterfaceConnection).GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (p.CanWrite)
                {
                    object val = p.GetValue(source, null);
                    p.SetValue(dest, val, null);
                }
            }
        }

        #region conn_id

        private System.String _conn_id;

        /// <summary>
        /// conn_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_id", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
        [Lib.EntityCore.SysTableIDGenerate]
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

        #region conn_code

        private System.String _conn_code;

        /// <summary>
        /// conn_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_code", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_code
        {
            get
            {
                return this._conn_code;
            }
            set
            {
                this._conn_code = value;
            }
        }

        #endregion

        #region conn_name

        private System.String _conn_name;

        /// <summary>
        /// conn_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_name", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_name
        {
            get
            {
                return this._conn_name;
            }
            set
            {
                this._conn_name = value;
            }
        }

        #endregion

        #region conn_type

        private System.String _conn_type;

        /// <summary>
        /// conn_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_type", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_type
        {
            get
            {
                return this._conn_type;
            }
            set
            {
                this._conn_type = value;
            }
        }

        #endregion

        #region conn_address

        private System.String _conn_address;

        /// <summary>
        /// conn_address
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_address", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_address
        {
            get
            {
                return this._conn_address;
            }
            set
            {
                this._conn_address = value;
            }
        }

        #endregion

        #region conn_db_driver

        private System.String _conn_db_driver;

        /// <summary>
        /// conn_db_driver
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_db_driver", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_db_driver
        {
            get
            {
                return this._conn_db_driver;
            }
            set
            {
                this._conn_db_driver = value;
            }
        }

        #endregion

        #region conn_db_dialet

        private System.String _conn_db_dialet;

        /// <summary>
        /// conn_db_dialet
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_db_dialet", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_db_dialet
        {
            get
            {
                return this._conn_db_dialet;
            }
            set
            {
                this._conn_db_dialet = value;
            }
        }

        #endregion

        #region conn_db_catelog

        private System.String _conn_db_catelog;

        /// <summary>
        /// conn_db_catelog
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_db_catelog", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_db_catelog
        {
            get
            {
                return this._conn_db_catelog;
            }
            set
            {
                this._conn_db_catelog = value;
            }
        }

        #endregion

        #region conn_desc

        private System.String _conn_desc;

        /// <summary>
        /// conn_desc
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_desc", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_desc
        {
            get
            {
                return this._conn_desc;
            }
            set
            {
                this._conn_desc = value;
            }
        }

        #endregion

        #region conn_login

        private System.String _conn_login;

        /// <summary>
        /// conn_login
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_login", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_login
        {
            get
            {
                return this._conn_login;
            }
            set
            {
                this._conn_login = value;
            }
        }

        #endregion

        #region conn_pass

        private System.String _conn_pass;

        /// <summary>
        /// conn_pass
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_pass", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_pass
        {
            get
            {
                return this._conn_pass;
            }
            set
            {
                this._conn_pass = value;
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

        private System.Byte _sys_default;

        /// <summary>
        /// sys_default
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "sys_default", DbType = DbType.Byte, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Byte sys_default
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

        #region conn_running_side

        private System.String _conn_running_side;

        /// <summary>
        /// conn_running_side
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "conn_running_side", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String conn_running_side
        {
            get
            {
                return this._conn_running_side;
            }
            set
            {
                this._conn_running_side = value;
            }
        }

        #endregion
    }
}
