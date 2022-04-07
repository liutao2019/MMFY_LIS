namespace wf.client.reagent.ReaControl
{
    partial class ReagentPurchaseEditor
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            this.lbCaption = new DevExpress.XtraEditors.LabelControl();
            this.txtReagentEdit = new DevExpress.XtraEditors.ButtonEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.txtReagentEdit.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbCaption
            // 
            this.lbCaption.Location = new System.Drawing.Point(4, 3);
            this.lbCaption.Name = "lbCaption";
            this.lbCaption.Size = new System.Drawing.Size(48, 14);
            this.lbCaption.TabIndex = 139;
            this.lbCaption.Text = "试剂信息";
            // 
            // txtReagentEdit
            // 
            this.txtReagentEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReagentEdit.EnterMoveNextControl = true;
            this.txtReagentEdit.Location = new System.Drawing.Point(63, 0);
            this.txtReagentEdit.Name = "txtReagentEdit";
            this.txtReagentEdit.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.txtReagentEdit.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            serializableAppearanceObject1.BorderColor = System.Drawing.Color.Gray;
            serializableAppearanceObject1.Options.UseBorderColor = true;
            toolTipTitleItem1.Text = "添加试剂";
            superToolTip1.Items.Add(toolTipTitleItem1);
            this.txtReagentEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus, "添加", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, superToolTip1, false)});
            this.txtReagentEdit.Properties.ReadOnly = true;
            this.txtReagentEdit.Size = new System.Drawing.Size(335, 20);
            this.txtReagentEdit.TabIndex = 138;
            this.txtReagentEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtReagentEdit_ButtonClick);
            this.txtReagentEdit.DoubleClick += new System.EventHandler(this.txtReagentEdit_DoubleClick);
            this.txtReagentEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtReagentEdit_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.lbCaption);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(63, 21);
            this.panel1.TabIndex = 140;
            // 
            // ReagentEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txtReagentEdit);
            this.Controls.Add(this.panel1);
            this.Name = "ReagentEditor";
            this.Size = new System.Drawing.Size(398, 21);
            this.Load += new System.EventHandler(this.ReagentEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtReagentEdit.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lbCaption;
        private DevExpress.XtraEditors.ButtonEdit txtReagentEdit;
        private System.Windows.Forms.Panel panel1;
    }
}
