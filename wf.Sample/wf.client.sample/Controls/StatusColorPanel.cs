using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.sample
{
    public partial class StatusColorPanel : UserControl
    {
        public StatusColorPanel()
        {
            InitializeComponent();
            this.Load += StatusColorPanel_Load;
        }

        private void StatusColorPanel_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            Color colPrinted = IStep.GetBarcodeConfigColor("Barcode_Color_Printed");//打印
            this.btnPrint.Appearance.BackColor = colPrinted;
            this.btnPrint.Appearance.BorderColor = colPrinted;

            Color colBlooded = IStep.GetBarcodeConfigColor("Barcode_Color_Blooded");//采集
            this.btnBlooded.Appearance.BackColor = colBlooded;
            this.btnBlooded.Appearance.BorderColor = colBlooded;

            Color Barcode_Color_Sended = IStep.GetBarcodeConfigColor("Barcode_Color_Sended");//收取
            this.btnCollected.Appearance.BackColor = Barcode_Color_Sended;
            this.btnCollected.Appearance.BorderColor = Barcode_Color_Sended;

            Color Barcode_Color_Reach = IStep.GetBarcodeConfigColor("Barcode_Color_Reach");//送达
            this.btnReach.Appearance.BackColor = Barcode_Color_Reach;
            this.btnReach.Appearance.BorderColor = Barcode_Color_Reach;

            Color colReceived = IStep.GetBarcodeConfigColor("Barcode_Color_Received");//签收
            this.btnReceived.Appearance.BackColor = colReceived;
            this.btnReceived.Appearance.BorderColor = colReceived;

            Color Barcode_Color_FirstReported = IStep.GetBarcodeConfigColor("Barcode_Color_FirstReported");//一审
            this.btnReported.Appearance.BackColor = Barcode_Color_FirstReported;
            this.btnReported.Appearance.BorderColor = Barcode_Color_FirstReported;

            Color Barcode_Color_Reported = IStep.GetBarcodeConfigColor("Barcode_Color_Reported");//二审
            this.btnAudit.Appearance.BackColor = Barcode_Color_Reported;
            this.btnAudit.Appearance.BorderColor = Barcode_Color_Reported;
        }


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


    }
}
