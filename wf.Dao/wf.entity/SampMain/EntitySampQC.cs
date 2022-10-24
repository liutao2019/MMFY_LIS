using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntitySampQC : EntityBase
    {
        public EntitySampQC()
        {
            SearchDateType = SearchDataType.标本生成时间;
            ListType = new List<string>();
            ListSampStatusId = new List<string>();
            ListSampBarId = new List<string>();
            SearchDeleteSampMain = true;
            HandleProc = ReturnProc.全部;
            SearchHospital = true;
        }

        /// <summary>
        /// 条码内部关联ID集合
        /// </summary>   
        public List<String> ListSampBarId { get; set; }

        /// <summary>
        /// UPID唯一号 目前滨海使用
        /// </summary>   
        public String PidUniqueId { get; set; }

        /// <summary>
        /// 时间范围-开始时间
        /// </summary>
        public String StartDate { get; set; }

        /// <summary>
        /// 时间范围-结束时间
        /// </summary>
        public String EndDate { get; set; }

        /// <summary>
        /// 查询类型
        /// </summary>
        public string SearchType { get; set; }

        /// <summary>
        /// 查询值
        /// </summary>
        public string SearchValue { get; set; }


        /// <summary>
        /// 匹配类型
        /// </summary>
        public MatchType matchType { get; set; }

        public string HospitalId { get; set; }

        #region 标本信息查询
        /// <summary>
        /// 采集人工号
        /// </summary>
        public string CollectionUserID { get; set; }

        /// <summary>
        /// 送检人工号
        /// </summary>
        public string SendUserID { get; set; }

        /// <summary>
        /// 送达人工号
        /// </summary>
        public string ReachUserID { get; set; }


        /// <summary>
        /// 签收人ID
        /// </summary>
        public string ReceivedUserID { get; set; }

        #endregion

        #region 标本查询所用

        /// <summary>
        /// 物理组集合
        /// </summary>
        public List<String> ListType { get; set; }

        /// <summary>
        ///科室编码
        /// </summary>   
        public String PidDeptCode { get; set; }

        /// <summary>
        ///科室名称
        /// </summary>   
        public String PidDeptName { get; set; }

        /// <summary>
        /// 查询状态
        /// </summary>
        public List<String> ListSampStatusId { get; set; }

        /// <summary>
        /// 0 常规时间 1 最后时间 2条码操作时间
        /// </summary>
        public SearchDataType SearchDateType { get; set; }

        /// <summary>
        /// 条码时间状态（条码操作明细表状态） SearchDateType为2的时候起效
        /// </summary>
        public string SerchDateStatus { get; set; }

        /// <summary>
        /// 标本名称
        /// </summary>
        public String PidName { get; set; }

        /// <summary>
        /// 患者身份证信息
        /// </summary>
        public String PidIdentityCard { get; set; }

        /// <summary>
        /// 架子号
        /// </summary>
        public String RegRackNo { get; set; }

        /// <summary>
        /// 标本项目Lis代码
        /// </summary>
        public String ComId { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>
        public String ItrId { get; set; }

        /// <summary>
        /// 是否左连接到条码明细表
        /// </summary>
        public bool LeftJoinSampleDetail { get; set; } = false;

        #endregion

        #region 条码下载查询所用

        /// <summary>
        /// 病历号
        /// </summary>
        public string PidInNo { get; set; }

        /// <summary>
        /// 结束病历号（体检下载的时候会指定号段）
        /// </summary>
        public string PidInNoEnd { get; set; }

        /// <summary>
        /// HIS卡号/医保卡
        /// </summary>
        public string PidSocialNo { get; set; }

        /// <summary>
        /// 来源（107门诊 108 住院 109 体检）
        /// </summary>
        public string PidSrcId { get; set; }

        /// <summary>
        /// 床号
        /// </summary>
        public string PidBedNo { get; set; }

        /// <summary>
        /// 是否查询删除的信息
        /// </summary>
        public bool SearchDeleteSampMain { get; set; }

        /// <summary>
        ///预留字段(现记录条码生成方式。手工条码会录入标识在此字段)
        /// </summary>   
        public String SampInfo { get; set; }
        #endregion

        #region 条码回退查询所用

        public ReturnProc HandleProc { get; set; }

        /// <summary>
        /// 是否只查询住院
        /// </summary>
        public bool SearchHospital { get; set; }
        #endregion


        #region 粤核酸用
        /// <summary>
        /// 粤核酸总码
        /// </summary>
        public String SampYhsBarCode { get; set; }
        #endregion
    }

    [Serializable]
    public enum SearchDataType : Int32
    {
        标本生成时间 = 0,
        标本最后操作时间 = 1,
        标本流程时间 = 2,
        标本下载时间 = 3
    }

    [Serializable]
    public enum ReturnProc
    {
        全部,
        已处理,
        未处理
    }
}
