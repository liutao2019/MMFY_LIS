using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using dcl.svr.cache;
using dcl.common;

namespace dcl.svr.resultcheck
{
   public  class Common
    {
        public static int GetConfigAge(decimal? decAge)
        {
            //获取年龄(分钟)
            int pat_age = -1;
            if (decAge != null && decAge.Value!=-1)
            {
                pat_age = Convert.ToInt32(decAge.Value);
            }
            else
            {
                //如果没有年龄则从配置获取
                string configCalAge = CacheSysConfig.Current.GetSystemConfig("GetRefOnNullAge");
                if (!string.IsNullOrEmpty(configCalAge)
                     && configCalAge != "不计算参考值")
                {
                    pat_age = AgeConverter.YearToMinute(Convert.ToInt32(configCalAge));
                }
            }
            return pat_age;
        }

        public static string GetConfigSex(string sexInput)
        {
            string ret = "0";

            if (sexInput != "1" && sexInput != "2")//年龄为空
            {
                string configCalSex = CacheSysConfig.Current.GetSystemConfig("GetRefOnNullSex");

                if (configCalSex == "不计算参考值")
                {
                    ret = "9";
                }
                else if (configCalSex == "男" || configCalSex == "默认")
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
