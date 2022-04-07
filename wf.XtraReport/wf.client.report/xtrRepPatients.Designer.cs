namespace dcl.client.report
{
    partial class xtrRepPatients
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xtrResu_Other = new DevExpress.XtraReports.UI.XRSubreport();
            this.xtrResu_Rig = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xtrRepResp = new DevExpress.XtraReports.UI.XRSubreport();
            this.xtrResu = new DevExpress.XtraReports.UI.XRSubreport();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xtrResu_Other,
            this.xtrResu_Rig,
            this.xrLine1,
            this.xtrRepResp,
            this.xtrResu});
            this.Detail.HeightF = 144F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xtrResu_Other
            // 
            this.xtrResu_Other.LocationFloat = new DevExpress.Utils.PointFloat(0F, 75F);
            this.xtrResu_Other.Name = "xtrResu_Other";
            this.xtrResu_Other.ReportSource = new dcl.client.report.xtrRepResulto();
            this.xtrResu_Other.SizeF = new System.Drawing.SizeF(192F, 58F);
            // 
            // xtrResu_Rig
            // 
            this.xtrResu_Rig.LocationFloat = new DevExpress.Utils.PointFloat(325F, 0F);
            this.xtrResu_Rig.Name = "xtrResu_Rig";
            this.xtrResu_Rig.ReportSource = new dcl.client.report.xtrRepResulto();
            this.xtrResu_Rig.SizeF = new System.Drawing.SizeF(325F, 58F);
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 58F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(650F, 17F);
            // 
            // xtrRepResp
            // 
            this.xtrRepResp.LocationFloat = new DevExpress.Utils.PointFloat(192F, 75F);
            this.xtrRepResp.Name = "xtrRepResp";
            this.xtrRepResp.ReportSource = new dcl.client.report.xtrRepResulto_p();
            this.xtrRepResp.SizeF = new System.Drawing.SizeF(458F, 68F);
            // 
            // xtrResu
            // 
            this.xtrResu.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xtrResu.Name = "xtrResu";
            this.xtrResu.ReportSource = new dcl.client.report.xtrRepResulto();
            this.xtrResu.SizeF = new System.Drawing.SizeF(325F, 58F);
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine2,
            this.xrLabel1});
            this.PageHeader.HeightF = 86F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.PageHeader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 67F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(650F, 17F);
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Times New Roman", 22F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(250F, 8F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(142F, 50F);
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "病人报表";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 0F;
            this.PageFooter.Name = "PageFooter";
            this.PageFooter.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.PageFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 100F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 100F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // xtrRepPatients
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.PageHeader,
            this.PageFooter,
            this.topMarginBand1,
            this.bottomMarginBand1});
            this.DataMember = "可设计字段";
            this.Version = "15.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
        private DevExpress.XtraReports.UI.XRLine xrLine1;
        public DevExpress.XtraReports.UI.XRSubreport xtrRepResp;
        public DevExpress.XtraReports.UI.XRSubreport xtrResu;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLine xrLine2;
        public DevExpress.XtraReports.UI.XRSubreport xtrResu_Rig;
        public DevExpress.XtraReports.UI.XRSubreport xtrResu_Other;
        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;
    }
}
