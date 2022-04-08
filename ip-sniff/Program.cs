using System;
using System.Drawing;
using System.Net.Sockets;

namespace ip_sniff
{
    internal class Program
    {
        private static string IP = "";
        private static bool showOnlyRunningPorts = false;
        private static bool showRunningServices = false;
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
            // get IP to scan
            Console.WriteLine("Please enter the ip address or range of ip address to scan");
            IP = Console.ReadLine();
            
            // ask user if they want to know only running ports
            Console.WriteLine("Would you like to be notified only when ports are open? (y/n)");
            string answer = Console.ReadLine();
            showOnlyRunningPorts = answer.ToLower() == "y";

            // ask user if they want to know running services on open ports
            Console.WriteLine("Would you like to see the running services on the ports? (y/n)");
            answer = Console.ReadLine();
            showRunningServices =answer.ToLower() == "y";
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
                        if(showRunningServices==true)
                        {
                            ScanServicesInPort(s);
                        }
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
        private static void ScanServicesInPort(int port){
            Console.WriteLine($"Scanning services on port {port}");
        }
    }
}
