using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.result.CommonPatientInput;
using dcl.client.wcf;
using System.Xml;
using dcl.entity;
using System.Linq;
using System.IO;

namespace dcl.client.result.PatControl
{
    public partial class BarcodeSampleInfo : UserControl
    {
        string strBarCode = string.Empty;
        /// <summary>
        /// 当前选中图像对应的数据行
        /// </summary>
        private EntitySampImage SelectedDataRow { get; set; }
        /// <summary>
        /// 浏览原始图像窗口
        /// </summary>
        PhotoViwer viwer;
        public BarcodeSampleInfo()
        {
            InitializeComponent();
            this.SelectedDataRow = null;
            viwer = new PhotoViwer();
        }

        public void Reset()
        {
            this.gridControlBarcode.DataSource = null;
            this.gridControlAdvice.DataSource = null;
            //this.gridControlImage.DataSource = null;
            this.lbBarcode.Text = "PAT_ID：";
        }

        public void LoadData(string pat_ID, List<EntityPidReportDetail> listDocAdvice,string barcode)
        {
            Reset();

            this.lbBarcode.Text = "PAT_ID：" + pat_ID;
            strBarCode = pat_ID;
            if (!string.IsNullOrEmpty(pat_ID) && pat_ID.Trim() != string.Empty)
            {
                //获取标本流转信息
                ProxySysOperationLog proxyOperLog = new ProxySysOperationLog();
                List<EntitySampProcessDetail> listSampProDetail=new List<EntitySampProcessDetail>();
                listSampProDetail = proxyOperLog.Service.GetSampProcessDetail(pat_ID);
                
                this.gridControlBarcode.DataSource = listSampProDetail;


                this.gridControlAdvice.DataSource = listDocAdvice;

                if (!string.IsNullOrEmpty(barcode))
                {
                    ProxySampOperateDetail proxySamOperDetail = new ProxySampOperateDetail();
                    List<EntitySampOperateDetail> listSampOperDetail = new List<EntitySampOperateDetail>();
                    listSampOperDetail = proxySamOperDetail.Service.GetBarCodeExtend(barcode).OrderByDescending(w => w.RsaScanDate).ToList() ;

                    this.gridControlBarcodeExtend.DataSource = listSampOperDetail;
                    List<EntitySampImage> listSampImage = new List<EntitySampImage>();
                    listSampImage = proxySamOperDetail.Service.GetSampImage(barcode);
                    if (listSampImage.Count > 0)
                    {
                        foreach (var drimage in listSampImage)
                        {
                            if (!string.IsNullOrEmpty(drimage.ImgImageValue.ToString()))
                            {
                                MemoryStream ms = new MemoryStream(drimage.ImgImageValue);

                                if (ms.Length > 0)
                                {
                                    //得到图像信息
                                    Image img = Image.FromStream(ms);
                                    //创建图像预览框
                                    TextPictureBox pb = new TextPictureBox();
                                    pb.Tag = false;
                                    pb.Click += new EventHandler(pb_Click);
                                    pb.DoubleClick += new EventHandler(pb_DoubleClick);
                                    pb.Image = img;
                                    pb.SampDataSource = drimage;
                                    pb.Height = 190;
                                    pb.Width = 190;
                                    //添加标题
                                    pb.Titles.Add(new TextPictureBox.TextPictureBoxTitle("", drimage.ImgImageName));
                                    pb.CheckVisible = false;
                                    this.flowLayoutPanel1.Controls.Add(pb);
                                }

                                ms.Close();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 点击图片,在点中的图片加边框模拟激活效果,图片浏览窗口已存在,把当前点中的图片加载到浏览窗口中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pb_Click(object sender, EventArgs e)
        {
            TextPictureBox currentpictureBox = sender as TextPictureBox;
            foreach (Control ctrl in this.flowLayoutPanel1.Controls)
            {
                if (ctrl is TextPictureBox)
                {
                    TextPictureBox pb = ctrl as TextPictureBox;
                    if (pb == currentpictureBox)
                    {
                        pb.BorderStyle = BorderStyle.FixedSingle;
                        this.SelectedDataRow = pb.SampDataSource;
                        if (this.viwer.Visible == true)
                        {
                            this.viwer.ViewPic(pb);
                        }
                    }
                    else
                    {
                        pb.BorderStyle = BorderStyle.None;
                    }

                    pb.Invalidate();
                }
            }
        }
        /// <summary>
        /// 双击弹出图片浏览窗
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pb_DoubleClick(object sender, EventArgs e)
        {
            if (viwer.Visible == false)
            {
                viwer.ViewPic(sender as TextPictureBox);
                viwer.Show();
            }
        }
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
        }
    }
}
