namespace dcl.client.result.PatControl
{
    partial class RoundPanelStatusCommon
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
            this.autopanel = new Lis.CustomControls.AutoFixSonPanel();
            this.SuspendLayout();
            // 
            // autopanel
            // 
            this.autopanel.AutoFixdirection = Lis.CustomControls.AutoFixSonPanel.Fixdirection.Vertical;
            this.autopanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autopanel.Location = new System.Drawing.Point(0, 0);
            this.autopanel.Name = "autopanel";
            this.autopanel.Size = new System.Drawing.Size(361, 43);
            this.autopanel.TabIndex = 0;
            // 
            // RoundPanelStatusCommon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autopanel);
            this.Name = "RoundPanelStatusCommon";
            this.Size = new System.Drawing.Size(361, 43);
            this.ResumeLayout(false);

        }

        #endregion

        private Lis.CustomControls.AutoFixSonPanel autopanel;
    }
}
