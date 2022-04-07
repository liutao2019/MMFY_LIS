namespace dcl.client.elisa
{
    partial class SampleControl
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox_converCol = new System.Windows.Forms.CheckBox();
            this.checkBox_converRow = new System.Windows.Forms.CheckBox();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel2);
            this.panelTop.Controls.Add(this.btnMoveLeft);
            this.panelTop.Controls.Add(this.btnMoveRight);
            this.panelTop.Controls.Add(this.panel1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(720, 60);
            this.panelTop.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBox_converCol);
            this.panel2.Controls.Add(this.checkBox_converRow);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(720, 21);
            this.panel2.TabIndex = 3;
            // 
            // checkBox_converCol
            // 
            this.checkBox_converCol.AutoSize = true;
            this.checkBox_converCol.Location = new System.Drawing.Point(160, 4);
            this.checkBox_converCol.Name = "checkBox_converCol";
            this.checkBox_converCol.Size = new System.Drawing.Size(120, 16);
            this.checkBox_converCol.TabIndex = 1;
            this.checkBox_converCol.Text = "列显示转换成字母";
            this.checkBox_converCol.UseVisualStyleBackColor = true;
            this.checkBox_converCol.CheckedChanged += new System.EventHandler(this.checkBox_converCol_CheckedChanged);
            // 
            // checkBox_converRow
            // 
            this.checkBox_converRow.AutoSize = true;
            this.checkBox_converRow.Location = new System.Drawing.Point(7, 4);
            this.checkBox_converRow.Name = "checkBox_converRow";
            this.checkBox_converRow.Size = new System.Drawing.Size(120, 16);
            this.checkBox_converRow.TabIndex = 0;
            this.checkBox_converRow.Text = "行显示转换成字母";
            this.checkBox_converRow.UseVisualStyleBackColor = true;
            this.checkBox_converRow.CheckedChanged += new System.EventHandler(this.checkBox_converRow_CheckedChanged);
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.Location = new System.Drawing.Point(3, 25);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(44, 23);
            this.btnMoveLeft.TabIndex = 1;
            this.btnMoveLeft.Text = "←";
            this.btnMoveLeft.UseVisualStyleBackColor = true;
            this.btnMoveLeft.Visible = false;
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.Location = new System.Drawing.Point(53, 25);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(44, 23);
            this.btnMoveRight.TabIndex = 1;
            this.btnMoveRight.Text = "→";
            this.btnMoveRight.UseVisualStyleBackColor = true;
            this.btnMoveRight.Visible = false;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(54, 23);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(100, 0, 0, 0);
            this.panel1.Size = new System.Drawing.Size(667, 34);
            this.panel1.TabIndex = 0;
            // 
            // panelLeft
            // 
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 60);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(54, 485);
            this.panelLeft.TabIndex = 1;
            // 
            // panelMain
            // 
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(54, 60);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(666, 485);
            this.panelMain.TabIndex = 2;
            // 
            // SampleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelTop);
            this.Name = "SampleControl";
            this.Size = new System.Drawing.Size(720, 545);
            this.panelTop.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnMoveLeft;
        private System.Windows.Forms.Button btnMoveRight;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBox_converRow;
        private System.Windows.Forms.CheckBox checkBox_converCol;
    }
}
