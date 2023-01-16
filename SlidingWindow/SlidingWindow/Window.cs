﻿using System.Net.Sockets;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace SlidingWindow;
public static class Window
{
    static int windowSize = 4;
    static int currentPos = 0;
    static int packetLen = 0;

    static int packagePartLenght = 2;

    static async Task<string> WaitForInputAsync()
    {
        string input = await Task.Run(() => Console.ReadLine());
        return await Task.FromResult(input);
    }

    public static async void WindowSend(UdpClient client)
    {
        string input = await WaitForInputAsync().Wait();
            //await Task.Run(() => Console.ReadLine());
        byte[] windowSend = Encoding.UTF8.GetBytes(input);
        while (currentPos <= windowSend.Length)
        {
            int currentByteLen = 0;
            for (int i = currentPos; i < windowSize; i++)
            {
                int intervalLength = (currentByteLen + packagePartLenght) - (currentByteLen);
                byte[] interval = new byte[intervalLength];

                Array.Copy(windowSend, currentByteLen, interval, 0, intervalLength);
                Console.WriteLine(Encoding.UTF8.GetString(interval));

                try
                {
                    await client.SendAsync(interval, interval.Length);
                    currentByteLen += packagePartLenght;
                }
                catch (SocketException e)
                {
                    Console.WriteLine(e.ToString());
                    client.Close();
                    break;
                }
                catch (ObjectDisposedException e)
                {
                    Console.WriteLine(e.ToString());
                    client.Close();
                    break;
                }
            }
            Thread.Sleep(1000);
        }
    }

    public static async void WindowReceive(UdpClient client)
    {
        while (true)
        {
            try
            {
                int currentStep = 0;
                Console.WriteLine("invoke methd");
                UdpReceiveResult result = await client.ReceiveAsync();
                byte[] dataReceived = result.Buffer;

                Console.WriteLine("2222222");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }


    public static async void WindowMove()
    {
        
    }

}

public enum PackageType
{
    Config,
    Message,
    Login,
    Logout,

}

