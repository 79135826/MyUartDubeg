using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UartCollect.Headle;
using UartCollect.Util;

namespace UartCollect.Core
{
    class FirstCommand
    {
        /// <summary>
        /// 生成优先执行命令
        /// </summary>
        /// <param name="devid">设备地址</param>
        /// <param name="writepramasname">写的参数名称：SET_DO1,SET_DO2</param>
        /// <param name="readpramasname">对应读取的参数名称</param>
        /// <param name="val">值</param>
        /// <returns></returns>
        public static bool Add_FirstCommand(int devid, string writepramasname, int val, string readpramasname,int DataLen, string Verify)
        {
            int address = devid;
            string valstr = string.Empty;
            if (CheckType(writepramasname) == 0)
            {
                //开关量
                if (val == 1) { valstr = GlobalData.DOUT_OPEN; } else { valstr = GlobalData.DOUT_CLOSE; }
            }
            else
            {
                //整数
                valstr = Build_Command(val);
            }
            string register = GlobalData.FirstCommandInfoDictionary[devid][writepramasname]["Register"];
            string funcode = GlobalData.FirstCommandInfoDictionary[devid][writepramasname]["FunCode"];
            string command = address.ToString("X2") + " " + funcode + " "+ Tools.intTohex(register, 4) + " " + valstr;
            //command += CRC16.CRCCalc(command);
            command += Util.Command.Calculate(command, writepramasname);
            command = command.Replace(" ", "");
            command = command.ToUpper();

            string Decode = writepramasname;
            string VerifyFlag = "";
            if (Verify == "HEAD")
            {
                VerifyFlag = Util.Command.VerifyFlag(DataLen, command, writepramasname);
            }
            if (!GlobalData.FirstCommandDictionary[devid].ContainsKey(writepramasname))
            {
                return false;
            }
            else
            {
                if (GlobalData.FirstCommandDictionary[devid][writepramasname]["Finish"] == "1" || GlobalData.FirstCommandDictionary[devid][writepramasname]["Command"] != command)
                {
                    GlobalData.FirstCommandCount += 1;
                    UIAction.AppendLog("新增命令：" + command);
                }
                else
                {
                    UIAction.AppendLog("命令已存在：" + command);
                }
                GlobalData.FirstCommandDictionary[devid][writepramasname]["Command"] = command;//命令
                GlobalData.FirstCommandDictionary[devid][writepramasname]["DataLen"] = DataLen.ToString();//数据长度
                GlobalData.FirstCommandDictionary[devid][writepramasname]["Error"] = "0";//错误次数
                GlobalData.FirstCommandDictionary[devid][writepramasname]["Finish"] = "0";//是否完成
                GlobalData.FirstCommandDictionary[devid][writepramasname]["HexData"] = "";//收到的16进制数据
                GlobalData.FirstCommandDictionary[devid][writepramasname]["RecTime"] = "";//最后收到数据时间"yyyy-MM-dd HH:mm:ss"
                GlobalData.FirstCommandDictionary[devid][writepramasname]["SendTime"] = "";//最后发送命令时间"yyyy-MM-dd HH:mm:ss"
                GlobalData.FirstCommandDictionary[devid][writepramasname]["VerifyFlag"] = VerifyFlag;//数据校验标志
                GlobalData.FirstCommandDictionary[devid][writepramasname]["Status"] = "0";//状态:0正常，1错误
                GlobalData.FirstCommandDictionary[devid][writepramasname]["Value"] = val.ToString();//写入的值
                GlobalData.FirstCommandDictionary[devid][writepramasname]["PramaName"] = readpramasname;//对应采集的参数名称
            }
            return true;
        }
        /// <summary>
        /// 生成优先执行非标命令
        /// </summary>
        /// <param name="devid">设备ID</param>
        /// <param name="writepramasname">写的参数名称：SET_DO1,SET_DO2</param>
        /// <param name="readpramasname">对应读取的参数名称</param>
        /// <param name="valstring">值字符串</param>
        /// <returns></returns>
        public static bool Add_FirstCommand(int devid, string writepramasname, string valstring, string readpramasname,int DataLen,string Verify)
        {
            int address = devid;
            string command = address.ToString("X2") + " 05 00 00 " + valstring;
            //command += CRC16.CRCCalc(command);
            command += Util.Command.Calculate(command, writepramasname);
            command = command.Replace(" ", "");
            command = command.ToUpper();

            string Decode = writepramasname;
            string VerifyFlag = "";
            if (Verify == "HEAD")
            {
                VerifyFlag = Util.Command.VerifyFlag(DataLen, command, writepramasname);
            }
            if (!GlobalData.FirstCommandDictionary[devid].ContainsKey(writepramasname))
            {
                return false;
            }
            else
            {
                if (GlobalData.FirstCommandDictionary[devid][writepramasname]["Finish"] == "1" || GlobalData.FirstCommandDictionary[devid][writepramasname]["Command"] != command)
                {
                    GlobalData.FirstCommandCount += 1;
                    UIAction.AppendLog("新增命令：" + command);
                }
                else
                {
                    UIAction.AppendLog("命令已存在：" + command);
                }
                GlobalData.FirstCommandDictionary[devid][writepramasname]["Command"] = command;//命令
                GlobalData.FirstCommandDictionary[devid][writepramasname]["DataLen"] = DataLen.ToString();//数据长度
                GlobalData.FirstCommandDictionary[devid][writepramasname]["Error"] = "0";//错误次数
                GlobalData.FirstCommandDictionary[devid][writepramasname]["Finish"] = "0";//是否完成
                GlobalData.FirstCommandDictionary[devid][writepramasname]["HexData"] = "";//收到的16进制数据
                GlobalData.FirstCommandDictionary[devid][writepramasname]["RecTime"] = "";//最后收到数据时间"yyyy-MM-dd HH:mm:ss"
                GlobalData.FirstCommandDictionary[devid][writepramasname]["SendTime"] = "";//最后发送命令时间"yyyy-MM-dd HH:mm:ss"
                GlobalData.FirstCommandDictionary[devid][writepramasname]["VerifyFlag"] = VerifyFlag;//数据校验标志
                GlobalData.FirstCommandDictionary[devid][writepramasname]["Status"] = "0";//状态:0正常，1错误
                GlobalData.FirstCommandDictionary[devid][writepramasname]["Value"] = "";//写入的值
                GlobalData.FirstCommandDictionary[devid][writepramasname]["PramaName"] = readpramasname;//对应采集的参数名称
            }
            return true;
        }
        public static string Build_Command(int val)
        {
            string ss = ConvertUtilClass.IntToHex(val);
            if (ss.Length == 2) { ss = "00 " + ss; }
            if (ss.Length == 4) { ss = ss.Substring(0, 2) + " " + ss.Substring(2, 2); }
            return ss;
        }
        private static int CheckType(string pramas)
        {
            int rs = 0;//0:开关量，1整数
            switch (pramas)
            {
                case "SET_DO1":
                case "SET_DO2":
                case "SET_DO3":
                case "SET_DO4":
                    rs = 0;
                    break;
                case "TEMI2500_SET_PTNO":
                    rs = 1;
                    break;
                default:
                    rs = 1;
                    break;
            }
            return rs;
        }
    }
}
