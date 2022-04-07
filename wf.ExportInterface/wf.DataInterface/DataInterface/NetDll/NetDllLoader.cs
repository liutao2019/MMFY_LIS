using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace Lib.DataInterface
{
    public class NetDllLoader : IDisposable
    {
        string _basePath = null;
        private string BasePath
        {
            get
            {
                if (_basePath == null)
                {
                    _basePath = FilePathHelper.GetBasePath();
                }
                return _basePath;
            }
        }

        NetDllInvokeProxy invoker = null;
        AppDomain appDomain = null;

        public NetDllLoader(string dllName)
        {
            Uri destUri;
            Uri baseUri = new Uri(this.BasePath);
            Uri.TryCreate(baseUri, dllName, out destUri);
            string fullPath = destUri.LocalPath;

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("找不到指定的文件:" + fullPath);

            #region MyRegion


            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationName = new FileInfo(fullPath).Name.Split('.')[0];
            setup.ApplicationBase = BasePath;
            //setup.PrivateBinPath = AppDomain.CurrentDomain.BaseDirectory;
            //setup.CachePath = setup.ApplicationBase;
            //setup.ShadowCopyFiles = "true";
            //setup.ShadowCopyDirectories = setup.ApplicationBase;
            System.Security.Policy.Evidence adevidence = AppDomain.CurrentDomain.Evidence;

            BindingFlags bindings = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public;

            appDomain = AppDomain.CreateDomain(this.GetType().Name, adevidence, setup);
            String name = Assembly.GetExecutingAssembly().GetName().FullName;


            object[] parms = { fullPath };
            invoker = (NetDllInvokeProxy)appDomain.CreateInstanceAndUnwrap(name,
               typeof(NetDllInvokeProxy).FullName, true, bindings, null, parms, null, null, adevidence);


            #endregion
        }

        public object Invoke(string fullClassName, string methodName, object[] inputArgs, out object[] outputArgs)
        {
            object ret = invoker.Invoke(fullClassName, methodName, inputArgs, out  outputArgs);
            return ret;
        }

        public Type GetInvokeType(string fullClassName)
        {
            return invoker.GetInvokeType(fullClassName);
        }

        public void Unload()
        {
            AppDomain.Unload(appDomain);
            if (this.invoker != null)
                this.invoker = null;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            this.Unload();
        }

        #endregion
    }

    [Serializable]
    public class NetDllInvokeProxy : MarshalByRefObject
    {
        public NetDllInvokeProxy(string dllPath)
        {
            _assembly = Assembly.LoadFile(dllPath);
        }

        Assembly _assembly = null;

        public Type GetInvokeType(string fullClassName)
        {
            Type tp = _assembly.GetType(fullClassName);
            return tp;
        }

        public object Invoke(string fullClassName, string methodName, object[] args, out object[] outArgs)
        {
            Type tp = GetInvokeType(fullClassName);
            if (tp == null)
                throw new Exception(string.Format("找不到指定的对象[{0}]", fullClassName));

            MethodInfo method = tp.GetMethod(methodName);
            if (method == null)
                throw new Exception(string.Format("找不到指定的方法[{0}]", methodName));

            object instance = Activator.CreateInstance(tp);
            object ret = method.Invoke(instance, args);

            List<object> output = new List<object>(args);
            outArgs = output.ToArray();
            return ret;
        }
    }
}
