using dcl.client.wcf;
using dcl.servececontract;

namespace dcl.client.frame
{
    public class ProxyCommonDic : ProxyBase<IDicCommon>
    {
        public ProxyCommonDic(string name)
            : base(name)
        {

        }

        public override string ConfigName
        {
            get { return "svc.basic"; }
        }

        public dcl.entity.EntityResponse New(dcl.entity.EntityRequest request)
        {
            return base.Service.New(request);
        }

        public dcl.entity.EntityResponse Delete(dcl.entity.EntityRequest request)
        {
            return base.Service.Delete(request);
        }

        public dcl.entity.EntityResponse Update(dcl.entity.EntityRequest request)
        {
            return base.Service.Update(request);
        }

        public dcl.entity.EntityResponse Search(dcl.entity.EntityRequest request)
        {
            return base.Service.Search(request);
        }

        public dcl.entity.EntityResponse Other(dcl.entity.EntityRequest request)
        {
            return base.Service.Other(request);
        }

        public dcl.entity.EntityResponse View(dcl.entity.EntityRequest request)
        {
            return base.Service.View(request);
        }
    }
}

