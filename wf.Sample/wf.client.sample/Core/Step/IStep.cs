using System;
using System.Collections.Generic;
using DevExpress.XtraGrid.Views.Grid;
using dcl.client.common;
using dcl.common;
using dcl.client.wcf;
using System.Drawing;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.sample
{
    /// <summary>
    /// 条码流程
    /// </summary>
    public abstract class IStep
    {
        protected IStep preStep;
        protected IStep nextStep;
        private IStepController stepController = new CoolStepController();

        /// <summary>
        /// 条码批次
        /// </summary>
        public string Bcfrequency { get; set; }

        public delegate void EnabledBarcodeInput();
        public event EnabledBarcodeInput enabledBarcodeInput;
        public delegate void EnlableSimpleSearchPanel();
        public event EnlableSimpleSearchPanel enlableSimpleSearchPanel;
        public Int32 FowardMinutes { get; set; }
        public bool EnabledFowardMinutes { set; get; }
        public virtual IStepController StepController { get { return stepController; } set { stepController = value; } }

        /// <summary>
        /// 只更新状态
        /// </summary>
        public virtual bool OnlyUpdateStatus { get { return false; } }
        public bool MustFinishPreviousAction
        {
            get
            {
                if (StepController != null)
                    return StepController.MustFinishPreviousAction;
                else
                    return false;
            }
            set
            {
                if (StepController != null)
                    StepController.MustFinishPreviousAction = value;
            }
        }

        /// <summary>
        /// 是否显示标本查询面板
        /// </summary>
        public virtual bool ShouldShowSimpleSearchPanel { get { return false; } }

        private bool _ShouldEnlableSimpleSearchPanel = true;

        /// <summary>
        /// 标本查询面板是否可用
        /// </summary>
        public virtual bool ShouldEnlableSimpleSearchPanel
        {
            get
            {
                return _ShouldEnlableSimpleSearchPanel;
            }
            set
            {
                _ShouldEnlableSimpleSearchPanel = value;
                if (enlableSimpleSearchPanel != null)
                {
                    enlableSimpleSearchPanel();
                }
            }
        }


        private bool _ShouldEnabledBarcodeInput = true;

        /// <summary>
        /// 是否能录入条码
        /// </summary>
        public bool ShouldEnabledBarcodeInput
        {
            get
            {
                return _ShouldEnabledBarcodeInput;
            }
            set
            {
                _ShouldEnabledBarcodeInput = value;
                if (enabledBarcodeInput != null)
                {
                    enabledBarcodeInput();
                }
            }
        }


        public bool ShouldDoAction
        {
            get
            {
                if (StepController != null)
                    return StepController.ShouldDoAction;
                else
                    return false;
            }
            set
            {
                if (StepController != null)
                    StepController.ShouldDoAction = value;
            }
        }

        public virtual IAudit Audit
        {
            get
            {
                if (BarcodeClientHelper.IsNormal())
                {
                    if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Interface_HospitalInterfaceMode") != "通用")
                    {
                        return new HISAudit();
                    }
                    else
                    {
                        return new LisAudit();
                    }
                }
                else
                {
                    return new OutlinkAudit();
                }
            }
        }
        public IPrint Printer { get; set; }

        public bool UpdateStatus(EntitySampOperation operation)
        {
            List<EntitySampMain> list = new List<EntitySampMain>();
            list.Add(BaseSampMain);
            return UpdateStatus(operation, list);
        }

        /// <summary>
        /// 新更新状态操作。
        /// </summary>
        /// <param name="signInfo"></param>
        /// <param name="dataRowView"></param>
        /// <returns></returns>
        internal bool UpdateStatus(EntitySampOperation operationInfo, List<EntitySampMain> barcodes)
        {
            operationInfo.OperationStatus = StepCode;
            operationInfo.OperationStatusName = StepName;
            operationInfo.OperationTime = GetServerTime();

            if (this.StepName != "条码查询")
            {
                ProxySampMain proxy = new ProxySampMain();

                //跟新条码状态
                bool result = proxy.Service.UpdateSampMainStatus(operationInfo, barcodes);

                return result;

            }
            else
            {
                ProxySampProcessDetail proxy = new ProxySampProcessDetail();
                foreach (EntitySampMain item in barcodes)
                {
                    proxy.Service.SaveSampProcessDetail(operationInfo, item);
                }

                return true;
            }
        }

        internal bool ComfirmAll(EntitySampOperation operationInfo, List<EntitySampMain> barcodes)
        {
            operationInfo.OperationStatus = StepCode;
            operationInfo.OperationStatusName = StepName;

            ProxySampMain proxy = new ProxySampMain();

            //跟新条码状态
            bool result = proxy.Service.UpdateSampMainStatus(operationInfo, barcodes);

            Int64 batchNo = 0;

            //更新条码批次
            if (this.StepName == "采集" && !string.IsNullOrEmpty(Bcfrequency) && Int64.TryParse(this.Bcfrequency, out batchNo))
                proxy.Service.UpdateSampMainBatchNo(batchNo, barcodes);

            return result;
        }

        /// <summary>
        /// 格式化GridView
        /// </summary>
        /// <param name="gvBarcode"></param>
        public virtual void FormatRow(GridView gvBarcode)
        {
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.BarcodeGenerate.ToString(), GetBarcodeConfigColor("Barcode_Color_Downloaded"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.BarcodePrint.ToString(), GetBarcodeConfigColor("Barcode_Color_Printed"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.SampleCollect.ToString(), GetBarcodeConfigColor("Barcode_Color_Blooded"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.SampleSend.ToString(), GetBarcodeConfigColor("Barcode_Color_Sended"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.SampleReach.ToString(), GetBarcodeConfigColor("Barcode_Color_Reach"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.SampleReceive.ToString(), GetBarcodeConfigColor("Barcode_Color_Received"));

            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.DeleteDetail.ToString(), GetBarcodeConfigColor("Barcode_Color_Downloaded"));

            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.SampleRegister.ToString(), GetBarcodeConfigColor("Barcode_Color_Received"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.Audit.ToString(), GetBarcodeConfigColor("Barcode_Color_Received"));

            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.UndoAudit.ToString(), GetBarcodeConfigColor("Barcode_Color_Received"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.Report.ToString(), GetBarcodeConfigColor("Barcode_Color_Reported"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.UndoReport.ToString(), GetBarcodeConfigColor("Barcode_Color_Received"));

            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.SampleReturn.ToString(), GetBarcodeConfigColor("Barcode_Color_Printed"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.SampleSecondSend.ToString(), GetBarcodeConfigColor("Barcode_Color_Received"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.AppendBarcode.ToString(), GetBarcodeConfigColor("Barcode_Color_Received"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", EnumBarcodeOperationCode.DeletePatient.ToString(), GetBarcodeConfigColor("Barcode_Color_Received"));
            FormatHelper.FormatRow(gvBarcode, "SampStatusId", "30", GetBarcodeConfigColor("Barcode_Color_Received"));


            FormatHelper.FormatRowBarcode(gvBarcode, "SampUrgentFlag", true, Color.FromArgb(64, 224, 208));
            FormatHelper.FormatRowIdentity(gvBarcode, "PidIdentityName", "临床路径", Color.FromArgb(0, 255, 0));
            //FormatHelper.FormatRowBarcode(gvBarcode, "DelFlag", true, System.Drawing.Color.Gold);
        }

        public static Color GetBarcodeConfigColor(string configCode)
        {
            string cfgValue = ConfigHelper.GetSysConfigValueWithoutLogin(configCode);
            string ColorStyle = ConfigHelper.GetSysConfigValueWithoutLogin("BarcodeInfo_ColorStyle");
            Color col;
            if (ColorStyle == "默认")
            {
                switch (cfgValue)
                {
                    case "黑色":
                        col = Color.White;
                        break;

                    case "红色":
                        col = Color.Pink;
                        break;

                    case "灰色":
                        col = Color.LightGray;
                        break;

                    case "蓝色":
                        col = Color.LightSkyBlue;
                        break;

                    case "绿色":
                        col = Color.SpringGreen;
                        break;

                    case "紫色":
                        col = Color.Violet;
                        break;

                    case "棕色":
                        col = Color.Tan;
                        break;

                    default:
                        col = Color.Black;
                        break;
                }
            }
            else if (ColorStyle == "整行字体")
            {
                //黑色,红色,灰色,蓝色,绿色,紫色
                switch (cfgValue)
                {
                    case "黑色":
                        col = Color.Black;
                        break;

                    case "红色":  //打印
                        col = Color.Red; //原色
                        break;

                    case "灰色": //报告
                        col = Color.Gray;
                        break;

                    case "蓝色": //签收
                        col = Color.Blue; //原色
                        break;

                    case "绿色": //采集
                        col = Color.Green; //原色
                        break;

                    case "紫色": //收取
                        col = Color.Fuchsia;  //原色
                        break;

                    case "棕色": //送达
                        col = Color.Sienna; //原色
                        break;

                    default:
                        col = Color.Black;
                        break;
                }
            }
            else
            {
                //黑色,红色,灰色,蓝色,绿色,紫色
                switch (cfgValue)
                {
                    case "黑色":
                        col = Color.White;
                        break;

                    case "红色":  //打印
                        col = Color.FromArgb(255, 147, 192);
                        break;

                    case "灰色": //报告
                        col = Color.FromArgb(190, 190, 190);
                        break;

                    case "蓝色": //签收
                        col = Color.FromArgb(98, 208, 254);
                        break;

                    case "绿色": //采集
                        col = Color.FromArgb(146, 255, 92);
                        break;

                    case "紫色": //收取
                        col = Color.FromArgb(158, 145, 254);
                        break;

                    case "棕色": //送达
                        col = Color.FromArgb(253, 179, 65);
                        break;

                    default:
                        col = Color.Black;
                        break;
                }
            }
            return col;

        }

        /// <summary>
        /// 是否可以发送信息
        /// </summary>
        public virtual bool CanSendMsg
        {
            get { return false; }
        }

        /// <summary>
        /// 上一个流程
        /// </summary>
        public virtual IStep PreStep
        {
            get
            {
                if (preStep == null)
                {
                    preStep = GetPreStep();
                    if (preStep != null)
                        preStep.BaseSampMain = BaseSampMain;
                }
                return preStep;
            }
        }

        public virtual string TimeOutString { get { return ""; } }

        public DateTime TimeOutValue { get; set; }

        /// <summary>
        /// 条码信息
        /// </summary>
        public EntitySampMain BaseSampMain { get; set; }

        /// <summary>
        /// 步骤代码
        /// </summary>
        public abstract string StepCode { get; }

        /// <summary>
        /// 生成前一个步骤
        /// </summary>
        /// <returns></returns>
        public IStep GetPreStep()
        {
            return StepFactory.CreatePrevStep(this);
        }

        /// <summary>
        /// 名称
        /// </summary>
        public abstract string StepName { get; }
        /// <summary>
        /// 是否没有完成前一流程
        /// </summary>
        /// <returns></returns>
        public virtual bool HasNotDoPreAction()
        {
            if (PreStep == null)
                return false;
            PreStep.BaseSampMain = BaseSampMain;
            return PreStep.HasNotDoneAction();
        }

        /// <summary>
        /// 是否完成当前的流程
        /// </summary>
        public bool HasDoneAction()
        {
            return !HasNotDoneAction();
        }

        public virtual bool HasSigned()
        {
            return BaseSampMain.SampStatusId == StatusHelper.Signed;
        }

        /// <summary> 是否未完成当前的流程 </summary>     
        public abstract bool HasNotDoneAction();

        /// <summary> 没有完成上一流程的提醒 </summary>
        public virtual string WarnForNotPrepare { get { return "该条码没有" + PreStep.StepName + BarcodeMoreInfo; } }

        /// <summary> 超时的提醒 </summary>
        public virtual string WarnForTimeOut { get { return "该条码离" + PreStep.StepName + "时间已经超过规定1小时,是否继续?" + BarcodeMoreInfo; } }

        /// <summary> 已经完成当前流程的提醒 </summary>
        public virtual string WarnForHasDone { get { return "该条码已经" + StepName + BarcodeMoreInfo; } }

        /// <summary> 需要签收的提醒 </summary>
        public string WarnForNeedSign { get { return "该条码需要" + StatusHelper.SignedName + BarcodeMoreInfo; } }

        /// <summary> 需要全部项目上机的提醒 </summary>
        public string WarnForAllSentToMachine { get { return "该条码项目已经全部上机" + BarcodeMoreInfo; } }

        private string BarcodeMoreInfo
        {
            get
            {
                return string.Format(@"

条码号:{0}  

姓名:{1}  性别:{2}  年龄:{3}

项目:{4}", BaseSampMain.SampBarCode, BaseSampMain.PidName, BaseSampMain.PidSex, BaseSampMain.SampAge, BaseSampMain.SampComName);
            }
        }

        #region Private Method

        public static DateTime GetServerTime()
        {
            return ServerDateTime.GetServerDateTime();
        }

        #endregion

        internal virtual bool NeedSigned()
        {
            return false;
        }

        internal virtual bool NeedCheckSentToMachine()
        {
            return false;
        }

        internal virtual bool HasAllSentToMachine()
        {
            return true;
        }
    }

    /// <summary>
    /// 流程控制：基本控制，严格控制
    /// </summary>
    public abstract class IStepController
    {
        public abstract bool MustFinishPreviousAction { get; set; }
        public virtual bool ShouldDoAction { get { return true; } set {; } }
    }

    public class BaseStepController : IStepController
    {

        public override bool MustFinishPreviousAction
        {
            get
            {
                return false;
            }
            set
            {

            }
        }

        public override bool ShouldDoAction
        {
            get
            {
                return false;
            }
            set
            {
                base.ShouldDoAction = value;
            }
        }
    }

    public class CoolStepController : IStepController
    {

        public override bool MustFinishPreviousAction
        {
            get
            {
                return true;
            }
            set
            {

            }
        }
    }

    public class NoStepController : IStepController
    {

        public override bool MustFinishPreviousAction
        {
            get
            {
                return false;
            }
            set
            {

            }
        }

        public override bool ShouldDoAction
        {
            get
            {
                return false;
            }
            set
            {
                base.ShouldDoAction = value;
            }
        }

    }



    public class StatusHelper
    {
        public static string Signed = "5";
        public static string SignedName = "签收";

    }



    /// <summary>
    /// 二次送检
    /// </summary>
    public class SecondSendStep : IStep
    {
        internal override bool NeedSigned()
        {
            return true;
        }

        internal override bool HasAllSentToMachine()
        {
            return false;
            //如果每个项目的上机标志都有
            //如果没有项目，报错            
            //return HasAllSendToMachine(BaseSampMain.ListSampDetail);
        }

        public override string StepCode
        {
            get { return "8"; }
        }

        public override string StepName
        {
            get { return "二次送检"; }
        }

        public override bool HasNotDoneAction()
        {
            return false;
        }

        public override IStepController StepController
        {
            get
            {
                return new NoStepController();
            }
            set
            {
                base.StepController = new NoStepController();
            }
        }

        public override bool ShouldShowSimpleSearchPanel
        {
            get
            {
                return true;
            }
        }


        internal override bool NeedCheckSentToMachine()
        {
            return true;
        }


        public override bool OnlyUpdateStatus
        {
            get
            {
                return false;
            }
        }

        public override bool CanSendMsg
        {
            get
            {
                return true;
            }
        }

        public override IAudit Audit
        {
            get
            {
                return base.Audit;
            }
        }
    }


    /// <summary>
    /// 回退
    /// </summary>
    public class ReturnStep : IStep
    {
        public override string StepCode
        {
            get { return "9"; }
        }

        public override string StepName
        {
            get { return "回退"; }
        }

        public override bool HasNotDoneAction()
        {
            return false;
        }

        public override bool OnlyUpdateStatus
        {
            get
            {
                return true;
            }
        }

        public override IStepController StepController
        {
            get
            {
                return new NoStepController();
            }
            set
            {
                base.StepController = new NoStepController();
            }
        }
    }

    /// <summary>
    /// 离心
    /// </summary>
    public class CentrifugateStep : IStep
    {
        public override string StepCode
        {
            get { return "6"; }
        }

        public override string StepName
        {
            get { return "离心"; }
        }

        public override bool HasNotDoneAction()
        {
            return false;
        }

        public override bool ShouldShowSimpleSearchPanel
        {
            get
            {
                return true;
            }
        }
        public override bool OnlyUpdateStatus
        {
            get
            {
                return true;
            }
        }

        public override IStepController StepController
        {
            get
            {
                return new NoStepController();
            }
            set
            {
                base.StepController = new NoStepController();
            }
        }
    }

    /// <summary>
    /// 标本上机
    /// </summary>
    public class InLabStep : IStep
    {
        public override string StepCode
        {
            get { return "7"; }
        }

        public override string StepName
        {
            get { return "标本上机"; }
        }

        public override bool HasNotDoneAction()
        {
            return false;
        }

        public override bool OnlyUpdateStatus
        {
            get
            {
                return true;
            }
        }
        public override bool ShouldShowSimpleSearchPanel
        {
            get
            {
                return true;
            }
        }
        public override IStepController StepController
        {
            get
            {
                return new NoStepController();
            }
            set
            {
                base.StepController = new NoStepController();
            }
        }
    }

    /// <summary>
    /// 耗材领取
    /// </summary>
    public class RenStep : IStep
    {
        public override string StepCode
        {
            get { return "121"; }
        }

        public override string StepName
        {
            get { return "耗材领取"; }
        }

        public override bool HasNotDoneAction()
        {
            return false;
        }

        public override bool OnlyUpdateStatus
        {
            get
            {
                return true;
            }
        }
        public override bool ShouldShowSimpleSearchPanel
        {
            get
            {
                return true;
            }
        }
        public override IStepController StepController
        {
            get
            {
                return new NoStepController();
            }
            set
            {
                base.StepController = new NoStepController();
            }
        }
    }

    /// <summary>
    /// 核酸提取
    /// </summary>
    public class HSTQStep : IStep
    {

        public override string StepCode
        {
            get { return "5501"; }
        }

        public override string StepName
        {
            get { return "核酸提取"; }
        }

        public override bool HasNotDoneAction()
        {
            return false;
        }

        public override bool OnlyUpdateStatus
        {
            get
            {
                return true;
            }
        }
        public override bool ShouldShowSimpleSearchPanel
        {
            get
            {
                return false;
            }
        }
        public override IStepController StepController
        {
            get
            {
                return new NoStepController();
            }
            set
            {
                base.StepController = new NoStepController();
            }
        }
    }

    /// <summary>
    /// 核酸定量扩增
    /// </summary>
    public class HSDLKZStep : IStep
    {
        public override string StepCode
        {
            get { return "5502"; }
        }

        public override string StepName
        {
            get { return "核酸定量扩增"; }
        }

        public override bool HasNotDoneAction()
        {
            return false;
        }

        public override bool OnlyUpdateStatus
        {
            get
            {
                return true;
            }
        }
        public override bool ShouldShowSimpleSearchPanel
        {
            get
            {
                return false;
            }
        }
        public override IStepController StepController
        {
            get
            {
                return new NoStepController();
            }
            set
            {
                base.StepController = new NoStepController();
            }
        }
    }
    /// <summary>
    /// 标本交接
    /// </summary>
    public class HandOverStep : IStep
    {
        public override string StepCode
        {
            get { return "5504"; }
        }

        public override string StepName
        {
            get { return "标本交接"; }
        }

        public override bool HasNotDoneAction()
        {
            return false;
        }

        public override bool OnlyUpdateStatus
        {
            get
            {
                return true;
            }
        }
        public override bool ShouldShowSimpleSearchPanel
        {
            get
            {
                return false;
            }
        }
        public override IStepController StepController
        {
            get
            {
                return new NoStepController();
            }
            set
            {
                base.StepController = new NoStepController();
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
            userQc.Password = dcl.common.EncryptClass.Encrypt(userInfo.Password);

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
