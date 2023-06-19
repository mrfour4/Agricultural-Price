using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TCPClient
{
    public partial class RegisterForm : Form
    {
        private TCP_Clients client;

        // help clients to login and register with just one connection to the server
        public TCP_Clients cli
        {
            set { client = value; }
        }

        public RegisterForm()
        {
            InitializeComponent();
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

        private bool confirmPassword()
        {
            if (txtPassword.Text == txtConfirmPassword.Text && txtPassword.Text.Length == txtConfirmPassword.Text.Length) return true;
            else
            {
                MessageBox.Show("Confirm Password does not match!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!validateUsername() || !validatePassword() || !confirmPassword())
            {
                return;
            }

            byte[] temp = Encoding.UTF8.GetBytes(txtPassword.Text);
            byte[] haspass = new SHA256CryptoServiceProvider().ComputeHash(temp);
            string hashedPassword = Convert.ToBase64String(haspass);


            client.RequestRegister(txtUsername.Text, hashedPassword);
            client.ReceiveResponse();
        }

        private void btnShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = (!btnShowPassword.Checked);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Text = txtPassword.Text = txtConfirmPassword.Text = string.Empty;
            btnShowPassword.Checked = false;
        }
    }
}
