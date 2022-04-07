namespace dcl.client.dicbasic
{
    partial class EiasaAoutoHoleModeControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAoutoSeq = new DevExpress.XtraEditors.SimpleButton();
            this.rbnMouseAuto = new System.Windows.Forms.RadioButton();
            this.radioButtonDefu = new System.Windows.Forms.RadioButton();
            this.txtLaterCol = new System.Windows.Forms.TextBox();
            this.txtLongCol = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLaterRow = new System.Windows.Forms.TextBox();
            this.txtLongRow = new System.Windows.Forms.TextBox();
            this.rbnLong = new System.Windows.Forms.RadioButton();
            this.rbnLater = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAoutoSeq);
            this.groupBox1.Controls.Add(this.rbnMouseAuto);
            this.groupBox1.Controls.Add(this.radioButtonDefu);
            this.groupBox1.Controls.Add(this.txtLaterCol);
            this.groupBox1.Controls.Add(this.txtLongCol);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtLaterRow);
            this.groupBox1.Controls.Add(this.txtLongRow);
            this.groupBox1.Controls.Add(this.rbnLong);
            this.groupBox1.Controls.Add(this.rbnLater);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(535, 154);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "快捷录入";
            // 
            // btnAoutoSeq
            // 
            this.btnAoutoSeq.Location = new System.Drawing.Point(285, 73);
            this.btnAoutoSeq.Name = "btnAoutoSeq";
            this.btnAoutoSeq.Size = new System.Drawing.Size(68, 39);
            this.btnAoutoSeq.TabIndex = 42;
            this.btnAoutoSeq.Text = "生成序号";
            this.btnAoutoSeq.Click += new System.EventHandler(this.btnAoutoSeq_Click);
            // 
            // rbnMouseAuto
            // 
            this.rbnMouseAuto.AutoSize = true;
            this.rbnMouseAuto.Location = new System.Drawing.Point(17, 48);
            this.rbnMouseAuto.Name = "rbnMouseAuto";
            this.rbnMouseAuto.Size = new System.Drawing.Size(143, 16);
            this.rbnMouseAuto.TabIndex = 31;
            this.rbnMouseAuto.Text = "鼠标点击自动累加填充";
            this.rbnMouseAuto.UseVisualStyleBackColor = true;
            this.rbnMouseAuto.CheckedChanged += new System.EventHandler(this.radioButtonDefu_CheckedChanged);
            // 
            // radioButtonDefu
            // 
            this.radioButtonDefu.AutoSize = true;
            this.radioButtonDefu.Checked = true;
            this.radioButtonDefu.Location = new System.Drawing.Point(18, 21);
            this.radioButtonDefu.Name = "radioButtonDefu";
            this.radioButtonDefu.Size = new System.Drawing.Size(71, 16);
            this.radioButtonDefu.TabIndex = 41;
            this.radioButtonDefu.TabStop = true;
            this.radioButtonDefu.Text = "手动输入";
            this.radioButtonDefu.UseVisualStyleBackColor = true;
            this.radioButtonDefu.CheckedChanged += new System.EventHandler(this.radioButtonDefu_CheckedChanged);
            // 
            // txtLaterCol
            // 
            this.txtLaterCol.Location = new System.Drawing.Point(212, 72);
            this.txtLaterCol.Name = "txtLaterCol";
            this.txtLaterCol.Size = new System.Drawing.Size(54, 21);
            this.txtLaterCol.TabIndex = 40;
            // 
            // txtLongCol
            // 
            this.txtLongCol.Location = new System.Drawing.Point(212, 96);
            this.txtLongCol.Name = "txtLongCol";
            this.txtLongCol.Size = new System.Drawing.Size(54, 21);
            this.txtLongCol.TabIndex = 37;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 12);
            this.label3.TabIndex = 39;
            this.label3.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(198, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 12);
            this.label2.TabIndex = 36;
            this.label2.Text = "X";
            // 
            // txtLaterRow
            // 
            this.txtLaterRow.Location = new System.Drawing.Point(141, 72);
            this.txtLaterRow.Name = "txtLaterRow";
            this.txtLaterRow.Size = new System.Drawing.Size(54, 21);
            this.txtLaterRow.TabIndex = 38;
            // 
            // txtLongRow
            // 
            this.txtLongRow.Location = new System.Drawing.Point(141, 96);
            this.txtLongRow.Name = "txtLongRow";
            this.txtLongRow.Size = new System.Drawing.Size(54, 21);
            this.txtLongRow.TabIndex = 0;
            // 
            // rbnLong
            // 
            this.rbnLong.AutoSize = true;
            this.rbnLong.Location = new System.Drawing.Point(17, 98);
            this.rbnLong.Name = "rbnLong";
            this.rbnLong.Size = new System.Drawing.Size(95, 16);
            this.rbnLong.TabIndex = 31;
            this.rbnLong.Text = "纵向模板填充";
            this.rbnLong.UseVisualStyleBackColor = true;
            this.rbnLong.CheckedChanged += new System.EventHandler(this.radioButtonDefu_CheckedChanged);
            // 
            // rbnLater
            // 
            this.rbnLater.AutoSize = true;
            this.rbnLater.Location = new System.Drawing.Point(17, 73);
            this.rbnLater.Name = "rbnLater";
            this.rbnLater.Size = new System.Drawing.Size(95, 16);
            this.rbnLater.TabIndex = 31;
            this.rbnLater.Text = "横向模板填充";
            this.rbnLater.UseVisualStyleBackColor = true;
            this.rbnLater.CheckedChanged += new System.EventHandler(this.radioButtonDefu_CheckedChanged);
            // 
            // EiasaAoutoHoleModeControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "EiasaAoutoHoleModeControl";
            this.Size = new System.Drawing.Size(535, 154);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtLongCol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLongRow;
        private System.Windows.Forms.RadioButton rbnLong;
        private System.Windows.Forms.RadioButton rbnLater;
        private System.Windows.Forms.RadioButton rbnMouseAuto;
        private System.Windows.Forms.RadioButton radioButtonDefu;
        private System.Windows.Forms.TextBox txtLaterCol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLaterRow;
        private DevExpress.XtraEditors.SimpleButton btnAoutoSeq;
    }
}
