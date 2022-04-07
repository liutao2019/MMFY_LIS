namespace dcl.client.report
{
    partial class FrmPrintConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrintConfiguration));
            this.lblPrintersName = new DevExpress.XtraEditors.LabelControl();
            this.lblPrinterLocation = new DevExpress.XtraEditors.LabelControl();
            this.gbPrinters = new System.Windows.Forms.GroupBox();
            this.comboBoxPrinter = new System.Windows.Forms.ComboBox();
            this.lblLocation = new DevExpress.XtraEditors.LabelControl();
            this.sysPrints = new dcl.client.common.SysToolBar();
            this.psdPrint = new System.Windows.Forms.PageSetupDialog();
            this.pdPrint = new System.Drawing.Printing.PrintDocument();
            this.rbVertical = new System.Windows.Forms.RadioButton();
            this.rbHorizontal = new System.Windows.Forms.RadioButton();
            this.gbDirection = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbContinuousPrint = new System.Windows.Forms.CheckBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rbPrintWith = new System.Windows.Forms.RadioButton();
            this.rbMachinePlay = new System.Windows.Forms.RadioButton();
            this.gbPrinters.SuspendLayout();
            this.gbDirection.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblPrintersName
            // 
            this.lblPrintersName.Location = new System.Drawing.Point(53, 50);
            this.lblPrintersName.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.lblPrintersName.Name = "lblPrintersName";
            this.lblPrintersName.Size = new System.Drawing.Size(56, 29);
            this.lblPrintersName.TabIndex = 0;
            this.lblPrintersName.Text = "名称:";
            // 
            // lblPrinterLocation
            // 
            this.lblPrinterLocation.Location = new System.Drawing.Point(728, 50);
            this.lblPrinterLocation.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.lblPrinterLocation.Name = "lblPrinterLocation";
            this.lblPrinterLocation.Size = new System.Drawing.Size(56, 29);
            this.lblPrinterLocation.TabIndex = 1;
            this.lblPrinterLocation.Text = "位置:";
            this.lblPrinterLocation.Visible = false;
            // 
            // gbPrinters
            // 
            this.gbPrinters.Controls.Add(this.comboBoxPrinter);
            this.gbPrinters.Controls.Add(this.lblLocation);
            this.gbPrinters.Controls.Add(this.lblPrintersName);
            this.gbPrinters.Controls.Add(this.lblPrinterLocation);
            this.gbPrinters.Location = new System.Drawing.Point(26, 29);
            this.gbPrinters.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.gbPrinters.Name = "gbPrinters";
            this.gbPrinters.Padding = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.gbPrinters.Size = new System.Drawing.Size(824, 120);
            this.gbPrinters.TabIndex = 2;
            this.gbPrinters.TabStop = false;
            this.gbPrinters.Text = "打印机设置";
            // 
            // comboBoxPrinter
            // 
            this.comboBoxPrinter.FormattingEnabled = true;
            this.comboBoxPrinter.Location = new System.Drawing.Point(129, 44);
            this.comboBoxPrinter.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.comboBoxPrinter.Name = "comboBoxPrinter";
            this.comboBoxPrinter.Size = new System.Drawing.Size(581, 37);
            this.comboBoxPrinter.TabIndex = 9;
            this.comboBoxPrinter.SelectedIndexChanged += new System.EventHandler(this.comboBoxPrinter_SelectedIndexChanged);
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(129, 108);
            this.lblLocation.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(0, 29);
            this.lblLocation.TabIndex = 3;
            // 
            // sysPrints
            // 
            this.sysPrints.AutoCloseButton = false;
            this.sysPrints.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysPrints.Location = new System.Drawing.Point(0, 417);
            this.sysPrints.Margin = new System.Windows.Forms.Padding(8, 11, 8, 11);
            this.sysPrints.Name = "sysPrints";
            this.sysPrints.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysPrints.NotWriteLogButtonNameList")));
            this.sysPrints.OrderCustomer = true;
            this.sysPrints.ShowItemToolTips = false;
            this.sysPrints.Size = new System.Drawing.Size(867, 145);
            this.sysPrints.TabIndex = 5;
            this.sysPrints.OnBtnConfirmClicked += new System.EventHandler(this.sysPrints_OnBtnConfirmClicked);
            this.sysPrints.BtnPrintSetClick += new System.EventHandler(this.sysPrints_BtnPrintSetClick);
            // 
            // psdPrint
            // 
            this.psdPrint.Document = this.pdPrint;
            // 
            // rbVertical
            // 
            this.rbVertical.AutoSize = true;
            this.rbVertical.Checked = true;
            this.rbVertical.Location = new System.Drawing.Point(53, 50);
            this.rbVertical.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.rbVertical.Name = "rbVertical";
            this.rbVertical.Size = new System.Drawing.Size(92, 33);
            this.rbVertical.TabIndex = 0;
            this.rbVertical.TabStop = true;
            this.rbVertical.Text = "纵向";
            this.rbVertical.UseVisualStyleBackColor = true;
            this.rbVertical.CheckedChanged += new System.EventHandler(this.rbVertical_CheckedChanged);
            // 
            // rbHorizontal
            // 
            this.rbHorizontal.AutoSize = true;
            this.rbHorizontal.Location = new System.Drawing.Point(467, 50);
            this.rbHorizontal.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.rbHorizontal.Name = "rbHorizontal";
            this.rbHorizontal.Size = new System.Drawing.Size(92, 33);
            this.rbHorizontal.TabIndex = 1;
            this.rbHorizontal.Text = "横向";
            this.rbHorizontal.UseVisualStyleBackColor = true;
            this.rbHorizontal.CheckedChanged += new System.EventHandler(this.rbHorizontal_CheckedChanged);
            // 
            // gbDirection
            // 
            this.gbDirection.Controls.Add(this.rbHorizontal);
            this.gbDirection.Controls.Add(this.rbVertical);
            this.gbDirection.Location = new System.Drawing.Point(26, 163);
            this.gbDirection.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.gbDirection.Name = "gbDirection";
            this.gbDirection.Padding = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.gbDirection.Size = new System.Drawing.Size(824, 116);
            this.gbDirection.TabIndex = 4;
            this.gbDirection.TabStop = false;
            this.gbDirection.Text = "方向";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbContinuousPrint);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.rbPrintWith);
            this.groupBox1.Controls.Add(this.rbMachinePlay);
            this.groupBox1.Location = new System.Drawing.Point(26, 294);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.groupBox1.Size = new System.Drawing.Size(824, 116);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "打印方式";
            // 
            // cbContinuousPrint
            // 
            this.cbContinuousPrint.AutoSize = true;
            this.cbContinuousPrint.Location = new System.Drawing.Point(610, 54);
            this.cbContinuousPrint.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.cbContinuousPrint.Name = "cbContinuousPrint";
            this.cbContinuousPrint.Size = new System.Drawing.Size(168, 33);
            this.cbContinuousPrint.TabIndex = 3;
            this.cbContinuousPrint.Text = "A4连续打印";
            this.cbContinuousPrint.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(467, 50);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(92, 33);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.Text = "自助";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rbPrintWith
            // 
            this.rbPrintWith.AutoSize = true;
            this.rbPrintWith.Location = new System.Drawing.Point(260, 50);
            this.rbPrintWith.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.rbPrintWith.Name = "rbPrintWith";
            this.rbPrintWith.Size = new System.Drawing.Size(92, 33);
            this.rbPrintWith.TabIndex = 1;
            this.rbPrintWith.Text = "套打";
            this.rbPrintWith.UseVisualStyleBackColor = true;
            this.rbPrintWith.CheckedChanged += new System.EventHandler(this.rbPrintWith_CheckedChanged);
            // 
            // rbMachinePlay
            // 
            this.rbMachinePlay.AutoSize = true;
            this.rbMachinePlay.Checked = true;
            this.rbMachinePlay.Location = new System.Drawing.Point(53, 50);
            this.rbMachinePlay.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.rbMachinePlay.Name = "rbMachinePlay";
            this.rbMachinePlay.Size = new System.Drawing.Size(92, 33);
            this.rbMachinePlay.TabIndex = 0;
            this.rbMachinePlay.TabStop = true;
            this.rbMachinePlay.Text = "机打";
            this.rbMachinePlay.UseVisualStyleBackColor = true;
            this.rbMachinePlay.CheckedChanged += new System.EventHandler(this.rbMachinePlay_CheckedChanged);
            // 
            // FrmPrintConfiguration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 562);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sysPrints);
            this.Controls.Add(this.gbDirection);
            this.Controls.Add(this.gbPrinters);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPrintConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "打印设置";
            this.Load += new System.EventHandler(this.FrmPrintConfiguration_Load);
            this.gbPrinters.ResumeLayout(false);
            this.gbPrinters.PerformLayout();
            this.gbDirection.ResumeLayout(false);
            this.gbDirection.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblPrintersName;
        private DevExpress.XtraEditors.LabelControl lblPrinterLocation;
        private System.Windows.Forms.GroupBox gbPrinters;
        private dcl.client.common.SysToolBar sysPrints;
        private System.Windows.Forms.PageSetupDialog psdPrint;
        private System.Drawing.Printing.PrintDocument pdPrint;
        private DevExpress.XtraEditors.LabelControl lblLocation;
        private System.Windows.Forms.RadioButton rbVertical;
        private System.Windows.Forms.RadioButton rbHorizontal;
        private System.Windows.Forms.GroupBox gbDirection;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbPrintWith;
        private System.Windows.Forms.RadioButton rbMachinePlay;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.CheckBox cbContinuousPrint;
        private System.Windows.Forms.ComboBox comboBoxPrinter;
    }
}