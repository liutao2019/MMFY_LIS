using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace wf.ShelfPrint
{
    public class ReadLoadClass
    {

        #region 基本函数

        [DllImport("mwrf32.dll", EntryPoint = "rf_card", SetLastError = true,
         CharSet = CharSet.Auto, ExactSpelling = false,
         CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_card(int icdev, int mode, out uint snr);
        [DllImport("mwrf32.dll", EntryPoint = "rf_request", SetLastError = true,
                 CharSet = CharSet.Auto, ExactSpelling = false,
                 CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_request(int icdev, int mode, out UInt16 tagtype);

        [DllImport("mwrf32.dll", EntryPoint = "rf_request_std", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_request_std(int icdev, int mode, out UInt16 tagtype);

        [DllImport("mwrf32.dll", EntryPoint = "rf_anticoll", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_anticoll(int icdev, int bcnt, out uint snr);

        [DllImport("mwrf32.dll", EntryPoint = "rf_select", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_select(int icdev, uint snr, out byte size);

        [DllImport("mwrf32.dll", EntryPoint = "rf_authentication", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_authentication(int icdev, int mode, int secnr);

        [DllImport("mwrf32.dll", EntryPoint = "rf_authentication_2", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_authentication_2(int icdev, int mode, int keynr, int blocknr);

        [DllImport("mwrf32.dll", EntryPoint = "rf_read", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_read(int icdev, int blocknr, [MarshalAs(UnmanagedType.LPArray)]byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_read_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_read_hex(int icdev, int blocknr, [MarshalAs(UnmanagedType.LPArray)]byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_write_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_write_hex(int icdev, int blocknr, [MarshalAs(UnmanagedType.LPArray)]byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_write", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_write(int icdev, int blocknr, [MarshalAs(UnmanagedType.LPArray)]byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_halt", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_halt(int icdev);

        [DllImport("mwrf32.dll", EntryPoint = "rf_initval", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_initval(int icdev, int blocknr, int val);

        [DllImport("mwrf32.dll", EntryPoint = "rf_readval", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_readval(int icdev, int blocknr, out uint val);

        [DllImport("mwrf32.dll", EntryPoint = "rf_increment", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_increment(int icdev, int blocknr, int val);

        [DllImport("mwrf32.dll", EntryPoint = "rf_decrement", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_decrement(int icdev, int blocknr, int val);

        [DllImport("mwrf32.dll", EntryPoint = "rf_restore", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_restore(int icdev, int blocknr);

        [DllImport("mwrf32.dll", EntryPoint = "rf_transfer", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_transfer(int icdev, int blocknr);



        [DllImport("mwrf32.dll", EntryPoint = "rf_changeb3", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_changeb3(int icdev, int secnr, [MarshalAs(UnmanagedType.LPArray)]byte[] keyA, UInt16 b0, UInt16 b1, UInt16 b2, UInt16 b3, UInt16 bk, [MarshalAs(UnmanagedType.LPArray)]byte[] keyB);

        [DllImport("mwrf32.dll", EntryPoint = "rf_init", SetLastError = true,
         CharSet = CharSet.Auto, ExactSpelling = false,
         CallingConvention = CallingConvention.StdCall)]
        //说明：初始化通讯接口
        public static extern int rf_init(Int16 port, int baud);

        [DllImport("mwrf32.dll", EntryPoint = "rf_exit", SetLastError = true,
         CharSet = CharSet.Auto, ExactSpelling = false,
         CallingConvention = CallingConvention.StdCall)]
        //说明：    关闭通讯口
        public static extern Int16 rf_exit(int icdev);

        [DllImport("mwrf32.dll", EntryPoint = "rf_get_status", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_get_status(int icdev, [MarshalAs(UnmanagedType.LPArray)]byte[] state);

        [DllImport("mwrf32.dll", EntryPoint = "rf_beep", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_beep(int icdev, int msec);

        [DllImport("mwrf32.dll", EntryPoint = "rf_load_key", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_load_key(int icdev, int mode, int secnr, [MarshalAs(UnmanagedType.LPArray)]byte[] keybuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_load_key_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_load_key_hex(int icdev, int mode, int secnr, [MarshalAs(UnmanagedType.LPArray)]byte[] keybuff);


        [DllImport("mwrf32.dll", EntryPoint = "a_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 a_hex([MarshalAs(UnmanagedType.LPArray)]byte[] asc, [MarshalAs(UnmanagedType.LPArray)]byte[] hex, int len);

        [DllImport("mwrf32.dll", EntryPoint = "hex_a", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 hex_a([MarshalAs(UnmanagedType.LPArray)]byte[] hex, [MarshalAs(UnmanagedType.LPArray)]byte[] asc, int len);

        [DllImport("mwrf32.dll", EntryPoint = "rf_reset", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_reset(int icdev, int msec);

        [DllImport("mwrf32.dll", EntryPoint = "rf_clr_control_bit", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_clr_control_bit(int icdev, int _b);


        [DllImport("mwrf32.dll", EntryPoint = "rf_set_control_bit", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_set_control_bit(int icdev, int _b);

        [DllImport("mwrf32.dll", EntryPoint = "rf_disp8", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_disp8(int icdev, short mode, [MarshalAs(UnmanagedType.LPArray)]byte[] disp);

        [DllImport("mwrf32.dll", EntryPoint = "rf_disp", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_disp(int icdev, short mode, int digit);

        [DllImport("mwrf32.dll", EntryPoint = "rf_encrypt", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_encrypt([MarshalAs(UnmanagedType.LPArray)]byte[] key, [MarshalAs(UnmanagedType.LPArray)]byte[] ptrsource, int len, [MarshalAs(UnmanagedType.LPArray)]byte[] ptrdest);

        [DllImport("mwrf32.dll", EntryPoint = "rf_decrypt", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_decrypt([MarshalAs(UnmanagedType.LPArray)]byte[] key, [MarshalAs(UnmanagedType.LPArray)]byte[] ptrsource, int len, [MarshalAs(UnmanagedType.LPArray)]byte[] ptrdest);

        [DllImport("mwrf32.dll", EntryPoint = "rf_srd_eeprom", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_srd_eeprom(int icdev, int offset, int len, [MarshalAs(UnmanagedType.LPArray)]byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_swr_eeprom", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_swr_eeprom(int icdev, int offset, int len, [MarshalAs(UnmanagedType.LPArray)]byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_setport", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_setport(int icdev, byte _byte);

        [DllImport("mwrf32.dll", EntryPoint = "rf_getport", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_getport(int icdev, out byte _byte);

        [DllImport("mwrf32.dll", EntryPoint = "rf_gettime", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_gettime(int icdev, [MarshalAs(UnmanagedType.LPArray)]byte[] time);


        [DllImport("mwrf32.dll", EntryPoint = "rf_gettime_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_gettime_hex(int icdev, [MarshalAs(UnmanagedType.LPArray)]byte[] time);

        [DllImport("mwrf32.dll", EntryPoint = "rf_settime_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_settime_hex(int icdev, [MarshalAs(UnmanagedType.LPArray)]byte[] time);

        [DllImport("mwrf32.dll", EntryPoint = "rf_settime", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_settime(int icdev, [MarshalAs(UnmanagedType.LPArray)]byte[] time);

        [DllImport("mwrf32.dll", EntryPoint = " rf_setbright", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_setbright(int icdev, byte bright);

        [DllImport("mwrf32.dll", EntryPoint = "rf_ctl_mode", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_ctl_mode(int icdev, int mode);

        [DllImport("mwrf32.dll", EntryPoint = "rf_disp_mode", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 rf_disp_mode(int icdev, int mode);

        [DllImport("mwrf32.dll", EntryPoint = "lib_ver", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        //说明：     返回设备当前状态
        public static extern Int16 lib_ver([MarshalAs(UnmanagedType.LPArray)]byte[] ver);

        #endregion

        #region 参数

        public int icdev; // 通讯设备标识符        
        public int st;
        public int sec;
        public int cardmode;
        public int keymode;

        #endregion

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Port">串口号</param>
        /// <param name="Baud">波特率</param>
        /// <returns>成功返回0，失败返回-1</returns>
        public virtual int Init(int Port, int Baud)
        {
            icdev = rf_init((Int16)Port, Baud);
            if (icdev <= 0) { return -1; }
            return 0;
        }
        #endregion

        #region 关闭
        /// <summary>
        /// 关闭
        /// </summary>
        /// <returns>成功返回0，失败返回-1</returns>
        public virtual int Close()
        {
            st = rf_exit(icdev);
            if (st != 0) { return -1; }
            return 0;

        }
        #endregion

        #region 读RFID卡
        /// <summary>
        /// 读RFID卡
        /// </summary>
        /// <param name="CardType">射频卡类型：1为S50,2为S70</param>
        /// <param name="SectionAddr">扇区号</param>
        /// <param name="BlockAddr">块号</param>
        /// <param name="SecKey">密码6个字节</param>
        /// <param name="CardData">读取的卡号</param>
        /// <returns>成功返回0，失败返回-1</returns>
        public virtual int ReadRFIDCard(int CardType, int SectionAddr, int BlockAddr, byte[] SecKey, ref string CardData)
        {
            uint num = 0u;
            string text = string.Empty;
            this.st = rf_card(this.icdev, this.cardmode, out num);
            int result;
            if (this.st != 0)
            {
                result = -1;
            }
            else
            {
                string text2 = num.ToString("X2");
                this.st = rf_load_key(this.icdev, 1, SectionAddr, SecKey);
                if (this.st != 0)
                {
                    result = -1;
                }
                else
                {
                    this.st = rf_authentication(this.icdev, 0, SectionAddr);
                    if (this.st != 0)
                    {
                        result = -1;
                    }
                    else
                    {
                        byte[] array = new byte[254];
                        byte[] array2 = new byte[254];
                        this.st = rf_read(this.icdev, SectionAddr * 4 + BlockAddr, array);
                        if (this.st != 0)
                        {
                            result = -1;
                        }
                        else
                        {
                            hex_a(array, array2, array.Length);
                            text = Encoding.Default.GetString(array2);
                            for (int i = 0; i < array.Length; i++)
                            {
                                CardData += text[i];
                            }
                            result = 0;
                        }
                    }
                }
            }

            return result;

        }

        #endregion
    }
}
