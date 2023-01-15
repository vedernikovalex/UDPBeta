using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SlidingWindow;

namespace UDPClient
{
    class ClientClass
    {
        private string ip = "127.0.0.1";
        private int port = 7777;
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
    }
}

