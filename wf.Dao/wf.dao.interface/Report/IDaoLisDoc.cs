using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoLisDoc
    {
        /// <summary>
        /// 保存文档
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        int Save(EntityLisDoc doc);

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        int Update(EntityLisDoc dco);

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        int Delete(EntityLisDoc doc);

        /// <summary>
        /// 查找文档
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        List<EntityLisDoc> QueryAll();


        /// <summary>
        /// 按日期和类型查找文档
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        List<EntityLisDoc> Query(DateTime beginTime, DateTime endTime, string docType);

    }
}
