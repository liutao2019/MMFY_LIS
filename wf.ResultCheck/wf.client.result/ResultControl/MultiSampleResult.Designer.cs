namespace dcl.client.result
{
    partial class MultiSampleResult
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            this.gdMultiSid = new DevExpress.XtraGrid.GridControl();
            this.gvMultiSid = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repProp = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gdMultiSid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMultiSid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repProp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gdMultiSid
            // 
            this.gdMultiSid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdMultiSid.EmbeddedNavigator.Name = "";
            gridLevelNode1.RelationName = "Level1";
            this.gdMultiSid.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.gdMultiSid.Location = new System.Drawing.Point(0, 0);
            this.gdMultiSid.MainView = this.gvMultiSid;
            this.gdMultiSid.Name = "gdMultiSid";
            this.gdMultiSid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repProp,
            this.repositoryItemTextEdit1});
            this.gdMultiSid.Size = new System.Drawing.Size(655, 391);
            this.gdMultiSid.TabIndex = 1;
            this.gdMultiSid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvMultiSid});
            // 
            // gvMultiSid
            // 
            this.gvMultiSid.GridControl = this.gdMultiSid;
            this.gvMultiSid.Name = "gvMultiSid";
            this.gvMultiSid.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvMultiSid.OptionsView.ShowGroupPanel = false;
            this.gvMultiSid.OptionsView.ShowIndicator = false;
            this.gvMultiSid.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvMultiSid_CellValueChanged);
            this.gvMultiSid.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gvMultiSid_CustomDrawCell);
            this.gvMultiSid.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvMultiSid_RowCellStyle);
            // 
            // repProp
            // 
            this.repProp.AutoHeight = false;
            this.repProp.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repProp.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pro_id", "编码", 80),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itm_prop", "项目特征", 100)});
            this.repProp.DisplayMember = "itm_prop";
            this.repProp.DropDownRows = 10;
            this.repProp.Name = "repProp";
            this.repProp.NullText = "";
            this.repProp.ShowFooter = false;
            this.repProp.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.repProp.ValueMember = "itm_prop";
            this.repProp.Leave += new System.EventHandler(this.repProp_Leave);
            this.repProp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repProp_KeyDown);
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.repositoryItemTextEdit1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repositoryItemTextEdit1_KeyDown);
            this.repositoryItemTextEdit1.Click += new System.EventHandler(this.repositoryItemTextEdit1_Click);
            // 
            // MultiSampleResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gdMultiSid);
            this.Name = "MultiSampleResult";
            this.Size = new System.Drawing.Size(655, 391);
            ((System.ComponentModel.ISupportInitialize)(this.gdMultiSid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMultiSid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repProp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);

        }


       

    

      

        #endregion

        private DevExpress.XtraGrid.GridControl gdMultiSid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMultiSid;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repProp;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
    }
}
