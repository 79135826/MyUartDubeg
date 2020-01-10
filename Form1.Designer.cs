namespace UartCollect
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_collectfile = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.button_datadir = new System.Windows.Forms.Button();
            this.textBox_datadir = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button_opendatadir = new System.Windows.Forms.Button();
            this.comboBox_fileformat = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_deverid = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_TimeOut = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_RefRate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox_autorun = new System.Windows.Forms.CheckBox();
            this.comboBox_record = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Box_BaudRate = new System.Windows.Forms.ComboBox();
            this.comboBox_SelectCom = new System.Windows.Forms.ComboBox();
            this.button_connectmqttsvr = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView_devdata = new System.Windows.Forms.DataGridView();
            this.status = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusMsg_A = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel_threadstatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem_go = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_registerset = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_dbset = new System.Windows.Forms.ToolStripMenuItem();
            this.buy = new System.Windows.Forms.ToolStripMenuItem();
            this.about = new System.Windows.Forms.ToolStripMenuItem();
            this.timer_getthreadstatus = new System.Windows.Forms.Timer(this.components);
            this.timer_updateshow = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox_aliyuniot_connect = new System.Windows.Forms.CheckBox();
            this.button_savepramas = new System.Windows.Forms.Button();
            this.textBox_DeviceSecret = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBox_DeviceName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox_ProductKey = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBox_Signmethod = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox_TcpServerPort = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox_TcpServer = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.richTextBox_log = new System.Windows.Forms.RichTextBox();
            this.textBox_httpurl = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_devdata)).BeginInit();
            this.status.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.Location = new System.Drawing.Point(20, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "设备串口：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10F);
            this.label2.Location = new System.Drawing.Point(242, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 11;
            this.label2.Text = "波特率：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_httpurl);
            this.groupBox1.Controls.Add(this.comboBox_collectfile);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.button_datadir);
            this.groupBox1.Controls.Add(this.textBox_datadir);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.button_opendatadir);
            this.groupBox1.Controls.Add(this.comboBox_fileformat);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.textBox_deverid);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox_TimeOut);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_RefRate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.checkBox_autorun);
            this.groupBox1.Controls.Add(this.comboBox_record);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.Box_BaudRate);
            this.groupBox1.Controls.Add(this.comboBox_SelectCom);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10F);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(505, 273);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "常用参数";
            // 
            // comboBox_collectfile
            // 
            this.comboBox_collectfile.DisplayMember = "Text";
            this.comboBox_collectfile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_collectfile.FormattingEnabled = true;
            this.comboBox_collectfile.Location = new System.Drawing.Point(307, 163);
            this.comboBox_collectfile.Name = "comboBox_collectfile";
            this.comboBox_collectfile.Size = new System.Drawing.Size(163, 21);
            this.comboBox_collectfile.TabIndex = 40;
            this.comboBox_collectfile.ValueMember = "Value";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 12F);
            this.label11.Location = new System.Drawing.Point(216, 165);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 16);
            this.label11.TabIndex = 39;
            this.label11.Text = "采集设置：";
            // 
            // button_datadir
            // 
            this.button_datadir.Location = new System.Drawing.Point(315, 197);
            this.button_datadir.Name = "button_datadir";
            this.button_datadir.Size = new System.Drawing.Size(44, 22);
            this.button_datadir.TabIndex = 37;
            this.button_datadir.Text = "浏览";
            this.button_datadir.UseVisualStyleBackColor = true;
            this.button_datadir.Click += new System.EventHandler(this.button_datadir_Click);
            // 
            // textBox_datadir
            // 
            this.textBox_datadir.Location = new System.Drawing.Point(98, 196);
            this.textBox_datadir.Name = "textBox_datadir";
            this.textBox_datadir.ReadOnly = true;
            this.textBox_datadir.Size = new System.Drawing.Size(211, 23);
            this.textBox_datadir.TabIndex = 36;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10F);
            this.label10.Location = new System.Drawing.Point(20, 200);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 14);
            this.label10.TabIndex = 35;
            this.label10.Text = "保存目录：";
            // 
            // button_opendatadir
            // 
            this.button_opendatadir.Location = new System.Drawing.Point(364, 197);
            this.button_opendatadir.Name = "button_opendatadir";
            this.button_opendatadir.Size = new System.Drawing.Size(45, 22);
            this.button_opendatadir.TabIndex = 34;
            this.button_opendatadir.Text = "打开";
            this.button_opendatadir.UseVisualStyleBackColor = true;
            this.button_opendatadir.Click += new System.EventHandler(this.button_opendatadir_Click);
            // 
            // comboBox_fileformat
            // 
            this.comboBox_fileformat.DisplayMember = "Text";
            this.comboBox_fileformat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_fileformat.FormattingEnabled = true;
            this.comboBox_fileformat.Location = new System.Drawing.Point(307, 130);
            this.comboBox_fileformat.Name = "comboBox_fileformat";
            this.comboBox_fileformat.Size = new System.Drawing.Size(85, 21);
            this.comboBox_fileformat.TabIndex = 33;
            this.comboBox_fileformat.ValueMember = "Value";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 12F);
            this.label9.Location = new System.Drawing.Point(216, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(88, 16);
            this.label9.TabIndex = 32;
            this.label9.Text = "保存格式：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10F);
            this.label8.Location = new System.Drawing.Point(378, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 14);
            this.label8.TabIndex = 31;
            this.label8.Text = "毫秒";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10F);
            this.label7.Location = new System.Drawing.Point(179, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 14);
            this.label7.TabIndex = 30;
            this.label7.Text = "毫秒";
            // 
            // textBox_deverid
            // 
            this.textBox_deverid.Location = new System.Drawing.Point(98, 161);
            this.textBox_deverid.Name = "textBox_deverid";
            this.textBox_deverid.Size = new System.Drawing.Size(78, 23);
            this.textBox_deverid.TabIndex = 29;
            this.textBox_deverid.Text = "1";
            this.textBox_deverid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10F);
            this.label5.Location = new System.Drawing.Point(18, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 14);
            this.label5.TabIndex = 28;
            this.label5.Text = "设备地址：";
            // 
            // textBox_TimeOut
            // 
            this.textBox_TimeOut.Location = new System.Drawing.Point(98, 96);
            this.textBox_TimeOut.Name = "textBox_TimeOut";
            this.textBox_TimeOut.Size = new System.Drawing.Size(78, 23);
            this.textBox_TimeOut.TabIndex = 27;
            this.textBox_TimeOut.Text = "500";
            this.textBox_TimeOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_TimeOut.TextChanged += new System.EventHandler(this.textBox_TimeOut_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10F);
            this.label4.Location = new System.Drawing.Point(17, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 14);
            this.label4.TabIndex = 26;
            this.label4.Text = "接收超时：";
            // 
            // textBox_RefRate
            // 
            this.textBox_RefRate.Location = new System.Drawing.Point(307, 59);
            this.textBox_RefRate.Name = "textBox_RefRate";
            this.textBox_RefRate.Size = new System.Drawing.Size(62, 23);
            this.textBox_RefRate.TabIndex = 25;
            this.textBox_RefRate.Text = "1000";
            this.textBox_RefRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_RefRate.TextChanged += new System.EventHandler(this.textBox_RefRate_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10F);
            this.label3.Location = new System.Drawing.Point(228, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 14);
            this.label3.TabIndex = 24;
            this.label3.Text = "采集频率：";
            // 
            // checkBox_autorun
            // 
            this.checkBox_autorun.AutoSize = true;
            this.checkBox_autorun.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_autorun.Location = new System.Drawing.Point(231, 97);
            this.checkBox_autorun.Name = "checkBox_autorun";
            this.checkBox_autorun.Size = new System.Drawing.Size(96, 18);
            this.checkBox_autorun.TabIndex = 23;
            this.checkBox_autorun.Text = "自动运行：";
            this.checkBox_autorun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_autorun.UseVisualStyleBackColor = true;
            this.checkBox_autorun.CheckedChanged += new System.EventHandler(this.checkBox_autorun_CheckedChanged);
            // 
            // comboBox_record
            // 
            this.comboBox_record.DisplayMember = "Text";
            this.comboBox_record.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_record.FormattingEnabled = true;
            this.comboBox_record.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.comboBox_record.Location = new System.Drawing.Point(98, 61);
            this.comboBox_record.Name = "comboBox_record";
            this.comboBox_record.Size = new System.Drawing.Size(78, 21);
            this.comboBox_record.TabIndex = 22;
            this.comboBox_record.ValueMember = "Value";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10F);
            this.label6.Location = new System.Drawing.Point(18, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 14);
            this.label6.TabIndex = 21;
            this.label6.Text = "记录频率：";
            // 
            // Box_BaudRate
            // 
            this.Box_BaudRate.DisplayMember = "Text";
            this.Box_BaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Box_BaudRate.FormattingEnabled = true;
            this.Box_BaudRate.Location = new System.Drawing.Point(307, 27);
            this.Box_BaudRate.Name = "Box_BaudRate";
            this.Box_BaudRate.Size = new System.Drawing.Size(62, 21);
            this.Box_BaudRate.TabIndex = 18;
            this.Box_BaudRate.ValueMember = "Value";
            // 
            // comboBox_SelectCom
            // 
            this.comboBox_SelectCom.DisplayMember = "Text";
            this.comboBox_SelectCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_SelectCom.FormattingEnabled = true;
            this.comboBox_SelectCom.Location = new System.Drawing.Point(98, 26);
            this.comboBox_SelectCom.Name = "comboBox_SelectCom";
            this.comboBox_SelectCom.Size = new System.Drawing.Size(78, 21);
            this.comboBox_SelectCom.TabIndex = 16;
            this.comboBox_SelectCom.ValueMember = "Value";
            // 
            // button_connectmqttsvr
            // 
            this.button_connectmqttsvr.Location = new System.Drawing.Point(414, 112);
            this.button_connectmqttsvr.Name = "button_connectmqttsvr";
            this.button_connectmqttsvr.Size = new System.Drawing.Size(76, 33);
            this.button_connectmqttsvr.TabIndex = 38;
            this.button_connectmqttsvr.Text = "连接";
            this.button_connectmqttsvr.UseVisualStyleBackColor = true;
            this.button_connectmqttsvr.Visible = false;
            this.button_connectmqttsvr.Click += new System.EventHandler(this.button_connectmqttsvr_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView_devdata);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("宋体", 10F);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(505, 487);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据列表";
            // 
            // dataGridView_devdata
            // 
            this.dataGridView_devdata.AllowUserToAddRows = false;
            this.dataGridView_devdata.AllowUserToDeleteRows = false;
            this.dataGridView_devdata.AllowUserToResizeColumns = false;
            this.dataGridView_devdata.AllowUserToResizeRows = false;
            this.dataGridView_devdata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_devdata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_devdata.Location = new System.Drawing.Point(3, 19);
            this.dataGridView_devdata.Name = "dataGridView_devdata";
            this.dataGridView_devdata.ReadOnly = true;
            this.dataGridView_devdata.RowTemplate.Height = 23;
            this.dataGridView_devdata.Size = new System.Drawing.Size(499, 465);
            this.dataGridView_devdata.TabIndex = 0;
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusMsg,
            this.toolStripStatusMsg_A,
            this.toolStripStatusLabel_threadstatus});
            this.status.Location = new System.Drawing.Point(0, 545);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(519, 22);
            this.status.TabIndex = 54;
            this.status.Text = "statusStrip1";
            // 
            // toolStripStatusMsg
            // 
            this.toolStripStatusMsg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusMsg.Font = new System.Drawing.Font("宋体", 10F);
            this.toolStripStatusMsg.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusMsg.Name = "toolStripStatusMsg";
            this.toolStripStatusMsg.Size = new System.Drawing.Size(35, 17);
            this.toolStripStatusMsg.Text = "消息";
            // 
            // toolStripStatusMsg_A
            // 
            this.toolStripStatusMsg_A.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.toolStripStatusMsg_A.IsLink = true;
            this.toolStripStatusMsg_A.Name = "toolStripStatusMsg_A";
            this.toolStripStatusMsg_A.Size = new System.Drawing.Size(0, 17);
            this.toolStripStatusMsg_A.ToolTipText = "点击打开";
            // 
            // toolStripStatusLabel_threadstatus
            // 
            this.toolStripStatusLabel_threadstatus.Name = "toolStripStatusLabel_threadstatus";
            this.toolStripStatusLabel_threadstatus.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabel_threadstatus.Text = "运行状态";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_go,
            this.ToolStripMenuItem_registerset,
            this.ToolStripMenuItem_dbset,
            this.buy,
            this.about});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(519, 25);
            this.menuStrip1.TabIndex = 55;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItem_go
            // 
            this.ToolStripMenuItem_go.BackColor = System.Drawing.Color.Red;
            this.ToolStripMenuItem_go.Name = "ToolStripMenuItem_go";
            this.ToolStripMenuItem_go.Size = new System.Drawing.Size(44, 21);
            this.ToolStripMenuItem_go.Text = "运行";
            this.ToolStripMenuItem_go.Click += new System.EventHandler(this.ToolStripMenuItem_go_Click);
            // 
            // ToolStripMenuItem_registerset
            // 
            this.ToolStripMenuItem_registerset.Font = new System.Drawing.Font("宋体", 10F);
            this.ToolStripMenuItem_registerset.Name = "ToolStripMenuItem_registerset";
            this.ToolStripMenuItem_registerset.Size = new System.Drawing.Size(75, 21);
            this.ToolStripMenuItem_registerset.Text = "采集设置";
            this.ToolStripMenuItem_registerset.Visible = false;
            this.ToolStripMenuItem_registerset.Click += new System.EventHandler(this.ToolStripMenuItem_registerset_Click);
            // 
            // ToolStripMenuItem_dbset
            // 
            this.ToolStripMenuItem_dbset.Font = new System.Drawing.Font("宋体", 10F);
            this.ToolStripMenuItem_dbset.Name = "ToolStripMenuItem_dbset";
            this.ToolStripMenuItem_dbset.Size = new System.Drawing.Size(89, 21);
            this.ToolStripMenuItem_dbset.Text = "数据库设置";
            this.ToolStripMenuItem_dbset.Click += new System.EventHandler(this.ToolStripMenuItem_dbset_Click);
            // 
            // buy
            // 
            this.buy.Font = new System.Drawing.Font("宋体", 10F);
            this.buy.Name = "buy";
            this.buy.Size = new System.Drawing.Size(75, 21);
            this.buy.Text = "订制开发";
            this.buy.Click += new System.EventHandler(this.buy_Click);
            // 
            // about
            // 
            this.about.Font = new System.Drawing.Font("宋体", 10F);
            this.about.Name = "about";
            this.about.Size = new System.Drawing.Size(47, 21);
            this.about.Text = "关于";
            this.about.Click += new System.EventHandler(this.about_Click);
            // 
            // timer_getthreadstatus
            // 
            this.timer_getthreadstatus.Interval = 1000;
            this.timer_getthreadstatus.Tick += new System.EventHandler(this.timer_getthreadstatus_Tick);
            // 
            // timer_updateshow
            // 
            this.timer_updateshow.Tick += new System.EventHandler(this.timer_updateshow_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("宋体", 10F);
            this.tabControl1.Location = new System.Drawing.Point(0, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(519, 520);
            this.tabControl1.TabIndex = 56;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(511, 493);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "数据显示";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(511, 493);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "参数设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox_aliyuniot_connect);
            this.groupBox3.Controls.Add(this.button_savepramas);
            this.groupBox3.Controls.Add(this.textBox_DeviceSecret);
            this.groupBox3.Controls.Add(this.button_connectmqttsvr);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Controls.Add(this.textBox_DeviceName);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.textBox_ProductKey);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.textBox_Signmethod);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.textBox_TcpServerPort);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.textBox_TcpServer);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.button2);
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(3, 282);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(505, 208);
            this.groupBox3.TabIndex = 28;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "物联网平台MQTT参数";
            this.groupBox3.Visible = false;
            // 
            // checkBox_aliyuniot_connect
            // 
            this.checkBox_aliyuniot_connect.AutoSize = true;
            this.checkBox_aliyuniot_connect.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox_aliyuniot_connect.Location = new System.Drawing.Point(408, 154);
            this.checkBox_aliyuniot_connect.Name = "checkBox_aliyuniot_connect";
            this.checkBox_aliyuniot_connect.Size = new System.Drawing.Size(82, 18);
            this.checkBox_aliyuniot_connect.TabIndex = 41;
            this.checkBox_aliyuniot_connect.Text = "自动连接";
            this.checkBox_aliyuniot_connect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox_aliyuniot_connect.UseVisualStyleBackColor = true;
            this.checkBox_aliyuniot_connect.CheckedChanged += new System.EventHandler(this.checkBox_aliyuniot_connect_CheckedChanged);
            // 
            // button_savepramas
            // 
            this.button_savepramas.Location = new System.Drawing.Point(408, 178);
            this.button_savepramas.Name = "button_savepramas";
            this.button_savepramas.Size = new System.Drawing.Size(82, 24);
            this.button_savepramas.TabIndex = 53;
            this.button_savepramas.Text = "保存参数";
            this.button_savepramas.UseVisualStyleBackColor = true;
            this.button_savepramas.Click += new System.EventHandler(this.button_savepramas_Click);
            // 
            // textBox_DeviceSecret
            // 
            this.textBox_DeviceSecret.Location = new System.Drawing.Point(109, 182);
            this.textBox_DeviceSecret.Name = "textBox_DeviceSecret";
            this.textBox_DeviceSecret.Size = new System.Drawing.Size(251, 23);
            this.textBox_DeviceSecret.TabIndex = 52;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 10F);
            this.label17.Location = new System.Drawing.Point(0, 186);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(105, 14);
            this.label17.TabIndex = 51;
            this.label17.Text = "DeviceSecret：";
            // 
            // textBox_DeviceName
            // 
            this.textBox_DeviceName.Location = new System.Drawing.Point(109, 146);
            this.textBox_DeviceName.Name = "textBox_DeviceName";
            this.textBox_DeviceName.Size = new System.Drawing.Size(251, 23);
            this.textBox_DeviceName.TabIndex = 50;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 10F);
            this.label16.Location = new System.Drawing.Point(14, 150);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(91, 14);
            this.label16.TabIndex = 49;
            this.label16.Text = "DeviceName：";
            // 
            // textBox_ProductKey
            // 
            this.textBox_ProductKey.Location = new System.Drawing.Point(109, 108);
            this.textBox_ProductKey.Name = "textBox_ProductKey";
            this.textBox_ProductKey.Size = new System.Drawing.Size(251, 23);
            this.textBox_ProductKey.TabIndex = 48;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 10F);
            this.label15.Location = new System.Drawing.Point(14, 112);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(91, 14);
            this.label15.TabIndex = 47;
            this.label15.Text = "ProductKey：";
            // 
            // textBox_Signmethod
            // 
            this.textBox_Signmethod.Location = new System.Drawing.Point(282, 70);
            this.textBox_Signmethod.Name = "textBox_Signmethod";
            this.textBox_Signmethod.Size = new System.Drawing.Size(78, 23);
            this.textBox_Signmethod.TabIndex = 46;
            this.textBox_Signmethod.Text = "hmacmd5";
            this.textBox_Signmethod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 10F);
            this.label14.Location = new System.Drawing.Point(201, 74);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 14);
            this.label14.TabIndex = 45;
            this.label14.Text = "签名方式：";
            // 
            // textBox_TcpServerPort
            // 
            this.textBox_TcpServerPort.Location = new System.Drawing.Point(109, 70);
            this.textBox_TcpServerPort.Name = "textBox_TcpServerPort";
            this.textBox_TcpServerPort.Size = new System.Drawing.Size(78, 23);
            this.textBox_TcpServerPort.TabIndex = 44;
            this.textBox_TcpServerPort.Text = "1883";
            this.textBox_TcpServerPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 10F);
            this.label13.Location = new System.Drawing.Point(42, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 14);
            this.label13.TabIndex = 43;
            this.label13.Text = "端口号：";
            // 
            // textBox_TcpServer
            // 
            this.textBox_TcpServer.Location = new System.Drawing.Point(109, 32);
            this.textBox_TcpServer.Name = "textBox_TcpServer";
            this.textBox_TcpServer.Size = new System.Drawing.Size(251, 23);
            this.textBox_TcpServer.TabIndex = 42;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10F);
            this.label12.Location = new System.Drawing.Point(28, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 14);
            this.label12.TabIndex = 41;
            this.label12.Text = "上传地址：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(435, 23);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(55, 22);
            this.button2.TabIndex = 40;
            this.button2.Text = "DO1关";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(435, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 22);
            this.button1.TabIndex = 39;
            this.button1.Text = "DO1开";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_3);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.richTextBox_log);
            this.tabPage3.Font = new System.Drawing.Font("宋体", 10F);
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(511, 493);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "运行日志";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // richTextBox_log
            // 
            this.richTextBox_log.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_log.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_log.Name = "richTextBox_log";
            this.richTextBox_log.Size = new System.Drawing.Size(511, 493);
            this.richTextBox_log.TabIndex = 0;
            this.richTextBox_log.Text = "";
            // 
            // textBox_httpurl
            // 
            this.textBox_httpurl.Location = new System.Drawing.Point(99, 231);
            this.textBox_httpurl.Name = "textBox_httpurl";
            this.textBox_httpurl.Size = new System.Drawing.Size(371, 23);
            this.textBox_httpurl.TabIndex = 55;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("宋体", 10F);
            this.label18.Location = new System.Drawing.Point(18, 235);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(77, 14);
            this.label18.TabIndex = 54;
            this.label18.Text = "HTTP网关：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 567);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.status);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "MODBUS数据采集";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_devdata)).EndInit();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ComboBox comboBox_SelectCom;
        private System.Windows.Forms.ComboBox Box_BaudRate;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem buy;
        private System.Windows.Forms.ToolStripMenuItem about;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMsg;
        internal System.Windows.Forms.Timer timer_getthreadstatus;
        private System.Windows.Forms.Timer timer_updateshow;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMsg_A;
        private System.Windows.Forms.DataGridView dataGridView_devdata;
        private System.Windows.Forms.ComboBox comboBox_record;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel_threadstatus;
        private System.Windows.Forms.CheckBox checkBox_autorun;
        private System.Windows.Forms.TextBox textBox_RefRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_TimeOut;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_deverid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_fileformat;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button_opendatadir;
        private System.Windows.Forms.Button button_datadir;
        private System.Windows.Forms.TextBox textBox_datadir;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_dbset;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_registerset;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_go;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox richTextBox_log;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button_connectmqttsvr;
        private System.Windows.Forms.ComboBox comboBox_collectfile;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox_TcpServer;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_TcpServerPort;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox_Signmethod;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox_ProductKey;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBox_DeviceName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox_DeviceSecret;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button button_savepramas;
        private System.Windows.Forms.CheckBox checkBox_aliyuniot_connect;
        private System.Windows.Forms.TextBox textBox_httpurl;
        private System.Windows.Forms.Label label18;
    }
}

