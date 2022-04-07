using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 操作结果数据
    /// </summary>
    [Serializable]
    public class EntityOperateError : EntityBase
    {
        public EnumOperateErrorCode ErrorCode { get; set; }

        public EnumOperateErrorLevel ErrorLevel { get; set; }
        public string Param { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }

        public string CustomMessageKey { get; set; }
        public string CustomMessageTitle { get; set; }

        public EntityOperateError()
        {
            this.ErrorLevel = EnumOperateErrorLevel.None;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}:{3}", ErrorCode, CustomMessageKey, CustomMessageTitle, Param);
        }
    }
}
