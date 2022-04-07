namespace dcl.client.msgclient
{
    partial class FrmSetting
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSetting));
            this.txtDept = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColSelDep = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dep_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dep_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsDeptInfo = new System.Windows.Forms.BindingSource();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.cbRunWhenStart = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbS = new System.Windows.Forms.RadioButton();
            this.rbMac = new System.Windows.Forms.RadioButton();
            this.rbNone = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbNeglectDep = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cbOriQT = new System.Windows.Forms.CheckBox();
            this.cbOriTJ = new System.Windows.Forms.CheckBox();
            this.cbOriMZ = new System.Windows.Forms.CheckBox();
            this.cbOriZY = new System.Windows.Forms.CheckBox();
            this.txtselDept = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDeptInfo)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDept
            // 
            this.txtDept.Location = new System.Drawing.Point(8, 25);
            this.txtDept.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtDept.Name = "txtDept";
            this.txtDept.ReadOnly = true;
            this.txtDept.Size = new System.Drawing.Size(169, 25);
            this.txtDept.TabIndex = 1;
            this.txtDept.Click += new System.EventHandler(this.txtDept_Click);
            this.txtDept.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDept_KeyDown);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSelDep,
            this.dep_code,
            this.dep_name});
            this.dataGridView1.DataSource = this.bsDeptInfo;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(232, 46);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(273, 214);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.Visible = false;
            this.dataGridView1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyDown);
            this.dataGridView1.Leave += new System.EventHandler(this.dataGridView1_Leave);
            this.dataGridView1.MouseLeave += new System.EventHandler(this.dataGridView1_MouseLeave);
            // 
            // ColSelDep
            // 
            this.ColSelDep.DataPropertyName = "Checked";
            this.ColSelDep.FillWeight = 35F;
            this.ColSelDep.HeaderText = "";
            this.ColSelDep.Name = "ColSelDep";
            this.ColSelDep.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColSelDep.Width = 35;
            // 
            // dep_code
            // 
            this.dep_code.DataPropertyName = "DeptCode";
            this.dep_code.HeaderText = "编码";
            this.dep_code.Name = "dep_code";
            this.dep_code.ReadOnly = true;
            this.dep_code.Width = 60;
            // 
            // dep_name
            // 
            this.dep_name.DataPropertyName = "DeptName";
            this.dep_name.HeaderText = "科室";
            this.dep_name.Name = "dep_name";
            this.dep_name.ReadOnly = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(216, 380);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 29);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(329, 380);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 29);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Location = new System.Drawing.Point(387, 10);
            this.txtUrl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(29, 18);
            this.txtUrl.TabIndex = 1;
            this.txtUrl.Visible = false;
            // 
            // cbRunWhenStart
            // 
            this.cbRunWhenStart.AutoSize = true;
            this.cbRunWhenStart.Location = new System.Drawing.Point(21, 40);
            this.cbRunWhenStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbRunWhenStart.Name = "cbRunWhenStart";
            this.cbRunWhenStart.Size = new System.Drawing.Size(89, 19);
            this.cbRunWhenStart.TabIndex = 6;
            this.cbRunWhenStart.Text = "开机启动";
            this.cbRunWhenStart.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbS);
            this.groupBox1.Controls.Add(this.rbMac);
            this.groupBox1.Controls.Add(this.rbNone);
            this.groupBox1.Location = new System.Drawing.Point(8, 116);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(421, 80);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "声音设置";
            // 
            // rbS
            // 
            this.rbS.AutoSize = true;
            this.rbS.Location = new System.Drawing.Point(141, 36);
            this.rbS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbS.Name = "rbS";
            this.rbS.Size = new System.Drawing.Size(58, 19);
            this.rbS.TabIndex = 0;
            this.rbS.Text = "音响";
            this.rbS.UseVisualStyleBackColor = true;
            // 
            // rbMac
            // 
            this.rbMac.AutoSize = true;
            this.rbMac.Location = new System.Drawing.Point(71, 36);
            this.rbMac.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbMac.Name = "rbMac";
            this.rbMac.Size = new System.Drawing.Size(58, 19);
            this.rbMac.TabIndex = 0;
            this.rbMac.Text = "主机";
            this.rbMac.UseVisualStyleBackColor = true;
            // 
            // rbNone
            // 
            this.rbNone.AutoSize = true;
            this.rbNone.Checked = true;
            this.rbNone.Location = new System.Drawing.Point(9, 36);
            this.rbNone.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rbNone.Name = "rbNone";
            this.rbNone.Size = new System.Drawing.Size(43, 19);
            this.rbNone.TabIndex = 0;
            this.rbNone.TabStop = true;
            this.rbNone.Text = "无";
            this.rbNone.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbRunWhenStart);
            this.groupBox2.Location = new System.Drawing.Point(8, 211);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(421, 80);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "启动设置";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbNeglectDep);
            this.groupBox3.Controls.Add(this.txtDept);
            this.groupBox3.Location = new System.Drawing.Point(8, 15);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox3.Size = new System.Drawing.Size(421, 94);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "科室设置";
            // 
            // cbNeglectDep
            // 
            this.cbNeglectDep.AutoSize = true;
            this.cbNeglectDep.Location = new System.Drawing.Point(16, 64);
            this.cbNeglectDep.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbNeglectDep.Name = "cbNeglectDep";
            this.cbNeglectDep.Size = new System.Drawing.Size(150, 19);
            this.cbNeglectDep.TabIndex = 5;
            this.cbNeglectDep.Text = "忽略科室(或病区)";
            this.cbNeglectDep.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cbOriQT);
            this.groupBox4.Controls.Add(this.cbOriTJ);
            this.groupBox4.Controls.Add(this.cbOriMZ);
            this.groupBox4.Controls.Add(this.cbOriZY);
            this.groupBox4.Location = new System.Drawing.Point(8, 304);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox4.Size = new System.Drawing.Size(421, 66);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "病人来源";
            // 
            // cbOriQT
            // 
            this.cbOriQT.AutoSize = true;
            this.cbOriQT.Checked = true;
            this.cbOriQT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOriQT.Location = new System.Drawing.Point(231, 30);
            this.cbOriQT.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbOriQT.Name = "cbOriQT";
            this.cbOriQT.Size = new System.Drawing.Size(59, 19);
            this.cbOriQT.TabIndex = 4;
            this.cbOriQT.Text = "其他";
            this.cbOriQT.UseVisualStyleBackColor = true;
            // 
            // cbOriTJ
            // 
            this.cbOriTJ.AutoSize = true;
            this.cbOriTJ.Checked = true;
            this.cbOriTJ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOriTJ.Location = new System.Drawing.Point(159, 30);
            this.cbOriTJ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbOriTJ.Name = "cbOriTJ";
            this.cbOriTJ.Size = new System.Drawing.Size(59, 19);
            this.cbOriTJ.TabIndex = 3;
            this.cbOriTJ.Text = "体检";
            this.cbOriTJ.UseVisualStyleBackColor = true;
            // 
            // cbOriMZ
            // 
            this.cbOriMZ.AutoSize = true;
            this.cbOriMZ.Checked = true;
            this.cbOriMZ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOriMZ.Location = new System.Drawing.Point(87, 30);
            this.cbOriMZ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbOriMZ.Name = "cbOriMZ";
            this.cbOriMZ.Size = new System.Drawing.Size(59, 19);
            this.cbOriMZ.TabIndex = 2;
            this.cbOriMZ.Text = "门诊";
            this.cbOriMZ.UseVisualStyleBackColor = true;
            // 
            // cbOriZY
            // 
            this.cbOriZY.AutoSize = true;
            this.cbOriZY.Checked = true;
            this.cbOriZY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOriZY.Location = new System.Drawing.Point(15, 30);
            this.cbOriZY.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbOriZY.Name = "cbOriZY";
            this.cbOriZY.Size = new System.Drawing.Size(59, 19);
            this.cbOriZY.TabIndex = 1;
            this.cbOriZY.Text = "住院";
            this.cbOriZY.UseVisualStyleBackColor = true;
            // 
            // txtselDept
            // 
            this.txtselDept.Location = new System.Drawing.Point(232, 19);
            this.txtselDept.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtselDept.Name = "txtselDept";
            this.txtselDept.Size = new System.Drawing.Size(272, 25);
            this.txtselDept.TabIndex = 11;
            this.txtselDept.Visible = false;
            this.txtselDept.TextChanged += new System.EventHandler(this.txtselDept_TextChanged);
            // 
            // FrmSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 414);
            this.Controls.Add(this.txtselDept);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtUrl);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(527, 459);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(331, 289);
            this.Name = "FrmSetting";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsDeptInfo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDept;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource bsDeptInfo;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.CheckBox cbRunWhenStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbS;
        private System.Windows.Forms.RadioButton rbMac;
        private System.Windows.Forms.RadioButton rbNone;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox cbOriTJ;
        private System.Windows.Forms.CheckBox cbOriMZ;
        private System.Windows.Forms.CheckBox cbOriZY;
        private System.Windows.Forms.CheckBox cbOriQT;
        private System.Windows.Forms.CheckBox cbNeglectDep;
        private System.Windows.Forms.TextBox txtselDept;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColSelDep;
        private System.Windows.Forms.DataGridViewTextBoxColumn dep_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn dep_name;
    }
}