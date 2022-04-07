using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using dcl.entity;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils.Win;
using DevExpress.XtraEditors.Popup;
using System.Windows.Forms;
using DevExpress.XtraGrid.Editors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;

namespace dcl.client.common
{
    ///<summary>
    ///hb add
    ///</summary>
    public class LookupEditSetter
    {
        #region LookUpEdit ApplyBinding下拉框绑定核心方法
        /// <summary>
        /// 对下拉控件进行自动绑定核心方法
        /// </summary>
        /// <param name="aLookUpEdit">绑定的控件</param>
        /// <param name="aBindingSource">绑定数据源</param>
        /// <param name="aAllowNull">绑定的字段是否允许置空值(NULL)</param>
        /// <param name="aValueMember">内部对于值字段</param>
        /// <param name="aDisplayMember">显示字段</param>
        public static void ApplyBinding<T>(LookUpEdit aLookUpEdit, IEnumerable<T> aBindingSource,
                                 bool aAllowNull, string aValueMember, string aDisplayMember,string DisplayField=null, string CaptionField = null)
        {
            ApplyBinding(aLookUpEdit.Properties, aBindingSource, aAllowNull, aValueMember, aDisplayMember, DisplayField, CaptionField);
        }

        /// <summary>
        /// 对下拉控件进行自动绑定核心方法
        /// </summary>
        /// <param name="aLookUpEdit">绑定的控件</param>
        /// <param name="aBindingSource">绑定数据源</param>
        /// <param name="aAllowNull">绑定的字段是否允许置空值(NULL)</param>
        /// <param name="aValueMember">内部对于值字段</param>
        /// <param name="aDisplayMember">显示字段</param>
        public static void ApplyBinding<T>(RepositoryItemLookUpEdit aLookUpEdit, IEnumerable<T> aBindingSource,
                                 bool aAllowNull, string aValueMember, string aDisplayMember
            , string DisplayField = null, string CaptionField = null)
        {
            aLookUpEdit.Columns.Clear();
            if (string.IsNullOrEmpty(DisplayField))
            {
                aLookUpEdit.Columns.Add(new LookUpColumnInfo(aValueMember));
                aLookUpEdit.Columns[0].Visible = false;
                aLookUpEdit.Columns.Add(new LookUpColumnInfo(aDisplayMember));
                aLookUpEdit.Columns[1].Visible = true;
            }
            else
            {
                string[] Fields = DisplayField.Split(',');
                for (int i = 0; i < Fields.Length; i++)
                {
                    if (!string.IsNullOrEmpty(DisplayField))
                        aLookUpEdit.Columns.Add(new LookUpColumnInfo(Fields[i], CaptionField.Split(',')[i]));
                    else
                        aLookUpEdit.Columns.Add(new LookUpColumnInfo(Fields[i]));
                }
            }

            aLookUpEdit.DataSource = aBindingSource;
            aLookUpEdit.ValueMember = aValueMember;
            aLookUpEdit.DisplayMember = aDisplayMember;
            aLookUpEdit.ShowHeader = true;
            aLookUpEdit.BestFitMode = BestFitMode.BestFitResizePopup;
            aLookUpEdit.NullText = string.Empty;

            Debug.Assert(aBindingSource != null, "aBindingSource != null");
            var c = aBindingSource.Count();
            if (c > 0 && c < 15)
            {
                aLookUpEdit.DropDownRows = c;
            }
            else if (c >= 15)
            {
                aLookUpEdit.DropDownRows = 15;
            }

            aLookUpEdit.ShowFooter = aAllowNull;
            aLookUpEdit.AllowNullInput = aAllowNull ? DefaultBoolean.True : DefaultBoolean.False;
            if (aAllowNull)
            {
                aLookUpEdit.Closed += LookUpEditClosed;
            }
        }


        private static void LookUpEditClosed(object aSender, ClosedEventArgs aE)
        {
            if (aE.CloseMode == PopupCloseMode.Cancel)
            {
                var editor = aSender as BaseEdit;
                if (editor != null)
                {
                    editor.EditValue = null;
                }
            }
        }
        #endregion


        #region SearchLookUpEdit  ApplyBinding下拉框绑定核心方法
        /// <summary>
        /// 对下拉控件进行自动绑定核心方法
        /// </summary>
        /// <param name="aLookUpEdit">绑定的控件</param>
        /// <param name="aBindingSource">绑定数据源</param>
        /// <param name="aAllowNull">绑定的字段是否允许置空值(NULL)</param>
        /// <param name="aValueMember">内部对于值字段</param>
        /// <param name="aDisplayMember">显示字段</param>
        public static void ApplyBinding<T>(SearchLookUpEdit aLookUpEdit, IEnumerable<T> aBindingSource,
                                 bool aAllowNull, string aValueMember, string aDisplayMember, string DisplayField = null, string CaptionField = null)
        {
            ApplyBinding(aLookUpEdit.Properties, aBindingSource, aAllowNull, aValueMember, aDisplayMember, DisplayField, CaptionField);
        }

        /// <summary>
        /// 对下拉控件进行自动绑定核心方法
        /// </summary>
        /// <param name="aLookUpEdit">绑定的控件</param>
        /// <param name="aBindingSource">绑定数据源</param>
        /// <param name="aAllowNull">绑定的字段是否允许置空值(NULL)</param>
        /// <param name="aValueMember">内部对于值字段</param>
        /// <param name="aDisplayMember">显示字段</param>
        public static void ApplyBinding<T>(RepositoryItemSearchLookUpEdit aLookUpEdit, IEnumerable<T> aBindingSource,
                                 bool aAllowNull, string aValueMember, string aDisplayMember
            , string DisplayField = null, string CaptionField = null)
        {
            aLookUpEdit.View.Columns.Clear();
            if (!string.IsNullOrEmpty(DisplayField)&& !string.IsNullOrEmpty(CaptionField))
            {
               
                string[] Fields = DisplayField.Split(',');
                string[] Captions = CaptionField.Split(',');
                for (int i = 0; i < Fields.Length; i++)
                {
                    GridColumn gc = new GridColumn();
                    gc.Name = Fields[i] + i;
                    gc.FieldName = Fields[i];
                    gc.Caption = Captions[i];
                    gc.VisibleIndex = i;
                    aLookUpEdit.View.Columns.Add(gc);
                }
            }

            aLookUpEdit.DataSource = aBindingSource;
            aLookUpEdit.ValueMember = aValueMember;
            aLookUpEdit.DisplayMember = aDisplayMember;
            aLookUpEdit.BestFitMode = BestFitMode.BestFitResizePopup;
            aLookUpEdit.NullText = string.Empty;

            Debug.Assert(aBindingSource != null, "aBindingSource != null");
            var c = aBindingSource.Count();

            aLookUpEdit.View.OptionsView.ColumnAutoWidth = false;
            aLookUpEdit.PopupFormSize = new System.Drawing.Size(100, 300);
            aLookUpEdit.ShowFooter = false;
            aLookUpEdit.AllowNullInput = aAllowNull ? DefaultBoolean.True : DefaultBoolean.False;
            if (aAllowNull)
            {
                aLookUpEdit.Closed += LookUpEditClosed;
            }
            aLookUpEdit.Popup += searchLookUpEdit1_Popup;
        }

        private static void searchLookUpEdit1_Popup(object sender, EventArgs e)
        {
            IPopupControl popup = sender as IPopupControl;
            PopupBaseForm Form = popup.PopupWindow as PopupBaseForm;
            LayoutControlItem btFindLCI = GetFindButtonLayoutItem(Form);
            btFindLCI.Visibility =  LayoutVisibility.Never;
        }

        static LayoutControlItem GetFindButtonLayoutItem(PopupBaseForm Form)
        {
            foreach (Control FormC in Form.Controls)
            {
                if (FormC is SearchEditLookUpPopup)
                {
                    SearchEditLookUpPopup SearchPopup = FormC as SearchEditLookUpPopup;
                    foreach (Control SearchPopupC in SearchPopup.Controls)
                    {
                        if (SearchPopupC is LayoutControl)
                        {
                            LayoutControl FormLayout = SearchPopupC as LayoutControl;
                            Control Button = FormLayout.GetControlByName("btFind");
                            if (Button != null)
                            {
                                return FormLayout.GetItemByControl(Button);
                            }

                        }
                    }
                }
            }
            return null;
        }


        #endregion

        #region ValueDisplayObject通用下拉框
        /// <summary>
        /// 对下拉控件进行自动绑定
        /// </summary>
        /// <param name="aLookUpEdit">绑定的控件</param>
        /// <param name="aBindingSource">绑定数据源</param>
        /// <param name="aAllowNull">绑定的字段是否允许置空值(NULL)</param>
        public static void Setup(LookUpEdit aLookUpEdit, IEnumerable<ValueDisplayObject> aBindingSource, bool aAllowNull)
        {
            Setup(aLookUpEdit.Properties, aBindingSource, aAllowNull);
        }

        /// <summary>
        /// 对下拉控件进行自动绑定
        /// </summary>
        /// <param name="aLookUpEdit">绑定的控件</param>
        /// <param name="aBindingSource">绑定数据源</param>
        /// <param name="aAllowNull">绑定的字段是否允许置空值(NULL)</param>
        public static void Setup(RepositoryItemLookUpEdit aLookUpEdit, IEnumerable<ValueDisplayObject> aBindingSource,
                                 bool aAllowNull = true)
        {
            ApplyBinding(aLookUpEdit, aBindingSource, aAllowNull, "Value", "Display");
        }
        #endregion

        
    }

}