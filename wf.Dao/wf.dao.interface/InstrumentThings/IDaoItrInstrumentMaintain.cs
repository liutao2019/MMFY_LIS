using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    public interface IDaoItrInstrumentMaintain
    {
        /// <summary>
        /// 新增保养字典信息
        /// </summary>
        /// <param name="instrmtMaintain"></param>
        /// <returns></returns>
        bool AddInstrmtMaintain(EntityDicItrInstrumentMaintain instrmtMaintain);

        /// <summary>
        /// 修改保养字典信息
        /// </summary>
        /// <param name="instrmtMaintain"></param>
        /// <returns></returns>
        bool UpdateInstrmtMaintain(EntityDicItrInstrumentMaintain instrmtMaintain);

        /// <summary>
        /// 删除保养字典信息
        /// </summary>
        /// <param name="mai_id"></param>
        /// <returns></returns>
        bool DeleteInstrmtMaintainByID(int mai_id);

        /// <summary>
        /// 查询保养字典信息
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        List<EntityDicItrInstrumentMaintain> GetInstrmtMaintains(string itr_id);
    }
}
