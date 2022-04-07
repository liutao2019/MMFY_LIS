using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace dcl.svr.resultcheck
{
    [Lib.EntityCore.EntityTableAttribute(TableName = "patients", DisplayName = "病人信息表")]
    [Serializable]
    public class EntityPatients4Audit : Lib.EntityCore.BaseEntity
    {
        #region pat_id

        private System.String _pat_id;

        /// <summary>
        /// pat_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_id", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
        public System.String pat_id
        {
            get
            {
                return this._pat_id;
            }
            set
            {
                System.String oldValue = this._pat_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_id = value;
                    this.OnPropertyChanged("pat_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_itr_id

        private System.String _pat_itr_id;

        /// <summary>
        /// pat_itr_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_itr_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_itr_id
        {
            get
            {
                return this._pat_itr_id;
            }
            set
            {
                System.String oldValue = this._pat_itr_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_itr_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_itr_id = value;
                    this.OnPropertyChanged("pat_itr_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sid

        private System.String _pat_sid;

        /// <summary>
        /// pat_sid
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sid", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_sid
        {
            get
            {
                return this._pat_sid;
            }
            set
            {
                System.String oldValue = this._pat_sid;
                bool cancel = false;
                this.OnPropertyChanging("pat_sid", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sid = value;
                    this.OnPropertyChanged("pat_sid", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_name

        private System.String _pat_name;

        /// <summary>
        /// pat_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_name
        {
            get
            {
                return this._pat_name;
            }
            set
            {
                System.String oldValue = this._pat_name;
                bool cancel = false;
                this.OnPropertyChanging("pat_name", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_name = value;
                    this.OnPropertyChanged("pat_name", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sex

        private System.String _pat_sex;

        /// <summary>
        /// pat_sex
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sex", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_sex
        {
            get
            {
                return this._pat_sex;
            }
            set
            {
                System.String oldValue = this._pat_sex;
                bool cancel = false;
                this.OnPropertyChanging("pat_sex", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sex = value;
                    this.OnPropertyChanged("pat_sex", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_age

        private System.Decimal? _pat_age;

        /// <summary>
        /// pat_age
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_age", DbType = DbType.Decimal, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Decimal? pat_age
        {
            get
            {
                return this._pat_age;
            }
            set
            {
                System.Decimal? oldValue = this._pat_age;
                bool cancel = false;
                this.OnPropertyChanging("pat_age", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_age = value;
                    this.OnPropertyChanged("pat_age", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_age_exp

        private System.String _pat_age_exp;

        /// <summary>
        /// pat_age_exp
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_age_exp", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_age_exp
        {
            get
            {
                return this._pat_age_exp;
            }
            set
            {
                System.String oldValue = this._pat_age_exp;
                bool cancel = false;
                this.OnPropertyChanging("pat_age_exp", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_age_exp = value;
                    this.OnPropertyChanged("pat_age_exp", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_age_unit

        private System.String _pat_age_unit;

        /// <summary>
        /// pat_age_unit
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_age_unit", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_age_unit
        {
            get
            {
                return this._pat_age_unit;
            }
            set
            {
                System.String oldValue = this._pat_age_unit;
                bool cancel = false;
                this.OnPropertyChanging("pat_age_unit", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_age_unit = value;
                    this.OnPropertyChanged("pat_age_unit", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_dep_id

        private System.String _pat_dep_id;

        /// <summary>
        /// pat_dep_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_dep_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_dep_id
        {
            get
            {
                return this._pat_dep_id;
            }
            set
            {
                System.String oldValue = this._pat_dep_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_dep_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_dep_id = value;
                    this.OnPropertyChanged("pat_dep_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_no_id

        private System.String _pat_no_id;

        /// <summary>
        /// pat_no_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_no_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_no_id
        {
            get
            {
                return this._pat_no_id;
            }
            set
            {
                System.String oldValue = this._pat_no_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_no_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_no_id = value;
                    this.OnPropertyChanged("pat_no_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_in_no

        private System.String _pat_in_no;

        /// <summary>
        /// pat_in_no
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_in_no", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_in_no
        {
            get
            {
                return this._pat_in_no;
            }
            set
            {
                System.String oldValue = this._pat_in_no;
                bool cancel = false;
                this.OnPropertyChanging("pat_in_no", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_in_no = value;
                    this.OnPropertyChanged("pat_in_no", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_admiss_times

        private System.Int32? _pat_admiss_times;

        /// <summary>
        /// pat_admiss_times
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_admiss_times", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_admiss_times
        {
            get
            {
                return this._pat_admiss_times;
            }
            set
            {
                System.Int32? oldValue = this._pat_admiss_times;
                bool cancel = false;
                this.OnPropertyChanging("pat_admiss_times", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_admiss_times = value;
                    this.OnPropertyChanged("pat_admiss_times", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_bed_no

        private System.String _pat_bed_no;

        /// <summary>
        /// pat_bed_no
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_bed_no", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_bed_no
        {
            get
            {
                return this._pat_bed_no;
            }
            set
            {
                System.String oldValue = this._pat_bed_no;
                bool cancel = false;
                this.OnPropertyChanging("pat_bed_no", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_bed_no = value;
                    this.OnPropertyChanged("pat_bed_no", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_c_name

        private System.String _pat_c_name;

        /// <summary>
        /// pat_c_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_c_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_c_name
        {
            get
            {
                return this._pat_c_name;
            }
            set
            {
                System.String oldValue = this._pat_c_name;
                bool cancel = false;
                this.OnPropertyChanging("pat_c_name", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_c_name = value;
                    this.OnPropertyChanged("pat_c_name", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_diag

        private System.String _pat_diag;

        /// <summary>
        /// pat_diag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_diag", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_diag
        {
            get
            {
                return this._pat_diag;
            }
            set
            {
                System.String oldValue = this._pat_diag;
                bool cancel = false;
                this.OnPropertyChanging("pat_diag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_diag = value;
                    this.OnPropertyChanged("pat_diag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_rem

        private System.String _pat_rem;

        /// <summary>
        /// pat_rem
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_rem", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_rem
        {
            get
            {
                return this._pat_rem;
            }
            set
            {
                System.String oldValue = this._pat_rem;
                bool cancel = false;
                this.OnPropertyChanging("pat_rem", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_rem = value;
                    this.OnPropertyChanged("pat_rem", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_work

        private System.String _pat_work;

        /// <summary>
        /// pat_work
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_work", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_work
        {
            get
            {
                return this._pat_work;
            }
            set
            {
                System.String oldValue = this._pat_work;
                bool cancel = false;
                this.OnPropertyChanging("pat_work", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_work = value;
                    this.OnPropertyChanged("pat_work", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_tel

        private System.String _pat_tel;

        /// <summary>
        /// pat_tel
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_tel", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_tel
        {
            get
            {
                return this._pat_tel;
            }
            set
            {
                System.String oldValue = this._pat_tel;
                bool cancel = false;
                this.OnPropertyChanging("pat_tel", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_tel = value;
                    this.OnPropertyChanged("pat_tel", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_email

        private System.String _pat_email;

        /// <summary>
        /// pat_email
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_email", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_email
        {
            get
            {
                return this._pat_email;
            }
            set
            {
                System.String oldValue = this._pat_email;
                bool cancel = false;
                this.OnPropertyChanging("pat_email", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_email = value;
                    this.OnPropertyChanged("pat_email", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_unit

        private System.String _pat_unit;

        /// <summary>
        /// pat_unit
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_unit", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_unit
        {
            get
            {
                return this._pat_unit;
            }
            set
            {
                System.String oldValue = this._pat_unit;
                bool cancel = false;
                this.OnPropertyChanging("pat_unit", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_unit = value;
                    this.OnPropertyChanged("pat_unit", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_address

        private System.String _pat_address;

        /// <summary>
        /// pat_address
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_address", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_address
        {
            get
            {
                return this._pat_address;
            }
            set
            {
                System.String oldValue = this._pat_address;
                bool cancel = false;
                this.OnPropertyChanging("pat_address", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_address = value;
                    this.OnPropertyChanged("pat_address", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_pre_week

        private System.String _pat_pre_week;

        /// <summary>
        /// pat_pre_week
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_pre_week", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_pre_week
        {
            get
            {
                return this._pat_pre_week;
            }
            set
            {
                System.String oldValue = this._pat_pre_week;
                bool cancel = false;
                this.OnPropertyChanging("pat_pre_week", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_pre_week = value;
                    this.OnPropertyChanged("pat_pre_week", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_height

        private System.String _pat_height;

        /// <summary>
        /// pat_height
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_height", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_height
        {
            get
            {
                return this._pat_height;
            }
            set
            {
                System.String oldValue = this._pat_height;
                bool cancel = false;
                this.OnPropertyChanging("pat_height", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_height = value;
                    this.OnPropertyChanged("pat_height", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_weight

        private System.String _pat_weight;

        /// <summary>
        /// pat_weight
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_weight", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_weight
        {
            get
            {
                return this._pat_weight;
            }
            set
            {
                System.String oldValue = this._pat_weight;
                bool cancel = false;
                this.OnPropertyChanging("pat_weight", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_weight = value;
                    this.OnPropertyChanged("pat_weight", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sam_id

        private System.String _pat_sam_id;

        /// <summary>
        /// pat_sam_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sam_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_sam_id
        {
            get
            {
                return this._pat_sam_id;
            }
            set
            {
                System.String oldValue = this._pat_sam_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_sam_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sam_id = value;
                    this.OnPropertyChanged("pat_sam_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_chk_id

        private System.String _pat_chk_id;

        /// <summary>
        /// pat_chk_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_chk_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_chk_id
        {
            get
            {
                return this._pat_chk_id;
            }
            set
            {
                System.String oldValue = this._pat_chk_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_chk_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_chk_id = value;
                    this.OnPropertyChanged("pat_chk_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_doc_id

        private System.String _pat_doc_id;

        /// <summary>
        /// pat_doc_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_doc_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_doc_id
        {
            get
            {
                return this._pat_doc_id;
            }
            set
            {
                System.String oldValue = this._pat_doc_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_doc_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_doc_id = value;
                    this.OnPropertyChanged("pat_doc_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_doc_name

        private System.String _pat_doc_name;

        /// <summary>
        /// pat_doc_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_doc_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_doc_name
        {
            get
            {
                return this._pat_doc_name;
            }
            set
            {
                System.String oldValue = this._pat_doc_name;
                bool cancel = false;
                this.OnPropertyChanging("pat_doc_name", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_doc_name = value;
                    this.OnPropertyChanged("pat_doc_name", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_i_code

        private System.String _pat_i_code;

        /// <summary>
        /// pat_i_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_i_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_i_code
        {
            get
            {
                return this._pat_i_code;
            }
            set
            {
                System.String oldValue = this._pat_i_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_i_code", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_i_code = value;
                    this.OnPropertyChanged("pat_i_code", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_chk_code

        private System.String _pat_chk_code;

        /// <summary>
        /// pat_chk_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_chk_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_chk_code
        {
            get
            {
                return this._pat_chk_code;
            }
            set
            {
                System.String oldValue = this._pat_chk_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_chk_code", value, oldValue, out cancel);
                //if (cancel == false)
                //{
                this._pat_chk_code = value;
                this.OnPropertyChanged("pat_chk_code", value, oldValue);
                //}
            }
        }

        #endregion

        #region pat_send_code

        private System.String _pat_send_code;

        /// <summary>
        /// pat_send_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_send_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_send_code
        {
            get
            {
                return this._pat_send_code;
            }
            set
            {
                System.String oldValue = this._pat_send_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_send_code", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_send_code = value;
                    this.OnPropertyChanged("pat_send_code", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_report_code

        private System.String _pat_report_code;

        /// <summary>
        /// pat_report_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_report_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_report_code
        {
            get
            {
                return this._pat_report_code;
            }
            set
            {
                System.String oldValue = this._pat_report_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_report_code", value, oldValue, out cancel);
                //if (cancel == false)
                //{
                this._pat_report_code = value;
                this.OnPropertyChanged("pat_report_code", value, oldValue);
                //}
            }
        }

        #endregion

        #region pat_ctype

        private System.String _pat_ctype;

        /// <summary>
        /// pat_ctype
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_ctype", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_ctype
        {
            get
            {
                return this._pat_ctype;
            }
            set
            {
                System.String oldValue = this._pat_ctype;
                bool cancel = false;
                this.OnPropertyChanging("pat_ctype", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_ctype = value;
                    this.OnPropertyChanged("pat_ctype", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_send_flag

        private System.Int32? _pat_send_flag;

        /// <summary>
        /// pat_send_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_send_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_send_flag
        {
            get
            {
                return this._pat_send_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_send_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_send_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_send_flag = value;
                    this.OnPropertyChanged("pat_send_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_prt_flag

        private System.Int32? _pat_prt_flag;

        /// <summary>
        /// pat_prt_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_prt_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_prt_flag
        {
            get
            {
                return this._pat_prt_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_prt_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_prt_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_prt_flag = value;
                    this.OnPropertyChanged("pat_prt_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_flag

        private System.Int32? _pat_flag;

        /// <summary>
        /// pat_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_flag
        {
            get
            {
                return this._pat_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_flag = value;
                    this.OnPropertyChanged("pat_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_reg_flag

        private System.Int32? _pat_reg_flag;

        /// <summary>
        /// pat_reg_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_reg_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_reg_flag
        {
            get
            {
                return this._pat_reg_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_reg_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_reg_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_reg_flag = value;
                    this.OnPropertyChanged("pat_reg_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_urgent_flag

        private System.Int32? _pat_urgent_flag;

        /// <summary>
        /// pat_urgent_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_urgent_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_urgent_flag
        {
            get
            {
                return this._pat_urgent_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_urgent_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_urgent_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_urgent_flag = value;
                    this.OnPropertyChanged("pat_urgent_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_look_code

        private System.String _pat_look_code;

        /// <summary>
        /// pat_look_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_look_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_look_code
        {
            get
            {
                return this._pat_look_code;
            }
            set
            {
                System.String oldValue = this._pat_look_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_look_code", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_look_code = value;
                    this.OnPropertyChanged("pat_look_code", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_exp

        private System.String _pat_exp;

        /// <summary>
        /// pat_exp
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_exp", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_exp
        {
            get
            {
                return this._pat_exp;
            }
            set
            {
                System.String oldValue = this._pat_exp;
                bool cancel = false;
                this.OnPropertyChanging("pat_exp", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_exp = value;
                    this.OnPropertyChanged("pat_exp", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_pid

        private System.String _pat_pid;

        /// <summary>
        /// pat_pid
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_pid", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_pid
        {
            get
            {
                return this._pat_pid;
            }
            set
            {
                System.String oldValue = this._pat_pid;
                bool cancel = false;
                this.OnPropertyChanging("pat_pid", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_pid = value;
                    this.OnPropertyChanged("pat_pid", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_date

        private System.DateTime? _pat_date;

        /// <summary>
        /// pat_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_date
        {
            get
            {
                return this._pat_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_date = value;
                    this.OnPropertyChanged("pat_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sdate

        private System.DateTime? _pat_sdate;

        /// <summary>
        /// 标本收取时间
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sdate", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_sdate
        {
            get
            {
                return this._pat_sdate;
            }
            set
            {
                System.DateTime? oldValue = this._pat_sdate;
                bool cancel = false;
                this.OnPropertyChanging("pat_sdate", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sdate = value;
                    this.OnPropertyChanged("pat_sdate", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_rec_date

        private System.DateTime? _pat_rec_date;

        /// <summary>
        /// pat_rec_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_rec_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_rec_date
        {
            get
            {
                return this._pat_rec_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_rec_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_rec_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_rec_date = value;
                    this.OnPropertyChanged("pat_rec_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_chk_date

        private System.DateTime? _pat_chk_date;

        /// <summary>
        /// pat_chk_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_chk_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_chk_date
        {
            get
            {
                return this._pat_chk_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_chk_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_chk_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_chk_date = value;
                    this.OnPropertyChanged("pat_chk_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_report_date

        private System.DateTime? _pat_report_date;

        /// <summary>
        /// pat_report_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_report_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_report_date
        {
            get
            {
                return this._pat_report_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_report_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_report_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_report_date = value;
                    this.OnPropertyChanged("pat_report_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_send_date

        private System.DateTime? _pat_send_date;

        /// <summary>
        /// pat_send_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_send_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_send_date
        {
            get
            {
                return this._pat_send_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_send_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_send_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_send_date = value;
                    this.OnPropertyChanged("pat_send_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_look_date

        private System.DateTime? _pat_look_date;

        /// <summary>
        /// pat_look_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_look_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_look_date
        {
            get
            {
                return this._pat_look_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_look_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_look_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_look_date = value;
                    this.OnPropertyChanged("pat_look_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_social_no

        private System.String _pat_social_no;

        /// <summary>
        /// pat_social_no
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_social_no", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_social_no
        {
            get
            {
                return this._pat_social_no;
            }
            set
            {
                System.String oldValue = this._pat_social_no;
                bool cancel = false;
                this.OnPropertyChanging("pat_social_no", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_social_no = value;
                    this.OnPropertyChanged("pat_social_no", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_emp_id

        private System.String _pat_emp_id;

        /// <summary>
        /// pat_emp_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_emp_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_emp_id
        {
            get
            {
                return this._pat_emp_id;
            }
            set
            {
                System.String oldValue = this._pat_emp_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_emp_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_emp_id = value;
                    this.OnPropertyChanged("pat_emp_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_bar_code

        private System.String _pat_bar_code;

        /// <summary>
        /// pat_bar_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_bar_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_bar_code
        {
            get
            {
                return this._pat_bar_code;
            }
            set
            {
                System.String oldValue = this._pat_bar_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_bar_code", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_bar_code = value;
                    this.OnPropertyChanged("pat_bar_code", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_host_order

        private System.String _pat_host_order;

        /// <summary>
        /// pat_host_order
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_host_order", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_host_order
        {
            get
            {
                return this._pat_host_order;
            }
            set
            {
                System.String oldValue = this._pat_host_order;
                bool cancel = false;
                this.OnPropertyChanging("pat_host_order", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_host_order = value;
                    this.OnPropertyChanged("pat_host_order", value, oldValue);
                }
            }
        }

        #endregion

        #region Pat_etagere

        private System.String _Pat_etagere;

        /// <summary>
        /// Pat_etagere
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "Pat_etagere", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String Pat_etagere
        {
            get
            {
                return this._Pat_etagere;
            }
            set
            {
                System.String oldValue = this._Pat_etagere;
                bool cancel = false;
                this.OnPropertyChanging("Pat_etagere", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._Pat_etagere = value;
                    this.OnPropertyChanged("Pat_etagere", value, oldValue);
                }
            }
        }

        #endregion

        #region Pat_place

        private System.String _Pat_place;

        /// <summary>
        /// Pat_place
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "Pat_place", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String Pat_place
        {
            get
            {
                return this._Pat_place;
            }
            set
            {
                System.String oldValue = this._Pat_place;
                bool cancel = false;
                this.OnPropertyChanging("Pat_place", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._Pat_place = value;
                    this.OnPropertyChanged("Pat_place", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sample_date

        private System.DateTime? _pat_sample_date;

        /// <summary>
        /// 标本采集时间
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sample_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_sample_date
        {
            get
            {
                return this._pat_sample_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_sample_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_sample_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sample_date = value;
                    this.OnPropertyChanged("pat_sample_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_apply_date

        private System.DateTime? _pat_apply_date;

        /// <summary>
        /// 标本签收时间
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_apply_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_apply_date
        {
            get
            {
                return this._pat_apply_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_apply_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_apply_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_apply_date = value;
                    this.OnPropertyChanged("pat_apply_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_jy_date

        private System.DateTime? _pat_jy_date;

        /// <summary>
        /// pat_jy_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_jy_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_jy_date
        {
            get
            {
                return this._pat_jy_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_jy_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_jy_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_jy_date = value;
                    this.OnPropertyChanged("pat_jy_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_reach_date

        private System.DateTime? _pat_reach_date;

        /// <summary>
        /// 标本送达时间
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_reach_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_reach_date
        {
            get
            {
                return this._pat_reach_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_reach_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_reach_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_reach_date = value;
                    this.OnPropertyChanged("pat_reach_date", value, oldValue);
                }
            }
        }

        #endregion



        #region Pat_prt_date

        private System.DateTime? _Pat_prt_date;

        /// <summary>
        /// Pat_prt_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "Pat_prt_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? Pat_prt_date
        {
            get
            {
                return this._Pat_prt_date;
            }
            set
            {
                System.DateTime? oldValue = this._Pat_prt_date;
                bool cancel = false;
                this.OnPropertyChanging("Pat_prt_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._Pat_prt_date = value;
                    this.OnPropertyChanged("Pat_prt_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sample_part

        private System.String _pat_sample_part;

        /// <summary>
        /// pat_sample_part
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sample_part", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_sample_part
        {
            get
            {
                return this._pat_sample_part;
            }
            set
            {
                System.String oldValue = this._pat_sample_part;
                bool cancel = false;
                this.OnPropertyChanging("pat_sample_part", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sample_part = value;
                    this.OnPropertyChanged("pat_sample_part", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_ori_id

        private System.String _pat_ori_id;

        /// <summary>
        /// pat_ori_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_ori_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_ori_id
        {
            get
            {
                return this._pat_ori_id;
            }
            set
            {
                System.String oldValue = this._pat_ori_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_ori_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_ori_id = value;
                    this.OnPropertyChanged("pat_ori_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_mid_info

        private System.String _pat_mid_info;

        /// <summary>
        /// pat_mid_info
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_mid_info", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_mid_info
        {
            get
            {
                return this._pat_mid_info;
            }
            set
            {
                System.String oldValue = this._pat_mid_info;
                bool cancel = false;
                this.OnPropertyChanging("pat_mid_info", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_mid_info = value;
                    this.OnPropertyChanged("pat_mid_info", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_comment

        private System.String _pat_comment;

        /// <summary>
        /// pat_comment
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_comment", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_comment
        {
            get
            {
                return this._pat_comment;
            }
            set
            {
                System.String oldValue = this._pat_comment;
                bool cancel = false;
                this.OnPropertyChanging("pat_comment", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_comment = value;
                    this.OnPropertyChanged("pat_comment", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_hospital_id

        private System.String _pat_hospital_id;

        /// <summary>
        /// pat_hospital_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_hospital_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_hospital_id
        {
            get
            {
                return this._pat_hospital_id;
            }
            set
            {
                System.String oldValue = this._pat_hospital_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_hospital_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_hospital_id = value;
                    this.OnPropertyChanged("pat_hospital_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_modified_times

        private System.Int32? _pat_modified_times;

        /// <summary>
        /// pat_modified_times
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_modified_times", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_modified_times
        {
            get
            {
                return this._pat_modified_times;
            }
            set
            {
                System.Int32? oldValue = this._pat_modified_times;
                bool cancel = false;
                this.OnPropertyChanging("pat_modified_times", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_modified_times = value;
                    this.OnPropertyChanged("pat_modified_times", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_fee_type

        private System.String _pat_fee_type;

        /// <summary>
        /// pat_fee_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_fee_type", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_fee_type
        {
            get
            {
                return this._pat_fee_type;
            }
            set
            {
                System.String oldValue = this._pat_fee_type;
                bool cancel = false;
                this.OnPropertyChanging("pat_fee_type", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_fee_type = value;
                    this.OnPropertyChanged("pat_fee_type", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sam_rem

        private System.String _pat_sam_rem;

        /// <summary>
        /// pat_sam_rem
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sam_rem", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_sam_rem
        {
            get
            {
                return this._pat_sam_rem;
            }
            set
            {
                System.String oldValue = this._pat_sam_rem;
                bool cancel = false;
                this.OnPropertyChanging("pat_sam_rem", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sam_rem = value;
                    this.OnPropertyChanged("pat_sam_rem", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_key

        private System.Int64? _pat_key;

        /// <summary>
        /// pat_key
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_key", DbType = DbType.Int64, IsPrimaryKey = false, IsDBGenerate = true)]
        public System.Int64? pat_key
        {
            get
            {
                return this._pat_key;
            }
            set
            {
                System.Int64? oldValue = this._pat_key;
                bool cancel = false;
                this.OnPropertyChanging("pat_key", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_key = value;
                    this.OnPropertyChanged("pat_key", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sample_receive_date

        private System.DateTime? _pat_sample_receive_date;

        /// <summary>
        /// pat_sample_receive_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sample_receive_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_sample_receive_date
        {
            get
            {
                return this._pat_sample_receive_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_sample_receive_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_sample_receive_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sample_receive_date = value;
                    this.OnPropertyChanged("pat_sample_receive_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_dep_name

        private System.String _pat_dep_name;

        /// <summary>
        /// pat_dep_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_dep_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_dep_name
        {
            get
            {
                return this._pat_dep_name;
            }
            set
            {
                System.String oldValue = this._pat_dep_name;
                bool cancel = false;
                this.OnPropertyChanging("pat_dep_name", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_dep_name = value;
                    this.OnPropertyChanged("pat_dep_name", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_ward_id

        private System.String _pat_ward_id;

        /// <summary>
        /// pat_ward_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_ward_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_ward_id
        {
            get
            {
                return this._pat_ward_id;
            }
            set
            {
                System.String oldValue = this._pat_ward_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_ward_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_ward_id = value;
                    this.OnPropertyChanged("pat_ward_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_ward_name

        private System.String _pat_ward_name;

        /// <summary>
        /// pat_ward_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_ward_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_ward_name
        {
            get
            {
                return this._pat_ward_name;
            }
            set
            {
                System.String oldValue = this._pat_ward_name;
                bool cancel = false;
                this.OnPropertyChanging("pat_ward_name", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_ward_name = value;
                    this.OnPropertyChanged("pat_ward_name", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_recheck_flag

        private System.Int32? _pat_recheck_flag;

        /// <summary>
        /// pat_recheck_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_recheck_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_recheck_flag
        {
            get
            {
                return this._pat_recheck_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_recheck_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_recheck_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_recheck_flag = value;
                    this.OnPropertyChanged("pat_recheck_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_upid

        private System.String _pat_upid;

        /// <summary>
        /// pat_upid
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_upid", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_upid
        {
            get
            {
                return this._pat_upid;
            }
            set
            {
                System.String oldValue = this._pat_upid;
                bool cancel = false;
                this.OnPropertyChanging("pat_upid", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_upid = value;
                    this.OnPropertyChanged("pat_upid", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_itr_audit_flag

        private System.Int32? _pat_itr_audit_flag;

        /// <summary>
        /// pat_itr_audit_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_itr_audit_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_itr_audit_flag
        {
            get
            {
                return this._pat_itr_audit_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_itr_audit_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_itr_audit_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_itr_audit_flag = value;
                    this.OnPropertyChanged("pat_itr_audit_flag", value, oldValue);
                }
            }
        }

        #endregion


        #region pat_pre_flag

        private System.Int32? _pat_pre_flag;

        /// <summary>
        /// pat_recheck_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_pre_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_pre_flag
        {
            get
            {
                return this._pat_pre_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_pre_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_pre_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_pre_flag = value;
                    this.OnPropertyChanged("pat_pre_flag", value, oldValue);
                }
            }
        }

        #endregion


        #region pat_pre_code

        private System.String _pat_pre_code;

        /// <summary>
        /// pat_upid
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_pre_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_pre_code
        {
            get
            {
                return this._pat_pre_code;
            }
            set
            {
                System.String oldValue = this._pat_pre_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_pre_code", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_pre_code = value;
                    this.OnPropertyChanged("pat_pre_code", value, oldValue);
                }
            }
        }

        #endregion


        #region pat_pre_date

        private System.DateTime? _pat_pre_date;

        /// <summary>
        /// pat_pre_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_pre_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_pre_date
        {
            get
            {
                return this._pat_pre_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_pre_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_pre_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_pre_date = value;
                    this.OnPropertyChanged("pat_pre_date", value, oldValue);
                }
            }
        }

        #endregion


    }

    [Lib.EntityCore.EntityTableAttribute(TableName = "patients_newborn", DisplayName = "病人信息表")]
    [Serializable]
    public class EntityPatients4AuditForBf : Lib.EntityCore.BaseEntity
    {
        #region pat_id

        private System.String _pat_id;

        /// <summary>
        /// pat_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_id", DbType = DbType.AnsiString, IsPrimaryKey = true, IsDBGenerate = false)]
        public System.String pat_id
        {
            get
            {
                return this._pat_id;
            }
            set
            {
                System.String oldValue = this._pat_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_id = value;
                    this.OnPropertyChanged("pat_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_itr_id

        private System.String _pat_itr_id;

        /// <summary>
        /// pat_itr_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_itr_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_itr_id
        {
            get
            {
                return this._pat_itr_id;
            }
            set
            {
                System.String oldValue = this._pat_itr_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_itr_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_itr_id = value;
                    this.OnPropertyChanged("pat_itr_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sid

        private System.String _pat_sid;

        /// <summary>
        /// pat_sid
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sid", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_sid
        {
            get
            {
                return this._pat_sid;
            }
            set
            {
                System.String oldValue = this._pat_sid;
                bool cancel = false;
                this.OnPropertyChanging("pat_sid", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sid = value;
                    this.OnPropertyChanged("pat_sid", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_name

        private System.String _pat_name;

        /// <summary>
        /// pat_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_name
        {
            get
            {
                return this._pat_name;
            }
            set
            {
                System.String oldValue = this._pat_name;
                bool cancel = false;
                this.OnPropertyChanging("pat_name", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_name = value;
                    this.OnPropertyChanged("pat_name", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sex

        private System.String _pat_sex;

        /// <summary>
        /// pat_sex
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sex", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_sex
        {
            get
            {
                return this._pat_sex;
            }
            set
            {
                System.String oldValue = this._pat_sex;
                bool cancel = false;
                this.OnPropertyChanging("pat_sex", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sex = value;
                    this.OnPropertyChanged("pat_sex", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_age

        private System.Decimal? _pat_age;

        /// <summary>
        /// pat_age
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_age", DbType = DbType.Decimal, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Decimal? pat_age
        {
            get
            {
                return this._pat_age;
            }
            set
            {
                System.Decimal? oldValue = this._pat_age;
                bool cancel = false;
                this.OnPropertyChanging("pat_age", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_age = value;
                    this.OnPropertyChanged("pat_age", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_age_exp

        private System.String _pat_age_exp;

        /// <summary>
        /// pat_age_exp
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_age_exp", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_age_exp
        {
            get
            {
                return this._pat_age_exp;
            }
            set
            {
                System.String oldValue = this._pat_age_exp;
                bool cancel = false;
                this.OnPropertyChanging("pat_age_exp", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_age_exp = value;
                    this.OnPropertyChanged("pat_age_exp", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_age_unit

        private System.String _pat_age_unit;

        /// <summary>
        /// pat_age_unit
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_age_unit", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_age_unit
        {
            get
            {
                return this._pat_age_unit;
            }
            set
            {
                System.String oldValue = this._pat_age_unit;
                bool cancel = false;
                this.OnPropertyChanging("pat_age_unit", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_age_unit = value;
                    this.OnPropertyChanged("pat_age_unit", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_dep_id

        private System.String _pat_dep_id;

        /// <summary>
        /// pat_dep_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_dep_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_dep_id
        {
            get
            {
                return this._pat_dep_id;
            }
            set
            {
                System.String oldValue = this._pat_dep_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_dep_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_dep_id = value;
                    this.OnPropertyChanged("pat_dep_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_no_id

        private System.String _pat_no_id;

        /// <summary>
        /// pat_no_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_no_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_no_id
        {
            get
            {
                return this._pat_no_id;
            }
            set
            {
                System.String oldValue = this._pat_no_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_no_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_no_id = value;
                    this.OnPropertyChanged("pat_no_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_in_no

        private System.String _pat_in_no;

        /// <summary>
        /// pat_in_no
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_in_no", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_in_no
        {
            get
            {
                return this._pat_in_no;
            }
            set
            {
                System.String oldValue = this._pat_in_no;
                bool cancel = false;
                this.OnPropertyChanging("pat_in_no", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_in_no = value;
                    this.OnPropertyChanged("pat_in_no", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_admiss_times

        private System.Int32? _pat_admiss_times;

        /// <summary>
        /// pat_admiss_times
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_admiss_times", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_admiss_times
        {
            get
            {
                return this._pat_admiss_times;
            }
            set
            {
                System.Int32? oldValue = this._pat_admiss_times;
                bool cancel = false;
                this.OnPropertyChanging("pat_admiss_times", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_admiss_times = value;
                    this.OnPropertyChanged("pat_admiss_times", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_bed_no

        private System.String _pat_bed_no;

        /// <summary>
        /// pat_bed_no
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_bed_no", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_bed_no
        {
            get
            {
                return this._pat_bed_no;
            }
            set
            {
                System.String oldValue = this._pat_bed_no;
                bool cancel = false;
                this.OnPropertyChanging("pat_bed_no", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_bed_no = value;
                    this.OnPropertyChanged("pat_bed_no", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_c_name

        private System.String _pat_c_name;

        /// <summary>
        /// pat_c_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_c_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_c_name
        {
            get
            {
                return this._pat_c_name;
            }
            set
            {
                System.String oldValue = this._pat_c_name;
                bool cancel = false;
                this.OnPropertyChanging("pat_c_name", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_c_name = value;
                    this.OnPropertyChanged("pat_c_name", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_diag

        private System.String _pat_diag;

        /// <summary>
        /// pat_diag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_diag", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_diag
        {
            get
            {
                return this._pat_diag;
            }
            set
            {
                System.String oldValue = this._pat_diag;
                bool cancel = false;
                this.OnPropertyChanging("pat_diag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_diag = value;
                    this.OnPropertyChanged("pat_diag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_rem

        private System.String _pat_rem;

        /// <summary>
        /// pat_rem
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_rem", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_rem
        {
            get
            {
                return this._pat_rem;
            }
            set
            {
                System.String oldValue = this._pat_rem;
                bool cancel = false;
                this.OnPropertyChanging("pat_rem", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_rem = value;
                    this.OnPropertyChanged("pat_rem", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_work

        private System.String _pat_work;

        /// <summary>
        /// pat_work
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_work", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_work
        {
            get
            {
                return this._pat_work;
            }
            set
            {
                System.String oldValue = this._pat_work;
                bool cancel = false;
                this.OnPropertyChanging("pat_work", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_work = value;
                    this.OnPropertyChanged("pat_work", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_tel

        private System.String _pat_tel;

        /// <summary>
        /// pat_tel
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_tel", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_tel
        {
            get
            {
                return this._pat_tel;
            }
            set
            {
                System.String oldValue = this._pat_tel;
                bool cancel = false;
                this.OnPropertyChanging("pat_tel", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_tel = value;
                    this.OnPropertyChanged("pat_tel", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_email

        private System.String _pat_email;

        /// <summary>
        /// pat_email
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_email", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_email
        {
            get
            {
                return this._pat_email;
            }
            set
            {
                System.String oldValue = this._pat_email;
                bool cancel = false;
                this.OnPropertyChanging("pat_email", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_email = value;
                    this.OnPropertyChanged("pat_email", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_unit

        private System.String _pat_unit;

        /// <summary>
        /// pat_unit
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_unit", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_unit
        {
            get
            {
                return this._pat_unit;
            }
            set
            {
                System.String oldValue = this._pat_unit;
                bool cancel = false;
                this.OnPropertyChanging("pat_unit", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_unit = value;
                    this.OnPropertyChanged("pat_unit", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_address

        private System.String _pat_address;

        /// <summary>
        /// pat_address
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_address", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_address
        {
            get
            {
                return this._pat_address;
            }
            set
            {
                System.String oldValue = this._pat_address;
                bool cancel = false;
                this.OnPropertyChanging("pat_address", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_address = value;
                    this.OnPropertyChanged("pat_address", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_pre_week

        private System.String _pat_pre_week;

        /// <summary>
        /// pat_pre_week
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_pre_week", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_pre_week
        {
            get
            {
                return this._pat_pre_week;
            }
            set
            {
                System.String oldValue = this._pat_pre_week;
                bool cancel = false;
                this.OnPropertyChanging("pat_pre_week", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_pre_week = value;
                    this.OnPropertyChanged("pat_pre_week", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_height

        private System.String _pat_height;

        /// <summary>
        /// pat_height
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_height", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_height
        {
            get
            {
                return this._pat_height;
            }
            set
            {
                System.String oldValue = this._pat_height;
                bool cancel = false;
                this.OnPropertyChanging("pat_height", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_height = value;
                    this.OnPropertyChanged("pat_height", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_weight

        private System.String _pat_weight;

        /// <summary>
        /// pat_weight
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_weight", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_weight
        {
            get
            {
                return this._pat_weight;
            }
            set
            {
                System.String oldValue = this._pat_weight;
                bool cancel = false;
                this.OnPropertyChanging("pat_weight", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_weight = value;
                    this.OnPropertyChanged("pat_weight", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sam_id

        private System.String _pat_sam_id;

        /// <summary>
        /// pat_sam_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sam_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_sam_id
        {
            get
            {
                return this._pat_sam_id;
            }
            set
            {
                System.String oldValue = this._pat_sam_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_sam_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sam_id = value;
                    this.OnPropertyChanged("pat_sam_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_chk_id

        private System.String _pat_chk_id;

        /// <summary>
        /// pat_chk_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_chk_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_chk_id
        {
            get
            {
                return this._pat_chk_id;
            }
            set
            {
                System.String oldValue = this._pat_chk_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_chk_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_chk_id = value;
                    this.OnPropertyChanged("pat_chk_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_doc_id

        private System.String _pat_doc_id;

        /// <summary>
        /// pat_doc_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_doc_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_doc_id
        {
            get
            {
                return this._pat_doc_id;
            }
            set
            {
                System.String oldValue = this._pat_doc_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_doc_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_doc_id = value;
                    this.OnPropertyChanged("pat_doc_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_doc_name

        private System.String _pat_doc_name;

        /// <summary>
        /// pat_doc_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_doc_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_doc_name
        {
            get
            {
                return this._pat_doc_name;
            }
            set
            {
                System.String oldValue = this._pat_doc_name;
                bool cancel = false;
                this.OnPropertyChanging("pat_doc_name", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_doc_name = value;
                    this.OnPropertyChanged("pat_doc_name", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_i_code

        private System.String _pat_i_code;

        /// <summary>
        /// pat_i_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_i_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_i_code
        {
            get
            {
                return this._pat_i_code;
            }
            set
            {
                System.String oldValue = this._pat_i_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_i_code", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_i_code = value;
                    this.OnPropertyChanged("pat_i_code", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_chk_code

        private System.String _pat_chk_code;

        /// <summary>
        /// pat_chk_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_chk_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_chk_code
        {
            get
            {
                return this._pat_chk_code;
            }
            set
            {
                System.String oldValue = this._pat_chk_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_chk_code", value, oldValue, out cancel);
                //if (cancel == false)
                //{
                this._pat_chk_code = value;
                this.OnPropertyChanged("pat_chk_code", value, oldValue);
                //}
            }
        }

        #endregion

        #region pat_send_code

        private System.String _pat_send_code;

        /// <summary>
        /// pat_send_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_send_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_send_code
        {
            get
            {
                return this._pat_send_code;
            }
            set
            {
                System.String oldValue = this._pat_send_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_send_code", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_send_code = value;
                    this.OnPropertyChanged("pat_send_code", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_report_code

        private System.String _pat_report_code;

        /// <summary>
        /// pat_report_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_report_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_report_code
        {
            get
            {
                return this._pat_report_code;
            }
            set
            {
                System.String oldValue = this._pat_report_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_report_code", value, oldValue, out cancel);
                //if (cancel == false)
                //{
                this._pat_report_code = value;
                this.OnPropertyChanged("pat_report_code", value, oldValue);
                //}
            }
        }

        #endregion

        #region pat_ctype

        private System.String _pat_ctype;

        /// <summary>
        /// pat_ctype
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_ctype", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_ctype
        {
            get
            {
                return this._pat_ctype;
            }
            set
            {
                System.String oldValue = this._pat_ctype;
                bool cancel = false;
                this.OnPropertyChanging("pat_ctype", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_ctype = value;
                    this.OnPropertyChanged("pat_ctype", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_send_flag

        private System.Int32? _pat_send_flag;

        /// <summary>
        /// pat_send_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_send_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_send_flag
        {
            get
            {
                return this._pat_send_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_send_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_send_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_send_flag = value;
                    this.OnPropertyChanged("pat_send_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_prt_flag

        private System.Int32? _pat_prt_flag;

        /// <summary>
        /// pat_prt_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_prt_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_prt_flag
        {
            get
            {
                return this._pat_prt_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_prt_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_prt_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_prt_flag = value;
                    this.OnPropertyChanged("pat_prt_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_flag

        private System.Int32? _pat_flag;

        /// <summary>
        /// pat_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_flag
        {
            get
            {
                return this._pat_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_flag = value;
                    this.OnPropertyChanged("pat_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_reg_flag

        private System.Int32? _pat_reg_flag;

        /// <summary>
        /// pat_reg_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_reg_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_reg_flag
        {
            get
            {
                return this._pat_reg_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_reg_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_reg_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_reg_flag = value;
                    this.OnPropertyChanged("pat_reg_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_urgent_flag

        private System.Int32? _pat_urgent_flag;

        /// <summary>
        /// pat_urgent_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_urgent_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_urgent_flag
        {
            get
            {
                return this._pat_urgent_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_urgent_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_urgent_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_urgent_flag = value;
                    this.OnPropertyChanged("pat_urgent_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_look_code

        private System.String _pat_look_code;

        /// <summary>
        /// pat_look_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_look_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_look_code
        {
            get
            {
                return this._pat_look_code;
            }
            set
            {
                System.String oldValue = this._pat_look_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_look_code", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_look_code = value;
                    this.OnPropertyChanged("pat_look_code", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_exp

        private System.String _pat_exp;

        /// <summary>
        /// pat_exp
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_exp", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_exp
        {
            get
            {
                return this._pat_exp;
            }
            set
            {
                System.String oldValue = this._pat_exp;
                bool cancel = false;
                this.OnPropertyChanging("pat_exp", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_exp = value;
                    this.OnPropertyChanged("pat_exp", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_pid

        private System.String _pat_pid;

        /// <summary>
        /// pat_pid
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_pid", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_pid
        {
            get
            {
                return this._pat_pid;
            }
            set
            {
                System.String oldValue = this._pat_pid;
                bool cancel = false;
                this.OnPropertyChanging("pat_pid", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_pid = value;
                    this.OnPropertyChanged("pat_pid", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_date

        private System.DateTime? _pat_date;

        /// <summary>
        /// pat_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_date
        {
            get
            {
                return this._pat_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_date = value;
                    this.OnPropertyChanged("pat_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sdate

        private System.DateTime? _pat_sdate;

        /// <summary>
        /// 标本收取时间
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sdate", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_sdate
        {
            get
            {
                return this._pat_sdate;
            }
            set
            {
                System.DateTime? oldValue = this._pat_sdate;
                bool cancel = false;
                this.OnPropertyChanging("pat_sdate", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sdate = value;
                    this.OnPropertyChanged("pat_sdate", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_rec_date

        private System.DateTime? _pat_rec_date;

        /// <summary>
        /// pat_rec_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_rec_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_rec_date
        {
            get
            {
                return this._pat_rec_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_rec_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_rec_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_rec_date = value;
                    this.OnPropertyChanged("pat_rec_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_chk_date

        private System.DateTime? _pat_chk_date;

        /// <summary>
        /// pat_chk_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_chk_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_chk_date
        {
            get
            {
                return this._pat_chk_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_chk_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_chk_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_chk_date = value;
                    this.OnPropertyChanged("pat_chk_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_report_date

        private System.DateTime? _pat_report_date;

        /// <summary>
        /// pat_report_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_report_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_report_date
        {
            get
            {
                return this._pat_report_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_report_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_report_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_report_date = value;
                    this.OnPropertyChanged("pat_report_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_send_date

        private System.DateTime? _pat_send_date;

        /// <summary>
        /// pat_send_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_send_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_send_date
        {
            get
            {
                return this._pat_send_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_send_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_send_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_send_date = value;
                    this.OnPropertyChanged("pat_send_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_look_date

        private System.DateTime? _pat_look_date;

        /// <summary>
        /// pat_look_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_look_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_look_date
        {
            get
            {
                return this._pat_look_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_look_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_look_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_look_date = value;
                    this.OnPropertyChanged("pat_look_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_social_no

        private System.String _pat_social_no;

        /// <summary>
        /// pat_social_no
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_social_no", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_social_no
        {
            get
            {
                return this._pat_social_no;
            }
            set
            {
                System.String oldValue = this._pat_social_no;
                bool cancel = false;
                this.OnPropertyChanging("pat_social_no", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_social_no = value;
                    this.OnPropertyChanged("pat_social_no", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_emp_id

        private System.String _pat_emp_id;

        /// <summary>
        /// pat_emp_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_emp_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_emp_id
        {
            get
            {
                return this._pat_emp_id;
            }
            set
            {
                System.String oldValue = this._pat_emp_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_emp_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_emp_id = value;
                    this.OnPropertyChanged("pat_emp_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_bar_code

        private System.String _pat_bar_code;

        /// <summary>
        /// pat_bar_code
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_bar_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_bar_code
        {
            get
            {
                return this._pat_bar_code;
            }
            set
            {
                System.String oldValue = this._pat_bar_code;
                bool cancel = false;
                this.OnPropertyChanging("pat_bar_code", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_bar_code = value;
                    this.OnPropertyChanged("pat_bar_code", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_host_order

        private System.String _pat_host_order;

        /// <summary>
        /// pat_host_order
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_host_order", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_host_order
        {
            get
            {
                return this._pat_host_order;
            }
            set
            {
                System.String oldValue = this._pat_host_order;
                bool cancel = false;
                this.OnPropertyChanging("pat_host_order", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_host_order = value;
                    this.OnPropertyChanged("pat_host_order", value, oldValue);
                }
            }
        }

        #endregion

        #region Pat_etagere

        private System.String _Pat_etagere;

        /// <summary>
        /// Pat_etagere
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "Pat_etagere", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String Pat_etagere
        {
            get
            {
                return this._Pat_etagere;
            }
            set
            {
                System.String oldValue = this._Pat_etagere;
                bool cancel = false;
                this.OnPropertyChanging("Pat_etagere", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._Pat_etagere = value;
                    this.OnPropertyChanged("Pat_etagere", value, oldValue);
                }
            }
        }

        #endregion

        #region Pat_place

        private System.String _Pat_place;

        /// <summary>
        /// Pat_place
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "Pat_place", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String Pat_place
        {
            get
            {
                return this._Pat_place;
            }
            set
            {
                System.String oldValue = this._Pat_place;
                bool cancel = false;
                this.OnPropertyChanging("Pat_place", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._Pat_place = value;
                    this.OnPropertyChanged("Pat_place", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sample_date

        private System.DateTime? _pat_sample_date;

        /// <summary>
        /// 标本采集时间
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sample_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_sample_date
        {
            get
            {
                return this._pat_sample_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_sample_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_sample_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sample_date = value;
                    this.OnPropertyChanged("pat_sample_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_apply_date

        private System.DateTime? _pat_apply_date;

        /// <summary>
        /// 标本签收时间
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_apply_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_apply_date
        {
            get
            {
                return this._pat_apply_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_apply_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_apply_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_apply_date = value;
                    this.OnPropertyChanged("pat_apply_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_jy_date

        private System.DateTime? _pat_jy_date;

        /// <summary>
        /// pat_jy_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_jy_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_jy_date
        {
            get
            {
                return this._pat_jy_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_jy_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_jy_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_jy_date = value;
                    this.OnPropertyChanged("pat_jy_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_reach_date

        private System.DateTime? _pat_reach_date;

        /// <summary>
        /// 标本送达时间
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_reach_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_reach_date
        {
            get
            {
                return this._pat_reach_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_reach_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_reach_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_reach_date = value;
                    this.OnPropertyChanged("pat_reach_date", value, oldValue);
                }
            }
        }

        #endregion



        #region Pat_prt_date

        private System.DateTime? _Pat_prt_date;

        /// <summary>
        /// Pat_prt_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "Pat_prt_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? Pat_prt_date
        {
            get
            {
                return this._Pat_prt_date;
            }
            set
            {
                System.DateTime? oldValue = this._Pat_prt_date;
                bool cancel = false;
                this.OnPropertyChanging("Pat_prt_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._Pat_prt_date = value;
                    this.OnPropertyChanged("Pat_prt_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sample_part

        private System.String _pat_sample_part;

        /// <summary>
        /// pat_sample_part
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sample_part", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_sample_part
        {
            get
            {
                return this._pat_sample_part;
            }
            set
            {
                System.String oldValue = this._pat_sample_part;
                bool cancel = false;
                this.OnPropertyChanging("pat_sample_part", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sample_part = value;
                    this.OnPropertyChanged("pat_sample_part", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_ori_id

        private System.String _pat_ori_id;

        /// <summary>
        /// pat_ori_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_ori_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_ori_id
        {
            get
            {
                return this._pat_ori_id;
            }
            set
            {
                System.String oldValue = this._pat_ori_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_ori_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_ori_id = value;
                    this.OnPropertyChanged("pat_ori_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_mid_info

        private System.String _pat_mid_info;

        /// <summary>
        /// pat_mid_info
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_mid_info", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_mid_info
        {
            get
            {
                return this._pat_mid_info;
            }
            set
            {
                System.String oldValue = this._pat_mid_info;
                bool cancel = false;
                this.OnPropertyChanging("pat_mid_info", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_mid_info = value;
                    this.OnPropertyChanged("pat_mid_info", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_comment

        private System.String _pat_comment;

        /// <summary>
        /// pat_comment
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_comment", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_comment
        {
            get
            {
                return this._pat_comment;
            }
            set
            {
                System.String oldValue = this._pat_comment;
                bool cancel = false;
                this.OnPropertyChanging("pat_comment", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_comment = value;
                    this.OnPropertyChanged("pat_comment", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_hospital_id

        private System.String _pat_hospital_id;

        /// <summary>
        /// pat_hospital_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_hospital_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_hospital_id
        {
            get
            {
                return this._pat_hospital_id;
            }
            set
            {
                System.String oldValue = this._pat_hospital_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_hospital_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_hospital_id = value;
                    this.OnPropertyChanged("pat_hospital_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_modified_times

        private System.Int32? _pat_modified_times;

        /// <summary>
        /// pat_modified_times
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_modified_times", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_modified_times
        {
            get
            {
                return this._pat_modified_times;
            }
            set
            {
                System.Int32? oldValue = this._pat_modified_times;
                bool cancel = false;
                this.OnPropertyChanging("pat_modified_times", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_modified_times = value;
                    this.OnPropertyChanged("pat_modified_times", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_fee_type

        private System.String _pat_fee_type;

        /// <summary>
        /// pat_fee_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_fee_type", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_fee_type
        {
            get
            {
                return this._pat_fee_type;
            }
            set
            {
                System.String oldValue = this._pat_fee_type;
                bool cancel = false;
                this.OnPropertyChanging("pat_fee_type", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_fee_type = value;
                    this.OnPropertyChanged("pat_fee_type", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sam_rem

        private System.String _pat_sam_rem;

        /// <summary>
        /// pat_sam_rem
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sam_rem", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_sam_rem
        {
            get
            {
                return this._pat_sam_rem;
            }
            set
            {
                System.String oldValue = this._pat_sam_rem;
                bool cancel = false;
                this.OnPropertyChanging("pat_sam_rem", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sam_rem = value;
                    this.OnPropertyChanged("pat_sam_rem", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_key

        private System.Int64? _pat_key;

        /// <summary>
        /// pat_key
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_key", DbType = DbType.Int64, IsPrimaryKey = false, IsDBGenerate = true)]
        public System.Int64? pat_key
        {
            get
            {
                return this._pat_key;
            }
            set
            {
                System.Int64? oldValue = this._pat_key;
                bool cancel = false;
                this.OnPropertyChanging("pat_key", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_key = value;
                    this.OnPropertyChanged("pat_key", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_sample_receive_date

        private System.DateTime? _pat_sample_receive_date;

        /// <summary>
        /// pat_sample_receive_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_sample_receive_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_sample_receive_date
        {
            get
            {
                return this._pat_sample_receive_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_sample_receive_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_sample_receive_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_sample_receive_date = value;
                    this.OnPropertyChanged("pat_sample_receive_date", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_dep_name

        private System.String _pat_dep_name;

        /// <summary>
        /// pat_dep_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_dep_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_dep_name
        {
            get
            {
                return this._pat_dep_name;
            }
            set
            {
                System.String oldValue = this._pat_dep_name;
                bool cancel = false;
                this.OnPropertyChanging("pat_dep_name", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_dep_name = value;
                    this.OnPropertyChanged("pat_dep_name", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_ward_id

        private System.String _pat_ward_id;

        /// <summary>
        /// pat_ward_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_ward_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_ward_id
        {
            get
            {
                return this._pat_ward_id;
            }
            set
            {
                System.String oldValue = this._pat_ward_id;
                bool cancel = false;
                this.OnPropertyChanging("pat_ward_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_ward_id = value;
                    this.OnPropertyChanged("pat_ward_id", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_ward_name

        private System.String _pat_ward_name;

        /// <summary>
        /// pat_ward_name
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_ward_name", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_ward_name
        {
            get
            {
                return this._pat_ward_name;
            }
            set
            {
                System.String oldValue = this._pat_ward_name;
                bool cancel = false;
                this.OnPropertyChanging("pat_ward_name", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_ward_name = value;
                    this.OnPropertyChanged("pat_ward_name", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_recheck_flag

        private System.Int32? _pat_recheck_flag;

        /// <summary>
        /// pat_recheck_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_recheck_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_recheck_flag
        {
            get
            {
                return this._pat_recheck_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_recheck_flag;
                bool cancel = false;
                this.OnPropertyChanging("pat_recheck_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_recheck_flag = value;
                    this.OnPropertyChanged("pat_recheck_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_upid

        private System.String _pat_upid;

        /// <summary>
        /// pat_upid
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_upid", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_upid
        {
            get
            {
                return this._pat_upid;
            }
            set
            {
                System.String oldValue = this._pat_upid;
                bool cancel = false;
                this.OnPropertyChanging("pat_upid", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_upid = value;
                    this.OnPropertyChanged("pat_upid", value, oldValue);
                }
            }
        }

        #endregion

        #region pat_pre_flag

        private System.Int32? _pat_pre_flag;

        /// <summary>
        /// pat_recheck_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_pre_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? pat_pre_flag
        {
            get
            {
                return this._pat_pre_flag;
            }
            set
            {
                System.Int32? oldValue = this._pat_pre_flag;
                bool cancel = false;
                this.OnPropertyChanging("_pat_pre_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_pre_flag = value;
                    this.OnPropertyChanged("_pat_pre_flag", value, oldValue);
                }
            }
        }

        #endregion


        #region pat_pre_code

        private System.String _pat_pre_code;

        /// <summary>
        /// pat_upid
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_pre_code", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String pat_pre_code
        {
            get
            {
                return this._pat_pre_code;
            }
            set
            {
                System.String oldValue = this._pat_pre_code;
                bool cancel = false;
                this.OnPropertyChanging("_pat_pre_code", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_pre_code = value;
                    this.OnPropertyChanged("_pat_pre_code", value, oldValue);
                }
            }
        }

        #endregion


        #region pat_pre_date

        private System.DateTime? _pat_pre_date;

        /// <summary>
        /// pat_pre_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "pat_pre_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? pat_pre_date
        {
            get
            {
                return this._pat_pre_date;
            }
            set
            {
                System.DateTime? oldValue = this._pat_pre_date;
                bool cancel = false;
                this.OnPropertyChanging("pat_pre_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._pat_pre_date = value;
                    this.OnPropertyChanged("pat_pre_date", value, oldValue);
                }
            }
        }

        #endregion




      

    }
}

