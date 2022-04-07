using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 质控评价:接口
    /// </summary>
    public interface IDaoObrQcAnalysis
    {

        /// <summary>
        /// 查询质控评价
        /// </summary>
        /// <param name="ItrId"></param>
        /// <param name="ItmId"></param>
        /// <param name="qanLevel"></param>
        /// <returns></returns>
        List<EntityObrQcAnalysis> SearchQcAnalysis(EntityObrQcResultQC qc);


        /// <summary>
        /// 新增质控评价
        /// </summary>
        /// <param name="qcAnalysis"></param>
        /// <returns></returns>
        Boolean InsertQcAnalysis(EntityObrQcAnalysis qcAnalysis);


        /// <summary>
        /// 修改质控评价
        /// </summary>
        /// <param name="qcAnalysis"></param>
        /// <returns></returns>
        Boolean UpdateQcAnalysis(EntityObrQcAnalysis qcAnalysis);


        /// <summary>
        /// 删除质控评价
        /// </summary>
        /// <param name="qcAnalysis"></param>
        /// <returns></returns>
        Boolean DeleteQcAnalysis(EntityObrQcAnalysis qcAnalysis);
    }
}
