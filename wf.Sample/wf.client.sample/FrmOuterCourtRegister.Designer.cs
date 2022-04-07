namespace dcl.client.sample
{
    partial class FrmOuterCourtRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOuterCourtRegister));
            dcl.client.sample.ReachStep reachStep1 = new dcl.client.sample.ReachStep();
            dcl.client.sample.CoolStepController coolStepController1 = new dcl.client.sample.CoolStepController();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.bsSampRegister = new System.Windows.Forms.BindingSource(this.components);
            this.bsCName = new System.Windows.Forms.BindingSource(this.components);
            this.bsPatient = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            this.txtBarcode = new DevExpress.XtraEditors.TextEdit();
            this.ctlTimeCountDown1 = new lis.client.control.ctlTimeCountDown();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.lbBarCode = new System.Windows.Forms.Label();
            this.patientControlForMed1 = new dcl.client.sample.PatientControlForMed();
            ((System.ComponentModel.ISupportInitialize)(this.bsSampRegister)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPatient)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "名称";
            this.gridColumn2.FieldName = "RackName";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "编码";
            this.gridColumn1.FieldName = "RackCode";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // bsSampRegister
            // 
            this.bsSampRegister.DataSource = typeof(dcl.entity.EntitySampRegister);
            // 
            // bsCName
            // 
            this.bsCName.DataSource = typeof(dcl.entity.EntitySampDetail);
            // 
            // bsPatient
            // 
            this.bsPatient.DataSource = typeof(dcl.entity.EntitySampMain);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sysToolBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1344, 81);
            this.panel1.TabIndex = 21;
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = false;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.AutoShortCuts = true;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 0);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(1344, 81);
            this.sysToolBar1.TabIndex = 0;
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(143, 38);
            this.txtBarcode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 12F);
            this.txtBarcode.Properties.Appearance.Options.UseFont = true;
            this.txtBarcode.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtBarcode.Properties.NullText = "请输入条码并回车";
            this.txtBarcode.Size = new System.Drawing.Size(195, 28);
            this.txtBarcode.TabIndex = 0;
            this.txtBarcode.ToolTip = "请输入条码并回车";
            // 
            // ctlTimeCountDown1
            // 
            this.ctlTimeCountDown1.BackColor = System.Drawing.Color.Transparent;
            this.ctlTimeCountDown1.Location = new System.Drawing.Point(346, 44);
            this.ctlTimeCountDown1.Margin = new System.Windows.Forms.Padding(5);
            this.ctlTimeCountDown1.Name = "ctlTimeCountDown1";
            this.ctlTimeCountDown1.Size = new System.Drawing.Size(46, 22);
            this.ctlTimeCountDown1.TabIndex = 16;
            this.ctlTimeCountDown1.TimeSeconds = 120;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lbBarCode);
            this.groupControl1.Controls.Add(this.txtBarcode);
            this.groupControl1.Controls.Add(this.ctlTimeCountDown1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 81);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1344, 79);
            this.groupControl1.TabIndex = 24;
            this.groupControl1.Text = "二次登记";
            // 
            // lbBarCode
            // 
            this.lbBarCode.AutoSize = true;
            this.lbBarCode.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBarCode.ForeColor = System.Drawing.Color.YellowGreen;
            this.lbBarCode.Location = new System.Drawing.Point(23, 38);
            this.lbBarCode.Name = "lbBarCode";
            this.lbBarCode.Size = new System.Drawing.Size(114, 28);
            this.lbBarCode.TabIndex = 29;
            this.lbBarCode.Text = "条码号:";
            // 
            // patientControlForMed1
            // 
            this.patientControlForMed1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patientControlForMed1.Location = new System.Drawing.Point(0, 160);
            this.patientControlForMed1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.patientControlForMed1.Name = "patientControlForMed1";
            this.patientControlForMed1.SelectType = dcl.client.sample.SelectType.Create;
            this.patientControlForMed1.SelectWhenNotPrint = false;
            this.patientControlForMed1.ShouldMultiSelect = false;
            this.patientControlForMed1.ShowCollectNotice = false;
            this.patientControlForMed1.Size = new System.Drawing.Size(1344, 767);
            reachStep1.BaseSampMain = null;
            reachStep1.Bcfrequency = null;
            reachStep1.EnabledFowardMinutes = false;
            reachStep1.FowardMinutes = 0;
            reachStep1.MustFinishPreviousAction = true;
            reachStep1.Printer = null;
            reachStep1.ShouldDoAction = true;
            reachStep1.ShouldEnabledBarcodeInput = true;
            reachStep1.ShouldEnlableSimpleSearchPanel = true;
            coolStepController1.MustFinishPreviousAction = true;
            coolStepController1.ShouldDoAction = true;
            reachStep1.StepController = coolStepController1;
            reachStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.patientControlForMed1.Step = reachStep1;
            this.patientControlForMed1.StepType = dcl.client.sample.StepType.Reach;
            this.patientControlForMed1.TabIndex = 25;
            // 
            // FrmOuterCourtRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 927);
            this.Controls.Add(this.patientControlForMed1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmOuterCourtRegister";
            this.Text = "标本二次登记";
            ((System.ComponentModel.ISupportInitialize)(this.bsSampRegister)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsCName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPatient)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtBarcode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource bsCName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.BindingSource bsSampRegister;
        public System.Windows.Forms.BindingSource bsPatient;
        private lis.client.control.ctlTimeCountDown ctlTimeCountDown1;
        public DevExpress.XtraEditors.TextEdit txtBarcode;
        private common.SysToolBar sysToolBar1;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private PatientControlForMed patientControlForMed1;
        private System.Windows.Forms.Label lbBarCode;
    }
}