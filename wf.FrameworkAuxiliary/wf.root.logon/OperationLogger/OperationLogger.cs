using System;
using System.Collections.Generic;

using System.Text;
using System.Data.SqlClient;
using System.Threading;
using dcl.entity;
using dcl.dao.interfaces;
using dcl.common;

namespace dcl.root.logon
{
    /// <summary>
    /// 操作日志记录工具
    /// </summary>
    public class OperationLogger
    {
        /// <summary>
        /// 记录日期
        /// </summary>
        DateTime today;

        /// <summary>
        /// 操作人
        /// </summary>
        string OperatorID;

        /// <summary>
        /// IP地址
        /// </summary>
        string IPAddress;

        /// <summary>
        /// 记录键值
        /// </summary>
        string OperationKey;

        /// <summary>
        /// 操作模块
        /// </summary>
        string Module;

        #region .ctor
        public OperationLogger(string operatorID, string ip, string module, string key)
        {
            today = DateTime.Now;
            OperatorID = operatorID;
            IPAddress = ip;
            OperationKey = key;
            Module = module;
            logs = new List<EntitySysOperationLog>();
        }
        #endregion

        /// <summary>
        /// 添加新增日志
        /// </summary>
        /// <param name="group"></param>
        /// <param name="content"></param>
        /// <param name="desc"></param>
        public void Add_AddLog(string group, string content, string desc)
        {
            AddLog(group, SysOperationType.ADD, content, desc);
        }

        /// <summary>
        /// 添加修改日志
        /// </summary>
        /// <param name="group"></param>
        /// <param name="content"></param>
        /// <param name="desc"></param>
        public void Add_ModifyLog(string group, string content, string desc)
        {
            AddLog(group, SysOperationType.MODIFY, content, desc);
        }

        /// <summary>
        /// 添加删除日志
        /// </summary>
        /// <param name="group"></param>
        /// <param name="content"></param>
        /// <param name="desc"></param>
        public void Add_DelLog(string group, string content, string desc)
        {
            AddLog(group, SysOperationType.DEL, content, desc);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="group"></param>
        /// <param name="operationType"></param>
        /// <param name="content"></param>
        /// <param name="desc"></param>
        public void AddLog(string group, string operationType, string content, string desc)
        {
            EntitySysOperationLog entity = new EntitySysOperationLog(group, operationType, content, desc);
            logs.Add(entity);
        }

        /// <summary>
        /// 记录列表
        /// </summary>
        public List<EntitySysOperationLog> logs { get; private set; }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="transHelper"></param>
        public void Log(bool start)

        {
            IDaoSysOperationLog logDao = DclDaoFactory.DaoHandler<IDaoSysOperationLog>();

            if (logDao != null && logs.Count > 0)
            {
                foreach (EntitySysOperationLog entity in logs)
                {
                    entity.OperatUserId = this.OperatorID;
                    entity.OperatServername = this.IPAddress;
                    entity.OperatDate = this.today;
                    entity.OperatKey = this.OperationKey;
                    entity.OperatModule = this.Module;

                    logDao.SaveSysOperationLog(entity);
                }
            }

        }

        public void Log()
        {
            new Thread(new ThreadStart(() =>
            {

                Log(true);

            })).Start();

        }
    }

    /// <summary>
    /// 日志实体
    /// </summary>
    public class OperationLoggerEntity
    {
        public string Group { get; set; }
        public string Content { get; set; }
        public string OperationType { get; set; }
        public string Description { get; set; }

        public OperationLoggerEntity(string group, string operationType, string content, string desc)
        {
            Group = group;
            Content = content;
            OperationType = operationType;
            Description = desc;
        }
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public struct SysOperationType
    {
        public const string ADD = "新增";
        public const string MODIFY = "修改";
        public const string DEL = "删除";
        public const string ITEMBACKUP = "项目复查备份";
    }


    /// <summary>
    /// 日志记录模块
    /// </summary>
    public struct SysOperationLogModule
    {
        public const string PATIENTS = "病人资料";
        public const string REAAPPLYGENTS = "试剂申领资料";
        public const string REAPURCHASE = "试剂采购资料";
        public const string REASTORAGE = "试剂入库资料";
        public const string READELIVERY = "试剂出库资料";
        public const string REALOSSREPORT = "试剂报损资料";
        public const string REASUBSCRIBE = "试剂申购资料";
    }


    /// <summary>
    /// 日志记录分组
    /// </summary>
    public struct SysOperationLogGroup
    {
        public const string PAT_INFO = "病人基本信息";
        public const string PAT_RESULT = "病人结果";
        public const string PAT_COMBINE = "检验组合";
        public const string PAT_IMAGERESULT = "图像结果";

        public const string REA_APPLYINFO = "试剂申领基本信息";
        public const string REA_PURCHASEINFO = "试剂采购基本信息";
        public const string REA_SUBSCRIBEINFO = "试剂申购基本信息";
        public const string REA_LOSSREPORTINFO = "试剂报损基本信息";
        public const string REA_DELIVERYINFO = "试剂出库基本信息";
        public const string REA_STORAGEINFO = "试剂入库基本信息";
    }
}
