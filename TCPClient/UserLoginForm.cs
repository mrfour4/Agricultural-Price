using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TCPClient
{
    public partial class UserLoginForm : Form
    {
        private TCP_Clients client = null;

        private Form connect = null;

        // help clients to login and register with just one connection to the server

        public TCP_Clients cli
        {
            set { client = value; }
        }

        public UserLoginForm()
        {
            InitializeComponent();
        }

        // ClientConnectForm

        public Form connectForm
        {
            set { connect = value; }
        }

        private bool validateUsername()
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Username is required !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtUsername.Text.Length < 6 || txtUsername.Text.Length > 20)
            {
                MessageBox.Show("Username must between [6-20] characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            Match res = Regex.Match(txtUsername.Text, @"^[A-Za-z0-9]+(?:[_-][A-Za-z0-9]+)*$");

            if (res.Value.Length <= 0)
            {
                MessageBox.Show("Invalid Username!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool validatePassword()
        {

            Match res2 = Regex.Match(txtPassword.Text, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_])[A-Za-z\d@$!%*?&_]{8,20}$");
            if (res2.Value.Length <= 0 || txtPassword.Text.ToLower() == txtUsername.Text.ToLower())
            {
                MessageBox.Show("Weak password!\nAt least 1 upper letter, 1 lower letter, 1 special letter \n(@, $, !, %, *, ?, &, _) and 1 number [0-9]\n\nPassword must be between [8-20] characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }


        private void btnShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = (!btnShowPassword.Checked);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (client != null)
                {
                    if (!validateUsername() || !validatePassword()) return;

                    // process send login string

                    byte[] temp = Encoding.UTF8.GetBytes(txtPassword.Text);
                    byte[] haspass = new SHA256CryptoServiceProvider().ComputeHash(temp);
                    string hashedPassword = Convert.ToBase64String(haspass);


                    client.RequestLogin(txtUsername.Text, hashedPassword);
                    client.ReceiveResponse();

                    if (client.session != null)
                    {
                        SearchForm searchForm = new SearchForm();
                        searchForm.cli = client;
                        searchForm.connectForm = connect;
                        searchForm.Show();
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm Register = new RegisterForm();
            Register.cli = client;
            Register.ShowDialog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = string.Empty;
            txtPassword.Text = string.Empty;
            btnShowPassword.Checked = false;
        }

        private void UserLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (client.session == null) connect.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
