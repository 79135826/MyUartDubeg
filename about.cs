﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UartCollect
{
    public partial class about : Form
    {
        public about()
        {
            InitializeComponent();
        }

        private void about_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
