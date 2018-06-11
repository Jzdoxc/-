using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace 配置网卡信息
{
    public class MyNetworkInformation
    {
        public string networkName { get; set; }
        public string ipaddress { get; set; }
        public string mask { get; set; }
        public string gateway { get; set; }
        public string dns1 { get; set; }
        public string dns2 { get; set; }
        public bool dhcp { get; set; }
        public static List<MyNetworkInformation> GetIPS()
        {
            List<MyNetworkInformation> list = new List<MyNetworkInformation>();
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface networkInterface in adapters)
            {
                MyNetworkInformation my = new MyNetworkInformation();
                try
                {
                    if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                        continue;
                    if (networkInterface.OperationalStatus == OperationalStatus.Up)
                    {
                        //获取网卡的IP属性
                        my.networkName = networkInterface.Name;
                        IPInterfaceProperties ip = networkInterface.GetIPProperties();
                        IPv4InterfaceProperties ipp = ip.GetIPv4Properties();
                        if (ipp.IsDhcpEnabled)
                        {
                           my.dhcp= true;
                        }
                        else
                        {
                            my.dhcp = false;
                        }
                        foreach (UnicastIPAddressInformation information in ip.UnicastAddresses)
                        {
                            if (information.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                my.ipaddress = information.Address.ToString();
                                my.mask = information.IPv4Mask.ToString();
                            }
                        }
                        my.gateway = ip.GatewayAddresses[0].Address.ToString();
                        my.dns1 = ip.DnsAddresses[0].ToString();
                        my.dns2 = ip.DnsAddresses[0].ToString();

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                list.Add(my);
            }
            return list;
        }
    }
}
