using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Xml;
using System.Configuration;
using Lib.LogManager;
using System.ServiceModel.Description;
using System.ServiceModel.Diagnostics;


namespace dcl.client.wcf
{
    public abstract class ProxyBase<TContract>
    {
        public TContract Service;

        public ProxyBase()
        {
            CreateProxy(ConfigName);
        }

        public ProxyBase(string name)
        {
            CreateProxy(name);
        }

        private void CreateProxy(string configName)
        {
            ChannelFactory<TContract> channelFactory = new ChannelFactory<TContract>(WCFEndpoint.Current.WCFBinding);
            foreach (OperationDescription op in channelFactory.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dataContractBehavior = op.Behaviors.Find<DataContractSerializerOperationBehavior>() as DataContractSerializerOperationBehavior;
                if (dataContractBehavior != null)
                {
                    dataContractBehavior.MaxItemsInObjectGraph = int.MaxValue;
                }
            }
            Service = channelFactory.CreateChannel(WCFEndpoint.Current.WCFEndpointAddress(configName));
        }

        public abstract string ConfigName { get; }

    }

    public class WCFEndpoint
    {
        private static WCFEndpoint _current = null;

        public static WCFEndpoint Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new WCFEndpoint();
                }
                return _current;
            }
        }

        private WCFEndpoint()
        {

            BasicHttpBinding binding = new BasicHttpBinding();
            binding.CloseTimeout = new TimeSpan(0, 5, 0);
            binding.OpenTimeout = new TimeSpan(0, 5, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
            binding.SendTimeout = new TimeSpan(0, 5, 0);

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

            binding.ReaderQuotas.MaxDepth = int.MaxValue;
            binding.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            binding.ReaderQuotas.MaxArrayLength = int.MaxValue;
            binding.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            binding.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;

            _binding = binding;

            InitEndpointAddressConfig();
        }

        Binding _binding = null;

        public Binding WCFBinding
        {
            get
            {
                return _binding;
            }
        }

        public void InitEndpointAddressConfig()
        {
            wcfAddr = ConfigurationManager.AppSettings["wcfAddr"];
        }

        private string wcfAddr;

        public EndpointAddress WCFEndpointAddress(string confName)
        {
            try
            {
                EndpointAddress addr = new EndpointAddress(wcfAddr + ServiceMap.AppSettings[confName]);
                return addr;
            }
            catch (Exception ex)
            {
                Logger.LogInfo("confName=" + confName + "\r\n" + ex.ToString());
                throw;
            }

        }
    }
}
