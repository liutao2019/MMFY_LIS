using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Specialized;

namespace dcl.client.common
{
    public class IniConfigMgr
    {
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        public static string fileName = "app.ini";
        static string path = System.Windows.Forms.Application.StartupPath;
        static string fileFullName = path + "\\" + fileName;
        static Dictionary<string, Dictionary<string, string>> configDic = new Dictionary<string, Dictionary<string, string>>();

        static IniConfigMgr()
        {

            if (!File.Exists(fileFullName))
            {
                File.Create(fileFullName);
            }
            else
            {
                StringCollection col = new StringCollection();
                ReadSections(col);
                foreach (string item in col)
                {
                    if (!configDic.ContainsKey(item))
                    {
                        configDic.Add(item, new Dictionary<string, string>());
                        NameValueCollection values = new NameValueCollection();
                        ReadSectionValues(item, values);
                        foreach (string key in values.Keys)
                        {
                            if (!configDic[item].ContainsKey(key))
                            {
                                configDic[item].Add(key, values[key]);
                            }
                        }

                    }
                }
            }
        }


        public static string GetConfig(string section, string key, string defaultValue)
        {

            if (configDic.ContainsKey(section) && configDic[section].ContainsKey(key))
            {
                return configDic[section][key];
            }
            else
            {
                if (!configDic.ContainsKey(section))
                {
                    configDic.Add(section, new Dictionary<string, string>());
                    configDic[section].Add(key, defaultValue);
                }
                if (!configDic[section].ContainsKey(key))
                {
                    configDic[section].Add(key, defaultValue);
                }

                WriteString(section, key, defaultValue);

                return defaultValue;
            }
        }

        public static bool GetConfig(string section, string key, bool defaultValue)
        {
            string value = GetConfig(section, key, defaultValue ? "1" : "0");
            if (value == "1" || value.ToLower() == Convert.ToString(true))
            {
                return true;
            }
            return false;
        }

        //写INI文件 
        public static void WriteString(string Section, string Ident, string Value)
        {
            if (!WritePrivateProfileString(Section, Ident, Value, fileFullName))
            {

                //  throw (new ApplicationException("写Ini文件出错"));
            }
            else
            {
                if (configDic.ContainsKey(Section))
                {
                    Dictionary<string, string> dict = configDic[Section];
                    if (dict.ContainsKey(Ident))
                    {
                        dict[Ident] = Value;
                    }
                }
            }
        }
        //读取INI文件指定 
        static string ReadString(string Section, string Ident, string Default)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(Section, Ident, Default, Buffer, Buffer.GetUpperBound(0), fileFullName);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文 
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }

        //读整数 
        static int ReadInteger(string Section, string Ident, int Default)
        {
            string intStr = ReadString(Section, Ident, Convert.ToString(Default));
            try
            {
                return Convert.ToInt32(intStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Default;
            }
        }

        //写整数 
        static void WriteInteger(string Section, string Ident, int Value)
        {
            WriteString(Section, Ident, Value.ToString());
        }

        //读布尔 
        static bool ReadBool(string Section, string Ident, bool Default)
        {
            try
            {
                return Convert.ToBoolean(ReadString(Section, Ident, Convert.ToString(Default)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Default;
            }
        }

        //写Bool 
        static void WriteBool(string Section, string Ident, bool Value)
        {
            WriteString(Section, Ident, Convert.ToString(Value));
        }

        //从Ini文件中，将指定的Section名称中的所有Ident添加到列表中 
        static void ReadSection(string Section, StringCollection Idents)
        {
            Byte[] Buffer = new Byte[16384];
            //Idents.Clear(); 

            int bufLen = GetPrivateProfileString(Section, null, null, Buffer, Buffer.GetUpperBound(0),
              fileFullName);
            //对Section进行解析 
            GetStringsFromBuffer(Buffer, bufLen, Idents);
        }

        static void GetStringsFromBuffer(Byte[] Buffer, int bufLen, StringCollection Strings)
        {
            Strings.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((Buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start);
                        Strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }
        //从Ini文件中，读取所有的Sections的名称 
        static void ReadSections(StringCollection SectionList)
        {
            //Note:必须得用Bytes来实现，StringBuilder只能取到第一个Section 
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = GetPrivateProfileString(null, null, null, Buffer,
              Buffer.GetUpperBound(0), fileFullName);
            GetStringsFromBuffer(Buffer, bufLen, SectionList);
        }
        //读取指定的Section的所有Value到列表中 
        static void ReadSectionValues(string Section, NameValueCollection Values)
        {
            StringCollection KeyList = new StringCollection();
            ReadSection(Section, KeyList);
            Values.Clear();
            foreach (string key in KeyList)
            {
                Values.Add(key, ReadString(Section, key, ""));

            }
        }
        ////读取指定的Section的所有Value到列表中， 
        //public void ReadSectionValues(string Section, NameValueCollection Values,char splitString)
        //{   string sectionValue;
        //    string[] sectionValueSplit;
        //    StringCollection KeyList = new StringCollection();
        //    ReadSection(Section, KeyList);
        //    Values.Clear();
        //    foreach (string key in KeyList)
        //    {   
        //        sectionValue=ReadString(Section, key, "");
        //        sectionValueSplit=sectionValue.Split(splitString);
        //        Values.Add(key, sectionValueSplit[0].ToString(),sectionValueSplit[1].ToString());

        //    }
        //}
        //清除某个Section 
        static void EraseSection(string Section)
        {
            // 
            if (!WritePrivateProfileString(Section, null, null, fileFullName))
            {
                throw (new ApplicationException("无法清除Ini文件中的Section"));
            }
        }
        //删除某个Section下的键 
        static void DeleteKey(string Section, string Ident)
        {
            WritePrivateProfileString(Section, Ident, null, fileFullName);
        }
        //Note:对于Win9X，来说需要实现UpdateFile方法将缓冲中的数据写入文件 
        //在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile 
        //执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。 
        static void UpdateFile()
        {
            WritePrivateProfileString(null, null, null, fileFullName);
        }

        //检查某个Section下的某个键值是否存在 
        static bool ValueExists(string Section, string Ident)
        {
            // 
            StringCollection Idents = new StringCollection();
            ReadSection(Section, Idents);
            return Idents.IndexOf(Ident) > -1;
        }

        //确保资源的释放 
        ~IniConfigMgr()
        {
            UpdateFile();
        }

    }
}
