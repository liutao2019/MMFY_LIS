namespace dcl.client.frame
{
    using DevExpress.XtraEditors;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;
    using System.Windows.Forms;
    using System.Collections.Specialized;
    using System.Data;
    using System.Collections.Generic;
    using DevExpress.XtraSplashScreen;
    using System.Threading;
    public class FrmBase : XtraForm
    {
        private CommonDelegate _biz;
        private static string _endpointConfigurationName = "svc.basic";
        private static string _remoteAddress = ConfigStaticHelper.Setting["wcfAddr"];
        private IContainer components;

        public static DataTable dtbControlGridView=new DataTable();

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FrmBase
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "FrmBase";
            this.ResumeLayout(false);

        }

        private void load()
        {
            if (UserInfo.entityUserInfo != null && UserInfo.entityUserInfo.AllFunc!=null )//&& UserInfo.dsUserInfo.Tables.Count > 0)
            {
                SetPower(this);
            }
            //FindGridviewControl(this.Controls);
        }

        private void SetPower(System.Windows.Forms.Control control)
        {
            if (this.Tag == null)
            {
                return;
            }
            string funcCode = this.Tag.ToString();

            foreach (System.Windows.Forms.Control c in control.Controls)
            {
                string moduleName = c.Name;

                if (UserInfo.HaveFunction(funcCode, moduleName) == false)
                {
                    c.Enabled = false;
                }

                SetPower(c);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
#if Release
            try
            {
#endif
                base.OnClosed(e);
                if (_biz != null)
                    this._biz.Close();
#if Release   
            }
            catch(Exception ex)
            {
                Logger.Logger.WriteException("FrmBase", "OnClosed", ex.ToString());
            }
#endif
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!base.DesignMode)
            {
                this.load();
            }
            base.OnLoad(e);
        }

        public CommonDelegate biz
        {
            get
            {
                if (this._biz == null)
                {
                    string remoteAddress = _remoteAddress + ConfigStaticHelper.Setting["svc." + base.Name];
                    this._biz = CommonDelegate.getDelegate(_endpointConfigurationName, remoteAddress);
                }
                return this._biz;
            }
        }




        private SplashScreenManager _loadForm;
        /// <summary>
        /// 等待窗体管理对象
        /// </summary>
        protected SplashScreenManager LoadForm
        {
            get
            {
                if (_loadForm == null)
                {
                    this._loadForm = new SplashScreenManager(this, typeof(PubWaitForm), true, true);
                    this._loadForm.ClosingDelay = 0;
                }
                return _loadForm;
            }
        }
        /// <summary>
        /// 显示等待窗体
        /// </summary>
        public void BeginLoading()
        {
            bool flag = !this.LoadForm.IsSplashFormVisible;
            if (flag)
            {
                this.LoadForm.ShowWaitForm();
                Thread.Sleep(300);
            }
        }
        /// <summary>
        /// 关闭等待窗体
        /// </summary>
        public void CloseLoading()
        {
            bool isSplashFormVisible = this.LoadForm.IsSplashFormVisible;
            if (isSplashFormVisible)
            {
                this.LoadForm.CloseWaitForm();
            }
        }

        #region DesignMode


        protected new bool DesignMode
        {
            get
            {
                bool returnFlag = false;
#if DEBUG
                if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                    returnFlag = true;
                else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper().Equals("DEVENV"))
                    returnFlag = true;
#endif
                return returnFlag;
            }
        }


        #endregion
    }

    public class ConfigStaticHelper
    {
        private static NameValueCollection setting;
        public static NameValueCollection Setting
        {
            get
            {
                if (setting == null)
                    setting = ConfigurationManager.AppSettings;

                return setting;
            }
        }
    }
}

