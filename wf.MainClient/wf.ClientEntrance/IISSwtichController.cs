using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using System.Text;
using dcl.servececontract;
using dcl.client.wcf;
using dcl.client.common;

namespace wf.ClientEntrance
{
    public enum ClientType
    {
        LisClient,
        BarCodeClient
    }
    public class IISSwtichController
    {
        public bool IsIISAvailable(string wcfAddr)
        {
            try
            {
                //IAnnouncement service = ChannelFactory<IAnnouncement>.CreateChannel(WCFBinding(), WCFEndpointAddress(wcfAddr));
                //return service.IsIISAvailable() == "HelloWorlds";

                return false;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool IsIISAvailableForNormal()
        {
            try
            {
                //using (ProxyAnnouncement proxy = new ProxyAnnouncement())
                //{
                //    return proxy.Service.IsIISAvailable() == "HelloWorlds";
                //}
                return false;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        private BasicHttpBinding WCFBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.CloseTimeout = new TimeSpan(0, 1, 0);
            binding.OpenTimeout = new TimeSpan(0, 1, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 1, 0);
            binding.SendTimeout = new TimeSpan(0, 1, 0);

            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;

            binding.MaxBufferPoolSize = int.MaxValue;
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.MessageEncoding = WSMessageEncoding.Text;
            binding.TextEncoding = Encoding.UTF8;

            binding.TransferMode = TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;
            return binding;
        }

        public EndpointAddress WCFEndpointAddress(string wcfAddr)
        {
            try
            {
                EndpointAddress addr = new EndpointAddress(wcfAddr + ConfigurationManager.AppSettings["svc.ProxyAnnouncement"]);
                return addr;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                throw;
            }

        }

        string GetConfigPath(ClientType clientType)
        {
            return AppDomain.CurrentDomain.SetupInformation.ApplicationBase +
                                (clientType == ClientType.LisClient ? "lis.client.exe" : "dcl.client.sampleclient.exe");
        }

        public List<string> GetWcfAddrList(ClientType clientType)
        {
            List<string> list = new List<string>();
            string configPath = GetConfigPath(clientType);
            for (int i = 1; i <= 10; i++)
            {
                string addr = ConfigHelper.GetSetting("wcfAddr_" + i, configPath);

                if (!string.IsNullOrEmpty(addr) && !list.Contains(addr))
                {
                    list.Add(addr);
                }
            }
            return list;
        }


        public bool SwtichAddr(ClientType clientType)
        {
            if (IsIISAvailableForNormal())
                return true;

            List<string> list = GetWcfAddrList(clientType);

            if (list.Count == 0)
            {
                Lib.LogManager.Logger.LogException(new Exception("配置文件无中间层服务地址列表节点（wcfAddr_1，wcfAddr_2）"));
                return false;
            }
            list.Remove(ConfigurationManager.AppSettings["wcfAddr"]);

            string configPath = GetConfigPath(clientType);

            foreach (string wcfAddr in list)
            {
                Lib.LogManager.Logger.LogException(new Exception(wcfAddr));

                if (IsIISAvailable(wcfAddr))
                {
                    ConfigHelper.UpdateSeting(configPath + ".config", "wcfAddr", wcfAddr);
                    return true;
                }
            }
            return false;
        }


    }
}
