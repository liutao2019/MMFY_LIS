using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoOaTableDetail
    {
        /// <summary>
        /// 根据表单代码查找表达内容
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        List<EntityOaTableDetail> GetTabDetailByTabCode(EntityOaTableDetail detail);

        /// <summary>
        /// 新增一条表单内容
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        bool InsertNewTabDetail(EntityOaTableDetail detail);

        /// <summary>
        /// 更新表单内容
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        bool UpdateTabDetail(EntityOaTableDetail detail);

        /// <summary>
        /// 删除一条表单内容
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        bool DeleteTabDetail(EntityOaTableDetail detail);

        /// <summary>
        /// 根据表单类型代码删除一条表单内容
        /// </summary>
        /// <param name="typeid"></param>
        /// <returns></returns>
        bool DeleteTabDetailByTypeCode(string typeCode);
    }
}
