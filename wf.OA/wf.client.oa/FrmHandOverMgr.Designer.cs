namespace dcl.client.oa
{
    partial class FrmHandOverMgr
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHandOverMgr));
            this.panel2 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.gridControlItr = new DevExpress.XtraGrid.GridControl();
            this.bsHo = new System.Windows.Forms.BindingSource(this.components);
            this.gvItr = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colitr_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitr_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelLayout = new System.Windows.Forms.Panel();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtho_timeInter = new DevExpress.XtraEditors.ButtonEdit();
            this.time3 = new DevExpress.XtraEditors.TimeEdit();
            this.time2 = new DevExpress.XtraEditors.TimeEdit();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.time1 = new DevExpress.XtraEditors.TimeEdit();
            this.txtType = new dcl.client.control.SelectDicLabProfession();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlItr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsHo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtho_timeInter.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.time3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.time2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.time1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.sysToolBar1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1203, 81);
            this.panel2.TabIndex = 1;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1203, 81);
            this.sysToolBar1.TabIndex = 1;
            this.sysToolBar1.OnBtnAddClicked += new System.EventHandler(this.sysToolBar1_OnBtnAddClicked);
            this.sysToolBar1.OnBtnModifyClicked += new System.EventHandler(this.sysToolBar1_OnBtnModifyClicked);
            this.sysToolBar1.OnBtnDeleteClicked += new System.EventHandler(this.sysToolBar1_OnBtnDeleteClicked);
            this.sysToolBar1.OnBtnSaveClicked += new System.EventHandler(this.sysToolBar1_OnBtnSaveClicked);
            this.sysToolBar1.OnBtnCancelClicked += new System.EventHandler(this.sysToolBar1_OnBtnCancelClicked);
            this.sysToolBar1.OnBtnRefreshClicked += new System.EventHandler(this.sysToolBar1_OnBtnRefreshClicked);
            // 
            // gridControlItr
            // 
            this.gridControlItr.DataSource = this.bsHo;
            this.gridControlItr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlItr.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControlItr.Location = new System.Drawing.Point(0, 0);
            this.gridControlItr.MainView = this.gvItr;
            this.gridControlItr.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControlItr.Name = "gridControlItr";
            this.gridControlItr.Size = new System.Drawing.Size(357, 756);
            this.gridControlItr.TabIndex = 48;
            this.gridControlItr.TabStop = false;
            this.gridControlItr.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvItr});
            // 
            // bsHo
            // 
            this.bsHo.DataSource = typeof(dcl.entity.EntityDicHandOver);
            // 
            // gvItr
            // 
            this.gvItr.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colitr_id,
            this.colitr_name});
            this.gvItr.GridControl = this.gridControlItr;
            this.gvItr.Name = "gvItr";
            this.gvItr.OptionsBehavior.Editable = false;
            this.gvItr.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvItr.OptionsView.ColumnAutoWidth = false;
            this.gvItr.OptionsView.ShowGroupPanel = false;
            this.gvItr.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvItr_FocusedRowChanged);
            // 
            // colitr_id
            // 
            this.colitr_id.Caption = "实验组编码";
            this.colitr_id.FieldName = "HoTypeId";
            this.colitr_id.Name = "colitr_id";
            this.colitr_id.Visible = true;
            this.colitr_id.VisibleIndex = 0;
            this.colitr_id.Width = 91;
            // 
            // colitr_name
            // 
            this.colitr_name.Caption = "实验组名称";
            this.colitr_name.FieldName = "TypeName";
            this.colitr_name.Name = "colitr_name";
            this.colitr_name.Visible = true;
            this.colitr_name.VisibleIndex = 1;
            this.colitr_name.Width = 105;
            // 
            // panelLayout
            // 
            this.panelLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLayout.Location = new System.Drawing.Point(0, 161);
            this.panelLayout.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelLayout.Name = "panelLayout";
            this.panelLayout.Size = new System.Drawing.Size(836, 595);
            this.panelLayout.TabIndex = 1;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(477, 134);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(195, 18);
            this.labelControl2.TabIndex = 55;
            this.labelControl2.Text = "分钟退出系统则弹出交班界面";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(341, 134);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 54;
            this.labelControl1.Text = "班次前后";
            // 
            // txtho_timeInter
            // 
            this.txtho_timeInter.EditValue = "10";
            this.txtho_timeInter.EnterMoveNextControl = true;
            this.txtho_timeInter.Location = new System.Drawing.Point(407, 131);
            this.txtho_timeInter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtho_timeInter.Name = "txtho_timeInter";
            this.txtho_timeInter.Size = new System.Drawing.Size(64, 24);
            this.txtho_timeInter.TabIndex = 53;
            // 
            // time3
            // 
            this.time3.EditValue = new System.DateTime(2013, 5, 22, 0, 0, 0, 0);
            this.time3.Location = new System.Drawing.Point(86, 131);
            this.time3.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.time3.Name = "time3";
            this.time3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.time3.Size = new System.Drawing.Size(197, 24);
            this.time3.TabIndex = 52;
            // 
            // time2
            // 
            this.time2.EditValue = new System.DateTime(2013, 5, 22, 0, 0, 0, 0);
            this.time2.Location = new System.Drawing.Point(86, 100);
            this.time2.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.time2.Name = "time2";
            this.time2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.time2.Size = new System.Drawing.Size(197, 24);
            this.time2.TabIndex = 51;
            // 
            // labelControl16
            // 
            this.labelControl16.Location = new System.Drawing.Point(19, 72);
            this.labelControl16.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(60, 18);
            this.labelControl16.TabIndex = 50;
            this.labelControl16.Text = "交班班次";
            // 
            // time1
            // 
            this.time1.EditValue = new System.DateTime(2013, 5, 22, 0, 0, 0, 0);
            this.time1.Location = new System.Drawing.Point(86, 69);
            this.time1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.time1.Name = "time1";
            this.time1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.time1.Size = new System.Drawing.Size(197, 24);
            this.time1.TabIndex = 49;
            // 
            // txtType
            // 
            this.txtType.AddEmptyRow = true;
            this.txtType.AutoScroll = true;
            this.txtType.BindByValue = false;
            this.txtType.colDisplay = "";
            this.txtType.colExtend1 = null;
            this.txtType.colInCode = "";
            this.txtType.colPY = "";
            this.txtType.colSeq = "";
            this.txtType.colValue = "";
            this.txtType.colWB = "";
            this.txtType.displayMember = null;
            this.txtType.EnterMoveNext = true;
            this.txtType.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtType.KeyUpDownMoveNext = false;
            this.txtType.LoadDataOnDesignMode = true;
            this.txtType.Location = new System.Drawing.Point(86, 38);
            this.txtType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtType.MaximumSize = new System.Drawing.Size(571, 27);
            this.txtType.MinimumSize = new System.Drawing.Size(57, 27);
            this.txtType.Name = "txtType";
            this.txtType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtType.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtType.Readonly = false;
            this.txtType.SaveSourceID = false;
            this.txtType.SelectFilter = null;
            this.txtType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtType.SelectOnly = true;
            this.txtType.Size = new System.Drawing.Size(197, 27);
            this.txtType.TabIndex = 5;
            this.txtType.UseExtend = false;
            this.txtType.valueMember = null;
            this.txtType.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.ValueChangedEventHandler(this.txtType_ValueChanged);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.splitContainerControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 81);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1203, 760);
            this.panelControl1.TabIndex = 2;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControlItr);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.panelLayout);
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1199, 756);
            this.splitContainerControl1.SplitterPosition = 357;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.txtho_timeInter);
            this.groupControl1.Controls.Add(this.txtType);
            this.groupControl1.Controls.Add(this.labelControl16);
            this.groupControl1.Controls.Add(this.time3);
            this.groupControl1.Controls.Add(this.time1);
            this.groupControl1.Controls.Add(this.time2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(836, 161);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "交班设定";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(34, 40);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(45, 18);
            this.labelControl3.TabIndex = 53;
            this.labelControl3.Text = "实验组";
            // 
            // FrmHandOverMgr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 841);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panel2);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmHandOverMgr";
            this.Text = "交班设定";
            this.Load += new System.EventHandler(this.FrmHandOverMgr_Load);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlItr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsHo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtho_timeInter.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.time3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.time2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.time1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.Panel panelLayout;
        private DevExpress.XtraGrid.GridControl gridControlItr;
        private DevExpress.XtraGrid.Views.Grid.GridView gvItr;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_id;
        private DevExpress.XtraGrid.Columns.GridColumn colitr_name;
        private System.Windows.Forms.BindingSource bsHo;
        private dcl.client.control.SelectDicLabProfession txtType;
        private DevExpress.XtraEditors.TimeEdit time3;
        private DevExpress.XtraEditors.TimeEdit time2;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private DevExpress.XtraEditors.TimeEdit time1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ButtonEdit txtho_timeInter;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}