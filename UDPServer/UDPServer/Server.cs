using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SlidingWindow;
using System.Configuration;

namespace UDPServer
{
    public class Server
    {
        private UdpClient senderClient, receiverClient;

        private bool running = false;
        private UdpReceiveResult result;
        private int port = 7777;
        private byte[] dataReceived;

        private List<User> users = new List<User>();

        public Server()
        {
            ConfigureConnection();
            Receive();
        }

        public void ConfigureConnection()
        {
            Console.WriteLine("!! Configurate your connection !!");
            Console.WriteLine("!! Get PORT of server from configuration file or assign it manualy? !!");
            Console.WriteLine("!! Write \"config/manual\" !!");
            string input = String.Empty;
            while (true)
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

        public void Receive()
        {
            while (running)
            {
                SlidingWindow.Window window = new SlidingWindow.Window(senderClient, receiverClient);
                window.WindowReceiver();
            }

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

        public void StartReceive()
        {

        }

        //not async, any client

        /*
        public void Receive()
        {
            while (true)
            {
                try
                {
                    dataReceived = udpServer.Receive(ref client);
                    string received = Encoding.UTF8.GetString(dataReceived);
                    Console.WriteLine((char)(int)PackageType.Login);
                    switch (received[0])
                    {
                        case (char)(int)PackageType.Login:
                            User newUser = new User(client);
                            Console.WriteLine("Logged a user: " + client);
                            users.Add(newUser);
                            break;
                        case (char)(int)PackageType.Message:
                            Thread threadDataHandle = new Thread(new ParameterizedThreadStart(HandleWindow));
                            threadDataHandle.Start(client);
                            break;
                        case (char)(int)PackageType.Logout:
                            for (int i = 0; i < users.Count; i++)
                            {
                                users[i].GetUserByClient(client);
                            }
                            break;
                        default:
                            Console.WriteLine("Under construction!");
                            break;
                    }

                    if (received[0] == ((char)PackageType.Login))
                    {
                        User newUser = new User(client);
                        Console.WriteLine("Logged a user: " + client);
                        users.Add(newUser);
                    }
                    if (received[0] == ((char)PackageType.Message))
                    {
                        Thread threadDataHandle = new Thread(new ParameterizedThreadStart(HandleWindow));
                        threadDataHandle.Start(client);

                    }
                    Console.WriteLine("Under construction!");

                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
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


        /// <summary>
        /// ///////////
        /// </summary>
        ///

        int windowSize = 4;
        int currentPos = 0;
        int packetSize = 2;
        byte[] windowReceived;
        int packagePartLenght = 2;

        public async void WindowSend(UdpClient client)
        {
            string input = "string to send string string";
            //await Task.Run(() => Console.ReadLine());
            byte[] windowSend = Encoding.UTF8.GetBytes(input);
            Thread.Sleep(3000);
            while (currentPos <= windowSend.Length)
            {
                Thread.Sleep(1000);
                int currentByteLen = 0;
                for (int i = currentPos; i < windowSize; i++)
                {
                    int intervalLength = (currentByteLen + packagePartLenght) - (currentByteLen);
                    byte[] interval = new byte[intervalLength];

                    Array.Copy(windowSend, currentByteLen, interval, 0, intervalLength);
                    Console.WriteLine(Encoding.UTF8.GetString(interval));

                    try
                    {
                        await client.SendAsync(interval, interval.Length);
                        currentByteLen += packagePartLenght;
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.ToString());
                        client.Close();
                        break;
                    }
                    catch (ObjectDisposedException e)
                    {
                        Console.WriteLine(e.ToString());
                        client.Close();
                        break;
                    }
                }
                Thread.Sleep(1000);
            }
        }

        /*
        bool notDone = true;
        public void WindowReceive()
        {
            while (notDone)
            {
                try
                {
                    int currentStep = 0;
                    Console.WriteLine("invoke methd");
                    var result = udpServer.Receive(ref client);
                    Console.WriteLine("2222222");
                    Console.WriteLine(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        */


        public static async void WindowMove()
        {

        }

    }

}


