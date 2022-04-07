using Lis.CustomControls;

namespace dcl.client.result.PatControl
{
    partial class RoundPanelResult
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
            this.autoFixSonPanel1 = new Lis.CustomControls.AutoFixSonPanel();
            this.rpView = new Lis.CustomControls.RoundPanel();
            this.rpNonResult = new Lis.CustomControls.RoundPanel();
            this.rpHasResult = new Lis.CustomControls.RoundPanel();
            this.rpAll = new Lis.CustomControls.RoundPanel();
            this.autoFixSonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoFixSonPanel1
            // 
            this.autoFixSonPanel1.AutoFixdirection = Lis.CustomControls.AutoFixSonPanel.Fixdirection.Horizontal;
            this.autoFixSonPanel1.AutoSetFixSon = true;
            this.autoFixSonPanel1.BackColor = System.Drawing.Color.Transparent;
            this.autoFixSonPanel1.Controls.Add(this.rpView);
            this.autoFixSonPanel1.Controls.Add(this.rpNonResult);
            this.autoFixSonPanel1.Controls.Add(this.rpHasResult);
            this.autoFixSonPanel1.Controls.Add(this.rpAll);
            this.autoFixSonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoFixSonPanel1.Location = new System.Drawing.Point(0, 0);
            this.autoFixSonPanel1.Name = "autoFixSonPanel1";
            this.autoFixSonPanel1.Size = new System.Drawing.Size(287, 26);
            this.autoFixSonPanel1.TabIndex = 0;
            // 
            // rpView
            // 
            this.rpView.AutoSetFont = true;
            this.rpView.BeginColor = System.Drawing.Color.LimeGreen;
            this.rpView.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpView.EndColor = System.Drawing.Color.LimeGreen;
            this.rpView.InnerText = "已查看";
            this.rpView.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpView.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpView.Location = new System.Drawing.Point(213, 0);
            this.rpView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpView.Name = "rpView";
            this.rpView.Radius = 1;
            this.rpView.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpView.Size = new System.Drawing.Size(71, 26);
            this.rpView.TabIndex = 9;
            this.rpView.Tag = "3";
            // 
            // rpNonResult
            // 
            this.rpNonResult.AutoSetFont = true;
            this.rpNonResult.BeginColor = System.Drawing.Color.LimeGreen;
            this.rpNonResult.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpNonResult.EndColor = System.Drawing.Color.LimeGreen;
            this.rpNonResult.InnerText = "无结果";
            this.rpNonResult.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpNonResult.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpNonResult.Location = new System.Drawing.Point(142, 0);
            this.rpNonResult.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpNonResult.Name = "rpNonResult";
            this.rpNonResult.Radius = 1;
            this.rpNonResult.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpNonResult.Size = new System.Drawing.Size(71, 26);
            this.rpNonResult.TabIndex = 8;
            this.rpNonResult.Tag = "2";
            // 
            // rpHasResult
            // 
            this.rpHasResult.AutoSetFont = true;
            this.rpHasResult.BeginColor = System.Drawing.Color.LimeGreen;
            this.rpHasResult.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpHasResult.EndColor = System.Drawing.Color.LimeGreen;
            this.rpHasResult.InnerText = "有结果";
            this.rpHasResult.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpHasResult.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpHasResult.Location = new System.Drawing.Point(71, 0);
            this.rpHasResult.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpHasResult.Name = "rpHasResult";
            this.rpHasResult.Radius = 1;
            this.rpHasResult.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpHasResult.Size = new System.Drawing.Size(71, 26);
            this.rpHasResult.TabIndex = 7;
            this.rpHasResult.Tag = "1";
            // 
            // rpAll
            // 
            this.rpAll.AutoSetFont = true;
            this.rpAll.BeginColor = System.Drawing.Color.DarkGreen;
            this.rpAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpAll.EndColor = System.Drawing.Color.DarkGreen;
            this.rpAll.InnerText = "查全部";
            this.rpAll.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpAll.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpAll.Location = new System.Drawing.Point(0, 0);
            this.rpAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpAll.Name = "rpAll";
            this.rpAll.Radius = 1;
            this.rpAll.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpAll.Size = new System.Drawing.Size(71, 26);
            this.rpAll.TabIndex = 6;
            this.rpAll.Tag = "0";
            // 
            // RoundPanelResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoFixSonPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RoundPanelResult";
            this.Size = new System.Drawing.Size(287, 26);
            this.autoFixSonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoFixSonPanel autoFixSonPanel1;
        private RoundPanel rpAll;
        private RoundPanel rpHasResult;
        private RoundPanel rpNonResult;
        private RoundPanel rpView;
    }
}
