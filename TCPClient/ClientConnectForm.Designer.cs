
namespace TCPClient
{
    partial class ClientConnectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientConnectForm));
            this.Control = new System.Windows.Forms.GroupBox();
            this.btnDisconnection = new System.Windows.Forms.Button();
            this.btnAuto = new System.Windows.Forms.CheckBox();
            this.btnConnection = new System.Windows.Forms.Button();
            this.Port = new System.Windows.Forms.Label();
            this.txtPortNum = new System.Windows.Forms.TextBox();
            this.IPaddress = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.Control.SuspendLayout();
            this.SuspendLayout();
            // 
            // Control
            // 
            this.Control.BackColor = System.Drawing.Color.Transparent;
            this.Control.Controls.Add(this.btnDisconnection);
            this.Control.Controls.Add(this.btnAuto);
            this.Control.Controls.Add(this.btnConnection);
            this.Control.Controls.Add(this.Port);
            this.Control.Controls.Add(this.txtPortNum);
            this.Control.Controls.Add(this.IPaddress);
            this.Control.Controls.Add(this.txtIPAddress);
            this.Control.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Control.Location = new System.Drawing.Point(16, 15);
            this.Control.Margin = new System.Windows.Forms.Padding(4);
            this.Control.Name = "Control";
            this.Control.Padding = new System.Windows.Forms.Padding(4);
            this.Control.Size = new System.Drawing.Size(981, 124);
            this.Control.TabIndex = 7;
            this.Control.TabStop = false;
            this.Control.Text = "Control";
            // 
            // btnDisconnection
            // 
            this.btnDisconnection.BackColor = System.Drawing.Color.Transparent;
            this.btnDisconnection.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnection.Location = new System.Drawing.Point(807, 59);
            this.btnDisconnection.Margin = new System.Windows.Forms.Padding(4);
            this.btnDisconnection.Name = "btnDisconnection";
            this.btnDisconnection.Size = new System.Drawing.Size(160, 27);
            this.btnDisconnection.TabIndex = 13;
            this.btnDisconnection.Text = "Disconnect";
            this.btnDisconnection.UseVisualStyleBackColor = false;
            this.btnDisconnection.Click += new System.EventHandler(this.btnDisconnection_Click);
            // 
            // btnAuto
            // 
            this.btnAuto.AutoSize = true;
            this.btnAuto.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAuto.Location = new System.Drawing.Point(39, 94);
            this.btnAuto.Margin = new System.Windows.Forms.Padding(4);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(102, 26);
            this.btnAuto.TabIndex = 12;
            this.btnAuto.Text = "Automatic";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.CheckedChanged += new System.EventHandler(this.btnAuto_CheckedChanged);
            // 
            // btnConnection
            // 
            this.btnConnection.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnection.Location = new System.Drawing.Point(595, 59);
            this.btnConnection.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(160, 27);
            this.btnConnection.TabIndex = 10;
            this.btnConnection.Text = "Connect";
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            // 
            // Port
            // 
            this.Port.AutoSize = true;
            this.Port.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Port.Location = new System.Drawing.Point(335, 36);
            this.Port.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(39, 22);
            this.Port.TabIndex = 9;
            this.Port.Text = "Port";
            this.Port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPortNum
            // 
            this.txtPortNum.Location = new System.Drawing.Point(339, 59);
            this.txtPortNum.Margin = new System.Windows.Forms.Padding(4);
            this.txtPortNum.Name = "txtPortNum";
            this.txtPortNum.Size = new System.Drawing.Size(165, 26);
            this.txtPortNum.TabIndex = 8;
            // 
            // IPaddress
            // 
            this.IPaddress.AutoSize = true;
            this.IPaddress.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IPaddress.Location = new System.Drawing.Point(35, 36);
            this.IPaddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.IPaddress.Name = "IPaddress";
            this.IPaddress.Size = new System.Drawing.Size(85, 22);
            this.IPaddress.TabIndex = 7;
            this.IPaddress.Text = "IP Address";
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIPAddress.Location = new System.Drawing.Point(39, 59);
            this.txtIPAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(205, 26);
            this.txtIPAddress.TabIndex = 6;
            // 
            // ClientConnectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1027, 166);
            this.Controls.Add(this.Control);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ClientConnectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TCPClient";
            this.Control.ResumeLayout(false);
            this.Control.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Control;
        private System.Windows.Forms.Button btnDisconnection;
        private System.Windows.Forms.CheckBox btnAuto;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.Label Port;
        private System.Windows.Forms.TextBox txtPortNum;
        private System.Windows.Forms.Label IPaddress;
        private System.Windows.Forms.TextBox txtIPAddress;
    }
}

