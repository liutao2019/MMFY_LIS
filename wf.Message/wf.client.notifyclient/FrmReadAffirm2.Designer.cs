namespace dcl.client.notifyclient
{
    partial class FrmReadAffirm2
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtdoc_name = new System.Windows.Forms.TextBox();
            this.txtdep_tel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.btnAffirm = new System.Windows.Forms.Button();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtdoc_num = new dcl.client.control.SelectDictSysUser();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtPwd);
            this.groupBox1.Controls.Add(this.btnAffirm);
            this.groupBox1.Controls.Add(this.txtUserId);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 262);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtdoc_num);
            this.groupBox2.Controls.Add(this.txtdoc_name);
            this.groupBox2.Controls.Add(this.txtdep_tel);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(68, 22);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(210, 130);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "临床信息";
            // 
            // txtdoc_name
            // 
            this.txtdoc_name.Location = new System.Drawing.Point(70, 57);
            this.txtdoc_name.Name = "txtdoc_name";
            this.txtdoc_name.Size = new System.Drawing.Size(129, 23);
            this.txtdoc_name.TabIndex = 20;
            this.txtdoc_name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtdoc_name_KeyDown);
            // 
            // txtdep_tel
            // 
            this.txtdep_tel.Location = new System.Drawing.Point(70, 92);
            this.txtdep_tel.Name = "txtdep_tel";
            this.txtdep_tel.Size = new System.Drawing.Size(129, 23);
            this.txtdep_tel.TabIndex = 19;
            this.txtdep_tel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtdep_tel_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "临床电话";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "临床姓名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "临床工号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 14);
            this.label1.TabIndex = 13;
            this.label1.Text = "用户";
            // 
            // txtPwd
            // 
            this.txtPwd.Location = new System.Drawing.Point(127, 190);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(140, 23);
            this.txtPwd.TabIndex = 16;
            this.txtPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmReadAffirm_KeyDown);
            // 
            // btnAffirm
            // 
            this.btnAffirm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAffirm.Location = new System.Drawing.Point(79, 224);
            this.btnAffirm.Name = "btnAffirm";
            this.btnAffirm.Size = new System.Drawing.Size(87, 27);
            this.btnAffirm.TabIndex = 11;
            this.btnAffirm.Text = "确认";
            this.btnAffirm.UseVisualStyleBackColor = true;
            this.btnAffirm.Click += new System.EventHandler(this.btnAffirm_Click);
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(127, 159);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(140, 23);
            this.txtUserId.TabIndex = 15;
            this.txtUserId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtUserId_KeyDown);
            // 
            // btnClose
            // 
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Location = new System.Drawing.Point(180, 224);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 27);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "关闭";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 14);
            this.label2.TabIndex = 14;
            this.label2.Text = "密码";
            // 
            // txtdoc_num
            // 
            this.txtdoc_num.BindByValue = false;
            this.txtdoc_num.colDisplay = "";
            this.txtdoc_num.colExtend1 = null;
            this.txtdoc_num.colInCode = "";
            this.txtdoc_num.colPY = "";
            this.txtdoc_num.colSeq = "";
            this.txtdoc_num.colValue = "";
            this.txtdoc_num.colWB = "";
            this.txtdoc_num.displayMember = null;
            this.txtdoc_num.EnterMoveNext = true;
            this.txtdoc_num.KeyUpDownMoveNext = false;
            this.txtdoc_num.LoadDataOnDesignMode = true;
            this.txtdoc_num.Location = new System.Drawing.Point(70, 24);
            this.txtdoc_num.MaximumSize = new System.Drawing.Size(500, 25);
            this.txtdoc_num.MinimumSize = new System.Drawing.Size(50, 23);
            this.txtdoc_num.Name = "txtdoc_num";
            this.txtdoc_num.PBorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.txtdoc_num.PFont = new System.Drawing.Font("Tahoma", 9F);
            this.txtdoc_num.Readonly = false;
            this.txtdoc_num.SaveSourceID = false;
            this.txtdoc_num.SelectFilter = null;
            this.txtdoc_num.SelectMode = dcl.client.control.HopePopSelectMode.SingleClick;
            this.txtdoc_num.SelectOnly = true;
            this.txtdoc_num.Size = new System.Drawing.Size(129, 23);
            this.txtdoc_num.TabIndex = 21;
            this.txtdoc_num.UseExtend = false;
            this.txtdoc_num.valueMember = null;
            this.txtdoc_num.ValueChanged += new dcl.client.control.DclPopSelect<dcl.entity.EntitySysUser>.ValueChangedEventHandler(this.txtdoc_num_ValueChanged);
            // 
            // FrmReadAffirm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(324, 262);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(340, 300);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(340, 300);
            this.Name = "FrmReadAffirm2";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "危急值确认";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Button btnAffirm;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtdep_tel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtdoc_name;
        private control.SelectDictSysUser txtdoc_num;
    }
}