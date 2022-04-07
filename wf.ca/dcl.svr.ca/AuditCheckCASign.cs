using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using dcl.entity;

namespace dcl.svr.ca
{
    public class AuditCheckCASign : AbstractCheckCASignClass, ICheckCASign
    {
        private EntityPidReportMain pat_info;

        private List<EntityObrResult> resultList;

        private List<EntityObrResultBact> resultBactList;

        private List<EntityObrResultDesc> resultDescList;

        private List<EntityObrResultAnti> resultAntiList;

        public AuditCheckCASign(EntityPidReportMain p_pat_info, List<EntityObrResult> p_resulto)
        {
            this.pat_info = p_pat_info;
            this.resultList = p_resulto;
        }

        public AuditCheckCASign(EntityPidReportMain reportMain, List<EntityObrResultBact> bactList, List<EntityObrResultDesc> descList, List<EntityObrResultAnti> antiList)
        {
            this.pat_info = reportMain;
            this.resultBactList = bactList;
            this.resultDescList = descList;
            this.resultAntiList = antiList;
        }

        /// <summary>
        /// 拼接签名字符串
        /// </summary>
        /// <returns></returns>
        public string CASignContentSplice()
        {
            StringBuilder content = new StringBuilder();
            if (pat_info != null)
            {
                content.Append("姓名：" + pat_info.PidName);
                content.Append(",性别：" + pat_info.PidSexName);
                content.Append(",年龄：" + pat_info.PidAgeStr);
                content.Append(",检验项目：" + pat_info.PidComName);
                content.Append(",科室：" + pat_info.PidDeptName);
                content.Append(",床号：" + pat_info.PidBedNo);
                content.Append(",标本类别：" + pat_info.SamName);
                content.Append(",标本状态：" + pat_info.BcStatus);
                content.Append(",诊断：" + pat_info.PidDiag);
                content.Append(",病历号：" + pat_info.PidInNo);
                content.Append(",样本号：" + pat_info.RepSid);
                content.Append(",送检医师：" + pat_info.DoctorName);
                content.Append(",检验时间：" + pat_info.SampCheckDate);
                content.Append(",检验师：" + pat_info.RepCheckUserName);
                content.Append(",审核医师：" + pat_info.RepAuditUserName);
                content.Append(",报告时间：" + pat_info.RepReportDate);
            }

            content.Append("；检验结果：");
            if (resultList != null)
            {
                foreach (var entityResult in resultList)
                {
                    content.AppendFormat("项目ID:{0}, 项目代码:{1}, 结果:{2};", entityResult.ItmId, entityResult.ItmEname, entityResult.ObrValue);
                }
                content.Remove(content.Length - 1, 1);
            }
            else
            {
                if(resultBactList != null && resultBactList.Count > 0)
                {

                    foreach (var bact in resultBactList)
                    {
                        content.Append("细菌名:" + bact.BacCname);
                    }
                }
                if (resultDescList != null && resultDescList.Count > 0)
                {

                    foreach (var desc in resultDescList)
                    {
                        content.Append("描述内容:" + desc.ObrDescribe);
                    }
                }
                if (resultAntiList != null && resultAntiList.Count > 0)
                {

                    foreach (var anti in resultAntiList)
                    {
                        content.Append("抗生素名称:" + anti.AntCname + " " + anti.ObrValue);
                    }
                }
            }

            return content.ToString();
        }
    }

}