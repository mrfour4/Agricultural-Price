using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace TCP_Server
{
    class TCPserver
    {
        //************ Constant *****************//

        private const string API_URL = "https://agriculturalprice-production-8daf.up.railway.app/api/v1/agricultural-price/";

        //************ Get key from Cloud Key Management ***********//

        private const string SECRECT_KEY = "OyjvQSvOKN4AxXDSwDISpwfJH416SSZS";

        //**********************************************************//

        private const int NUM_OF_BYTES = 10485760; // 10 MB
        private const int TIME_REFRESH_DATA = 180000; // 30 minutes

        private const string FOLDER = "DB/";
        private const string PATH_DATA = FOLDER + "Agriculturals/";
        private const string PATH_ACCOUNT = FOLDER + "accounts.json";
        private const string PATH_LOGS = "Logs/";

        private const string ACTION_REGISTER = "register";
        private const string ACTION_LOGIN = "login";
        private const string ACTION_LOGOUT = "logout";
        private const string ACTION_OPTION = "option";
        private const string ACTION_SEARCH = "search";

        //************ Variable *****************//

        private string ipAddress;
        private int port;
        private int buffer_len;
        private byte[] buffer;
        private Socket ListenSocket;
        private List<Socket> listClients;
        private static Thread thread;
        private RichTextBox rtbServerLog, rtbClientStatus;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private int cntTimer;

        // ************** Logic handling function ********************//

        private string formatDate(string date)
        {
            string[] fmats = date.Split('-');

            string day = (Int32.Parse(fmats[0]) < 10) ? "0" + fmats[0] : fmats[0];
            string month = (Int32.Parse(fmats[1]) < 10) ? "0" + fmats[1] : fmats[1];
            string year = fmats[2];

            return $"{year}-{month}-{day}";
        }

        private bool isConnected(Socket socket)
        {
            try
            {
                return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
            }
            catch (SocketException)
            {
                return false;
            }
        }


        // *************** Method get data ***********************//

        // get account data base
        private Dictionary<string, Dictionary<string, string>> getAccountDataBase()
        {

            // if file does not exist in folder , create it

            if (File.Exists(PATH_ACCOUNT) == false)
            {
                Guid id = Guid.NewGuid();
                JObject initialData = new JObject(
                    new JProperty("username", new JObject()),
                    new JProperty("password", new JObject()),
                    new JProperty("session", new JObject()),
                    new JProperty("id", new JObject(new JProperty("number", id.ToString())))
                );
                File.WriteAllText(PATH_ACCOUNT, initialData.ToString());
            }


            Dictionary<string, Dictionary<string, string>> Object = new Dictionary<string, Dictionary<string, string>>();
            string json = File.ReadAllText(PATH_ACCOUNT);

            Object = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
            return Object;
        }

        // get data from file json and using for searching form
        private static string FetchData(string filename = "Default")
        {
            if (filename.IndexOf("Default") >= 0)
            {
                DateTime todaysDate = DateTime.Now.Date;
                filename = todaysDate.Day + "-" + todaysDate.Month + "-" + todaysDate.Year;
            }

            string data = File.ReadAllText(PATH_DATA + filename + ".json");

            if (!string.IsNullOrEmpty(data) && !string.IsNullOrWhiteSpace(data))
            {
                JObject obj = JsonConvert.DeserializeObject<JObject>(data);
                JArray valueArray = obj["agriculturals"]["value"].Value<JArray>();

                string json = JsonConvert.SerializeObject(valueArray);
                return json;
            }

            return null;
        }

        // get option data from client's request.
        private void fetchOption(ref string type, ref string product, ref string brand)
        {
            JArray agriculturalArr = JsonConvert.DeserializeObject<JArray>(FetchData());

            Dictionary<string, string> agriculturalInfo;

            // use for marking the string which was selected
            Dictionary<string, bool> flag = new Dictionary<string, bool>();

            foreach (JToken obj in agriculturalArr)
            {
                agriculturalInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(obj.ToString());

                if (flag.ContainsKey(agriculturalInfo["type"]) == false)
                {
                    type += agriculturalInfo["type"] + ",";
                    flag.Add(agriculturalInfo["type"], true);
                }

                if (flag.ContainsKey(agriculturalInfo["product"]) == false)
                {
                    product += agriculturalInfo["product"] + ",";
                    flag.Add(agriculturalInfo["product"], true);
                }

                if (flag.ContainsKey(agriculturalInfo["brand"]) == false)
                {
                    brand += agriculturalInfo["brand"] + ",";
                    flag.Add(agriculturalInfo["brand"], true);
                }
            }

            // remove last ","

            type = type.Remove(type.Length - 1, 1);
            product = product.Remove(product.Length - 1, 1);
            brand = brand.Remove(brand.Length - 1, 1);

        }

        private async void getDataByDate(string date)
        {
            // check whether folder is existing in folder or not

            if (Directory.Exists(FOLDER) == false) Directory.CreateDirectory(FOLDER);
            if (Directory.Exists(PATH_DATA) == false) Directory.CreateDirectory(PATH_DATA);

            string apiUrl = API_URL + (formatDate(date));


            try
            {

                // Get data from web json file
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(apiUrl);
                httpWebRequest.Method = WebRequestMethods.Http.Get;
                httpWebRequest.Accept = "application/json";

                WebResponse response = httpWebRequest.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                // Process response
                string fileNameT = date;

                // write to file DB (data base)
                File.WriteAllText(PATH_DATA + fileNameT + ".json", responseString);

            }
            catch (HttpRequestException ex)
            {
                // Handle HTTP errors
                MessageBox.Show($"An HTTP error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                // Handle other errors
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async void getDataAndSaveToFile()
        {
            while (true)
            {
                // check whether folder is existing in folder or not
                if (Directory.Exists(FOLDER) == false) Directory.CreateDirectory(FOLDER);
                if (Directory.Exists(PATH_DATA) == false) Directory.CreateDirectory(PATH_DATA);

                try
                {
                    // Create HttpClient instance
                    HttpClient client = new HttpClient();

                    // Make GET request and get response
                    HttpResponseMessage response = await client.GetAsync(API_URL + DateTime.Now.ToString("yyyy-MM-dd"));
                    response.EnsureSuccessStatusCode(); // Throws exception if not successful

                    // Read response content as string
                    string responseString = await response.Content.ReadAsStringAsync();

                    // Process response
                    // write data from web to file (by day - month - year)
                    DateTime date = DateTime.Now.Date;
                    string filename = date.Day + "-" + date.Month + "-" + date.Year;

                    // write to file DB (database)
                    File.WriteAllText(PATH_DATA + filename + ".json", responseString);

                    // Analysis json file
                    Thread.Sleep(TIME_REFRESH_DATA);

                }
                catch (HttpRequestException ex)
                {
                    // Handle HTTP errors
                    MessageBox.Show($"An HTTP error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                catch (Exception ex)
                {
                    // Handle other errors
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // *************** Handle data processing ***********************//

        private void handleRegister(
            ref Socket current,
            ref Dictionary<string, string> Object,
            ref Dictionary<string, Dictionary<string, string>> DB
        )
        {
            // no username or password ==>  return
            if (Object.ContainsKey("username") == false || Object.ContainsKey("password") == false) return;

            if (DB["username"].ContainsKey(Object["username"]) == true)
            {
                // user already exists

                byte[] sendData = Encoding.UTF8.GetBytes("{\"action\":\"register\", \"status\":\"error\", \"info\":\"Username already exists !\"}");
                current.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(sendCallback), current);
            }
            else
            {
                // create user
                string id = Guid.NewGuid().ToString();
                DB["username"].Add(Object["username"], id);
                DB["password"].Add(id, Object["password"]);
                DB["id"]["number"] = id;

                string jsonstr = JsonConvert.SerializeObject(DB);
                File.WriteAllText(PATH_ACCOUNT, jsonstr);

                // write on server log
                string clientIP = ((IPEndPoint)current.RemoteEndPoint).Address.ToString();
                string clientPort = ((IPEndPoint)current.RemoteEndPoint).Port.ToString();

                // send response
                byte[] sendData = Encoding.UTF8.GetBytes("{\"action\":\"register\", \"status\":\"success\"}");

                current.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(sendCallback), current);
                tcpInvoke("Client IP: " + clientIP + " - Port: " + clientPort + " signed up - " + "Username: " + Object["username"] + "\n", rtbServerLog);
            }
        }

        private void handleLogin(
            ref Socket current,
            ref Dictionary<string, string> Object,
            ref Dictionary<string, Dictionary<string, string>> DB
        )
        {
            if (Object.ContainsKey("username") == false || Object.ContainsKey("password") == false) return;

            if (DB["username"].ContainsKey(Object["username"]) == true)
            {
                string id = DB["username"][Object["username"]];

                // check session in real time
                if (DB["session"].ContainsKey(id) == false)
                {
                    string pass = DB["password"][id];
                    if (pass == Object["password"] && pass.Length == Object["password"].Length)
                    {
                        // generate JWT token
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.ASCII.GetBytes(SECRECT_KEY);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(ClaimTypes.Name, Object["username"]),
                                new Claim(ClaimTypes.NameIdentifier, id),
                                new Claim(ClaimTypes.AuthenticationMethod, "JWT")
                            }),
                            Expires = DateTime.UtcNow.AddDays(7),
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var authToken = tokenHandler.WriteToken(token);

                        // store token in database
                        DB["session"][id] = authToken;
                        string jsontr = JsonConvert.SerializeObject(DB);
                        File.WriteAllText(PATH_ACCOUNT, jsontr);

                        // write on server log
                        string clientIP = ((IPEndPoint)current.RemoteEndPoint).Address.ToString();
                        string clientPort = ((IPEndPoint)current.RemoteEndPoint).Port.ToString();

                        // send response

                        byte[] sendData = Encoding.UTF8.GetBytes("{\"action\":\"login\", \"status\":\"success\", \"username\":\"" + Object["username"] + "\", \"session\":\"" + authToken + "\", \"id\":\"" + id + "\"}");
                        current.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(sendCallback), current);

                        tcpInvoke("Client IP: " + clientIP + " - Port: " + clientPort + " has logged in - " + "Username: " + Object["username"] + "\n", rtbServerLog);
                    }
                    else
                    {
                        byte[] sendData = Encoding.UTF8.GetBytes("{\"action\":\"login\", \"status\":\"error\", \"info\":\"Wrong username or password !\"}");
                        current.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(sendCallback), current);
                    }
                }
                // if login in real-time now ==> notification 
                else
                {
                    byte[] sendData = Encoding.UTF8.GetBytes("{\"action\":\"login\", \"status\":\"error\", \"info\":\"Your account has already logged in!\"}");
                    current.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(sendCallback), current);
                }

            }
            // no username in data base (non-existent)
            else
            {
                byte[] sendData = Encoding.UTF8.GetBytes("{\"action\":\"login\", \"status\":\"error\", \"info\":\"Username does not exist! Please register an account !\"}");
                current.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(sendCallback), current);
            }
        }

        private void handleLogout(
           ref Socket current,
           ref Dictionary<string, string> Object,
           ref Dictionary<string, Dictionary<string, string>> DB
        )
        {
            if (Object.ContainsKey("id") == false) return;
            else
            {
                DB["session"].Remove(Object["id"]);
                string jsonstr = JsonConvert.SerializeObject(DB);

                // write on server log

                string clientIP = ((IPEndPoint)current.RemoteEndPoint).Address.ToString();
                string clientPort = ((IPEndPoint)current.RemoteEndPoint).Port.ToString();

                File.WriteAllText(PATH_ACCOUNT, jsonstr);

                byte[] sendData = Encoding.UTF8.GetBytes("{\"action\":\"logout\", \"status\":\"success\"}");
                current.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(sendCallback), current);

                tcpInvoke("Client IP: " + clientIP + " - Port: " + clientPort + " logged out - " + "Username: " + Object["username"] + "\n", rtbServerLog);
            }
        }

        private void handleOption(Socket current)
        {

            string type, product, brand;
            type = product = brand = null;

            fetchOption(ref type, ref product, ref brand);

            string response = "{\"action\":\"option\", \"type\":\"" + type + "\", \"product\":\"" + product + "\", \"brand\":\"" + brand + "\"}";
            byte[] sendData = Encoding.UTF8.GetBytes(response);

            // send data to client
            current.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(sendCallback), current);
        }

        private void handleSearch(ref Socket current, ref Dictionary<string, string> Object)
        {
            if (!Object.ContainsKey("date") || !Object.ContainsKey("type") || !Object.ContainsKey("product") || !Object.ContainsKey("brand")) return;

            // if file data does not exist, create it

            if (Object["date"] != "Default" && File.Exists(PATH_DATA + Object["date"] + ".json") == false)
            {
                getDataByDate(Object["date"]);
            }

            string data = search(Object["date"], Object["type"], Object["product"], Object["brand"]);

            string response = "{\"action\":\"search\",\"value\":" + data + "}";
            byte[] sendData = Encoding.UTF8.GetBytes(response);

            current.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, new AsyncCallback(sendCallback), current);
        }

        private void dispatchAction(ref Socket current, int received)
        {
            // convert received data to json

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string json = Encoding.UTF8.GetString(recBuf);

            Dictionary<string, string> Object = new Dictionary<string, string>();
            Object = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            try
            {
                // no action ==> return

                if (Object.ContainsKey("action") == false) return;

                // action == "login" or action == "register"

                if (Object["action"] == ACTION_LOGIN || Object["action"] == ACTION_REGISTER || Object["action"] == ACTION_LOGOUT)
                {
                    Dictionary<string, Dictionary<string, string>> DB = getAccountDataBase();

                    if (Object["action"] == ACTION_REGISTER)
                    {
                        handleRegister(ref current, ref Object, ref DB);
                    }
                    else if (Object["action"] == ACTION_LOGIN)
                    {
                        handleLogin(ref current, ref Object, ref DB);
                    }
                    else if (Object["action"] == ACTION_LOGOUT)
                    {
                        handleLogout(ref current, ref Object, ref DB);
                    }
                }
                else if (Object["action"] == ACTION_OPTION)
                {
                    handleOption(current);
                }
                else if (Object["action"] == ACTION_SEARCH)
                {
                    handleSearch(ref current, ref Object);
                }

                // continue receive request
                current.BeginReceive(buffer, 0, buffer_len, SocketFlags.None, new AsyncCallback(ReceiveCallback), current);
            }
            // catch  exception
            catch (SocketException socketException)
            {
                MessageBox.Show(socketException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (ObjectDisposedException objectDisposedException)
            {
                MessageBox.Show(objectDisposedException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // process searching form
        private static string search(string date, string type, string product, string brand)
        {
            List<string> agriculturalData = new List<string>();

            JArray AgriInfo = JsonConvert.DeserializeObject<JArray>(FetchData(date));

            Dictionary<string, string> agriculturalArr;

            if (type == null || product == null || brand == null) return null;

            foreach (JToken jsonObj in AgriInfo)
            {
                agriculturalArr = jsonObj.ToObject<Dictionary<string, string>>();

                if (!agriculturalArr.ContainsKey("type") || !agriculturalArr.ContainsKey("brand") || !agriculturalArr.ContainsKey("product")) continue;

                if (brand != "Default" && agriculturalArr["brand"].IndexOf(brand) < 0) continue;

                if (type != "Default" && agriculturalArr["type"].IndexOf(type) < 0) continue;

                if (product != "Default" && agriculturalArr["product"].IndexOf(product) < 0) continue;

                agriculturalData.Add(jsonObj.ToString());
            }

            string json = JsonConvert.SerializeObject(agriculturalData);

            return (json.Length > 0) ? json : null;
        }

        public void stop()
        {
            try
            {
                string filename = DateTime.Now.ToString("dd-MM-yyyy-HHmmss");

                string pathFolder = PATH_LOGS + DateTime.Now.ToString("dd-MM-yyyy");

                if (Directory.Exists(PATH_LOGS) == false) Directory.CreateDirectory(PATH_LOGS);

                if (Directory.Exists(pathFolder) == false)
                {
                    Directory.CreateDirectory(pathFolder);
                }

                File.WriteAllText(pathFolder + "/" + filename + ".log", rtbServerLog.Text);


                // stop server and starting countdown

                cntTimer = 5; // 5s
                var sendData = Encoding.UTF8.GetBytes("Server will close after " + cntTimer.ToString() + " seconds.\r\n");

                rtbServerLog.Text = Encoding.UTF8.GetString(sendData);

                startAsyncTimedWork();
                clearInvoke(rtbServerLog);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // *************** Callback method ***********************//

        private void acceptCallback(IAsyncResult AR)
        {
            Socket socket;
            try
            {
                socket = ListenSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException)
            {
                MessageBox.Show("Server closed", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            listClients.Add(socket);

            // Show on left-hand form
            string clientIP = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
            string clientPort = ((IPEndPoint)socket.RemoteEndPoint).Port.ToString();

            // write on form list client
            tcpInvoke("Client IP: " + clientIP + " - Port: " + clientPort + "\n", rtbClientStatus);

            socket.BeginReceive(buffer, 0, buffer_len, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            ListenSocket.BeginAccept(acceptCallback, null);
        }

        private void sendCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = (Socket)AR.AsyncState;
                socket.EndSend(AR);
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;

            if (isConnected(current) == false)
            {
                string clientIP = ((IPEndPoint)current.RemoteEndPoint).Address.ToString();
                string clientPort = ((IPEndPoint)current.RemoteEndPoint).Port.ToString();

                // write on form rtbClientStatus

                printListCLients(rtbClientStatus, "Client IP: " + clientIP + " - Port: " + clientPort + "\n", "");
                current.Close();

                listClients.Remove(current);
                return;
            }

            try
            {
                received = current.EndReceive(AR);
            }
            catch (Exception)
            {
                tcpInvoke("Client disconnected\n", rtbServerLog);
                current.Close();
                listClients.Remove(current);
                return;
            }

            dispatchAction(ref current, received);
        }

        // *************** Write data on UI  ***********************//

        // write data to richtextbox in main thread
        void tcpInvoke(string text, RichTextBox current)
        {
            if (current.IsDisposed == false)
            {
                current.Invoke(new MethodInvoker(delegate
                {
                    current.Text += text;
                }));
            }
        }

        // server closing notice
        void ServerClosingNotification(string text, RichTextBox current)
        {
            if (current.IsDisposed == false)
            {
                current.Invoke(new MethodInvoker(delegate { current.Text = text; }));
            }
        }

        // print out the list of clients
        private void printListCLients(RichTextBox obj, string strdOld, string strNew)
        {
            if (obj.IsDisposed == false)
            {
                obj.Invoke(new MethodInvoker(delegate
                {
                    obj.Text = obj.Text.Replace(strdOld, strNew);
                }));
            }
        }

        // clear data to richtextbox in main thread
        private static void clearInvoke(RichTextBox current)
        {
            if (current.IsDisposed == false)
            {
                current.Invoke(new MethodInvoker(delegate
                {
                    current.Clear();
                }));
            }
        }

        // *************** Exception handling ***********************//

        private void startAsyncTimedWork()
        {
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
            timer.Interval = 999; // infinite
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            cntTimer--;
            var sendData = Encoding.UTF8.GetBytes("Server will close after " + cntTimer.ToString() + " seconds.\r\n");

            ServerClosingNotification(Encoding.UTF8.GetString(sendData), rtbServerLog);

            if (cntTimer == 0)
            {
                timer.Stop();
                timer.Tick -= Timer_Tick;

                byte[] sendData_client = Encoding.UTF8.GetBytes("{\"action\":\"server-closed\",\"info\":\"Server was closed !\"}");

                foreach (Socket socket in listClients)
                {
                    socket.BeginSend(sendData_client, 0, sendData_client.Length, SocketFlags.None, new AsyncCallback(sendCallback), socket);
                }
                closeAllSockets();
            }
        }

        private void closeAllSockets()
        {
            try
            {
                clearInvoke(rtbClientStatus);

                foreach (Socket socket in listClients)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Disconnect(false);
                }

                ListenSocket.Close();
                thread.Abort();

                ServerClosingNotification("Server closed\n", rtbServerLog);

                // release memory
                GC.Collect();
            }

            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ObjectDisposedException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // *************** Public method ***********************//
        public TCPserver(string ipAddress, int port, int len_buffer, RichTextBox obj, RichTextBox obj2, int len = NUM_OF_BYTES)
        {
            this.ipAddress = ipAddress;
            this.port = port;
            this.buffer_len = len_buffer;
            this.buffer = new byte[len_buffer];
            this.rtbServerLog = obj;
            this.rtbClientStatus = obj2;
        }

        public bool canRun()
        {
            listClients = new List<Socket>();
            ListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            if (ListenSocket == null)
            {
                MessageBox.Show("Create socket with error.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                ListenSocket.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // always listen
            ListenSocket.Listen(0);

            // get data from Web
            thread = new Thread(getDataAndSaveToFile);
            thread.Start();

            // clear previous session (client's account) 
            // delete the account session if the server suddenly crashes

            Dictionary<string, Dictionary<string, string>> DB = getAccountDataBase();
            DB["session"].Clear();

            // after that, re-write DB
            string jsonstr = JsonConvert.SerializeObject(DB);
            File.WriteAllText(PATH_ACCOUNT, jsonstr);

            // write on server log
            tcpInvoke("Server is running...\n", rtbServerLog);

            // begin accept
            ListenSocket.BeginAccept(acceptCallback, null);
            return true;
        }

        ~TCPserver()
        {
            closeAllSockets();
        }
    }
}


