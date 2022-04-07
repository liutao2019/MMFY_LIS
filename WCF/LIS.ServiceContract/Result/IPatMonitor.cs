using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IPatMonitor
    {
        /// <summary>
        /// 获取标本进程信息
        /// </summary>
        /// <param name="type_id"></param>
        /// <param name="itr_id"></param>
        /// <param name="patDate"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse GetPatMonitor(string type_id, string itr_id, DateTime patDate);
    }
}
