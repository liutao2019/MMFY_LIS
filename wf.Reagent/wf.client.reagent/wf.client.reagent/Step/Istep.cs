using dcl.client.common;
using dcl.client.wcf;
using dcl.common;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wf.client.reagent
{
    public abstract class Istep
    {
        /// <summary>
        /// 步骤代码
        /// </summary>
        public abstract string StepCode { get; }
        /// <summary>
        /// 名称
        /// </summary>
        public abstract string StepName { get; }

        public virtual IAudit Audit
        {
            get
            {
                if (ConfigHelper.GetSysConfigValueWithoutLogin("Interface_PwdMode") != "通用")
                {
                    return new HISAudit();
                }
                else
                {
                    return new LisAudit();
                }
            }
        }
    }

    public abstract class IAudit
    {
        public bool ShouldAuditWhenPrint { get; set; }

        public AuditInfo AuditWhenPrint(AuditInfo userInfo)
        {
            if (this.ShouldAuditWhenPrint)
            {
                return AuditWhenPrintImpl(userInfo);
            }
            else
                return null;
        }

        protected virtual AuditInfo AuditWhenPrintImpl(AuditInfo userInfo)
        {
            return null;
        }

    }

    public class HISAudit : IAudit
    {
        protected override AuditInfo AuditWhenPrintImpl(AuditInfo userInfo)
        {
            ProxySMHisInterfaces proxyUser = new ProxySMHisInterfaces();

            AuditInfo info = proxyUser.Service.HisUserAudit(userInfo);
            if (info == null)
            {
                IAudit lisAudit = new LisAudit();
                lisAudit.ShouldAuditWhenPrint = true;
                info = lisAudit.AuditWhenPrint(userInfo);
            }

            return info;
        }
    }

    public class OutlinkAudit : IAudit
    {
        protected override AuditInfo AuditWhenPrintImpl(AuditInfo userInfo)
        {
            string result = Outlink.VerifyStaff(Outlink.GenerateAuditInfo(userInfo));
            string[] strResult = result.Split(';');

            if (strResult[2].IndexOf("0") >= 0)
                return null;

            userInfo.UserName = strResult[1].Split('=')[1];
            userInfo.UserStfId = strResult[0].Split('=')[1];

            return userInfo;
        }
    }

    public class LisAudit : IAudit
    {
        protected override AuditInfo AuditWhenPrintImpl(AuditInfo userInfo)
        {
            EntityUserQc userQc = new EntityUserQc();

            userQc.LoginId = userInfo.UserId;
            userQc.Password = EncryptClass.Encrypt(userInfo.Password);

            ProxySysUserInfo proxyUser = new ProxySysUserInfo();
            List<EntitySysUser> listUser = proxyUser.Service.SysUserQuery(userQc);
            if (listUser.Count > 0)
            {
                AuditInfo auditInfo = new AuditInfo();
                auditInfo.UserId = listUser[0].UserLoginid;
                auditInfo.UserName = listUser[0].UserName;
                auditInfo.UserStfId = listUser[0].UserIncode;
                return auditInfo;
            }
            else
                return null;
        }

    }

}
