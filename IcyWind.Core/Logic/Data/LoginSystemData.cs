using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace IcyWind.Core.Logic.Data
{
    public static class LoginSystemData
    {
        /// <summary>
        /// Used to get the MacAddress of all Ethernet Interfaces
        /// </summary>
        public static string MacAddress
        {
            get
            {
                var result = string.Empty;
                var runningNetworkInterfaces = from allInterfaces in NetworkInterface.GetAllNetworkInterfaces() where allInterfaces.NetworkInterfaceType == NetworkInterfaceType.Ethernet select allInterfaces;
                foreach (var netInterface in runningNetworkInterfaces)
                {

                    result += netInterface.GetPhysicalAddress();
                    result += ",";
                }
                return result.TrimEnd(',');
            }
        }

        /// <summary>
        /// Just return Windows 7 for no reason
        /// </summary>
        public static string OperatingSystem => "Windows 7";
    }
}
