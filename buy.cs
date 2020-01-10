using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UartCollect
{
    public partial class buy : Form
    {
        public buy()
        {
            InitializeComponent();
        }

        private void buy_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openhttpurl(linkLabel1.Text); 
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openhttpurl(linkLabel2.Text); 
        }
        private void QQ_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openhttpurl("http://wpa.qq.com/msgrd?v=3&uin="+QQ.Text+"&site=qq&menu=yes"); 
        }
        private void QQ2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openhttpurl("http://wpa.qq.com/msgrd?v=3&uin=" + QQ2.Text + "&site=qq&menu=yes");
        }
        private void openhttpurl(string url)
        {
            //从注册表中读取默认浏览器可执行文件路径  
            try
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
                string s = key.GetValue("").ToString();
                string apppath = s.Substring(0, s.IndexOf(".exe") + 5);
                System.Diagnostics.Process.Start(apppath, url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开网址错误："+ex.ToString());
            }
        }


    }
}
