using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace dcl.entity
{
    /// <summary>
    /// 院网接口病人信息
    /// </summary>
    [Serializable]
    public class InterfacePatientInfo
    {
        public NetInterfaceType InterfaceType { get; set; }

        public InterfacePatientInfo()
        {
            this.PatientMi = new List<EntityPatientsMi_4Barcode>();
            InterfaceType = NetInterfaceType.Interface;

            this.Name = string.Empty;
            this.Sex = "0";

            this.DateSampleCollect = null;
            this.DateSended = null;
            this.DateReceived = null;

            this.BarCode = string.Empty;
        }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        ///// <summary>
        ///// 年龄，单位：分钟
        ///// </summary>
        //public int? Age
        //{
        //    get
        //    {
        //        return AgeConverter.AgeValueTextToMinute(AgeValue);
        //    }
        //}

        ///// <summary>
        ///// 年龄显示格式：xx年xx月xx天...
        ///// </summary>
        //public string AgeText
        //{
        //    get
        //    {
        //        if (AgeValue == null)
        //        {
        //            return string.Empty;
        //        }
        //        else
        //        {
        //            string str = string.Empty;
        //            str = AgeConverter.TrimZeroValue(AgeValue);
        //            str = AgeConverter.ValueToText(AgeValue);
        //            return str;
        //        }
        //    }
        //}

        public int bc_print_flag { get; set; }


        /// <summary>
        /// 年龄存储格式：YMDHm
        /// </summary>
        public string AgeValue { get; set; }

        /// <summary>
        /// 性别，1男 2女 0未知
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 性别显示名称
        /// </summary>
        public string SexText
        {
            get
            {
                if (Sex == "1")
                {
                    return "男";
                }
                else if (Sex == "2")
                {
                    return "女";
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// 标本类别ID
        /// </summary>
        public string SampleID { get; set; }

        /// <summary>
        /// 标本类别名称
        /// </summary>
        public string SampleName { get; set; }

        /// <summary>
        /// 检查类型，Lis对应ID
        /// </summary>
        public string CheckType { get; set; }

        /// <summary>
        /// 送检科室编码
        /// </summary>
        public string SenderDeptCode { get; set; }

        /// <summary>
        /// 送检部门名称
        /// </summary>
        public string SenderDeptName { get; set; }

        /// <summary>
        /// 病区编码
        /// </summary>
        public string WardCode { get; set; }

        /// <summary>
        /// 病区名称
        /// </summary>
        public string WardName { get; set; }

        /// <summary>
        /// 送检者ID
        /// </summary>
        public string SenderID { get; set; }

        /// <summary>
        /// 送检者名称
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 病床号
        /// </summary>
        public string BedNumber { get; set; }

        /// <summary>
        /// 临床诊断ID
        /// </summary>
        public string DiagID { get; set; }

        /// <summary>
        /// 临床诊断名称
        /// </summary>
        public string DiagText { get; set; }

        /// <summary>
        /// 检查目的ID
        /// </summary>
        public string CheckPurposeID { get; set; }

        /// <summary>
        /// 检查目的名称
        /// </summary>
        public string CheckPurposeText { get; set; }

        /// <summary>
        /// 医嘱执行时间
        /// </summary>
        public DateTime? DateApply { get; set; }


        /// <summary>
        /// 采样时间
        /// </summary>
        public DateTime? DateSampleCollect { get; set; }


        /// <summary>
        /// 送检时间
        /// </summary>
        public DateTime? DateSended { get; set; }


        /// <summary>
        /// 送达时间
        /// </summary>
        public DateTime? DateReach { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime? DateReceived { get; set; }

        /// <summary>
        /// 条码号
        /// </summary>
        public string BarCode { get; set; }

        /// <summary>
        /// 费用类别
        /// </summary>
        public string FeeType { get; set; }

        /// <summary>
        /// 标本备注ID
        /// </summary>
        public string SamRem { get; set; }

        /// <summary>
        /// 标本备注名称
        /// </summary>
        public string SamRemName { get; set; }

        /// <summary>
        /// 条码状态
        /// </summary>
        public string BarcodeStatus { get; set; }

        /// <summary>
        /// 病人来源(id)
        /// </summary>
        public string Ori_id { get; set; }

        /// <summary>
        /// 病人来源(名称)
        /// </summary>
        public string Ori_name { get; set; }

        /// <summary>
        /// 病人id类型
        /// </summary>
        public string PatientsIDType { get; set; }

        /// <summary>
        /// 病人id类型
        /// </summary>
        public string PatientsID { get; set; }

        /// <summary>
        /// 体检id
        /// </summary>
        public string pat_emp_id { get; set; }

        /// <summary>
        /// 医保卡号/诊疗卡号
        /// </summary>
        public string SocialNo { get; set; }

        /// <summary>
        /// 自定义ID号
        /// </summary>
        public string PatPid { get; set; }

        /// <summary>
        /// 病人唯一号UPID
        /// </summary>
        [System.ComponentModel.Description("病人唯一号UPID")]
        public string PatUPID { get; set; }



        public string PatIdentity { get; set; }

        /// <summary>
        /// 申请号
        /// </summary>
        public string PatAppNo { get; set; }

        /// <summary>
        /// 体检单位名称
        /// </summary>
        public string pat_emp_company_name { get; set; }

        /// <summary>
        /// 就诊次数
        /// </summary>
        public int AdmissTimes { get; set; }

        #region 辅助信息
        /// <summary>
        /// 职业
        /// </summary>
        public string Job { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        public string PlaceOfWork { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// EMail
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// 体重
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// 出生日期--扩展信息
        /// </summary>
        public DateTime? birthday { get; set; }
        #endregion



        public List<EntityPatientsMi_4Barcode> PatientMi { get; set; }
    }
}
