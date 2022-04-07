/*  
 * 警告：
 * 本源代码所有权归广州慧扬健康科技有限公司(下称“本公司”)所有，已采取保密措施加以保护。  受《中华人民共和国刑法》、
 * 《反不正当竞争法》和《国家工商行政管理局关于禁止侵犯商业秘密行为的若干规定》等相关法律法规的保护。未经本公司书面
 * 许可，任何人披露、使用或者允许他人使用本源代码，必将受到相关法律的严厉惩罚。
 * Warning: 
 * The ownership of this source code belongs to Guangzhou Wisefly Technology Co., Ltd.(hereinafter referred to as "the company"), 
 * which is protected by the criminal law of the People's Republic of China, the anti unfair competition law and the 
 * provisions of the State Administration for Industry and Commerce on prohibiting the infringement of business secrets, etc. 
 * Without the written permission of the company, anyone who discloses, uses or allows others to use this source code 
 * will be severely punished by the relevant laws.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// Rel_rea_inventory
    /// </summary>
    [Serializable]
    public class EntityReaStoreCount:EntityBase
    {
        [FieldMapAttribute(ClabName = "Rri_Drea_id", MedName = "Rri_Drea_id", WFName = "Rri_Drea_id")]
        public System.String Rri_Drea_id { get; set; }
        [FieldMapAttribute(ClabName = "Rri_Count", MedName = "Rri_Count", WFName = "Rri_Count")]
        public System.Int32 Rri_Count { get; set; }
        [FieldMapAttribute(ClabName = "Rsd_id", MedName = "Rsd_id", WFName = "Rsd_id",DBIdentity =true)]
        public System.Int64 Rsd_id { get; set; }

    }
}
