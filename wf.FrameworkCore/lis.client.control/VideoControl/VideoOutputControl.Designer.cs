namespace lis.client.control
{
    partial class VideoOutputControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnCloseCamera = new DevExpress.XtraEditors.SimpleButton();
            this.btnOpenCamera = new DevExpress.XtraEditors.SimpleButton();
            this.btnTruncate = new DevExpress.XtraEditors.SimpleButton();
            this.btnSetFormat = new DevExpress.XtraEditors.SimpleButton();
            this.btnSetCameraImage = new DevExpress.XtraEditors.SimpleButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureEdit1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(640, 510);
            this.panel1.TabIndex = 0;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit1.Location = new System.Drawing.Point(0, 0);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Size = new System.Drawing.Size(640, 510);
            this.pictureEdit1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCloseCamera);
            this.panel2.Controls.Add(this.btnOpenCamera);
            this.panel2.Controls.Add(this.btnTruncate);
            this.panel2.Controls.Add(this.btnSetFormat);
            this.panel2.Controls.Add(this.btnSetCameraImage);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 450);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(640, 60);
            this.panel2.TabIndex = 1;
            // 
            // btnCloseCamera
            // 
            this.btnCloseCamera.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseCamera.Appearance.Options.UseFont = true;
            this.btnCloseCamera.Location = new System.Drawing.Point(336, 3);
            this.btnCloseCamera.Name = "btnCloseCamera";
            this.btnCloseCamera.Size = new System.Drawing.Size(75, 54);
            this.btnCloseCamera.TabIndex = 0;
            this.btnCloseCamera.Text = "关闭视频";
            this.btnCloseCamera.Click += new System.EventHandler(this.btnCloseCamera_Click);
            // 
            // btnOpenCamera
            // 
            this.btnOpenCamera.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenCamera.Appearance.Options.UseFont = true;
            this.btnOpenCamera.Location = new System.Drawing.Point(255, 3);
            this.btnOpenCamera.Name = "btnOpenCamera";
            this.btnOpenCamera.Size = new System.Drawing.Size(75, 54);
            this.btnOpenCamera.TabIndex = 0;
            this.btnOpenCamera.Text = "打开视频";
            this.btnOpenCamera.Click += new System.EventHandler(this.btnOpenCamera_Click);
            // 
            // btnTruncate
            // 
            this.btnTruncate.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTruncate.Appearance.Options.UseFont = true;
            this.btnTruncate.Location = new System.Drawing.Point(174, 3);
            this.btnTruncate.Name = "btnTruncate";
            this.btnTruncate.Size = new System.Drawing.Size(75, 54);
            this.btnTruncate.TabIndex = 0;
            this.btnTruncate.Text = "截图";
            this.btnTruncate.Click += new System.EventHandler(this.btnTruncate_Click);
            // 
            // btnSetFormat
            // 
            this.btnSetFormat.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetFormat.Appearance.Options.UseFont = true;
            this.btnSetFormat.Location = new System.Drawing.Point(93, 3);
            this.btnSetFormat.Name = "btnSetFormat";
            this.btnSetFormat.Size = new System.Drawing.Size(75, 54);
            this.btnSetFormat.TabIndex = 0;
            this.btnSetFormat.Text = "设置格式";
            this.btnSetFormat.Click += new System.EventHandler(this.btnSetFormat_Click);
            // 
            // btnSetCameraImage
            // 
            this.btnSetCameraImage.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetCameraImage.Appearance.Options.UseFont = true;
            this.btnSetCameraImage.Location = new System.Drawing.Point(12, 3);
            this.btnSetCameraImage.Name = "btnSetCameraImage";
            this.btnSetCameraImage.Size = new System.Drawing.Size(75, 54);
            this.btnSetCameraImage.TabIndex = 0;
            this.btnSetCameraImage.Text = "设置视频";
            this.btnSetCameraImage.Click += new System.EventHandler(this.btnSetCameraImage_Click);
            // 
            // VideoOutputControl
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "VideoOutputControl";
            this.Size = new System.Drawing.Size(640, 510);
            this.Load += new System.EventHandler(this.VideoOutputControl_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton btnCloseCamera;
        private DevExpress.XtraEditors.SimpleButton btnOpenCamera;
        private DevExpress.XtraEditors.SimpleButton btnTruncate;
        private DevExpress.XtraEditors.SimpleButton btnSetFormat;
        private DevExpress.XtraEditors.SimpleButton btnSetCameraImage;

    }
}
