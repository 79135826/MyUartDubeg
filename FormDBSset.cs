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
    public partial class FormDBSset : Form
    {
        public FormDBSset()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AppConfig.SetAppConfig("DBSERVER_IP", textBox_ip.Text);
            AppConfig.SetAppConfig("DBSERVER_PORT", textBox_port.Text);
            AppConfig.SetAppConfig("DBSERVER_DATABASE", textBox_db.Text);
            AppConfig.SetAppConfig("DBSERVER_LOGINNAME", textBox_name.Text);
            AppConfig.SetAppConfig("DBSERVER_PASSWORD", textBox_pwd.Text);
            AppConfig.SetAppConfig("TABLENAME", textBox_tablename.Text);
            MessageBox.Show("保存成功");
            Close();
        }

        private void FormDBSset_Load(object sender, EventArgs e)
        {
            textBox_ip.Text= AppConfig.GetAppConfig("DBSERVER_IP");
            textBox_port.Text = AppConfig.GetAppConfig("DBSERVER_PORT");
            textBox_db.Text = AppConfig.GetAppConfig("DBSERVER_DATABASE");
            textBox_name.Text = AppConfig.GetAppConfig("DBSERVER_LOGINNAME");
            textBox_pwd.Text = AppConfig.GetAppConfig("DBSERVER_PASSWORD");
            textBox_tablename.Text= AppConfig.GetAppConfig("TABLENAME");
        }
    }
}
