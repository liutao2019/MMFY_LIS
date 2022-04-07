using dcl.entity;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IPatEnterNew
    {
        /// <summary>
        /// 追加条码
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="pat_date"></param>
        /// <param name="pat_bar_code"></param>
        /// <param name="barcode_to_append"></param>
        /// <param name="pat_itr_name"></param>
        /// <param name="pat_sid"></param>
        /// <param name="loginId"></param>
        /// <param name="loginName"></param>
        /// <returns></returns>
        [OperationContract]
        bool PatientAdditionalBarcode(string pat_id, DateTime pat_date, string pat_bar_code, string barcode_to_append, string pat_itr_name, string pat_sid, string loginId, string loginName);


        /// <summary>
        /// 获取报告解读
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        [OperationContract]
        EntityPidReportMain GetReportInterpretation(string pat_id);

        /// <summary>
        /// 获取评价信息
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityReportComment> GetReportComment(string pat_id);

        /// <summary>
        /// 保存评价
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OperationContract]
        int SaveReportComment(EntityReportComment model);

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="rcKey"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteReportComment(string rcKey);

        /// <summary>
        /// 获取检验知识库
        /// </summary>
        /// <param name="com_id"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicItmItem> GetItmByComID(string com_id);
    }
}
