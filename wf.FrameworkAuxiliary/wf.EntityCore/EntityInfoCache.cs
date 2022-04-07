using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    internal class EntityInfoCache
    {
        private static Object instanceLock = new Object();
        private static EntityInfoCache _instance = null;

        internal static EntityInfoCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new EntityInfoCache();
                        }
                    }
                }
                return _instance;
            }
        }

        private Dictionary<Type, EntityInfo> entityInfoCache = null;
        private EntityInfoCache()
        {
            entityInfoCache = new Dictionary<Type, EntityInfo>();
        }

        internal EntityInfo Get(Type type)
        {
            if (Exist(type))
            {
                return entityInfoCache[type];
            }
            else
            {
                return null;
            }
        }

        internal bool Exist(Type type)
        {
            foreach (var item in entityInfoCache.Keys)
            {
                if (item == type)
                {
                    return true;
                }
            }
            return false;
        }

        internal void Put(Type type, EntityInfo entityInfo)
        {
            //if (!Exist(type))
            //{
            lock (entityInfoCache)
            {
                if (!Exist(type))
                {
                    entityInfoCache.Add(type, entityInfo);
                }
            }
            //}
        }
    }
}
