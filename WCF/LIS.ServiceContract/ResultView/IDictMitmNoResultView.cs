
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace dcl.servececontract
{
    [ServiceContract]
    public interface IDictMitmNoResultView
    {
        /// <summary>
        /// 查询仪器结果数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        /// <param name="result_type"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [OperationContract]
        List<EntityDicObrResultOriginal> GetInstructmentResult2(DateTime date, string itr_id, int result_type, string filter);

        /// <summary>
        /// 查询项目字典数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse SearchItem(EntityRequest request);

        /// <summary>
        /// 插入或更新仪器通道字典数据
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        [OperationContract]
        EntityResponse SaveOrUpdateMitmNo(List<EntityDicMachineCode> ds);

        /// <summary>
        /// 获取所用压缩后字节流
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        byte[] GetAllBuffer(DateTime date, string itr_id, int result_type, string filter);



    }
}
