using System;
using System.Collections.Generic;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.IO;
using dcl.client.result.CommonPatientInput;
using dcl.client.wcf;
using dcl.root.logon;
using dcl.client.frame;
using lis.client.control;
using dcl.client.common;
using dcl.entity;
using O2S.Components.PDFRender4NET;

namespace dcl.client.result.PatControl
{
    /// <summary>
    /// 图像列表控件
    /// 
    /// </summary>
    public partial class PatPhotoList : UserControl
    {
        /// <summary>
        /// 病人ID
        /// </summary>
        public string PatID
        {
            get;
            private set;
        }

        /// <summary>
        /// 浏览原始图像窗口
        /// </summary>
        PhotoViwer viwer;
        List<EntityObrResultImage> DataSource = null;

        public PatPhotoList()
        {
            InitializeComponent();
            this.SelectedDataRow = null;

            viwer = new PhotoViwer();
        }

        public void SetPanelNoVis()
        {
            panel1.Visible = false;
            panel2.Visible = false;
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="dtResultoP"></param>
        public void LoadPhotos(List<EntityObrResultImage> listResultoP, bool showPanel = true)
        {
            Reset();
            DataSource = listResultoP;
            if (listResultoP != null && listResultoP.Count > 0)
            {
                try
                {
                    //panel1为工具栏容器
                    this.panel1.Visible = true;
                    if (!showPanel)
                    {
                        this.panel1.Visible = false;
                        this.panel2.Visible = false;
                    }
                    //遍历图像数据
                    foreach (var drimage in listResultoP)
                    {
                        #region 先前就已经注释的代码
                        //if (drimage["pres_chr"] == null || drimage["pres_chr"] == DBNull.Value)
                        //{
                        //    if (!Compare.IsNullOrDBNull(drimage["pres_base64"]) && drimage["pres_base64"].ToString().Trim() != string.Empty)
                        //    {
                        //        string base64 = drimage["pres_base64"].ToString();
                        //        byte[] bytes = Convert.FromBase64String(base64); 
                        //        MemoryStream memStream = new MemoryStream(bytes);
                        //        BinaryFormatter binFormatter = new BinaryFormatter();
                        //        Image img = (Image)binFormatter.Deserialize(memStream);
                        //        //创建图像预览框
                        //        TextPictureBox pb = new TextPictureBox();
                        //        pb.Tag = false;
                        //        pb.Click += new EventHandler(pb_Click);
                        //        pb.DoubleClick += new EventHandler(pb_DoubleClick);
                        //        pb.Image = img;
                        //        pb.DataSource = drimage;
                        //        pb.Height = 200;
                        //        pb.Width = 200;
                        //        //pb.Titles.Add(new TextPictureBox.TextPictureBoxTitle("日期：", drimage["pres_date"] == DBNull.Value ? "" : ((DateTime)drimage["pres_date"]).ToString("yyyy-MM-dd")));

                        //        //添加标题
                        //        pb.Titles.Add(new TextPictureBox.TextPictureBoxTitle("项目：", drimage["pres_it_ecd"].ToString()));

                        //        this.flowLayoutPanel1.Controls.Add(pb);

                        //        memStream.Close();

                        //    }
                        //}
                        #endregion

                        if (!string.IsNullOrEmpty(drimage.ObrImage.ToString()))
                        //if (!Compare.IsNullOrDBNull(drimage["pres_base64"]) && drimage["pres_base64"].ToString().Trim() != string.Empty)
                        {
                            //File.WriteAllBytes(@"c:\a.pdf", (byte[])drimage["pres_chr"]);
                            MemoryStream ms = new MemoryStream(drimage.ObrImage);

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
                                pb.DataSource = drimage;
                                pb.Height = 190;
                                pb.Width = 190;
                                //pb.Titles.Add(new TextPictureBox.TextPictureBoxTitle("日期：", drimage["pres_date"] == DBNull.Value ? "" : ((DateTime)drimage["pres_date"]).ToString("yyyy-MM-dd")));

                                //添加标题
                                pb.Titles.Add(new TextPictureBox.TextPictureBoxTitle("项目：", drimage.ObrItmEname));

                                this.flowLayoutPanel1.Controls.Add(pb);
                            }

                            ms.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteException(this.GetType().Name, "LoadPhotos(DataTable dtResultoP)", ex.ToString());
                    lis.client.control.MessageDialog.Show("加载图像失败", "提示");
                }
            }
        }

        public void LoadPhotos(string pat_id)
        {
            this.PatID = pat_id;

            //DataTable dt = PatientCRUDClient.NewInstance.GetPatResultImage(pat_id);
            ProxyObrResultImage proxyImage = new ProxyObrResultImage();
            List<EntityObrResultImage> listObrResImage = new List<EntityObrResultImage>();
            listObrResImage = proxyImage.Service.GetObrResultImage(pat_id);//获取病人图像结果

            LoadPhotos(listObrResImage);
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

        public object GetSelectedDataRow()
        {
            return this.SelectedDataRow;
        }

        /// <summary>
        /// 当前选中图像对应的数据行
        /// </summary>
        private EntityObrResultImage SelectedDataRow { get; set; }

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
                        this.SelectedDataRow = pb.DataSource;
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

        public void Reset()
        {
            this.flowLayoutPanel1.Controls.Clear();
            this.DataSource = null;
            this.SelectedDataRow = null;
            this.panel1.Visible = false;
        }

        /// <summary>
        /// 点击删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            //获取当前记录的审核状态
            //string strPatState = PatientEnterClient.NewInstance.GetPatientState(this.PatID);
            string strPatState = string.Empty;
            ProxyPidReportMain proxyPidRepMain = new ProxyPidReportMain();
            EntityPatientQC patientCondition = new EntityPatientQC();
            patientCondition.RepId = this.PatID;
            List<EntityPidReportMain> listPatients = proxyPidRepMain.Service.PatientQuery(patientCondition);
            if (listPatients.Count > 0)
            {
                if (listPatients[0].RepInitialFlag != 0 && (listPatients[0].RepStatus == 0 || listPatients[0].RepStatus == null))
                {
                    strPatState = "1";
                }
                else
                {
                    if (listPatients[0].RepStatus != null)
                        strPatState = listPatients[0].RepStatus.ToString();
                }
            }

            string pres_id;
            string pres_itm_ecd;
            long pres_key;
            if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
            {
                bool hasSelect = false;
                StringBuilder sb = new StringBuilder();
                List<EntityObrResultImage> listObrResImg = new List<EntityObrResultImage>();
                foreach (Control ctrl in this.flowLayoutPanel1.Controls)
                {
                    if (ctrl is TextPictureBox)
                    {
                        TextPictureBox pb = ctrl as TextPictureBox;
                        if (pb.CheckState)
                        {
                            hasSelect = true;
                            EntityObrResultImage eyImg = pb.DataSource;

                            sb.Append(eyImg.ObrItmEname + ";");
                            listObrResImg.Add(eyImg);
                        }
                    }
                }

                if (hasSelect)
                {
                    if (
                        lis.client.control.MessageDialog.Show(string.Format("您确定要删除项目[{0}]的图像结果吗？", sb), "确认",
                                                              MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        EntitySysOperationLog client = new EntitySysOperationLog();
                        client.OperatUserId = UserInfo.loginID;
                        client.OperatServername = UserInfo.ip;

                        foreach (var dataInfo in listObrResImg)
                        {
                            pres_id = dataInfo.ObrId;
                            pres_itm_ecd = dataInfo.ObrItmEname;
                            pres_key = dataInfo.ObrSn;

                            //EntityOperationResult opResult = PatientCRUDClient.NewInstance.DeletePatPhotoResult(client,pres_id,pres_itm_ecd,pres_key);
                            ProxyObrResultImage proxyObrResImg = new ProxyObrResultImage();
                            EntityResponse opResult = proxyObrResImg.Service.DeletePatPhotoResult(client, pres_id, pres_itm_ecd, pres_key);

                            if (opResult.Scusess)
                            {
                                this.DataSource.Remove(dataInfo);

                                TextPictureBox SelectedPicBox = null;

                                foreach (Control ctrl in this.flowLayoutPanel1.Controls)
                                {
                                    if (ctrl is TextPictureBox)
                                    {
                                        TextPictureBox pBox = ctrl as TextPictureBox;
                                        if (pBox.DataSource == dataInfo)
                                        {
                                            SelectedPicBox = pBox;
                                            break;
                                        }
                                    }
                                }

                                //移除图片
                                if (SelectedPicBox != null)
                                {
                                    this.flowLayoutPanel1.Controls.Remove(SelectedPicBox);
                                }

                            }
                        }
                    }
                }

                if (hasSelect) return;
                //移除被删除的数据
                if (this.SelectedDataRow != null)
                {
                    pres_id = this.SelectedDataRow.ObrId;
                    pres_itm_ecd = this.SelectedDataRow.ObrItmEname;

                    pres_key = this.SelectedDataRow.ObrSn;

                    if (lis.client.control.MessageDialog.Show(string.Format("您确定要删除项目[{0}]的图像结果吗？", pres_itm_ecd), "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        EntitySysOperationLog client = new EntitySysOperationLog();
                        client.OperatUserId = UserInfo.loginID;
                        client.OperatServername = UserInfo.ip;

                        //EntityOperationResult opResult = PatientCRUDClient.NewInstance.DeletePatPhotoResult(client, pres_id, pres_itm_ecd, pres_key);
                        ProxyObrResultImage proxyObrResImg = new ProxyObrResultImage();
                        EntityResponse opResult = proxyObrResImg.Service.DeletePatPhotoResult(client, pres_id, pres_itm_ecd, pres_key);

                        if (opResult.Scusess)
                        {
                            this.DataSource.Remove(this.SelectedDataRow);

                            TextPictureBox SelectedPicBox = null;

                            foreach (Control ctrl in this.flowLayoutPanel1.Controls)
                            {
                                if (ctrl is TextPictureBox)
                                {
                                    TextPictureBox pBox = ctrl as TextPictureBox;
                                    if (pBox.DataSource == this.SelectedDataRow)
                                    {
                                        SelectedPicBox = pBox;
                                        break;
                                    }
                                }
                            }

                            //移除图片
                            if (SelectedPicBox != null)
                            {
                                this.flowLayoutPanel1.Controls.Remove(SelectedPicBox);
                            }
                            this.SelectedDataRow = null;
                        }
                        else
                        {
                            lis.client.control.MessageDialog.Show("删除失败", "提示");
                        }
                    }
                }
                else
                {
                    lis.client.control.MessageDialog.Show("请选择需要删除的图片", "提示");
                }

            }
            else//已审核、报告、打印
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能删除", "提示");
            }
        }

        private void PatPhotoList_Load(object sender, EventArgs e)
        {
            //PatID = string.Empty;
        }

        /// <summary>
        /// 导入图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.PatID))
            {
                lis.client.control.MessageDialog.Show("当前没有病人资料不能导入", "提示");
                return;
            }

            //获取当前记录的审核状态
            //string strPatState = PatientEnterClient.NewInstance.GetPatientState(this.PatID);
            string strPatState = string.Empty;
            #region 查询出病人信息进而获取审核状态
            ProxyPidReportMain proxyPidRepMain = new ProxyPidReportMain();
            EntityPatientQC patientCondition = new EntityPatientQC();
            patientCondition.RepId = this.PatID;
            List<EntityPidReportMain> listPatients = proxyPidRepMain.Service.PatientQuery(patientCondition);
            if (listPatients.Count > 0)
            {
                if (listPatients[0].RepInitialFlag != 0 && (listPatients[0].RepStatus == 0 || listPatients[0].RepStatus == null))
                {
                    strPatState = "1";
                }
                else
                {
                    if (listPatients[0].RepStatus != null)
                        strPatState = listPatients[0].RepStatus.ToString();
                }
            }
            #endregion
            openFileDialog1.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.png;*.PDF";
            if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog1.FileName != null && !string.IsNullOrEmpty(openFileDialog1.FileName))
                    {
                        if (!string.IsNullOrEmpty(this.PatID))
                        {
                            //调用病人信息查询方法（proxyPidRepMain.Service.PatientQuery(patientCondition);）查询出病人信息,故注释下行
                            //DataTable dttempPatInfo = PatientCRUDClient.NewInstance.GetPatientInfo(this.PatID);//获取病人信息

                            if (listPatients != null && listPatients.Count > 0)
                            {
                                int i = 0;
                                foreach (string fileName in openFileDialog1.FileNames)
                                {
                                    System.IO.MemoryStream Ms = new MemoryStream();
                                    EntityObrResultImage eyObrResImg = new EntityObrResultImage();
                                    if (fileName.Contains("PDF") || fileName.Contains("pdf"))
                                    {
                                        PDFFile file = PDFFile.Open(fileName);
                                        for (i = 1; i <= file.PageCount; i++)
                                        {
                                            Bitmap imge = file.GetPageImage(i-1,70);
                                            imge.Save(Ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        byte[] imgPdf = new byte[Ms.Length];
                                        Ms.Position = 0;
                                        Ms.Read(imgPdf, 0, Convert.ToInt32(Ms.Length));
                                        Ms.Close();
                                        eyObrResImg.ObrImage = imgPdf; //图像文件
                                    }
                                    else
                                    {
                                        Image img_temp = Image.FromFile(fileName);
                                        img_temp.Save(Ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        byte[] img = new byte[Ms.Length];
                                        Ms.Position = 0;
                                        Ms.Read(img, 0, Convert.ToInt32(Ms.Length));
                                        Ms.Close();
                                        eyObrResImg.ObrImage = img; //图像文件
                                    }
                                    eyObrResImg.ObrId = listPatients[0].RepId;//标石ID
                                    eyObrResImg.ObrItmEname = "image"; //图像类别
                                    eyObrResImg.ObrDate = listPatients[0].RepInDate.Value; //日期
                                    eyObrResImg.ObrSid = Convert.ToDecimal(listPatients[0].RepSid);//样本号
                                    eyObrResImg.ObrItrId = listPatients[0].RepItrId; //仪器代码
                                    eyObrResImg.ObrFlag = 1; //生效标志 为1

                                    ProxyObrResultImage proxyObrResImg = new ProxyObrResultImage();
                                    i = proxyObrResImg.Service.SaveObrResultImage(eyObrResImg);
                                }
                                if (i != 0 || i != -1)
                                {
                                    MessageDialog.ShowAutoCloseDialog("图像保存成功", 1m);
                                    LoadPhotos(this.PatID);
                                }
                            }
                            else
                            {
                                lis.client.control.MessageDialog.Show("导入失败，找不到当前病人的信息！", "提示");
                            }
                        }
                    }
                }
            }
            else//已审核、报告、打印
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能导入", "提示");
            }
        }

        /// <summary>
        /// 图像采集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImaq_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PatID))
            {
                lis.client.control.MessageDialog.Show("当前没有病人资料不能采集", "提示");
                return;
            }

            //获取当前记录的审核状态
            //string strPatState = PatientEnterClient.NewInstance.GetPatientState(PatID);
            string strPatState = string.Empty;
            #region 查询出病人信息,进而获取当前记录的审核状态
            ProxyPidReportMain proxyPidRepMain = new ProxyPidReportMain();
            EntityPatientQC patientCondition = new EntityPatientQC();
            patientCondition.RepId = this.PatID;
            List<EntityPidReportMain> listPatients = proxyPidRepMain.Service.PatientQuery(patientCondition);
            if (listPatients.Count > 0)
            {
                if (listPatients[0].RepInitialFlag != 0 && (listPatients[0].RepStatus == 0 || listPatients[0].RepStatus == null))
                {
                    strPatState = "1";
                }
                else
                {
                    if (listPatients[0].RepStatus != null)
                        strPatState = listPatients[0].RepStatus.ToString();
                }
            }
            #endregion

            if (strPatState == LIS_Const.PATIENT_FLAG.Natural || strPatState == string.Empty)//未审核
            {
                if (!string.IsNullOrEmpty(PatID))
                {
                    FrmImageAcquisition imageAcquisition = new FrmImageAcquisition(PatID);
                    if (imageAcquisition.show)
                    {
                        imageAcquisition.ShowDialog();
                        LoadPhotos(this.PatID);
                    }
                }
            }
            else//已审核、报告、打印
            {
                lis.client.control.MessageDialog.Show("当前记录已" + LocalSetting.Current.Setting.AuditWord + "，不能采集", "提示");
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            List<EntityObrResultImage> listObrResImg = new List<EntityObrResultImage>();
            foreach (Control ctrl in this.flowLayoutPanel1.Controls)
            {
                if (ctrl is TextPictureBox)
                {
                    TextPictureBox pb = ctrl as TextPictureBox;
                    if (pb.CheckState)
                    {
                        EntityObrResultImage eyImg = pb.DataSource;
                        listObrResImg.Add(eyImg);
                    }
                }
            }
            if (listObrResImg.Count > 0)
            {
                foreach (EntityObrResultImage image in listObrResImg)
                {
                    SaveFileDialog ofd = new SaveFileDialog();
               //     ofd.DefaultExt = "png";
                    ofd.Filter = "图片文件(*.png)|*.bmp;*.jpg;*.jpeg;*.png;*.PDF";
                    ofd.Title = "导出图片";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (ofd.FileName.Trim() == string.Empty)
                        {
                            lis.client.control.MessageDialog.ShowAutoCloseDialog("文件名不能为空！");
                            return;
                        }
                        MemoryStream ms = new MemoryStream(image.ObrImage);
                        Image img = Image.FromStream(ms);
                        img.Save(ofd.FileName);
                    }

                }


            }
            else {
                lis.client.control.MessageDialog.Show("请勾选需要导出的图片", "提示");
            }
        }
    }
}
