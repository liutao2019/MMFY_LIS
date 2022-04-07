using dcl.client.common;
using dcl.client.report;
using wf.ShelfPrint.CustomControl;
using wf.ShelfPrint.Properties;
using dcl.client.wcf;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using dcl.client.cache;

namespace wf.ShelfPrint
{
    public partial class FrmMainPrint : Form
    {
        FrmKeybord keybord;

        public FrmMainPrint()
        {
            InitializeComponent();
        }

        FrmMssage frmMsg = null;

        List<EntityTouchPrintData> listReturn = new List<EntityTouchPrintData>();

        string ImagePath = Application.StartupPath + @"\JPG\KeyBoard\";

        int returnTime = 10;

        int settingTime = 10;

        FrmReturnMssage frmReturnMsg = null;

        string CardMode = string.Empty;

        string DevPort = string.Empty;

        int PaperQuantity = 50;

        int PaperWarningQuantity = 20;

        string DotNotPrintItrID = string.Empty;

        bool AutoPrint = true;

        string SystemPassword = string.Empty;

        string PrintType = "0";

        UInt32 Hdle = 0;

        private void FrmMainPrint_Load(object sender, EventArgs e)
        {
            txtInput.Text = "";
            lblName.Text = "";

            this.pnLogo.DoubleClick += PnLogo_DoubleClick;

            string appPath = Application.StartupPath;
            System.IO.DirectoryInfo topDir = System.IO.Directory.GetParent(appPath);
            string topDirPath = topDir.FullName;//topDirPath即上层目录
            string reportPath = topDirPath + @"\lis\xtraReport\";
            DCLTouchReportPrint.ReportPath = reportPath;

            keybord = new FrmKeybord();
            keybord.TargetControl = txtInput;

            this.gvPrintData.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.gvPrintData_CellPainting);
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.txtInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtInput_KeyDown);
            this.btnKeyBord.Click += new System.EventHandler(this.btnKeyBord_Click);


            this.Player.settings.setMode("loop", true);
            this.Player.URL = Application.StartupPath + @"/宣传视频.mp4";
            this.ActiveControl = txtInput;
            gvPrintData.AutoGenerateColumns = false;
            clmPreview.Visible = false;
            Control.CheckForIllegalCrossThreadCalls = false;

            //foreach (string item in this.Videos)
            //{
            //    this.axWindowsMediaPlayer1.currentPlaylist.appendItem(this.axWindowsMediaPlayer1.newMedia(item));  // 将视频逐个添加至播放列表 
            //}

            if (!Directory.Exists(Application.StartupPath + @"\pdf"))
            {
                Directory.CreateDirectory(Application.StartupPath + @"\pdf");
            }

            PrintType = ConfigurationManager.AppSettings["PrintType"];
            string strShowTime = ConfigurationManager.AppSettings["ShowTime"];
            string strWeChatImage = ConfigurationManager.AppSettings["WeChatImage"];
            string strNoName1 = ConfigurationManager.AppSettings["NoName1"];
            string strNoName2 = ConfigurationManager.AppSettings["NoName2"];
            string strPrompt1 = ConfigurationManager.AppSettings["Prompt1"];
            string strPrompt2 = ConfigurationManager.AppSettings["Prompt2"];
            string strPrompt3 = ConfigurationManager.AppSettings["Prompt3"];
            string strPrompt4 = ConfigurationManager.AppSettings["Prompt4"];
            string strBackgroundImage = ConfigurationManager.AppSettings["BackgroundImage"];
            string strTitleImage = ConfigurationManager.AppSettings["TitleImage"];

            lblNoName1.Text = strNoName1;
            lblNoName2.Text = strNoName2;
            lblPrompt1.Text = strPrompt1;
            lblPrompt2.Text = strPrompt2;
            lblPrompt3.Text = strPrompt3;
            lblPrompt4.Text = strPrompt4;

            if (!string.IsNullOrEmpty(strWeChatImage) && File.Exists(ImagePath + strWeChatImage))
            {
                plWeChat.BackgroundImage = new Bitmap(ImagePath + strWeChatImage);
            }
            else
            {
                plWeChat.Visible = false;
                plWeChatMsg.Visible = false;
            }

            if (!string.IsNullOrEmpty(strBackgroundImage) && File.Exists(ImagePath + strBackgroundImage))
            {
                this.BackgroundImage = new Bitmap(ImagePath + strBackgroundImage);
            }

            if (!string.IsNullOrEmpty(strTitleImage) && File.Exists(ImagePath + strTitleImage))
            {
                plTop.BackgroundImage = new Bitmap(ImagePath + strTitleImage);
            }

            int time = 10;
            if (int.TryParse(strShowTime, out time))
            {
                settingTime = time;
                returnTime = time;
            }

            if (CardReaderFactory.DCLCardReader != null)
            {
                CardReaderFactory.DCLCardReader.Open();
                tmRead.Enabled = true;
            }

            string strPaperQuantity = ConfigurationManager.AppSettings["PaperQuantity"];
            if (!string.IsNullOrEmpty(strPaperQuantity))
            {
                int.TryParse(strPaperQuantity, out PaperQuantity);
            }

            string strPaperWarningQuantity = ConfigurationManager.AppSettings["PaperWarningQuantity"];
            if (!string.IsNullOrEmpty(strPaperWarningQuantity))
            {
                int.TryParse(strPaperWarningQuantity, out PaperWarningQuantity);
            }

            //string strKeyBoardEnable = ConfigurationManager.AppSettings["KeyBoardEnable"];
            //if (strKeyBoardEnable == "N")
            //{
            //    txtInput.Enabled = false;
            //    txtInput.BackColor = Color.White;
            //    this.btnSearch.Click -= new System.EventHandler(this.btnSearch_Click);
            //}

            string strPreviewEnable = ConfigurationManager.AppSettings["PreviewEnable"];
            if (strPreviewEnable == "Y")
            {
                clmPreview.Visible = true;
                this.gvPrintData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvPrintData_CellContentClick);
            }

            DotNotPrintItrID = ConfigurationManager.AppSettings["DotNotPrintItrID"];

            if (ConfigurationManager.AppSettings["AutoPrint"] == "否")
                AutoPrint = false;


            if (AutoPrint)
            {
                pbPrint.Visible = false;
            }

            SystemPassword = ConfigurationManager.AppSettings["SystemPassword"];
        }

        private void PnLogo_DoubleClick(object sender, EventArgs e)
        {
            //frmSign fs = new frmSign();
            //if (fs.ShowDialog() == DialogResult.OK)
            //{
            //    this.Close();
            //}
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            keybord.Hide();
            Search();
        }


        private void btnKeyBord_Click(object sender, EventArgs e)
        {
            keybord.Show();
        }

        List<EntityTouchPrintData> listPrint = null;
        List<EntityTouchPrintData> listNotPrint = null;

        private void Search()
        {
            if (txtInput.Text.Trim() != string.Empty)
            {
                if (!string.IsNullOrEmpty(SystemPassword) &&
                    txtInput.Text.Trim() == SystemPassword)
                {
                    FrmSystem frmSys = new FrmSystem();
                    if (frmSys.ShowDialog() == DialogResult.Yes)
                    {
                        int.TryParse(frmSys.PaperQuantity, out PaperQuantity);
                        MessageBox.Show("已重置纸张数为：" + frmSys.PaperQuantity);
                    }
                    txtInput.Text = string.Empty;
                    return;
                }

                listPrint = new List<EntityTouchPrintData>();
                listNotPrint = new List<EntityTouchPrintData>();

                tmRead.Enabled = false;
                Thread loading = new Thread(ShowLoading);
                loading.Start();

                Thread.Sleep(1000);

                //MessageBox.Show("1");

                ProxyTouchPrint proxy = new ProxyTouchPrint();
                List<EntityTouchPrintData> listPrintData = proxy.Service.PatientPrintQuery(txtInput.Text.Trim(), "'107','109'", PrintType);
                //MessageBox.Show("2");
                if (listPrintData.Count > 0)
                {
                    if (PaperQuantity < PaperWarningQuantity)
                    {
                        if (frmMsg != null)
                        {
                            frmMsg.ShowMsg("打印机缺纸，请联系工作人员。", Color.Red);
                            Thread.Sleep(3000);
                                frmMsg.Hide();
                            txtInput.Text = string.Empty;
                            tmRead.Enabled = true;
                            return;
                        }
                    }

                   // MessageBox.Show("3");

                    lblTime.Visible = true;

                    tmReturn.Enabled = true;

                    gvPrintData.DataSource = listPrintData;
                    gvPrintData.ClearSelection();

                    lblName.Text = listPrintData[0].PidName;

                    //MessageBox.Show("4");

                    if (frmMsg != null)
                        frmMsg.Hide();

                    plView.Visible = true;

                    string msgImagePath = string.Empty;
                    listReturn = listPrintData.FindAll(f => f.RepStatus == "标本回退");
                    if (listReturn.Count > 0)
                    {
                        msgImagePath = ImagePath + "Notice2.png";
                        pbMsg_Click(null, null);
                    }
                    else
                        msgImagePath = ImagePath + "Notice.png";

                    if (File.Exists(msgImagePath))
                        pbMsg.BackgroundImage = new Bitmap(msgImagePath);

                    foreach (EntityTouchPrintData printData in listPrintData)
                    {
                        if (printData.RepStatus == "打印中")
                        {
                            if (!string.IsNullOrEmpty(DotNotPrintItrID))
                            {
                                if (DotNotPrintItrID.Contains(printData.RepItrId))
                                {
                                    printData.RepStatus = "不允许打印";
                                    listNotPrint.Add(printData);
                                }
                                else
                                {
                                    listPrint.Add(printData);
                                }
                            }
                            else
                            {
                                listPrint.Add(printData);
                            }
                        }
                    }

                    if (AutoPrint)//自动打印报告，才显示未出结果等信息
                    {
                        int noResult = listPrintData.FindAll(f => f.RepStatus != "已打印" 
                        && f.RepStatus != "打印中").Count;

                        if (noResult > 0)
                        {
                            lblNoResult.Text = noResult.ToString();
                            lblMsg3.Visible = true;
                            lblMsg4.Visible = true;
                            lblNoResult.Visible = true;
                        }
                        else
                        {
                            lblMsg3.Visible = false;
                            lblMsg4.Visible = false;
                            lblNoResult.Visible = false;
                        }
                    }

                    if (listPrint.Count == 0 && listNotPrint.Count == 0)
                    {
                        lblMsg1.Text = "您暂无报告需要打印...";
                        lblSum.Visible = false;
                        lblMsg2.Visible = false;
                        pbPrint.Visible = false;
                        txtInput.Text = "";
                        //lblName.Text = "";
                    }
                    else
                    {
                        if (AutoPrint)
                        {
                            Print();
                        }
                        else
                        {
                            pbPrint.Visible = true;
                            lblSum.Visible = false;
                            lblMsg2.Visible = false;
                            lblMsg1.Text = "请点击右上方的打印按钮，打印您的报告单！";
                        }
                    }
                }
                else
                {
                    frmMsg.ShowMsg(" 未查询到您的信息！", Color.Red);
                    Thread.Sleep(1000);

                    txtInput.Text = string.Empty;

                    if (frmMsg != null)
                        frmMsg.Hide();
                }
                tmRead.Enabled = true;
            }
        }

        private void Print()
        {
            if (listPrint == null || listPrint.Count <= 0)
            {
                return;
            }

            pbPrint.Visible = false;

            if (listPrint.Count > 0)
            {
                lblMsg1.Text = "正在打印中，共";
                lblSum.Text = listPrint.Count.ToString();
                lblSum.Visible = true;
                lblMsg2.Visible = true;

                List<string> listPatID = new List<string>();

                if (PrintType == "0")
                {
                    foreach (EntityTouchPrintData item in listPrint)
                    {
                        if (!string.IsNullOrEmpty(item.RepPDF))
                        {
                            byte[] arr = Convert.FromBase64String(item.RepPDF);
                            //string strPath = string.Format("D:\\" + item.RepId + ".pdf");
                            //File.WriteAllBytes(strPath, arr);
                            MemoryStream ms = new MemoryStream(arr);
                            PDFPrint print = new PDFPrint();
                            print.Print(ms);
                            listPatID.Add(item.RepId);
                        }
                    }
                    if (listPatID.Count > 0)
                    {
                        new ProxyPidReportMain().Service.UpdatePrintState_whitOperator(listPatID, "4", "-1", "触摸屏", "自助打印程序打印");
                        PaperQuantity -= listPatID.Count;
                    }
                }
                else
                {
                    int sequence = 0;
                    List<EntityDCLPrintParameter> listPara = new List<EntityDCLPrintParameter>();
                    foreach (EntityTouchPrintData item in listPrint)
                    {
                        EntityDCLPrintParameter printParameter = new EntityDCLPrintParameter();
                        printParameter.RepId = item.RepId;
                        printParameter.ReportCode = DictInstrmt.Instance.GetItrPrtTemplate(item.RepItrId);
                        listPara.Add(printParameter);
                        sequence++;
                        listPatID.Add(item.RepId);
                    }

                    Thread combineItemThread = new Thread(new ThreadStart(delegate ()
                    {
                        DCLTouchReportPrint.BatchPrint(listPara);
                    }));
                    combineItemThread.IsBackground = true;
                    combineItemThread.Start();
                    if (listPatID.Count > 0)
                    {
                        new ProxyPidReportMain().Service.UpdatePrintState_whitOperator(listPatID, "4", "-1", "触摸屏", "自助打印程序打印");
                        PaperQuantity -= listPatID.Count;
                    }
                }

            }

            if (listNotPrint.Count > 0)
            {
                string strMsg = "您有" + listNotPrint.Count.ToString() + "份报告" + ConfigurationManager.AppSettings["DotNotPrintItmAndValueText"];
                tmReturn.Enabled = false;
                frmMsg.ShowMsg(strMsg, Color.Red);
                Thread.Sleep(1000);
                frmMsg.Hide();
                pbReturn_Click(null, null);
            }
        }


        private void pbReturn_Click(object sender, EventArgs e)
        {
            txtInput.Text = "";
            lblName.Text = "";

            if (frmReturnMsg != null)
            {
                frmReturnMsg.Close();
                frmReturnMsg = null;
            }
            plView.Visible = false;
            txtInput.Text = string.Empty;
            returnTime = settingTime;
            lblTime.Visible = false;
            tmReturn.Enabled = false;
            lblTime.Text = returnTime.ToString();

            if(AutoPrint)
                pbPrint.Visible = false;
            else
                pbPrint.Visible = true;

            listPrint = new List<EntityTouchPrintData>();
            gvPrintData.DataSource = listPrint;

        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Search();
            }
        }

        private void ShowLoading()
        {
            if (frmMsg == null)
                frmMsg = new FrmMssage();

            frmMsg.ShowMsg("信息读取中，请稍后。", Color.Black);
            frmMsg.ShowDialog();
        }

        private void HideLoading()
        {
            if (frmMsg != null)
                frmMsg.Hide();
        }
   

        private void pbMsg_Click(object sender, EventArgs e)
        {
            if (listReturn != null && listReturn.Count > 0)
            {
                frmReturnMsg = new FrmReturnMssage(listReturn);
                frmReturnMsg.ShowDialog();
            }

        }

        private void gvPrintData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            try
            {
                if (e.ColumnIndex == 4)
                {
                    DataGridViewRow dgr = gvPrintData.Rows[e.RowIndex];
                    string strRepStatus = dgr.Cells["ColumnRepStatus"].Value.ToString();

                    if (strRepStatus == "已打印")
                        e.CellStyle.ForeColor = Color.Blue;
                    else if (strRepStatus == "打印中")
                        e.CellStyle.ForeColor = Color.Green;
                    else
                        e.CellStyle.ForeColor = Color.Red;
                }
            }
            catch
            {

            }
        }

        private void tmReturn_Tick(object sender, EventArgs e)
        {
            returnTime -= 1;
            lblTime.Text = returnTime.ToString();

            if (returnTime == 0)
                pbReturn_Click(sender, e);
        }

        private void tmRead_Tick(object sender, EventArgs e)
        {
            if (CardReaderFactory.DCLCardReader != null)
            {
                string strCardNumber = CardReaderFactory.DCLCardReader.Reader();
                
                if (!string.IsNullOrEmpty(strCardNumber))
                {
                    tmRead.Stop();
                    txtInput.Text = strCardNumber.Trim();
                    Search();
                    tmRead.Start();
                }
            }

        }


        private void gvPrintData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                List<EntityTouchPrintData> listPrintData = (List<EntityTouchPrintData>)gvPrintData.DataSource;

                EntityTouchPrintData printData = listPrintData[e.RowIndex];

                string filePath = Application.StartupPath + @"\pdf\" + printData.RepId + ".pdf";

                if (!File.Exists(filePath))
                {
                    if (!string.IsNullOrEmpty(printData.RepPDF))
                    {
                        byte[] arr = Convert.FromBase64String(printData.RepPDF);

                        FileStream fs = new FileStream(filePath, FileMode.Create);
                        //开始写入
                        fs.Write(arr, 0, arr.Length);
                        //清空缓冲区、关闭流
                        fs.Flush();
                        fs.Close();
                        System.Diagnostics.Process.Start(filePath);
                    }
                }
                else
                    System.Diagnostics.Process.Start(filePath);
            }

        }

        private void pbPrint_Click(object sender, EventArgs e)
        {
            Print();
        }

        private void FrmMainPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CardReaderFactory.DCLCardReader != null)
                CardReaderFactory.DCLCardReader.Close();
        }

        private void ReadHpCardAndDownForSZZT()
        {
            try
            {
                string cardNumber = string.Empty;

                string Key = "FACED0614321";

                int RetType = 0;

                int ComId = Convert.ToInt32(DevPort);

                StringBuilder sb = new StringBuilder();
                //Lib.LogManager.Logger.LogInfo("DevCardReading ComId:" + ComId.ToString());
                int result = ZTDLL.ZT_DEV_ReadAll(ComId, false, 4000, true, 1, 0, 96, Key, false, true, 0, ref RetType, sb);

                if (string.IsNullOrEmpty(sb.ToString()))
                {
                    result = ZTDLL.ZT_DEV_ReadAll(ComId, false, 4000, true, 2, 0, 96, Key, false, true, 0, ref RetType, sb);
                    //Lib.LogManager.Logger.LogInfo("ZT_DEV_ReadAll(2):" + result.ToString());
                }
                if (string.IsNullOrEmpty(sb.ToString()))
                {
                    result = ZTDLL.ZT_DEV_ReadAll(ComId, false, 4000, true, 3, 0, 96, Key, false, true, 0, ref RetType, sb);
                    //Lib.LogManager.Logger.LogInfo("ZT_DEV_ReadAll(3):" + result.ToString());
                }

                cardNumber = sb.ToString();

                if (!string.IsNullOrEmpty(cardNumber))
                {
                    StringBuilder strOutCard = new StringBuilder();
                    ZTDLL.ZT_BcdToAsc(cardNumber, strOutCard);
                    txtInput.Text = strOutCard.ToString();
                    Search();
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("中山三院农行读卡器读取接口", ex);
            }
        }

        private void ReadCardAndDownForCRT()
        {
            try
            {
                //打开串口
                Hdle = CRTdll.CommOpen(DevPort);
                if (Hdle != 0)
                {
                    byte[] _TrackData = new byte[500];
                    int _TrackDataLen = 0;
                    int result = -1;
                    //读卡
                    result = CRTdll.MC_ReadTrack(Hdle, 0x30, 0x37, ref _TrackDataLen, _TrackData);
                    if (result == 0)
                    {
                        int n;
                        int position1 = 0;
                        int position2 = 0;
                        int position3 = 0;
                        string Tra2Buf = "";
                        for (n = 0; n < _TrackDataLen; n++)
                        {
                            if (_TrackData[n] == 31)
                            {
                                position1 = n;
                                break;
                            }
                        }
                        for (n = position1 + 1; n < _TrackDataLen; n++)
                        {
                            if (_TrackData[n] == 31)
                            {
                                position2 = n;
                                break;
                            }
                        }
                        for (n = position2 + 1; n < _TrackDataLen; n++)
                        {
                            if (_TrackData[n] == 31)
                            {
                                position3 = n;
                                break;
                            }
                        }
                        if (_TrackData[position2 + 1] == 89)
                        {

                            for (n = position2 + 2; n < position3; n++)
                            {
                                Tra2Buf += (char)_TrackData[n];
                            }
                            txtInput.Text = Tra2Buf;
                            Search();
                            if (!string.IsNullOrEmpty(Tra2Buf))
                            {
                                //退卡
                                int reset = CRTdll.CRT310_Reset(Hdle, 0x01);
                            }

                        }
                    }
                }
                //关闭串口
                int close = CRTdll.CommClose(Hdle);
            }

            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("广州12医院创智电动读卡器读取接口", ex);
            }
        }


        //MessageBox.Show("您单击的是第" + (e.RowIndex + 1) + "行第" + (e.ColumnIndex + 1) + "列！");
        //MessageBox.Show("单元格的内容是：" + dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
    }
}

