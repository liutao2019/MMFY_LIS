using dcl.entity;


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 危急值消息:接口
    /// </summary>
    [ServiceContract]
    public interface IDicObrMessage
    {
        /// <summary>
        /// 根据多个科室代码获取科室消息
        /// </summary>
        /// <param name="dept_codes"></param>
        /// <returns></returns>
        [OperationContract]
        ObrMessageReceiveCollection GetMessageByDepts(string dept_codes);

        /// <summary>
        /// 刷新缓存
        /// </summary>
        [OperationContract]
        void RefreshDeptMessage();

        /// <summary>
        /// 根据科室代码获取科室消息
        /// </summary>
        /// <param name="dept_codes"></param>
        /// <returns></returns>
        [OperationContract]
        ObrMessageReceiveCollection GetDeptMessageByDeptCode(string dept_code);

        /// <summary>
        /// 删除消息-并保存危急值编辑框内容
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="messageID">消息ID</param>
        /// <param name="bPhiDelete">是否为物理删除：数据库中删除/置删除标志</param>
        /// <param name="pat_id">病人表主索引ID</param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteMessageByIDPatId(EntityAuditInfo objAuditInfo, string messageID, bool bPhiDelete, string pat_id);

        /// <summary>
        /// 删除消息
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="messageID"></param>
        /// <param name="bPhiDelete"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteMessageByID(EntityAuditInfo objAuditInfo, string messageID, bool bPhiDelete);

        /// <summary>
        /// 删除信息根据信息ID，同时更新病人表危急值查看标志
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="messageID"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteMessageByIDAndUpdateCriticalChecker(EntityAuditInfo objAuditInfo, string messageID, string pat_id);

        /// <summary>
        /// 根据参数对象从配置文件中获取值
        /// </summary>
        /// <param name="confiigcode"></param>
        /// <returns></returns>
        [OperationContract]
        string GetConfigValue(string confiigcode);

        /// <summary>
        /// 沙井HIS账号验证(涉及Oracle数据库连接，保留之前代码)
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        [OperationContract]
        DataSet SJHisCheckPassWord(DataSet ds);

        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntitySysUser> LisCheckPassWord(EntitySysUser user);

        /// <summary>
        /// 获取科室信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicPubDept> GetDeptInfo();
        

        /// <summary>
        /// 根据病人信息来获取危急值处理信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityPidReportMain> GetUrgentflagAndPatlookcodeByPatid(string pat_id);

        /// <summary>
        /// 处理回退标本
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="strOperatorID"></param>
        /// <param name="strOperatorName"></param>
        /// <param name="currentServerTime"></param>
        /// <param name="bc_remark"></param>
        [OperationContract]
        void HandleReturnMessage(string barcode, string strOperatorID, string strOperatorName, string currentServerTime, string bc_remark);

        /// <summary>
        /// 获取仪器质控信息
        /// </summary>
        /// <param name="itr_type"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicQcRuleMes> GetItrQcMessage(string itr_type);
    }
}
