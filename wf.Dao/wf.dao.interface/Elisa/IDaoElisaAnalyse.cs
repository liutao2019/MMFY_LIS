using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoElisaAnalyse
    {
        /// <summary>
        /// 获取酶标板孔原始结果
        /// </summary>
        /// <param name="ItrId"></param>
        /// <param name="dtImmDate"></param>
        /// <returns></returns>
        List<EntityObrElisaResult> GetElisaResult(string ItrId, DateTime dtImmDate);
        /// <summary>
        /// 根据仪器ID获取半定量数据
        /// </summary>
        /// <param name="ItrId"></param>
        /// <returns></returns>
        List<EntityDicQcConvert> GetQCConvert(string ItrId);
        /// <summary>
        /// 更新酶标板孔原始结果的原始结果值
        /// </summary>
        /// <param name="ResId"></param>
        /// <param name="ResValue"></param>
        /// <returns></returns>
        bool UpdateResValue(string ResId, string ResValue);
        /// <summary>
        ///  更新酶标板孔原始结果表
        /// </summary>
        /// <param name="QcResult"></param>
        /// <returns></returns>
        bool UpdateElisaResult(EntityObrElisaResult QcResult);
    }
}
