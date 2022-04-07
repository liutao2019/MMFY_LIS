using dcl.client.cache;
using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.client.sample
{
    /// <summary>
    /// 手工条码
    /// </summary>
    public class Manual : IPrint
    {
        public override bool NeedPrint
        {
            get
            {
                return false;
            }
        }
        #region IPrint 成员

        public override DateTimeRange GetDefaultAdviceTime()
        {
            DateTime dtNow = ServerDateTime.GetServerDateTime();
            return new DateTimeRange(dtNow.Date, dtNow.Date.AddDays(1).AddMinutes(-1));  
        }

        #endregion

        public override LoadDataType LoadDataType
        {
            get {return LoadDataType.Add; }
        }

        public override string Name
        {
            get { return "手工"; }
        }

        public override bool ShouldMergeCollect
        {
            get { return false; }
        }
    }
}
