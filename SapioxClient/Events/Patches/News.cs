using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SapioxClient.Events.Patches
{
    public class News
    {
        [HarmonyPatch(typeof(NewsLoader), nameof(NewsLoader.Start))]
        [HarmonyPrefix]
        public static bool OnRequest(NewsLoader __instance)
        {
            try
            {
                __instance._announcements = new Il2CppSystem.Collections.Generic.List<NewsLoader.Announcement>();
                //__instance._announcements.Add(new NewsLoader.Announcement($"Welcome to <color=#ff6f00>Sapiox Client!</color>", "<b><size=20>Join our discord server!: <color=blue><u><link='https://discord.gg/UsNfqvx2Mz'>https://discord.gg/UsNfqvx2Mz</link></u></color></size></b>", "27.11.2021", "https://discord.gg/UsNfqvx2Mz", null));
                __instance._announcements.Add(new NewsLoader.Announcement($"Amogus update!!!", "<b><color=red><u>sus</u></color></b>", "23.06.2022", "https://discord.gg/UsNfqvx2Mz", null));
                __instance.ShowAnnouncement(0);

                return false;
            }
            catch(Exception e)
            {
                SapioxManager.log.Error($"{typeof(News).FullName}.{nameof(OnRequest)}:\n{e}");
                return false;
            }
        }
    }
}
