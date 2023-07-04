using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;

namespace TCPClient
{
    public partial class ContributeForm : Form
    {
        string apiUrl = "https://agricultural-price-api.onrender.com/api/v1/agricultural-price/update";

        public ContributeForm()
        {
            InitializeComponent();
        }

        private async void sendContribute(string product, string marketName, int minPrice, int maxPrice)
        {
            // Create JSON payload
            var payload = new
            {
                product = product,
                marketName = marketName,
                minPrice = minPrice,
                maxPrice = maxPrice
            };


            // Serialize payload to JSON string
            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);

            using (HttpClient client = new HttpClient())
            {
                // Set request content type to JSON
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // Send PUT request with JSON body
                HttpResponseMessage response = await client.PutAsync(apiUrl, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                // Handle response
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Request succeeded", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnContribute.Enabled = true;
                    txtProduct.Text = txtPriceMin.Text = txtPriceMax.Text = "";

                }
                else
                {
                    MessageBox.Show("Request failed with status code: " + response.StatusCode, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnContribute.Enabled = true;

                }
            }
        }

        private void btnContribute_Click(object sender, EventArgs e)
        {
            if ((string.IsNullOrEmpty(cbType.Text) || string.IsNullOrWhiteSpace(cbType.Text)))
            {
                MessageBox.Show("Please choose type", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((string.IsNullOrEmpty(cbBrand.Text) || string.IsNullOrWhiteSpace(cbBrand.Text)))
            {
                MessageBox.Show("Please choose brand", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int pMinPrice, pMaxPrice;


            if (!int.TryParse(txtPriceMin.Text, out pMinPrice))
            {
                MessageBox.Show("Min price invalid", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!int.TryParse(txtPriceMax.Text, out pMaxPrice))
            {
                MessageBox.Show("Max price invalid", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Create JSON payload
            var payload = new
            {
                product = txtProduct.Text,
                marketName = cbBrand.Text,
                minPrice = pMinPrice,
                maxPrice = pMaxPrice
            };

            btnContribute.Enabled = false;

            sendContribute(payload.product, payload.marketName, payload.minPrice, payload.maxPrice);

            btnContribute.Enabled = true;
        }
    }
}
