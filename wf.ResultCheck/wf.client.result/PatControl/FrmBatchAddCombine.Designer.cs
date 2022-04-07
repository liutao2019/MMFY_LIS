namespace dcl.client.result.PatControl
{
    partial class FrmBatchAddCombine
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem1 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem2 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip3 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem3 = new DevExpress.Utils.ToolTipTitleItem();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SuperToolTip superToolTip4 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipTitleItem toolTipTitleItem4 = new DevExpress.Utils.ToolTipTitleItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBatchAddCombine));
            this.lblDestSidBegin = new DevExpress.XtraEditors.LabelControl();
            this.txtEndNumgoal = new DevExpress.XtraEditors.TextEdit();
            this.txtStartNumgoal = new DevExpress.XtraEditors.TextEdit();
            this.txtCombineEdit = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl16 = new DevExpress.XtraEditors.LabelControl();
            this.label1 = new System.Windows.Forms.Label();
            this.sysToolBar1 = new dcl.client.common.SysToolBar();
            ((System.ComponentModel.ISupportInitialize)(this.txtEndNumgoal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartNumgoal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCombineEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDestSidBegin
            // 
            this.lblDestSidBegin.Appearance.ForeColor = System.Drawing.Color.Black;
            this.lblDestSidBegin.Location = new System.Drawing.Point(72, 92);
            this.lblDestSidBegin.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.lblDestSidBegin.Name = "lblDestSidBegin";
            this.lblDestSidBegin.Size = new System.Drawing.Size(104, 29);
            this.lblDestSidBegin.TabIndex = 20;
            this.lblDestSidBegin.Text = "样  本  号";
            // 
            // txtEndNumgoal
            // 
            this.txtEndNumgoal.EnterMoveNextControl = true;
            this.txtEndNumgoal.Location = new System.Drawing.Point(472, 85);
            this.txtEndNumgoal.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txtEndNumgoal.Name = "txtEndNumgoal";
            this.txtEndNumgoal.Properties.Mask.EditMask = "\\d+";
            this.txtEndNumgoal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtEndNumgoal.Size = new System.Drawing.Size(238, 36);
            this.txtEndNumgoal.TabIndex = 19;
            // 
            // txtStartNumgoal
            // 
            this.txtStartNumgoal.EnterMoveNextControl = true;
            this.txtStartNumgoal.Location = new System.Drawing.Point(195, 85);
            this.txtStartNumgoal.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txtStartNumgoal.Name = "txtStartNumgoal";
            this.txtStartNumgoal.Properties.Mask.EditMask = "\\d+";
            this.txtStartNumgoal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.txtStartNumgoal.Size = new System.Drawing.Size(238, 36);
            this.txtStartNumgoal.TabIndex = 18;
            // 
            // txtCombineEdit
            // 
            this.txtCombineEdit.EnterMoveNextControl = true;
            this.txtCombineEdit.Location = new System.Drawing.Point(195, 193);
            this.txtCombineEdit.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txtCombineEdit.Name = "txtCombineEdit";
            this.txtCombineEdit.Properties.Appearance.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCombineEdit.Properties.Appearance.Options.UseFont = true;
            this.txtCombineEdit.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.txtCombineEdit.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            toolTipTitleItem1.Text = "刷新当前组合对应的项目";
            superToolTip1.Items.Add(toolTipTitleItem1);
            toolTipTitleItem2.Text = "移除没有结果的项目";
            superToolTip2.Items.Add(toolTipTitleItem2);
            serializableAppearanceObject3.BorderColor = System.Drawing.Color.Gray;
            serializableAppearanceObject3.Options.UseBorderColor = true;
            toolTipTitleItem3.Text = "添加组合";
            superToolTip3.Items.Add(toolTipTitleItem3);
            serializableAppearanceObject4.BorderColor = System.Drawing.Color.Gray;
            serializableAppearanceObject4.Options.UseBorderColor = true;
            toolTipTitleItem4.Text = "移除组合";
            superToolTip4.Items.Add(toolTipTitleItem4);
            this.txtCombineEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Up, "刷新当前组合对应的项目", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, superToolTip1, false),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Down, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "", null, superToolTip2, false),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus, "添加", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject3, "", null, superToolTip3, false),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Minus, "移除", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject4, "", null, superToolTip4, false)});
            this.txtCombineEdit.Properties.ReadOnly = true;
            this.txtCombineEdit.Size = new System.Drawing.Size(516, 36);
            this.txtCombineEdit.TabIndex = 33;
            this.txtCombineEdit.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.txtCombineEdit_ButtonClick);
            // 
            // labelControl16
            // 
            this.labelControl16.Location = new System.Drawing.Point(72, 201);
            this.labelControl16.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.labelControl16.Name = "labelControl16";
            this.labelControl16.Size = new System.Drawing.Size(96, 29);
            this.labelControl16.TabIndex = 34;
            this.labelControl16.Text = "组合项目";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(444, 94);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 29);
            this.label1.TabIndex = 35;
            this.label1.Text = "-";
            // 
            // sysToolBar1
            // 
            this.sysToolBar1.AutoCloseButton = true;
            this.sysToolBar1.AutoEnableButtons = false;
            this.sysToolBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sysToolBar1.Location = new System.Drawing.Point(0, 278);
            this.sysToolBar1.Margin = new System.Windows.Forms.Padding(13, 15, 13, 15);
            this.sysToolBar1.Name = "sysToolBar1";
            this.sysToolBar1.NotWriteLogButtonNameList = ((System.Collections.Generic.List<string>)(resources.GetObject("sysToolBar1.NotWriteLogButtonNameList")));
            this.sysToolBar1.OrderCustomer = true;
            this.sysToolBar1.ShowItemToolTips = false;
            this.sysToolBar1.Size = new System.Drawing.Size(789, 145);
            this.sysToolBar1.TabIndex = 36;
            this.sysToolBar1.OnBtnConfirmClicked += new System.EventHandler(this.sysToolBar1_OnBtnConfirmClicked);
            // 
            // FrmBatchAddCombine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 423);
            this.Controls.Add(this.sysToolBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCombineEdit);
            this.Controls.Add(this.labelControl16);
            this.Controls.Add(this.lblDestSidBegin);
            this.Controls.Add(this.txtEndNumgoal);
            this.Controls.Add(this.txtStartNumgoal);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.On;
            this.LookAndFeel.SkinName = "Blue";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "FrmBatchAddCombine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量添加组合";
            this.Load += new System.EventHandler(this.FrmBatchAddCombine_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtEndNumgoal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartNumgoal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCombineEdit.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblDestSidBegin;
        private DevExpress.XtraEditors.TextEdit txtEndNumgoal;
        private DevExpress.XtraEditors.TextEdit txtStartNumgoal;
        private DevExpress.XtraEditors.ButtonEdit txtCombineEdit;
        private DevExpress.XtraEditors.LabelControl labelControl16;
        private System.Windows.Forms.Label label1;
        private dcl.client.common.SysToolBar sysToolBar1;
    }
}