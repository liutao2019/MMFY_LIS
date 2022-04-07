namespace wf.client.reagent
{
    partial class FrmPrintConfigurationV2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrintConfigurationV2));
            this.psdPrint = new System.Windows.Forms.PageSetupDialog();
            this.pdPrint = new System.Drawing.Printing.PrintDocument();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxWay = new System.Windows.Forms.ComboBox();
            this.rdoLabellerPrint = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxReturnPrint = new System.Windows.Forms.ComboBox();
            this.lblPrinter = new System.Windows.Forms.Label();
            this.comboBoxPrinter = new System.Windows.Forms.ComboBox();
            this.lblBPPrintInfo_old = new System.Windows.Forms.Label();
            this.btnBPPrintSetting_old = new System.Windows.Forms.Button();
            this.rdoBPPrint_old = new System.Windows.Forms.RadioButton();
            this.lblBPPrintInfo = new System.Windows.Forms.Label();
            this.btnDriverPrintSetting = new System.Windows.Forms.Button();
            this.btnBPPrintSetting = new System.Windows.Forms.Button();
            this.rdoDriverPrint = new System.Windows.Forms.RadioButton();
            this.rdoBPPrint = new System.Windows.Forms.RadioButton();
            this.sysPrints = new dcl.client.common.SysToolBar();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // psdPrint
            // 
            this.psdPrint.AllowMargins = false;
            this.psdPrint.AllowOrientation = false;
            this.psdPrint.AllowPaper = false;
            this.psdPrint.Document = this.pdPrint;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBoxWay);
            this.groupBox1.Controls.Add(this.rdoLabellerPrint);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxReturnPrint);
            this.groupBox1.Controls.Add(this.lblPrinter);
            this.groupBox1.Controls.Add(this.comboBoxPrinter);
            this.groupBox1.Controls.Add(this.lblBPPrintInfo_old);
            this.groupBox1.Controls.Add(this.btnBPPrintSetting_old);
            this.groupBox1.Controls.Add(this.rdoBPPrint_old);
            this.groupBox1.Controls.Add(this.lblBPPrintInfo);
            this.groupBox1.Controls.Add(this.btnDriverPrintSetting);
            this.groupBox1.Controls.Add(this.btnBPPrintSetting);
            this.groupBox1.Controls.Add(this.rdoDriverPrint);
            this.groupBox1.Controls.Add(this.rdoBPPrint);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(588, 595);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "打印方式";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(281, 405);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 22);
            this.label3.TabIndex = 15;
            this.label3.Text = "调用方式";
            this.label3.Visible = false;
            // 
            // comboBoxWay
            // 
            this.comboBoxWay.FormattingEnabled = true;
            this.comboBoxWay.Items.AddRange(new object[] {
            "DLL",
            "EXE"});
            this.comboBoxWay.Location = new System.Drawing.Point(371, 396);
            this.comboBoxWay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxWay.Name = "comboBoxWay";
            this.comboBoxWay.Size = new System.Drawing.Size(126, 29);
            this.comboBoxWay.TabIndex = 14;
            this.comboBoxWay.Visible = false;
            this.comboBoxWay.SelectedIndexChanged += new System.EventHandler(this.comboBoxWay_SelectedIndexChanged);
            // 
            // rdoLabellerPrint
            // 
            this.rdoLabellerPrint.AutoSize = true;
            this.rdoLabellerPrint.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.rdoLabellerPrint.Location = new System.Drawing.Point(48, 403);
            this.rdoLabellerPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoLabellerPrint.Name = "rdoLabellerPrint";
            this.rdoLabellerPrint.Size = new System.Drawing.Size(121, 26);
            this.rdoLabellerPrint.TabIndex = 13;
            this.rdoLabellerPrint.TabStop = true;
            this.rdoLabellerPrint.Text = "贴标机打印";
            this.rdoLabellerPrint.UseVisualStyleBackColor = true;
            this.rdoLabellerPrint.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(368, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 18);
            this.label2.TabIndex = 12;
            this.label2.Text = "(与条码打印机不同时配置)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(55, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 22);
            this.label1.TabIndex = 11;
            this.label1.Text = "回执打印机";
            // 
            // comboBoxReturnPrint
            // 
            this.comboBoxReturnPrint.FormattingEnabled = true;
            this.comboBoxReturnPrint.Location = new System.Drawing.Point(162, 139);
            this.comboBoxReturnPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxReturnPrint.Name = "comboBoxReturnPrint";
            this.comboBoxReturnPrint.Size = new System.Drawing.Size(202, 29);
            this.comboBoxReturnPrint.TabIndex = 10;
            this.comboBoxReturnPrint.SelectedIndexChanged += new System.EventHandler(this.comboBoxReturnPrint_SelectedIndexChanged);
            // 
            // lblPrinter
            // 
            this.lblPrinter.AutoSize = true;
            this.lblPrinter.Location = new System.Drawing.Point(55, 97);
            this.lblPrinter.Name = "lblPrinter";
            this.lblPrinter.Size = new System.Drawing.Size(100, 22);
            this.lblPrinter.TabIndex = 9;
            this.lblPrinter.Text = "条码打印机";
            // 
            // comboBoxPrinter
            // 
            this.comboBoxPrinter.FormattingEnabled = true;
            this.comboBoxPrinter.Location = new System.Drawing.Point(162, 92);
            this.comboBoxPrinter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxPrinter.Name = "comboBoxPrinter";
            this.comboBoxPrinter.Size = new System.Drawing.Size(202, 29);
            this.comboBoxPrinter.TabIndex = 8;
            this.comboBoxPrinter.SelectedValueChanged += new System.EventHandler(this.comboBoxPrinter_SelectedValueChanged);
            // 
            // lblBPPrintInfo_old
            // 
            this.lblBPPrintInfo_old.AutoSize = true;
            this.lblBPPrintInfo_old.Location = new System.Drawing.Point(87, 344);
            this.lblBPPrintInfo_old.Name = "lblBPPrintInfo_old";
            this.lblBPPrintInfo_old.Size = new System.Drawing.Size(56, 22);
            this.lblBPPrintInfo_old.TabIndex = 7;
            this.lblBPPrintInfo_old.Text = "label1";
            // 
            // btnBPPrintSetting_old
            // 
            this.btnBPPrintSetting_old.Location = new System.Drawing.Point(371, 304);
            this.btnBPPrintSetting_old.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBPPrintSetting_old.Name = "btnBPPrintSetting_old";
            this.btnBPPrintSetting_old.Size = new System.Drawing.Size(128, 38);
            this.btnBPPrintSetting_old.TabIndex = 6;
            this.btnBPPrintSetting_old.Text = "设置";
            this.btnBPPrintSetting_old.UseVisualStyleBackColor = true;
            this.btnBPPrintSetting_old.Click += new System.EventHandler(this.btnBPPrintSetting_old_Click);
            // 
            // rdoBPPrint_old
            // 
            this.rdoBPPrint_old.AutoSize = true;
            this.rdoBPPrint_old.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.rdoBPPrint_old.Location = new System.Drawing.Point(48, 304);
            this.rdoBPPrint_old.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoBPPrint_old.Name = "rdoBPPrint_old";
            this.rdoBPPrint_old.Size = new System.Drawing.Size(153, 26);
            this.rdoBPPrint_old.TabIndex = 5;
            this.rdoBPPrint_old.TabStop = true;
            this.rdoBPPrint_old.Text = "北洋串口(旧版)";
            this.rdoBPPrint_old.UseVisualStyleBackColor = true;
            // 
            // lblBPPrintInfo
            // 
            this.lblBPPrintInfo.AutoSize = true;
            this.lblBPPrintInfo.Location = new System.Drawing.Point(87, 250);
            this.lblBPPrintInfo.Name = "lblBPPrintInfo";
            this.lblBPPrintInfo.Size = new System.Drawing.Size(56, 22);
            this.lblBPPrintInfo.TabIndex = 4;
            this.lblBPPrintInfo.Text = "label1";
            // 
            // btnDriverPrintSetting
            // 
            this.btnDriverPrintSetting.Location = new System.Drawing.Point(371, 52);
            this.btnDriverPrintSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDriverPrintSetting.Name = "btnDriverPrintSetting";
            this.btnDriverPrintSetting.Size = new System.Drawing.Size(128, 38);
            this.btnDriverPrintSetting.TabIndex = 3;
            this.btnDriverPrintSetting.Text = "设置";
            this.btnDriverPrintSetting.UseVisualStyleBackColor = true;
            this.btnDriverPrintSetting.Click += new System.EventHandler(this.btnDriverPrintSetting_Click);
            // 
            // btnBPPrintSetting
            // 
            this.btnBPPrintSetting.Location = new System.Drawing.Point(371, 202);
            this.btnBPPrintSetting.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBPPrintSetting.Name = "btnBPPrintSetting";
            this.btnBPPrintSetting.Size = new System.Drawing.Size(128, 38);
            this.btnBPPrintSetting.TabIndex = 2;
            this.btnBPPrintSetting.Text = "设置";
            this.btnBPPrintSetting.UseVisualStyleBackColor = true;
            this.btnBPPrintSetting.Click += new System.EventHandler(this.btnBPPrintSetting_Click);
            // 
            // rdoDriverPrint
            // 
            this.rdoDriverPrint.AutoSize = true;
            this.rdoDriverPrint.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.rdoDriverPrint.Location = new System.Drawing.Point(48, 52);
            this.rdoDriverPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoDriverPrint.Name = "rdoDriverPrint";
            this.rdoDriverPrint.Size = new System.Drawing.Size(103, 26);
            this.rdoDriverPrint.TabIndex = 1;
            this.rdoDriverPrint.TabStop = true;
            this.rdoDriverPrint.Text = "驱动打印";
            this.rdoDriverPrint.UseVisualStyleBackColor = true;
            this.rdoDriverPrint.CheckedChanged += new System.EventHandler(this.rdoDriverPrint_CheckedChanged);
            // 
            // rdoBPPrint
            // 
            this.rdoBPPrint.AutoSize = true;
            this.rdoBPPrint.Font = new System.Drawing.Font("Tahoma", 10.5F);
            this.rdoBPPrint.Location = new System.Drawing.Point(48, 205);
            this.rdoBPPrint.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoBPPrint.Name = "rdoBPPrint";
            this.rdoBPPrint.Size = new System.Drawing.Size(103, 26);
            this.rdoBPPrint.TabIndex = 0;
            this.rdoBPPrint.TabStop = true;
            this.rdoBPPrint.Text = "北洋串口";
            this.rdoBPPrint.UseVisualStyleBackColor = true;
            // 
            // sysPrints
            // 
            this.sysPrints.AutoCloseButton = true;
            this.sysPrints.AutoEnableButtons = false;
            this.sysPrints.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysPrints.Location = new System.Drawing.Point(0, 595);
            this.sysPrints.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.sysPrints.Name = "sysPrints";
            this.sysPrints.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysPrints.NotWriteLogButtonNameList")));
            this.sysPrints.ShowItemToolTips = false;
            this.sysPrints.Size = new System.Drawing.Size(588, 81);
            this.sysPrints.TabIndex = 5;
            this.sysPrints.OnBtnSaveClicked += new System.EventHandler(this.sysPrints_OnBtnSaveClicked);
            this.sysPrints.BtnPrintSetClick += new System.EventHandler(this.sysPrints_BtnPrintSetClick);
            // 
            // FrmPrintConfigurationV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 676);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sysPrints);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPrintConfigurationV2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "条码打印机设置";
            this.Load += new System.EventHandler(this.FrmPrintConfiguration_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private dcl.client.common.SysToolBar sysPrints;
        private System.Windows.Forms.PageSetupDialog psdPrint;
        private System.Drawing.Printing.PrintDocument pdPrint;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoDriverPrint;
        private System.Windows.Forms.RadioButton rdoBPPrint;
        private System.Windows.Forms.Button btnDriverPrintSetting;
        private System.Windows.Forms.Button btnBPPrintSetting;
        private System.Windows.Forms.Label lblBPPrintInfo;
        private System.Windows.Forms.Label lblBPPrintInfo_old;
        private System.Windows.Forms.Button btnBPPrintSetting_old;
        private System.Windows.Forms.RadioButton rdoBPPrint_old;
        private System.Windows.Forms.Label lblPrinter;
        private System.Windows.Forms.ComboBox comboBoxPrinter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxReturnPrint;
        private System.Windows.Forms.RadioButton rdoLabellerPrint;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxWay;
    }
}