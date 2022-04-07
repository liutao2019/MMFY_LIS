using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraLayout;
using DevExpress.Utils.Menu;
using dcl.client.frame;
using dcl.client.frame.runsetting.Lab;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace dcl.client.oa
{
    public class LayoutControlConfigForHand
    {
        LayoutControl layoutControl;
        string moduleName;

        public string TypeID { get; set; }

        public bool AllowCustomize { get; set; }

        public LayoutControlConfigForHand(LayoutControl layout, string currentModuleName)
        {
            if (layout != null && currentModuleName.Trim(null) != string.Empty)
            {
                layoutControl = layout;
                moduleName = currentModuleName;
                AttachConfig();
            }
        }

        public void AttachConfig()
        {

            layoutControl.ShowContextMenu += new LayoutMenuEventHandler(layout_ShowContextMenu);

        }

        public void ApplyConfig()
        {
            if (layoutControl != null&&!string.IsNullOrEmpty(TypeID))
            {
               
              layoutControl.OptionsCustomizationForm.ShowPropertyGrid = true;
              
                layoutControl.RegisterCustomPropertyGridWrapper(typeof(LayoutControlItem), typeof(CustomLayoutControlItemPropertyWrapper));
                layoutControl.RegisterCustomPropertyGridWrapper(typeof(LayoutControlGroup), typeof(CustomLayoutControlGroupPropertyWrapper));

                PatEnterLayoutControlSetting setting = PatEnterLayoutControlSetting.LoadGroup(moduleName, TypeID);
                if (setting != null && setting.LayoutData != null)
                {
                    System.IO.Stream stream = new System.IO.MemoryStream(setting.LayoutData);

                    layoutControl.RestoreLayoutFromStream(stream);
                    stream.Seek(0, System.IO.SeekOrigin.Begin);
                }
            }
        }

        void layout_ShowContextMenu(object sender, LayoutMenuEventArgs e)
        {
            e.Menu.Items[0].Caption = "定制面板";

            if (AllowCustomize || UserInfo.isAdmin)
            {
                bool bHasSavePanel = false;
                bool bHasRestorePanel = false;
                foreach (DXMenuItem item in e.Menu.Items)
                {
                    if (item.Caption == "保存版面")
                    {
                        bHasSavePanel = true;
                    }

                    if (item.Caption == "还原版面")
                    {
                        bHasRestorePanel = true;
                    }

                }
                if (!bHasSavePanel)
                {
                    e.Menu.Items.Add(new DXMenuItem("保存版面", new EventHandler(SaveLayout)));
                }

                if (!bHasRestorePanel)
                {
                    e.Menu.Items.Add(new DXMenuItem("还原版面", new EventHandler(RestoreLayout)));
                }
            }
            else
            {
                e.Allow = false;
            }
        }

        /// <summary>
        /// 用流的形式保存面板布局
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveLayout(object sender, EventArgs e)
        {
            System.IO.Stream stream = new System.IO.MemoryStream();
            layoutControl.SaveLayoutToStream(stream);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            byte[] tplData = new byte[stream.Length];
            stream.Read(tplData, 0, (int)stream.Length);
            stream.Close();

        
                if (lis.client.control.MessageDialog.Show("是否保存/覆盖该面板？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    PatEnterLayoutControlSetting setting = new PatEnterLayoutControlSetting();
                    setting.LayoutData = tplData;
                    PatEnterLayoutControlSetting.SaveGroup(setting, moduleName, TypeID);

                    lis.client.control.MessageDialog.Show("保存成功！", "提示");
                }
           


        }

        private void RestoreLayout(object sender, EventArgs e)
        {
            try
            {
                layoutControl.RestoreDefaultLayout();
              //PatEnterLayoutControlSetting.d(moduleName, UserInfo.loginID);

                lis.client.control.MessageDialog.Show("面板还原将在下一次进入时生效", "提示");
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("操作失败", "提示");
            }
        }
    }

    public class CustomLayoutControlGroupPropertyWrapper : BasePropertyGridObjectWrapper
    {

        public override BasePropertyGridObjectWrapper Clone()
        {
            return (BasePropertyGridObjectWrapper)base.MemberwiseClone();
        }
    }

    /// <summary>
    /// layout面板属性
    /// </summary>
    public class CustomLayoutControlItemPropertyWrapper : BasePropertyGridObjectWrapper
    {
        protected LayoutControlItem Item
        {
            get { return WrappedObject as LayoutControlItem; }
        }
        [DescriptionAttribute("设置控件名称")]
        public string 名称
        {
            get { return Item.Text; }
            set { Item.Text = value; }
        }
        [DescriptionAttribute("设置控件默认值")]
        public string 控件内容
        {
            get { return Item.Control.Text; }
            set { Item.Control.Text = value; }
        }

        [DescriptionAttribute("设置控件字体的样式")]
        public Font 字体样式
        {
            get { return Item.Control.Font; }
            set { Item.Control.Font = value; }
        }

        [DescriptionAttribute("设置控件字体颜色")]
        public Color 字体颜色
        {
            get { return Item.Control.ForeColor; }
            set { Item.Control.ForeColor = value; }
        }

        [DescriptionAttribute("设置控件的索引")]
        public int 索引
        {
            get { return Item.Control.TabIndex; }
            set { Item.Control.TabIndex = value; }
        }
        [DescriptionAttribute("设置控件文本对齐方式")]
        public DevExpress.Utils.Locations 文本对齐方式
        {
            get { return Item.TextLocation; }
            set { Item.TextLocation = value; }
        }
        [DescriptionAttribute("设置控件文本的可见状态")]
        public bool 文本隐藏
        {
            get { return Item.TextVisible; }
            set { Item.TextVisible = value; }
        }

        public override BasePropertyGridObjectWrapper Clone()
        {
            return (BasePropertyGridObjectWrapper)base.MemberwiseClone();
        }
    }
}
