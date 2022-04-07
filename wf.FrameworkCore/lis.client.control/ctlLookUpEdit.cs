using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Controls;

namespace lis.client.control
{
    public partial class ctlLookUpEdit : LookUpEdit
    {
        //private Font thisFont = new Font("宋体", 10.5f);

        public new bool DesignMode
        {
            get { return System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() == "devenv"; }
        }

        #region InitializeComponent
        public ctlLookUpEdit()
        {
            InitializeComponent();

            #region 属性
            this.Properties.NullText = "";
            this.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.AutoFilter;
            if (DesignMode)
            {
                //使设计时也能看到下拉按钮
                this.Properties.Buttons.Insert(0, new EditorButton(ButtonPredefines.Combo, ""));
            }

            this.AddDeleteButton();
            #endregion

            #region 字体
            ////this.Font = thisFont;
            //this.Properties.AppearanceDropDown.Options.UseFont = true;
            ////this.Properties.AppearanceDropDown.Font = thisFont;
            //this.Properties.Appearance.Options.UseFont = true;
            ////this.Properties.Appearance.Font = thisFont;
            //this.Properties.AppearanceDisabled.Options.UseFont = true;
            ////this.Properties.AppearanceDisabled.Font = thisFont;
            //this.Properties.AppearanceDropDownHeader.Options.UseFont = true;
            ////this.Properties.AppearanceDropDownHeader.Font = thisFont;
            //this.Properties.AppearanceFocused.Options.UseFont = true;
            ////this.Properties.AppearanceFocused.Font = thisFont;
            //this.Properties.AppearanceReadOnly.Options.UseFont = true;
            ////this.Properties.AppearanceReadOnly.Font = thisFont;
            #endregion

            this.Properties.ActionButtonIndex = 1;
        }


        private void AddDeleteButton()
        {
            bool hasDeleteButton = false;
            foreach (EditorButton btn in this.Properties.Buttons)
            {
                if (btn.Kind == ButtonPredefines.Delete)
                {
                    hasDeleteButton = true;
                }
            }
            if (hasDeleteButton == false)
            {
                this.Properties.Buttons.Insert(0, new EditorButton(ButtonPredefines.Delete, ""));
                this.ButtonClick += new ButtonPressedEventHandler(ctlLookUpEdit_ButtonClick);
            }
        }

        void ctlLookUpEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                this.EditValue = null;
            }
        }
        #endregion

        #region 符合RepositoryItem使用要求的属性
        public override string EditorTypeName { get { return "ctlLookUpEdit"; } }
        public static EditorClassInfo info = new EditorClassInfo("ctlLookUpEdit", typeof(ctlLookUpEdit), typeof(ctlRepositoryItemLookUpEdit), typeof(DevExpress.XtraEditors.ViewInfo.PopupContainerEditViewInfo), new ButtonEditPainter(), true, null, typeof(DevExpress.Accessibility.PopupEditAccessible));
        protected override EditorClassInfo EditorClassInfo
        {
            get
            {
                if (EditorRegistrationInfo.Default.Editors.IndexOf(info) == -1)
                {
                    EditorRegistrationInfo.Default.Editors.Add(info);
                }
                return info;
            }
        }
        public new ctlRepositoryItemLookUpEdit Properties
        {
            get
            {
                return base.Properties as ctlRepositoryItemLookUpEdit;
            }
        }
        protected override void DoShowPopup()
        {
            if (Properties.Columns.Count == 0) Properties.PopulateColumns();
            base.DoShowPopup();
        }
        protected override DevExpress.XtraEditors.Popup.PopupBaseForm CreatePopupForm()
        {
            return new ctlPopupLookUpEditForm(this);
        }

        protected internal new ctlPopupLookUpEditForm PopupForm { get { return base.PopupForm as ctlPopupLookUpEditForm; } }
        #endregion

        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (Properties.ReadOnly) return;
                if (value == null) value = string.Empty;
                ProcessText(new KeyPressHelper(value, value.Length, 0, Properties.MaxLength), true);
            }
        }

        public override object EditValue
        {
            get
            {
                return base.EditValue;
            }
            set
            {
                base.EditValue = value;
                string displayText = Properties.GetDisplayText(value);
                MaskBox.SetEditValue(value, displayText, true);
                MaskBox.MaskBoxSelectionStart = MaskBox.MaskBoxText.Length;
                MaskBox.MaskBoxSelectionLength = 0;
            }
        }

        protected  void ProcessText(DevExpress.XtraEditors.Controls.KeyPressHelper helper, bool canImmediatePopup)
        {
            bool prevOpen = IsPopupOpen;
            if ((canImmediatePopup && Properties.ImmediatePopup) || Properties.SearchMode == SearchMode.AutoFilter)
            {
                ShowPopup();
                if (helper.Text == string.Empty)
                {
                    MaskBox.SetEditValue(null, helper.Text, true);
                }
                else
                {
                    MaskBox.SetEditValue(EditValue, helper.Text, true);
                }
                MaskBox.MaskBoxSelectionStart = helper.SelectionStart;
                MaskBox.MaskBoxSelectionLength = helper.SelectionLength;

                if (PopupForm != null)
                {
                    if (Properties.SearchMode == SearchMode.AutoFilter)
                    {
                        if (IsDisplayTextValid)
                        {
                            PopupForm.Filter.FilterPrefix = AutoSearchText;
                        }
                        else
                        {
                            PopupForm.Filter.FilterPrefix = (IsMaskBoxAvailable ? MaskBox.MaskBoxText : AutoSearchText);
                        }

                        if (PopupForm.Filter.VisibleCount > 0)
                        {
                            //默认第一行获得焦点
                            PopupForm.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        AutoSearchText = string.Empty;
                    }
                    if (AutoSearchText == string.Empty) PopupForm.SelectedIndex = Properties.DataAdapter.FindValueIndex(Properties.ValueMember, EditValue);
                    if (!prevOpen && Properties.SearchMode == SearchMode.OnlyInPopup)
                    {
                        PopupForm.ProcessKeyPress(new KeyPressEventArgs(helper.CharValue));
                    }
                }
            }
        }
    }

    /// <summary>
    /// ctlRepositoryItemLookUpEdit
    /// </summary>
    public class ctlRepositoryItemLookUpEdit : RepositoryItemLookUpEdit
    {
        //private Font thisFont = new Font("宋体", 10.5f);

        public ctlRepositoryItemLookUpEdit()
            : base()
        {
            #region 字体
            this.NullText = "";
            //this.AppearanceDropDown.Options.UseFont = true;
            ////this.AppearanceDropDown.Font = thisFont;
            //this.Appearance.Options.UseFont = true;
            ////this.Appearance.Font = thisFont;
            //this.AppearanceDisabled.Options.UseFont = true;
            ////this.AppearanceDisabled.Font = thisFont;
            //this.AppearanceDropDownHeader.Options.UseFont = true;
            ////this.AppearanceDropDownHeader.Font = thisFont;
            //this.AppearanceFocused.Options.UseFont = true;
            ////this.AppearanceFocused.Font = thisFont;
            //this.AppearanceReadOnly.Options.UseFont = true;
            //this.AppearanceReadOnly.Font = thisFont;
            #endregion

            #region 删除按钮
            this.AddDeleteButton();
            #endregion

            this.ActionButtonIndex = 1;
        }

        protected override DevExpress.XtraEditors.ListControls.LookUpListDataAdapter CreateDataAdapter()
        {
            return new ctlLookUpListDataAdapter(this);
        }

        public new ctlLookUpListDataAdapter DataAdapter
        {
            get
            {
                return base.DataAdapter as ctlLookUpListDataAdapter;
            }
        }

        public override string EditorTypeName
        {
            get
            {
                if (EditorRegistrationInfo.Default.Editors.IndexOf(ctlLookUpEdit.info) == -1)
                {
                    EditorRegistrationInfo.Default.Editors.Add(ctlLookUpEdit.info);
                }
                return "ctlLookUpEdit";
            }
        }


        private void AddDeleteButton()
        {
            bool hasDeleteButton = false;
            foreach (EditorButton btn in this.Buttons)
            {
                if (btn.Kind == ButtonPredefines.Delete)
                {
                    hasDeleteButton = true;
                }
            }
            if (hasDeleteButton == false)
            {
                this.Buttons.Insert(0,(new EditorButton(ButtonPredefines.Delete, "")));
                this.ButtonClick += new ButtonPressedEventHandler(ctlLookUpEdit_ButtonClick);
            }
        }

        void ctlLookUpEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Delete)
            {
                ctlLookUpEdit lookUpEdit = sender as ctlLookUpEdit;
                lookUpEdit.EditValue = null;
            }
        }
    }
}
