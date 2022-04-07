namespace Lib.DataInterface.Implement
{
    partial class frmDataInterfaceCommandEditor
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
            this.ctrlDataInterfaceCommandEditor1 = new Lib.DataInterface.Implement.ctrlDataInterfaceCommandEditor();
            this.SuspendLayout();
            // 
            // ctrlDataInterfaceCommandEditor1
            // 
            this.ctrlDataInterfaceCommandEditor1.CanEditModuleName = true;
            this.ctrlDataInterfaceCommandEditor1.DataAccessMode = Lib.DataInterface.Implement.EnumDataAccessMode.DirectToDB;
            this.ctrlDataInterfaceCommandEditor1.DefaultModuleName = null;
            this.ctrlDataInterfaceCommandEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlDataInterfaceCommandEditor1.Font = new System.Drawing.Font("宋体", 10.5F);
            this.ctrlDataInterfaceCommandEditor1.Location = new System.Drawing.Point(0, 0);
            this.ctrlDataInterfaceCommandEditor1.Margin = new System.Windows.Forms.Padding(4);
            this.ctrlDataInterfaceCommandEditor1.Name = "ctrlDataInterfaceCommandEditor1";
            this.ctrlDataInterfaceCommandEditor1.ShowMenubar = true;
            this.ctrlDataInterfaceCommandEditor1.Size = new System.Drawing.Size(957, 568);
            this.ctrlDataInterfaceCommandEditor1.TabIndex = 0;
            // 
            // frmDataInterfaceCommandEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 568);
            this.Controls.Add(this.ctrlDataInterfaceCommandEditor1);
            this.Name = "frmDataInterfaceCommandEditor";
            this.Text = "frmDataInterfaceCommandEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlDataInterfaceCommandEditor ctrlDataInterfaceCommandEditor1;

    }
}