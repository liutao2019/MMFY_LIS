using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoSysItfContrast
    {
        /// <summary>
        /// 保存接口参数对照
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        bool SaveSysContrast(EntitySysItfContrast con);
        /// <summary>
        /// 更新接口参数对照
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        bool UpdateSysContrast(EntitySysItfContrast con);
        /// <summary>
        /// 删除接口参数对照
        /// </summary>
        /// <param name="conId"></param>
        /// <returns></returns>
        bool DeleteSysContrast(int conId);
        /// <summary>
        /// 获取接口参数对照
        /// </summary>
        /// <param name="interId"></param>
        /// <returns></returns>
        List<EntitySysItfContrast> GetSysContrast(string interId);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="interId"></param>
        /// <returns></returns>
        List<EntitySysItfContrast> GetSysContrast();

    }
}
