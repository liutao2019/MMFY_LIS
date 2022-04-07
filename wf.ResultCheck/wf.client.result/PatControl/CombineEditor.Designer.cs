namespace dcl.client.result.PatControl
{
    partial class CombineEditor
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            this.lbCaption = new DevExpress.XtraEditors.LabelControl();
            this.txtCombineEdit = new DevExpress.XtraEditors.ButtonEdit();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.txtCombineEdit.Properties)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbCaption
            // 
            this.lbCaption.Location = new System.Drawing.Point(5, 4);
            this.lbCaption.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbCaption.Name = "lbCaption";
            this.lbCaption.Size = new System.Drawing.Size(60, 18);
            this.lbCaption.TabIndex = 139;
            this.lbCaption.Text = "组合项目";
            // 
            // txtCombineEdit
            // 
            this.txtCombineEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCombineEdit.EnterMoveNextControl = true;
            this.txtCombineEdit.Location = new System.Drawing.Point(84, 0);
            this.txtCombineEdit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCombineEdit.Name = "txtCombineEdit";
            this.txtCombineEdit.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.txtCombineEdit.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            toolTipTitleItem1.Text = "补全缺失项目";
            superToolTip1.Items.Add(toolTipTitleItem1);
            toolTipTitleItem2.Text = "移除没有结果的项目";
            superToolTip2.Items.Add(toolTipTitleItem2);
            serializableAppearanceObject3.BorderColor = System.Drawing.Color.Gray;
            serializableAppearanceObject3.Options.UseBorderColor = true;
            toolTipTitleItem3.Text = "添加组合";
            superToolTip3.Items.Add(toolTipTitleItem3);
            this.txtCombineEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Up, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, superToolTip1, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, superToolTip2, true),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus, "添加", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, superToolTip3, false)});
            this.txtCombineEdit.Properties.ReadOnly = true;
            this.txtCombineEdit.Size = new System.Drawing.Size(447, 24);
            this.txtCombineEdit.TabIndex = 138;
            this.txtCombineEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtCombineEdit_ButtonClick);
            this.txtCombineEdit.DoubleClick += new System.EventHandler(this.txtCombineEdit_DoubleClick);
            this.txtCombineEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCombineEdit_KeyDown);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.lbCaption);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(84, 26);
            this.panel1.TabIndex = 140;
            // 
            // CombineEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.txtCombineEdit);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "CombineEditor";
            this.Size = new System.Drawing.Size(531, 26);
            this.Load += new System.EventHandler(this.CombineEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtCombineEdit.Properties)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lbCaption;
        private DevExpress.XtraEditors.ButtonEdit txtCombineEdit;
        private System.Windows.Forms.Panel panel1;
    }
}
