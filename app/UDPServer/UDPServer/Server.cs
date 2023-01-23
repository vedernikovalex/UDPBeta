using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SlidingWindow;
using System.Configuration;

namespace UDPServer
{
    /// <summary>
    /// Server class which initialize a UDP server 
    /// Contains its UdpClient socket for sending data and UdpClient socket for receiving data
    /// </summary>
    public class Server
    {
        private UdpClient senderClient, receiverClient;

        /// <summary>
        /// Server state
        /// </summary>
        private bool running = false;

        /// <summary>
        /// Port of server
        /// </summary>
        private int port;

        /// <summary>
        /// List of connected users
        /// </summary>
        private List<User> users = new List<User>();

        /// <summary>
        /// Constructor which starts a configuration of a server with ConfigureConnection method
        /// Proceeds to listen for incoming data with Receiver method
        /// </summary>
        public Server()
        {
            ConfigureConnection();
            Receiver();
        }
        
        /// <summary>
        /// Function for configuring servers UdpClient sender and receiver
        /// Can be configured by config file or manualy by user input
        /// </summary>
        public void ConfigureConnection()
        {
            Console.WriteLine("!! Configurate your connection !!");
            Console.WriteLine("!! Get PORT of server from configuration file or assign it manualy? !!");
            Console.WriteLine("!! Write \"config/manual\" !!");
            string input = String.Empty;
            while (!running)
            {
                Console.WriteLine(" ");
                Console.Write(">> ");
                input = Console.ReadLine().ToLower();
                if (input == "config")
                {
                    try
                    {
                        port = Int32.Parse(ConfigurationManager.AppSettings.Get("port"));
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("!! Configuration file was not found or incorrectly formated !!");
                    }
                    break;
                }
                else if (input == "manual")
                {
                    int portInputServer = 0;
                    Console.WriteLine("!! Port of server !!");
                    while (true)
                    {
                        Console.WriteLine(" ");
                        Console.Write(">> ");
                        portInputServer = Int32.Parse(Console.ReadLine());
                        if (portInputServer >= 1 && portInputServer <= 65535)
                        {
                            port = portInputServer;
                            break;
                        }
                        Console.WriteLine("!! Please provide a valid PORT !!");
                    }
                    break;
                }
                Console.WriteLine("!! Please write \"config\" OR \"manual\" !!");
            }
            if (port != 0)
            {
                senderClient = new UdpClient(port);
                receiverClient = new UdpClient(port + 1);
                running = true;
                Console.WriteLine("");
                Console.WriteLine("!! Configuration was successful !!");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("!! Configuration wasn't successful !!");
                ConfigureConnection();
            }
        }

        /// <summary>
        /// Receiver method for listening for all incoming data using SlidingWindow class method 
        /// Creates user if not exists already and broadcasts received message to all users
        /// </summary>
        public void Receiver()
        {
            string message = String.Empty;
            IPEndPoint sender = null;

            try
            {
                SlidingWindow.Window window = new SlidingWindow.Window(senderClient, receiverClient);
                Thread receiveThread = new Thread(new ThreadStart(window.WindowReceiver));
                receiveThread.Start();
                receiveThread.Join();
                message = window.ReceivedMessage;
                sender = window.SenderDestination;
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ToString());
                return;
            }
            User newUser = new User(sender);
            if (users.Contains(newUser))
            {
                SendAll(newUser, message);
            }
            else
            {
                newUser = new User(sender);
                users.Add(newUser);
                SendAll(newUser, message);
            }
            

        }


        public (string, IPEndPoint) Receive()
        {
            string message = String.Empty;
            IPEndPoint sender = null;

            SlidingWindow.Window window = new SlidingWindow.Window(senderClient, receiverClient);
            Thread receiveThread = new Thread(() => window.WindowReceiver());
            receiveThread.Start();
            //window.WindowReceiver();
            receiveThread.Join();
            //return (message, sender);
            return (window.ReceivedMessage, window.SenderDestination);
        }

        /// <summary>
        /// Sends a message to all users except itself
        /// </summary>
        /// <param name="user"> Sender </param>
        /// <param name="message"> Message to send</param>
        public void SendAll(User user, string message)
        {
            Console.WriteLine(user.ClientDestination + " >> " + message);
        }


        /*
        public void Receive()
        {
            while (true)
            {
                try
                {
                    dataReceived = udpServer.Receive(ref client);
                    string received = Encoding.UTF8.GetString(dataReceived);
                    string position = received[0].ToString();
                    Console.WriteLine(received);
                    udpServer.Send(Encoding.UTF8.GetBytes(position), position.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        */



        //not async, any client
        private void Handle(object client)
        {
            IPEndPoint clientEndPoint = (IPEndPoint)client;

            //byte[] dataToSend = Encoding.UTF8.GetBytes(count.ToString());
            //udpServer.Send(dataToSend, dataToSend.Length, clientEndPoint);
        }

        private void HandleWindow(object client)
        {
            IPEndPoint clientEndPoint = (IPEndPoint)client;


        }

        public static async void WindowMove()
        {

        }

    }

}


