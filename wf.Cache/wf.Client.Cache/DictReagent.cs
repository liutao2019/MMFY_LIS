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
using dcl.entity;

namespace dcl.client.cache
{
    public class DictReagent
    {
        public static DictReagent Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DictReagent();
                }
                return _instance;
            }
        }

        private static DictReagent _instance;


        public List<EntityReaSetting> DclDictRea
        {
            get
            {
                return CacheClient.GetCache<EntityReaSetting>();
            }
        }

        public EntityReaSetting GetRea(string group_id)
        {
            if (group_id == null) group_id = string.Empty;
            List<EntityReaSetting> drsRea = this.DclDictRea.Where(i => i.Drea_group == group_id).ToList();
            if (drsRea.Count > 0)
            {
                return drsRea[0];
            }
            else
            {
                return null;
            }
        }
        public EntityReaSetting GetReaByID(string rea_id)
        {
            if (rea_id == null) rea_id = string.Empty;
            List<EntityReaSetting> drsRea = this.DclDictRea.Where(i => i.Drea_id == rea_id).ToList();
            if (drsRea.Count > 0)
            {
                return drsRea[0];
            }
            else
            {
                return null;
            }
        }
        public EntityReaSetting GetReaBySupID(string sup_id)
        {
            if (sup_id == null) sup_id = string.Empty;
            List<EntityReaSetting> drsRea = this.DclDictRea.Where(i => i.Drea_supplier == sup_id).ToList();
            if (drsRea.Count > 0)
            {
                return drsRea[0];
            }
            else
            {
                return null;
            }
        }
    }
}
