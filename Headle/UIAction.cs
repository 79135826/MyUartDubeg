using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UartCollect.Headle
{
    public static class UIAction
    {
        private static Form1 form1=new Form1();
        public static void AppendLog(string msg)
        {
            form1.AppendLog(msg);
        }
    }
}
