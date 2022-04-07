using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace dcl.client.frame.runsetting
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "IRunTimeSettingService")]
    public interface IRunTimeSettingService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IRunTimeSettingService/Save", ReplyAction = "http://tempuri.org/IRunTimeSettingService/SaveResponse")]
        void Save(string key, byte[] data);

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IRunTimeSettingService/Load", ReplyAction = "http://tempuri.org/IRunTimeSettingService/LoadResponse")]
        byte[] Load(string key);

     
        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IRunTimeSettingService/Delete", ReplyAction = "http://tempuri.org/IRunTimeSettingService/LoadResponse")]
        void Delete(string key);
    }

    public class RuntimeSettingClient : System.ServiceModel.ClientBase<IRunTimeSettingService>, IRunTimeSettingService
    {
        public RuntimeSettingClient(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public void Save(string key, byte[] data)
        {
            base.Channel.Save(key, data);
        }

        public byte[] Load(string key)
        {
            return base.Channel.Load(key);
        }


        public void Delete(string key)
        {
            base.Channel.Delete(key);
        }
    }
}
