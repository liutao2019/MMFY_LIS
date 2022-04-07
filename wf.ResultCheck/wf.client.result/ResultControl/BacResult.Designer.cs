namespace dcl.client.result
{
    partial class BacResult
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
            this.cmbDescribe = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.gdMultiSid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMultiSid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDescribe)).BeginInit();
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
            this.cmbDescribe});
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
            // cmbDescribe
            // 
            this.cmbDescribe.AutoHeight = false;
            this.cmbDescribe.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbDescribe.DropDownRows = 15;
            this.cmbDescribe.Name = "cmbDescribe";
            // 
            // BacResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gdMultiSid);
            this.Name = "BacResult";
            this.Size = new System.Drawing.Size(655, 391);
            this.Load += new System.EventHandler(this.BacResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gdMultiSid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvMultiSid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbDescribe)).EndInit();
            this.ResumeLayout(false);

        }


       

    

      

        #endregion

        private DevExpress.XtraGrid.GridControl gdMultiSid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvMultiSid;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cmbDescribe;
    }
}
