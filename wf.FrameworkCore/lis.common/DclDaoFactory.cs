using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dcl.common
{
    public static class DclDaoFactory
    {
        public static T DaoHandler<T>()
        {
            T plugin = default(T);
            try
            {
                if (ListType == null || ListType.Count == 0)
                {
                    string pluginGroup = ConfigurationManager.AppSettings["DCL.ExtDataInterface"];

                    string strDBName = string.Empty;

                    if (string.IsNullOrEmpty(pluginGroup))
                        strDBName = "med";
                    else if (pluginGroup.IndexOf("clab") > -1)
                        strDBName = "clab";
                    else if (pluginGroup.IndexOf("wf") > -1)
                        strDBName = "wf";
                    else
                        strDBName = "med";

                    string strClassPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + string.Format(@"\bin\wf.dao.{0}.dll", strDBName);

                    Assembly assembly = Assembly.LoadFile(strClassPath);

                    ListType = assembly.GetTypes().ToList();
                }

                if (ListType.Count > 0)
                {
                    string strFullName = typeof(T).FullName;

                    foreach (Type t in ListType)
                    {
                        var listInterface = t.GetInterfaces();
                        foreach (var ins in listInterface)
                        {
                            if (ins.FullName == strFullName)
                            {
                                plugin = (T)Activator.CreateInstance(t);
                                return plugin;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(typeof(T).Name, ex);
            }

            return plugin;
        }

        private static CompositionContainer container;

        private static CompositionContainer Container
        {
            get
            {
                if (container == null)
                {
                    AggregateCatalog catalog = new AggregateCatalog(new DirectoryCatalog(@"bin\", "dcl.dao.*.dll"));
                    container = new CompositionContainer(catalog);
                }
                return container;
            }
        }

        private static List<Type> ListType
        {
            get; set;
        }

    }
}
