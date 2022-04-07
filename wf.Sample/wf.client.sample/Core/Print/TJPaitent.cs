using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using dcl.client.common;

using dcl.common.extensions;
using System.Windows.Forms;
using dcl.common;
using dcl.client.frame;
using dcl.client.cache;

namespace dcl.client.sample
{
    /// <summary>
    /// 体检条码
    /// </summary>
    public class TJPaitent : IPrint
    {
        public override DateTimeRange GetDefaultAdviceTime()
        {
            double dblStart = -30;
            double dblEnd = 1;
            try
            {
                //获取参数设置查询日期范围
                dblStart = Convert.ToDouble(ConfigHelper.GetSysConfigValueWithoutLogin("TjBarcodeStartDownDate"));
                dblEnd = Convert.ToDouble(ConfigHelper.GetSysConfigValueWithoutLogin("TjBarcodeEndDownDate")) + 1;
            }
            catch (Exception)
            {
                dblStart = -30;
                dblEnd = 1;

            }
            DateTime dtNow = ServerDateTime.GetServerDateTime();
            return new DateTimeRange(dtNow.Date.AddDays(dblStart), dtNow.Date.AddDays(dblEnd).AddSeconds(-1));
        }

        public override void Init()
        {
            Printablor.OutpatientInit();
        }

        public override LoadDataType LoadDataType
        {
            get {
                string downloadType = ConfigHelper.GetSysConfigValueWithoutLogin("TJDownloadType");
                if (downloadType=="添加")
                    return LoadDataType.Add;
                else
                    return LoadDataType.DownLoad; 
            }
        }

        public override bool ShowSpecialComfirmWhenPrint()
        {
            return true;
        }

        public override string Name
        {
            get { return "体检"; }
        }

        public override bool ShouldMergeCollect
        {
            get { return true; }
        }


    }
}
