using HarmonyLib;
using SapioxClient.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SapioxClient.Events.Patches
{
    public class ServerList
    {
        public static List<ServerEntry> Servers { get; set; } = new List<ServerEntry>();

        [HarmonyPatch(typeof(NewServerBrowser), nameof(NewServerBrowser.DownloadList))]
        [HarmonyPrefix]
        public static bool OnServerListAwake()
        {

            return true;
        }

        [HarmonyPatch(typeof(NewServerBrowser), nameof(NewServerBrowser.OnEnable))]
        [HarmonyPrefix]
        public static bool OnServerListEnable(NewServerBrowser __instance)
        {
            var filter = __instance.GetComponent<ServerFilter>();
            Composite(filter);
            return true;
        }

        private static void Composite(ServerFilter filter)
        {
            filter.FilteredListItems = new Il2CppSystem.Collections.Generic.List<ServerListItem>();
            var servers = new List<ServerEntry>();
            servers.Add(new ServerEntry
            {
                ip = "localhost",
                port = 7777,
                players = "2/25",
                info = "Tm9yZGhvbHouZGU=",
                pastebin = "bcGKt77D",
                version = "1.0",
                whitelist = false,
                modded = true,
                friendlyFire = true,
                officialCode = byte.MinValue
            });
            foreach (var serverEntry in servers)
            {
                AddServer(filter, serverEntry);
            }
            filter.DisplayServers();
            try
            {
                GameObject.Find("New Main Menu/Servers/ServerBrowser/Loading").active = false;
            }
            catch { }
            Log.Info("Servers displayed");
        }

        public static void AddServer(ServerFilter filter, ServerEntry entry)
        {
            var leakingObject = new LeakingObject<ServerListItem>();
            leakingObject.decorated = new ServerListItem
            {
                ip = (String)entry.ip,
                port = entry.port,
                players = (String)entry.players,
                info = (String)entry.info,
                pastebin = (String)entry.pastebin,
                version = (String)entry.version,
                whitelist = entry.whitelist,
                modded = entry.modded,
                friendlyFire = entry.friendlyFire,
                officialCode = entry.officialCode
            };
            filter.FilteredListItems.Add(leakingObject.decorated);
            leakingObject.Dispose();
        }

        public class ServerEntry
        {
            public string ip { get; set; }
            public ushort port { get; set; } = 7777;
            public string players { get; set; }
            public string info { get; set; }
            public string pastebin { get; set; }
            public string version { get; set; }
            public bool whitelist { get; set; } = false;
            public bool modded { get; set; } = true;
            public bool friendlyFire { get; set; } = true;
            public byte officialCode { get; set; } = Byte.MinValue;

        }
    }
}
