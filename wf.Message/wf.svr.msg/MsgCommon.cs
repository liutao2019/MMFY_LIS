using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.common;
using dcl.svr.cache;

namespace dcl.svr.msg
{
    public class MsgCommon
    {
        /// <summary>
        /// 根据配置转换获取年龄分钟(当年龄为空值时)
        /// </summary>
        /// <param name="ageInput">输入(分钟)</param>
        /// <returns></returns>
        public static int GetConfigAge(object ageMinuteInput)
        {
            if (!Compare.IsEmpty(ageMinuteInput))
            {
                int ret = -1;

                if (int.TryParse(ageMinuteInput.ToString(), out ret))
                {
                    if (ret >= 0)
                    {
                        return ret;
                    }
                }
                else
                {
                }
            }
            else
            {
            }

            string configCalAge = CacheSysConfig.Current.GetSystemConfig("GetRefOnNullAge");

            int calage = -1;

            if (!string.IsNullOrEmpty(configCalAge)
                && configCalAge != "不计算参考值")
            {
                calage = AgeConverter.YearToMinute(Convert.ToInt32(configCalAge));
            }
            return calage;
        }

        public static string GetConfigSex(object sexInput)
        {
            string ret = string.Empty;
            if (Compare.IsEmpty(sexInput) || (sexInput.ToString() != "1" && sexInput.ToString() != "2" && sexInput.ToString() != "0"))//年龄为空
            {
                string configCalSex = CacheSysConfig.Current.GetSystemConfig("GetRefOnNullSex");

                if (configCalSex == "不计算参考值")
                {
                    ret = "9";
                }
                else if (configCalSex == "默认")
                {
                    ret = "0";
                }
                else if (configCalSex == "男")
                {
                    ret = "1";
                }
                else if (configCalSex == "女")
                {
                    ret = "2";
                }
            }
            else
            {
                ret = sexInput.ToString();
            }

            return ret;
        }
    }
}
