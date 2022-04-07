using System;
using System.Collections.Generic;
using System.Text;
using Lib.EntityCore;
using System.Data;

namespace Lib.DataInterface.Implement
{
    /// <summary>
    /// dict_DataConvertContrast
    /// </summary>
    [Lib.EntityCore.EntityTableAttribute(TableName = "dict_DataConvertContrast", DisplayName = "dict_DataConvertContrast")]
    [Serializable]
    public class EntityDictDataConvertContrast //: Lib.EntityCore.BaseEntity
    {
        #region con_id

        private System.String _con_id;

        /// <summary>
        /// con_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "con_id", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
        [SysTableIDGenerate]
        public System.String con_id
        {
            get
            {
                return this._con_id;
            }
            set
            {
                this._con_id = value;
            }
        }

        #endregion

        #region rule_id

        private System.String _rule_id;

        /// <summary>
        /// rule_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "rule_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
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

        #region con_src_datatype

        private System.String _con_src_datatype;

        /// <summary>
        /// con_src_datatype
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "con_src_datatype", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String con_src_datatype
        {
            get
            {
                return this._con_src_datatype;
            }
            set
            {
                this._con_src_datatype = value;
            }
        }

        #endregion

        #region con_src_value

        private System.String _con_src_value;

        /// <summary>
        /// con_src_value
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "con_src_value", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String con_src_value
        {
            get
            {
                return this._con_src_value;
            }
            set
            {
                this._con_src_value = value;
            }
        }

        #endregion

        #region con_dest_value

        private System.String _con_dest_value;

        /// <summary>
        /// con_dest_value
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "con_dest_value", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String con_dest_value
        {
            get
            {
                return this._con_dest_value;
            }
            set
            {
                this._con_dest_value = value;
            }
        }

        #endregion

        #region con_enabled

        private System.Boolean _con_enabled;

        /// <summary>
        /// con_enabled
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "con_enabled", DbType = DbType.Boolean, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Boolean con_enabled
        {
            get
            {
                return this._con_enabled;
            }
            set
            {
                this._con_enabled = value;
            }
        }

        #endregion

        #region con_seq

        private System.Int32 _con_seq;

        /// <summary>
        /// con_seq
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "con_seq", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32 con_seq
        {
            get
            {
                return this._con_seq;
            }
            set
            {
                this._con_seq = value;
            }
        }

        #endregion


        public EntityDictDataConvertContrast()
        {
            this.con_enabled = true;
            this.con_src_datatype = null;// typeof(string).FullName;
        }
    }

}
