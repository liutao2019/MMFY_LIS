using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.entity;

namespace dcl.client.result.PatControl
{
    /// <summary>
    /// 带标题的图像框
    /// </summary>
    public partial class TextPictureBox : UserControl
    {
        public TextPictureBox()
        {
            InitializeComponent();

            //标题文字
            this.Titles = new List<TextPictureBoxTitle>();
            this.BorderColor = Color.Black;
            this.BorderWidth = 0;

            this.DataSource = null;


            toolTip1 = new ToolTip();

            toolTip1.AutoPopDelay = 5000;
            toolTip1.InitialDelay = 100;
            toolTip1.ReshowDelay = 500;
            
            toolTip1.ShowAlways = true;


        }

        ToolTip toolTip1 = null;
        public EntityObrResultImage DataSource { get; set; }

        public EntitySampImage SampDataSource { get; set; }

        private int BorderWidth { get; set; }
        private Color BorderColor { get; set; }

        private void TextPictureBox_Load(object sender, EventArgs e)
        {

            toolTip1.SetToolTip(this.pictureBox1, "双击查看原图");

            //文本区域高度=标题数x30像素
            this.flowLayoutPanel1.Height = this.Titles.Count * 30;

            foreach (TextPictureBoxTitle t in this.Titles)
            {
                Label lb = new Label();
                lb.Text = t.Caption + t.Text;
                lb.Width = this.Width - 10;
                this.flowLayoutPanel1.Controls.Add(lb);
            }


        }

        /// <summary>
        /// 图像
        /// </summary>
        public Image Image
        {
            get
            {
                return this.pictureBox1.Image;
            }
            set
            {
                this.pictureBox1.Image = value;
            }
        }

        public bool CheckState
        {
            get
            {
                return this.chkIsAutoAu.Checked;
            }
        }

        public List<TextPictureBoxTitle> Titles { get; set; }

        /// <summary>
        /// 图像标题
        /// </summary>
        [Serializable]
        public class TextPictureBoxTitle
        {
            public TextPictureBoxTitle(string caption, string text)
            {
                this.Caption = caption;
                this.Text = text;
            }
            public string Caption { get; set; }
            public string Text { get; set; }

            public override string ToString()
            {
                return this.Caption + this.Text;
            }
        }
        public bool CheckVisible
        {
            get
            {
                return this.chkIsAutoAu.Visible;
            }
            set
            {
                this.chkIsAutoAu.Visible = value;
            }
        }

        /// <summary>
        /// 点击PictureBox控件/标题发出当前用户控件的Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        /// <summary>
        /// 点击PictureBox控件/标题发出当前用户控件的Click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayoutPanel1_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        /// <summary>
        /// 点击PictureBox控件/标题发出当前用户控件的DoubleClick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            this.OnDoubleClick(e);
        }

        /// <summary>
        /// 点击PictureBox控件/标题发出当前用户控件的DoubleClick事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void flowLayoutPanel1_DoubleClick(object sender, EventArgs e)
        {
            this.OnDoubleClick(e);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            //if (this.BorderWidth > 0)
            //{
            //    e.Graphics.DrawLine(new Pen(new SolidBrush(this.BorderColor), this.BorderWidth), new Point(0, 0), new Point(0, this.flowLayoutPanel1.Height));
            //    e.Graphics.DrawLine(new Pen(new SolidBrush(this.BorderColor), this.BorderWidth), new Point(0, this.flowLayoutPanel1.Height), new Point(this.flowLayoutPanel1.Width, this.flowLayoutPanel1.Height));
            //    e.Graphics.DrawLine(new Pen(new SolidBrush(this.BorderColor), this.BorderWidth), new Point(this.flowLayoutPanel1.Width, this.flowLayoutPanel1.Height), new Point(this.flowLayoutPanel1.Width, 0));
            //}
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //if (this.BorderWidth > 0)
            //{
            //    e.Graphics.DrawLine(new Pen(new SolidBrush(this.BorderColor), this.BorderWidth), new Point(0, 0), new Point(0, this.pictureBox1.Height));
            //    e.Graphics.DrawLine(new Pen(new SolidBrush(this.BorderColor), this.BorderWidth), new Point(0, this.pictureBox1.Height), new Point(this.pictureBox1.Height, this.flowLayoutPanel1.Width));
            //    e.Graphics.DrawLine(new Pen(new SolidBrush(this.BorderColor), this.BorderWidth), new Point(0, 0), new Point(0, this.pictureBox1.Size.Width));
            //}
        }




        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);

            this.pictureBox1.Invalidate();
            this.flowLayoutPanel1.Invalidate();
        }
    }
}
