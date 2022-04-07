using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace dcl.client.common
{
    public class PathManager
    {
        static PathManager()
        {
            bool useAppPath = System.Configuration.ConfigurationManager.AppSettings["AppPathSetting_UseAppPath"] == "1";
            if (useAppPath)
            {
                SettingPath = Application.StartupPath + @"\his\";
                ReportPath = Application.StartupPath + @"\lis\xtraReport\";
                OutLinkPath = @"C:\Program Files\medchange\";
                MessageClientPath = System.Configuration.ConfigurationManager.AppSettings["AppPathSetting_MessageClientPath"];
                SettingLisPath = Application.StartupPath + @"\lis\";
            }
            else
            {
                SettingPath = @"C:\Program Files\medchange\his\";
                ReportPath = @"C:\Program Files\medchange\lis\xtraReport\";
                OutLinkPath = @"C:\Program Files\medchange\";
                MessageClientPath = "C:\\Program Files\\medchange\\HopeLisClient\\";
                SettingLisPath = @"C:\Program Files\medchange\lis\";
            }
        }
        /// <summary>
        ///配置，对应以前路径 @"C:\Program Files\hope\his";
        /// </summary>
        public static string SettingPath { get; private set; }
        public static string ReportPath { get; private set; }
        public static string OutLinkPath { get; private set; }
        public static string MessageClientPath { get; private set; }
        public static string SettingLisPath { get; private set; }
    }
}
