namespace dcl.client.dicbasic
{
    partial class ConIcdCombine
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gcIcd = new DevExpress.XtraGrid.GridControl();
            this.bsIcd = new System.Windows.Forms.BindingSource();
            this.gvIcd = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIcdId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIcdName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.gcItemIn = new DevExpress.XtraGrid.GridControl();
            this.bsItemIn = new System.Windows.Forms.BindingSource();
            this.gvItemIn = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colComId1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComName1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.splitContainerControl3 = new DevExpress.XtraEditors.SplitContainerControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.btnDown = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelAllUser = new DevExpress.XtraEditors.SimpleButton();
            this.btnUp = new DevExpress.XtraEditors.SimpleButton();
            this.btnDelUser = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddUser = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddAllUser = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.gcItemNotIn = new DevExpress.XtraGrid.GridControl();
            this.bsItemNotIn = new System.Windows.Forms.BindingSource();
            this.gvItemNotIn = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colComId = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colComName = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcIcd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsIcd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvIcd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcItemIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItemIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItemIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl3)).BeginInit();
            this.splitContainerControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcItemNotIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItemNotIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItemNotIn)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.gcIcd);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(1328, 862);
            this.splitContainerControl1.SplitterPosition = 476;
            this.splitContainerControl1.TabIndex = 0;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gcIcd
            // 
            this.gcIcd.DataSource = this.bsIcd;
            this.gcIcd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcIcd.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcIcd.Location = new System.Drawing.Point(0, 0);
            this.gcIcd.MainView = this.gvIcd;
            this.gcIcd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcIcd.Name = "gcIcd";
            this.gcIcd.Size = new System.Drawing.Size(381, 862);
            this.gcIcd.TabIndex = 0;
            this.gcIcd.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvIcd});
            // 
            // bsIcd
            // 
            this.bsIcd.DataSource = typeof(dcl.entity.EntityDicPubIcd);
            this.bsIcd.CurrentChanged += new System.EventHandler(this.bsIcd_CurrentChanged);
            // 
            // gvIcd
            // 
            this.gvIcd.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIcdId,
            this.colIcdName});
            this.gvIcd.GridControl = this.gcIcd;
            this.gvIcd.Name = "gvIcd";
            this.gvIcd.OptionsFind.AlwaysVisible = true;
            this.gvIcd.OptionsFind.FindNullPrompt = "请输入关键字进行查询！";
            this.gvIcd.OptionsView.ShowFooter = true;
            this.gvIcd.OptionsView.ShowGroupPanel = false;
            this.gvIcd.OptionsView.ShowIndicator = false;
            this.gvIcd.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvIcd_FocusedRowChanged);
            // 
            // colIcdId
            // 
            this.colIcdId.Caption = "编码";
            this.colIcdId.FieldName = "IcdId";
            this.colIcdId.Name = "colIcdId";
            this.colIcdId.OptionsColumn.ReadOnly = true;
            this.colIcdId.Visible = true;
            this.colIcdId.VisibleIndex = 0;
            this.colIcdId.Width = 67;
            // 
            // colIcdName
            // 
            this.colIcdName.Caption = "诊断名称";
            this.colIcdName.FieldName = "IcdName";
            this.colIcdName.Name = "colIcdName";
            this.colIcdName.OptionsColumn.ReadOnly = true;
            this.colIcdName.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "IcdName", "记录总数:{0:0.##}")});
            this.colIcdName.Visible = true;
            this.colIcdName.VisibleIndex = 1;
            this.colIcdName.Width = 294;
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.groupControl1);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.splitContainerControl3);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(941, 862);
            this.splitContainerControl2.SplitterPosition = 469;
            this.splitContainerControl2.TabIndex = 0;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.gcItemIn);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(375, 862);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "已包含组合";
            // 
            // gcItemIn
            // 
            this.gcItemIn.AllowDrop = true;
            this.gcItemIn.DataSource = this.bsItemIn;
            this.gcItemIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcItemIn.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcItemIn.Location = new System.Drawing.Point(2, 27);
            this.gcItemIn.MainView = this.gvItemIn;
            this.gcItemIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcItemIn.Name = "gcItemIn";
            this.gcItemIn.Size = new System.Drawing.Size(371, 833);
            this.gcItemIn.TabIndex = 0;
            this.gcItemIn.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvItemIn});
            this.gcItemIn.DragOver += new System.Windows.Forms.DragEventHandler(this.gcItemIn_DragOver);
            this.gcItemIn.DoubleClick += new System.EventHandler(this.gcItemIn_DoubleClick);
            // 
            // bsItemIn
            // 
            this.bsItemIn.DataSource = typeof(dcl.entity.EntityDicCombine);
            // 
            // gvItemIn
            // 
            this.gvItemIn.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colComId1,
            this.colComName1});
            this.gvItemIn.GridControl = this.gcItemIn;
            this.gvItemIn.Name = "gvItemIn";
            this.gvItemIn.OptionsView.ShowFooter = true;
            this.gvItemIn.OptionsView.ShowGroupPanel = false;
            this.gvItemIn.OptionsView.ShowIndicator = false;
            // 
            // colComId1
            // 
            this.colComId1.Caption = "编码";
            this.colComId1.FieldName = "ComId";
            this.colComId1.Name = "colComId1";
            this.colComId1.Visible = true;
            this.colComId1.VisibleIndex = 0;
            this.colComId1.Width = 62;
            // 
            // colComName1
            // 
            this.colComName1.Caption = "组合项目";
            this.colComName1.FieldName = "ComName";
            this.colComName1.Name = "colComName1";
            this.colComName1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "ComName", "记录总数:{0:0.##}")});
            this.colComName1.Visible = true;
            this.colComName1.VisibleIndex = 1;
            this.colComName1.Width = 258;
            // 
            // splitContainerControl3
            // 
            this.splitContainerControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl3.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainerControl3.Name = "splitContainerControl3";
            this.splitContainerControl3.Panel1.Controls.Add(this.groupControl2);
            this.splitContainerControl3.Panel1.Text = "Panel1";
            this.splitContainerControl3.Panel2.Controls.Add(this.groupControl3);
            this.splitContainerControl3.Panel2.Text = "Panel2";
            this.splitContainerControl3.Size = new System.Drawing.Size(560, 862);
            this.splitContainerControl3.SplitterPosition = 75;
            this.splitContainerControl3.TabIndex = 0;
            this.splitContainerControl3.Text = "splitContainerControl3";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.btnDown);
            this.groupControl2.Controls.Add(this.btnDelAllUser);
            this.groupControl2.Controls.Add(this.btnUp);
            this.groupControl2.Controls.Add(this.btnDelUser);
            this.groupControl2.Controls.Add(this.btnAddUser);
            this.groupControl2.Controls.Add(this.btnAddAllUser);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(60, 862);
            this.groupControl2.TabIndex = 0;
            this.groupControl2.Text = "操作";
            // 
            // btnDown
            // 
            this.btnDown.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnDown.Location = new System.Drawing.Point(5, 444);
            this.btnDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(50, 30);
            this.btnDown.TabIndex = 11;
            this.btnDown.Text = "↓";
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnDelAllUser
            // 
            this.btnDelAllUser.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnDelAllUser.Location = new System.Drawing.Point(5, 357);
            this.btnDelAllUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelAllUser.Name = "btnDelAllUser";
            this.btnDelAllUser.Size = new System.Drawing.Size(50, 30);
            this.btnDelAllUser.TabIndex = 9;
            this.btnDelAllUser.Text = ">>";
            this.btnDelAllUser.Click += new System.EventHandler(this.btnDelAllUser_Click);
            // 
            // btnUp
            // 
            this.btnUp.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnUp.Location = new System.Drawing.Point(5, 400);
            this.btnUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(50, 30);
            this.btnUp.TabIndex = 10;
            this.btnUp.Text = "↑";
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDelUser
            // 
            this.btnDelUser.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnDelUser.Location = new System.Drawing.Point(5, 312);
            this.btnDelUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelUser.Name = "btnDelUser";
            this.btnDelUser.Size = new System.Drawing.Size(50, 30);
            this.btnDelUser.TabIndex = 8;
            this.btnDelUser.Text = ">";
            this.btnDelUser.Click += new System.EventHandler(this.btnDelUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnAddUser.Location = new System.Drawing.Point(5, 269);
            this.btnAddUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(50, 30);
            this.btnAddUser.TabIndex = 7;
            this.btnAddUser.Text = "<";
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);
            // 
            // btnAddAllUser
            // 
            this.btnAddAllUser.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnAddAllUser.Location = new System.Drawing.Point(5, 226);
            this.btnAddAllUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddAllUser.Name = "btnAddAllUser";
            this.btnAddAllUser.Size = new System.Drawing.Size(50, 30);
            this.btnAddAllUser.TabIndex = 6;
            this.btnAddAllUser.Text = "<<";
            this.btnAddAllUser.Click += new System.EventHandler(this.btnAddAllUser_Click);
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.gcItemNotIn);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl3.Location = new System.Drawing.Point(0, 0);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(494, 862);
            this.groupControl3.TabIndex = 0;
            this.groupControl3.Text = "未包含组合";
            // 
            // gcItemNotIn
            // 
            this.gcItemNotIn.AllowDrop = true;
            this.gcItemNotIn.DataSource = this.bsItemNotIn;
            this.gcItemNotIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcItemNotIn.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcItemNotIn.Location = new System.Drawing.Point(2, 27);
            this.gcItemNotIn.MainView = this.gvItemNotIn;
            this.gcItemNotIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gcItemNotIn.Name = "gcItemNotIn";
            this.gcItemNotIn.Size = new System.Drawing.Size(490, 833);
            this.gcItemNotIn.TabIndex = 1;
            this.gcItemNotIn.UseDisabledStatePainter = false;
            this.gcItemNotIn.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvItemNotIn});
            this.gcItemNotIn.DragOver += new System.Windows.Forms.DragEventHandler(this.gcItemNotIn_DragOver);
            this.gcItemNotIn.DoubleClick += new System.EventHandler(this.gcItemNotIn_DoubleClick);
            this.gcItemNotIn.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gcItemNotIn_MouseMove);
            // 
            // bsItemNotIn
            // 
            this.bsItemNotIn.DataSource = typeof(dcl.entity.EntityDicCombine);
            // 
            // gvItemNotIn
            // 
            this.gvItemNotIn.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colComId,
            this.colComName});
            this.gvItemNotIn.GridControl = this.gcItemNotIn;
            this.gvItemNotIn.Name = "gvItemNotIn";
            this.gvItemNotIn.OptionsFind.AlwaysVisible = true;
            this.gvItemNotIn.OptionsView.ShowFooter = true;
            this.gvItemNotIn.OptionsView.ShowGroupPanel = false;
            this.gvItemNotIn.OptionsView.ShowIndicator = false;
            // 
            // colComId
            // 
            this.colComId.Caption = "编码";
            this.colComId.FieldName = "ComId";
            this.colComId.Name = "colComId";
            this.colComId.Visible = true;
            this.colComId.VisibleIndex = 0;
            this.colComId.Width = 63;
            // 
            // colComName
            // 
            this.colComName.Caption = "组合名称";
            this.colComName.FieldName = "ComName";
            this.colComName.Name = "colComName";
            this.colComName.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "ComName", "记录总数:{0:0.##}")});
            this.colComName.Visible = true;
            this.colComName.VisibleIndex = 1;
            this.colComName.Width = 478;
            // 
            // ConIcdCombine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerControl1);
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "ConIcdCombine";
            this.Size = new System.Drawing.Size(1328, 862);
            this.Load += new System.EventHandler(this.ConIcdCombine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcIcd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsIcd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvIcd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcItemIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItemIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItemIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl3)).EndInit();
            this.splitContainerControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcItemNotIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsItemNotIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvItemNotIn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gcIcd;
        private DevExpress.XtraGrid.Views.Grid.GridView gvIcd;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.GridControl gcItemIn;
        private DevExpress.XtraGrid.Views.Grid.GridView gvItemIn;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl3;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraGrid.GridControl gcItemNotIn;
        private DevExpress.XtraGrid.Views.Grid.GridView gvItemNotIn;
        private System.Windows.Forms.BindingSource bsIcd;
        private System.Windows.Forms.BindingSource bsItemIn;
        private System.Windows.Forms.BindingSource bsItemNotIn;
        private DevExpress.XtraGrid.Columns.GridColumn colIcdId;
        private DevExpress.XtraGrid.Columns.GridColumn colIcdName;
        private DevExpress.XtraGrid.Columns.GridColumn colComId1;
        private DevExpress.XtraGrid.Columns.GridColumn colComName1;
        private DevExpress.XtraGrid.Columns.GridColumn colComId;
        private DevExpress.XtraGrid.Columns.GridColumn colComName;
        private DevExpress.XtraEditors.SimpleButton btnDown;
        private DevExpress.XtraEditors.SimpleButton btnDelAllUser;
        private DevExpress.XtraEditors.SimpleButton btnUp;
        private DevExpress.XtraEditors.SimpleButton btnDelUser;
        private DevExpress.XtraEditors.SimpleButton btnAddUser;
        private DevExpress.XtraEditors.SimpleButton btnAddAllUser;
    }
}
