using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.root.logon
{
    /// <summary>
    /// 日志记录
    /// </summary>
    public class ExceptionLogEntity
    {
        public DateTime timestamp { get; set; }
        internal ExceptionLogType logType { get; set; }
        public string ProcessName { get; set; }
        public string Module { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            string format =
@"类型：{1} 
时间：{0}
进程：{2}
模块：{3}
标题：{4}
附加信息:
{5}
";
            return string.Format(format, timestamp, logType, ProcessName, Module, Title, Message);
        }
    }
}
