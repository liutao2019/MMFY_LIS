using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{ 
    /// <summary>
    /// 病人结果查询条件
    /// </summary>
    [Serializable]
    public class EntityResultQC : EntityBase
    {
        public EntityResultQC()
        {
            ListObrId = new List<string>();
            IsCheck = true;
            OnlyGetNonePatInfoResult = false;
            IsCopy = false;
            listItmIds = new List<string>();
            IsNullItmPrtFlag = false;
            IsNullComPrtFlag = false;
            IsCheckMB = false;
            ItmIdIsNull = false;
            ItmComIdIsNull = false;
            OnlyReport = false;
            UrgentFlag = false;
        }

        /// <summary>
        /// 标识ID集合
        /// </summary>
        public List<string> ListObrId { get; set; }

        /// <summary>
        /// 病人唯一号标识集合
        /// </summary>
        public List<string> ListPidInNo { get; set; }

        /// <summary>
        /// 项目编码集合
        /// </summary>
        public List<string> ListDitmEcode { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public String ItmId { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>
        public String ItrId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public String StartObrDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public String EndObrDate { get; set; }

        /// <summary>
        /// 不属于检验或者审核状态 默认为true    
        /// </summary>
        public bool IsCheck { get; set; }


        /// <summary>
        /// 只查询2审的结果  默认为false  
        /// </summary>
        public bool OnlyReport { get; set; }

        /// <summary>
        /// 是否只获取无病人资料的结果  结果合并中使用 默认为false
        /// </summary>
        public bool OnlyGetNonePatInfoResult { get; set; }


        /// <summary>
        /// 是否复制  结果合并中使用 默认为false
        /// </summary>
        public bool IsCopy { get; set; }

        /// <summary>
        /// 结果有效标志
        /// </summary>
        public String ObrFlag { get; set; }

        /// <summary>
        /// 判断结果值是否为空
        /// </summary>
        public Boolean ResChrIsNull { get; set; }

        /// <summary>
        /// 主键ID
        /// </summary>
        public String ObrSn { get; set; }

        /// <summary>
        /// 项目id集合
        /// </summary>
        public List<string> listItmIds { get; set; }

        /// <summary>
        /// 是否过滤项目打印标记
        /// </summary>
        public Boolean IsNullItmPrtFlag { get; set; }

        /// <summary>
        /// 是否过滤组合项目打印标记
        /// </summary>
        public Boolean IsNullComPrtFlag { get; set; }

        /// <summary>
        /// 是否选中酶标
        /// </summary>
        public Boolean IsCheckMB { get; set; }

        /// <summary>
        /// 项目Id是否为空
        /// </summary>
        public bool ItmIdIsNull { get; set; }

        /// <summary>
        /// 组合ID是否为空
        /// </summary>
        public bool ItmComIdIsNull { get; set; }

        /// <summary>
        /// 只去危急结果
        /// </summary>
        public bool UrgentFlag { get; set; }

        #region 病人信息表条件
        /// <summary>
        /// 病人开始时间
        /// </summary>
        public String StartRepInDate { get; set; }

        /// <summary>
        /// 病人结束时间
        /// </summary>
        public String EndRepInDate { get; set; }

        /// <summary>
        /// 报告Id
        /// </summary>
        public String RepId { get; set; }

        /// <summary>
        /// 标本Id
        /// </summary>
        public String PidSamId { get; set; }

        /// <summary>
        /// 病人Id
        /// </summary>
        public String PidInNo { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        public String PidName { get; set; }

        /// <summary>
        /// 病人性别
        /// </summary>
        public String PidSex { get; set; }

        /// <summary>
        /// 病人条码号
        /// </summary>
        public String RepBarCode { get; set; }

        /// <summary>
        /// 独特ID
        /// </summary>
        public String PidUniqueId { get; set; }

        /// <summary>
        /// 就诊卡号
        /// </summary>
        public String PidSocialNo { get; set; }

        /// <summary>
        /// 住院次数
        /// </summary>
        public String PidAddmissTimes { get; set; }

        #endregion

        #region 附加字段 样本号(查询条件)
        /// <summary>
        /// 样本号
        /// </summary>
        public String RepSid { get; set; }
        #endregion

    }
}
