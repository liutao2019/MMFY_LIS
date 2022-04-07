using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.entity;
using System.Data;
using System.Collections;

namespace dcl.dao.interfaces
{
    public interface IDaoSaveStatTemp
    {
        /// <summary>
        /// 根据模板名称和模板类型删除报告模板
        /// </summary>
        /// <param name="StaName"></param>
        /// <param name="StaType"></param>
        /// <returns></returns>
        bool DeleteStatTemp(string StaName,string StaType);

        /// <summary>
        /// 新增报告模板信息
        /// </summary>
        /// <param name="TpTemplate"></param>
        /// <returns></returns>
        bool InsertStatTemp(EntityTpTemplate TpTemplate);
    }
}
