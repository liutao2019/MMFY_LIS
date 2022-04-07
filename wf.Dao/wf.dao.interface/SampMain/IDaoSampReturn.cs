using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoSampReturn
    {
        /// <summary>
        /// 处理回退条码
        /// </summary>
        /// <param name="sampBarId"></param>
        /// <returns></returns>
        Boolean HandleSampReturn(String sampBarId);

        /// <summary>
        /// 新增回退信息
        /// </summary>
        /// <param name="sampReturn"></param>
        /// <returns></returns>
        Boolean SaveSampReturn(EntitySampReturn sampReturn);

        /// <summary>
        /// 查询回退信息
        /// </summary>
        /// <param name="sampQc"></param>
        /// <returns></returns>

        List<EntitySampReturn> GetSampReturn(EntitySampQC sampQc);


        /// <summary>
        /// 更新回退条码的信息
        /// </summary>
        /// <param name="sampReturn"></param>
        /// <returns></returns>
        bool UpdateReturnMessage(EntitySampReturn sampReturn);
    }
}
