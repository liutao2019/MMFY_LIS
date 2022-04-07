using System;
using System.Collections.Generic;
using dcl.entity;
using System.Linq;

namespace dcl.client.cache
{
    public class DictInstrmt
    {
        public static DictInstrmt Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DictInstrmt();
                }
                return _instance;
            }
        }

        private static DictInstrmt _instance;


        public List<EntityDicInstrument> DclDictItr
        {
            get
            {
                return CacheClient.GetCache<EntityDicInstrument>();
            }
        }
        /// <summary>
        /// 仪器组合
        /// </summary>
        public List<EntityDicItrCombine> DictItrCom
        {
            get
            {
                return CacheClient.GetCache<EntityDicItrCombine>();
            }
        }

        public DictInstrmt()
        {

        }

        /// <summary>
        /// 获取仪器的专业组别ID
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrPTypeID(string itr_id)
        {
            try
            {
                string ptype = string.Empty;

                if (itr_id == null) itr_id = string.Empty;
                List<EntityDicInstrument> drsItr = this.DclDictItr.Where(i => i.ItrId == itr_id).ToList();

                if (drsItr.Count > 0 && !string.IsNullOrEmpty(drsItr[0].ItrLabId))
                {
                    ptype = drsItr[0].ItrLabId.ToString();
                }

                return ptype;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return string.Empty;
            }
        }



        /// <summary>
        /// 获取仪器的专业组别ID
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public bool ShowPicInResList(string itr_id)
        {
            bool istrue = false;
            try
            {
                string itr_pic_flag = string.Empty;

                if (itr_id == null) itr_id = string.Empty;
                List<EntityDicInstrument> drsItr = this.DclDictItr.Where(i => i.ItrId == itr_id).ToList();

                if (drsItr.Count > 0)
                {
                    itr_pic_flag = drsItr[0].ItrImageFlag.ToString();
                }

                return itr_pic_flag == "1";
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return istrue;
            }
        }


        /// <summary>
        /// 获取仪器的数据类型
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrRepFlag(string itr_id)
        {
            try
            {
                string ptype = string.Empty;

                if (itr_id == null) itr_id = string.Empty;
                List<EntityDicInstrument> drsItr = this.DclDictItr.Where(i => i.ItrId == itr_id).ToList();

                if (drsItr.Count > 0 && !string.IsNullOrEmpty(drsItr[0].ItrReportType))
                {
                    ptype = drsItr[0].ItrReportType.ToString();
                }

                return ptype;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取仪器的编号
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrMid(string itr_id)
        {
            try
            {
                string itr_mid = string.Empty;

                if (itr_id == null) itr_id = string.Empty;
                List<EntityDicInstrument> listItr = this.DclDictItr.Where(w => w.ItrId == itr_id).ToList();

                if (listItr.Count > 0 && listItr[0].ItrEname != null)
                {
                    itr_mid = listItr[0].ItrEname;
                }

                return itr_mid;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取仪器默认组合ID
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrComID(string itr_id)
        {
            EntityDicInstrument dr = GetItr(itr_id);
            if (dr != null && !string.IsNullOrEmpty(dr.ItrComId))
            {
                return dr.ItrComId;
            }
            return string.Empty;
        }


        public string GetItrPrtTemplate(string itr_id)
        {
            EntityDicInstrument dr = GetItr(itr_id);
            if (dr != null && !string.IsNullOrEmpty(dr.ItrReportId))
            {
                return dr.ItrReportId;
            }
            return string.Empty;
        }

        public EntityDicInstrument GetItr(string itr_id)
        {
            if (itr_id == null) itr_id = string.Empty;
            List<EntityDicInstrument> drsItr = this.DclDictItr.Where(i => i.ItrId == itr_id).ToList();
            if (drsItr.Count > 0)
            {
                return drsItr[0];
            }
            else
            {
                return null;
            }
        }
        public EntityDicInstrument GetDclItr(string itr_id)
        {
            if (itr_id == null) itr_id = string.Empty;
            List<EntityDicInstrument> listItr = this.DclDictItr.Where(w => w.ItrId == itr_id).ToList();
            if (listItr.Count > 0)
            {
                return listItr[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取仪器的通讯方式 0=单向 1=双向
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public int GetItrHostFlag(string itr_id)
        {
            int hostflag = 1;

            EntityDicInstrument dr = GetItr(itr_id);

            if (dr != null)
            {
                if (!IsEmpty(dr.ItrCommType) && dr.ItrCommType.ToString().Trim() != string.Empty)
                {
                    hostflag = Convert.ToInt32(dr.ItrCommType);
                }
            }

            return hostflag;
        }

        /// <summary>
        /// 获取仪器组合
        /// </summary>
        /// <param name="getMergeItrCombine">是否获取合并仪器组合</param>
        /// <returns></returns>
        public List<string> GetItrCombineID(string itr_id, bool getMergeItrCombine)
        {
            List<string> listCom = new List<string>();

            string sqlItrCombineIn = string.Format(" '{0}' ", itr_id);

            if (getMergeItrCombine)
            {
                //查找所有"存储仪器"为当前仪器的仪器
                List<EntityDicInstrument> drsConItr = this.DclDictItr.Where(i => i.ItrReportItrId == itr_id).ToList();

                if (drsConItr.Count > 0)
                {
                    foreach (EntityDicInstrument drItr in drsConItr)
                    {
                        string itrid = drItr.ItrId.ToString();
                        sqlItrCombineIn += string.Format(",'{0}'", itrid);
                    }
                }
            }
            List<EntityDicItrCombine> drs = DictItrCom.Where(i => sqlItrCombineIn.Contains(i.ItrId)).ToList();

            foreach (EntityDicItrCombine dr in drs)
            {
                if (!IsEmpty(dr.ComId))
                {
                    listCom.Add(dr.ComId.ToString());
                }
            }

            return listCom;
        }

        /// <summary>
        /// 获取仪器组合
        /// </summary>
        /// <param name="getMergeItrCombine">是否获取合并仪器组合</param>
        /// <returns></returns>
        public List<EntityDicItrCombine> GetItrCombine(string itr_id, bool getMergeItrCombine)
        {
            List<EntityDicItrCombine> ret = new List<EntityDicItrCombine>();

            string sqlItrCombineIn = string.Format(" '{0}' ", itr_id);

            if (getMergeItrCombine)
            {
                //查找所有"存储仪器"为当前仪器的仪器
                List<EntityDicInstrument> drsConItr = this.DclDictItr.Where(i => i.ItrReportItrId == itr_id).ToList();

                if (drsConItr.Count > 0)
                {
                    foreach (EntityDicInstrument drItr in drsConItr)
                    {
                        string itrid = drItr.ItrId.ToString();
                        sqlItrCombineIn += string.Format(",'{0}'", itrid);
                    }
                }
            }
            List<EntityDicItrCombine> drs = DictItrCom.Where(i => sqlItrCombineIn.Contains(i.ItrId)).ToList();

            foreach (EntityDicItrCombine dr in drs)
            {
                if (!IsEmpty(dr.ComId))
                {
                    ret.Add(dr);// listCom.Add(dr["com_id"].ToString());
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取仪器所属物理组ID
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        public string GetItrCTypeID(string itr_id)
        {
            string ctype_id = string.Empty;

            EntityDicInstrument instr = GetDclItr(itr_id);
            if (instr != null)
            {
                ctype_id = instr.ItrLabId;
            }

            return ctype_id;
        }


        /// <summary>
        /// DBNull或Null或""时为真
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool IsEmpty(object obj)
        {
            return IsNullOrDBNull(obj) || obj.ToString() == "";
        }


        public bool IsNullOrDBNull(object obj)
        {
            bool b = false;
            if (obj == null || obj == DBNull.Value)
            {
                b = true;
            }
            return b;
        }
    }
}
