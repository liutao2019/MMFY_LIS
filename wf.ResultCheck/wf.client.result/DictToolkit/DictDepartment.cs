using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dcl.client.common;
using dcl.common;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result.DictToolkit
{
    public class DictDepartment
    {
        public static DictDepartment Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DictDepartment();
                }
                return _instance;
            }
        }

        private static DictDepartment _instance;

        public List<EntityDicPubDept> DictItr
        {
            get
            {
                List<EntityDicPubDept> list = CacheClient.GetCache<EntityDicPubDept>();
                return list;
            }
        }

        public string GetWardCode(string dep_code)
        {
           List<EntityDicPubDept> list = DictItr.Where(w=>w.DeptCode==dep_code).ToList();
            if (list.Count > 0 && !Compare.IsEmpty(list[0].DeptOrgId))
            {
                return list[0].DeptOrgId;
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
