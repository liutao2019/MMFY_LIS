using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 系统登录错误信息
    /// </summary>
    [Serializable]
    public class EntityLoginErrorInfo : EntityBase
    {
        /// <summary>
        ///错误信息
        /// </summary>   
        [FieldMapAttribute(ClabName = "_ERROR_MSG", MedName = "_ERROR_MSG", WFName = "_ERROR_MSG")]
        public string ErrorMsg { get; set; }

        /// <summary>
        ///错误详细信息
        /// </summary>   
        [FieldMapAttribute(ClabName = "_ERROR_DETAIL", MedName = "_ERROR_DETAIL", WFName = "_ERROR_DETAIL")]
        public String ErrorDetail { get; set; }

        /// <summary>
        ///错误码
        /// </summary>   
        [FieldMapAttribute(ClabName = "_ERROR_CODE", MedName = "_ERROR_CODE",WFName = "_ERROR_CODE")]
        public String ErrorCode { get; set; }

        /// <summary>
        ///登录状态
        /// </summary>   
        [FieldMapAttribute(ClabName = "loginStatus", MedName = "loginStatus", WFName = "loginStatus")]
        public Int32 LoginStatus { get; set; }

    }
}
