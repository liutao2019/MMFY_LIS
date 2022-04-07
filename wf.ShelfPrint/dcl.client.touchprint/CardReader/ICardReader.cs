using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wf.ShelfPrint
{
    public interface ICardReader
    {
        /// <summary>
        /// 打开读卡器
        /// </summary>
        /// <returns></returns>
        bool Open();

        /// <summary>
        /// 读卡
        /// </summary>
        /// <returns></returns>
        string Reader();

        /// <summary>
        /// 关闭读卡器
        /// </summary>
        /// <returns></returns>
        bool Close();
    }
}
