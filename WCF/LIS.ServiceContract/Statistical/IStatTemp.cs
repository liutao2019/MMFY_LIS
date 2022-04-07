using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using System.Data;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IStatTemp
    {
        /// <summary>
        /// 保存报告模板信息
        /// </summary>
        /// <param name="statQC"></param>
        /// <returns></returns>
        [OperationContract]
        bool InsertTpTemplate(List<EntityTpTemplate> ds);

        /// <summary>
        /// 删除报告模板
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteStatTemp(string name, string type);
    }
}
