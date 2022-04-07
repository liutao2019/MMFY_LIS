using System.Collections.Generic;
using dcl.entity;
using System.Linq;

namespace dcl.client.cache
{
    public class DictType
    {
        public static DictType Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DictType();
                }
                return _instance;
            }
        }

        private static DictType _instance;

        public List<EntityDicPubProfession> ListDictType
        {
            get
            {

                List<EntityDicPubProfession> list = CacheClient.GetCache<EntityDicPubProfession>();
                return list;
            }
        }

        public EntityDicPubProfession GetCType(string type_id)
        {
            List<EntityDicPubProfession>list = ListDictType.Where(w=>w.ProId.ToString()==type_id).ToList();

            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 根据或去组别名称
        /// </summary>
        /// <param name="type_id"></param>
        /// <returns></returns>
        public string GetTypeName(string type_id)
        {
            List<EntityDicPubProfession> list = ListDictType.Where(w => w.ProId.ToString() == type_id).ToList();

            if (list.Count > 0 && !string.IsNullOrEmpty(list[0].ProName))
            {
                return list[0].ProName;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
