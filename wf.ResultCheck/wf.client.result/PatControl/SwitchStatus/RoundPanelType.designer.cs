using Lis.CustomControls;

namespace dcl.client.result.PatControl
{
    partial class RoundPanelType
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
            this.rpChecked = new Lis.CustomControls.RoundPanel();
            this.rpUrgentInvestigation = new Lis.CustomControls.RoundPanel();
            this.rpPhysicalExam = new Lis.CustomControls.RoundPanel();
            this.rpIutpatient = new Lis.CustomControls.RoundPanel();
            this.rpOutpatient = new Lis.CustomControls.RoundPanel();
            this.rpAll = new Lis.CustomControls.RoundPanel();
            this.autoFixSonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // autoFixSonPanel1
            // 
            this.autoFixSonPanel1.AutoFixdirection = Lis.CustomControls.AutoFixSonPanel.Fixdirection.Horizontal;
            this.autoFixSonPanel1.AutoSetFixSon = true;
            this.autoFixSonPanel1.BackColor = System.Drawing.Color.Transparent;
            this.autoFixSonPanel1.Controls.Add(this.rpChecked);
            this.autoFixSonPanel1.Controls.Add(this.rpUrgentInvestigation);
            this.autoFixSonPanel1.Controls.Add(this.rpPhysicalExam);
            this.autoFixSonPanel1.Controls.Add(this.rpIutpatient);
            this.autoFixSonPanel1.Controls.Add(this.rpOutpatient);
            this.autoFixSonPanel1.Controls.Add(this.rpAll);
            this.autoFixSonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoFixSonPanel1.Location = new System.Drawing.Point(0, 0);
            this.autoFixSonPanel1.Name = "autoFixSonPanel1";
            this.autoFixSonPanel1.Size = new System.Drawing.Size(360, 26);
            this.autoFixSonPanel1.TabIndex = 0;
            // 
            // rpChecked
            // 
            this.rpChecked.AutoSetFont = true;
            this.rpChecked.BeginColor = System.Drawing.Color.LimeGreen;
            this.rpChecked.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpChecked.EndColor = System.Drawing.Color.LimeGreen;
            this.rpChecked.InnerText = "已查";
            this.rpChecked.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpChecked.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpChecked.Location = new System.Drawing.Point(300, 0);
            this.rpChecked.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpChecked.Name = "rpChecked";
            this.rpChecked.Radius = 1;
            this.rpChecked.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpChecked.Size = new System.Drawing.Size(60, 26);
            this.rpChecked.TabIndex = 11;
            this.rpChecked.Tag = "111";
            // 
            // rpUrgentInvestigation
            // 
            this.rpUrgentInvestigation.AutoSetFont = true;
            this.rpUrgentInvestigation.BeginColor = System.Drawing.Color.LimeGreen;
            this.rpUrgentInvestigation.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpUrgentInvestigation.EndColor = System.Drawing.Color.LimeGreen;
            this.rpUrgentInvestigation.InnerText = "急查";
            this.rpUrgentInvestigation.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpUrgentInvestigation.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpUrgentInvestigation.Location = new System.Drawing.Point(240, 0);
            this.rpUrgentInvestigation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpUrgentInvestigation.Name = "rpUrgentInvestigation";
            this.rpUrgentInvestigation.Radius = 1;
            this.rpUrgentInvestigation.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpUrgentInvestigation.Size = new System.Drawing.Size(60, 26);
            this.rpUrgentInvestigation.TabIndex = 10;
            this.rpUrgentInvestigation.Tag = "110";
            // 
            // rpPhysicalExam
            // 
            this.rpPhysicalExam.AutoSetFont = true;
            this.rpPhysicalExam.BeginColor = System.Drawing.Color.LimeGreen;
            this.rpPhysicalExam.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpPhysicalExam.EndColor = System.Drawing.Color.LimeGreen;
            this.rpPhysicalExam.InnerText = "体检";
            this.rpPhysicalExam.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpPhysicalExam.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpPhysicalExam.Location = new System.Drawing.Point(180, 0);
            this.rpPhysicalExam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpPhysicalExam.Name = "rpPhysicalExam";
            this.rpPhysicalExam.Radius = 1;
            this.rpPhysicalExam.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpPhysicalExam.Size = new System.Drawing.Size(60, 26);
            this.rpPhysicalExam.TabIndex = 9;
            this.rpPhysicalExam.Tag = "109";
            // 
            // rpIutpatient
            // 
            this.rpIutpatient.AutoSetFont = true;
            this.rpIutpatient.BeginColor = System.Drawing.Color.LimeGreen;
            this.rpIutpatient.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpIutpatient.EndColor = System.Drawing.Color.LimeGreen;
            this.rpIutpatient.InnerText = "住院";
            this.rpIutpatient.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpIutpatient.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpIutpatient.Location = new System.Drawing.Point(120, 0);
            this.rpIutpatient.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpIutpatient.Name = "rpIutpatient";
            this.rpIutpatient.Radius = 1;
            this.rpIutpatient.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpIutpatient.Size = new System.Drawing.Size(60, 26);
            this.rpIutpatient.TabIndex = 8;
            this.rpIutpatient.Tag = "108";
            // 
            // rpOutpatient
            // 
            this.rpOutpatient.AutoSetFont = true;
            this.rpOutpatient.BeginColor = System.Drawing.Color.LimeGreen;
            this.rpOutpatient.Dock = System.Windows.Forms.DockStyle.Left;
            this.rpOutpatient.EndColor = System.Drawing.Color.LimeGreen;
            this.rpOutpatient.InnerText = "门诊";
            this.rpOutpatient.InnerTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.rpOutpatient.InnerTextFont = new System.Drawing.Font("微软雅黑", 24F);
            this.rpOutpatient.Location = new System.Drawing.Point(60, 0);
            this.rpOutpatient.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rpOutpatient.Name = "rpOutpatient";
            this.rpOutpatient.Radius = 1;
            this.rpOutpatient.RoundeStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.rpOutpatient.Size = new System.Drawing.Size(60, 26);
            this.rpOutpatient.TabIndex = 7;
            this.rpOutpatient.Tag = "107";
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
            this.rpAll.Size = new System.Drawing.Size(60, 26);
            this.rpAll.TabIndex = 6;
            this.rpAll.Tag = "0";
            // 
            // RoundPanelType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoFixSonPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "RoundPanelType";
            this.Size = new System.Drawing.Size(360, 26);
            this.autoFixSonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AutoFixSonPanel autoFixSonPanel1;
        private RoundPanel rpAll;
        private RoundPanel rpOutpatient;
        private RoundPanel rpIutpatient;
        private RoundPanel rpPhysicalExam;
        private RoundPanel rpUrgentInvestigation;
        private RoundPanel rpChecked;
    }
}
