using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dcl.client.common;

namespace dcl.client.notifyclient
{
    public class BcSamplToReceiveTATNotifyHelper
    {

        /// <summary>
        /// TAT提醒(采集到签收)
        /// </summary>
        private FrmBcSamplToReceiveTATNotify frmSamplToReceiveTATNotify = null;


        #region Instance
        private static BcSamplToReceiveTATNotifyHelper _instance = null;

        private static object instanceLock = new object();

        public static BcSamplToReceiveTATNotifyHelper Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BcSamplToReceiveTATNotifyHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        public void start()
        {
            //系统配置：检验是否启动条码[采集_签收]TAT提醒
            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("BCSamplToReceive_TAT_IsNotify") == "是")
            {
                if (frmSamplToReceiveTATNotify == null)
                {
                    frmSamplToReceiveTATNotify = new dcl.client.notifyclient.FrmBcSamplToReceiveTATNotify();
                    frmSamplToReceiveTATNotify.startShowFrm();//开启
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void stop()
        {
            if (frmSamplToReceiveTATNotify != null)
            {
                frmSamplToReceiveTATNotify.StriogClose();//关闭并释放资源
                frmSamplToReceiveTATNotify = null;//置空
            }
        }
    }
}
