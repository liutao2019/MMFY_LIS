namespace dcl.client.notifyclient
{
    partial class FrmShowBcTATClew
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.scrollingText1 = new lis.client.control.ScrollingTextControl.ScrollingText();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // scrollingText1
            // 
            this.scrollingText1.BackColor = System.Drawing.Color.Moccasin;
            this.scrollingText1.BorderColor = System.Drawing.Color.Black;
            this.scrollingText1.Cursor = System.Windows.Forms.Cursors.Default;
            this.scrollingText1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.scrollingText1.ForegroundBrush = null;
            this.scrollingText1.Location = new System.Drawing.Point(0, 0);
            this.scrollingText1.Name = "scrollingText1";
            this.scrollingText1.ScrollDirection = lis.client.control.ScrollingTextControl.ScrollDirection.RightToLeft;
            this.scrollingText1.ScrollText = "暂无信息";
            this.scrollingText1.ShowBorder = false;
            this.scrollingText1.Size = new System.Drawing.Size(156, 32);
            this.scrollingText1.StopScrollOnMouseOver = false;
            this.scrollingText1.TabIndex = 12;
            this.scrollingText1.TabStop = false;
            this.scrollingText1.TextScrollDistance = 2;
            this.scrollingText1.TextScrollSpeed = 45;
            this.scrollingText1.VerticleTextPosition = lis.client.control.ScrollingTextControl.VerticleTextPosition.Center;
            // 
            // FrmShowBcTATClew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(156, 32);
            this.Controls.Add(this.scrollingText1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmShowBcTATClew";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "TAT提醒";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmShowBcTATClew_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        public lis.client.control.ScrollingTextControl.ScrollingText scrollingText1;
    }
}