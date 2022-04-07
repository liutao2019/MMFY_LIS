namespace dcl.client.result.PatControl
{
    partial class ControlPatListNew
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlPatListNew));
            this.cmbBarSearchPatType = new DevExpress.XtraEditors.LookUpEdit();
            this.txtBarSearchCondition = new DevExpress.XtraEditors.TextEdit();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.危急值记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAllowMidReport = new System.Windows.Forms.ToolStripMenuItem();
            this.审核ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消审核ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打印报告ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除标本ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.批量添加组合ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.报告解读ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更新为传染病toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bsPatList = new System.Windows.Forms.BindingSource(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.txtPatInstructment = new dcl.client.control.SelectDicInstrument();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtPatType = new dcl.client.control.SelectDicLabProfession();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.dtEnd = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dtBegin = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.pnlFilterBar = new DevExpress.XtraEditors.PanelControl();
            this.cmbGridFilterPatientState = new DevExpress.XtraEditors.LookUpEdit();
            this.cmbGridFilterPatCType = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.lbRecordCount = new System.Windows.Forms.Label();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.rptOrigin = new dcl.client.result.PatControl.RoundPanelType();
            this.roundPanelGroup = new dcl.client.result.PatControl.RoundPanelGroup();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridViewPatientList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.col_icon = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemImageEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageEdit();
            this.col_pat_sid_int = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_pat_sid = new DevExpress.XtraGrid.Columns.GridColumn();
            this.col_pat_flag = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbBarSearchPatType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarSearchCondition.Properties)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsPatList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlFilterBar)).BeginInit();
            this.pnlFilterBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGridFilterPatientState.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGridFilterPatCType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPatientList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbBarSearchPatType
            // 
            this.cmbBarSearchPatType.EditValue = "pat_sid_int";
            this.cmbBarSearchPatType.Location = new System.Drawing.Point(74, 83);
            this.cmbBarSearchPatType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbBarSearchPatType.Name = "cmbBarSearchPatType";
            this.cmbBarSearchPatType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbBarSearchPatType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("name", "类型"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("value", "value", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default)});
            this.cmbBarSearchPatType.Properties.DisplayMember = "name";
            this.cmbBarSearchPatType.Properties.NullText = "";
            this.cmbBarSearchPatType.Properties.ValueMember = "value";
            this.cmbBarSearchPatType.Size = new System.Drawing.Size(120, 24);
            this.cmbBarSearchPatType.TabIndex = 0;
            // 
            // txtBarSearchCondition
            // 
            this.txtBarSearchCondition.Location = new System.Drawing.Point(234, 83);
            this.txtBarSearchCondition.Margin = new System.Windows.Forms.Padding(4);
            this.txtBarSearchCondition.Name = "txtBarSearchCondition";
            this.txtBarSearchCondition.Size = new System.Drawing.Size(121, 24);
            this.txtBarSearchCondition.TabIndex = 1;
            this.txtBarSearchCondition.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.txtBarSearchCondition_EditValueChanging);
            this.txtBarSearchCondition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBarSearchCondition_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.menuConfig,
            this.危急值记录ToolStripMenuItem,
            this.menuAllowMidReport,
            this.审核ToolStripMenuItem,
            this.取消审核ToolStripMenuItem,
            this.打印报告ToolStripMenuItem,
            this.删除标本ToolStripMenuItem,
            this.保存信息ToolStripMenuItem,
            this.批量添加组合ToolStripMenuItem,
            this.报告解读ToolStripMenuItem,
            this.更新为传染病toolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(199, 274);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 6);
            // 
            // menuConfig
            // 
            this.menuConfig.Name = "menuConfig";
            this.menuConfig.Size = new System.Drawing.Size(198, 24);
            this.menuConfig.Text = "面板设置";
            this.menuConfig.Click += new System.EventHandler(this.menuConfig_Click);
            // 
            // 危急值记录ToolStripMenuItem
            // 
            this.危急值记录ToolStripMenuItem.Name = "危急值记录ToolStripMenuItem";
            this.危急值记录ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.危急值记录ToolStripMenuItem.Text = "危急值记录";
            this.危急值记录ToolStripMenuItem.Click += new System.EventHandler(this.危急值记录ToolStripMenuItem_Click);
            // 
            // menuAllowMidReport
            // 
            this.menuAllowMidReport.Name = "menuAllowMidReport";
            this.menuAllowMidReport.Size = new System.Drawing.Size(198, 24);
            this.menuAllowMidReport.Text = "允许发送中期报告";
            this.menuAllowMidReport.Visible = false;
            this.menuAllowMidReport.Click += new System.EventHandler(this.menuAllowMidReport_Click);
            // 
            // 审核ToolStripMenuItem
            // 
            this.审核ToolStripMenuItem.Name = "审核ToolStripMenuItem";
            this.审核ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.审核ToolStripMenuItem.Text = "审核报告";
            this.审核ToolStripMenuItem.Visible = false;
            // 
            // 取消审核ToolStripMenuItem
            // 
            this.取消审核ToolStripMenuItem.Name = "取消审核ToolStripMenuItem";
            this.取消审核ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.取消审核ToolStripMenuItem.Text = "取消审核";
            this.取消审核ToolStripMenuItem.Visible = false;
            // 
            // 打印报告ToolStripMenuItem
            // 
            this.打印报告ToolStripMenuItem.Name = "打印报告ToolStripMenuItem";
            this.打印报告ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.打印报告ToolStripMenuItem.Text = "打印报告";
            this.打印报告ToolStripMenuItem.Visible = false;
            // 
            // 删除标本ToolStripMenuItem
            // 
            this.删除标本ToolStripMenuItem.Name = "删除标本ToolStripMenuItem";
            this.删除标本ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.删除标本ToolStripMenuItem.Text = "删除标本";
            this.删除标本ToolStripMenuItem.Visible = false;
            // 
            // 保存信息ToolStripMenuItem
            // 
            this.保存信息ToolStripMenuItem.Name = "保存信息ToolStripMenuItem";
            this.保存信息ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.保存信息ToolStripMenuItem.Text = "保存资料";
            this.保存信息ToolStripMenuItem.Visible = false;
            // 
            // 批量添加组合ToolStripMenuItem
            // 
            this.批量添加组合ToolStripMenuItem.Name = "批量添加组合ToolStripMenuItem";
            this.批量添加组合ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.批量添加组合ToolStripMenuItem.Text = "批量添加组合";
            this.批量添加组合ToolStripMenuItem.Click += new System.EventHandler(this.批量添加组合ToolStripMenuItem_Click);
            // 
            // 报告解读ToolStripMenuItem
            // 
            this.报告解读ToolStripMenuItem.Name = "报告解读ToolStripMenuItem";
            this.报告解读ToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.报告解读ToolStripMenuItem.Text = "报告解读";
            this.报告解读ToolStripMenuItem.Click += new System.EventHandler(this.报告解读ToolStripMenuItem_Click);
            // 
            // 更新为传染病toolStripMenuItem
            // 
            this.更新为传染病toolStripMenuItem.Name = "更新为传染病toolStripMenuItem";
            this.更新为传染病toolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.更新为传染病toolStripMenuItem.Text = "更新为传染病";
            this.更新为传染病toolStripMenuItem.Click += new System.EventHandler(this.更新为传染病ToolStripMenuItem_Click);
            // 
            // bsPatList
            // 
            this.bsPatList.DataSource = typeof(dcl.entity.EntityPidReportMain);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "查询.png");
            this.imageList1.Images.SetKeyName(1, "16px-绿.png");
            this.imageList1.Images.SetKeyName(2, "16px-黄.png");
            this.imageList1.Images.SetKeyName(3, "16px-红.png");
            this.imageList1.Images.SetKeyName(4, "flag-blue.ico");
            this.imageList1.Images.SetKeyName(5, "flag-orange.ico");
            this.imageList1.Images.SetKeyName(6, "flag-purple.ico");
            this.imageList1.Images.SetKeyName(7, "flag-black.ico");
            this.imageList1.Images.SetKeyName(8, "flag-green.ico");
            this.imageList1.Images.SetKeyName(9, "flag-yellow.ico");
            this.imageList1.Images.SetKeyName(10, "flag-red.ico");
            // 
            // txtPatInstructment
            // 
            this.txtPatInstructment.AddEmptyRow = true;
            this.txtPatInstructment.AutoScroll = true;
            this.txtPatInstructment.BindByValue = false;
            this.txtPatInstructment.colDisplay = "";
            this.txtPatInstructment.colExtend1 = null;
            this.txtPatInstructment.colInCode = "";
            this.txtPatInstructment.colPY = "";
            this.txtPatInstructment.colSeq = "";
            this.txtPatInstructment.colValue = "";
            this.txtPatInstructment.colWB = "";
            this.txtPatInstructment.displayMember = null;
            this.txtPatInstructment.EnterMoveNext = true;
            this.txtPatInstructment.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtPatInstructment.KeyUpDownMoveNext = true;
            this.txtPatInstructment.LoadDataOnDesignMode = true;
            this.txtPatInstructment.Location = new System.Drawing.Point(234, 56);
            this.txtPatInstructment.Margin = new System.Windows.Forms.Padding(4);
            this.txtPatInstructment.MaximumSize = new System.Drawing.Size(667, 26);
            this.txtPatInstructment.MinimumSize = new System.Drawing.Size(67, 26);
            this.txtPatInstructment.Name = "txtPatInstructment";
            this.txtPatInstructment.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtPatInstructment.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtPatInstructment.Readonly = false;
            this.txtPatInstructment.SaveSourceID = false;
            this.txtPatInstructment.SelectFilter = null;
            this.txtPatInstructment.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtPatInstructment.SelectOnly = true;
            this.txtPatInstructment.ShowAllInstrmt = false;
            this.txtPatInstructment.Size = new System.Drawing.Size(121, 26);
            this.txtPatInstructment.TabIndex = 20;
            this.txtPatInstructment.UseExtend = false;
            this.txtPatInstructment.valueMember = null;
            this.txtPatInstructment.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicInstrument>.ValueChangedEventHandler(this.txtPatInstructment_ValueChanged);
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(201, 85);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(30, 18);
            this.labelControl8.TabIndex = 12;
            this.labelControl8.Text = "键值";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(11, 85);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(60, 18);
            this.labelControl7.TabIndex = 10;
            this.labelControl7.Text = "关键查询";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(201, 58);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(30, 18);
            this.labelControl6.TabIndex = 8;
            this.labelControl6.Text = "仪器";
            // 
            // txtPatType
            // 
            this.txtPatType.AddEmptyRow = true;
            this.txtPatType.AutoScroll = true;
            this.txtPatType.BindByValue = false;
            this.txtPatType.colDisplay = "";
            this.txtPatType.colExtend1 = null;
            this.txtPatType.colInCode = "";
            this.txtPatType.colPY = "";
            this.txtPatType.colSeq = "type_seq";
            this.txtPatType.colValue = "";
            this.txtPatType.colWB = "";
            this.txtPatType.displayMember = null;
            this.txtPatType.EnterMoveNext = true;
            this.txtPatType.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtPatType.KeyUpDownMoveNext = true;
            this.txtPatType.LoadDataOnDesignMode = true;
            this.txtPatType.Location = new System.Drawing.Point(74, 56);
            this.txtPatType.Margin = new System.Windows.Forms.Padding(4);
            this.txtPatType.MaximumSize = new System.Drawing.Size(667, 26);
            this.txtPatType.MinimumSize = new System.Drawing.Size(67, 26);
            this.txtPatType.Name = "txtPatType";
            this.txtPatType.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtPatType.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtPatType.Readonly = false;
            this.txtPatType.SaveSourceID = false;
            this.txtPatType.SelectFilter = null;
            this.txtPatType.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtPatType.SelectOnly = true;
            this.txtPatType.Size = new System.Drawing.Size(120, 26);
            this.txtPatType.TabIndex = 7;
            this.txtPatType.UseExtend = false;
            this.txtPatType.valueMember = null;
            this.txtPatType.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.ValueChangedEventHandler(this.txtPatType_ValueChanged);
            this.txtPatType.DisplayTextChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntityDicPubProfession>.TextChangedEventHandler(this.txtPatType_DisplayTextChanged);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(25, 58);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(45, 18);
            this.labelControl5.TabIndex = 6;
            this.labelControl5.Text = "实验组";
            // 
            // dtEnd
            // 
            this.dtEnd.EditValue = null;
            this.dtEnd.Location = new System.Drawing.Point(234, 29);
            this.dtEnd.Margin = new System.Windows.Forms.Padding(4);
            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtEnd.Size = new System.Drawing.Size(121, 24);
            this.dtEnd.TabIndex = 5;
            this.dtEnd.EditValueChanged += new System.EventHandler(this.dtEnd_EditValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(210, 31);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(14, 18);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "—";
            // 
            // dtBegin
            // 
            this.dtBegin.EditValue = null;
            this.dtBegin.Location = new System.Drawing.Point(74, 29);
            this.dtBegin.Margin = new System.Windows.Forms.Padding(4);
            this.dtBegin.Name = "dtBegin";
            this.dtBegin.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dtBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtBegin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtBegin.Size = new System.Drawing.Size(120, 24);
            this.dtBegin.TabIndex = 3;
            this.dtBegin.EditValueChanged += new System.EventHandler(this.dtBegin_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(10, 31);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "检验日期";
            // 
            // pnlFilterBar
            // 
            this.pnlFilterBar.Controls.Add(this.cmbGridFilterPatientState);
            this.pnlFilterBar.Controls.Add(this.cmbGridFilterPatCType);
            this.pnlFilterBar.Controls.Add(this.labelControl4);
            this.pnlFilterBar.Controls.Add(this.labelControl3);
            this.pnlFilterBar.Controls.Add(this.lbRecordCount);
            this.pnlFilterBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFilterBar.Location = new System.Drawing.Point(0, 749);
            this.pnlFilterBar.Margin = new System.Windows.Forms.Padding(4);
            this.pnlFilterBar.Name = "pnlFilterBar";
            this.pnlFilterBar.Size = new System.Drawing.Size(365, 23);
            this.pnlFilterBar.TabIndex = 1;
            // 
            // cmbGridFilterPatientState
            // 
            this.cmbGridFilterPatientState.EditValue = "-1";
            this.cmbGridFilterPatientState.Location = new System.Drawing.Point(164, 2);
            this.cmbGridFilterPatientState.Margin = new System.Windows.Forms.Padding(4);
            this.cmbGridFilterPatientState.Name = "cmbGridFilterPatientState";
            this.cmbGridFilterPatientState.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGridFilterPatientState.Properties.Appearance.Options.UseFont = true;
            this.cmbGridFilterPatientState.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbGridFilterPatientState.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "编码", 10, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("value", 15, "状态")});
            this.cmbGridFilterPatientState.Properties.DisplayMember = "value";
            this.cmbGridFilterPatientState.Properties.NullText = "";
            this.cmbGridFilterPatientState.Properties.ValueMember = "id";
            this.cmbGridFilterPatientState.Size = new System.Drawing.Size(97, 22);
            this.cmbGridFilterPatientState.TabIndex = 159;
            this.cmbGridFilterPatientState.Visible = false;
            this.cmbGridFilterPatientState.EditValueChanged += new System.EventHandler(this.cmbGridFilterPatientState_EditValueChanged);
            // 
            // cmbGridFilterPatCType
            // 
            this.cmbGridFilterPatCType.EditValue = "-1";
            this.cmbGridFilterPatCType.EnterMoveNextControl = true;
            this.cmbGridFilterPatCType.Location = new System.Drawing.Point(667, 4);
            this.cmbGridFilterPatCType.Margin = new System.Windows.Forms.Padding(4);
            this.cmbGridFilterPatCType.Name = "cmbGridFilterPatCType";
            this.cmbGridFilterPatCType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbGridFilterPatCType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "类型", 20, DevExpress.Utils.FormatType.None, "", false, DevExpress.Utils.HorzAlignment.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("value", "类型")});
            this.cmbGridFilterPatCType.Properties.DisplayMember = "value";
            this.cmbGridFilterPatCType.Properties.HeaderClickMode = DevExpress.XtraEditors.Controls.HeaderClickMode.AutoSearch;
            this.cmbGridFilterPatCType.Properties.ImmediatePopup = true;
            this.cmbGridFilterPatCType.Properties.NullText = "";
            this.cmbGridFilterPatCType.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
            this.cmbGridFilterPatCType.Properties.ValueMember = "id";
            this.cmbGridFilterPatCType.Size = new System.Drawing.Size(69, 24);
            this.cmbGridFilterPatCType.TabIndex = 160;
            this.cmbGridFilterPatCType.Visible = false;
            this.cmbGridFilterPatCType.EditValueChanged += new System.EventHandler(this.cmbGridFilterPatCType_EditValueChanged);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(616, 8);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(45, 18);
            this.labelControl4.TabIndex = 161;
            this.labelControl4.Text = "类型：";
            this.labelControl4.Visible = false;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 7.8F);
            this.labelControl3.Location = new System.Drawing.Point(128, 3);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(42, 17);
            this.labelControl3.TabIndex = 162;
            this.labelControl3.Text = "状态：";
            this.labelControl3.Visible = false;
            // 
            // lbRecordCount
            // 
            this.lbRecordCount.AutoSize = true;
            this.lbRecordCount.Location = new System.Drawing.Point(4, 3);
            this.lbRecordCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRecordCount.Name = "lbRecordCount";
            this.lbRecordCount.Size = new System.Drawing.Size(76, 18);
            this.lbRecordCount.TabIndex = 3;
            this.lbRecordCount.Text = "记录数：0";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.rptOrigin);
            this.groupControl1.Controls.Add(this.txtPatInstructment);
            this.groupControl1.Controls.Add(this.roundPanelGroup);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.txtBarSearchCondition);
            this.groupControl1.Controls.Add(this.labelControl7);
            this.groupControl1.Controls.Add(this.cmbBarSearchPatType);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.dtBegin);
            this.groupControl1.Controls.Add(this.txtPatType);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.dtEnd);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(365, 163);
            this.groupControl1.TabIndex = 167;
            this.groupControl1.Text = "查询";
            this.groupControl1.UseDisabledStatePainter = false;
            // 
            // rptOrigin
            // 
            this.rptOrigin.Location = new System.Drawing.Point(14, 134);
            this.rptOrigin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rptOrigin.Name = "rptOrigin";
            this.rptOrigin.Size = new System.Drawing.Size(343, 22);
            this.rptOrigin.TabIndex = 1;
            // 
            // roundPanelGroup
            // 
            this.roundPanelGroup.Location = new System.Drawing.Point(14, 111);
            this.roundPanelGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.roundPanelGroup.Name = "roundPanelGroup";
            this.roundPanelGroup.Size = new System.Drawing.Size(343, 22);
            this.roundPanelGroup.TabIndex = 0;
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // gridControl1
            // 
            this.gridControl1.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControl1.DataSource = this.bsPatList;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gridControl1.Location = new System.Drawing.Point(0, 163);
            this.gridControl1.MainView = this.gridViewPatientList;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit3,
            this.repositoryItemPictureEdit1,
            this.repositoryItemImageEdit1});
            this.gridControl1.Size = new System.Drawing.Size(365, 586);
            this.gridControl1.TabIndex = 168;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewPatientList});
            // 
            // gridViewPatientList
            // 
            this.gridViewPatientList.ActiveFilterEnabled = false;
            this.gridViewPatientList.Appearance.FocusedRow.BackColor = System.Drawing.Color.Transparent;
            this.gridViewPatientList.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridViewPatientList.Appearance.Row.Options.UseBackColor = true;
            this.gridViewPatientList.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.gridViewPatientList.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.col_icon,
            this.col_pat_sid_int,
            this.col_pat_sid,
            this.col_pat_flag});
            this.gridViewPatientList.GridControl = this.gridControl1;
            this.gridViewPatientList.Name = "gridViewPatientList";
            this.gridViewPatientList.OptionsDetail.AllowZoomDetail = false;
            this.gridViewPatientList.OptionsDetail.EnableMasterViewMode = false;
            this.gridViewPatientList.OptionsDetail.ShowDetailTabs = false;
            this.gridViewPatientList.OptionsDetail.SmartDetailExpand = false;
            this.gridViewPatientList.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
            this.gridViewPatientList.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewPatientList.OptionsSelection.MultiSelect = true;
            this.gridViewPatientList.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridViewPatientList.OptionsView.ColumnAutoWidth = false;
            this.gridViewPatientList.OptionsView.ShowGroupPanel = false;
            this.gridViewPatientList.OptionsView.ShowIndicator = false;
            // 
            // col_icon
            // 
            this.col_icon.Caption = " ";
            this.col_icon.ColumnEdit = this.repositoryItemImageEdit1;
            this.col_icon.FieldName = "pat_icon";
            this.col_icon.Name = "col_icon";
            this.col_icon.OptionsColumn.AllowEdit = false;
            this.col_icon.OptionsColumn.AllowFocus = false;
            this.col_icon.OptionsColumn.AllowMove = false;
            this.col_icon.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.True;
            this.col_icon.OptionsFilter.AllowAutoFilter = false;
            this.col_icon.OptionsFilter.AllowFilter = false;
            this.col_icon.Visible = true;
            this.col_icon.VisibleIndex = 1;
            this.col_icon.Width = 20;
            // 
            // repositoryItemImageEdit1
            // 
            this.repositoryItemImageEdit1.AllowFocused = false;
            this.repositoryItemImageEdit1.AutoHeight = false;
            this.repositoryItemImageEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageEdit1.Name = "repositoryItemImageEdit1";
            this.repositoryItemImageEdit1.ReadOnly = true;
            // 
            // col_pat_sid_int
            // 
            this.col_pat_sid_int.Caption = "样本号";
            this.col_pat_sid_int.FieldName = "RepSid";
            this.col_pat_sid_int.Name = "col_pat_sid_int";
            this.col_pat_sid_int.OptionsColumn.AllowEdit = false;
            this.col_pat_sid_int.OptionsColumn.AllowMove = false;
            this.col_pat_sid_int.OptionsFilter.AllowAutoFilter = false;
            this.col_pat_sid_int.OptionsFilter.AllowFilter = false;
            this.col_pat_sid_int.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.col_pat_sid_int.Visible = true;
            this.col_pat_sid_int.VisibleIndex = 2;
            this.col_pat_sid_int.Width = 70;
            // 
            // col_pat_sid
            // 
            this.col_pat_sid.AppearanceCell.Options.UseTextOptions = true;
            this.col_pat_sid.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.col_pat_sid.Caption = "样本号";
            this.col_pat_sid.FieldName = "RepSid";
            this.col_pat_sid.Name = "col_pat_sid";
            this.col_pat_sid.OptionsColumn.AllowEdit = false;
            this.col_pat_sid.OptionsColumn.AllowMove = false;
            this.col_pat_sid.OptionsColumn.ReadOnly = true;
            this.col_pat_sid.OptionsFilter.AllowFilter = false;
            this.col_pat_sid.SortMode = DevExpress.XtraGrid.ColumnSortMode.Custom;
            this.col_pat_sid.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            this.col_pat_sid.Width = 70;
            // 
            // col_pat_flag
            // 
            this.col_pat_flag.Caption = "pat_flag";
            this.col_pat_flag.FieldName = "RepStatus";
            this.col_pat_flag.Name = "col_pat_flag";
            this.col_pat_flag.OptionsColumn.AllowMove = false;
            this.col_pat_flag.Width = 56;
            // 
            // repositoryItemLookUpEdit3
            // 
            this.repositoryItemLookUpEdit3.AutoHeight = false;
            this.repositoryItemLookUpEdit3.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit3.DisplayMember = "value";
            this.repositoryItemLookUpEdit3.Name = "repositoryItemLookUpEdit3";
            this.repositoryItemLookUpEdit3.NullText = "";
            this.repositoryItemLookUpEdit3.ValueMember = "id";
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            // 
            // ControlPatListNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.pnlFilterBar);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ControlPatListNew";
            this.Size = new System.Drawing.Size(365, 772);
            this.Load += new System.EventHandler(this.ControlPatList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cmbBarSearchPatType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarSearchCondition.Properties)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsPatList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlFilterBar)).EndInit();
            this.pnlFilterBar.ResumeLayout(false);
            this.pnlFilterBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGridFilterPatientState.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbGridFilterPatCType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPatientList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            this.ResumeLayout(false);

        }



        #endregion
        private System.Windows.Forms.BindingSource bsPatList;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuConfig;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem 危急值记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuAllowMidReport;
        private System.Windows.Forms.ToolStripMenuItem 审核ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消审核ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打印报告ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除标本ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存信息ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem 批量添加组合ToolStripMenuItem;
        protected DevExpress.XtraEditors.PanelControl pnlFilterBar;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        public DevExpress.XtraEditors.LookUpEdit cmbBarSearchPatType;
        public DevExpress.XtraEditors.TextEdit txtBarSearchCondition;
        public DevExpress.XtraEditors.DateEdit dtEnd;
        public DevExpress.XtraEditors.DateEdit dtBegin;
        public dcl.client.control.SelectDicLabProfession txtPatType;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private System.Windows.Forms.Label lbRecordCount;
        protected DevExpress.XtraEditors.LookUpEdit cmbGridFilterPatCType;
        protected DevExpress.XtraEditors.LabelControl labelControl4;
        protected DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit cmbGridFilterPatientState;
        private System.Windows.Forms.ToolStripMenuItem 报告解读ToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem 更新为传染病toolStripMenuItem;
        public control.SelectDicInstrument txtPatInstructment;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private RoundPanelGroup roundPanelGroup;
        private RoundPanelType rptOrigin;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        public DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit3;
        public DevExpress.XtraGrid.Columns.GridColumn col_pat_flag;
        public DevExpress.XtraGrid.Columns.GridColumn col_pat_sid;
        private DevExpress.XtraGrid.Columns.GridColumn col_pat_sid_int;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageEdit repositoryItemImageEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn col_icon;
        public DevExpress.XtraGrid.Views.Grid.GridView gridViewPatientList;
        public DevExpress.XtraGrid.GridControl gridControl1;
    }
}
