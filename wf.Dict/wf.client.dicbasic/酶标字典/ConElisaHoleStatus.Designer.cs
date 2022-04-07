using lis.client.control;
namespace dcl.client.dicbasic
{
    partial class ConElisaHoleStatus
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
            this.statusDescribeControl1 = new dcl.client.elisa.controls.StatusDescribeControl();
            this.sampleControl1 = new dcl.client.elisa.SampleControl();
            this.bsEiasaHoleStatus = new System.Windows.Forms.BindingSource(this.components);
            this.gcHoleStatus = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcolImh_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsEiasaHoleStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHoleStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.gcHoleStatus);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(933, 817);
            this.panel3.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.statusDescribeControl1);
            this.panel1.Controls.Add(this.sampleControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(611, 817);
            this.panel1.TabIndex = 2;
            // 
            // statusDescribeControl1
            // 
            this.statusDescribeControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.statusDescribeControl1.Location = new System.Drawing.Point(0, 762);
            this.statusDescribeControl1.Name = "statusDescribeControl1";
            this.statusDescribeControl1.Size = new System.Drawing.Size(611, 55);
            this.statusDescribeControl1.TabIndex = 2;
            // 
            // sampleControl1
            // 
            this.sampleControl1.DataBindings.Add(new System.Windows.Forms.Binding("FormatHoleValues", this.bsEiasaHoleStatus, "StaHoleStaus", true));
            this.sampleControl1.DefaultValueOfHoles = "1";
            this.sampleControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sampleControl1.FormatHoleValues = ",1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1," +
    "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1," +
    "1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1";
            this.sampleControl1.HoleMode = null;
            this.sampleControl1.HoleStatus = null;
            this.sampleControl1.ImmID = null;
            this.sampleControl1.ItemType = dcl.client.elisa.ControlType.TextBox;
            this.sampleControl1.Location = new System.Drawing.Point(0, 0);
            this.sampleControl1.MarginOfControls = 38;
            this.sampleControl1.Name = "sampleControl1";
            this.sampleControl1.SampleRange = null;
            this.sampleControl1.Size = new System.Drawing.Size(611, 817);
            this.sampleControl1.TabIndex = 1;
            // 
            // bsEiasaHoleStatus
            // 
            this.bsEiasaHoleStatus.DataSource = typeof(dcl.entity.EntityDicElisaStatus);
            // 
            // gcHoleStatus
            // 
            this.gcHoleStatus.DataSource = this.bsEiasaHoleStatus;
            this.gcHoleStatus.Dock = System.Windows.Forms.DockStyle.Right;
            this.gcHoleStatus.Location = new System.Drawing.Point(611, 0);
            this.gcHoleStatus.MainView = this.gridView1;
            this.gcHoleStatus.Name = "gcHoleStatus";
            this.gcHoleStatus.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBox1});
            this.gcHoleStatus.Size = new System.Drawing.Size(322, 817);
            this.gcHoleStatus.TabIndex = 0;
            this.gcHoleStatus.TabStop = false;
            this.gcHoleStatus.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcolImh_id,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn1});
            this.gridView1.GridControl = this.gcHoleStatus;
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
            this.gcolImh_id.FieldName = "StaId";
            this.gcolImh_id.Name = "gcolImh_id";
            this.gcolImh_id.OptionsColumn.AllowEdit = false;
            this.gcolImh_id.Visible = true;
            this.gcolImh_id.VisibleIndex = 0;
            this.gcolImh_id.Width = 47;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "孔位状态模式";
            this.gridColumn2.FieldName = "StaName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "StaName", "记录总数:{0:0.##}")});
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 135;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "孔位状态值";
            this.gridColumn3.FieldName = "StaHoleStaus";
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
            // ConElisaHoleStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Name = "ConElisaHoleStatus";
            this.Size = new System.Drawing.Size(933, 817);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsEiasaHoleStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gcHoleStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraGrid.GridControl gcHoleStatus;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gcolImh_id;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private System.Windows.Forms.BindingSource bsEiasaHoleStatus;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.Panel panel1;
        private dcl.client.elisa.SampleControl sampleControl1;
        private dcl.client.elisa.controls.StatusDescribeControl statusDescribeControl1;

    }
}
