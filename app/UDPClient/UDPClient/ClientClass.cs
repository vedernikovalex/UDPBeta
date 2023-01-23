using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SlidingWindow;
using System.Configuration;

namespace UDPClient
{
    /// <summary>
    /// Client class
    /// Contains IP address and port to configure server connection
    /// Contains port to initialize UdpClient sender and listener
    /// Contains IPEndPoint of a server to have a sending destination 
    /// </summary>
    public class ClientClass
    {
        public string input, ip;
        public int port, portClient;

        private bool connected = false;

        private UdpClient senderClient, receiverClient;
        private IPEndPoint endPoint;

        private readonly UserInput userInput = new UserInput();
        private readonly ServerConfig configurationInput = new ServerConfig();

        /// <summary>
        /// Constructor which starts ConfigureConnection method
        /// Proceeds to Connect method which initialize a connection with server
        /// </summary>
        public ClientClass()
        {
            ConfigureConnection();
            Connect();
        }

        public bool Connected
        {
            get { return connected; }
        }

        /// <summary>
        /// Configures parameters for server connection by config file or manual user input
        /// </summary>
        public void ConfigureConnection()
        {
            RedundantMethods.UserAlert("CONFIGURATION", "Configurate your connection", "Get IP Address/PORT of server from configuration file or assign it manualy");
            input = String.Empty;
            while (true)
            {
                input = userInput.GetInput("Write \"config\" or \"manual\"");
                if (input == "config")
                {
                    try
                    {
                        ip = ConfigurationManager.AppSettings.Get("ip");
                        port = Int32.Parse(ConfigurationManager.AppSettings.Get("port"));
                        portClient = Int32.Parse(ConfigurationManager.AppSettings.Get("portClient"));
                    }
                    catch (FormatException e)
                    {
                        RedundantMethods.UserAlert("Configuration file was not found or incorrectly formated");
                    }
                    break;
                }
                else if (input == "manual")
                {
                    while (true)
                    {
                        string inputIp = configurationInput.GetIP("IP Address");
                        if (IPAddress.TryParse(inputIp, out IPAddress valid) && !String.IsNullOrEmpty(inputIp))
                        {
                            ip = inputIp;
                            break;
                        }
                        RedundantMethods.UserAlert("IP Address was invalid");
                    }
                    while (true)
                    {
                        int portInputServer = Int32.Parse(configurationInput.GetPORT("Port of SERVER"));
                        if (portInputServer >= 1 && portInputServer <= 65535)
                        {
                            port = portInputServer;
                            break;
                        }
                        RedundantMethods.UserAlert("Port was invalid");
                    }
                    while (true)
                    {
                        int portInput = Int32.Parse(configurationInput.GetPORTClient("Port of CLIENT"));
                        if (portInput >= 1 && portInput <= 65535)
                        {
                            portClient = portInput;
                            break;
                        }
                        RedundantMethods.UserAlert("Port was invalid");
                    }
                    break;
                }
                RedundantMethods.UserAlert("There was an error in inputed message");
            }
            if (port != 0 && !String.IsNullOrEmpty(ip))
            {
                endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                senderClient = new UdpClient(portClient);
                receiverClient = new UdpClient(portClient + 1);
                RedundantMethods.UserAlert("Configuration was successful", "Server config: " + endPoint);
            }
            else
            {
                RedundantMethods.UserAlert("Configuration wasn't successful");
                ConfigureConnection();
            }
        }

        /// <summary>
        /// Esabilishes server connection and proceeds to start receiving and input methods
        /// </summary>
        public void Connect()
        {
            try
            {
                senderClient.Connect(endPoint);
                senderClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                connected = true;
                RedundantMethods.UserAlert("Successfully connected", "You are able to participate in chatting now");
                StartInput();
                StartReceive();
            }
            catch (SocketException e)
            {
                RedundantMethods.UserExceptionError(e.Message, "No active server found");
                Disconnet();
            }
            catch (NullReferenceException e)
            {
                RedundantMethods.UserExceptionError(e.Message, "No active server found");
                Disconnet();
            }
            catch (ObjectDisposedException e)
            {
                RedundantMethods.UserExceptionError(e.Message, "Server connection was dispossed");
                Disconnet();
            }
        }

        /// <summary>
        /// Handles disconnectance of client from server
        /// </summary>
        public void Disconnet()
        {
            try
            {
                senderClient.Close();
                receiverClient.Close();
            }
            catch(SocketException e)
            {
                RedundantMethods.UserExceptionError(e.Message, "Already disconnected");
            }
            finally
            {
                connected = false;
            }
        }

        /// <summary>
        /// listening for incoming data using thread 
        /// </summary>
        public void StartReceive()
        {
            //Thread receiverWindow = new Thread(() => ReceiveNew());
            //receiverWindow.Start();
        }

        /// <summary>
        /// allowing to type in message for sending to server
        /// </summary>
        public void StartInput()
        {
            Thread writerWindow = new Thread(() => Message());
            writerWindow.Start();
        }

        /// <summary>
        /// Using SlidingWindow WindowSender method in a thread to send a message via Sliding Window Protocol 
        /// </summary>
        public void Message()
        {
            while (connected)
            {
                string input = userInput.GetInput();
                if (!String.IsNullOrWhiteSpace(input))
                {
                    try
                    {
                        SlidingWindow.Window window = new SlidingWindow.Window(senderClient, receiverClient);
                        window.WindowSender(input, endPoint);
                    }
                    catch(ArgumentNullException e)
                    {
                        RedundantMethods.UserExceptionError(e.Message, "Unvalid or null argument passed into a function");
                    }
                }
                else
                {
                    RedundantMethods.UserAlert("Input message cannot be blank or whitespace", "Please provide a valid input");
                }
            }
        }
    
    }
}

