using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoPatientRecheck
    {
        /// <summary>
        /// 插入病人复查信息
        /// </summary>
        /// <param name="recheck"></param>
        /// <returns></returns>
        bool InsertPatientRecheck(EntityPatientRecheck recheck);

        /// <summary>
        /// 删除病人复查信息
        /// </summary>
        /// <param name="recheck"></param>
        /// <returns></returns>
        bool DeletePatientRecheck(EntityPatientRecheck recheck);
    }
}
