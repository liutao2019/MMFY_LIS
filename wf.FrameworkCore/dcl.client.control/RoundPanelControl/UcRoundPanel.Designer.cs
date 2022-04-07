namespace dcl.client.control.RoundPanelControl
{
    partial class UcRoundPanel
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
            this.autopanel = new Lis.CustomControls.AutoFixSonPanel();
            this.SuspendLayout();
            // 
            // autopanel
            // 
            this.autopanel.AutoFixdirection = Lis.CustomControls.AutoFixSonPanel.Fixdirection.Vertical;
            this.autopanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autopanel.Location = new System.Drawing.Point(0, 0);
            this.autopanel.Margin = new System.Windows.Forms.Padding(2);
            this.autopanel.Name = "autopanel";
            this.autopanel.Size = new System.Drawing.Size(202, 31);
            this.autopanel.TabIndex = 1;
            // 
            // UcRoundPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autopanel);
            this.Name = "UcRoundPanel";
            this.Size = new System.Drawing.Size(202, 31);
            this.ResumeLayout(false);

        }

        #endregion

        private Lis.CustomControls.AutoFixSonPanel autopanel;
    }
}
