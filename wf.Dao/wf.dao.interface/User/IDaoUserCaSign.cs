using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;
using System.Collections;

namespace dcl.dao.interfaces
{
    public interface IDaoUserCaSign
    {
        /// <summary>
        /// 插入CA认证使用表
        /// </summary>
        /// <param name="CaSign"></param>
        /// <returns></returns>
        bool InsertCaSign(List<EntityCaSign> CaSign);
        /// <summary>
        /// 获取Ca密钥使用情况
        /// </summary>
        /// <param name="CerId"></param>
        /// <param name="EntityId"></param>
        /// <returns></returns>
        List<EntityCaSign> GetCaSign(string CerId, string EntityId);
    }
}
