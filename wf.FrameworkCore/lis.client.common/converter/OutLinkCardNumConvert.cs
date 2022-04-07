using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.LogManager;

namespace dcl.client.common
{
    public class OutLinkCardNumConvert : BaseInputConvert
    {

        public override string Convert(string input)
        {
            string result = input;
            if (!string.IsNullOrEmpty(input) && input.Length > 9)
            {
                DataTable mzPatInfo = new dcl.common.OutlinkClient().GetMZPatient(input);
                if (mzPatInfo != null && mzPatInfo.Rows.Count > 0)
                {
                    result = mzPatInfo.Rows[0]["MZNO"].ToString();
                }
                else
                {
                    Logger.LogInfo("OutLinkCardNumConvert", "无法转换" + input);
                }
            }
            return result;
        }
    }
}
