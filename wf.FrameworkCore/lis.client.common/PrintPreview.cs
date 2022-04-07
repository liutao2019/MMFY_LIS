using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;

using System.Text;
using System.Windows.Forms;
using dcl.client.frame;

namespace dcl.client.common
{
    public partial class PrintPreview : UserControl
    {
        public PrintPreview()
        {
            InitializeComponent();
            this.Load += PrintPreview_Load;
           
        }

        private void PrintPreview_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            string strProp = UserInfo.GetSysConfigValue("ReportProportional");
            if (strProp != "")
            {
                printControl1.Zoom = float.Parse(strProp);
            }
        }

        public void SetCanExportFile(bool canExport)
        {
            if (canExport)
            {
                this.printPreviewBarItem23.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                this.printPreviewBarItem24.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                this.printPreviewBarItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                this.printPreviewBarItem23.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.printPreviewBarItem24.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                this.printPreviewBarItem3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }
        }

        public void SetPrintPreviewBarItem23Command(string strCommand)
        {
            if (string.IsNullOrEmpty(strCommand)) strCommand = "ExportGraphic";

            switch (strCommand)
            {
                case "ExportGraphic":
                    this.printPreviewBarItem23.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportGraphic;
                    break;
                case "ExportRtf":
                    this.printPreviewBarItem23.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportRtf;
                    break;
                default:
                    this.printPreviewBarItem23.Command = DevExpress.XtraPrinting.PrintingSystemCommand.ExportGraphic;
                    break;
            }
        }
    }
}
