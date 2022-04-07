using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;

namespace dcl.dao.interfaces
{
    public interface IDaoRuntimeSetting
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="eySysInfeLog"></param>
        /// <returns></returns>
        void Save(string key, byte[] data);

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="eySysInfeLog"></param>
        /// <returns></returns>
        byte[] Load(string key);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        void Delete(string key);
    }
}
