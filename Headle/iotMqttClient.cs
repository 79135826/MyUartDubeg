using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Threading.Tasks;
using log4net;

namespace UartCollect.Headle
{
    class iotMqttClient
    {
        //日志初始化
        public static readonly ILog Log = LogManager.GetLogger("RollingLogFileAppender");
        private string ProductKey = "a1oNfo4Qy69";
        private string DeviceName = "moni1";
        private string DeviceSecret = "0FPg5ALNM28H527BmvDlIvOkAGtEYesV";
        private string RegionId = "cn-shanghai";
        private string PubTopic = "";
        private string SubTopic = "";
        private MqttClient client = null;

        public iotMqttClient() {
            PubTopic = "/sys/" + ProductKey + "/" + DeviceName + "/thing/event/property/post";
            SubTopic = "/sys/" + ProductKey + "/" + DeviceName + "/thing/event/property/set";
        }

        public void Main()
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

            string mqttUserName = DeviceName + "&" + ProductKey;
            string mqttPassword = IotSignUtils.sign(dict, DeviceSecret, signmethod);
            string mqttClientId = clientId + "|securemode=3,signmethod=" + signmethod + ",timestamp=" + t + "|";

            string targetServer = ProductKey + ".iot-as-mqtt." + RegionId + ".aliyuncs.com";

            ConnectMqtt(targetServer, mqttClientId, mqttUserName, mqttPassword);
        }

        public void ConnectMqtt(string targetServer, string mqttClientId, string mqttUserName, string mqttPassword)
        {
            client = new MqttClient(targetServer);
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1_1;

            client.Connect(mqttClientId, mqttUserName, mqttPassword, false, 60);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            
        }
        Random rd = new Random();
        public int Publish()
        {
            if (client == null) return 0;
            //发布消息
            double v = rd.Next(22000, 24000) * 0.01;
            string t = Convert.ToString(DateTimeOffset.Now.ToUnixTimeMilliseconds());
            string content = "{\"id\":\"" + t + "\",\"version\":\"1.0\",\"method\":\"thing.event.property.post\",\"params\":{\"value\": {\"Ua\": "+ v+"},\"time\": " + t + "}}";
            int id = client.Publish(PubTopic, Encoding.ASCII.GetBytes(content));

            //订阅消息
            //client.Subscribe(new string[] { SubTopic }, new byte[] { 0 });
            return id;
        }

        public void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received
            string topic = e.Topic;
            string message = Encoding.ASCII.GetString(e.Message);
            Log.Info(message);
        }
    }
}
