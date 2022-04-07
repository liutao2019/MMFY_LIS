using System;
using System.Drawing;
using System.IO;

namespace dcl.common
{
    public class PictureToBase64Helper
    {
        static string DebugFileName = @"C:\Base64String.ini";

        /// <summary>
        /// 将图片转换成Base64String
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static string ImageToBase64String(Image image)
        {
            try
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                //先新建一个Bitmap对象
                Bitmap bitmap = new Bitmap(image);
                //调用save方法是，bitmap好像不能被引用，微软的bug？？？
                //所以不能直接image.save
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] buffer = ms.ToArray();
                string Base64String = Convert.ToBase64String(buffer);
                SaveFile(Base64String);
                return Base64String;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将流保存成Base64String
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string StreamToBase64String(Stream stream)
        {
            try
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                // 设置当前流的位置为流的开始
                stream.Seek(0, SeekOrigin.Begin);

                string Base64String = Convert.ToBase64String(buffer);
                SaveFile(Base64String);
                return Base64String;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将Base64String转换成图片
        /// </summary>
        /// <param name="Base64String"></param>
        /// <returns></returns>
        public static Image Base64StringToImage(string Base64String)
        {
            Base64String = GetBase64StringFile(Base64String);
            if (Base64String.Length > 0)
            {
                Byte[] bitmapData = new Byte[Base64String.Length];
                bitmapData = Convert.FromBase64String(Base64String);
                MemoryStream streamBitmap = new MemoryStream(bitmapData);
                return Image.FromStream(streamBitmap);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Base64StringToMemoryStream
        /// </summary>
        /// <param name="Base64String"></param>
        /// <returns></returns>
        public static MemoryStream Base64StringToMemoryStream(string Base64String)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                byte[] arr = Convert.FromBase64String(Base64String);
                ms.Write(arr, 0, arr.GetLength(0));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ms;
        }

        #region 文件读写

        private static string GetBase64StringFile(string Base64String)
        {
            string buff = string.Empty;
            try
            {
                FileStream fs = new FileStream(DebugFileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                buff = sr.ReadToEnd();
                sr.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                buff = string.Empty;
            }
            if (string.IsNullOrEmpty(buff))
                return Base64String;
            return buff;
        }

        private static void SaveFile(string Base64String)
        {
            try
            {
                string FileName = "Base64String.ini";
                FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(Base64String);
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #endregion
    }
}
