using System;
using System.Collections.Generic;
using System.Linq;
using dcl.entity;

namespace dcl.svr.cache
{
    /// <summary>
    /// 项目危急值缓存
    /// </summary>
    public class DictItemUrgentValueCache
    {
        private static DictItemUrgentValueCache _instance = null;
        private static object padlock = new object();

        public static DictItemUrgentValueCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DictItemUrgentValueCache();
                        }
                    }
                }
                return _instance;
            }
        }

        public List<EntityDicUtgentValue> DclCache { get; private set; }
        private DictItemUrgentValueCache()
        {
            ThreadRefresh();
        }


        public EntityDicUtgentValue GetValue(string itm_id, string sam_id, string dep_code, string sex, int age_int, string ugt_icd_name = "")
        {
            if (string.IsNullOrEmpty(dep_code))
                dep_code = "-1";

            if (string.IsNullOrEmpty(sam_id))
                sam_id = "-1";

            if (sex == "1" || sex == "男")
                sex = "男";
            else if (sex == "2" || sex == "女")
                sex = "女";
            else
                sex = string.Empty;

            var query = from item in this.DclCache
                        where item.AgeLInt <= age_int && item.AgeHInt >= age_int
                        && item.UgtItmId == itm_id
                        && (item.UgtSex == sex || item.UgtSex == "0" || string.IsNullOrEmpty(item.UgtSex))
                        && (item.UgtSamId == sam_id || item.UgtSamId == "-1")
                        && (item.UgtDepCode == dep_code || item.UgtDepCode == "-1")
                        orderby item.UgtSamId descending, item.UgtDepCode descending, item.UgtSex descending
                        select item;

            if (!string.IsNullOrEmpty(ugt_icd_name))
            {
                query = from item in this.DclCache
                        where item.AgeLInt <= age_int && item.AgeHInt >= age_int
                        && (item.UgtIcdName.Contains(ugt_icd_name.Trim()))
                        && item.UgtItmId == itm_id
                        && (item.UgtSex == sex || item.UgtSex == "0" || string.IsNullOrEmpty(item.UgtSex))
                        && (item.UgtSamId == sam_id || item.UgtSamId == "-1")
                        && (item.UgtDepCode == dep_code || item.UgtDepCode == "-1")
                        orderby item.UgtSamId descending, item.UgtDepCode descending, item.UgtSex descending
                        select item;
            }

            if (query.Count() > 0)
            {
                List<EntityDicUtgentValue> list = query.ToList<EntityDicUtgentValue>();
                return list[0];
            }
            else
            {
                return null;
            }
        }

        public EntityDicUtgentValue GetDclValue(string itm_id, string sam_id, string dep_code, string sex, int age_int, string patDiag = "")
        {
            if (string.IsNullOrEmpty(dep_code))
                dep_code = "-1";

            if (string.IsNullOrEmpty(sam_id))
                sam_id = "-1";

            if (sex == "1" || sex == "男")
                sex = "男";
            else if (sex == "2" || sex == "女")
                sex = "女";
            else
                sex = string.Empty;

            var query = from item in this.DclCache
                        where item.AgeLInt <= age_int && item.AgeHInt >= age_int
                        && item.UgtItmId == itm_id
                        && (item.UgtSex == sex || item.UgtSex == "0" || string.IsNullOrEmpty(item.UgtSex))
                        && (item.UgtSamId == sam_id || item.UgtSamId == "-1")
                        && (item.UgtDepCode == dep_code || item.UgtDepCode == "-1")
                        && item.UgtIcdName.Contains(patDiag)
                        orderby item.UgtSamId descending, item.UgtDepCode descending, item.UgtSex descending
                        select item;

            if (query.Count() > 0)
            {
                List<EntityDicUtgentValue> list = query.ToList<EntityDicUtgentValue>();
                return list[0];
            }
            else
            {
                return null;
            }
        }

        private void ThreadRefresh()
        {
            lock (padlock)
            {
                try
                {
                    CacheDataBIZ cache = new CacheDataBIZ();
                    EntityResponse response = cache.GetCacheData("EntityDicUtgentValue");
                    DclCache = response.GetResult<List<EntityDicUtgentValue>>();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
