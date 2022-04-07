using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 酶标计算公式
    /// 旧表名:def_Elisa_calu 新表名:Rel_Elisa_calculaformula
    /// </summary>
    [Serializable]
    public class EntityDicElisaCalu : EntityBase
    {
       
            /// <summary>
            ///编号
            /// </summary>   
            [FieldMapAttribute(ClabName = "imc_id", MedName = "cal_id", WFName = "Recal_id")]
            public String CalId { get; set; }

            /// <summary>
            ///测试项目编号
            /// </summary>   
            [FieldMapAttribute(ClabName = "imc_itm_id", MedName = "cal_itm_id", WFName = "Recal_Ditm_id")]
            public String CalItmId { get; set; }

            /// <summary>
            ///计算公式表达式
            /// </summary>   
            [FieldMapAttribute(ClabName = "imc_calc", MedName = "cal_expression", WFName = "Recal_expression")]
            public String CalExpression { get; set; }

            /// <summary>
            ///删除标志
            /// </summary>   
            [FieldMapAttribute(ClabName = "imc_del", MedName = "del_flag", WFName = "del_flag")]
            public String CalDelFlag { get; set; }

        }


    
}
