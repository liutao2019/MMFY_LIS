using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.InteropServices;

namespace dcl.client.common
{
   [StructLayout( LayoutKind.Sequential)]
   public struct MyMessage
   {
      /// <summary>当前消息包含的数据</summary>   
      private Object msg;

      public Object Msg
      {
         get { return msg; }
         set { msg = value; }
      }

      public MyMessage(object msg)
      {
         this.msg = msg;
      }

      public MyMessage(IntPtr ptr)
      {
         msg = null;

         if (ptr != IntPtr.Zero)
         {
            try
            {
               MyMessage m = (MyMessage)Marshal.PtrToStructure(ptr, typeof(MyMessage));
               msg = m.Msg;
            }
            catch 
            {
               
            }
         }
      }
   }

    /// <summary>
    /// windows消息 助手类
    /// </summary>
   public class MessageHelper
   {
      private IntPtr hWnd = IntPtr.Zero;
      public const int WM_USER = 0x400;
      public const int WM_ICCARD = WM_USER + 1;
      public const int WM_CLOSE_FORM = WM_USER + 2;
      public const int WM_IMPORT = WM_USER + 3;
      public const int WM_START = WM_USER + 4;

      public MessageHelper(IntPtr hWnd)
      {
         this.hWnd = hWnd;
      }

      [DllImport("user32.dll")]
      extern internal static int SendMessage(IntPtr hWnd, int msg, int wParam, ref MyMessage lParam);

      public int SendMsg( int msg, int wParam, Object lParam)
      {
         MyMessage message = new MyMessage(lParam);
         return SendMessage(hWnd, msg, wParam, ref message);
      }
   }
}
