using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IDicItrInstrumentMaintain 
    {
        /// <summary>
        /// 新增保养字典信息
        /// </summary>
        /// <param name="instrmtMaintain"></param>
        /// <returns></returns>
        [OperationContract]
        bool AddInstrmtMaintain(EntityDicItrInstrumentMaintain instrmtMaintain);

        /// <summary>
        /// 修改保养字典信息
        /// </summary>
        /// <param name="instrmtMaintain"></param>
        /// <returns></returns>
        [OperationContract]
        bool UpdateInstrmtMaintain(EntityDicItrInstrumentMaintain instrmtMaintain);

        /// <summary>
        /// 删除保养字典信息
        /// </summary>
        /// <param name="mai_id"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteInstrmtMaintainByID(int mai_id);

        /// <summary>
        /// 查询保养字典信息
        /// </summary>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicItrInstrumentMaintain> GetInstrmtMaintains(string itr_id);

        /// <summary>
        /// 检索组别字典信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicPubProfession> SearchDicPubProfession();

        /// <summary>
        /// 查询组别字典信息数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        [OperationContract]
       List<EntityDicInstrument> GetInstrmts(string strWhere);


    }
}
