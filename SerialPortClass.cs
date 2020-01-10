using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using log4net;

namespace UartCollect
{
    public class SerialPortClass
    {
        //日志初始化
        private static readonly ILog Log = LogManager.GetLogger("RollingLogFileAppender");
        private SerialPort Com;
        private bool SerialPortIsReceiving = false;
        private List<byte> buffer;

        public readonly string ComName;
        public readonly int BaudRate;
        public bool opened;
        public string ReceiveHead;

        public bool ReceiveFlag = false;
        public string ReceiveDataBuff = string.Empty;
        public string ReceiveData = string.Empty;
        public int ReceiveDataLen;
        public string ReceiveVerify = string.Empty;
        public int ReceiveVerifyLen;

        public SerialPortClass(string _comname, int _baudrate,SerialPort com)
        {
            ComName = _comname;
            BaudRate = _baudrate;
            buffer = new List<byte>(4096);
            Com = com;
        }
        //打开串口
        public bool Open()
        {
            if (Com.IsOpen == true)
            {
                opened = true;
                return opened;
            }
            try
            {
                //设置串口相关属性
                Com.PortName = ComName;
                Com.BaudRate = BaudRate;
                Com.Parity = Parity.None;
                Com.DataBits = 8;
                Com.StopBits = StopBits.One;
                Com.RtsEnable = false;
                Com.DtrEnable = false;
                //Com.DataReceived -= serialPort_DataReceivedEventHandler;
                Com.DataReceived += serialPort_DataReceivedEventHandler;
                Com.Encoding = Encoding.Default;
                //开启串口
                Com.Open();
                if (Com.IsOpen)
                {
                    opened = true;
                    return opened;
                }
                else
                {
                    opened = false;
                    return opened;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return false;
            }
        }
        // 此函数将编码后的消息传递给串口
        public bool SendData(byte[] data)
        {
            if (Com.IsOpen)
            {
                try
                {
                    return senddata(data);
                }
                catch (Exception ex)
                {
                    Log.Error("发送命令错误：" + ex.ToString());
                    return false;
                }
            }
            else
            {
                Log.Info("串口未开启!");
                return false;
            }
        }
        // 此函数将编码后的消息传递给串口
        private bool senddata(byte[] data)
        {
            try
            {
                //将消息传递给串口
                while (SerialPortIsReceiving)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                Com.Write(data, 0, data.Length);
                ReceiveData = string.Empty;
                ReceiveFlag = false;
                //清空缓存
                buffer.Clear();
                ReceiveDataBuff = string.Empty;
                return true;
            }catch(Exception ex){
                Log.Error("将消息传递给串口异常：\r\n" + ex.ToString());
                return false;
            }
        }
        //收到数据处理 暂未使用
        public void Com_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPortIsReceiving = true;
            try
            {
                int data;
                while (Com.BytesToRead >= 0)
                {
                    data = 0x00;
                    data = Com.ReadByte();
                    ReceiveDataBuff += data.ToString("X").PadLeft(2, '0');
                    if (ReceiveDataBuff.Length == ReceiveDataLen)
                    {
                        _Veryfy(ReceiveVerify, ReceiveDataBuff);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ReceiveFlag = false;
                Log.Debug("接收数据错误:" + ex.ToString());
            }
            finally
            {
                SerialPortIsReceiving = false;
            }
        }
        
        //关闭串口
        public bool ClosePort()
        {
            if (!Com.IsOpen)
            {
                opened = false;
                return true;
            }
            try
            {
                Com.DataReceived -= serialPort_DataReceivedEventHandler;
                while (SerialPortIsReceiving)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                Com.Close();
                Com.Dispose();
                if (Com.IsOpen)
                {
                    return false;
                }
                else
                {
                    opened = false;
                    Log.Info("串口关闭成功" + ComName + ":" + BaudRate);
                    return true;
                }
            }
            catch(Exception ex)
            {
                Log.Error("关闭串口失败"+ex.ToString());
                return false;
            }
        }
        //数据验证选择器
        private void _Veryfy(string method, string data)
        {
            switch (method)
            {
                case "CRC16":
                    VeryfyCrc16(data);
                    break;
                case "HEAD":
                    VeryfyHead(data);
                    break;
                default:
                    VeryfyTrue(data);//跳过验证
                    break;
            }
        }
        //CRC16验证
        public void VeryfyCrc16(string data)
        {
            if (CRC16.RecCRC16(data))
            {
                VeryfyTrue(data);//验证通过
            }
        }
        //Head验证
        public void VeryfyHead(string data)
        {
            string head = data.Substring(0, ReceiveVerifyLen);
            if (string.Equals(head, ReceiveHead))
            {
                VeryfyTrue(data);//验证通过
            }
            else
            {
                Log.Info("VeryfyHead ERROR,head/ReceiveHead:" + head + "/" + ReceiveHead + "/" + data);
            }
        }
        //完成验证
        public void VeryfyTrue(string data)
        {
            ReceiveData = data;
            ReceiveFlag = true;
        }
        /// <summary>
        /// 串口数据接收事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serialPort_DataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPortIsReceiving = true;
            try
            {
                int n = Com.BytesToRead;
                byte[] buf = new byte[n];
                Com.Read(buf, 0, n);
                //1.缓存数据           
                buffer.AddRange(buf);
                //2.完整性判断         
                if (buffer.Count == ReceiveDataLen)
                {
                    byte[] readBuffer = new byte[buffer.Count];
                    buffer.CopyTo(0, readBuffer, 0, buffer.Count);
                    _Veryfy(ReceiveVerify, ConvertUtilClass.byteTostring(readBuffer));
                    GC.KeepAlive(readBuffer);
                }
                GC.KeepAlive(buf);
                GC.KeepAlive(buffer);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            finally {
                SerialPortIsReceiving = false;
            }
        }
    }
}
