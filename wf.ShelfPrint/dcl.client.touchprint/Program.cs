using dcl.client.wcf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace wf.ShelfPrint
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread load = new Thread(Load);
            load.Start();
            Application.Run(new FrmMainPrint());
        }

        //提高第一次查询速度，预加载链接
        private static void Load()
        {
            ProxyTouchPrint proxy = new ProxyTouchPrint();
            proxy.Service.PatientPrintQuery("-1", "107");
        }
    }
}
