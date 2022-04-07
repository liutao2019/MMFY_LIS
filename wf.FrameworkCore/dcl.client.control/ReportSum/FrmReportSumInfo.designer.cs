namespace dcl.client.control
{
    partial class FrmReportSumInfo
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
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.txtSumInfo = new DevExpress.XtraEditors.MemoEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnComment = new DevExpress.XtraEditors.SimpleButton();
            this.btnLisKnowleage = new DevExpress.XtraEditors.SimpleButton();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gcComment = new DevExpress.XtraGrid.GridControl();
            this.gvComment = new DevExpress.XtraGrid.Views.Card.CardView();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.txtCommentInfo = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSumInfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcComment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvComment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCommentInfo.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.txtSumInfo);
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(525, 542);
            this.splitContainerControl1.SplitterPosition = 251;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // txtSumInfo
            // 
            this.txtSumInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSumInfo.Location = new System.Drawing.Point(0, 0);
            this.txtSumInfo.Name = "txtSumInfo";
            this.txtSumInfo.Properties.ReadOnly = true;
            this.txtSumInfo.Properties.UseReadOnlyAppearance = false;
            this.txtSumInfo.Size = new System.Drawing.Size(525, 219);
            this.txtSumInfo.TabIndex = 1;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnComment);
            this.panelControl1.Controls.Add(this.btnLisKnowleage);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 219);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(525, 32);
            this.panelControl1.TabIndex = 2;
            // 
            // btnComment
            // 
            this.btnComment.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnComment.Appearance.Options.UseBackColor = true;
            this.btnComment.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnComment.Location = new System.Drawing.Point(197, 4);
            this.btnComment.Name = "btnComment";
            this.btnComment.Size = new System.Drawing.Size(95, 23);
            this.btnComment.TabIndex = 1;
            this.btnComment.Text = "评   价";
            this.btnComment.Click += new System.EventHandler(this.btnComment_Click);
            // 
            // btnLisKnowleage
            // 
            this.btnLisKnowleage.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnLisKnowleage.Appearance.Options.UseBackColor = true;
            this.btnLisKnowleage.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnLisKnowleage.Location = new System.Drawing.Point(5, 4);
            this.btnLisKnowleage.Name = "btnLisKnowleage";
            this.btnLisKnowleage.Size = new System.Drawing.Size(95, 23);
            this.btnLisKnowleage.TabIndex = 0;
            this.btnLisKnowleage.Text = "检验项目知识库";
            this.btnLisKnowleage.Click += new System.EventHandler(this.btnLisKnowleage_Click);
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Horizontal = false;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.gcComment);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(525, 286);
            this.splitContainerControl2.SplitterPosition = 178;
            this.splitContainerControl2.TabIndex = 1;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // gcComment
            // 
            this.gcComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcComment.Location = new System.Drawing.Point(0, 0);
            this.gcComment.MainView = this.gvComment;
            this.gcComment.Name = "gcComment";
            this.gcComment.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
            this.gcComment.Size = new System.Drawing.Size(525, 178);
            this.gcComment.TabIndex = 1;
            this.gcComment.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvComment});
            // 
            // gvComment
            // 
            this.gvComment.Appearance.CardCaption.Options.UseTextOptions = true;
            this.gvComment.Appearance.CardCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.gvComment.CardCaptionFormat = "     {2}     {3}  ";
            this.gvComment.CardWidth = 450;
            this.gvComment.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn5,
            this.gridColumn8,
            this.gridColumn9});
            this.gvComment.FocusedCardTopFieldIndex = 0;
            this.gvComment.GridControl = this.gcComment;
            this.gvComment.MaximumCardColumns = 1;
            this.gvComment.MaximumCardRows = 6;
            this.gvComment.Name = "gvComment";
            this.gvComment.OptionsBehavior.AutoHorzWidth = true;
            this.gvComment.OptionsView.ShowCardExpandButton = false;
            this.gvComment.OptionsView.ShowFieldCaptions = false;
            this.gvComment.OptionsView.ShowQuickCustomizeButton = false;
            this.gvComment.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Auto;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "内容";
            this.gridColumn5.ColumnEdit = this.repositoryItemMemoEdit1;
            this.gridColumn5.FieldName = "RcComment";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.OptionsColumn.ReadOnly = true;
            this.gridColumn5.ToolTip = "双击查看明细";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "项目名称";
            this.gridColumn8.FieldName = "RcOpname";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowFocus = false;
            this.gridColumn8.OptionsColumn.ReadOnly = true;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "gridColumn9";
            this.gridColumn9.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.gridColumn9.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn9.FieldName = "RcDate";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsColumn.AllowFocus = false;
            this.gridColumn9.OptionsColumn.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.txtCommentInfo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(525, 103);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "评价";
            // 
            // btnSave
            // 
            this.btnSave.Appearance.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnSave.Appearance.Options.UseBackColor = true;
            this.btnSave.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnSave.Location = new System.Drawing.Point(185, 77);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(95, 23);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "保   存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtCommentInfo
            // 
            this.txtCommentInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommentInfo.Location = new System.Drawing.Point(12, 21);
            this.txtCommentInfo.Name = "txtCommentInfo";
            this.txtCommentInfo.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCommentInfo.Size = new System.Drawing.Size(501, 53);
            this.txtCommentInfo.TabIndex = 1;
            // 
            // FrmReportSumInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 542);
            this.Controls.Add(this.splitContainerControl1);
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmReportSumInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "解读内容";
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtSumInfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcComment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvComment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtCommentInfo.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.MemoEdit txtSumInfo;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnComment;
        private DevExpress.XtraEditors.SimpleButton btnLisKnowleage;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraGrid.GridControl gcComment;
        private DevExpress.XtraGrid.Views.Card.CardView gvComment;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.MemoEdit txtCommentInfo;
    }
}