using System;
using System.Text;
using System.Data;
using System.Collections.Generic;

namespace Lib.DataInterface.Implement
{
    /// <summary>
    /// dict_DataInterfaceMappingContrast
    /// </summary>
    [Lib.EntityCore.EntityTableAttribute(TableName = "dict_DataInterfaceMappingContrast", DisplayName = "院网接口对照明细表")]
    [Serializable]
    public class EntityDictDataInterfaceMappingContrast //: Lib.EntityCore.BaseEntity
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

        #region mitm_dest_field

        private System.String _mitm_dest_field;

        /// <summary>
        /// mitm_dest_field
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "mitm_dest_field", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
        public System.String mitm_dest_field
        {
            get
            {
                return this._mitm_dest_field;
            }
            set
            {
                this._mitm_dest_field = value;
            }
        }

        #endregion

        #region mitm_dest_datatype

        private System.String _mitm_dest_datatype;

        /// <summary>
        /// mitm_dest_datatype
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "mitm_dest_datatype", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String mitm_dest_datatype
        {
            get
            {
                return this._mitm_dest_datatype;
            }
            set
            {
                this._mitm_dest_datatype = value;
            }
        }

        #endregion

        #region mitm_src_field

        private System.String _mitm_src_field;

        /// <summary>
        /// mitm_src_field
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "mitm_src_field", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String mitm_src_field
        {
            get
            {
                return this._mitm_src_field;
            }
            set
            {
                this._mitm_src_field = value;
            }
        }

        #endregion

        #region mitm_src_datatype

        private System.String _mitm_src_datatype;

        /// <summary>
        /// mitm_src_datatype
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "mitm_src_datatype", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String mitm_src_datatype
        {
            get
            {
                return this._mitm_src_datatype;
            }
            set
            {
                this._mitm_src_datatype = value;
            }
        }

        #endregion

        #region mitm_seq

        private System.Int32 _mitm_seq;

        /// <summary>
        /// mitm_seq
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "mitm_seq", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32 mitm_seq
        {
            get
            {
                return this._mitm_seq;
            }
            set
            {
                this._mitm_seq = value;
            }
        }

        #endregion

        #region mitm_enabled

        private System.Boolean _mitm_enabled;

        /// <summary>
        /// mitm_enabled
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "mitm_enabled", DbType = DbType.Boolean, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Boolean mitm_enabled
        {
            get
            {
                return this._mitm_enabled;
            }
            set
            {
                this._mitm_enabled = value;
            }
        }

        #endregion

        #region mitm_convert_rule_id

        private System.String _mitm_convert_rule_id;

        /// <summary>
        /// mitm_convert_rule_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "mitm_convert_rule_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String mitm_convert_rule_id
        {
            get
            {
                return this._mitm_convert_rule_id;
            }
            set
            {
                this._mitm_convert_rule_id = value;
            }
        }

        #endregion

        #region mitm_desc

        private System.String _mitm_desc;

        /// <summary>
        /// mitm_desc
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "mitm_desc", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String mitm_desc
        {
            get
            {
                return this._mitm_desc;
            }
            set
            {
                this._mitm_desc = value;
            }
        }

        #endregion

        public EntityDictDataInterfaceMappingContrast()
        {

        }

        public override string ToString()
        {
            return string.Format("{0}{1} -> {2}{3}"
                , this.mitm_src_field
                , string.IsNullOrEmpty(this.mitm_src_datatype) ? "" : "(" + this.mitm_src_datatype + ")"
                , this.mitm_dest_field
                , string.IsNullOrEmpty(this.mitm_dest_datatype) ? "" : "(" + this.mitm_dest_datatype + ")"
                );
        }
    }
}