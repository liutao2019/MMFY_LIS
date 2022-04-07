using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace dcl.client.common
{
    public class LastInputChecker
    {
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            // 设置结构体块容量   
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            // 捕获的时间   
            [MarshalAs(UnmanagedType.U4)]
            public uint dwTime;
        }
        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        /// <summary>
        /// 获取键盘和鼠标没有操作的时间(毫秒)  
        /// </summary>
        /// <returns></returns>
        public static long GetLastInputTime()
        {
            LASTINPUTINFO vLastInputInfo = new LASTINPUTINFO();
            vLastInputInfo.cbSize = Marshal.SizeOf(vLastInputInfo);
            // 捕获时间   
            if (!GetLastInputInfo(ref vLastInputInfo))
                return 0;
            else
                return Environment.TickCount - (long)vLastInputInfo.dwTime;
        }  
    }
}
