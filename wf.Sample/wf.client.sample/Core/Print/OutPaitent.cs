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
    /// 门诊条码
    /// </summary>
    public class OutPaitent : IPrint
    {

        public override DateTimeRange GetDefaultAdviceTime()
        {
            double dblStart = -28;
            double dblEnd = 1;
            try
            {
                //获取参数设置查询日期范围
                dblStart = Convert.ToDouble(ConfigHelper.GetSysConfigValueWithoutLogin("OpBarcodeDownStartDate"));
                dblEnd = Convert.ToDouble(ConfigHelper.GetSysConfigValueWithoutLogin("OpBarcodeDownEndDate")) + 1;
            }
            catch (Exception)
            {
                dblStart = -28;
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
            get { return LoadDataType.DownLoad; }
        }

        public override string Name
        {
            get { return "门诊"; }
        }

        public override bool ShouldMergeCollect
        {
            get { return true; }
        }

        public override DockStyle ButtonDock
        {
            get
            {
                return DockStyle.Top;
            }
        }
    }
}
