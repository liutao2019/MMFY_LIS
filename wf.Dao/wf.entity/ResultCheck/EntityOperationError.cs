using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityOperationError
    {
        public EnumOperationErrorCode ErrorCode { get; set; }

        public EnumOperationErrorLevel ErrorLevel { get; set; }
        public string Param { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }

        public string CustomMessageKey { get; set; }
        public string CustomMessageTitle { get; set; }

        public EntityOperationError()
        {
            this.ErrorLevel = EnumOperationErrorLevel.None;
        }

        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}:{3}", ErrorCode, CustomMessageKey, CustomMessageTitle, Param);
        }
    }

    //[Serializable]
    //public class EntityCustomOperationError : EntityOperationError
    //{
    //    public string CustomMessageKey { get; set; }
    //    public string CustomMessageTitle { get; set; }

    //    public EntityCustomOperationError()
    //        : base()
    //    {
    //        this.CustomMessageTitle = string.Empty;
    //    }
    //}
}
