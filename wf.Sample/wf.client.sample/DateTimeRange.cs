using System;
using System.Collections.Generic;

using System.Text;
using dcl.common.extensions;
using dcl.common;

namespace dcl.client.sample
{
    /// <summary>
    /// 时间区间
    /// </summary>
    public class DateTimeRange
    {
        public DateTimeRange(DateTime start, DateTime end)
        {
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime End { get; set; }
    }
}