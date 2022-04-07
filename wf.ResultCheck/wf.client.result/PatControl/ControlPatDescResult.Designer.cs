namespace dcl.client.result.PatControl
{
    partial class ControlPatDescResult
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
            this.txtDesc = new DevExpress.XtraEditors.MemoEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSaveAsTemplete = new DevExpress.XtraEditors.SimpleButton();
            this.btnTemplateSelect = new DevExpress.XtraEditors.SimpleButton();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDesc
            // 
            this.txtDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDesc.Location = new System.Drawing.Point(0, 0);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(562, 524);
            this.txtDesc.TabIndex = 150;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnSaveAsTemplete);
            this.panel1.Controls.Add(this.btnTemplateSelect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(562, 30);
            this.panel1.TabIndex = 151;
            // 
            // btnSaveAsTemplete
            // 
            this.btnSaveAsTemplete.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnSaveAsTemplete.Location = new System.Drawing.Point(149, 4);
            this.btnSaveAsTemplete.Name = "btnSaveAsTemplete";
            this.btnSaveAsTemplete.Size = new System.Drawing.Size(136, 23);
            this.btnSaveAsTemplete.TabIndex = 1;
            this.btnSaveAsTemplete.Text = "保存当前内容为模板";
            this.btnSaveAsTemplete.Click += new System.EventHandler(this.btnSaveAsTemplete_Click);
            // 
            // btnTemplateSelect
            // 
            this.btnTemplateSelect.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.btnTemplateSelect.Location = new System.Drawing.Point(7, 4);
            this.btnTemplateSelect.Name = "btnTemplateSelect";
            this.btnTemplateSelect.Size = new System.Drawing.Size(136, 23);
            this.btnTemplateSelect.TabIndex = 0;
            this.btnTemplateSelect.Text = "选择模板";
            this.btnTemplateSelect.Click += new System.EventHandler(this.btnTemplateSelect_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtDesc);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(562, 524);
            this.panel2.TabIndex = 152;
            // 
            // ControlPatDescResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ControlPatDescResult";
            this.Size = new System.Drawing.Size(562, 554);
            this.Load += new System.EventHandler(this.ControlPatDescResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtDesc.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected DevExpress.XtraEditors.MemoEdit txtDesc;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraEditors.SimpleButton btnTemplateSelect;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraEditors.SimpleButton btnSaveAsTemplete;
    }
}
