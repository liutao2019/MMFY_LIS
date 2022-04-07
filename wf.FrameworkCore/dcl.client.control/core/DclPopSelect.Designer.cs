namespace dcl.client.control
{
    partial class DclPopSelect<T>
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
            this.popupContainerControl1 = new DevExpress.XtraEditors.PopupContainerControl();
            this.popupContainerEdit1 = new DevExpress.XtraEditors.PopupContainerEdit();
            this.bsSource = new System.Windows.Forms.BindingSource();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).BeginInit();
            this.SuspendLayout();
            // 
            // popupContainerControl1
            // 
            this.popupContainerControl1.Location = new System.Drawing.Point(17, 62);
            this.popupContainerControl1.Name = "popupContainerControl1";
            this.popupContainerControl1.Size = new System.Drawing.Size(432, 264);
            this.popupContainerControl1.TabIndex = 0;
            // 
            // popupContainerEdit1
            // 
            this.popupContainerEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.popupContainerEdit1.Location = new System.Drawing.Point(0, 0);
            this.popupContainerEdit1.Margin = new System.Windows.Forms.Padding(0);
            this.popupContainerEdit1.Name = "popupContainerEdit1";
            this.popupContainerEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Delete, "", 0, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "置为空值", null, null, false)});
            this.popupContainerEdit1.Properties.PopupControl = this.popupContainerControl1;
            this.popupContainerEdit1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.popupContainerEdit1.Size = new System.Drawing.Size(133, 24);
            this.popupContainerEdit1.TabIndex = 1;
            this.popupContainerEdit1.Popup += new System.EventHandler(this.popupContainerEdit1_Popup);
            this.popupContainerEdit1.Closed += new DevExpress.XtraEditors.Controls.ClosedEventHandler(this.popupContainerEdit1_Closed);
            this.popupContainerEdit1.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.popupContainerEdit1_ButtonClick);
            this.popupContainerEdit1.EditValueChanged += new System.EventHandler(this.popupContainerEdit1_EditValueChanged);
            this.popupContainerEdit1.TextChanged += new System.EventHandler(this.popupContainerEdit1_TextChanged);
            this.popupContainerEdit1.Enter += new System.EventHandler(this.popupContainerEdit1_Enter);
            this.popupContainerEdit1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.popupContainerEdit1_KeyDown);
            this.popupContainerEdit1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.popupContainerEdit1_KeyPress);
            this.popupContainerEdit1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.popupContainerEdit1_PreviewKeyDown);
            // 
            // DclPopSelect
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.popupContainerEdit1);
            this.Controls.Add(this.popupContainerControl1);
            this.MaximumSize = new System.Drawing.Size(500, 21);
            this.MinimumSize = new System.Drawing.Size(50, 21);
            this.Name = "DclPopSelect";
            this.Size = new System.Drawing.Size(133, 21);
            this.Load += new System.EventHandler(this.HopePopSelect_Load);
            this.Resize += new System.EventHandler(this.HopePopSelect_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupContainerEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraEditors.PopupContainerEdit popupContainerEdit1;
        public DevExpress.XtraEditors.PopupContainerControl popupContainerControl1;
        private System.Windows.Forms.BindingSource bsSource;
    }
}
