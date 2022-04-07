using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using System.Data;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface ILisDoc
    {
        [OperationContract]
        int Delete(EntityLisDoc lisDoc);

        [OperationContract]
        List<EntityLisDoc> QueryAll();

        /// <summary>
        /// 按日期和类型查找数据文档
        /// </summary>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <param name="docType"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityLisDoc> Query(DateTime beginTime, DateTime endTime, string docType);

        [OperationContract]
        int Save(EntityLisDoc lisDoc);
    }
}
