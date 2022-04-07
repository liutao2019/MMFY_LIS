using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 图像报告表:接口
    /// </summary>
    public interface IDaoObrResultImage : IDaoBase
    {
        /// <summary>
        /// 获取病人图像结果（查询图像报告表数据）
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        List<EntityObrResultImage> GetObrResultImage(string pat_id);

        /// <summary>
        /// 删除图像结果
        /// </summary>
        /// <param name="pres_key"></param>
        /// <returns></returns>
        bool DeletePatPhotoResult(long pres_key);

        /// <summary>
        /// 根据ObrId删除图像结果
        /// </summary>
        /// <param name="pres_key"></param>
        /// <returns></returns>
        bool DeleteObrResultImageByObrId(string obrId);

        /// <summary>
        /// 保存图像结果
        /// </summary>
        /// <param name="eyObrResImg"></param>
        /// <returns></returns>
        int SaveObrResultImage(EntityObrResultImage eyObrResImg);
    }
}
