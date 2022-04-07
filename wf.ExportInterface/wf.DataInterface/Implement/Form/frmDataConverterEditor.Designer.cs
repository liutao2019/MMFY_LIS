namespace Lib.DataInterface.Implement
{
    partial class frmDataConverterEditor
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
            this.ctrlDataConverterEditor1 = new Lib.DataInterface.Implement.ctrlDataConverterEditor();
            this.SuspendLayout();
            // 
            // ctrlDataConverterEditor1
            // 
            this.ctrlDataConverterEditor1.CanEditModuleName = true;
            this.ctrlDataConverterEditor1.DataAccessMode = Lib.DataInterface.Implement.EnumDataAccessMode.DirectToDB;
            this.ctrlDataConverterEditor1.DefaultModuleName = null;
            this.ctrlDataConverterEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlDataConverterEditor1.Font = new System.Drawing.Font("宋体", 10.5F);
            this.ctrlDataConverterEditor1.Location = new System.Drawing.Point(0, 0);
            this.ctrlDataConverterEditor1.Name = "ctrlDataConverterEditor1";
            this.ctrlDataConverterEditor1.ShowMenubar = true;
            this.ctrlDataConverterEditor1.Size = new System.Drawing.Size(807, 508);
            this.ctrlDataConverterEditor1.TabIndex = 0;
            // 
            // frmDataConverterEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 508);
            this.Controls.Add(this.ctrlDataConverterEditor1);
            this.Name = "frmDataConverterEditor";
            this.Text = "frmDataConverterEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlDataConverterEditor ctrlDataConverterEditor1;
    }
}