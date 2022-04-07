using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using dcl.client.common;

namespace dcl.client.notifyclient
{
    /// <summary>
    /// 危急值内部提醒
    /// </summary>
    public class UrgentNotifyHelper
    {
        /// <summary>
        ///  危急值内部提醒
        /// </summary>
        private FrmUrgentNotify frmUNotify = null;

        /// <summary>
        /// 仪器危急值提醒
        /// </summary>
        private FrmItrUrgentNotify frmItrNotify = null;

        /// <summary>
        /// 组合TAT提醒
        /// </summary>
        private FrmComTATNotify frmTATNotify = null;

        /// <summary>
        /// 仪器质控提醒
        /// </summary>
        private FrmQCNotify frmItrQcNotify = null;

        /// <summary>
        /// 条码TAT提醒
        /// </summary>
        private FrmBcComTATNotify frmBCComTATNotify = null;

        /// <summary>
        /// 条码TAT提醒列表显示
        /// </summary>
        //private FrmBcComTATNotifyNew frmBCComTATNotifyNew = null;

        #region Instance
        private static UrgentNotifyHelper _instance = null;

        private static object instanceLock = new object();

        public static UrgentNotifyHelper Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (instanceLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new UrgentNotifyHelper();
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
            ////检验是否启动危急值内部提醒
            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_In_IsNotify") == "是")
            {
                bool IsUrgentNotity = false;
                if (LocalSetting.Current.Setting.IsUrgentNotity == "0")
                {
                    IsUrgentNotity = false;
                }
                else if (LocalSetting.Current.Setting.IsUrgentNotity == "1")
                {
                    IsUrgentNotity = true;
                }
                else
                {
                    IsUrgentNotity = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_LocalSetting") == "是";
                }

                //是否允许本地启动危急值内部提醒功能
                if (IsUrgentNotity)
                {
                    if (frmUNotify == null)
                    {
                        int littleWinTime = 10;//危急值信息多少分钟后提示小窗口
                        int bigWintime = 20;//危急值信息多少分钟后提示大窗口
                        int jcShowTime = 60;//急查信息显示时间

                        //系统配置：危急值信息多少分钟后提示[小窗]
                        string StrLittleWin = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_A_NotifyDate");

                        //系统配置：危急值信息多少分钟后提示[大窗]
                        string StrBigWin = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_B_NotifyDate");

                        if (int.TryParse(StrLittleWin, out littleWinTime))
                        {
                            littleWinTime = littleWinTime <= 0 ? 10 : littleWinTime;

                            if (littleWinTime > 600)//如果大于10个小时,用回默认值10
                            {
                                littleWinTime = 10;
                            }
                        }
                        else
                        {
                            littleWinTime = 10;
                        }

                        if (int.TryParse(StrBigWin, out bigWintime))
                        {
                            bigWintime = bigWintime < 0 ? 20 : bigWintime;

                            if (bigWintime > 8200)//如果大于130个小时,用回默认值20
                            {
                                bigWintime = 20;
                            }
                        }
                        else
                        {
                            bigWintime = 20;
                        }
                        //系统配置：急查报告信息多少分钟后显示
                        string StrLittleWin2 = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_Report_NotifyDate");
                        if (int.TryParse(StrLittleWin2, out jcShowTime))
                        {
                            jcShowTime = jcShowTime <= 0 ? 14400 : jcShowTime;

                            if (jcShowTime > 6000)//如果大于100个小时,用最大值6000
                            {
                                jcShowTime = 6000;
                            }
                        }
                        else
                        {
                            jcShowTime = 60;
                        }

                        frmUNotify = new dcl.client.notifyclient.FrmUrgentNotify();
                        frmUNotify.LittleWinTime = littleWinTime;
                        frmUNotify.BigWinTime = bigWintime;
                        frmUNotify.JCShowTime = jcShowTime;
                        frmUNotify.startShowFrm();//开启
                    }
                }
                else
                {
                    if (frmUNotify != null)
                    {
                        frmUNotify.ClosePreLastShow();
                        frmUNotify.StriogClose();//关闭并释放资源
                        frmUNotify = null;//置空
                    }
                }
            }

            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_Instrmt_IsNotify") == "是")
            {
                bool IsItrUrgentNotity = false;//本地是否启动

                if (LocalSetting.Current.Setting.IsItrUrgentNotity == "1")
                {
                    IsItrUrgentNotity = true;
                }
                else
                {
                    IsItrUrgentNotity = false;
                }

                #region 检验是否启动仪器危急值提醒

                if (IsItrUrgentNotity)
                {
                    if (frmItrNotify == null)
                    {
                        frmItrNotify = new dcl.client.notifyclient.FrmItrUrgentNotify();
                        frmItrNotify.startShowFrm();//开启
                    }
                }
                else
                {

                    if (frmItrNotify != null)
                    {
                        frmItrNotify.StriogClose();//关闭并释放资源
                        frmItrNotify = null;//置空
                    }
                }

                #endregion
            }
            if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Combine_TAT_IsNotify") == "是")
            {
                bool IsComTATNotity = false;//本地是否启动

                if (LocalSetting.Current.Setting.IsTATNotify == "1")
                {
                    IsComTATNotity = true;
                }
                else
                {
                    IsComTATNotity = false;
                }

                #region 检验是否启动组合TAT提醒

                if (IsComTATNotity)
                {
                    if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("TATListView") == "是")
                    {
                        //if (frmBCComTATNotifyNew == null)
                        //{
                        FrmBcComTATNotifyNew frmBCComTATNotifyNew = new dcl.client.notifyclient.FrmBcComTATNotifyNew();
                        frmBCComTATNotifyNew.blBcTAT = false;
                        frmBCComTATNotifyNew.startShowFrm();//开启
                        //}
                    }
                    else
                    {
                        if (frmTATNotify == null)
                        {
                            frmTATNotify = new dcl.client.notifyclient.FrmComTATNotify();
                            frmTATNotify.startShowFrm();//开启
                        }
                    }
                }
                else
                {
                    if (frmTATNotify != null)
                    {
                        frmTATNotify.StriogClose();//关闭并释放资源
                        frmTATNotify = null;//置空
                    }
                }

                #endregion
            }
            if (dcl.client.common.LocalSetting.Current.Setting.IsQCNotify == "1")
            {
                bool IsQCNotify = false;//本地是否启动

                if (dcl.client.common.LocalSetting.Current.Setting.IsQCNotify == "1")
                {
                    IsQCNotify = true;
                }
                else
                {
                    IsQCNotify = false;
                }

                #region 检验是否启动仪器质控提醒

                if (IsQCNotify)
                {
                    if (frmItrQcNotify == null)
                    {
                        frmItrQcNotify = new dcl.client.notifyclient.FrmQCNotify();
                        frmItrQcNotify.startShowFrm();//开启
                    }
                }
                else
                {
                    if (frmItrQcNotify != null)
                    {
                        frmItrQcNotify.StriogClose();//关闭并释放资源
                        frmItrQcNotify = null;//置空
                    }
                }

                #endregion
            }

            if (dcl.client.common.LocalSetting.Current.Setting.IsBcTATNotify == "1"
                && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("BCCombine_TAT_IsNotify") == "是")
            {

                #region 检验是否启动条码TAT提醒
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("TATListView") == "是")
                {
                    //if (frmBCComTATNotifyNew == null)
                    //{
                    FrmBcComTATNotifyNew frmBCComTATNotifyNew = new dcl.client.notifyclient.FrmBcComTATNotifyNew();
                    frmBCComTATNotifyNew.blBcTAT = true;
                    frmBCComTATNotifyNew.startShowFrm();//开启
                    //}
                }
                else
                {
                    if (frmBCComTATNotify == null)
                    {
                        frmBCComTATNotify = new dcl.client.notifyclient.FrmBcComTATNotify();
                        frmBCComTATNotify.startShowFrm();//开启
                    }
                }

                #endregion
            }

            if (dcl.client.common.LocalSetting.Current.Setting.IsQCNotify != "1" && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Combine_TAT_IsNotify") != "是"
                && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_Instrmt_IsNotify") != "是"
                && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("Urgent_In_IsNotify") != "是"
                && dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("BCCombine_TAT_IsNotify") != "是")
            {
                this.stop();
            }

        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void stop()
        {
            if (frmUNotify != null)
            {
                frmUNotify.ClosePreLastShow();
                frmUNotify.StriogClose();//关闭并释放资源
                frmUNotify = null;//置空
            }

            if (frmItrNotify != null)
            {
                frmItrNotify.StriogClose();//关闭并释放资源
                frmItrNotify = null;//置空
            }

            if (frmItrQcNotify != null)
            {
                frmItrQcNotify.StriogClose();//关闭并释放资源
                frmItrQcNotify = null;//置空
            }

            if (frmBCComTATNotify != null)
            {
                frmBCComTATNotify.StriogClose();//关闭并释放资源
                frmBCComTATNotify = null;//置空
            }

            if (frmTATNotify != null)
            {
                frmTATNotify.StriogClose();//关闭并释放资源
                frmTATNotify = null;//置空
            }
        }
    }
}
