using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace dcl.client.sample
{
    public partial class frmBloodPicViewer : Form
    {
        public frmBloodPicViewer()
        {
            InitializeComponent();
        }
        public void InitPic(byte[] pic)
        {
            if (pic != null)
            {
                try
                {

                    this.pictureBox1.Image = Image.FromStream(new MemoryStream(pic));
                }
                catch (Exception ex)
                {

                    lis.client.control.MessageDialog.Show("照片已损坏，无法查看！");
                }
            }
        }
    }
}
