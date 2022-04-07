using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;

namespace dcl.client.common
{
    /// <summary>
    /// 二维码生成类
    /// </summary>
    public class QcCodeHelper
    {
        /// <summary>
        /// 文件存放路径
        /// </summary>
        private string strFileSavePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "QcCodePic\\";

        /// <summary>
        /// 生成二维码图片jpg
        /// </summary>
        /// <param name="barcode"></param>
        public void CreateQcCodePic(string barcode)
        {
            try
            {
                if (string.IsNullOrEmpty(barcode))
                {
                    return;
                }

                if (!System.IO.Directory.Exists(strFileSavePath))
                {
                    System.IO.Directory.CreateDirectory(strFileSavePath);
                }

                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 8;//4;
                qrCodeEncoder.QRCodeVersion = 1;//8
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                
                System.Drawing.Image image = qrCodeEncoder.Encode(barcode);

                //string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff").ToString() + ".jpg";
                string filename = barcode + ".jpg";
                //string filepath = Server.MapPath(@"~\Upload") + "\\" + filename;
                string filepath = strFileSavePath + filename;

                if (!System.IO.File.Exists(filepath))
                {
                    System.IO.FileStream fs = new System.IO.FileStream(filepath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                    image.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);

                    fs.Close();
                    image.Dispose();
                }
                
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 删除旧文件(2小时前)
        /// </summary>
        public void DelOldFile()
        {
            try
            {
                if (!System.IO.Directory.Exists(strFileSavePath))
                {
                    System.IO.Directory.CreateDirectory(strFileSavePath);
                    return;
                }

                string[] strlist1 = System.IO.Directory.GetFiles(strFileSavePath);
                List<string> list_file = new List<string>();
                for (int i = 0; i < strlist1.Length; i++)
                {
                    if (System.IO.File.Exists(strlist1[i]))
                    {
                        System.IO.FileInfo f = new System.IO.FileInfo(strlist1[i]);
                        //if (f.Extension == ".pdf")
                        {
                            if (f.LastWriteTime < DateTime.Now.AddHours(-2))
                            {
                                f.Delete();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 二维码解码
        /// </summary>
        /// <param name="filePath">图片路径</param>
        /// <returns></returns>
        public string CodeDecoder(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return null;
            System.Drawing.Bitmap myBitmap = new System.Drawing.Bitmap(System.Drawing.Image.FromFile(filePath));

            QRCodeDecoder decoder = new QRCodeDecoder();
            string decodedString = decoder.decode(new ThoughtWorks.QRCode.Codec.Data.QRCodeBitmapImage(myBitmap));
            return decodedString;
        }
    }
}
