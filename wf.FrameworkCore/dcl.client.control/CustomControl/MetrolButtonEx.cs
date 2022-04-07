using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Lis.CustomControls
{
    public partial class MetrolButtonEx : UserControl
    {

        public MetrolButtonEx()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 控件的默认图片
        /// </summary>
        private Image imageM = null;
        [Description("控件的默认图片")]
        public Image ImageM
        {
            get { return imageM; }
            set
            {
                imageM = value;
                label.Image = imageM;
            }
        }
        /// <summary>
        /// 光标移动到控件上方显示的图片
        /// </summary>
        private Image imageMove = null;
        [Description("光标移动到控件上方显示的图片")]
        public Image ImageMove
        {
            get { return imageMove; }
            set { imageMove = value; }
        }
        /// <summary>
        /// 光标离开控件显示的图片
        /// </summary>
        private Image imageLeave = null;
        [Description("光标离开控件显示的图片")]
        public Image ImageLeave
        {
            get { return imageLeave; }
            set { imageLeave = value; }
        }
        /// <summary>
        /// 控件的背景色
        /// </summary>
        private Color backColorM = Color.Transparent;
        [Description("控件的背景色")]
        public Color BackColorM
        {
            get { return backColorM; }
            set
            {
                backColorM = value;
                label.BackColor = backColorM;
            }
        }
        /// <summary>
        /// 光标移动到控件上方显示的颜色
        /// </summary>
        private Color backColorMove = Color.Transparent;
        [Description("光标移动到控件上方显示的颜色")]
        public Color BackColorMove
        {
            get { return backColorMove; }
            set { backColorMove = value; }
        }
        /// <summary>
        /// 光标离开控件显示的背景色
        /// </summary>
        private Color backColorLeave = Color.Transparent;
        [Description("光标离开控件显示的背景色")]
        public Color BackColorLeave
        {
            get { return backColorLeave; }
            set { backColorLeave = value; }
        }
        /// <summary>
        /// 控件的文字提示
        /// </summary>
        private string textM = "";
        [Description("显示的文字")]
        public string TextM
        {
            get { return textM; }
            set
            {
                textM = value;
                label.Text = textM;
            }
        }
        /// <summary>
        /// 文字的颜色
        /// </summary>
        private Color textColor = Color.Black;
        [Description("文字的颜色")]
        public Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                label.ForeColor = textColor;
            }
        }
        /// <summary>
        /// 用于显示文本的字体
        /// </summary>
        private Font fontM = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        [Description("用于显示文本的字体")]
        public Font FontM
        {
            get { return fontM; }
            set
            {
                fontM = value;
                label.Font = fontM;
            }
        }
        /// <summary>
        /// 单击事件
        /// </summary>
        public event EventHandler ButtonClick;
        /// <summary>
        /// 单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void label_Click(object sender, EventArgs e)
        {
            if (ButtonClick != null)
            {
                ButtonClick(sender, e);
            }
        }

        private void label_MouseMove(object sender, MouseEventArgs e)
        {
            if (backColorMove != Color.Transparent)
            {
                BackColorM = backColorMove;
            }
            if (imageMove != null)
            {
                ImageM = imageMove;
            }
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            if (backColorLeave != Color.Transparent)
            {
                BackColorM = backColorLeave;
            }
            if (imageLeave != null)
            {
                ImageM = imageLeave;
            }
        }
    }
}
