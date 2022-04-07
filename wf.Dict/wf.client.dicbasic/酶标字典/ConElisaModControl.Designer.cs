using lis.client.control;
namespace dcl.client.dicbasic
{
    partial class ConElisaModControl
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.gcImmMod = new DevExpress.XtraGrid.GridControl();
            this.bsEiasaMod = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcolMod_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.iComboYinYang = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcImmMod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsEiasaMod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iComboYinYang)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gcImmMod);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(757, 686);
            this.panel3.TabIndex = 3;
            // 
            // gcImmMod
            // 
            this.gcImmMod.DataSource = this.bsEiasaMod;
            this.gcImmMod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcImmMod.Location = new System.Drawing.Point(0, 0);
            this.gcImmMod.MainView = this.gridView1;
            this.gcImmMod.Name = "gcImmMod";
            this.gcImmMod.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1,
            this.iComboYinYang});
            this.gcImmMod.Size = new System.Drawing.Size(757, 686);
            this.gcImmMod.TabIndex = 0;
            this.gcImmMod.TabStop = false;
            this.gcImmMod.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // bsEiasaMod
            // 
            this.bsEiasaMod.DataSource = typeof(dcl.entity.EntityDicElisaMeaning);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcolMod_id,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8});
            this.gridView1.GridControl = this.gcImmMod;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsFind.AlwaysVisible = true;
            this.gridView1.OptionsFind.FindNullPrompt = "请输入关键字进行查询！";
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // gcolMod_id
            // 
            this.gcolMod_id.Caption = "编号";
            this.gcolMod_id.FieldName = "MeagId";
            this.gcolMod_id.Name = "gcolMod_id";
            this.gcolMod_id.OptionsColumn.AllowEdit = false;
            this.gcolMod_id.Visible = true;
            this.gcolMod_id.VisibleIndex = 0;
            this.gcolMod_id.Width = 53;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "HBsAg";
            this.gridColumn2.ColumnEdit = this.iComboYinYang;
            this.gridColumn2.FieldName = "MeagItemA";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 55;
            // 
            // iComboYinYang
            // 
            this.iComboYinYang.AutoHeight = false;
            this.iComboYinYang.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.iComboYinYang.Items.AddRange(new object[] {
            "-",
            "+"});
            this.iComboYinYang.Name = "iComboYinYang";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "HBsAb";
            this.gridColumn3.ColumnEdit = this.iComboYinYang;
            this.gridColumn3.FieldName = "MeagItemB";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 69;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "HBeAg";
            this.gridColumn4.ColumnEdit = this.iComboYinYang;
            this.gridColumn4.FieldName = "MeagItemC";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 58;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "HBeAb";
            this.gridColumn5.ColumnEdit = this.iComboYinYang;
            this.gridColumn5.FieldName = "MeagItemD";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "HBcAb";
            this.gridColumn6.ColumnEdit = this.iComboYinYang;
            this.gridColumn6.FieldName = "MeagItemE";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 59;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "频度";
            this.gridColumn7.ColumnEdit = this.repositoryItemComboBox1;
            this.gridColumn7.FieldName = "MeagProbability";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            this.gridColumn7.Width = 48;
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "常见",
            "少见",
            "禁止"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "临床意义";
            this.gridColumn8.FieldName = "MeagConclusion";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "MeagConclusion", "记录总数:{0:0.##}")});
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            this.gridColumn8.Width = 328;
            // 
            // ConElisaModControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Name = "ConElisaModControl";
            this.Size = new System.Drawing.Size(757, 686);
            this.Load += new System.EventHandler(this.ConElisaModControl_Load);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcImmMod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsEiasaMod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iComboYinYang)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraGrid.GridControl gcImmMod;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gcolMod_id;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private System.Windows.Forms.BindingSource bsEiasaMod;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox iComboYinYang;

    }
}
