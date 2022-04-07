using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 质控评价:接口
    /// </summary>
    [ServiceContract]
    public interface IDicObrQcAnalysis
    {

        /// <summary>
        /// 查询质控评价
        /// </summary>
        /// <param name="ItrId"></param>
        /// <param name="ItmId"></param>
        /// <param name="qanLevel"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrQcAnalysis> SearchQcAnalysis(EntityObrQcResultQC qc);


        /// <summary>
        /// 新增质控评价
        /// </summary>
        /// <param name="qcAnalysis"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean InsertQcAnalysis(EntityObrQcAnalysis qcAnalysis);


        /// <summary>
        /// 修改质控评价
        /// </summary>
        /// <param name="qcAnalysis"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean UpdateQcAnalysis(EntityObrQcAnalysis qcAnalysis);


        /// <summary>
        /// 删除质控评价
        /// </summary>
        /// <param name="qcAnalysis"></param>
        /// <returns></returns>
        [OperationContract]
        Boolean DeleteQcAnalysis(EntityObrQcAnalysis qcAnalysis);
    }
}
