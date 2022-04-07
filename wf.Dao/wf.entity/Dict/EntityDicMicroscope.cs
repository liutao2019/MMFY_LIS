using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 镜检字典表
    /// </summary>
    [Serializable]
    
    public class EntityDicMicroscope : EntityBase
    {
        public EntityDicMicroscope()
        {
            MicroSortNo = 0;
        }
        
        /// <summary>
        /// 镜检编码
        /// </summary>                       
        public String MicroId { get; set; }

        /// <summary>
        /// 镜检类型
        /// </summary>                       
        public String MicroName { get; set; } 


        /// <summary>
        /// 拼音码
        /// </summary>                       
        public String MicroPyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>                       
        public String MicroWbCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>                       
        public Int32 MicroSortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>                       
        public String MicroDelFlag { get; set; }

        /// <summary>
        /// 镜检编码
        /// </summary>                       
        public String SourceId { get; set; }
    }
}
