using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UartCollect.Headle
{
    class HttpApi
    {
        //日志初始化
        static readonly ILog Log = LogManager.GetLogger("RollingLogFileAppender");
        public static void PostData(Dictionary<string, string> dic, string url)
        {
            if (url.Length < 1) return;
            //开启线程池上传数据
            ThreadPool.QueueUserWorkItem((object obj) =>
            {
                if (obj is Dictionary<string, string> _dic)
                {
                    HttpHelper http = new HttpHelper();
                    HttpItem httpitem = new HttpItem()
                    {
                        URL = url,//URL     必需项
                        Encoding = System.Text.Encoding.UTF8,//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
                        //Encoding = Encoding.Default,
                        Method = "Post",//可选项 默认为Get
                        Timeout = 10000,//连接超时时间     可选项默认为100000
                        //ReadWriteTimeout = 30000,//写入Post数据超时时间     可选项默认为30000
                        //IsToLower = false,//得到的HTML代码是否转成小写     可选项默认转小写
                        //Cookie = "",//字符串Cookie     可选项
                        //UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)",//用户的浏览器类型，版本，操作系统     可选项有默认值
                        Accept = "application/json",//返回类型    可选项有默认值
                        ContentType = "application/json", //"application/x-www-form-urlencoded",//提交的数据格式    可选项有默认值
                        //Referer = "http://www.sufeinet.com",//来源URL     可选项
                        //Allowautoredirect = true,//是否根据３０１跳转     可选项
                        //CerPath = "d:\\123.cer",//证书绝对路径     可选项不需要证书时可以不写这个参数
                        //Connectionlimit = 1024,//最大连接数     可选项 默认为1024
                        Postdata = JsonUntity.SerializeDictionaryToJsonString(dic),//Post数据     可选项GET时不需要写
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
                        Log.Info("上传数据结果：" + html);
                    }
                    else
                    {
                        Log.Info("上传数据结果：" + result.StatusCode);
                    }
                }
            }, dic);

        }
    }
}
