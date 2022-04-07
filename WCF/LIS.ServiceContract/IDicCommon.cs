using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 字典操作接口
    /// </summary>
    [ServiceContract]
    public interface IDicCommon
    {
        [OperationContract]
        EntityResponse New(EntityRequest request);

        [OperationContract]
        EntityResponse Delete(EntityRequest request);

        [OperationContract]
        EntityResponse Update(EntityRequest request);

        [OperationContract]
        EntityResponse Search(EntityRequest request);

        [OperationContract]
        EntityResponse Other(EntityRequest request);

        [OperationContract]
        EntityResponse View(EntityRequest request);
    }
}
