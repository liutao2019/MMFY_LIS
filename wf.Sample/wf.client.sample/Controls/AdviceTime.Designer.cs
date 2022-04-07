namespace dcl.client.sample
{
    partial class AdviceTime
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
            this.lblStart = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimeControl2 = new dcl.client.sample.DateTimeControl();
            this.dateTimeControl1 = new dcl.client.sample.DateTimeControl();
            this.SuspendLayout();
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblStart.Location = new System.Drawing.Point(1, 10);
            this.lblStart.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(44, 18);
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "日期";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(174, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "至";
            // 
            // dateTimeControl2
            // 
            this.dateTimeControl2.Day = 1;
            this.dateTimeControl2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimeControl2.Hour = 23;
            this.dateTimeControl2.Location = new System.Drawing.Point(199, 4);
            this.dateTimeControl2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.dateTimeControl2.Minute = 59;
            this.dateTimeControl2.Month = 5;
            this.dateTimeControl2.Name = "dateTimeControl2";
            this.dateTimeControl2.ShowTime = true;
            this.dateTimeControl2.Size = new System.Drawing.Size(165, 36);
            this.dateTimeControl2.TabIndex = 1;
            this.dateTimeControl2.Value = new System.DateTime(2007, 5, 1, 23, 59, 0, 0);
            this.dateTimeControl2.Year = 2007;
            // 
            // dateTimeControl1
            // 
            this.dateTimeControl1.Day = 1;
            this.dateTimeControl1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimeControl1.Hour = 0;
            this.dateTimeControl1.Location = new System.Drawing.Point(43, 4);
            this.dateTimeControl1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.dateTimeControl1.Minute = 0;
            this.dateTimeControl1.Month = 1;
            this.dateTimeControl1.Name = "dateTimeControl1";
            this.dateTimeControl1.ShowTime = true;
            this.dateTimeControl1.Size = new System.Drawing.Size(135, 36);
            this.dateTimeControl1.TabIndex = 0;
            this.dateTimeControl1.Value = new System.DateTime(2007, 1, 1, 0, 0, 0, 0);
            this.dateTimeControl1.Year = 2007;
            // 
            // AdviceTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimeControl2);
            this.Controls.Add(this.dateTimeControl1);
            this.Controls.Add(this.lblStart);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AdviceTime";
            this.Size = new System.Drawing.Size(374, 40);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStart;
        private DateTimeControl dateTimeControl1;
        private DateTimeControl dateTimeControl2;
        private System.Windows.Forms.Label label1;
    }
}
