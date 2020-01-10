using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UartCollect.Util
{
    public static class Command
    {
        public static string Calculate(string body, string type)
        {
            string command = string.Empty;
            switch (type)
            {
                default:
                    command = CRC16.CRCCalc(body);
                    break;
            }
            return command;
        }
        public static string VerifyFlag(int len, string command, string type)
        {
            string verifyflag = string.Empty;
            switch (type)
            {
                default:
                    string dl = ConvertUtilClass.IntToHex(len - 5);
                    verifyflag = command.Substring(0, 4) + "" + dl;
                    break;
            }
            return verifyflag.ToUpper();
        }
    }
}
