using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dcl.client.common;

namespace dcl.client.notifyclient
{
    /// <summary>
    /// LED内部通知
    /// </summary>
    public class LEDNotifyHelper
    {
        /// <summary>
        ///  LED内部通知
        /// </summary>
        private FrmShowLEDMsg frmShowLed= null;

        #region Instance
        private static LEDNotifyHelper _instance = null;

        private static object instanceLock = new object();

        public static LEDNotifyHelper Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new LEDNotifyHelper();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

         /// <summary>
        /// 启动
        /// </summary>
        public void start()
        {
            //检验是否启动LED内部通知
            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("LEDMSG_In_IsNotify") == "是")
            {
                if (frmShowLed == null)
                {
                    frmShowLed = new dcl.client.notifyclient.FrmShowLEDMsg();
                    frmShowLed.startShowFrm();//开启
                }
            }
            else
            {
                this.stop();
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void stop()
        {
            if (frmShowLed != null)
            {
                frmShowLed.StriogClose();//关闭并释放资源
                frmShowLed = null;//置空
            }
        }
    }
}
