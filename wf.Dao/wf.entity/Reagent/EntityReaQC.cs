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
    [Serializable]
    public class EntityReaQC : EntityBase
    {
        
        public String ObrSn { get; set; }

        /// <summary>
        /// 入库单号
        /// </summary>
        public String PurNo { get; set; }
        /// <summary>
        /// 单号ID
        /// </summary>
        public String ReaNo { get; set; }

        /// <summary>
        /// 试剂编码
        /// </summary>
        public String ReaId { get; set; }
        /// <summary>
        /// 供货商
        /// </summary>
        public String SupId { get; set; }
        /// <summary>
        /// 试剂组别
        /// </summary>
        public String GrpId { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 条形码
        /// </summary>
        public String Barcode { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? DateStart { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? DateEnd { get; set; }
        /// <summary>
        /// 状态 1-未审核 4-已审核 2-审核不通过 8-未打印 9-已完成
        /// </summary>
        public String ReaStatus { get; set; }

        public String ReaPrintFlag { get; set; }

        public bool WithTime { get; set; }

        public bool JudgeCount { get; set; }

        public bool JudgeValidtime { get; set; }
    }
}
