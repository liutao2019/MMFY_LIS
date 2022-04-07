using lis.client.control;
namespace dcl.client.dicbasic
{
    partial class ConElisaCalcControl
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
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.bsEiasaCalc = new System.Windows.Forms.BindingSource(this.components);
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton9 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton10 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton17 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton8 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton18 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton19 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton20 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton23 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton16 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton14 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton22 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton13 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton11 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton12 = new DevExpress.XtraEditors.SimpleButton();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ilookupItem = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.bsDictItem = new System.Windows.Forms.BindingSource(this.components);
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsEiasaCalc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilookupItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDictItem)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Controls.Add(this.gridControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(757, 686);
            this.panel3.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.memoEdit1);
            this.panel1.Controls.Add(this.panelControl3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(164, 686);
            this.panel1.TabIndex = 50;
            // 
            // memoEdit1
            // 
            this.memoEdit1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bsEiasaCalc, "CalExpression", true));
            this.memoEdit1.Location = new System.Drawing.Point(19, 40);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(306, 127);
            this.memoEdit1.TabIndex = 53;
            // 
            // bsEiasaCalc
            // 
            this.bsEiasaCalc.DataSource = typeof(dcl.entity.EntityDicElisaCalu);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.simpleButton9);
            this.panelControl3.Controls.Add(this.simpleButton6);
            this.panelControl3.Controls.Add(this.simpleButton3);
            this.panelControl3.Controls.Add(this.simpleButton10);
            this.panelControl3.Controls.Add(this.simpleButton17);
            this.panelControl3.Controls.Add(this.simpleButton8);
            this.panelControl3.Controls.Add(this.simpleButton5);
            this.panelControl3.Controls.Add(this.simpleButton2);
            this.panelControl3.Controls.Add(this.simpleButton18);
            this.panelControl3.Controls.Add(this.simpleButton7);
            this.panelControl3.Controls.Add(this.simpleButton4);
            this.panelControl3.Controls.Add(this.simpleButton1);
            this.panelControl3.Controls.Add(this.simpleButton19);
            this.panelControl3.Controls.Add(this.simpleButton20);
            this.panelControl3.Controls.Add(this.simpleButton23);
            this.panelControl3.Controls.Add(this.simpleButton16);
            this.panelControl3.Controls.Add(this.simpleButton14);
            this.panelControl3.Controls.Add(this.simpleButton22);
            this.panelControl3.Controls.Add(this.simpleButton13);
            this.panelControl3.Controls.Add(this.simpleButton11);
            this.panelControl3.Controls.Add(this.simpleButton12);
            this.panelControl3.Location = new System.Drawing.Point(19, 194);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(285, 400);
            this.panelControl3.TabIndex = 52;
            // 
            // simpleButton9
            // 
            this.simpleButton9.Location = new System.Drawing.Point(19, 126);
            this.simpleButton9.Name = "simpleButton9";
            this.simpleButton9.Size = new System.Drawing.Size(71, 47);
            this.simpleButton9.TabIndex = 50;
            this.simpleButton9.Tag = "7";
            this.simpleButton9.Text = "7";
            this.simpleButton9.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Location = new System.Drawing.Point(20, 72);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(71, 47);
            this.simpleButton6.TabIndex = 50;
            this.simpleButton6.Tag = "4";
            this.simpleButton6.Text = "4";
            this.simpleButton6.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(21, 19);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(71, 47);
            this.simpleButton3.TabIndex = 50;
            this.simpleButton3.Tag = "1";
            this.simpleButton3.Text = "1";
            this.simpleButton3.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton10
            // 
            this.simpleButton10.Location = new System.Drawing.Point(20, 233);
            this.simpleButton10.Name = "simpleButton10";
            this.simpleButton10.Size = new System.Drawing.Size(71, 47);
            this.simpleButton10.TabIndex = 50;
            this.simpleButton10.Tag = "+";
            this.simpleButton10.Text = "+";
            this.simpleButton10.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton17
            // 
            this.simpleButton17.Location = new System.Drawing.Point(107, 180);
            this.simpleButton17.Name = "simpleButton17";
            this.simpleButton17.Size = new System.Drawing.Size(71, 47);
            this.simpleButton17.TabIndex = 51;
            this.simpleButton17.Tag = "0";
            this.simpleButton17.Text = "0";
            this.simpleButton17.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton8
            // 
            this.simpleButton8.Location = new System.Drawing.Point(107, 126);
            this.simpleButton8.Name = "simpleButton8";
            this.simpleButton8.Size = new System.Drawing.Size(71, 47);
            this.simpleButton8.TabIndex = 51;
            this.simpleButton8.Tag = "8";
            this.simpleButton8.Text = "8";
            this.simpleButton8.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Location = new System.Drawing.Point(108, 72);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(71, 47);
            this.simpleButton5.TabIndex = 51;
            this.simpleButton5.Tag = "5";
            this.simpleButton5.Text = "5";
            this.simpleButton5.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(110, 19);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(71, 47);
            this.simpleButton2.TabIndex = 51;
            this.simpleButton2.Tag = "2";
            this.simpleButton2.Text = "2";
            this.simpleButton2.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton18
            // 
            this.simpleButton18.Location = new System.Drawing.Point(195, 233);
            this.simpleButton18.Name = "simpleButton18";
            this.simpleButton18.Size = new System.Drawing.Size(71, 47);
            this.simpleButton18.TabIndex = 51;
            this.simpleButton18.Tag = "-";
            this.simpleButton18.Text = "-";
            this.simpleButton18.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton7
            // 
            this.simpleButton7.Location = new System.Drawing.Point(195, 126);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Size = new System.Drawing.Size(71, 47);
            this.simpleButton7.TabIndex = 52;
            this.simpleButton7.Tag = "9";
            this.simpleButton7.Text = "9";
            this.simpleButton7.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(195, 72);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(71, 47);
            this.simpleButton4.TabIndex = 52;
            this.simpleButton4.Tag = "6";
            this.simpleButton4.Text = "6";
            this.simpleButton4.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(195, 19);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(71, 47);
            this.simpleButton1.TabIndex = 52;
            this.simpleButton1.Tag = "3";
            this.simpleButton1.Text = "3";
            this.simpleButton1.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton19
            // 
            this.simpleButton19.Location = new System.Drawing.Point(19, 180);
            this.simpleButton19.Name = "simpleButton19";
            this.simpleButton19.Size = new System.Drawing.Size(71, 47);
            this.simpleButton19.TabIndex = 52;
            this.simpleButton19.Tag = "*";
            this.simpleButton19.Text = "*";
            this.simpleButton19.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton20
            // 
            this.simpleButton20.Location = new System.Drawing.Point(195, 180);
            this.simpleButton20.Name = "simpleButton20";
            this.simpleButton20.Size = new System.Drawing.Size(71, 47);
            this.simpleButton20.TabIndex = 53;
            this.simpleButton20.Tag = "/";
            this.simpleButton20.Text = "/";
            this.simpleButton20.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton23
            // 
            this.simpleButton23.Location = new System.Drawing.Point(107, 287);
            this.simpleButton23.Name = "simpleButton23";
            this.simpleButton23.Size = new System.Drawing.Size(71, 47);
            this.simpleButton23.TabIndex = 56;
            this.simpleButton23.Tag = "[空白]";
            this.simpleButton23.Text = "空白对照";
            this.simpleButton23.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton16
            // 
            this.simpleButton16.Location = new System.Drawing.Point(21, 287);
            this.simpleButton16.Name = "simpleButton16";
            this.simpleButton16.Size = new System.Drawing.Size(71, 47);
            this.simpleButton16.TabIndex = 48;
            this.simpleButton16.Tag = "[阴性]";
            this.simpleButton16.Text = "阴性";
            this.simpleButton16.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton14
            // 
            this.simpleButton14.Location = new System.Drawing.Point(195, 341);
            this.simpleButton14.Name = "simpleButton14";
            this.simpleButton14.Size = new System.Drawing.Size(71, 47);
            this.simpleButton14.TabIndex = 44;
            this.simpleButton14.Tag = "[质控H]";
            this.simpleButton14.Text = "质控H";
            this.simpleButton14.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton22
            // 
            this.simpleButton22.Location = new System.Drawing.Point(108, 233);
            this.simpleButton22.Name = "simpleButton22";
            this.simpleButton22.Size = new System.Drawing.Size(71, 47);
            this.simpleButton22.TabIndex = 55;
            this.simpleButton22.Tag = "[样本]";
            this.simpleButton22.Text = "样本";
            this.simpleButton22.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton13
            // 
            this.simpleButton13.Location = new System.Drawing.Point(20, 341);
            this.simpleButton13.Name = "simpleButton13";
            this.simpleButton13.Size = new System.Drawing.Size(71, 47);
            this.simpleButton13.TabIndex = 43;
            this.simpleButton13.Tag = "[质控L]";
            this.simpleButton13.Text = "质控L";
            this.simpleButton13.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton11
            // 
            this.simpleButton11.Location = new System.Drawing.Point(195, 287);
            this.simpleButton11.Name = "simpleButton11";
            this.simpleButton11.Size = new System.Drawing.Size(71, 47);
            this.simpleButton11.TabIndex = 41;
            this.simpleButton11.Tag = "[阳性]";
            this.simpleButton11.Text = "阳性";
            this.simpleButton11.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // simpleButton12
            // 
            this.simpleButton12.Location = new System.Drawing.Point(107, 341);
            this.simpleButton12.Name = "simpleButton12";
            this.simpleButton12.Size = new System.Drawing.Size(71, 47);
            this.simpleButton12.TabIndex = 42;
            this.simpleButton12.Tag = "[质控M]";
            this.simpleButton12.Text = "质控M";
            this.simpleButton12.Click += new System.EventHandler(this.CalcButton_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.bsEiasaCalc;
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.gridControl1.Location = new System.Drawing.Point(164, 0);
            this.gridControl1.MainView = this.gridView3;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ilookupItem});
            this.gridControl1.Size = new System.Drawing.Size(593, 686);
            this.gridControl1.TabIndex = 49;
            this.gridControl1.TabStop = false;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView3});
            // 
            // gridView3
            // 
            this.gridView3.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11});
            this.gridView3.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridView3.GridControl = this.gridControl1;
            this.gridView3.Name = "gridView3";
            this.gridView3.OptionsFind.AlwaysVisible = true;
            this.gridView3.OptionsFind.FindNullPrompt = "请输入关键字进行查询！";
            this.gridView3.OptionsView.ColumnAutoWidth = false;
            this.gridView3.OptionsView.ShowFooter = true;
            this.gridView3.OptionsView.ShowGroupPanel = false;
            this.gridView3.OptionsView.ShowIndicator = false;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "编号";
            this.gridColumn9.FieldName = "CalId";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsColumn.FixedWidth = true;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 0;
            this.gridColumn9.Width = 77;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "计算项目";
            this.gridColumn10.ColumnEdit = this.ilookupItem;
            this.gridColumn10.FieldName = "CalItmId";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.FixedWidth = true;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 1;
            this.gridColumn10.Width = 86;
            // 
            // ilookupItem
            // 
            this.ilookupItem.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ilookupItem.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmId", 200, "编号"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmEcode", 200, "项目代码"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ItmName", 400, "名称")});
            this.ilookupItem.DataSource = this.bsDictItem;
            this.ilookupItem.DisplayMember = "ItmEcode";
            this.ilookupItem.Name = "ilookupItem";
            this.ilookupItem.NullText = "";
            this.ilookupItem.ValueMember = "ItmId";
            // 
            // bsDictItem
            // 
            this.bsDictItem.DataSource = typeof(dcl.entity.EntityDicItmItem);
            this.bsDictItem.Filter = "";
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "计算公式";
            this.gridColumn11.FieldName = "CalExpression";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "CalExpression", "记录总数:{0:0.##}")});
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 2;
            this.gridColumn11.Width = 146;
            // 
            // ConElisaCalcControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel3);
            this.Name = "ConElisaCalcControl";
            this.Size = new System.Drawing.Size(757, 686);
            this.Load += new System.EventHandler(this.ConElisaCalcControl_Load);
            this.panel3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsEiasaCalc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ilookupItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDictItem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.BindingSource bsEiasaCalc;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private System.Windows.Forms.BindingSource bsDictItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit ilookupItem;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton10;
        private DevExpress.XtraEditors.SimpleButton simpleButton18;
        private DevExpress.XtraEditors.SimpleButton simpleButton19;
        private DevExpress.XtraEditors.SimpleButton simpleButton20;
        private DevExpress.XtraEditors.SimpleButton simpleButton23;
        private DevExpress.XtraEditors.SimpleButton simpleButton16;
        private DevExpress.XtraEditors.SimpleButton simpleButton14;
        private DevExpress.XtraEditors.SimpleButton simpleButton22;
        private DevExpress.XtraEditors.SimpleButton simpleButton13;
        private DevExpress.XtraEditors.SimpleButton simpleButton11;
        private DevExpress.XtraEditors.SimpleButton simpleButton12;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton9;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton17;
        private DevExpress.XtraEditors.SimpleButton simpleButton8;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;

    }
}
