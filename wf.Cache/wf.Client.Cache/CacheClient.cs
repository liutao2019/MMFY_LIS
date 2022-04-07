using dcl.client.wcf;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace dcl.client.cache
{
    public class CacheClient
    {
         static Dictionary<String, Object> cache = new Dictionary<string, object>();

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetCache<T>()
        {
            return GetCache<T>(typeof(T).Name);
        }

        /// <summary>
        /// 获取缓存集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        private static List<T> GetCache<T>(String cacheName)
        {
            List<T> list = new List<T>();
            try
            {
                if (cache.ContainsKey(cacheName))
                {
                    list = (List<T>)cache[cacheName];
                }
                else
                {
                    object cacheValue = GetCache(cacheName);
                    if (cacheValue != null)
                    {
                        list = (List<T>)cacheValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return list;
        }

        /// <summary>
        /// 获取缓存数据
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public static Object GetCache(String cacheName)
        {
            if (cache.ContainsKey(cacheName))
            {
                return cache[cacheName];
            }
            else
            {
                try
                {
                    EntityResponse respone = new ProxyCacheData().Service.GetCacheData(cacheName);
                    object result = respone.GetResult();
                    if (result != null)
                    {
                        if (!cache.ContainsKey(cacheName))
                        {
                            cache.Add(cacheName, result);
                        }
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }

            }

            return null;
        }

        /// <summary>
        /// 获取缓存副本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public static List<T> GetCacheCopy<T>()
        {
            return Clone<T>(GetCache<T>(typeof(T).Name));
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public static void ClearCache()
        {
            if (cache != null && cache.Count > 0)
                cache.Clear();
        }

        /// <summary>
        /// 克隆集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="List"></param>
        /// <returns></returns>
        private static List<T> Clone<T>(object List)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }

        /// <summary>
        /// 是否开启检验功能,默认开启 
        /// 配置请在 Web.config 添加SystemType 节点
        /// </summary>
        /// <returns></returns>
        public static bool EnableLisFunc()
        {
            var list = GetCache<EntityServerSetting>();

            if (list.Count > 0 && list[0].SystemType == "MIC")
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 是否启用微生物系统相关功能
        /// 配置请在 Web.config 添加SystemType 节点
        /// </summary>
        /// <returns></returns>
        public static bool EnableMicFunc()
        {
            var list = GetCache<EntityServerSetting>();

            if (list.Count > 0)
            {
                return list[0].SystemType == "MIC" || list[0].SystemType == "ALL";
            }
            return false;
        }
    }
}
