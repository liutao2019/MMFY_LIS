using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace dcl.client.common
{
    public class BankCarReader
    {
        [DllImport("LKEDriver_V21.dll")]
        public static extern int LKE_OpenCOM(int COM, int Baud);

        [DllImport("LKEDriver_V21.dll")]
        public static extern int LKE_CloseCOM();

        [DllImport("LKEDriver_V21.dll")]
        public static extern int LKE_SelectExtPort(String ExPort);

        [DllImport("LKEDriver_V21.dll")]
        public static extern int LKE_MSR_Read(int TrackType, StringBuilder SecTrack, StringBuilder TriTrack, int OutTime);
    }
}
