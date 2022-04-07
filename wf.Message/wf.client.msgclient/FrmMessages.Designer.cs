namespace dcl.client.msgclient
{
    partial class FrmMessages
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMessages));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.btnSetting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSoundOption = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiWithout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHost = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBugle = new System.Windows.Forms.ToolStripMenuItem();
            this.btnShowOption = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmWinMax = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmWinNor = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMegClose = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClose = new System.Windows.Forms.ToolStripMenuItem();
            this.修改密码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "检验消息";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSetting,
            this.toolStripSeparator2,
            this.btnSoundOption,
            this.btnShowOption,
            this.toolStripSeparator1,
            this.btnMegClose,
            this.btnClose,
            this.修改密码ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(209, 232);
            // 
            // btnSetting
            // 
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(208, 36);
            this.btnSetting.Text = "设置";
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(205, 6);
            // 
            // btnSoundOption
            // 
            this.btnSoundOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiWithout,
            this.tsmiHost,
            this.tsmiBugle});
            this.btnSoundOption.Name = "btnSoundOption";
            this.btnSoundOption.Size = new System.Drawing.Size(208, 36);
            this.btnSoundOption.Text = "声音设置";
            // 
            // tsmiWithout
            // 
            this.tsmiWithout.Name = "tsmiWithout";
            this.tsmiWithout.Size = new System.Drawing.Size(160, 38);
            this.tsmiWithout.Text = "无";
            this.tsmiWithout.Click += new System.EventHandler(this.tsmiWithout_Click);
            // 
            // tsmiHost
            // 
            this.tsmiHost.Name = "tsmiHost";
            this.tsmiHost.Size = new System.Drawing.Size(160, 38);
            this.tsmiHost.Text = "主机";
            this.tsmiHost.Click += new System.EventHandler(this.tsmiHost_Click);
            // 
            // tsmiBugle
            // 
            this.tsmiBugle.Checked = true;
            this.tsmiBugle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiBugle.Name = "tsmiBugle";
            this.tsmiBugle.Size = new System.Drawing.Size(160, 38);
            this.tsmiBugle.Text = "音响";
            this.tsmiBugle.Click += new System.EventHandler(this.tsmiBugle_Click);
            // 
            // btnShowOption
            // 
            this.btnShowOption.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmWinMax,
            this.tsmWinNor});
            this.btnShowOption.Name = "btnShowOption";
            this.btnShowOption.Size = new System.Drawing.Size(208, 36);
            this.btnShowOption.Text = "窗口状态";
            // 
            // tsmWinMax
            // 
            this.tsmWinMax.Name = "tsmWinMax";
            this.tsmWinMax.Size = new System.Drawing.Size(184, 38);
            this.tsmWinMax.Text = "最大化";
            this.tsmWinMax.Click += new System.EventHandler(this.tsmWinMax_Click);
            // 
            // tsmWinNor
            // 
            this.tsmWinNor.Checked = true;
            this.tsmWinNor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmWinNor.Name = "tsmWinNor";
            this.tsmWinNor.Size = new System.Drawing.Size(184, 38);
            this.tsmWinNor.Text = "还   原";
            this.tsmWinNor.Click += new System.EventHandler(this.tsmWinNor_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(205, 6);
            // 
            // btnMegClose
            // 
            this.btnMegClose.Name = "btnMegClose";
            this.btnMegClose.Size = new System.Drawing.Size(208, 36);
            this.btnMegClose.Text = "消息框关闭";
            this.btnMegClose.Click += new System.EventHandler(this.btnMegClose_Click);
            // 
            // btnClose
            // 
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(208, 36);
            this.btnClose.Text = "退出";
            this.btnClose.Visible = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // 修改密码ToolStripMenuItem
            // 
            this.修改密码ToolStripMenuItem.Name = "修改密码ToolStripMenuItem";
            this.修改密码ToolStripMenuItem.Size = new System.Drawing.Size(208, 36);
            this.修改密码ToolStripMenuItem.Text = "修改密码";
            this.修改密码ToolStripMenuItem.Visible = false;
            this.修改密码ToolStripMenuItem.Click += new System.EventHandler(this.修改密码ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 308);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(550, 40);
            this.panel1.TabIndex = 1;
            this.panel1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(460, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "如有疑问，请及时联系检验科(3438)";
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.Color.LightBlue;
            this.listView1.BackgroundImage = global::dcl.client.msgclient.Properties.Resources._18;
            this.listView1.BackgroundImageTiled = true;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(550, 308);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.SmallIcon;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(40, 40);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(550, 72);
            this.webBrowser1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(550, 308);
            this.panel2.TabIndex = 3;
            this.panel2.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.webBrowser1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(550, 72);
            this.panel4.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.btnConfirm);
            this.panel3.Controls.Add(this.txtPwd);
            this.panel3.Controls.Add(this.txtLogin);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 72);
            this.panel3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(550, 236);
            this.panel3.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(12, 142);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(530, 80);
            this.label4.TabIndex = 3;
            this.label4.Text = "*  请查看完病人检验危急报告结果后，输入医生、护士工作站的账号及密码，确认查看后方可自动关闭本界面。";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnConfirm.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.Location = new System.Drawing.Point(620, 10);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(220, 84);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "确认查看";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // txtPwd
            // 
            this.txtPwd.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPwd.Location = new System.Drawing.Point(310, 34);
            this.txtPwd.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(174, 35);
            this.txtPwd.TabIndex = 1;
            this.txtPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPwd_KeyDown);
            // 
            // txtLogin
            // 
            this.txtLogin.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtLogin.Location = new System.Drawing.Point(-142, 34);
            this.txtLogin.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(198, 35);
            this.txtLogin.TabIndex = 1;
            this.txtLogin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(206, 40);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 28);
            this.label3.TabIndex = 0;
            this.label3.Text = "密码：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(-276, 40);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 28);
            this.label2.TabIndex = 0;
            this.label2.Text = "用户名：";
            // 
            // FrmMessages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(550, 348);
            this.ControlBox = false;
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MinimizeBox = false;
            this.Name = "FrmMessages";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "危急值报告及急查报告提示";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMessages_FormClosing);
            this.Load += new System.EventHandler(this.FrmMessages_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.FrmMessages_PreviewKeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btnMegClose;
        private System.Windows.Forms.ToolStripMenuItem btnClose;
        private System.Windows.Forms.ToolStripMenuItem btnSetting;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem btnSoundOption;
        private System.Windows.Forms.ToolStripMenuItem tsmiWithout;
        private System.Windows.Forms.ToolStripMenuItem tsmiHost;
        private System.Windows.Forms.ToolStripMenuItem tsmiBugle;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem btnShowOption;
        private System.Windows.Forms.ToolStripMenuItem tsmWinMax;
        private System.Windows.Forms.ToolStripMenuItem tsmWinNor;
        private System.Windows.Forms.ToolStripMenuItem 修改密码ToolStripMenuItem;
        //private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer1;
    }
}

