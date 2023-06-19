using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TCPClient
{
    public partial class ClientConnectForm : Form
    {
        // Constant

        private const int NUM_OF_BYTES = 10485760; // 10 MB

        // Variable

        private TCP_Clients client = null;

        public ClientConnectForm()
        {
            InitializeComponent();
        }

        private void btnAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (btnAuto.Checked == false)
            {
                txtIPAddress.Text = txtPortNum.Text = string.Empty;
                txtIPAddress.ReadOnly = txtPortNum.ReadOnly = false;
            }
            else
            {
                txtIPAddress.Text = "127.0.0.1";
                txtPortNum.Text = "8080";
                txtIPAddress.ReadOnly = txtPortNum.ReadOnly = true;
            }
        }

        private bool validateIPAdress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress) || string.IsNullOrWhiteSpace(txtIPAddress.Text))
            {
                // MessageBox.Show("IP address is required", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            Regex validateIPv4Regex = new Regex("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");

            return validateIPv4Regex.IsMatch(ipAddress);

        }

        private bool validatePort(string port)
        {
            if (string.IsNullOrEmpty(port) || string.IsNullOrWhiteSpace(txtPortNum.Text))
            {
                // MessageBox.Show("Port is required", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            Regex validatePortRegex = new Regex("^((6553[0-5])|(655[0-2][0-9])|(65[0-4][0-9]{2})|(6[0-4][0-9]{3})|([1-5][0-9]{4})|([0-5]{0,5})|([0-9]{1,4}))$");
            return validatePortRegex.IsMatch(port);

        }

        private bool validateForm()
        {
            if (validateIPAdress(txtIPAddress.Text) && validatePort(txtPortNum.Text))
            {
                return true;
            }
            else
            {
                MessageBox.Show("IP adress or port invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            if (!validateForm())
            {
                return;
            }
            try
            {
                if (client == null)
                {
                    client = new TCP_Clients(txtIPAddress.Text, Int32.Parse(txtPortNum.Text), NUM_OF_BYTES);
                    if (client.run())
                    {
                        btnDisconnection.Enabled = true;
                        btnConnection.Enabled = false;
                        UserLoginForm login = new UserLoginForm();
                        login.cli = client;
                        login.connectForm = this;
                        login.Show();
                        Hide();
                    }
                    else
                    {
                        client = null;

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDisconnection_Click(sender, e);
            }
        }

        private void btnDisconnection_Click(object sender, EventArgs e)
        {
            client.disconnect();
            client = null;
            MessageBox.Show("Disconnected to server successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnConnection.Enabled = true;
            btnDisconnection.Enabled = false;
            txtIPAddress.ReadOnly = txtPortNum.ReadOnly = false;

            btnAuto.Enabled = true;
            btnAuto.Checked = false;

            txtIPAddress.Text = string.Empty;
            txtPortNum.Text = string.Empty;
        }
    }
}
