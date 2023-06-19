using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace TCPClient
{
    public class TCP_Clients
    {

        // Constant

        private const int NUM_OF_BYTES = 10485760; // 10 MB

        private byte[] key = Convert.FromBase64String("N/zUxdGCNZPq6d8E7VGu4awmX06vafrWFwZq1vP6ccY=");
        private byte[] iv = Convert.FromBase64String("IIaHKQYNn33SqrHn2tyKQQ==");
        private AesCryptoServiceProvider aes;

        private const string ACTION_REGISTER = "register";
        private const string ACTION_LOGIN = "login";
        private const string ACTION_LOGOUT = "logout";
        private const string ACTION_OPTION = "option";
        private const string ACTION_SEARCH = "search";
        private const string ACTION_CLOSE_SERVER = "server-closed";
        // Variable

        private Socket clientSocket;
        private IPAddress iPAddress;
        private int port;
        private int buffLen;

        public string userAccount;
        public string session;
        public string id;

        // search response from server

        public Dictionary<string, List<string>> searchOption;
        public List<Dictionary<string, string>> agriculturalData;

        // *************** Handle data processing ***********************//

        private void handleRegister(ref Dictionary<string, string> Object)
        {
            if (Object.ContainsKey("status") == false) return;

            if (Object["status"] == "success")
            {
                MessageBox.Show("Register successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(Object["info"], "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void handleLogin(ref Dictionary<string, string> Object)
        {
            if (Object.ContainsKey("status") == false) return;

            if (Object["status"] == "success")
            {
                userAccount = Object["username"];
                session = Object["session"];
                id = Object["id"];

                MessageBox.Show("Login successfully!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show(Object["info"], "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void handleAction(ref Dictionary<string, string> Object)
        {
            if (Object.ContainsKey("type") == false || Object.ContainsKey("brand") == false || Object.ContainsKey("product") == false) return;

            searchOption = new Dictionary<string, List<string>>();
            searchOption.Add("type", Object["type"].Split(',').ToList());
            searchOption.Add("product", Object["product"].Split(',').ToList());
            searchOption.Add("brand", Object["brand"].Split(',').ToList());
        }

        private void handleSearch(ref JToken Object2)
        {
            agriculturalData = new List<Dictionary<string, string>>();

            JArray listProducts = Object2["value"].ToObject<JArray>();

            foreach (JToken product in listProducts)
            {
                agriculturalData.Add(JsonConvert.DeserializeObject<Dictionary<string, string>>(product.ToString()));
            }
        }

        private void handleCloseServer(ref Dictionary<string, string> Object)
        {
            disconnect();
            MessageBox.Show(Object["info"], "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

        private void dispatchAction(ref Dictionary<string, string> Object, ref JToken Object2)
        {
            if (Object.ContainsKey("action") && Object["action"] == ACTION_REGISTER)
            {
                handleRegister(ref Object);
            }
            else if (Object.ContainsKey("action") && Object["action"] == ACTION_LOGIN)
            {
                handleLogin(ref Object);
            }
            else if (Object.ContainsKey("action") && Object["action"] == ACTION_OPTION)
            {
                handleAction(ref Object);
            }
            else if (Object2["action"] != null && Object2["action"].ToString() == ACTION_SEARCH)
            {
                handleSearch(ref Object2);
            }
            else if (Object.ContainsKey("action") && Object["action"] == ACTION_CLOSE_SERVER)
            {
                handleCloseServer(ref Object);
            }
        }


        // *************** Handle request ***********************//

        public void SendRequest(string request)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(request);
                clientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // request logim
        public void RequestLogin(string username, string password)
        {
            string request = "{\"action\":\"login\", \"username\":\"" + username + "\", \"password\":\"" + password + "\"}";
            SendRequest(request);
        }

        // request register
        public void RequestRegister(string username, string password)
        {
            string request = "{\"action\":\"register\", \"username\":\"" + username + "\", \"password\":\"" + password + "\"}";
            SendRequest(request);
        }

        // request logout
        public void RequestLogout(string id, string username)
        {
            string request = "{\"action\":\"logout\",\"id\":\"" + id + "\", \"username\":\"" + username + "\"}";
            SendRequest(request);
            userAccount = null;
            session = null;
            id = string.Empty;
        }

        // search request for searching form
        public void RequestSearch(string date, string type, string product, string brand)
        {
            string request = "{\"action\":\"search\", \"date\":\"" + date + "\", \"type\":\"" + type + "\", \"product\":\"" + product + "\", \"brand\":\"" + brand + "\"}";
            SendRequest(request);
        }

        // option - product of data in searching form
        public void RequestOption()
        {
            string request = "{\"action\":\"option\"}";
            SendRequest(request);
        }

        public void ReceiveResponse()
        {
            try
            {
                byte[] responds = new byte[buffLen];
                int received = clientSocket.Receive(responds, SocketFlags.None);

                if (received == 0) return;

                byte[] data = new byte[received];
                Array.Copy(responds, data, received);

                // get response from server

                string cipher = Encoding.UTF8.GetString(data);

                string json = decryptedData(cipher);

                JToken Object2 = JsonConvert.DeserializeObject<JToken>(json);

                Dictionary<string, string> Object = new Dictionary<string, string>();

                if (Object2["action"].ToString() == ACTION_LOGIN || Object2["action"].ToString() == ACTION_REGISTER || Object2["action"].ToString() == ACTION_OPTION || Object2["action"].ToString() == ACTION_CLOSE_SERVER)
                {
                    Object = Object2.ToObject<Dictionary<string, string>>();
                }

                dispatchAction(ref Object, ref Object2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *************** Encrypt and Decrypt data  ***********************//
        private string encryptedData(string message)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(message);

            // Encrypt the data
            byte[] encryptedBytes;
            using (ICryptoTransform encryptor = aes.CreateEncryptor())
            {
                encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
            }

            string res = Convert.ToBase64String(encryptedBytes);
            return res;
        }

        private string decryptedData(string cipher)
        {
            byte[] encryptedBytes = Convert.FromBase64String(cipher);
            // Decrypt the data
            byte[] decryptedBytes;
            using (ICryptoTransform decryptor = aes.CreateDecryptor())
            {
                decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
            }

            // Convert decrypted bytes to string
            string res = Encoding.UTF8.GetString(decryptedBytes);

            return res;
        }



        // *************** Logic function ***********************//
        public TCP_Clients(string ipAddress, int port, int buffLen = NUM_OF_BYTES)
        {
            this.iPAddress = IPAddress.Parse(ipAddress);
            this.port = port;
            this.buffLen = buffLen;
            this.clientSocket = null;
            this.userAccount = null;
            this.session = null;
            this.id = string.Empty;

            aes = new AesCryptoServiceProvider();
            aes.Key = key;
            aes.IV = iv;
        }

        public bool run()
        {
            if (connect() == false) return false;

            MessageBox.Show("Connected to server successfully", "Connected", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return true;

        }

        private bool connect()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (clientSocket == null)
            {
                MessageBox.Show("Client cannot create socket !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            clientSocket.Connect(iPAddress, port);

            if (!clientSocket.Connected)
            {
                MessageBox.Show("Client cannot connect to server !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public void disconnect()
        {
            try
            {
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Disconnect(false);
                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        ~TCP_Clients()
        {
            disconnect();
        }

    }
}
