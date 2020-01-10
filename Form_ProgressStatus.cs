using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UartCollect
{
    public partial class Form_ProgressStatus : Form
    {
        public Form_ProgressStatus()
        {
            InitializeComponent();
        }
        public bool Increase(int nValue, string nInfo)
        {
            if (nValue > 0)
            {
                if (nValue < progressBar.Maximum)
                {
                    progressBar.Value = nValue;
                    this.label2.Text = nInfo;
                    Application.DoEvents();
                    progressBar.Update();
                    progressBar.Refresh();
                    this.label2.Update();
                    this.label2.Refresh();
                    return true;
                }
                else
                {
                    progressBar.Value = progressBar.Maximum;
                    this.label2.Text = nInfo;
                    Thread.Sleep(500);
                    this.Close();//执行完之后，自动关闭子窗体
                    return false;
                }
            }
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CloseForm();
        }
        private void CloseForm()
        {
            Close();
            Dispose();
        }

        private void Form_ProgressStatus_Load(object sender, EventArgs e)
        {
            progressBar.Maximum = 100;
            this.Activate();
        }
    }
}
