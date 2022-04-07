namespace dcl.client.notifyclient
{
    partial class FrmShowLEDMsg
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
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // scrollingText1
            // 
            this.scrollingText1.BackColor = System.Drawing.Color.Black;
            this.scrollingText1.BorderColor = System.Drawing.Color.Black;
            this.scrollingText1.Cursor = System.Windows.Forms.Cursors.Default;
            this.scrollingText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrollingText1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.scrollingText1.ForeColor = System.Drawing.Color.White;
            this.scrollingText1.ForegroundBrush = null;
            this.scrollingText1.Location = new System.Drawing.Point(0, 0);
            this.scrollingText1.Name = "scrollingText1";
            this.scrollingText1.ScrollDirection = lis.client.control.ScrollingTextControl.ScrollDirection.RightToLeft;
            this.scrollingText1.ScrollText = "暂无信息";
            this.scrollingText1.ShowBorder = false;
            this.scrollingText1.Size = new System.Drawing.Size(354, 25);
            this.scrollingText1.StopScrollOnMouseOver = false;
            this.scrollingText1.TabIndex = 13;
            this.scrollingText1.TabStop = false;
            this.scrollingText1.TextScrollDistance = 2;
            this.scrollingText1.TextScrollSpeed = 45;
            this.scrollingText1.VerticleTextPosition = lis.client.control.ScrollingTextControl.VerticleTextPosition.Center;
            this.scrollingText1.DoubleClick += new System.EventHandler(this.scrollingText1_DoubleClick);
            // 
            // FrmShowLEDMsg
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(354, 25);
            this.ControlBox = false;
            this.Controls.Add(this.scrollingText1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmShowLEDMsg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FrmShowLEDMsg";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        public lis.client.control.ScrollingTextControl.ScrollingText scrollingText1;
        private System.Windows.Forms.Timer timer1;
    }
}