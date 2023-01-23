using Microsoft.VisualStudio.TestTools.UnitTesting;
using UDPClient;
using System.Net.Sockets;
using Moq;

namespace UDPTest
{
    [TestClass]
    public class ClientClassTest
    {

        private UdpClient senderClient;
        private UdpClient receiverClient;
        private SlidingWindow.Window window;
        private string ip;
        private int port;
        private int portClient;
        private UDPClient.ClientClass client;

        [TestInitialize]
        public void InitClientConfig()
        {
            UDPClient.ClientClass client = new UDPClient.ClientClass();

        }

        [TestInitialize]
        public void InitClientManual()
        {
            ip = "127.0.0.1";
            port = 7777;
            portClient = 8888;




        }
    }
}