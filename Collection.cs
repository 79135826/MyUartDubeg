using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using log4net;
using System.IO.Ports;
using UartCollect.Headle;

namespace UartCollect
{
    //采集类////////////////////////////////////////////////////////////////////////////////////////////////////////
    class Collection
    {
        //日志初始化
        public static readonly ILog Log = LogManager.GetLogger("RollingLogFileAppender");
        private Form1 form1;
        private Thread thread;
        public event EventHandler<EventArgs> serailstop;
        private bool start;//内部开关
        private SerialPortClass serialport;

        private readonly string ComName;
        private readonly int BaudRate;
        private readonly int TimeOut;
        private readonly Dictionary<int, Dictionary<string, string>> DIC;
        public Collection(string _ComName, int _BaudRate, Dictionary<int, Dictionary<string, string>> _dic, Thread th,Form1 f)
        {
            ComName = _ComName;
            BaudRate = _BaudRate;
            DIC = _dic;
            form1 = f;
            thread = th;
        }
        public void SetStatusMsg(string msg)
        {
            try
            {
                form1.txtTest1 = msg;
            }
            catch(Exception ex){
                Log.Error("设置状态栏消息异常："+ex.ToString());
            }
        }
        //终止
        private void close_Collect()
        {
            start = false;
            GlobalData.STOP = true;
            GlobalData.START = false;
            serailstop(this, EventArgs.Empty);


        }
        //采集程序串口初始化
        public void go_Collect()
        {
            if (!GlobalData.START)
            {
                return;
            }
            bool success=false;
            try
            {
                //初始化串口并开启
                using (SerialPort Com = new SerialPort())
                {
                    serialport = new SerialPortClass(ComName, BaudRate, Com);
                    if (serialport.Open())
                    {
                        Log.Info("串口开启:" + ComName + " " + BaudRate);
                        success = true;
                        start = true;//内部开关
                        run_WhileCollect();
                    }
                    else
                    {
                        Log.Warn("串口开启失败:" + ComName + " " + BaudRate);
                        return;
                    }
                    GC.KeepAlive(serialport);
                    GC.KeepAlive(Com);
                }
                GC.KeepAlive(this);
            }
            catch (Exception ex)
            {
                Log.Error("串口开启失败:" + ex.ToString());
                success = false;
            }
            finally {
                if (!success)
                {
                    SetStatusMsg("串口异常:" + ComName + " " + BaudRate);
                    close_Collect();
                }
            }

        }
        //循环运行采集程序
        private void run_WhileCollect()
        {
            //Stopwatch sw = new Stopwatch();
            int j = 0;
            try
            {
                Stopwatch sw = new Stopwatch();
                while (serialport.opened)
                {
                    if (!GlobalData.START || start == false)
                    {
                        if (serialport.ClosePort())
                        {
                            GlobalData.STOP = true;
                        }
                        else
                        {
                            Thread.Sleep(1);
                        }
                    }
                    else
                    {
                        SetStatusMsg("数据采集正在进行：" + j);
                        sw.Start();
                        run_PollingCommand(DIC);
                        long RunPollTimes=0;
                        while (RunPollTimes < GlobalData.RefRate)
                        {
                            RunPollTimes = sw.ElapsedMilliseconds;
                            Thread.Sleep(1);
                        }
                        sw.Stop();
                        sw.Reset();
                        j++;
                        GlobalData.STOP = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("循环运行采集程序异常：" + ex.ToString());
                close_Collect();
            }
        }
        private void run_PollingCommand(Dictionary<int, Dictionary<string, string>> polldic)
        {
            bool revstate = false;
            bool success = false;
            Stopwatch sw = new Stopwatch();
            long i = 0;
            for (int j = 0; j < polldic.Count; j++) //for
            {
                if (!GlobalData.START) { start = false; }
                if (!start || !serialport.opened) { break; }
                if (GlobalData.FirstCommandCount > 0)//如果有优先命令，那么就先执行
                {
                    Exec_FirstCommand();
                }
                var item = polldic.ElementAt(j);  //for
                int count = item.Key;
                revstate = false;
                try
                {
                    int timeout = GlobalData.TimeOut;//接收数据超时时间：毫秒
                    if (timeout < 1) { timeout = 200; }
                    int.TryParse(polldic[count]["DataLen"], out int datalen);
                    serialport.ReceiveDataLen = (datalen+10)/2;
                    serialport.ReceiveVerify = "CRC16";//数据验证方法
                    serialport.ReceiveHead = "010302";
                    serialport.ReceiveVerifyLen = 6;
                    string command = polldic[count]["Command"];
                    byte[] datas = ConvertUtilClass.HexstrToByte(command);
                    if (serialport.SendData(datas))
                    {
                        //接收数据延时
                        sw.Start();
                        Thread.Sleep(1);
                        int y = 0;
                        while (sw.ElapsedMilliseconds < timeout && start)
                        {
                            if (serialport.ReceiveFlag)
                            {
                                revstate = true;
                                //接收到正确的数据
                                polldic[count]["HexData"] = serialport.ReceiveData;
                                do_DataDecode(count, polldic[count]);//数据解析
                                break;
                            }
                            else
                            {
                                Thread.Sleep(1);
                            }
                            y++;
                        }
                        i = sw.ElapsedMilliseconds;
                        sw.Stop();
                        sw.Reset();
                        if (revstate == false)
                        {
                            Log.Error("此命令未收到回应，编号:" + command+ ",DataLen:" + polldic[count]["DataLen"]);
                        }
                        success = true;
                        Log.Info("serialport.ReceiveData：" + serialport.ReceiveData);
                    }
                    else {
                        success = false;
                        break;
                    }
                }
                catch(Exception ex)
                {
                    Log.Error("发送轮询命令错误:" + ex.ToString());
                    success = false;
                }
                finally
                {
                    if (!success)
                    {
                        SetStatusMsg("串口异常！");
                        close_Collect();
                    }
                }
            }
            GC.KeepAlive(sw);

        }
        //执行数据解析
        //dic：轮询存储器
        private void do_DataDecode(int id, Dictionary<string, string> dic)
        {
            string[] data = { id.ToString(), dic["HexData"], dic["DataLen"], dic["Decode"] };
            if (string.IsNullOrEmpty(data[1])) { return; }
            ThreadPool.QueueUserWorkItem(new WaitCallback(convert), data);
        }
        private void convert(object data)
        {
            try
            {
                string[] arr = (string[])data;
                int id = Convert.ToInt32(arr[0]);
                string HexData = arr[1];
                int DataLen = Convert.ToInt32(arr[2]);
                string Decode = arr[3];
                string ss = arr[1].Substring(6, DataLen);
                string Value = string.Empty;
                switch (Decode)
                {
                    case "FLOAT-0.001":
                        Value= Convert.ToDouble(Convert.ToInt32(ss, 16)*0.001).ToString();
                        break;
                    case "FLOAT-0.01":
                        Value = Convert.ToDouble(Convert.ToInt32(ss, 16) * 0.01).ToString();
                        break;
                    case "FLOAT-0.1":
                        Value = Convert.ToDouble(Convert.ToInt32(ss, 16) * 0.1).ToString();
                        break;
                    case "INT-10":
                        Value = (Convert.ToInt32(ss, 16) * 10).ToString();
                        break;
                    case "INT":
                        Value = Convert.ToInt32(ss, 16).ToString();
                        break;
                    case "STRING":
                        Value = ConvertUtilClass.HexToChar(ss);
                        break;
                    case "FLOAT-IEEE754":
                        Value = ConvertUtilClass.HexToFloat(ss).ToString();
                        break;
                    default:
                        Value = (Convert.ToInt32(ss, 16)).ToString();
                        break;
                }
                GlobalData.DevicesDataStorage[0][id]["Value"] = Value;
                GlobalData.DevicesDataStorage[0][id]["HexData"] = HexData;
                GlobalData.DevicesDataStorage[0][id]["RefTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//更新时间
            }catch(Exception ex)
            {
                string msg = "数据转存错误：" + ex.ToString();
                UIAction.AppendLog(msg);
                Log.Fatal(msg);
            }
        }
        //执行优先命令
        private void Exec_FirstCommand()
        {
            bool success = false;
            try
            {
                bool revstate;
                Stopwatch sw = new Stopwatch();
                long i;
                for (int j = 0; j < GlobalData.FirstCommandDictionary.Count; j++)//遍历设备
                {
                    if (!GlobalData.START) { start = false; }
                    if (!start || !serialport.opened) { break; }
                    var item = GlobalData.FirstCommandDictionary.ElementAt(j);
                    int devid = item.Key;
                    for (int l = 0; l < GlobalData.FirstCommandDictionary[devid].Count; l++)//遍历可写命令
                    {
                        var item2 = GlobalData.FirstCommandDictionary[devid].ElementAt(l);
                        string pramaname = item2.Key;
                        if (!GlobalData.FirstCommandDictionary[devid].ContainsKey(pramaname)) { Log.Info(pramaname + ":此设备无此参数"); continue; }
                        if (GlobalData.FirstCommandDictionary[devid][pramaname]["Finish"] == "1" || GlobalData.FirstCommandDictionary[devid][pramaname]["Command"].Length == 0) continue;//命令已经完成则下一个
                        if (GlobalData.FirstCommandDictionary[devid][pramaname]["Status"] == "1")
                        {
                            if (!string.IsNullOrEmpty(GlobalData.FirstCommandDictionary[devid][pramaname]["SendTime"]))
                            {
                                DateTime.TryParse(GlobalData.FirstCommandDictionary[devid][pramaname]["SendTime"], out DateTime sendtime);
                                TimeSpan timeSpan = DateTime.Now - sendtime;
                                int.TryParse(GlobalData.FirstCommandDictionary[devid][pramaname]["ErrorWait"], out int errorwait);
                                if (timeSpan.TotalSeconds > errorwait)
                                {
                                    GlobalData.FirstCommandDictionary[devid][pramaname]["Status"] = "0";
                                    GlobalData.FirstCommandDictionary[devid][pramaname]["Error"] = "0";
                                    Log.Info("到达错误等待时间:" + GlobalData.FirstCommandDictionary[devid][pramaname]["Command"]);
                                }
                                else
                                {
                                    Thread.Sleep(1);
                                    continue;
                                }
                            }
                        }
                        revstate = false;
                        int.TryParse(GlobalData.FirstCommandDictionary[devid][pramaname]["TimeOut"], out int timeout);//接收数据超时时间：毫秒
                        if (TimeOut != 0) { timeout = TimeOut; }
                        int.TryParse(GlobalData.FirstCommandDictionary[devid][pramaname]["DataLen"], out int datalen);
                        serialport.ReceiveDataLen = datalen;
                        serialport.ReceiveVerify = GlobalData.FirstCommandDictionary[devid][pramaname]["Verify"];//数据验证方法
                        if (string.Equals(GlobalData.FirstCommandDictionary[devid][pramaname]["Verify"], "HEAD"))
                        {
                            serialport.ReceiveHead = GlobalData.FirstCommandDictionary[devid][pramaname]["VerifyFlag"];
                            serialport.ReceiveVerifyLen = GlobalData.FirstCommandDictionary[devid][pramaname]["VerifyFlag"].Length;
                        }
                        GlobalData.FirstCommandDictionary[devid][pramaname]["SendTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string command = GlobalData.FirstCommandDictionary[devid][pramaname]["Command"];
                        byte[] datas = ConvertUtilClass.HexstrToByte(command);
                        int resend = 0;
                    RESEND://重新发送
                        if (serialport.SendData(datas))
                        {
                            //接收数据延时
                            resend++;
                            sw.Start();
                            while (sw.ElapsedMilliseconds < timeout && start)
                            {
                                if (serialport.ReceiveFlag)
                                {
                                    revstate = true;
                                    //接收到正确的数据
                                    GlobalData.FirstCommandDictionary[devid][pramaname]["HexData"] = serialport.ReceiveData;
                                    GlobalData.FirstCommandDictionary[devid][pramaname]["Error"] = "0";
                                    GlobalData.FirstCommandDictionary[devid][pramaname]["Status"] = "0";
                                    GlobalData.FirstCommandDictionary[devid][pramaname]["RecTime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    bool decoderesult = Exec_FirstCommand_DataConvert(command, serialport.ReceiveData, GlobalData.FirstCommandDictionary[devid][pramaname]["Decode"]);//数据解析结果
                                    if (decoderesult)//如果数据解析结果为真则移除此命令
                                    {
                                        SetStatusMsg("设备" +GlobalData.FirstCommandDictionary[devid][pramaname]["Remark"] + "命令正确执行！");//发送通知
                                        if (GlobalData.FirstCommandDictionary[devid][pramaname]["PramaName"].Length > 0)
                                        {
                                            GlobalData.DevicesDataStorage[0][devid]["Value"] = GlobalData.FirstCommandDictionary[devid][pramaname]["Value"];//更新值
                                        }
                                        GlobalData.FirstCommandDictionary[devid][pramaname]["Finish"] = "1";//命令完成
                                        if (GlobalData.FirstCommandCount > 0)GlobalData.FirstCommandCount -= 1; //更新数量
                                    }
                                    break;
                                }
                                else
                                {
                                    Thread.Sleep(10);
                                }
                            }
                            i = sw.ElapsedMilliseconds;
                            sw.Stop();
                            sw.Reset();
                            if (revstate == false)
                            {
                                if (resend == 1)
                                {
                                    SetStatusMsg("重新发送");
                                    goto RESEND;//重新发送
                                }
                                GlobalData.FirstCommandDictionary[devid][pramaname]["Finish"] = "1";
                                GlobalData.FirstCommandDictionary[devid][pramaname]["Error"] = "1";//改为错误状态
                                string Remark = GlobalData.FirstCommandDictionary[devid][pramaname]["Remark"];
                                SetStatusMsg(Remark + "通讯异常！");
                                if (GlobalData.FirstCommandCount > 0) GlobalData.FirstCommandCount -= 1; //更新数量

                            }
                            success = true;
                        }
                        else
                        {
                            success = false;
                            break;
                        }
                    }
                }
                success = true;
                GC.KeepAlive(success);
                GC.KeepAlive(sw);
            }
            catch (Exception ex)
            {
                Log.Error("通讯错误:" + ex.ToString());
                success = false;
            }
            finally
            {
                if (!success)
                {
                    SetStatusMsg("串口异常！");
                    close_Collect();
                }
            }
        }
        private bool Exec_FirstCommand_DataConvert(string command, string data, string method)
        {
            bool result = false;
            try
            {
                switch (method)
                {
                    case "SET_DO1":
                    case "SET_DO2":
                    case "SET_DO3":
                    case "SET_DO4":
                        if (command.Substring(0, 12) == data.Substring(0, 12))
                        {
                            result= true;
                        }
                        break;

                }
                return result;
            }
            catch (Exception ex)
            {
                Log.Debug("数据解码错误:" + ex.ToString());
                return result;
            }
        }
    }

}
