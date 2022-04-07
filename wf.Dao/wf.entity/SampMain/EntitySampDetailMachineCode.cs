using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 条码上机信息项目通道码信息
    /// </summary>
    [Serializable]
    public class EntitySampDetailMachineCode : EntityBase
    {
        /// <summary>
        /// HIS项目名称 关联EntitySampDetail OrderName
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_his_name", MedName = "order_name", WFName = "order_name", DBIdentity = true)]
        public string OrderName { get; set; }

        /// <summary>
        /// 项目代码 关联EntityDicCombineDetail的ComItmEname
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_itm_ecd", MedName = "com_itm_ename", WFName = "com_itm_ename")]
        public String ComItmEname { get; set; }

        /// <summary>
        /// 项目ID 关联关联EntityDicCombineDetail的ComItmEname的ComItmId 
        /// </summary>   
        [FieldMapAttribute(ClabName = "com_itm_id", MedName = "com_itm_id", WFName = "com_itm_id")]
        public String ComItmId { get; set; }

        /// <summary>
        /// 仪器组合ID 关联EntityItrCombine的ComId
        /// </summary>   
        [FieldMapAttribute(ClabName = "itr_com_id", MedName = "itr_com_id", WFName = "itr_com_id")]
        public Int32 ItrComId { get; set; }

        /// <summary>
        /// HIS项目名称 关联关联EntitySampDetail的ComId
        /// </summary>   
        [FieldMapAttribute(ClabName = "bc_lis_code", MedName = "com_id", WFName = "com_id")]
        public String ComId { get; set; }

        /// <summary>
        /// 通道码 关联EntityDicMachineCode的MacCode
        /// </summary>   
        [FieldMapAttribute(ClabName = "mit_cno", MedName = "mac_code", WFName = "mac_code")]
        public String MacCode { get; set; }

    }
}
