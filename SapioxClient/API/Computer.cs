using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SapioxClient.API
{
    public static class Computer
    {
        public static string Mac => NetworkInterface
                .GetAllNetworkInterfaces()
                .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                .Select(nic => nic.GetPhysicalAddress().ToString())
                .FirstOrDefault() ?? "Unknown";

        public static string PcName => Environment.MachineName ?? "Unknown";

        public static string UserName => Environment.UserName ?? "Unknown";

        public static string ApplicationDataDir
        {
            get
            {
                var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Synapse Client");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }

        public static void OpenUrl(string url) => Application.OpenURL(url);
    }
}
