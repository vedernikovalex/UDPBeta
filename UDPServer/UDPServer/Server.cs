using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using SlidingWindow;

namespace UDPServer
{
	public class Server
	{
        private UdpClient udpServer;
        private IPEndPoint client;
        private bool connected = false;
        private UdpReceiveResult result;
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
            // Create a new UdpClient object on the local IP address and port number.

            while (true)
            {
                // Receive data from any client.
                dataReceived = udpServer.Receive(ref client);
                Console.WriteLine("Data received: " + Encoding.UTF8.GetString(dataReceived));
                // Start a new thread to handle the client's request.
                Thread threadDataHandle = new Thread(new ParameterizedThreadStart(HandleData));
                threadDataHandle.Start(client);
            }
        }

        private void HandleData(object client)
        {
            count += 1;
            // Convert the client object to an IPEndPoint.
            IPEndPoint clientEndPoint = (IPEndPoint)client;
            // Send a message back to the client.

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

        public async void WindowReceive()
        {
            while (true)
            {
                try
                {
                    int currentStep = 0;
                    Console.WriteLine("invoke methd");
                    UdpReceiveResult result = await udpServer.ReceiveAsync();
                    byte[] dataReceived = result.Buffer;

                    Console.WriteLine("2222222");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
    }
}

