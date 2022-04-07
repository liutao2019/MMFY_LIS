namespace dcl.client.result.PatControl
{
    partial class FrmItemCompare
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmItemCompare));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new DevExpress.XtraEditors.SimpleButton();
            this.btnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtSID = new DevExpress.XtraEditors.TextEdit();
            this.txtDate = new DevExpress.XtraEditors.DateEdit();
            this.lblTime = new DevExpress.XtraEditors.LabelControl();
            this.txtItr = new dcl.client.control.SelectDicInstrument();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.gcData = new DevExpress.XtraGrid.GridControl();
            this.gridViewPatientList = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPatientList)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.labelControl2);
            this.panel1.Controls.Add(this.txtSID);
            this.panel1.Controls.Add(this.txtDate);
            this.panel1.Controls.Add(this.lblTime);
            this.panel1.Controls.Add(this.txtItr);
            this.panel1.Controls.Add(this.labelControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1822, 108);
            this.panel1.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnExport.Location = new System.Drawing.Point(1543, 17);
            this.btnExport.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(193, 64);
            this.btnExport.TabIndex = 16;
            this.btnExport.Text = "导出比对结果";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnRefresh.Location = new System.Drawing.Point(1339, 17);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(143, 64);
            this.btnRefresh.TabIndex = 15;
            this.btnRefresh.Text = "增加";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(819, 35);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(120, 29);
            this.labelControl2.TabIndex = 14;
            this.labelControl2.Text = "样  本 号：";
            // 
            // txtSID
            // 
            this.txtSID.EnterMoveNextControl = true;
            this.txtSID.Location = new System.Drawing.Point(949, 29);
            this.txtSID.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtSID.Name = "txtSID";
            this.txtSID.Size = new System.Drawing.Size(286, 36);
            this.txtSID.TabIndex = 13;
            // 
            // txtDate
            // 
            this.txtDate.EditValue = null;
            this.txtDate.EnterMoveNextControl = true;
            this.txtDate.Location = new System.Drawing.Point(147, 29);
            this.txtDate.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtDate.Name = "txtDate";
            this.txtDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.txtDate.Size = new System.Drawing.Size(204, 36);
            this.txtDate.TabIndex = 10;
            // 
            // lblTime
            // 
            this.lblTime.Location = new System.Drawing.Point(19, 35);
            this.lblTime.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(120, 29);
            this.lblTime.TabIndex = 11;
            this.lblTime.Text = "测定日期：";
            // 
            // txtItr
            // 
            this.txtItr.AddEmptyRow = true;
            this.txtItr.BindByValue = false;
            this.txtItr.colDisplay = "";
            this.txtItr.colExtend1 = null;
            this.txtItr.colInCode = "";
            this.txtItr.colPY = "";
            this.txtItr.colSeq = "";
            this.txtItr.colValue = "";
            this.txtItr.colWB = "";
            this.txtItr.displayMember = null;
            this.txtItr.EnterMoveNext = true;
            this.txtItr.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.txtItr.KeyUpDownMoveNext = false;
            this.txtItr.LoadDataOnDesignMode = true;
            this.txtItr.Location = new System.Drawing.Point(524, 29);
            this.txtItr.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtItr.MaximumSize = new System.Drawing.Size(929, 43);
            this.txtItr.MinimumSize = new System.Drawing.Size(93, 43);
            this.txtItr.Name = "txtItr";
            this.txtItr.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtItr.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtItr.Readonly = false;
            this.txtItr.SaveSourceID = false;
            this.txtItr.SelectFilter = null;
            this.txtItr.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtItr.SelectOnly = true;
            this.txtItr.ShowAllInstrmt = true;
            this.txtItr.Size = new System.Drawing.Size(247, 43);
            this.txtItr.TabIndex = 12;
            this.txtItr.UseExtend = false;
            this.txtItr.valueMember = null;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(396, 35);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(120, 29);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "对比仪器：";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 866);
            this.panel2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1822, 118);
            this.panel2.TabIndex = 1;
            this.panel2.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.gcData);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 108);
            this.panel3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1822, 758);
            this.panel3.TabIndex = 2;
            // 
            // gcData
            // 
            this.gcData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcData.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcData.Location = new System.Drawing.Point(0, 0);
            this.gcData.MainView = this.gridViewPatientList;
            this.gcData.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.gcData.Name = "gcData";
            this.gcData.Size = new System.Drawing.Size(1822, 758);
            this.gcData.TabIndex = 1;
            this.gcData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewPatientList});
            // 
            // gridViewPatientList
            // 
            this.gridViewPatientList.Appearance.OddRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridViewPatientList.Appearance.OddRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridViewPatientList.Appearance.OddRow.Options.UseBackColor = true;
            this.gridViewPatientList.Appearance.OddRow.Options.UseForeColor = true;
            this.gridViewPatientList.Appearance.SelectedRow.BackColor = System.Drawing.Color.Blue;
            this.gridViewPatientList.Appearance.SelectedRow.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gridViewPatientList.GridControl = this.gcData;
            this.gridViewPatientList.Name = "gridViewPatientList";
            this.gridViewPatientList.OptionsBehavior.Editable = false;
            this.gridViewPatientList.OptionsBehavior.ImmediateUpdateRowPosition = false;
            this.gridViewPatientList.OptionsBehavior.KeepFocusedRowOnUpdate = false;
            this.gridViewPatientList.OptionsView.ColumnAutoWidth = false;
            this.gridViewPatientList.OptionsView.ShowGroupPanel = false;
            this.gridViewPatientList.RowCellStyle += new DevExpress.XtraGrid.Views.Grid.RowCellStyleEventHandler(this.gridViewPatientList_RowCellStyle);
            // 
            // FrmItemCompare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1822, 984);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FrmItemCompare";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "仪器结果比对";
            this.Load += new System.EventHandler(this.FrmItemCompare_Load);
            this.Shown += new System.EventHandler(this.FrmItemCompare_Shown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDate.Properties)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewPatientList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtSID;
        private DevExpress.XtraEditors.DateEdit txtDate;
        private DevExpress.XtraEditors.LabelControl lblTime;
        private dcl.client.control.SelectDicInstrument txtItr;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnRefresh;
        private DevExpress.XtraGrid.GridControl gcData;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewPatientList;
        private DevExpress.XtraEditors.SimpleButton btnExport;
    }
}