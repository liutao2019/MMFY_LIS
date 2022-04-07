using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.svr.cache;
using dcl.pub.entities.dict;

namespace dcl.svr.sample
{
    /// <summary>
    /// 组合合并拆分逻辑
    /// </summary>
    public class CombineSplitMergeBIZ
    {
        private string _ori_id = string.Empty;
        public string OriID
        {
            get
            {
                return _ori_id;
            }
            set
            {
                if (value == null)
                {
                    _ori_id = string.Empty;
                }
                else
                {
                    _ori_id = value.Trim();
                }
            }
        }

        public CombineSplitMergeBIZ(string ori_id)
        {
            this.OriID = ori_id;
        }

        /// <summary>
        /// 获得大小组合拆分后的组合
        /// </summary>
        /// <param name="coms_id"></param>
        /// <returns></returns>
        public List<EntityDictCombineBar> GetSplitedCombineBarByComsID(string[] coms_id)
        {
            List<EntityDictCombineBar> ret = new List<EntityDictCombineBar>();

            foreach (string com_id in coms_id)
            {
                EntityDictCombineBar com_bar = DictCombineBarCache.Current.GetCombineBarWithComID(com_id, this._ori_id);
                if (com_bar != null)//如果不为空
                {
                    if (com_bar.com_split_flag == 1)//如果有拆分标志
                    {
                        string[] subComs = DictCombineSplitCache.Current.GetSplitedCombine(com_bar.com_id);

                        List<EntityDictCombineBar> subComsBar = GetSplitedCombineBarByComsID(subComs);

                        #region 生成特殊项目合并大组合ID

                        if (com_bar.com_father_flag == 1)
                        {
                            string guid_MergeComID = new Lib.DAC.GlobalSysTableIDGenerator().Generate("bc_patients", "MergeComID", "");
                            com_bar.MergeComID = guid_MergeComID + "," + com_bar.com_id;
                        }

                        #endregion

                        foreach (EntityDictCombineBar sub_com_bar in subComsBar)
                        {
                            if (!ret.Exists(i => i.com_id == sub_com_bar.com_id))
                            {

                                if (com_bar.com_father_flag == 1)
                                {
                                    //小组合加入,特殊项目合并大组合ID
                                    sub_com_bar.MergeComID = com_bar.MergeComID;
                                }

                                ret.Add(sub_com_bar);
                            }
                        }
                    }
                    else
                    {
                        ret.Add(com_bar);
                    }
                }
                else//如果为空
                {
                    EntityDictCombine comInfo = DictCombineCache.Current.GetCombineByID(com_id, true);

                    com_bar = new EntityDictCombineBar();
                    com_bar.com_ori_id = this.OriID;
                    com_bar.com_id = com_id;
                    com_bar.com_his_name = comInfo.com_name;

                    ret.Add(com_bar);
                }
            }
            return ret;
        }

        public List<EntityDictCombineBar> GetSplitedCombineBarByHISCode(string[] his_code)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 合并条码
        /// </summary>
        /// <param name="source_combine"></param>
        /// <returns></returns>
        public Dictionary<string, List<EntityDictCombineBar>> MergeCombine(List<EntityDictCombineBar> source_combine)
        {
            Dictionary<string, List<EntityDictCombineBar>> ret = new Dictionary<string, List<EntityDictCombineBar>>();

            //遍历传入的组合信息
            foreach (EntityDictCombineBar com_bar in source_combine)
            {
                string split_code = com_bar.com_split_code;

                if (string.IsNullOrEmpty(split_code))
                {
                    split_code = Guid.NewGuid().ToString();
                }

                if (ret.Keys.Contains(split_code))
                {
                    ret[split_code].Add(com_bar);
                }
                else
                {
                    List<EntityDictCombineBar> list = new List<EntityDictCombineBar>();
                    list.Add(com_bar);

                    ret.Add(split_code, list);
                }
            }

            return ret;
        }

        public Dictionary<string, List<EntityDictCombineBar>> SplitAndMerge(string[] coms_id)
        {
            List<EntityDictCombineBar> source_combine = GetSplitedCombineBarByComsID(coms_id);
            Dictionary<string, List<EntityDictCombineBar>> ret = MergeCombine(source_combine);
            return ret;
        }
    }
}
