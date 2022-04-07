using System;
using System.Collections.Generic;
using dcl.client.frame;
using dcl.client.common;

namespace dcl.client.sample
{
    public partial class FrmBCSignIn : FrmCommon
    {
        public FrmBCSignIn()
        {
            InitializeComponent();
            if (DesignMode)
                return;

            UserInfo.SkipPower = false;
            allowSampling = UserInfo.HaveFunction(219);
            allowSend = UserInfo.HaveFunction(220);
            this.Activated += new EventHandler(FrmBCSignIn_Activated);
        }


        void FrmBCSignIn_Activated(object sender, EventArgs e)
        {
            bcConfirm4.ClearAndFocusBarcode();
        }



        bool isSpecVi = true;

        /// <summary>
        /// 禁用送达的用户权限
        /// </summary>
        bool stopValidReachFunc = false;
        public FrmBCSignIn(bool stopValidReachFunc, bool allowSampling, bool allowSend,bool allowReach)
        {
            InitializeComponent();
            isSpecVi = false;
            if (DesignMode)
                return;

            this.stopValidReachFunc = stopValidReachFunc;

            this.allowSampling = allowSampling;
            this.allowSend = allowSend;
            this.allowReach = allowReach;
        }


        public FrmBCSignIn(bool allowSampling, bool allowSend)
        {
            InitializeComponent();
            isSpecVi = false;
            if (DesignMode)
                return;

            this.allowSampling = allowSampling;
            this.allowSend = allowSend;
        }

        public FrmBCSignIn(bool allowSampling, bool allowSend, string doctName, string doctCode)
        {
            InitializeComponent();
            isSpecVi = false;
            if (DesignMode)
                return;

            this.allowSampling = allowSampling;
            this.allowSend = allowSend;
            if (allowSampling)
            {
                bcConfirm1.DoctorCode = doctCode;
                bcConfirm1.DoctorName = doctName;
            }
        }

        private bool allowSampling = false;
        private bool allowSend = false;
        private bool allowReach = false;

        private void FrmBCSignIn_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            this.bcConfirm1.StepType = dcl.client.sample.StepType.Sampling;
            this.bcConfirm8.StepType = dcl.client.sample.StepType.Ren;
            this.bcConfirm2.StepType = dcl.client.sample.StepType.Send;
            this.bcConfirm3.StepType = dcl.client.sample.StepType.Reach;
            this.bcConfirm4.StepType = dcl.client.sample.StepType.Confirm;
            this.bcConfirm5.StepType = dcl.client.sample.StepType.SecondSend;
            //this.bcConfirm6.StepType = dcl.client.sample.StepType.Centrifugate;
            this.bcConfirm7.StepType = dcl.client.sample.StepType.InLab;
            //this.bcConfirm9.StepType = dcl.client.sample.StepType.Hstq;
            //this.bcConfirm10.StepType = dcl.client.sample.StepType.Hsdlkz;
            this.bcConfirmHaveOver.StepType = dcl.client.sample.StepType.HandOver;

            InitTabFromConfig();

            if(!stopValidReachFunc)
                allowReach = UserInfo.HaveFunction(221);

            bool allowComfirm = UserInfo.HaveFunction(222);
            bool allowSeconSend = UserInfo.HaveFunction(247);

            bool allowRen = UserInfo.HaveFunctionByCodeForAll("FrmBC_EnableRen");
            bool allowHSTQ = UserInfo.HaveFunctionByCodeForAll("FrmBC_EnableHSTQ");//核酸提取
            bool allowHSDLKZ = UserInfo.HaveFunctionByCodeForAll("FrmBC_EnableHSDLKZ");//核酸定量扩增
            bool allowHaveOver = UserInfo.HaveFunctionByCodeForAll("FrmBC_EnableHaveOver");//标本交接


            if (ConfigHelper.GetSysConfigValueWithoutLogin("BarCode_ShowSpecialStep") == "是" && isSpecVi)
            {
                tabInLab.PageVisible = true;
            }
            else
            {
                tabInLab.PageVisible = false;
            }
            if (ConfigHelper.GetSysConfigValueWithoutLogin("Barcode_ReachConfirm") == "是")
            {
                tabComfirm.PageVisible = false;
            }
            else
            {
                tabComfirm.PageVisible = true;
            }
            if (!allowSampling)
            {
                tabSampling.PageVisible = false;
            }
            if (!allowSend)
                tabSend.PageVisible = false;

            if (!allowReach)
                tabReach.PageVisible = false;

            if (!allowComfirm)
                tabComfirm.PageVisible = false;

            if (!allowSeconSend)
                this.tabSecondReach.PageVisible = false;

            if (!allowRen || !isSpecVi)
                this.tabRen.PageVisible = false;

            if (!allowHaveOver || !isSpecVi)
                this.tabHaveOver.PageVisible = false;

            tabSampling.Focus();
            bcConfirm1.txtBarcode.Focus();

            bcConfirm1.ThisForm = this;
            bcConfirm2.ThisForm = this;
            bcConfirm3.ThisForm = this;
            bcConfirm4.ThisForm = this;
            bcConfirm5.ThisForm = this;
            bcConfirm7.ThisForm = this;
            bcConfirm8.ThisForm = this;
            bcConfirmHaveOver.ThisForm = this;
            tabSign.SelectedPageChanged += tabSign_SelectedPageChanged;
        }


        private void InitTabFromConfig()
        {
            InitBarcodePower();
        }

        private void InitBarcodePower()
        {
            System.Windows.Forms.Form fc = this.MdiParent;
            string leaveSamplingStep = UserInfo.GetSysConfigValue("8");
            string leaveSendStep = UserInfo.GetSysConfigValue("9");
            string leaveReachStep = UserInfo.GetSysConfigValue("10");
            List<IStep> stepList = new List<IStep>();
            stepList.Add(new PrintStep());
            if (leaveSamplingStep == "是")
                tabSampling.PageVisible = false;
            else
                stepList.Add(new SamplingStep());

            if (leaveSendStep == "是")
                tabSend.PageVisible = false;
            else
                stepList.Add(new SendStep());

            if (leaveReachStep == "是")
                tabReach.PageVisible = false;
            else
                stepList.Add(new ReachStep());

            stepList.Add(new ReceiveStep());
            StepFactory.StepList = stepList;
        }

        void tabSign_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (DesignMode)
                return;

            if (tabSign.SelectedTabPage == tabSampling)
            {
                bcConfirm1.TimeCountDown1ResetWhenZero();
            }
            if (tabSign.SelectedTabPage == tabSend)
            {
                bcConfirm2.TimeCountDown1ResetWhenZero();
            }
            if (tabSign.SelectedTabPage == tabReach)
            {
                bcConfirm3.TimeCountDown1ResetWhenZero();
            }
            if (tabSign.SelectedTabPage == tabComfirm)
            {
                bcConfirm4.TimeCountDown1ResetWhenZero();
            }
            if (tabSign.SelectedTabPage == tabSecondReach)
            {
                bcConfirm5.TimeCountDown1ResetWhenZero();
            }
            if (tabSign.SelectedTabPage == tabInLab)
            {
                bcConfirm7.TimeCountDown1ResetWhenZero();
            }
        }
    }
}