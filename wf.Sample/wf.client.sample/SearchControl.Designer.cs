namespace dcl.client.sample
{
    partial class SearchControl
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
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            this.txtSort = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSort.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl17
            // 
            this.labelControl17.Location = new System.Drawing.Point(6, 6);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(48, 14);
            this.labelControl17.TabIndex = 45;
            this.labelControl17.Text = "数据检索";
            // 
            // txtSort
            // 
            this.txtSort.EnterMoveNextControl = true;
            this.txtSort.Location = new System.Drawing.Point(67, 3);
            this.txtSort.Name = "txtSort";
            this.txtSort.Size = new System.Drawing.Size(122, 21);
            this.txtSort.TabIndex = 46;
            this.txtSort.ToolTip = "空格可支持多词检索，如“生化 细菌”";
            this.txtSort.EditValueChanged += new System.EventHandler(this.txtSort_EditValueChanged);
            // 
            // SearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl17);
            this.Controls.Add(this.txtSort);
            this.Name = "SearchControl";
            this.Size = new System.Drawing.Size(191, 28);
            ((System.ComponentModel.ISupportInitialize)(this.txtSort.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.TextEdit txtSort;
    }
}
