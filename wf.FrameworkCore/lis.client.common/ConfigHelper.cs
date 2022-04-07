using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using System.Data;
using System.Xml;
using System.Drawing;

namespace dcl.client.common
{
    public class ConfigHelper
    {
        public static string fileName = "dcl.client.sample.dll";//System.IO.Path.GetDirectoryName(Application.ExecutablePath) ;//+  @"dcl.client.sample.dll";//System.IO.Path.GetFileName(Application.ExecutablePath);

        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="key">Key值</param>
        /// <param name="newValue">新值</param>
        public static bool UpdateSeting(string key, string newValue)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(fileName);
                string value = config.AppSettings.Settings[key].Value = newValue;
                config.Save();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="aFileName"></param>
        /// <param name="key">Key值</param>
        /// <param name="newValue">新值</param>
        public static bool UpdateSeting(string aFileName,string key, string newValue)
        {
            try
            {
                WriteSetting(aFileName, key, newValue);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }

            return true;
        }

        public static void WriteSetting(string path,string key, string value)
        {
            XmlDocument doc = loadConfigDocument(path);
            XmlNode node = doc.SelectSingleNode("//appSettings");
            if (node == null) throw new InvalidOperationException("appSettings section not found in config file.");
            
                XmlElement elem = (XmlElement) node.SelectSingleNode(string.Format("//add[@key='{0}']", key));
                if (elem != null)
                {
                    elem.SetAttribute("value", value);
                }
                else
                {
                    elem = doc.CreateElement("add");
                    elem.SetAttribute("key", key);
                    elem.SetAttribute("value", value);
                    node.AppendChild(elem);
                }
                doc.Save(path);
         
        }

        private static XmlDocument loadConfigDocument(string path)
        {
            XmlDocument doc = null;

            doc = new XmlDocument();
            doc.Load(path);
            return doc;
        }

        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <param name="key">Key值</param>
        public static string GetSetting(string key)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(fileName);
                string value = config.AppSettings.Settings[key].Value;
                return value;
            }
            catch (Exception)
            {
                return "";
            }

        }


        /// <summary>
        /// 获取配置文件信息
        /// </summary>
        /// <param name="key">Key值</param>
        /// <param name="aFileName"></param>
        public static string GetSetting(string key,string aFileName)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(aFileName);
                string value = config.AppSettings.Settings[key].Value;
                return value;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        /// <summary>
        /// 增加配置文件信息
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">新值</param>       
        public static bool AddSetting(string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(fileName);
                config.AppSettings.Settings.Add(key, value);
                config.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 读取xml关于某键值
        /// </summary>
        /// <param name="tKey"></param>
        public static string ReadXml(string tKey, string aFileName)
        {
            string tValue = "";

            string filepath = aFileName;

            try
            {
                if (!File.Exists(filepath))
                {
                    return "";
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(tKey))
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        tValue = row[tKey].ToString();
                        ds.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                return "";
            }
            return tValue;
        }

        public static string GetSysConfigValueWithoutLogin(string configCode)
        {
            return dcl.client.cache.CacheSysconfig.Current.GetSystemConfig(configCode);
        }

        public static bool IsNotOutlink()
        {
            return dcl.client.frame.UserInfo.GetSysConfigValue("GetPatientsInfoType") == "通用";
        }

        public static Color GetBarcodeConfigColor(string configCode)
        {

            string cfgValue = ConfigHelper.GetSysConfigValueWithoutLogin(configCode);

            Color col;
            //黑色,红色,灰色,蓝色,绿色,紫色
            switch (cfgValue)
            {
                case "黑色":
                    col = Color.Black;
                    break;

                case "红色":
                    col = Color.Red;
                    break;

                case "灰色":
                    col = Color.DarkGray;
                    break;
                case "粉红色":
                    col = Color.Pink;
                    break;
                case "黄色":
                    col = Color.Yellow;
                    break;
                case "蓝色":
                    col = Color.Blue;
                    break;

                case "绿色":
                    col = Color.Green;
                    break;

                case "紫色":
                    col = Color.Purple;
                    break;
                case "白色":
                    col = Color.White;
                    break;
                case "棕色":
                    col = Color.Brown;
                    break;

                default:
                    col = Color.Black;
                    break;
            }
            return col;
        }
    }
}
