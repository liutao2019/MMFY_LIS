using System;
using dcl.svr.msg;
using System.Diagnostics;

namespace dcl.pub.wcf
{
    public class Global : System.Web.HttpApplication
    {
        #region Timer

        /// <summary>
        /// 条码监控
        /// </summary>
        static System.Timers.Timer timerBarcodeMonitor = new System.Timers.Timer();

        /// <summary>
        /// 仪器危急值数据缓存
        /// </summary>
        static System.Timers.Timer timerCacheInstrmtUrgentMsg = new System.Timers.Timer();


        /// <summary>
        /// 回退条码缓存
        /// </summary>
        static System.Timers.Timer timerCacheSampReturn = new System.Timers.Timer();

        /// <summary>
        /// 组合TAT数据缓存
        /// </summary>
        static System.Timers.Timer timerCacheCombineTATMsg = new System.Timers.Timer();

        #endregion

        protected void Application_Start(object sender, EventArgs e)
        {
            timerBarcodeMonitor.Interval = 120000;

            //仪器危急值数据缓存
            timerCacheInstrmtUrgentMsg.Interval = 30 * 1000;//30秒


            timerCacheSampReturn.Interval = 30 * 1000;//30秒

            //组合TAT数据缓存
            timerCacheCombineTATMsg.Interval = 30 * 1000;//30秒

            try
            {

                timerBarcodeMonitor.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timerBarcodeMonitor.Start();

                //timerCacheInstrmtUrgentMsg.Elapsed += new System.Timers.ElapsedEventHandler(timerCacheInstrmtUrgentMsg_Elapsed);
                //timerCacheInstrmtUrgentMsg.Start();

                timerCacheSampReturn.Elapsed += new System.Timers.ElapsedEventHandler(timerCacheSampReturn_Elapsed);
                timerCacheSampReturn.Start();


                timerCacheCombineTATMsg.Elapsed += new System.Timers.ElapsedEventHandler(timerCacheCombineTATMsg_Elapsed);
                timerCacheCombineTATMsg.Start();
            }
            catch (Exception ex)
            {
                timerBarcodeMonitor.Stop();
                timerCacheSampReturn.Stop();
                Trace.WriteLine("exception：" + ex.ToString());
            }
        }


        /// <summary>
        /// 回退条码缓存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerCacheSampReturn_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            dcl.svr.cache.SampReturnCache.Current.Refresh();
        }
        /// <summary>
        /// 仪器危急值数据缓存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerCacheInstrmtUrgentMsg_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            dcl.svr.msg.InstrmtUrgentTATMsgCache.Current.Refresh();//实体改造新增
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UserObrMessageCache.Current.Refresh();
            DeptObrMessageCache.Current.Refresh();
            UrgentObrMessageCache.Current.Refresh();
        }

        /// <summary>
        /// 组合TAT数据缓存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timerCacheCombineTATMsg_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            dcl.svr.msg.CombineTATMsgCache.Current.Refresh();
        }
    }
}