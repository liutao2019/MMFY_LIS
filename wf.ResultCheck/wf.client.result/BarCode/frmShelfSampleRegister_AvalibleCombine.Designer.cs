namespace dcl.client.result
{
    partial class frmShelfSampleRegister_AvalibleCombine
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
            this.gridControlLeft = new DevExpress.XtraGrid.GridControl();
            this.gridViewLeft = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewLeft)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlLeft
            // 
            this.gridControlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlLeft.Location = new System.Drawing.Point(0, 0);
            this.gridControlLeft.MainView = this.gridViewLeft;
            this.gridControlLeft.Name = "gridControlLeft";
            this.gridControlLeft.Size = new System.Drawing.Size(617, 395);
            this.gridControlLeft.TabIndex = 4;
            this.gridControlLeft.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewLeft});
            // 
            // gridViewLeft
            // 
            this.gridViewLeft.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gridViewLeft.GridControl = this.gridControlLeft;
            this.gridViewLeft.Name = "gridViewLeft";
            this.gridViewLeft.OptionsBehavior.Editable = false;
            this.gridViewLeft.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewLeft.OptionsView.ShowGroupPanel = false;
            this.gridViewLeft.DoubleClick += new System.EventHandler(this.gridViewLeft_DoubleClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "项目编码";
            this.gridColumn1.FieldName = "ComId";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 118;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "项目名称";
            this.gridColumn2.FieldName = "ComName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 276;
            // 
            // frmShelfSampleRegister_AvalibleCombine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 395);
            this.Controls.Add(this.gridControlLeft);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmShelfSampleRegister_AvalibleCombine";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "组合选择";
            this.Deactivate += new System.EventHandler(this.frmShelfSampleRegister_AvalibleCombine_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmShelfSampleRegister_AvalibleCombine_FormClosing);
            this.Load += new System.EventHandler(this.frmShelfSampleRegister_AvalibleCombine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewLeft)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControlLeft;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewLeft;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    }
}