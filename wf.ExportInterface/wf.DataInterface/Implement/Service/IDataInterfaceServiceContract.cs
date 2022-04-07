using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Lib.DataInterface.Implement
{
    [ServiceContract]
    public interface IDataInterfaceServiceContract
    {
        #region Connection

        /// <summary>
        /// 获取连接信息
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataInterfaceConnection[] GetConnections(string module);

        /// <summary>
        /// 根据id获取连接信息
        /// </summary>
        /// <param name="conn_id"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataInterfaceConnection GetConnectionByID(string conn_id);

        [OperationContract]
        string TestConnection(EntityDictDataInterfaceConnection obj);

        /// <summary>
        /// 删除连接信息
        /// </summary>
        /// <param name="obj"></param>
        [OperationContract]
        void ConnectionDelete(EntityDictDataInterfaceConnection obj);

        /// <summary>
        /// 保存连接信息
        /// </summary>
        /// <param name="obj"></param>
        [OperationContract]
        void ConnectionSave(EntityDictDataInterfaceConnection obj);
        #endregion

        #region Command
        /// <summary>
        /// 获取执行指令信息
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataInterfaceCommand[] GetCommands(string module);

        /// <summary>
        /// 根据id获取执行命令信息
        /// </summary>
        /// <param name="cmd_id"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataInterfaceCommand GetCommandByID(string cmd_id);

        /// <summary>
        /// 根据分组获取执行命令信息
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataInterfaceCommand[] GetCommandByGroup(string groupName);

        /// <summary>
        /// 根据命令id获取执行命令参数
        /// </summary>
        /// <param name="cmd_id"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataInterfaceCommandParameter[] GetParametersByCmdID(string cmd_id);

        /// <summary>
        /// 获取执行命令参数
        /// </summary>
        /// <param name="cmd_id"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataInterfaceCommandParameter[] GetParameters();

        /// <summary>
        /// 删除命令信息
        /// </summary>
        /// <param name="obj"></param>
        [OperationContract]
        void CommandDelete(EntityDictDataInterfaceCommand obj);

        /// <summary>
        /// 保存命令信息
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="listParams"></param>
        [OperationContract]
        void CommandSave(EntityDictDataInterfaceCommand cmd, EntityDictDataInterfaceCommandParameter[] listParams);
        #endregion

        #region Converter
        /// <summary>
        /// 获取数据转换信息
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataConverter[] GetConverters(string module);

        /// <summary>
        /// 根据id获取数据装换信息
        /// </summary>
        /// <param name="rule_id"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataConverter GetConverterByID(string rule_id);

        /// <summary>
        /// 根据转换规则ID获取对照转换信息
        /// </summary>
        /// <param name="rule_id"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataConvertContrast[] GetConverterContrastsByConverterID(string rule_id);

        /// <summary>
        /// 获取对照转换信息
        /// </summary>
        /// <param name="rule_id"></param>
        /// <returns></returns>
        [OperationContract]
        EntityDictDataConvertContrast[] GetConverterContrasts();

        /// <summary>
        /// 删除转换规则
        /// </summary>
        /// <param name="converter"></param>
        [OperationContract]
        void ConverterDelete(EntityDictDataConverter converter);

        /// <summary>
        /// 保存转换规则
        /// </summary>
        /// <param name="converter"></param>
        /// <param name="contrasts"></param>
        [OperationContract]
        void ConverterSave(EntityDictDataConverter converter, EntityDictDataConvertContrast[] contrasts);
        #endregion

        [OperationContract]
        object ExecuteScalar(string cmd_id, InterfaceDataBindingItem[] dataBindings);
    }
}
