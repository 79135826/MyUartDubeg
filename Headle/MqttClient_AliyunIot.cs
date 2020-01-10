using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using MQTTnet.Client;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;


namespace UartCollect.Headle
{
    class MqttClient_AliyunIot
    {
        //MQTT参数
        private IMqttClient mqttClient = null;
        private bool isReconnect=true;
        private string ClientId = "";
        private string TcpServer = "a1oNfo4Qy69.iot-as-mqtt.cn-shanghai.aliyuncs.com";
        private int TcpServerPort = 1883;
        private string Signmethod = "hmacmd5";
        private string ProductKey = "a1oNfo4Qy69";
        private string DeviceName = "moni1";
        private string DeviceSecret = "0FPg5ALNM28H527BmvDlIvOkAGtEYesV";
        private string PubTopic = "";
        public int PubTopicId=0;
        private string SubTopic = "";
        private string UserName = "";
        private string PassWord = "";

        public MqttClient_AliyunIot(string _tcpserver, string _port,string _signmethod,string _productkey,string devicename,string _devicesecret)
        {
            /*
            TcpServer = _tcpserver;
            TcpServerPort = Convert.ToInt32(_port);
            Signmethod = _signmethod;
            ProductKey = _productkey;
            DeviceName = devicename;
            DeviceSecret = _devicesecret;*/
        }

        private void BuldingLoginInfo()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string clientId = host.AddressList.FirstOrDefault(
                ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
            string t = Convert.ToString(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("productKey", ProductKey);
            dict.Add("deviceName", DeviceName);
            dict.Add("clientId", clientId);
            dict.Add("timestamp", t);
            UserName = DeviceName + "&" + ProductKey;
            PassWord = IotSignUtils.sign(dict, DeviceSecret, Signmethod);
            ClientId = clientId + "|securemode=3,signmethod=" + Signmethod + ",timestamp=" + t + "|";
        }
        public async Task ConnectMqttServerAsync()
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
                    .WithCredentials(UserName, PassWord)
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
                UIAction.AppendLog($"连接到MQTT服务器失败：" + ex.ToString());
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

            UIAction.AppendLog($">> ### RECEIVED APPLICATION MESSAGE ###");
            UIAction.AppendLog($">> Topic = {e.ApplicationMessage.Topic}");
            UIAction.AppendLog($">> Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            UIAction.AppendLog($">> QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
            UIAction.AppendLog($">> Retain = {e.ApplicationMessage.Retain}");
        }
        /// <summary>
        /// MQTT客户端连接成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_Connected(object sender, EventArgs e)
        {
            UIAction.AppendLog("已连接到MQTT服务器！");
        }
        /// <summary>
        /// MQTT客户端连接关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MqttClient_Disconnected(object sender, EventArgs e)
        {
                DateTime curTime = new DateTime();
                curTime = DateTime.UtcNow;
                UIAction.AppendLog($">> [{curTime.ToLongTimeString()}]");
                UIAction.AppendLog("已断开MQTT连接！");
            //Reconnecting
            if (isReconnect)
            {
                Task.Run(async () =>
                {
                    UIAction.AppendLog("正在尝试重新连接");
                    await Task.Delay(TimeSpan.FromSeconds(5));
                    try
                    {
                        await ConnectMqttServerAsync();
                    }
                    catch (Exception ex)
                    {
                        UIAction.AppendLog("连接失败：" + ex.ToString());
                    }
                });
            }
            else
            {
                UIAction.AppendLog("已下线！");
            }
        }
        /// <summary>
        /// 发布主题
        /// </summary>
        /// <returns></returns>
        public async Task Publish(string topic,string content)
        {
            if (string.IsNullOrEmpty(topic))
            {
                return;
            }
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
                .WithPayload(content)
                .WithAtMostOnceQoS()
                .WithRetainFlag(true)
                .Build();
            PubTopicId = mqttClient.PublishAsync(message).Id;
            await mqttClient.PublishAsync(message);
        }
        /// <summary>
        /// 订阅主题
        /// </summary>
        /// <returns></returns>
        public async Task Subscribe(string topic)
        {
            if (string.IsNullOrEmpty(topic))
            {
                UIAction.AppendLog("订阅主题不能为空！");
                return;
            }
            if (!mqttClient.IsConnected)
            {
                UIAction.AppendLog("MQTT客户端尚未连接！");
                return;
            }
            // Subscribe to a topic
            await mqttClient.SubscribeAsync(new TopicFilterBuilder()
                .WithTopic(topic)
                .WithAtMostOnceQoS()
                .Build()
                );
            UIAction.AppendLog($"已订阅[{topic}]主题");
        }
    }
}
