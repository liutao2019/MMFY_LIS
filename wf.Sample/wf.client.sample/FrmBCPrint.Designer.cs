using dcl.entity;

namespace dcl.client.sample
{
    partial class FrmBCPrint
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
            dcl.client.sample.Inpatient inpatient1 = new dcl.client.sample.Inpatient();
            dcl.client.sample.PrintStep printStep1 = new dcl.client.sample.PrintStep();
            dcl.client.sample.CoolStepController coolStepController1 = new dcl.client.sample.CoolStepController();
            dcl.client.sample.OutPaitent outPaitent1 = new dcl.client.sample.OutPaitent();
            dcl.client.sample.PrintStep printStep2 = new dcl.client.sample.PrintStep();
            dcl.client.sample.CoolStepController coolStepController2 = new dcl.client.sample.CoolStepController();
            dcl.client.sample.TJPaitent tjPaitent1 = new dcl.client.sample.TJPaitent();
            dcl.client.sample.PrintStep printStep3 = new dcl.client.sample.PrintStep();
            dcl.client.sample.CoolStepController coolStepController3 = new dcl.client.sample.CoolStepController();
            dcl.client.sample.OutPaitent outPaitent2 = new dcl.client.sample.OutPaitent();
            dcl.client.sample.SamplingStep samplingStep1 = new dcl.client.sample.SamplingStep();
            dcl.client.sample.CoolStepController coolStepController4 = new dcl.client.sample.CoolStepController();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBCPrint));
            this.tabMain = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.bcPrintControl = new dcl.client.sample.BCPrintControl();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.bcPrintControl1 = new dcl.client.sample.BCPrintControl();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.bcManual1 = new dcl.client.sample.BCManual();
            this.xtraTabPage4 = new DevExpress.XtraTab.XtraTabPage();
            this.bcPrintControl2 = new dcl.client.sample.BCPrintControl();
            this.xtraTabPage5 = new DevExpress.XtraTab.XtraTabPage();
            this.queueNumber1 = new dcl.client.sample.QueueNumberControl();
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).BeginInit();
            this.tabMain.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            this.xtraTabPage3.SuspendLayout();
            this.xtraTabPage4.SuspendLayout();
            this.xtraTabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.tabMain.Appearance.Options.UseFont = true;
            this.tabMain.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.tabMain.AppearancePage.Header.Options.UseFont = true;
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.HeaderAutoFill = DevExpress.Utils.DefaultBoolean.False;
            this.tabMain.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabMain.Name = "tabMain";
            this.tabMain.RightToLeftLayout = DevExpress.Utils.DefaultBoolean.True;
            this.tabMain.SelectedTabPage = this.xtraTabPage1;
            this.tabMain.ShowTabHeader = DevExpress.Utils.DefaultBoolean.True;
            this.tabMain.Size = new System.Drawing.Size(1327, 782);
            this.tabMain.TabIndex = 1;
            this.tabMain.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3,
            this.xtraTabPage4,
            this.xtraTabPage5});
            this.tabMain.TabPageWidth = 50;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.bcPrintControl);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1320, 746);
            this.xtraTabPage1.Text = "住院";
            // 
            // bcPrintControl
            // 
            this.bcPrintControl.AutoSize = true;
            this.bcPrintControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bcPrintControl.BaseInfoContainer = null;
            this.bcPrintControl.BaseInfoContainerExt = null;
            this.bcPrintControl.ControlsEnable = false;
            this.bcPrintControl.DeptCode = null;
            this.bcPrintControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcPrintControl.DoctorCode = null;
            this.bcPrintControl.DoctorId = null;
            this.bcPrintControl.DoctorName = null;
            this.bcPrintControl.IsActionSuccess = true;
            this.bcPrintControl.IsAlone = false;
            this.bcPrintControl.IsFormatTJCode = false;
            this.bcPrintControl.Location = new System.Drawing.Point(0, 0);
            this.bcPrintControl.Margin = new System.Windows.Forms.Padding(6);
            this.bcPrintControl.Name = "bcPrintControl";
            this.bcPrintControl.NameControl = null;
            this.bcPrintControl.PatID = null;
            inpatient1.BCDownLoadInfo = null;
            inpatient1.IsAlone = false;
            inpatient1.Printablor = null;
            inpatient1.SignInfo = null;
            this.bcPrintControl.Printer = inpatient1;
            this.bcPrintControl.PrintType = PrintType.Inpatient;
            this.bcPrintControl.ShowReturnMessage = false;
            this.bcPrintControl.ShowReturnMessageMz = false;
            this.bcPrintControl.Size = new System.Drawing.Size(1320, 746);
            this.bcPrintControl.SpellControl = null;
            printStep1.BaseSampMain = null;
            printStep1.Bcfrequency = null;
            printStep1.EnabledFowardMinutes = false;
            printStep1.FowardMinutes = 0;
            printStep1.MustFinishPreviousAction = true;
            printStep1.Printer = null;
            printStep1.ShouldDoAction = true;
            printStep1.ShouldEnabledBarcodeInput = true;
            printStep1.ShouldEnlableSimpleSearchPanel = true;
            coolStepController1.MustFinishPreviousAction = true;
            coolStepController1.ShouldDoAction = true;
            printStep1.StepController = coolStepController1;
            printStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcPrintControl.Step = printStep1;
            this.bcPrintControl.StepType = dcl.client.sample.StepType.Print;
            this.bcPrintControl.TabIndex = 0;
            this.bcPrintControl.WuBiControl = null;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.bcPrintControl1);
            this.xtraTabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1320, 746);
            this.xtraTabPage2.Text = "门诊";
            // 
            // bcPrintControl1
            // 
            this.bcPrintControl1.BaseInfoContainer = null;
            this.bcPrintControl1.BaseInfoContainerExt = null;
            this.bcPrintControl1.ControlsEnable = false;
            this.bcPrintControl1.DeptCode = null;
            this.bcPrintControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcPrintControl1.DoctorCode = null;
            this.bcPrintControl1.DoctorId = null;
            this.bcPrintControl1.DoctorName = null;
            this.bcPrintControl1.IsActionSuccess = true;
            this.bcPrintControl1.IsAlone = false;
            this.bcPrintControl1.IsFormatTJCode = false;
            this.bcPrintControl1.Location = new System.Drawing.Point(0, 0);
            this.bcPrintControl1.Margin = new System.Windows.Forms.Padding(6);
            this.bcPrintControl1.Name = "bcPrintControl1";
            this.bcPrintControl1.NameControl = null;
            this.bcPrintControl1.PatID = null;
            outPaitent1.BCDownLoadInfo = null;
            outPaitent1.IsAlone = false;
            outPaitent1.Printablor = null;
            outPaitent1.SignInfo = null;
            this.bcPrintControl1.Printer = outPaitent1;
            this.bcPrintControl1.PrintType = PrintType.Outpatient;
            this.bcPrintControl1.ShowReturnMessage = false;
            this.bcPrintControl1.ShowReturnMessageMz = false;
            this.bcPrintControl1.Size = new System.Drawing.Size(1320, 746);
            this.bcPrintControl1.SpellControl = null;
            printStep2.BaseSampMain = null;
            printStep2.Bcfrequency = null;
            printStep2.EnabledFowardMinutes = false;
            printStep2.FowardMinutes = 0;
            printStep2.MustFinishPreviousAction = true;
            printStep2.Printer = null;
            printStep2.ShouldDoAction = true;
            printStep2.ShouldEnabledBarcodeInput = true;
            printStep2.ShouldEnlableSimpleSearchPanel = true;
            coolStepController2.MustFinishPreviousAction = true;
            coolStepController2.ShouldDoAction = true;
            printStep2.StepController = coolStepController2;
            printStep2.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcPrintControl1.Step = printStep2;
            this.bcPrintControl1.StepType = dcl.client.sample.StepType.Print;
            this.bcPrintControl1.TabIndex = 1;
            this.bcPrintControl1.WuBiControl = null;
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.bcManual1);
            this.xtraTabPage3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(1320, 746);
            this.xtraTabPage3.Text = "手工";
            // 
            // bcManual1
            // 
            this.bcManual1.DeptCode = null;
            this.bcManual1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcManual1.IsAlone = false;
            this.bcManual1.Location = new System.Drawing.Point(0, 0);
            this.bcManual1.Margin = new System.Windows.Forms.Padding(5);
            this.bcManual1.Name = "bcManual1";
            this.bcManual1.Size = new System.Drawing.Size(1320, 746);
            this.bcManual1.TabIndex = 0;
            // 
            // xtraTabPage4
            // 
            this.xtraTabPage4.Controls.Add(this.bcPrintControl2);
            this.xtraTabPage4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtraTabPage4.Name = "xtraTabPage4";
            this.xtraTabPage4.Size = new System.Drawing.Size(1320, 746);
            this.xtraTabPage4.Text = "体检";
            // 
            // bcPrintControl2
            // 
            this.bcPrintControl2.BaseInfoContainer = null;
            this.bcPrintControl2.BaseInfoContainerExt = null;
            this.bcPrintControl2.ControlsEnable = false;
            this.bcPrintControl2.DeptCode = null;
            this.bcPrintControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcPrintControl2.DoctorCode = null;
            this.bcPrintControl2.DoctorId = null;
            this.bcPrintControl2.DoctorName = null;
            this.bcPrintControl2.IsActionSuccess = true;
            this.bcPrintControl2.IsAlone = false;
            this.bcPrintControl2.IsFormatTJCode = false;
            this.bcPrintControl2.Location = new System.Drawing.Point(0, 0);
            this.bcPrintControl2.Margin = new System.Windows.Forms.Padding(6);
            this.bcPrintControl2.Name = "bcPrintControl2";
            this.bcPrintControl2.NameControl = null;
            this.bcPrintControl2.PatID = null;
            tjPaitent1.BCDownLoadInfo = null;
            tjPaitent1.IsAlone = false;
            tjPaitent1.Printablor = null;
            tjPaitent1.SignInfo = null;
            this.bcPrintControl2.Printer = tjPaitent1;
            this.bcPrintControl2.PrintType = PrintType.TJ;
            this.bcPrintControl2.ShowReturnMessage = false;
            this.bcPrintControl2.ShowReturnMessageMz = false;
            this.bcPrintControl2.Size = new System.Drawing.Size(1320, 746);
            this.bcPrintControl2.SpellControl = null;
            printStep3.BaseSampMain = null;
            printStep3.Bcfrequency = null;
            printStep3.EnabledFowardMinutes = false;
            printStep3.FowardMinutes = 0;
            printStep3.MustFinishPreviousAction = true;
            printStep3.Printer = null;
            printStep3.ShouldDoAction = true;
            printStep3.ShouldEnabledBarcodeInput = true;
            printStep3.ShouldEnlableSimpleSearchPanel = true;
            coolStepController3.MustFinishPreviousAction = true;
            coolStepController3.ShouldDoAction = true;
            printStep3.StepController = coolStepController3;
            printStep3.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcPrintControl2.Step = printStep3;
            this.bcPrintControl2.StepType = dcl.client.sample.StepType.Print;
            this.bcPrintControl2.TabIndex = 2;
            this.bcPrintControl2.WuBiControl = null;
            // 
            // xtraTabPage5
            // 
            this.xtraTabPage5.Controls.Add(this.queueNumber1);
            this.xtraTabPage5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtraTabPage5.Name = "xtraTabPage5";
            this.xtraTabPage5.Size = new System.Drawing.Size(1320, 746);
            this.xtraTabPage5.Text = "叫号";
            // 
            // queueNumber1
            // 
            this.queueNumber1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueNumber1.Location = new System.Drawing.Point(0, 0);
            this.queueNumber1.Margin = new System.Windows.Forms.Padding(5);
            this.queueNumber1.Name = "queueNumber1";
            outPaitent2.BCDownLoadInfo = null;
            outPaitent2.IsAlone = false;
            outPaitent2.Printablor = null;
            outPaitent2.SignInfo = null;
            this.queueNumber1.Printer = outPaitent2;
            this.queueNumber1.Size = new System.Drawing.Size(1320, 746);
            samplingStep1.BaseSampMain = null;
            samplingStep1.Bcfrequency = null;
            samplingStep1.EnabledFowardMinutes = false;
            samplingStep1.FowardMinutes = 0;
            samplingStep1.MustFinishPreviousAction = true;
            samplingStep1.Printer = null;
            samplingStep1.ShouldDoAction = true;
            samplingStep1.ShouldEnabledBarcodeInput = true;
            samplingStep1.ShouldEnlableSimpleSearchPanel = true;
            coolStepController4.MustFinishPreviousAction = true;
            coolStepController4.ShouldDoAction = true;
            samplingStep1.StepController = coolStepController4;
            samplingStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.queueNumber1.Step = samplingStep1;
            this.queueNumber1.StepType = dcl.client.sample.StepType.Sampling;
            this.queueNumber1.TabIndex = 0;
            // 
            // FrmBCPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1327, 782);
            this.Controls.Add(this.tabMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmBCPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "检验申请";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmBCPrint_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmBCPrint_FormClosed);
            this.Load += new System.EventHandler(this.FrmBCPrint_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.FrmBCPrint_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.tabMain)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage1.PerformLayout();
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage3.ResumeLayout(false);
            this.xtraTabPage4.ResumeLayout(false);
            this.xtraTabPage5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraTab.XtraTabControl tabMain;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private BCPrintControl bcPrintControl1;
        private BCManual bcManual1;
        private QueueNumberControl queueNumber1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage4;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage5;
        private BCPrintControl bcPrintControl2;
        private BCPrintControl bcPrintControl;
    }
}