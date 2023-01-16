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
        private int count;

        public Server()
        {
            udpServer = new UdpClient(new IPEndPoint(IPAddress.Any, port)); ;
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


        public void ReceiveFromClients()
        {
            while (true)
            {
                //not async, any client
                dataReceived = udpServer.Receive(ref client);
                Console.WriteLine("Data received: " + Encoding.UTF8.GetString(dataReceived));

                Thread threadDataHandle = new Thread(new ParameterizedThreadStart(HandleData));
                threadDataHandle.Start(client);
            }
        }

        private void HandleData(object client)
        {
            count += 1;
            IPEndPoint clientEndPoint = (IPEndPoint)client;

            byte[] dataToSend = Encoding.UTF8.GetBytes(count.ToString());
            udpServer.Send(dataToSend, dataToSend.Length, clientEndPoint);
        }

        public void ClientStateSwitch()
        {
            connected = connected ? false : true;
        }

        int windowSize = 4;
        int currentPos = 0;
        int packetSize = 2;
        byte[] windowReceived;

    }
}

