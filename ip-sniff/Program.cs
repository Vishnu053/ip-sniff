using System;
using System.Drawing;
using System.Net.Sockets;

namespace ip_sniff
{
    internal class Program
    {
        private static string IP = "";
        private static int[] PortsToScan = new int[]
       {
            22,
            80,
            443,
            3389,
            8080
       };
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to IP Sniff!");
            AcceptIpFromUser();
            ScanIp();
        }
        //function to accept a single or a range of ip address and scan each ip address for open ports
        private static void AcceptIpFromUser()
        {
            Console.WriteLine("Please enter the ip address or range of ip address to scan");
            IP=Console.ReadLine();
        }
        private static void ScanIp()
        {
            //scan the ip address
            Console.Clear();
            foreach (int s in PortsToScan)
            {
                using (TcpClient Scan = new TcpClient())
                {
                    try
                    {
                        Scan.Connect(IP, s);
                        Console.WriteLine($"[{s}] | OPEN", Color.Green);
                    }
                    catch
                    {
                        Console.WriteLine($"[{s}] | CLOSED", Color.Red);
                    }
                }
            }
        }
    }
}
