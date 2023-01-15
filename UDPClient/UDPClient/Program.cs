using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UDPClient;
using SlidingWindow;

class Program
{
    public static void Main()
    {
        ClientClass clientCurrent = new ClientClass();
        Thread receiveThread = new Thread(() => clientCurrent.Receive());
        Thread sendThread = new Thread(() => clientCurrent.Send());

        Thread sendWindowThread = new Thread(() => SlidingWindow.Window.WindowSend(clientCurrent.Client));
        //Thread receiveWindowThread = new Thread(clientCurrent.WindowReceive);
        //Thread windowMoveThread = new Thread(clientCurrent.WindowMove);

        clientCurrent.Connect();

        if (clientCurrent.Connected)
        {
            sendWindowThread.Start();
            //receiveWindowThread.Start();
            //windowMoveThread.Start();

            //sendThread.Start();
            //receiveThread.Start();
        }

    }
}
