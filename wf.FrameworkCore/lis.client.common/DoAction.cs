using System.Configuration;
using dcl.client.frame;
using System.Data;

namespace dcl.client.common
{
    public class DoAction
    {
        private string wcfAddress = ConfigurationManager.AppSettings["wcfAddr"];
        private string endpointConfigurationName = "svc.basic";

        public DataSet DoNew(string svc, DataSet ds)
        {
            string remoteAddress = wcfAddress + svc;
            return CommonDelegate.getDelegate(endpointConfigurationName, remoteAddress).doNew(ds);
        }

        //public ProxyCommonDic biz
        //{
        //    get
        //    {
        //        if (this._biz == null)
        //        {
        //            ProxyCommonDic proxy = new ProxyCommonDic("svc." + base.Name);
        //            this._biz = proxy;
        //        }
        //        return this._biz;
        //    }
        //}

        //public DataSet DoDel(string svc, DataSet ds)
        //{
        //    string remoteAddress = wcfAddress + svc;
        //    return CommonDelegate.getDelegate(endpointConfigurationName, remoteAddress).doDel(ds);
        //}

        //public DataSet DoOther(string svc, DataSet ds)
        //{
        //    string remoteAddress = wcfAddress + svc;
        //    return CommonDelegate.getDelegate(endpointConfigurationName, remoteAddress).doOther(ds);
        //}

        //public DataSet DoSearch(string svc, DataSet ds)
        //{
        //    string remoteAddress = wcfAddress + svc;
        //    return CommonDelegate.getDelegate(endpointConfigurationName, remoteAddress).doSearch(ds);
        //}

        //public DataSet DoUpdate(string svc, DataSet ds)
        //{
        //    string remoteAddress = wcfAddress + svc;
        //    return CommonDelegate.getDelegate(endpointConfigurationName, remoteAddress).doUpdate(ds);
        //}

    }
}
