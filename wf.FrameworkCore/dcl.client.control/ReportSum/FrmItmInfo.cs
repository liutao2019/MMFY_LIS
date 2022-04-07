using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.control
{
    public partial class FrmItmInfo : Form
    {
        public FrmItmInfo()
        {
            InitializeComponent();
        }
        public void LoadItem(EntityDicItmItem itm, EntityDicCombine combine)
        {
            this.Text = string.Format("项目信息:{0}({1})", itm.ItmName, itm.ItmEcode);
            StringBuilder sb = new StringBuilder();
            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size+1 , FontStyle.Bold);
            richTextBox1.SelectionBackColor = Color.LightGray;
            string str = string.Format("【{0}临床意义】", combine.ComName);
            if (str.Length < 68)
            {
                string t_count = string.Empty;
                int count = Convert.ToInt32((56 - str.Length) / 3);
                for (int i = 0; i < count; i++)
                {
                    t_count += "\t";
                }
                str = str + t_count;
            }
            this.richTextBox1.AppendText(str + "\r\n\r\n");
            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size, richTextBox1.Font.Style);

            this.richTextBox1.AppendText("   " + itm.ItmMeaning + "\r\n\r\n");

            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size + 1, FontStyle.Bold);
            richTextBox1.SelectionBackColor = Color.FromArgb(235, 236, 239);
            this.richTextBox1.AppendText("【检查目的】" + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n");
            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size, richTextBox1.Font.Style);
            this.richTextBox1.AppendText("   " + itm.ItmContent + "\r\n\r\n");

            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size + 1, FontStyle.Bold);
            richTextBox1.SelectionBackColor = Color.FromArgb(235, 236, 239);
            this.richTextBox1.AppendText("【项目临床意义】" + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n");
            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size, richTextBox1.Font.Style);
            this.richTextBox1.AppendText("   " + combine.ComContent + "\r\n\r\n");

            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size + 1, FontStyle.Bold);
            richTextBox1.SelectionBackColor = Color.FromArgb(235, 236, 239);
            this.richTextBox1.AppendText("【参考范围】" + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n");
            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size, richTextBox1.Font.Style);
            this.richTextBox1.AppendText("   " + itm.ItmRef + "\r\n\r\n");

            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size + 1, FontStyle.Bold);
            richTextBox1.SelectionBackColor = Color.FromArgb(235, 236, 239);
            this.richTextBox1.AppendText("【结果影响因素】" + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n");
            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size, richTextBox1.Font.Style);
            this.richTextBox1.AppendText("   "+combine.ComInfluence + "\r\n");
            this.richTextBox1.AppendText("   "+itm.ResultInfluence + "\r\n\r\n");


            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size + 1, FontStyle.Bold);
            richTextBox1.SelectionBackColor = Color.FromArgb(235, 236, 239);
            this.richTextBox1.AppendText("【采集前注意事项】" + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n");
            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size, richTextBox1.Font.Style);
            this.richTextBox1.AppendText("   " + combine.ComSamNotice1 + "\r\n\r\n");

            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size + 1, FontStyle.Bold);
            richTextBox1.SelectionBackColor = Color.FromArgb(235, 236, 239);
            this.richTextBox1.AppendText("【保存及运输】" + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n");
            richTextBox1.SelectionFont = new Font("宋体", richTextBox1.Font.Size, richTextBox1.Font.Style);
            this.richTextBox1.AppendText("   " + combine.ComSaveNotice1 + "\r\n\r\n");
           
        }
    }
}
