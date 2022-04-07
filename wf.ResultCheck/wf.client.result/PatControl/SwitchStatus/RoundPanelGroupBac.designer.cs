using Lis.CustomControls;

namespace dcl.client.result.PatControl
{
    partial class RoundPanelGroupBac
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
            this.rpNunPrint = new Lis.CustomControls.RoundPanel();
            this.rpUrgent = new Lis.CustomControls.RoundPanel();
            this.rpNonReport = new Lis.CustomControls.RoundPanel();
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
            this.autoFixSonPanel1.Controls.Add(this.rpNunPrint);
            this.autoFixSonPanel1.Controls.Add(this.rpUrgent);
            this.autoFixSonPanel1.Controls.Add(this.rpNonReport);
            this.autoFixSonPanel1.Controls.Add(this.rpNonAudit);
            this.autoFixSonPanel1.Controls.Add(this.rpAll);
            this.autoFixSonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoFixSonPanel1.Location = new System.Drawing.Point(0, 0);
            this.autoFixSonPanel1.Name = "autoFixSonPanel1";
            this.autoFixSonPanel1.Size = new System.Drawing.Size(286, 26);
            this.autoFixSonPanel1.TabIndex = 0;
            // 
            // rpNunPrint
            // 
            this.rpNunPrint.AutoSetFont = true;
            this.rpNunPrint.BeginColor = System.Drawing.Color.MediumTurquoise;
            this.rpNunPrint.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpNunPrint.EndColor = System.Drawing.Color.MediumTurquoise;
            this.rpNunPrint.InnerText = "未打印";
            this.rpNunPrint.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpNunPrint.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpNunPrint.Location = new System.Drawing.Point(228, 0);
            this.rpNunPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpNunPrint.Name = "rpNunPrint";
            this.rpNunPrint.Radius = 1;
            this.rpNunPrint.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpNunPrint.Size = new System.Drawing.Size(57, 26);
            this.rpNunPrint.TabIndex = 10;
            this.rpNunPrint.Tag = "5";
            // 
            // rpUrgent
            // 
            this.rpUrgent.AutoSetFont = true;
            this.rpUrgent.BeginColor = System.Drawing.Color.MediumTurquoise;
            this.rpUrgent.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpUrgent.EndColor = System.Drawing.Color.MediumTurquoise;
            this.rpUrgent.InnerText = "危急值";
            this.rpUrgent.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpUrgent.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpUrgent.Location = new System.Drawing.Point(171, 0);
            this.rpUrgent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpUrgent.Name = "rpUrgent";
            this.rpUrgent.Radius = 1;
            this.rpUrgent.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpUrgent.Size = new System.Drawing.Size(57, 26);
            this.rpUrgent.TabIndex = 9;
            this.rpUrgent.Tag = "4";
            // 
            // rpNonReport
            // 
            this.rpNonReport.AutoSetFont = true;
            this.rpNonReport.BeginColor = System.Drawing.Color.MediumTurquoise;
            this.rpNonReport.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpNonReport.EndColor = System.Drawing.Color.MediumTurquoise;
            this.rpNonReport.InnerText = "未报告";
            this.rpNonReport.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpNonReport.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpNonReport.Location = new System.Drawing.Point(114, 0);
            this.rpNonReport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpNonReport.Name = "rpNonReport";
            this.rpNonReport.Radius = 1;
            this.rpNonReport.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpNonReport.Size = new System.Drawing.Size(57, 26);
            this.rpNonReport.TabIndex = 8;
            this.rpNonReport.Tag = "3";
            // 
            // rpNonAudit
            // 
            this.rpNonAudit.AutoSetFont = true;
            this.rpNonAudit.BeginColor = System.Drawing.Color.MediumTurquoise;
            this.rpNonAudit.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpNonAudit.EndColor = System.Drawing.Color.MediumTurquoise;
            this.rpNonAudit.InnerText = "未审核";
            this.rpNonAudit.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpNonAudit.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpNonAudit.Location = new System.Drawing.Point(57, 0);
            this.rpNonAudit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpNonAudit.Name = "rpNonAudit";
            this.rpNonAudit.Radius = 1;
            this.rpNonAudit.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpNonAudit.Size = new System.Drawing.Size(57, 26);
            this.rpNonAudit.TabIndex = 7;
            this.rpNonAudit.Tag = "2";
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
            this.rpAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpAll.Name = "rpAll";
            this.rpAll.Radius = 1;
            this.rpAll.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpAll.Size = new System.Drawing.Size(57, 26);
            this.rpAll.TabIndex = 6;
            this.rpAll.Tag = "0";
            // 
            // RoundPanelGroupBac
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoFixSonPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RoundPanelGroupBac";
            this.Size = new System.Drawing.Size(286, 26);
            this.autoFixSonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoFixSonPanel autoFixSonPanel1;
        private RoundPanel rpAll;
        private RoundPanel rpNonAudit;
        private RoundPanel rpNonReport;
        private RoundPanel rpUrgent;
        private RoundPanel rpNunPrint;
    }
}
