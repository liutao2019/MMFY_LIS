using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 标本信息扩展表：接口
    /// </summary>
    public interface IDaoSampOperateDetail
    {
        /// <summary>
        /// 获取条码流程状态记录(获取标本扩展表数据)
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        List<EntitySampOperateDetail> GetBarCodeExtend(string barCode);

        /// <summary>
        /// 获取标本检测图像
        /// </summary>
        /// <param name="barCode"></param>
        /// <returns></returns>
        List<EntitySampImage> GetSampImage(string barCode);

    }
}
