using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.result.CommonPatientInput
{
    public class DictCombine
    {
        public static DictCombine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DictCombine();
                }
                return _instance;
            }
        }

        private static DictCombine _instance;

        List<EntityDicCombine> dtCombine
        {
            get
            {
                return CacheClient.GetCache<EntityDicCombine>();
            }
        }

        public DictCombine()
        {
        }

        /// <summary>
        /// 根据ID获取组合
        /// </summary>
        /// <param name="com_id"></param>
        /// <returns></returns>
        public EntityDicCombine GetCombinebyID(string com_id)
        {
            EntityDicCombine combine = dtCombine.Find(i => i.ComId == com_id);

            if (combine != null)
            {
                return combine;
            }

            return null;
        }
    }
}
