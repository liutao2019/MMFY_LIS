using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 报告SQL的where条件
    /// </summary>
    [Serializable]
    public class EntityReportCondition
    {
        /// <summary>
        ///条件
        /// </summary>   
        public String Where { get; set; }

        /// <summary>
        ///代码
        /// </summary>   
        public String Code { get; set; }

        /// <summary>
        ///开始时间
        /// </summary>   
        public String StartDate { get; set; }

        /// <summary>
        ///结束时间
        /// </summary>   
        public String EndDate { get; set; }

        /// <summary>
        ///当前时间
        /// </summary>   
        public String NowDate { get; set; }

        /// <summary>
        ///标题
        /// </summary>   
        public String Title { get; set; }

        /// <summary>
        ///分类
        /// </summary>   
        public String Group { get; set; }

        /// <summary>
        ///选择分类
        /// </summary>   
        public String GroupSelect { get; set; }

        /// <summary>
        ///项目分类
        /// </summary>   
        public String GroupItem { get; set; }

        /// <summary>
        ///类型
        /// </summary>   
        public String Type { get; set; }

        /// <summary>
        ///所有分类
        /// </summary>   
        public String GroupAllah { get; set; }

        /// <summary>
        ///额外条件
        /// </summary>   
        public String SubWhere { get; set; }

        /// <summary>
        ///项目条件
        /// </summary>   
        public String ItemWhere { get; set; }

        /// <summary>
        ///条码条件
        /// </summary>   
        public String BarWhere { get; set; }

    }
}
