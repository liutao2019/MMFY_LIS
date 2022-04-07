using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using dcl.entity;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IReaSetting
    {
        /// <summary>
        /// 查询试剂物数据(所有)
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityReaSetting> SearchReaSettingAll();

        /// <summary>
        /// 保存试剂库数据
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse SaveReaSetting(EntityReaSetting reaSetting);

        /// <summary>
        /// 更新试剂库数据
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateReaSetting(EntityReaSetting reaSetting);

        /// <summary>
        /// 删除试剂物明细数据
        /// </summary>
        /// <param name="qcMateria"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteReaSetting(EntityReaSetting reaSetting);
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="module"></param>
        /// <param name="loginID"></param>
        /// <param name="ip"></param>
        /// <param name="mac"></param>
        /// <param name="message"></param>
        [OperationContract]
        void LogLogin(string type, string module, string loginID, string ip, string mac, string message);
        [OperationContract]
        string GetReaBarcodeByID(string reaid);
        [OperationContract]
        bool UpdateBarcode(EntityReaSetting reaSetting);
    }
}
