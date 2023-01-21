using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SlidingWindow;
using System.Configuration;

namespace UDPClient
{
    public class ClientClass
    {
        private string input, ip;
        private int port, portClient;

        private bool connected = false;

        private UdpClient senderClient, receiverClient;
        private IPEndPoint endPoint;



        public ClientClass()
        {
            ConfigureConnection();
            Connect();
        }

        public bool Connected
        {
            get { return connected; }
        }

        public void ConfigureConnection()
        {
            Console.WriteLine("!! Configurate your connection !!");
            Console.WriteLine("!! Get IP Address/PORT of server from configuration file or assign it manualy? !!");
            Console.WriteLine("!! Write \"config/manual\" !!");
            input = String.Empty;
            while (true)
            {
                Console.WriteLine(" ");
                Console.Write(">> ");
                input = Console.ReadLine().ToLower();
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
                        Console.WriteLine("!! Configuration file was not found or incorrectly formated !!");
                    }
                    break;
                }
                else if (input == "manual")
                {
                    string inputIp = String.Empty;
                    Console.WriteLine("!! IP Address !!");
                    while (true)
                    {
                        Console.WriteLine(" ");
                        Console.Write(">> ");
                        inputIp = Console.ReadLine();
                        if (IPAddress.TryParse(inputIp, out IPAddress valid) && !String.IsNullOrEmpty(inputIp))
                        {
                            ip = inputIp;
                            break;
                        }
                        Console.WriteLine("!! Please provide a valid IP Address !!");
                    }
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
                    int portInput = 0;
                    Console.WriteLine("!! Port of CLIENT !!");
                    while (true)
                    {
                        Console.WriteLine(" ");
                        Console.Write(">> ");
                        portInput = Int32.Parse(Console.ReadLine());
                        if (portInput >= 1 && portInput <= 65535)
                        {
                            portClient = portInput;
                            break;
                        }
                        Console.WriteLine("!! Please provide a valid PORT !!");
                    }
                    break;
                }
                Console.WriteLine("!! Please write \"config\" OR \"manual\" !!");
            }
            if (port != 0 && !String.IsNullOrEmpty(ip))
            {
                endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                senderClient = new UdpClient(portClient);
                receiverClient = new UdpClient(portClient + 1);
                Console.WriteLine("!! Configuration was successful !!");
                Console.WriteLine("!! Server config: " + endPoint);
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("!! Configuration wasn't successful !!");
                ConfigureConnection();
            }
        }

        public void Connect()
        {
            try
            {
                senderClient.Connect(endPoint);
                connected = true;
                Console.WriteLine("!! Successfully connected !!");
                StartInput();
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ToString());
                Disconnet();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.ToString());
                Disconnet();
            }
        }

        public void Disconnet()
        {
            senderClient.Close();
            receiverClient.Close();
            connected=false;
        }

        public void StartReceive()
        {
            Thread receiverWindow = new Thread(() => ReceiveNew());
            receiverWindow.Start();
        }

        public void StartInput()
        {
            Thread writerWindow = new Thread(() => Message());
            writerWindow.Start();
        }

        /*
        public void Send()
        {
            while (connected)
            {
                Console.WriteLine(" ");
                Console.Write(">> ");
                input = Console.ReadLine();
                data = Encoding.UTF8.GetBytes(input);
                senderClient.SendAsync(data, data.Length);
            }
        }
        */

        /*
        public async void Receive()
        {
            while (connected)
            {
                try
                {
                    result = await client.ReceiveAsync();
                    data = result.Buffer;
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.ToString());
                    Close();
                    connected = false;
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e.ToString());
                    Close();
                    connected = false;
                }

                receivedData = Encoding.UTF8.GetString(data, 0, data.Length);
                Console.WriteLine(receivedData);
                Console.WriteLine("");
            }
        }
        */

        /*
        private void Close()
        {
            client.Close();
        }
        */


        /// <summary>
        /// ////////////////
        /// </summary>
        ///

        /*
        int windowSize = 4;
        int currentPos = 0;
        int packetSize = 2;
        byte[] windowReceived;
        int packagePartLenght = 2;
        byte[] windowSend;

        */

        /*
        public void WindowSend(string message, IPEndPoint server)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            bool ackAllData = false;

            while (!ackAllData)
            {
                int currentByteLen = 0;
                for (int i = currentPos; i < windowSize; i++)
                {
                    int intervalLength = (currentByteLen + packagePartLenght) - (currentByteLen);
                    byte[] interval = new byte[intervalLength];

                    Array.Copy(windowSend, currentByteLen, interval, 0, intervalLength);
                    Console.WriteLine(Encoding.UTF8.GetString(interval));

                    try
                    {
                        client.Send(interval, interval.Length);
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
        */

        public void ReceiveNew()
        {
            while (connected)
            {
                try
                {

                }
                catch (Exception e)
                {

                }
            }
        }

        public void Message()
        {
            string input;
            while (connected)
            {
                input = Console.ReadLine();
                if (!String.IsNullOrWhiteSpace(input))
                {
                    try
                    {
                        SlidingWindow.Window window = new SlidingWindow.Window(senderClient, receiverClient);
                        window.WindowSender(input, endPoint);
                    }
                    catch(ArgumentNullException e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Cannot be null or blank");
                }
            }

        }

        //make a receiver to accept byte[] of size 2 as [number of byte, byte intself]

    }
}

