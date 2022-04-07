using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;

namespace dcl.client.report
{
    public class ReportSetting
    {
       // static string xmlFile = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\hope\lis\printXml\printConfig.xml";
        static string xmlFile = dcl.client.common.PathManager.SettingLisPath + @"printXml\printConfig.xml";

        static ReportSetting()
        {
            Refresh();
        }

        public static void Refresh()
        {
            if (File.Exists(xmlFile))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(xmlFile);
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt != null && dt.Rows.Count >0)
                    {
                        if (dt.Columns.Contains("paper"))
                        {
                            Paper = dt.Rows[0]["paper"].ToString();
                        }
                        if (dt.Columns.Contains("heigth"))
                        {
                            Heigth = dt.Rows[0]["heigth"].ToString();
                        }
                        if (dt.Columns.Contains("width"))
                        {
                            Width = dt.Rows[0]["width"].ToString();
                        }
                        if (dt.Columns.Contains("printName"))
                        {
                            PrintName = dt.Rows[0]["printName"].ToString();
                        }

                        if (dt.Rows[0]["landscape"] != null && dt.Rows[0]["landscape"].ToString().ToLower() == "true")
                        {
                            Landscape = true;
                        }
                        else
                        {
                            Landscape = false;
                        }

                        if (dt.Columns.Contains("printType"))
                        {
                            PrintType = dt.Rows[0]["printType"].ToString();
                        }

                        if (dt.Columns.Contains("ContinuousPrinting"))
                        {
                            if (dt.Rows[0]["ContinuousPrinting"] != null && dt.Rows[0]["ContinuousPrinting"].ToString() == "1")
                            {
                                ContinuousPrinting = true;
                            }
                            else
                            {
                                ContinuousPrinting = false;
                            }
                        }


                    }
                }
            }
        }


        public static string Paper { get; private set; } = "";
        public static string Heigth { get; private set; } = "";
        public static string Width { get; private set; } = "";
        public static string PrintName { get; private set; } = "";
        public static bool Landscape { get; private set; } = false;
        public static string PrintType { get; private set; } = "";
        public static bool ContinuousPrinting { get; private set; } = false;

    }
}
