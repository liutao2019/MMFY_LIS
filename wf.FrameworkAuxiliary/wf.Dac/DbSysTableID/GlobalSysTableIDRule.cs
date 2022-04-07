using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC
{
    public class GlobalSysTableIDRule
    {
        private const char SplitChar = ';';

        private int _step = 1;

        /// <summary>
        /// SysTableID生成方式
        /// </summary>
        public EnumSysTableIDGenerateType IDGenerateType { get; set; }

        /// <summary>
        /// 初始值
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// 每次增长值
        /// </summary>
        public int Step
        {
            get
            {
                return _step;
            }
            set
            {
                if (value == 0)
                    _step = 1;
                else
                    _step = value;
            }
        }

        public int Length { get; set; }
        public string CustomString { get; set; }

        public GlobalSysTableIDRule()
        {
            this.IDGenerateType = EnumSysTableIDGenerateType.Int32;
            this.Start = 1;
            this._step = 1;
            this.Length = 0;
        }

        public static GlobalSysTableIDRule Parse(string input)
        {
            GlobalSysTableIDRule rule = new GlobalSysTableIDRule();

            if (input != null && input.Trim().Length > 0)
            {
                string[] splitedString = input.Split(new char[] { SplitChar }, StringSplitOptions.RemoveEmptyEntries);

                //整型自增方式
                if (splitedString[0].ToLower() == "int32" || splitedString[0].ToLower() == "int64")
                {
                    string type = splitedString[0].ToLower();

                    //rule.DefauleRule = false;
                    rule.IDGenerateType = (EnumSysTableIDGenerateType)Enum.Parse(typeof(EnumSysTableIDGenerateType), type, true);

                    if (splitedString.Length > 1)//开始值
                    {
                        rule.Start = Convert.ToInt32(splitedString[1]);
                    }

                    if (splitedString.Length > 2)//每次递增
                    {
                        rule.Step = Convert.ToInt32(splitedString[2]);
                    }

                    if (splitedString.Length > 3)//最大长度
                    {
                        rule.Length = Convert.ToInt32(splitedString[3]);
                    }
                }
                else if (splitedString[0].ToLower() == "custom")//自定义方式
                {
                    if (splitedString.Length > 1)
                    {
                        rule.IDGenerateType = EnumSysTableIDGenerateType.Custom;
                        rule.CustomString = splitedString[1];
                        //rule.DefauleRule = false;
                    }
                    else
                    {
                        //rule.DefauleRule = true;
                    }
                }
                else
                {
                    //rule.DefauleRule = true;
                }
            }
            else
            {
                //rule.DefauleRule = true;
            }
            return rule;
        }

        public override string ToString()
        {
            if (IDGenerateType == EnumSysTableIDGenerateType.Custom)
            {
                //Type:{0};CustomString:{1}
                return string.Format("{0}{2}{1}", IDGenerateType, CustomString, SplitChar);
            }
            else if (IDGenerateType == EnumSysTableIDGenerateType.Int32
                  || IDGenerateType == EnumSysTableIDGenerateType.Int64
                    )
            {
                //Type:{0};Start:{2};Step:{3};MaxLength:{4}
                return string.Format("{0}{4}{1}{4}{2}{4}{3}", IDGenerateType, Start, Step, Length, SplitChar);
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// SysTableID生成方式
    /// </summary>
    public enum EnumSysTableIDGenerateType
    {
        /// <summary>
        /// 32位整型序列
        /// </summary>
        Int32 = 0,

        /// 64<summary>
        /// 位整型序列
        /// </summary>
        Int64 = 1,

        /// <summary>
        /// 自定义
        /// </summary>
        Custom = 2,
    }

    //public enum EnumSysTableIDDataType
    //{
    //    Int32,
    //    Int64,
    //    String,
    //}
}
