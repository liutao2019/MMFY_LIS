using System;
using System.Collections.Generic;

using System.Text;
using System.Reflection;

namespace dcl.entity
{
    public class FieldsNameConventer<FieldsDefine> where FieldsDefine : class
    {
        static object objLock = new object();
        private static FieldsNameConventer<FieldsDefine> _instance = null;
        public static FieldsNameConventer<FieldsDefine> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new FieldsNameConventer<FieldsDefine>();
                        }
                    }
                }
                return _instance;
            }
        }

        public string GetFieldCHS(string fieldname)
        {
            foreach (FieldInfo fieldInfo in fis)
            {
                if (fieldInfo.Name == fieldname)
                {
                    return fieldInfo.GetValue(new PatientFields()).ToString();
                }
            }

            return string.Empty;
        }
        public string GetReaFieldCHS(string fieldname)
        {
            foreach (FieldInfo fieldInfo in fis)
            {
                if (fieldInfo.Name == fieldname)
                {
                    return fieldInfo.GetValue(new ReaApplyFields()).ToString();
                }
            }

            return string.Empty;
        }
        public string GetDataFieldCHS<T>(string fieldname) where T : new()
        {
            foreach (FieldInfo fieldInfo in fis)
            {
                if (fieldInfo.Name == fieldname)
                {
                    return fieldInfo.GetValue(new T()).ToString();
                }
            }

            return string.Empty;
        }

        public FieldsNameConventer()
        {
            fis = typeof(FieldsDefine).GetFields();
        }

        private FieldInfo[] fis;
    }

    public class PatientFields
    {
        public string RepSid = "样本号";
        public string PidName = "姓名";
        public string PidSex = "性别";
        public string PidAgeExp = "年龄";
        public string PidIdtId = "病人ID类型";

        public string PidInNo = "病人ID";
        public string PidBedNo = "床号";
        public string PidComName = "组合名称";
        public string PidDiag = "临床诊断";
        public string PidRemark = "标本状态";

        public string PidWork = "职业";
        public string PidTel = "联系电话";
        public string PidEmail = "邮箱";
        public string PidUnit = "单位";
        public string PidAddress = "地址";

        public string PidPreWeek = "孕周";
        public string PidHeight = "身高";
        public string PidWeight = "体重";
        public string PidSamId = "样本类型ID";
        public string PidPurpId = "检查目的";

        public string PidDoctorCode = "送检医生";
        public string RepCheckUserId = "检验者";
        public string RepAuditUserId = "审核者";
        public string RepCtype = "检验类别";

        public string RepRemark = "备注";
        public string SampSendDate = "送检日期";
        public string RepAuditDate = "审核日期";
        public string RepReportDate = "报告日期";
        public string PidSocialNo = "HIS号";

        public string RepBarCode = "条码号";
        public string SampCollectionDate = "采集时间";
        public string SampCheckDate = "检验时间";
        public string SampApplyDate = "接收时间";
        public string CollectionPart = "采集部位";

        public string PidSrcId = "病人来源";
        public string RepItrAnalysis = "仪器信息";
        public string RepComment = "处理意见";
        
    }

    public class ReaApplyFields
    {
        public string Ray_no = "申领单号";

        public string Ray_applier = "申领人";
        public string Ray_auditor = "审核人";

        public string Ray_remark = "备注";
        public string Ray_applydate = "申领日期";
        public string Ray_auditdate = "审核日期";

    }

    public class ReaPurchaseFields
    {
        public string Rpc_no = "采购单号";

        public string Rpc_applier = "采购人";
        public string Rpc_auditor = "审核人";

        public string Rpc_remark = "备注";
        public string Rpc_applydate = "采购日期";
        public string Rpc_auditdate = "审核日期";

    }

    public class ReaSubscribeFields
    {
        public string Rsb_no = "申购单号";

        public string Rsb_applier = "申购人";
        public string Rsb_auditor = "审核人";

        public string Rsb_remark = "备注";
        public string Rsb_applydate = "申购日期";
        public string Rsb_auditdate = "审核日期";

    }

    public class ReaStorageFields
    {
        public string Rsr_no = "入库单号";

        public string Rsr_operator = "入库人";
        public string Rsr_auditor = "审核人";

        public string Rsr_remark = "备注";
        public string Rsr_date = "入库日期";
        public string Rsr_auditdate = "审核日期";

    }

    public class ReaDeliveryFields
    {
        public string Rdl_no = "出库单号";

        public string Rdl_operator = "出库人";
        public string Rdl_auditor = "审核人";

        public string Rdl_remark = "备注";
        public string Rdl_date = "出库日期";
        public string Rdl_auditdate = "审核日期";

    }

    public class ReaLossReportFields
    {
        public string Rlr_no = "报损单号";

        public string Rlr_operator = "报损人";
        public string Rlr_auditor = "审核人";

        public string Rlr_remark = "备注";
        public string Rlr_date = "报损日期";
        public string Rlr_auditdate = "审核日期";

    }
}
