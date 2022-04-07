namespace dcl.client.result
{
    partial class ImmResult
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
            this.gdImm = new DevExpress.XtraGrid.GridControl();
            this.gvImm = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repText = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gdImm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvImm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repText)).BeginInit();
            this.SuspendLayout();
            // 
            // gdImm
            // 
            this.gdImm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdImm.EmbeddedNavigator.Name = "";
            this.gdImm.Location = new System.Drawing.Point(0, 0);
            this.gdImm.MainView = this.gvImm;
            this.gdImm.Name = "gdImm";
            this.gdImm.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repText});
            this.gdImm.Size = new System.Drawing.Size(607, 376);
            this.gdImm.TabIndex = 2;
            this.gdImm.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvImm});
            // 
            // gvImm
            // 
            this.gvImm.GridControl = this.gdImm;
            this.gvImm.Name = "gvImm";
            this.gvImm.OptionsNavigation.EnterMoveNextColumn = true;
            this.gvImm.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvImm.OptionsView.ShowGroupPanel = false;
            this.gvImm.OptionsView.ShowIndicator = false;
            this.gvImm.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvImm_CellValueChanged);
            this.gvImm.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvImm_RowCellStyle);
            // 
            // repText
            // 
            this.repText.AutoHeight = false;
            this.repText.Name = "repText";
            this.repText.DoubleClick += new System.EventHandler(this.repText_DoubleClick);
            // 
            // ImmResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gdImm);
            this.Name = "ImmResult";
            this.Size = new System.Drawing.Size(607, 376);
            this.Load += new System.EventHandler(this.ImmResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gdImm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvImm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repText)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gdImm;
        private DevExpress.XtraGrid.Views.Grid.GridView gvImm;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repText;
    }
}
