using HarmonyLib;
using SapioxClient.API;
using SapioxClient.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerRuntimeLib;

namespace SapioxClient.Events.Patches
{
    public class SmallPatches
    {
        [HarmonyPatch(typeof(ServerRoles), nameof(ServerRoles.Start))]
        [HarmonyPrefix]
        public static bool StartNicknameSync(ServerRoles __instance)
        {
            var ns = __instance.GetComponent<NicknameSync>();
            if (ns.isLocalPlayer)
            {
                Log.Info("Loaded Player!!");
                /*__instance.RemoteAdmin = true;
                __instance.RaEverywhere = true;
                __instance.SetOverwatchStatus(true);
                __instance.SetGroup(new UserGroup() { BadgeColor = "red", BadgeText = "ok", HiddenByDefault = true, KickPower = 222 }, true, true);
                __instance.GlobalBadgeType = 1;
                __instance.NetworkGlobalBadge = "SCPSL TESTER";*/

                __instance.gameObject.AddComponent<LocalPlayer>();
            }
            return true;
        }
    }
}
