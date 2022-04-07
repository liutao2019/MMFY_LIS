using Lis.CustomControls;

namespace wf.client.reagent
{
    partial class RoundLossReportGroup
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
            this.rpReturn = new Lis.CustomControls.RoundPanel();
            this.rpAudit = new Lis.CustomControls.RoundPanel();
            this.rpNonAudit = new Lis.CustomControls.RoundPanel();
            this.rpAll = new Lis.CustomControls.RoundPanel();
            this.autoFixSonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoFixSonPanel1
            // 
            this.autoFixSonPanel1.AutoFixdirection = Lis.CustomControls.AutoFixSonPanel.Fixdirection.Horizontal;
            this.autoFixSonPanel1.AutoSetFixSon = true;
            this.autoFixSonPanel1.BackColor = System.Drawing.Color.Transparent;
            this.autoFixSonPanel1.Controls.Add(this.rpReturn);
            this.autoFixSonPanel1.Controls.Add(this.rpAudit);
            this.autoFixSonPanel1.Controls.Add(this.rpNonAudit);
            this.autoFixSonPanel1.Controls.Add(this.rpAll);
            this.autoFixSonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoFixSonPanel1.Location = new System.Drawing.Point(0, 0);
            this.autoFixSonPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.autoFixSonPanel1.Name = "autoFixSonPanel1";
            this.autoFixSonPanel1.Size = new System.Drawing.Size(315, 20);
            this.autoFixSonPanel1.TabIndex = 0;
            // 
            // rpReturn
            // 
            this.rpReturn.AutoSetFont = true;
            this.rpReturn.BeginColor = System.Drawing.Color.MediumTurquoise;
            this.rpReturn.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpReturn.EndColor = System.Drawing.Color.MediumTurquoise;
            this.rpReturn.InnerText = "已撤销";
            this.rpReturn.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpReturn.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpReturn.Location = new System.Drawing.Point(156, 0);
            this.rpReturn.Name = "rpReturn";
            this.rpReturn.Radius = 1;
            this.rpReturn.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpReturn.Size = new System.Drawing.Size(52, 20);
            this.rpReturn.TabIndex = 9;
            this.rpReturn.Tag = "2";
            // 
            // rpAudit
            // 
            this.rpAudit.AutoSetFont = true;
            this.rpAudit.BeginColor = System.Drawing.Color.MediumTurquoise;
            this.rpAudit.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpAudit.EndColor = System.Drawing.Color.MediumTurquoise;
            this.rpAudit.InnerText = "已报损";
            this.rpAudit.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpAudit.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpAudit.Location = new System.Drawing.Point(104, 0);
            this.rpAudit.Name = "rpAudit";
            this.rpAudit.Radius = 1;
            this.rpAudit.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpAudit.Size = new System.Drawing.Size(52, 20);
            this.rpAudit.TabIndex = 8;
            this.rpAudit.Tag = "4";
            // 
            // rpNonAudit
            // 
            this.rpNonAudit.AutoSetFont = true;
            this.rpNonAudit.BeginColor = System.Drawing.Color.MediumTurquoise;
            this.rpNonAudit.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpNonAudit.EndColor = System.Drawing.Color.MediumTurquoise;
            this.rpNonAudit.InnerText = "未报损";
            this.rpNonAudit.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpNonAudit.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpNonAudit.Location = new System.Drawing.Point(52, 0);
            this.rpNonAudit.Name = "rpNonAudit";
            this.rpNonAudit.Radius = 1;
            this.rpNonAudit.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpNonAudit.Size = new System.Drawing.Size(52, 20);
            this.rpNonAudit.TabIndex = 7;
            this.rpNonAudit.Tag = "1";
            // 
            // rpAll
            // 
            this.rpAll.AutoSetFont = true;
            this.rpAll.BeginColor = System.Drawing.Color.Teal;
            this.rpAll.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpAll.EndColor = System.Drawing.Color.Teal;
            this.rpAll.InnerText = "查全部";
            this.rpAll.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpAll.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpAll.Location = new System.Drawing.Point(0, 0);
            this.rpAll.Name = "rpAll";
            this.rpAll.Radius = 1;
            this.rpAll.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpAll.Size = new System.Drawing.Size(52, 20);
            this.rpAll.TabIndex = 6;
            this.rpAll.Tag = "0";
            // 
            // RoundLossReportGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoFixSonPanel1);
            this.Name = "RoundLossReportGroup";
            this.Size = new System.Drawing.Size(315, 20);
            this.autoFixSonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoFixSonPanel autoFixSonPanel1;
        private RoundPanel rpAll;
        private RoundPanel rpNonAudit;
        private RoundPanel rpAudit;
        private RoundPanel rpReturn;
    }
}
