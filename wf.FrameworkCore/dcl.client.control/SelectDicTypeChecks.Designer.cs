namespace dcl.client.control
{
    partial class SelectDicTypeChecks
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
            this.lueType = new DevExpress.XtraEditors.PopupContainerEdit();
            this.lueTypeSource = new DevExpress.XtraEditors.PopupContainerControl();
            this.tlType = new DevExpress.XtraTreeList.TreeList();
            this.colTypeName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colTypeId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            ((System.ComponentModel.ISupportInitialize)(this.lueType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueTypeSource)).BeginInit();
            this.lueTypeSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tlType)).BeginInit();
            this.SuspendLayout();
            // 
            // lueType
            // 
            this.lueType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lueType.Location = new System.Drawing.Point(0, 0);
            this.lueType.Name = "lueType";
            this.lueType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lueType.Properties.PopupControl = this.lueTypeSource;
            this.lueType.Size = new System.Drawing.Size(147, 20);
            this.lueType.TabIndex = 0;
            this.lueType.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.lueType_Closed);
            // 
            // lueTypeSource
            // 
            this.lueTypeSource.Controls.Add(this.tlType);
            this.lueTypeSource.Location = new System.Drawing.Point(27, 126);
            this.lueTypeSource.Name = "lueTypeSource";
            this.lueTypeSource.Size = new System.Drawing.Size(305, 300);
            this.lueTypeSource.TabIndex = 1;
            // 
            // tlType
            // 
            this.tlType.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colTypeName,
            this.colTypeId});
            this.tlType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlType.KeyFieldName = "TypeNodeId";
            this.tlType.Location = new System.Drawing.Point(0, 0);
            this.tlType.Name = "tlType";
            this.tlType.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.tlType.OptionsView.ShowCheckBoxes = true;
            this.tlType.OptionsView.ShowIndicator = false;
            this.tlType.ParentFieldName = "TypeNode";
            this.tlType.Size = new System.Drawing.Size(305, 300);
            this.tlType.TabIndex = 0;
            this.tlType.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.tlType_AfterCheckNode);
            // 
            // colTypeName
            // 
            this.colTypeName.Caption = "物理组";
            this.colTypeName.FieldName = "ProName";
            this.colTypeName.MinWidth = 32;
            this.colTypeName.Name = "colTypeName";
            this.colTypeName.OptionsColumn.AllowEdit = false;
            this.colTypeName.Visible = true;
            this.colTypeName.VisibleIndex = 0;
            // 
            // colTypeId
            // 
            this.colTypeId.Caption = "编码";
            this.colTypeId.FieldName = "ProID";
            this.colTypeId.Name = "colTypeId";
            // 
            // SelectDicTypeChecks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lueTypeSource);
            this.Controls.Add(this.lueType);
            this.Name = "SelectDicTypeChecks";
            this.Size = new System.Drawing.Size(147, 21);
            this.Load += new System.EventHandler(this.SelectDicTypeChecks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lueType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lueTypeSource)).EndInit();
            this.lueTypeSource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tlType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PopupContainerEdit lueType;
        private DevExpress.XtraEditors.PopupContainerControl lueTypeSource;
        private DevExpress.XtraTreeList.TreeList tlType;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colTypeName;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colTypeId;
    }
}
