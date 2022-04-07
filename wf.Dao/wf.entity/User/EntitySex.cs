using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 性别列表
    /// </summary>
    [Serializable]
    public class EntitySex : EntityBase
    {
        public EntitySex()
        {
            Checked = false;
        }
        /// <summary>
        ///性别名称
        /// </summary>   
        public String SexName { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        public String SexPy { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        public String SexWb { get; set; }

        /// <summary>
        ///主键
        /// </summary>   
        public String SpId { get; set; }

        /// <summary>
        ///是否选中
        /// </summary>   
        public Boolean Checked { get; set; }

    }
}
