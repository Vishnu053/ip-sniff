using System;
using System.Drawing;
using System.Net.Sockets;

namespace ip_sniff
{
    internal class Program
    {
        private static string IP = "";
        private static bool showOnlyRunningPorts = false;
        private static int[] PortsToScan = new int[]
       {
            22,
            80,
            8080
       };
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to IP Sniff!");
            AcceptIpFromUser();
            ScanIp();
        }
        private static void AcceptIpFromUser()
        {
            Console.WriteLine("Please enter the ip address or range of ip address to scan");
            IP = Console.ReadLine();
            Console.WriteLine("Would you like to be notified only when ports are open? (y/n)");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                showOnlyRunningPorts = true;
            }
        }
        private static void ScanIp()
        {
            Console.Clear();
            foreach (int s in PortsToScan)
            // for(int s =0; s<10000;s++)
            {
                using (TcpClient Scan = new TcpClient())
                {
                    try
                    {
                        Scan.Connect(IP, s);
                        Console.WriteLine($"[{s}] is open", Color.Green);
                    }
                    catch
                    {
                        if (showOnlyRunningPorts == false)
                        {
                            Console.WriteLine($"[{s}] is closed", Color.Red);
                        }

                    }
                }
            }
        }
    }
}
