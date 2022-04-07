using System;
using System.Collections.Generic;
using System.Text;
using dcl.client.common;

namespace dcl.client.common
{
    public class ConverterCreator
    {
        private static ConverterCreator _instance = null;
        public static ConverterCreator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ConverterCreator();
                }
                return _instance;
            }
        }
        string convertType = string.Empty;

        public BaseInputConvert Converter { get; private set; }
        private ConverterCreator()
        {
            convertType = ConfigHelper.GetSysConfigValueWithoutLogin("Report_ReportQuery_Converter");
            switch (convertType)
            {
                case "佛山市一健康卡转换":
                    Converter = new OutLinkCardNumConvert();
                    break;

                default:
                    Converter = new BaseInputConvert();
                    break;
            }
        }
    }
}
