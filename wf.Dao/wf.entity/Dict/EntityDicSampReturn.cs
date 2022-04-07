using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 标本回退字典表：bc_message
    /// 旧表名:dic_samp_return 新表名:Dict_sample_return
    /// </summary>
    [Serializable]
    public class EntityDicSampReturn : EntityBase
    {
        public EntityDicSampReturn()
        {
            this.Checked = false;
        }

        /// <summary>
        /// 编号
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_id", MedName = "return_id",WFName = "Dreturn_id")]
        public Int32 ReturnId { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_content", MedName = "return_content", WFName = "Dreturn_content")]
        public String ReturnContent { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_user_id", MedName = "return_user_id", WFName = "Dreturn_Buser_id")]
        public String ReturnUserId { get; set; }


        #region 附加字段 是否选中

        public Boolean Checked { get; set; }

        #endregion
    }
}
