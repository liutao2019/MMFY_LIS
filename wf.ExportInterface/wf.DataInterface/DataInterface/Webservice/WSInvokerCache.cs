using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DataInterface
{
    class WSInvokerCache
    {
        static WSInvokerCache _instance = null;
        static object padlock = new object();

        public static WSInvokerCache Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                        {
                            _instance = new WSInvokerCache();
                        }
                    }
                }
                return _instance;
            }
        }

        WSInvokerCache()
        {
            this._cacheInvokeInstance = new Dictionary<string, object>();
            this._cacheInvokeType = new Dictionary<string, Type>();
        }

        Dictionary<string, object> _cacheInvokeInstance = null;
        Dictionary<string, Type> _cacheInvokeType = null;

        public object GetInvokeInstance(string wsdlAddress)
        {
            object obj = null;
            if (_cacheInvokeInstance.TryGetValue(wsdlAddress, out obj))
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        public Type GetInvokeType(string wsdlAddress)
        {
            Type tp = null;
            if (_cacheInvokeType.TryGetValue(wsdlAddress, out tp))
            {
                return tp;
            }
            else
            {
                return null;
            }
        }

        public void Put(string wsdlAddress, Type invokeType, object invokeInstance)
        {
            if (!this._cacheInvokeInstance.ContainsKey(wsdlAddress))
            {
                this._cacheInvokeInstance.Add(wsdlAddress, invokeInstance);
                this._cacheInvokeType.Add(wsdlAddress, invokeType);
            }
            else
            {
                this._cacheInvokeInstance[wsdlAddress] = invokeInstance;
                this._cacheInvokeType[wsdlAddress] = invokeType;
            }
        }
    }
}