using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace Lib.DataInterface.Implement
{
    /// <summary>
    /// dict_DataInterfaceMapping
    /// </summary>
    [Lib.EntityCore.EntityTableAttribute(TableName = "dict_DataInterfaceMapping", DisplayName = "数据接口对照表")]
    [Serializable]
    public class EntityDictDataInterfaceMapping //: Lib.EntityCore.BaseEntity
    {
        #region map_id

        private System.String _map_id;

        /// <summary>
        /// map_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "map_id", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
        public System.String map_id
        {
            get
            {
                return this._map_id;
            }
            set
            {
                this._map_id = value;
            }
        }

        #endregion

        #region map_type

        private System.String _map_type;

        /// <summary>
        /// map_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "map_type", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String map_type
        {
            get
            {
                return this._map_type;
            }
            set
            {
                this._map_type = value;
            }
        }

        #endregion

        #region map_object_type

        private System.String _map_object_type;

        /// <summary>
        /// map_object_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "map_object_type", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String map_object_type
        {
            get
            {
                return this._map_object_type;
            }
            set
            {
                this._map_object_type = value;
            }
        }

        #endregion

        #region map_seq

        private System.Int32 _map_seq;

        /// <summary>
        /// map_seq
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "map_seq", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32 map_seq
        {
            get
            {
                return this._map_seq;
            }
            set
            {
                this._map_seq = value;
            }
        }

        #endregion

        #region map_enabled

        private System.Boolean _map_enabled;

        /// <summary>
        /// map_enabled
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "map_enabled", DbType = DbType.Boolean, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Boolean map_enabled
        {
            get
            {
                return this._map_enabled;
            }
            set
            {
                this._map_enabled = value;
            }
        }

        #endregion

        #region map_name

        private System.String _map_name;

        /// <summary>
        /// map_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "map_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String map_name
        {
            get
            {
                return this._map_name;
            }
            set
            {
                this._map_name = value;
            }
        }

        #endregion

        #region map_desc

        private System.String _map_desc;

        /// <summary>
        /// map_desc
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "map_desc", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String map_desc
        {
            get
            {
                return this._map_desc;
            }
            set
            {
                this._map_desc = value;
            }
        }

        #endregion

        #region sys_module

        private System.String _sys_module;

        /// <summary>
        /// sys_module
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "sys_module", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
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

        public List<EntityDictDataInterfaceMappingContrast> ContrastItems { get; set; }

        public EntityDictDataInterfaceMapping()
        {
            this.ContrastItems = new List<EntityDictDataInterfaceMappingContrast>();
        }

        public override string ToString()
        {
            return this._map_name;
        }
    }
}
