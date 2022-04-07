using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dcl.pub.entities;
using dcl.pub.entities.dict;

namespace dcl.svr.resultcheck
{
    /// <summary>
    /// resulto
    /// </summary>
    [Lib.EntityCore.EntityTableAttribute(TableName = "resulto", DisplayName = "结果表")]
    public class EntityResulto4Audit : Lib.EntityCore.BaseEntity
    {
        public EntityDictItemSam referenceSam = null;
        public EntityDictItemMi referenceValue = null;
        public EntityDictItem refItem = null;

        #region res_key

        private System.Int64? _res_key;

        /// <summary>
        /// res_key
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_key", DbType = DbType.Int64, IsPrimaryKey = true, IsDBGenerate = false)]
        public System.Int64? res_key
        {
            get
            {
                return this._res_key;
            }
            set
            {
                System.Int64? oldValue = this._res_key;
                bool cancel = false;
                this.OnPropertyChanging("res_key", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_key = value;
                    this.OnPropertyChanged("res_key", value, oldValue);
                }
            }
        }

        #endregion

        #region res_id

        private System.String _res_id;

        /// <summary>
        /// res_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_id
        {
            get
            {
                return this._res_id;
            }
            set
            {
                System.String oldValue = this._res_id;
                bool cancel = false;
                this.OnPropertyChanging("res_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_id = value;
                    this.OnPropertyChanged("res_id", value, oldValue);
                }
            }
        }

        #endregion

        #region res_itr_id

        private System.String _res_itr_id;

        /// <summary>
        /// res_itr_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_itr_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_itr_id
        {
            get
            {
                return this._res_itr_id;
            }
            set
            {
                System.String oldValue = this._res_itr_id;
                bool cancel = false;
                this.OnPropertyChanging("res_itr_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_itr_id = value;
                    this.OnPropertyChanged("res_itr_id", value, oldValue);
                }
            }
        }

        #endregion

        #region res_sid

        private System.String _res_sid;

        /// <summary>
        /// res_sid
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_sid", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_sid
        {
            get
            {
                return this._res_sid;
            }
            set
            {
                System.String oldValue = this._res_sid;
                bool cancel = false;
                this.OnPropertyChanging("res_sid", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_sid = value;
                    this.OnPropertyChanged("res_sid", value, oldValue);
                }
            }
        }

        #endregion

        #region res_itm_id

        private System.String _res_itm_id;

        /// <summary>
        /// res_itm_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_itm_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_itm_id
        {
            get
            {
                return this._res_itm_id;
            }
            set
            {
                System.String oldValue = this._res_itm_id;
                bool cancel = false;
                this.OnPropertyChanging("res_itm_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_itm_id = value;
                    this.OnPropertyChanged("res_itm_id", value, oldValue);
                }
            }
        }

        #endregion

        #region res_itm_ecd

        private System.String _res_itm_ecd;

        /// <summary>
        /// res_itm_ecd
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_itm_ecd", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_itm_ecd
        {
            get
            {
                return this._res_itm_ecd;
            }
            set
            {
                System.String oldValue = this._res_itm_ecd;
                bool cancel = false;
                this.OnPropertyChanging("res_itm_ecd", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_itm_ecd = value;
                    this.OnPropertyChanged("res_itm_ecd", value, oldValue);
                }
            }
        }

        #endregion

        #region res_chr

        private System.String _res_chr;

        /// <summary>
        /// res_chr
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_chr", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_chr
        {
            get
            {
                return this._res_chr;
            }
            set
            {
                System.String oldValue = this._res_chr;
                bool cancel = false;
                this.OnPropertyChanging("res_chr", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_chr = value;
                    this.OnPropertyChanged("res_chr", value, oldValue);
                }
            }
        }

        #endregion

        #region res_od_chr

        private System.String _res_od_chr;

        /// <summary>
        /// res_od_chr
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_od_chr", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_od_chr
        {
            get
            {
                return this._res_od_chr;
            }
            set
            {
                System.String oldValue = this._res_od_chr;
                bool cancel = false;
                this.OnPropertyChanging("res_od_chr", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_od_chr = value;
                    this.OnPropertyChanged("res_od_chr", value, oldValue);
                }
            }
        }

        #endregion

        #region res_cast_chr

        private System.Decimal? _res_cast_chr;

        /// <summary>
        /// res_cast_chr
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_cast_chr", DbType = DbType.Decimal, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Decimal? res_cast_chr
        {
            get
            {
                return this._res_cast_chr;
            }
            set
            {
                System.Decimal? oldValue = this._res_cast_chr;
                bool cancel = false;
                this.OnPropertyChanging("res_cast_chr", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_cast_chr = value;
                    this.OnPropertyChanged("res_cast_chr", value, oldValue);
                }
            }
        }

        #endregion

        #region res_unit

        private System.String _res_unit;

        /// <summary>
        /// res_unit
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_unit", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_unit
        {
            get
            {
                return this._res_unit;
            }
            set
            {
                System.String oldValue = this._res_unit;
                bool cancel = false;
                this.OnPropertyChanging("res_unit", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_unit = value;
                    this.OnPropertyChanged("res_unit", value, oldValue);
                }
            }
        }

        #endregion

        #region res_price

        private System.Decimal? _res_price;

        /// <summary>
        /// res_price
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_price", DbType = DbType.Decimal, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Decimal? res_price
        {
            get
            {
                return this._res_price;
            }
            set
            {
                System.Decimal? oldValue = this._res_price;
                bool cancel = false;
                this.OnPropertyChanging("res_price", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_price = value;
                    this.OnPropertyChanged("res_price", value, oldValue);
                }
            }
        }

        #endregion

        #region res_ref_l

        private System.String _res_ref_l;

        /// <summary>
        /// res_ref_l
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_ref_l", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_ref_l
        {
            get
            {
                return this._res_ref_l;
            }
            set
            {
                System.String oldValue = this._res_ref_l;
                bool cancel = false;
                this.OnPropertyChanging("res_ref_l", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_ref_l = value;
                    this.OnPropertyChanged("res_ref_l", value, oldValue);
                }
            }
        }

        #endregion

        #region res_ref_h

        private System.String _res_ref_h;

        /// <summary>
        /// res_ref_h
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_ref_h", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_ref_h
        {
            get
            {
                return this._res_ref_h;
            }
            set
            {
                System.String oldValue = this._res_ref_h;
                bool cancel = false;
                this.OnPropertyChanging("res_ref_h", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_ref_h = value;
                    this.OnPropertyChanged("res_ref_h", value, oldValue);
                }
            }
        }

        #endregion

        #region res_ref_exp

        private System.String _res_ref_exp;

        /// <summary>
        /// res_ref_exp
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_ref_exp", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_ref_exp
        {
            get
            {
                return this._res_ref_exp;
            }
            set
            {
                System.String oldValue = this._res_ref_exp;
                bool cancel = false;
                this.OnPropertyChanging("res_ref_exp", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_ref_exp = value;
                    this.OnPropertyChanged("res_ref_exp", value, oldValue);
                }
            }
        }

        #endregion

        #region res_ref_flag

        private System.String _res_ref_flag;

        /// <summary>
        /// res_ref_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_ref_flag", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_ref_flag
        {
            get
            {
                return this._res_ref_flag;
            }
            set
            {
                System.String oldValue = this._res_ref_flag;
                bool cancel = false;
                this.OnPropertyChanging("res_ref_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_ref_flag = value;
                    this.OnPropertyChanged("res_ref_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region res_meams

        private System.String _res_meams;

        /// <summary>
        /// res_meams
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_meams", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_meams
        {
            get
            {
                return this._res_meams;
            }
            set
            {
                System.String oldValue = this._res_meams;
                bool cancel = false;
                this.OnPropertyChanging("res_meams", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_meams = value;
                    this.OnPropertyChanged("res_meams", value, oldValue);
                }
            }
        }

        #endregion

        #region res_date

        private System.DateTime? _res_date;

        /// <summary>
        /// res_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? res_date
        {
            get
            {
                return this._res_date;
            }
            set
            {
                System.DateTime? oldValue = this._res_date;
                bool cancel = false;
                this.OnPropertyChanging("res_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_date = value;
                    this.OnPropertyChanged("res_date", value, oldValue);
                }
            }
        }

        #endregion

        #region res_flag

        private System.Int32? _res_flag;

        /// <summary>
        /// res_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? res_flag
        {
            get
            {
                return this._res_flag;
            }
            set
            {
                System.Int32? oldValue = this._res_flag;
                bool cancel = false;
                this.OnPropertyChanging("res_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_flag = value;
                    this.OnPropertyChanged("res_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region res_type

        private System.Int32? _res_type;

        /// <summary>
        /// res_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_type", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? res_type
        {
            get
            {
                return this._res_type;
            }
            set
            {
                System.Int32? oldValue = this._res_type;
                bool cancel = false;
                this.OnPropertyChanging("res_type", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_type = value;
                    this.OnPropertyChanged("res_type", value, oldValue);
                }
            }
        }

        #endregion

        #region res_rep_type

        private System.Int32? _res_rep_type;

        /// <summary>
        /// res_rep_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_rep_type", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? res_rep_type
        {
            get
            {
                return this._res_rep_type;
            }
            set
            {
                System.Int32? oldValue = this._res_rep_type;
                bool cancel = false;
                this.OnPropertyChanging("res_rep_type", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_rep_type = value;
                    this.OnPropertyChanged("res_rep_type", value, oldValue);
                }
            }
        }

        #endregion

        #region res_com_id

        private System.String _res_com_id;

        /// <summary>
        /// res_com_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_com_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_com_id
        {
            get
            {
                return this._res_com_id;
            }
            set
            {
                System.String oldValue = this._res_com_id;
                bool cancel = false;
                this.OnPropertyChanging("res_com_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_com_id = value;
                    this.OnPropertyChanged("res_com_id", value, oldValue);
                }
            }
        }

        #endregion

        #region res_itm_rep_ecd

        private System.String _res_itm_rep_ecd;

        /// <summary>
        /// res_itm_rep_ecd
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_itm_rep_ecd", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_itm_rep_ecd
        {
            get
            {
                return this._res_itm_rep_ecd;
            }
            set
            {
                System.String oldValue = this._res_itm_rep_ecd;
                bool cancel = false;
                this.OnPropertyChanging("res_itm_rep_ecd", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_itm_rep_ecd = value;
                    this.OnPropertyChanged("res_itm_rep_ecd", value, oldValue);
                }
            }
        }

        #endregion

        #region res_itr_ori_id

        private System.String _res_itr_ori_id;

        /// <summary>
        /// res_itr_ori_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_itr_ori_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_itr_ori_id
        {
            get
            {
                return this._res_itr_ori_id;
            }
            set
            {
                System.String oldValue = this._res_itr_ori_id;
                bool cancel = false;
                this.OnPropertyChanging("res_itr_ori_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_itr_ori_id = value;
                    this.OnPropertyChanged("res_itr_ori_id", value, oldValue);
                }
            }
        }

        #endregion

        #region res_ref_type

        private System.Int32? _res_ref_type;

        /// <summary>
        /// res_ref_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_ref_type", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? res_ref_type
        {
            get
            {
                return this._res_ref_type;
            }
            set
            {
                System.Int32? oldValue = this._res_ref_type;
                bool cancel = false;
                this.OnPropertyChanging("res_ref_type", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_ref_type = value;
                    this.OnPropertyChanged("res_ref_type", value, oldValue);
                }
            }
        }

        #endregion

        #region res_exp

        private System.String _res_exp;

        /// <summary>
        /// res_exp
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_exp", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_exp
        {
            get
            {
                return this._res_exp;
            }
            set
            {
                System.String oldValue = this._res_exp;
                bool cancel = false;
                this.OnPropertyChanging("res_exp", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_exp = value;
                    this.OnPropertyChanged("res_exp", value, oldValue);
                }
            }
        }

        #endregion

        #region res_chr2

        private System.String _res_chr2;

        /// <summary>
        /// res_chr
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_chr2", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_chr2
        {
            get
            {
                return this._res_chr2;
            }
            set
            {
                System.String oldValue = this._res_chr2;
                bool cancel = false;
                this.OnPropertyChanging("res_chr2", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_chr2 = value;
                    this.OnPropertyChanged("res_chr2", value, oldValue);
                }
            }
        }

        #endregion


        #region res_chr3

        private System.String _res_chr3;

        /// <summary>
        /// res_chr
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_chr3", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_chr3
        {
            get
            {
                return this._res_chr3;
            }
            set
            {
                System.String oldValue = this._res_chr3;
                bool cancel = false;
                this.OnPropertyChanging("res_chr3", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_chr3 = value;
                    this.OnPropertyChanged("res_chr3", value, oldValue);
                }
            }
        }

        #endregion


        #region res_verify

        private System.String _res_verify;

        /// <summary>
        /// res_verify
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_verify", DbType = DbType.String, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_verify
        {
            get
            {
                return this._res_verify;
            }
            set
            {
                System.String oldValue = this._res_verify;
                bool cancel = false;
                this.OnPropertyChanging("res_verify", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_verify = value;
                    this.OnPropertyChanged("res_verify", value, oldValue);
                }
            }
        }

        #endregion

        

        public bool NeedDelete = false;

        public System.String ext_res_ref_flag { get; set; }
    }

    /// <summary>
    /// resulto
    /// </summary>
    [Lib.EntityCore.EntityTableAttribute(TableName = "resulto_newborn", DisplayName = "结果表")]
    public class EntityResulto4AuditForBf : Lib.EntityCore.BaseEntity
    {
        public EntityDictItemSam referenceSam = null;
        public EntityDictItemMi referenceValue = null;
        public EntityDictItem refItem = null;

        #region res_key

        private System.Int64? _res_key;

        /// <summary>
        /// res_key
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_key", DbType = DbType.Int64, IsPrimaryKey = true, IsDBGenerate = false)]
        public System.Int64? res_key
        {
            get
            {
                return this._res_key;
            }
            set
            {
                System.Int64? oldValue = this._res_key;
                bool cancel = false;
                this.OnPropertyChanging("res_key", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_key = value;
                    this.OnPropertyChanged("res_key", value, oldValue);
                }
            }
        }

        #endregion

        #region res_id

        private System.String _res_id;

        /// <summary>
        /// res_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_id
        {
            get
            {
                return this._res_id;
            }
            set
            {
                System.String oldValue = this._res_id;
                bool cancel = false;
                this.OnPropertyChanging("res_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_id = value;
                    this.OnPropertyChanged("res_id", value, oldValue);
                }
            }
        }

        #endregion

        #region res_itr_id

        private System.String _res_itr_id;

        /// <summary>
        /// res_itr_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_itr_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_itr_id
        {
            get
            {
                return this._res_itr_id;
            }
            set
            {
                System.String oldValue = this._res_itr_id;
                bool cancel = false;
                this.OnPropertyChanging("res_itr_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_itr_id = value;
                    this.OnPropertyChanged("res_itr_id", value, oldValue);
                }
            }
        }

        #endregion

        #region res_sid

        private System.String _res_sid;

        /// <summary>
        /// res_sid
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_sid", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_sid
        {
            get
            {
                return this._res_sid;
            }
            set
            {
                System.String oldValue = this._res_sid;
                bool cancel = false;
                this.OnPropertyChanging("res_sid", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_sid = value;
                    this.OnPropertyChanged("res_sid", value, oldValue);
                }
            }
        }

        #endregion

        #region res_itm_id

        private System.String _res_itm_id;

        /// <summary>
        /// res_itm_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_itm_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_itm_id
        {
            get
            {
                return this._res_itm_id;
            }
            set
            {
                System.String oldValue = this._res_itm_id;
                bool cancel = false;
                this.OnPropertyChanging("res_itm_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_itm_id = value;
                    this.OnPropertyChanged("res_itm_id", value, oldValue);
                }
            }
        }

        #endregion

        #region res_itm_ecd

        private System.String _res_itm_ecd;

        /// <summary>
        /// res_itm_ecd
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_itm_ecd", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_itm_ecd
        {
            get
            {
                return this._res_itm_ecd;
            }
            set
            {
                System.String oldValue = this._res_itm_ecd;
                bool cancel = false;
                this.OnPropertyChanging("res_itm_ecd", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_itm_ecd = value;
                    this.OnPropertyChanged("res_itm_ecd", value, oldValue);
                }
            }
        }

        #endregion

        #region res_chr

        private System.String _res_chr;

        /// <summary>
        /// res_chr
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_chr", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_chr
        {
            get
            {
                return this._res_chr;
            }
            set
            {
                System.String oldValue = this._res_chr;
                bool cancel = false;
                this.OnPropertyChanging("res_chr", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_chr = value;
                    this.OnPropertyChanged("res_chr", value, oldValue);
                }
            }
        }

        #endregion

        #region res_od_chr

        private System.String _res_od_chr;

        /// <summary>
        /// res_od_chr
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_od_chr", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_od_chr
        {
            get
            {
                return this._res_od_chr;
            }
            set
            {
                System.String oldValue = this._res_od_chr;
                bool cancel = false;
                this.OnPropertyChanging("res_od_chr", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_od_chr = value;
                    this.OnPropertyChanged("res_od_chr", value, oldValue);
                }
            }
        }

        #endregion

        #region res_cast_chr

        private System.Decimal? _res_cast_chr;

        /// <summary>
        /// res_cast_chr
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_cast_chr", DbType = DbType.Decimal, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Decimal? res_cast_chr
        {
            get
            {
                return this._res_cast_chr;
            }
            set
            {
                System.Decimal? oldValue = this._res_cast_chr;
                bool cancel = false;
                this.OnPropertyChanging("res_cast_chr", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_cast_chr = value;
                    this.OnPropertyChanged("res_cast_chr", value, oldValue);
                }
            }
        }

        #endregion

        #region res_unit

        private System.String _res_unit;

        /// <summary>
        /// res_unit
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_unit", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_unit
        {
            get
            {
                return this._res_unit;
            }
            set
            {
                System.String oldValue = this._res_unit;
                bool cancel = false;
                this.OnPropertyChanging("res_unit", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_unit = value;
                    this.OnPropertyChanged("res_unit", value, oldValue);
                }
            }
        }

        #endregion

        #region res_price

        private System.Decimal? _res_price;

        /// <summary>
        /// res_price
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_price", DbType = DbType.Decimal, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Decimal? res_price
        {
            get
            {
                return this._res_price;
            }
            set
            {
                System.Decimal? oldValue = this._res_price;
                bool cancel = false;
                this.OnPropertyChanging("res_price", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_price = value;
                    this.OnPropertyChanged("res_price", value, oldValue);
                }
            }
        }

        #endregion

        #region res_ref_l

        private System.String _res_ref_l;

        /// <summary>
        /// res_ref_l
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_ref_l", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_ref_l
        {
            get
            {
                return this._res_ref_l;
            }
            set
            {
                System.String oldValue = this._res_ref_l;
                bool cancel = false;
                this.OnPropertyChanging("res_ref_l", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_ref_l = value;
                    this.OnPropertyChanged("res_ref_l", value, oldValue);
                }
            }
        }

        #endregion

        #region res_ref_h

        private System.String _res_ref_h;

        /// <summary>
        /// res_ref_h
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_ref_h", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_ref_h
        {
            get
            {
                return this._res_ref_h;
            }
            set
            {
                System.String oldValue = this._res_ref_h;
                bool cancel = false;
                this.OnPropertyChanging("res_ref_h", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_ref_h = value;
                    this.OnPropertyChanged("res_ref_h", value, oldValue);
                }
            }
        }

        #endregion

        #region res_ref_exp

        private System.String _res_ref_exp;

        /// <summary>
        /// res_ref_exp
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_ref_exp", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_ref_exp
        {
            get
            {
                return this._res_ref_exp;
            }
            set
            {
                System.String oldValue = this._res_ref_exp;
                bool cancel = false;
                this.OnPropertyChanging("res_ref_exp", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_ref_exp = value;
                    this.OnPropertyChanged("res_ref_exp", value, oldValue);
                }
            }
        }

        #endregion

        #region res_ref_flag

        private System.String _res_ref_flag;

        /// <summary>
        /// res_ref_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_ref_flag", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_ref_flag
        {
            get
            {
                return this._res_ref_flag;
            }
            set
            {
                System.String oldValue = this._res_ref_flag;
                bool cancel = false;
                this.OnPropertyChanging("res_ref_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_ref_flag = value;
                    this.OnPropertyChanged("res_ref_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region res_meams

        private System.String _res_meams;

        /// <summary>
        /// res_meams
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_meams", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_meams
        {
            get
            {
                return this._res_meams;
            }
            set
            {
                System.String oldValue = this._res_meams;
                bool cancel = false;
                this.OnPropertyChanging("res_meams", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_meams = value;
                    this.OnPropertyChanged("res_meams", value, oldValue);
                }
            }
        }

        #endregion

        #region res_date

        private System.DateTime? _res_date;

        /// <summary>
        /// res_date
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_date", DbType = DbType.DateTime, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.DateTime? res_date
        {
            get
            {
                return this._res_date;
            }
            set
            {
                System.DateTime? oldValue = this._res_date;
                bool cancel = false;
                this.OnPropertyChanging("res_date", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_date = value;
                    this.OnPropertyChanged("res_date", value, oldValue);
                }
            }
        }

        #endregion

        #region res_flag

        private System.Int32? _res_flag;

        /// <summary>
        /// res_flag
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_flag", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? res_flag
        {
            get
            {
                return this._res_flag;
            }
            set
            {
                System.Int32? oldValue = this._res_flag;
                bool cancel = false;
                this.OnPropertyChanging("res_flag", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_flag = value;
                    this.OnPropertyChanged("res_flag", value, oldValue);
                }
            }
        }

        #endregion

        #region res_type

        private System.Int32? _res_type;

        /// <summary>
        /// res_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_type", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? res_type
        {
            get
            {
                return this._res_type;
            }
            set
            {
                System.Int32? oldValue = this._res_type;
                bool cancel = false;
                this.OnPropertyChanging("res_type", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_type = value;
                    this.OnPropertyChanged("res_type", value, oldValue);
                }
            }
        }

        #endregion

        #region res_rep_type

        private System.Int32? _res_rep_type;

        /// <summary>
        /// res_rep_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_rep_type", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? res_rep_type
        {
            get
            {
                return this._res_rep_type;
            }
            set
            {
                System.Int32? oldValue = this._res_rep_type;
                bool cancel = false;
                this.OnPropertyChanging("res_rep_type", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_rep_type = value;
                    this.OnPropertyChanged("res_rep_type", value, oldValue);
                }
            }
        }

        #endregion

        #region res_com_id

        private System.String _res_com_id;

        /// <summary>
        /// res_com_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_com_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_com_id
        {
            get
            {
                return this._res_com_id;
            }
            set
            {
                System.String oldValue = this._res_com_id;
                bool cancel = false;
                this.OnPropertyChanging("res_com_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_com_id = value;
                    this.OnPropertyChanged("res_com_id", value, oldValue);
                }
            }
        }

        #endregion

        #region res_itm_rep_ecd

        private System.String _res_itm_rep_ecd;

        /// <summary>
        /// res_itm_rep_ecd
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_itm_rep_ecd", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_itm_rep_ecd
        {
            get
            {
                return this._res_itm_rep_ecd;
            }
            set
            {
                System.String oldValue = this._res_itm_rep_ecd;
                bool cancel = false;
                this.OnPropertyChanging("res_itm_rep_ecd", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_itm_rep_ecd = value;
                    this.OnPropertyChanged("res_itm_rep_ecd", value, oldValue);
                }
            }
        }

        #endregion

        #region res_itr_ori_id

        private System.String _res_itr_ori_id;

        /// <summary>
        /// res_itr_ori_id
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_itr_ori_id", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_itr_ori_id
        {
            get
            {
                return this._res_itr_ori_id;
            }
            set
            {
                System.String oldValue = this._res_itr_ori_id;
                bool cancel = false;
                this.OnPropertyChanging("res_itr_ori_id", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_itr_ori_id = value;
                    this.OnPropertyChanged("res_itr_ori_id", value, oldValue);
                }
            }
        }

        #endregion

        #region res_ref_type

        private System.Int32? _res_ref_type;

        /// <summary>
        /// res_ref_type
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_ref_type", DbType = DbType.Int32, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.Int32? res_ref_type
        {
            get
            {
                return this._res_ref_type;
            }
            set
            {
                System.Int32? oldValue = this._res_ref_type;
                bool cancel = false;
                this.OnPropertyChanging("res_ref_type", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_ref_type = value;
                    this.OnPropertyChanged("res_ref_type", value, oldValue);
                }
            }
        }

        #endregion

        #region res_exp

        private System.String _res_exp;

        /// <summary>
        /// res_exp
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_exp", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_exp
        {
            get
            {
                return this._res_exp;
            }
            set
            {
                System.String oldValue = this._res_exp;
                bool cancel = false;
                this.OnPropertyChanging("res_exp", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_exp = value;
                    this.OnPropertyChanged("res_exp", value, oldValue);
                }
            }
        }

        #endregion

        #region res_chr2

        private System.String _res_chr2;

        /// <summary>
        /// res_chr
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_chr2", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_chr2
        {
            get
            {
                return this._res_chr2;
            }
            set
            {
                System.String oldValue = this._res_chr2;
                bool cancel = false;
                this.OnPropertyChanging("res_chr2", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_chr2 = value;
                    this.OnPropertyChanged("res_chr2", value, oldValue);
                }
            }
        }

        #endregion


        #region res_chr3

        private System.String _res_chr3;

        /// <summary>
        /// res_chr
        /// </summary>
        [Lib.EntityCore.FieldMapAttribute(DBColumnName = "res_chr3", DbType = DbType.AnsiString, IsPrimaryKey = false, IsDBGenerate = false)]
        public System.String res_chr3
        {
            get
            {
                return this._res_chr3;
            }
            set
            {
                System.String oldValue = this._res_chr3;
                bool cancel = false;
                this.OnPropertyChanging("res_chr3", value, oldValue, out cancel);
                if (cancel == false)
                {
                    this._res_chr3 = value;
                    this.OnPropertyChanged("res_chr3", value, oldValue);
                }
            }
        }

        #endregion

        public bool NeedDelete = false;

     
    }



}






