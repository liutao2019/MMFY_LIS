using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityWhonetSample : EntityBase
    {
        public EntityWhonetSample()
        {
            this.Results = new List<EntityWhonetResult>();
        }

        /// <summary>
        /// 患者就诊号
        /// </summary>
        public string PATIENT_ID { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string FIRST_NAME { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public string LAST_NAME { get; set; }

        /// <summary>
        /// 性别 1男 2女 0未知
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime? DateBirth { get; set; }

        /// <summary>
        /// 机构
        /// </summary>
        public string INSTITUT { get; set; }

        /// <summary>
        /// 病区
        /// </summary>
        public string WARD { get; set; }

        /// <summary>
        /// 病人类型
        /// </summary>
        public string WARD_TYPE { get; set; }

        /// <summary>
        /// 科室
        /// </summary>
        public string DEPARTMENT { get; set; }

        /// <summary>
        /// 标本号
        /// </summary>
        public string SPEC_NUM { get; set; }

        /// <summary>
        /// 标本日期
        /// </summary>
        public DateTime SPEC_DATE { get; set; }

        /// <summary>
        /// 未知
        /// </summary>
        public string SPEC_RES { get; set; }

        /// <summary>
        /// 标本类型,whonet标本类别缩写码
        /// </summary>
        public string SPEC_TYPE { get; set; }

        /// <summary>
        /// 标本类型,whonet标本类别编码/ID
        /// </summary>
        public string SPEC_CODE { get; set; }

        public string PAT_TYPE { get; set; }

        public string ORG_TYPE { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string COMMENT { get; set; }

        /// <summary>
        /// β内酰胺酶结果：+、-
        /// </summary>
        public string BETA_LACT { get; set; }

        /// <summary>
        /// 超广谱内酰胺酶结果：+、-
        /// </summary>
        public string ESBL { get; set; }

        /// <summary>
        /// mrsa结果
        /// </summary>
        public string MRSA { get; set; }

        /// <summary>
        /// vre结果
        /// </summary>
        public string VRE { get; set; }

        /// <summary>
        /// MLS_DTEST结果
        /// </summary>
        public string MLS_DTEST { get; set; }

        /// <summary>
        /// 检出菌whonet码，3位
        /// </summary>
        public string OrgWhoCode { get; set; }

        /// <summary>
        /// 检出菌抗生素结果
        /// </summary>
        public List<EntityWhonetResult> Results { get; set; }
    }
}
