using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Configuration;


namespace SlidingWindow
{
    /// <summary>
    /// Window class containing all methods of Sliding Window Protocol 
    /// </summary>
    public class Window
    {
        /// <summary>
        /// configuration of a window
        /// </summary>
        private int windowSize, packetSize, windowStart, windowEnd;

        /// <summary>
        /// Senders and receiver UdpClient and IPEndPoints
        /// </summary>
        private UdpClient senderClient, receiverClient;
        private IPEndPoint senderDestination, receiverDestination;

        /// <summary>
        /// byte arrays for transfering data
        /// </summary>
        private byte[] buffer, config;

        /// <summary>
        /// Received message storing
        /// </summary>
        private string receivedMessage;

        //public int currentPosition;

        /// <summary>
        /// Constructor that takes a sender and receiver ports
        /// Configures a window by config file
        /// </summary>
        /// <param name="sender"> UdpClient sender port </param>
        /// <param name="receiver"> UdpClient receiver port </param>
        public Window(UdpClient sender, UdpClient receiver)
        {
            senderClient = sender;
            receiverClient = receiver;
            try
            {
                windowSize = 4;
                //Int32.Parse(ConfigurationManager.AppSettings.Get("windowSize"));
                packetSize = 4;
                             //Int32.Parse(ConfigurationManager.AppSettings.Get("packetSize"));
            }
            catch (FormatException e)
            {
                Console.WriteLine("!! Configuration file was not found or incorrectly formated !!");
            }
        }

        public string ReceivedMessage
        {
            get { return receivedMessage; }
        }

        public UdpClient SenderClient
        {
            get { return senderClient; }
        }

        public UdpClient ReceiverClient
        {
            get { return receiverClient; }
        }

        public IPEndPoint SenderDestination
        {
            get { return senderDestination; }
        }

        public IPEndPoint ReceiverDestination
        {
            get { return receiverDestination; }
        }

        /// <summary>
        /// Window sender method which is constantly sending a part of a divided data packets until its moved further
        /// Starts and ends transmission
        /// Contains windowMover thread that accepts acknowledged packets and moves the window
        /// </summary>
        /// <param name="message"> Message to send </param>
        /// <param name="receiver"> To whom is being send </param>
        public void WindowSender(string message, IPEndPoint receiver)
        {
            windowStart = 0;
            windowEnd = windowSize - 1;
            bool ackAllData = false;

            byte[] byteMessage = Encoding.UTF8.GetBytes(message);

            //legth of message to be exact as as packet size 
            //if packets cannot be divided to needed packet size -> smaller
            buffer = new byte[byteMessage.Length + packetSize - (byteMessage.Length % packetSize)];
            Array.Copy(byteMessage, buffer, byteMessage.Length);
            int packetCount = (int)Math.Ceiling((double)buffer.Length / packetSize);

            Thread senderThread = new Thread(() =>
            {
                try
                {
                    int configState = 0;
                    config = BitConverter.GetBytes(configState);
                    senderClient.Send(config, config.Length);
                    Console.WriteLine(BitConverter.ToInt32(config));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e.ToString());
                }
                catch(SocketException e)
                {
                    Console.WriteLine("!! Endpoint connection seems to be closed !!");
                    
                }
                catch (ObjectDisposedException e)
                {

                }
                while (!ackAllData)
                {
                    for (int i = windowStart; i <= windowEnd; i++)
                    {
                        if (i < packetCount)
                        {
                            byte[] currentPacket = PacketDivider(packetSize, i, buffer);
                            try
                            {
                                senderClient.Send(currentPacket, currentPacket.Length);
                                Console.WriteLine("Packet was sent: " + Encoding.UTF8.GetString(currentPacket) + " | " + i);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                }
                try
                {
                    int configState = 1;
                    config = BitConverter.GetBytes(configState);
                    senderClient.Send(config, config.Length);
                    Console.WriteLine(BitConverter.ToInt32(config));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            });

            Thread moverThread = new Thread(() =>
            {
                while (!ackAllData)
                {
                    try
                    {
                        byte[] moverBuffer = senderClient.Receive(ref receiver);
                        int positionAck = BitConverter.ToInt32(moverBuffer, 0);
                        Console.WriteLine("received position of: " + positionAck);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            });

            senderThread.Start();
        }

        /// <summary>
        /// Window receiver method which listens for incomming data
        /// Detects start and end of transmission
        /// Proceeds to 
        /// </summary>
        public void WindowReceiver()
        {
            Dictionary<int, byte[]> packets = new Dictionary<int, byte[]>();
            bool allReceived = false, packetStartReceived = false;

            Thread listenerThread = new Thread(() =>
            {
                Console.WriteLine("thread started");
                while (!allReceived)
                {
                    byte[] data;
                    Console.WriteLine("invoke 1");
                    try
                    {
                        Console.WriteLine("invoke 2");
                        data = receiverClient.Receive(ref senderDestination);
                        Console.WriteLine("invoke 3");
                        Console.WriteLine(data);
                    }
                    catch (SocketException e)
                    {
                        Console.WriteLine(e.ToString());
                        break;
                    }
                    Console.WriteLine("invoke 4");
                    if (data.Length == 1)
                    {
                        if (BitConverter.ToInt32(data) == 0)
                        {
                            packetStartReceived = true;
                        }
                        else if(BitConverter.ToInt32(data) == 1)
                        {
                            Console.WriteLine("all received");
                            packetStartReceived = false;
                            allReceived = true;
                            break;
                        }
                    }
                    if(data.Length == windowSize + 4 && packetStartReceived)
                    {
                        string packetPart = Encoding.UTF8.GetString(data);
                        int packetNumber = BitConverter.ToInt32(data, 0);
                        Console.WriteLine(packetNumber);
                        Console.WriteLine(packetPart);

                        packets.Add(packetNumber, data);
                    }
                }
            });
            Console.WriteLine("invoke 5");

            listenerThread.Start();


            byte[] completedPackets = new byte[packets.Count * packetSize];
            for (int i = 0; i < packets.Count; i++)
            {
                //add up all packets
            }
            string message = Encoding.UTF8.GetString(completedPackets);
            Console.WriteLine(message);
            receivedMessage = message;
        }


        /// <summary>
        /// Dividing given packets to predetermined sized parts including 4 byte int which is packet part number
        /// </summary>
        /// <param name="packetSize"> Size of packet part </param>
        /// <param name="currentPosition"> Current position of packet part </param>
        /// <param name="message"> Message to divide </param>
        /// <returns></returns>
        public static byte[] PacketDivider(int packetSize, int currentPosition, byte[] message)
        {
            byte[] currentPositionBytes = BitConverter.GetBytes(currentPosition);
            byte[] packet = new byte[currentPositionBytes.Length + packetSize];

            //position in bytes
            int curerntByteInterval = packetSize * currentPosition;
            Array.Copy(BitConverter.GetBytes(currentPosition), packet, 1);
            Array.Copy(message, curerntByteInterval, packet, currentPositionBytes.Length, packetSize);
            return packet;
        }

    }
}
