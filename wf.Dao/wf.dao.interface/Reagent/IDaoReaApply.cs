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
using dcl.dao.core;
using dcl.entity;

namespace dcl.dao.interfaces
{
    public interface IDaoReaApply: IDaoBase
    {
        string GetReaSID_MaxPlusOne(DateTime date, string stepCode);
        bool ExsitSidOrHostOrder(string rea_sid, DateTime rea_in_date);
        bool InsertNewReaApply(EntityReaApply apply);
        List<EntityReaApply> QueryApplyList(EntityReaQC reaQC);
        bool UpdateReaApplyData(EntityReaApply reaApply);
        bool DeleteReaApplyData(EntityReaApply reaApply);
        EntityReaApply GetReaApplyInfo(string rayNo);
        bool ReturnReaApplyData(EntityReaApply reaApply);
        void UpdateApplyStatus(string rayNo, string printId, DateTime date);
        void UpdateStatus(string rayNo, int status);
            }
}
