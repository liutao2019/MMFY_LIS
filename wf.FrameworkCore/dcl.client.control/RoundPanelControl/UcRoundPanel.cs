using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Lis.CustomControls;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

namespace dcl.client.control.RoundPanelControl
{
    public partial class UcRoundPanel : DevExpress.XtraEditors.XtraUserControl
    {
        //定义委托
        public delegate void PanelGroupHandle(object sender, EventArgs e);
        //定义事件
        public event PanelGroupHandle RoundPanelGroupClick;

        private RoundPanel curRoundPanel;

        List<UcRoundPanelItem> _items;

        public List<UcRoundPanelItem> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                DoneRoundPanel(value);
                this.Invalidate();
            }
        }

        private void DoneRoundPanel(List<UcRoundPanelItem> value)
        {
            autopanel.Controls.Clear();

            if (value == null || value.Count == 0)
            {
                return;
            }

            int i = 0;
            foreach (UcRoundPanelItem item in value)
            {
                RoundPanel rp = new RoundPanel();
                rp.AutoSetFont = true;
                rp.BeginColor = item.BeginColor; //System.Drawing.Color.DarkGreen;
                rp.Dock = System.Windows.Forms.DockStyle.Left;
                rp.EndColor = item.EndColor; //System.Drawing.Color.DarkGreen;
                rp.InnerText = item.Caption;
                rp.InnerTextColor = item.CaptionColor;// System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                rp.InnerTextFont = item._Font;//new System.Drawing.Font("微软雅黑", 24F);
                rp.Location = new System.Drawing.Point(0, 0);
                rp.Margin = item.Margin;// new System.Windows.Forms.Padding(3, 4, 3, 4);
                rp.Radius = 1;
                rp.RoundeStyle = item._RoundStyle;// Lis.CustomControls.RoundPanel.RoundStyle.None;
                rp.Size = new System.Drawing.Size(60, 26);
                rp.TabIndex = i;
                rp.Tag = item.Value;
                rp.Click += Rp_Click;
                autopanel.Controls.Add(rp);
                i += 1;
            }
        }

        public UcRoundPanel()
        {
            InitializeComponent();
            _items = new List<UcRoundPanelItem>();
        }

        private void Rp_Click(object sender, EventArgs e)
        {
            GenDefaultColor();
            (sender as RoundPanel).BeginColor = Color.DarkGreen;
            (sender as RoundPanel).EndColor = Color.DarkGreen;
            curRoundPanel = (sender as RoundPanel);
            RoundPanelGroupClick?.Invoke(sender, e);
        }

        private void GenDefaultColor()
        {

            foreach (Control col in autopanel.Controls)
            {
                if (col is RoundPanel)
                {
                    (col as RoundPanel).BeginColor = Color.LimeGreen;
                    (col as RoundPanel).EndColor = Color.LimeGreen;
                }
            }
        }

        public RoundPanel GetCurRoundPanel()
        {
            return curRoundPanel;
        }

        public void SetValue(string value)
        {
            if (Items == null || Items.Count == 0)
                return;
            foreach (Control col in autopanel.Controls)
            {
                if (col is RoundPanel)
                {
                    RoundPanel rp = col as RoundPanel;
                    if (rp.Tag?.ToString() != value)
                    {
                        rp.BeginColor = Color.LimeGreen;
                        rp.EndColor = Color.LimeGreen;
                    }
                    else
                    {
                        rp.BeginColor = Color.DarkGreen;
                        rp.EndColor = Color.DarkGreen;
                        curRoundPanel = rp;
                        RoundPanelGroupClick?.Invoke(rp, null);
                    }

                }
            }
        }



    }

    /// <summary>
    /// 对控件UcRoundPanelItem的Items的子项目进行定制
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(UcRoundPanelItemConverter))]
    public class UcRoundPanelItem
    {
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }

        [XmlIgnore()]
        /// <summary>
        /// 字体
        /// </summary>
        public Font _Font { get; set; }

        [XmlIgnore()]
        /// <summary>
        /// BeginColor
        /// </summary>
        public Color BeginColor { get; set; }

        [XmlIgnore()]
        /// <summary>
        /// EndColor
        /// </summary>
        public Color EndColor { get; set; }

        [XmlIgnore()]
        /// <summary>
        /// 文本颜色
        /// </summary>
        public Color CaptionColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Padding Margin { get; set; }

        public Lis.CustomControls.RoundPanel.RoundStyle _RoundStyle { get; set; }

        [Browsable(false)]
        [XmlElement("BeginColorString")]
        public string BeginColorString
        {
            get
            {
                return SerializeColor(BeginColor);
            }
            set
            {
                BeginColor = DeserializeColor(value);
            }
        }

        [Browsable(false)]
        [XmlElement("EndColorString")]
        public string EndColorString
        {
            get
            {
                return SerializeColor(EndColor);
            }
            set
            {
                EndColor = DeserializeColor(value);
            }
        }

        [Browsable(false)]
        [XmlElement("CaptionColorString")]
        public string CaptionColorString
        {
            get
            {
                return SerializeColor(CaptionColor);
            }
            set
            {
                CaptionColor = DeserializeColor(value);
            }
        }

        [Browsable(false)]
        [XmlElement("X_Font")]
        public XmlFont X_Font
        {
            get
            {
                return SerializeFont(_Font);
            }
            set
            {
                _Font = DeserializeFont(value);
            }
        }


        #region Color对象的序列化与反序列化
        public string SerializeColor(Color color)
        {
            if (color.IsNamedColor)
                return string.Format("{0}:{1}",
                    ColorFormat.NamedColor, color.Name);
            else
                return string.Format("{0}:{1}:{2}:{3}:{4}",
                    ColorFormat.ARGBColor,
                    color.A, color.R, color.G, color.B);

        }

        public Color DeserializeColor(string color)
        {
            byte a, r, g, b;
            string[] pieces = color.Split(new char[] { ':' });
            ColorFormat colorType = (ColorFormat)
                Enum.Parse(typeof(ColorFormat), pieces[0], true);
            switch (colorType)
            {
                case ColorFormat.NamedColor:
                    return Color.FromName(pieces[1]);
                case ColorFormat.ARGBColor:
                    a = byte.Parse(pieces[1]);
                    r = byte.Parse(pieces[2]);
                    g = byte.Parse(pieces[3]);
                    b = byte.Parse(pieces[4]);
                    return Color.FromArgb(a, r, g, b);
            }
            return Color.Empty;
        }
        #endregion

        #region Font的序列化与反序列化
        protected static XmlFont SerializeFont(Font font)
        {
            return new XmlFont(font);
        }



        protected static Font DeserializeFont(XmlFont font)
        {
            return font.ToFont();
        }
        #endregion



        /// <summary>
        /// 赋予初始值
        /// </summary>
        public UcRoundPanelItem()
        {
            this.BeginColor = System.Drawing.Color.DarkGreen;
            this.EndColor = System.Drawing.Color.DarkGreen;
            this.CaptionColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._Font = new System.Drawing.Font("微软雅黑", 24F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._RoundStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.Caption = "";
            this.Value = 0;
        }

        public UcRoundPanelItem(string S1,int S2)
        {
            this.BeginColor = System.Drawing.Color.DarkGreen;
            this.EndColor = System.Drawing.Color.DarkGreen;
            this.CaptionColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._Font = new System.Drawing.Font("微软雅黑", 24F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this._RoundStyle = Lis.CustomControls.RoundPanel.RoundStyle.None;
            this.Caption = S1;
            this.Value = S2;
        }

        public UcRoundPanelItem(string S1, int S2, Font S3,Color S4,Color S5,Color S6, Padding S7,RoundPanel.RoundStyle S8)
        {
            this.BeginColor = S4;
            this.EndColor = S5;
            this.CaptionColor = S6;
            this._Font = S3;
            this.Margin = S7;
            this._RoundStyle = S8;
            this.Caption = S1;
            this.Value = S2;
        }
    }

    public class UcRoundPanelItemConverter : TypeConverter
    {
        /// <summary>
        /// 是否可从指定类型转换到本类型
        /// </summary>
        /// <param name="context">提供组件的上下文信息，例如 组件的容器和属性描述</param>
        /// <param name="sourceType">指定对象的类型</param>
        /// <returns>能够转换返回ture 否则返回false</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(String))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 从指定类型转换到本类型
        /// </summary>
        /// <param name="context">提供组件的上下文信息，例如 组件的容器和属性描述</param>
        /// <param name="culture">本地化对象</param>
        /// <param name="value">欲转换对象</param>
        /// <returns>返回转换对象</returns>
        public override object ConvertFrom(
            ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value
            )
        {
            if (value is String)
            {
                UcRoundPanelItem item = Deserialize(typeof(UcRoundPanelItem), value.ToString()) as UcRoundPanelItem;
                return item;
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// 是否可以转换到指定类型
        /// </summary>
        /// <param name="context">提供组件的上下文信息，例如 组件的容器和属性描述</param>
        /// <param name="destinationType">指定装换对象的类型</param>
        /// <returns>能够转换返回ture 否则返回false</returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(String))
            {
                return true;
            }
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// 转换到指定类型
        /// </summary>
        /// <param name="context">提供组件的上下文信息，例如 组件的容器和属性描述</param>
        /// <param name="culture">本地化对象</param>
        /// <param name="value">待转换对象</param>
        /// <param name="destinationType">指定装换对象的类型</param>
        /// <returns>返回指定类型对象的实例</returns>
        public override object ConvertTo(
            ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value,
            Type destinationType
            )
        {
            String result = "";
            if (destinationType == typeof(String))
            {
                UcRoundPanelItem item = (UcRoundPanelItem)value;
                result = Serializer(typeof(UcRoundPanelItem),item);
                return result;
            }

            // 如果对象类型为InstanceDescriptor
            if (destinationType == typeof(InstanceDescriptor))
            {
                #region 使用反射构造指定类型对象的实例

                UcRoundPanelItem item = (UcRoundPanelItem)value;

                //
                // 获取构造函数信息的对象
                //
                ConstructorInfo ci = typeof(UcRoundPanelItem).GetConstructor(new Type[] { typeof(string), typeof(Int32),
                typeof(Font),typeof(Color),typeof(Color),typeof(Color),typeof(Padding),typeof(RoundPanel.RoundStyle)});

                //
                // 返回可序列化的对象【TypeDescriptor 的若干方法使用 InstanceDescriptor 来表示或实例化对象】
                //
                return new InstanceDescriptor(ci, new object[] { item.Caption, item.Value, item._Font,item.BeginColor,item.EndColor,
                item.CaptionColor,item.Margin,item._RoundStyle});

                #endregion
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public string Serializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            //序列化对象  
            xml.Serialize(Stream, obj);
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();
            sr.Dispose();
            Stream.Dispose();
            return str;
        }

        public object Deserialize(Type type, string xml)
        {
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(type);
                return xmldes.Deserialize(sr);
            }
        }

    }

    public enum ColorFormat
    {
        NamedColor,
        ARGBColor
    }


    public struct XmlFont
    {
        public string FontFamily;
        public GraphicsUnit GraphicsUnit;
        public float Size;
        public FontStyle Style;
        public XmlFont(Font f)
        {
            FontFamily = f.FontFamily.Name;
            GraphicsUnit = f.Unit;
            Size = f.Size;
            Style = f.Style;
        }

        public Font ToFont()
        {
            return new Font(FontFamily, Size, Style, GraphicsUnit);
        }

    }

}
