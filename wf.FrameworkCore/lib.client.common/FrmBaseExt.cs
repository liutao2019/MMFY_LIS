namespace dcl.client.frame
{
    using DevExpress.XtraEditors;
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Drawing;

    public class FrmBaseExt : XtraForm
    {
        private CommonDelegateExt _biz;
        private static string _endpointConfigurationName = "svc.basic";
        private static string _remoteAddress = ConfigurationManager.AppSettings["wcfAddr"];

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FrmBaseExt
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "FrmBaseExt";
         
            this.ResumeLayout(false);

            this.Tag = this.GetType().FullName;

        }

        private void load()
        {
            if (UserInfo.entityUserInfo != null && UserInfo.entityUserInfo.AllFunc!=null) //&& UserInfo.dsUserInfo.Tables.Count > 0)
            {
                SetPower(this);
            }
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
                    c.Visible = false;
                }

                SetPower(c);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            try
            {
                this._biz.Close();
            }
            catch
            {
            }
            base.OnClosed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!base.DesignMode)
            {
                this.load();
            }
            base.OnLoad(e);
        }

        public CommonDelegateExt biz
        {
            get
            {
                if (this._biz == null)
                {
                    string remoteAddress = _remoteAddress + ConfigurationManager.AppSettings["svc." + base.Name];
                    this._biz = CommonDelegateExt.getDelegate(_endpointConfigurationName, remoteAddress);
                }
                return this._biz;
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
}
