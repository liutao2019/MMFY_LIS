using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntitySampMain
    {
        public EntitySampMain()
        {
            SampDate = DateTime.Now;
            SampOccDate = DateTime.Now;
            SampLastactionDate = DateTime.Now;
            PidAdmissTimes = 0;
            SampPrintFlag = 0;
            SampMinCapcity = 0;
            SampBarBatchNo = 0;
        }


        /// <summary>
        /// 自增ID
        /// </summary> 
        public Int32 SampSn { get; set; }

        /// <summary>
        /// 内部关联ID
        /// </summary>     
        public String SampBarId { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>                       
        public String SampBarCode { get; set; }

        /// <summary>
        /// 条码批次
        /// </summary>                       
        public Int32 SampBarBatchNo { get; set; }

        /// <summary>
        /// 病人标识类型
        /// </summary>                       
        public String PidIdtId { get; set; }

        /// <summary>
        /// 病人标识
        /// </summary>                       
        public String PidInNo { get; set; }

        /// <summary>
        /// 床号
        /// </summary>                       
        public String PidBedNo { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>                       
        public String PidName { get; set; }

        /// <summary>
        /// 性别(0-未知 1-男 2-女)
        /// </summary>                       
        public String PidSex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>                       
        public String PidAge { get; set; }

        /// <summary>
        /// HIS科室编码
        /// </summary>                       
        public String PidDeptCode { get; set; }

        /// <summary>
        /// HIS科室名称
        /// </summary>                       
        public String PidDeptName { get; set; }

        /// <summary>
        /// 诊断
        /// </summary>                       
        public String PidDiag { get; set; }

        /// <summary>
        /// 开单医生工号
        /// </summary>                       
        public String PidDoctorCode { get; set; }

        /// <summary>
        /// 开单医生姓名
        /// </summary>                       
        public String PidDoctorName { get; set; }

        /// <summary>
        /// 组合累加显示名称
        /// </summary>                       
        public String SampComName { get; set; }

        /// <summary>
        /// 标本编码
        /// </summary>                       
        public String SampSamId { get; set; }

        /// <summary>
        /// 标本名称
        /// </summary>                       
        public String SampSamName { get; set; }

        /// <summary>
        /// 条码生成日期
        /// </summary>                       
        public DateTime SampDate { get; set; }

        /// <summary>
        /// 就诊次数
        /// </summary>                       
        public Int32 PidAdmissTimes { get; set; }

        /// <summary>
        /// 条码打印标志
        /// </summary>                       
        public Int32 SampPrintFlag { get; set; }

        /// <summary>
        /// 打印时间
        /// </summary>                       
        public DateTime? SampPrintDate { get; set; }

        /// <summary>
        /// 打印者工号
        /// </summary>                       
        public String SampPrintUserId { get; set; }

        /// <summary>
        /// 打印者姓名
        /// </summary>                       
        public String SampPrintUserName { get; set; }

        /// <summary>
        /// 采集容器编码
        /// </summary>                       
        public String SampTubCode { get; set; }

        /// <summary>
        /// 采集容器名称
        /// </summary>                       
        public String SampTubName { get; set; }

        /// <summary>
        /// 当前条码最小采集量
        /// </summary>                       
        public Int32 SampMinCapcity { get; set; }

        /// <summary>
        /// 采集量单位(固定单位 L、ML)
        /// </summary>                       
        public String SampCapcityUnit { get; set; }

        /// <summary>
        /// 急查标志
        /// </summary>                       
        public Boolean SampUrgentFlag { get; set; }

        /// <summary>
        /// 类别(预留用)
        /// </summary>                       
        public String SampType { get; set; }

        /// <summary>
        /// 病人类别编码(门诊、住院，对应dict_origin表)
        /// </summary>                       
        public String PidSrcId { get; set; }

        /// <summary>
        /// 病人类别名称
        /// </summary>                       
        public String PidSrcName { get; set; }

        /// <summary>
        /// 检验科接收标志(0-未接收 1-已接收)
        /// </summary>                       
        public Int32 ReceiverFlag { get; set; }

        /// <summary>
        /// 检验科接收日期
        /// </summary>                       
        public DateTime? ReceiverDate { get; set; }

        /// <summary>
        /// 检验科接收者工号
        /// </summary>                       
        public String ReceiverUserId { get; set; }

        /// <summary>
        /// 检验科接收者姓名
        /// </summary>                       
        public String ReceiverUserName { get; set; }

        /// <summary>
        /// 标本采集标志(0-未采集 1-已采集)
        /// </summary>                       
        public int CollectionFlag { get; set; }

        /// <summary>
        /// 标本采集时间
        /// </summary>                       
        public DateTime? CollectionDate { get; set; }

        /// <summary>
        /// 标本采集者工号
        /// </summary>                       
        public String CollectionUserId { get; set; }

        /// <summary>
        /// 标本采集者姓名
        /// </summary>                       
        public String CollectionUserName { get; set; }

        /// <summary>
        /// 标本送检标志
        /// </summary>                       
        public Int32 SendFlag { get; set; }

        /// <summary>
        /// 标本送检时间
        /// </summary>                       
        public DateTime? SendDate { get; set; }

        /// <summary>
        /// 标本送检者工号
        /// </summary>                       
        public String SendUserId { get; set; }

        /// <summary>
        /// 标本送检者姓名
        /// </summary>                       
        public String SendUserName { get; set; }

        /// <summary>
        /// 标本送达标志
        /// </summary>                       
        public Int32 ReachFlag { get; set; }

        /// <summary>
        /// 标本送达时间
        /// </summary>                       
        public DateTime? ReachDate { get; set; }

        /// <summary>
        /// 标本送达者工号
        /// </summary>                       
        public String ReachUserId { get; set; }

        /// <summary>
        /// 标本送达者姓名
        /// </summary>                       
        public String ReachUserName { get; set; }

        /// <summary>
        /// 备注(注意事项)
        /// </summary>                       
        public String SampRemark { get; set; }

        /// <summary>
        /// 下载计算机
        /// </summary>                       
        public String SampComputer { get; set; }

        /// <summary>
        /// HIS卡号/医保卡(预留用)
        /// </summary>                       
        public String PidSocialNo { get; set; }

        /// <summary>
        /// 体检病人ID
        /// </summary>                       
        public String PidExamNo { get; set; }

        /// <summary>
        /// 预留字段
        /// </summary>                       
        public String SampInfo { get; set; }

        /// <summary>
        /// 医院编码
        /// </summary>                       
        public String PidOrgId { get; set; }

        /// <summary>
        /// 条码类型(0-打印条码 1-预制条码)
        /// </summary>                       
        public Int32 SampBarType { get; set; }

        /// <summary>
        /// 标本状态ID  (0-未打印,1-打印,2-采集, 3-已收取,4-已送检,5-签收,6-检验中,7-已检验)
        /// </summary>                       
        public String SampStatusId { get; set; }

        /// <summary>
        /// 状态中文简称
        /// </summary>                       
        public String SampStatusName { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>                       
        public String DelFlag { get; set; }

        /// <summary>
        /// 病人出生日期
        /// </summary>                       
        public DateTime? PidBirthday { get; set; }

        /// <summary>
        /// 抽血标识
        /// </summary>                       
        public Boolean SampBloodFlag { get; set; }

        /// <summary>
        /// 条码医嘱时间
        /// </summary>                       
        public DateTime SampOccDate { get; set; }

        /// <summary>
        /// 标本备注ID
        /// </summary>                       
        public String SampRemId { get; set; }

        /// <summary>
        /// 标本备注名称
        /// </summary>                       
        public String SampRemContent { get; set; }

        /// <summary>
        /// 回退标记
        /// </summary>                       
        public Boolean SampReturnFlag { get; set; }

        /// <summary>
        /// 打印次数（读取字典,表示该条码需要打印几次）
        /// </summary>                       
        public Int32 SampPrintTime { get; set; }

        /// <summary>
        /// 最后操作时间
        /// </summary>                       
        public DateTime SampLastactionDate { get; set; }

        /// <summary>
        /// 回退次数
        /// </summary>                       
        public Int32 SampReturnTimes { get; set; }

        /// <summary>
        /// 地址
        /// </summary>                       
        public String PidAddress { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>                       
        public String PidTel { get; set; }

        /// <summary>
        /// 第三方系统ID
        /// </summary>                       
        public String PidPatno { get; set; }

        /// <summary>
        /// 体检单位
        /// </summary>                       
        public String PidExamCompany { get; set; }

        /// <summary>
        /// 二次送检时间
        /// </summary>                       
        public DateTime? SecondSendDate { get; set; }

        /// <summary>
        /// 二次送检者工号
        /// </summary>                       
        public String SecondSendUserId { get; set; }

        /// <summary>
        /// 二次送检者姓名
        /// </summary>                       
        public String SecondSendUserName { get; set; }

        /// <summary>
        /// 二次送检标识
        /// </summary>                       
        public String SecondSendFlag { get; set; }

        /// <summary>
        /// 申请单号
        /// </summary>                       
        public String SampApplyNo { get; set; }

        /// <summary>
        /// 费用类别
        /// </summary>                       
        public String PidInsuId { get; set; }

        /// <summary>
        /// 标本送检地
        /// </summary>                       
        public String SampSendDest { get; set; }

        /// <summary>
        /// 体检单位名称
        /// </summary>                       
        public String PidExamCompanyName { get; set; }

        /// <summary>
        /// 体检单位部门
        /// </summary>                       
        public String PidExamCompanyDept { get; set; }

        /// <summary>
        /// 前处理标记 仪器用
        /// </summary>                       
        public Int32 SampPreProcess { get; set; }

        /// <summary>
        /// 检验旧门诊条码
        /// </summary>                       
        public String SampOriginalBarcode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>                       
        public String SampCollectionNotice { get; set; }

        /// <summary>
        /// UPID唯一号 目前滨海使用
        /// </summary>                       
        public String PidUniqueId { get; set; }

        /// <summary>
        /// 保存拆分大组合(特殊合并)ID
        /// </summary>                       
        public String SampMergeComId { get; set; }

        /// <summary>
        /// 人员身份
        /// </summary>                       
        public Int32 PidIdentity { get; set; }

        /// <summary>
        /// 急查状态(急，脑卒中，非急：1,2,0)
        /// </summary>                       
        public Int32 SampUrgentStatus { get; set; }

        /// <summary>
        /// 最后操作地点
        /// </summary>                       
        public String SampLastactionPlace { get; set; }

        /// <summary>
        /// 二次送检标识
        /// </summary>                       
        public Int32 BcSecondsendFlag { get; set; }

        /// <summary>
        /// 最后操作人ID
        /// </summary>                       
        public String SampLastactionUserId { get; set; }

        /// <summary>
        /// 二次签收标志
        /// </summary>                       
        public Int32 SecondReceiveFlag { get; set; }

        /// <summary>
        /// 吸氧浓度--暂时滨海使用
        /// </summary>                       
        public String SampOxygenConcentration { get; set; }

        /// <summary>
        /// 体温--暂时滨海使用
        /// </summary>                       
        public String SampTemperature { get; set; }

        /// <summary>
        /// 批号
        /// </summary>                       
        public String SampPackNo { get; set; }


    }
}
