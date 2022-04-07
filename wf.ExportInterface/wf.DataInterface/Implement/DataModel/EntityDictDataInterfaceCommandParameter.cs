using System;
using System.Text;
using System.Data;
using System.Reflection;

namespace Lib.DataInterface.Implement
{
    /// <summary>
    /// dict_DataInterfaceParams
    /// </summary>
    [Lib.EntityCore.EntityTableAttribute(TableName = "dict_DataInterfaceCommandParameter", DisplayName = "dict_DataInterfaceCommandParameter")]
    [Serializable]
    public class EntityDictDataInterfaceCommandParameter : ICloneable //: Lib.EntityCore.BaseEntity
    {
        #region cmd_id

        private System.String _cmd_id;

        /// <summary>
        /// cmd_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "cmd_id", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
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

        #region param_name

        private System.String _param_name;

        /// <summary>
        /// param_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_name", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
        public System.String param_name
        {
            get
            {
                return this._param_name;
            }
            set
            {
                this._param_name = value;
            }
        }

        #endregion

        #region param_direction

        private System.String _param_direction;

        /// <summary>
        /// param_direction
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_direction", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String param_direction
        {
            get
            {
                return this._param_direction;
            }
            set
            {
                this._param_direction = value;
            }
        }

        #endregion

        #region param_datatype

        private System.String _param_datatype;

        /// <summary>
        /// param_datatype
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_datatype", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String param_datatype
        {
            get
            {
                return this._param_datatype;
            }
            set
            {
                this._param_datatype = value;
            }
        }

        #endregion

        #region param_length

        private System.Int32? _param_length;

        /// <summary>
        /// param_length
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_length", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? param_length
        {
            get
            {
                return this._param_length;
            }
            set
            {
                this._param_length = value;
            }
        }

        #endregion

        #region param_seq

        private System.Int32 _param_seq;

        /// <summary>
        /// param_seq
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_seq", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32 param_seq
        {
            get
            {
                return this._param_seq;
            }
            set
            {
                this._param_seq = value;
            }
        }

        #endregion

        #region param_isbound

        private System.Int16 _param_isbound;

        /// <summary>
        /// param_isbound
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_isbound", DbType = DbType.Int16, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int16 param_isbound
        {
            get
            {
                return this._param_isbound;
            }
            set
            {
                this._param_isbound = value;
            }
        }

        #endregion

        #region param_databind

        private System.String _param_databind;

        /// <summary>
        /// param_databind
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_databind", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String param_databind
        {
            get
            {
                return this._param_databind;
            }
            set
            {
                this._param_databind = value;
            }
        }

        #endregion

        #region param_enabledlog

        private System.Int16 _param_enabledlog;

        /// <summary>
        /// param_enabledlog
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_enabledlog", DbType = DbType.Int16, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int16 param_enabledlog
        {
            get
            {
                return this._param_enabledlog;
            }
            set
            {
                this._param_enabledlog = value;
            }
        }

        #endregion

        //#region param_value_expression

        //private System.String _param_value_expression;

        ///// <summary>
        ///// param_value_expression
        ///// </summary>
        //[Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_value_expression", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        //public System.String param_value_expression
        //{
        //    get
        //    {
        //        return this._param_value_expression;
        //    }
        //    set
        //    {
        //        this._param_value_expression = value;
        //    }
        //}

        //#endregion

        #region param_enabled

        private System.Int16 _param_enabled;

        /// <summary>
        /// param_enabled
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_enabled", DbType = DbType.Int16, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int16 param_enabled
        {
            get
            {
                return this._param_enabled;
            }
            set
            {
                this._param_enabled = value;
            }
        }

        #endregion

        #region param_converter_id

        private System.String _param_converter_id;

        /// <summary>
        /// param_converter_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_converter_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String param_converter_id
        {
            get
            {
                return this._param_converter_id;
            }
            set
            {
                this._param_converter_id = value;
            }
        }

        #endregion

        #region param_desc

        private System.String _param_desc;

        /// <summary>
        /// param_desc
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "param_desc", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String param_desc
        {
            get
            {
                return this._param_desc;
            }
            set
            {
                this._param_desc = value;
            }
        }

        #endregion

        public EntityDictDataInterfaceCommandParameter()
        {
            this.param_enabled = 1;
            this.param_isbound = 0;
            this.param_seq = 0;
            this.param_enabledlog = 0;
            this.param_direction = EnumDataInterfaceParameterDirection.Input.ToString();
        }

        public static void PropertiesClone(EntityDictDataInterfaceCommandParameter source, EntityDictDataInterfaceCommandParameter dest)
        {
            PropertyInfo[] props = typeof(EntityDictDataInterfaceCommandParameter).GetProperties();
            foreach (PropertyInfo p in props)
            {
                if (p.CanWrite)
                {
                    object val = p.GetValue(source, null);
                    p.SetValue(dest, val, null);
                }
            }
        }

        #region ICloneable 成员

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}
