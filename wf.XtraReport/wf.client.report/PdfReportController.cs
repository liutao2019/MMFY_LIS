using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using iTextSharp.text;
using Color = System.Drawing.Color;
using Rectangle = iTextSharp.text.Rectangle;

namespace dcl.client.report
{
    public class PdfReportController
    {
        internal Dictionary<string, MemoryStream> GenPdfReport(Dictionary<string, XtraReport> xtraReports)
        {
            Dictionary<string, MemoryStream> result = new Dictionary<string, MemoryStream>();
            foreach (string key in xtraReports.Keys)
            {
                List<MemoryStream> imageStream = ExportToImage(xtraReports[key]);
                if (imageStream.Count == 0)
                {
                    return null;
                }
             

                MemoryStream ms = new MemoryStream();

                iTextSharp.text.Document document = new iTextSharp.text.Document(new Rectangle(PageSize.A4.Width, PageSize.A4.Height / 2));
                   
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms);

                document.Open();
                int i = 0;
                foreach (MemoryStream memoryStream in imageStream)
                {
                    if (i != 0)
                    {
                        document.NewPage();
                    }
                    i++;
                    iTextSharp.text.Image je = iTextSharp.text.Image.GetInstance(memoryStream.ToArray());
                    float percent = (document.PageSize.Width - document.LeftMargin - document.RightMargin) / je.Width * 100;
                    je.ScalePercent(percent);
                    document.Add(je);

                }

                document.Close();
                result.Add(key,ms);
            }
            return result;
        }

        private List<MemoryStream> ExportToImage(XtraReport xr)
        {
            List<MemoryStream> resut = new List<MemoryStream>();

            ImageExportOptions options = new ImageExportOptions();
            options.ExportMode = ImageExportMode.SingleFilePageByPage
;
            options.Format = ImageFormat.Png;

            for (int i = 0; i < xr.Pages.Count; i++)
            {
                options.PageRange = (i + 1).ToString();
                MemoryStream stream = new MemoryStream();
                xr.ExportToImage(stream, options);

                resut.Add(TractImage(stream));
            }

            return resut;

        }

        MemoryStream TractImage(MemoryStream image)
        {
            MemoryStream result = new MemoryStream();

            Bitmap bit = new Bitmap(image);
            bit = KiResizeImage(bit, bit.Width, bit.Height);
            System.Drawing.Imaging.BitmapData bd = bit.LockBits(new System.Drawing.Rectangle(0, 0, bit.Width, bit.Height),
                              System.Drawing.Imaging.ImageLockMode.ReadOnly, bit.PixelFormat);
            bit.UnlockBits(bd);



            bit.Save(result, ImageFormat.Jpeg);

            bit.Dispose();

            return result;
        }

        /// <summary>
        /// 设置图片分辨率
        /// </summary>
        /// <param name="bmp">原始Bitmap</param>
        /// <param name="newW">新的宽度</param>
        /// <param name="newH">新的高度</param>
        /// <returns>处理以后的图片</returns>
        private static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);

                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // 插值算法的质量 
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, newW, newH), new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);

                //去边框
                for (int i = 0; i < 6; i++)
                {
                    Pen p = new Pen(Color.White);
                    g.DrawLine(p, new Point(0, 0), new Point(0, newH));
                    g.DrawLine(p, new Point(newW - 1, 0), new Point(newW - 1, newH));
                    g.DrawLine(p, new Point(0, newH - 1), new Point(newW - 1, newH - 1));
                    g.DrawLine(p, new Point(0, 0), new Point(newW - 1, 0));
                }
                g.Dispose();
                return b;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                // Logger.WriteException("", "设置图片分辨率时出错", ex.ToString());
                return null;
            }
        }
    }
}
