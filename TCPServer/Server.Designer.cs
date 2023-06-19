
namespace TCPServer
{
    partial class TCPServer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCPServer));
            this.CONTROLS = new System.Windows.Forms.GroupBox();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.checkboxAuto = new System.Windows.Forms.CheckBox();
            this.Port = new System.Windows.Forms.Label();
            this.txtPortNum = new System.Windows.Forms.TextBox();
            this.IPAddress = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIPAdress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbServerLog = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.MENU = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbClientStatus = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.CONTROLS.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CONTROLS
            // 
            this.CONTROLS.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CONTROLS.Controls.Add(this.btnRestart);
            this.CONTROLS.Controls.Add(this.btnStop);
            this.CONTROLS.Controls.Add(this.btnRun);
            this.CONTROLS.Controls.Add(this.checkboxAuto);
            this.CONTROLS.Controls.Add(this.Port);
            this.CONTROLS.Controls.Add(this.txtPortNum);
            this.CONTROLS.Controls.Add(this.IPAddress);
            this.CONTROLS.Controls.Add(this.label1);
            this.CONTROLS.Controls.Add(this.txtIPAdress);
            this.CONTROLS.ForeColor = System.Drawing.Color.Black;
            this.CONTROLS.Location = new System.Drawing.Point(0, 33);
            this.CONTROLS.Margin = new System.Windows.Forms.Padding(4);
            this.CONTROLS.Name = "CONTROLS";
            this.CONTROLS.Padding = new System.Windows.Forms.Padding(4);
            this.CONTROLS.Size = new System.Drawing.Size(1059, 145);
            this.CONTROLS.TabIndex = 7;
            this.CONTROLS.TabStop = false;
            this.CONTROLS.Text = "CONTROLS";
            // 
            // btnRestart
            // 
            this.btnRestart.Enabled = false;
            this.btnRestart.Location = new System.Drawing.Point(825, 46);
            this.btnRestart.Margin = new System.Windows.Forms.Padding(4);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(100, 28);
            this.btnRestart.TabIndex = 11;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(696, 46);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(100, 28);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(557, 46);
            this.btnRun.Margin = new System.Windows.Forms.Padding(4);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(100, 28);
            this.btnRun.TabIndex = 9;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // checkboxAuto
            // 
            this.checkboxAuto.AutoSize = true;
            this.checkboxAuto.Location = new System.Drawing.Point(48, 81);
            this.checkboxAuto.Margin = new System.Windows.Forms.Padding(4);
            this.checkboxAuto.Name = "checkboxAuto";
            this.checkboxAuto.Size = new System.Drawing.Size(88, 20);
            this.checkboxAuto.TabIndex = 8;
            this.checkboxAuto.Text = "Automatic";
            this.checkboxAuto.UseVisualStyleBackColor = true;
            this.checkboxAuto.CheckedChanged += new System.EventHandler(this.checkboxAuto_CheckedChanged);
            // 
            // Port
            // 
            this.Port.AutoSize = true;
            this.Port.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Port.Location = new System.Drawing.Point(309, 26);
            this.Port.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(39, 22);
            this.Port.TabIndex = 4;
            this.Port.Text = "Port";
            // 
            // txtPortNum
            // 
            this.txtPortNum.Location = new System.Drawing.Point(313, 49);
            this.txtPortNum.Margin = new System.Windows.Forms.Padding(4);
            this.txtPortNum.Name = "txtPortNum";
            this.txtPortNum.Size = new System.Drawing.Size(160, 22);
            this.txtPortNum.TabIndex = 3;
            // 
            // IPAddress
            // 
            this.IPAddress.AutoSize = true;
            this.IPAddress.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPAddress.Location = new System.Drawing.Point(47, 26);
            this.IPAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.IPAddress.Name = "IPAddress";
            this.IPAddress.Size = new System.Drawing.Size(85, 22);
            this.IPAddress.TabIndex = 2;
            this.IPAddress.Text = "IP Address";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 1;
            // 
            // txtIPAdress
            // 
            this.txtIPAdress.Location = new System.Drawing.Point(48, 49);
            this.txtIPAdress.Margin = new System.Windows.Forms.Padding(4);
            this.txtIPAdress.Name = "txtIPAdress";
            this.txtIPAdress.Size = new System.Drawing.Size(212, 22);
            this.txtIPAdress.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(673, 162);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 22);
            this.label2.TabIndex = 22;
            this.label2.Text = "Client Status (connected)";
            // 
            // rtbServerLog
            // 
            this.rtbServerLog.BackColor = System.Drawing.Color.White;
            this.rtbServerLog.Cursor = System.Windows.Forms.Cursors.No;
            this.rtbServerLog.Location = new System.Drawing.Point(0, 186);
            this.rtbServerLog.Margin = new System.Windows.Forms.Padding(4);
            this.rtbServerLog.Name = "rtbServerLog";
            this.rtbServerLog.ReadOnly = true;
            this.rtbServerLog.Size = new System.Drawing.Size(668, 432);
            this.rtbServerLog.TabIndex = 17;
            this.rtbServerLog.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MENU});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1064, 28);
            this.menuStrip1.TabIndex = 20;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // MENU
            // 
            this.MENU.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.btnAbout,
            this.toolStripSeparator2,
            this.btnExit});
            this.MENU.Name = "MENU";
            this.MENU.Size = new System.Drawing.Size(65, 24);
            this.MENU.Text = "MENU";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(130, 6);
            // 
            // btnAbout
            // 
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(133, 26);
            this.btnAbout.Text = "About";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(130, 6);
            // 
            // btnExit
            // 
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(133, 26);
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // rtbClientStatus
            // 
            this.rtbClientStatus.BackColor = System.Drawing.Color.White;
            this.rtbClientStatus.Cursor = System.Windows.Forms.Cursors.No;
            this.rtbClientStatus.Location = new System.Drawing.Point(677, 186);
            this.rtbClientStatus.Margin = new System.Windows.Forms.Padding(4);
            this.rtbClientStatus.Name = "rtbClientStatus";
            this.rtbClientStatus.ReadOnly = true;
            this.rtbClientStatus.Size = new System.Drawing.Size(380, 432);
            this.rtbClientStatus.TabIndex = 21;
            this.rtbClientStatus.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 162);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 22);
            this.label3.TabIndex = 23;
            this.label3.Text = "Server log";
            // 
            // TCPServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 620);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rtbClientStatus);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.rtbServerLog);
            this.Controls.Add(this.CONTROLS);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TCPServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TCPServer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TCPServer_FormClosing);
            this.CONTROLS.ResumeLayout(false);
            this.CONTROLS.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox CONTROLS;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.CheckBox checkboxAuto;
        private System.Windows.Forms.Label Port;
        private System.Windows.Forms.TextBox txtPortNum;
        private System.Windows.Forms.Label IPAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIPAdress;
        private System.Windows.Forms.RichTextBox rtbServerLog;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem MENU;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem btnAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem btnExit;
        private System.Windows.Forms.RichTextBox rtbClientStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

