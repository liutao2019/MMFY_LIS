using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Runtime.InteropServices;

namespace dcl.client.msgclient
{
    public class PlaySoundMgr
    {


        private PlaySoundMgr()
        {
            Refresh();
        }
        static PlaySoundMgr _instance = null;


        public static PlaySoundMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlaySoundMgr();
                }
                return _instance;
            }
        }

        public string PlaySoundMode
        {
            get;
            private set;
        }

        public void Refresh()
        {
            PlaySoundMode = System.Configuration.ConfigurationManager.AppSettings["PlaySoundMode"];
            try
            {
                //if (PlaySoundMode == "2")
                //{
                if (SoundControl.IsMuted())
                {
                    Mute();
                }
                for (int temp_up = 1; temp_up <= 50; temp_up++)//系统音量默认最大
                {
                    VolumeUp();
                }
                //  }
            }
            catch (Exception ex)
            {

            }
        }


        public void SaveConfig(string mode)
        {
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings.Remove("PlaySoundMode");
            config.AppSettings.Settings.Add("PlaySoundMode", mode);
            config.Save(ConfigurationSaveMode.Modified);
            Refresh();
        }

        public void PlaySound()
        {
            if (string.IsNullOrEmpty(PlaySoundMode) || PlaySoundMode == "1")
            {
                playBeep();//主机发声

            }
            else if (PlaySoundMode == "2")
            {
                PlaySound("WindowsNotify.wav", 0, SND_ASYNC | SND_FILENAME);//音响发声
            }
        }
        private const byte VK_VOLUME_MUTE = 0xAD;
        private const byte VK_VOLUME_DOWN = 0xAE;
        private const byte VK_VOLUME_UP = 0xAF;
        private const UInt32 KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const UInt32 KEYEVENTF_KEYUP = 0x0002;
        [DllImport("user32.dll")]
        static extern Byte MapVirtualKey(UInt32 uCode, UInt32 uMapType);
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, UInt32 dwFlags, UInt32 dwExtraInfo);
        /// <summary>
        /// +2音量
        /// </summary>
        private static void VolumeUp()
        {
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_UP, MapVirtualKey(VK_VOLUME_UP, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// -2音量
        /// </summary>
        private static void VolumeDown()
        {
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_DOWN, MapVirtualKey(VK_VOLUME_DOWN, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }

        /// <summary>
        /// 静音
        /// </summary>
        private static void Mute()
        {
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY, 0);
            keybd_event(VK_VOLUME_MUTE, MapVirtualKey(VK_VOLUME_MUTE, 0), KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
        }
        /// <summary>
        /// 主机发声音
        /// </summary>
        /// <param name="frequency"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        [DllImport("Kernel32.dll", EntryPoint = "Beep")]
        private static extern bool Beep(int frequency, int duration);

        /// <summary>
        /// 音响设备发声音
        /// </summary>
        /// <param name="pszSound"></param>
        /// <param name="hmod"></param>
        /// <param name="fdwSound"></param>
        /// <returns></returns>
        [DllImport("winmm.dll")]
        private static extern bool PlaySound(string pszSound, int hmod, int fdwSound);
        private const int SND_FILENAME = 0x00020000; public const int SND_ASYNC = 0x0001;

        /// <summary>
        /// 主机发声
        /// </summary>
        private bool playBeep()
        {
            try
            {
                //响的时长
                const int L0 = 1600, L1 = 800, L2 = 400, L3 = 200, L4 = 100;
                //音符-频率
                const int noteC = 264, noteD = 297, noteE = 330, noteF = 352, noteG = 396, noteA = 440, noteB = 495;

                //自定义声音
                Beep(noteB, L3);
                Beep(noteB, L3);
                Beep(noteB, L3);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
