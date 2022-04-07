namespace dcl.client.dicbasic
{
    partial class ConNobact
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
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private void InitializeComponent()
        {
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.cbIsPublic = new System.Windows.Forms.CheckBox();
            this.bsNobact = new System.Windows.Forms.BindingSource();
            this.label11 = new System.Windows.Forms.Label();
            this.panelComIDs = new System.Windows.Forms.Panel();
            this.gcApparatus = new DevExpress.XtraGrid.GridControl();
            this.bindingSourceCom = new System.Windows.Forms.BindingSource();
            this.gvApparatus = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colitm_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colcom_itm_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.lookUpEditNobactType = new DevExpress.XtraEditors.LookUpEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNob_cname = new DevExpress.XtraEditors.MemoEdit();
            this.txt_wb = new DevExpress.XtraEditors.TextEdit();
            this.txt_py = new DevExpress.XtraEditors.TextEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSeq = new DevExpress.XtraEditors.SpinEdit();
            this.lookUpEdit_reg_flag = new DevExpress.XtraEditors.LookUpEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNob_id = new DevExpress.XtraEditors.TextEdit();
            this.selectDict_Type = new dcl.client.control.SelectDicPubProfession();
            this.selectDict_Checkb1 = new dcl.client.control.SelectDicCheckPurpose();
            this.txtIncode = new DevExpress.XtraEditors.TextEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.label13 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colnob_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colnob_cname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colnob_type_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colnob_incode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colnob_py = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colnob_wb = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsNobact)).BeginInit();
            this.panelComIDs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcApparatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvApparatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditNobactType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNob_cname.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_wb.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_py.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeq.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_reg_flag.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNob_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIncode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // cbIsPublic
            // 
            this.cbIsPublic.AutoSize = true;
            this.cbIsPublic.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.bsNobact, "SmePublic", true));
            this.cbIsPublic.Location = new System.Drawing.Point(121, 445);
            this.cbIsPublic.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cbIsPublic.Name = "cbIsPublic";
            this.cbIsPublic.Size = new System.Drawing.Size(75, 22);
            this.cbIsPublic.TabIndex = 169;
            this.cbIsPublic.Text = "是公用";
            this.cbIsPublic.UseVisualStyleBackColor = true;
            // 
            // bsNobact
            // 
            this.bsNobact.DataSource = typeof(dcl.entity.EntityDicMicSmear);
            this.bsNobact.PositionChanged += new System.EventHandler(this.bsNobact_PositionChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(77, 445);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 18);
            this.label11.TabIndex = 168;
            this.label11.Text = "公用";
            // 
            // panelComIDs
            // 
            this.panelComIDs.Controls.Add(this.gcApparatus);
            this.panelComIDs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelComIDs.Location = new System.Drawing.Point(2, 27);
            this.panelComIDs.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelComIDs.Name = "panelComIDs";
            this.panelComIDs.Size = new System.Drawing.Size(400, 357);
            this.panelComIDs.TabIndex = 167;
            // 
            // gcApparatus
            // 
            this.gcApparatus.AllowDrop = true;
            this.gcApparatus.DataSource = this.bindingSourceCom;
            this.gcApparatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcApparatus.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcApparatus.Location = new System.Drawing.Point(0, 0);
            this.gcApparatus.MainView = this.gvApparatus;
            this.gcApparatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcApparatus.Name = "gcApparatus";
            this.gcApparatus.Size = new System.Drawing.Size(400, 357);
            this.gcApparatus.TabIndex = 2;
            this.gcApparatus.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvApparatus,
            this.gridView2});
            // 
            // bindingSourceCom
            // 
            this.bindingSourceCom.DataSource = typeof(dcl.entity.EntityDicCombine);
            // 
            // gvApparatus
            // 
            this.gvApparatus.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.colitm_name,
            this.colcom_itm_id});
            this.gvApparatus.GridControl = this.gcApparatus;
            this.gvApparatus.Name = "gvApparatus";
            this.gvApparatus.OptionsBehavior.AllowIncrementalSearch = true;
            this.gvApparatus.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvApparatus.OptionsSelection.MultiSelect = true;
            this.gvApparatus.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.SmartTag;
            this.gvApparatus.OptionsView.ShowAutoFilterRow = true;
            this.gvApparatus.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn4.FieldName = "Checked";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.ShowCaption = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            this.gridColumn4.Width = 20;
            // 
            // colitm_name
            // 
            this.colitm_name.Caption = "组合名称";
            this.colitm_name.FieldName = "ComName";
            this.colitm_name.Name = "colitm_name";
            this.colitm_name.OptionsColumn.AllowEdit = false;
            this.colitm_name.Visible = true;
            this.colitm_name.VisibleIndex = 2;
            this.colitm_name.Width = 157;
            // 
            // colcom_itm_id
            // 
            this.colcom_itm_id.Caption = "编号";
            this.colcom_itm_id.FieldName = "ComId";
            this.colcom_itm_id.Name = "colcom_itm_id";
            this.colcom_itm_id.OptionsColumn.AllowEdit = false;
            this.colcom_itm_id.Visible = true;
            this.colcom_itm_id.VisibleIndex = 1;
            this.colcom_itm_id.Width = 61;
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.gcApparatus;
            this.gridView2.Name = "gridView2";
            // 
            // lookUpEditNobactType
            // 
            this.lookUpEditNobactType.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsNobact, "SmeClass", true));
            this.lookUpEditNobactType.Location = new System.Drawing.Point(121, 410);
            this.lookUpEditNobactType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lookUpEditNobactType.Name = "lookUpEditNobactType";
            this.lookUpEditNobactType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditNobactType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "编码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("name", "名称")});
            this.lookUpEditNobactType.Properties.NullText = "";
            this.lookUpEditNobactType.Size = new System.Drawing.Size(267, 24);
            this.lookUpEditNobactType.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(77, 411);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 18);
            this.label8.TabIndex = 24;
            this.label8.Text = "类型";
            // 
            // txtNob_cname
            // 
            this.txtNob_cname.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsNobact, "SmeName", true));
            this.txtNob_cname.Location = new System.Drawing.Point(121, 73);
            this.txtNob_cname.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNob_cname.Name = "txtNob_cname";
            this.txtNob_cname.Size = new System.Drawing.Size(267, 99);
            this.txtNob_cname.TabIndex = 23;
            this.txtNob_cname.Leave += new System.EventHandler(this.txtNob_cname_Leave);
            // 
            // txt_wb
            // 
            this.txt_wb.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsNobact, "SmeWbCode", true));
            this.txt_wb.Enabled = false;
            this.txt_wb.Location = new System.Drawing.Point(121, 378);
            this.txt_wb.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_wb.Name = "txt_wb";
            this.txt_wb.Size = new System.Drawing.Size(267, 24);
            this.txt_wb.TabIndex = 22;
            this.txt_wb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Editor_KeyDown);
            // 
            // txt_py
            // 
            this.txt_py.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsNobact, "SmePyCode", true));
            this.txt_py.Enabled = false;
            this.txt_py.Location = new System.Drawing.Point(121, 346);
            this.txt_py.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt_py.Name = "txt_py";
            this.txt_py.Size = new System.Drawing.Size(267, 24);
            this.txt_py.TabIndex = 21;
            this.txt_py.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Editor_KeyDown);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(62, 347);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 18);
            this.label10.TabIndex = 20;
            this.label10.Text = "拼音码";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(62, 380);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 18);
            this.label9.TabIndex = 19;
            this.label9.Text = "五笔码";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(77, 316);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 18);
            this.label7.TabIndex = 17;
            this.label7.Text = "顺序";
            // 
            // txtSeq
            // 
            this.txtSeq.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsNobact, "SmeSortNo", true));
            this.txtSeq.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txtSeq.Location = new System.Drawing.Point(121, 314);
            this.txtSeq.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtSeq.Size = new System.Drawing.Size(267, 24);
            this.txtSeq.TabIndex = 16;
            this.txtSeq.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Editor_KeyDown);
            // 
            // lookUpEdit_reg_flag
            // 
            this.lookUpEdit_reg_flag.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsNobact, "SmePositiveFlag", true));
            this.lookUpEdit_reg_flag.Location = new System.Drawing.Point(121, 282);
            this.lookUpEdit_reg_flag.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lookUpEdit_reg_flag.Name = "lookUpEdit_reg_flag";
            this.lookUpEdit_reg_flag.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEdit_reg_flag.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("id", "编码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("name", "名称")});
            this.lookUpEdit_reg_flag.Properties.NullText = "";
            this.lookUpEdit_reg_flag.Size = new System.Drawing.Size(267, 24);
            this.lookUpEdit_reg_flag.TabIndex = 15;
            this.lookUpEdit_reg_flag.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Editor_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 283);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 18);
            this.label6.TabIndex = 14;
            this.label6.Text = "阳性标志";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 247);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 18);
            this.label5.TabIndex = 13;
            this.label5.Text = "默认检查目的";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 216);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 18);
            this.label4.TabIndex = 12;
            this.label4.Text = "输入码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(77, 180);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 18);
            this.label3.TabIndex = 11;
            this.label3.Text = "组别";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 10;
            this.label2.Text = "涂片结果";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 18);
            this.label1.TabIndex = 9;
            this.label1.Text = "编码";
            // 
            // txtNob_id
            // 
            this.txtNob_id.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsNobact, "SmeId", true));
            this.txtNob_id.Enabled = false;
            this.txtNob_id.Location = new System.Drawing.Point(121, 41);
            this.txtNob_id.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNob_id.Name = "txtNob_id";
            this.txtNob_id.Size = new System.Drawing.Size(267, 24);
            this.txtNob_id.TabIndex = 4;
            // 
            // selectDict_Type
            // 
            this.selectDict_Type.AddEmptyRow = true;
            this.selectDict_Type.AutoScroll = true;
            this.selectDict_Type.BindByValue = true;
            this.selectDict_Type.colDisplay = "";
            this.selectDict_Type.colExtend1 = null;
            this.selectDict_Type.colInCode = "";
            this.selectDict_Type.colPY = "";
            this.selectDict_Type.colSeq = "";
            this.selectDict_Type.colValue = "";
            this.selectDict_Type.colWB = "";
            this.selectDict_Type.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsNobact, "SmeType", true));
            this.selectDict_Type.displayMember = "";
            this.selectDict_Type.EnterMoveNext = true;
            this.selectDict_Type.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDict_Type.KeyUpDownMoveNext = false;
            this.selectDict_Type.LoadDataOnDesignMode = true;
            this.selectDict_Type.Location = new System.Drawing.Point(121, 180);
            this.selectDict_Type.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectDict_Type.MaximumSize = new System.Drawing.Size(571, 27);
            this.selectDict_Type.MinimumSize = new System.Drawing.Size(57, 27);
            this.selectDict_Type.Name = "selectDict_Type";
            this.selectDict_Type.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDict_Type.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDict_Type.Readonly = false;
            this.selectDict_Type.SaveSourceID = false;
            this.selectDict_Type.SelectFilter = null;
            this.selectDict_Type.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDict_Type.SelectOnly = true;
            this.selectDict_Type.Size = new System.Drawing.Size(267, 27);
            this.selectDict_Type.TabIndex = 6;
            this.selectDict_Type.UseExtend = false;
            this.selectDict_Type.valueMember = "";
            // 
            // selectDict_Checkb1
            // 
            this.selectDict_Checkb1.AddEmptyRow = true;
            this.selectDict_Checkb1.AutoScroll = true;
            this.selectDict_Checkb1.BindByValue = false;
            this.selectDict_Checkb1.colDisplay = "";
            this.selectDict_Checkb1.colExtend1 = null;
            this.selectDict_Checkb1.colInCode = "";
            this.selectDict_Checkb1.colPY = "";
            this.selectDict_Checkb1.colSeq = "";
            this.selectDict_Checkb1.colValue = "";
            this.selectDict_Checkb1.colWB = "";
            this.selectDict_Checkb1.DataBindings.Add(new System.Windows.Forms.Binding("valueMember", this.bsNobact, "SmePurpId", true));
            this.selectDict_Checkb1.displayMember = null;
            this.selectDict_Checkb1.EnterMoveNext = true;
            this.selectDict_Checkb1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDict_Checkb1.KeyUpDownMoveNext = false;
            this.selectDict_Checkb1.LoadDataOnDesignMode = true;
            this.selectDict_Checkb1.Location = new System.Drawing.Point(121, 247);
            this.selectDict_Checkb1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectDict_Checkb1.MaximumSize = new System.Drawing.Size(571, 27);
            this.selectDict_Checkb1.MinimumSize = new System.Drawing.Size(57, 27);
            this.selectDict_Checkb1.Name = "selectDict_Checkb1";
            this.selectDict_Checkb1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDict_Checkb1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDict_Checkb1.Readonly = false;
            this.selectDict_Checkb1.SaveSourceID = false;
            this.selectDict_Checkb1.SelectFilter = null;
            this.selectDict_Checkb1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDict_Checkb1.SelectOnly = true;
            this.selectDict_Checkb1.Size = new System.Drawing.Size(267, 27);
            this.selectDict_Checkb1.TabIndex = 8;
            this.selectDict_Checkb1.UseExtend = false;
            this.selectDict_Checkb1.valueMember = null;
            // 
            // txtIncode
            // 
            this.txtIncode.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bsNobact, "SmeCCode", true));
            this.txtIncode.Location = new System.Drawing.Point(121, 215);
            this.txtIncode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtIncode.Name = "txtIncode";
            this.txtIncode.Size = new System.Drawing.Size(267, 24);
            this.txtIncode.TabIndex = 7;
            this.txtIncode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Editor_KeyDown);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.label13);
            this.panelControl1.Controls.Add(this.txtFilter);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(716, 47);
            this.panelControl1.TabIndex = 0;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(22, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(68, 18);
            this.label13.TabIndex = 15;
            this.label13.Text = "数据检索";
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(92, 13);
            this.txtFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(156, 26);
            this.txtFilter.TabIndex = 14;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.Panel2;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl2);
            this.splitContainerControl1.Panel1.Controls.Add(this.panelControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl3);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1126, 861);
            this.splitContainerControl1.SplitterPosition = 404;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gridControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 47);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(716, 814);
            this.panelControl2.TabIndex = 2;
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsNobact;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(712, 810);
            this.gridControl1.TabIndex = 48;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colnob_id,
            this.colnob_cname,
            this.colnob_type_name,
            this.colnob_incode,
            this.colnob_py,
            this.colnob_wb});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // colnob_id
            // 
            this.colnob_id.Caption = "编码";
            this.colnob_id.FieldName = "SmeId";
            this.colnob_id.Name = "colnob_id";
            this.colnob_id.Visible = true;
            this.colnob_id.VisibleIndex = 0;
            this.colnob_id.Width = 47;
            // 
            // colnob_cname
            // 
            this.colnob_cname.Caption = "涂片结果";
            this.colnob_cname.FieldName = "SmeName";
            this.colnob_cname.Name = "colnob_cname";
            this.colnob_cname.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "SmeName", "记录总数:{0:0.##}")});
            this.colnob_cname.Visible = true;
            this.colnob_cname.VisibleIndex = 1;
            this.colnob_cname.Width = 119;
            // 
            // colnob_type_name
            // 
            this.colnob_type_name.Caption = "组别";
            this.colnob_type_name.FieldName = "SmeProName";
            this.colnob_type_name.Name = "colnob_type_name";
            this.colnob_type_name.Visible = true;
            this.colnob_type_name.VisibleIndex = 2;
            // 
            // colnob_incode
            // 
            this.colnob_incode.Caption = "输入码";
            this.colnob_incode.FieldName = "SmeCCode";
            this.colnob_incode.Name = "colnob_incode";
            this.colnob_incode.Visible = true;
            this.colnob_incode.VisibleIndex = 3;
            this.colnob_incode.Width = 61;
            // 
            // colnob_py
            // 
            this.colnob_py.Caption = "拼音码";
            this.colnob_py.FieldName = "SmePyCode";
            this.colnob_py.Name = "colnob_py";
            this.colnob_py.Visible = true;
            this.colnob_py.VisibleIndex = 4;
            this.colnob_py.Width = 61;
            // 
            // colnob_wb
            // 
            this.colnob_wb.Caption = "五笔码";
            this.colnob_wb.FieldName = "SmeWbCode";
            this.colnob_wb.Name = "colnob_wb";
            this.colnob_wb.Visible = true;
            this.colnob_wb.VisibleIndex = 5;
            this.colnob_wb.Width = 67;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.panelComIDs);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 475);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(404, 386);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "可使用组合";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.label1);
            this.groupControl3.Controls.Add(this.cbIsPublic);
            this.groupControl3.Controls.Add(this.txtSeq);
            this.groupControl3.Controls.Add(this.txtIncode);
            this.groupControl3.Controls.Add(this.lookUpEdit_reg_flag);
            this.groupControl3.Controls.Add(this.label11);
            this.groupControl3.Controls.Add(this.label7);
            this.groupControl3.Controls.Add(this.selectDict_Checkb1);
            this.groupControl3.Controls.Add(this.label6);
            this.groupControl3.Controls.Add(this.selectDict_Type);
            this.groupControl3.Controls.Add(this.label5);
            this.groupControl3.Controls.Add(this.lookUpEditNobactType);
            this.groupControl3.Controls.Add(this.label10);
            this.groupControl3.Controls.Add(this.txtNob_id);
            this.groupControl3.Controls.Add(this.label4);
            this.groupControl3.Controls.Add(this.label9);
            this.groupControl3.Controls.Add(this.txt_py);
            this.groupControl3.Controls.Add(this.label8);
            this.groupControl3.Controls.Add(this.label3);
            this.groupControl3.Controls.Add(this.txtNob_cname);
            this.groupControl3.Controls.Add(this.label2);
            this.groupControl3.Controls.Add(this.txt_wb);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl3.Location = new System.Drawing.Point(0, 0);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(404, 475);
            this.groupControl3.TabIndex = 4;
            this.groupControl3.Text = "基本信息";
            // 
            // ConNobact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.Controls.Add(this.splitContainerControl1);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ConNobact";
            this.Size = new System.Drawing.Size(1126, 861);
            this.Load += new System.EventHandler(this.ConNobact_Load);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsNobact)).EndInit();
            this.panelComIDs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcApparatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvApparatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditNobactType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNob_cname.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_wb.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_py.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSeq.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEdit_reg_flag.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNob_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtIncode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource bsNobact;
        private DevExpress.XtraEditors.TextEdit txtNob_id;
        private dcl.client.control.SelectDicPubProfession selectDict_Type;
        private DevExpress.XtraEditors.TextEdit txtIncode;
        private dcl.client.control.SelectDicCheckPurpose selectDict_Checkb1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.LookUpEdit lookUpEdit_reg_flag;
        private DevExpress.XtraEditors.SpinEdit txtSeq;
        private DevExpress.XtraEditors.TextEdit txt_wb;
        private DevExpress.XtraEditors.TextEdit txt_py;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.MemoEdit txtNob_cname;
        private DevExpress.XtraEditors.LookUpEdit lookUpEditNobactType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.BindingSource bindingSourceCom;
        private System.Windows.Forms.Panel panelComIDs;
        private System.Windows.Forms.CheckBox cbIsPublic;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtFilter;
        private DevExpress.XtraGrid.GridControl gcApparatus;
        private DevExpress.XtraGrid.Views.Grid.GridView gvApparatus;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn colitm_name;
        private DevExpress.XtraGrid.Columns.GridColumn colcom_itm_id;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colnob_wb;
        private DevExpress.XtraGrid.Columns.GridColumn colnob_py;
        private DevExpress.XtraGrid.Columns.GridColumn colnob_incode;
        private DevExpress.XtraGrid.Columns.GridColumn colnob_type_name;
        private DevExpress.XtraGrid.Columns.GridColumn colnob_cname;
        private DevExpress.XtraGrid.Columns.GridColumn colnob_id;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}
