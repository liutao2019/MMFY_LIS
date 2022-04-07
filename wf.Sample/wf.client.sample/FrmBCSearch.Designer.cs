namespace dcl.client.sample
{
    partial class FrmBCSearch
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
            dcl.client.sample.SelectStep selectStep1 = new dcl.client.sample.SelectStep();
            dcl.client.sample.CoolStepController coolStepController1 = new dcl.client.sample.CoolStepController();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBCSearch));
            this.bcSearchControl1 = new dcl.client.sample.BCSearchControl();
            this.SuspendLayout();
            // 
            // bcSearchControl1
            // 
            this.bcSearchControl1.BaseInfoContainer = null;
            this.bcSearchControl1.BaseInfoContainerExt = null;
            this.bcSearchControl1.ControlsEnable = false;
            this.bcSearchControl1.DeptCode = null;
            this.bcSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bcSearchControl1.IsActionSuccess = true;
            this.bcSearchControl1.IsForFee = false;
            this.bcSearchControl1.Location = new System.Drawing.Point(0, 0);
            this.bcSearchControl1.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.bcSearchControl1.Name = "bcSearchControl1";
            this.bcSearchControl1.NameControl = null;
            this.bcSearchControl1.Size = new System.Drawing.Size(1618, 879);
            this.bcSearchControl1.SpellControl = null;
            selectStep1.BaseSampMain = null;
            selectStep1.Bcfrequency = null;
            selectStep1.EnabledFowardMinutes = false;
            selectStep1.FowardMinutes = 0;
            selectStep1.MustFinishPreviousAction = true;
            selectStep1.Printer = null;
            selectStep1.ShouldDoAction = true;
            selectStep1.ShouldEnabledBarcodeInput = true;
            selectStep1.ShouldEnlableSimpleSearchPanel = true;
            coolStepController1.MustFinishPreviousAction = true;
            coolStepController1.ShouldDoAction = true;
            selectStep1.StepController = coolStepController1;
            selectStep1.TimeOutValue = new System.DateTime(((long)(0)));
            this.bcSearchControl1.Step = selectStep1;
            this.bcSearchControl1.StepType = dcl.client.sample.StepType.Select;
            this.bcSearchControl1.TabIndex = 0;
            this.bcSearchControl1.WuBiControl = null;
            // 
            // FrmBCSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1618, 879);
            this.Controls.Add(this.bcSearchControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmBCSearch";
            this.Text = "条码查询统计";
            this.ResumeLayout(false);

        }

        #endregion

        private BCSearchControl bcSearchControl1;
    }
}