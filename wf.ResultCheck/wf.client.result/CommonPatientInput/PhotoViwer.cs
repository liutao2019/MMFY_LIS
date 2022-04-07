using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.result.PatControl;
using dcl.client.frame;

namespace dcl.client.result.CommonPatientInput
{
    public partial class PhotoViwer : FrmCommon
    {
        public PhotoViwer()
        {
            InitializeComponent();
        }

        public void ViewPic(TextPictureBox pb)
        {
            this.pictureBox1.Width = pb.Image.Width;
            this.pictureBox1.Height = pb.Image.Height;
            this.pictureBox1.Image = pb.Image;
        }

        private void PhotoViwer_Load(object sender, EventArgs e)
        {
            
        }

        private void PhotoViwer_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }


    }
}
