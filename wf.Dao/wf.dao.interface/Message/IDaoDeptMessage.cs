using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 危急值消息:接口
    /// </summary>
    public interface IDaoDeptMessage
    {
        /// <summary>
        /// 删除消息-并保存危急值编辑框内容
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="messageID">消息ID</param>
        /// <param name="bPhiDelete">是否为物理删除：数据库中删除/置删除标志</param>
        /// <param name="pat_id">病人表主索引ID</param>
        /// <returns></returns>
        bool DeleteMessageByIDPatId(EntityAuditInfo objAuditInfo, string messageID, bool bPhiDelete, string pat_id);

        /// <summary>
        /// 删除消息 
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="messageID"></param>
        /// <param name="bPhiDelete"></param>
        /// <returns></returns>
        bool DeleteMessageByID(EntityAuditInfo objAuditInfo, string messageID, bool bPhiDelete);

        /// <summary>
        /// 删除信息根据信息ID，同时更新病人表危急值查看标志
        /// </summary>
        /// <param name="objAuditInfo"></param>
        /// <param name="messageID"></param>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        bool DeleteMessageByIDAndUpdateCriticalChecker(EntityAuditInfo objAuditInfo, string messageID, string pat_id);
        
        /// <summary>
        /// 处理回退标本
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="strOperatorID"></param>
        /// <param name="strOperatorName"></param>
        /// <param name="currentServerTime"></param>
        /// <param name="bc_remark"></param>
        void HandleReturnMessage(string barcode, string strOperatorID, string strOperatorName, string currentServerTime, string bc_remark);

    }
}
