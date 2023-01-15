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
        Thread thread = new Thread(() => server.ReceiveFromClients());
        //thread.Start();

        Thread threadWindowReceive = new Thread(() => SlidingWindow.Window.WindowReceive(server.UdpServer));
        threadWindowReceive.Start();
        //threadWindowReceive.Join();
    }
}
