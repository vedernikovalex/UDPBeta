using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UDPServer;

public class UdpServer
{
    public static void Main()
    {
        Server server = new Server();
        // Create a new thread for the server to listen for clients.
        Thread threadReceive = new Thread(() => server.Receive());
        threadReceive.Start();

        //Thread threadWindowReceive = new Thread(() => SlidingWindow.Window.WindowReceive(server.UdpServer));
        //Thread threadWindowSend = new Thread(() => SlidingWindow.Window.WindowSend(server.UdpServer));

        //Thread threadWindowReceive = new Thread(() => server.WindowReceive(server.UdpServer));
        //Thread threadWindowSend = new Thread(() => server.WindowSend(server.UdpServer));


        //threadWindowSend.Start();
        //threadWindowReceive.Start();
        //server.WindowReceive(server.UdpServer);
        //threadWindowReceive.Join();
    }
}
