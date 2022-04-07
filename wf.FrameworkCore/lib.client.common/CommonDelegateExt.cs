using System.ServiceModel;
using System.Diagnostics;
using System.Data;
using System.CodeDom.Compiler;

namespace dcl.client.frame
{
    [DebuggerStepThrough, GeneratedCode("System.ServiceModel", "3.0.0.0")]
    public class CommonDelegateExt : ClientBase<ICommonBIZExt>, ICommonBIZExt
	{
        public CommonDelegateExt(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
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

        public DataSet Search(string where)
        {
            return base.Channel.Search(where);
        }
        
        public int Update(string set,string where)
        {
            return base.Channel.Update(set,where);
        }

        public int Insert(string columns, string values)
        {
            return base.Channel.Insert(columns, values);
        }

        public static CommonDelegateExt getDelegate(string endpointConfigurationName, string remoteAddress)
        {
            return new CommonDelegateExt(endpointConfigurationName, remoteAddress);
        }
	}
}
