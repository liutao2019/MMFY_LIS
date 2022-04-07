using System;
using System.Collections.Generic;

using System.Text;

namespace dcl.entity
{
    public static class BarcodeTable
    {
        /// <summary>
        /// 接口表
        /// </summary>
        public struct Interfaces
        {
            public const string TableName = "dict_interfaces";
            public const string ID = "in_id";
            /// <summary>
            /// 名称
            /// </summary>
            public const string Name = "in_name";
            /// <summary>
            /// 数据库地址
            /// </summary>
            public const string DBAddress = "in_db_address";
            /// <summary>
            /// 数据库名
            /// </summary>
            public const string DBName = "in_db_name";
            /// <summary>
            /// 用户名
            /// </summary>
            public const string DBUsername = "in_db_username";
            /// <summary>
            /// 密码
            /// </summary>
            public const string DBPassword = "in_db_password";
            /// <summary>
            /// 连接类型
            /// </summary>
            public const string DBConnnectType = "in_db_connect_type";

            /// <summary>
            /// 接口取数据方式:视图;存储过程
            /// </summary>
            public const string InterfaceFetchType = "in_interface_fetchtype";

            /// <summary>
            /// 接口类型:None,Interface,Barcode
            /// </summary>
            public const string InterfaceType = "in_interface_type";

            /// <summary>
            /// 接口调用语句
            /// </summary>
            public const string InterfaceName = "in_interface_sql";

            /// <summary>
            /// 医院
            /// </summary>
            public const string InterfaceHospital = "in_interface_hospital";

            /// <summary>
            /// 返回DataTable表名称
            /// </summary>
            public const string ReturnDataTableName = "in_return_tablename";

        }


        /// <summary>
        /// 接口字段对照表
        /// </summary>
        public struct Contrast
        {
            public const string TableName = "dict_contrast";
            public const string ID = "con_id";
            /// <summary>
            /// His接口编号
            /// </summary>
            public const string InterfaceID = "con_interface_id";
            /// <summary>
            /// 接口的传入列
            /// </summary>
            public const string HisColumns = "con_interface_columns";
            /// <summary>
            /// Lis系统的列
            /// </summary>
            public const string LisColumns = "con_lis_columns";
            /// <summary>
            /// 转换规则
            /// </summary>
            public const string Rule = "con_rule";

            /// <summary>
            /// 类型：0-对照表  1-中转表
            /// </summary>
            public const string Type = "con_type";

            /// <summary>
            /// 列名备注
            /// </summary>
            public const string Description = "con_desc";
            /// <summary>
            /// 返回表名称
            /// </summary>
            public const string ReturnTable = "con_tablename";
            /// <summary>
            /// 查询排序码
            /// </summary>
            public const string SearchSeq = "con_search_seq";
            /// <summary>
            /// 列的数据类型
            /// </summary>
            public const string DataType = "con_datatype";

            /// <summary>
            /// 查询字段格式化脚本
            /// </summary>
            public const string SearchScript = "con_script_for_search";
        }

        /// <summary>
        /// 条码表(病人信息表)
        /// </summary>
        public struct Patient
        {
            public const string ID = "bc_id";

            /// <summary>
            /// 姓名
            /// </summary>
            public const string Name = "bc_name";

            /// <summary>
            /// 性别
            /// </summary>
            public const string Sex = "bc_sex";

            /// <summary>
            /// 年龄
            /// </summary>
            public const string Age = "bc_age";

            /// <summary>
            /// HIS科室编码
            /// </summary>
            public const string DepartmentCode = "bc_d_code";

            /// <summary>
            /// HIS科室名称
            /// </summary>
            public const string Department = "bc_d_name";
            public const String BarcodeNumber = "bc_bar_no";
            public const String BarcodeDisplayNumber = "bc_bar_code";
            public const String TableName = "bc_patients";
            public const string Urgent = "bc_urgent_flag";
            //体检号
            public const string EMPID = "bc_emp_id";
            /// <summary>
            /// ID
            /// </summary>
            public const string PatientID = "bc_in_no";

            /// <summary>
            /// ID类型
            /// </summary>
            public const string IDType = "bc_no_id";

            /// <summary>
            /// 开单医生工号
            /// </summary>
            public const string DoctorID = "bc_doct_code";

            /// <summary>
            /// 开单医生姓名
            /// </summary>
            public const string Doctor = "bc_doct_name";
            /// <summary>
            /// 病人来源
            /// </summary>
            public const string OriID = "bc_ori_id";
            public const string OriName = "bc_ori_name";
            /// <summary>
            /// 条码生成时间
            /// </summary>
            public const string CreateTime = "bc_date";
            public const string OrderExecuteTime = "bc_occ_date";
            public const string Cuvette = "bc_cuv_name";
            public const string CuvetteCode = "bc_cuv_code"; //试管类型
            /// <summary>
            /// 检查项目
            /// </summary>
            public const string Item = "bc_his_name";
            public const string CollecterName = "bc_blood_name";
            public const string CollecterJobName = "bc_blood_code";
            public const string CollectTime = "bc_blood_date";
            public const string CollectFlag = "bc_blood_flag";
            public const string SenderName = "bc_send_name";
            public const string SenderJobName = "bc_send_code";
            public const string SendTime = SampleSendDate;
            public const string SendFlag = SampleSendFlag;
            public const string ReceiverName = "bc_receiver_name";
            public const string ReceiverJobName = "bc_receiver_code";
            public const string ReceiveTime = SampleReceiveDate;
            public const string ReceiveFlag = SampleReceiveFlag;
            public const string PrinterName = "bc_print_name";
            public const string PrinterJobName = "bc_print_code";
            public const string PrintTime = "bc_print_date";
            public const string PrintFlag = "bc_print_flag";
            public const string ReacherName = "bc_reach_name";
            public const string ReacherJobName = "bc_reach_code";
            public const string ReachTime = "bc_reach_date";
            public const string ReachFlag = "bc_reach_flag";
            /// <summary>
            /// 单条码需打印次数
            /// </summary>
            public const string PrintCount = "bc_print_time";
            /// <summary>
            /// 是否抽血
            /// </summary>
            public const string BloodFlag = "bc_blood";
            /// <summary>
            /// 样本采集时间
            /// </summary>
            public const string SampleCollectDate = "bc_blood_date";
            /// <summary>
            /// 样本送检标识
            /// </summary>
            public const string SampleSendFlag = "bc_send_flag";
            /// <summary>
            /// 样本送检时间
            /// </summary>
            public const string SampleSendDate = "bc_send_date";
            /// <summary>
            /// 样本接收标识
            /// </summary>
            public const string SampleReceiveFlag = "bc_receiver_flag";
            /// <summary>
            /// 样本接收时间
            /// </summary>
            public const string SampleReceiveDate = "bc_receiver_date";

            /// <summary>
            /// 医保卡号或HIS卡号
            /// </summary>
            public const string SocialNumber = "bc_social_no";

            /// <summary>
            /// 当前状态的代码 bc_status
            /// </summary>
            public const string StatusCode = "bc_status";

            /// <summary>
            /// 当前状态的中文简称
            /// </summary>
            public const string StatusName = "bc_status_cname";

            /// <summary>
            /// 删除标识
            /// </summary>
            public const String DeleteFlag = "bc_del";

            /// <summary>
            /// 诊断
            /// </summary>
            public const string Diagnosis = "bc_diag";

            /// <summary>
            /// 病床号
            /// </summary>
            public const string BedNumber = "bc_bed_no";
            /// <summary>
            /// 备注
            /// </summary>
            public const string Remark = "bc_exp";
            /// <summary>
            /// 标本类别ID
            /// </summary>
            public const string SampleID = "bc_sam_id";
            /// <summary>
            /// 标本类别
            /// </summary>
            public const string SampleName = "bc_sam_name";
            /// <summary>
            /// 标本备注ID
            /// </summary>
            public const string SampleRemarkID = "bc_sam_rem_id";
            /// <summary>
            /// 标本备注
            /// </summary>
            public const string SampleRemarkName = "bc_sam_rem_name";
            /// <summary>
            /// 就诊次数
            /// </summary>
            public const string DiagnoseTime = "bc_times";

            /// <summary>
            /// 医嘱执行时间
            /// </summary>
            public const string DateApply = "bc_occ_date";
            /// <summary>
            /// 物理组ID
            /// </summary>
            public const string CTypeID = "bc_ctype";

            /// <summary>
            /// 回退标志
            /// </summary>
            public const string ReturnFlag = "bc_return_flag";

            /// <summary>
            /// 回退次数
            /// </summary>
            public const string ReturnTimes = "bc_return_times";

            /// <summary>
            /// 最后操作时间
            /// </summary>
            public const string LastActionTime = "bc_lastaction_time";

            /// <summary>
            /// 地址
            /// </summary>
            public const string Address = "bc_address";

            /// <summary>
            /// 联系电话
            /// </summary>
            public const string Tel = "bc_tel";

            /// <summary>
            /// 如果是非检验条码则会生成这一列数据
            /// </summary>
            public const string Bc_printtype = "bc_printtype";

            public const string BC_frequency = "bc_frequency";
        }

        /// <summary>
        /// 条码明细表(项目明细表)
        /// </summary>
        public struct CName
        {
            public const string ID = "bc_id";
            public const String TableName = "bc_cname";
            public const String HisName = "bc_his_name";
            public const String BarcodeNumber = "bc_bar_no";
            public const String BarcodeDisplayNumber = "bc_bar_code";
            public const string PatientID = "bc_yz_id";
            public const string Type = "bc_ctype";

            /// <summary>
            /// 删除标识
            /// </summary>
            public const String DeleteFlag = "bc_del";

            /// <summary>
            /// HIS组合编码
            /// </summary>
            public const string HisCode = "bc_his_code";
            /// <summary>
            /// 医嘱执行日期
            /// </summary>
            public const string ActionTime = "bc_occ_date";
            /// <summary>
            /// 医嘱申请日期
            /// </summary>
            public const string ApplyTime = "bc_apply_date";
            /// <summary>
            /// LIS组合编码
            /// </summary>
            public const string LisCombineCode = "bc_lis_code";
            /// <summary>
            /// 医嘱ID
            /// </summary>
            public const string OrderID = "bc_yz_id";
            /// <summary>
            /// 医嘱申请日期
            /// </summary>
            public const string OrderRequireDate = "bc_apply_date";
            /// <summary>
            /// 医嘱执行日期
            /// </summary>
            public const string OrderExecuteDate = "bc_occ_date";
            /// <summary>
            /// 登记标志(0-未登记 1-已登记)
            /// </summary>
            public const string EnrolFlag = "bc_enrol_flag";
            /// <summary>
            /// 价格
            /// </summary>
            public const string Price = "bc_price";
            /// <summary>
            /// 单位
            /// </summary>
            public const string Unit = "bc_unit";
            /// <summary>
            /// 显示标志(0-不显示 1-显示)
            /// </summary>
            public const string ViewFlag = "bc_view_flag";
            /// <summary>
            /// 组合项目名称
            /// </summary>
            public const string CombineName = "bc_name";
            /// <summary>
            /// 登记标志
            /// </summary>
            public const string SignFlag = "bc_flag";
            /// <summary>
            /// 采血注意
            /// </summary>
            public const string BloodNotice = "bc_blood_notice";
            /// <summary>
            /// 保存注意
            /// </summary>
            public const string SaveNotice = "bc_save_notice";
            /// <summary>
            /// 上机标志
            /// </summary>
            public const string MachineFlag = "bc_modify_flag";


        }
    }
}
