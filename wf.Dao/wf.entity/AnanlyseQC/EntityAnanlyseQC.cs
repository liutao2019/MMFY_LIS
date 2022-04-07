using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 分析查询实体
    /// </summary>
    [Serializable]
    public class EntityAnanlyseQC : EntityBase
    {
        public EntityAnanlyseQC()
        {
            listSid = new List<EntitySid>();
            listSort = new List<EntitySortNo>();
            ListItrId = new List<string>();
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// 审核名称
        /// </summary>
        public string auditWord { get; set; }

        /// <summary>
        /// 报告名称
        /// </summary>
        public string reportWord { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime DateEnd { get; set; }

        /// <summary>
        /// 样本范围
        /// </summary>
        public List<EntitySid> listSid { get; set; }

        /// <summary>
        /// 序号范围
        /// </summary>
        public List<EntitySortNo> listSort { get; set; }

        /// <summary>
        /// 是否查单病人
        /// </summary>
        public Boolean IsSingleSearch { get; set; }

        /// <summary>
        /// 是否启用外部查询
        /// </summary>
        public Boolean CanSearchOuterReport { get; set; }

        /// <summary>
        /// 仪器编码
        /// </summary>
        public String ItrId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public String StrClassWhere { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public String StrOtherWhere { get; set; }

        /// <summary>
        /// 标本类别
        /// </summary>
        public String SamId { get; set; }

        /// <summary>
        /// A仪器
        /// </summary>
        public String DoubleBlindItrA { get; set; }

        /// <summary>
        /// B仪器
        /// </summary>
        public String DoubleBlindItrB { get; set; }

        /// <summary>
        /// 病人历史结果是否只按姓名查询
        /// </summary>
        public Boolean Lab_ResultHistoryContrainName { get; set; }

        /// <summary>
        /// 样本号或者流水号范围
        /// </summary>
        public String NumRange { get; set; }

        /// <summary>
        /// 是否外部报告单
        /// </summary>
        public Boolean IsOuterReport { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public String ItmId { get; set; }

        /// <summary>
        /// 系统配置：细菌管理同时有药敏与无菌结果时优先药敏
        /// </summary>
        public String BacLabExistsAnAndCsSelAn { get; set; }

        /// <summary>
        /// 查询过滤无打印项目
        /// </summary>
        public String SelectFiterNoPrintItem { get; set; }

        /// <summary>
        /// 检查没打印报告
        /// </summary>
        public String ReportCheckNotPrintReport { get; set; }

        /// <summary>
        /// 外部报告形式
        /// </summary>
        public String OuterReportRepStyle { get; set; }

        /// <summary>
        /// 外部报告代码
        /// </summary>
        public String OuterReportCode { get; set; }

        /// <summary>
        /// 外部报告公共代码
        /// </summary>
        public String OuterReportCommonRepCode { get; set; }

        /// <summary>
        /// 是否可以从老数据库读取历史数据
        /// </summary>
        public String LabEnableReadHistoryFromOldDB { get; set; }

        /// <summary>
        /// 科室编码
        /// </summary>
        public String DepId { get; set; }

        /// <summary>
        /// 报告状态 0-未审核 1-已审核 2-已报告
        /// </summary>
        public String RepStatus { get; set; }

        /// <summary>
        /// 中间标志
        /// </summary>
        public String PatMidFlag { get; set; }

        /// <summary>
        /// 病人身份
        /// </summary>
        public Boolean PatIdentity { get; set; }

        /// <summary>
        /// 是否有权限查看没审核报告
        /// </summary>
        public Boolean IsCanLookNotAuditReport { get; set; }

        /// <summary>
        /// 是否过滤特定病人
        /// </summary>
        public Boolean IsFilterSpecPat { get; set; }

        /// <summary>
        /// 通过HIS接口方式是否为通用
        /// </summary>
        public Boolean IsNotOutlink { get; set; }

        /// <summary>
        /// 仪器ID组合
        /// </summary>
        public List<string> ListItrId { get; set; }

        public String listItrId
        {
            get
            {
                string itrIds = string.Empty;
                foreach (string item in ListItrId)
                {
                    itrIds += string.Format(",'{0}'", item);
                }
                if (itrIds.Length > 0)
                {
                    itrIds = itrIds.Remove(0, 1);
                }
                return itrIds;
            }
        }

        /// <summary>
        /// 是否有GGALL的权限
        /// </summary>
        public Boolean IsGGALL { get; set; }

        /// <summary>
        /// 是否有GGBJDX的权限
        /// </summary>
        public Boolean GGBJDX { get; set; }

        /// <summary>
        /// 是否有GGGWU的权限
        /// </summary>
        public Boolean GGGWU { get; set; }

        /// <summary>
        /// 是否有GGTD的权限
        /// </summary>
        public Boolean GGTD { get; set; }

        /// <summary>
        /// 是否有GGGR的权限
        /// </summary>
        public Boolean GGGR { get; set; }

        /// <summary>
        /// 是否有GGGB的权限
        /// </summary>
        public Boolean GGGB { get; set; }

        /// <summary>
        /// 标识ID
        /// </summary>
        public String PatId { get; set; }

        /// <summary>
        /// 时间查询字段
        /// </summary>
        public String StrSelectTime { get; set; }

        /// <summary>
        /// 时间查询类型
        /// </summary>
        public string DateType { get; set; }

        /// <summary>
        /// 查询多病人还是单病人
        /// </summary>
        public Boolean SearchManyPatients { get; set; }

        /// <summary>
        /// 科室ID过滤
        /// </summary>
        public String DepIdFilter { get; set; }

        /// <summary>
        /// 旧检验数据
        /// </summary>
        public String PatCType { get; set; }

        /// <summary>
        /// 独立查询客户端时间查询字段
        /// </summary>
        public String StrSelectType { get; set; }

        /// <summary>
        /// 外部报告单查询接口模式
        /// </summary>
        public Boolean SearchOuterInterfaceMode { get; set; }

        /// <summary>
        /// 是否启用CDR报告查询
        /// </summary>
        public Boolean SearchCDR { get; set; }

        /// <summary>
        /// 申请医生
        /// </summary>
        public String PatDocName { get; set; }

        /// <summary>
        /// 体检单位名称
        /// </summary>
        public String PatEmpCompanyName { get; set; }

        /// <summary>
        /// 组合名称
        /// </summary>
        public String PatCName { get; set; }

        /// <summary>
        /// 组合id
        /// </summary>
        public String PatComId { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        public String PatName { get; set; }

        /// <summary>
        /// 科室名称
        /// </summary>
        public String PatDepName { get; set; }

        /// <summary>
        /// 病区代码
        /// </summary>
        public String PatWardId { get; set; }

        /// <summary>
        /// 是否允许多病人查询
        /// </summary>
        public Boolean EnableMutliPatQuery { get; set; }

        /// <summary>
        /// 是否自动
        /// </summary>
        public Boolean IsAuto { get; set; }

        /// <summary>
        /// 是否直接返回
        /// </summary>
        public Boolean IsReturn { get; set; }

        /// <summary>
        /// ID类型
        /// </summary>
        public String PatNoId { get; set; }

        /// <summary>
        /// 申请单号
        /// </summary>
        public String StrCisYzId { get; set; }

        /// <summary>
        /// 住院检验报告查询接口唯一字段
        /// </summary>
        public String SelectColumn { get; set; }

        /// <summary>
        /// 是否住院检验报告查询接口只用一个字段
        /// </summary>
        public Boolean isOnlyOneSelColumn { get; set; }

        /// <summary>
        /// 输入ID
        /// </summary>
        public String PatInNo { get; set; }

        /// <summary>
        /// 病人ID
        /// </summary>
        public String RepId { get; set; }

        /// <summary>
        /// 串行数字
        /// </summary>
        public String SerialNumber { get; set; }

        /// <summary>
        /// 标本接收日期
        /// </summary>
        public DateTime? PatSampleReceiveDate { get; set; }

        /// <summary>
        /// 医院院网接口模式
        /// </summary>
        public String HospitalInterfaceMode { get; set; }

        /// <summary>
        /// 报告查询[病历号]所用字段
        /// </summary>
        public String UseColumns { get; set; }

        /// <summary>
        /// 物理组别
        /// </summary>
        public String TypeId { get; set; }

        /// <summary>
        /// 来源ID
        /// </summary>
        public String OriId { get; set; }

        /// <summary>
        /// 是否有权限看保密数据
        /// </summary>
        public Boolean EnableLookSecretData { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public String PatBarCode { get; set; }

        /// <summary>
        /// 是否金域报表自定义连接地址
        /// </summary>
        public Boolean IsKMReportDIYCon { get; set; }

        /// <summary>
        /// 是否有权限查看未审核数据
        /// </summary>
        public Boolean IsLookNaturalReport { get; set; }

        /// <summary>
        /// 床号
        /// </summary>
        public String PatBedNo { get; set; }

        /// <summary>
        /// 诊断
        /// </summary>
        public String PatDiag { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public String PatTel { get; set; }

        /// <summary>
        /// 查询用病历号
        /// </summary>
        public String TxtInNoSql { get; set; }

        /// <summary>
        /// 病历号
        /// </summary>
        public String TxtInNo { get; set; }

        /// <summary>
        /// 体检ID
        /// </summary>
        public String PatEmpId { get; set; }

        /// <summary>
        /// 唯一号
        /// </summary>
        public String PatUpid { get; set; }

        /// <summary>
        /// 报告查询[病历号]所用字段1
        /// </summary>
        public String TempSelNoStr1 { get; set; }

        /// <summary>
        /// 报告查询[病历号]所用字段2
        /// </summary>
        public String TempSelNoStr2 { get; set; }

        /// <summary>
        /// 选择代码
        /// </summary>
        public String DepSelectCode { get; set; }

        /// <summary>
        /// 查找类型
        /// </summary>
        public String SearchType { get; set; }

        /// <summary>
        /// 是否住院调用
        /// </summary>
        public bool EnbState { get; set; }

        /// <summary>
        /// 条码标志
        /// </summary>
        public String CombineFlag { get; set; }

        /// <summary>
        /// 操作者id
        /// </summary>
        public String OperatorID { get; set; }

        /// <summary>
        /// 操作者名称
        /// </summary>
        public String OperatorName { get; set; }

        /// <summary>
        /// 操作地点
        /// </summary>
        public String StrPlace { get; set; }

        /// <summary>
        /// 检验者(录入人ID)
        /// </summary>
        public String PidCheckUserId { get; set; }

        /// <summary>
        /// 一审人ID
        /// </summary>
        public String PidAuditUserId { get; set; }

        /// <summary>
        /// 备注信息--记录ip
        /// </summary>
        public String StrRemark { get; set; }

        /// <summary>
        /// 匹配类型
        /// </summary>
        public MatchType matchType { get; set; }

        /// <summary>
        /// 报告类型
        /// </summary>
        public ReportType reportType { get; set; }

        /// <summary>
        /// 病人资料类型
        /// </summary>
        public PatInfoType patInfoType { get; set; }

        /// <summary>
        /// 范围类型
        /// </summary>
        public RangeType numType { get; set; }

        #region 报表代码
        /// <summary>
        /// 报表代码
        /// </summary>
        public String RepCode
        {
            get
            {
                if (string.IsNullOrEmpty(_repCode))
                {
                    return "summaryPrint";
                }
                else
                {
                    return _repCode;
                }
            }
        }
        public string _repCode { get; set; }
        #endregion

        public Boolean isSortByCombine { get; set; }

        /// <summary>
        /// 医院ID
        /// </summary>
        public String HospitalId { get; set; }

    }

    /// <summary>
    /// 样本实体
    /// </summary>
    [Serializable]
    public class EntitySid
    {
        /// <summary>
        /// 开始样本号
        /// </summary>
        public int StartSid { get; set; }
        /// <summary>
        /// 结束样本号
        /// </summary>
        public int? EndSid { get; set; }
    }

    /// <summary>
    /// 序号实体
    /// </summary>
    public class EntitySortNo
    {
        /// <summary>
        /// 开始序号
        /// </summary>
        public int StartNo { get; set; }
        /// <summary>
        /// 结束序号
        /// </summary>
        public int? EndNo { get; set; }
    }

    /// <summary>
    /// 测定结果类型
    /// </summary>
    public enum ReportType
    {
        /// <summary>
        /// 审核测定结果
        /// </summary>
        SHCDJG,

        /// <summary>
        /// 阳性测定结果
        /// </summary>
        YXCDJG,

        /// <summary>
        /// 未审测定结果
        /// </summary>
        WSCDJG
    }

    /// <summary>
    /// 病人资料类型
    /// </summary>
    public enum PatInfoType
    {
        /// <summary>
        /// 摘要病人资料
        /// </summary>
        ZYBRZL,
        /// <summary>
        /// 详细病人资料
        /// </summary>
        XXBRZL
    }

    /// <summary>
    /// 范围类型
    /// </summary>
    public enum RangeType
    {
        /// <summary>
        /// 样本号
        /// </summary>
        PatSid,
        /// <summary>
        /// 流水号
        /// </summary>
        PatHostOrder
    }

    /// <summary>
    /// 匹配类型
    /// </summary>
    public enum MatchType
    {
        /// <summary>
        /// 全模糊
        /// </summary>
        QUANMOHU,
        /// <summary>
        /// 半模糊
        /// </summary>
        BANMOHU,
        /// <summary>
        /// 全匹配
        /// </summary>
        QUANPIPEI
    }
}
