using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace dcl.client.report
{
    public class PrinterCache
    {
        private static object objectLock = new object();

        private static PrinterCache _current = null;

        public static PrinterCache Current
        {
            get
            {
                if (_current == null)
                {
                    lock (objectLock)
                    {
                        if (_current == null)
                        {
                            _current = new PrinterCache();
                        }
                    }
                }
                return _current;
            }
        }

        ManagementObjectSearcher query;

        ManagementObjectCollection _printercollection = null;

        public ManagementObjectCollection PrintersCollection
        {
            get
            {
                return _printercollection;
            }
        }

        private PrinterCache()
        {
            if (query == null)
            {
                query = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
                _printercollection = query.Get();
            }
        }
    }
}
