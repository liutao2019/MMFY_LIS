namespace dcl.client.result
{
    partial class ItemResult
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
            this.components = new System.ComponentModel.Container();
            this.gdItem = new DevExpress.XtraGrid.GridControl();
            this.gvItem = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repItem1 = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.bsItemProp = new System.Windows.Forms.BindingSource(this.components);
            this.repProp1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repItem = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.bsItem = new System.Windows.Forms.BindingSource(this.components);
            this.repProp = new lis.client.control.ctlRepositoryItemLookUpEdit();
            this.repComProp = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repResulto = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gdItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItemProp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repProp1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repProp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repComProp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repResulto)).BeginInit();
            this.SuspendLayout();
            // 
            // gdItem
            // 
            this.gdItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gdItem.Location = new System.Drawing.Point(0, 0);
            this.gdItem.MainView = this.gvItem;
            this.gdItem.Name = "gdItem";
            this.gdItem.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repItem1,
            this.repProp1,
            this.repItem,
            this.repProp,
            this.repComProp,
            this.repResulto});
            this.gdItem.Size = new System.Drawing.Size(369, 354);
            this.gdItem.TabIndex = 0;
            this.gdItem.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvItem});
            this.gdItem.DoubleClick += new System.EventHandler(this.gdItem_DoubleClick);
            // 
            // gvItem
            // 
            this.gvItem.GridControl = this.gdItem;
            this.gvItem.Name = "gvItem";
            this.gvItem.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gvItem.OptionsView.ShowGroupPanel = false;
            this.gvItem.OptionsView.ShowIndicator = false;
            this.gvItem.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gvItem_RowCellStyle);
            this.gvItem.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvItem_CellValueChanged);
            // 
            // repItem1
            // 
            this.repItem1.ActionButtonIndex = 1;
            this.repItem1.AutoHeight = false;
            this.repItem1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItem1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("PtyItmId", 50, "编码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("PtyItmEname", 75, "项目代码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("PtyItmName", 100, "项目名称")});
            this.repItem1.DataSource = this.bsItemProp;
            this.repItem1.DisplayMember = "PtyItmEname";
            this.repItem1.DropDownRows = 10;
            this.repItem1.Name = "repItem1";
            this.repItem1.NullText = "";
            this.repItem1.PopupWidth = 250;
            this.repItem1.ShowFooter = false;
            this.repItem1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.repItem1.ValueMember = "PtyItmId";
            this.repItem1.EditValueChanged += new System.EventHandler(this.repItem_EditValueChanged);
            this.repItem1.Leave += new System.EventHandler(this.repItem_Leave);
            // 
            // bsItemProp
            // 
            this.bsItemProp.DataSource = typeof(dcl.entity.EntityDefItmProperty);
            // 
            // repProp1
            // 
            this.repProp1.ActionButtonIndex = 1;
            this.repProp1.AutoHeight = false;
            this.repProp1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repProp1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("pro_id", 50, "编码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("itm_prop", 120, "项目特征")});
            this.repProp1.DisplayMember = "itm_prop";
            this.repProp1.DropDownRows = 10;
            this.repProp1.Name = "repProp1";
            this.repProp1.NullText = "";
            this.repProp1.PopupWidth = 170;
            this.repProp1.ShowFooter = false;
            this.repProp1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.repProp1.ValueMember = "itm_prop";
            this.repProp1.Enter += new System.EventHandler(this.repProp_Enter);
            this.repProp1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repProp_KeyDown);
            this.repProp1.Leave += new System.EventHandler(this.repProp_Leave);
            // 
            // repItem
            // 
            this.repItem.ActionButtonIndex = 1;
            this.repItem.AutoHeight = false;
            this.repItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repItem.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmId", 50, "编码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmEcode", 75, "项目代码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmName", 100, "项目名称")});
            this.repItem.DataSource = this.bsItem;
            this.repItem.DisplayMember = "ItmEcode";
            this.repItem.DropDownRows = 10;
            this.repItem.Name = "repItem";
            this.repItem.NullText = "";
            this.repItem.PopupWidth = 250;
            this.repItem.ShowFooter = false;
            this.repItem.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.repItem.ValueMember = "ItmId";
            this.repItem.EditValueChanged += new System.EventHandler(this.repItem_EditValueChanged);
            this.repItem.Leave += new System.EventHandler(this.repItem_Leave);
            // 
            // bsItem
            // 
            this.bsItem.DataSource = typeof(dcl.entity.EntityDicItmItem);
            // 
            // repProp
            // 
            this.repProp.ActionButtonIndex = 1;
            this.repProp.AutoHeight = false;
            this.repProp.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repProp.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("PtyId", 50, "编码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("PtyItmProperty", 120, "特征")});
            this.repProp.DataSource = this.bsItemProp;
            this.repProp.DisplayMember = "PtyItmProperty";
            this.repProp.DropDownRows = 10;
            this.repProp.Name = "repProp";
            this.repProp.NullText = "";
            this.repProp.PopupWidth = 170;
            this.repProp.ShowFooter = false;
            this.repProp.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.repProp.ValueMember = "PtyItmProperty";
            this.repProp.Enter += new System.EventHandler(this.repProp_Enter);
            this.repProp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repProp_KeyDown);
            this.repProp.Leave += new System.EventHandler(this.repProp_Leave);
            // 
            // repComProp
            // 
            this.repComProp.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.repComProp.AutoComplete = false;
            this.repComProp.AutoHeight = false;
            this.repComProp.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repComProp.ImmediatePopup = true;
            this.repComProp.Name = "repComProp";
            this.repComProp.Enter += new System.EventHandler(this.repProp_Enter);
            this.repComProp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repProp_KeyDown);
            this.repComProp.Leave += new System.EventHandler(this.repProp_Leave);
            // 
            // repResulto
            // 
            this.repResulto.AutoHeight = false;
            this.repResulto.Name = "repResulto";
            this.repResulto.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repResulto_KeyDown);
            // 
            // ItemResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gdItem);
            this.Name = "ItemResult";
            this.Size = new System.Drawing.Size(369, 354);
            
            ((System.ComponentModel.ISupportInitialize)(this.gdItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItemProp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repProp1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repProp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repComProp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repResulto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gdItem;
        private DevExpress.XtraGrid.Views.Grid.GridView gvItem;
        private lis.client.control.ctlRepositoryItemLookUpEdit repItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repProp1;
        private lis.client.control.ctlRepositoryItemLookUpEdit repItem;
        private lis.client.control.ctlRepositoryItemLookUpEdit repProp;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repComProp;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repResulto;
        private System.Windows.Forms.BindingSource bsItem;
        private System.Windows.Forms.BindingSource bsItemProp;
    }
}
