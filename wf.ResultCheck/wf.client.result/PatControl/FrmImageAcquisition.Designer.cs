namespace dcl.client.result.PatControl
{
    partial class FrmImageAcquisition
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImageAcquisition));
            this.tb_light = new System.Windows.Forms.TrackBar();
            this.tb_contrast = new System.Windows.Forms.TrackBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.tb_red = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_blue = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_green = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.videPlayer = new AForge.Controls.VideoSourcePlayer();
            this.panel4 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.tb_light)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_contrast)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_red)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_blue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_green)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_light
            // 
            this.tb_light.Location = new System.Drawing.Point(141, 215);
            this.tb_light.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.tb_light.Maximum = 255;
            this.tb_light.Minimum = -255;
            this.tb_light.Name = "tb_light";
            this.tb_light.Size = new System.Drawing.Size(823, 90);
            this.tb_light.TabIndex = 13;
            // 
            // tb_contrast
            // 
            this.tb_contrast.Location = new System.Drawing.Point(141, 19);
            this.tb_contrast.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.tb_contrast.Maximum = 100;
            this.tb_contrast.Minimum = -100;
            this.tb_contrast.Name = "tb_contrast";
            this.tb_contrast.Size = new System.Drawing.Size(823, 90);
            this.tb_contrast.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.sysToolBar1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 1284);
            this.panel2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(2017, 143);
            this.panel2.TabIndex = 27;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = false;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(2017, 143);
            this.sysToolBar1.TabIndex = 25;
            // 
            // tb_red
            // 
            this.tb_red.Location = new System.Drawing.Point(1031, 19);
            this.tb_red.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.tb_red.Maximum = 255;
            this.tb_red.Name = "tb_red";
            this.tb_red.Size = new System.Drawing.Size(823, 90);
            this.tb_red.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(977, 128);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 29);
            this.label5.TabIndex = 21;
            this.label5.Text = "绿";
            // 
            // tb_blue
            // 
            this.tb_blue.Location = new System.Drawing.Point(1031, 217);
            this.tb_blue.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.tb_blue.Maximum = 255;
            this.tb_blue.Name = "tb_blue";
            this.tb_blue.Size = new System.Drawing.Size(823, 90);
            this.tb_blue.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(977, 228);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 29);
            this.label4.TabIndex = 20;
            this.label4.Text = "蓝";
            // 
            // tb_green
            // 
            this.tb_green.Location = new System.Drawing.Point(1031, 118);
            this.tb_green.Margin = new System.Windows.Forms.Padding(11, 12, 11, 12);
            this.tb_green.Maximum = 255;
            this.tb_green.Name = "tb_green";
            this.tb_green.Size = new System.Drawing.Size(823, 90);
            this.tb_green.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(977, 29);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 29);
            this.label3.TabIndex = 19;
            this.label3.Text = "红";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 29);
            this.label1.TabIndex = 17;
            this.label1.Text = "对比度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(61, 228);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 29);
            this.label2.TabIndex = 18;
            this.label2.Text = "亮度";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            this.splitContainer1.Size = new System.Drawing.Size(2017, 1284);
            this.splitContainer1.SplitterDistance = 948;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 28;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pictureEdit1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(964, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1053, 948);
            this.panel3.TabIndex = 3;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Size = new System.Drawing.Size(1053, 1013);
            this.pictureEdit1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.videPlayer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(964, 948);
            this.panel1.TabIndex = 2;
            // 
            // videPlayer
            // 
            this.videPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videPlayer.Location = new System.Drawing.Point(0, 0);
            this.videPlayer.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.videPlayer.Name = "videPlayer";
            this.videPlayer.Size = new System.Drawing.Size(964, 948);
            this.videPlayer.TabIndex = 1;
            this.videPlayer.Text = "videPlayer";
            this.videPlayer.VideoSource = null;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.tb_red);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.tb_contrast);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.tb_light);
            this.panel4.Controls.Add(this.tb_green);
            this.panel4.Controls.Add(this.tb_blue);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(2017, 326);
            this.panel4.TabIndex = 2;
            // 
            // FrmImageAcquisition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2017, 1427);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.MaximizeBox = false;
            this.Name = "FrmImageAcquisition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图像采集";
            ((System.ComponentModel.ISupportInitialize)(this.tb_light)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_contrast)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tb_red)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_blue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_green)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar tb_light;
        private System.Windows.Forms.TrackBar tb_contrast;
        private System.Windows.Forms.Panel panel2;
        private dcl.client.common.SysToolBar sysToolBar1;
        private System.Windows.Forms.TrackBar tb_red;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar tb_blue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar tb_green;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel1;
        private AForge.Controls.VideoSourcePlayer videPlayer;
    }
}