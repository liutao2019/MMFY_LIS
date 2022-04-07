namespace dcl.client.frame
{
    using System;
    using System.CodeDom.Compiler;
    using System.Data;
    using System.Diagnostics;
    using System.ServiceModel;

    public class CommonDelegate : ClientBase<ICommonBIZ>, ICommonBIZ
    {
        public CommonDelegate(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public DataSet doDel(DataSet ds)
        {
            return base.Channel.doDel(ds);
        }

        public DataSet doNew(DataSet ds)
        {
            return base.Channel.doNew(ds);
        }

        public DataSet doOther(DataSet ds)
        {
            return base.Channel.doOther(ds);
        }

        public DataSet doSearch(DataSet ds)
        {
            return base.Channel.doSearch(ds);
        }

        public DataSet doUpdate(DataSet ds)
        {
            return base.Channel.doUpdate(ds);
        }

        public DataSet doView(DataSet ds)
        {
            return base.Channel.doView(ds);
        }

        public static CommonDelegate getDelegate(string endpointConfigurationName, string remoteAddress)
        {
            return new CommonDelegate(endpointConfigurationName, remoteAddress);
        }

        
    }
}

