using System;
using System.Collections.Generic;

using System.Text;
using dcl.client.common;
using System.Data;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result.DictToolkit
{
    public class DictPatNumberType
    {
        public static DictPatNumberType Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DictPatNumberType();
                }
                return _instance;
            }
        }


        private static DictPatNumberType _instance;

        public List<EntityDicPubIdent> listDictType
        {
            get
            {
                List<EntityDicPubIdent> list = CacheClient.GetCache<EntityDicPubIdent>();
                return list;
            }
        }

        public EntityDicPubIdent GetNoType(string no_id)
        {
            if (string.IsNullOrEmpty(no_id))
            {
                return null;
            }

            List<EntityDicPubIdent> list= listDictType.Where(w=>w.IdtId==no_id).ToList();
            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public string GetOriID_byNoType(string no_id)
        {
            string ret = string.Empty;

            if (!string.IsNullOrEmpty(no_id))
            {
             EntityDicPubIdent ident = GetNoType(no_id);
                if (ident != null)
                {
                    ret = ident.IdtSrcId;
                }
            }

            return ret;
        }
    }
}
