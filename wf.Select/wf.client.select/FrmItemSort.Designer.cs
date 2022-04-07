namespace dcl.client.resultquery
{
    partial class FrmItemSort
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmItemSort));
            this.pnlBar = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.txtXHEnd = new DevExpress.XtraEditors.TextEdit();
            this.txtXHStart = new DevExpress.XtraEditors.TextEdit();
            this.lueInstrmt = new dcl.client.control.SelectDicInstrument();
            this.lueDepart = new dcl.client.control.SelectDicPubDept();
            this.lueItem = new dcl.client.control.SelectDicItmItem();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtSwatchEnd = new DevExpress.XtraEditors.TextEdit();
            this.txtSwatchStart = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.deEnd = new DevExpress.XtraEditors.DateEdit();
            this.deStart = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.gcSort = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.pnlBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtXHEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXHStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSwatchEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSwatchStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcSort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlBar
            // 
            this.pnlBar.Controls.Add(this.sysToolBar1);
            this.pnlBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBar.Location = new System.Drawing.Point(0, 0);
            this.pnlBar.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnlBar.Name = "pnlBar";
            this.pnlBar.Size = new System.Drawing.Size(1618, 130);
            this.pnlBar.TabIndex = 0;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1618, 130);
            this.sysToolBar1.TabIndex = 0;
            this.sysToolBar1.OnBtnSearchClicked += new System.EventHandler(this.sysToolBar1_OnBtnSearchClicked);
            this.sysToolBar1.OnBtnPrintClicked += new System.EventHandler(this.sysToolBar1_OnBtnPrintClicked);
            this.sysToolBar1.OnPrintPreviewClicked += new System.EventHandler(this.sysToolBar1_OnPrintPreviewClicked);
            this.sysToolBar1.OnBtnExportClicked += new System.EventHandler(this.sysToolBar1_OnBtnExportClicked);
            this.sysToolBar1.OnCloseClicked += new System.EventHandler(this.sysToolBar1_OnCloseClicked);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.txtXHEnd);
            this.panelControl1.Controls.Add(this.txtXHStart);
            this.panelControl1.Controls.Add(this.lueInstrmt);
            this.panelControl1.Controls.Add(this.lueDepart);
            this.panelControl1.Controls.Add(this.lueItem);
            this.panelControl1.Controls.Add(this.labelControl7);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.txtSwatchEnd);
            this.panelControl1.Controls.Add(this.txtSwatchStart);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.deEnd);
            this.panelControl1.Controls.Add(this.deStart);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 130);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(622, 1105);
            this.panelControl1.TabIndex = 1;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(20, 184);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(96, 29);
            this.labelControl8.TabIndex = 148;
            this.labelControl8.Text = "流水范围";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(357, 184);
            this.labelControl9.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(17, 29);
            this.labelControl9.TabIndex = 149;
            this.labelControl9.Text = "~";
            // 
            // txtXHEnd
            // 
            this.txtXHEnd.EditValue = "";
            this.txtXHEnd.EnterMoveNextControl = true;
            this.txtXHEnd.Location = new System.Drawing.Point(390, 178);
            this.txtXHEnd.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtXHEnd.Name = "txtXHEnd";
            this.txtXHEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.txtXHEnd.Properties.Mask.EditMask = "\\d?\\d?\\d?\\d?\\d?\\d?\\d?\\d?\\d?\\d?\\d?\\d?";
            this.txtXHEnd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtXHEnd.Size = new System.Drawing.Size(215, 36);
            this.txtXHEnd.TabIndex = 147;
            // 
            // txtXHStart
            // 
            this.txtXHStart.EditValue = "";
            this.txtXHStart.EnterMoveNextControl = true;
            this.txtXHStart.Location = new System.Drawing.Point(128, 178);
            this.txtXHStart.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtXHStart.Name = "txtXHStart";
            this.txtXHStart.Properties.Mask.EditMask = "\\d?\\d?\\d?\\d?\\d?\\d?\\d?\\d?\\d?\\d?\\d?\\d?";
            this.txtXHStart.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtXHStart.Size = new System.Drawing.Size(215, 36);
            this.txtXHStart.TabIndex = 146;
            // 
            // lueInstrmt
            // 
            this.lueInstrmt.AddEmptyRow = true;
            this.lueInstrmt.BindByValue = false;
            this.lueInstrmt.colDisplay = "";
            this.lueInstrmt.colExtend1 = null;
            this.lueInstrmt.colInCode = "";
            this.lueInstrmt.colPY = "";
            this.lueInstrmt.colSeq = "";
            this.lueInstrmt.colValue = "";
            this.lueInstrmt.colWB = "";
            this.lueInstrmt.displayMember = null;
            this.lueInstrmt.EnterMoveNext = true;
            this.lueInstrmt.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lueInstrmt.KeyUpDownMoveNext = false;
            this.lueInstrmt.LoadDataOnDesignMode = true;
            this.lueInstrmt.Location = new System.Drawing.Point(128, 261);
            this.lueInstrmt.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lueInstrmt.MaximumSize = new System.Drawing.Size(929, 41);
            this.lueInstrmt.MinimumSize = new System.Drawing.Size(93, 41);
            this.lueInstrmt.Name = "lueInstrmt";
            this.lueInstrmt.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueInstrmt.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lueInstrmt.Readonly = false;
            this.lueInstrmt.SaveSourceID = false;
            this.lueInstrmt.SelectFilter = null;
            this.lueInstrmt.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueInstrmt.SelectOnly = true;
            this.lueInstrmt.ShowAllInstrmt = false;
            this.lueInstrmt.Size = new System.Drawing.Size(477, 41);
            this.lueInstrmt.TabIndex = 4;
            this.lueInstrmt.UseExtend = false;
            this.lueInstrmt.valueMember = null;
            this.lueInstrmt.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.lueInstrmt_ValueChanged);
            // 
            // lueDepart
            // 
            this.lueDepart.AddEmptyRow = true;
            this.lueDepart.BindByValue = false;
            this.lueDepart.colDisplay = "";
            this.lueDepart.colExtend1 = null;
            this.lueDepart.colInCode = "";
            this.lueDepart.colPY = "";
            this.lueDepart.colSeq = "";
            this.lueDepart.colValue = "";
            this.lueDepart.colWB = "";
            this.lueDepart.displayMember = null;
            this.lueDepart.EnterMoveNext = true;
            this.lueDepart.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lueDepart.KeyUpDownMoveNext = false;
            this.lueDepart.LoadDataOnDesignMode = true;
            this.lueDepart.Location = new System.Drawing.Point(128, 416);
            this.lueDepart.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lueDepart.MaximumSize = new System.Drawing.Size(929, 41);
            this.lueDepart.MinimumSize = new System.Drawing.Size(93, 41);
            this.lueDepart.Name = "lueDepart";
            this.lueDepart.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueDepart.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lueDepart.Readonly = false;
            this.lueDepart.SaveSourceID = false;
            this.lueDepart.SelectFilter = null;
            this.lueDepart.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueDepart.SelectOnly = true;
            this.lueDepart.Size = new System.Drawing.Size(477, 41);
            this.lueDepart.TabIndex = 6;
            this.lueDepart.UseExtend = false;
            this.lueDepart.valueMember = null;
            // 
            // lueItem
            // 
            this.lueItem.AddEmptyRow = true;
            this.lueItem.BindByValue = false;
            this.lueItem.colDisplay = "";
            this.lueItem.colExtend1 = null;
            this.lueItem.colInCode = "";
            this.lueItem.colPY = "";
            this.lueItem.colSeq = "ItmSortNo";
            this.lueItem.colValue = "";
            this.lueItem.colWB = "";
            this.lueItem.displayMember = null;
            this.lueItem.EnterMoveNext = true;
            this.lueItem.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lueItem.KeyUpDownMoveNext = false;
            this.lueItem.LoadDataOnDesignMode = true;
            this.lueItem.Location = new System.Drawing.Point(126, 338);
            this.lueItem.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lueItem.MaximumSize = new System.Drawing.Size(929, 41);
            this.lueItem.MinimumSize = new System.Drawing.Size(93, 41);
            this.lueItem.Name = "lueItem";
            this.lueItem.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueItem.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lueItem.Readonly = false;
            this.lueItem.SaveSourceID = false;
            this.lueItem.SelectFilter = null;
            this.lueItem.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueItem.SelectOnly = true;
            this.lueItem.Size = new System.Drawing.Size(479, 41);
            this.lueItem.TabIndex = 5;
            this.lueItem.UseExtend = false;
            this.lueItem.valueMember = null;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(357, 106);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(17, 29);
            this.labelControl7.TabIndex = 10;
            this.labelControl7.Text = "~";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(20, 416);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(96, 29);
            this.labelControl6.TabIndex = 9;
            this.labelControl6.Text = "所属科室";
            // 
            // txtSwatchEnd
            // 
            this.txtSwatchEnd.EnterMoveNextControl = true;
            this.txtSwatchEnd.Location = new System.Drawing.Point(390, 101);
            this.txtSwatchEnd.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtSwatchEnd.Name = "txtSwatchEnd";
            this.txtSwatchEnd.Properties.Mask.EditMask = "n0";
            this.txtSwatchEnd.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSwatchEnd.Size = new System.Drawing.Size(215, 36);
            this.txtSwatchEnd.TabIndex = 3;
            // 
            // txtSwatchStart
            // 
            this.txtSwatchStart.EnterMoveNextControl = true;
            this.txtSwatchStart.Location = new System.Drawing.Point(128, 101);
            this.txtSwatchStart.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtSwatchStart.Name = "txtSwatchStart";
            this.txtSwatchStart.Properties.Mask.EditMask = "n0";
            this.txtSwatchStart.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtSwatchStart.Size = new System.Drawing.Size(215, 36);
            this.txtSwatchStart.TabIndex = 2;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(20, 106);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(96, 29);
            this.labelControl5.TabIndex = 6;
            this.labelControl5.Text = "样本范围";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(20, 338);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(96, 29);
            this.labelControl4.TabIndex = 5;
            this.labelControl4.Text = "测定项目";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(20, 261);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(96, 29);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "仪器代码";
            // 
            // deEnd
            // 
            this.deEnd.EditValue = null;
            this.deEnd.EnterMoveNextControl = true;
            this.deEnd.Location = new System.Drawing.Point(390, 25);
            this.deEnd.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.deEnd.Name = "deEnd";
            this.deEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deEnd.Size = new System.Drawing.Size(215, 36);
            this.deEnd.TabIndex = 1;
            // 
            // deStart
            // 
            this.deStart.EditValue = null;
            this.deStart.EnterMoveNextControl = true;
            this.deStart.Location = new System.Drawing.Point(128, 25);
            this.deStart.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.deStart.Name = "deStart";
            this.deStart.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deStart.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.deStart.Size = new System.Drawing.Size(215, 36);
            this.deStart.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(357, 29);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(17, 29);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "~";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(20, 29);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 29);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "日期范围";
            // 
            // gcSort
            // 
            this.gcSort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcSort.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcSort.Location = new System.Drawing.Point(622, 130);
            this.gcSort.MainView = this.gridView1;
            this.gcSort.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcSort.Name = "gcSort";
            this.gcSort.Size = new System.Drawing.Size(996, 1105);
            this.gcSort.TabIndex = 2;
            this.gcSort.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gcSort;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // FrmItemSort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1618, 1235);
            this.Controls.Add(this.gcSort);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.pnlBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FrmItemSort";
            this.Text = "项目分类查询";
            this.Load += new System.EventHandler(this.FrmItemSort_Load);
            this.pnlBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtXHEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtXHStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSwatchEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSwatchStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deStart.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcSort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlBar;
        private dcl.client.common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl gcSort;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.DateEdit deEnd;
        private DevExpress.XtraEditors.DateEdit deStart;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private dcl.client.control.SelectDicItmItem lueItem;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit txtSwatchEnd;
        private DevExpress.XtraEditors.TextEdit txtSwatchStart;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private dcl.client.control.SelectDicPubDept lueDepart;
        private dcl.client.control.SelectDicInstrument lueInstrmt;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.TextEdit txtXHEnd;
        private DevExpress.XtraEditors.TextEdit txtXHStart;
    }
}