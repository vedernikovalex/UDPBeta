using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using SlidingWindow;

namespace UDPTests
{
    [TestClass]
    public class SlidingWindowTest
    {
        private UdpClient sender;
        private UdpClient receiver;

        private Slidin window;
        private SlidingWindow window2;

        private UdpClient sender2;
        private UdpClient receiver2;

        [TestMethod]
        public void TestMethod1()
        {

        }

    }

}