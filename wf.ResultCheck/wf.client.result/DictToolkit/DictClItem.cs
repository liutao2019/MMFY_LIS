using System.Collections.Generic;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.result.DictToolkit
{
    public class DictClItem
    {
        public static DictClItem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DictClItem();
                }
                return _instance;
            }
        }

        private static DictClItem _instance;


        /// <summary>
        /// 项目基本信息表
        /// </summary>
        public List<EntityDicItmCalu> Dict_cl_item
        {
            get
            {
                return CacheClient.GetCache<EntityDicItmCalu>();
            }
        }

        public string GetCalItems(string itmId)
        {
            string ret = string.Empty;

            List<EntityDicItmCalu> drs = this.Dict_cl_item.FindAll(i => i.ItmId == itmId && i.CalFlag == 1);

            if (drs.Count > 0)
            {
                List<string> list = new List<string>();

                foreach (EntityDicItmCalu dr in drs)
                {
                    string[] cal_variable = dr.CalVariable.ToString().Split(',');

                    if (cal_variable.Length > 0)
                    {
                        foreach (string item_ecd in cal_variable)
                        {
                            if (!string.IsNullOrEmpty(item_ecd))
                            {
                                if (!list.Exists(i => i == item_ecd))
                                {
                                    list.Add(item_ecd);
                                }
                            }
                        }
                    }
                }

                foreach (string item_ecd in list)
                {
                    ret += string.Format(",{0}", item_ecd);
                }
                if (ret.Length > 0)
                    ret = ret.Remove(0, 1);
            }
            return ret;
        }
    }
}
