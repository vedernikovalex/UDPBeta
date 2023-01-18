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
        private UdpClient udpServer;
        private IPEndPoint client;
        private bool connected = false;
        private UdpReceiveResult result;
        //private int port = Int32.Parse(ConfigurationManager.AppSettings.Get("port"));
        private int port = 7777;
        private byte[] dataReceived;

        private List<User> users = new List<User>();

        public Server()
        {
            udpServer = new UdpClient(new IPEndPoint(IPAddress.Any, port));
            client = new IPEndPoint(IPAddress.Any, 0);
        }

        public UdpClient UdpServer
        {
            get { return udpServer; }
            set { udpServer = value; }
        }

        public IPEndPoint Client
        {
            get { return client; }
            set { client = value; }
        }

        //not async, any client
        public void Receive()
        {
            while (true)
            {
                try
                {
                    dataReceived = udpServer.Receive(ref client);
                    string received = Encoding.UTF8.GetString(dataReceived);
                    Console.WriteLine(received[0]);
                    switch ((int)received[0])
                    {
                        case (int)PackageType.Login:
                            User newUser = new User(client);
                            Console.WriteLine("Logged a user: " + client);
                            users.Add(newUser);
                            break;
                        case (int)PackageType.Message:
                            Thread threadDataHandle = new Thread(new ParameterizedThreadStart(HandleWindow));
                            threadDataHandle.Start(client);
                            break;
                        case (int)PackageType.Logout:
                            for (int i = 0; i < users.Count; i++)
                            {
                                users[i].GetUserByClient(client);
                            }
                            break;
                        default:
                            Console.WriteLine("Under construction!");
                            break;
                    }
                    /*
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
                    */
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }

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

        public void ClientStateSwitch()
        {
            connected = connected ? false : true;
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


        public static async void WindowMove()
        {

        }

    }

}


