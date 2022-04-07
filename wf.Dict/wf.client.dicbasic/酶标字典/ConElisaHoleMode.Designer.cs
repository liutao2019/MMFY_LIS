using lis.client.control;
namespace dcl.client.dicbasic
{
    partial class ConElisaHoleMode
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.sampleControl1 = new dcl.client.elisa.SampleControl();
            this.bsEiasaHoleMode = new System.Windows.Forms.BindingSource(this.components);
            this.eiasaAoutoHoleModeControl1 = new dcl.client.dicbasic.EiasaAoutoHoleModeControl();
            this.gcHoleMode = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcolImh_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsEiasaHoleMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHoleMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.gcHoleMode);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(933, 817);
            this.panel3.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.sampleControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(564, 817);
            this.panel1.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.eiasaAoutoHoleModeControl1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 635);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(564, 182);
            this.panel4.TabIndex = 2;
            // 
            // sampleControl1
            // 
            this.sampleControl1.DataBindings.Add(new System.Windows.Forms.Binding("FormatHoleValues", this.bsEiasaHoleMode, "SortHoleSorting", true));
            this.sampleControl1.DefaultValueOfHoles = null;
            this.sampleControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sampleControl1.FormatHoleValues = ",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,," +
    ",,,,,,,,,,,,,,,";
            this.sampleControl1.HoleMode = null;
            this.sampleControl1.HoleStatus = null;
            this.sampleControl1.ImmID = null;
            this.sampleControl1.ItemType = dcl.client.elisa.ControlType.TextBox;
            this.sampleControl1.Location = new System.Drawing.Point(0, 0);
            this.sampleControl1.MarginOfControls = 38;
            this.sampleControl1.Name = "sampleControl1";
            this.sampleControl1.SampleRange = null;
            this.sampleControl1.Size = new System.Drawing.Size(564, 817);
            this.sampleControl1.TabIndex = 1;
            // 
            // bsEiasaHoleMode
            // 
            this.bsEiasaHoleMode.DataSource = typeof(dcl.entity.EntityDicElisaSort);
            // 
            // eiasaAoutoHoleModeControl1
            // 
            this.eiasaAoutoHoleModeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eiasaAoutoHoleModeControl1.Location = new System.Drawing.Point(0, 0);
            this.eiasaAoutoHoleModeControl1.Name = "eiasaAoutoHoleModeControl1";
            this.eiasaAoutoHoleModeControl1.Size = new System.Drawing.Size(564, 182);
            this.eiasaAoutoHoleModeControl1.TabIndex = 0;
            // 
            // gcHoleMode
            // 
            this.gcHoleMode.DataSource = this.bsEiasaHoleMode;
            this.gcHoleMode.Dock = System.Windows.Forms.DockStyle.Right;
            this.gcHoleMode.Location = new System.Drawing.Point(564, 0);
            this.gcHoleMode.MainView = this.gridView1;
            this.gcHoleMode.Name = "gcHoleMode";
            this.gcHoleMode.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1});
            this.gcHoleMode.Size = new System.Drawing.Size(369, 817);
            this.gcHoleMode.TabIndex = 0;
            this.gcHoleMode.TabStop = false;
            this.gcHoleMode.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcolImh_id,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn1});
            this.gridView1.GridControl = this.gcHoleMode;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsFind.AlwaysVisible = true;
            this.gridView1.OptionsFind.FindNullPrompt = "请输入关键字进行查询！";
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // gcolImh_id
            // 
            this.gcolImh_id.Caption = "编号";
            this.gcolImh_id.FieldName = "SortId";
            this.gcolImh_id.Name = "gcolImh_id";
            this.gcolImh_id.OptionsColumn.AllowEdit = false;
            this.gcolImh_id.Visible = true;
            this.gcolImh_id.VisibleIndex = 0;
            this.gcolImh_id.Width = 47;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "孔位序号模式";
            this.gridColumn2.FieldName = "SortName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "SortName", "记录总数:{0:0.##}")});
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 135;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "孔位序号值";
            this.gridColumn3.FieldName = "SortHoleSorting";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "样板ID";
            this.gridColumn1.FieldName = "PlateId";
            this.gridColumn1.Name = "gridColumn1";
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
            // ConElisaHoleMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Name = "ConElisaHoleMode";
            this.Size = new System.Drawing.Size(933, 817);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsEiasaHoleMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHoleMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraGrid.GridControl gcHoleMode;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gcolImh_id;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private System.Windows.Forms.BindingSource bsEiasaHoleMode;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.Panel panel1;
        private dcl.client.elisa.SampleControl sampleControl1;
        private System.Windows.Forms.Panel panel4;
        private dcl.client.dicbasic.EiasaAoutoHoleModeControl eiasaAoutoHoleModeControl1;

    }
}
