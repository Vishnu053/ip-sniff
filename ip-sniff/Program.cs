using System;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;

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
            Console.WriteLine($"Your IP address is {GetLocalIPAddress()}");
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
            //check if its localhost or ip address
            if (CheckIfValidIP(IP))
            {
                //checked if the ip is live
                if (PingAddress(IP))
                {
                    Console.WriteLine($"{IP} is up");
                    foreach (int s in PortsToScan)
                    // for(int s =0; s<10000;s++)
                    {
                        using (TcpClient Scan = new TcpClient())
                        {
                            try
                            {
                                Scan.Connect(IP, s);
                                Console.WriteLine($"[{s}] is open", Color.Green);
                                if (showRunningServices == true)
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
                else
                {
                    Console.WriteLine($"{IP} is down. Cannot perform further actions until the IP comes back up.");
                }
            }
            else
            {
                Console.WriteLine($"What you provided ({IP}) does not seem to be a valid IP address.");

            }
        }
        //function to get local IP address
        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        
        private static bool CheckIfValidIP(string ipAddr)
        {
            Match match = Regex.Match(ipAddr, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
            return match.Success;
        }
        private static void ScanServicesInPort(int port){
            Console.WriteLine($"Scanning services on port {port}");
        }
        private static bool PingAddress(string address)
        {
            bool pingable = false;
            using(var pinger= new Ping())
            {
                try
                {
                    PingReply reply = pinger.Send(address);
                    pingable = reply.Status == IPStatus.Success;

                }
                catch (PingException)
                {
                    pingable = false;
                    throw;
                }
                finally
                {
                    if(pinger != null)
                    {
                        pinger.Dispose();
                    }
                }

                return pingable;
            }
        }
    }
}
