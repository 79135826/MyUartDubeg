using System.Collections.Generic;
using System.Collections.Concurrent;

namespace UartCollect
{
    public static class GlobalData
    {
        //设备参数存储器
        public static ConcurrentDictionary<int, Dictionary<int, Dictionary<string, string>>> DevicesDataStorage;
        public static ConcurrentDictionary<int, Dictionary<string, Dictionary<string, string>>> FirstCommandDictionary;

        public static ConcurrentDictionary<int, Dictionary<string, Dictionary<string, string>>> FirstCommandInfoDictionary;
        public static int FirstCommandCount=0;
        public static bool START;
        public static bool STOP;
        public static int RefRate;
        public static int TimeOut;
        internal static string DOUT_OPEN= "FF 00";
        internal static string DOUT_CLOSE= "00 00";

        public static ConcurrentDictionary<int, Dictionary<int, Dictionary<string, string>>> Read_DevicesDataStorage
        {
            get { return DevicesDataStorage; }
        }
    }
}
