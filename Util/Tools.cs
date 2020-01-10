using System;
using System.Text;
using System.Threading;

namespace UartCollect.Util
{
    public static class Tools
    {
        //等待一会儿：second秒
        public static void WaitFor(double second = 1)
        {
            second *= 1000;
            int j = 0;
            while (j < second)
            {
                Thread.Sleep(1);
                System.Windows.Forms.Application.DoEvents();
                j++;
            }
        }
        /// <summary>
        /// 十进制转16进制，可指定输出长度，补0
        /// </summary>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string intTohex(string address,int len)
        {
            string j = Convert.ToString(Convert.ToInt32(address), 16);
            while (j.Length < len)
            {
                j= "0" + j;
            }
            return AddSpace(j);
        }
        /// <summary>
        /// 增加空格
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
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
    }
}
