using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.EntityCore;
using System.Reflection;

namespace Lib.DataInterface.Implement
{
    /// <summary>
    /// dict_DataConvertRule
    /// </summary>
    [Lib.EntityCore.EntityTableAttribute(TableName = "EntityDictDataConverter", DisplayName = "dict_DataConvertRule")]
    [Serializable]
    public class EntityDictDataConverter //: Lib.EntityCore.BaseEntity
    {
        public string _display4ComboBox
        {
            get
            {
                if (string.IsNullOrEmpty(this._rule_id))
                    return string.Empty;
                else
                    return this._rule_id + "|" + this._rule_name;
            }
        }

        public static void PropertiesClone(EntityDictDataConverter source, EntityDictDataConverter dest)
        {
            PropertyInfo[] props = typeof(EntityDictDataConverter).GetProperties();
            foreach (PropertyInfo p in props)
            {
                object val = p.GetValue(source, null);
                p.SetValue(dest, val, null);
            }
        }

        public EntityDictDataConverter()
        {
            this.sys_default = false;
            this.rule_type = EnumDataInterfaceConverterType.System.ToString();
            this.rule_dest_datatype = typeof(string).FullName;
            this.rule_src_datatype = typeof(string).FullName;
            this.rule_type = EnumDataInterfaceConverterType.Contrast.ToString();
        }

        #region rule_id

        private System.String _rule_id;

        /// <summary>
        /// rule_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "rule_id", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
        [SysTableIDGenerate]
        public System.String rule_id
        {
            get
            {
                return this._rule_id;
            }
            set
            {
                this._rule_id = value;
            }
        }

        #endregion

        #region rule_type

        private System.String _rule_type;

        /// <summary>
        /// rule_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "rule_type", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String rule_type
        {
            get
            {
                return this._rule_type;
            }
            set
            {
                this._rule_type = value;
            }
        }

        #endregion

        #region rule_ref_id

        private System.String _rule_ref_id;

        /// <summary>
        /// rule_ref_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "rule_ref_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String rule_ref_id
        {
            get
            {
                return this._rule_ref_id;
            }
            set
            {
                this._rule_ref_id = value;
            }
        }

        #endregion

        #region rule_name

        private System.String _rule_name;

        /// <summary>
        /// rule_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "rule_name", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String rule_name
        {
            get
            {
                return this._rule_name;
            }
            set
            {
                this._rule_name = value;
            }
        }

        #endregion

        #region rule_src_datatype

        private System.String _rule_src_datatype;

        /// <summary>
        /// rule_dest_datatype
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "rule_src_datatype", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String rule_src_datatype
        {
            get
            {
                return this._rule_src_datatype;
            }
            set
            {
                this._rule_src_datatype = value;
            }
        }

        #endregion

        #region rule_dest_datatype

        private System.String _rule_dest_datatype;

        /// <summary>
        /// rule_dest_datatype
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "rule_dest_datatype", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String rule_dest_datatype
        {
            get
            {
                return this._rule_dest_datatype;
            }
            set
            {
                this._rule_dest_datatype = value;
            }
        }

        #endregion

        #region rule_dest_dataformet

        private System.String _rule_dest_dataformet;

        /// <summary>
        /// rule_dest_dataformet
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "rule_dest_dataformet", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String rule_dest_dataformet
        {
            get
            {
                return this._rule_dest_dataformet;
            }
            set
            {
                this._rule_dest_dataformet = value;
            }
        }

        #endregion

        #region sys_default

        private System.Boolean _sys_default;

        /// <summary>
        /// sys_default
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "sys_default", DbType = DbType.Boolean, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Boolean sys_default
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

        #region rule_seq

        private System.Int32 _rule_seq;

        /// <summary>
        /// rule_seq
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "rule_seq", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32 rule_seq
        {
            get
            {
                return this._rule_seq;
            }
            set
            {
                this._rule_seq = value;
            }
        }

        #endregion

        #region rule_desc

        private System.String _rule_desc;

        /// <summary>
        /// rule_desc
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "rule_desc", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String rule_desc
        {
            get
            {
                return this._rule_desc;
            }
            set
            {
                this._rule_desc = value;
            }
        }

        #endregion
    }
}
