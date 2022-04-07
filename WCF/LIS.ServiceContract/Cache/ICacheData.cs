using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ICacheData
    {
        [OperationContract]
        EntityResponse GetCacheData(String cacheName);
    }
}
