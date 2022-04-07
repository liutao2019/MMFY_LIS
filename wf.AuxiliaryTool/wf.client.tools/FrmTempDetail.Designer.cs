namespace dcl.client.tools
{
    partial class FrmTempDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTempDetail));
            this.endTime = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.startTime = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.bsDictHar = new System.Windows.Forms.BindingSource();
            this.bsTemHar = new System.Windows.Forms.BindingSource();
            this.panel3 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colDhName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDhCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colThTemperature = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colThDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDhName1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDtLLimit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.endTime.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTime.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDictHar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTemHar)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // endTime
            // 
            this.endTime.EditValue = null;
            this.endTime.Location = new System.Drawing.Point(642, 56);
            this.endTime.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.endTime.Name = "endTime";
            this.endTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.endTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.endTime.Size = new System.Drawing.Size(229, 36);
            this.endTime.TabIndex = 5;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(520, 60);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(96, 29);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "结束时间";
            // 
            // startTime
            // 
            this.startTime.EditValue = null;
            this.startTime.Location = new System.Drawing.Point(162, 56);
            this.startTime.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.startTime.Name = "startTime";
            this.startTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.startTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.startTime.Size = new System.Drawing.Size(247, 36);
            this.startTime.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(49, 60);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(96, 29);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "开始时间";
            // 
            // bsDictHar
            // 
            this.bsDictHar.DataSource = typeof(dcl.entity.EntityDictHarvester);
            this.bsDictHar.CurrentChanged += new System.EventHandler(this.bsTemHar_CurrentChanged);
            // 
            // bsTemHar
            // 
            this.bsTemHar.DataSource = typeof(dcl.entity.EntityTemHarvester);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.sysToolBar1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2043, 131);
            this.panel3.TabIndex = 5;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(8, 11, 8, 11);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(2043, 131);
            this.sysToolBar1.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.endTime);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.startTime);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(3, 3);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(2037, 111);
            this.groupControl2.TabIndex = 6;
            this.groupControl2.Text = "查询条件";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.splitContainerControl1);
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 131);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(2043, 947);
            this.panelControl1.TabIndex = 7;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(3, 114);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControl2);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(2037, 830);
            this.splitContainerControl1.SplitterPosition = 479;
            this.splitContainerControl1.TabIndex = 7;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsDictHar;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(778, 830);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colDhName,
            this.colDhCode});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // colDhName
            // 
            this.colDhName.Caption = "名称";
            this.colDhName.FieldName = "DhName";
            this.colDhName.Name = "colDhName";
            this.colDhName.OptionsColumn.AllowEdit = false;
            this.colDhName.OptionsColumn.AllowFocus = false;
            this.colDhName.Visible = true;
            this.colDhName.VisibleIndex = 1;
            // 
            // colDhCode
            // 
            this.colDhCode.Caption = "编码";
            this.colDhCode.FieldName = "DhCode";
            this.colDhCode.Name = "colDhCode";
            this.colDhCode.OptionsColumn.AllowEdit = false;
            this.colDhCode.OptionsColumn.AllowFocus = false;
            this.colDhCode.Visible = true;
            this.colDhCode.VisibleIndex = 0;
            // 
            // gridControl2
            // 
            this.gridControl2.DataSource = this.bsTemHar;
            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl2.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gridControl2.Location = new System.Drawing.Point(0, 0);
            this.gridControl2.MainView = this.gridView2;
            this.gridControl2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gridControl2.Name = "gridControl2";
            this.gridControl2.Size = new System.Drawing.Size(1249, 830);
            this.gridControl2.TabIndex = 0;
            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.colThTemperature,
            this.colThDate,
            this.colDhName1,
            this.colDtLLimit,
            this.gridColumn2});
            this.gridView2.GridControl = this.gridControl2;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "湿度";
            this.gridColumn1.FieldName = "ThHumidity";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            // 
            // colThTemperature
            // 
            this.colThTemperature.Caption = "温度";
            this.colThTemperature.FieldName = "ThTemperature";
            this.colThTemperature.Name = "colThTemperature";
            this.colThTemperature.OptionsColumn.AllowEdit = false;
            this.colThTemperature.OptionsColumn.AllowFocus = false;
            this.colThTemperature.Visible = true;
            this.colThTemperature.VisibleIndex = 3;
            // 
            // colThDate
            // 
            this.colThDate.Caption = "监测时间";
            this.colThDate.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.colThDate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colThDate.FieldName = "ThDate";
            this.colThDate.Name = "colThDate";
            this.colThDate.OptionsColumn.AllowEdit = false;
            this.colThDate.OptionsColumn.AllowFocus = false;
            this.colThDate.Visible = true;
            this.colThDate.VisibleIndex = 1;
            // 
            // colDhName1
            // 
            this.colDhName1.Caption = "设备";
            this.colDhName1.FieldName = "DhName";
            this.colDhName1.Name = "colDhName1";
            this.colDhName1.OptionsColumn.AllowEdit = false;
            this.colDhName1.OptionsColumn.AllowFocus = false;
            this.colDhName1.Visible = true;
            this.colDhName1.VisibleIndex = 0;
            // 
            // colDtLLimit
            // 
            this.colDtLLimit.Caption = "下限";
            this.colDtLLimit.FieldName = "DtLLimit";
            this.colDtLLimit.Name = "colDtLLimit";
            this.colDtLLimit.OptionsColumn.AllowEdit = false;
            this.colDtLLimit.OptionsColumn.AllowFocus = false;
            this.colDtLLimit.Visible = true;
            this.colDtLLimit.VisibleIndex = 5;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "上限";
            this.gridColumn2.FieldName = "DtHLimit";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowFocus = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
            // 
            // FrmTempDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2043, 1078);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "FrmTempDetail";
            this.Text = "FrmTempDetail";
            ((System.ComponentModel.ISupportInitialize)(this.endTime.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTime.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDictHar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsTemHar)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit startTime;
        private System.Windows.Forms.BindingSource bsDictHar;
        private System.Windows.Forms.BindingSource bsTemHar;
        private DevExpress.XtraEditors.DateEdit endTime;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Panel panel3;
        private common.SysToolBar sysToolBar1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn colDtLLimit;
        private DevExpress.XtraGrid.Columns.GridColumn colDhName1;
        private DevExpress.XtraGrid.Columns.GridColumn colThDate;
        private DevExpress.XtraGrid.Columns.GridColumn colThTemperature;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.GridControl gridControl2;
        private DevExpress.XtraGrid.Columns.GridColumn colDhCode;
        private DevExpress.XtraGrid.Columns.GridColumn colDhName;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
    }
}