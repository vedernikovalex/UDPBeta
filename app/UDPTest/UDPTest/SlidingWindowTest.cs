using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlidingWindow;
using System.Net.Sockets;

namespace UDPTest
{
    [TestClass]
    public class SlidingWindowTest
    {

        private UdpClient senderClient;
        private UdpClient receiverClient;
        private SlidingWindow.Window window;
        private string ip;
        private int port;

        [TestInitialize]
        public void InitWindowConfig()
        {
            ip = "127.0.0.1";
            port = 7777;

            senderClient = new UdpClient(port);
            receiverClient = new UdpClient(port+1);
            window = new SlidingWindow.Window(senderClient, receiverClient);
        }
    }
}