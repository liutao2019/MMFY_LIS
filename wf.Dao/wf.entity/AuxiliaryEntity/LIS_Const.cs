using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.entity
{
    public static class LIS_Const
    {
        public struct del_flag
        {
            /// <summary>
            /// 可用
            /// </summary>
            public const String OPEN = "0";
            /// <summary>
            /// 删除
            /// </summary>
            public const String DEL = "1";
            /// <summary>
            /// 历史
            /// </summary>
            public const String HISTORY = "2";
        }
        /// <summary>
        /// 试剂审核等,对应数据库表
        /// </summary>
        public struct ReagentPopedomCode
        {
            /// <summary>
            /// 主管审批权限
            /// </summary>
            public const string Audit = "434";

            /// <summary>
            /// 申领保存
            /// </summary>
            public const string ApplySave = "436";

            /// <summary>
            /// 申领删除
            /// </summary>
            public const string ApplyDelete = "437";

            /// <summary>
            /// 申领撤销
            /// </summary>
            public const string ApplyReturn = "435";
            /// <summary>
            /// 申购保存
            /// </summary>
            public const string SubscribeSave = "438";

            /// <summary>
            /// 申购删除
            /// </summary>
            public const string SubscribeDelete = "440";

            /// <summary>
            /// 申购撤销
            /// </summary>
            public const string SubscribeReturn = "439";

            /// <summary>
            /// 采购保存
            /// </summary>
            public const string PurchaseSave = "441";

            /// <summary>
            /// 采购删除
            /// </summary>
            public const string PurchaseDelete = "443";

            /// <summary>
            /// 采购撤销
            /// </summary>
            public const string PurchaseReturn = "442";

            /// <summary>
            /// 入库保存
            /// </summary>
            public const string StorageSave = "444";

            /// <summary>
            /// 入库删除
            /// </summary>
            public const string StorageDelete = "446";

            /// <summary>
            /// 入库撤销
            /// </summary>
            public const string StorageReturn = "447";

            /// <summary>
            /// 入库审核
            /// </summary>
            public const string StorageAudit = "445";

            /// <summary>
            /// 出库保存
            /// </summary>
            public const string DeliverySave = "448";

            /// <summary>
            /// 出库删除
            /// </summary>
            public const string DeliveryDelete = "450";

            /// <summary>
            /// 出库撤销
            /// </summary>
            public const string DeliveryReturn = "451";

            /// <summary>
            /// 出库审核
            /// </summary>
            public const string DeliveryAudit = "449";

            /// <summary>
            /// 报损保存
            /// </summary>
            public const string LossSave = "452";

            /// <summary>
            /// 报损删除
            /// </summary>
            public const string LossDelete = "455";

            /// <summary>
            /// 报损撤销
            /// </summary>
            public const string LossReturn = "454";

            /// <summary>
            /// 报损审核
            /// </summary>
            public const string LossAudit = "453";
        }
        /// <summary>
        /// 试剂资料状态
        /// </summary>
        public struct REAGENT_FLAG
        {
            /// <summary>
            /// 未审核(原始状态)
            /// </summary>
            public const string Natural = "1";

            /// <summary>
            /// 未通过
            /// </summary>
            public const string Returned = "2";

            /// <summary>
            /// 已审核
            /// </summary>
            public const string Audited = "4";

            /// <summary>
            /// 未打印
            /// </summary>
            public const string UnPrinted = "8";
            /// <summary>
            /// 已完成
            /// </summary>
            public const string Done = "9";
        }
        /// <summary>
        /// 检验信息删除参数
        /// </summary>
        public struct STRUCT_LAB_DEL_FLAG
        {
            /// <summary>
            /// 只删除主资料
            /// </summary>
            public static String DEL_PAT = "0";
            /// <summary>
            /// 连结果一起删除
            /// </summary>
            public static String DEL_ALL = "1";
            /// <summary>
            /// 提示是否删除结果
            /// </summary>
            public static String DEL_ALTER = "2";
        }



        /// <summary>
        ///操作类型 
        /// </summary>
        public struct action_type
        {
            public const String Update = "Update";
            public const String New = "New";
        }

        /// <summary>
        /// 病人资料状态
        /// </summary>
        public struct PATIENT_FLAG
        {
            /// <summary>
            /// 未审核(原始状态)
            /// </summary>
            public const string Natural = "0";

            /// <summary>
            /// 已报告
            /// </summary>
            public const string Reported = "2";

            /// <summary>
            /// 已审核
            /// </summary>
            public const string Audited = "1";

            /// <summary>
            /// 已打印
            /// </summary>
            public const string Printed = "4";
        }


        /// <summary>
        /// 仪器数据类型
        /// </summary>
        public struct InstmtDataType
        {
            /// <summary>
            /// 01-普通 
            /// </summary>
            public const string Normal = "1";
            /// <summary>
            /// 02-酶标 
            /// </summary>
            public const string Eiasa = "2";
            /// <summary>
            /// 03-细菌
            /// </summary>         
            public const string Bacteria = "3";
            /// <summary>
            /// 04-描述
            /// </summary>         
            public const string Description = "4";

            /// <summary>
            /// 05-过敏原(3结果)
            /// </summary>         
            public const string Rapid = "5";

            /// <summary>
            /// 06-新生儿筛查
            /// </summary>         
            public const string BabyFilter = "6";

            /// <summary>
            /// 07-骨髓
            /// </summary>         
            public const string Marrow = "7";
        }


        /// <summary>
        /// 用户审核、报告权限ID,对应数据库表
        /// </summary>
        public struct BillPopedomCode
        {
            /// <summary>
            /// 审核权限
            /// </summary>
            public const string Audit = "189";

            /// <summary>
            /// 反审
            /// </summary>
            public const string UndoAudit = "190";

            /// <summary>
            /// 报告
            /// </summary>
            public const string Report = "193";

            /// <summary>
            /// 取消报告
            /// </summary>
            public const string UndoReport = "194";

            /// <summary>
            /// 删除
            /// </summary>
            public const string Delete = "197";
        }


        /// <summary>
        /// 病人资料录入网格显示方式
        /// </summary>
        public struct ResultGridVisibleStyle
        {
            public const int Single = 0;
            public const int Double = 1;
            public const int Tree = 3;
        }

        /// <summary>
        /// 病人资料结果类型
        /// </summary>
        public struct PatResultType
        {
            /// <summary>
            /// 手工输入
            /// </summary>
            public const string Normal = "0";

            /// <summary>
            /// 仪器输入
            /// </summary>
            public const string Itr = "1";

            /// <summary>
            /// 关联计算结果
            /// </summary>
            public const string Cal = "2";

            /// <summary>
            /// 默认结果
            /// </summary>
            public const string Default = "3";
        }

        /// <summary>
        /// 系统配置代码表
        /// </summary>
        public struct SystemConfigurationCode
        {
            /// <summary>
            /// 审核_审核期限(小时)
            /// </summary>
            public const string AuditExpireHours = "AuditExpireHours";

            /// <summary>
            /// 审核_超出阈值仍能审核
            /// </summary>
            public const string OverThresholdCanAudit = "OverThresholdCanAudit";

            /// <summary>
            /// 审核_超出参考值是否提示
            /// </summary>
            public const string OverRefRemind = "OverRefRemind";

            /// <summary>
            /// 审核_结果类型不符时提示
            /// </summary>
            public const string ResDataTypeErrorRemind = "ResDataTypeErrorRemind";

            /// <summary>
            /// 检验报告管理_允许用户自定义面板
            /// </summary>
            public const string AllowCustomizePanel = "AllowCustomizePanel";

            /// <summary>
            /// 允许跳过审核直接报告
            /// </summary>
            public const string AllowStepAuditToReport = "AllowStepAuditToReport";

            /// <summary>
            /// 审核时取消病人召回信息
            /// </summary>
            public const string CancelCallBackPatientOnAudit = "Audit_CancelCallBackPatient";

            /// <summary>
            /// 审核时是否能够插入必录缺省结果项目
            /// </summary>
            public const string CanInsertDefultItemResult = "Audit_CanInsertDefultItemResult";
        }

        public struct MicReportFlag
        {
            /// <summary>
            /// 发送过
            /// </summary>
            public const String Yes = "1";
            /// <summary>
            /// 未发送过
            /// </summary>
            public const String N0 = "0";
        }
    }
}
