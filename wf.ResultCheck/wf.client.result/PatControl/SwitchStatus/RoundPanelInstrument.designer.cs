using Lis.CustomControls;

namespace dcl.client.result.PatControl
{
    partial class RoundPanelInstrument
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
            this.rpTitle = new Lis.CustomControls.RoundPanel();
            this.rpNunTesting = new Lis.CustomControls.RoundPanel();
            this.rpTesting = new Lis.CustomControls.RoundPanel();
            this.rpNunThrough = new Lis.CustomControls.RoundPanel();
            this.rpThrough = new Lis.CustomControls.RoundPanel();
            this.rpAll = new Lis.CustomControls.RoundPanel();
            this.autoFixSonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoFixSonPanel1
            // 
            this.autoFixSonPanel1.AutoFixdirection = Lis.CustomControls.AutoFixSonPanel.Fixdirection.Horizontal;
            this.autoFixSonPanel1.AutoSetFixSon = true;
            this.autoFixSonPanel1.BackColor = System.Drawing.Color.Transparent;
            this.autoFixSonPanel1.Controls.Add(this.rpTitle);
            this.autoFixSonPanel1.Controls.Add(this.rpNunTesting);
            this.autoFixSonPanel1.Controls.Add(this.rpTesting);
            this.autoFixSonPanel1.Controls.Add(this.rpNunThrough);
            this.autoFixSonPanel1.Controls.Add(this.rpThrough);
            this.autoFixSonPanel1.Controls.Add(this.rpAll);
            this.autoFixSonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoFixSonPanel1.Location = new System.Drawing.Point(0, 0);
            this.autoFixSonPanel1.Name = "autoFixSonPanel1";
            this.autoFixSonPanel1.Size = new System.Drawing.Size(426, 26);
            this.autoFixSonPanel1.TabIndex = 0;
            // 
            // rpTitle
            // 
            this.rpTitle.AutoSetFont = true;
            this.rpTitle.BeginColor = System.Drawing.Color.Black;
            this.rpTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpTitle.EndColor = System.Drawing.Color.Black;
            this.rpTitle.InnerText = "仪器审";
            this.rpTitle.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpTitle.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpTitle.Location = new System.Drawing.Point(355, 0);
            this.rpTitle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpTitle.Name = "rpTitle";
            this.rpTitle.Radius = 1;
            this.rpTitle.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpTitle.Size = new System.Drawing.Size(71, 26);
            this.rpTitle.TabIndex = 12;
            this.rpTitle.Tag = "0";
            // 
            // rpNunTesting
            // 
            this.rpNunTesting.AutoSetFont = true;
            this.rpNunTesting.BeginColor = System.Drawing.Color.DarkOrange;
            this.rpNunTesting.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpNunTesting.EndColor = System.Drawing.Color.DarkOrange;
            this.rpNunTesting.InnerText = "未检测";
            this.rpNunTesting.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpNunTesting.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpNunTesting.Location = new System.Drawing.Point(284, 0);
            this.rpNunTesting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpNunTesting.Name = "rpNunTesting";
            this.rpNunTesting.Radius = 1;
            this.rpNunTesting.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpNunTesting.Size = new System.Drawing.Size(71, 26);
            this.rpNunTesting.TabIndex = 11;
            this.rpNunTesting.Tag = "4";
            // 
            // rpTesting
            // 
            this.rpTesting.AutoSetFont = true;
            this.rpTesting.BeginColor = System.Drawing.Color.DarkOrange;
            this.rpTesting.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpTesting.EndColor = System.Drawing.Color.DarkOrange;
            this.rpTesting.InnerText = "检测中";
            this.rpTesting.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpTesting.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpTesting.Location = new System.Drawing.Point(213, 0);
            this.rpTesting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpTesting.Name = "rpTesting";
            this.rpTesting.Radius = 1;
            this.rpTesting.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpTesting.Size = new System.Drawing.Size(71, 26);
            this.rpTesting.TabIndex = 10;
            this.rpTesting.Tag = "3";
            // 
            // rpNunThrough
            // 
            this.rpNunThrough.AutoSetFont = true;
            this.rpNunThrough.BeginColor = System.Drawing.Color.DarkOrange;
            this.rpNunThrough.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpNunThrough.EndColor = System.Drawing.Color.DarkOrange;
            this.rpNunThrough.InnerText = "未通过";
            this.rpNunThrough.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpNunThrough.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpNunThrough.Location = new System.Drawing.Point(142, 0);
            this.rpNunThrough.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpNunThrough.Name = "rpNunThrough";
            this.rpNunThrough.Radius = 1;
            this.rpNunThrough.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpNunThrough.Size = new System.Drawing.Size(71, 26);
            this.rpNunThrough.TabIndex = 9;
            this.rpNunThrough.Tag = "2";
            // 
            // rpThrough
            // 
            this.rpThrough.AutoSetFont = true;
            this.rpThrough.BeginColor = System.Drawing.Color.DarkOrange;
            this.rpThrough.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpThrough.EndColor = System.Drawing.Color.DarkOrange;
            this.rpThrough.InnerText = "已通过";
            this.rpThrough.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpThrough.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpThrough.Location = new System.Drawing.Point(71, 0);
            this.rpThrough.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpThrough.Name = "rpThrough";
            this.rpThrough.Radius = 1;
            this.rpThrough.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpThrough.Size = new System.Drawing.Size(71, 26);
            this.rpThrough.TabIndex = 8;
            this.rpThrough.Tag = "1";
            // 
            // rpAll
            // 
            this.rpAll.AutoSetFont = true;
            this.rpAll.BeginColor = System.Drawing.Color.Sienna;
            this.rpAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpAll.EndColor = System.Drawing.Color.Sienna;
            this.rpAll.InnerText = "查全部";
            this.rpAll.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpAll.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpAll.Location = new System.Drawing.Point(0, 0);
            this.rpAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpAll.Name = "rpAll";
            this.rpAll.Radius = 1;
            this.rpAll.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpAll.Size = new System.Drawing.Size(71, 26);
            this.rpAll.TabIndex = 7;
            this.rpAll.Tag = "0";
            // 
            // RoundPanelInstrument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoFixSonPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RoundPanelInstrument";
            this.Size = new System.Drawing.Size(426, 26);
            this.autoFixSonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoFixSonPanel autoFixSonPanel1;
        private RoundPanel rpAll;
        private RoundPanel rpThrough;
        private RoundPanel rpNunThrough;
        private RoundPanel rpTesting;
        private RoundPanel rpNunTesting;
        private RoundPanel rpTitle;
    }
}
