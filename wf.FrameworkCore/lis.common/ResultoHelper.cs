using System;
using System.Collections.Generic;

using System.Text;
using dcl.common.extensions;

namespace dcl.common
{
    /// <summary>
    /// 结果表Helper
    /// </summary>
    public class ResultoHelper
    {
        /// <summary>
        /// 更改结果表的标识ID最后的标本号
        /// </summary>
        /// <param name="res_id"></param>
        /// <param name="oldSampleID">老的样本号</param>
        /// <param name="newSampleID">新样本号</param>
        /// <returns></returns>
        public static string ModifyResID(string res_id, string oldSampleID, string newSampleID)
        {
            if (Extensions.IsEmpty(oldSampleID) || Extensions.IsEmpty(newSampleID))
                return res_id;
            int index = res_id.LastIndexOf(oldSampleID);
            if (index < 0)
                return res_id;

            //更换样本号
            return res_id.Substring(0, index) + newSampleID.Trim();
        }

        /// <summary>
        /// 生成结果表的标识ID
        /// </summary>
        /// <param name="instrumentID">仪器ID</param>
        /// <param name="createDate">结果日期</param>
        /// <param name="sampleID">样本号</param>
        /// <returns></returns>
        public static string GenerateResID(string instrumentID, DateTime createDate, string sampleID)
        {
            if (Extensions.IsEmpty(instrumentID) || Extensions.IsEmpty(sampleID) || createDate == null)
                return "";
            return string.Format("{0}{1}{2}", instrumentID.Trim(), createDate.ToString("yyyyMMdd"), sampleID.Trim());
        }
    }
}
