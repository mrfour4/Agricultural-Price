using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCP_Server;

namespace TCPServer
{
    public partial class TCPServer : Form
    {
        // constant
        private const int NUM_OF_BYTES = 10485760; // 10 MB
        private const int LEN_BUFFER = 4096;

        // variable
        TCPserver server = null;

        public TCPServer()
        {
            InitializeComponent();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Name: TCPSever\t\t\t\t\n\nAuthor: Quí Tứ, Khang Kim, Thanh Tài \t\t\n\nVersion: 1.0.1\t\t\n\nDate: 08/05/2023", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void TCPServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show("If exit then the server will exit and all clients will be disconnected. Do you want to exit?", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (res == DialogResult.No) e.Cancel = true;
            else e.Cancel = false;
        }

        private void checkboxAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (checkboxAuto.Checked == false)
            {
                txtIPAdress.Text = txtPortNum.Text = "";
                txtIPAdress.ReadOnly = txtPortNum.ReadOnly = false;
            }
            else
            {
                txtIPAdress.Text = "127.0.0.1";
                txtPortNum.Text = "8080";
                txtIPAdress.ReadOnly = txtPortNum.ReadOnly = true;
            }
        }


        private bool validateIPAdress(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress) || string.IsNullOrWhiteSpace(txtIPAdress.Text))
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
                //MessageBox.Show("Port is required", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            Regex validatePortRegex = new Regex("^((6553[0-5])|(655[0-2][0-9])|(65[0-4][0-9]{2})|(6[0-4][0-9]{3})|([1-5][0-9]{4})|([0-5]{0,5})|([0-9]{1,4}))$");
            return validatePortRegex.IsMatch(port);

        }

        private bool validateForm()
        {
            if (validateIPAdress(txtIPAdress.Text) && validatePort(txtPortNum.Text))
            {
                return true;
            }
            else
            {
                MessageBox.Show("IP adress or port invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (validateForm())
            {
                server = new TCPserver(txtIPAdress.Text, Int32.Parse(txtPortNum.Text), LEN_BUFFER, rtbServerLog, rtbClientStatus, NUM_OF_BYTES);
                if (server.canRun())
                {
                    btnRun.Enabled = checkboxAuto.Enabled = false;
                    btnStop.Enabled = btnRestart.Enabled = true;
                }
            }
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            if (server != null)
            {
                server.stop();

                // Avoid errors that cause crashes when stop server 

                await Task.Delay(5000);

                btnStop.Enabled = btnRestart.Enabled = false;
                btnRun.Enabled = checkboxAuto.Enabled = true;
            }
        }

        private async void btnRestart_Click(object sender, EventArgs e)
        {
            if (server != null)
            {
                btnStop_Click(sender, e);

                // Avoid errors that cause crashes when stop server 
                await Task.Delay(5100);

                btnRun_Click(sender, e);
            }
        }
    }
}
