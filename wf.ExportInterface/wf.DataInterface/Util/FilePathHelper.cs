using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    class FilePathHelper
    {
        public static string GetBasePath()
        {
            string stmp = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            stmp = stmp.Substring(0, stmp.LastIndexOf('/'));//删除文件名
            string basePath = stmp + "/";
            if (basePath == null)
            {
                basePath = AppDomain.CurrentDomain.BaseDirectory;
            }
            basePath = basePath.Replace(@"\\", @"\");

            return basePath;
        }
    }
}
