using O2S.Components.PDFRender4NET;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace wf.ShelfPrint
{
    public class PDFPrint
    {
        public int Print(Stream stream)
        {

            int isOK = 0;
            PDFFile file = PDFFile.Open(stream);
            //PDFFile file = PDFFile.Open(@"C:\Users\李小盒\Desktop\P30152_180103120921\test.pdf");
            PrinterSettings settings = new PrinterSettings();
            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();
            settings.PrintToFile = false;

            O2S.Components.PDFRender4NET.Printing.PDFPrintSettings pdfPrintSettings = new O2S.Components.PDFRender4NET.Printing.PDFPrintSettings(settings);


            string autosize = ConfigurationManager.AppSettings["AutoFitPaperSize"];
            if (!string.IsNullOrEmpty(autosize) && autosize=="1")
            {
                PaperSize paper = new PaperSize("Custom", (int)file.GetPageSize(0).Width / 72 * 100, (int)file.GetPageSize(0).Height / 72 * 100);
                paper.RawKind = (int)PaperKind.Custom;
                pdfPrintSettings.PaperSize = paper;
            }
            pdfPrintSettings.PageScaling = O2S.Components.PDFRender4NET.Printing.PageScaling.FitToPrinterMarginsProportional;
            pdfPrintSettings.PrinterSettings.Copies = 1;

            try
            {
                file.Print(pdfPrintSettings);
                isOK = 1;
            }
            catch (Exception ex)
            {
                isOK = -1;
                Lib.LogManager.Logger.LogException(ex);
            }
            finally
            {
                file.Dispose();
            }
            return isOK;
        }
    }
}
