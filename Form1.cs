using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using log4net;
using System.Collections.Concurrent;
using System.IO;
using UartCollect.Util;
using UartCollect.Headle;
using MQTTnet.Client;
using System.Threading.Tasks;
using System.Timers;
using MQTTnet;
using System.Net;
using System.Linq;
using UartCollect.Core;

namespace UartCollect
{
    public partial class Form1 : Form
    {       
        //日志初始化
        public static readonly ILog Log = LogManager.GetLogger("RollingLogFileAppender");

        //定义命令字典
        private static Dictionary<int, string[]> Rscmd;
        private static Dictionary<int, Dictionary<string, string>> PollDic;
        private static DateTime lastrecode;
        private static string autorun;
        private static DataTable combodt_comport,combodt_recordrate, combodt_baudrate, combodt_fileformat, collectfile_dt;
        //数据保存目录
        private string DataDir;
        //采集设置文件目录
        private string CollectConfigDir= @"CollectConfig/";
        private string CollectConfigFile=string.Empty;
        //MQTT参数
        private IMqttClient mqttClient = null;
        private bool isReconnect = true;

        private string Params=string.Empty;
        //开始
        public Form1()
        {
            InitializeComponent();
            DataDir = string.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (DateTime.Now > Convert.ToDateTime("2020-4-20 23:59:59"))
            {
                MessageBox.Show("软件使用已到期，请联系QQ：121671895！", "Error");
                Close();
                return;
            }
            string[] str = SerialPort.GetPortNames();
            if (str == null)
            {
                MessageBox.Show("本机没有串口！", "Error");
                return;
            }
            

            if (AppConfig.GetAppConfig("DATADIR").Length > 0)
            {
                DataDir = AppConfig.GetAppConfig("DATADIR");
                if (!Directory.Exists(DataDir))
                {
                    DataDir = System.AppDomain.CurrentDomain.BaseDirectory + "Data\\";
                    if (!Directory.Exists(DataDir))
                    {
                        Directory.CreateDirectory(DataDir);
                        Log.Info("创建数据目录："+ DataDir);
                    }
                }
            }
            else
            {
                DataDir = System.AppDomain.CurrentDomain.BaseDirectory + "Data\\";
                if (!Directory.Exists(DataDir))
                {
                    Directory.CreateDirectory(DataDir);
                    Log.Info("创建数据目录：" + DataDir);
                }
            }
            AppConfig.SetAppConfig("DATADIR", DataDir);
            textBox_datadir.Text = DataDir;

            AppendLog("程序启动:" + DateTime.Now);
            int index = 0, i = 0;
            //添加串口项目
            combodt_comport = new DataTable();
            combodt_comport.Columns.Add("Text", Type.GetType("System.String"));
            combodt_comport.Columns.Add("Value", Type.GetType("System.String"));
            string comport = AppConfig.GetAppConfig("COMPORT");

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {//获取有多少个COM口  
                //comboBox_SelectCom.Items.Add(s);
                combodt_comport.Rows.Add(s, s);
                if (comport == s) { index = i; }
                i++;
            }
            comboBox_SelectCom.DataSource = combodt_comport;
            comboBox_SelectCom.SelectedIndex = index;
            GC.KeepAlive(combodt_comport);
            
            //串口设置默认选择项  

            init_dataGridView_data();
            //为tableLayoutPanel1增加缓冲，解决TableLayoutPanel控件闪烁
            string recordrate = AppConfig.GetAppConfig("RECORDRATE");
            i = 0;
            index = 0;
            combodt_recordrate = new DataTable();
            combodt_recordrate.Columns.Add("Text", Type.GetType("System.String"));
            combodt_recordrate.Columns.Add("Value", Type.GetType("System.String"));
            combodt_recordrate.Rows.Add("1分钟", "60");
            combodt_recordrate.Rows.Add("5分钟", "300");
            combodt_recordrate.Rows.Add("10分钟", "600");
            combodt_recordrate.Rows.Add("15分钟", "900");
            combodt_recordrate.Rows.Add("30分钟", "1800");
            combodt_recordrate.Rows.Add("1小时", "3600");
            combodt_recordrate.Rows.Add("2小时", "7200");
            combodt_recordrate.Rows.Add("4小时", "14400");
            combodt_recordrate.Rows.Add("12小时", "43200");
            foreach (DataRow row in combodt_recordrate.Rows)
            {
                if (recordrate == row[1].ToString()) { index = i; break; }
                i++;
            }
            GC.KeepAlive(recordrate);
            comboBox_record.DataSource = combodt_recordrate;
            GC.KeepAlive(combodt_recordrate);
            comboBox_record.SelectedIndex = index;
            //波特率
            string baudrate = AppConfig.GetAppConfig("BAUDRATE");
            i = 0;
            index = 0;
            combodt_baudrate = new DataTable();
            combodt_baudrate.Columns.Add("Text", Type.GetType("System.String"));
            combodt_baudrate.Columns.Add("Value", Type.GetType("System.String"));
            combodt_baudrate.Rows.Add("2400", "2400");
            combodt_baudrate.Rows.Add("4800", "4800");
            combodt_baudrate.Rows.Add("9600", "9600");
            combodt_baudrate.Rows.Add("19200", "19200");
            combodt_baudrate.Rows.Add("115200", "115200");
            foreach (DataRow row in combodt_baudrate.Rows)
            {
                if (baudrate == row[1].ToString()) { index = i; break; }
                i++;
            }
            Box_BaudRate.DataSource = combodt_baudrate;
            Box_BaudRate.SelectedIndex = index;
            GC.KeepAlive(baudrate);
            GC.KeepAlive(combodt_baudrate);
            GC.KeepAlive(index);
            //保存的文件格式
            string fileformat =  AppConfig.GetAppConfig("FILEFORMAT");
            i = 0;
            index = 0;
            combodt_fileformat = new DataTable();
            combodt_fileformat.Columns.Add("Text", Type.GetType("System.String"));
            combodt_fileformat.Columns.Add("Value", Type.GetType("System.String"));
            combodt_fileformat.Rows.Add("TXT", "TXT");
            combodt_fileformat.Rows.Add("CSV", "CSV");
            combodt_fileformat.Rows.Add("MSSQL数据库", "MSSQL");
            combodt_fileformat.Rows.Add("MYSQL数据库", "MYSQL");
            combodt_fileformat.Rows.Add("HTTP网关", "HTTP");
            foreach (DataRow row in combodt_fileformat.Rows)
            {
                if (fileformat == row[1].ToString()) { index = i; break; }
                i++;
            }
            comboBox_fileformat.DataSource = combodt_fileformat;
            comboBox_fileformat.SelectedIndex = index;
            GC.KeepAlive(baudrate);
            GC.KeepAlive(combodt_fileformat);
            GC.KeepAlive(index);
            //采集参数文件
            string collectconfigfile = AppConfig.GetAppConfig("COLLECTCONFIGFILE");
            i = 0;
            index = 0;
            collectfile_dt = new DataTable();
            collectfile_dt.Columns.Add("Text", Type.GetType("System.String"));
            collectfile_dt.Columns.Add("Value", Type.GetType("System.String"));

            DirectoryInfo root = new DirectoryInfo(CollectConfigDir);
            FileInfo[] files = root.GetFiles();
            foreach (FileInfo item in files)
            {
                collectfile_dt.Rows.Add(item.Name, item.FullName);
            }

            foreach (DataRow row in collectfile_dt.Rows)
            {
                if (collectconfigfile == row[1].ToString()) { index = i; break; }
                i++;
            }
            comboBox_collectfile.DataSource = collectfile_dt;
            comboBox_collectfile.SelectedIndex = index;
            //读取参数
            GlobalData.RefRate = Convert.ToInt32(AppConfig.GetAppConfig("REFRATE"));
            textBox_RefRate.Text = GlobalData.RefRate.ToString();
            GlobalData.TimeOut = Convert.ToInt32(AppConfig.GetAppConfig("TIMEOUT"));
            textBox_TimeOut.Text = GlobalData.TimeOut.ToString();
            textBox_deverid.Text = AppConfig.GetAppConfig("DEVERID");
            textBox_httpurl.Text = AppConfig.GetAppConfig("HTTPURL");
            //读取阿里物联网平台参数
            textBox_TcpServer.Text=AppConfig.GetAppConfig("ALIYUN_IOT_TcpServer");
            textBox_TcpServerPort.Text=AppConfig.GetAppConfig("ALIYUN_IOT_TcpServerPort");
            textBox_Signmethod.Text=AppConfig.GetAppConfig("ALIYUN_IOT_Signmethod");
            textBox_DeviceName.Text=AppConfig.GetAppConfig("ALIYUN_IOT_DeviceName");
            textBox_DeviceSecret.Text=AppConfig.GetAppConfig("ALIYUN_IOT_DeviceSecret");
            textBox_ProductKey.Text=AppConfig.GetAppConfig("ALIYUN_IOT_ProductKey");
            //初始化对象
            UnInit();
            //是否自动运行
            autorun = AppConfig.GetAppConfig("AUTORUN");
            if (autorun == "1")
            {
                Init();
                checkBox_autorun.Checked = true;
                start_work(comboBox_SelectCom.Text, Box_BaudRate.Text);
            }
        }

        private void Init()
        {
            if (Rscmd != null) return;
            Rscmd = new Dictionary<int, string[]>();
            //采集数据
            //格式：{"寄存器地址","寄存器个数","返回数据长度","解析方法","单位","名称","列名","数据类型"}
            /*
            Rscmd.Add(0, new string[]{"20", "1","HEXTOFLOAT0.001","KHZ", "频率","pinlv","float" });
            Rscmd.Add(1, new string[]{"21", "1", "HEXTOFLOAT0.01", "S", "超声时间", "shijian", "float" });
            Rscmd.Add(2, new string[]{"22", "1", "HEXTOFLOAT0.01", "KJ", "超声能量", "nengliang", "float" });
            Rscmd.Add(3, new string[]{"23", "1", "HEXTOFLOAT0.01", "MM", "绝对深度", "jueduishendu", "float" });
            Rscmd.Add(4, new string[]{"24", "1", "HEXTOFLOAT0.01", "MM", "相对深度", "xiangduishendu", "float" });
            Rscmd.Add(5, new string[]{"25", "1", "HEXTOFLOAT0.001", "KW", "功率", "gonglv", "float" });
            Rscmd.Add(6, new string[]{"26", "1", "HEXTOINT1", "件", "计数", "jishu", "int" });
            Rscmd.Add(8, new string[]{"28", "1", "HEXTOFLOAT0.01", "KG", "压力", "fali", "float" });
            Rscmd.Add(9, new string[]{"29", "2", "HEXTOSTRING", "", "结果", "jieguo", "float" });
            Rscmd.Add(10,new string[]{"99", "1", "HEXTOINT1", "", "状态", "zhuantai", "varchar" });
            */
            string data = FileHelper.FileToString(comboBox_collectfile.SelectedValue.ToString(), Encoding.Default);
            string[] dataarr = data.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (dataarr.Length > 1)
            {
                for (int j = 1; j < dataarr.Length; j++)
                {
                    string d = dataarr[j].TrimEnd(',');
                    if (d.Length > 0) Rscmd.Add(j - 1, d.Split(','));
                }
            }

            if (Rscmd.Count < 1)
            {
                MessageBox.Show("采集参数未设置！");
                Rscmd = null;
                return;
            }
            //Rscmd.Add(0, new string[] { "7", "2", "HEXTOFLOAT0.01", "℃", "温度", "wendu", "float" });
            //Rscmd.Add(1, new string[] { "9", "2", "HEXTOFLOAT0.01", "Mpa", "压力", "yali", "float" });
            //Rscmd.Add(2, new string[] { "13", "2", "HEXTOFLOAT0.01", "立方/H", "流量", "liuliang", "float" });

            string devid = Tools.intTohex(textBox_deverid.Text, 2);
            int devaddress = int.Parse(textBox_deverid.Text);

            //初始化设备参数存储
            GlobalData.DevicesDataStorage = new ConcurrentDictionary<int, Dictionary<int, Dictionary<string, string>>>();
            GlobalData.DevicesDataStorage.TryAdd(0, new Dictionary<int, Dictionary<string, string>>());

            GlobalData.FirstCommandDictionary = new ConcurrentDictionary<int, Dictionary<string, Dictionary<string, string>>>();
            GlobalData.FirstCommandDictionary.TryAdd(devaddress, new Dictionary<string, Dictionary<string, string>>());

            GlobalData.FirstCommandInfoDictionary = new ConcurrentDictionary<int, Dictionary<string, Dictionary<string, string>>>();
            GlobalData.FirstCommandInfoDictionary.TryAdd(devaddress, new Dictionary<string, Dictionary<string, string>>());

            PollDic = new Dictionary<int, Dictionary<string, string>>();
            foreach (var item in Rscmd)
            {
                if (item.Value[7] == "Y")
                {
                    string decode=item.Value[2];
                    GlobalData.FirstCommandInfoDictionary[devaddress].Add(decode, new Dictionary<string, string>());
                    GlobalData.FirstCommandInfoDictionary[devaddress][decode].Add("FunCode", item.Value[8]); //功能码
                    GlobalData.FirstCommandInfoDictionary[devaddress][decode].Add("Register", item.Value[0]); //寄存器地址
                    GlobalData.FirstCommandInfoDictionary[devaddress][decode].Add("Unit", item.Value[3]); //单位
                    GlobalData.FirstCommandInfoDictionary[devaddress][decode].Add("Remark", item.Value[4]); //说明
                    GlobalData.FirstCommandInfoDictionary[devaddress][decode].Add("DataType", item.Value[6]); //数据类型
                                                                                                            //初始化优先命令
                    if (!GlobalData.FirstCommandDictionary[devaddress].ContainsKey(decode))
                    {
                        GlobalData.FirstCommandDictionary[devaddress].Add(decode, new Dictionary<string, string>());
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("Command", string.Empty);//命令
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("Decode", decode);//解码方法
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("DataLen", string.Empty);//数据长度
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("TimeOut", "500");
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("SerialPort", comboBox_SelectCom.SelectedValue.ToString());//串口名称
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("Error", "0");//错误次数
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("ErrorWait", "60");//错误等待时间：秒
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("Finish", "0");//是否完成
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("HexData", "");//收到的16进制数据
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("RecTime", "");//最后收到数据时间"yyyy-MM-dd HH:mm:ss"
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("SendTime", "");//最后发送命令时间"yyyy-MM-dd HH:mm:ss"
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("Interval", "0");//采集频率
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("Verify", string.Empty);//数据校验方法
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("VerifyFlag", string.Empty);//数据校验标志
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("Remark", item.Value[4]);
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("Status", "0");//状态:0正常，1错误
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("PramaName", string.Empty);//对应采集的参数名称
                        GlobalData.FirstCommandDictionary[devaddress][decode].Add("Value", string.Empty);//写入的值
                    }
                }
                else
                {
                    int.TryParse(item.Value[0], out int sid);
                    //sid--;//寄存器地址-1
                    string funcode = item.Value[8];
                    string command = devid + " " + funcode + " " + Tools.intTohex(sid.ToString(), 4) + " " +
                                     Tools.intTohex(item.Value[1], 4); //设备地址+采集项目+CRC16校验
                    command += CRC16.CRCCalc(command);
                    command = command.Replace(" ", "");
                    GlobalData.DevicesDataStorage[0].Add(item.Key, new Dictionary<string, string>());
                    GlobalData.DevicesDataStorage[0][item.Key].Add("Command", command); //值
                    GlobalData.DevicesDataStorage[0][item.Key].Add("Value", ""); //值
                    GlobalData.DevicesDataStorage[0][item.Key].Add("Unit", item.Value[3]); //单位
                    GlobalData.DevicesDataStorage[0][item.Key].Add("Remark", item.Value[4]); //说明
                    GlobalData.DevicesDataStorage[0][item.Key].Add("HexData", ""); //更新时间
                    GlobalData.DevicesDataStorage[0][item.Key].Add("RefTime", ""); //更新时间
                    GlobalData.DevicesDataStorage[0][item.Key].Add("ColumnName", item.Value[5]); //列名
                    GlobalData.DevicesDataStorage[0][item.Key].Add("DataType", item.Value[6]); //数据类型

                    PollDic.Add(item.Key, new Dictionary<string, string>());
                    PollDic[item.Key].Add("Command", command); //命令
                    PollDic[item.Key].Add("HexData", "");
                    PollDic[item.Key].Add("Decode", item.Value[2]); //解码方法
                    PollDic[item.Key].Add("DataLen", (Convert.ToInt32(item.Value[1]) * 4).ToString()); //数据长度(16进制字符串长度)
                    PollDic[item.Key].Add("TimeOut", textBox_TimeOut.Text);
                }
            }
        }

        private void UnInit()
        {
            GlobalData.DevicesDataStorage = null;
            GlobalData.FirstCommandDictionary = null;
            PollDic = null;
            Rscmd = null;
        }

        //调用采集线程
        Thread CollectionThread;
        private void using_CollectionThread(string comm, string baudrate)
        {
            lastrecode = DateTime.Now.AddSeconds(-57);//初始化最后记录时间
            //传递串口名称和波特率初始化
            int _baudrate = Convert.ToInt32(baudrate);
            Collection collection = new Collection(comm, _baudrate, PollDic, CollectionThread,this);
            collection.serailstop += tiger_stop_work;//绑定事件
            CollectionThread = new Thread(() => run_CollectionThread(collection));
            CollectionThread.TrySetApartmentState(ApartmentState.STA);
            //有对应串口则开启线程
            CollectionThread.Start();
            CollectionThread.IsBackground = true;
            timer_getthreadstatus.Enabled = true;
        }
        //开启采集线程
        private void run_CollectionThread(Collection collection)
        {
            //传递串口名称和波特率初始化
            collection.go_Collect();
        }
        //触发事件
       private void tiger_stop_work(object sender, EventArgs e)
       {
           BeginInvoke(new MethodInvoker(delegate
           {
               statues("停止工作",0);
               Log.Info("from:"+sender.ToString()+" 命令：停止工作");
               ToolStripMenuItem_go.Text = "运行";
               SetClose();
               timer_updateshow.Stop();
           }));
       }
        //停止工作
        public void stop_work()
        {
            timer_updateshow.Stop();
            if (GlobalData.START)
            {
                GlobalData.START = false;
                ToolStripMenuItem_go.Text = "运行";
                ToolStripMenuItem_go.BackColor = Color.Red;
                SetClose();
                statues("停止工作",0);
                UnInit();
            }
        }
        //开始工作
        public void start_work(string comname,string baudrate)
        {
            if (Rscmd == null)
            {
                string msg = "采集参数未设置！";
                AppendLog(msg);
                BeginInvoke(new MethodInvoker(delegate
                {
                    MessageBox.Show(msg);
                }));
                return;
            }

            BeginInvoke(new MethodInvoker(delegate
            {
                dataGridView_devdata.Rows.Clear();
            }));

            string postfix = comboBox_fileformat.SelectedValue.ToString();
            if (postfix == "MSSQL" || postfix == "MYSQL")
            {
                if (!CheckDBIsEnabled(postfix))
                {
                    string msg = "数据库连接不可用，请先设置好连接参数！" + AppConfig.GetAppConfig("MSSQL_CONNECTSTRING");
                    AppendLog(msg);
                    BeginInvoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show(msg);
                        openform_dbset();
                    }));
                    return;
                }
            }

            int.TryParse(comboBox_record.SelectedValue.ToString(), out int record);
            timer_updateshow.Start();
            if (!GlobalData.START)
            {
                GlobalData.START = true;
                using_CollectionThread(comname,baudrate);
                ToolStripMenuItem_go.Text = "停止";
                ToolStripMenuItem_go.BackColor = Color.Green;
                SetOpen();
                AppendLog("开始工作");
            }
        }
        /// <summary>
        /// 数据库连接设置 调用
        /// </summary>
        Form form_dbset = null;
        private void openform_dbset()
        {
            if (form_dbset == null || form_dbset.IsDisposed)
            {
                form_dbset = new FormDBSset
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                form_dbset.Show();
            }
            form_dbset.Activate();
        }
        /// <summary>
        /// 采集设置 调用
        /// </summary>
        Form form_registerset = null;
        private void openform_registerset()
        {
            if (form_registerset == null || form_registerset.IsDisposed)
            {
                form_registerset = new FormRegisterSet
                {
                    StartPosition = FormStartPosition.CenterScreen
                };
                form_registerset.Show();
            }
            form_registerset.Activate();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (GlobalData.START)
            {
                stop_work();
            }
            else
            {
                start_work(comboBox_SelectCom.Text, Box_BaudRate.Text);
            }
        }

        private void SetOpen()
        {
            comboBox_SelectCom.Enabled = false;
            Box_BaudRate.Enabled = false;
            comboBox_record.Enabled = false;
            checkBox_autorun.Enabled = false;
            textBox_RefRate.Enabled = false;
            textBox_TimeOut.Enabled = false;
        }
        private void SetClose()
        {
            comboBox_SelectCom.Enabled = true;
            Box_BaudRate.Enabled = true;
            comboBox_record.Enabled = true;
            checkBox_autorun.Enabled = true;
            textBox_RefRate.Enabled = true;
            textBox_TimeOut.Enabled = true;
        }
        //打开串口
        private void open_uart_btn_Click(object sender, EventArgs e)
        {
            
        }
        private void button_Send_Click(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_clear_rec_Click(object sender, EventArgs e)
        {
            
        }


        private void button5_Click(object sender, EventArgs e)
        {
        }
        public void statues(string msgtext,int type)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                if (type == 0) { toolStripStatusMsg.ForeColor = Color.Black; }
                if (type == 1) { toolStripStatusMsg.ForeColor = Color.DarkTurquoise; }
                if (type == 2) { toolStripStatusMsg.ForeColor = Color.Red; }
                toolStripStatusMsg.Text = msgtext;
                AppendLog(msgtext);
            }));
            
        }

        public void AppendLog(string msg)
        {
            try
            {
                Invoke((new Action(() =>
                {
                    richTextBox_log.AppendText(DateTime.Now.ToString("yyyy-M-d HH:mm:ss") + ":" + msg + "\r\n");
                })));
            }
            catch
            {
                
            }
            

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
        }
        //购买
        Form form_buy;
        private void buy_Click(object sender, EventArgs e)
        {
            form_buy = new buy();
            form_buy.Show();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                GlobalData.START = false;
                Util.Tools.WaitFor(0.02);
                SavePramas();
            }
            catch (Exception ex)
            {
                Log.Info(ex.ToString());
            }
        }
        //关于本软件
        Form form_about;
        private void about_Click(object sender, EventArgs e)
        {
            form_about = new about();
            form_about.Show();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        //刷新线程状态
        private void timer_getthreadstatus_Tick(object sender, EventArgs e)
        {
            if (CollectionThread != null)
            {
                string status=CollectionThread.ThreadState.ToString();
                if(status.Contains("Aborted")){
                    status = "正在中止";
                }
                if (status.Contains("Stopped"))
                {
                    status = "停止";
                }
                if(status.Contains("Background")){
                    status="运行";
                }
                toolStripStatusLabel_threadstatus.Text = status;
            }
        }
        //检查数据库是否可用
        private bool CheckDBIsEnabled(string postfix)
        {
            bool result = false;
            switch (postfix)
            {
                case "MSSQL":
                    string connstr = string.Format("server={0};uid={1};pwd={2};database={3}", AppConfig.GetAppConfig("DBSERVER_IP")+","+ AppConfig.GetAppConfig("DBSERVER_PORT"), AppConfig.GetAppConfig("DBSERVER_LOGINNAME"), AppConfig.GetAppConfig("DBSERVER_PASSWORD"), AppConfig.GetAppConfig("DBSERVER_DATABASE"));
                    AppConfig.SetAppConfig("MSSQL_CONNECTSTRING", connstr);
                    MSSQLHelper.ConnectionString = connstr;
                    result = MSSQLHelper.do_SqlConnectExists(10*1000);
                    break;
                case "MYSQL":
                    result = MSSQLHelper.do_SqlConnectExists(10);
                    break;
                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 记录数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_updateshow_Tick(object sender, EventArgs e)
        {
            show_dataGridView_devdata();
            TimeSpan ts = DateTime.Now - lastrecode;
            string postfix = comboBox_fileformat.SelectedValue.ToString();
            if (ts.TotalMilliseconds >= Convert.ToInt32(comboBox_record.SelectedValue)*1000)
            {
                switch (postfix)
                {
                    case "TXT":
                        Do_SaveDataByTXT();
                        break;
                    case "CSV":
                        Do_SaveDataByCSV();
                        break;
                    case "MSSQL":
                        Do_SaveDataByMSSQL();
                        break;
                    case "MYSQL":
                        Do_SaveDataByMYSQL();
                        break;
                    case "HTTP":
                        Do_SaveDataByHTTP();
                        break;
                    default:
                        Do_SaveDataByTXT();
                        break;
                }
                lastrecode = DateTime.Now;
            }
        }
        /// <summary>
        /// 保存数据 TXT格式
        /// </summary>
        private void Do_SaveDataByTXT()
        {
            if (GlobalData.Read_DevicesDataStorage == null) return;
            string postfix = comboBox_fileformat.SelectedValue.ToString();
            string filename = DateTime.Now.ToString("yyyy-MM-dd") + "." + postfix;
            string filePath = DataDir + filename;
            StringBuilder sb = new StringBuilder();
            foreach (var item in GlobalData.Read_DevicesDataStorage[0])
            {
                sb.Append(item.Value["Remark"] + "：" + item.Value["Value"] + " " + item.Value["Unit"] + ",");
            }
            sb.Append("\r\n");
            FileHelper.AppendText(filePath, sb.ToString());
            lastrecode = DateTime.Now;
        }
        /// <summary>
        /// 保存数据 CSV格式
        /// </summary>
        private void Do_SaveDataByCSV()
        {
            if (GlobalData.Read_DevicesDataStorage == null) return;
            string postfix = comboBox_fileformat.SelectedValue.ToString();
            string filename = DateTime.Now.ToString("yyyy-MM-dd") + "." + postfix;
            string filePath = DataDir + filename; ;
            StringBuilder sb = new StringBuilder();
            if (!FileHelper.IsExistFile(filePath))
            {
                foreach (var item in GlobalData.Read_DevicesDataStorage[0])
                {
                    sb.Append(item.Value["Remark"] + item.Value["Unit"] + ",");
                }
                sb.Append("\r\n");
            }
            foreach (var item in GlobalData.Read_DevicesDataStorage[0])
            {
                string val = item.Value["Value"];
                if (val.Length < 1) val = "0";
                sb.Append(val + ",");
            }
            sb.Append("\r\n");
            FileHelper.AppendText(filePath, sb.ToString());
            lastrecode = DateTime.Now;
        }
        /// <summary>
        /// 保存数据 MSSQL
        /// </summary>
        private void Do_SaveDataByMSSQL()
        {
            if (GlobalData.Read_DevicesDataStorage == null) return;
            string tablename = AppConfig.GetAppConfig("TABLENAME");
            if (tablename.Length < 1)
            {
                Log.Error("数据表名称未设置！");
                return;
            }
            if (!MSSQLHelper.TabExists(tablename))
            {
                StringBuilder creattable = new StringBuilder();
                creattable.Append("CREATE TABLE [dbo].[" + tablename + "](");
                creattable.Append("[Id] [int] IDENTITY(1,1) NOT NULL,");
                //creattable.Append("[DevicesId] [int] NOT NULL,");
                foreach (var item in GlobalData.Read_DevicesDataStorage[0])
                {
                    //if (string.isnullorempty(item2.value["length"]))
                    //{
                        creattable.Append("[" + item.Value["ColumnName"] + "] [" + item.Value["DataType"] + "] null,");
                    //}
                    //else
                    //{
                    //creattable.Append("[" + item.Value["format"] + "] [" + item.Value["format"] + "](" + item.Value["length"] + ") null,");
                    //}
                }
                creattable.Append("[InsertTime] [datetime] NULL\r\n");
                creattable.Append("CONSTRAINT [PK_" + tablename + "] PRIMARY KEY CLUSTERED\r\n");
                creattable.Append("([Id] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]");
                try
                {
                    //Log.Error("创建数据表,SQL:\r\n" + creattable.ToString());
                    MSSQLHelper.ExecuteSql(creattable.ToString());
                    creattable.Clear();
                }
                catch (Exception ex)
                {
                    Log.Error("创建数据表错误,SQL:\r\n" + creattable.ToString() + "\r\n" + ex.ToString());
                }
            }
            StringBuilder keys = new StringBuilder();
            StringBuilder vals = new StringBuilder();
            //拼接SQL语句
            foreach (var item in GlobalData.Read_DevicesDataStorage[0])
            {
                string key = item.Value["ColumnName"];
                keys.Append("["+key + "],");
                string val = item.Value["Value"];
                if (val.Length < 1) val = "0";
                vals.Append("'"+val + "',");
            }
            keys.Append("InsertTime");
            vals.Append("getdate()");
            string sql="insert into ["+ tablename+ "](" + keys.ToString() + ") values(" + vals.ToString() + ")";
            int i = MSSQLHelper.ExecuteSql(sql);
            Log.Info(string.Format("新增数据:{0}条", i));
        }
        /// <summary>
        /// 保存数据 MYSQL
        /// </summary>
        private void Do_SaveDataByMYSQL()
        {
            
        }
        /// <summary>
        /// 保存数据 HTTP
        /// </summary>
        private void Do_SaveDataByHTTP()
        {
            if (GlobalData.Read_DevicesDataStorage == null) return;
            Dictionary<string, string> data = new Dictionary<string, string>();
            foreach (var item in GlobalData.Read_DevicesDataStorage[0])
            {
                string key = item.Value["ColumnName"];
                string val = item.Value["Value"];
                if (val.Length < 1) return;
                data.Add(key,val);
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("devid", "19080808188");
            dic.Add("netinfo", "127.0.0.1:8859");
            dic.Add("data", JsonUntity.SerializeDictionaryToJsonString(data));
            HttpApi.PostData(dic, textBox_httpurl.Text);
            AppendLog("上传数据："+ JsonUntity.SerializeDictionaryToJsonString(data));
        }
        ///初始化线程状态
        DataGridViewTextBoxColumn acCode;
        private void init_dataGridView_data()
        {
            dataGridView_devdata.TopLeftHeaderCell.Value = "编号";
            dataGridView_devdata.RowHeadersWidth = 60;
            acCode = new DataGridViewTextBoxColumn();
            acCode.Name = "name";
            acCode.DataPropertyName = "name";
            acCode.HeaderText = "名称";
            dataGridView_devdata.Columns.Add(acCode);
            acCode = new DataGridViewTextBoxColumn();
            acCode.Name = "value";
            acCode.DataPropertyName = "value";
            acCode.HeaderText = "数值";
            dataGridView_devdata.Columns.Add(acCode);
            acCode = new DataGridViewTextBoxColumn();
            acCode.Name = "unit";
            acCode.DataPropertyName = "unit";
            acCode.HeaderText = "单位";
            dataGridView_devdata.Columns.Add(acCode);
            acCode = new DataGridViewTextBoxColumn();
            acCode.Name = "updatetime";
            acCode.DataPropertyName = "updatetime";
            acCode.HeaderText = "更新时间";
            dataGridView_devdata.Columns.Add(acCode);
            acCode.Dispose();
            dataGridView_devdata.Columns[0].Width = 100;
            dataGridView_devdata.Columns[1].Width = 100;
            dataGridView_devdata.Columns[2].Width = 60;
            dataGridView_devdata.Columns[3].Width = 160;
            dataGridView_devdata.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_devdata.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_devdata.Columns[2].SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_devdata.Columns[3].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        private void show_dataGridView_devdata()
        {
            if (GlobalData.Read_DevicesDataStorage == null) return;
            StringBuilder sb=new StringBuilder();
            //dataGridView_devdata.Rows.Clear();
            int row = 0;
            Dictionary<int, Dictionary<string, string>> _DevicesDataStorage = GlobalData.Read_DevicesDataStorage[0];
            foreach (var item in _DevicesDataStorage)
            {
                if (dataGridView_devdata.RowCount == _DevicesDataStorage.Count)
                {
                    dataGridView_devdata.Rows[row].Cells[0].Value = item.Value["Remark"];
                    dataGridView_devdata.Rows[row].Cells[1].Value = item.Value["Value"];
                    dataGridView_devdata.Rows[row].Cells[2].Value = item.Value["Unit"];
                    dataGridView_devdata.Rows[row].Cells[3].Value = item.Value["RefTime"];
                    row += 1;
                }
                else
                {
                    int index = this.dataGridView_devdata.Rows.Add();
                    dataGridView_devdata.Rows[index].HeaderCell.Value = (index + 1).ToString();
                    dataGridView_devdata.Rows[index].Cells[0].Value = item.Value["Remark"];
                    dataGridView_devdata.Rows[index].Cells[1].Value = item.Value["Value"];
                    dataGridView_devdata.Rows[index].Cells[2].Value = item.Value["Unit"];
                    dataGridView_devdata.Rows[index].Cells[3].Value = item.Value["RefTime"];
                }
                //自适应设置
                //dataGridView_devdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                string t = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
                sb.Append("\""+ item.Value["ColumnName"] + "\":{\"value\":" + item.Value["Value"] + ",\"time\":" + t + "},");
            }
            Params = sb.ToString().TrimEnd(',');
        }

        private void textBox_RefRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GlobalData.RefRate = Convert.ToInt32(textBox_RefRate.Text);
            }
            catch {
                textBox_RefRate.Text = GlobalData.RefRate.ToString();
            }
        }

        private void button_opendatadir_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Explorer.exe", DataDir);
        }
        /// <summary>
        /// 设置数据保存目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_datadir_Click(object sender, EventArgs e)
        {
            string defaultPath = "";
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            //打开的文件夹浏览对话框上的描述  
            dialog.Description = "请选择一个文件夹";
            //是否显示对话框左下角 新建文件夹 按钮，默认为 true  
            dialog.ShowNewFolderButton = false;
            //首次defaultPath为空，按FolderBrowserDialog默认设置（即桌面）选择  
            if (textBox_datadir.Text != "")
            {
                //设置此次默认目录为上一次选中目录  
                dialog.SelectedPath = textBox_datadir.Text;
            }
            //按下确定选择的按钮  
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                defaultPath = dialog.SelectedPath;
            }
            if (!string.IsNullOrEmpty(defaultPath))
            {
                textBox_datadir.Text = defaultPath + @"\";
                DataDir = textBox_datadir.Text;
                AppConfig.SetAppConfig("DATADIR", DataDir);
                Log.Info("数据保存至："+ DataDir);
            }
        }

        private void ToolStripMenuItem_dbset_Click(object sender, EventArgs e)
        {
            openform_dbset();
        }

        private void ToolStripMenuItem_registerset_Click(object sender, EventArgs e)
        {
            openform_registerset();
        }

        private void textBox_TimeOut_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GlobalData.TimeOut = Convert.ToInt32(textBox_TimeOut.Text);
            }
            catch
            {
                textBox_TimeOut.Text = GlobalData.TimeOut.ToString();
            }
        }
        /// <summary>
        /// 开始运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_go_Click(object sender, EventArgs e)
        {
            if (GlobalData.START)
            {
                stop_work();
            }
            else
            {
                Init();
                start_work(comboBox_SelectCom.Text, Box_BaudRate.Text);
            }
        }
        #region 任务列表导出数据进度条
        //缓冲显示命令处理
        private Form_ProgressStatus form_ProgressStatus = null;//弹出的子窗体(用于显示进度条)
        private delegate bool IncreaseHandle2(int val, string txt);//代理创建
        private IncreaseHandle2 d_Increase = null;//声明代理，用于后面的实例化代理

        private void Show_Form_ProgressStatus()
        {
            form_ProgressStatus = new Form_ProgressStatus();
            d_Increase = new IncreaseHandle2(form_ProgressStatus.Increase);
            form_ProgressStatus.ShowDialog();
            form_ProgressStatus = null;
        }
        #endregion
        private void button1_Click_3(object sender, EventArgs e)
        {
            Thread thdSub = new Thread(new ThreadStart(set_Do));
            thdSub.SetApartmentState(ApartmentState.STA);
            thdSub.Start();

            
        }

        private void set_Do()
        {
            //委托显示状态窗口
            MethodInvoker mi = new MethodInvoker(Show_Form_ProgressStatus);
            this.BeginInvoke(mi);
            Thread.Sleep(100);
            object objReturn = null;

            FirstCommand.Add_FirstCommand(1, "SET_DO1", 1, "DO1", 8, "HEAD");
            objReturn = this.Invoke(this.d_Increase, new object[] { 40, "发送命令" });
            int l = 0;
            while (GlobalData.FirstCommandDictionary[1]["SET_DO1"]["Finish"] == "0")
            {
                if (l > 1000)
                {
                    objReturn = this.Invoke(this.d_Increase, new object[] { 100, "" });
                    MessageBox.Show("通讯异常，操作失败！");
                    return;
                }
                objReturn = this.Invoke(this.d_Increase, new object[] { 50, "等待完成"+l });
                Thread.Sleep(10);
                //Application.DoEvents();
                l++;
            }
            string msg = string.Empty;
            if (GlobalData.FirstCommandDictionary[1]["SET_DO1"]["Error"] == "1")
            {
                msg = "通讯异常！";
            }
            else
            {
                msg = "命令完成！";
            }
                objReturn = this.Invoke(this.d_Increase, new object[] {99, msg });
                Thread.Sleep(1000);
                objReturn = this.Invoke(this.d_Increase, new object[] {100, msg });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FirstCommand.Add_FirstCommand(1, "SET_DO1", 0, "DO1", 8, "HEAD");
        }
        public string txtTest1
        {
            set { this.toolStripStatusMsg.Text = value; }
            get { return this.toolStripStatusMsg.Text; }
        }

        private void checkBox_autorun_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_autorun.Checked)
            {
                AppConfig.SetAppConfig("AUTORUN", "1");

            }
            else {
                AppConfig.SetAppConfig("AUTORUN", "0");
            }

        }
        /// <summary>
        /// 上报属性值 
        /// </summary>
        private void TimerAction_MqttPublic(object obj)
        {
            //if (imc != null) AppendLog(imc.Publish().ToString());
            if (mqttClient != null)
            {
                if (mqttClient.IsConnected) Task.Run(async () => { await Publish(); });
            }
        }

        /// <summary>
        /// HTTP客户端 调用
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        private string HttpClient(string url, string postdata)
        {
            HttpHelper http = new HttpHelper();
            HttpItem httpitem = new HttpItem()
            {
                URL = url,//URL     必需项
                Encoding = System.Text.Encoding.UTF8,//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
                //Encoding = Encoding.Default,
                Method = "Post",//可选项 默认为Get
                //Timeout = 100000,//连接超时时间     可选项默认为100000
                //ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000
                //IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写
                //Cookie = "",//字符串Cookie     可选项
                //UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值
                Accept = "application/json",//返回类型    可选项有默认值
                ContentType = "application/x-www-form-urlencoded",//提交的数据格式    可选项有默认值
                //Referer = "http://www.sufeinet.com",//来源URL     可选项
                //Allowautoredirect = true,//是否根据３０１跳转     可选项
                //CerPath = "d:\\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数
                //Connectionlimit = 1024,//最大连接数     可选项 默认为1024
                Postdata = postdata,//Post数据     可选项GET时不需要写
                //PostDataType = PostDataType.String,//默认为传入String类型，也可以设置PostDataType.Byte传入Byte类型数据
                //ProxyIp = "192.168.1.105：8015",//代理服务器ID 端口可以直接加到后面以：分开就行了    可选项 不需要代理 时可以不设置这三个参数
                //ProxyPwd = "123456",//代理服务器密码     可选项
                //ProxyUserName = "administrator",//代理服务器账户名     可选项
                //ResultType = ResultType.String,//返回数据类型，是Byte还是String
                //PostdataByte = System.Text.Encoding.Default.GetBytes("测试一下"),//如果PostDataType为Byte时要设置本属性的值
                //CookieCollection = new System.Net.CookieCollection(),//可以直接传一个Cookie集合进来
            };
            //httpitem.Header.Add("enctype", "Content-Type:application/x-www-form-urlencoded");
            //httpitem.Header.Add("测试Key2", "测试Value2");
            //得到HTML代码
            HttpResult result = http.GetHtml(httpitem);
            //取出返回的Cookie
            //string cookie = result.Cookie;
            //返回的Html内容
            string html = result.Html;
            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                //表示访问成功，具体的大家就参考HttpStatusCode类
                return html;
            }
            else
            {
                return string.Empty;
            }
        }
        /*****************************************************************/
        //MQTT客户端
        private string ClientId= "";//"${clientId}|securemode=3,signmethod=hmacsha1"
        private string TcpServer= "a1oNfo4Qy69.iot-as-mqtt.cn-shanghai.aliyuncs.com";//"${YourProductKey}.iot-as-mqtt.${region}.aliyuncs.com"
        private int TcpServerPort=1883;
        private string ProductKey = "a1oNfo4Qy69";
        private string DeviceName = "moni1";
        private string DeviceSecret = "0FPg5ALNM28H527BmvDlIvOkAGtEYesV";
        private string PubTopic = "";
        private string SubTopic = "";
        private string UserName = "";
        private string PassWord = "";
        /// <summary>
        /// 签名 MD5
        /// </summary>
        /// <returns></returns>
        private HashEncrypt hashEncrypt = new HashEncrypt(false, true);
        private void BuldingLoginInfo()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string clientId = host.AddressList.FirstOrDefault(
                ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
            string t = Convert.ToString(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            string signmethod = "hmacmd5";
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("productKey", ProductKey);
            dict.Add("deviceName", DeviceName);
            dict.Add("clientId", clientId);
            dict.Add("timestamp", t);

            UserName = DeviceName + "&" + ProductKey;
            PassWord = IotSignUtils.sign(dict, DeviceSecret, signmethod);
            ClientId = clientId + "|securemode=3,signmethod=" + signmethod + ",timestamp=" + t + "|";
        }
        Random rd = new Random();
        private async Task Publish()
        {
            string topic = "/sys/" + ProductKey + "/" + DeviceName + "/thing/event/property/post";

            if (string.IsNullOrEmpty(topic))
            {
                MessageBox.Show("发布主题不能为空！");
                return;
            }

            string t = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            string inputString = "{\"id\":\"" + t + "\",\"version\":\"1.0\",\"method\":\"thing.event.property.post\",\"params\":{\"value\": {\"Ua\": " + rd.Next(22000, 24000) * 0.01 + "},\"time\": " + t + "}}";

            //2.4.0版本的
            //var appMsg = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes(inputString), MqttQualityOfServiceLevel.AtMostOnce, false);
            //mqttClient.PublishAsync(appMsg);

            ///qos=0，WithAtMostOnceQoS,消息的分发依赖于底层网络的能力。
            ///接收者不会发送响应，发送者也不会重试。消息可能送达一次也可能根本没送达。
            ///感觉类似udp
            ///QoS 1: 至少分发一次。服务质量确保消息至少送达一次。
            ///QoS 2: 仅分发一次
            ///这是最高等级的服务质量，消息丢失和重复都是不可接受的。使用这个服务质量等级会有额外的开销。
            ///
            ///例如，想要收集电表读数的用户可能会决定使用QoS 1等级的消息，
            ///因为他们不能接受数据在网络传输途中丢失
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(inputString)
                .WithAtMostOnceQoS()
                .WithRetainFlag(true)
                .Build();
            int id = mqttClient.PublishAsync(message).Id;
            await mqttClient.PublishAsync(message);
            AppendLog(string.Format("上报属性：{0}", id));
        }

        private async Task Subscribe()
        {
            string topic = SubTopic;

            if (string.IsNullOrEmpty(topic))
            {
                MessageBox.Show("订阅主题不能为空！");
                return;
            }

            if (!mqttClient.IsConnected)
            {
                MessageBox.Show("MQTT客户端尚未连接！");
                return;
            }

            // Subscribe to a topic
            await mqttClient.SubscribeAsync(new TopicFilterBuilder()
                .WithTopic(topic)
                .WithAtMostOnceQoS()
                .Build()
                );

            //2.4.0
            //await mqttClient.SubscribeAsync(new List<TopicFilter> {
            //    new TopicFilter(topic, MqttQualityOfServiceLevel.AtMostOnce)
            //});

            Invoke((new Action(() =>
            {
                AppendLog($"已订阅[{topic}]主题{Environment.NewLine}");
            })));
            //txtSubTopic.Enabled = false;
            //btnSubscribe.Enabled = false;
        }
        private System.Threading.Timer PublicTimer = null;
        private MqttClient_AliyunIot aliyun = null;
        private void button_connectmqttsvr_Click(object sender, EventArgs e)
        {
            /*
            isReconnect = true;
            Task.Run(async () => { await ConnectMqttServerAsync(); });            
            if (PublicTimer == null)
            {
                PublicTimer = new System.Threading.Timer(new TimerCallback(TimerAction_MqttPublic), null, 0, 3000);
            }
            string pr = Params;
            */
        }
        private void checkBox_aliyuniot_connect_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_aliyuniot_connect.Checked)
            {
                if (aliyun == null)
                {
                    aliyun = new MqttClient_AliyunIot(textBox_TcpServer.Text, textBox_TcpServerPort.Text,textBox_Signmethod.Text, textBox_ProductKey.Text, textBox_DeviceName.Text,textBox_DeviceSecret.Text);
                    Task.Run(async () => { await aliyun.ConnectMqttServerAsync(); });
                    if (PublicTimer == null)
                    {
                        PublicTimer = new System.Threading.Timer(new TimerCallback(Publish2), null, 0, 3000);
                    }
                }

            }
            else
            {
                if (aliyun != null)
                {
                    aliyun=null;
                }
                if (PublicTimer != null)
                {
                    PublicTimer.Dispose();
                }
            }
        }
        private void Publish2(object obj)
        {
            if (aliyun == null) return;
            string t = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
            //string content = "{\"id\":\"" + t + "\",\"version\":\"1.0\",\"method\":\"thing.event.property.post\",\"params\":{\"value\": {\"Ua\": " + rd.Next(22000, 22999) * 0.01 + "},\"time\": " + t + "}}";
            string content = "{\"id\":\"" + t + "\",\"version\":\"1.0\",\"method\":\"thing.event.property.post\",\"params\":{"+Params+"}}";
            string topic = "/sys/" + ProductKey + "/" + DeviceName + "/thing/event/property/post";
            Task.Run(async () => { await aliyun.Publish(topic, content); });
            AppendLog("上报属性："+aliyun.PubTopicId.ToString());
        }

        private void button_savepramas_Click(object sender, EventArgs e)
        {
            SavePramas();
        }

        private void SavePramas()
        {
            AppConfig.SetAppConfig("COMPORT", comboBox_SelectCom.SelectedValue.ToString());
            AppConfig.SetAppConfig("BAUDRATE", Box_BaudRate.SelectedValue.ToString());
            AppConfig.SetAppConfig("RECORDRATE", comboBox_record.SelectedValue.ToString());
            AppConfig.SetAppConfig("REFRATE", textBox_RefRate.Text);
            AppConfig.SetAppConfig("DEVERID", textBox_deverid.Text);
            AppConfig.SetAppConfig("HTTPURL", textBox_httpurl.Text);
            AppConfig.SetAppConfig("FILEFORMAT", comboBox_fileformat.SelectedValue.ToString());
            AppConfig.SetAppConfig("COLLECTCONFIGFILE", comboBox_collectfile.SelectedValue.ToString());

            AppConfig.SetAppConfig("ALIYUN_IOT_TcpServer", textBox_TcpServer.Text);
            AppConfig.SetAppConfig("ALIYUN_IOT_TcpServerPort", textBox_TcpServerPort.Text);
            AppConfig.SetAppConfig("ALIYUN_IOT_Signmethod", textBox_Signmethod.Text);
            AppConfig.SetAppConfig("ALIYUN_IOT_DeviceName", textBox_DeviceName.Text);
            AppConfig.SetAppConfig("ALIYUN_IOT_DeviceSecret", textBox_DeviceSecret.Text);
            AppConfig.SetAppConfig("ALIYUN_IOT_ProductKey", textBox_ProductKey.Text);

        }

        

        private async Task ConnectMqttServerAsync()
        {
            // Create a new MQTT client.
            BuldingLoginInfo();
            if (mqttClient == null)
            {
                MqttFactory factory = new MqttFactory();
                mqttClient = factory.CreateMqttClient();
                mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;
                mqttClient.Connected += MqttClient_Connected;
                mqttClient.Disconnected += MqttClient_Disconnected;
            }

            //非托管客户端
            try
            {
                ////Create TCP based options using the builder.
                //var options1 = new MqttClientOptionsBuilder()
                //    .WithClientId("client001")
                //    .WithTcpServer("192.168.88.3")
                //    .WithCredentials("bud", "%spencer%")
                //    .WithTls()
                //    .WithCleanSession()
                //    .Build();

                //// Use TCP connection.
                //var options2 = new MqttClientOptionsBuilder()
                //    .WithTcpServer("192.168.88.3", 8222) // Port is optional
                //    .Build();

                //// Use secure TCP connection.
                //var options3 = new MqttClientOptionsBuilder()
                //    .WithTcpServer("192.168.88.3")
                //    .WithTls()
                //    .Build();

                //Create TCP based options using the builder.
                IMqttClientOptions options = new MqttClientOptionsBuilder()
                    .WithClientId(ClientId)
                    .WithTcpServer(TcpServer, TcpServerPort)
                    .WithCredentials(UserName,PassWord)
                    .WithKeepAlivePeriod(TimeSpan.FromSeconds(500))
                    //.WithTls()//服务器端没有启用加密协议，这里用tls的会提示协议异常
                    .WithCleanSession()
                    .Build();

                //// For .NET Framwork & netstandard apps:
                //MqttTcpChannel.CustomCertificateValidationCallback = (x509Certificate, x509Chain, sslPolicyErrors, mqttClientTcpOptions) =>
                //{
                //    if (mqttClientTcpOptions.Server == "server_with_revoked_cert")
                //    {
                //        return true;
                //    }

                //    return false;
                //};

                //2.4.0版本的
                //var options0 = new MqttClientTcpOptions
                //{
                //    Server = "127.0.0.1",
                //    ClientId = Guid.NewGuid().ToString().Substring(0, 5),
                //    UserName = "u001",
                //    Password = "p001",
                //    CleanSession = true
                //};

                await mqttClient.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                //isReconnect = false;
                AppendLog($"连接到MQTT服务器失败：" + ex.ToString());
            }

            //托管客户端
            //try
            //{
                //// Setup and start a managed MQTT client.
                //var options = new ManagedMqttClientOptionsBuilder()
                //    .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                //    .WithClientOptions(new MqttClientOptionsBuilder()
                //        .WithClientId("Client_managed")
                //        .WithTcpServer("192.168.88.3", 8223)
                //        .WithTls()
                //        .Build())
                //    .Build();

                //var mqttClient = new MqttFactory().CreateManagedMqttClient();
                //await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("my/topic").Build());
                //await mqttClient.StartAsync(options);
            //}
            //catch (Exception)
            //{

            //}
        }
        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            Invoke((new Action(() =>
            {
                AppendLog($">> ### RECEIVED APPLICATION MESSAGE ###");
            })));
            Invoke((new Action(() =>
            {
                AppendLog($">> Topic = {e.ApplicationMessage.Topic}");
            })));
            Invoke((new Action(() =>
            {
                AppendLog($">> Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            })));
            Invoke((new Action(() =>
            {
                AppendLog($">> QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
            })));
            Invoke((new Action(() =>
            {
                AppendLog($">> Retain = {e.ApplicationMessage.Retain}");
            })));
        }
        /// <summary>
        /// MQTT客户端连接成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_Connected(object sender, EventArgs e)
        {
            AppendLog("已连接到MQTT服务器！");
        }
        /// <summary>
        /// MQTT客户端连接关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_Disconnected(object sender, EventArgs e)
        {
            Invoke((new Action(() =>
            {
                DateTime curTime = new DateTime();
                curTime = DateTime.UtcNow;
                AppendLog($">> [{curTime.ToLongTimeString()}]");
                AppendLog("已断开MQTT连接！");
            })));
            //Reconnecting
            if (isReconnect)
            {
                Invoke((new Action(() =>
                {
                    AppendLog("正在尝试重新连接");
                })));
                Invoke((new Action(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    try
                    {
                        await ConnectMqttServerAsync();
                    }
                    catch(Exception ex)
                    {
                        AppendLog("连接失败："+ex.ToString());
                    }
                })));
            }
            else
            {
                Invoke((new Action(() =>
                {
                    AppendLog("已下线！");
                })));
            }
        }

    }
}
