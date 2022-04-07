using System;
using System.Text;
using System.Data;
using Lib.EntityCore;

namespace Lib.DataInterface.Implement
{
    /// <summary>
    /// dict_DataInterface
    /// </summary>
    [Lib.EntityCore.EntityTableAttribute(TableName = "dict_DataInterface", DisplayName = "dict_DataInterface")]
    [Serializable]
    public class EntityDictDataInterface //: Lib.EntityCore.BaseEntity
    {
        #region in_id

        private System.String _in_id;

        /// <summary>
        /// in_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_id", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
        [SysTableIDGenerateAttribute]
        public System.String in_id
        {
            get
            {
                return this._in_id;
            }
            set
            {
                this._in_id = value;
            }
        }

        #endregion

        #region in_name

        private System.String _in_name;

        /// <summary>
        /// in_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String in_name
        {
            get
            {
                return this._in_name;
            }
            set
            {
                this._in_name = value;
            }
        }

        #endregion

        #region in_type

        private System.String _in_type;

        /// <summary>
        /// in_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_type", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String in_type
        {
            get
            {
                return this._in_type;
            }
            set
            {
                this._in_type = value;
            }
        }

        #endregion

        #region in_option1

        private System.String _in_option1;

        /// <summary>
        /// in_option1
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_option1", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String in_option1
        {
            get
            {
                return this._in_option1;
            }
            set
            {
                this._in_option1 = value;
            }
        }

        #endregion

        #region in_option2

        private System.String _in_option2;

        /// <summary>
        /// in_option2
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_option2", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String in_option2
        {
            get
            {
                return this._in_option2;
            }
            set
            {
                this._in_option2 = value;
            }
        }

        #endregion

        #region in_option3

        private System.String _in_option3;

        /// <summary>
        /// in_option3
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_option3", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String in_option3
        {
            get
            {
                return this._in_option3;
            }
            set
            {
                this._in_option3 = value;
            }
        }

        #endregion

        #region in_option4

        private System.String _in_option4;

        /// <summary>
        /// in_option4
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_option4", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String in_option4
        {
            get
            {
                return this._in_option4;
            }
            set
            {
                this._in_option4 = value;
            }
        }

        #endregion

        #region in_option5

        private System.String _in_option5;

        /// <summary>
        /// in_option5
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_option5", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String in_option5
        {
            get
            {
                return this._in_option5;
            }
            set
            {
                this._in_option5 = value;
            }
        }

        #endregion

        #region in_desc

        private System.String _in_desc;

        /// <summary>
        /// in_desc
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_desc", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String in_desc
        {
            get
            {
                return this._in_desc;
            }
            set
            {
                this._in_desc = value;
            }
        }

        #endregion

        #region in_seq

        private System.Int32 _in_seq;

        /// <summary>
        /// in_seq
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_seq", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32 in_seq
        {
            get
            {
                return this._in_seq;
            }
            set
            {
                this._in_seq = value;
            }
        }

        #endregion

        #region in_cmd_id

        private System.String _in_cmd_id;

        /// <summary>
        /// in_cmd_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_cmd_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String in_cmd_id
        {
            get
            {
                return this._in_cmd_id;
            }
            set
            {
                this._in_cmd_id = value;
            }
        }

        #endregion

        #region in_contrast_id

        private System.String _in_contrast_id;

        /// <summary>
        /// in_contrast_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "in_contrast_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String in_contrast_id
        {
            get
            {
                return this._in_contrast_id;
            }
            set
            {
                this._in_contrast_id = value;
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

        public EntityDictDataInterfaceMapping ContrastInfo { get; set; }

        public override string ToString()
        {
            return this.in_name;
        }
    }
}
