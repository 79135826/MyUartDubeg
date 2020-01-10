using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace UartCollect
{
    public class CRC16
    {
        //日志初始化
        public static readonly ILog Log = LogManager.GetLogger("RollingLogFileAppender");
        public CRC16()
        {
        }
        //校验crc16
        public static bool check(byte[] buff, int index, int len)
        {  //buff是待校验数组，index为起始索引，len为校验长度
            //buff是待校验数组，index为起始索引，len为校验长度
            uint i, j;
            byte h, l;
            byte c, d;
            h = 0x55;
            l = 0xaa;
            for (i = (uint)index; i < index + len; i++)
            {
                h = (byte)(buff[i] ^ h);
                for (j = 0; j < 8; j++)
                {
                    c = (byte)(l & 0x80);
                    l <<= 1;
                    d = (byte)(h & 0x80);
                    h <<= 1;
                    if (c != 0)
                        h |= 0x01;
                    if (d != 0)
                    {
                        //
                        h = (byte)(h ^ 0x10);
                        l = (byte)(l ^ 0x21);
                    }
                }
            }
            if ((h == 0) && (l == 0))
                return true;
            else
                return false;
        }
        //计算一个byte数组中指定位置的crc16
        public static byte[] cal(byte[] buff, int index, int len)
        {
            //buff是待校验数组，index为起始索引，len为校验长度
            uint i, j;
            byte h, l;
            byte c, d;
            h = 0x55;
            l = 0xaa;
            for (i = (uint)index; i < index + len; i++)
            {
                h = (byte)(buff[i] ^ h);
                for (j = 0; j < 8; j++)
                {
                    c = (byte)(l & 0x80);
                    l <<= 1;
                    d = (byte)(h & 0x80);
                    h <<= 1;
                    if (c != 0)
                        h |= 0x01;
                    if (d != 0)
                    {
                        //
                        h = (byte)(h ^ 0x10);
                        l = (byte)(l ^ 0x21);
                    }
                }
            }
            byte[] resu = new byte[2];
            resu[0] = (byte)h;
            resu[1] = (byte)l;
            return resu;
        }
        /// <summary>
        /// CRC校验
        /// </summary>
        /// <param name="data">校验数据</param>
        /// <returns>高低8位</returns>
        public static string CRCCalc(string data)
        {
            string[] datas = data.Split(' ');
            List<byte> bytedata = new List<byte>();
            foreach (string str in datas)
            {
                bytedata.Add(byte.Parse(str, System.Globalization.NumberStyles.AllowHexSpecifier));
            }
            byte[] crcbuf = bytedata.ToArray();
            //计算并填写CRC校验码
            int crc = 0xffff;
            int len = crcbuf.Length;
            for (int n = 0; n < len; n++)
            {
                byte i;
                crc = crc ^ crcbuf[n];
                for (i = 0; i < 8; i++)
                {
                    int TT;
                    TT = crc & 1;
                    crc = crc >> 1;
                    crc = crc & 0x7fff;
                    if (TT == 1)
                    {
                        crc = crc ^ 0xa001;
                    }
                    crc = crc & 0xffff;
                }
            }
            string[] redata = new string[2];
            redata[1] = Convert.ToString((byte)((crc >> 8) & 0xff), 16);
            redata[0] = Convert.ToString((byte)((crc & 0xff)), 16);
            if (redata[0].Length < 2) { redata[0] = "0" + redata[0]; }
            if (redata[1].Length < 2) { redata[1] = "0" + redata[1]; }
            return redata[0] + " " + redata[1];
        }
        //正常使用
        //数据校验
        //data：收到的16进制数据，无空格
        public static bool RecCRC16(string data)
        {
            if (string.IsNullOrEmpty(data)) { return false; }
            string crc = data.Substring(data.Length - 4, 4);
            string crcdata = data.Substring(0, data.Length - 4);
            string c = string.Empty;
            for (int i = 0; i < crcdata.Length; i = i + 2)
            {
                c += crcdata.Substring(i, 2) + " ";
            }
            c = c.Remove(c.LastIndexOf(" "), 1);
            string mycrc = CRCCalc(c).Replace(" ", "").ToUpper();
            bool result = string.Equals(crc, mycrc);
            if (!result)
            {
                Log.Info("RecCRC16 ERROR,crc/mycrc:" + crc + "/" + mycrc + "/" + data);
            }
            return result;

        }
        //测试使用
        public static string RecCRC162(byte[] crcbuf)
        {
            //byte[] crcbuf = new byte[bytedata.Count];
            //bytedata.CopyTo(0, crcbuf, 0, bytedata.Count);

            //byte[] crcbuf = bytedata.ToArray();
            //计算并填写CRC校验码
            int crc = 0xffff;
            int len = crcbuf.Length;
            for (int n = 0; n < len; n++)
            {
                byte i;
                crc = crc ^ crcbuf[n];
                for (i = 0; i < 8; i++)
                {
                    int TT;
                    TT = crc & 1;
                    crc = crc >> 1;
                    crc = crc & 0x7fff;
                    if (TT == 1)
                    {
                        crc = crc ^ 0xa001;
                    }
                    crc = crc & 0xffff;
                }
            }
            string[] redata = new string[2];
            redata[1] = Convert.ToString((byte)((crc >> 8) & 0xff), 16);
            redata[0] = Convert.ToString((byte)((crc & 0xff)), 16);
            if (redata[0].Length < 2) { redata[0] = "0" + redata[0]; }
            if (redata[1].Length < 2) { redata[1] = "0" + redata[1]; }
            Log.Debug("RecCRC162:" + redata[0] + redata[1]);
            return redata[0] + " " + redata[1];
        }
    }
}
