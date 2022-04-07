namespace dcl.client.sample
{
    partial class FrmBCQuery
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBCQuery));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.checkBox_searchSign = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.dateSearch = new DevExpress.XtraEditors.DateEdit();
            this.selectDict_Depart1 = new dcl.client.control.SelectDicPubDept();
            this.dateEnd = new DevExpress.XtraEditors.DateEdit();
            this.lueDepart = new dcl.client.control.SelectDicPubDept();
            this.cbStatus = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label13 = new System.Windows.Forms.Label();
            this.lueTypes = new lis.client.control.SelectDict_TypeChecks();
            this.label12 = new System.Windows.Forms.Label();
            this.cbSignerSearchType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSigner = new DevExpress.XtraEditors.TextEdit();
            this.btnInquiry = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateSearch.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateSearch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbStatus.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSignerSearchType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSigner.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.checkBox_searchSign);
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Controls.Add(this.label16);
            this.groupControl1.Controls.Add(this.dateSearch);
            this.groupControl1.Controls.Add(this.selectDict_Depart1);
            this.groupControl1.Controls.Add(this.dateEnd);
            this.groupControl1.Controls.Add(this.lueDepart);
            this.groupControl1.Controls.Add(this.cbStatus);
            this.groupControl1.Controls.Add(this.label13);
            this.groupControl1.Controls.Add(this.lueTypes);
            this.groupControl1.Controls.Add(this.label12);
            this.groupControl1.Controls.Add(this.cbSignerSearchType);
            this.groupControl1.Controls.Add(this.label11);
            this.groupControl1.Controls.Add(this.txtSigner);
            this.groupControl1.Location = new System.Drawing.Point(14, 16);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(432, 198);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "条件";
            // 
            // checkBox_searchSign
            // 
            this.checkBox_searchSign.AutoSize = true;
            this.checkBox_searchSign.Checked = true;
            this.checkBox_searchSign.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_searchSign.Location = new System.Drawing.Point(217, 156);
            this.checkBox_searchSign.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBox_searchSign.Name = "checkBox_searchSign";
            this.checkBox_searchSign.Size = new System.Drawing.Size(210, 22);
            this.checkBox_searchSign.TabIndex = 27;
            this.checkBox_searchSign.Text = "是否只查条码最后状态流程";
            this.checkBox_searchSign.UseVisualStyleBackColor = true;
            this.checkBox_searchSign.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(223, 55);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 15);
            this.label2.TabIndex = 26;
            this.label2.Text = "至";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 54);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 15);
            this.label1.TabIndex = 25;
            this.label1.Text = "时间范围:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(34, 157);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(45, 15);
            this.label16.TabIndex = 24;
            this.label16.Text = "科室:";
            this.label16.Visible = false;
            // 
            // dateSearch
            // 
            this.dateSearch.EditValue = null;
            this.dateSearch.EnterMoveNextControl = true;
            this.dateSearch.Location = new System.Drawing.Point(80, 50);
            this.dateSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateSearch.Name = "dateSearch";
            this.dateSearch.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dateSearch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateSearch.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateSearch.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateSearch.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateSearch.Properties.EditFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateSearch.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateSearch.Properties.Mask.EditMask = "";
            this.dateSearch.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dateSearch.Size = new System.Drawing.Size(130, 24);
            this.dateSearch.TabIndex = 0;
            // 
            // selectDict_Depart1
            // 
            this.selectDict_Depart1.AddEmptyRow = true;
            this.selectDict_Depart1.BindByValue = false;
            this.selectDict_Depart1.colDisplay = "";
            this.selectDict_Depart1.colExtend1 = null;
            this.selectDict_Depart1.colInCode = "";
            this.selectDict_Depart1.colPY = "";
            this.selectDict_Depart1.colSeq = "";
            this.selectDict_Depart1.colValue = "";
            this.selectDict_Depart1.colWB = "";
            this.selectDict_Depart1.displayMember = null;
            this.selectDict_Depart1.EnterMoveNext = true;
            this.selectDict_Depart1.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.selectDict_Depart1.KeyUpDownMoveNext = false;
            this.selectDict_Depart1.LoadDataOnDesignMode = true;
            this.selectDict_Depart1.Location = new System.Drawing.Point(80, 153);
            this.selectDict_Depart1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.selectDict_Depart1.MaximumSize = new System.Drawing.Size(572, 27);
            this.selectDict_Depart1.MinimumSize = new System.Drawing.Size(57, 27);
            this.selectDict_Depart1.Name = "selectDict_Depart1";
            this.selectDict_Depart1.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.selectDict_Depart1.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.selectDict_Depart1.Readonly = false;
            this.selectDict_Depart1.SaveSourceID = false;
            this.selectDict_Depart1.SelectFilter = null;
            this.selectDict_Depart1.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.selectDict_Depart1.SelectOnly = false;
            this.selectDict_Depart1.Size = new System.Drawing.Size(130, 27);
            this.selectDict_Depart1.TabIndex = 23;
            this.selectDict_Depart1.UseExtend = false;
            this.selectDict_Depart1.valueMember = null;
            this.selectDict_Depart1.Visible = false;
            // 
            // dateEnd
            // 
            this.dateEnd.EditValue = null;
            this.dateEnd.EnterMoveNextControl = true;
            this.dateEnd.Location = new System.Drawing.Point(254, 50);
            this.dateEnd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.dateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateEnd.Properties.DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
            this.dateEnd.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEnd.Properties.EditFormat.FormatString = "u";
            this.dateEnd.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateEnd.Properties.Mask.EditMask = "";
            this.dateEnd.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.dateEnd.Size = new System.Drawing.Size(129, 24);
            this.dateEnd.TabIndex = 1;
            // 
            // lueDepart
            // 
            this.lueDepart.AddEmptyRow = true;
            this.lueDepart.BindByValue = false;
            this.lueDepart.colDisplay = "";
            this.lueDepart.colExtend1 = "dep_code";
            this.lueDepart.colInCode = "";
            this.lueDepart.colPY = "";
            this.lueDepart.colSeq = "";
            this.lueDepart.colValue = "";
            this.lueDepart.colWB = "";
            this.lueDepart.displayMember = null;
            this.lueDepart.EnterMoveNext = true;
            this.lueDepart.FilterMode = dcl.client.control.DclPopFilterMode.FuzzyMatching;
            this.lueDepart.KeyUpDownMoveNext = false;
            this.lueDepart.LoadDataOnDesignMode = true;
            this.lueDepart.Location = new System.Drawing.Point(80, 117);
            this.lueDepart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lueDepart.MaximumSize = new System.Drawing.Size(572, 27);
            this.lueDepart.MinimumSize = new System.Drawing.Size(57, 27);
            this.lueDepart.Name = "lueDepart";
            this.lueDepart.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueDepart.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.lueDepart.Readonly = false;
            this.lueDepart.SaveSourceID = false;
            this.lueDepart.SelectFilter = null;
            this.lueDepart.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.lueDepart.SelectOnly = true;
            this.lueDepart.Size = new System.Drawing.Size(130, 27);
            this.lueDepart.TabIndex = 20;
            this.lueDepart.UseExtend = false;
            this.lueDepart.valueMember = null;
            this.lueDepart.Visible = false;
            // 
            // cbStatus
            // 
            this.cbStatus.EditValue = "送达";
            this.cbStatus.EnterMoveNextControl = true;
            this.cbStatus.Location = new System.Drawing.Point(254, 117);
            this.cbStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.cbStatus.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbStatus.Properties.Items.AddRange(new object[] {
            "送达",
            "收取",
            "二次送检"});
            this.cbStatus.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbStatus.Size = new System.Drawing.Size(129, 24);
            this.cbStatus.TabIndex = 16;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(212, 124);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 15);
            this.label13.TabIndex = 19;
            this.label13.Text = "状态:";
            // 
            // lueTypes
            // 
            this.lueTypes.displayMember = "";
            this.lueTypes.Location = new System.Drawing.Point(80, 117);
            this.lueTypes.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.lueTypes.Name = "lueTypes";
            this.lueTypes.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.lueTypes.Size = new System.Drawing.Size(130, 36);
            this.lueTypes.TabIndex = 14;
            this.lueTypes.valueMember = null;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(20, 119);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(58, 18);
            this.label12.TabIndex = 18;
            this.label12.Text = "目的地:";
            // 
            // cbSignerSearchType
            // 
            this.cbSignerSearchType.EditValue = "工号";
            this.cbSignerSearchType.EnterMoveNextControl = true;
            this.cbSignerSearchType.Location = new System.Drawing.Point(80, 84);
            this.cbSignerSearchType.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbSignerSearchType.Name = "cbSignerSearchType";
            this.cbSignerSearchType.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.cbSignerSearchType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbSignerSearchType.Properties.Items.AddRange(new object[] {
            "工号",
            "姓名",
            "病人ID",
            "批号",
            "条码号"});
            this.cbSignerSearchType.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cbSignerSearchType.Size = new System.Drawing.Size(130, 24);
            this.cbSignerSearchType.TabIndex = 12;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(20, 89);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 15);
            this.label11.TabIndex = 15;
            this.label11.Text = "确认人:";
            // 
            // txtSigner
            // 
            this.txtSigner.EnterMoveNextControl = true;
            this.txtSigner.Location = new System.Drawing.Point(254, 84);
            this.txtSigner.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSigner.Name = "txtSigner";
            this.txtSigner.Size = new System.Drawing.Size(129, 24);
            this.txtSigner.TabIndex = 13;
            // 
            // btnInquiry
            // 
            this.btnInquiry.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btnInquiry.Appearance.Options.UseFont = true;
            this.btnInquiry.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnInquiry.Location = new System.Drawing.Point(204, 220);
            this.btnInquiry.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnInquiry.Name = "btnInquiry";
            this.btnInquiry.Size = new System.Drawing.Size(118, 43);
            this.btnInquiry.TabIndex = 17;
            this.btnInquiry.Text = "查询";
            this.btnInquiry.Click += new System.EventHandler(this.btnInquiry_Click);
            // 
            // btnClose
            // 
            this.btnClose.Appearance.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btnClose.Appearance.Options.UseFont = true;
            this.btnClose.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnClose.Location = new System.Drawing.Point(328, 220);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(118, 43);
            this.btnClose.TabIndex = 18;
            this.btnClose.Text = "取消";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmBCQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 270);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.btnInquiry);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmBCQuery";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "条码查询";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateSearch.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateSearch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbStatus.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbSignerSearchType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSigner.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.DateEdit dateSearch;
        private DevExpress.XtraEditors.DateEdit dateEnd;
        private dcl.client.control.SelectDicPubDept lueDepart;
        private DevExpress.XtraEditors.ComboBoxEdit cbStatus;
        private System.Windows.Forms.Label label13;
        private lis.client.control.SelectDict_TypeChecks lueTypes;
        private System.Windows.Forms.Label label12;
        private DevExpress.XtraEditors.ComboBoxEdit cbSignerSearchType;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.TextEdit txtSigner;
        private DevExpress.XtraEditors.SimpleButton btnInquiry;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private System.Windows.Forms.Label label16;
        private dcl.client.control.SelectDicPubDept selectDict_Depart1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox_searchSign;
    }
}