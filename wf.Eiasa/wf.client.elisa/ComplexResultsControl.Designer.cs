namespace dcl.client.elisa
{
    partial class ComplexResultsControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtOD = new DevExpress.XtraEditors.TextEdit();
            this.cbRes = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtOriginal = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOD.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbRes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOriginal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtOD
            // 
            this.txtOD.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtOD.EditValue = "";
            this.txtOD.EnterMoveNextControl = true;
            this.txtOD.Location = new System.Drawing.Point(0, 0);
            this.txtOD.Margin = new System.Windows.Forms.Padding(2);
            this.txtOD.Name = "txtOD";
            this.txtOD.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.txtOD.Properties.Appearance.Options.UseFont = true;
            this.txtOD.Properties.AutoHeight = false;
            this.txtOD.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtOD.Size = new System.Drawing.Size(50, 16);
            this.txtOD.TabIndex = 0;
            // 
            // cbRes
            // 
            this.cbRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbRes.EditValue = "";
            this.cbRes.EnterMoveNextControl = true;
            this.cbRes.Location = new System.Drawing.Point(0, 16);
            this.cbRes.Name = "cbRes";
            this.cbRes.Properties.Appearance.Font = new System.Drawing.Font("宋体", 8F);
            this.cbRes.Properties.Appearance.Options.UseFont = true;
            this.cbRes.Properties.AutoHeight = false;
            this.cbRes.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.cbRes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cbRes.Properties.Items.AddRange(new object[] {
            "-",
            "+",
            "±"});
            this.cbRes.Size = new System.Drawing.Size(50, 24);
            this.cbRes.TabIndex = 1;
            this.cbRes.EditValueChanged += new System.EventHandler(this.cbRes_EditValueChanged);
            // 
            // txtOriginal
            // 
            this.txtOriginal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtOriginal.EditValue = "";
            this.txtOriginal.EnterMoveNextControl = true;
            this.txtOriginal.Location = new System.Drawing.Point(0, 40);
            this.txtOriginal.Margin = new System.Windows.Forms.Padding(2);
            this.txtOriginal.Name = "txtOriginal";
            this.txtOriginal.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.txtOriginal.Properties.Appearance.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtOriginal.Properties.Appearance.Options.UseFont = true;
            this.txtOriginal.Properties.Appearance.Options.UseForeColor = true;
            this.txtOriginal.Properties.AutoHeight = false;
            this.txtOriginal.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.txtOriginal.Size = new System.Drawing.Size(50, 16);
            this.txtOriginal.TabIndex = 2;
            // 
            // ComplexResultsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.cbRes);
            this.Controls.Add(this.txtOD);
            this.Controls.Add(this.txtOriginal);
            this.Font = new System.Drawing.Font("宋体", 10.5F);
            this.Name = "ComplexResultsControl";
            this.Size = new System.Drawing.Size(50, 56);
            ((System.ComponentModel.ISupportInitialize)(this.txtOD.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cbRes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtOriginal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtOD;
        private DevExpress.XtraEditors.ComboBoxEdit cbRes;
        private DevExpress.XtraEditors.TextEdit txtOriginal;
    }
}
