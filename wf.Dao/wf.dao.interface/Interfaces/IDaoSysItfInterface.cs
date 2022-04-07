using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoSysItfInterface
    {
        /// <summary>
        /// 保存接口
        /// </summary>
        /// <param name="inter"></param>
        /// <returns></returns>
        bool SaveSysInterface(EntitySysItfInterface inter);
        /// <summary>
        /// 更新接口
        /// </summary>
        /// <param name="inter"></param>
        /// <returns></returns>
        bool UpdateSysInterface(EntitySysItfInterface inter);
        /// <summary>
        /// 删除接口
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        bool DeleteSysInterface(string inId);
        /// <summary>
        /// 获取接口
        /// </summary>
        /// <returns></returns>
        List<EntitySysItfInterface> GetSysInterface();

        /// <summary>
        /// 根据类型获取接口
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <returns></returns>
        List<EntitySysItfInterface> GetSysInterface(string interfaceType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="interfaceKey"></param>
        /// <returns></returns>
        EntitySysItfInterface CardDataConvert(string cardData, string interfaceKey);

    }
}
