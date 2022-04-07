namespace dcl.client.dicbasic
{
    partial class ConComReptime
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
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.bsGetReportTime = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colrt_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrt_code = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrt_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrt_time_start = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrt_time_end = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrt_type = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrt_day = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colrt_time = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPropertiesValueChanedLog = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labTime = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtTime = new DevExpress.XtraEditors.SpinEdit();
            this.txtCode = new DevExpress.XtraEditors.TextEdit();
            this.txtWeek = new DevExpress.XtraEditors.TextEdit();
            this.labDayDes = new DevExpress.XtraEditors.LabelControl();
            this.txtDay = new DevExpress.XtraEditors.SpinEdit();
            this.testartTime = new DevExpress.XtraEditors.TimeEdit();
            this.teEndTime = new DevExpress.XtraEditors.TimeEdit();
            this.labDay = new DevExpress.XtraEditors.LabelControl();
            this.cbType = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGetReportTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWeek.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1222, 744);
            this.splitContainerControl1.SplitterPosition = 869;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsGetReportTime;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(869, 744);
            this.gridControl1.TabIndex = 53;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            this.gridControl1.Load += new System.EventHandler(this.on_Load);
            this.gridControl1.Click += new System.EventHandler(this.gridControl1_Click);
            // 
            // bsGetReportTime
            // 
            this.bsGetReportTime.DataSource = typeof(dcl.entity.EntityDicComReptime);
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colrt_id,
            this.colrt_code,
            this.colrt_name,
            this.colrt_time_start,
            this.colrt_time_end,
            this.colrt_type,
            this.colrt_day,
            this.colrt_time,
            this.colPropertiesValueChanedLog});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.Editable = false;
            this.gridView1.OptionsFind.AlwaysVisible = true;
            this.gridView1.OptionsFind.FindNullPrompt = "请输入关键字进行查询！";
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // colrt_id
            // 
            this.colrt_id.Caption = "编码";
            this.colrt_id.FieldName = "RetId";
            this.colrt_id.Name = "colrt_id";
            this.colrt_id.Visible = true;
            this.colrt_id.VisibleIndex = 0;
            this.colrt_id.Width = 44;
            // 
            // colrt_code
            // 
            this.colrt_code.Caption = "代码";
            this.colrt_code.FieldName = "RetCode";
            this.colrt_code.Name = "colrt_code";
            this.colrt_code.Visible = true;
            this.colrt_code.VisibleIndex = 1;
            this.colrt_code.Width = 59;
            // 
            // colrt_name
            // 
            this.colrt_name.Caption = "名称";
            this.colrt_name.FieldName = "RetName";
            this.colrt_name.Name = "colrt_name";
            this.colrt_name.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "RetName", "记录总数:{0:0.##}")});
            this.colrt_name.Visible = true;
            this.colrt_name.VisibleIndex = 2;
            this.colrt_name.Width = 362;
            // 
            // colrt_time_start
            // 
            this.colrt_time_start.Caption = "开始时间";
            this.colrt_time_start.FieldName = "StartTime";
            this.colrt_time_start.Name = "colrt_time_start";
            this.colrt_time_start.OptionsFilter.AllowFilter = false;
            this.colrt_time_start.Visible = true;
            this.colrt_time_start.VisibleIndex = 3;
            this.colrt_time_start.Width = 59;
            // 
            // colrt_time_end
            // 
            this.colrt_time_end.Caption = "结束时间";
            this.colrt_time_end.FieldName = "EndTime";
            this.colrt_time_end.Name = "colrt_time_end";
            this.colrt_time_end.Visible = true;
            this.colrt_time_end.VisibleIndex = 4;
            this.colrt_time_end.Width = 69;
            // 
            // colrt_type
            // 
            this.colrt_type.Caption = "rt_type";
            this.colrt_type.FieldName = "RetType";
            this.colrt_type.Name = "colrt_type";
            // 
            // colrt_day
            // 
            this.colrt_day.Caption = "rt_day";
            this.colrt_day.FieldName = "RetDay";
            this.colrt_day.Name = "colrt_day";
            // 
            // colrt_time
            // 
            this.colrt_time.Caption = "rt_time";
            this.colrt_time.FieldName = "RetTime";
            this.colrt_time.Name = "colrt_time";
            // 
            // colPropertiesValueChanedLog
            // 
            this.colPropertiesValueChanedLog.Caption = "PropertiesValueChanedLog";
            this.colPropertiesValueChanedLog.FieldName = "PropertiesValueChanedLog";
            this.colPropertiesValueChanedLog.Name = "colPropertiesValueChanedLog";
            this.colPropertiesValueChanedLog.OptionsColumn.ReadOnly = true;
            // 
            // groupControl1
            // 
            this.groupControl1.AutoSize = true;
            this.groupControl1.Controls.Add(this.panelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(347, 744);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "基本信息";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.memoEdit1);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labTime);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.txtTime);
            this.panelControl1.Controls.Add(this.txtCode);
            this.panelControl1.Controls.Add(this.txtWeek);
            this.panelControl1.Controls.Add(this.labDayDes);
            this.panelControl1.Controls.Add(this.txtDay);
            this.panelControl1.Controls.Add(this.testartTime);
            this.panelControl1.Controls.Add(this.teEndTime);
            this.panelControl1.Controls.Add(this.labDay);
            this.panelControl1.Controls.Add(this.cbType);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(2, 27);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(343, 715);
            this.panelControl1.TabIndex = 83;
            // 
            // memoEdit1
            // 
            this.memoEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsGetReportTime, "RetName", true));
            this.memoEdit1.Location = new System.Drawing.Point(94, 45);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(235, 335);
            this.memoEdit1.TabIndex = 86;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl6.Location = new System.Drawing.Point(46, 454);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 22);
            this.labelControl6.TabIndex = 4;
            this.labelControl6.Text = "类型";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl4.Location = new System.Drawing.Point(10, 420);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(72, 22);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "结束时间";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl3.Location = new System.Drawing.Point(10, 388);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(72, 22);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "开始时间";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl2.Location = new System.Drawing.Point(46, 43);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(36, 22);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "内容";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl5.Location = new System.Drawing.Point(13, 690);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(200, 18);
            this.labelControl5.TabIndex = 85;
            this.labelControl5.Text = "带红色\"*\"号的项目为必填项！";
            // 
            // labTime
            // 
            this.labTime.Location = new System.Drawing.Point(248, 544);
            this.labTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labTime.Name = "labTime";
            this.labTime.Size = new System.Drawing.Size(51, 18);
            this.labTime.TabIndex = 1;
            this.labTime.Text = "labTime";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labelControl1.Location = new System.Drawing.Point(46, 15);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 22);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "简称";
            // 
            // txtTime
            // 
            this.txtTime.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsGetReportTime, "RetTime", true));
            this.txtTime.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtTime.EnterMoveNextControl = true;
            this.txtTime.Location = new System.Drawing.Point(94, 541);
            this.txtTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTime.Name = "txtTime";
            this.txtTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtTime.Size = new System.Drawing.Size(148, 24);
            this.txtTime.TabIndex = 6;
            // 
            // txtCode
            // 
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsGetReportTime, "RetCode", true));
            this.txtCode.EnterMoveNextControl = true;
            this.txtCode.Location = new System.Drawing.Point(94, 14);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtCode.Properties.MaxLength = 15;
            this.txtCode.Size = new System.Drawing.Size(235, 24);
            this.txtCode.TabIndex = 0;
            // 
            // txtWeek
            // 
            this.txtWeek.EnterMoveNextControl = true;
            this.txtWeek.Location = new System.Drawing.Point(12, 541);
            this.txtWeek.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtWeek.Name = "txtWeek";
            this.txtWeek.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.txtWeek.Properties.MaxLength = 14;
            this.txtWeek.Size = new System.Drawing.Size(70, 24);
            this.txtWeek.TabIndex = 5;
            // 
            // labDayDes
            // 
            this.labDayDes.Location = new System.Drawing.Point(248, 503);
            this.labDayDes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labDayDes.Name = "labDayDes";
            this.labDayDes.Size = new System.Drawing.Size(69, 18);
            this.labDayDes.TabIndex = 1;
            this.labDayDes.Text = "labDayDes";
            // 
            // txtDay
            // 
            this.txtDay.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtDay.EnterMoveNextControl = true;
            this.txtDay.Location = new System.Drawing.Point(94, 499);
            this.txtDay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDay.Name = "txtDay";
            this.txtDay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDay.Size = new System.Drawing.Size(148, 24);
            this.txtDay.TabIndex = 5;
            // 
            // testartTime
            // 
            this.testartTime.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsGetReportTime, "StartTime", true));
            this.testartTime.EditValue = new System.DateTime(2011, 4, 11, 0, 0, 0, 0);
            this.testartTime.EnterMoveNextControl = true;
            this.testartTime.Location = new System.Drawing.Point(94, 387);
            this.testartTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.testartTime.Name = "testartTime";
            this.testartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.testartTime.Properties.DisplayFormat.FormatString = "HH:mm:ss";
            this.testartTime.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.testartTime.Size = new System.Drawing.Size(235, 24);
            this.testartTime.TabIndex = 2;
            // 
            // teEndTime
            // 
            this.teEndTime.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsGetReportTime, "EndTime", true));
            this.teEndTime.EditValue = new System.DateTime(2011, 4, 11, 0, 0, 0, 0);
            this.teEndTime.EnterMoveNextControl = true;
            this.teEndTime.Location = new System.Drawing.Point(94, 419);
            this.teEndTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.teEndTime.Name = "teEndTime";
            this.teEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.teEndTime.Properties.DisplayFormat.FormatString = "HH:mm:ss";
            this.teEndTime.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.teEndTime.Size = new System.Drawing.Size(235, 24);
            this.teEndTime.TabIndex = 3;
            // 
            // labDay
            // 
            this.labDay.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.labDay.Location = new System.Drawing.Point(31, 500);
            this.labDay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labDay.Name = "labDay";
            this.labDay.Size = new System.Drawing.Size(51, 21);
            this.labDay.TabIndex = 1;
            this.labDay.Text = "labDay";
            // 
            // cbType
            // 
            this.cbType.EditValue = "小时间隔";
            this.cbType.EnterMoveNextControl = true;
            this.cbType.Location = new System.Drawing.Point(94, 451);
            this.cbType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbType.Name = "cbType";
            this.cbType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.cbType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbType.Properties.Items.AddRange(new object[] {
            "小时间隔",
            "天间隔",
            "指定周几"});
            this.cbType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbType.Size = new System.Drawing.Size(235, 24);
            this.cbType.TabIndex = 4;
            // 
            // ConComReptime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "ConComReptime";
            this.Size = new System.Drawing.Size(1222, 744);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsGetReportTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtWeek.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbType.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        //private lis.client.control.BarControl barControl1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.BindingSource bsGetReportTime;
        private DevExpress.XtraGrid.Columns.GridColumn colrt_id;
        private DevExpress.XtraGrid.Columns.GridColumn colrt_code;
        private DevExpress.XtraGrid.Columns.GridColumn colrt_name;
        private DevExpress.XtraGrid.Columns.GridColumn colrt_time_start;
        private DevExpress.XtraGrid.Columns.GridColumn colrt_time_end;
        private DevExpress.XtraGrid.Columns.GridColumn colrt_type;
        private DevExpress.XtraGrid.Columns.GridColumn colrt_day;
        private DevExpress.XtraGrid.Columns.GridColumn colrt_time;
        private DevExpress.XtraGrid.Columns.GridColumn colPropertiesValueChanedLog;
        private DevExpress.XtraEditors.TextEdit txtCode;
        private DevExpress.XtraEditors.ComboBoxEdit cbType;
        private DevExpress.XtraEditors.SpinEdit txtTime;
        private DevExpress.XtraEditors.SpinEdit txtDay;
        private DevExpress.XtraEditors.TimeEdit teEndTime;
        private DevExpress.XtraEditors.TimeEdit testartTime;
        private DevExpress.XtraEditors.LabelControl labTime;
        private DevExpress.XtraEditors.LabelControl labDay;
        private DevExpress.XtraEditors.LabelControl labDayDes;
        private DevExpress.XtraEditors.TextEdit txtWeek;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
    }
}
