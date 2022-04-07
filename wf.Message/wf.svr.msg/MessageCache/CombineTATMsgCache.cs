using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.msg
{
    /// <summary>
    /// 组合TAT数据
    /// </summary>
    public class CombineTATMsgCache
    {
        #region singleton
        private static object objLock = new object();

        private static CombineTATMsgCache _instance = null;

        /// <summary>
        /// 当时是否没在处理
        /// </summary>
        private static bool IsCurrNotDisposal { get; set; }

        public static CombineTATMsgCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new CombineTATMsgCache();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 组合TAT信息缓存
        /// </summary>
        public List<EntityDicMsgTAT> cache = null;

        /// <summary>
        /// 条码组合TAT信息缓存
        /// </summary>
        public List<EntityDicMsgTAT> cacheBc = null;

        /// <summary>
        /// 条码组合检验中TAT信息缓存
        /// </summary>
        public List<EntityDicMsgTAT> cacheBcLab = null;

        /// <summary>
        /// 条码TAT信息缓存(采集到签收)
        /// </summary>
        public List<EntityDicMsgTAT> cacheBcSamplToReceive = null;

        /// <summary>
        /// 仪器危急值数据
        /// </summary>
        private CombineTATMsgCache()
        {
            this.cache = new List<EntityDicMsgTAT>();
            //this.cache.TableName = "ComTATMsgCache";

            this.cacheBc = new List<EntityDicMsgTAT>();

            this.cacheBcLab = new List<EntityDicMsgTAT>();
            //this.cacheBcLab.TableName = "BcComLabTATMsgCache";

            this.cacheBcSamplToReceive = new List<EntityDicMsgTAT>();
            //this.cacheBcSamplToReceive.TableName = "SamplToReceiveTATMsgCache";

            IsCurrNotDisposal = true;
        }
        #endregion

        /// <summary>
        /// 刷新缓存
        /// </summary>
        public void Refresh()
        {
            this.cache = this.GetCombineTATmsgToCache();
            this.cacheBc = this.GetBcComTAtMsgToCache();
            this.cacheBcLab = this.GetBcComLabTAtMsgToCache();
            this.cacheBcSamplToReceive = this.GetBcSamplToReceiveTAtMsgToCache();
        }

        /// <summary>
        /// 获取组合TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        public List<EntityDicMsgTAT> GetCombineTATmsgToCache()
        {
            try
            {
                //总开关--是否启动组合TAT提醒
                bool Message_ShowTATMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Combine_TAT_IsNotify") == "是";

                //服务端web.config也可以控制关闭功能
                string webconfig_Combine_TAT_IsNotify = System.Configuration.ConfigurationManager.AppSettings["Combine_TAT_IsNotify"];
                if (!string.IsNullOrEmpty(webconfig_Combine_TAT_IsNotify)
                    && (webconfig_Combine_TAT_IsNotify.ToUpper() == "N" || webconfig_Combine_TAT_IsNotify.ToUpper() == "NO"))
                {
                    Message_ShowTATMsg = false;
                }

                if (!Message_ShowTATMsg)
                {
                    //当不启动时
                    return null;
                }
            }
            catch
            {
                return null;
            }
            List<EntityDicMsgTAT> listbResult = null;
            try
            {
                bool isOutLink = (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Combine_TAT_IsNotify") == "outlink");
                listbResult = new CombineTATMsgBIZ().GetCombineTATmsgToCacheDao(isOutLink);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("获取组合TAT数据", ex);
            }

            return listbResult;
        }

        /// <summary>
        /// 获取条码组合检验中TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        public List<EntityDicMsgTAT> GetBcComLabTAtMsgToCache()
        {
            try
            {
                //总开关--检验是否启动条码组合TAT提醒
                bool Message_ShowTATMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BCCombine_TAT_IsNotify") == "是";

                //服务端web.config也可以控制关闭功能
                string webconfig_BCCombine_TAT_IsNotify = System.Configuration.ConfigurationManager.AppSettings["BCCombine_TAT_IsNotify"];
                if (!string.IsNullOrEmpty(webconfig_BCCombine_TAT_IsNotify)
                    && (webconfig_BCCombine_TAT_IsNotify.ToUpper() == "N" || webconfig_BCCombine_TAT_IsNotify.ToUpper() == "NO"))
                {
                    Message_ShowTATMsg = false;
                }

                if (!Message_ShowTATMsg)
                {
                    //当不启动时
                    return null;
                }
            }
            catch
            {
                return null;
            }

            List<EntityDicMsgTAT> listBResult = new List<EntityDicMsgTAT>();
            listBResult = new CombineTATMsgBIZ().GetBcComLabTAtMsgToCacheDao();

            return listBResult;
        }

        /// <summary>
        /// 获取条码组合TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        public List<EntityDicMsgTAT> GetBcComTAtMsgToCache()
        {
            try
            {
                //总开关--检验是否启动条码组合TAT提醒
                bool Message_ShowTATMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BCCombine_TAT_IsNotify") == "是";

                //服务端web.config也可以控制关闭功能
                string webconfig_BCCombine_TAT_IsNotify = System.Configuration.ConfigurationManager.AppSettings["BCCombine_TAT_IsNotify"];
                if (!string.IsNullOrEmpty(webconfig_BCCombine_TAT_IsNotify)
                    && (webconfig_BCCombine_TAT_IsNotify.ToUpper() == "N" || webconfig_BCCombine_TAT_IsNotify.ToUpper() == "NO"))
                {
                    Message_ShowTATMsg = false;
                }

                if (!Message_ShowTATMsg)
                {
                    //当不启动时
                    return null;
                }
            }
            catch
            {
                return null;
            }

            List<EntityDicMsgTAT> listbResult = null;
            listbResult = new CombineTATMsgBIZ().GetBcComTAtMsgToCacheDao();

            return listbResult;
        }


        /// <summary>
        /// 获取条码(采集到签收)TAT数据(仅取24小时内)
        /// </summary>
        /// <returns></returns>
        public List<EntityDicMsgTAT> GetBcSamplToReceiveTAtMsgToCache()
        {
            try
            {
                //系统配置：检验是否启动条码[采集_签收]TAT提醒
                bool Message_ShowTATMsg = dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("BCSamplToReceive_TAT_IsNotify") == "是";

                if (!Message_ShowTATMsg)
                {
                    //当不启动时
                    return null;
                }
            }
            catch
            {
                return null;
            }

            List<EntityDicMsgTAT> listBcSamToReceive = null;

            listBcSamToReceive = new CombineTATMsgBIZ().GetBcSamplToReceiveTAtMsgToCacheDao();

            return listBcSamToReceive;
        }

        /// <summary>
        /// 根据筛选条件获取组合TAT数据（未用上）
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<EntityDicMsgTAT> GetComTATMessage(string strWhere)
        {
            //if (strWhere == null || string.IsNullOrEmpty(strWhere))
            //{
            //    return this.cache;
            //}
            //else
            //{
            //    try
            //    {
            //        if (this.cache != null && this.cache.Rows.Count > 0)
            //        {
            //            DataTable dtCope = this.cache.Clone();
            //            DataRow[] drArray = this.cache.Select(strWhere);

            //            foreach (DataRow drItem in drArray)
            //            {
            //                dtCope.Rows.Add(drItem.ItemArray);
            //            }
            //            dtCope.TableName = "ComTATMsgCache";
            //            return dtCope;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Lib.LogManager.Logger.LogException("获取缓存组合TAT数据", ex);
            //    }
            //    return this.cache;
            //}
            return null;
        }

        /// <summary>
        /// 根据筛选条件获取条码组合检验中TAT数据(未用上)
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<EntityDicMsgTAT> GetBcComLabTATMessage(string strWhere)
        {
            //这个方法暂时用不上，注释掉
            //if (strWhere == null || string.IsNullOrEmpty(strWhere))
            //{
            //    return this.cacheBcLab;
            //}
            //else
            //{
            //    try
            //    {
            //        if (this.cacheBcLab != null && this.cacheBcLab.Rows.Count > 0)
            //        {
            //            DataTable dtCope = this.cacheBcLab.Clone();
            //            DataRow[] drArray = this.cacheBcLab.Select(strWhere);

            //            foreach (DataRow drItem in drArray)
            //            {
            //                dtCope.Rows.Add(drItem.ItemArray);
            //            }
            //            dtCope.TableName = "BcComLabTATMsgCache";
            //            return dtCope;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Lib.LogManager.Logger.LogException("获取缓存条码组合检验中TAT数据", ex);
            //    }
            //    return this.cacheBcLab;
            //}
            return null;
        }

        /// <summary>
        /// 根据筛选条件获取条码组合TAT数据(未用上)
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<EntityDicMsgTAT> GetBcComTATMessage(string strWhere)
        {
            //暂时用不上，先屏蔽
            //if (strWhere == null || string.IsNullOrEmpty(strWhere))
            //{
            //    return this.cacheBc;
            //}
            //else
            //{
            //    try
            //    {
            //        if (this.cacheBc != null && this.cacheBc.Rows.Count > 0)
            //        {
            //            DataTable dtCope = this.cacheBc.Clone();
            //            DataRow[] drArray = this.cacheBc.Select(strWhere);

            //            foreach (DataRow drItem in drArray)
            //            {
            //                dtCope.Rows.Add(drItem.ItemArray);
            //            }
            //            dtCope.TableName = "BcComTATMsgCache";
            //            return dtCope;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Lib.LogManager.Logger.LogException("获取缓存条码组合TAT数据", ex);
            //    }
            //    return this.cacheBc;
            //}
            return null;
        }

        /// <summary>
        /// 根据筛选条件获取条码(采集到签收)TAT数据(暂未用上)
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public List<EntityDicMsgTAT> GetBcBcSamplToReceiveTATMessage(string strWhere)
        {
            //if (strWhere == null || string.IsNullOrEmpty(strWhere))
            //{
            //    return this.cacheBcSamplToReceive;
            //}
            //else
            //{
            //    try
            //    {
            //        if (this.cacheBcSamplToReceive != null && this.cacheBcSamplToReceive.Rows.Count > 0)
            //        {
            //            DataTable dtCope = this.cacheBcSamplToReceive.Clone();
            //            DataRow[] drArray = this.cacheBcSamplToReceive.Select(strWhere);

            //            foreach (DataRow drItem in drArray)
            //            {
            //                dtCope.Rows.Add(drItem.ItemArray);
            //            }
            //            dtCope.TableName = "SamplToReceiveTATMsgCache";
            //            return dtCope;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Lib.LogManager.Logger.LogException("获取缓存条码(采集到签收)TAT数据", ex);
            //    }
            //    return this.cacheBcSamplToReceive;
            //}
            return null;
        }

    }
}