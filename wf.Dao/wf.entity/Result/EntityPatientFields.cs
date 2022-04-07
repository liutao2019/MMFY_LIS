using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace dcl.entity
{ 
    public class EntityPatientFields
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

    public class DclFieldsNameConventer<FieldsDefine> where FieldsDefine : class
    {
        static object objLock = new object();
        private static DclFieldsNameConventer<FieldsDefine> _instance = null;
        public static DclFieldsNameConventer<FieldsDefine> Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DclFieldsNameConventer<FieldsDefine>();
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
                    return fieldInfo.GetValue(new EntityPatientFields()).ToString();
                }
            }

            return string.Empty;
        }

        public DclFieldsNameConventer()
        {
            fis = typeof(FieldsDefine).GetFields();
        }

        private FieldInfo[] fis;
    }
}
