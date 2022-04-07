using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.ServiceModel;

using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IOfficeHandOver
    {
      
        /// <summary>
        /// 获得交班设定
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicHandOver> GetDictHandoverList();

        /// <summary>
        /// 更新交班设定
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateDictHandoverList(List<EntityDicHandOver> list);

        /// <summary>
        /// 删除交班设定
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteDictHandover(string typeid);

    }
}
