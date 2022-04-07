namespace dcl.client.oa
{
    partial class FrmHandOverNullRes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHandOverNullRes));
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbtCom = new System.Windows.Forms.RadioButton();
            this.rbtItm = new System.Windows.Forms.RadioButton();
            this.btnclose = new System.Windows.Forms.Button();
            this.btnSure = new System.Windows.Forms.Button();
            this.bsNullResult = new System.Windows.Forms.BindingSource();
            this.rtxtResChr = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemComboBox2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gcNullResult = new DevExpress.XtraGrid.GridControl();
            this.gvNullResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox3 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox4 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsNullResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtResChr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcNullResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvNullResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbtCom);
            this.panel1.Controls.Add(this.rbtItm);
            this.panel1.Controls.Add(this.btnclose);
            this.panel1.Controls.Add(this.btnSure);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 986);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1657, 112);
            this.panel1.TabIndex = 0;
            // 
            // rbtCom
            // 
            this.rbtCom.AutoSize = true;
            this.rbtCom.Location = new System.Drawing.Point(303, 27);
            this.rbtCom.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.rbtCom.Name = "rbtCom";
            this.rbtCom.Size = new System.Drawing.Size(92, 33);
            this.rbtCom.TabIndex = 3;
            this.rbtCom.Tag = "1";
            this.rbtCom.Text = "组合";
            this.rbtCom.UseVisualStyleBackColor = true;
            this.rbtCom.CheckedChanged += new System.EventHandler(this.rbtCom_CheckedChanged);
            // 
            // rbtItm
            // 
            this.rbtItm.AutoSize = true;
            this.rbtItm.Checked = true;
            this.rbtItm.Location = new System.Drawing.Point(184, 27);
            this.rbtItm.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.rbtItm.Name = "rbtItm";
            this.rbtItm.Size = new System.Drawing.Size(92, 33);
            this.rbtItm.TabIndex = 2;
            this.rbtItm.TabStop = true;
            this.rbtItm.Tag = "1";
            this.rbtItm.Text = "项目";
            this.rbtItm.UseVisualStyleBackColor = true;
            this.rbtItm.CheckedChanged += new System.EventHandler(this.rbtItm_CheckedChanged);
            // 
            // btnclose
            // 
            this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnclose.Location = new System.Drawing.Point(1359, 31);
            this.btnclose.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnclose.Name = "btnclose";
            this.btnclose.Size = new System.Drawing.Size(162, 56);
            this.btnclose.TabIndex = 1;
            this.btnclose.Text = "关闭";
            this.btnclose.UseVisualStyleBackColor = true;
            this.btnclose.Click += new System.EventHandler(this.btnclose_Click);
            // 
            // btnSure
            // 
            this.btnSure.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSure.Location = new System.Drawing.Point(1118, 31);
            this.btnSure.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(162, 56);
            this.btnSure.TabIndex = 0;
            this.btnSure.Text = "确定保存";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // rtxtResChr
            // 
            this.rtxtResChr.AutoHeight = false;
            this.rtxtResChr.Name = "rtxtResChr";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // repositoryItemComboBox2
            // 
            this.repositoryItemComboBox2.AutoHeight = false;
            this.repositoryItemComboBox2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox2.Name = "repositoryItemComboBox2";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // gcNullResult
            // 
            this.gcNullResult.DataSource = this.bsNullResult;
            this.gcNullResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcNullResult.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gcNullResult.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gcNullResult.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gcNullResult.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gcNullResult.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gcNullResult.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcNullResult.Location = new System.Drawing.Point(0, 0);
            this.gcNullResult.MainView = this.gvNullResult;
            this.gcNullResult.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcNullResult.Name = "gcNullResult";
            this.gcNullResult.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rtxtResChr,
            this.repositoryItemComboBox1,
            this.repositoryItemComboBox2,
            this.repositoryItemTextEdit1,
            this.repositoryItemComboBox3,
            this.repositoryItemComboBox4});
            this.gcNullResult.ShowOnlyPredefinedDetails = true;
            this.gcNullResult.Size = new System.Drawing.Size(1657, 986);
            this.gcNullResult.TabIndex = 2;
            this.gcNullResult.TabStop = false;
            this.gcNullResult.UseEmbeddedNavigator = true;
            this.gcNullResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvNullResult});
            // 
            // gvNullResult
            // 
            this.gvNullResult.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9});
            this.gvNullResult.GridControl = this.gcNullResult;
            this.gvNullResult.Name = "gvNullResult";
            this.gvNullResult.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvNullResult.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvNullResult.OptionsSelection.MultiSelect = true;
            this.gvNullResult.OptionsView.ColumnAutoWidth = false;
            this.gvNullResult.OptionsView.EnableAppearanceOddRow = true;
            this.gvNullResult.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "病人号";
            this.gridColumn1.FieldName = "pat_in_no";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "病人姓名";
            this.gridColumn2.FieldName = "pat_name";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "仪器";
            this.gridColumn3.FieldName = "itr_mid";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 90;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "序号";
            this.gridColumn4.FieldName = "pat_host_order";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 87;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "样本号";
            this.gridColumn5.FieldName = "pat_sid";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 88;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "组合";
            this.gridColumn6.FieldName = "com_name";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 97;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "项目";
            this.gridColumn7.FieldName = "com_itm_ecd";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            this.gridColumn7.Width = 89;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "未完成原因";
            this.gridColumn8.ColumnEdit = this.repositoryItemComboBox3;
            this.gridColumn8.FieldName = "reason";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            this.gridColumn8.Width = 139;
            // 
            // repositoryItemComboBox3
            // 
            this.repositoryItemComboBox3.AutoHeight = false;
            this.repositoryItemComboBox3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox3.Name = "repositoryItemComboBox3";
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "处理意见";
            this.gridColumn9.ColumnEdit = this.repositoryItemComboBox4;
            this.gridColumn9.FieldName = "opinion";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 8;
            this.gridColumn9.Width = 132;
            // 
            // repositoryItemComboBox4
            // 
            this.repositoryItemComboBox4.AutoHeight = false;
            this.repositoryItemComboBox4.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox4.Name = "repositoryItemComboBox4";
            // 
            // FrmHandOverNullRes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1657, 1098);
            this.Controls.Add(this.gcNullResult);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FrmHandOverNullRes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "交接标本登记";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmHandOverNullRes_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsNullResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtxtResChr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcNullResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvNullResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.BindingSource bsNullResult;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rtxtResChr;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraGrid.GridControl gcNullResult;
        private DevExpress.XtraGrid.Views.Grid.GridView gvNullResult;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox3;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox4;
        private System.Windows.Forms.Button btnclose;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.RadioButton rbtCom;
        private System.Windows.Forms.RadioButton rbtItm;
    }
}