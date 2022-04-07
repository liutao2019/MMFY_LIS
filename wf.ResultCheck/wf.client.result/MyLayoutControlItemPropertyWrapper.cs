using System;
using System.Collections.Generic;

using System.Text;
using System.ComponentModel;
using DevExpress.XtraLayout;
using System.Drawing;

namespace dcl.client.result
{
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
