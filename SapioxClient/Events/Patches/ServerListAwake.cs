using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapioxClient.Events.Patches
{
    [HarmonyPatch(typeof(NewServerBrowser), nameof(NewServerBrowser.DownloadList))]
    internal static class ServerListAwake
    {
        [HarmonyPrefix]
        public static bool OnServerListAwake()
        {

            return false;
        }
    }
}
