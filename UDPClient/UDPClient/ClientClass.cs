using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SlidingWindow;
using System.Configuration;

namespace UDPClient
{
    class ClientClass
    {
        //private string ip = ConfigurationManager.AppSettings.Get("ip");
        private string ip = "127.0.0.1";
        //private int port = Int32.Parse(ConfigurationManager.AppSettings.Get("port"));
        private int port = 7777;
        private byte[] dataReceived;
        private bool connected = false;
        private UdpClient client;
        private IPEndPoint endPoint;
        private UdpReceiveResult result;
        private byte[] data = new byte[1024];
        private string input, receivedData;

        public ClientClass()
        {
            endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            client = new UdpClient();
        }

        public bool Connected
        {
            get { return connected; }
        }

        public UdpClient Client
        {
            get { return client; }
        }

        public void Connect()
        {
            try
            {
                client.Connect(endPoint);
                ClientStateSwitch();
                Console.WriteLine("Successfully connected");
                Console.WriteLine("Server IP address: " + ip + " with port: " + port);
            }
            catch (SocketException e)
            {
                Console.WriteLine(e.ToString());
                Close();
                ClientStateSwitch();
            }
        }

        public void Send()
        {
            while (connected)
            {
                Console.WriteLine(" ");
                Console.Write(">> ");
                input = Console.ReadLine();
                data = Encoding.UTF8.GetBytes(input);
                client.SendAsync(data, data.Length);
            }
        }

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
                    ClientStateSwitch();
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e.ToString());
                    Close();
                    ClientStateSwitch();
                }

                receivedData = Encoding.UTF8.GetString(data, 0, data.Length);
                Console.WriteLine(receivedData);
                Console.WriteLine("");
            }
        }

        private void Close()
        {
            client.Close();
        }

        private void ClientStateSwitch()
        {
            connected = connected ? false : true;
        }


        /// <summary>
        /// ////////////////
        /// </summary>
        ///

        int windowSize = 4;
        int currentPos = 0;
        int packetSize = 2;
        byte[] windowReceived;
        int packagePartLenght = 2;

        public void WindowSend(UdpClient client)
        {
            string input;
            while (true)
            {
                input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    break;
                }
                Console.WriteLine("Cannot be null or blank");
            }
            byte[] windowSend = Encoding.UTF8.GetBytes("2"+input);
            Thread.Sleep(1000);
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


        //make a receiver to accept byte[] of size 2 as [number of byte, byte intself]s

    }
}

