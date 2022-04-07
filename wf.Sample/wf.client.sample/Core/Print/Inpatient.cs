using System;
using dcl.common;
using dcl.client.common;
using dcl.client.cache;

namespace dcl.client.sample
{
    /// <summary>
    /// 住院条码
    /// </summary>
    public class Inpatient : IPrint
    {

        #region IPatient Members

        public override DateTimeRange GetDefaultAdviceTime()
        {
            double dblStart = -2;
            double dblEnd = 1;
            try
            {
                //获取参数设置查询日期范围
                dblStart = Convert.ToDouble(ConfigHelper.GetSysConfigValueWithoutLogin("InBarcodeStartDownDate"));
                dblEnd = Convert.ToDouble(ConfigHelper.GetSysConfigValueWithoutLogin("InBarcodeEndDownDate")) + 1;
            }
            catch (Exception)
            {
                dblStart = -2;
                dblEnd = 1;

            }
            DateTime dtNow = ServerDateTime.GetServerDateTime();
            return new DateTimeRange(dtNow.Date.AddDays(dblStart), dtNow.Date.AddDays(dblEnd).AddSeconds(-1));
        }


        public override void Init()
        {
            Printablor.InpatientInit();
        }

        public override bool ShowSpecialComfirmWhenPrint()
        {
            if (ConfigHelper.GetSysConfigValueWithoutLogin("DummySampling") == "是")
            {
                FrmCheckTime frmCheckTime = FrmCheckTimeInstance;
                frmCheckTime.ShowDialog();
                if (frmCheckTime.Cannel == true)
                    return false;
                SignInfo signInfo = new SignInfo();
                signInfo.SignTime = frmCheckTime.SignTime.ToString(CommonValue.DateTimeLongFormat);
                this.SignInfo = signInfo;
            }
            else
            {
                SignInfo signInfo = new SignInfo();
                signInfo.SignTime = DateTime.Now.ToString(CommonValue.DateTimeLongFormat);
                this.SignInfo = signInfo;
            }
            return true;
        }
        #endregion

        public override LoadDataType LoadDataType
        {
            get
            {
                return LoadDataType.DownLoad;
            }
        }
        private FrmCheckTime frmCheckTime;
        public FrmCheckTime FrmCheckTimeInstance
        {
            get
            {
                if (frmCheckTime == null)
                    frmCheckTime = new FrmCheckTime(IStep.GetServerTime().AddMinutes(20));
                else
                    frmCheckTime.SetTime(IStep.GetServerTime().AddMinutes(20));

                return frmCheckTime;
            }
        }

        public override string Name
        {
            get { return "住院"; }
        }

        public override bool ShouldMergeCollect
        {
            get { return false; }
        }

        public override IAudit Audit
        {
            get
            {
                return new OutlinkAudit();
            }
        }

    }
}
