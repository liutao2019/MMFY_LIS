namespace dcl.client.samstock
{
    partial class FrmPrintTempBarCode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrintTempBarCode));
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dateOpTime = new DevExpress.XtraEditors.DateEdit();
            this.txtCtype = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtBarcode = new DevExpress.XtraEditors.TextEdit();
            this.btnPrint = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateOpTime.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateOpTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCtype.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(39, 44);
            this.labelControl9.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(75, 18);
            this.labelControl9.TabIndex = 15;
            this.labelControl9.Text = "架子条码：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dateOpTime);
            this.groupBox1.Controls.Add(this.txtCtype);
            this.groupBox1.Controls.Add(this.labelControl2);
            this.groupBox1.Controls.Add(this.labelControl3);
            this.groupBox1.Controls.Add(this.txtBarcode);
            this.groupBox1.Controls.Add(this.labelControl9);
            this.groupBox1.Location = new System.Drawing.Point(16, 18);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(327, 188);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置";
            // 
            // dateOpTime
            // 
            this.dateOpTime.EditValue = null;
            this.dateOpTime.Location = new System.Drawing.Point(128, 123);
            this.dateOpTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dateOpTime.Name = "dateOpTime";
            this.dateOpTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateOpTime.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateOpTime.Size = new System.Drawing.Size(191, 24);
            this.dateOpTime.TabIndex = 23;
            // 
            // txtCtype
            // 
            this.txtCtype.Location = new System.Drawing.Point(127, 82);
            this.txtCtype.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCtype.Name = "txtCtype";
            this.txtCtype.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtCtype.Properties.Appearance.Options.UseBackColor = true;
            this.txtCtype.Properties.ReadOnly = true;
            this.txtCtype.Size = new System.Drawing.Size(191, 24);
            this.txtCtype.TabIndex = 22;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(34, 127);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(75, 18);
            this.labelControl2.TabIndex = 20;
            this.labelControl2.Text = "归档时间：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(50, 87);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 18);
            this.labelControl3.TabIndex = 19;
            this.labelControl3.Text = "实验组：";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(127, 39);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtBarcode.Properties.Appearance.Options.UseBackColor = true;
            this.txtBarcode.Properties.ReadOnly = true;
            this.txtBarcode.Size = new System.Drawing.Size(191, 24);
            this.txtBarcode.TabIndex = 18;
            // 
            // btnPrint
            // 
            this.btnPrint.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnPrint.Location = new System.Drawing.Point(144, 224);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(99, 35);
            this.btnPrint.TabIndex = 17;
            this.btnPrint.Text = "打印";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.simpleButton1.Location = new System.Drawing.Point(251, 224);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(90, 35);
            this.simpleButton1.TabIndex = 18;
            this.simpleButton1.Text = "取消";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // FrmPrintTempBarCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 276);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "FrmPrintTempBarCode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmPrintTempBarCode";
            this.Shown += new System.EventHandler(this.FrmPrintTempBarCode_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dateOpTime.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateOpTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCtype.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl9;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevExpress.XtraEditors.SimpleButton btnPrint;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit txtBarcode;
        private DevExpress.XtraEditors.TextEdit txtCtype;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.DateEdit dateOpTime;
    }
}