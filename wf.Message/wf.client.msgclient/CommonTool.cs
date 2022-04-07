using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;
using System.Configuration;

namespace dcl.client.msgclient
{
    public class CommonTool
    {
        public static bool runWhenStart(bool started)
        {

            //if (File.Exists(Application.StartupPath + "\\AutoUpdate.exe"))
            //{
            //    return runWhenStart(started, "检验系统危急值", Application.StartupPath + "\\AutoUpdate.exe");

            //}
            //else
            //{
                return runWhenStart(started, "广州慧扬检验系统危急值", Application.ExecutablePath);
            //}

        }
        /// <summary>
        /// 修改或增加配置项
        /// </summary>
        /// <param name="newKey">键</param>
        /// <param name="newValue">值</param>
        public  static void UpdateAppSettings(string newKey, string newValue)
        {
            try
            {
                // Get the configuration file.
                System.Configuration.Configuration config =
                  ConfigurationManager.OpenExeConfiguration(
                  ConfigurationUserLevel.None);

                //添加
                config.AppSettings.Settings.Add(newKey, newValue);

                //修改
                config.AppSettings.Settings[newKey].Value = newValue;

                // Save the configuration file.
                config.Save(ConfigurationSaveMode.Modified);

                // Force a reload of the changed section.
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch
            {
                //throw;
            }

        }
        /// <summary> 
        /// 设置程序开机启动 
        /// 或取消开机启动 
        /// </summary> 
        /// <param name="started">设置开机启动，或者取消开机启动</param> 
        /// <param name="exeName">注册表中程序的名字</param> 
        /// <param name="path">开机启动的程序路径</param> 
        /// <returns>开启或则停用是否成功</returns> 
        public static bool runWhenStart(bool started, string exeName, string path)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);//打开注册表子项 
            if (key == null)//如果该项不存在的话，则创建该子项 
            {
                key = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            }
            if (started == true)
            {
                try
                {
                    key.SetValue(exeName, path);//设置为开机启动 
                    key.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("设置{0}自启动失败！\r\n{1}", exeName, ex.Message));

                    return false;
                }
            }
            else
            {
                try
                {
                    key.DeleteValue(exeName);//取消开机启动 
                    key.Close();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
    }
}
