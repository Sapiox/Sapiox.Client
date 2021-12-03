using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapioxClient.Events.Patches
{
    internal static class ServerList
    {
        [HarmonyPatch(typeof(NewServerBrowser), nameof(NewServerBrowser.DownloadList))]
        [HarmonyPrefix]
        public static bool OnServerListAwake()
        {

            return false;
        }
    }
}
