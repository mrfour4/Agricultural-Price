using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TCPClient
{
    public partial class SearchForm : Form
    {
        private TCP_Clients client = null;
        private Form connect = null;

        public TCP_Clients cli
        {
            set { client = value; }
        }

        public Form connectForm
        {
            set { connect = value; }
        }

        public SearchForm()
        {
            InitializeComponent();
        }

        private void FormLogin()
        {
            UserLoginForm login = new UserLoginForm();
            login.cli = client;
            login.connectForm = connect;
            login.Show();
            Close();
        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            if (client.session == null) Close();
            else
            {

                // request option:  type ? product ? or brand?
                client.RequestOption();
                client.ReceiveResponse(); // receive request option
                                          // check valid search option
                                          // client.SearchOption.Count > 0
                if (client.searchOption.Count > 0)
                {
                    // add to combobox
                    foreach (string obj in client.searchOption["type"]) cbProduct.Items.Add(obj);
                    foreach (string obj in client.searchOption["product"]) cbType.Items.Add(obj);
                    foreach (string obj in client.searchOption["brand"]) cbBrand.Items.Add(obj);

                }
                else
                {
                    MessageBox.Show("Load option failed!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Request all infor (type, product, brand, date) in file  and display in data table ( mode default )
                client.RequestSearch("Default", "Default", "Default", "Default");
                client.ReceiveResponse();

                string[] Item = new string[6];
                ListViewItem LItems;

                if (client.agriculturalData == null) return;

                foreach (Dictionary<string, string> child in client.agriculturalData)
                {
                    Item[0] = child["product"];
                    Item[1] = child["type"];
                    Item[2] = child["brand"];
                    Item[3] = child["price"];
                    Item[4] = child["updated"]; //date

                    LItems = new ListViewItem(Item);
                    DataTable.Items.Add(LItems);
                }
            }
        }

        private void Reset_button_Click(object sender, EventArgs e)
        {
            client.RequestSearch("Default", "Default", "Default", "Default");
            client.ReceiveResponse();
            cbProduct.Text = "";
            cbType.Text = "";
            cbBrand.Text = "";
            DateBox.Text = "";


            DataTable.Items.Clear();

            string[] Item = new string[6];
            ListViewItem LItems;
            if (client.agriculturalData == null) return;
            foreach (Dictionary<string, string> child in client.agriculturalData)
            {
                Item[0] = child["type"];
                Item[1] = child["product"];
                Item[2] = child["brand"];
                Item[3] = child["price"];
                Item[4] = child["updated"]; //date
                // add new row
                LItems = new ListViewItem(Item);
                DataTable.Items.Add(LItems);
            }
        }
        private void Logout_button_Click(object sender, EventArgs e)
        {
            client.RequestLogout(client.id, client.userAccount);
            client.ReceiveResponse();
            FormLogin();
        }

        private void SearchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client.session != null)
            {
                Logout_button_Click(sender, new EventArgs());
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (DateBox.Value > DateTime.Now)
            {
                MessageBox.Show("Invalid date", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string type = (string.IsNullOrEmpty(cbProduct.Text) || string.IsNullOrWhiteSpace(cbProduct.Text)) ? "Default" : cbProduct.Text;
            string product = (string.IsNullOrEmpty(cbType.Text) || string.IsNullOrWhiteSpace(cbType.Text)) ? "Default" : cbType.Text;
            string brand = (string.IsNullOrEmpty(cbBrand.Text) || string.IsNullOrWhiteSpace(cbBrand.Text)) ? "Default" : cbBrand.Text;
            string date = DateBox.Value.Day + "-" + DateBox.Value.Month + "-" + DateBox.Value.Year;

            client.RequestSearch(date, type, product, brand);
            client.ReceiveResponse();

            // update data table
            //  clear old data of items in data table
            DataTable.Items.Clear();

            string[] Item = new string[5];
            ListViewItem LItems;

            if (client.agriculturalData == null) return;

            foreach (Dictionary<string, string> child in client.agriculturalData)
            {
                Item[0] = child["type"];
                Item[1] = child["product"];
                Item[2] = child["brand"];
                Item[3] = child["price"];
                Item[4] = child["updated"]; //date

                // add new row
                LItems = new ListViewItem(Item);
                DataTable.Items.Add(LItems);
            }
        }

        private void btnContribute_Click(object sender, EventArgs e)
        {
            ContributeForm contributeForm = new ContributeForm();
            contributeForm.Show();
        }
    }
}
