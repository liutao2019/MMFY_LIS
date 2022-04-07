namespace dcl.client.result
{
    partial class FrmImagView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImagView));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRotation = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.nextPic = new DevExpress.XtraEditors.SimpleButton();
            this.lastPic = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRotation);
            this.panel1.Controls.Add(this.simpleButton2);
            this.panel1.Controls.Add(this.nextPic);
            this.panel1.Controls.Add(this.lastPic);
            this.panel1.Controls.Add(this.simpleButton1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 865);
            this.panel1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1424, 104);
            this.panel1.TabIndex = 0;
            // 
            // btnRotation
            // 
            this.btnRotation.Location = new System.Drawing.Point(392, 14);
            this.btnRotation.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.btnRotation.Name = "btnRotation";
            this.btnRotation.Size = new System.Drawing.Size(139, 48);
            this.btnRotation.TabIndex = 2;
            this.btnRotation.Text = "旋转";
            this.btnRotation.Click += new System.EventHandler(this.btnRotation_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(542, 14);
            this.simpleButton2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(139, 48);
            this.simpleButton2.TabIndex = 1;
            this.simpleButton2.Text = "缩小";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // nextPic
            // 
            this.nextPic.Location = new System.Drawing.Point(994, 14);
            this.nextPic.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.nextPic.Name = "nextPic";
            this.nextPic.Size = new System.Drawing.Size(139, 48);
            this.nextPic.TabIndex = 0;
            this.nextPic.Text = "下一张";
            this.nextPic.Click += new System.EventHandler(this.nextPic_Click);
            // 
            // lastPic
            // 
            this.lastPic.Location = new System.Drawing.Point(843, 14);
            this.lastPic.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lastPic.Name = "lastPic";
            this.lastPic.Size = new System.Drawing.Size(139, 48);
            this.lastPic.TabIndex = 0;
            this.lastPic.Text = "上一张";
            this.lastPic.Click += new System.EventHandler(this.lastPic_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(693, 14);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(139, 48);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "放大";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click_1);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureEdit1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1424, 865);
            this.panel2.TabIndex = 1;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit1.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.AllowScrollOnMouseWheel = DevExpress.Utils.DefaultBoolean.True;
            this.pictureEdit1.Properties.AllowScrollViaMouseDrag = true;
            this.pictureEdit1.Properties.AllowZoomOnMouseWheel = DevExpress.Utils.DefaultBoolean.True;
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Always;
            this.pictureEdit1.Properties.ShowScrollBars = true;
            this.pictureEdit1.Size = new System.Drawing.Size(1424, 865);
            this.pictureEdit1.TabIndex = 0;
            // 
            // FrmImagView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1424, 969);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "FrmImagView";
            this.Text = "图像预览";
            this.Load += new System.EventHandler(this.FrmImagView_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton nextPic;
        private DevExpress.XtraEditors.SimpleButton lastPic;
        private DevExpress.XtraEditors.SimpleButton btnRotation;
    }
}