using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 日志查询条件
    /// </summary>
    [Serializable]
    public class EntityLogQc : EntityBase
    {
        /// <summary>
        ///操作模块
        /// </summary>   
        public string OperatModule { get; set; }

        /// <summary>
        ///开始时间
        /// </summary>   
        public string DateStart { get; set; }


        /// <summary>
        ///结束时间
        /// </summary>   
        public String DateEnd { get; set; }

        /// <summary>
        ///操作人id
        /// </summary>   
        public String OperationUserId { get; set; }

        /// <summary>
        /// 操作值（病人id）
        /// </summary>
        public string Operatkey { get; set; }

        /// <summary>
        /// 操作内容(质控日志修改查询（项目代码）)
        /// </summary>
        public string OperatObject { get; set; }


        /// <summary>
        /// 操作项目
        /// </summary>
        public string OperatGroup { get; set; }
    }
}
