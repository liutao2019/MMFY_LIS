using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.ListControls;
using DevExpress.XtraEditors.Repository;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.Data;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using DevExpress.XtraEditors.Registrator;

namespace lis.client.control
{
    /// <summary>
    /// ctlPopupLookUpEditForm
    /// </summary>
    public partial class ctlPopupLookUpEditForm : PopupLookUpEditForm
    {
        public ctlPopupLookUpEditForm(LookUpEdit ownerEdit)
            : base(ownerEdit)
        {

        }

        [Browsable(false)]
        public new ctlLookUpEdit OwnerEdit { get { return base.OwnerEdit as ctlLookUpEdit; } }

        protected override ILookUpDataFilter CreateLookUpFilter()
        {
            return OwnerEdit.Properties.DataAdapter;
        }

        [Category(CategoryName.Behavior)]
        public new ctlLookUpListDataAdapter Filter { get { return OwnerEdit.Properties.DataAdapter; } }
    }

    public class ctlLookUpListDataAdapter : LookUpListDataAdapter
    {
        ctlRepositoryItemLookUpEdit item;
        string filterPrefix = string.Empty;

        public ctlLookUpListDataAdapter(ctlRepositoryItemLookUpEdit item)
            : base(item)
        {
            this.item = item;
            this.filterPrefix = string.Empty;
        }

        public new ctlRepositoryItemLookUpEdit Item
        {
            get
            {
                return item;
            }
        }

        /// <summary>
        /// 生成过滤条件
        /// </summary>
        /// <returns></returns>
        protected override string CreateFilterExpression()
        {
            string strFilter = string.Empty;
            if (string.IsNullOrEmpty(FilterPrefix) == false)
            {
                foreach (DataColumnInfo col in this.Columns)
                {
                    strFilter += " or [" + col.Name + "] like '%" + FilterPrefix.Replace("'", "''") + "%'";
                }
            }
            if (strFilter != string.Empty)
            {
                strFilter = strFilter.Substring(4);
            }

            return strFilter;
        }
    }
}
