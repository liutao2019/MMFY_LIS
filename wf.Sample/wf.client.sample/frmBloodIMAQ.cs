using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using dcl.client.common;
using System.Drawing.Imaging;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Camera_NET;

namespace dcl.client.sample
{
    public partial class frmBloodIMAQ : Form
    {
        public frmBloodIMAQ(Form parentForm)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Load += new EventHandler(frmBloodIMAQ_Load);
            this.Shown += new EventHandler(frmBloodIMAQ_Shown);
            this.parentForm = parentForm;
            parentForm.FormClosed += new FormClosedEventHandler(parentForm_FormClosed);
            parentForm.Activated += new EventHandler(parentForm_Activated);
            parentForm.Deactivate += new EventHandler(parentForm_Deactivate);
            this.cameraControl2.MouseClick += new MouseEventHandler(cameraControl2_MouseClick);
            cameraControl2.OutputVideoSizeChanged += new EventHandler(cameraControl2_OutputVideoSizeChanged);
        }
        string writeStr = string.Empty;
        bool changing = false;
        Form parentForm = null;
        void frmBloodIMAQ_Shown(object sender, EventArgs e)
        {
            CameraChoice _CameraChoice = new CameraChoice();

            //  COMException
            // Get List of devices (cameras)
            _CameraChoice.UpdateDeviceList();

            // To get an example of camera and resolution change look at other code samples 
            if (_CameraChoice.Devices.Count > 0)
            {
                // Device moniker. It's like device id or handle.
                // Run first camera if we have one
                var camera_moniker = _CameraChoice.Devices[0].Mon;

                // Set selected camera to camera control with default resolution
                string resolutionStr = "";
                try
                {
                    resolutionStr = IniConfigMgr.GetConfig("BloodIMAQ", "Resolution", "");
                }
                catch (Exception ex)
                {

                    Lib.LogManager.Logger.LogException(ex);
                }
                Resolution resolution = ConvertToResolution(resolutionStr);
                try
                {
                    cameraControl2.SetCamera(camera_moniker, resolution);
                    cameraControl2.MixerEnabled = true;
                    //WriteStrOnImage("hello world!");

                }
                catch (COMException ex)
                {
                    lis.client.control.MessageDialog.Show("启动摄像头失败！请检查驱动程序是否正确，或者有别的应用程序在占用此设备。");
                    // throw;
                }
                catch (Exception ex)
                {
                    lis.client.control.MessageDialog.Show("启动摄像头失败！");
                    Lib.LogManager.Logger.LogException(ex);
                }

            }
            else
            {
                lis.client.control.MessageDialog.Show("未检测到摄像头设备！请检查摄像头是否连接正常，或者驱动程序是否正确安装。");
            }
        }

        void cameraControl2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.contextMenuStrip1.Show(this, e.Location);
            }
        }

        void parentForm_Deactivate(object sender, EventArgs e)
        {
            if (!changing)
            {
                this.Visible = false;
            }

        }

        void parentForm_Activated(object sender, EventArgs e)
        {
            changing = true;
            IntPtr handler = GetActiveWindow();
            this.Visible = true;

            SetActiveWindow(handler);
            changing = false;
        }

        void parentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            cameraControl2.CloseCamera();
            this.Close();
        }

       
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();//获得当前活动窗体
        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hwnd);//设置活动窗体
        void frmBloodIMAQ_Load(object sender, EventArgs e)
        {
            System.Drawing.Rectangle rect = System.Windows.Forms.Screen.GetWorkingArea(this);//实例化一个当前窗口的对象
            System.Drawing.Rectangle rect2 = new System.Drawing.Rectangle(rect.Right - this.Width - 1, rect.Bottom - this.Height - 1, this.Width, this.Height);//为实例化的对象创建工作区域
            string x = rect2.X.ToString();
            string y = rect2.Y.ToString();
            string width = rect2.Width.ToString();
            string height = rect2.Height.ToString();
            try
            {
                x = IniConfigMgr.GetConfig("BloodIMAQ", "x", x);
                y = IniConfigMgr.GetConfig("BloodIMAQ", "y", y);
                width = IniConfigMgr.GetConfig("BloodIMAQ", "width", width);
                height = IniConfigMgr.GetConfig("BloodIMAQ", "height", height);
                this.SetBounds(int.Parse(x), int.Parse(y), int.Parse(width), int.Parse(height));//设置当前窗体的边界 
            }
            catch
            {
                this.SetBounds(rect2.X, rect2.Y, rect2.Width, rect2.Height);//设置当前窗体的边界 

            }


            this.Activated += new EventHandler(frmBloodIMAQ_MouseEnter);
            this.Deactivate += new EventHandler(frmBloodIMAQ_MouseLeave);
            this.btnso.SelectedIndexChanged += new EventHandler(btnso_SelectedIndexChanged);

        }



        void frmBloodIMAQ_MouseLeave(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
            try
            {
                IniConfigMgr.WriteString("BloodIMAQ", "x", this.Bounds.X.ToString());
                IniConfigMgr.WriteString("BloodIMAQ", "y", this.Bounds.Y.ToString());
                IniConfigMgr.WriteString("BloodIMAQ", "width", this.Bounds.Width.ToString());
                IniConfigMgr.WriteString("BloodIMAQ", "height", this.Bounds.Height.ToString());
            }
            catch
            {


            }

        }

        void frmBloodIMAQ_MouseEnter(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.Sizable;
        }
        public void ShowForm()
        {
            changing = true;
            IntPtr handler = GetActiveWindow();
            this.Show();

            SetActiveWindow(handler);
            changing = false;
        }



        public Image GrabImage()
        {
            Image img = this.cameraControl2.SnapshotOutputImage();
            // img.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\a.jpg");
            return img;
        }

        public byte[] GrabImageByte()
        {
            Image img = GrabImage();
            if (img != null)
            {
                MemoryStream stream = new MemoryStream();
                img.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
            return null;
        }



        private void btncsetting_Click(object sender, EventArgs e)
        {
            Camera.DisplayPropertyPage_Device(cameraControl2.Moniker, this.parentForm.Handle);
        }
        ResolutionList GetResolutionList()
        {
            try
            {
                return Camera.GetResolutionList(cameraControl2.Moniker);
            }
            catch (Exception ex)
            {

                //throw;
            }
            return null;
        }
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            this.btnso.Enabled = this.cameraControl2.CameraCreated;
            this.btncsetting.Enabled = this.cameraControl2.CameraCreated;
            if (this.cameraControl2.CameraCreated)
            {
                this.btnso.Items.Clear();

                if (!cameraControl2.CameraCreated)
                    return;


                ResolutionList resolutions = GetResolutionList();
                if (resolutions == null)
                    return;


                int index_to_select = -1;

                for (int index = 0; index < resolutions.Count; index++)
                {
                    this.btnso.Items.Add(resolutions[index].ToString());

                    if (resolutions[index].CompareTo(cameraControl2.Resolution) == 0)
                    {
                        index_to_select = index;
                    }
                }

                // select current resolution
                if (index_to_select >= 0)
                {
                    this.btnso.SelectedIndex = index_to_select;
                }
            }
        }
        void btnso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cameraControl2.CameraCreated)
                return;

            int comboBoxResolutionIndex = btnso.SelectedIndex;
            if (comboBoxResolutionIndex < 0)
            {
                return;
            }
            ResolutionList resolutions = GetResolutionList();

            if (resolutions == null)
                return;

            if (comboBoxResolutionIndex >= resolutions.Count)
                return; // throw

            if (0 == resolutions[comboBoxResolutionIndex].CompareTo(cameraControl2.Resolution))
            {
                // this resolution is already selected
                return;
            }
            try
            {
                cameraControl2.SetCamera(cameraControl2.Moniker, resolutions[comboBoxResolutionIndex]);

                IniConfigMgr.WriteString("BloodIMAQ", "Resolution", resolutions[comboBoxResolutionIndex].ToString());

            }
            catch (Exception ex)
            {

                Lib.LogManager.Logger.LogException(ex);
            }
            // Recreate camera

        }
        Resolution ConvertToResolution(string resolutionStr)
        {
            if (string.IsNullOrEmpty(resolutionStr))
            {
                return null;
            }
            Resolution result = null;
            string[] array = resolutionStr.Split(new char[] { 'x' }, StringSplitOptions.RemoveEmptyEntries);
            if (array.Length > 0)
            {
                int width = int.Parse(array[0]);
                int height = int.Parse(array[1]);
                return new Resolution(width, height);

            }
            return result;
        }
        void WriteStrOnImage(string str)
        {
            writeStr = str;
            int w = cameraControl2.OutputVideoSize.Width;
            int h = cameraControl2.OutputVideoSize.Height;

            if (w <= 0 || h <= 0)
                return;

            // Create RGB bitmap
            Bitmap bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);
            // configure antialiased drawing or not

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;


            // Clear the bitmap with the color key
            g.Clear(cameraControl2.GDIColorKey);

            Font font = new Font("Tahoma", 16);
            Brush textColorKeyed = new SolidBrush(Color.Yellow);

            g.DrawString(str, font, textColorKeyed, 4, h - 30);
            g.Dispose();

            cameraControl2.OverlayBitmap = bmp;
        }
       
        void cameraControl2_OutputVideoSizeChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(writeStr))
            {
                WriteStrOnImage(this.writeStr);
            }
        }

    }
}
