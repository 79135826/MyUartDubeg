using System;
using System.Text;

namespace UartCollect
{
    class ConvertUtilClass
    {
        //设备地址转换
        public static string addresstohex(int address)
        {
            string j = Convert.ToString(address, 16);
            if (j.Length < 2)
            {
                return "0" + j;
            }
            else
            {
                return j;
            }
        }
        // 16进制编码
        public static byte[] HexstrToByte(string hexString)
        {
            //hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0) hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                //returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Replace(" ", ""), 16);
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public static string IntToHex(int i)
        {
            string s = Convert.ToString(i, 16);
            if (s.Length == 1) s = "0" + s;
            if (s.Length == 3) s = "0" + s;
            return AddSpace(s);
        }
        //增加空格
        public static string AddSpace(string s)
        {
            if (s.Length > 2)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < s.Length; i += 2)
                {
                    sb.Append(s.Substring(i, 2) + " ");
                }
                return sb.ToString().TrimEnd(' ');
            }
            else
            {
                return s;
            }
        }
        //byte[] 转string
        public static string byteTostring(byte[] data)
        {
            StringBuilder bs = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                bs.AppendFormat("{0:X2}" + " ", data[i]);
            }
            return bs.ToString().Replace(" ", "");
        }
        //16进制浮点数转10进制
        public static float HexToFloat(string strHex)
        {
            string strBase16 = strHex;
            if (strBase16.Length != 8) { return -1; }
            string strTemp = "";
            double temp = 0;
            int m_s = 0; //   数符   
            int m_e = 0; //   阶   
            double m_x = 0; //   小数部分   
            double m_re = 0; //   计算结果   

            strTemp = strBase16.Substring(0, 2);
            temp = Convert.ToInt32(strTemp, 16) & 0x80;
            if (temp == 128) m_s = 1;
            strTemp = strBase16.Substring(0, 3);
            temp = Convert.ToInt32(strTemp, 16) & 0x7f8;
            m_e = Convert.ToInt32(temp / Math.Pow(2, 3));
            strTemp = strBase16.Substring(2, 6);
            temp = Convert.ToInt32(strTemp, 16) & 0x7fffff;
            m_x = temp / Math.Pow(2, 23);
            m_re = Math.Pow(-1, m_s) * (1 + m_x) * Math.Pow(2, m_e - 127);
            //return decimal.Round(Convert.ToDecimal(m_re), 5);
            return float.Parse(decimal.Round(Convert.ToDecimal(m_re), 5).ToString());
        }
        //16进制数据转换为数组,默认8位的数据
        //data:去除地址位、数据长度位、CRC校验位的连续16进制字符串
        public static string[] HexToStringArray(string data, int datalen = 8)
        {
            //data = data.Substring(6, data.Length - 10);
            string[] c = new string[data.Length / datalen];
            int j = 0;
            for (int i = 0; i < data.Length; i += datalen)
            {
                c[j] = data.Substring(i, datalen);
                j++;
            }
            return c;
        }

        public static string HexToChar(string data)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i += 2)
            {
                int ii = Convert.ToInt32(data.Substring(i, 2), 16);
                if(ii>63)sb.Append(Convert.ToChar(ii).ToString());
            }
            return sb.ToString();
        }
    }
}
