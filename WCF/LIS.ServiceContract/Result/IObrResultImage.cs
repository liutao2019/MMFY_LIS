using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    /// <summary>
    /// 图像报告表:接口
    /// </summary>
    [ServiceContract]
    public interface IObrResultImage
    {
        /// <summary>
        /// 获取病人图像结果（查询图像报告表数据）
        /// </summary>
        /// <param name="pat_id"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityObrResultImage> GetObrResultImage(string pat_id);

        /// <summary>
        /// 删除图像结果
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="pat_id"></param>
        /// <param name="itm_ecd"></param>
        /// <param name="pres_key"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse DeletePatPhotoResult(EntitySysOperationLog userInfo, string pat_id, string itm_ecd, long pres_key);

        /// <summary>
        /// 保存图像结果
        /// </summary>
        /// <param name="eyObrResImg"></param>
        /// <returns></returns>
        [OperationContract]
        int SaveObrResultImage(EntityObrResultImage eyObrResImg);

        /// <summary>
        /// 根据ObrId删除图像结果
        /// </summary>
        /// <param name="obrId"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeletePatPhotoResultByObrId(string obrId);
    }
}
