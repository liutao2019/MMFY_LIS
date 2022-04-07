using System.Collections.Generic;
using System.Data;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result.CommonPatientInput
{
    public class DictCombineMi
    {
        public static DictCombineMi Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DictCombineMi();
                }
                return _instance;
            }
        }

        private static DictCombineMi _instance;


        List<EntityDicCombineDetail> dtDictCombineMi
        {
            get
            {
                return CacheClient.GetCache<EntityDicCombineDetail>();
            }
        }

        public DictCombineMi()
        {
        }

        /// <summary>
        /// 获取组合对应的所有项目
        /// </summary>
        /// <param name="com_id"></param>
        /// <returns></returns>
        public List<EntityDicCombineDetail> GetCombineMi(string com_id, string sex)
        {
            List<EntityDicCombineDetail> drsComItems = null;
            if (sex == null || sex.ToString().Trim() == string.Empty)
            {
                //获取组合对应的所有项目
                drsComItems = this.dtDictCombineMi.FindAll(i => i.ComId == com_id);
            }
            else
            {
                drsComItems = this.dtDictCombineMi.FindAll(i => i.ComId == com_id);
                                                            //&& (i.ItmMatchSex == sex || i.ItmMatchSex == ""
                                                            //|| i.ItmMatchSex == "0" || i.ItmMatchSex == null));
            }

            return drsComItems.OrderBy(i => i.ComSortNo).ToList();
        }

        public List<EntityDicCombineDetail> GetCombineMibyResult(List<EntityPidReportDetail> dtPatMi, List<EntityObrResult> dtResult)
        {
            List<EntityDicCombineDetail> dtRet = new List<EntityDicCombineDetail>();
            if (dtPatMi != null && dtPatMi.Count > 0)
            {
                string strPatMiWhere = string.Empty;
                List<string> comIds = new List<string>();
                List<string> itmIds = new List<string>();
                foreach (EntityPidReportDetail patMi in dtPatMi)
                {
                    comIds.Add(patMi.ComId);
                }
                string strResult = string.Empty;
                foreach (EntityObrResult res in dtResult)
                {
                    itmIds.Add(res.ItmId);
                }

                if (itmIds.Count > 0)
                    dtRet = (from x in this.dtDictCombineMi
                             where comIds.ToArray().Contains(x.ComId)
                                    && !itmIds.ToArray().Contains(x.ComItmId)
                             select x).ToList();
                else
                    dtRet = (from x in this.dtDictCombineMi
                             where comIds.ToArray().Contains(x.ComId)
                             select x).ToList();
            }
            return dtRet;
        }
    }
}
