namespace dcl.client.sample
{
    partial class FrmBCSignIn
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
            dcl.client.sample.SamplingStep samplingStep1 = new dcl.client.sample.SamplingStep();
            dcl.client.sample.CoolStepController coolStepController1 = new dcl.client.sample.CoolStepController();
            dcl.client.sample.RenStep renStep1 = new dcl.client.sample.RenStep();
            dcl.client.sample.NoStepController noStepController1 = new dcl.client.sample.NoStepController();
            dcl.client.sample.SendStep sendStep1 = new dcl.client.sample.SendStep();
            dcl.client.sample.CoolStepController coolStepController2 = new dcl.client.sample.CoolStepController();
            dcl.client.sample.ReachStep reachStep1 = new dcl.client.sample.ReachStep();
            dcl.client.sample.CoolStepController coolStepController3 = new dcl.client.sample.CoolStepController();
            dcl.client.sample.ReceiveStep receiveStep1 = new dcl.client.sample.ReceiveStep();
            dcl.client.sample.CoolStepController coolStepController4 = new dcl.client.sample.CoolStepController();
            dcl.client.sample.SecondSendStep secondSendStep1 = new dcl.client.sample.SecondSendStep();
            dcl.client.sample.NoStepController noStepController2 = new dcl.client.sample.NoStepController();
            dcl.client.sample.InLabStep inLabStep1 = new dcl.client.sample.InLabStep();
            dcl.client.sample.NoStepController noStepController3 = new dcl.client.sample.NoStepController();
            dcl.client.sample.HandOverStep handOverStep1 = new dcl.client.sample.HandOverStep();
            dcl.client.sample.NoStepController noStepController4 = new dcl.client.sample.NoStepController();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBCSignIn));
            this.bcConfirm1 = new dcl.client.sample.BCConfirm();
            this.tabSign = new DevExpress.XtraTab.XtraTabControl();
            this.tabSampling = new DevExpress.XtraTab.XtraTabPage();
            this.tabRen = new DevExpress.XtraTab.XtraTabPage();
            this.bcConfirm8 = new dcl.client.sample.BCConfirm();
            this.tabSend = new DevExpress.XtraTab.XtraTabPage();
            this.bcConfirm2 = new dcl.client.sample.BCConfirm();
            this.tabReach = new DevExpress.XtraTab.XtraTabPage();
            this.bcConfirm3 = new dcl.client.sample.BCConfirm();
            this.tabComfirm = new DevExpress.XtraTab.XtraTabPage();
            this.bcConfirm4 = new dcl.client.sample.BCConfirm();
            this.tabSecondReach = new DevExpress.XtraTab.XtraTabPage();
            this.bcConfirm5 = new dcl.client.sample.BCConfirm();
            this.tabInLab = new DevExpress.XtraTab.XtraTabPage();
            this.bcConfirm7 = new dcl.client.sample.BCConfirm();
            this.tabHaveOver = new DevExpress.XtraTab.XtraTabPage();
            this.bcConfirmHaveOver = new dcl.client.sample.BCConfirm();
            ((System.ComponentModel.ISupportInitialize)(this.tabSign)).BeginInit();
            this.tabSign.SuspendLayout();
            this.tabSampling.SuspendLayout();
            this.tabRen.SuspendLayout();
            this.tabSend.SuspendLayout();
            this.tabReach.SuspendLayout();
            this.tabComfirm.SuspendLayout();
            this.tabSecondReach.SuspendLayout();
            this.tabInLab.SuspendLayout();
            this.tabHaveOver.SuspendLayout();
            this.SuspendLayout();
            // 
            // bcConfirm1
            // 
            this.bcConfirm1.AutoSize = true;
            this.bcConfirm1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bcConfirm1.BaseInfoContainer = null;
            this.bcConfirm1.BaseInfoContainerExt = null;
            this.bcConfirm1.ControlsEnable = false;
            this.bcConfirm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcConfirm1.DoctorCode = null;
            this.bcConfirm1.DoctorName = null;
            this.bcConfirm1.IsActionSuccess = true;
            this.bcConfirm1.Location = new System.Drawing.Point(0, 0);
            this.bcConfirm1.Margin = new System.Windows.Forms.Padding(6);
            this.bcConfirm1.Name = "bcConfirm1";
            this.bcConfirm1.NameControl = null;
            this.bcConfirm1.Size = new System.Drawing.Size(1160, 843);
            this.bcConfirm1.SpellControl = null;
            samplingStep1.BaseSampMain = null;
            samplingStep1.Bcfrequency = null;
            samplingStep1.EnabledFowardMinutes = false;
            samplingStep1.FowardMinutes = 0;
            samplingStep1.MustFinishPreviousAction = true;
            samplingStep1.Printer = null;
            samplingStep1.ShouldDoAction = true;
            samplingStep1.ShouldEnabledBarcodeInput = true;
            samplingStep1.ShouldEnlableSimpleSearchPanel = true;
            coolStepController1.MustFinishPreviousAction = true;
            coolStepController1.ShouldDoAction = true;
            samplingStep1.StepController = coolStepController1;
            samplingStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcConfirm1.Step = samplingStep1;
            this.bcConfirm1.StepType = dcl.client.sample.StepType.Sampling;
            this.bcConfirm1.TabIndex = 0;
            this.bcConfirm1.ThisForm = null;
            this.bcConfirm1.WuBiControl = null;
            // 
            // tabSign
            // 
            this.tabSign.AppearancePage.Header.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.tabSign.AppearancePage.Header.Options.UseFont = true;
            this.tabSign.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSign.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Bottom;
            this.tabSign.HeaderOrientation = DevExpress.XtraTab.TabOrientation.Horizontal;
            this.tabSign.Location = new System.Drawing.Point(0, 0);
            this.tabSign.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabSign.Name = "tabSign";
            this.tabSign.RightToLeftLayout = DevExpress.Utils.DefaultBoolean.True;
            this.tabSign.SelectedTabPage = this.tabSampling;
            this.tabSign.Size = new System.Drawing.Size(1167, 879);
            this.tabSign.TabIndex = 1;
            this.tabSign.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.tabRen,
            this.tabSampling,
            this.tabSend,
            this.tabReach,
            this.tabComfirm,
            this.tabSecondReach,
            this.tabInLab,
            this.tabHaveOver});
            // 
            // tabSampling
            // 
            this.tabSampling.Controls.Add(this.bcConfirm1);
            this.tabSampling.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabSampling.Name = "tabSampling";
            this.tabSampling.Size = new System.Drawing.Size(1160, 843);
            this.tabSampling.Text = "标本采集";
            // 
            // tabRen
            // 
            this.tabRen.Controls.Add(this.bcConfirm8);
            this.tabRen.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabRen.Name = "tabRen";
            this.tabRen.Size = new System.Drawing.Size(1160, 843);
            this.tabRen.Text = "耗材领取";
            // 
            // bcConfirm8
            // 
            this.bcConfirm8.AutoSize = true;
            this.bcConfirm8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bcConfirm8.BaseInfoContainer = null;
            this.bcConfirm8.BaseInfoContainerExt = null;
            this.bcConfirm8.ControlsEnable = false;
            this.bcConfirm8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcConfirm8.DoctorCode = null;
            this.bcConfirm8.DoctorName = null;
            this.bcConfirm8.IsActionSuccess = true;
            this.bcConfirm8.Location = new System.Drawing.Point(0, 0);
            this.bcConfirm8.Margin = new System.Windows.Forms.Padding(6);
            this.bcConfirm8.Name = "bcConfirm8";
            this.bcConfirm8.NameControl = null;
            this.bcConfirm8.Size = new System.Drawing.Size(1160, 843);
            this.bcConfirm8.SpellControl = null;
            renStep1.BaseSampMain = null;
            renStep1.Bcfrequency = null;
            renStep1.EnabledFowardMinutes = false;
            renStep1.FowardMinutes = 0;
            renStep1.MustFinishPreviousAction = false;
            renStep1.Printer = null;
            renStep1.ShouldDoAction = false;
            renStep1.ShouldEnabledBarcodeInput = true;
            renStep1.ShouldEnlableSimpleSearchPanel = true;
            noStepController1.MustFinishPreviousAction = false;
            noStepController1.ShouldDoAction = false;
            renStep1.StepController = noStepController1;
            renStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcConfirm8.Step = renStep1;
            this.bcConfirm8.StepType = dcl.client.sample.StepType.Sampling;
            this.bcConfirm8.TabIndex = 2;
            this.bcConfirm8.ThisForm = null;
            this.bcConfirm8.WuBiControl = null;
            // 
            // tabSend
            // 
            this.tabSend.Controls.Add(this.bcConfirm2);
            this.tabSend.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabSend.Name = "tabSend";
            this.tabSend.Size = new System.Drawing.Size(1160, 843);
            this.tabSend.Text = "标本收取";
            // 
            // bcConfirm2
            // 
            this.bcConfirm2.AutoSize = true;
            this.bcConfirm2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bcConfirm2.BaseInfoContainer = null;
            this.bcConfirm2.BaseInfoContainerExt = null;
            this.bcConfirm2.ControlsEnable = false;
            this.bcConfirm2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcConfirm2.DoctorCode = null;
            this.bcConfirm2.DoctorName = null;
            this.bcConfirm2.IsActionSuccess = true;
            this.bcConfirm2.Location = new System.Drawing.Point(0, 0);
            this.bcConfirm2.Margin = new System.Windows.Forms.Padding(6);
            this.bcConfirm2.Name = "bcConfirm2";
            this.bcConfirm2.NameControl = null;
            this.bcConfirm2.Size = new System.Drawing.Size(1160, 843);
            this.bcConfirm2.SpellControl = null;
            sendStep1.BaseSampMain = null;
            sendStep1.Bcfrequency = null;
            sendStep1.EnabledFowardMinutes = false;
            sendStep1.FowardMinutes = 0;
            sendStep1.MustFinishPreviousAction = true;
            sendStep1.Printer = null;
            sendStep1.ShouldDoAction = true;
            sendStep1.ShouldEnabledBarcodeInput = true;
            sendStep1.ShouldEnlableSimpleSearchPanel = true;
            coolStepController2.MustFinishPreviousAction = true;
            coolStepController2.ShouldDoAction = true;
            sendStep1.StepController = coolStepController2;
            sendStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcConfirm2.Step = sendStep1;
            this.bcConfirm2.StepType = dcl.client.sample.StepType.Sampling;
            this.bcConfirm2.TabIndex = 1;
            this.bcConfirm2.ThisForm = null;
            this.bcConfirm2.WuBiControl = null;
            // 
            // tabReach
            // 
            this.tabReach.Controls.Add(this.bcConfirm3);
            this.tabReach.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabReach.Name = "tabReach";
            this.tabReach.Size = new System.Drawing.Size(1160, 843);
            this.tabReach.Text = "标本送达";
            // 
            // bcConfirm3
            // 
            this.bcConfirm3.AutoSize = true;
            this.bcConfirm3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bcConfirm3.BaseInfoContainer = null;
            this.bcConfirm3.BaseInfoContainerExt = null;
            this.bcConfirm3.ControlsEnable = false;
            this.bcConfirm3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcConfirm3.DoctorCode = null;
            this.bcConfirm3.DoctorName = null;
            this.bcConfirm3.IsActionSuccess = true;
            this.bcConfirm3.Location = new System.Drawing.Point(0, 0);
            this.bcConfirm3.Margin = new System.Windows.Forms.Padding(6);
            this.bcConfirm3.Name = "bcConfirm3";
            this.bcConfirm3.NameControl = null;
            this.bcConfirm3.Size = new System.Drawing.Size(1160, 843);
            this.bcConfirm3.SpellControl = null;
            reachStep1.BaseSampMain = null;
            reachStep1.Bcfrequency = null;
            reachStep1.EnabledFowardMinutes = false;
            reachStep1.FowardMinutes = 0;
            reachStep1.MustFinishPreviousAction = true;
            reachStep1.Printer = null;
            reachStep1.ShouldDoAction = true;
            reachStep1.ShouldEnabledBarcodeInput = true;
            reachStep1.ShouldEnlableSimpleSearchPanel = true;
            coolStepController3.MustFinishPreviousAction = true;
            coolStepController3.ShouldDoAction = true;
            reachStep1.StepController = coolStepController3;
            reachStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcConfirm3.Step = reachStep1;
            this.bcConfirm3.StepType = dcl.client.sample.StepType.Sampling;
            this.bcConfirm3.TabIndex = 1;
            this.bcConfirm3.ThisForm = null;
            this.bcConfirm3.WuBiControl = null;
            // 
            // tabComfirm
            // 
            this.tabComfirm.Controls.Add(this.bcConfirm4);
            this.tabComfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabComfirm.Name = "tabComfirm";
            this.tabComfirm.Size = new System.Drawing.Size(1160, 843);
            this.tabComfirm.Text = "标本签收";
            // 
            // bcConfirm4
            // 
            this.bcConfirm4.AutoSize = true;
            this.bcConfirm4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bcConfirm4.BaseInfoContainer = null;
            this.bcConfirm4.BaseInfoContainerExt = null;
            this.bcConfirm4.ControlsEnable = false;
            this.bcConfirm4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcConfirm4.DoctorCode = null;
            this.bcConfirm4.DoctorName = null;
            this.bcConfirm4.IsActionSuccess = true;
            this.bcConfirm4.Location = new System.Drawing.Point(0, 0);
            this.bcConfirm4.Margin = new System.Windows.Forms.Padding(6);
            this.bcConfirm4.Name = "bcConfirm4";
            this.bcConfirm4.NameControl = null;
            this.bcConfirm4.Size = new System.Drawing.Size(1160, 843);
            this.bcConfirm4.SpellControl = null;
            receiveStep1.BaseSampMain = null;
            receiveStep1.Bcfrequency = null;
            receiveStep1.EnabledFowardMinutes = false;
            receiveStep1.FowardMinutes = 0;
            receiveStep1.MustFinishPreviousAction = true;
            receiveStep1.Printer = null;
            receiveStep1.ShouldDoAction = true;
            receiveStep1.ShouldEnabledBarcodeInput = true;
            receiveStep1.ShouldEnlableSimpleSearchPanel = true;
            coolStepController4.MustFinishPreviousAction = true;
            coolStepController4.ShouldDoAction = true;
            receiveStep1.StepController = coolStepController4;
            receiveStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcConfirm4.Step = receiveStep1;
            this.bcConfirm4.StepType = dcl.client.sample.StepType.Sampling;
            this.bcConfirm4.TabIndex = 1;
            this.bcConfirm4.ThisForm = null;
            this.bcConfirm4.WuBiControl = null;
            // 
            // tabSecondReach
            // 
            this.tabSecondReach.Controls.Add(this.bcConfirm5);
            this.tabSecondReach.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabSecondReach.Name = "tabSecondReach";
            this.tabSecondReach.Size = new System.Drawing.Size(1160, 843);
            this.tabSecondReach.Text = "二次送检";
            // 
            // bcConfirm5
            // 
            this.bcConfirm5.AutoSize = true;
            this.bcConfirm5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bcConfirm5.BaseInfoContainer = null;
            this.bcConfirm5.BaseInfoContainerExt = null;
            this.bcConfirm5.ControlsEnable = false;
            this.bcConfirm5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcConfirm5.DoctorCode = null;
            this.bcConfirm5.DoctorName = null;
            this.bcConfirm5.IsActionSuccess = true;
            this.bcConfirm5.Location = new System.Drawing.Point(0, 0);
            this.bcConfirm5.Margin = new System.Windows.Forms.Padding(6);
            this.bcConfirm5.Name = "bcConfirm5";
            this.bcConfirm5.NameControl = null;
            this.bcConfirm5.Size = new System.Drawing.Size(1160, 843);
            this.bcConfirm5.SpellControl = null;
            secondSendStep1.BaseSampMain = null;
            secondSendStep1.Bcfrequency = null;
            secondSendStep1.EnabledFowardMinutes = false;
            secondSendStep1.FowardMinutes = 0;
            secondSendStep1.MustFinishPreviousAction = false;
            secondSendStep1.Printer = null;
            secondSendStep1.ShouldDoAction = false;
            secondSendStep1.ShouldEnabledBarcodeInput = true;
            secondSendStep1.ShouldEnlableSimpleSearchPanel = true;
            noStepController2.MustFinishPreviousAction = false;
            noStepController2.ShouldDoAction = false;
            secondSendStep1.StepController = noStepController2;
            secondSendStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcConfirm5.Step = secondSendStep1;
            this.bcConfirm5.StepType = dcl.client.sample.StepType.Sampling;
            this.bcConfirm5.TabIndex = 2;
            this.bcConfirm5.ThisForm = null;
            this.bcConfirm5.WuBiControl = null;
            // 
            // tabInLab
            // 
            this.tabInLab.Controls.Add(this.bcConfirm7);
            this.tabInLab.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabInLab.Name = "tabInLab";
            this.tabInLab.Size = new System.Drawing.Size(1160, 843);
            this.tabInLab.Text = "标本上机";
            // 
            // bcConfirm7
            // 
            this.bcConfirm7.AutoSize = true;
            this.bcConfirm7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bcConfirm7.BaseInfoContainer = null;
            this.bcConfirm7.BaseInfoContainerExt = null;
            this.bcConfirm7.ControlsEnable = false;
            this.bcConfirm7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcConfirm7.DoctorCode = null;
            this.bcConfirm7.DoctorName = null;
            this.bcConfirm7.IsActionSuccess = true;
            this.bcConfirm7.Location = new System.Drawing.Point(0, 0);
            this.bcConfirm7.Margin = new System.Windows.Forms.Padding(6);
            this.bcConfirm7.Name = "bcConfirm7";
            this.bcConfirm7.NameControl = null;
            this.bcConfirm7.Size = new System.Drawing.Size(1160, 843);
            this.bcConfirm7.SpellControl = null;
            inLabStep1.BaseSampMain = null;
            inLabStep1.Bcfrequency = null;
            inLabStep1.EnabledFowardMinutes = false;
            inLabStep1.FowardMinutes = 0;
            inLabStep1.MustFinishPreviousAction = false;
            inLabStep1.Printer = null;
            inLabStep1.ShouldDoAction = false;
            inLabStep1.ShouldEnabledBarcodeInput = true;
            inLabStep1.ShouldEnlableSimpleSearchPanel = true;
            noStepController3.MustFinishPreviousAction = false;
            noStepController3.ShouldDoAction = false;
            inLabStep1.StepController = noStepController3;
            inLabStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcConfirm7.Step = inLabStep1;
            this.bcConfirm7.StepType = dcl.client.sample.StepType.Sampling;
            this.bcConfirm7.TabIndex = 2;
            this.bcConfirm7.ThisForm = null;
            this.bcConfirm7.WuBiControl = null;
            // 
            // tabHaveOver
            // 
            this.tabHaveOver.Controls.Add(this.bcConfirmHaveOver);
            this.tabHaveOver.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabHaveOver.Name = "tabHaveOver";
            this.tabHaveOver.Size = new System.Drawing.Size(1160, 843);
            this.tabHaveOver.Text = "标本交接";
            // 
            // bcConfirmHaveOver
            // 
            this.bcConfirmHaveOver.AutoSize = true;
            this.bcConfirmHaveOver.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bcConfirmHaveOver.BaseInfoContainer = null;
            this.bcConfirmHaveOver.BaseInfoContainerExt = null;
            this.bcConfirmHaveOver.ControlsEnable = false;
            this.bcConfirmHaveOver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcConfirmHaveOver.DoctorCode = null;
            this.bcConfirmHaveOver.DoctorName = null;
            this.bcConfirmHaveOver.IsActionSuccess = true;
            this.bcConfirmHaveOver.Location = new System.Drawing.Point(0, 0);
            this.bcConfirmHaveOver.Margin = new System.Windows.Forms.Padding(6);
            this.bcConfirmHaveOver.Name = "bcConfirmHaveOver";
            this.bcConfirmHaveOver.NameControl = null;
            this.bcConfirmHaveOver.Size = new System.Drawing.Size(1160, 843);
            this.bcConfirmHaveOver.SpellControl = null;
            handOverStep1.BaseSampMain = null;
            handOverStep1.Bcfrequency = null;
            handOverStep1.EnabledFowardMinutes = false;
            handOverStep1.FowardMinutes = 0;
            handOverStep1.MustFinishPreviousAction = false;
            handOverStep1.Printer = null;
            handOverStep1.ShouldDoAction = false;
            handOverStep1.ShouldEnabledBarcodeInput = true;
            handOverStep1.ShouldEnlableSimpleSearchPanel = true;
            noStepController4.MustFinishPreviousAction = false;
            noStepController4.ShouldDoAction = false;
            handOverStep1.StepController = noStepController4;
            handOverStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcConfirmHaveOver.Step = handOverStep1;
            this.bcConfirmHaveOver.StepType = dcl.client.sample.StepType.Sampling;
            this.bcConfirmHaveOver.TabIndex = 2;
            this.bcConfirmHaveOver.ThisForm = null;
            this.bcConfirmHaveOver.WuBiControl = null;
            // 
            // FrmBCSignIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1167, 879);
            this.Controls.Add(this.tabSign);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.TouchUIMode = DevExpress.Utils.DefaultBoolean.False;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmBCSignIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标本确认";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmBCSignIn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabSign)).EndInit();
            this.tabSign.ResumeLayout(false);
            this.tabSampling.ResumeLayout(false);
            this.tabSampling.PerformLayout();
            this.tabRen.ResumeLayout(false);
            this.tabRen.PerformLayout();
            this.tabSend.ResumeLayout(false);
            this.tabSend.PerformLayout();
            this.tabReach.ResumeLayout(false);
            this.tabReach.PerformLayout();
            this.tabComfirm.ResumeLayout(false);
            this.tabComfirm.PerformLayout();
            this.tabSecondReach.ResumeLayout(false);
            this.tabSecondReach.PerformLayout();
            this.tabInLab.ResumeLayout(false);
            this.tabInLab.PerformLayout();
            this.tabHaveOver.ResumeLayout(false);
            this.tabHaveOver.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BCConfirm bcConfirm1;
        private DevExpress.XtraTab.XtraTabControl tabSign;
        private DevExpress.XtraTab.XtraTabPage tabSampling;
        private DevExpress.XtraTab.XtraTabPage tabSend;
        private DevExpress.XtraTab.XtraTabPage tabReach;
        private DevExpress.XtraTab.XtraTabPage tabComfirm;
        private BCConfirm bcConfirm2;
        private BCConfirm bcConfirm3;
        private BCConfirm bcConfirm4;
        private DevExpress.XtraTab.XtraTabPage tabSecondReach;
        private BCConfirm bcConfirm5;
        private BCConfirm bcConfirm7;
        private BCConfirm bcConfirm8;
        private BCConfirm bcConfirmHaveOver;
        private DevExpress.XtraTab.XtraTabPage tabInLab;
        private DevExpress.XtraTab.XtraTabPage tabRen;
        private DevExpress.XtraTab.XtraTabPage tabHaveOver;
    }
}