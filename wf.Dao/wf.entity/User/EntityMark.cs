using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 标准列表
    /// </summary>
    [Serializable]
    public class EntityMark:EntityBase
    {
        public EntityMark()
        {
            Checked = false;
        }
        /// <summary>
        ///性别名称
        /// </summary>   
        public String MarkName { get; set; }

        /// <summary>
        ///拼音码
        /// </summary>   
        public String MarkPy { get; set; }

        /// <summary>
        ///五笔码
        /// </summary>   
        public String MarkWb { get; set; }

        /// <summary>
        ///主键
        /// </summary>   
        public string SpId { get; set; }

        /// <summary>
        ///是否选中
        /// </summary>   
        public Boolean Checked { get; set; }

    }
}
